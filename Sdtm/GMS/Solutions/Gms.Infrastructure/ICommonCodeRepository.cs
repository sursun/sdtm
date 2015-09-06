﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain;

namespace Gms.Infrastructure
{
    public interface ICommonCodeRepository : IRepositoryBase<CommonCode>
    {
        IList<CommonCode> GetRoot(CommonCodeType type);
        IList<CommonCode> GetChildren(int parentId);
        CommonCode GetGdType();
        CommonCode GetBy(CommonCodeType type);
        CommonCode GetBy(String name, CommonCodeType type);
        CommonCode GetOrCreate(String name, CommonCodeType type);

    }
}
