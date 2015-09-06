using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Security;
using Gms.Common;
using Gms.Domain;
using Gms.Domain.Examine;
using Gms.Domain.FollowUp;
using Gms.Domain.Health;
using SharpArch.NHibernate.Web.Mvc;

namespace Gms.Web.Mvc.Controllers
{
    using System.Web.Mvc;
    [HandleError]
    public class ImportController : BaseController
    {
        public ActionResult Index()
        {          
            return View();
        }

        public ActionResult ImportPatient()
        {
            string str = Path.Combine(Server.MapPath("~/Upload"), "patient.xls");
            try
            {
                var dt = str.GetOledbFirstTableData();// CreateDataTable(strConn);

                DataRow[] dr = dt.Select("类别<>' '");

                int nCount = 0;
                foreach (var tmpDr in dr)
                {
                    int nType = int.Parse(GetDataRowItem(tmpDr, PatientExcelHead.类别));

                    var patient = CreatePatient(tmpDr);

                    String strTemp = "";

                    if (nType == 0)
                    {
                        strTemp = "基线资料";

                        TryCreateBaseLine(patient, tmpDr);
                    }
                    else if (nType == 1)
                    {
                        strTemp = "复诊记录";

                        FuZhenAdd(patient, tmpDr);
                    }
                    else if (nType == 2)
                    {
                        strTemp = "调整记录";

                        TiaoZhengAdd(patient, tmpDr);
                    }
                    else if (nType == 3)
                    {
                        strTemp = "年度随访";

                        AnnualAdd(patient, tmpDr);
                    }

                    SetFollowUpInfo(patient, tmpDr);

                    UpdatePatientDiseaseStage(tmpDr);

                    nCount ++;

                    Debug.WriteLine("成功导入第【{0}】行数据【{1}】的{2}", nCount, patient.RealName, strTemp);
                    
                }

            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess();
        }

        [Transaction]
        public ActionResult ImportMedicate()
        {
            string str = Path.Combine(Server.MapPath("~/Upload"), "medicate.xls");
            try
            {
                var dt = str.GetOledbFirstTableData();// CreateDataTable(strConn);

                DataRow[] dr = dt.Select("记录名称<>' '");

                DataRow rowHead = null;
                foreach (var dataRow in dr)
                {
                    String strName = GetDataRowItem(dataRow, (int)MedicateHead.姓名);
                    if (strName == "head")
                    {
                        rowHead = dataRow;
                        break;
                    }
                }

                if (rowHead == null)
                {
                    throw new Exception("数据格式不规范");
                }

                int nCount = 0;
                foreach (var tmpDr in dr)
                {
                    String strName = GetDataRowItem(tmpDr, (int)MedicateHead.姓名);

                    if (strName == "head")
                    {
                        continue;
                    }

                    Patient patient = this.PatientRepository.GetByName(strName);
                    if (patient == null)
                    {
                        //患者不存在，不重复添加
                        Debug.WriteLine(String.Format("患者{0}不存在", strName));
                        continue;
                    }

                    var list = GetMedicates(patient, tmpDr, rowHead);
                    if (list == null || list.Count < 1)
                    {
                        continue;
                    }



                    string strTypeName = "基线资料";
                    String strTmp = GetDataRowItem(tmpDr, (int)MedicateHead.记录名称);
                    Treatment treatment = null;
                    if (strTmp == strTypeName)
                    {
                        BaseLine bl = this.BaseLineRepository.GetBy(patient.Id);

                        if (bl == null)
                        {
                            Debug.WriteLine(String.Format("患者{0}基线资料不存在", strName));
                            continue;
                        }

                        treatment = bl.Treatment;
                        
                    }
                    else
                    {
                        var q = new FollowUpQuery() {PatientId = patient.Id, Name = strTmp};
                        IList<TiaoZheng> tiaoZhengs = this.TiaoZhengRepository.GetAll(q);
                        if (tiaoZhengs.Count > 0)
                        {
                            treatment = tiaoZhengs[0].Treatment;
                        }
                        else
                        {
                            IList<FuZhen> fuZhens = this.FuZhenRepository.GetAll(q);
                            if (fuZhens.Count > 0)
                            {
                                treatment = fuZhens[0].Treatment;
                            }
                            else
                            {
                                IList<Annual> annuals = this.AnnualRepository.GetAll(q);
                                if (annuals.Count > 0)
                                {
                                    treatment = annuals[0].Treatment;
                                }
                            }
                        }
                    }

                    if (treatment != null)
                    {
                        treatment.Medicates.Clear();
                        foreach (var item in list)
                        {
                            var medicine = this.MedicineRepository.GetBy(item.Medicine.ChemicalName);
                            if (medicine == null)
                            {
                                medicine = this.MedicineRepository.SaveOrUpdate(item.Medicine);
                            }
                            item.Medicine = medicine;

                            treatment.Medicates.Add(this.MedicateRepository.SaveOrUpdate(item));
                        }

                        this.TreatmentRepository.SaveOrUpdate(treatment);
                    }

                    nCount ++;

                    Debug.WriteLine("成功导入第【{0}】条数据【{1}】", nCount, patient.RealName);
                    
                }

            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess();
        }
        
        public ActionResult ImportClinic()
        {
            string str = Path.Combine(Server.MapPath("~/Upload"), "clinic.xls");
            try
            {
                var dt = str.GetOledbFirstTableData();// CreateDataTable(strConn);

                DataRow[] dr = dt.Select("身份证号码<>' '");

                int nCount = 0;
                foreach (var tmpDr in dr)
                {
                    String strIdCard = GetClinicDataRowItem(tmpDr, ClinicExcelHead.身份证号码);

                    Patient patient = this.PatientRepository.Get(strIdCard);
                    if (patient == null)
                    {
                        //身份证号已经存在，不重复添加
                        Debug.WriteLine("身份证号{0}不存在", strIdCard);
                        continue;
                    }
                    
                    nCount ++;

                    Debug.WriteLine("成功导入第【{0}】行数据【{1}】", nCount, patient.RealName);
                    
                }

            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess();
        }

        [Transaction]
        public ActionResult ImportMedicine()
        {
            string str = Path.Combine(Server.MapPath("~/Upload"), "medicine.xls");
            try
            {
                var dt = str.GetOledbFirstTableData();// CreateDataTable(strConn);

                DataRow[] dr = dt.Select("YPMC<>' '");

                int nCount = 0;
                foreach (var tmpDr in dr)
                {
                    String strType = "";
                    object obj = tmpDr.ItemArray[3];

                    if (obj != null)
                    {
                        strType = obj.ToString().Trim();
                    }

                    var mType = this.CommonCodeRepository.GetOrCreate(strType, CommonCodeType.药品类别);

                    string strNameC = "";
                    string strNameN = "";
                    string strName = "";
                    obj = tmpDr.ItemArray[0];
                    if (obj != null)
                    {
                        strName = obj.ToString().Trim();
                    }
                    if (strName.Length > 1)
                    {
                        string[] strArr = strName.Split('|');

                        if (strArr.Length > 0)
                        {
                            strNameC = strNameN = strArr[0];
                        }

                        if (strArr.Length > 1)
                        {
                            strNameN = strArr[1];
                        }
                    }

                    string strPinYin = "";
                    obj = tmpDr.ItemArray[1];
                    if (obj != null)
                    {
                        strPinYin = obj.ToString().Trim();
                    }

                    string strModel = "";
                    obj = tmpDr.ItemArray[2];
                    if (obj != null)
                    {
                        strModel = obj.ToString().Trim();
                    }

                    var medicine = this.MedicineRepository.SaveOrUpdate(new Medicine()
                    {
                        MedicineType = mType,ChemicalName = strNameC,Model = strModel,NormalName = strNameN,PinYin = strPinYin
                    });
                    
                    nCount++;

                    Debug.WriteLine("成功导入第【{0}】行数据【{1}】", nCount, medicine.NormalName);

                }

            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess();
        }

        [Transaction]
        public Patient CreatePatient(DataRow tmpDr)
        {
            String strCodeNo = GetDataRowItem(tmpDr,PatientExcelHead.身份证号码);
            Patient patient = this.PatientRepository.Get(strCodeNo);
            if (patient != null)
            {
                //身份证号已经存在，不重复添加
                Console.WriteLine("身份证号{0}已经存在", strCodeNo);
                return patient;
            }

            String strTemp = "";

            patient = new Patient();

            patient.DiabetesType = DiabetesType.普通糖尿病;
            patient.CodeNo = GetDataRowItem(tmpDr,PatientExcelHead.患者原始编号);
            patient.RealName = GetDataRowItem(tmpDr,PatientExcelHead.姓名);
            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.性别);
            if (strTemp.Length > 0)
            {
                patient.Sex = (Sex)Enum.Parse(typeof(Sex), strTemp);
            }
            
            patient.IdCard = GetDataRowItem(tmpDr,PatientExcelHead.身份证号码);
            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.出生年月);
            patient.Birthday = DateTimeParseExact(strTemp, "yyyy-MM-dd");
       
            patient.Area =
                this.CommonCodeRepository.GetOrCreate(GetDataRowItem(tmpDr,PatientExcelHead.所在省份),
                    CommonCodeType.地区);
            patient.Mobile1 = GetDataRowItem(tmpDr,PatientExcelHead.手机号码);
            patient.Mobile2 = GetDataRowItem(tmpDr,PatientExcelHead.固定电话);
            patient.Diabetes =
                this.CommonCodeRepository.GetOrCreate(GetDataRowItem(tmpDr,PatientExcelHead.糖尿病类型),
                    CommonCodeType.糖尿病类型);
            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.诊断日期);
            patient.DiagnoseDate = DateTimeParseExact(strTemp, "yyyy-MM-dd");
            
            patient.DiseaseStage = DiseaseStage.维持阶段;

            //
            String strDoctorName = GetDataRowItem(tmpDr,PatientExcelHead.医生);

            patient.Doctor = GetOrCreateDoctor(strDoctorName);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.填表时间);
            patient.CreateTime = DateTimeParseExact(strTemp, "yyyy-MM-dd");

            return this.PatientRepository.SaveOrUpdate(patient);
        }

        [Transaction]
        public void UpdatePatientDiseaseStage(DataRow tmpDr)
        {
            String strCodeNo = GetDataRowItem(tmpDr, PatientExcelHead.身份证号码);
            Patient patient = this.PatientRepository.Get(strCodeNo);
            if (patient == null)
            {
                Debug.WriteLine("身份证号{0}不存在", strCodeNo);
                return;
            }

            String strTemp = GetDataRowItem(tmpDr, PatientExcelHead.病程阶段);
  
            if (strTemp.Length > 0)
            {
                patient.DiabetesType = (DiabetesType)Enum.Parse(typeof(DiabetesType), strTemp);
            }

            PatientRepository.SaveOrUpdate(patient);
        }

        public string GetDataRowItem(DataRow tmpDr,PatientExcelHead head)
        {
            string strRet = "";

            object obj = tmpDr.ItemArray[(int)head];

            if (obj != null)
            {
                strRet = obj.ToString();
            }

            return strRet;
        }

        public string GetClinicDataRowItem(DataRow tmpDr, ClinicExcelHead head)
        {
            string strRet = "";

            object obj = tmpDr.ItemArray[(int)head];

            if (obj != null)
            {
                strRet = obj.ToString();
            }

            return strRet;
        }

        public string GetDataRowItem(DataRow tmpDr, int nIndex)
        {
            string strRet = "";

            object obj = tmpDr.ItemArray[nIndex];

            if (obj != null)
            {
                strRet = obj.ToString();
            }

            return strRet;
        }

        [Transaction]
        public Doctor GetOrCreateDoctor(string name)
        {
            if (name.IsNullOrEmpty())
            {
                return null;
            }

            Doctor doctor = this.DoctorRepository.Get(name);

            if (doctor != null)
            {
                return doctor;
            }

            doctor = new Doctor();

            MembershipUser membershipuser = Membership.GetUser(name);

            if (membershipuser != null)
            {
                Console.WriteLine("创建{0}失败!", name);
                return null;
            }

            membershipuser = Membership.CreateUser(name, "123");

            doctor.MemberShipId = (Guid) membershipuser.ProviderUserKey;
            doctor.LoginName = name;
            doctor.CodeNo = name;
            doctor.RealName = name;
            doctor.CreateTime = membershipuser.CreationDate;
            doctor.LoginName = doctor.CodeNo;

            doctor = this.DoctorRepository.SaveOrUpdate(doctor);

            return doctor;
        }
        
        [Transaction]
        public Physical CreatePhysical(Patient patient, DataRow tmpDr)
        {
            Physical physical = new Physical();
            physical.Patient = patient;

            physical.Height = GetDataRowItem(tmpDr, PatientExcelHead.身高);
            physical.BMI = GetDataRowItem(tmpDr, PatientExcelHead.BMI);
            physical.Waistline = GetDataRowItem(tmpDr, PatientExcelHead.腰围);
            physical.Hipline = GetDataRowItem(tmpDr, PatientExcelHead.臀围);
            physical.WaistHipline = GetDataRowItem(tmpDr, PatientExcelHead.腰臀比);

            return this.PhysicalRepository.SaveOrUpdate(physical);
        }

        [Transaction]
        public Uroscopy CreateUroscopy(Patient patient, DataRow tmpDr)
        {
            String strTemp = "";

            Uroscopy uroscopy = new Uroscopy();
            uroscopy.Patient = patient;

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.酮体);
            uroscopy.Ketone = GetExInt(strTemp);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.尿糖);
            uroscopy.UrineSugar = GetExInt(strTemp);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.蛋白);
            uroscopy.UrineProtein = GetExInt(strTemp);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.符号);
            uroscopy.UCrBy = GetCompareChar(strTemp);

            uroscopy.UCrValue = GetDataRowItem(tmpDr, PatientExcelHead.尿微量白蛋白尿肌酐);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.单位);
            uroscopy.UCrUnit = GetUnit(strTemp);

            uroscopy.UrineProtein24H = GetDataRowItem(tmpDr, PatientExcelHead.小时24尿蛋白定量);

            return this.UroscopyRepository.SaveOrUpdate(uroscopy);
        }

        [Transaction]
        public Blood CreateBlood(Patient patient, DataRow tmpDr)
        {
            String strTemp = "";
            Blood blood = new Blood();
            blood.Patient = patient;

            blood.Hb = GetDataRowItem(tmpDr, PatientExcelHead.血红蛋白Hb);
            blood.HCT = GetDataRowItem(tmpDr, PatientExcelHead.红细胞压积HCT);
            blood.MCV = GetDataRowItem(tmpDr, PatientExcelHead.红细胞平均体积MCV);
            blood.RBC = GetDataRowItem(tmpDr, PatientExcelHead.红细胞计数RBC);
            blood.WBC = GetDataRowItem(tmpDr, PatientExcelHead.白细胞计数WBC);
            blood.PLT = GetDataRowItem(tmpDr, PatientExcelHead.血小板计数PLT);
            blood.XueTong = GetDataRowItem(tmpDr, PatientExcelHead.血酮);
            blood.Glu0M = GetDataRowItem(tmpDr, PatientExcelHead.GLU0min);
            blood.Glu30M = GetDataRowItem(tmpDr, PatientExcelHead.GLU30min);
            blood.Glu60M = GetDataRowItem(tmpDr, PatientExcelHead.GLU60min);
            blood.Glu120M = GetDataRowItem(tmpDr, PatientExcelHead.GLU120min);
            blood.Glu180M = GetDataRowItem(tmpDr, PatientExcelHead.GLU180min);
            blood.Insulin0M = GetDataRowItem(tmpDr, PatientExcelHead.胰岛素0min);
            blood.Insulin30M = GetDataRowItem(tmpDr, PatientExcelHead.胰岛素30min);
            blood.Insulin60M = GetDataRowItem(tmpDr, PatientExcelHead.胰岛素60min);
            blood.Insulin120M = GetDataRowItem(tmpDr, PatientExcelHead.胰岛素120min);
            blood.Insulin180M = GetDataRowItem(tmpDr, PatientExcelHead.胰岛素180min);
            blood.CTai0M = GetDataRowItem(tmpDr, PatientExcelHead.C肽0min);
            blood.CTai30M = GetDataRowItem(tmpDr, PatientExcelHead.C肽30min);
            blood.CTai60M = GetDataRowItem(tmpDr, PatientExcelHead.C肽60min);
            blood.CTai120M = GetDataRowItem(tmpDr, PatientExcelHead.C肽120min);
            blood.CTai180M = GetDataRowItem(tmpDr, PatientExcelHead.C肽180min);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.GAD);
            blood.Gad = GetExInt(strTemp);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.IAA);
            blood.Iaa = GetExInt(strTemp);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.ICA);
            blood.Ica = GetExInt(strTemp);

            blood.Cea = GetDataRowItem(tmpDr, PatientExcelHead.癌胚抗原CEA);
            blood.Afp = GetDataRowItem(tmpDr, PatientExcelHead.甲胎蛋白AFP);
            blood.Par = GetDataRowItem(tmpDr, PatientExcelHead.血小板聚集率);
            blood.Fib = GetDataRowItem(tmpDr, PatientExcelHead.纤维蛋白原);
            blood.Crp = GetDataRowItem(tmpDr, PatientExcelHead.超敏C反应蛋白CRP);
            blood.Tt3 = GetDataRowItem(tmpDr, PatientExcelHead.TT3);
            blood.Tt4 = GetDataRowItem(tmpDr, PatientExcelHead.TT4);
            blood.Ft3 = GetDataRowItem(tmpDr, PatientExcelHead.FT3);
            blood.Ft4 = GetDataRowItem(tmpDr, PatientExcelHead.FT4);
            blood.Tsh = GetDataRowItem(tmpDr, PatientExcelHead.TSH);
            blood.Trab = GetDataRowItem(tmpDr, PatientExcelHead.TR_Ab);
            blood.Tgab = GetDataRowItem(tmpDr, PatientExcelHead.TG_Ab);
            blood.Tpoab = GetDataRowItem(tmpDr, PatientExcelHead.TPO_Ab);
            blood.Tg = GetDataRowItem(tmpDr, PatientExcelHead.TG);

            return this.BloodRepository.SaveOrUpdate(blood);
        }

        [Transaction]
        public TnbBasic CreateTnbBasic(Patient patient,DataRow tmpDr)
        {
            TnbBasic tnbBasic = new TnbBasic();
            tnbBasic.Patient = patient;
            tnbBasic.Weight = GetDataRowItem(tmpDr, PatientExcelHead.体重);
            tnbBasic.BloodPressureHigh = GetDataRowItem(tmpDr, PatientExcelHead.收缩压);
            tnbBasic.BloodPressureLow = GetDataRowItem(tmpDr, PatientExcelHead.舒张压);
            tnbBasic.FBG = GetDataRowItem(tmpDr, PatientExcelHead.空腹血糖);
            tnbBasic.PBG = GetDataRowItem(tmpDr, PatientExcelHead.餐后血糖);
            tnbBasic.HbA1c = GetDataRowItem(tmpDr, PatientExcelHead.HbA1C);

            return this.TnbBasicRepository.SaveOrUpdate(tnbBasic);
        }

        [Transaction]
        public BloodRt CreateBloodRt(Patient patient, DataRow tmpDr)
        {
            BloodRt bloodRt = new BloodRt();
            bloodRt.Patient = patient;

            bloodRt.Tc = GetDataRowItem(tmpDr, PatientExcelHead.总胆固醇TC);
            bloodRt.Tg = GetDataRowItem(tmpDr, PatientExcelHead.甘油三脂TG);
            bloodRt.Hdl = GetDataRowItem(tmpDr, PatientExcelHead.高密度蛋白HDLC);
            bloodRt.LDL = GetDataRowItem(tmpDr, PatientExcelHead.低密度胆固醇LDLC);
            bloodRt.Bun = GetDataRowItem(tmpDr, PatientExcelHead.BUN);
            bloodRt.Scr = GetDataRowItem(tmpDr, PatientExcelHead.SCr);
            bloodRt.Egfr = GetDataRowItem(tmpDr, PatientExcelHead.eGFR);
            bloodRt.Ast = GetDataRowItem(tmpDr, PatientExcelHead.AST);
            bloodRt.Alt = GetDataRowItem(tmpDr, PatientExcelHead.ALT);
            bloodRt.Ggt = GetDataRowItem(tmpDr, PatientExcelHead.GGT);
            bloodRt.Ua = GetDataRowItem(tmpDr, PatientExcelHead.血尿酸UA);

            return this.BloodRtRepository.SaveOrUpdate(bloodRt);
        }

        [Transaction]
        public Eye CreateEye(Patient patient, DataRow tmpDr)
        {
            String strTemp = "";

            Eye eye = new Eye();
            eye.Patient = patient;

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.眼睛检查日期);
            eye.ExamDate = DateTimeParseExact(strTemp, "yyyy-MM-dd");

            eye.VisionL = GetDataRowItem(tmpDr, PatientExcelHead.视力左);
            eye.VisionR = GetDataRowItem(tmpDr, PatientExcelHead.视力右);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.眼睛检查类型);
            if (strTemp == "眼底镜")
            {
                eye.Fundus = 0;
            }
            else if (strTemp == "眼底摄片")
            {
                eye.Fundus = 1;
            }
            else if (strTemp == "荧光造影")
            {
                eye.Fundus = 2;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.白内障左);
            if (strTemp.Length > 0)
            {
                eye.CataractL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.白内障右);
            if (strTemp.Length > 0)
            {
                eye.CataractR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.青光眼左);
            if (strTemp.Length > 0)
            {
                eye.GlaucomaL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.青光眼右);
            if (strTemp.Length > 0)
            {
                eye.GlaucomaR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.黄斑病左);
            if (strTemp.Length > 0)
            {
                eye.MaculopathyL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.黄斑病右);
            if (strTemp.Length > 0)
            {
                eye.MaculopathyR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.视网膜病变左);
            if (strTemp.Length > 0)
            {
                eye.RetinopathyL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.视网膜病变右);
            if (strTemp.Length > 0)
            {
                eye.RetinopathyR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.激光治疗左);
            if (strTemp.Length > 0)
            {
                eye.LaserL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.激光治疗右);
            if (strTemp.Length > 0)
            {
                eye.LaserR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.其他眼疾左);
            if (strTemp.Length > 0)
            {
                eye.OtherL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.其他眼疾右);
            if (strTemp.Length > 0)
            {
                eye.OtherR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            return this.EyeRepository.SaveOrUpdate(eye);
        }

        [Transaction]
        public Legs CreateLegs(Patient patient, DataRow tmpDr)
        {
            String strTemp = "";
            Legs legs = new Legs();
            legs.Patient = patient;

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.下肢血管及神经病变筛查检查日期);
            legs.ExamDate = DateTimeParseExact(strTemp, "yyyy-MM-dd");

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.皮肤颜色左);
            if (strTemp == "正常")
            {
                legs.ColorL = 0;
            }
            else if (strTemp == "苍白")
            {
                legs.ColorL = 1;
            }
            else if (strTemp == "暗紫")
            {
                legs.ColorL = 2;
            }
            else if (strTemp == "未查")
            {
                legs.ColorL = 3;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.皮肤颜色右);
            if (strTemp == "正常")
            {
                legs.ColorR = 0;
            }
            else if (strTemp == "苍白")
            {
                legs.ColorR = 1;
            }
            else if (strTemp == "暗紫")
            {
                legs.ColorR = 2;
            }
            else if (strTemp == "未查")
            {
                legs.ColorR = 3;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.霉菌感染左);
            if (strTemp == "否")
            {
                legs.MouldL = 0;
            }
            else if (strTemp == "灰甲")
            {
                legs.MouldL = 1;
            }
            else if (strTemp == "脚气")
            {
                legs.MouldL = 2;
            }
            else if (strTemp == "未查")
            {
                legs.MouldL = 3;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.霉菌感染右);
            if (strTemp == "否")
            {
                legs.MouldR = 0;
            }
            else if (strTemp == "灰甲")
            {
                legs.MouldR = 1;
            }
            else if (strTemp == "脚气")
            {
                legs.MouldR = 2;
            }
            else if (strTemp == "未查")
            {
                legs.MouldR = 3;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.胼胝左);
            if (strTemp.Length > 0)
            {
                legs.CallusL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.胼胝右);
            if (strTemp.Length > 0)
            {
                legs.CallusR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.溃疡左);
            if (strTemp.Length > 0)
            {
                legs.AnabrosisL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.溃疡右);
            if (strTemp.Length > 0)
            {
                legs.AnabrosisR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.坏疽左);
            if (strTemp.Length > 0)
            {
                legs.GangreneL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.坏疽右);
            if (strTemp.Length > 0)
            {
                legs.GangreneR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.坏疽性质左);
            if (strTemp == "未查")
            {
                legs.GangreneTypeL = 0;
            }
            else if (strTemp == "干性")
            {
                legs.GangreneTypeL = 1;
            }
            else if (strTemp == "湿性")
            {
                legs.GangreneTypeL = 2;
            }
            else if (strTemp == "混合性")
            {
                legs.GangreneTypeL = 3;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.坏疽性质右);
            if (strTemp == "未查")
            {
                legs.GangreneTypeR = 0;
            }
            else if (strTemp == "干性")
            {
                legs.GangreneTypeR = 1;
            }
            else if (strTemp == "湿性")
            {
                legs.GangreneTypeR = 2;
            }
            else if (strTemp == "混合性")
            {
                legs.GangreneTypeR = 3;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.畸形左);
            if (strTemp.Length > 0)
            {
                legs.MalformationL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.畸形右);
            if (strTemp.Length > 0)
            {
                legs.MalformationR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.尼龙丝感觉缺失左);
            if (strTemp.Length > 0)
            {
                legs.AnesthesiaL = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.尼龙丝感觉缺失右);
            if (strTemp.Length > 0)
            {
                legs.AnesthesiaR = (YesNoUncheck)Enum.Parse(typeof(YesNoUncheck), strTemp);
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.足背动脉左);
            legs.ZubDongmaiL = GetDongMaiBD(strTemp);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.足背动脉左);
            legs.ZubDongmaiR = GetDongMaiBD(strTemp);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.胫后动脉左);
            legs.JinghDongmaiL = GetDongMaiBD(strTemp);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.胫后动脉右);
            legs.JinghDongmaiR = GetDongMaiBD(strTemp);

            legs.UpperBaspL = GetDataRowItem(tmpDr, PatientExcelHead.上肢肱动脉收缩压左);
            legs.UpperBaspR = GetDataRowItem(tmpDr, PatientExcelHead.上肢肱动脉收缩压右);
            legs.XianhAsbpL = GetDataRowItem(tmpDr, PatientExcelHead.下肢内踝动脉收缩压左);
            legs.XianhAsbpR = GetDataRowItem(tmpDr, PatientExcelHead.下肢内踝动脉收缩压右);
            legs.DpaspL = GetDataRowItem(tmpDr, PatientExcelHead.足背动脉收缩压左);
            legs.DpaspR = GetDataRowItem(tmpDr, PatientExcelHead.足背动脉收缩压右);
            legs.ABIL = GetDataRowItem(tmpDr, PatientExcelHead.ABI值);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.温度觉左);
            if (strTemp == "正常")
            {
                legs.ThermalSenseL = 0;
            }
            else if (strTemp == "异常")
            {
                legs.ThermalSenseL = 1;
            }
            else if (strTemp == "未查")
            {
                legs.ThermalSenseL = 2;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.温度觉右);
            if (strTemp == "正常")
            {
                legs.ThermalSenseR = 0;
            }
            else if (strTemp == "异常")
            {
                legs.ThermalSenseR = 1;
            }
            else if (strTemp == "未查")
            {
                legs.ThermalSenseR = 2;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.下肢血管超声左);
            if (strTemp == "正常")
            {
                legs.LivuL = 0;
            }
            else if (strTemp == "斑块")
            {
                legs.LivuL = 1;
            }
            else if (strTemp == "狭窄")
            {
                legs.LivuL = 2;
            }
            else if (strTemp == "未查")
            {
                legs.LivuL = 3;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.下肢血管超声右);
            if (strTemp == "正常")
            {
                legs.LivuR = 0;
            }
            else if (strTemp == "斑块")
            {
                legs.LivuR = 1;
            }
            else if (strTemp == "狭窄")
            {
                legs.LivuR = 2;
            }
            else if (strTemp == "未查")
            {
                legs.LivuR = 3;
            }

            return this.LegsRepository.SaveOrUpdate(legs);
        }

        [Transaction]
        public UnClassified CreateUnClassified(Patient patient, DataRow tmpDr)
        {
            String strTemp = "";
            UnClassified unClassified = new UnClassified();
            unClassified.Patient = patient;

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.皮肤颜色左);
            if (strTemp == "正常")
            {
                unClassified.ElectroCardiogram = 0;
            }
            else if (strTemp == "心律失常")
            {
                unClassified.ElectroCardiogram = 1;
            }
            else if (strTemp == "ST异常")
            {
                unClassified.ElectroCardiogram = 2;
            }
            else if (strTemp == "传导阻滞")
            {
                unClassified.ElectroCardiogram = 3;
            }
            else if (strTemp == "陈旧性心梗")
            {
                unClassified.ElectroCardiogram = 4;
            }
            else if (strTemp == "其他")
            {
                unClassified.ElectroCardiogram = 5;
            }
            else if (strTemp == "未查")
            {
                unClassified.ElectroCardiogram = 6;
            }

            unClassified.ElectroCardiogramInfo = GetDataRowItem(tmpDr, PatientExcelHead.心电图备注);

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.颈动脉血管超声左);
            if (strTemp == "正常")
            {
                unClassified.CarotidUltraL = 0;
            }
            else if (strTemp == "斑块")
            {
                unClassified.CarotidUltraL = 1;
            }
            else if (strTemp == "狭窄")
            {
                unClassified.CarotidUltraL = 2;
            }
            else if (strTemp == "未查")
            {
                unClassified.CarotidUltraL = 3;
            }
            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.颈动脉血管超声右);
            if (strTemp == "正常")
            {
                unClassified.CarotidUltraR = 0;
            }
            else if (strTemp == "斑块")
            {
                unClassified.CarotidUltraR = 1;
            }
            else if (strTemp == "狭窄")
            {
                unClassified.CarotidUltraR = 2;
            }
            else if (strTemp == "未查")
            {
                unClassified.CarotidUltraR = 3;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.头颅CT);
            if (strTemp == "正常")
            {
                unClassified.HeadCT = 0;
            }
            else if (strTemp == "腔隙性梗塞")
            {
                unClassified.HeadCT = 1;
            }
            else if (strTemp == "脑血栓")
            {
                unClassified.HeadCT = 2;
            }
            else if (strTemp == "脑出血")
            {
                unClassified.HeadCT = 3;
            }
            else if (strTemp == "未查")
            {
                unClassified.HeadCT = 4;
            }

            strTemp = GetDataRowItem(tmpDr, PatientExcelHead.脂肪肝);
            if (strTemp == "未查")
            {
                unClassified.FattyLiver = 0;
            }
            else if (strTemp == "有")
            {
                unClassified.FattyLiver = 1;
            }
            else if (strTemp == "无")
            {
                unClassified.FattyLiver = 2;
            }

            unClassified.KidneyLongL = GetDataRowItem(tmpDr, PatientExcelHead.肾脏左长);
            unClassified.KidneyWidthL = GetDataRowItem(tmpDr, PatientExcelHead.肾脏左宽);
            unClassified.KidneyHeightL = GetDataRowItem(tmpDr, PatientExcelHead.肾脏左高);
            unClassified.KidneyLongR = GetDataRowItem(tmpDr, PatientExcelHead.肾脏右长);
            unClassified.KidneyWidthR = GetDataRowItem(tmpDr, PatientExcelHead.肾脏右宽);
            unClassified.KidneyHeightR = GetDataRowItem(tmpDr, PatientExcelHead.肾脏右高);
            unClassified.BOther = GetDataRowItem(tmpDr, PatientExcelHead.B超其他);

            return this.UnClassifiedRepository.SaveOrUpdate(unClassified);
        }

        [Transaction]
        public BaseLine TryCreateBaseLine(Patient patient, DataRow tmpDr)
        {
            if (patient == null)
            {
                return null;
            }

            BaseLine baseLine = this.BaseLineRepository.GetBy(patient.Id);

            if (baseLine == null)
            {
                String strTemp = "";

                baseLine = new BaseLine();
                baseLine.Patient = patient;
                baseLine.CreateTime = DateTime.Now;
                baseLine.Doctor = patient.Doctor;
                baseLine.PracticeDoctor = GetOrCreateDoctor(GetDataRowItem(tmpDr,PatientExcelHead.填表人));

                #region identification
                
                Identification identification = new Identification();
                identification.Patient = patient;
                identification.HighestWeight = GetDataRowItem(tmpDr,PatientExcelHead.既往最高体重);

                strTemp = GetDataRowItem(tmpDr, PatientExcelHead.费用类型);
                if (strTemp.Length > 0)
                {
                    identification.PayType = (PayType)Enum.Parse(typeof(PayType), strTemp);
                }
                
                identification.Nation =
                    this.CommonCodeRepository.GetOrCreate(GetDataRowItem(tmpDr,PatientExcelHead.民族),
                        CommonCodeType.民族);
                identification.EducationalLevel =
                    this.CommonCodeRepository.GetOrCreate(GetDataRowItem(tmpDr,PatientExcelHead.教育水平),
                        CommonCodeType.教育水平);
                identification.Job =
                    this.CommonCodeRepository.GetOrCreate(GetDataRowItem(tmpDr, PatientExcelHead.职业),
                        CommonCodeType.职业);
                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.吸烟史);
                if (strTemp.Length > 0)
                {
                    identification.Smoking = (YouWuJie)Enum.Parse(typeof(YouWuJie), strTemp);
                }
                identification.SmokingYear = GetDataRowItem(tmpDr,PatientExcelHead.吸烟年);
                identification.SmokingCount = GetDataRowItem(tmpDr,PatientExcelHead.日吸烟量);

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.饮酒史);
                if (strTemp.Length > 0)
                {
                    identification.Drink = (YouWuJie)Enum.Parse(typeof(YouWuJie), strTemp);
                }
                identification.DrinkYear = GetDataRowItem(tmpDr,PatientExcelHead.饮酒年);
                identification.DrinkCapacity = GetDataRowItem(tmpDr,PatientExcelHead.周饮酒量);

                baseLine.Identification = this.IdentificationRepository.SaveOrUpdate(identification);

                #endregion

                #region medicalHistory
                MedicalHistory medicalHistory = new MedicalHistory();
                medicalHistory.Patient = patient;
                medicalHistory.GuanXinBingCourse = GetDataRowItem(tmpDr,PatientExcelHead.冠心病年);
                if (medicalHistory.GuanXinBingCourse.Length > 0 && medicalHistory.GuanXinBingCourse != "0")
                {
                    int nTmp = int.Parse(medicalHistory.GuanXinBingCourse);
                    medicalHistory.GuanXinBing = YesNoUnclear.是;
                    medicalHistory.GuanXinBingDateTime = new DateTime(DateTime.Now.Year - nTmp,DateTime.Now.Month,DateTime.Now.Day);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.心绞痛);
                if (strTemp.Length > 0)
                {
                    medicalHistory.XinJiaoTong = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.心肌梗塞);
                if (strTemp.Length > 0)
                {
                    medicalHistory.XinJiGs = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.血管再通);
                if (strTemp.Length > 0)
                {
                    medicalHistory.XueGuanZt = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.冠脉搭桥);
                if (strTemp.Length > 0)
                {
                    medicalHistory.GuanMaiDq = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.心衰);
                if (strTemp.Length > 0)
                {
                    medicalHistory.XinShuai = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.高血压);
                if (strTemp.Length > 0)
                {
                    medicalHistory.GaoXueYa = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                    if (medicalHistory.GaoXueYa == YesNoUnclear.是)
                    {
                        medicalHistory.GaoXueYaCourse = GetDataRowItem(tmpDr,PatientExcelHead.高血压病程);

                        strTemp = GetDataRowItem(tmpDr,PatientExcelHead.高血压诊断日期);

                        medicalHistory.GaoXueYaDateTime = DateTimeParseExact(strTemp, "MM dd yyyy");
                    }
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.脑出血);
                if (strTemp.Length > 0)
                {
                    medicalHistory.NaoChuXue = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.脑梗死);
                if (strTemp.Length > 0)
                {
                    medicalHistory.NaoGengSe = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.尿酸与痛风名称);
                if (strTemp.Contains("高尿酸"))
                {
                    medicalHistory.GaoNiaoSxz = YesNoUnclear.是;
                }
                if (strTemp.Contains("关节炎"))
                {
                    medicalHistory.TongFengGjy = YesNoUnclear.是;
                }
                if (strTemp.Contains("肾病"))
                {
                    medicalHistory.TongFengSb = YesNoUnclear.是;
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.原发性肾小球肾炎);
                if (strTemp.Length > 0)
                {
                    medicalHistory.ShenXiaoQy = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.蛋白尿);
                if (strTemp.Length > 0)
                {
                    medicalHistory.DanBaiNiao = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.肾病蛋白尿出现时间);
                medicalHistory.DanBaiNiaoDateTime = DateTimeParseExact(strTemp, "yyyy-MM-dd");

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.肾病血液透析);
                if (strTemp.Length > 0)
                {
                    medicalHistory.XueYeTx = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.肾病血液透析开始时间);
                medicalHistory.XueYeTxDateTime = DateTimeParseExact(strTemp, "yyyy-MM-dd");

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.肾病腹透治);
                if (strTemp.Length > 0)
                {
                    medicalHistory.FuTouZl = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.肿瘤);
                IList<MedicalHistoryItem> list1 = MedicalHistoryItemParseExact(strTemp);
                if (list1.Count > 0)
                {
                    medicalHistory.ZhongLiu = YesNo.是;
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.手术史);
                IList<MedicalHistoryItem> list2 = MedicalHistoryItemParseExact(strTemp);
                if (list2.Count > 0)
                {
                    medicalHistory.ShouShu = YesNo.是;
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.骨折史);
                IList<MedicalHistoryItem> list3 = MedicalHistoryItemParseExact(strTemp);
                if (list3.Count > 0)
                {
                    medicalHistory.GuZhe = YesNo.是;
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.踝关节以下截肢);
                if (strTemp.Length > 0)
                {
                    medicalHistory.HuaiGuanjXia = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.截肢踝以上);
                if (strTemp.Length > 0)
                {
                    medicalHistory.HuaiGuanjShang = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.下肢血管旁路再通治疗);
                if (strTemp.Length > 0)
                {
                    medicalHistory.XueGuanZl = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.眼底病变);
                if (strTemp.Length > 0)
                {
                    medicalHistory.YanDiBb = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.神经病变);
                if (strTemp.Length > 0)
                {
                    medicalHistory.ShenJingBb = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.糖尿病足);
                if (strTemp.Length > 0)
                {
                    medicalHistory.TnbZu = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.糖尿病肾病);
                if (strTemp.Length > 0)
                {
                    medicalHistory.TnbShenBing = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.糖尿病酮症酸中毒);
                if (strTemp.Length > 0)
                {
                    medicalHistory.TnbTongZd = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.非酮症高渗综合症);
                if (strTemp.Length > 0)
                {
                    medicalHistory.FeiTongZhz = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.乳酸酸中毒);
                if (strTemp.Length > 0)
                {
                    medicalHistory.RuShuanZd = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                medicalHistory.QingDxtTimes = GetDataRowItem(tmpDr,PatientExcelHead.轻度低血糖的次数);
                medicalHistory.ZhongDxtTimes = GetDataRowItem(tmpDr,PatientExcelHead.严重低血糖的次数);

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.间歇性跛行);
                if (strTemp.Length > 0)
                {
                    medicalHistory.BoXing = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.阳痿);
                if (strTemp.Length > 0)
                {
                    medicalHistory.YangWei = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.麻木);
                if (strTemp.Length > 0)
                {
                    medicalHistory.MaMu = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.视物模糊);
                if (strTemp.Length > 0)
                {
                    medicalHistory.ShiWuMh = (YesNo)Enum.Parse(typeof(YesNo), strTemp);
                }
                
                baseLine.MedicalHistory = this.MedicalHistoryRepository.SaveOrUpdate(medicalHistory);
                
                foreach (var medicalHistoryItem in list1)
                {
                    medicalHistoryItem.MedicalHistory = baseLine.MedicalHistory;
                    medicalHistoryItem.MedicalHistoryType = MedicalHistoryType.肿瘤;

                    this.MedicalHistoryItemRepository.SaveOrUpdate(medicalHistoryItem);
                }
  
                foreach (var medicalHistoryItem in list2)
                {
                    medicalHistoryItem.MedicalHistory = baseLine.MedicalHistory;
                    medicalHistoryItem.MedicalHistoryType = MedicalHistoryType.手术;

                    this.MedicalHistoryItemRepository.SaveOrUpdate(medicalHistoryItem);
                }
                
                foreach (var medicalHistoryItem in list2)
                {
                    medicalHistoryItem.MedicalHistory = baseLine.MedicalHistory;
                    medicalHistoryItem.MedicalHistoryType = MedicalHistoryType.骨折;

                    this.MedicalHistoryItemRepository.SaveOrUpdate(medicalHistoryItem);
                }

                #endregion

                #region familyHistory
                FamilyHistory familyHistory = new FamilyHistory();
                familyHistory.Patient = patient;

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.DM家族史父亲);
                if (strTemp.Length > 0)
                {
                    familyHistory.Father = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                strTemp = GetDataRowItem(tmpDr,PatientExcelHead.DM家族史母亲);
                if (strTemp.Length > 0)
                {
                    familyHistory.Mother = (YesNoUnclear)Enum.Parse(typeof(YesNoUnclear), strTemp);
                }

                familyHistory.Sibling = GetDataRowItem(tmpDr,PatientExcelHead.兄弟姐妹总数);
                familyHistory.SiblingSick = GetDataRowItem(tmpDr,PatientExcelHead.兄弟姐妹除本人外患糖尿病人数);
                familyHistory.Children = GetDataRowItem(tmpDr,PatientExcelHead.子女总数);
                familyHistory.ChildrenSick = GetDataRowItem(tmpDr,PatientExcelHead.子女患糖尿病人数);

                baseLine.FamilyHistory = this.FamilyHistoryRepository.SaveOrUpdate(familyHistory);
                #endregion
                
                baseLine.Physical = CreatePhysical(patient,tmpDr);
                baseLine.Uroscopy = CreateUroscopy(patient,tmpDr);
                baseLine.Blood = CreateBlood(patient,tmpDr);
                baseLine.BloodRt = CreateBloodRt(patient, tmpDr);
                baseLine.TnbBasic = CreateTnbBasic(patient,tmpDr);
                baseLine.Eye = CreateEye(patient,tmpDr);
                baseLine.Legs = CreateLegs(patient, tmpDr);
                baseLine.UnClassified = CreateUnClassified(patient, tmpDr);

                #region diagnoses
                Diagnoses diagnoses = new Diagnoses();
                diagnoses.Patient = patient;
                baseLine.Diagnoses = this.DiagnosesRepository.SaveOrUpdate(diagnoses);
                #endregion

                #region treatment
                Treatment treatment = new Treatment();
                treatment.Patient = patient;
                baseLine.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);
                #endregion

                //保存基线资料
                baseLine = this.BaseLineRepository.SaveOrUpdate(baseLine);
            }

            return baseLine;
        }

        [Transaction]
        public void SetFollowUpInfo(Patient patient, DataRow tmpDr)
        {
            String strTemp = GetDataRowItem(tmpDr,PatientExcelHead.下次随访时间);

            var dateTime = DateTimeParseExact(strTemp, "yyyy-MM-dd");

            FollowUpInfo followUpInfo = this.FollowUpInfoRepository.GetBy(patient.Id);
            if (followUpInfo == null)
            {
                this.FollowUpInfoRepository.SaveOrUpdate(new FollowUpInfo()
                {
                    Patient = patient,
                    Doctor = patient.Doctor,
                    FollowUpDate = dateTime,
                    ModifyTime = DateTime.Now
                });
            }
            else
            {
                if (followUpInfo.FollowUpDate < dateTime)
                {
                    followUpInfo.FollowUpDate = dateTime;
                    this.FollowUpInfoRepository.SaveOrUpdate(followUpInfo);
                }
            }
        }

        [Transaction]
        public FuZhen FuZhenAdd(Patient patient, DataRow tmpDr)
        {
            var fuzhen = new FuZhen();
            fuzhen.Name = GetDataRowItem(tmpDr, PatientExcelHead.记录名称);
            fuzhen.Patient = patient;
            fuzhen.Doctor = GetOrCreateDoctor(GetDataRowItem(tmpDr, PatientExcelHead.医生));
            fuzhen.PracticeDoctor = GetOrCreateDoctor(GetDataRowItem(tmpDr, PatientExcelHead.填表人));

            String strTemp = GetDataRowItem(tmpDr,PatientExcelHead.填表时间);
            fuzhen.CreateTime = DateTimeParseExact(strTemp, "yyyy-MM-dd");

            fuzhen.TnbBasic = CreateTnbBasic(patient, tmpDr);

            fuzhen.BloodRt = CreateBloodRt(patient, tmpDr);

            Treatment treatment = new Treatment();
            treatment.Patient = patient;
            fuzhen.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);

            return this.FuZhenRepository.SaveOrUpdate(fuzhen);
        }

        [Transaction]
        public TiaoZheng TiaoZhengAdd(Patient patient, DataRow tmpDr)
        {
            var tiaozheng = new TiaoZheng();
            tiaozheng.Name = GetDataRowItem(tmpDr, PatientExcelHead.记录名称);
            tiaozheng.Patient = patient;
            tiaozheng.Doctor = GetOrCreateDoctor(GetDataRowItem(tmpDr, PatientExcelHead.医生));
            tiaozheng.PracticeDoctor = GetOrCreateDoctor(GetDataRowItem(tmpDr, PatientExcelHead.填表人));

            String strTemp = GetDataRowItem(tmpDr,PatientExcelHead.填表时间);
            tiaozheng.CreateTime = DateTimeParseExact(strTemp, "yyyy-MM-dd");

            tiaozheng.TnbBasic = CreateTnbBasic(patient, tmpDr);
            
            Treatment treatment = new Treatment();
            treatment.Patient = patient;
            tiaozheng.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);

            return this.TiaoZhengRepository.SaveOrUpdate(tiaozheng);
        }

        [Transaction]
        public Annual AnnualAdd(Patient patient, DataRow tmpDr)
        {
            var annual = new Annual();
            annual.Name = GetDataRowItem(tmpDr, PatientExcelHead.记录名称);
            annual.Patient = patient;
            annual.Doctor = GetOrCreateDoctor(GetDataRowItem(tmpDr, PatientExcelHead.医生));
            annual.PracticeDoctor = GetOrCreateDoctor(GetDataRowItem(tmpDr, PatientExcelHead.填表人));

            String strTemp = GetDataRowItem(tmpDr, PatientExcelHead.填表时间);
            annual.CreateTime = DateTimeParseExact(strTemp, "yyyy-MM-dd");

            annual.TnbBasic = CreateTnbBasic(patient, tmpDr);
            annual.Physical = CreatePhysical(patient, tmpDr);
            annual.Uroscopy = CreateUroscopy(patient, tmpDr);
            annual.Blood = CreateBlood(patient, tmpDr);
            annual.BloodRt = this.CreateBloodRt(patient, tmpDr);
            annual.Eye = CreateEye(patient, tmpDr);
            annual.Legs = CreateLegs(patient, tmpDr);
            annual.UnClassified = CreateUnClassified(patient, tmpDr);

            Diagnoses diagnoses = new Diagnoses();
            diagnoses.Patient = patient;
            annual.Diagnoses = this.DiagnosesRepository.SaveOrUpdate(diagnoses);

            Treatment treatment = new Treatment();
            treatment.Patient = patient;
            annual.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);

            return this.AnnualRepository.SaveOrUpdate(annual);
        }
        
        public DateTime DateTimeParseExact(string strDate, string format)
        {
            try
            {
                return DateTime.ParseExact(strDate.Trim(), format, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return DateTimeEx.Default();
            }
        }

        public IList<MedicalHistoryItem> MedicalHistoryItemParseExact(string strData)
        {
            var result = new List<MedicalHistoryItem>();

            if (strData.IsNullOrEmpty())
            {
                return result;
            }

            string[] strArr = strData.Split('|');

            foreach (var s in strArr)
            {
                if (s.Length > 3)
                {
                    

                    string[] sArr = s.Split('，');

                    if (sArr.Length >= 3)
                    {
                        MedicalHistoryItem item = new MedicalHistoryItem();

                        string[] sTmp = sArr[0].Split('：');
                        if (sTmp.Length >= 2)
                        {
                            item.Name = sTmp[1];
                        }

                        sTmp = sArr[1].Split('：');
                        if (sTmp.Length >= 2)
                        {
                            item.Note = sTmp[1];
                        }

                        sTmp = sArr[2].Split('：');
                        if (sTmp.Length >= 2)
                        {
                            item.DateTime = DateTimeParseExact(sTmp[1], "yyyy-MM-dd HH:mm:ss");
                        }

                        result.Add(item);
                    }


                }
            }


            return result;
        }

        public IList<Medicate> GetMedicates(Patient patient, DataRow tmpDr, DataRow headRow)
        {
            List<Medicate> list = new List<Medicate>();

            int nIndex = (int) MedicateHead.填表时间;
            nIndex++;

            int nLength = tmpDr.ItemArray.Length;

            while (nIndex < nLength)
            {
                String strTmp = GetDataRowItem(tmpDr, nIndex);
                if (strTmp.IsNullOrEmpty())
                {
                    nIndex += 4;
                }
                else
                {
                    Medicate medicate = new Medicate();
                    medicate.Medicine = new Medicine();
                    medicate.Medicine.ChemicalName = strTmp;
                    medicate.Medicine.NormalName = GetDataRowItem(headRow, nIndex);
                    
                    nIndex++;
                    if (nIndex >= nLength)
                        break;
                    strTmp = GetDataRowItem(tmpDr, nIndex);
                    medicate.Usage = (Usage)Enum.Parse(typeof (Usage), strTmp);

                    nIndex++;
                    if (nIndex >= nLength)
                        break;
                    strTmp = GetDataRowItem(tmpDr, nIndex);
                    medicate.Dosage = strTmp;

                    nIndex++;
                    if (nIndex >= nLength)
                        break;
                    strTmp = GetDataRowItem(tmpDr, nIndex);
                    medicate.Note = strTmp;

                    medicate.DoseType = DoseType.口服;

                    list.Add(medicate);

                    nIndex++;
                }
            }

            return list;
        }
        
        public int GetExInt(string name)
        {
            int nRet = -1;

            if (name.IsNullOrEmpty())
            {
                return nRet;
            }

            name = name.Trim();
            if (name == "未查")
            {
                nRet = 0;
            }
            else if (name == "(-)")
            {
                nRet = 1;
            }
            else if (name == "+")
            {
                nRet = 2;
            }
            else if (name == "++")
            {
                nRet = 3;
            }
            else if (name == "+++")
            {
                nRet = 4;
            }

            return nRet;
        }

        public int GetCompareChar(string name)
        {
            int nRet = -1;

            if (name.IsNullOrEmpty())
            {
                return nRet;
            }

            if (name == "<")
            {
                nRet = 0;
            }
            else if (name == "+")
            {
                nRet = 1;
            }
            else if (name == ">")
            {
                nRet = 2;
            }

            return nRet;
        }

        public int GetUnit(string name)
        {
            int nRet = -1;

            if (name.IsNullOrEmpty())
            {
                return nRet;
            }

            if (name == "ug/min")
            {
                nRet = 0;
            }
            else if (name == "mg/g")
            {
                nRet = 1;
            }
            else if (name == "mg/24h")
            {
                nRet = 2;
            }
            else if (name == "mg/mmol")
            {
                nRet = 3;
            }

            return nRet;
        }
        
        public int GetDongMaiBD(string name)
        {
            int nRet = -1;

            if (name.IsNullOrEmpty())
            {
                return nRet;
            }

            if (name == "存在")
            {
                nRet = 0;
            }
            else if (name == "消失")
            {
                nRet = 1;
            }
            else if (name == "未查")
            {
                nRet = 2;
            }

            return nRet;
        }

    }

    public enum PatientExcelHead
    {
        类别,
        记录名称,
        姓名,
        性别,
        出生年月,
        年龄,
        所在省份,
        固定电话,
        手机号码,
        教育水平,
        职业,
        民族,
        籍贯,
        身份证号码,
        医院编号,
        就诊医院,
        患者编号,
        患者原始编号,
        住院号,
        费用类型,
        诊断日期,
        糖尿病病程,
        糖尿病类型,
        吸烟史,
        吸烟年,
        日吸烟量,
        饮酒史,
        饮酒年,
        周饮酒量,
        既往最高体重,
        冠心病年,
        心绞痛,
        心肌梗塞,
        血管再通,
        冠脉搭桥,
        心衰,
        高血压,
        高血压诊断日期,
        高血压病程,
        脑出血,
        脑梗死,
        尿酸与痛风名称,
        原发性肾小球肾炎,
        蛋白尿,
        肾病蛋白尿出现时间,
        肾病血液透析,
        肾病血液透析开始时间,
        肾病腹透治,
        肾病腹透治疗开始时间,
        肿瘤,
        手术史,
        骨折史,
        踝关节以下截肢,
        截肢踝以上,
        下肢血管旁路再通治疗,
        眼底病变,
        神经病变,
        糖尿病足,
        糖尿病肾病,
        糖尿病酮症酸中毒,
        非酮症高渗综合症,乳酸酸中毒,
        轻度低血糖的次数,严重低血糖的次数,血糖值低于39次数,
        间歇性跛行,阳痿,麻木,
        视物模糊,DM家族史父亲,
        DM家族史母亲,兄弟姐妹除本人外患糖尿病人数,兄弟姐妹总数,子女患糖尿病人数,子女总数,
        身高,体重,BMI,腰围,臀围,腰臀比,收缩压,舒张压,酮体,
        尿糖,蛋白,符号,尿微量白蛋白尿肌酐,
        单位,小时24尿蛋白定量,
        血红蛋白Hb,红细胞压积HCT,
        红细胞平均体积MCV,红细胞计数RBC,
        白细胞计数WBC,血小板计数PLT,
        空腹血糖,餐后血糖, HbA1C, 
        血酮, 
        GLU0min, GLU30min,GLU60min, GLU120min, GLU180min,
        胰岛素0min,胰岛素30min,胰岛素60min,胰岛素120min,胰岛素180min,
        C肽0min,C肽30min,C肽60min,C肽120min,C肽180min,
        GAD,IAA,ICA,总胆固醇TC,甘油三脂TG,高密度蛋白HDLC,低密度胆固醇LDLC,BUN,SCr,eGFR,AST,ALT,GGT,
        血尿酸UA,癌胚抗原CEA,甲胎蛋白AFP ,血小板聚集率,纤维蛋白原,超敏C反应蛋白CRP,
        TT3,TT4,FT3,FT4,TSH,TR_Ab,TG_Ab,TPO_Ab ,TG,
        眼睛检查日期,视力左,视力右,眼睛检查类型,白内障左,白内障右,青光眼左,青光眼右,黄斑病左,黄斑病右,视网膜病变左,视网膜病变右,激光治疗左,激光治疗右,其他眼疾左,其他眼疾右,
        下肢血管及神经病变筛查检查日期,皮肤颜色左,皮肤颜色右,霉菌感染左,霉菌感染右,胼胝左,胼胝右,
        溃疡左,溃疡位置左,溃疡右,溃疡位置右,
        坏疽左, 坏疽位置左, 坏疽性质左, 坏疽右, 坏疽位置右,坏疽性质右, 
        畸形左, 畸形右, 尼龙丝感觉缺失左, 尼龙丝感觉缺失右,
        足背动脉左,足背动脉右,胫后动脉左,胫后动脉右,上肢肱动脉收缩压左,上肢肱动脉收缩压右,下肢内踝动脉收缩压左,下肢内踝动脉收缩压右,
        足背动脉收缩压左,足背动脉收缩压右,ABI值,
        足趾血压左,足趾血压右,TPI值左,TPI值右,
        第一足趾左,第一足趾右,内踝左,内踝右,温度觉左,温度觉右,
        下肢血管超声左, 下肢血管超声百分比左, 下肢血管超声右, 下肢血管超声百分比右,心电图, 心电图备注, 颈动脉血管超声左,颈动脉血管超声百分比左, 颈动脉血管超声右,颈动脉血管超声百分比右,颈动脉血管超声其他,
        头颅CT,脂肪肝,肾脏左长 ,肾脏左宽,肾脏左高,肾脏右长,肾脏右宽,肾脏右高,B超其他,其他检查,医疗结论,
        病程阶段, 饮食运动治疗, 药物治疗方案, 其他药物治疗,其他备注,血糖检测方案调整, 下次随访时间, 医生, 填表人, 填表时间
    }

    public enum ClinicExcelHead
    {
        身份证号码,病种标识,事件名称,登记时间,是否健在,死亡时间,死亡原因,目前状态,最后一次联系日期,
        发生轻度血糖次数,发生严重低血糖次数,DKA,DKA发生时间,HNKC,HNKC发生时间,乳酸中毒,乳酸中毒发生时间,心绞痛,
        心绞痛发生时间,心肌梗死,心肌梗死发生时间,心衰,心衰发生时间,CABG,CABG发生时间,血管再通,血管再通发生时间,TIA,TIA发生时间,
        脑梗,脑梗发生时间,脑出血,脑出血发生时间,肿瘤,肿瘤发生时间,肿瘤部位,合并肾病,合并肾病发生时间,透析,透析发生时间,移植,移植发生时间,
        视网膜病变,视网膜病变发生时间,失明,失明发生时间,视力减退,视力减退发生时间,糖尿病足,糖尿病足发生时间,病足破口,病足破口发生时间,病足溃疡,
        病足溃疡发生时间,是否住院,住院时间,住院原因,绑定随访
    }

    public enum MedicateHead
    {
        记录名称,姓名,填表时间
    }
}
