

using Gms.Domain.Attribute;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 基线资料
    /// 糖尿病家族史
    /// </summary>
    public class FamilyHistory:Completion
    {
        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 父亲
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear Father { get; set; }

        /// <summary>
        /// 母亲
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear Mother { get; set; }

        /// <summary>
        /// 兄弟姐妹总数
        /// </summary>
        [FieldNeed]
        public virtual string Sibling { get; set; }

        /// <summary>
        /// 兄弟姐妹糖尿病人数
        /// </summary>
        [FieldNeed]
        public virtual string SiblingSick { get; set; }

        /// <summary>
        /// 孩子总数
        /// </summary>
        [FieldNeed]
        public virtual string Children { get; set; }

        /// <summary>
        /// 孩子糖尿病人数
        /// </summary>
        [FieldNeed]
        public virtual string ChildrenSick { get; set; }
    }
}
