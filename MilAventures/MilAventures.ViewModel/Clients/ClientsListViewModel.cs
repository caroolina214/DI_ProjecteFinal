using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MilAventures.ViewModel.Clients
{
    public class ClientListViewModel : BaseViewModel
    {
        private readonly ClientRepository _repo;

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set { _clients = value; OnPropertyChanged(nameof(Clients)); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterClients(); }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; OnPropertyChanged(nameof(SelectedClient)); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public ClientListViewModel(ClientRepository repo)
        {
            _repo = repo;
            LoadClients();

            AddCommand = new RelayCommand(_ => OpenAddDialog());
            EditCommand = new RelayCommand(_ => OpenEditDialog(), _ => SelectedClient != null);
            DeleteCommand = new RelayCommand(_ => DeleteClient(), _ => SelectedClient != null);
        }

        private void LoadClients()
        {
            Clients = new ObservableCollection<Client>(_repo.GetAll());
        }

        private void FilterClients()
        {
            var all = _repo.GetAll();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Clients = new ObservableCollection<Client>(all);
            }
            else
            {
                var filtered = all.Where(c =>
                    c.name.ToLower().Contains(SearchText.ToLower()) ||
                    c.surname.ToLower().Contains(SearchText.ToLower()) ||
                    (c.email != null && c.email.ToLower().Contains(SearchText.ToLower())) ||
                    (c.phone != null && c.phone.ToLower().Contains(SearchText.ToLower())));
                Clients = new ObservableCollection<Client>(filtered);
            }
        }

        private void OpenAddDialog()
        {
            var dialog = new ClientDialogViewModel(_repo, null);
            dialog.OnSaved += LoadClients;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void OpenEditDialog()
        {
            var dialog = new ClientDialogViewModel(_repo, SelectedClient);
            dialog.OnSaved += LoadClients;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void DeleteClient()
        {
            var result = MessageBox.Show(
                $"Segur que vols eliminar el client '{SelectedClient.name} {SelectedClient.surname}'?",
                "Confirmar eliminació",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _repo.Delete(SelectedClient.id_client);
                LoadClients();
            }
        }
    }
}