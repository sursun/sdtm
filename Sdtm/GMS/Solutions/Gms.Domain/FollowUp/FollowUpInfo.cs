using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Gms.Common;
using Gms.Domain.Attribute;
using Newtonsoft.Json;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.FollowUp
{
    /// <summary>
    /// 随访设置
    /// </summary>
    public class FollowUpInfo:Entity
    {
        public FollowUpInfo()
        {
            this.FollowUpDate = DateTime.Now;
            this.ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 随访状态
        /// </summary>
        public virtual FollowUpStatus FollowUpStatus { get; set; }

        /// <summary>
        /// 随访方式
        /// </summary>
        public virtual FollowUpWay FollowUpWay { get; set; }

        /// <summary>
        /// 随访时间
        /// </summary>
        public virtual DateTime FollowUpDate { get; set; }

        /// <summary>
        /// 随访备注
        /// </summary>
        public virtual String Note { get; set; }

        /// <summary>
        /// 复诊状态
        /// </summary>
        [NotMap]
        public virtual FuZhenStatus FuZhenStatus
        {
            get
            {
                if(FollowUpDate < (DateTime.Now - TimeSpan.FromDays(7)))
                    return FuZhenStatus.过期;

                if (FollowUpDate > (DateTime.Now + TimeSpan.FromDays(7)))
                    return FuZhenStatus.待诊;

                return FuZhenStatus.候诊;
            }
        }

        /// <summary>
        /// 最后一次修改人
        /// </summary>
        public virtual Doctor Doctor { get; set; }

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public virtual DateTime ModifyTime { get; set; }
    }

    public class FollowUpInfoQuery : QueryBase
    {
        /// <summary>
        /// 患者 ID
        /// </summary>
        public int? PatientId { get; set; }

        /// <summary>
        /// 患者 IDs
        /// </summary>
        public String PatientIds { get; set; }

        /// <summary>
        /// 患者 真实姓名
        /// </summary>
        public String RealName { get; set; }

        /// <summary>
        /// 患者 身份证号
        /// </summary>
        public String IdCard { get; set; }

        /// <summary>
        /// 糖尿病类型|诊断分型
        /// </summary>
        public int? DiabetesId { get; set; }

        /// <summary>
        /// 复诊类型
        /// 
        ///      过期  |                候诊                |      待诊
        /// -----------|-----------------||-----------------|----------------
        ///            |7天前          |今天|          7天后|                   
        /// 
        /// </summary>
        public FuZhenStatus? FuZhenStatus { get; set; }

        /// <summary>
        /// 主治医生ID
        /// </summary>
        public int? DoctorId { get; set; }

        /// <summary>
        /// 主治医生编号
        /// </summary>
        public String DoctorCodeNo { get; set; }

        /// <summary>
        /// 随访时间
        /// </summary>
        public Range<DateTime?> FollowUpDate { get; set; }
    }
}
