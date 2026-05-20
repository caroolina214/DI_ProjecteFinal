using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MilAventures.ViewModel.Equipments
{
    public class EquipmentDialogViewModel : BaseViewModel
    {
        private readonly EquipmentRepository _repo;
        private readonly bool _isEdit;
        private int _id;

        public event Action OnSaved;
        public string DialogTitle => _isEdit ? "Editar equipament" : "Nou equipament";

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
        public int Units { get; set; }
        public int MinStock { get; set; }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); }
        }

        private EquipmentStatus _selectedStatus;
        public EquipmentStatus SelectedStatus
        {
            get => _selectedStatus;
            set { _selectedStatus = value; OnPropertyChanged(nameof(SelectedStatus)); }
        }

        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<EquipmentStatus> Statuses { get; set; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public EquipmentDialogViewModel(EquipmentRepository repo, Equipment equipment)
        {
            _repo = repo;
            _isEdit = equipment != null;

            var context = repo._context;
            Categories = new ObservableCollection<Category>(
                new CategoryRepository(context).GetAll());
            Statuses = new ObservableCollection<EquipmentStatus>(
                new EquipmentStatusRepository(context).GetAll());

            if (_isEdit)
            {
                _id = equipment.id_equipment;
                Title = equipment.title;
                Description = equipment.description;
                PricePerDay = equipment.price_per_day;
                Units = equipment.units;
                MinStock = equipment.min_stock;
                SelectedCategory = Categories.FirstOrDefault(c => c.id_category == equipment.id_category);
                SelectedStatus = Statuses.FirstOrDefault(s => s.id_status == equipment.id_status);
            }
            else
            {
                Units = 0;
                MinStock = 2;
                PricePerDay = 0;
            }

            SaveCommand = new RelayCommand(_ => Save(),
                _ => !string.IsNullOrWhiteSpace(Title) && SelectedCategory != null && SelectedStatus != null);
            CancelCommand = new RelayCommand(_ => NavigationService.Instance.CloseDialog());
        }

        private void Save()
        {
            if (_isEdit)
            {
                var entity = _repo.GetById(_id);
                entity.title = Title;
                entity.description = Description;
                entity.price_per_day = PricePerDay;
                entity.units = Units;
                entity.min_stock = MinStock;
                entity.id_category = SelectedCategory.id_category;
                entity.id_status = SelectedStatus.id_status;
                _repo.Update(entity);
            }
            else
            {
                _repo.Add(new Equipment
                {
                    title = Title,
                    description = Description,
                    price_per_day = PricePerDay,
                    units = Units,
                    min_stock = MinStock,
                    id_category = SelectedCategory.id_category,
                    id_status = SelectedStatus.id_status
                });
            }

            OnSaved?.Invoke();
            NavigationService.Instance.CloseDialog();
        }
    }
}