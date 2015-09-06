using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public interface IIdentificationRepository : IRepositoryBase<Identification>
    {
        Identification GetBy(int patientid);
    }
}
