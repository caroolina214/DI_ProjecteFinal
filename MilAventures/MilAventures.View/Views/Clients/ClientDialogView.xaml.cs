using MilAventures.ViewModel;
using MilAventures.ViewModel.Clients;
using System.Windows;

namespace MilAventures.View.Views.Clients
{
    public partial class ClientDialogView : Window
    {
        public ClientDialogView(ClientDialogViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
            NavigationService.Instance.OnCloseDialog += Close;
            Closed += (s, e) => NavigationService.Instance.OnCloseDialog -= Close;
        }
    }
}