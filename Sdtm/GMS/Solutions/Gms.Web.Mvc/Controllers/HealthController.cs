using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Gms.Common;
using Gms.Domain;
using Gms.Domain.Health;
using MvcContrib.EnumerableExtensions;
using SharpArch.NHibernate.Web.Mvc;

namespace Gms.Web.Mvc.Controllers
{
    public class HealthController : BaseController
    {

        #region 医疗诊断
        public ActionResult Ylzd(int id)
        {
            Diagnoses diagnoses = this.DiagnosesRepository.Get(id);
            return View(diagnoses);
        }

        public ActionResult DiseaseList(int diagnosesid)
        {
            Diagnoses diagnoses = this.DiagnosesRepository.Get(diagnosesid);

            var list = diagnoses.Diseases.Select(c => DiseaseModel.From(c)).ToList();

            return Json(list);
        }

        [Transaction]
        public ActionResult SaveOrUpdateDiagnoses(int diagnosesid, string diagnoseslist,int? diabetesid, DiseaseStage stage)
        {
            try
            {
                var jser = new JavaScriptSerializer();
                IList<DiseaseModel> datas = jser.Deserialize<IList<DiseaseModel>>(diagnoseslist);

                Diagnoses diagnoses = this.DiagnosesRepository.Get(diagnosesid);
                if (diagnoses == null)
                {
                    throw new Exception("信息不完整，请刷新后再试！");
                }
     
                //去掉没有的
                for (int i = diagnoses.Diseases.Count - 1; i >= 0; i--)
                {
                    var item1 = diagnoses.Diseases[i];

                    bool bFlag = false;

                    for (int j = datas.Count - 1; j >= 0; j--)
                    {
                        var item2 = datas[j];

                        if (item1.Id == item2.Id)
                        {
                            bFlag = true;

                            datas.Remove(item2);
                        }
                    }

                    if (!bFlag)
                    {
                        diagnoses.Diseases.Remove(item1);
                    }
                }

                //添加新增的
                foreach (var item in datas)
                {
                    diagnoses.Diseases.Add(this.DiseaseRepository.Get(item.Id));
                }

                if (diabetesid.HasValue)
                {
                    diagnoses.Diabetes = this.CommonCodeRepository.Get(diabetesid.Value);
                }
                else
                {
                    diagnoses.Diabetes = diagnoses.Patient.Diabetes;
                }
                
                diagnoses.DiseaseStage = stage;

                diagnoses = this.DiagnosesRepository.SaveOrUpdate(diagnoses);

               
                //同时修改患者资料
                Patient patient = this.PatientRepository.Get(diagnoses.Patient.Id);
                patient.Diabetes = diagnoses.Diabetes;
                patient.DiseaseStage = diagnoses.DiseaseStage;
                
                this.PatientRepository.SaveOrUpdate(patient);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess();
        }
 

        //[Transaction]
        //public ActionResult AddDisease(int diagnosesid,int diseaseid)
        //{
        //    Diagnoses diagnoses = this.DiagnosesRepository.Get(diagnosesid);
        //    Disease disease = this.DiseaseRepository.Get(diseaseid);
        //    if (diagnoses == null || disease == null )
        //    {
        //        return JsonError("添加的信息不正确");
        //    }

        //    if (diagnoses.Diseases.Contains(disease))
        //        return JsonSuccess();

        //    diagnoses.Diseases.Add(disease);

        //    this.DiagnosesRepository.SaveOrUpdate(diagnoses);
            
        //    return JsonSuccess();
        //}

        //[Transaction]
        //public ActionResult DeleteDisease(int diagnosesid,int diseaseid)
        //{
        //    Diagnoses diagnoses = this.DiagnosesRepository.Get(diagnosesid);
        //    Disease disease = this.DiseaseRepository.Get(diseaseid);
        //    if (diagnoses == null || disease == null)
        //    {
        //        return JsonError("信息不正确");
        //    }

        //    if (diagnoses.Diseases.Contains(disease))
        //    {
        //        diagnoses.Diseases.Remove(disease);
        //    }

        //    this.DiagnosesRepository.SaveOrUpdate(diagnoses);

        //    return JsonSuccess();
        //}

        //[Transaction]
        //public ActionResult UpdateDiabetes(int diagnosesid,int diabetesid)
        //{
        //    Diagnoses diagnoses = this.DiagnosesRepository.Get(diagnosesid);
        //    CommonCode diabetes = this.CommonCodeRepository.Get(diabetesid);
        //    if (diagnoses == null || diabetes== null)
        //    {
        //        return JsonError("添加的信息不正确");
        //    }

        //    diagnoses.Diabetes = diabetes;

        //    this.DiagnosesRepository.SaveOrUpdate(diagnoses);

        //    return JsonSuccess();
        //}

        //[Transaction]
        //public ActionResult UpdateDiseaseStage(int diagnosesid,DiseaseStage stage)
        //{
        //    Diagnoses diagnoses = this.DiagnosesRepository.Get(diagnosesid);
            
        //    if (diagnoses == null)
        //    {
        //        return JsonError("添加的信息不正确");
        //    }

        //    diagnoses.DiseaseStage = stage;

        //    this.DiagnosesRepository.SaveOrUpdate(diagnoses);

        //    return JsonSuccess();
        //}

        #endregion

        #region 诊疗方案
        public ActionResult Zlfa(int id)
        {
            Treatment treatment = this.TreatmentRepository.Get(id);
            return View(treatment);
        }

        public ActionResult MedicateList(int treatmentid)
        {
            Treatment treatment = this.TreatmentRepository.Get(treatmentid);

            var list = treatment.Medicates.Select(c => MedicateModel.From(c)).ToList();

            return Json(list);
        }

        [Transaction]
        public ActionResult SaveOrUpdateTreatment(int treatmentid, string medicates, string other, string sport, string note)
        {
            try
            {
                var jser = new JavaScriptSerializer();
                IList<MedicateModel> datas = jser.Deserialize<IList<MedicateModel>>(medicates);

                Treatment treatment = this.TreatmentRepository.Get(treatmentid);
                if (treatment == null)
                {
                    throw new Exception("信息不完整，请刷新后再试！");
                }

                IList<Medicate> dts = new List<Medicate>();

                //找到存在的进行更新
                foreach (var item1 in treatment.Medicates)
                {
                    for (int i = datas.Count-1; i >= 0; i--)
                    {
                        var item2 = datas[i];
                  
                        if (item1.Medicine.Id == item2.Id)
                        {
                            item1.Dosage = item2.Dosage;
                            item1.Usage = (Usage) Enum.Parse(typeof (Usage), item2.UsageName);
                            //item1.StartDateTime = DateTime.Parse(item2.StartDateTimeString);
                            item1.Note = item2.Note;

                            dts.Add(item1);
                            datas.Remove(item2);
                        }
                    }
               }

                //添加新增的
                foreach (var item in datas)
                {
                    dts.Add(new Medicate()
                    {
                        Medicine = this.MedicineRepository.Get(item.Id),
                        Dosage = item.Dosage,
                        Usage = (Usage)Enum.Parse(typeof(Usage), item.UsageName),
                        //StartDateTime = DateTime.Parse(item.StartDateTimeString),
                        Note = item.Note
                    });
                }
                
                //循环保存诊疗方案
                treatment.Medicates.Clear();
                foreach (var item in dts)
                {
                    treatment.Medicates.Add(this.MedicateRepository.SaveOrUpdate(item));
                }

                treatment.Other = other;
                treatment.Sport = sport;
                treatment.Note = note;

                this.TreatmentRepository.SaveOrUpdate(treatment);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
 
            return JsonSuccess();
        }

        public ActionResult GetDoseType()
        {
            var data = (from Enum e in Enum.GetValues(typeof(DoseType))
                        select new
                        {
                            DoseTypeValue = e.ToString(),//Int32.Parse(Enum.Format(typeof(Usage), e, "d")),
                            DoseTypeName = e.ToString()
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 临床事件
        public ActionResult Clinic(int id)
        {
            var patient = this.PatientRepository.Get(id);
            return View(patient);
        }

        public ActionResult ClinicList(ClinicQuery query)
        {
            var list = this.ClinicRepository.GetList(query);

            var data = list.Data.Select(c => ClinicModel.From(c)).ToList(); 

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        public ActionResult ClinicDetail(int clinicid)
        {
            var clinic = this.ClinicRepository.Get(clinicid);
            
            return View(clinic);
        }

        public ActionResult ClinicPingGu(int clinicid)
        {
            var clinic = this.ClinicRepository.Get(clinicid);

            return View(clinic);
        }

        [Transaction]
        public ActionResult ClinicAdd(int patientid,string name)
        {
            var patient = this.PatientRepository.Get(patientid);

            var clinic = new Clinic();
            clinic.Patient = patient;

            clinic.DeathTime = DateTimeEx.Default();
            clinic.DkaDateTime = DateTimeEx.Default();
            clinic.HnkcDateTime = DateTimeEx.Default();
            clinic.RuShuanZdDateTime = DateTimeEx.Default();
            clinic.XinJiaoTongDateTime = DateTimeEx.Default();
            clinic.XinJiGsDateTime = DateTimeEx.Default();
            clinic.XinShuaiDateTime = DateTimeEx.Default();
            clinic.CabgDateTime = DateTimeEx.Default();
            clinic.XueGuanZtDateTime = DateTimeEx.Default();
            clinic.TiaDateTime = DateTimeEx.Default();
            clinic.NaoChuXueDateTime = DateTimeEx.Default();
            clinic.NaoGengSeDateTime = DateTimeEx.Default();
            clinic.ZhongLiuDateTime = DateTimeEx.Default();
            clinic.TouXiDateTime = DateTimeEx.Default();
            clinic.YiZhiDateTime = DateTimeEx.Default();
            clinic.TnbZuDateTime = DateTimeEx.Default();
            clinic.TnbZuPkDateTime = DateTimeEx.Default();
            clinic.TnbZuKyDateTime = DateTimeEx.Default();
            clinic.TnbShenBingDateTime = DateTimeEx.Default();
            clinic.TnbSwmDateTime = DateTimeEx.Default();
            clinic.ShiMingDateTime = DateTimeEx.Default();
            clinic.ShiLiJtDateTime = DateTimeEx.Default();
            clinic.InHospitalDateTime = DateTimeEx.Default();
            clinic.CreateTime = DateTime.Now;
            if (name.IsNullOrEmpty())
            {
                clinic.Name = clinic.CreateTime.ToString("yyyy-MM-dd") + "临床事件";
            }
            else
            {
                clinic.Name = name;
            }
            

            clinic = this.ClinicRepository.SaveOrUpdate(clinic);

            return JsonSuccess(clinic);
        }

        [Transaction]
        public ActionResult ClinicDelete(int clinicid)
        {
            try
            {
                var obj = this.ClinicRepository.Get(clinicid);

                this.ClinicRepository.Delete(obj);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }


        [Transaction]
        public ActionResult ClinicSaveOrUpdate(Clinic clinic)
        {
            try
            {
                if (clinic == null)
                {
                    throw new Exception("信息不完整，请刷新后再试！");
                }

                clinic = this.ClinicRepository.Get(clinic.Id);

                TryUpdateModel(clinic);

                clinic = this.ClinicRepository.SaveOrUpdate(clinic);

                //更新患者的随访状态20150906
                var followUpInfo = this.FollowUpInfoRepository.GetBy(clinic.Patient.Id);
                followUpInfo.FollowUpStatus = clinic.FollowUpStatus;
                this.FollowUpInfoRepository.SaveOrUpdate(followUpInfo);

            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess();
        }

        [Transaction]
        public ActionResult ClinicUpdateName(int clinicid, string name)
        {
            try
            {
                var obj = this.ClinicRepository.Get(clinicid);

                if (obj != null && name != null && name.Length > 0)
                {
                    obj.Name = name;
                    this.ClinicRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #endregion

        #region 健康教育

        public ActionResult Education(int id)
        {
            var healthEducation = GetOrCreateHealthEducation(id);

            return View(healthEducation);
        }

        [Transaction]
        public HealthEducation GetOrCreateHealthEducation(int patientid)
        {
            var healthEducation = this.HealthEducationRepository.GetBy(patientid);

            if (healthEducation == null)
            {
                healthEducation = new HealthEducation();
                healthEducation.Patient = this.PatientRepository.Get(patientid);

                healthEducation = this.HealthEducationRepository.SaveOrUpdate(healthEducation);
            }

            return healthEducation;
        }

        public ActionResult EducationList(EducationQuery query)
        {
            var list = this.EducationRepository.GetList(query);

            var data = list.Data.Select(c => EducationModel.From(c)).ToList(); ;

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        public ActionResult EducationEdit(int id,int? patientid)
        {
            var education = this.EducationRepository.Get(id);

            if (education == null && patientid.HasValue)
            {
                education = new Education();
                education.Patient = this.PatientRepository.Get(patientid.Value);
            }

            return View(education);
        }

        [Transaction]
        public ActionResult EducationSaveOrUpdate(Education education, int[] checkForEducationFlag)
        {
            if (checkForEducationFlag == null)
            {
                return JsonError("教育内容为必选项（至少选择1项）");
            }

            try
            {
                if (education == null)
                {
                    throw new Exception("信息不完整，请刷新后再试！");
                }
                
                if (education.Id > 0)
                {
                    education = this.EducationRepository.Get(education.Id);

                    TryUpdateModel(education);
                }
                
                HealthEducation healthEducation = this.HealthEducationRepository.GetBy(education.Patient.Id);

                education.EducationFlag = 0;
                foreach (var flag in checkForEducationFlag)
                {
                    education.EducationFlag |= flag;
                    healthEducation.EducationFlag |= flag;
                }

                if (healthEducation.YingYangShi != YesNoUnclear.是)
                {
                    healthEducation.YingYangShi = education.YingYangShi;
                }

                if (healthEducation.HuShi != YesNoUnclear.是)
                {
                    healthEducation.HuShi = education.HuShi;
                }

                if (healthEducation.ZuBing != YesNoUnclear.是)
                {
                    healthEducation.ZuBing = education.ZuBing;
                }

                this.HealthEducationRepository.SaveOrUpdate(healthEducation);
                education = this.EducationRepository.SaveOrUpdate(education);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess(education);
        }
        
        [Transaction]
        public ActionResult EducationDelete(int id)
        {
            try
            {
                var obj = this.EducationRepository.Get(id);
                int patientid = obj.Patient.Id;

                this.EducationRepository.Delete(obj);

                var healthEduction = this.HealthEducationRepository.GetBy(patientid);

                var list = this.EducationRepository.GetList(new EducationQuery() {PatientId = patientid});

                healthEduction.EducationFlag = 0;
                healthEduction.YingYangShi = YesNoUnclear.否;
                healthEduction.HuShi = YesNoUnclear.否;
                healthEduction.ZuBing = YesNoUnclear.否;
                foreach (var item in list.Data)
                {
                    healthEduction.EducationFlag |= item.EducationFlag;

                    if (item.YingYangShi == YesNoUnclear.是)
                    {
                        healthEduction.YingYangShi = YesNoUnclear.是;
                    }
                    if (item.HuShi == YesNoUnclear.是)
                    {
                        healthEduction.HuShi = YesNoUnclear.是;
                    }
                    if (item.ZuBing == YesNoUnclear.是)
                    {
                        healthEduction.ZuBing = YesNoUnclear.是;
                    }
                }

                this.HealthEducationRepository.SaveOrUpdate(healthEduction);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #endregion

        #region 糖尿病评估相关量表

        public ActionResult EvaScale(int id)
        {
            var patient = this.PatientRepository.Get(id);

            return View(patient);
        }

        public ActionResult EvaScaleList(EvaluationScaleQuery query)
        {
            var list = this.EvaluationScaleRepository.GetList(query);

            var data = list.Data.Select(c => EvaScaleModel.From(c)).ToList(); ;

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        public ActionResult EvaScaleEdit(int id, int? patientid,ScaleType? type)
        {
            var scale = this.EvaluationScaleRepository.Get(id);

            if (scale == null)
            {
                scale = new EvaluationScale();

                if (patientid.HasValue && type.HasValue)
                {
                    scale.Patient = this.PatientRepository.Get(patientid.Value);
                    scale.ScaleType = type.Value;
                }
            }

            if (scale.ScaleType == ScaleType.简易智能状态评定表MMSE)
            {
                return View("EvaScaleMMSE",scale);
            }
            
            if (scale.ScaleType == ScaleType.Zung焦虑自我评价表)
            {
                return View("EvaScaleZungJL", scale);
            }
            
            return View("EvaScaleZungYY", scale);
        }
        
        [Transaction]
        public ActionResult EvaScaleSaveOrUpdate(EvaluationScale scale, int[] scaleresult, int? scaletotal)
        {
            if (scaleresult == null || !scaletotal.HasValue)
            {
                return JsonError("评估内容为必选项（至少选择1项）");
            }

            try
            {
                if (scale == null)
                {
                    throw new Exception("信息不完整，请刷新后再试！");
                }

                if (scale.Id > 0)
                {
                    scale = this.EvaluationScaleRepository.Get(scale.Id);

                    TryUpdateModel(scale);
                }

                scale.ScaleName = scale.ScaleType.ToString();
                
                scale.Result = scaletotal.Value;

                scale.Answers = "";
                foreach (var result in scaleresult)
                {
                    scale.Answers += result.ToString();
                    scale.Answers += "|";
                }

                scale = this.EvaluationScaleRepository.SaveOrUpdate(scale);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess(scale);
        }

        [Transaction]
        public ActionResult EvaScaleDelete(int id)
        {
            try
            {
                var obj = this.EvaluationScaleRepository.Get(id);

                this.EvaluationScaleRepository.Delete(obj);
        
                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #endregion
    }

    public class MedicateModel:MedicineModel
    {
        public MedicateModel()
        {

        }

        public MedicateModel(Medicate medicate):base(medicate.Medicine)
        {
            this.MedicateId = medicate.Id;
            this.DoseTypeName = Enum.GetName(typeof(DoseType), medicate.DoseType);
            this.Dosage = medicate.Dosage;
            this.UsageName = Enum.GetName(typeof(Usage), medicate.Usage);
            this.StartDateTimeString = medicate.StartDateTime.ToString("yyyy-M-d");
            this.Note = medicate.Note;
        }

        public int MedicateId { get; set; }

        /// <summary>
        /// 给药途径
        /// </summary>
        public String DoseTypeName { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public String Dosage { get; set; }

        /// <summary>
        /// 用法
        /// </summary>
        public String UsageName { get; set; }

        /// <summary>
        /// 开始用药时间
        /// </summary>
        public String StartDateTimeString { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Note { get; set; }

        public static MedicateModel From(Medicate medicate)
        {
            return new MedicateModel(medicate);
        }
    }

    public class ClinicModel
    {
        public ClinicModel(Clinic clinic)
        {
            this.ClinicId = clinic.Id;
            this.Name = clinic.Name;
            this.IsBreathing = clinic.IsBreathing.ToString();


            this.Note = "";
            this.Note += clinic.Dka ? "DKA|" : "";
            this.Note += clinic.Hnkc ? "HNKC|" : "";
            this.Note += clinic.RuShuanZd ? "乳酸酸中毒|" : "";
            this.Note += clinic.XinJiaoTong ? "心绞痛|" : "";
            this.Note += clinic.XinJiGs ? "心肌梗塞|" : "";
            this.Note += clinic.XinShuai ? "心衰|" : "";
            this.Note += clinic.Cabg ? "CABG|" : "";
            this.Note += clinic.XueGuanZt ? "血管再通|" : "";
            this.Note += clinic.Tia ? "TIA|" : "";
            this.Note += clinic.NaoChuXue ? "脑出血|" : "";
            this.Note += clinic.NaoGengSe ? "脑梗塞|" : "";
            this.Note += clinic.ZhongLiu ? "肿瘤|" : "";
            this.Note += clinic.TouXi ? "透析|" : "";
            this.Note += clinic.YiZhi ? "移植|" : "";
            this.Note += clinic.TnbZu ? "糖尿病足|" : "";
            this.Note += clinic.TnbZuPk ? "糖尿病足(破口)|" : "";
            this.Note += clinic.TnbZuKy ? "糖尿病足(溃疡)|" : "";
            this.Note += clinic.TnbShenBing ? "糖尿病肾病|" : "";
            this.Note += clinic.TnbSwm ? "糖尿病视网膜病变|" : "";
            this.Note += clinic.ShiMing ? "失明|" : "";
            this.Note += clinic.ShiLiJt ? "视力减退|" : "";

            this.CreateTimeString = clinic.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public int ClinicId { get; set; }
        
        /// <summary>
        /// 事件名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 是否健在
        /// </summary>
        public String IsBreathing { get; set; }
        
        /// <summary>
        /// 并发症、特殊事件或新的疾病
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 并发症、登记时间
        /// </summary>
        public String CreateTimeString { get; set; }

        public static ClinicModel From(Clinic clinic)
        {
            return new ClinicModel(clinic);
        }
    }

    public class EducationModel
    {
        public EducationModel(Education education)
        {
            this.EducationId = education.Id;
            this.Teacher = education.Teacher;
            this.YingYangShi = education.YingYangShi.ToString();
            this.HuShi = education.HuShi.ToString();
            this.ZuBing = education.ZuBing.ToString();
            this.CreateTimeString = education.CreateTime.ToString("yyyy-MM-dd");
        }

        public int EducationId { get; set; }

        /// <summary>
        /// 教师
        /// </summary>
        public String Teacher { get; set; }
        
        /// <summary>
        /// 营养师教育  
        /// </summary>
        public String YingYangShi { get; set; }

        /// <summary>
        /// 糖尿病护士教育
        /// </summary>
        public String HuShi { get; set; }

        /// <summary>
        /// 足病防护教育
        /// </summary>
        public String ZuBing { get; set; }

        /// <summary>
        /// 教育时间
        /// </summary>
        public String CreateTimeString { get; set; }

        public static EducationModel From(Education education)
        {
            return new EducationModel(education);
        }
    }

    public class EvaScaleModel
    {
        public EvaScaleModel(EvaluationScale scale)
        {
            this.EvaluationScaleId = scale.Id;
            this.Name = scale.ScaleName;
            this.Result = scale.Result;
            this.CreateTimeString = scale.CreateTime.ToString("yyyy-MM-dd");
        }

        public int EvaluationScaleId { get; set; }

        /// <summary>
        /// 量表名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 评估得分
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 评估日期
        /// </summary>
        public String CreateTimeString { get; set; }

        public static EvaScaleModel From(EvaluationScale scale)
        {
            return new EvaScaleModel(scale);
        }
    }
}
