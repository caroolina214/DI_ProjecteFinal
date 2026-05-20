using System;
using System.Windows;

namespace MilAventures.ViewModel.Reports
{
    public class InformesViewModel : BaseViewModel
    {
        public RelayCommand InformeActivitatsCommand { get; }
        public RelayCommand InformeReservesEstatCommand { get; }
        public RelayCommand InformeEstadistiquesCommand { get; }
        public RelayCommand InformeReservesDatesCommand { get; }

        public InformesViewModel()
        {
            InformeActivitatsCommand = new RelayCommand(_ =>
                NavigationService.Instance.OpenReport("Activitats", null, null));

            InformeReservesEstatCommand = new RelayCommand(_ =>
                NavigationService.Instance.OpenReport("ReservesEstat", null, null));

            InformeEstadistiquesCommand = new RelayCommand(_ =>
                NavigationService.Instance.OpenReport("Estadistiques", null, null));

            InformeReservesDatesCommand = new RelayCommand(_ =>
                NavigationService.Instance.OpenReport("ReservesDates",
                    DateTime.Today.AddMonths(-1), DateTime.Today));
        }
    }
}