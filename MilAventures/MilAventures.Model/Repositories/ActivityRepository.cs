using MilAventures.Model.Context;
using MilAventures.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MilAventures.Model.Repositories
{
    public class ActivityRepository : GenericRepository<Activity>
    {
        public ActivityRepository(MilAventuresContext context) : base(context) { }

        public override IEnumerable<Activity> GetAll()
        {
            return _dbSet
                .Include(a => a.Category)
                .Include(a => a.Guide)
                .ToList();
        }

        public override Activity GetById(int id)
        {
            return _dbSet
                .Include(a => a.Category)
                .Include(a => a.Guide)
                .FirstOrDefault(a => a.id_activity == id);
        }
    }
}