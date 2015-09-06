using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Common;
using Gms.Domain.Examine;
using Gms.Domain.Health;

namespace Gms.Domain.FollowUp
{
    /// <summary>
    /// 随访年度评估
    /// </summary>
    public class Annual:FollowUp
    {
        public Annual()
        {
            FollowUpType = FollowUpType.年度评估;
        }

        #region 体格检查

        /// <summary>
        /// 体格检查
        /// </summary>
        public virtual Physical Physical { get; set; }

        /// <summary>
        /// 尿液检查
        /// </summary>
        public virtual Uroscopy Uroscopy { get; set; }

        /// <summary>
        /// 血液检查
        /// </summary>
        public virtual Blood Blood { get; set; }

        /// <summary>
        /// 血液其他检查
        /// </summary>
        public virtual BloodRt BloodRt { get; set; }

        #endregion

        #region 并发症筛查

        /// <summary>
        /// 眼睛检查
        /// </summary>
        public virtual Eye Eye { get; set; }

       /// <summary>
        /// 下肢血管及神经病变筛查
       /// </summary>
        public virtual Legs Legs { get; set; }

        /// <summary>
        /// 其他筛查
        /// </summary>
        public virtual UnClassified UnClassified { get; set; }

        #endregion

        /// <summary>
        /// 医疗诊断
        /// </summary>
        public virtual Diagnoses Diagnoses { get; set; }

        /// <summary>
        /// 诊疗方案
        /// </summary>
        public virtual Treatment Treatment { get; set; }

        /// <summary>
        /// EQ-5D问卷（生活质量评估）
        /// </summary>
        public virtual String EQ5D { get; set; }
    }
    
}
