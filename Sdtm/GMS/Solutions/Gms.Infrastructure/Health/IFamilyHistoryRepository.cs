using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public interface IFamilyHistoryRepository : IRepositoryBase<FamilyHistory>
    {
        FamilyHistory GetBy(int patientid);
    }
}
