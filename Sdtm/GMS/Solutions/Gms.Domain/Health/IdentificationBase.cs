using System;
using System.Xml.Serialization;
using Gms.Common;
using Gms.Domain.Attribute;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 基线资料
    /// 一般情况 
    /// </summary>
    public abstract class IdentificationBase : Completion
    {
        /// <summary>
        /// 患者
        /// </summary>
        ///[XmlIgnore]
        public virtual Patient Patient { get; set; }
        
        /// <summary>
        /// 民族
        /// </summary>
        [FieldNeed]
        public virtual CommonCode Nation { get; set; }

        /// <summary>
        /// 教育水平
        /// </summary>
        [FieldNeed]
        public virtual CommonCode EducationalLevel { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        [FieldNeed]
        public virtual CommonCode Job { get; set; }


        /// <summary>
        /// 血型
        /// </summary>
        [FieldNeed]
        public virtual BloodType BloodType { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [FieldNeed]
        public virtual String Address { get; set; }

        /// <summary>
        /// 是否吸烟
        /// </summary>
        [FieldNeed]
        public virtual YouWuJie Smoking { get; set; }

        /// <summary>
        /// 是否吸烟
        /// ?年
        /// </summary>
        [FieldNeed]
        public virtual string SmokingYear { get; set; }

        /// <summary>
        /// ？支/天
        /// </summary>
        [FieldNeed]
        public virtual string SmokingCount { get; set; }

        /// <summary>
        /// 是否喝酒
        /// </summary>
        [FieldNeed]
        public virtual YouWuJie Drink { get; set; }

        /// <summary>
        /// 是否喝酒
        /// 喝酒？年
        /// </summary>
        [FieldNeed]
        public virtual string DrinkYear { get; set; }

        /// <summary>
        /// 是否喝酒
        /// 酒量
        /// </summary>
        [FieldNeed]
        public virtual string DrinkCapacity { get; set; }
        
        /// <summary>
        /// 是否健在
        /// </summary>
        [FieldNeed]
        public virtual YesNo IsBreathing { get; set; }
 
    }
}
