using MilAventures.Model.Context;
using MilAventures.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace MilAventures.Model.Repositories
{
    public class GuideRepository : GenericRepository<Guide>
    {
        public GuideRepository(MilAventuresContext context) : base(context) { }

        /// <summary>Comprova si ja existeix un guia amb el mateix email.</summary>
        public bool ExistsEmail(string email, int excludeId = 0)
        {
            return _dbSet.Any(g => g.email == email && g.id_guide != excludeId);
        }
    }
}