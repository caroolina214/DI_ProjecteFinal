using MilAventures.Model.Models;
using System.Collections.ObjectModel;

namespace MilAventures.ViewModel.Bookings
{
    /// <summary>
    /// ViewModel per a cada línia d'una reserva.
    /// Pot representar una activitat o un element d'equipament.
    /// </summary>
    public class BookingLineViewModel : BaseViewModel
    {
        /// <summary>Identificador de la línia (0 si és nova).</summary>
        public int Id { get; set; }

        private bool _priceManuallySet = false;

        private Activity _selectedActivity;

        /// <summary>Activitat seleccionada per a aquesta línia.</summary>
        public Activity SelectedActivity
        {
            get => _selectedActivity;
            set
            {
                _selectedActivity = value;
                OnPropertyChanged(nameof(SelectedActivity));
                OnPropertyChanged(nameof(IsActivity));
                OnPropertyChanged(nameof(IsEquipment));
                if (value != null)
                {
                    _selectedEquipment = null;
                    OnPropertyChanged(nameof(SelectedEquipment));
                    if (!_priceManuallySet)
                        PriceAtMoment = value.price_per_person;
                }
            }
        }

        private Equipment _selectedEquipment;

        /// <summary>Equipament seleccionat per a aquesta línia.</summary>
        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged(nameof(SelectedEquipment));
                OnPropertyChanged(nameof(IsActivity));
                OnPropertyChanged(nameof(IsEquipment));
                if (value != null)
                {
                    _selectedActivity = null;
                    OnPropertyChanged(nameof(SelectedActivity));
                    if (!_priceManuallySet)
                        PriceAtMoment = value.price_per_day;
                }
            }
        }

        private int _quantity;

        /// <summary>Quantitat d'unitats per a aquesta línia.</summary>
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
        }

        private decimal _priceAtMoment;

        /// <summary>Preu en el moment de la reserva (congelat).</summary>
        public decimal PriceAtMoment
        {
            get => _priceAtMoment;
            set { _priceAtMoment = value; OnPropertyChanged(nameof(PriceAtMoment)); }
        }

        /// <summary>Retorna true si la línia és d'activitat.</summary>
        public bool IsActivity => _selectedEquipment == null;

        /// <summary>Retorna true si la línia és d'equipament.</summary>
        public bool IsEquipment => _selectedEquipment != null;

        /// <summary>Activitats disponibles per seleccionar.</summary>
        public ObservableCollection<Activity> AvailableActivities { get; set; }

        /// <summary>Equipament disponible per seleccionar.</summary>
        public ObservableCollection<Equipment> AvailableEquipments { get; set; }

        /// <summary>
        /// Estableix el preu original de la reserva sense sobreescriure'l
        /// quan es carrega una línia existent.
        /// </summary>
        public void SetOriginalPrice(decimal price)
        {
            _priceManuallySet = true;
            PriceAtMoment = price;
        }
    }
}