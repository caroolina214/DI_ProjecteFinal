using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MilAventures.ViewModel.Bookings
{
    /// <summary>
    /// ViewModel per al diàleg de creació i edició de reserves.
    /// Gestiona la lògica de negoci, validacions i transicions d'estat.
    /// </summary>
    public class BookingDialogViewModel : BaseViewModel
    {
        private readonly BookingRepository _repo;
        private readonly bool _isEdit;
        private int _id;
        private string _originalStatusCode;

        /// <summary>Event que s'invoca quan la reserva es desa correctament.</summary>
        public event Action OnSaved;

        /// <summary>Títol del diàleg segons si és creació o edició.</summary>
        public string DialogTitle => _isEdit ? "Editar reserva" : "Nova reserva";

        /// <summary>Indica si la reserva és de només lectura (no pendent).</summary>
        public bool IsEditable => _originalStatusCode == "PENDING";
        public bool IsStatusOnly => _originalStatusCode == "ACCEPTED" || _originalStatusCode == "RUNNING";

        private Client _selectedClient;
        /// <summary>Client seleccionat per a la reserva.</summary>
        public Client SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; OnPropertyChanged(nameof(SelectedClient)); }
        }

        private BookingStatus _selectedStatus;
        /// <summary>Estat actual de la reserva.</summary>
        public BookingStatus SelectedStatus
        {
            get => _selectedStatus;
            set { _selectedStatus = value; OnPropertyChanged(nameof(SelectedStatus)); }
        }

        private int _participants;
        /// <summary>Nombre de participants de la reserva.</summary>
        public int Participants
        {
            get => _participants;
            set { _participants = value; OnPropertyChanged(nameof(Participants)); RecalculateTotal(); }
        }

        private string _notes;
        /// <summary>Notes o observacions de la reserva.</summary>
        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(nameof(Notes)); }
        }

        private decimal _totalPrice;
        /// <summary>Preu total calculat de la reserva.</summary>
        public decimal TotalPrice
        {
            get => _totalPrice;
            set { _totalPrice = value; OnPropertyChanged(nameof(TotalPrice)); }
        }

        private ObservableCollection<BookingStatus> _availableStatuses;
        /// <summary>Estats disponibles segons la transició permesa des de l'estat actual.</summary>
        public ObservableCollection<BookingStatus> AvailableStatuses
        {
            get => _availableStatuses;
            set { _availableStatuses = value; OnPropertyChanged(nameof(AvailableStatuses)); }
        }

        /// <summary>Línies de la reserva (activitats i equipament).</summary>
        public ObservableCollection<BookingLineViewModel> Lines { get; set; }

        /// <summary>Llista de clients actius disponibles.</summary>
        public ObservableCollection<Client> Clients { get; set; }

        /// <summary>Tots els estats de reserva disponibles.</summary>
        public ObservableCollection<BookingStatus> Statuses { get; set; }

        /// <summary>Activitats disponibles per afegir a les línies.</summary>
        public ObservableCollection<Activity> Activities { get; set; }

        /// <summary>Equipament disponible per afegir a les línies.</summary>
        public ObservableCollection<Equipment> Equipments { get; set; }

        /// <summary>Comanda per desar la reserva.</summary>
        public RelayCommand SaveCommand { get; }

        /// <summary>Comanda per cancel·lar i tancar el diàleg.</summary>
        public RelayCommand CancelCommand { get; }

        /// <summary>Comanda per afegir una línia d'activitat.</summary>
        public RelayCommand AddActivityLineCommand { get; }

        /// <summary>Comanda per afegir una línia d'equipament.</summary>
        public RelayCommand AddEquipmentLineCommand { get; }

        /// <summary>Comanda per eliminar una línia de la reserva.</summary>
        public RelayCommand RemoveLineCommand { get; }

        /// <summary>
        /// Constructor del BookingDialogViewModel.
        /// </summary>
        /// <param name="repo">Repositori de reserves.</param>
        /// <param name="booking">Reserva a editar, o null si és nova.</param>
        public BookingDialogViewModel(BookingRepository repo, Booking booking)
        {
            _repo = repo;
            _isEdit = booking != null;

            var context = repo._context;
            Clients = new ObservableCollection<Client>(
                new ClientRepository(context).GetAll().Where(c => c.status));
            Statuses = new ObservableCollection<BookingStatus>(
                new BookingStatusRepository(context).GetAll());
            Activities = new ObservableCollection<Activity>(
                new ActivityRepository(context).GetAll());
            Equipments = new ObservableCollection<Equipment>(
                new EquipmentRepository(context).GetAll());
            Lines = new ObservableCollection<BookingLineViewModel>();

            if (_isEdit)
            {
                _id = booking.id_booking;
                _originalStatusCode = booking.BookingStatus?.code ?? "PENDING";

                SelectedClient = Clients.FirstOrDefault(c => c.id_client == booking.id_client);
                SelectedStatus = Statuses.FirstOrDefault(s => s.id_book_status == booking.id_book_status);
                Participants = booking.participants;
                Notes = booking.notes;
                TotalPrice = booking.total_price;

                // Filtrar estats disponibles segons l'estat actual
                AvailableStatuses = new ObservableCollection<BookingStatus>(
                    GetAvailableStatuses(_originalStatusCode));

                foreach (var line in booking.BookingLines)
                {
                    var vm = new BookingLineViewModel
                    {
                        Id = line.id_line,
                        Quantity = line.quantity,
                        AvailableActivities = Activities,
                        AvailableEquipments = Equipments
                    };
                    vm.SetOriginalPrice(line.price_at_moment);
                    if (line.activityId != null)
                        vm.SelectedActivity = Activities.FirstOrDefault(a => a.id_activity == line.activityId);
                    else if (line.equipmentId != null)
                        vm.SelectedEquipment = Equipments.FirstOrDefault(e => e.id_equipment == line.equipmentId);
                    Lines.Add(vm);
                }
            }
            else
            {
                _originalStatusCode = "PENDING";
                Participants = 1;
                SelectedStatus = Statuses.FirstOrDefault(s => s.code == "PENDING");
                AvailableStatuses = new ObservableCollection<BookingStatus>(
                    Statuses.Where(s => s.code == "PENDING"));
            }

            SaveCommand = new RelayCommand(_ => Save(),
                _ => SelectedClient != null && SelectedStatus != null && Lines.Count > 0);
            CancelCommand = new RelayCommand(_ => NavigationService.Instance.CloseDialog());
            AddActivityLineCommand = new RelayCommand(_ => AddLine(true));
            AddEquipmentLineCommand = new RelayCommand(_ => AddLine(false));
            RemoveLineCommand = new RelayCommand(line => RemoveLine(line as BookingLineViewModel));
        }

        /// <summary>
        /// Retorna els estats als quals es pot transitar des de l'estat actual.
        /// </summary>
        /// <param name="currentCode">Codi de l'estat actual.</param>
        private IEnumerable<BookingStatus> GetAvailableStatuses(string currentCode)
        {
            switch (currentCode)
            {
                case "PENDING":
                    return Statuses;
                case "ACCEPTED":
                    return Statuses.Where(s =>
                        s.code == "ACCEPTED" ||
                        s.code == "RUNNING" ||
                        s.code == "CANCELED");
                case "RUNNING":
                    return Statuses.Where(s =>
                        s.code == "RUNNING" ||
                        s.code == "ENDED");
                case "ENDED":
                case "CANCELED":
                    return Statuses.Where(s => s.code == currentCode);
                default:
                    return Statuses;
            }
        }

        /// <summary>Afegeix una nova línia d'activitat o equipament a la reserva.</summary>
        private void AddLine(bool isActivity)
        {
            var vm = new BookingLineViewModel
            {
                Quantity = isActivity ? Participants : 1,
                AvailableActivities = Activities,
                AvailableEquipments = Equipments
            };

            if (isActivity)
                vm.SelectedActivity = Activities.FirstOrDefault();
            else
                vm.SelectedEquipment = Equipments.FirstOrDefault();

            Lines.Add(vm);
            RecalculateTotal();
        }

        /// <summary>Elimina una línia de la reserva.</summary>
        private void RemoveLine(BookingLineViewModel line)
        {
            if (line != null)
            {
                Lines.Remove(line);
                RecalculateTotal();
            }
        }

        /// <summary>Recalcula el preu total sumant totes les línies.</summary>
        private void RecalculateTotal()
        {
            TotalPrice = Lines.Sum(l => l.PriceAtMoment * l.Quantity);
        }

        /// <summary>
        /// Valida i desa la reserva a la base de dades.
        /// Comprova stock i transicions d'estat permeses.
        /// </summary>
        private void Save()
        {
            // Validació stock equipament
            foreach (var line in Lines)
            {
                if (line.SelectedEquipment != null && line.Quantity > line.SelectedEquipment.units)
                {
                    MessageBox.Show(
                        $"Stock insuficient de '{line.SelectedEquipment.title}'. Disponible: {line.SelectedEquipment.units}",
                        "Stock insuficient", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // Construir les línies noves
            var newLines = Lines.Select(l => new BookingLine
            {
                quantity = l.Quantity,
                price_at_moment = l.PriceAtMoment,
                activityId = l.SelectedActivity?.id_activity,
                equipmentId = l.SelectedEquipment?.id_equipment
            }).ToList();

            if (_isEdit)
            {
                var booking = new Booking
                {
                    id_booking = _id,
                    id_client = SelectedClient.id_client,
                    id_book_status = SelectedStatus.id_book_status,
                    participants = Participants,
                    notes = Notes,
                    total_price = TotalPrice
                };
                _repo.UpdateWithLines(booking, newLines);
            }
            else
            {
                var booking = new Booking
                {
                    id_client = SelectedClient.id_client,
                    id_book_status = SelectedStatus.id_book_status,
                    participants = Participants,
                    notes = Notes,
                    total_price = TotalPrice,
                    created_at = DateTime.Now,
                    BookingLines = new System.Collections.Generic.List<BookingLine>()
                };
                foreach (var line in newLines)
                    booking.BookingLines.Add(line);
                _repo.Add(booking);
            }

            OnSaved?.Invoke();
            NavigationService.Instance.CloseDialog();
        }
    }
}