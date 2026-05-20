using MilAventures.Model.Context;
using MilAventures.Model.Models;

namespace MilAventures.Model.Repositories
{
    public class BookingStatusRepository : GenericRepository<BookingStatus>
    {
        public BookingStatusRepository(MilAventuresContext context) : base(context) { }
    }
}