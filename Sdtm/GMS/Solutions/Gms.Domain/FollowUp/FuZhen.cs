using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Examine;
using Gms.Domain.Health;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.FollowUp
{
    /// <summary>
    /// 维持复诊
    /// </summary>
    public class FuZhen:FollowUp
    {
        public FuZhen()
        {
            FollowUpType = FollowUpType.复诊;;
        }

        /// <summary>
        /// 血液常规检查
        /// </summary>
        public virtual BloodRt BloodRt { get; set; }

        /// <summary>
        /// 诊疗方案
        /// </summary>
        public virtual Treatment Treatment { get; set; }
    }

}
