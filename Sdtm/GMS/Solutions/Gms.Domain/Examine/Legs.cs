
using System;
using Gms.Domain.Attribute;

namespace Gms.Domain.Examine
{
    /// <summary>
    /// 并发症证筛查
    /// 下肢血管及神经病变筛查
    /// </summary>
    public class Legs : ExamineBase
    {
        /// <summary>
        /// 足皮肤颜色
        /// 正常
        /// 苍白
        /// 暗紫
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int ColorL { get; set; }

        /// <summary>
        /// 足皮肤颜色
        /// 正常
        /// 苍白
        /// 暗紫
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int ColorR { get; set; }

        /// <summary>
        /// 霉菌感染
        /// 否
        /// 灰甲
        /// 脚气
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int MouldL { get; set; }

        /// <summary>
        /// 霉菌感染
        /// 否
        /// 灰甲
        /// 脚气
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int MouldR { get; set; }
        
        /// <summary>
        /// 胼胝 
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck CallusL { get; set; }

        /// <summary>
        /// 胼胝 
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck CallusR { get; set; }

        /// <summary>
        /// 溃疡 
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck AnabrosisL { get; set; }

        /// <summary>
        /// 溃疡 
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck AnabrosisR { get; set; }

        /// <summary>
        /// 溃疡部位
        /// </summary>
        [FieldNeed]
        public virtual String AnabrosisNameL { get; set; }

        /// <summary>
        /// 溃疡部位
        /// </summary>
        [FieldNeed]
        public virtual String AnabrosisNameR { get; set; }

        /// <summary>
        /// 坏疽 
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck GangreneL { get; set; }


        /// <summary>
        /// 坏疽 
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck GangreneR { get; set; }

        /// <summary>
        /// 坏疽部位
        /// </summary>
        [FieldNeed]
        public virtual String GangreneNameL { get; set; }

        /// <summary>
        /// 坏疽部位
        /// </summary>
        [FieldNeed]
        public virtual String GangreneNameR { get; set; }

        /// <summary>
        /// 坏疽性质 
        /// </summary>
        [FieldNeed]
        public virtual int GangreneTypeL { get; set; }

        /// <summary>
        /// 坏疽性质 
        /// </summary>
        [FieldNeed]
        public virtual int GangreneTypeR { get; set; }

        /// <summary>
        /// 
        /// 畸形
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck MalformationL { get; set; }

        /// <summary>
        /// 
        /// 畸形
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck MalformationR { get; set; }

        /// <summary>
        /// 
        /// 尼龙丝感觉缺失
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck AnesthesiaL { get; set; }

        /// <summary>
        /// 
        /// 尼龙丝感觉缺失
        /// </summary>
        [FieldNeed]
        public virtual YesNoUncheck AnesthesiaR { get; set; }

        /// <summary>
        /// 动脉搏动
        /// 足背动脉
        /// 存在
        /// 消失
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int ZubDongmaiL { get; set; }

        /// <summary>
        /// 动脉搏动
        /// 足背动脉
        /// 存在
        /// 消失
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int ZubDongmaiR { get; set; }

        /// <summary>
        /// 动脉搏动
        /// 胫后动脉
        /// 存在
        /// 消失
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int JinghDongmaiL { get; set; }

        /// <summary>
        /// 动脉搏动
        /// 胫后动脉
        /// 存在
        /// 消失
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int JinghDongmaiR { get; set; }

        /// <summary>
        /// 踝/肱动脉压比值(ABI) 
        /// 上肢肱动脉收缩压
        /// mmHg
        /// </summary>
        [FieldNeed]
        public virtual string UpperBaspL { get; set; }

        /// <summary>
        /// 踝/肱动脉压比值(ABI) 
        /// 上肢肱动脉收缩压
        /// mmHg
        /// </summary>
        [FieldNeed]
        public virtual string UpperBaspR { get; set; }

        /// <summary>
        /// 踝/肱动脉压比值(ABI) 
        /// 下肢内踝动脉收缩压
        /// mmHg
        /// </summary>
        [FieldNeed]
        public virtual string XianhAsbpL { get; set; }

        /// <summary>
        /// 踝/肱动脉压比值(ABI) 
        /// 下肢内踝动脉收缩压
        /// mmHg
        /// </summary>
        [FieldNeed]
        public virtual string XianhAsbpR { get; set; }

        /// <summary>
        /// 踝/肱动脉压比值(ABI) 
        /// 足背动脉收缩压
        /// mmHg
        /// </summary>
        [FieldNeed]
        public virtual string DpaspL { get; set; }

        /// <summary>
        /// 踝/肱动脉压比值(ABI) 
        /// 足背动脉收缩压
        /// mmHg
        /// </summary>
        [FieldNeed]
        public virtual string DpaspR { get; set; }

        /// <summary>
        /// 踝/肱动脉压比值(ABI) 
        /// ABI 值
        /// </summary>
        [FieldNeed]
        public virtual string ABIL { get; set; }

        /// <summary>
        /// 踝/肱动脉压比值(ABI) 
        /// ABI 值
        /// </summary>
        [FieldNeed]
        public virtual string ABIR { get; set; }

        ///// <summary>
        ///// 踝/肱动脉压比值(ABI) 
        ///// 足趾血压
        ///// </summary>
        //[FieldNeed]
        //public virtual string ToeBPL { get; set; }
        
        ///// <summary>
        ///// 踝/肱动脉压比值(ABI) 
        ///// 足趾血压
        ///// </summary>
        //[FieldNeed]
        //public virtual string ToeBPR { get; set; }

        ///// <summary>
        ///// 踝/肱动脉压比值(ABI) 
        ///// TPI 值
        ///// </summary>
        //[FieldNeed]
        //public virtual string TPIL { get; set; }

        ///// <summary>
        ///// 踝/肱动脉压比值(ABI) 
        ///// TPI 值
        ///// </summary>
        //[FieldNeed]
        //public virtual string TPIR { get; set; }

        
        /// <summary>
        /// 生物震感阈值(VPT)测量 
        /// 感觉阈值测定
        /// </summary>
        [FieldNeed]
        public virtual string TouchFeelingL { get; set; }

        /// <summary>
        /// 生物震感阈值(VPT)测量 
        /// 感觉阈值测定
        /// </summary>
        [FieldNeed]
        public virtual string TouchFeelingR { get; set; }

///// <summary>
//        /// 生物震感阈值(VPT)测量 
//        /// 第一足趾
//        /// </summary>
//        [FieldNeed]
//        public virtual string Toe1StL { get; set; }

//        /// <summary>
//        /// 生物震感阈值(VPT)测量 
//        /// 第一足趾
//        /// </summary>
//        [FieldNeed]
//        public virtual string Toe1StR { get; set; }

//        /// <summary>
//        /// 生物震感阈值(VPT)测量 
//        /// 内踝
//        /// </summary>
//        [FieldNeed]
//        public virtual string MalleolusL { get; set; }

//        /// <summary>
//        /// 生物震感阈值(VPT)测量 
//        /// 内踝
//        /// </summary>
//        [FieldNeed]
//        public virtual string MalleolusR { get; set; }
        /// <summary>
        /// 生物震感阈值(VPT)测量 
        /// 温度觉
        /// 正常
        /// 异常
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int ThermalSenseL { get; set; }

        /// <summary>
        /// 生物震感阈值(VPT)测量 
        /// 温度觉
        /// 正常
        /// 异常
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int ThermalSenseR { get; set; }

        /// <summary>
        /// 生物震感阈值(VPT)测量 
        /// 下肢血管超声
        /// 正常
        /// 斑块
        /// 狭窄
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int LivuL { get; set; }

        /// <summary>
        /// 生物震感阈值(VPT)测量 
        /// 下肢血管超声
        /// 正常
        /// 斑块
        /// 狭窄
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int LivuR { get; set; }

    }
}
