using MilAventures.Model.Context;
using MilAventures.Model.Repositories;
using MilAventures.ViewModel.Activities;
using MilAventures.ViewModel.Bookings;
using MilAventures.ViewModel.Categories;
using MilAventures.ViewModel.Clients;
using MilAventures.ViewModel.Equipments;
using MilAventures.ViewModel.Guides;
using MilAventures.ViewModel.Reports;

namespace MilAventures.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentView;
        public BaseViewModel CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        private string _currentPage;
        public string CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }

        public RelayCommand NavInicCommand { get; }
        public RelayCommand NavCategoriesCommand { get; }
        public RelayCommand NavGuidesCommand { get; }
        public RelayCommand NavClientsCommand { get; }
        public RelayCommand NavActivitiesCommand { get; }
        public RelayCommand NavEquipmentCommand { get; }
        public RelayCommand NavBookingsCommand { get; }
        public RelayCommand NavInformesCommand { get; }

        private readonly MilAventuresContext _context;

        public MainViewModel()
        {
            _context = new MilAventuresContext();

            NavInicCommand = new RelayCommand(_ =>
            {
                CurrentPage = "Inici";
                NavigationService.Instance.NavigateTo(
                    new CategoryListViewModel(new CategoryRepository(_context)));
            });

            NavCategoriesCommand = new RelayCommand(_ =>
            {
                CurrentPage = "Categories";
                NavigationService.Instance.NavigateTo(
                    new CategoryListViewModel(new CategoryRepository(_context)));
            });

            NavGuidesCommand = new RelayCommand(_ =>
            {
                CurrentPage = "Guies";
                NavigationService.Instance.NavigateTo(
                    new GuideListViewModel(new GuideRepository(_context)));
            });

            NavClientsCommand = new RelayCommand(_ => {
                CurrentPage = "Clients";
                NavigationService.Instance.NavigateTo(
                    new ClientListViewModel(new ClientRepository(_context)));
            });

            NavActivitiesCommand = new RelayCommand(_ => { 
                CurrentPage = "Activitats";
                NavigationService.Instance.NavigateTo(
                    new ActivityListViewModel(new ActivityRepository(_context)));
            });

            NavEquipmentCommand = new RelayCommand(_ => { 
                CurrentPage = "Material";
                NavigationService.Instance.NavigateTo(
                    new EquipmentListViewModel(new EquipmentRepository(_context)));
            });

            NavBookingsCommand = new RelayCommand(_ => { 
                CurrentPage = "Reserves";
                NavigationService.Instance.NavigateTo(
                    new BookingListViewModel(new BookingRepository(_context)));
            });


            NavInformesCommand = new RelayCommand(_ =>
            {
                CurrentPage = "Informes";
                NavigationService.Instance.NavigateTo(new InformesViewModel());
            });

            NavigationService.Instance.OnNavigate += vm => CurrentView = vm;

            CurrentPage = "Inici";
            NavigationService.Instance.NavigateTo(
                new CategoryListViewModel(new CategoryRepository(_context)));
        }
    }
}