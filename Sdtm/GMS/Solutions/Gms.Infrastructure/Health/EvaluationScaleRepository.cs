using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class EvaluationScaleRepository : RepositoryBase<EvaluationScale>, IEvaluationScaleRepository
    {
        protected override IQueryable<EvaluationScale> LoadQuery<TQ>(TQ query)
        {
            IQueryable<EvaluationScale> q = base.LoadQuery(query);
            var evaluationScaleQuery = query as EvaluationScaleQuery;
            if (evaluationScaleQuery == null) return q;

            if (evaluationScaleQuery.PatientId.HasValue)
            {
                q = q.Where(c => c.Patient.Id == evaluationScaleQuery.PatientId);
            }

            return q;
        }
        
    }
}
