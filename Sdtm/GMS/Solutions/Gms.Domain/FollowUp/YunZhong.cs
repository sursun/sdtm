using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Health;

namespace Gms.Domain.FollowUp
{
    public class YunZhong : GdFollowUp
    {
        /// <summary>
        /// 酮体
        /// "(-)"
        /// "+"
        /// "++"
        /// "+++" 
        /// </summary>
        public virtual int Ketone { get; set; }

        /// <summary>
        /// 餐后血糖
        /// 小时
        /// </summary>
        public virtual string PbgHours { get; set; }

        /// <summary>
        /// 餐后血糖
        /// 血糖值（mmol/L ）
        /// </summary>
        public virtual string PbgValue { get; set; }

        /// <summary>
        /// 血糖监测频率（次/月）
        /// </summary>
        public virtual string BloodCheckTimes { get; set; }

        /// <summary>
        /// FBG（mmol/L ）
        /// </summary>
        public virtual string Fbg { get; set; }

        /// <summary>
        /// PBG 早（mmol/L ）
        /// </summary>
        public virtual string Pbg0 { get; set; }

        /// <summary>
        /// PBG 中（mmol/L ）
        /// </summary>
        public virtual string Pbg1 { get; set; }

        /// <summary>
        /// PBG 晚（mmol/L ）
        /// </summary>
        public virtual string Pbg2 { get; set; }

        /// <summary>
        /// 低血糖次数（过去一周）
        /// 0
        /// 1-2
        /// 3-5
        /// >5
        /// </summary>
        public virtual int DxtTimes { get; set; }

        /// <summary>
        /// 糖化血红蛋白
        /// </summary>
        public virtual string HbA1c { get; set; }
        
        /// <summary>
        /// 肝功能--ALT (Iu/L)
        /// </summary>
        public virtual string ALT { get; set; }

        /// <summary>
        /// 肝功能--GGT (Iu/L)
        /// </summary>
        public virtual string GGT { get; set; }
        
        /// <summary>
        /// 肾功能--BUN (umol/L )
        /// </summary>
        public virtual string BUN { get; set; }

        /// <summary>
        /// 尿蛋白
        /// "-"
        /// "+"
        /// "++"
        /// "+++" 
        /// </summary>
        public virtual int UrineProtein { get; set; }

        /// <summary>
        /// 血脂--总胆固醇 (mmol/L )
        /// </summary>
        public virtual string TC { get; set; }

        /// <summary>
        /// 血脂--甘油三酯 (mmol/L )
        /// </summary>
        public virtual string TG { get; set; }

        /// <summary>
        /// 血脂--高密度脂蛋白 (mmol/L ) 
        /// </summary>
        public virtual string HDL { get; set; }

        /// <summary>
        /// 血脂--低密度胆固醇 (mmol/L )
        /// </summary>
        public virtual string LDL { get; set; }

        /// <summary>
        /// 血常规 (g/L)
        /// </summary>
        public virtual string Hb { get; set; }

        /// <summary>
        /// 诊疗方案
        /// </summary>
        public virtual Treatment Treatment { get; set; }
    }
}
