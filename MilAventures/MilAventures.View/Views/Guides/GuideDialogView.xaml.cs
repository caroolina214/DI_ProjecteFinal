using MilAventures.ViewModel;
using MilAventures.ViewModel.Guides;
using System.Windows;

namespace MilAventures.View.Views.Guides
{
    public partial class GuideDialogView : Window
    {
        public GuideDialogView(GuideDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            NavigationService.Instance.OnCloseDialog += Close;
            Closed += (s, e) => NavigationService.Instance.OnCloseDialog -= Close;
        }
    }
}