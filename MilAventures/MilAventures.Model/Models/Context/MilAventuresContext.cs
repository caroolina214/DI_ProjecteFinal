using MilAventures.Model.Models;
using System.Data.Entity;

namespace MilAventures.Model.Context
{
    public class MilAventuresContext : DbContext
    {
        public MilAventuresContext() : base("name=MilAventuresConnection"){ }

        public DbSet<Category> Categories { get; set; }
        public DbSet<BookingStatus> BookingStatuses { get; set; }
        public DbSet<EquipmentStatus> EquipmentStatuses { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingLine> BookingLines { get; set; }

    }
}