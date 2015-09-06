using System;
using Gms.Common;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Health
{
    /// <summary>
    /// 临床事件
    /// </summary>
    public class Clinic:Entity
    {
        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public virtual String Name { get; set; }

        #region 是否健在
        /// <summary>
        /// 是否健在
        /// </summary>
        public virtual YesNo IsBreathing { get; set; }

        /// <summary>
        /// 死亡时间
        /// </summary>
        public virtual DateTime DeathTime { get; set; }

        /// <summary>
        /// 死亡原因
        /// </summary>
        public virtual String DeathReason { get; set; }

        #endregion

        /// <summary>
        /// 随访状态（该个体目前状态）
        /// </summary>
        public virtual FollowUpStatus FollowUpStatus { get; set; }

        #region 距上次随访至今

        /// <summary>
        /// 轻度 低血糖的次数
        /// </summary>
        public virtual string QingDxtTimes { get; set; }

        /// <summary>
        /// 严重 低血糖的次数
        /// </summary>
        public virtual string ZhongDxtTimes { get; set; }


        #endregion

        #region 是否发生以下急性并发症

        /// <summary>
        /// DKA
        /// </summary>
        public virtual bool Dka { get; set; }

        /// <summary>
        /// DKA
        /// </summary>
        public virtual DateTime DkaDateTime { get; set; }

        /// <summary>
        /// HNKC
        /// </summary>
        public virtual bool Hnkc { get; set; }

        /// <summary>
        /// HNKC
        /// </summary>
        public virtual DateTime HnkcDateTime { get; set; }
        
        /// <summary>
        /// 乳酸酸中毒
        /// </summary>
        public virtual bool RuShuanZd { get; set; }

        /// <summary>
        /// 乳酸酸中毒
        /// </summary>
        public virtual DateTime RuShuanZdDateTime { get; set; }
        
        #endregion

        #region 自上次随访后，是否患有以下事件/新诊断的疾病

        #region 心血管系统
        /// <summary>
        /// 心绞痛
        /// </summary>
        public virtual bool XinJiaoTong { get; set; }

        /// <summary>
        /// 心绞痛 时间
        /// </summary>
        public virtual DateTime XinJiaoTongDateTime { get; set; }

        /// <summary>
        /// 心肌梗塞
        /// </summary>
        public virtual bool XinJiGs { get; set; }

        /// <summary>
        /// 心肌梗塞 时间
        /// </summary>
        public virtual DateTime XinJiGsDateTime { get; set; }

        /// <summary>
        /// 心衰
        /// </summary>
        public virtual bool XinShuai { get; set; }

        /// <summary>
        /// 心衰 时间
        /// </summary>
        public virtual DateTime XinShuaiDateTime { get; set; }

        /// <summary>
        /// CABG
        /// </summary>
        public virtual bool Cabg { get; set; }

        /// <summary>
        /// CABG 时间
        /// </summary>
        public virtual DateTime CabgDateTime { get; set; }

        /// <summary>
        /// 血管再通 
        /// </summary>
        public virtual bool XueGuanZt { get; set; }

        /// <summary>
        /// 血管再通 时间 
        /// </summary>
        public virtual DateTime XueGuanZtDateTime { get; set; }

        /// <summary>
        /// TIA 
        /// </summary>
        public virtual bool Tia { get; set; }

        /// <summary>
        /// TIA 时间 
        /// </summary>
        public virtual DateTime TiaDateTime { get; set; }

        #endregion

        #region 中枢神经系统

        /// <summary>
        /// 脑出血
        /// </summary>
        public virtual bool NaoChuXue { get; set; }

        /// <summary>
        /// 脑出血 时间
        /// </summary>
        public virtual DateTime NaoChuXueDateTime { get; set; }
        
        /// <summary>
        /// 脑梗塞
        /// </summary>
        public virtual bool NaoGengSe { get; set; }

        /// <summary>
        /// 脑梗塞 时间
        /// </summary>
        public virtual DateTime NaoGengSeDateTime { get; set; }

        #endregion

        /// <summary>
        /// 肿瘤
        /// </summary>
        public virtual bool ZhongLiu { get; set; }

        /// <summary>
        /// 肿瘤 时间
        /// </summary>
        public virtual DateTime ZhongLiuDateTime { get; set; }

        /// <summary>
        /// 肿瘤部位
        /// </summary>
        public virtual String ZhongLiuBuWei { get; set; }

        #region 糖尿病慢性并发症

        /// <summary>
        /// 透析
        /// </summary>
        public virtual bool TouXi { get; set; }

        /// <summary>
        /// 透析 时间
        /// </summary>
        public virtual DateTime TouXiDateTime { get; set; }

        /// <summary>
        /// 移植
        /// </summary>
        public virtual bool YiZhi { get; set; }

        /// <summary>
        /// 移植 时间
        /// </summary>
        public virtual DateTime YiZhiDateTime { get; set; }

        /// <summary>
        /// 糖尿病足
        /// </summary>
        public virtual bool TnbZu { get; set; }

        /// <summary>
        /// 糖尿病足 时间
        /// </summary>
        public virtual DateTime TnbZuDateTime { get; set; }

        /// <summary>
        /// 糖尿病足(破口)
        /// </summary>
        public virtual bool TnbZuPk { get; set; }

        /// <summary>
        /// 糖尿病足(破口) 时间
        /// </summary>
        public virtual DateTime TnbZuPkDateTime { get; set; }

        /// <summary>
        /// 糖尿病足(溃疡)
        /// </summary>
        public virtual bool TnbZuKy { get; set; }

        /// <summary>
        /// 糖尿病足(溃疡) 时间
        /// </summary>
        public virtual DateTime TnbZuKyDateTime { get; set; }

        /// <summary>
        /// 糖尿病肾病
        /// </summary>
        public virtual bool TnbShenBing { get; set; }

        /// <summary>
        /// 糖尿病肾病 时间
        /// </summary>
        public virtual DateTime TnbShenBingDateTime { get; set; }

        /// <summary>
        /// 糖尿病视网膜病变
        /// </summary>
        public virtual bool TnbSwm { get; set; }

        /// <summary>
        /// 糖尿病视网膜病变 时间
        /// </summary>
        public virtual DateTime TnbSwmDateTime { get; set; }

        /// <summary>
        /// 失明
        /// </summary>
        public virtual bool ShiMing { get; set; }

        /// <summary>
        /// 失明 时间
        /// </summary>
        public virtual DateTime ShiMingDateTime { get; set; }

        /// <summary>
        /// 视力减退
        /// </summary>
        public virtual bool ShiLiJt { get; set; }

        /// <summary>
        /// 视力减退 时间
        /// </summary>
        public virtual DateTime ShiLiJtDateTime { get; set; }
        
        #endregion

        #endregion

        #region 是否因任何原因入院

        /// <summary>
        /// 是否住院
        /// </summary>
        public virtual YesNo InHospital { get; set; }

        /// <summary>
        /// 住院时间
        /// </summary>
        public virtual DateTime InHospitalDateTime { get; set; }

        /// <summary>
        /// 住院原因
        /// </summary>
        public virtual String InHospitalReason { get; set; }

        #endregion

        /// <summary>
        /// 登记时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

    }

    public class ClinicQuery : QueryBase
    {
        /// <summary>
        /// 患者
        /// </summary>
        public int? PatientId { get; set; }
    }
}
