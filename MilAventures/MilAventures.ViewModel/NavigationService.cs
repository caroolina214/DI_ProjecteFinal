
using MilAventures.ViewModel;
using System;

namespace MilAventures.ViewModel
{
    public class NavigationService
    {
        private static NavigationService _instance;
        public static NavigationService Instance => _instance ?? (_instance = new NavigationService());

        public event Action<BaseViewModel> OnNavigate;
        public event Action<BaseViewModel> OnOpenDialog;
        public event Action OnCloseDialog;
        public event Action<string, DateTime?, DateTime?> OnOpenReport;

        public void OpenReport(string tipus, DateTime? dataInici, DateTime? dataFi)
        {
            OnOpenReport?.Invoke(tipus, dataInici, dataFi);
        }

        public void NavigateTo(BaseViewModel viewModel)
        {
            OnNavigate?.Invoke(viewModel);
        }

        public void OpenDialog(BaseViewModel viewModel)
        {
            OnOpenDialog?.Invoke(viewModel);
        }

        public void CloseDialog()
        {
            OnCloseDialog?.Invoke();
        }
    }
}