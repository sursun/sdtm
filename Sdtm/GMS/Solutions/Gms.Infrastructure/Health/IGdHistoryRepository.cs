using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public interface IGdHistoryRepository : IRepositoryBase<GdHistory>
    {
        GdHistory GetBy(int patientid);
    }
}
