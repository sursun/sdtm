using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class DiagnosesRepository : RepositoryBase<Diagnoses>, IDiagnosesRepository
    {
        public override Diagnoses SaveOrUpdate(Diagnoses entity)
        {
            entity.Computer();
            return base.SaveOrUpdate(entity);
        }
    }
}
