using System.Collections.Generic;
using System.Linq;
using Gms.Domain;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class MedicalHistoryItemRepository : RepositoryBase<MedicalHistoryItem>, IMedicalHistoryItemRepository
    {
        public IList<MedicalHistoryItem> GetBy(int mId, MedicalHistoryType type)
        {
            return Query.Where(c => (c.MedicalHistory.Id==mId && c.MedicalHistoryType == type )).ToList();
        }
    }
}
