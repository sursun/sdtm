using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain;

namespace Gms.Infrastructure
{
    public interface IPatientRepository : IRepositoryBase<Patient>
    {
        Patient Get(String idCard);
        Patient GetByCodeNo(string codeno);
        Patient GetByName(string name);
    }
}
