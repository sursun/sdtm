using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public interface IGdmRiskRepository : IRepositoryBase<GdmRisk>
    {
        GdmRisk GetBy(int patientid);
    }
}
