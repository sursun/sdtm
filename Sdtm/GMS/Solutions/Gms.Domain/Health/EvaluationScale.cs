using System;
using Gms.Common;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 糖尿病评估相关量表
    /// </summary>
    public class EvaluationScale:Entity
    {
        public EvaluationScale()
        {
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 量表类型
        /// </summary>
        public virtual ScaleType ScaleType { get; set; }

        /// <summary>
        /// 量表名称
        /// </summary>
        public virtual String ScaleName { get; set; }

        /// <summary>
        /// 试卷名称
        /// 试卷路径，如：test.htm
        /// </summary>
        public virtual String PaperName { get; set; }

        /// <summary>
        /// 评估得分
        /// </summary>
        public virtual int Result { get; set; }

        /// <summary>
        /// 选项得分
        /// 用|分割
        /// </summary>
        public virtual String Answers { get; set; }

        /// <summary>
        /// 评估日期
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

    }

    public class EvaluationScaleQuery : QueryBase
    {
        /// <summary>
        /// 患者
        /// </summary>
        public int? PatientId { get; set; }
    }
}
