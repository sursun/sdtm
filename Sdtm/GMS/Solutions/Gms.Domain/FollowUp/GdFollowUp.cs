using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Common;
using Gms.Domain.Examine;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.FollowUp
{
    /// <summary>
    /// 妊娠糖尿病随访
    /// </summary>
    public class GdFollowUp : Completion
    {
        public GdFollowUp()
        {
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 随访名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 孕周
        /// </summary>
        public virtual string PregnancyWeeks { get; set; }

        /// <summary>
        /// 体重(Kg) 
        /// </summary>
        public virtual string Weight { get; set; }

        /// <summary>  
        /// 血压（mmHg）
        /// 高压值
        /// </summary>
        public virtual string BloodPressureHigh { get; set; }

        /// <summary>  
        /// 血压（mmHg）
        /// 低压值
        /// </summary>
        public virtual string BloodPressureLow { get; set; }
        
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
        /// 填表（随访）时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }

    public class GdFollowUpQuery : QueryBase
    {
        /// <summary>
        /// 患者
        /// </summary>
        public int? PatientId { get; set; }

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
