using Gms.Domain.Attribute;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain.Health
{
    public class GdHistory:Completion
    {
        /// <summary>
        /// 患者
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// 疾病史
        /// 0x00000001 高血压
        /// 0x00000002 甲状腺疾病
        /// 0x00000004 脂肪肝
        /// 0x00000008 心脏病
        /// 0x00000010 血脂异常
        /// 0x00000020 肾脏疾病
        /// </summary>
        public virtual int IllnessFlag { get; set; }

        #region 孕产史

        /// <summary>
        /// 死胎人次
        /// </summary>
        [FieldNeed]
        public virtual string SiTai { get; set; }

        /// <summary>
        /// 死产人次
        /// </summary>
        [FieldNeed]
        public virtual string SiChan { get; set; }

        /// <summary>
        /// 流产人次
        /// </summary>
        [FieldNeed]
        public virtual string LiuChan { get; set; }

        /// <summary>
        /// 足月产人次
        /// </summary>
        [FieldNeed]
        public virtual string ZuYueChan { get; set; }

        /// <summary>
        /// 早产人次
        /// </summary>
        [FieldNeed]
        public virtual string ZaoChan { get; set; }

        #endregion

        /// <summary>
        /// 妊娠并发症
        /// 0x00000001 妊高症
        /// 0x00000002 羊水过多
        /// 0x00000004 低血糖
        /// 0x00000008 乳酸酸中毒
        /// 0x00000010 酮症酸中毒
        /// 0x00000020 高渗综合症
        /// </summary>
        public virtual int GdSyndromeFlag { get; set; }

        /// <summary>
        /// 妊娠糖尿病史
        /// </summary>
        [FieldNeed]
        public virtual YesNo GdEver { get; set; }

        /// <summary>
        /// GDM分级
        /// </summary>
        public virtual GdmLevel GdmLevel { get; set; }

        /// <summary>
        /// 家族史（一级亲属）
        /// 0x00000001 糖尿病
        /// 0x00000002 甲状腺疾病
        /// 0x00000004 心血管疾病
        /// 0x00000008 高血压
        /// </summary>
        public virtual int FamilyIllnessFlag { get; set; }
    }
}
