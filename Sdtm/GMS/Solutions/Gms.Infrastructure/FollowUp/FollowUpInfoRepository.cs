using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Utils;
using Gms.Common;
using Gms.Domain;
using Gms.Domain.FollowUp;

namespace Gms.Infrastructure.FollowUp
{
    public class FollowUpInfoRepository : RepositoryBase<FollowUpInfo>, IFollowUpInfoRepository
    {
        protected override IQueryable<FollowUpInfo> LoadQuery<TQ>(TQ query)
        {
            IQueryable<FollowUpInfo> q = base.LoadQuery(query);
            var entityQuery = query as FollowUpInfoQuery;
            if (entityQuery == null) return q;

            if (entityQuery.PatientId.HasValue)
            {
                q = q.Where(c => c.Patient.Id == entityQuery.PatientId);
            }

            if (!entityQuery.PatientIds.IsNullOrEmpty())
            {
                int[] dd = entityQuery.PatientIds.Split('|').Where(c => c.Length > 0).Select(c => (int.Parse(c))).ToArray();
                //var dd = new int[5] { 1, 2, 3, 4, 5 };  
                q = q.Where(c =>dd.Contains(c.Patient.Id));
            }

            if (!entityQuery.RealName.IsNullOrEmpty())
            {
                q = q.Where(c => c.Patient.RealName.Contains(entityQuery.RealName));
            }

            if (!entityQuery.IdCard.IsNullOrEmpty())
            {
                q = q.Where(c => c.Patient.IdCard.Contains(entityQuery.IdCard));
            }

            if (entityQuery.DiabetesId.HasValue)
            {
                q = q.Where(c => c.Patient.Diabetes.Id == entityQuery.DiabetesId);
            }
            
            if (entityQuery.FuZhenStatus.HasValue)
            {
                //q = q.Where(c => c.FuZhenStatus == entityQuery.FuZhenStatus);

                switch (entityQuery.FuZhenStatus)
                {
                    case FuZhenStatus.候诊:
                        {
                            q = q.Where(c => c.FollowUpDate >= (DateTime.Now - TimeSpan.FromDays(7)));
                            q = q.Where(c => c.FollowUpDate <= (DateTime.Now + TimeSpan.FromDays(7)));
                        }
                        break;
                    case FuZhenStatus.待诊:
                        q = q.Where(c => c.FollowUpDate > (DateTime.Now + TimeSpan.FromDays(7)));
                        break;
                    case FuZhenStatus.过期:
                        q = q.Where(c => c.FollowUpDate < (DateTime.Now - TimeSpan.FromDays(7)));
                        break;
                    default:
                        break;
                }

            }

            if (entityQuery.DoctorId.HasValue)
            {
                q = q.Where(c => c.Patient.Doctor.Id == entityQuery.DoctorId);
            }

            if (!entityQuery.DoctorCodeNo.IsNullOrEmpty())
            {
                q = q.Where(c => c.Patient.Doctor.CodeNo.Contains(entityQuery.DoctorCodeNo));
            }
           
            if (entityQuery.FollowUpDate != null)
            {
                if (entityQuery.FollowUpDate.Start.HasValue)
                {
                    q = q.Where(c => c.FollowUpDate >= entityQuery.FollowUpDate.Start);
                }

                if (entityQuery.FollowUpDate.End.HasValue)
                {
                    q = q.Where(c => c.FollowUpDate < entityQuery.FollowUpDate.End);
                }
            }

            return q;
        }

        public FollowUpInfo GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }
    }
}
