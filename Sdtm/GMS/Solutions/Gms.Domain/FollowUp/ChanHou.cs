using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Common;
using Gms.Domain.Attribute;

namespace Gms.Domain.FollowUp
{
    public class ChanHou:GdFollowUp
    {
        public ChanHou()
        {
            ChildbirthDateTime = DateTimeEx.Default();
        }

        #region 分娩情况及新生儿管理

        /// <summary>
        /// 分娩日期
        /// </summary>
        public virtual DateTime ChildbirthDateTime { get; set; }

        /// <summary>
        /// 产式
        /// </summary>
        public virtual BirthType BirthType { get; set; }

        /// <summary>
        /// 产后24小时出血量（ml）
        /// </summary>
        public virtual string ChuXueLiang { get; set; }

        /// <summary>
        /// 产时产后并发症
        /// 0x00000001 早产	
        /// 0x00000002 产后出血
        /// 0x00000004 妊高症	
        /// 0x00000008 子痫	
        /// 0x00000010 胎膜早破 
        /// 0x00000020 滞产	
        /// 0x00000040 脐带绕颈/脱垂	
        /// 0x00000080 子宫破裂 
        /// 0x00000100 软产道血肿	
        /// 0x00000200 切口感染	
        /// 0x00000400 产褥感染 
        /// 0x00000800 其他	
        /// </summary>
        public virtual int BingFa { get; set; }

        /// <summary>
        /// 新生儿体重(g)
        /// </summary>
        public virtual string BabyWeight { get; set; }

        /// <summary>
        /// Apgar评分
        /// </summary>
        public virtual string Apgar { get; set; }

        /// <summary>
        /// 新生儿并产症
        /// 0x00000001 重度窒息
        /// 0x00000002 肺炎
        /// 0x00000004 产伤
        /// 0x00000008 呼吸窘迫
        /// 0x00000010 脐部感染
        /// 0x00000020 硬肿症
        /// 0x00000040 其他
        /// </summary>
        public virtual int BabyBing { get; set; }

        /// <summary>
        /// 出生缺陷
        /// </summary>
        public virtual String BabyQuexian { get; set; }

        /// <summary>
        /// 存活情况
        /// 活产
        /// 死胎
        /// 死产
        /// </summary>
        public virtual int Breathing { get; set; }

        /// <summary>
        /// 存活情况
        /// 描述
        /// </summary>
        public virtual String DeathReason { get; set; }

        /// <summary>
        /// 新生儿低血糖
        /// </summary>
        public virtual YesNo Dxt { get; set; }

        /// <summary>
        /// 血糖值(mmol/L)
        /// 出生后1~2小时血糖低于2.2mmol/L（40mg/dL）
        /// </summary>
        public virtual String DxtValue { get; set; }

        #endregion

        #region 产后6~12周随访记录

        /// <summary>
        /// 是否母乳喂养
        /// </summary>
        public virtual YesNo BreastFeeding { get; set; }

        /// <summary>
        /// OGTT
        /// GLU(mmol/L )
        /// 0min 
        /// </summary>
        public virtual string Glu0M { get; set; }

        /// <summary>
        /// OGTT
        /// GLU(mmol/L )
        /// 30min 
        /// </summary>
        public virtual string Glu30M { get; set; }
        
        /// <summary>
        /// OGTT
        /// GLU(mmol/L )
        /// 120min 
        /// </summary>
        public virtual string Glu120M { get; set; }

        /// <summary>
        /// OGTT
        /// 胰岛素(uIu/ml )
        /// 0min 
        /// </summary>
        public virtual string Insulin0M { get; set; }

        /// <summary>
        /// OGTT
        /// 胰岛素(uIu/ml )
        /// 30min 
        /// </summary>
        public virtual string Insulin30M { get; set; }

        /// <summary>
        /// OGTT
        /// 胰岛素(uIu/ml )
        /// 120min 
        /// </summary>
        public virtual string Insulin120M { get; set; }


        /// <summary>
        /// GAD
        /// </summary>
        public virtual YinYangUncheck Gad { get; set; }

        /// <summary>
        /// ICA
        /// </summary>
        public virtual YinYangUncheck Ica { get; set; }

        /// <summary>
        /// IAA
        /// </summary>
        public virtual YinYangUncheck Iaa { get; set; }
        
        #endregion

        /// <summary>
        /// 糖尿病类型
        /// </summary>
        [FieldNeed]
        public virtual CommonCode Diabetes { get; set; }
    }
}
