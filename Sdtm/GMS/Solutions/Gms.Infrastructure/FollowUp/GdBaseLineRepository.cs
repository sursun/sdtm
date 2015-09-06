using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.FollowUp;

namespace Gms.Infrastructure.FollowUp
{
    public class GdBaseLineRepository : RepositoryBase<GdBaseLine>, IGdBaseLineRepository
    {
        public GdBaseLine GetBy(int patientid)
        {
            return Query.FirstOrDefault(c => c.Patient.Id == patientid);
        }
    }
}
