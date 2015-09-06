using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.FollowUp;

namespace Gms.Infrastructure.FollowUp
{
    public interface IGdFollowUpBaseRepository<T> : IRepositoryBase<T> where T : Gms.Domain.FollowUp.GdFollowUp
    {
        
    }
}
