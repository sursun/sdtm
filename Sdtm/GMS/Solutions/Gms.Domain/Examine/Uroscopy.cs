
using Gms.Domain.Attribute;

namespace Gms.Domain.Examine
{
    /// <summary>
    /// 尿液检查
    /// </summary>
    public class Uroscopy : ExamineBase
    {
        /// <summary>
        /// 酮体
        /// "未查"
        /// "(-)"
        /// "+"
        /// "++"
        /// "+++" 
        /// </summary>
        [FieldNeed]
        public virtual int Ketone { get; set; }

        /// <summary>
        /// 尿糖
        /// "未查"
        /// "(-)"
        /// "+"
        /// "++"
        /// "+++" 
        /// </summary>
        [FieldNeed]
        public virtual int UrineSugar { get; set; }

        /// <summary>
        /// 尿蛋白
        /// "未查"
        /// "(-)"
        /// "+"
        /// "++"
        /// "+++" 
        /// </summary>
        [FieldNeed]
        public virtual int UrineProtein { get; set; }

        /// <summary>
        /// 尿微量白蛋白  值
        /// </summary>
        [FieldNeed]
        public virtual string mALB { get; set; }

        /// <summary>
        /// 尿肌酐   值
        /// </summary>
        [FieldNeed]
        public virtual string UCr { get; set; }

        /// <summary>
        /// 尿微量白蛋白(/尿肌酐)   值
        /// </summary>
        [FieldNeed]
        public virtual string UCrValue { get; set; }

        /// <summary>
        /// 尿微量白蛋白(/尿肌酐)   对比
        /// <
        /// =
        /// >
        /// </summary>
        [FieldNeed]
        public virtual int UCrBy { get; set; }

        /// <summary>
        /// 尿微量白蛋白(/尿肌酐)   单位
        /// ug/min 
        /// mg/g 
        /// mg/24h
        /// mg/mmol
        /// </summary>
        [FieldNeed]
        public virtual int UCrUnit { get; set; }

        /// <summary>
        /// 24小时尿蛋白定量（g/24h ）
        /// </summary>
        [FieldNeed]
        public virtual string UrineProtein24H { get; set; }

        /// <summary>
        /// 尿碘（ug/L）
        /// </summary>
        [FieldNeed]
        public virtual string UrineIdoine { get; set; }

    }
}
