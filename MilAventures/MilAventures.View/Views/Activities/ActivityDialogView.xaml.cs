using MilAventures.ViewModel;
using MilAventures.ViewModel.Activities;
using System.Windows;

namespace MilAventures.View.Views.Activities
{
    public partial class ActivityDialogView : Window
    {
        public ActivityDialogView(ActivityDialogViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
            NavigationService.Instance.OnCloseDialog += Close;
            Closed += (s, e) => NavigationService.Instance.OnCloseDialog -= Close;
        }
    }
}