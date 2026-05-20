using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MilAventures.ViewModel.Equipments
{
    public class EquipmentListViewModel : BaseViewModel
    {
        private readonly EquipmentRepository _repo;

        private ObservableCollection<Equipment> _equipments;
        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set { _equipments = value; OnPropertyChanged(nameof(Equipments)); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterEquipments(); }
        }

        private Equipment _selectedEquipment;
        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set { _selectedEquipment = value; OnPropertyChanged(nameof(SelectedEquipment)); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public EquipmentListViewModel(EquipmentRepository repo)
        {
            _repo = repo;
            LoadEquipments();

            AddCommand = new RelayCommand(_ => OpenAddDialog());
            EditCommand = new RelayCommand(_ => OpenEditDialog(), _ => SelectedEquipment != null);
            DeleteCommand = new RelayCommand(_ => DeleteEquipment(), _ => SelectedEquipment != null);
        }

        private void LoadEquipments()
        {
            Equipments = new ObservableCollection<Equipment>(_repo.GetAll());
        }

        private void FilterEquipments()
        {
            var all = _repo.GetAll();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Equipments = new ObservableCollection<Equipment>(all);
            }
            else
            {
                var filtered = all.Where(e =>
                    e.title.ToLower().Contains(SearchText.ToLower()) ||
                    (e.description != null && e.description.ToLower().Contains(SearchText.ToLower())) ||
                    (e.Category != null && e.Category.code.ToLower().Contains(SearchText.ToLower())));
                Equipments = new ObservableCollection<Equipment>(filtered);
            }
        }

        private void OpenAddDialog()
        {
            var dialog = new EquipmentDialogViewModel(_repo, null);
            dialog.OnSaved += LoadEquipments;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void OpenEditDialog()
        {
            var dialog = new EquipmentDialogViewModel(_repo, SelectedEquipment);
            dialog.OnSaved += LoadEquipments;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void DeleteEquipment()
        {
            var result = MessageBox.Show(
                $"Segur que vols eliminar '{SelectedEquipment.title}'?",
                "Confirmar eliminació",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _repo.Delete(SelectedEquipment.id_equipment);
                LoadEquipments();
            }
        }
    }
}