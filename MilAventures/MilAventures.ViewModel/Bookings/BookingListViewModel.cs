using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MilAventures.ViewModel.Bookings
{
    public class BookingListViewModel : BaseViewModel
    {
        private readonly BookingRepository _repo;

        private ObservableCollection<Booking> _bookings;
        public ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            set { _bookings = value; OnPropertyChanged(nameof(Bookings)); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterBookings(); }
        }

        private Booking _selectedBooking;
        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set { _selectedBooking = value; OnPropertyChanged(nameof(SelectedBooking)); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public BookingListViewModel(BookingRepository repo)
        {
            _repo = repo;
            LoadBookings();

            AddCommand = new RelayCommand(_ => OpenAddDialog());
            EditCommand = new RelayCommand(_ => OpenEditDialog(), _ => SelectedBooking != null);
            DeleteCommand = new RelayCommand(_ => DeleteBooking(), _ => SelectedBooking != null);
        }

        private void LoadBookings()
        {
            Bookings = new ObservableCollection<Booking>(_repo.GetAll());
        }

        private void FilterBookings()
        {
            var all = _repo.GetAll();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Bookings = new ObservableCollection<Booking>(all);
            }
            else
            {
                var filtered = all.Where(b =>
                    (b.Client != null && b.Client.name.ToLower().Contains(SearchText.ToLower())) ||
                    (b.Client != null && b.Client.surname.ToLower().Contains(SearchText.ToLower())) ||
                    (b.BookingStatus != null && b.BookingStatus.code.ToLower().Contains(SearchText.ToLower())));
                Bookings = new ObservableCollection<Booking>(filtered);
            }
        }

        private void OpenAddDialog()
        {
            var dialog = new BookingDialogViewModel(_repo, null);
            dialog.OnSaved += LoadBookings;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void OpenEditDialog()
        {
            var dialog = new BookingDialogViewModel(_repo, SelectedBooking);
            dialog.OnSaved += LoadBookings;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void DeleteBooking()
        {
            var result = MessageBox.Show(
                $"Segur que vols eliminar la reserva #{SelectedBooking.id_booking}?",
                "Confirmar eliminació",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _repo.Delete(SelectedBooking.id_booking);
                LoadBookings();
            }
        }
    }
}