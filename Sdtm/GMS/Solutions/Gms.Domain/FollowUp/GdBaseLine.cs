using System;
using Gms.Domain.Examine;
using Gms.Domain.Health;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.FollowUp
{
    public class GdBaseLine:Entity
    {
        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 一般情况
        /// </summary>
        public virtual GdIdentification GdIdentification { get; set; }

        /// <summary>
        /// 既往史(病史回顾)
        /// </summary>
        public virtual GdHistory GdHistory { get; set; }

        /// <summary>
        /// GDM风险评估
        /// </summary>
        public virtual GdmRisk GdmRisk { get; set; }

        #region 体格检查

        /// <summary>
        /// 体格检查
        /// </summary>
        public virtual GdPhysical GdPhysical { get; set; }

        /// <summary>
        /// 血液常规检查
        /// </summary>
        public virtual BloodRt BloodRt { get; set; }

        #endregion
        

        /// <summary>
        /// 诊疗方案
        /// </summary>
        public virtual Treatment Treatment { get; set; }

        /// <summary>
        /// 责任医生
        /// </summary>
        public virtual Doctor Doctor { get; set; }

        /// <summary>
        /// 见习医生（填表人）
        /// </summary>
        public virtual Doctor PracticeDoctor { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Note { get; set; }

        /// <summary>
        /// 填写时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }
}
