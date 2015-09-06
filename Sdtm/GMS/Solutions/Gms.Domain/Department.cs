﻿using System.Web.Script.Serialization;
using Gms.Common;
using Newtonsoft.Json;
using SharpArch.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gms.Domain
{
    /// <summary>
    /// 科室
    /// </summary>
    public class Department : Entity
    {
        /// <summary>
        /// 上级科室
        /// </summary>
        [XmlIgnore]
        [ScriptIgnore]
        public virtual Department Parent { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public virtual int Level { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Note { get; set; }

        public virtual String ParentString()
        {
            String strRet = "";
            Department parentItem = Parent;

            while (parentItem != null)
            {
                var tmp = parentItem.Name + ">>" ;
                
                strRet = tmp + strRet;

                parentItem = parentItem.Parent;
            }

            return strRet;
        }

        /// <summary>
        /// 下级部门
        /// </summary>
        [XmlIgnore]
        public virtual IList<Department> Subs { get; set; }

    }

    public class DepartmentQuery : QueryBase
    {
        /// <summary>
        /// 上级科室
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Note { get; set; }
    }
}
