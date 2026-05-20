using CrystalDecisions.CrystalReports.Engine;
using MilAventures.Model.Context;
using MilAventures.Model.Repositories;
using System;
using System.Linq;

namespace MilAventures.Reports
{
    public static class InformeFactory
    {
        /// <summary>Informe 1 — Llistat simple d'activitats.</summary>
        public static ReportDocument CrearInformeActivitats()
        {
            var ds = new dsActivitats();
            var context = new MilAventuresContext();
            var repo = new ActivityRepository(context);

            foreach (var a in repo.GetAll())
            {
                ds.Activitat.Rows.Add(
                    a.title,
                    a.Category?.code ?? "-",
                    a.Guide != null ? a.Guide.name + " " + a.Guide.surname : "-",
                    ConvertirDificultat(a.difficulty),
                    a.price_per_person,
                    a.max_participants,
                    a.init_date,
                    a.start_end_point ?? "-"
                );
            }

            var informe = new rptActivitats();
            informe.SetDataSource(ds);
            return informe;
        }

        /// <summary>Informe 2 — Reserves agrupades per estat.</summary>
        public static ReportDocument CrearInformeReservesPerEstat()
        {
            var ds = new dsReserves();
            var context = new MilAventuresContext();
            var repo = new BookingRepository(context);

            foreach (var b in repo.GetAll())
            {
                ds.Reserva.Rows.Add(
                    b.Client != null ? b.Client.name + " " + b.Client.surname : "-",
                    b.BookingStatus?.code ?? "-",
                    b.created_at,
                    b.total_price,
                    b.participants
                );
            }

            var informe = new rptReservesPerEstat();
            informe.SetDataSource(ds);
            return informe;
        }

        /// <summary>Informe 3 — Estadístiques generals.</summary>
        public static ReportDocument CrearInformeEstadistiques()
        {
            var ds = new dsEstadistiques();
            var context = new MilAventuresContext();
            var repo = new BookingRepository(context);
            var reserves = repo.GetAll().ToList();

            int numReserves = reserves.Count;
            decimal total = reserves.Sum(b => b.total_price);
            decimal mitjana = numReserves > 0 ? total / numReserves : 0;

            ds.Estadistica.Rows.Add(numReserves, total, mitjana);

            var informe = new rptEstadistiques();
            informe.SetDataSource(ds);
            return informe;
        }

        /// <summary>Informe 4 — Reserves filtrades per rang de dates.</summary>
        public static ReportDocument CrearInformeReservesDates(DateTime dataInici, DateTime dataFi)
        {
            var ds = new dsReserves();
            var context = new MilAventuresContext();
            var repo = new BookingRepository(context);

            var reserves = repo.GetAll()
                .Where(b => b.created_at >= dataInici && b.created_at <= dataFi)
                .ToList();

            foreach (var b in reserves)
            {
                ds.Reserva.Rows.Add(
                    b.Client != null ? b.Client.name + " " + b.Client.surname : "-",
                    b.BookingStatus?.code ?? "-",
                    b.created_at,
                    b.total_price,
                    b.participants
                );
            }

            var informe = new rptReservesPerDates();
            informe.SetDataSource(ds);
            return informe;
        }

        private static string ConvertirDificultat(int diff)
        {
            switch (diff)
            {
                case 1: return "Fàcil";
                case 2: return "Principiant";
                case 3: return "Mitjà";
                case 4: return "Avançat";
                case 5: return "Expert";
                default: return "-";
            }
        }
    }
}