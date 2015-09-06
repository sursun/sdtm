using Gms.Domain.Attribute;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 基线资料
    /// 一般情况
    /// 其他糖尿病
    /// </summary>
    public class Identification : IdentificationBase
    {
        /// <summary>
        /// 既往最高体重
        /// </summary>
        [FieldNeed]
        public virtual string HighestWeight { get; set; }

        /// <summary>
        /// 付费方式
        /// </summary>
        [FieldNeed]
        public virtual PayType PayType { get; set; }
 
    }
}
