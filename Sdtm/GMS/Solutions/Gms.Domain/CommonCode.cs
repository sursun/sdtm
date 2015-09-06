﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Gms.Common;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain
{
    public class CommonCode:Entity
    {
        /// <summary>
        /// 父级
        /// </summary>
        [XmlIgnore]
        [ScriptIgnore]
        public virtual CommonCode Parent { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual CommonCodeType Type { get; set; }

        /// <summary>
        /// 扩展
        /// </summary>
        public virtual String Param { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Note { get; set; }

        /// <summary>
        /// 下属编码
        /// </summary>
        [XmlIgnore]
        public virtual IList<CommonCode> Subs { get; set; }

        public virtual String FullNameString()
        {
            String strRet = Name;
            CommonCode parentItem = Parent;

            while (parentItem != null)
            {
                var tmp = parentItem.Name + ">>";

                strRet = tmp + strRet;

                parentItem = parentItem.Parent;
            }

            return strRet;
        }
        
    }

    public class CommonCodeQuery : QueryBase
    {
        public int? ParentId { get; set; }

        public String Name { get; set; }

        public CommonCodeType? Type { get; set; }
    }
}
