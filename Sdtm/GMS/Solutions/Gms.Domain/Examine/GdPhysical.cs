
using Gms.Domain.Attribute;

namespace Gms.Domain.Examine
{
    /// <summary>
    /// 妊娠糖尿病体格检查
    /// </summary>
    public class GdPhysical : ExamineBase
    {

        #region  体格检查

        /// <summary>
        /// 体重(Kg) 
        /// </summary>
        [FieldNeed]
        public virtual string Weight { get; set; }

        /// <summary>  
        /// 血压（mmHg）
        /// 高压值
        /// </summary>
        [FieldNeed]
        public virtual string BloodPressureHigh { get; set; }

        /// <summary>  
        /// 血压（mmHg）
        /// 低压值
        /// </summary>
        [FieldNeed]
        public virtual string BloodPressureLow { get; set; }

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
        /// 尿碘（ug/L）
        /// </summary>
        [FieldNeed]
        public virtual string UrineIdoine { get; set; }

        /// <summary>
        /// 尿碘 孕周
        /// </summary>
        [FieldNeed]
        public virtual string UrineIdoineWeek { get; set; }

        #region 血糖

        /// <summary>
        /// 随机血糖（mmol/L）
        /// </summary>
        [FieldNeed]
        public virtual string Rbg { get; set; }

        /// <summary>
        /// 餐后两小时血糖（mmol/L）
        /// </summary>
        [FieldNeed]
        public virtual string Pbg { get; set; }

        /// <summary>
        /// 50g葡萄糖耐量实验
        /// 空腹血糖（mmol/L）
        /// </summary>
        [FieldNeed]
        public virtual string Fbg50 { get; set; }

        /// <summary>
        /// 50g葡萄糖耐量实验
        /// 1小时血糖（mmol/L）
        /// </summary>
        public virtual string Pbg1H50 { get; set; }

        /// <summary>
        /// 75g葡萄糖耐量实验
        /// 空腹血糖（mmol/L）
        /// </summary>
        [FieldNeed]
        public virtual string Fbg75 { get; set; }

        /// <summary>
        /// 75g葡萄糖耐量实验
        /// 1小时血糖（mmol/L）
        /// </summary>
        [FieldNeed]
        public virtual string Pbg1H75 { get; set; }

        /// <summary>
        /// 75g葡萄糖耐量实验
        /// 2小时血糖（mmol/L）
        /// </summary>
        [FieldNeed]
        public virtual string Pbg2H75 { get; set; }

        /// <summary>
        /// 75g葡萄糖耐量实验
        /// 0分钟胰岛素（Iu/ml）
        /// </summary>
        [FieldNeed]
        public virtual string Ins { get; set; }

        /// <summary>
        /// 75g葡萄糖耐量实验
        /// 60分钟胰岛素（Iu/ml）
        /// </summary>
        [FieldNeed]
        public virtual string Ins60 { get; set; }

        /// <summary>
        /// 75g葡萄糖耐量实验
        /// 120分钟胰岛素（Iu/ml）
        /// </summary>
        [FieldNeed]
        public virtual string Ins120 { get; set; }

        #endregion

        /// <summary>
        /// 糖化血红蛋白(HbA1c% )
        /// </summary>
        [FieldNeed]
        public virtual string HbA1c { get; set; }

        /// <summary>
        /// 糖化血红蛋白 孕周
        /// </summary>
        [FieldNeed]
        public virtual string HbA1cWeek { get; set; }
        
        /// <summary>
        /// 甲状腺功能
        /// TT3(ng/mL)
        /// </summary>
        [FieldNeed]
        public virtual string TT3 { get; set; }

        /// <summary>
        /// 甲状腺功能
        /// TT4(nmol/L)
        /// </summary>
        [FieldNeed]
        public virtual string TT4 { get; set; }

        /// <summary>
        /// 甲状腺功能
        /// FT3(pmol/L)
        /// </summary>
        [FieldNeed]
        public virtual string FT3 { get; set; }

        /// <summary>
        /// 甲状腺功能
        /// FT4(pmol/L)
        /// </summary>
        [FieldNeed]
        public virtual string FT4 { get; set; }

        /// <summary>
        /// 甲状腺功能
        /// TSH(uIu/mL)
        /// </summary>
        [FieldNeed]
        public virtual string TSH { get; set; }

        #endregion

    }
}
