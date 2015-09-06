using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class EducationRepository : RepositoryBase<Education>, IEducationRepository
    {
        protected override IQueryable<Education> LoadQuery<TQ>(TQ query)
        {
            IQueryable<Education> q = base.LoadQuery(query);
            var educationQuery = query as EducationQuery;
            if (educationQuery == null) return q;

            if (educationQuery.PatientId.HasValue)
            {
                q = q.Where(c => c.Patient.Id == educationQuery.PatientId);
            }

            return q;
        }
        
    }
}
