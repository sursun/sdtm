using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class GdmRiskRepository : RepositoryBase<GdmRisk>, IGdmRiskRepository
    {
        //protected override IQueryable<FollowUpInfo> LoadQuery<TQ>(TQ query)
        //{
        //    IQueryable<Patient> q = base.LoadQuery(query);
        //    var patientQuery = query as PatientQuery;
        //    if (patientQuery == null) return q;
            
        //    return q;
        //}

        public GdmRisk GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }

        public override GdmRisk SaveOrUpdate(GdmRisk entity)
        {
            entity.Computer();
            return base.SaveOrUpdate(entity);
        }
    }
}
