using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Attribute;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Examine
{
    /// <summary>
    /// 并发症筛查
    /// 眼睛检查
    /// </summary>
    public class Eye : ExamineBase
    {

        /// <summary>
        /// 眼底检查
        /// 眼底镜
        /// 眼底摄片
        /// 荧光造影
        /// </summary>
        [FieldNeed]
        public virtual int Fundus { get; set; }

        /// <summary>
        /// 视力
        /// </summary>
        [FieldNeed]
        public virtual string VisionL { get; set; }

        /// <summary>
        /// 视力
        /// </summary>
        [FieldNeed]
        public virtual string VisionR { get; set; }

        /// <summary>
        /// 白内障
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck CataractL { get; set; }

        /// <summary>
        /// 白内障
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck CataractR { get; set; }

        /// <summary>
        /// 青光眼
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck GlaucomaL { get; set; }

        /// <summary>
        /// 青光眼
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck GlaucomaR { get; set; }

        /// <summary>
        /// 黄斑病
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck MaculopathyL { get; set; }

        /// <summary>
        /// 黄斑病
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck MaculopathyR { get; set; }

        /// <summary>
        /// 视网膜病变
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck RetinopathyL { get; set; }
        
        /// <summary>
        /// 视网膜病变
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck RetinopathyR { get; set; }

        /// <summary>
        /// 激光治疗
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck LaserL { get; set; }

        /// <summary>
        /// 激光治疗
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck LaserR { get; set; }

        /// <summary>
        /// 其他
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck OtherL { get; set; }
        
        /// <summary>
        /// 其他眼疾
        /// 是
        /// 否
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck OtherR { get; set; }
    }
}
