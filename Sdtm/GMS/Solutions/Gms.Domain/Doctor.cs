using System;
using Gms.Common;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain
{
    /// <summary>
    /// 医生
    /// </summary>
    public class Doctor:Entity
    {
        public Doctor()
        {
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 所属科室
        /// </summary>
        public virtual Department Department { get; set; }

        /// <summary>
        /// 职工编号
        /// </summary>
        public virtual String CodeNo { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public virtual String ProfessionalLevel { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public virtual String Duty { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public virtual String LoginName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid MemberShipId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual String RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual Sex Sex { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public virtual String Mobile { get; set; }

        /// <summary>
        /// 范围类型
        /// </summary>
        public virtual ScopeType ScopeType { get; set; }

        /// <summary>
        /// 自定义范围
        /// </summary>
        public virtual String Scope { get; set; }

        /// <summary>
        /// 启用|禁用
        /// </summary>
        public virtual Enabled Enabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Note { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        
    }

    public class DoctorQuery : QueryBase
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public String LoginName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public String RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex? Sex { get; set; }

        /// <summary>
        /// 所属科室
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// 职工编号
        /// </summary>
        public String CodeNo { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public String ProfessionalLevel { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public String Duty { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public String Mobile { get; set; }

        /// <summary>
        /// 启用|禁用
        /// </summary>
        public virtual Enabled? Enabled { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public String Note { get; set; }
    }
}
