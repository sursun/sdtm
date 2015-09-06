﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain;
using SharpArch.Domain.PersistenceSupport;

namespace Gms.Infrastructure
{
    public interface IMedicineRepository : IRepositoryBase<Medicine>
    {
        Medicine GetBy(string chemicalName);
    }
}
