using Gms.Common;
using Gms.Domain.Examine;
using Gms.Domain.Health;
using SharpArch.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gms.Domain.FollowUp
{
    /// <summary>
    /// 基线资料
    /// </summary>
    public class BaseLine : Entity
    {
        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 一般情况
        /// </summary>
        public virtual Identification Identification { get; set; }

        /// <summary>
        /// 既往史
        /// </summary>
        public virtual MedicalHistory MedicalHistory { get; set; }

        /// <summary>
        /// 糖尿病家族史
        /// </summary>
        public virtual FamilyHistory FamilyHistory { get; set; }

        #region  体格检查

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
        /// 血液常规检查
        /// </summary>
        public virtual BloodRt BloodRt { get; set; }

        /// <summary>
        /// 糖尿病基本检查
        /// </summary>
        public virtual TnbBasic TnbBasic { get; set; }

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
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }
  
}
