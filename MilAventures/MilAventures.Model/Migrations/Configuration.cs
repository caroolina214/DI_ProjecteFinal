namespace MilAventures.Model.Migrations
{
    using MilAventures.Model.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Context.MilAventuresContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Context.MilAventuresContext context)
        {
            context.BookingStatuses.AddOrUpdate(s => s.code,
                new BookingStatus { code = "PENDING", description = "Reserva pendent de confirmar" },
                new BookingStatus { code = "ACCEPTED", description = "Reserva confirmada" },
                new BookingStatus { code = "RUNNING", description = "Reserva en curs" },
                new BookingStatus { code = "ENDED", description = "Reserva finalitzada" },
                new BookingStatus { code = "CANCELED", description = "Reserva cancel·lada" }
            );

            context.EquipmentStatuses.AddOrUpdate(s => s.code,
                new EquipmentStatus { code = "AVAILABLE", description = "Equipament disponible" },
                new EquipmentStatus { code = "MAINTENANCE", description = "En manteniment" },
                new EquipmentStatus { code = "OUTDATED", description = "Fora de servei" }
            );

            context.SaveChanges();
        }
    }
}
