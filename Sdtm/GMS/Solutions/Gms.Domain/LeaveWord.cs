using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain
{
    public class LeaveWord:Entity
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 留言内容
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 留言人
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public virtual int Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual Sex Sex { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public virtual String Mobile { get; set; }

        /// <summary>
        /// 留言时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 回复人
        /// </summary>
        public virtual Doctor Doctor { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public virtual String Reply { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public virtual DateTime ReplyTime { get; set; }

        /// <summary>
        /// 留言状态
        /// </summary>
        public virtual MessageStatus Status { get; set; }

    }
}
