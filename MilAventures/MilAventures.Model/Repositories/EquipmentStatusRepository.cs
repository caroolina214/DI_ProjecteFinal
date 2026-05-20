using MilAventures.Model.Context;
using MilAventures.Model.Models;

namespace MilAventures.Model.Repositories
{
    public class EquipmentStatusRepository : GenericRepository<EquipmentStatus>
    {
        public EquipmentStatusRepository(MilAventuresContext context) : base(context) { }
    }
}