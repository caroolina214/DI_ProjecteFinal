using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MilAventures.ViewModel.Activities
{
    public class ActivityDialogViewModel : BaseViewModel
    {
        private readonly ActivityRepository _repo;
        private readonly bool _isEdit;
        private int _id;

        public event Action OnSaved;
        public string DialogTitle => _isEdit ? "Editar activitat" : "Nova activitat";

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        private DateTime _initDate;
        public DateTime InitDate
        {
            get => _initDate;
            set { _initDate = value; OnPropertyChanged(nameof(InitDate)); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(nameof(EndDate)); }
        }

        private string _difficulty;
        public string Difficulty
        {
            get => _difficulty;
            set { _difficulty = value; OnPropertyChanged(nameof(Difficulty)); }
        }

        private int _maxParticipants;
        public int MaxParticipants
        {
            get => _maxParticipants;
            set { _maxParticipants = value; OnPropertyChanged(nameof(MaxParticipants)); }
        }

        private string _startEndPoint;
        public string StartEndPoint
        {
            get => _startEndPoint;
            set { _startEndPoint = value; OnPropertyChanged(nameof(StartEndPoint)); }
        }

        private decimal _pricePerPerson;
        public decimal PricePerPerson
        {
            get => _pricePerPerson;
            set { _pricePerPerson = value; OnPropertyChanged(nameof(PricePerPerson)); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); }
        }

        private Guide _selectedGuide;
        public Guide SelectedGuide
        {
            get => _selectedGuide;
            set { _selectedGuide = value; OnPropertyChanged(nameof(SelectedGuide)); }
        }

        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Guide> Guides { get; set; }
        public ObservableCollection<string> Difficulties { get; set; } =
            new ObservableCollection<string> { "Fàcil", "Principiant", "Mitjà", "Avançat", "Expert" };

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ActivityDialogViewModel(ActivityRepository repo, Activity activity)
        {
            _repo = repo;
            _isEdit = activity != null;

            var context = repo._context;
            Categories = new ObservableCollection<Category>(
                new CategoryRepository(context).GetAll());
            Guides = new ObservableCollection<Guide>(
                new GuideRepository(context).GetAll().Where(g => g.status));

            if (_isEdit)
            {
                _id = activity.id_activity;
                Title = activity.title;
                Description = activity.description;
                InitDate = activity.init_date;
                EndDate = activity.end_date;
                Difficulty = ConvertDifficulty(activity.difficulty);
                MaxParticipants = activity.max_participants;
                StartEndPoint = activity.start_end_point;
                PricePerPerson = activity.price_per_person;
                SelectedCategory = Categories.FirstOrDefault(c => c.id_category == activity.id_category);
                SelectedGuide = Guides.FirstOrDefault(g => g.id_guide == activity.id_guide);
            }
            else
            {
                InitDate = DateTime.Today;
                EndDate = DateTime.Today.AddHours(4);
                Difficulty = "Fàcil";
                MaxParticipants = 10;
                PricePerPerson = 0;
            }

            SaveCommand = new RelayCommand(_ => Save(),
                _ => !string.IsNullOrWhiteSpace(Title) && SelectedCategory != null);
            CancelCommand = new RelayCommand(_ => NavigationService.Instance.CloseDialog());
        }

        /// <summary>Converteix int a text de dificultat.</summary>
        private string ConvertDifficulty(int diff)
        {
            switch (diff)
            {
                case 1: return "Fàcil";
                case 2: return "Principiant";
                case 3: return "Mitjà";
                case 4: return "Avançat";
                case 5: return "Expert";
                default: return "Fàcil";
            }
        }

        /// <summary>Converteix text de dificultat a int.</summary>
        private int ConvertDifficultyToInt(string diff)
        {
            switch (diff)
            {
                case "Fàcil": return 1;
                case "Principiant": return 2;
                case "Mitjà": return 3;
                case "Avançat": return 4;
                case "Expert": return 5;
                default: return 1;
            }
        }

        /// <summary>Valida i desa l'activitat a la BD.</summary>
        private void Save()
        {
            if (EndDate <= InitDate)
            {
                MessageBox.Show("La data de fi ha de ser posterior a la data d'inici.",
                    "Error de dates", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_isEdit)
            {
                var entity = _repo.GetById(_id);
                entity.title = Title;
                entity.description = Description;
                entity.init_date = InitDate;
                entity.end_date = EndDate;
                entity.difficulty = ConvertDifficultyToInt(Difficulty);
                entity.max_participants = MaxParticipants;
                entity.start_end_point = StartEndPoint;
                entity.price_per_person = PricePerPerson;
                entity.id_category = SelectedCategory.id_category;
                entity.id_guide = SelectedGuide?.id_guide ?? entity.id_guide;
                _repo.Update(entity);
            }
            else
            {
                _repo.Add(new Activity
                {
                    title = Title,
                    description = Description,
                    init_date = InitDate,
                    end_date = EndDate,
                    difficulty = ConvertDifficultyToInt(Difficulty),
                    max_participants = MaxParticipants,
                    start_end_point = StartEndPoint,
                    price_per_person = PricePerPerson,
                    id_category = SelectedCategory.id_category,
                    id_guide = SelectedGuide?.id_guide ?? 0
                });
            }

            OnSaved?.Invoke();
            NavigationService.Instance.CloseDialog();
        }
    }
}