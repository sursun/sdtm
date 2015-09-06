using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Utils;
using Gms.Common;
using Gms.Domain;
using Gms.Domain.Examine;
using Gms.Domain.FollowUp;

namespace Gms.Infrastructure.FollowUp
{
    public class FollowUpBaseRepository<T> : RepositoryBase<T>, IFollowUpBaseRepository<T> where T : Gms.Domain.FollowUp.FollowUp
    {
        protected override IQueryable<T> LoadQuery<TQ>(TQ query)
        {
            IQueryable<T> q = base.LoadQuery(query);
            var followUpQuery = query as FollowUpQuery;
            if (followUpQuery == null) return q;

            if (followUpQuery.PatientId.HasValue)
            {
                q = q.Where(c => c.Patient.Id == followUpQuery.PatientId);
            }
            
            if (followUpQuery.FollowUpType.HasValue)
            {
                q = q.Where(c => c.FollowUpType == followUpQuery.FollowUpType);
            }

            if (!followUpQuery.Name.IsNullOrEmpty())
            {
                q = q.Where(c => c.Name.Contains(followUpQuery.Name));
            }

            if (followUpQuery.DoctorId.HasValue)
            {
                q = q.Where(c => c.Doctor.Id == followUpQuery.DoctorId);
            }

            if (followUpQuery.PracticeDoctorId.HasValue)
            {
                q = q.Where(c => c.PracticeDoctor.Id == followUpQuery.PracticeDoctorId);
            }

            if (followUpQuery.CreateTime != null)
            {
                if (followUpQuery.CreateTime.Start.HasValue)
                {
                    q = q.Where(c => c.CreateTime >= followUpQuery.CreateTime.Start);
                }

                if (followUpQuery.CreateTime.End.HasValue)
                {
                    q = q.Where(c => c.CreateTime < followUpQuery.CreateTime.End);
                }
            }

            return q;
        }
    }
}
