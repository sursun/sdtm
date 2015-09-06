using System.Collections.Generic;
using Gms.Domain;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public interface IMedicalHistoryItemRepository : IRepositoryBase<MedicalHistoryItem>
    {
        IList<MedicalHistoryItem> GetBy(int mId, MedicalHistoryType type);
    }
}
