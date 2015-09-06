using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Examine;

namespace Gms.Infrastructure.Examine
{
    public class ExamineBaseRepository<T> : RepositoryBase<T>, IExamineBaseRepository<T> where T : ExamineBase
    {
        public T GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }

        public IList<T> GetListBy(int patientid)
        {
            return Query.Where(c => c.Patient.Id == patientid).OrderBy(c => c.ExamDate).ToList();
        }

        public override T SaveOrUpdate(T entity)
        {
            entity.Computer();
            return base.SaveOrUpdate(entity);
        }
    }
}
