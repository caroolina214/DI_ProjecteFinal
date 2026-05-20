using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace MilAventures.Reports
{
    public partial class Window1 : Window
    {
        private ReportDocument _informe;

        public Window1(ReportDocument informe, string titol)
        {
            InitializeComponent();

            Title = titol;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _informe = informe;

            reportViewer.Owner = this;
            reportViewer.ViewerCore.ReportSource = informe;
            reportViewer.ToggleSidePanel = SAPBusinessObjects.WPF.Viewer.Constants.SidePanelKind.None;
            reportViewer.ShowToggleSidePanelButton = false;
            reportViewer.ShowToolbar = false;
        }

        /// <summary>Exporta l'informe a PDF i obre el diàleg de guardar.</summary>
        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Title = "Guardar informe com a PDF",
                Filter = "PDF|*.pdf",
                FileName = Title + ".pdf"
            };

            if (saveDialog.ShowDialog() == true)
            {
                var options = new ExportOptions();
                var destOptions = new DiskFileDestinationOptions
                {
                    DiskFileName = saveDialog.FileName
                };

                options.ExportDestinationType = ExportDestinationType.DiskFile;
                options.ExportFormatType = ExportFormatType.PortableDocFormat;
                options.ExportDestinationOptions = destOptions;

                _informe.Export(options);

                MessageBox.Show("Informe exportat correctament!",
                    "PDF guardat", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}