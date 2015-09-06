using System;
using System.Xml.Serialization;

namespace Gms.Domain.Examine
{
    public abstract class ExamineBase : Completion
    {
        protected ExamineBase()
        {
            ExamDate = DateTime.Now;
        }

        /// <summary>
        /// 患者
        /// </summary>
        [XmlIgnore]
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 检查日期
        /// </summary>
        public virtual DateTime ExamDate { get; set; }
    }
}
