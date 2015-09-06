using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Common;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain
{
    /// <summary>
    /// 疾病（病种）
    /// </summary>
    public class Disease:Entity
    {
        /// <summary>
        /// 疾病类型
        /// </summary>
        public virtual CommonCode Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 国际码
        /// </summary>
        public virtual String CodeNo { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        public virtual String PinYin { get; set; }

    }

    public class DiseaseQuery : QueryBase
    {
        /// <summary>
        /// 疾病类型
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 国际码
        /// </summary>
        public String CodeNo { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        public String PinYin { get; set; }
    }
}
