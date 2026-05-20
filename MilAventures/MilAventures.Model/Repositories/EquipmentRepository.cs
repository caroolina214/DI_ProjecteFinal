using MilAventures.Model.Context;
using MilAventures.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MilAventures.Model.Repositories
{
    public class EquipmentRepository : GenericRepository<Equipment>
    {
        public EquipmentRepository(MilAventuresContext context) : base(context) { }

        /// <summary>Retorna tot l'equipament amb categoria i estat carregats.</summary>
        public override IEnumerable<Equipment> GetAll()
        {
            return _dbSet
                .Include(e => e.Category)
                .Include(e => e.EquipmentStatus)
                .ToList();
        }

        public override Equipment GetById(int id)
        {
            return _dbSet
                .Include(e => e.Category)
                .Include(e => e.EquipmentStatus)
                .FirstOrDefault(e => e.id_equipment == id);
        }

        /// <summary>Retorna equipament amb stock per sota del mínim.</summary>
        public IEnumerable<Equipment> GetLowStock()
        {
            return _dbSet
                .Include(e => e.Category)
                .Where(e => e.units <= e.min_stock)
                .ToList();
        }
    }
}