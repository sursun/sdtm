using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public interface IHealthEducationRepository : IRepositoryBase<HealthEducation>
    {
        HealthEducation GetBy(int patientid);
    }
}
