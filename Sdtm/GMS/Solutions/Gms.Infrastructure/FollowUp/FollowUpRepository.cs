using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.FollowUp;

namespace Gms.Infrastructure.FollowUp
{
    public class FollowUpRepository : FollowUpBaseRepository<Gms.Domain.FollowUp.FollowUp>, IFollowUpRepository
    {

        public IList<Domain.FollowUp.FollowUp> GetListBy(int patientid)
        {
            return Query.Where(c => c.Patient.Id == patientid).ToList();
        }
       
    }
}
