using System;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 用药方案
    /// </summary>
    public class Medicate:Entity
    {
        public Medicate()
        {
            this.StartDateTime = DateTime.Now;
        }

        /// <summary>
        /// 药物
        /// </summary>
        public virtual Medicine Medicine { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public virtual String Dosage { get; set; }

        /// <summary>
        /// 用法
        /// </summary>
        public virtual Usage Usage { get; set; }

        /// <summary>
        /// 给药途径
        /// </summary>
        public virtual DoseType DoseType { get; set; }

        /// <summary>
        /// 开始用药时间
        /// </summary>
        public virtual DateTime StartDateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Note { get; set; }
    }
}
