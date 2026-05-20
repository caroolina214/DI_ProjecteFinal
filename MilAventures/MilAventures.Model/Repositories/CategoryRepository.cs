using MilAventures.Model.Context;
using MilAventures.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace MilAventures.Model.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(MilAventuresContext context) : base(context) { }

        /// <summary>Comprova si existeix una categoria amb el mateix codi.</summary>
        public bool ExistsCode(string code, int excludeId = 0)
        {
            return _dbSet.Any(c => c.code == code && c.id_category != excludeId);
        }
    }
}