using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Common;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain
{
    /// <summary>
    /// 药品
    /// </summary>
    public class Medicine:Entity
    {
        public Medicine()
        {
            RecommendTime = DateTime.Now;
        }

        /// <summary>
        /// 药物类型
        /// </summary>
        public virtual CommonCode MedicineType { get; set; }

        /// <summary>
        /// 常用名称
        /// </summary>
        public virtual String NormalName { get; set; }

        /// <summary>
        /// 化学名称
        /// </summary>
        public virtual String ChemicalName { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        public virtual String PinYin { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public virtual String Model { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public virtual YesNo Recommend { get; set; }

        /// <summary>
        /// 推荐时间
        /// </summary>
        public virtual DateTime RecommendTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Note { get; set; }

        /// <summary>
        /// 禁用|启用
        /// </summary>
        public virtual Enabled Enabled { get; set; }
    }

    public class MedicineQuery : QueryBase
    {
        /// <summary>
        /// 药物类型
        /// </summary>
        public int? MedicineTypeId { get; set; }

        /// <summary>
        /// 常用名称
        /// </summary>
        public String NormalName { get; set; }

        /// <summary>
        /// 化学名称
        /// </summary>
        public String ChemicalName { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        public String PinYin { get; set; }
        
        /// <summary>
        /// 是否推荐
        /// </summary>
        public YesNo? Recommend { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 禁用|启用
        /// </summary>
        public Enabled? Enabled { get; set; }
    }

}
