using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace MilAventures.ViewModel.Clients
{
    public class ClientDialogViewModel : BaseViewModel
    {
        private readonly ClientRepository _repo;
        private readonly bool _isEdit;
        private int _id;

        public event Action OnSaved;
        public string DialogTitle => _isEdit ? "Editar client" : "Nou client";

        private string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set { _photoPath = value; OnPropertyChanged(nameof(PhotoPath)); }
        }
        private string _photoFileName;

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand SelectPhotoCommand { get; }

        public ClientDialogViewModel(ClientRepository repo, Client client)
        {
            _repo = repo;
            _isEdit = client != null;

            if (_isEdit)
            {
                _id = client.id_client;
                Name = client.name;
                Surname = client.surname;
                Email = client.email;
                Phone = client.phone;
                Status = client.status;
                _photoFileName = client.photo;
                PhotoPath = GetFullPhotoPath(client.photo);
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
                MessageBox.Show("Ja existeix un client amb aquest email.",
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
                entity.status = Status;
                entity.photo = _photoFileName;
                _repo.Update(entity);
            }
            else
            {
                _repo.Add(new Client
                {
                    name = Name,
                    surname = Surname,
                    email = Email,
                    phone = Phone,
                    status = Status,
                    photo = _photoFileName
                });
            }

            OnSaved?.Invoke();
            NavigationService.Instance.CloseDialog();
        }
    }
}