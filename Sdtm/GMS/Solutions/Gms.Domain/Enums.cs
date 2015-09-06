using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gms.Domain
{
    public enum UserType
    {
        医生,
        患者
    }

    public enum Sex
    {
        男,
        女
    }

    public enum Enabled
    {
        启用,
        禁用
    }

    /// <summary>
    /// 糖尿病类型大类
    /// </summary>
    public enum DiabetesType
    {
        普通糖尿病,
        妊娠糖尿病
    }

    public enum YesNo
    {
        否,
        是
    }

    public enum YesNoUnclear
    {
        否,
        是,
        不清楚
    }

    public enum YesNoUncheck
    {
        否,
        是,
        未查
    }

    public enum YouWuJie
    {
        无,
        有,
        戒
    }

    public enum BloodType
    {
        A, B, AB, O, 其他
    }

    public enum CommonCodeType
    {
        地区,
        民族,
        职业,
        糖尿病类型,
        栏目类型,
        教育水平,
        疾病类别,
        药品类别,
        最新患者编号,
        其他
    }

    public enum MessageStatus
    {
        未查看, 已查看, 已回复,
    }

    /// <summary>
    /// 主治医生可查看患者范围
    /// </summary>
    public enum ScopeType
    {
        自己,
        全部,
        自定义
    }

    /// <summary>
    /// 病程阶段
    /// </summary>
    public enum DiseaseStage
    {
        维持阶段, 调整阶段
    }

    /// <summary>
    /// 随访方式
    /// </summary>
    public enum FollowUpWay
    {
        面对面随访, 电话随访
    }

    /// <summary>
    /// 随访类型
    /// </summary>
    public enum FollowUpType
    {
        复诊, 调整, 年度评估
    }

    /// <summary>
    /// 随访状态
    /// </summary>
    public enum FollowUpStatus
    {
        参加随访, 暂时离开超过六个月, 拒绝随访, 失访
    }

    /// <summary>
    /// 复诊状态
    /// </summary>
    public enum FuZhenStatus
    {
        待诊, 候诊, 过期
    }

    public enum Usage
    {
        一日一次, 一日两次, 一日三次
    }

    public enum DoseType
    {
        皮下注射,口服
    }

    public enum PayType
    {
        自费, 公费, 医保
    }

    /// <summary>
    /// GDM分级
    /// </summary>
    public enum GdmLevel
    {
        A1, A2
    }

    /// <summary>
    /// 产式
    /// </summary>
    public enum BirthType
    {
        顺产, 剖腹产, 其他
    }

    public enum YinYangUncheck
    {
        阴性, 阳性, 未查
    }

    public enum EducationFlag
    {
        糖尿病急性并发症防治 = 0x00000001,
        饮食及运动指导 = 0x00000002,
        口服药物知识 =0x00000004,
        胰岛素使用及注射技巧 =0x00000008,
        自我检测与护理 =0x00000010,
        慢性并发症筛查与防治 =0x00000020,
        糖尿病足部自我护理 =0x00000040,
        低血糖防治 =0x00000080
    }

    public enum ScaleType
    {
        简易智能状态评定表MMSE,
        Zung抑郁评定量表,
        Zung焦虑自我评价表
    }

    /// <summary>
    /// 高危因素
    /// </summary>
    public enum HighRiskFlag
    {
        一级亲属患2型糖尿病 = 0x00000001,
        GDM史 = 0x00000002,
        糖耐量异常史 = 0x00000004,
        超重或肥胖尤其重度肥胖 = 0x00000008,
        多囊卵巢综合症 = 0x00000010,
        年龄大于35岁 = 0x00000020,
        不明原因的死胎或死产或流产史 = 0x00000040,
        巨大儿分娩史 = 0x00000080,
        大于胎龄儿分娩史 = 0x00000100,
        胎儿畸形或羊水过多史 = 0x00000200,
        妊娠期发现胎儿大于孕周 = 0x00000400,
        反复外阴阴道假丝酵母菌病 = 0x00000800
    }

    /// <summary>
    /// 疾病史
    /// </summary>
    public enum IllnessFlag
    {
        高血压 = 0x00000001,
        甲状腺疾病 = 0x00000002,
        脂肪肝 = 0x00000004,
        心脏病 = 0x00000008,
        多囊卵巢综合症 = 0x00000010,
        肾脏疾病 = 0x00000020
    }

    /// <summary>
    /// 妊娠并发症
    /// </summary>
    public enum GdSyndromeFlag
    {
        妊高症 = 0x00000001,
        羊水过多 = 0x00000002,
        低血糖 = 0x00000004,
        乳酸酸中毒 = 0x00000008,
        酮症酸中毒 = 0x00000010,
        高渗综合症 = 0x00000020
    }
    
    /// <summary>
    /// 家族病史
    /// </summary>
    public enum FamilyIllnessFlag
    {
        糖尿病 = 0x00000001,
        甲状腺疾病 = 0x00000002,
        心血管疾病 = 0x00000004,
        高血压 = 0x00000008
    }

    /// <summary>
    /// 产时产后并发症
    /// </summary>
    public enum ChanHouBfzFlag
    {
        无 = 0x00000001,
        早产 = 0x00000002,
        产后出血 = 0x00000004,
        妊高症 = 0x00000008,
        子痫 = 0x00000010,
        胎膜早破 = 0x00000020,
        滞产 = 0x00000040,
        脐带绕颈或脱垂 = 0x00000080,
        子宫破裂 = 0x00000100,
        软产道血肿 = 0x00000200,
        切口感染 = 0x00000400,
        产褥感染 = 0x00000800,
        其他 = 0x00001000
    }

    /// <summary>
    /// 新生儿并产症
    /// </summary>
    public enum BabyBfzFlag
    {
        无 = 0x00000001,
        重度窒息 = 0x00000002,
        肺炎 = 0x00000004,
        产伤 = 0x00000008,
        呼吸窘迫 = 0x00000010,
        脐部感染 = 0x00000020,
        硬肿症 = 0x00000040,
        其他 = 0x00000080
    }

    /// <summary>
    /// 既往史 
    /// </summary>
    public enum MedicalHistoryType
    {
        肿瘤,手术,骨折
    }

    public enum Exam
    {
        HbA1c,
        舒张压,
        收缩压,
        LDL,
        尿微量白蛋白,
        眼睛,
        下肢血管及神经病变,
        感觉阈值
    }
}

