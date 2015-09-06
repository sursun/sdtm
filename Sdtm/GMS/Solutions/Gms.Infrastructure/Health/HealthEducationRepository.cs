using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class HealthEducationRepository : RepositoryBase<HealthEducation>, IHealthEducationRepository
    {
        public HealthEducation GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }
    }
}
