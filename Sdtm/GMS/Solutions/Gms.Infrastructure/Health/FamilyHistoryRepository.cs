using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class FamilyHistoryRepository : RepositoryBase<FamilyHistory>, IFamilyHistoryRepository
    {
        public FamilyHistory GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }

        public override FamilyHistory SaveOrUpdate(FamilyHistory entity)
        {
            entity.Computer();
            return base.SaveOrUpdate(entity);
        }
    }
}
