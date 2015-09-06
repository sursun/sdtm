using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Gms.Domain;
using Gms.Domain.Examine;
using Gms.Domain.FollowUp;
using Gms.Domain.Health;
using NHibernate.Hql.Classic;
using SharpArch.NHibernate.Web.Mvc;

namespace Gms.Web.Mvc.Controllers
{
    public class BaseLineController : BaseController
    {
        public ActionResult Index(int id)
        {
            var patient = this.PatientRepository.Get(id);

            return View(patient);
        }

        public ActionResult TableDatas(int id)
        {
            IList<BaseLineTableItemModel> list = null;

            var patient = this.PatientRepository.Get(id);

            if (patient.DiabetesType == DiabetesType.妊娠糖尿病)
            {
                list = GetGdTableData(patient);
            }
            else
            {
                list = GetTableData(patient);
            }

            return Json(list);
        }

        public IList<BaseLineTableItemModel> GetTableData(Patient patient)
        {
            IList<BaseLineTableItemModel> list = new List<BaseLineTableItemModel>();

            BaseLine baseLine = this.TryCreateBaseLine(patient);
     
            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = baseLine.Id, 
                Title = "一般情况",
                Id = baseLine.Identification.Id,
                Doctor = baseLine.Doctor == null ? "" : baseLine.Doctor.RealName,
                CreateUser = baseLine.PracticeDoctor == null ? "" : baseLine.PracticeDoctor.RealName,
                CreateTime = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = baseLine.Identification.Process(),
                Url = "Ybqk",
                Params = "id=" + baseLine.Identification.Id
            });

            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = baseLine.Id,
                Title = "既往史",
                Id = baseLine.MedicalHistory.Id,
                Doctor = baseLine.Doctor == null ? "" : baseLine.Doctor.RealName,
                CreateUser = baseLine.PracticeDoctor == null ? "" : baseLine.PracticeDoctor.RealName,
                CreateTime = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = baseLine.MedicalHistory.Process(),
                Url = "Jws",
                Params = "id=" + baseLine.MedicalHistory.Id
            });

            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = baseLine.Id,
                Title = "糖尿病相关家族史",
                Id = baseLine.FamilyHistory.Id,
                Doctor = baseLine.Doctor == null ? "" : baseLine.Doctor.RealName,
                CreateUser = baseLine.PracticeDoctor == null ? "" : baseLine.PracticeDoctor.RealName,
                CreateTime = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = baseLine.FamilyHistory.Process(),
                Url = "Jzs",
                Params = "id=" + baseLine.FamilyHistory.Id
            });

            int completed = 0;
            int total = 0;
            completed += baseLine.Physical.Completed;
            completed += baseLine.Uroscopy.Completed;
            completed += baseLine.Blood.Completed;
            completed += baseLine.BloodRt.Completed;
            completed += baseLine.TnbBasic.Completed;

            total += baseLine.Physical.Total;
            total += baseLine.Uroscopy.Total;
            total += baseLine.Blood.Total;
            total += baseLine.BloodRt.Total;
            total += baseLine.TnbBasic.Total;
           
            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = baseLine.Id,
                Title = "体格检查",
                Id = baseLine.Id,
                Doctor = baseLine.Doctor == null ? "" : baseLine.Doctor.RealName,
                CreateUser = baseLine.PracticeDoctor == null ? "" : baseLine.PracticeDoctor.RealName,
                CreateTime = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = total == 0 ? 0 : completed * 100 / total,
                Url = "Tgjc",
                Params = String.Format("physicalid={0}&uroscopyid={1}&bloodid={2}&bloodrtid={3}&tnbbasicid={4}",
                baseLine.Physical.Id,baseLine.Uroscopy.Id,baseLine.Blood.Id,baseLine.BloodRt.Id,baseLine.TnbBasic.Id)
            });

            completed = 0;
            total = 0;
            completed += baseLine.Eye.Completed;
            completed += baseLine.Legs.Completed;
            completed += baseLine.UnClassified.Completed;

            total += baseLine.Eye.Total;
            total += baseLine.Legs.Total;
            total += baseLine.UnClassified.Total;

            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = baseLine.Id,
                Title = "并发症筛查",
                Id = baseLine.Id,
                Doctor = baseLine.Doctor == null ? "" : baseLine.Doctor.RealName,
                CreateUser = baseLine.PracticeDoctor == null ? "" : baseLine.PracticeDoctor.RealName,
                CreateTime = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = total == 0 ? 0 : completed * 100 / total,
                Url = "Bfz",
                Params = String.Format("eyeid={0}&legsid={1}&unclassifiedid={2}",
                baseLine.Eye.Id, baseLine.Legs.Id, baseLine.UnClassified.Id)
            });

            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = baseLine.Id,
                Title = "医疗诊断",
                Id = baseLine.Diagnoses.Id,
                Doctor = baseLine.Doctor == null ? "" : baseLine.Doctor.RealName,
                CreateUser = baseLine.PracticeDoctor == null ? "" : baseLine.PracticeDoctor.RealName,
                CreateTime = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = baseLine.Diagnoses.Process(),
                Url = "Ylzd",
                Params = "id=" + baseLine.Diagnoses.Id
            });

            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = baseLine.Id,
                Title = "诊疗方案",
                Id = baseLine.Treatment.Id,
                Doctor = baseLine.Doctor == null ? "" : baseLine.Doctor.RealName,
                CreateUser = baseLine.PracticeDoctor == null ? "" : baseLine.PracticeDoctor.RealName,
                CreateTime = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = baseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = baseLine.Treatment.Process(),
                Url = "Zlfa",
                Params = "id=" + baseLine.Treatment.Id
            });

            return list;
        }

        public IList<BaseLineTableItemModel> GetGdTableData(Patient patient)
        {
            IList<BaseLineTableItemModel> list = new List<BaseLineTableItemModel>();

            GdBaseLine gdBaseLine = this.TryCreateGdBaseLine(patient);

            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = gdBaseLine.Id,
                Title = "一般情况",
                Id = gdBaseLine.GdIdentification.Id,
                Doctor = gdBaseLine.Doctor == null ? "" : gdBaseLine.Doctor.RealName,
                CreateUser = gdBaseLine.PracticeDoctor == null ? "" : gdBaseLine.PracticeDoctor.RealName,
                CreateTime = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = gdBaseLine.GdIdentification.Process(),
                Url = "GdYbqk",
                Params = "id=" + gdBaseLine.GdIdentification.Id
            });

            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = gdBaseLine.Id,
                Title = "病史回顾",
                Id = gdBaseLine.GdHistory.Id,
                Doctor = gdBaseLine.Doctor == null ? "" : gdBaseLine.Doctor.RealName,
                CreateUser = gdBaseLine.PracticeDoctor == null ? "" : gdBaseLine.PracticeDoctor.RealName,
                CreateTime = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = gdBaseLine.GdHistory.Process(),
                Url = "GdHistory",
                Params = "id=" + gdBaseLine.GdHistory.Id
            });

            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = gdBaseLine.Id,
                Title = "GDM风险评估",
                Id = gdBaseLine.GdmRisk.Id,
                Doctor = gdBaseLine.Doctor == null ? "" : gdBaseLine.Doctor.RealName,
                CreateUser = gdBaseLine.PracticeDoctor == null ? "" : gdBaseLine.PracticeDoctor.RealName,
                CreateTime = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = gdBaseLine.GdmRisk.Process(),
                Url = "GdmRisk",
                Params = "id=" + gdBaseLine.GdmRisk.Id
            });

            int completed = 0;
            int total = 0;
            completed += gdBaseLine.GdPhysical.Completed;
            completed += gdBaseLine.BloodRt.Completed;

            total += gdBaseLine.GdPhysical.Total;
            total += gdBaseLine.BloodRt.Total;
           
            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = gdBaseLine.Id,
                Title = "体格检查",
                Id = gdBaseLine.Id,
                Doctor = gdBaseLine.Doctor == null ? "" : gdBaseLine.Doctor.RealName,
                CreateUser = gdBaseLine.PracticeDoctor == null ? "" : gdBaseLine.PracticeDoctor.RealName,
                CreateTime = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = total == 0 ? 0 : completed * 100 / total,
                Url = "GdTgjc",
                Params = String.Format("gdphysicalid={0}&bloodrtid={1}",
                gdBaseLine.GdPhysical.Id, gdBaseLine.BloodRt.Id)
            });
            
        
            list.Add(new BaseLineTableItemModel()
            {
                BaseLineId = gdBaseLine.Id,
                Title = "诊疗方案",
                Id = gdBaseLine.Treatment.Id,
                Doctor = gdBaseLine.Doctor == null ? "" : gdBaseLine.Doctor.RealName,
                CreateUser = gdBaseLine.PracticeDoctor == null ? "" : gdBaseLine.PracticeDoctor.RealName,
                CreateTime = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                ModifyDate = gdBaseLine.CreateTime.ToString("yyyy-M-d dddd"),
                Progress = gdBaseLine.Treatment.Process(),
                Url = "Zlfa",
                Params = "id=" + gdBaseLine.Treatment.Id
            });

            return list;
        }

        /// <summary>
        /// 创建基线资料
        /// 如果已经存在则直接返回
        /// </summary>
        /// <param name="patient"></param>
        [Transaction]
        public BaseLine TryCreateBaseLine(Patient patient)
        {
            if (patient == null)
            {
                return null;
            }

            BaseLine baseLine = this.BaseLineRepository.GetBy(patient.Id);

            if (baseLine == null)
            {
                baseLine = new BaseLine();
                baseLine.Patient = patient;
                baseLine.CreateTime = DateTime.Now;
                baseLine.Doctor = patient.Doctor;
                baseLine.PracticeDoctor = CurrentUser;

                Identification identification = new Identification();
                identification.Patient = patient;
                baseLine.Identification = this.IdentificationRepository.SaveOrUpdate(identification);

                MedicalHistory medicalHistory = new MedicalHistory();
                medicalHistory.Patient = patient;
                baseLine.MedicalHistory = this.MedicalHistoryRepository.SaveOrUpdate(medicalHistory);

                FamilyHistory familyHistory = new FamilyHistory();
                familyHistory.Patient = patient;
                baseLine.FamilyHistory = this.FamilyHistoryRepository.SaveOrUpdate(familyHistory);

                Physical physical = new Physical();
                physical.Patient = patient;
                baseLine.Physical = this.PhysicalRepository.SaveOrUpdate(physical);

                Uroscopy uroscopy = new Uroscopy();
                uroscopy.Patient = patient;
                baseLine.Uroscopy = this.UroscopyRepository.SaveOrUpdate(uroscopy);

                Blood blood = new Blood();
                blood.Patient = patient;
                baseLine.Blood = this.BloodRepository.SaveOrUpdate(blood);

                BloodRt bloodRt = new BloodRt();
                bloodRt.Patient = patient;
                baseLine.BloodRt = this.BloodRtRepository.SaveOrUpdate(bloodRt);

                TnbBasic tnbBasic = new TnbBasic();
                tnbBasic.Patient = patient;
                baseLine.TnbBasic = this.TnbBasicRepository.SaveOrUpdate(tnbBasic);

                Eye eye = new Eye();
                eye.Patient = patient;
                baseLine.Eye = this.EyeRepository.SaveOrUpdate(eye);

                Legs legs = new Legs();
                legs.Patient = patient;
                baseLine.Legs = this.LegsRepository.SaveOrUpdate(legs);

                UnClassified unClassified = new UnClassified();
                unClassified.Patient = patient;
                baseLine.UnClassified = this.UnClassifiedRepository.SaveOrUpdate(unClassified);

                Diagnoses diagnoses = new Diagnoses();
                diagnoses.Patient = patient;
                baseLine.Diagnoses = this.DiagnosesRepository.SaveOrUpdate(diagnoses);

                Treatment treatment = new Treatment();
                treatment.Patient = patient;
                baseLine.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);

                //保存基线资料
                baseLine = this.BaseLineRepository.SaveOrUpdate(baseLine);
            }

            return baseLine;
        }
        
        /// <summary>
        /// 创建妊娠基线资料
        /// 如果已经存在则直接返回
        /// </summary>
        /// <param name="patient"></param>
        [Transaction]
        public GdBaseLine TryCreateGdBaseLine(Patient patient)
        {
            if (patient == null)
            {
                return null;
            }

            GdBaseLine gdBaseLine = this.GdBaseLineRepository.GetBy(patient.Id);

            if (gdBaseLine == null)
            {
                gdBaseLine = new GdBaseLine();
                gdBaseLine.Patient = patient;
                gdBaseLine.CreateTime = DateTime.Now;
                gdBaseLine.Doctor = CurrentUser;
                gdBaseLine.PracticeDoctor = CurrentUser;

                GdIdentification gdIdentification = new GdIdentification();
                gdIdentification.Patient = patient;
                gdBaseLine.GdIdentification = this.GdIdentificationRepository.SaveOrUpdate(gdIdentification);

                GdHistory gdHistory = new GdHistory();
                gdHistory.Patient = patient;
                gdBaseLine.GdHistory = this.GdHistoryRepository.SaveOrUpdate(gdHistory);

                GdPhysical gdPhysical = new GdPhysical();
                gdPhysical.Patient = patient;
                gdBaseLine.GdPhysical = this.GdPhysicalRepository.SaveOrUpdate(gdPhysical);

                GdmRisk gdmRisk = new GdmRisk();
                gdmRisk.Patient = patient;
                gdBaseLine.GdmRisk = this.GdmRiskRepository.SaveOrUpdate(gdmRisk);

                BloodRt bloodRt = new BloodRt();
                bloodRt.Patient = patient;
                gdBaseLine.BloodRt = this.BloodRtRepository.SaveOrUpdate(bloodRt);

                Treatment treatment = new Treatment();
                treatment.Patient = patient;
                gdBaseLine.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);

                //保存基线资料
                gdBaseLine = this.GdBaseLineRepository.SaveOrUpdate(gdBaseLine);
            }

            return gdBaseLine;
        }

        [Transaction]
        public ActionResult UpdateDoctor(int patientid, int? doctorid, int? pdoctorid)
        {
            if (!(doctorid.HasValue || pdoctorid.HasValue))
            {
                return JsonError("请选择一个医生");
            }
            var patient = this.PatientRepository.Get(patientid);

            if (patient.DiabetesType == DiabetesType.普通糖尿病)
            {
                var baseline = this.BaseLineRepository.GetBy(patientid);

                if (doctorid.HasValue)
                {
                    baseline.Doctor = this.DoctorRepository.Get(doctorid.Value);
                }

                if (pdoctorid.HasValue)
                {
                    baseline.PracticeDoctor = this.DoctorRepository.Get(pdoctorid.Value);
                }
                
                this.BaseLineRepository.SaveOrUpdate(baseline);
            }
            else
            {
                var baseline = this.GdBaseLineRepository.GetBy(patientid);

                if (doctorid.HasValue)
                {
                    baseline.Doctor = this.DoctorRepository.Get(doctorid.Value);
                }

                if (pdoctorid.HasValue)
                {
                    baseline.PracticeDoctor = this.DoctorRepository.Get(pdoctorid.Value);
                }

                this.GdBaseLineRepository.SaveOrUpdate(baseline);
                
            }

            return JsonSuccess();
        }

        #region 一般情况
        public ActionResult Ybqk(int id)
        {
            Identification idf = this.IdentificationRepository.Get(id);

            return View(idf);
        }
        
        [Transaction]
        public ActionResult YbqkSave(Identification idf)
        {
            try
            {
                if (idf == null)
                {
                    throw new Exception("无效数据");
                }

                if (idf.Id < 1)
                {
                    throw new Exception("信息不存在");
                }

                //更新患者信息
                if (idf.Patient.Id > 0)
                {
                    var patient = this.PatientRepository.Get(idf.Patient.Id);
                    TryUpdateModel(patient, "Patient");
                    this.PatientRepository.SaveOrUpdate(patient);
                }

                idf = this.IdentificationRepository.Get(idf.Id);

                TryUpdateModel(idf);
             
                idf = this.IdentificationRepository.SaveOrUpdate(idf);

                return JsonSuccess(idf);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
            
        }

        public ActionResult GdYbqk(int id)
        {
            GdIdentification idf = this.GdIdentificationRepository.Get(id);

            return View(idf);
        }
        
        [Transaction]
        public ActionResult GdYbqkSave(GdIdentification idf)
        {
            try
            {
                if (idf == null)
                {
                    throw new Exception("无效数据");
                }

                if (idf.Id < 1)
                {
                    throw new Exception("信息不存在");
                }

                //更新患者信息
                if (idf.Patient.Id > 0)
                {
                    var patient = this.PatientRepository.Get(idf.Patient.Id);
                    TryUpdateModel(patient, "Patient");
                    this.PatientRepository.SaveOrUpdate(patient);
                }

                idf = this.GdIdentificationRepository.Get(idf.Id);

                TryUpdateModel(idf);

                idf = this.GdIdentificationRepository.SaveOrUpdate(idf);

                return JsonSuccess(idf);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
            
        }

        #endregion

        #region 既往史(病史回顾)
        public ActionResult Jws(int id)
        {
            MedicalHistory medicalHistory = this.MedicalHistoryRepository.Get(id);

            return View(medicalHistory);
        }

        [Transaction]
        public ActionResult JwsSave(MedicalHistory medicalHistory)
        {
            try
            {
                if (medicalHistory == null)
                {
                    throw new Exception("无效数据");
                }

                if (medicalHistory.Id < 1)
                {
                    throw new Exception("信息不存在");
                }

                medicalHistory = this.MedicalHistoryRepository.Get(medicalHistory.Id);

                TryUpdateModel(medicalHistory);
  
                medicalHistory = this.MedicalHistoryRepository.SaveOrUpdate(medicalHistory);

                return JsonSuccess(medicalHistory);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        //public ActionResult JwsZhongLiu(int id)
        //{
        //    MedicalHistory medicalHistory = this.MedicalHistoryRepository.Get(id);

        //    return View(medicalHistory);
        //}

        //public ActionResult JwsZhongLiuList(int id)
        //{
        //    MedicalHistory medicalHistory = this.MedicalHistoryRepository.Get(id);

        //    var list = medicalHistory.ZhongLiuItems;
        //    var data = list.Select(c => MedicalHistoryItemModel.From(c)).ToList();
        //    var result = new { total = data.Count, rows = data };
        //    return Json(result);
        //}

        //[Transaction]
        //public ActionResult JwsZhongLiuAdd(int mid, string name,string note,DateTime datetime)
        //{
        //    try
        //    {
        //        var medicalHistory = this.MedicalHistoryRepository.Get(mid);

        //        if (medicalHistory == null)
        //        {
        //            throw new Exception("无效数据");
        //        }

        //        var item =
        //            this.MedicalHistoryItemRepository.SaveOrUpdate(new MedicalHistoryItem()
        //            {
        //                Name = name,
        //                DateTime = datetime,
        //                Note = note
        //            });
        //        medicalHistory.ZhongLiuItems.Add(item);

        //        medicalHistory = this.MedicalHistoryRepository.SaveOrUpdate(medicalHistory);

        //        return JsonSuccess(medicalHistory);
        //    }
        //    catch (Exception ex)
        //    {
        //        return JsonError(ex.Message);
        //    }
        //}

        public ActionResult JwsItem(int id, MedicalHistoryType hType)
        {
            MedicalHistory medicalHistory = this.MedicalHistoryRepository.Get(id);
            ViewData["TypeFlag"] = (int)hType;

            return View(medicalHistory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeFlag">0，肿瘤史1,手术史，2，骨折史</param>
        /// <returns></returns>
        public ActionResult JwsItemList(int mid, MedicalHistoryType hType)
        {
            var list = this.MedicalHistoryItemRepository.GetBy(mid, hType);

            var data = list.Select(c => MedicalHistoryItemModel.From(c)).ToList();
            var result = new { total = data.Count, rows = data };
            return Json(result);
        }

        [Transaction]
        public ActionResult JwsItemAdd(int mid, MedicalHistoryType hType, string name, string note, DateTime datetime)
        {
            try
            {
                var medicalHistory = this.MedicalHistoryRepository.Get(mid);

                if (medicalHistory == null)
                {
                    throw new Exception("无效数据");
                }

                var item =this.MedicalHistoryItemRepository.SaveOrUpdate(new MedicalHistoryItem()
                    {
                        MedicalHistory = medicalHistory,
                        Name = name,
                        Note = note,
                        MedicalHistoryType = hType,
                        DateTime = datetime
                    });

                return JsonSuccess(item);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult JwsItemDelete(int mid, int id)
        {
            try
            {
                var item = this.MedicalHistoryItemRepository.Get(id);

                if ( item == null)
                {
                    throw new Exception("无效数据");
                }

                this.MedicalHistoryItemRepository.Delete(item);
   
                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        public ActionResult GdHistory(int id)
        {
            GdHistory gdHistory = this.GdHistoryRepository.Get(id);

            return View(gdHistory);
        }

        [Transaction]
        public ActionResult GdHistorySave(GdHistory gdHistory, int[] checkForIllnessFlag, int[] checkForGdSyndromeFlag, int[] checkForFamilyIllnessFlag)
        {
            try
            {
                if (gdHistory == null)
                {
                    throw new Exception("无效数据");
                }

                if (gdHistory.Id < 1)
                {
                    throw new Exception("信息不存在");
                }

                gdHistory = this.GdHistoryRepository.Get(gdHistory.Id);

                TryUpdateModel(gdHistory);

                //疾病史
                gdHistory.IllnessFlag = 0;
                if (checkForIllnessFlag != null)
                {
                    foreach (var flag in checkForIllnessFlag)
                    {
                        gdHistory.IllnessFlag |= flag;
                    }
                }

                //妊娠并发症
                gdHistory.GdSyndromeFlag = 0;
                if (checkForGdSyndromeFlag != null)
                {
                    foreach (var flag in checkForGdSyndromeFlag)
                    {
                        gdHistory.GdSyndromeFlag |= flag;
                    }
                }

                //家族史（一级亲属）
                gdHistory.FamilyIllnessFlag = 0;
                if (checkForFamilyIllnessFlag != null)
                {
                    foreach (var flag in checkForFamilyIllnessFlag)
                    {
                        gdHistory.FamilyIllnessFlag |= flag;
                    }
                }


                gdHistory = this.GdHistoryRepository.SaveOrUpdate(gdHistory);

                return JsonSuccess(gdHistory);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
        #endregion

        #region 糖尿病家族史
        [Transaction]
        public ActionResult Jzs(int id)
        {
            FamilyHistory familyHistory = this.FamilyHistoryRepository.Get(id);

            return View(familyHistory);
        }

        [Transaction]
        public ActionResult JzsSave(FamilyHistory familyHistory)
        {
            try
            {
                if (familyHistory == null)
                {
                    throw new Exception("无效数据");
                }

                if (familyHistory.Id < 1)
                {
                    throw new Exception("信息不存在");
                }

                familyHistory = this.FamilyHistoryRepository.Get(familyHistory.Id);

                TryUpdateModel(familyHistory);

                familyHistory = this.FamilyHistoryRepository.SaveOrUpdate(familyHistory);

                return JsonSuccess(familyHistory);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #endregion

        #region GDM风险评估
        [Transaction]
        public ActionResult GdmRisk(int id)
        {
            GdmRisk gdmRisk = this.GdmRiskRepository.Get(id);

            return View(gdmRisk);
        }

        [Transaction]
        public ActionResult GdmRiskSave(GdmRisk gdmRisk, int[] checkForHighRiskFlag)
        {
            try
            {
                if (gdmRisk == null)
                {
                    throw new Exception("无效数据");
                }

                if (gdmRisk.Id < 1)
                {
                    throw new Exception("信息不存在");
                }

                gdmRisk = this.GdmRiskRepository.Get(gdmRisk.Id);

                TryUpdateModel(gdmRisk);

                gdmRisk.HighRiskFlag = 0;
                if (checkForHighRiskFlag != null)
                {
                    foreach (var flag in checkForHighRiskFlag)
                    {
                        gdmRisk.HighRiskFlag |= flag;
                    }
                }

                gdmRisk = this.GdmRiskRepository.SaveOrUpdate(gdmRisk);

                return JsonSuccess(gdmRisk);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #endregion

        #region 体格检查
        public ActionResult Tgjc(int physicalid, int uroscopyid, int bloodid, int bloodrtid, int tnbbasicid)
        {
            TgjcModel tgjcModel = new TgjcModel();
 
            tgjcModel.Physical = this.PhysicalRepository.Get(physicalid);
            tgjcModel.Uroscopy = this.UroscopyRepository.Get(uroscopyid);
            tgjcModel.Blood = this.BloodRepository.Get(bloodid);
            tgjcModel.BloodRt = this.BloodRtRepository.Get(bloodrtid);
            tgjcModel.TnbBasic = this.TnbBasicRepository.Get(tnbbasicid);

            return View(tgjcModel);
        }

        [Transaction]
        public ActionResult TgjcSave(Physical physical, Uroscopy uroscopy, Blood blood, BloodRt bloodRt, TnbBasic tnbBasic)
        {
            try
            {
                //体格检查
                if (physical.Id > 0)
                {
                    physical = this.PhysicalRepository.Get(physical.Id);
                    TryUpdateModel(physical, "Physical");
                    this.PhysicalRepository.SaveOrUpdate(physical);
                }
                
                //尿液检查
                if (uroscopy.Id > 0)
                {
                    uroscopy = this.UroscopyRepository.Get(uroscopy.Id);
                    TryUpdateModel(uroscopy, "Uroscopy");
                    this.UroscopyRepository.SaveOrUpdate(uroscopy);
                }
                
                //血液检查
                if (blood.Id > 0)
                {
                    blood = this.BloodRepository.Get(blood.Id);
                    TryUpdateModel(blood,"Blood");
                    this.BloodRepository.SaveOrUpdate(blood);
                }
                
                //血液常规检查
                if (bloodRt.Id > 0)
                {
                    bloodRt = this.BloodRtRepository.Get(bloodRt.Id);
                    TryUpdateModel(bloodRt,"BloodRt");
                    this.BloodRtRepository.SaveOrUpdate(bloodRt);
                }
                
                //糖尿病基本检查
                if (tnbBasic.Id > 0)
                {
                    tnbBasic = this.TnbBasicRepository.Get(tnbBasic.Id);
                    TryUpdateModel(tnbBasic,"TnbBasic");
                    this.TnbBasicRepository.SaveOrUpdate(tnbBasic);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
        
        public ActionResult GdTgjc(int gdphysicalid, int bloodrtid)
        {
            GdTgjcModel tgjcModel = new GdTgjcModel();

            tgjcModel.GdPhysical = this.GdPhysicalRepository.Get(gdphysicalid);
            tgjcModel.BloodRt = this.BloodRtRepository.Get(bloodrtid);

            return View(tgjcModel);
        }

        [Transaction]
        public ActionResult GdTgjcSave(GdPhysical gdPhysical, BloodRt bloodRt)
        {
            try
            {
                //体格检查
                if (gdPhysical.Id > 0)
                {
                    gdPhysical = this.GdPhysicalRepository.Get(gdPhysical.Id);
                    TryUpdateModel(gdPhysical, "GdPhysical");
                    this.GdPhysicalRepository.SaveOrUpdate(gdPhysical);
                }

                //血液常规检查
                if (bloodRt.Id > 0)
                {
                    bloodRt = this.BloodRtRepository.Get(bloodRt.Id);
                    TryUpdateModel(bloodRt,"BloodRt");
                    this.BloodRtRepository.SaveOrUpdate(bloodRt);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
        
       //[Transaction]
       // public ActionResult TgjcSave(BaseLine baseLine, Physical physical, Uroscopy uroscopy, Blood blood, BloodRt bloodRt, TnbBasic tnbBasic)
       // {
       //     try
       //     {
       //         if (baseLine == null)
       //         {
       //             throw new Exception("无效数据");
       //         }

       //         if (baseLine.Id < 1)
       //         {
       //             throw new Exception("信息不存在");
       //         }
                
       //         //
       //         //标示是否更新baseline
       //         //
       //         bool bFalg = false;

       //         //体格检查
       //         if (physical.Id > 0)
       //         {
       //             physical = this.PhysicalRepository.Get(physical.Id);
       //             TryUpdateModel(physical, "Physical");
       //         }
       //         else
       //         {
       //             bFalg = true;
       //         }

       //         baseLine.Physical = this.PhysicalRepository.SaveOrUpdate(physical);

       //         //尿液检查
       //         if (uroscopy.Id > 0)
       //         {
       //             uroscopy = this.UroscopyRepository.Get(uroscopy.Id);
       //             TryUpdateModel(uroscopy, "Uroscopy");
       //         }
       //         else
       //         {
       //             bFalg = true;
       //         }

       //         baseLine.Uroscopy = this.UroscopyRepository.SaveOrUpdate(uroscopy);

       //         //血液检查
       //         if (blood.Id > 0)
       //         {
       //             blood = this.BloodRepository.Get(blood.Id);
       //             TryUpdateModel(blood,"Blood");
       //         }
       //         else
       //         {
       //             bFalg = true;
       //         }
   
       //         baseLine.Blood = this.BloodRepository.SaveOrUpdate(blood);

       //         //血液常规检查
       //         if (bloodRt.Id > 0)
       //         {
       //             bloodRt = this.BloodRtRepository.Get(bloodRt.Id);
       //             TryUpdateModel(bloodRt,"BloodRt");
       //         }
       //         else
       //         {
       //             bFalg = true;
       //         }

       //         baseLine.BloodRt = this.BloodRtRepository.SaveOrUpdate(bloodRt);

       //         //糖尿病基本检查
       //         if (tnbBasic.Id > 0)
       //         {
       //             tnbBasic = this.TnbBasicRepository.Get(tnbBasic.Id);
       //             TryUpdateModel(tnbBasic,"TnbBasic");
       //         }
       //         else
       //         {
       //             bFalg = true;
       //         }

       //         baseLine.TnbBasic = this.TnbBasicRepository.SaveOrUpdate(tnbBasic);

       //         if (bFalg)
       //         {
       //             baseLine = this.BaseLineRepository.Get(baseLine.Id);
       //             TryUpdateModel(baseLine);
       //             this.BaseLineRepository.SaveOrUpdate(baseLine);
       //         }


       //         return JsonSuccess();
       //     }
       //     catch (Exception ex)
       //     {
       //         return JsonError(ex.Message);
       //     }
       // }

        #endregion

        #region 并发症筛查
        public ActionResult Bfz(int eyeid,int legsid,int unclassifiedid)
        {
            BfzModel bfzModel = new BfzModel();

            bfzModel.Eye = this.EyeRepository.Get(eyeid);
            bfzModel.Legs = this.LegsRepository.Get(legsid);
            bfzModel.UnClassified = this.UnClassifiedRepository.Get(unclassifiedid);

            return View(bfzModel);
        }

        [Transaction]
        public ActionResult BfzSave( Eye eye,Legs legs,UnClassified unClassified)
        {
            try
            {
                //眼睛检查
                if (eye.Id > 0)
                {
                    eye = this.EyeRepository.Get(eye.Id);
                    TryUpdateModel(eye, "Eye");
                    this.EyeRepository.SaveOrUpdate(eye);
                }

                //下肢血管及神经病变筛查
                if (legs.Id > 0)
                {
                    legs = this.LegsRepository.Get(legs.Id);
                    TryUpdateModel(legs, "Legs");
                    this.LegsRepository.SaveOrUpdate(legs);
                }
                
                //其他筛查
                if (unClassified.Id > 0)
                {
                    unClassified = this.UnClassifiedRepository.Get(unClassified.Id);
                    TryUpdateModel(unClassified, "UnClassified");
                    this.UnClassifiedRepository.SaveOrUpdate(unClassified);
                }
                
                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #endregion

    }

    public class BaseLineTableItemModel
    {
        public int BaseLineId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string FollowuDate { get; set; }
        public string Doctor { get; set; }
        public string CreateUser { get; set; }
        public string CreateTime { get; set; }
        public string ModifyDate { get; set; }
        public int Progress { get; set; }
        public string Url { get; set; }
        public string Params { get; set; }
    }

    public class TgjcModel
    {
        /// <summary>
        /// 所属类别Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 体格检查
        /// </summary>
        public Physical Physical { get; set; }

        /// <summary>
        /// 尿液检查
        /// </summary>
        public Uroscopy Uroscopy { get; set; }

        /// <summary>
        /// 血液检查
        /// </summary>
        public Blood Blood { get; set; }

        /// <summary>
        /// 血液其他检查
        /// </summary>
        public BloodRt BloodRt { get; set; }

        /// <summary>
        /// 糖尿病基本检查
        /// </summary>
        public TnbBasic TnbBasic { get; set; }
    }

    public class GdTgjcModel
    {
        /// <summary>
        /// 所属类别Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 体格检查
        /// </summary>
        public GdPhysical GdPhysical { get; set; }
        
        /// <summary>
        /// 血液其他检查
        /// </summary>
        public BloodRt BloodRt { get; set; }
    }

    public class BfzModel
    {
        /// <summary>
        /// 所属类别Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 眼睛检查
        /// </summary>
        public Eye Eye { get; set; }

        /// <summary>
        /// 下肢血管及神经病变筛查
        /// </summary>
        public Legs Legs { get; set; }

        /// <summary>
        /// 其他筛查
        /// </summary>
        public UnClassified UnClassified { get; set; }
    }

    public class MedicalHistoryItemModel
    {
        public MedicalHistoryItemModel(MedicalHistoryItem item)
        {
            this.Id = item.Id;
            this.Name = item.Name;
            this.Note = item.Note;
            this.DateTimeString = item.DateTime.ToString("yyyy-MM-dd");
        }

        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public String DateTimeString { get; set; }


        public static MedicalHistoryItemModel From(MedicalHistoryItem item)
        {
            return new MedicalHistoryItemModel(item);
        }
    }
}
