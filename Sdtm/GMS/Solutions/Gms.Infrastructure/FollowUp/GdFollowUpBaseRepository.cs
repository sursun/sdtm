using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Examine;
using Gms.Domain.FollowUp;

namespace Gms.Infrastructure.FollowUp
{
    public class GdFollowUpBaseRepository<T> : RepositoryBase<T>, IGdFollowUpBaseRepository<T> where T : Gms.Domain.FollowUp.GdFollowUp
    {
        protected override IQueryable<T> LoadQuery<TQ>(TQ query)
        {
            IQueryable<T> q = base.LoadQuery(query);
            var entityQuery = query as GdFollowUpQuery;
            if (entityQuery == null) return q;

            if (entityQuery.PatientId.HasValue)
            {
                q = q.Where(c => c.Patient.Id == entityQuery.PatientId);
            }

            if (entityQuery.DoctorId.HasValue)
            {
                q = q.Where(c => c.Doctor.Id == entityQuery.DoctorId);
            }

            if (entityQuery.PracticeDoctorId.HasValue)
            {
                q = q.Where(c => c.PracticeDoctor.Id == entityQuery.PracticeDoctorId);
            }

            if (entityQuery.CreateTime != null)
            {
                if (entityQuery.CreateTime.Start.HasValue)
                {
                    q = q.Where(c => c.CreateTime >= entityQuery.CreateTime.Start);
                }

                if (entityQuery.CreateTime.End.HasValue)
                {
                    q = q.Where(c => c.CreateTime < entityQuery.CreateTime.End);
                }
            }

            return q;
        }
    }
}
