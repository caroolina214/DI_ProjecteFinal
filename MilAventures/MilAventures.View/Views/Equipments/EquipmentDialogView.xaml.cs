using MilAventures.ViewModel;
using MilAventures.ViewModel.Equipments;
using System.Windows;

namespace MilAventures.View.Views.Equipments
{
    public partial class EquipmentDialogView : Window
    {
        public EquipmentDialogView(EquipmentDialogViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
            NavigationService.Instance.OnCloseDialog += Close;
            Closed += (s, e) => NavigationService.Instance.OnCloseDialog -= Close;
        }
    }
}