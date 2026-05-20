using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MilAventures.ViewModel.Categories
{
    public class CategoryListViewModel : BaseViewModel
    {
        private readonly CategoryRepository _repo;

        // Llista completa i llista filtrada
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        // Text de cerca
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterCategories();
            }
        }

        // Categoria seleccionada al DataGrid
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); }
        }

        // Comandes
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public CategoryListViewModel(CategoryRepository repo)
        {
            _repo = repo;
            LoadCategories();

            AddCommand = new RelayCommand(_ => OpenAddDialog());
            EditCommand = new RelayCommand(_ => OpenEditDialog(), _ => SelectedCategory != null);
            DeleteCommand = new RelayCommand(_ => DeleteCategory(), _ => SelectedCategory != null);
        }

        /// <summary>Carrega totes les categories de la BD.</summary>
        private void LoadCategories()
        {
            var all = _repo.GetAll();
            Categories = new ObservableCollection<Category>(all);
        }

        /// <summary>Filtra la llista pel text de cerca.</summary>
        private void FilterCategories()
        {
            var all = _repo.GetAll();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Categories = new ObservableCollection<Category>(all);
            }
            else
            {
                var filtered = all.Where(c =>
                    c.code.ToLower().Contains(SearchText.ToLower()) ||
                    (c.description != null && c.description.ToLower().Contains(SearchText.ToLower())));
                Categories = new ObservableCollection<Category>(filtered);
            }
        }

        /// <summary>Obri el dialog per afegir una nova categoria.</summary>
        private void OpenAddDialog()
        {
            var dialog = new CategoryDialogViewModel(_repo, null);
            dialog.OnSaved += LoadCategories;
            NavigationService.Instance.OpenDialog(dialog);
        }

        /// <summary>Obri el dialog per editar la categoria seleccionada.</summary>
        private void OpenEditDialog()
        {
            var dialog = new CategoryDialogViewModel(_repo, SelectedCategory);
            dialog.OnSaved += LoadCategories;
            NavigationService.Instance.OpenDialog(dialog);
        }

        /// <summary>Elimina la categoria seleccionada després de confirmació.</summary>
        private void DeleteCategory()
        {
            var result = MessageBox.Show(
                $"Segur que vols eliminar la categoria '{SelectedCategory.code}'?",
                "Confirmar eliminació",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _repo.Delete(SelectedCategory.id_category);
                LoadCategories();
            }
        }
    }
}