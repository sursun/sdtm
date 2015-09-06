using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Common;
using Gms.Domain.Attribute;
using Gms.Domain.Examine;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.FollowUp
{
    public class FollowUp : Entity
    {
        public FollowUp()
        {
            this.CreateTime = DateTime.Now;
            this.ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 随访类型
        /// </summary>
        public virtual FollowUpType FollowUpType { get; set; }

        /// <summary>
        /// 随访名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 糖尿病基本检查
        /// </summary>
        public virtual TnbBasic TnbBasic { get; set; }

        /// <summary>
        /// 病程阶段
        /// </summary>
        public virtual DiseaseStage DiseaseStage { get; set; }

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
        /// 随访时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime ModifyTime { get; set; }
    }

    public class FollowUpQuery : QueryBase
    {
        /// <summary>
        /// 患者 ID
        /// </summary>
        public int? PatientId { get; set; }
        
        /// <summary>
        /// 随访类型
        /// </summary>
        public FollowUpType? FollowUpType { get; set; }

        /// <summary>
        /// 随访名称
        /// </summary>
        public String Name { get; set; }
        
        /// <summary>
        /// 责任医生
        /// </summary>
        public int? DoctorId { get; set; }

        /// <summary>
        /// 见习医生（填表人）
        /// </summary>
        public int? PracticeDoctorId { get; set; }
  
        /// <summary>
        /// 随访时间
        /// </summary>
        public Range<DateTime?> CreateTime { get; set; }
    }
}
