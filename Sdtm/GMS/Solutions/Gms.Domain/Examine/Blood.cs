using Gms.Domain.Attribute;

namespace Gms.Domain.Examine
{
    /// <summary>
    /// 血液检查
    /// </summary>
    public class Blood : ExamineBase
    {
        /// <summary>
        /// 血红蛋白(g/L )
        /// </summary>
        [FieldNeed]
        public virtual string Hb { get; set; }

        /// <summary>
        /// 红细胞压积
        /// </summary>
        [FieldNeed]
        public virtual string HCT { get; set; }

        /// <summary>
        /// 红细胞平均体积(fl)
        /// </summary>
        [FieldNeed]
        public virtual string MCV { get; set; }

        /// <summary>
        /// 红细胞计数(×10 12)
        /// </summary>
        [FieldNeed]
        public virtual string RBC { get; set; }

        /// <summary>
        /// 白细胞计数(×10 9)
        /// </summary>
        [FieldNeed]
        public virtual string WBC { get; set; }

        /// <summary>
        /// 血小板计数(×10 9)
        /// </summary>
        [FieldNeed]
        public virtual string PLT { get; set; }
        
        /// <summary>
        /// 血酮(mmol/L )
        /// </summary>
        [FieldNeed]
        public virtual string XueTong { get; set; }

        /// <summary>
        /// 血清/碳酸氢盐(mmol/L )
        /// </summary>
        [FieldNeed]
        public virtual string XueQing { get; set; }

        /// <summary>
        /// 75gOGTT
        /// GLU(mmol/L )
        /// 0min 
        /// </summary>
        [FieldNeed]
        public virtual string Glu0M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// GLU(mmol/L )
        /// 30min 
        /// </summary>
        [FieldNeed]
        public virtual string Glu30M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// GLU(mmol/L )
        /// 60min 
        /// </summary>
        [FieldNeed]
        public virtual string Glu60M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// GLU(mmol/L )
        /// 120min 
        /// </summary>
        [FieldNeed]
        public virtual string Glu120M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// GLU(mmol/L )
        /// 180min 
        /// </summary>
        [FieldNeed]
        public virtual string Glu180M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰岛素(uIu/ml )
        /// 0min 
        /// </summary>
        [FieldNeed]
        public virtual string Insulin0M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰岛素(uIu/ml )
        /// 30min 
        /// </summary>
        [FieldNeed]
        public virtual string Insulin30M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰岛素(uIu/ml )
        /// 60min 
        /// </summary>
        [FieldNeed]
        public virtual string Insulin60M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰岛素(uIu/ml )
        /// 120min 
        /// </summary>
        [FieldNeed]
        public virtual string Insulin120M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰岛素(uIu/ml )
        /// 180min 
        /// </summary>
        [FieldNeed]
        public virtual string Insulin180M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// C-肽(ng/ml )
        /// 0min 
        /// </summary>
        [FieldNeed]
        public virtual string CTai0M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// C-肽(ng/ml )
        /// 30min 
        /// </summary>
        [FieldNeed]
        public virtual string CTai30M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// C-肽(ng/ml )
        /// 60min 
        /// </summary>
        [FieldNeed]
        public virtual string CTai60M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// C-肽(ng/ml )
        /// 120min 
        /// </summary>
        [FieldNeed]
        public virtual string CTai120M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// C-肽(ng/ml )
        /// 180min 
        /// </summary>
        [FieldNeed]
        public virtual string CTai180M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰高血糖素
        /// 0min 
        /// </summary>
        [FieldNeed]
        public virtual string Glucagon0M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰高血糖素
        /// 30min 
        /// </summary>
        [FieldNeed]
        public virtual string Glucagon30M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰高血糖素
        /// 60min 
        /// </summary>
        [FieldNeed]
        public virtual string Glucagon60M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰高血糖素
        /// 120min 
        /// </summary>
        [FieldNeed]
        public virtual string Glucagon120M { get; set; }

        /// <summary>
        /// 75gOGTT
        /// 胰高血糖素
        /// 180min 
        /// </summary>
        [FieldNeed]
        public virtual string Glucagon180M { get; set; }

        /// <summary>
        /// 糖尿病有关抗体--GAD 
        /// 未查
        /// （-）
        /// +
        /// </summary>
        [FieldNeed]
        public virtual int Gad { get; set; }
        
        /// <summary>
        /// 糖尿病有关抗体--IAA 
        /// 未查
        /// （-）
        /// +
        /// </summary>
        [FieldNeed]
        public virtual int Iaa { get; set; }

        /// <summary>
        /// 糖尿病有关抗体--ICA 
        /// 未查
        /// （-）
        /// +
        /// </summary>
        [FieldNeed]
        public virtual int Ica { get; set; }
        
        /// <summary>
        /// 癌胚抗原(ng/mL )
        /// </summary>
        [FieldNeed]
        public virtual string Cea { get; set; }
        
        /// <summary>
        /// 甲胎蛋白(ng/mL)
        /// </summary>
        [FieldNeed]
        public virtual string Afp { get; set; }

        /// <summary>
        /// 血小板聚集率(%)
        /// </summary>
        [FieldNeed]
        public virtual string Par { get; set; }
        
        /// <summary>
        /// 纤维蛋白原(G/L)
        /// </summary>
        [FieldNeed]
        public virtual string Fib { get; set; }

        /// <summary>
        /// 超敏C反应蛋白(mg/L)
        /// </summary>
        [FieldNeed]
        public virtual string Crp { get; set; }

        /// <summary>
        /// 甲状腺功能
        /// TT3(ng/mL)
        /// </summary>
        [FieldNeed]
        public virtual string Tt3 { get; set; }

        /// <summary>
        /// 甲状腺功能
        /// TT4(nmol/L)
        /// </summary>
        [FieldNeed]
        public virtual string Tt4 { get; set; }

        /// <summary>
        /// 甲状腺功能
        /// FT3(pmol/L)
        /// </summary>
        [FieldNeed]
        public virtual string Ft3 { get; set; }

        /// <summary>
        /// 甲状腺功能
        /// FT4(pmol/L)
        /// </summary>
        [FieldNeed]
        public virtual string Ft4 { get; set; }

        /// <summary>
        /// 甲状腺功能
        /// TSH(uIu/mL)
        /// </summary>
        [FieldNeed]
        public virtual string Tsh { get; set; }

        /// <summary>
        /// 甲状腺相关抗体
        /// TR-Ab(IU/L)
        /// </summary>
        [FieldNeed]
        public virtual string Trab { get; set; }

        /// <summary>
        /// 甲状腺相关抗体
        /// TG-Ab(KU/L)
        /// </summary>
        [FieldNeed]
        public virtual string Tgab { get; set; }

        /// <summary>
        /// 甲状腺相关抗体
        /// TPO-Ab(KU/L)
        /// </summary>
        [FieldNeed]
        public virtual string Tpoab { get; set; }

        /// <summary>
        /// 甲状腺相关抗体
        /// TG(ng/ml)
        /// </summary>
        [FieldNeed]
        public virtual string Tg { get; set; }
        
    }
}
