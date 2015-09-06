using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Examine;

namespace Gms.Infrastructure.Examine
{
    public interface IExamineBaseRepository<T> : IRepositoryBase<T> where T : ExamineBase
    {
        T GetBy(int patientid);
        IList<T> GetListBy(int patientid);
    }
}
