using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class MedicalHistoryRepository : RepositoryBase<MedicalHistory>, IMedicalHistoryRepository
    {
        public MedicalHistory GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }

        public override MedicalHistory SaveOrUpdate(MedicalHistory entity)
        {
            entity.Computer();
            return base.SaveOrUpdate(entity);
        }
    }
}
