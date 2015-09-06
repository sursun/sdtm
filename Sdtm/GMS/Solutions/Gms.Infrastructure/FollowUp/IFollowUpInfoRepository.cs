using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.FollowUp;

namespace Gms.Infrastructure.FollowUp
{
    public interface IFollowUpInfoRepository : IRepositoryBase<FollowUpInfo>
    {
        FollowUpInfo GetBy(int patientid);
    }
}
