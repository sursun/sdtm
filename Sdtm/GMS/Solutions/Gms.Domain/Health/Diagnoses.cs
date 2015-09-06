using System.Collections.Generic;
using Gms.Domain.Attribute;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 医疗诊断
    /// </summary>
    public class Diagnoses:Completion
    {
        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 疾病
        /// </summary>
        [FieldNeed]
        public virtual IList<Disease> Diseases { get; set; }

        /// <summary>
        /// 糖尿病类型
        /// </summary>
        public virtual CommonCode Diabetes { get; set; }

        /// <summary>
        /// 病程阶段
        /// </summary>
        public virtual DiseaseStage DiseaseStage { get; set; }
    }
}
