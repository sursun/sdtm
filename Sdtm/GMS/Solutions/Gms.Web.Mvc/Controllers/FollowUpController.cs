using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FluentNHibernate.Utils;
using Gms.Common;
using Gms.Domain;
using Gms.Domain.Examine;
using Gms.Domain.FollowUp;
using Gms.Domain.Health;
using NHibernate.Proxy.DynamicProxy;
using SharpArch.NHibernate.Web.Mvc;

namespace Gms.Web.Mvc.Controllers
{
    public class FollowUpController : BaseController
    {
        public ActionResult Patient(int id)
        {
            var patient = this.PatientRepository.Get(id);

            if (patient.DiabetesType == DiabetesType.妊娠糖尿病)
            {
                return View("GdPatient", patient);
            }

            return View(patient);
        }

        #region 就诊备忘录

        public ActionResult Notice()
        {
            return View(new FollowUpInfo());
        }

        public ActionResult NoticeList(FollowUpInfoQuery query)
        {
            if (CurrentUser != null )
            {
                if (CurrentUser.ScopeType == ScopeType.自己)
                {
                    query.DoctorId = CurrentUser.Id;
                }
                else if (CurrentUser.ScopeType == ScopeType.自定义)
                {
                    query.PatientIds = CurrentUser.Scope;
                }
            }
            

            var list = this.FollowUpInfoRepository.GetList(query);

            IList<FollowUpInfo> tmpList = list.Data;

            //if (CurrentUser != null && CurrentUser.ScopeType != ScopeType.全部)
            //{
            //    int[] dd = CurrentUser.Scope.Split('|').Where(c => c.Length > 0).Select(c => (int.Parse(c))).ToArray();
            //    tmpList = dd.Select(i => tmpList.FirstOrDefault(c => c.Id == i)).ToList();
            //}

            IList<NoticeModel> data = tmpList.Select(c => NoticeModel.From(c)).ToList();

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);

        }

        #endregion

        #region 随访设置

        public ActionResult FollowUpInfoView(int patientid)
        {
            var info = FollowUpInfoGetOrCreate(patientid);
            
            return View(info);
        }

        public ActionResult FollowUpInfoEdit(int patientid)
        {
            var info = this.FollowUpInfoGetOrCreate(patientid);
            
            return View(info);
        }
        
        [Transaction]
        public FollowUpInfo FollowUpInfoGetOrCreate(int patientid)
        {

            var info = this.FollowUpInfoRepository.GetBy(patientid);
            if (info == null)
            {
                info = new FollowUpInfo();
                info.Doctor = CurrentUser;
                info.Patient = this.PatientRepository.Get(patientid);
                info = this.FollowUpInfoRepository.SaveOrUpdate(info);
            }

            return info;
        }

        [Transaction]
        public ActionResult InfoSaveOrUpdate(FollowUpInfo info)
        {
            try
            {
                if (info.Id > 0)
                {
                    info = this.FollowUpInfoRepository.Get(info.Id);
                    TryUpdateModel(info);
                }
                info.Doctor = CurrentUser;
                this.FollowUpInfoRepository.SaveOrUpdate(info);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }


        #endregion

        #region 随访

        public ActionResult FollowUp(int id)
        {
            var patient = this.PatientRepository.Get(id);

            return View(patient);
        }

        #endregion

        #region 复诊

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Patient Id</param>
        /// <returns></returns>
        public ActionResult FuZhen(int id)
        {
            var patient = this.PatientRepository.Get(id);

            return View(patient);
        }

        public ActionResult FuZhenList(FollowUpQuery query)
        {
            var list = this.FuZhenRepository.GetList(query);
            var data = list.Data.Select(c => FuZhenModel.From(c)).ToList(); ;

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);

        }

        public ActionResult FuZhenDetail(int fuzhenid)
        {
            var fuzhen = this.FuZhenRepository.Get(fuzhenid);

            int completed = 0;
            int total = 0;
            completed += fuzhen.BloodRt.Completed;
            completed += fuzhen.TnbBasic.Completed;

            total += fuzhen.BloodRt.Total;
            total += fuzhen.TnbBasic.Total;

            ViewData["pinggu"] = total == 0 ? 0 : completed * 100 / total;
            ViewData["treatment"] = fuzhen.Treatment.Process();

            return View(fuzhen);
        }

        public ActionResult FuZhenPingGu(int fuzhenid)
        {
            var fuzhen = this.FuZhenRepository.Get(fuzhenid);
            return View(fuzhen);
        }
        
        [Transaction]
        public ActionResult FuZhenPingGuSave(FuZhen fuZhen, BloodRt bloodRt, TnbBasic tnbBasic)
        {
            try
            {
                if (fuZhen == null)
                {
                    throw new Exception("无效数据");
                }

                if (fuZhen.Id < 1)
                {
                    throw new Exception("信息不存在");
                }
             
                //血液常规检查
                if (bloodRt.Id > 0)
                {
                    bloodRt = this.BloodRtRepository.Get(bloodRt.Id);
                    TryUpdateModel(bloodRt, "BloodRt");
                }
                fuZhen.BloodRt = this.BloodRtRepository.SaveOrUpdate(bloodRt);

                //糖尿病基本检查
                if (tnbBasic.Id > 0)
                {
                    tnbBasic = this.TnbBasicRepository.Get(tnbBasic.Id);
                    TryUpdateModel(tnbBasic, "TnbBasic");
                }
                fuZhen.TnbBasic = this.TnbBasicRepository.SaveOrUpdate(tnbBasic);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }


        [Transaction]
        public ActionResult FuZhenAdd(int patientid,string name)
        {
            var patient = this.PatientRepository.Get(patientid);

            var fuzhen = new FuZhen();
            
            fuzhen.Patient = patient;
            fuzhen.Doctor = patient.Doctor;
            fuzhen.PracticeDoctor = CurrentUser;

            TnbBasic tnbBasic = new TnbBasic();
            tnbBasic.Patient = patient;
            fuzhen.TnbBasic = this.TnbBasicRepository.SaveOrUpdate(tnbBasic);

            BloodRt bloodRt = new BloodRt();
            bloodRt.Patient = patient;
            fuzhen.BloodRt = this.BloodRtRepository.SaveOrUpdate(bloodRt);

            Treatment treatment = new Treatment();
            treatment.Patient = patient;
            fuzhen.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);


            if (name.IsNullOrEmpty())
            {
                fuzhen.Name = fuzhen.CreateTime.ToString("yyyy-MM-dd") + fuzhen.FollowUpType.ToString();
            }
            else
            {
                fuzhen.Name = name;
            }

            fuzhen = this.FuZhenRepository.SaveOrUpdate(fuzhen);

            return JsonSuccess(fuzhen);
        }

        [Transaction]
        public ActionResult FuZhenDelete(int fuzhenid)
        {
            try
            {
                var obj = this.FuZhenRepository.Get(fuzhenid);

                this.FuZhenRepository.Delete(obj);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult FuZhenUpdate(int fuzhenid, int? doctorid, DiseaseStage? stage)
        {
            try
            {
                var obj = this.FuZhenRepository.Get(fuzhenid);

                if (doctorid.HasValue)
                {
                    obj.Doctor = this.DoctorRepository.Get(doctorid.Value);
                }

                if (stage.HasValue)
                {
                    obj.DiseaseStage = stage.Value;

                    //同时修改患者资料
                    Patient patient = this.PatientRepository.Get(obj.Patient.Id);
                    patient.DiseaseStage = obj.DiseaseStage;
                    this.PatientRepository.SaveOrUpdate(patient);
                }

                if (doctorid.HasValue || stage.HasValue)
                {
                    this.FuZhenRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult FuZhenUpdateName(int fuzhenid, string name)
        {
            try
            {
                var obj = this.FuZhenRepository.Get(fuzhenid);


                if (obj != null && name!=null && name.Length > 0)
                {
                    obj.Name = name;
                    this.FuZhenRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #endregion

        #region 调整

        public ActionResult TiaoZheng(int id)
        {
            var patient = this.PatientRepository.Get(id);

            return View(patient);
        }

        public ActionResult TiaoZhengList(FollowUpQuery query)
        {
            var list = this.TiaoZhengRepository.GetList(query);
            var data = list.Data.Select(c => TiaoZhengModel.From(c)).ToList(); ;

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        public ActionResult TiaoZhengDetail(int tiaozhengid)
        {
            var tiaozheng = this.TiaoZhengRepository.Get(tiaozhengid);

            return View(tiaozheng);
        }

        [Transaction]
        public ActionResult TiaoZhengPingGu(int tiaozhengid)
        {
            TiaoZheng tiaozheng = this.TiaoZhengRepository.Get(tiaozhengid);
            
            return View(tiaozheng);
        }

        [Transaction]
        public ActionResult TiaoZhengAdd(int patientid,string name)
        {
            var patient = this.PatientRepository.Get(patientid);

            var tiaozheng = new TiaoZheng();
            
            tiaozheng.Patient = patient;
            tiaozheng.Doctor = patient.Doctor;
            tiaozheng.PracticeDoctor = CurrentUser;
            
            TnbBasic tnbBasic = new TnbBasic();
            tnbBasic.Patient = patient;
            tiaozheng.TnbBasic = this.TnbBasicRepository.SaveOrUpdate(tnbBasic);
            
            Treatment treatment = new Treatment();
            treatment.Patient = patient;
            tiaozheng.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);


            if (name.IsNullOrEmpty())
            {
                tiaozheng.Name = tiaozheng.CreateTime.ToString("yyyy-MM-dd") + tiaozheng.FollowUpType.ToString();
            }
            else
            {
                tiaozheng.Name = name;
            }

            tiaozheng = this.TiaoZhengRepository.SaveOrUpdate(tiaozheng);

            return JsonSuccess(tiaozheng);
        }
        
        [Transaction]
        public ActionResult TiaoZhengDelete(int tiaozhengid)
        {
            try
            {
                var obj = this.TiaoZhengRepository.Get(tiaozhengid);

                this.TiaoZhengRepository.Delete(obj);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult TiaoZhengSave(TiaoZheng tiaozheng)
        {
            try
            {
                if (tiaozheng == null)
                {
                    throw new Exception("无效数据");
                }

                if (tiaozheng.Id < 1)
                {
                    throw new Exception("信息不存在");
                }
                
                tiaozheng = this.TiaoZhengRepository.Get(tiaozheng.Id);

                UpdateModel(tiaozheng);

                this.TiaoZhengRepository.SaveOrUpdate(tiaozheng);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
            return JsonSuccess();
        }

        [Transaction]
        public ActionResult TiaoZhengUpdate(int tiaozhengid, int? doctorid, DiseaseStage? stage)
        {
            try
            {
                var obj = this.TiaoZhengRepository.Get(tiaozhengid);

                if (doctorid.HasValue)
                {
                    obj.Doctor = this.DoctorRepository.Get(doctorid.Value);
                }

                if (stage.HasValue)
                {
                    obj.DiseaseStage = stage.Value;

                    //同时修改患者资料
                    Patient patient = this.PatientRepository.Get(obj.Patient.Id);
                    patient.DiseaseStage = obj.DiseaseStage;

                    this.PatientRepository.SaveOrUpdate(patient);
                }

                if (doctorid.HasValue || stage.HasValue)
                {
                    this.TiaoZhengRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult TiaoZhengUpdateName(int followupid, string name)
        {
            try
            {
                var obj = this.TiaoZhengRepository.Get(followupid);

                if (obj != null && name != null && name.Length > 0)
                {
                    obj.Name = name;
                    this.TiaoZhengRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
        
        #endregion

        #region 年度评估

        public ActionResult Annual(int id)
        {
            var patient = this.PatientRepository.Get(id);

            return View(patient);
        }

        public ActionResult AnnualList(FollowUpQuery query)
        {
            var list = this.AnnualRepository.GetList(query);

            var data = list.Data.Select(c => AnnualModel.From(c)).ToList(); ;

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        public ActionResult AnnualDetail(int annualid)
        {
            var annual = this.AnnualRepository.Get(annualid);

            return View(annual);
        }
        
        [Transaction]
        public ActionResult AnnualAdd(int patientid,string name)
        {
            var patient = this.PatientRepository.Get(patientid);

            var annual = new Annual();
            
            annual.Patient = patient;
            annual.Doctor = patient.Doctor;
            annual.PracticeDoctor = CurrentUser;
            
            TnbBasic tnbBasic = new TnbBasic();
            tnbBasic.Patient = patient;
            annual.TnbBasic = this.TnbBasicRepository.SaveOrUpdate(tnbBasic);

            Physical physical = new Physical();
            physical.Patient = patient;
            annual.Physical = this.PhysicalRepository.SaveOrUpdate(physical);
            
            Uroscopy uroscopy = new Uroscopy();
            uroscopy.Patient = patient;
            annual.Uroscopy = this.UroscopyRepository.SaveOrUpdate(uroscopy);

            Blood blood = new Blood();
            blood.Patient = patient;
            annual.Blood = this.BloodRepository.SaveOrUpdate(blood);

            BloodRt bloodRt = new BloodRt();
            bloodRt.Patient = patient;
            annual.BloodRt = this.BloodRtRepository.SaveOrUpdate(bloodRt);

            Eye eye = new Eye();
            eye.Patient = patient;
            annual.Eye = this.EyeRepository.SaveOrUpdate(eye);

            Legs legs = new Legs();
            legs.Patient = patient;
            annual.Legs = this.LegsRepository.SaveOrUpdate(legs);

            UnClassified unClassified = new UnClassified();
            unClassified.Patient = patient;
            annual.UnClassified = this.UnClassifiedRepository.SaveOrUpdate(unClassified);

            Diagnoses diagnoses = new Diagnoses();
            diagnoses.Patient = patient;
            annual.Diagnoses = this.DiagnosesRepository.SaveOrUpdate(diagnoses);

            Treatment treatment = new Treatment();
            treatment.Patient = patient;
            annual.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);


            if (name.IsNullOrEmpty())
            {
                annual.Name = annual.CreateTime.ToString("yyyy-MM-dd") + annual.FollowUpType.ToString();
            }
            else
            {
                annual.Name = name;
            }

            annual = this.AnnualRepository.SaveOrUpdate(annual);

            return JsonSuccess(annual);
        }
        
        [Transaction]
        public ActionResult AnnualDelete(int annualid)
        {
            try
            {
                var obj = this.AnnualRepository.Get(annualid);

                this.AnnualRepository.Delete(obj);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        public ActionResult AnnualEQ5D(int id)
        {
            var annual = this.AnnualRepository.Get(id);

            return View(annual);
        }

        [Transaction]
        public ActionResult AnnualSaveEQ5D(Annual annual, int xingdong, int zjzgzj, int rchd, int ttbsf, int jlyy, int xgzk)
        {
            try
            {
                if (annual == null)
                {
                    throw new Exception("信息不完整，请刷新后再试");
                }

                annual = this.AnnualRepository.Get(annual.Id);

                String streq;
                streq = xingdong.ToString();
                streq += "|";
                streq += zjzgzj.ToString();
                streq += "|";
                streq += rchd.ToString();
                streq += "|";
                streq += ttbsf.ToString();
                streq += "|";
                streq += jlyy.ToString();
                streq += "|";
                streq += xgzk.ToString();

                annual.EQ5D = streq;

                this.AnnualRepository.SaveOrUpdate(annual);
                

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult AnnualUpdate(int annualid, int? doctorid)
        {
            try
            {
                var obj = this.AnnualRepository.Get(annualid);

                if (doctorid.HasValue)
                {
                    obj.Doctor = this.DoctorRepository.Get(doctorid.Value);
               
                    this.AnnualRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult AnnualUpdateName(int followupid, string name)
        {
            try
            {
                var obj = this.AnnualRepository.Get(followupid);

                if (obj != null && name != null && name.Length > 0)
                {
                    obj.Name = name;
                    this.AnnualRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
        
        #endregion

        #region 妊娠糖尿病

        #region 孕中随访
        public ActionResult YunZhong(int id)
        {
            var patient = this.PatientRepository.Get(id);

            return View(patient);
        }

        public ActionResult YunZhongList(GdFollowUpQuery query)
        {
            var list = this.YunZhongRepository.GetList(query);

            var data = list.Data.Select(c => YunZhongModel.From(c)).ToList(); ;

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        public ActionResult YunZhongDetail(int yunzhongid)
        {
            var yunzhong = this.YunZhongRepository.Get(yunzhongid);

            return View(yunzhong);
        }

        public ActionResult YunZhongZhiBiao(int yunzhongid)
        {
            var yunzhong = this.YunZhongRepository.Get(yunzhongid);

            return View(yunzhong);
        }

        [Transaction]
        public ActionResult YunZhongAdd(int patientid,string name)
        {
            var patient = this.PatientRepository.Get(patientid);

            var yunzhong = new YunZhong();
 
            yunzhong.Patient = patient;
            yunzhong.Doctor = patient.Doctor;
            yunzhong.PracticeDoctor = CurrentUser;
            
            Treatment treatment = new Treatment();
            treatment.Patient = patient;
            yunzhong.Treatment = this.TreatmentRepository.SaveOrUpdate(treatment);

            if (name.IsNullOrEmpty())
            {
                yunzhong.Name = yunzhong.CreateTime.ToString("yyyy-MM-dd") + "孕中随访";
            }
            else
            {
                yunzhong.Name = name;
            }

            yunzhong = this.YunZhongRepository.SaveOrUpdate(yunzhong);

            return JsonSuccess(yunzhong);
        }

        [Transaction]
        public ActionResult YunZhongDelete(int yunzhongid)
        {
            try
            {
                var obj = this.YunZhongRepository.Get(yunzhongid);

                this.YunZhongRepository.Delete(obj);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult YunZhongSave(YunZhong yunzhong)
        {
            try
            {
                if (yunzhong == null)
                {
                    throw new Exception("无效数据");
                }

                if (yunzhong.Id < 1)
                {
                    throw new Exception("信息不存在");
                }

                yunzhong = this.YunZhongRepository.Get(yunzhong.Id);

                UpdateModel(yunzhong);

                this.YunZhongRepository.SaveOrUpdate(yunzhong);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
            return JsonSuccess();
        }

        [Transaction]
        public ActionResult YunZhongUpdate(int yunzhongid, int? doctorid)
        {
            try
            {
                var obj = this.YunZhongRepository.Get(yunzhongid);

                if (doctorid.HasValue)
                {
                    obj.Doctor = this.DoctorRepository.Get(doctorid.Value);

                    this.YunZhongRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult YunZhongUpdateName(int followupid, string name)
        {
            try
            {
                var obj = this.YunZhongRepository.Get(followupid);

                if (obj != null && name != null && name.Length > 0)
                {
                    obj.Name = name;
                    this.YunZhongRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
        #endregion

        #region 产后随访
        public ActionResult ChanHou(int id)
        {
            var patient = this.PatientRepository.Get(id);

            return View(patient);
        }

        public ActionResult ChanHouList(GdFollowUpQuery query)
        {
            var list = this.ChanHouRepository.GetList(query);

            var data = list.Data.Select(c => ChanHouModel.From(c)).ToList(); ;

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        public ActionResult ChanHouDetail(int chanhouid)
        {
            var chanhou = this.ChanHouRepository.Get(chanhouid);

            return View(chanhou);
        }

        public ActionResult ChanHouZhiBiao(int chanhouid)
        {
            var chanhou = this.ChanHouRepository.Get(chanhouid);

            return View(chanhou);
        }

        [Transaction]
        public ActionResult ChanHouAdd(int patientid,string name)
        {
            var patient = this.PatientRepository.Get(patientid);

            var chanhou = new ChanHou();
            
            chanhou.Patient = patient;
            chanhou.Doctor = patient.Doctor;
            chanhou.PracticeDoctor = CurrentUser;

            if (name.IsNullOrEmpty())
            {
                chanhou.Name = chanhou.CreateTime.ToString("yyyy-MM-dd") + "产后随访";
            }
            else
            {
                chanhou.Name = name;
            }

            chanhou = this.ChanHouRepository.SaveOrUpdate(chanhou);

            return JsonSuccess(chanhou);
        }

        [Transaction]
        public ActionResult ChanHouDelete(int chanhouid)
        {
            try
            {
                var obj = this.ChanHouRepository.Get(chanhouid);

                this.ChanHouRepository.Delete(obj);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult ChanHouSave(ChanHou chanhou)
        {
            try
            {
                if (chanhou == null)
                {
                    throw new Exception("无效数据");
                }

                if (chanhou.Id < 1)
                {
                    throw new Exception("信息不存在");
                }

                chanhou = this.ChanHouRepository.Get(chanhou.Id);

                UpdateModel(chanhou);

                this.ChanHouRepository.SaveOrUpdate(chanhou);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
            return JsonSuccess();
        }

        [Transaction]
        public ActionResult ChanHouUpdate(int chanhouid, int? doctorid)
        {
            try
            {
                var obj = this.ChanHouRepository.Get(chanhouid);

                if (doctorid.HasValue)
                {
                    obj.Doctor = this.DoctorRepository.Get(doctorid.Value);

                    this.ChanHouRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult ChanHouUpdateName(int followupid, string name)
        {
            try
            {
                var obj = this.ChanHouRepository.Get(followupid);

                if (obj != null && name != null && name.Length > 0)
                {
                    obj.Name = name;
                    this.ChanHouRepository.SaveOrUpdate(obj);
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #endregion

        #endregion

        #region 曲线图

        public ActionResult ChartLine(int id)
        {
            var patient = this.PatientRepository.Get(id);

            return View(patient);
        }

        public ActionResult GetChartDatas(int patientid,int type)
        {
            IList<ChartModel> list = new List<ChartModel>();

            if (type == 1)
            {
                var datas = this.TnbBasicRepository.GetListBy(patientid);
                list = datas.Select(c => (new ChartModel()
                {
                    Date = c.ExamDate,
                    Param1 = c.HbA1c
                })).ToList();
            }
            else if (type == 2)
            {
                var datas = this.TnbBasicRepository.GetListBy(patientid);
                list = datas.Select(c => (new ChartModel()
                {
                    Date = c.ExamDate,
                    Param1 = c.BloodPressureHigh,
                    Param2 = c.BloodPressureLow
                })).ToList();
            }
            else if (type == 3)
            {
                var datas = this.BloodRtRepository.GetListBy(patientid);
                list = datas.Select(c => (new ChartModel()
                {
                    Date = c.ExamDate,
                    Param1 = c.LDL
                })).ToList();
            }
           
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

    public class FuZhenModel
    {
        public FuZhenModel()
        {
        }

        public FuZhenModel(FuZhen fuzhen)
        {
            this.FuZhenId = fuzhen.Id;
            this.Name = fuzhen.Name;
            this.CreateTimeString = fuzhen.CreateTime.ToString("yyyy-M-d");
            this.DoctorName = fuzhen.Doctor == null ? "" : fuzhen.Doctor.RealName;
            this.DiseaseStageName = fuzhen.DiseaseStage.ToString();
        }

        public int FuZhenId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 就诊日期
        /// </summary>
        public String CreateTimeString { get; set; }

        /// <summary>
        /// 责任医生
        /// </summary>
        public String DoctorName { get; set; }

        /// <summary>
        /// 病程阶段
        /// </summary>
        public String DiseaseStageName { get; set; }


        public static FuZhenModel From(FuZhen fuzhen)
        {
            return new FuZhenModel(fuzhen);
        }

    }

    public class TiaoZhengModel
    {
        public TiaoZhengModel()
        {
        }

        public TiaoZhengModel(TiaoZheng tiaozheng)
        {
            this.TiaoZhengId = tiaozheng.Id;
            this.Name = tiaozheng.Name;
            this.CreateTimeString = tiaozheng.CreateTime.ToString("yyyy-M-d");
            this.DoctorName = tiaozheng.Doctor == null ? "" : tiaozheng.Doctor.RealName;
        }

        public int TiaoZhengId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 访视日期
        /// </summary>
        public String CreateTimeString { get; set; }

        /// <summary>
        /// 责任医生
        /// </summary>
        public String DoctorName { get; set; }


        public static TiaoZhengModel From(TiaoZheng tiaozheng)
        {
            return new TiaoZhengModel(tiaozheng);
        }

    }

    public class AnnualModel
    {
        public AnnualModel()
        {
        }

        public AnnualModel(Annual annual)
        {
            this.AnnualId = annual.Id;
            this.Name = annual.Name;
            this.CreateTimeString = annual.CreateTime.ToString("yyyy-M-d");
        }

        public int AnnualId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 评估日期
        /// </summary>
        public String CreateTimeString { get; set; }

        public static AnnualModel From(Annual annual)
        {
            return new AnnualModel(annual);
        }
    }

    public class YunZhongModel
    {
        public YunZhongModel()
        {
        }

        public YunZhongModel(YunZhong yunZhong)
        {
            this.YunZhongId = yunZhong.Id;
            this.Name = yunZhong.Name;
            this.CreateTimeString = yunZhong.CreateTime.ToString("yyyy-M-d");
            this.PregnancyWeeks = yunZhong.PregnancyWeeks;
            this.BloodPressure = String.Format("{0}/{1}",yunZhong.BloodPressureHigh,yunZhong.BloodPressureLow);
            this.DoctorName = yunZhong.Doctor == null ? "" : yunZhong.Doctor.RealName;
        }

        public int YunZhongId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 随访日期
        /// </summary>
        public String CreateTimeString { get; set; }

        /// <summary>
        /// 孕周
        /// </summary>
        public String PregnancyWeeks { get; set; }

        /// <summary>  
        /// 血压（mmHg）
        /// </summary>
        public String BloodPressure { get; set; }
        
        /// <summary>
        /// 责任医生
        /// </summary>
        public String DoctorName { get; set; }


        public static YunZhongModel From(YunZhong yunZhong)
        {
            return new YunZhongModel(yunZhong);
        }
    }
    
    public class ChanHouModel
    {
        public ChanHouModel()
        {
        }

        public ChanHouModel(ChanHou chanHou)
        {
            this.ChanHouId = chanHou.Id;
            this.Name = chanHou.Name;
            this.CreateTimeString = chanHou.CreateTime.ToString("yyyy-M-d");
            this.DoctorName = chanHou.Doctor == null ? "" : chanHou.Doctor.RealName;
        }

        public int ChanHouId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 随访日期
        /// </summary>
        public String CreateTimeString { get; set; }
        
        /// <summary>
        /// 责任医生
        /// </summary>
        public String DoctorName { get; set; }

        public static ChanHouModel From(ChanHou chanHou)
        {
            return new ChanHouModel(chanHou);
        }
    }

    public class ChartModel
    {
        public String Name { get; set; }
        public float Float1 { get; set; }
        public float Float2 { get; set; }
        public float Float3 { get; set; }
        public float Float4 { get; set; }
        public int Int1 { get; set; }
        
        public DateTime Date { get; set; }
        public String Param1 { get; set; }
        public String Param2 { get; set; }
    }

    public class NoticeModel
    {
        public NoticeModel()
        {
        }

        public NoticeModel(FollowUpInfo info)
        {
            if (info == null)
            {
                return;
            }

            if (info.Patient != null)
            {
                this.PatientId = info.Patient.Id;
                this.PatientCodeNo = info.Patient.CodeNo;
                this.PatientName = info.Patient.RealName;
                this.PatientIdCard = info.Patient.IdCard;
                this.PatientSex = info.Patient.Sex.ToString();
                this.PatientAge = info.Patient.Age;
                this.Mobile = info.Patient.Mobile1;
                this.DiabetesString = info.Patient.Diabetes == null ? "" : info.Patient.Diabetes.Name;
                this.DoctorName = info.Patient.Doctor == null ? "" : info.Patient.Doctor.RealName;
            }

            this.FollowUpDateString = info.FollowUpDate.ToString("yyyy-MM-dd");
            this.FollowUpWayString = info.FollowUpWay.ToString();
            this.FuZhenStatusString = info.FuZhenStatus.ToString();
        }

        /// <summary>
        /// 患者Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// 患者编号
        /// </summary>
        public String PatientCodeNo { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public String PatientName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public String PatientIdCard { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public String PatientSex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int PatientAge { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public String Mobile { get; set; }

        /// <summary>
        /// 糖尿病类型|诊断分型
        /// </summary>
        public String DiabetesString { get; set; }

        /// <summary>
        /// 下次复诊时间
        /// </summary>
        public String FollowUpDateString { get; set; }

        /// <summary>
        /// 随访方式
        /// </summary>
        public String FollowUpWayString { get; set; }
        
        /// <summary>
        /// 复诊状态
        /// </summary>
        public String FuZhenStatusString { get; set; }

        /// <summary>
        /// 主治医生
        /// </summary>
        public String DoctorName { get; set; }

        public static NoticeModel From(FollowUpInfo info)
        {
            return new NoticeModel(info);
        }
    }
}
