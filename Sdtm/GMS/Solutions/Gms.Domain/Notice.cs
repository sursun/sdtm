using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain
{
    public class Notice:Entity
    {
        /// <summary>
        /// 栏目类型
        /// </summary>
        public virtual CommonCode ColumnType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 发布内容
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 发布者
        /// </summary>
        public virtual Doctor Doctor { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }
}
