using System.Linq;
using Gms.Domain.Health;

namespace Gms.Infrastructure.Health
{
    public class ClinicRepository : RepositoryBase<Clinic>, IClinicRepository
    {
        protected override IQueryable<Clinic> LoadQuery<TQ>(TQ query)
        {
            IQueryable<Clinic> q = base.LoadQuery(query);
            var clinicQuery = query as ClinicQuery;
            if (clinicQuery == null) return q;

            if (clinicQuery.PatientId.HasValue)
            {
                q = q.Where(c => c.Patient.Id == clinicQuery.PatientId);
            }

            return q;
        }

    }
}
