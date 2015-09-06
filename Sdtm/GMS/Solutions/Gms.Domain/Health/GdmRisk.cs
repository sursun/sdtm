using Gms.Domain.Attribute;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Health
{
    /// <summary>
    /// GDM风险评估
    /// </summary>
    public class GdmRisk : Completion
    {
        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 高危因素
        /// 0x00000001 一级亲属患2型糖尿病	
        /// 0x00000002 GDM史 
        /// 0x00000004 糖耐量异常史	
        /// 0x00000008 超重或肥胖（尤其是重度肥胖）
        /// 0x00000010 多囊卵巢综合征	
        /// 0x00000020 年龄≥35岁 
        /// 0x00000040 不明原因的死胎，死产，流产史	
        /// 0x00000080 巨大儿分娩史 
        /// 0x00000100 大于胎龄儿分娩史	
        /// 0x00000200 胎儿畸形和羊水过多史 
        /// 0x00000400 妊娠期发现胎儿大于孕周	
        /// 0x00000800 反复外阴阴道假丝酵母菌病
        /// </summary>
        [FieldNeed]
        public virtual int HighRiskFlag { get; set; }

        /// <summary>
        /// 孕前体重
        /// </summary>
        [FieldNeed]
        public virtual string WeightBefore { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        [FieldNeed]
        public virtual string Height { get; set; }

        /// <summary>
        /// BMI (kg/m2)
        /// </summary>
        public virtual string Bmi { get; set; }
    }
}
