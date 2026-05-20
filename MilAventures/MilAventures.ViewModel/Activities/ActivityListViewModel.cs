using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MilAventures.ViewModel.Activities
{
    public class ActivityListViewModel : BaseViewModel
    {
        private readonly ActivityRepository _repo;

        private ObservableCollection<Activity> _activities;
        public ObservableCollection<Activity> Activities
        {
            get => _activities;
            set { _activities = value; OnPropertyChanged(nameof(Activities)); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterActivities(); }
        }

        private Activity _selectedActivity;
        public Activity SelectedActivity
        {
            get => _selectedActivity;
            set { _selectedActivity = value; OnPropertyChanged(nameof(SelectedActivity)); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public ActivityListViewModel(ActivityRepository repo)
        {
            _repo = repo;
            LoadActivities();

            AddCommand = new RelayCommand(_ => OpenAddDialog());
            EditCommand = new RelayCommand(_ => OpenEditDialog(), _ => SelectedActivity != null);
            DeleteCommand = new RelayCommand(_ => DeleteActivity(), _ => SelectedActivity != null);
        }

        private void LoadActivities()
        {
            Activities = new ObservableCollection<Activity>(_repo.GetAll());
        }

        private void FilterActivities()
        {
            var all = _repo.GetAll();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Activities = new ObservableCollection<Activity>(all);
            }
            else
            {
                var filtered = all.Where(a =>
                    a.title.ToLower().Contains(SearchText.ToLower()) ||
                    (a.Category != null && a.Category.code.ToLower().Contains(SearchText.ToLower())) ||
                    (a.Guide != null && a.Guide.name.ToLower().Contains(SearchText.ToLower())));
                Activities = new ObservableCollection<Activity>(filtered);
            }
        }

        private void OpenAddDialog()
        {
            var dialog = new ActivityDialogViewModel(_repo, null);
            dialog.OnSaved += LoadActivities;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void OpenEditDialog()
        {
            var dialog = new ActivityDialogViewModel(_repo, SelectedActivity);
            dialog.OnSaved += LoadActivities;
            NavigationService.Instance.OpenDialog(dialog);
        }

        private void DeleteActivity()
        {
            var result = MessageBox.Show(
                $"Segur que vols eliminar '{SelectedActivity.title}'?",
                "Confirmar eliminació",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _repo.Delete(SelectedActivity.id_activity);
                LoadActivities();
            }
        }
    }
}