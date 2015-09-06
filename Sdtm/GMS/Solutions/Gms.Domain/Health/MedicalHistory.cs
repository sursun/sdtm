using System;
using System.Collections.Generic;
using Gms.Domain.Attribute;
using SharpArch.Domain.DomainModel;
using Gms.Common;

namespace Gms.Domain.Health
{
    public class MedicalHistoryItem : Entity
    {
        public MedicalHistoryItem()
        {
            DateTime = DateTimeEx.Default();
        }

        /// <summary>
        /// 所属既往史
        /// </summary>
        public virtual MedicalHistory MedicalHistory { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual MedicalHistoryType MedicalHistoryType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Note { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime DateTime { get; set; }
    }

    /// <summary>
    /// 基线资料
    /// 既往史
    /// </summary>
    public class MedicalHistory : Completion
    {
        public MedicalHistory()
        {
            GuanXinBingDateTime = DateTimeEx.Default();
            GaoXueYaDateTime = DateTimeEx.Default();
            DanBaiNiaoDateTime = DateTimeEx.Default();
            XueYeTxDateTime = DateTimeEx.Default();
            FuTouZlDateTime = DateTimeEx.Default();
            
            
        }

        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        #region 心血管系统

        /// <summary>
        /// 心肌梗塞
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear GuanXinBing { get; set; }

        /// <summary>
        /// 冠心病？年
        /// </summary>
        [FieldNeed]
        public virtual String GuanXinBingCourse { get; set; }

        /// <summary>
        /// 冠心病诊断时间
        /// </summary>
        public virtual DateTime GuanXinBingDateTime { get; set; }

        /// <summary>
        /// 心绞痛
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear XinJiaoTong { get; set; }

        /// <summary>
        /// 心肌梗塞
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear XinJiGs { get; set; }

        /// <summary>
        /// 血管再通 
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear XueGuanZt { get; set; }

        /// <summary>
        /// 冠脉搭桥
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear GuanMaiDq { get; set; }

        /// <summary>
        /// 心衰
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear XinShuai { get; set; }

        /// <summary>
        /// 高血压
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear GaoXueYa { get; set; }

        /// <summary>
        /// 高血压诊断时间
        /// </summary>
        public virtual DateTime GaoXueYaDateTime { get; set; }

        /// <summary>
        /// 高血压病程
        /// </summary>
        public virtual String GaoXueYaCourse { get; set; }

        
        #endregion

        #region 中枢神经系统

        /// <summary>
        /// 脑出血
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear NaoChuXue { get; set; }

        /// <summary>
        /// 脑梗塞
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear NaoGengSe { get; set; }

        #endregion

        #region 高尿酸血症及痛风

        /// <summary>
        /// 高尿酸血症
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear GaoNiaoSxz { get; set; }

        /// <summary>
        /// 急性痛风性关节炎
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear TongFengGjy { get; set; }

        /// <summary>
        /// 痛风性肾病
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear TongFengSb { get; set; }

        #endregion

        #region 肾病

        /// <summary>
        /// 原发性肾小球肾炎
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear ShenXiaoQy { get; set; }

        /// <summary>
        /// 蛋白尿
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear DanBaiNiao { get; set; }

        /// <summary>
        /// 尿蛋白出现时间
        /// </summary>
        public virtual DateTime DanBaiNiaoDateTime { get; set; }

        /// <summary>
        /// 血液透析
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear XueYeTx { get; set; }

        /// <summary>
        /// 血液透析开始时间
        /// </summary>
        public virtual DateTime XueYeTxDateTime { get; set; }

        /// <summary>
        /// 腹透治疗
        /// </summary>
        [FieldNeed]
        public virtual YesNoUnclear FuTouZl { get; set; }

        /// <summary>
        /// 腹透治疗 开始时间
        /// </summary>
        public virtual DateTime FuTouZlDateTime { get; set; }

        #endregion


        /// <summary>
        /// 肿瘤
        /// </summary>
        [FieldNeed]
        public virtual YesNo ZhongLiu { get; set; }
        
        /// <summary>
        /// 手术
        /// </summary>
        [FieldNeed]
        public virtual YesNo ShouShu { get; set; }
 
        /// <summary>
        /// 骨折
        /// </summary>
        [FieldNeed]
        public virtual YesNo GuZhe { get; set; }


        #region 截肢史

        /// <summary>
        /// 踝关节以上
        /// </summary>
        [FieldNeed]
        public virtual YesNo HuaiGuanjShang { get; set; }

        /// <summary>
        /// 踝关节以下
        /// </summary>
        [FieldNeed]
        public virtual YesNo HuaiGuanjXia { get; set; }

        #endregion

        #region 下肢血管旁路/再通治疗

        /// <summary>
        /// 下肢血管旁路/再通治疗
        /// </summary>
        [FieldNeed]
        public virtual YesNo XueGuanZl { get; set; }

        #endregion

        #region 糖尿病慢性并发症病史

        /// <summary>
        /// 眼底病变
        /// </summary>
        [FieldNeed]
        public virtual YesNo YanDiBb { get; set; }

        /// <summary>
        /// 神经病变
        /// </summary>
        [FieldNeed]
        public virtual YesNo ShenJingBb { get; set; }

        /// <summary>
        /// 糖尿病足
        /// </summary>
        [FieldNeed]
        public virtual YesNo TnbZu { get; set; }

        /// <summary>
        /// 糖尿病肾病
        /// </summary>
        [FieldNeed]
        public virtual YesNo TnbShenBing { get; set; }

        #endregion

        #region 糖尿病急性并发症

        /// <summary>
        /// 糖尿病酮症酸中毒 
        /// </summary>
        [FieldNeed]
        public virtual YesNo TnbTongZd { get; set; }

        /// <summary>
        /// 非酮症高渗综合症 
        /// </summary>
        [FieldNeed]
        public virtual YesNo FeiTongZhz { get; set; }

        /// <summary>
        /// 乳酸酸中毒 
        /// </summary>
        [FieldNeed]
        public virtual YesNo RuShuanZd { get; set; }

        /// <summary>
        /// 1年内发生 轻度 低血糖的次数
        /// </summary>
        [FieldNeed]
        public virtual string QingDxtTimes { get; set; }

        /// <summary>
        /// 1年内发生 严重 低血糖的次数
        /// </summary>
        [FieldNeed]
        public virtual string ZhongDxtTimes { get; set; }

        #endregion

        #region 主要症状

        /// <summary>
        /// 间歇性跛行
        /// </summary>
        [FieldNeed]
        public virtual YesNo BoXing { get; set; }

        /// <summary>
        /// 阳痿
        /// </summary>
        [FieldNeed]
        public virtual YesNo YangWei { get; set; }

        /// <summary>
        /// 麻木
        /// </summary>
        [FieldNeed]
        public virtual YesNo MaMu { get; set; }

        /// <summary>
        /// 视物模糊
        /// </summary>
        [FieldNeed]
        public virtual YesNo ShiWuMh { get; set; }

        #endregion
    }
}
