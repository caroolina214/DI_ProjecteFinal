using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System;
using System.Windows;

namespace MilAventures.ViewModel.Categories
{
    public class CategoryDialogViewModel : BaseViewModel
    {
        private readonly CategoryRepository _repo;
        private readonly bool _isEdit;

        public event Action OnSaved;

        private string _code;
        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(nameof(Code)); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        private int _id;

        public string DialogTitle => _isEdit ? "Editar categoria" : "Nova categoria";

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public CategoryDialogViewModel(CategoryRepository repo, Category category)
        {
            _repo = repo;
            _isEdit = category != null;

            if (_isEdit)
            {
                _id = category.id_category;
                Code = category.code;
                Description = category.description;
            }

            SaveCommand = new RelayCommand(_ => Save(), _ => !string.IsNullOrWhiteSpace(Code));
            CancelCommand = new RelayCommand(_ => NavigationService.Instance.CloseDialog());
        }

        /// <summary>Valida i desa la categoria a la BD.</summary>
        private void Save()
        {
            if (_repo.ExistsCode(Code, _isEdit ? _id : 0))
            {
                MessageBox.Show("Ja existeix una categoria amb aquest codi.",
                    "Codi duplicat", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_isEdit)
            {
                var entity = _repo.GetById(_id);
                entity.code = Code;
                entity.description = Description;
                _repo.Update(entity);
            }
            else
            {
                _repo.Add(new Category { code = Code, description = Description });
            }

            OnSaved?.Invoke();
            NavigationService.Instance.CloseDialog();
        }
    }
}