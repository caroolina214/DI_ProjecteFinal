using MilAventures.Model.Context;
using MilAventures.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MilAventures.Model.Repositories
{
    /// <summary>
    /// Repositori per a la gestió de reserves.
    /// Inclou càrrega de relacions i actualització de línies.
    /// </summary>
    public class BookingRepository : GenericRepository<Booking>
    {
        /// <summary>Constructor del repositori de reserves.</summary>
        public BookingRepository(MilAventuresContext context) : base(context) { }

        /// <summary>Retorna totes les reserves amb les seues relacions carregades.</summary>
        public override IEnumerable<Booking> GetAll()
        {
            return _dbSet
                .Include(b => b.Client)
                .Include(b => b.BookingStatus)
                .Include(b => b.BookingLines.Select(l => l.Activity))
                .Include(b => b.BookingLines.Select(l => l.Equipment))
                .ToList();
        }

        /// <summary>Retorna una reserva per ID amb totes les relacions carregades.</summary>
        public override Booking GetById(int id)
        {
            return _dbSet
                .Include(b => b.Client)
                .Include(b => b.BookingStatus)
                .Include(b => b.BookingLines.Select(l => l.Activity))
                .Include(b => b.BookingLines.Select(l => l.Equipment))
                .FirstOrDefault(b => b.id_booking == id);
        }

        /// <summary>
        /// Actualitza una reserva eliminant les línies antigues i inserint les noves.
        /// Soluciona el problema del Clear() en EF6.
        /// </summary>
        public void UpdateWithLines(Booking booking, IEnumerable<BookingLine> newLines)
        {
            var entity = GetById(booking.id_booking);

            // Eliminar línies antigues
            var oldLines = entity.BookingLines.ToList();
            foreach (var line in oldLines)
                _context.Entry(line).State = EntityState.Deleted;

            // Actualitzar camps principals
            entity.id_client = booking.id_client;
            entity.id_book_status = booking.id_book_status;
            entity.participants = booking.participants;
            entity.notes = booking.notes;
            entity.total_price = booking.total_price;

            // Afegir línies noves
            foreach (var line in newLines)
            {
                line.bookingId = entity.id_booking;
                _context.Entry(line).State = EntityState.Added;
            }

            _context.SaveChanges();
        }
    }
}