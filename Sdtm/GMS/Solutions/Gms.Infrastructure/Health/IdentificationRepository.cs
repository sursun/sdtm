using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class IdentificationRepository : RepositoryBase<Identification>, IIdentificationRepository
    {
        //protected override IQueryable<FollowUpInfo> LoadQuery<TQ>(TQ query)
        //{
        //    IQueryable<Patient> q = base.LoadQuery(query);
        //    var patientQuery = query as PatientQuery;
        //    if (patientQuery == null) return q;
            
        //    return q;
        //}

        public Identification GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }

        public override Identification SaveOrUpdate(Identification entity)
        {
            entity.Computer();
            return base.SaveOrUpdate(entity);
        }
    }
}
