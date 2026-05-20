using MilAventures.Model.Context;
using MilAventures.Model.Models;
using System.Linq;

namespace MilAventures.Model.Repositories
{
    public class ClientRepository : GenericRepository<Client>
    {
        public ClientRepository(MilAventuresContext context) : base(context) { }

        /// <summary>Comprova si ja existeix un client amb el mateix email.</summary>
        public bool ExistsEmail(string email, int excludeId = 0)
        {
            return _dbSet.Any(c => c.email == email && c.id_client != excludeId);
        }
    }
}