using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public interface IMedicalHistoryRepository : IRepositoryBase<MedicalHistory>
    {
        MedicalHistory GetBy(int patientid);
    }
}
