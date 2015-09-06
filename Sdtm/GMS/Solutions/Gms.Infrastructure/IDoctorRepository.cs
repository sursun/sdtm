using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain;
using SharpArch.Domain.PersistenceSupport;

namespace Gms.Infrastructure
{
    public interface IDoctorRepository : IRepositoryBase<Doctor>
    {
        Doctor Get(Guid membershipId);
        Doctor Get(string loginName);
        //string GetLoginName(string userName);
        IList<Doctor> GetSope(int id);
    }
}
