using System;
using System.Collections.Generic;
using Gms.Domain.Attribute;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 诊疗方案
    /// </summary>
    public class Treatment:Completion
    {
        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 用药方案
        /// </summary>
        [FieldNeed]
        public virtual IList<Medicate> Medicates { get; set; }

        /// <summary>
        /// 其他药物方案
        /// </summary>
        [FieldNeed]
        public virtual String Other { get; set; }

        /// <summary>
        /// 饮食运动治疗
        /// </summary>
        [FieldNeed]
        public virtual String Sport { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Note { get; set; }
    }
}
