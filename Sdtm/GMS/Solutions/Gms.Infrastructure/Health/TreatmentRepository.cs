using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class TreatmentRepository : RepositoryBase<Treatment>, ITreatmentRepository
    {
        public override Treatment SaveOrUpdate(Treatment entity)
        {
            entity.Computer();
            return base.SaveOrUpdate(entity);
        }
        
    }
}
