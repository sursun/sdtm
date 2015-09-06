using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Utils;
using Gms.Common;
using Gms.Domain;
using NHibernate.Criterion;
using NHibernate.Util;

namespace Gms.Infrastructure
{
    public class DoctorRepository : RepositoryBase<Doctor>, IDoctorRepository
    {
        protected override IQueryable<Doctor> LoadQuery<TQ>(TQ query)
        {
            IQueryable<Doctor> q = base.LoadQuery(query);
            var doctorQuery = query as DoctorQuery;
            if (doctorQuery == null) return q;

            if (!string.IsNullOrEmpty(doctorQuery.LoginName))
            {
                q = q.Where(c => c.LoginName.Contains(doctorQuery.LoginName));
            }

            if (!string.IsNullOrEmpty(doctorQuery.RealName))
            {
                q = q.Where(c => c.RealName.Contains(doctorQuery.RealName));
            }

            if (doctorQuery.Sex.HasValue)
            {
                q = q.Where(c => c.Sex.Equals(doctorQuery.Sex));
            }
            
            if (doctorQuery.DepartmentId.HasValue)
            {
                q = q.Where(c => c.Department.Id == doctorQuery.DepartmentId);
            }

            if (!string.IsNullOrEmpty(doctorQuery.CodeNo))
            {
                q = q.Where(c => c.CodeNo.Contains(doctorQuery.CodeNo));
            }

            if (!string.IsNullOrEmpty(doctorQuery.ProfessionalLevel))
            {
                q = q.Where(c => c.ProfessionalLevel.Contains(doctorQuery.ProfessionalLevel));
            }

            if (!string.IsNullOrEmpty(doctorQuery.Duty))
            {
                q = q.Where(c => c.Duty.Contains(doctorQuery.Duty));
            }

            if (!string.IsNullOrEmpty(doctorQuery.Mobile))
            {
                q = q.Where(c => c.Mobile.Contains(doctorQuery.Mobile));
            }

            if (doctorQuery.Enabled.HasValue)
            {
                q = q.Where(c => c.Enabled.Equals(doctorQuery.Enabled));
            }
            
            if (!string.IsNullOrEmpty(doctorQuery.Note))
            {
                q = q.Where(c => c.Note.Contains(doctorQuery.Note));
            }

            return q;
        }

        public Doctor Get(Guid membershipId)
        {
            return Query.FirstOrDefault(c => c.MemberShipId == membershipId);
        }
        
        public Doctor Get(string loginName)
        {
            return Query.FirstOrDefault(c => c.LoginName == loginName);
        }

        public IList<Doctor> GetSope(int id)
        {
            var doctor = Get(id);

            if (doctor == null || doctor.Scope.IsNullOrEmpty())
            {
                return new List<Doctor>();
            }

           // var q = Query;

            var list = GetAll();
           
            int[] dd = doctor.Scope.Split('|').Where(c => c.Length > 0).Select(c => (int.Parse(c))).ToArray();


            IList<Doctor> data = dd.Select(i => list.FirstOrDefault(c => c.Id == i)).ToList();
            //for (int i=0;i < dd.Length;i++)
            //{
            //    var n = dd[i];
            //    q.In() = EnumerableExtensions.Any(q.Where(c => c.Id == n));
            //}

           // q = (from item in q where item.Id.IsIn(dd) select item);

            return data;
        }

    }
}
