using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Common;
using Gms.Domain;

namespace Gms.Infrastructure
{
    public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
        protected override IQueryable<Patient> LoadQuery<TQ>(TQ query)
        {
            IQueryable<Patient> q = base.LoadQuery(query);
            var patientQuery = query as PatientQuery;
            if (patientQuery == null) return q;

            if (patientQuery.PatientId.HasValue)
            {
                q = q.Where(c => c.Id == patientQuery.PatientId);
            }

            if (!patientQuery.PatientIds.IsNullOrEmpty())
            {
                int[] dd = patientQuery.PatientIds.Split('|').Where(c => c.Length > 0).Select(c => (int.Parse(c))).ToArray();
                //var dd = new int[5] { 1, 2, 3, 4, 5 };  
                q = q.Where(c => dd.Contains(c.Id));
            }

            if (!string.IsNullOrEmpty(patientQuery.RealName))
            {
                q = q.Where(c => c.RealName.Contains(patientQuery.RealName));
            }

            if (!string.IsNullOrEmpty(patientQuery.CodeNo))
            {
                q = q.Where(c => c.CodeNo.Contains(patientQuery.CodeNo));
            }

            if (patientQuery.Sex.HasValue)
            {
                q = q.Where(c => c.Sex == patientQuery.Sex);
            }

            //if (!string.IsNullOrEmpty(patientQuery.Mobile1))
            //{
            //    q = q.Where(c => c.Mobile1.Contains(patientQuery.Mobile1));
            //}

            if (!string.IsNullOrEmpty(patientQuery.IdCard))
            {
                q = q.Where(c => c.IdCard.Contains(patientQuery.IdCard));
            }

            if (patientQuery.Birthday != null)
            {
                if (patientQuery.Birthday.Start.HasValue)
                {
                    q = q.Where(c => c.Birthday >= patientQuery.Birthday.Start);
                }

                if (patientQuery.Birthday.End.HasValue)
                {
                    q = q.Where(c => c.Birthday < patientQuery.Birthday.End);
                }
            }

            if (patientQuery.AreaId.HasValue)
            {
                q = q.Where(c => c.Area.Id == patientQuery.AreaId);
            }

            if (patientQuery.DiabetesType.HasValue)
            {
                q = q.Where(c => c.DiabetesType == patientQuery.DiabetesType);
            }

            //if (patientQuery.DiagnoseDate != null)
            //{
            //    if (patientQuery.DiagnoseDate.Start.HasValue)
            //    {
            //        q = q.Where(c => c.DiagnoseDate >= patientQuery.DiagnoseDate.Start);
            //    }

            //    if (patientQuery.DiagnoseDate.End.HasValue)
            //    {
            //        q = q.Where(c => c.DiagnoseDate < patientQuery.DiagnoseDate.End);
            //    }
            //}

            if (patientQuery.DiabetesId.HasValue)
            {
                q = q.Where(c => c.Diabetes.Id == patientQuery.DiabetesId);
            }

            if (patientQuery.DiseaseStage.HasValue)
            {
                q = q.Where(c => c.DiseaseStage == patientQuery.DiseaseStage);
            }

            //if (patientQuery.IsBreathing.HasValue)
            //{
            //    q = q.Where(c => c.IsBreathing == patientQuery.IsBreathing);
            //}

            if (!string.IsNullOrEmpty(patientQuery.Note))
            {
                q = q.Where(c => c.Note.Contains(patientQuery.Note));
            }

            if (patientQuery.CreateTime != null)
            {
                if (patientQuery.CreateTime.Start.HasValue)
                {
                    q = q.Where(c => c.CreateTime >= patientQuery.CreateTime.Start);
                }

                if (patientQuery.CreateTime.End.HasValue)
                {
                    q = q.Where(c => c.CreateTime < patientQuery.CreateTime.End);
                }
            }

            if (patientQuery.DoctorId.HasValue)
            {
                q = q.Where(c => c.Doctor.Id == patientQuery.DoctorId);
            }
            
            return q;
        }

        public Patient Get(String idCard)
        {
            return Query.FirstOrDefault(c => c.IdCard.Equals(idCard));
        }

        public Patient GetByCodeNo(string codeno)
        {
            return Query.FirstOrDefault(c => c.CodeNo.Equals(codeno));
        }

        public Patient GetByName(string name)
        {
            return Query.FirstOrDefault(c => c.RealName.Equals(name));
        }
    }
}
