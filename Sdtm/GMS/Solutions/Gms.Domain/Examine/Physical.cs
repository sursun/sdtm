
using System;
using Gms.Domain.Attribute;

namespace Gms.Domain.Examine
{
    /// <summary>
    /// 体格检查
    /// </summary>
    public class Physical : ExamineBase
    {
        /// <summary>
        /// 身高(cm)  
        /// </summary>
        [FieldNeed]
        public virtual string Height { get; set; }
        
        /// <summary>
        /// BMI(kg/m2) 
        /// </summary>
        [FieldNeed]
        public virtual String BMI { get; set; }

        /// <summary>
        /// 腰围(cm)  
        /// </summary>
        [FieldNeed]
        public virtual string Waistline { get; set; }

        /// <summary>
        /// 臀围(cm)  
        /// </summary>
        [FieldNeed]
        public virtual string Hipline { get; set; }
        
        /// <summary>
        /// 腰臀比
        /// </summary>
        [FieldNeed]
        public virtual string WaistHipline { get; set; }

    }
}
