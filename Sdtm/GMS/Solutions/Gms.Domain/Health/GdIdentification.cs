using System;
using Gms.Common;
using Gms.Domain.Attribute;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 基线资料
    /// 一般情况
    /// 妊娠糖尿病
    /// </summary>
    public class GdIdentification : IdentificationBase
    {
        public GdIdentification()
        {
            this.LastMenstrualPeriod = DateTimeEx.Default();
        }

        /// <summary>
        /// 末次月经
        /// </summary>
        [FieldNeed]
        public virtual DateTime LastMenstrualPeriod { get; set; }

        /// <summary>
        /// 孕周
        /// </summary>
        [FieldNeed]
        public virtual string PregnancyWeeks { get; set; }

    }
}
