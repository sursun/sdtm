using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public interface IGdIdentificationRepository : IRepositoryBase<GdIdentification>
    {
        GdIdentification GetBy(int patientid);
    }
}
