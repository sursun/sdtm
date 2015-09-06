using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Attribute;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Examine
{
    /// <summary>
    /// 糖尿病基本检查
    /// </summary>
    public class TnbBasic:ExamineBase
    {
        /// <summary>  
        /// 血压（mmHg）
        /// 高压值/收缩压
        /// </summary>
        [FieldNeed]
        public virtual string BloodPressureHigh { get; set; }

        /// <summary>  
        /// 血压（mmHg）
        /// 低压值/舒张压
        /// </summary>
        [FieldNeed]
        public virtual string BloodPressureLow { get; set; }
        
        /// <summary>
        /// 空腹血糖(mmol/L )
        /// 最近一周 指血糖
        /// </summary>
        [FieldNeed]
        public virtual string FBG { get; set; }

        /// <summary>
        /// 餐后血糖(mmol/L )
        /// </summary>
        [FieldNeed]
        public virtual string PBG { get; set; }

        /// <summary>
        /// 糖化血红蛋白(% )
        /// </summary>
        [FieldNeed]
        public virtual string HbA1c { get; set; }

        /// <summary>
        /// 体重(Kg) 
        /// </summary>
        [FieldNeed]
        public virtual string Weight { get; set; }
    }
}
