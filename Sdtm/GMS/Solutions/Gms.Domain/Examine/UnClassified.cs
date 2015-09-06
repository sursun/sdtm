using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Attribute;

namespace Gms.Domain.Examine
{
    /// <summary>
    /// 并发症筛查
    /// 其他筛查
    /// </summary>
    public class UnClassified:ExamineBase
    {
        /// <summary>
        /// 心电图
        /// 正常
        /// 心律失常
        /// ST异常
        /// 传导阻滞
        /// 陈旧性心梗
        /// 其他
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int ElectroCardiogram { get; set; }

        /// <summary>
        /// 心电图
        /// 说明
        /// </summary>
        [FieldNeed]
        public virtual String ElectroCardiogramInfo { get; set; }

        /// <summary>
        /// 颈动脉血管超声
        /// 正常
        /// 斑块
        /// 狭窄
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int CarotidUltraL { get; set; }

        /// <summary>
        /// 颈动脉血管超声
        /// 百分比
        /// </summary>
        public virtual String CarotidUltraAreaL { get; set; }

        /// <summary>
        /// 颈动脉血管超声
        /// 正常
        /// 斑块
        /// 狭窄
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int CarotidUltraR { get; set; }

        /// <summary>
        /// 颈动脉血管超声
        /// 百分比
        /// </summary>
        public virtual String CarotidUltraAreaR { get; set; }

        /// <summary>
        /// 颈动脉血管超声
        /// </summary>
        [FieldNeed]
        public virtual String CarotidUltraInfo { get; set; }

        /// <summary>
        /// 头颅CT
        /// 正常
        /// 腔隙性梗塞
        /// 脑血栓
        /// 脑出血
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int HeadCT { get; set; }

        /// <summary>
        /// B超
        /// 脂肪肝
        /// 有
        /// 无
        /// 未查
        /// </summary>
        [FieldNeed]
        public virtual int FattyLiver { get; set; }

        /// <summary>
        /// B超
        /// 肾脏
        /// 长（cm）
        /// </summary>
        [FieldNeed]
        public virtual string KidneyLongL { get; set; }

        /// <summary>
        /// B超
        /// 肾脏
        /// 长（cm）
        /// </summary>
        [FieldNeed]
        public virtual string KidneyLongR { get; set; }

        /// <summary>
        /// B超
        /// 肾脏
        /// 宽（cm）
        /// </summary>
        [FieldNeed]
        public virtual string KidneyWidthL { get; set; }

        /// <summary>
        /// B超
        /// 肾脏
        /// 宽（cm）
        /// </summary>
        [FieldNeed]
        public virtual string KidneyWidthR { get; set; }

        /// <summary>
        /// B超
        /// 肾脏
        /// 高（cm）
        /// </summary>
        [FieldNeed]
        public virtual string KidneyHeightL { get; set; }
    
        /// <summary>
        /// B超
        /// 肾脏
        /// 高（cm）
        /// </summary>
        [FieldNeed]
        public virtual string KidneyHeightR { get; set; }

        /// <summary>
        /// B超
        /// 其他
        /// </summary>
        [FieldNeed]
        public virtual String BOther { get; set; }   

        /// <summary>
        /// 其他检查
        /// </summary>
        public virtual String OtherInfo { get; set; }
    }
}
