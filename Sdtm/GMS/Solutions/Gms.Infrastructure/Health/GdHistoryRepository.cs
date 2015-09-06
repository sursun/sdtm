using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class GdHistoryRepository : RepositoryBase<GdHistory>, IGdHistoryRepository
    {
        //protected override IQueryable<FollowUpInfo> LoadQuery<TQ>(TQ query)
        //{
        //    IQueryable<Patient> q = base.LoadQuery(query);
        //    var patientQuery = query as PatientQuery;
        //    if (patientQuery == null) return q;
            
        //    return q;
        //}

        public GdHistory GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }

        public override GdHistory SaveOrUpdate(GdHistory entity)
        {
            entity.Computer();
            return base.SaveOrUpdate(entity);
        }
    }
}
