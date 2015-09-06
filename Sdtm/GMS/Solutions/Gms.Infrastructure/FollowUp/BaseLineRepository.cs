using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.FollowUp;

namespace Gms.Infrastructure.FollowUp
{
    public class BaseLineRepository : RepositoryBase<BaseLine>, IBaseLineRepository
    {
        public BaseLine GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }
    }
}
