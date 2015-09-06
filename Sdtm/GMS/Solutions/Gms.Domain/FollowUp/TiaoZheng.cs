using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Examine;
using Gms.Domain.Health;

namespace Gms.Domain.FollowUp
{
    public class TiaoZheng:FollowUp
    {
        public TiaoZheng()
        {
            FollowUpType = FollowUpType.调整;
        }

        #region 调整目标
        /// <summary>  
        /// 血压（mmHg）
        /// 高压值
        /// </summary>
        public virtual string BloodPressureHigh { get; set; }

        /// <summary>  
        /// 血压（mmHg）
        /// 低压值
        /// </summary>
        public virtual string BloodPressureLow { get; set; }

        /// <summary>
        /// 空腹血糖(mmol/L )
        /// 最近一周 指血糖
        /// </summary>
        public virtual string FBG { get; set; }

        /// <summary>
        /// 餐后血糖(mmol/L )
        /// </summary>
        public virtual string PBG { get; set; }

        /// <summary>
        /// 糖化血红蛋白(% )
        /// </summary>
        public virtual string HbA1c { get; set; }

        /// <summary>
        /// 低密度胆固醇(mmol/L )
        /// </summary>
        public virtual string LDL { get; set; }

        /// <summary>
        /// 体重(Kg) 
        /// </summary>
        public virtual string Weight { get; set; }

        #endregion

        #region 距上次随访至今

        /// <summary>
        /// 轻度 低血糖的次数
        /// </summary>
        public virtual string QingDxtTimes { get; set; }

        /// <summary>
        /// 严重 低血糖的次数
        /// </summary>
        public virtual string ZhongDxtTimes { get; set; }

        /// <summary>
        /// 血糖值低于3.9mmol/L 的次数
        /// </summary>
        public virtual string DxtTimes { get; set; }

        #endregion

        #region 血糖检测方案调整

        /// <summary>
        /// 血糖检测时间
        /// 0x00000001 空腹
        /// 0x00000002 早餐后
        /// 0x00000004 中餐前
        /// 0x00000008 中餐后
        /// 0x00000010 晚餐前
        /// 0x00000020 晚餐后
        /// 0x00000040 睡前
        /// 0x00000080 凌晨3点
        /// </summary>
        public virtual int BloodCheckFlag { get; set; }

        /// <summary>
        /// 血糖检测 内容
        /// </summary>
        public virtual string BloodCheckInfo { get; set; }

        #endregion

        #region 诊疗方案

        public virtual Treatment Treatment { get; set; }

        #endregion
    }
}
