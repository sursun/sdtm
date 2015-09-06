using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Gms.Common;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 健康教育
    /// </summary>
    public class HealthEducation:Entity
    {
        /// <summary>
        /// 患者
        /// </summary>
        ///[XmlIgnore]
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 参加过的健康教育
        /// 0x00000001 糖尿病急性并发症防治
        /// 0x00000002 饮食、运动指导
        /// 0x00000004 口服药物知识
        /// 0x00000008 胰岛素使用及注射技巧
        /// 0x00000010 自我检测与护理
        /// 0x00000020 慢性并发症筛查与防治
        /// 0x00000040 糖尿病足部自我护理
        /// 0x00000080 低血糖防治
        /// </summary>
        public virtual int EducationFlag { get; set; }
        
        #region 既往教育

        /// <summary>
        /// 营养师教育  
        /// </summary>
        public virtual YesNoUnclear YingYangShi { get; set; }

        /// <summary>
        /// 糖尿病护士教育
        /// </summary>
        public virtual YesNoUnclear HuShi { get; set; }

        /// <summary>
        /// 足病防护教育
        /// </summary>
        public virtual YesNoUnclear ZuBing { get; set; }

        #endregion
    }

    /// <summary>
    /// 教育内容
    /// </summary>
    public class Education : Entity
    {
        public Education()
        {
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 教育内容
        /// 0x00000001 糖尿病急性并发症防治
        /// 0x00000002 饮食、运动指导
        /// 0x00000004 口服药物知识
        /// 0x00000008 胰岛素使用及注射技巧
        /// 0x00000010 自我检测与护理
        /// 0x00000020 慢性并发症筛查与防治
        /// 0x00000040 糖尿病足部自我护理
        /// 0x00000080 低血糖防治
        /// </summary>
        public virtual int EducationFlag { get; set; }

        /// <summary>
        /// 教师
        /// </summary>
        public virtual String Teacher { get; set; }

        /// <summary>
        /// 教育时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        #region 既往教育

        /// <summary>
        /// 营养师教育  
        /// </summary>
        public virtual YesNoUnclear YingYangShi { get; set; }

        /// <summary>
        /// 糖尿病护士教育
        /// </summary>
        public virtual YesNoUnclear HuShi { get; set; }

        /// <summary>
        /// 足病防护教育
        /// </summary>
        public virtual YesNoUnclear ZuBing { get; set; }

        #endregion
    }

    public class EducationQuery : QueryBase
    {
        public int? PatientId { get; set; }
    }
}
