using MilAventures.ViewModel;
using MilAventures.ViewModel.Bookings;
using System.Windows;

namespace MilAventures.View.Views.Bookings
{
    public partial class BookingDialogView : Window
    {
        public BookingDialogView(BookingDialogViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
            NavigationService.Instance.OnCloseDialog += Close;
            Closed += (s, e) => NavigationService.Instance.OnCloseDialog -= Close;
        }
    }
}