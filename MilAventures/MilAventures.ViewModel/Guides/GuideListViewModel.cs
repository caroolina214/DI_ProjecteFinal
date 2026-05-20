using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MilAventures.ViewModel.Guides
{
    public class GuideListViewModel : BaseViewModel
    {
        private readonly GuideRepository _repo;

        private ObservableCollection<Guide> _guides;
        public ObservableCollection<Guide> Guides
        {
            get => _guides;
            set { _guides = value; OnPropertyChanged(nameof(Guides)); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterGuides(); }
        }

        private Guide _selectedGuide;
        public Guide SelectedGuide
        {
            get => _selectedGuide;
            set { _selectedGuide = value; OnPropertyChanged(nameof(SelectedGuide)); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public GuideListViewModel(GuideRepository repo)
        {
            _repo = repo;
            LoadGuides();

            AddCommand = new RelayCommand(_ => OpenAddDialog());
            EditCommand = new RelayCommand(_ => OpenEditDialog(), _ => SelectedGuide != null);
            DeleteCommand = new RelayCommand(_ => DeleteGuide(), _ => SelectedGuide != null);
        }

        private void LoadGuides()
        {
            var all = _repo.GetAll();
            Guides = new ObservableCollection<Guide>(all);
        }

        private void FilterGuides()
        {
            var all = _repo.GetAll();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Guides = new ObservableCollection<Guide>(all);
            }
            else
            {
                var filtered = all.Where(g =>
                    g.name.ToLower().Contains(SearchText.ToLower()) ||
                    g.surname.ToLower().Contains(SearchText.ToLower()) ||
                    (g.email != null && g.email.ToLower().Contains(SearchText.ToLower())) ||
                    (g.specialty != null && g.specialty.ToLower().Contains(SearchText.ToLower())));
                Guides = new ObservableCollection<Guide>(filtered);
            }
        }

        private void OpenAddDialog()
        {
            var dialog = new GuideDialogViewModel(_repo, null);
            dialog.OnSaved += LoadGuides;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void OpenEditDialog()
        {
            var dialog = new GuideDialogViewModel(_repo, SelectedGuide);
            dialog.OnSaved += LoadGuides;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void DeleteGuide()
        {
            var result = MessageBox.Show(
                $"Segur que vols eliminar el guia '{SelectedGuide.name} {SelectedGuide.surname}'?",
                "Confirmar eliminació",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _repo.Delete(SelectedGuide.id_guide);
                LoadGuides();
            }
        }
    }
}