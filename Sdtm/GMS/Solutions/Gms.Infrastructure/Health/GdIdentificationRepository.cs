using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class GdIdentificationRepository : RepositoryBase<GdIdentification>, IGdIdentificationRepository
    {
        //protected override IQueryable<FollowUpInfo> LoadQuery<TQ>(TQ query)
        //{
        //    IQueryable<Patient> q = base.LoadQuery(query);
        //    var patientQuery = query as PatientQuery;
        //    if (patientQuery == null) return q;
            
        //    return q;
        //}

        public GdIdentification GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }

        public override GdIdentification SaveOrUpdate(GdIdentification entity)
        {
            entity.Computer();
            return base.SaveOrUpdate(entity);
        }
    }
}
