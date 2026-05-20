using MilAventures.View.Views.Categories;
using MilAventures.View.Views.Clients;
using MilAventures.View.Views.Equipments;
using MilAventures.View.Views.Guides;
using MilAventures.View.Views.Activities;
using MilAventures.View.Views.Bookings;
using MilAventures.ViewModel;
using MilAventures.ViewModel.Categories;
using MilAventures.ViewModel.Clients;
using MilAventures.ViewModel.Guides;
using MilAventures.ViewModel.Equipments;
using MilAventures.ViewModel.Activities;
using MilAventures.ViewModel.Bookings;
using MilAventures.Reports;
using System;
using System.Windows;

namespace MilAventures
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                DataContext = new MainViewModel();
                NavigationService.Instance.OnOpenDialog += OpenDialog;
                NavigationService.Instance.OnOpenReport += (tipus, dataInici, dataFi) =>
                {
                    MilAventures.Reports.Window1 finestra = null;
                    switch (tipus)
                    {
                        case "Activitats":
                            finestra = new Window1(
                                MilAventures.Reports.InformeFactory.CrearInformeActivitats(),
                                "Llistat d'Activitats");
                            break;
                        case "ReservesEstat":
                            finestra = new MilAventures.Reports.Window1(
                                MilAventures.Reports.InformeFactory.CrearInformeReservesPerEstat(),
                                "Reserves per Estat");
                            break;
                        case "Estadistiques":
                            finestra = new MilAventures.Reports.Window1(
                                MilAventures.Reports.InformeFactory.CrearInformeEstadistiques(),
                                "Estadístiques");
                            break;
                        case "ReservesDates":
                            finestra = new MilAventures.Reports.Window1(
                                MilAventures.Reports.InformeFactory.CrearInformeReservesDates(
                                    dataInici ?? DateTime.Today.AddMonths(-1),
                                    dataFi ?? DateTime.Today),
                                "Reserves per Dates");
                            break;
                    }
                    if (finestra != null) finestra.Show();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException?.Message, "Error MainViewModel");
            }
        }

        private void OpenDialog(BaseViewModel vm)
        {
            if (vm is CategoryDialogViewModel categoryVm)
            {
                var dialog = new CategoryDialogView(categoryVm);
                dialog.Owner = this;
                dialog.ShowDialog();
            }
            else if (vm is GuideDialogViewModel guideVm)
            {
                var dialog = new GuideDialogView(guideVm);
                dialog.Owner = this;
                dialog.ShowDialog();
            }
            else if (vm is ClientDialogViewModel clientVm)
            {
                var dialog = new ClientDialogView(clientVm);
                dialog.Owner = this;
                dialog.ShowDialog();
            }
            else if (vm is EquipmentDialogViewModel eqVm)
            {
                var dialog = new EquipmentDialogView(eqVm);
                dialog.Owner = this;
                dialog.ShowDialog();
            }
            else if (vm is ActivityDialogViewModel actVm)
            {
                var dialog = new ActivityDialogView(actVm);
                dialog.Owner = this;
                dialog.ShowDialog();
            }
            else if (vm is BookingDialogViewModel bookingVm)
            {
                var dialog = new BookingDialogView(bookingVm);
                dialog.Owner = this;
                dialog.ShowDialog();
            }
        }
    }
}