using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain.Attribute;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Examine
{
    /// <summary>
    /// 血液常规检查
    /// </summary>
    public class BloodRt : ExamineBase
    {
        /// <summary>
        /// 肝功能--ALT (Iu/L)
        /// </summary>
        [FieldNeed]
        public virtual string Alt { get; set; }

        /// <summary>
        /// 肝功能--AST (Iu/L)
        /// </summary>
        [FieldNeed]
        public virtual string Ast { get; set; }

        /// <summary>
        /// 肝功能--GGT (Iu/L)
        /// </summary>
        [FieldNeed]
        public virtual string Ggt { get; set; }

        /// <summary>
        /// 血尿酸(umol/L )
        /// </summary>
        [FieldNeed]
        public virtual string Ua { get; set; }

        /// <summary>
        /// 肾功能--BUN (umol/L )
        /// </summary>
        [FieldNeed]
        public virtual string Bun { get; set; }

        /// <summary>
        /// 肾功能--SCr (umol/L )
        /// </summary>
        [FieldNeed]
        public virtual string Scr { get; set; }

        /// <summary>
        /// 肾功能--eGFR (ml/min)
        /// Cockcroft-Gault(C-G)
        /// </summary>
        [FieldNeed]
        public virtual string Egfr { get; set; }
        
        /// <summary>
        /// 血脂--总胆固醇 (mmol/L )
        /// </summary>
        [FieldNeed]
        public virtual string Tc { get; set; }

        /// <summary>
        /// 血脂--甘油三酯 (mmol/L )
        /// </summary>
        [FieldNeed]
        public virtual string Tg { get; set; }

        /// <summary>
        /// 血脂--高密度脂蛋白 (mmol/L ) 
        /// </summary>
        [FieldNeed]
        public virtual string Hdl { get; set; }

        /// <summary>
        /// 血脂--低密度胆固醇 (mmol/L )
        /// </summary>
        [FieldNeed]
        public virtual string LDL { get; set; }
        
    }
}
