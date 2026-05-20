using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace MilAventures.ViewModel.Guides
{
    public class GuideDialogViewModel : BaseViewModel
    {
        private readonly GuideRepository _repo;
        private readonly bool _isEdit;
        private int _id;

        public event Action OnSaved;
        public string DialogTitle => _isEdit ? "Editar guia" : "Nou guia";

        // Ruta completa per mostrar la imatge a la UI
        private string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set { _photoPath = value; OnPropertyChanged(nameof(PhotoPath)); }
        }

        // Nom del fitxer que es guarda a la BD
        private string _photoFileName;

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Specialty { get; set; }
        public string Credentials { get; set; }
        public string ExperienceLevel { get; set; }
        public bool Status { get; set; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand SelectPhotoCommand { get; }

        public GuideDialogViewModel(GuideRepository repo, Guide guide)
        {
            _repo = repo;
            _isEdit = guide != null;

            if (_isEdit)
            {
                _id = guide.id_guide;
                Name = guide.name;
                Surname = guide.surname;
                Email = guide.email;
                Phone = guide.phone;
                Specialty = guide.specialty;
                Credentials = guide.credentials;
                ExperienceLevel = guide.experience_level;
                Status = guide.status;
                _photoFileName = guide.photo;
                PhotoPath = GetFullPhotoPath(guide.photo);
            }
            else
            {
                Status = true;
            }

            SaveCommand = new RelayCommand(_ => Save(),
                _ => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname));
            CancelCommand = new RelayCommand(_ => NavigationService.Instance.CloseDialog());
            SelectPhotoCommand = new RelayCommand(_ => SelectPhoto());
        }

        /// <summary>Obri el diàleg de selecció de foto.</summary>
        private void SelectPhoto()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Selecciona una foto",
                Filter = "Imatges|*.jpg;*.jpeg;*.png;*.bmp",
                Multiselect = false
            };

            if (dialog.ShowDialog() == true)
            {
                string destFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MilAventures", "Photos");
                Directory.CreateDirectory(destFolder);


                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(dialog.FileName)}";
                string destPath = Path.Combine(destFolder, fileName);
                File.Copy(dialog.FileName, destPath, overwrite: true);

                _photoFileName = fileName;
                PhotoPath = destPath;
            }
        }

        /// <summary>Construeix la ruta completa a partir del nom del fitxer.</summary>
        private string GetFullPhotoPath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MilAventures", "Photos", fileName);
        }

        private void Save()
        {
            if (!string.IsNullOrWhiteSpace(Email) && _repo.ExistsEmail(Email, _isEdit ? _id : 0))
            {
                MessageBox.Show("Ja existeix un guia amb aquest email.",
                    "Email duplicat", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_isEdit)
            {
                var entity = _repo.GetById(_id);
                entity.name = Name;
                entity.surname = Surname;
                entity.email = Email;
                entity.phone = Phone;
                entity.specialty = Specialty;
                entity.credentials = Credentials;
                entity.experience_level = ExperienceLevel;
                entity.status = Status;
                entity.photo = _photoFileName;
                _repo.Update(entity);
            }
            else
            {
                _repo.Add(new Guide
                {
                    name = Name,
                    surname = Surname,
                    email = Email,
                    phone = Phone,
                    specialty = Specialty,
                    credentials = Credentials,
                    experience_level = ExperienceLevel,
                    status = Status,
                    photo = _photoFileName
                });
            }

            OnSaved?.Invoke();
            NavigationService.Instance.CloseDialog();
        }
    }
}