using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Gms.Common;
using Gms.Domain;
using SharpArch.NHibernate.Web.Mvc;

namespace Gms.Web.Mvc.Controllers
{
    public class PatientController : BaseController
    {
        public ActionResult Index(DiabetesType type)
        {
            return View(type);
        }

        public ActionResult Add(DiabetesType type)
        {
            ViewData["CodeNo"] = "";
            var item = this.CommonCodeRepository.GetBy(CommonCodeType.最新患者编号);
            if (item != null)
            {
                ViewData["CodeNo"] = item.Name;
            }
            
            return View(type);
        }

        public ActionResult List(PatientQuery query)
        {
            if (CurrentUser != null)
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

            var list = this.PatientRepository.GetList(query);
            var data = new List<PatientModel>();
            foreach (var item in list.Data)
            {
                PatientModel tm = PatientModel.From(item);
               
                var followUpInfo = this.FollowUpInfoRepository.GetBy(item.Id);
                if (followUpInfo != null)
                {
                    tm.FollowUpStatusString = followUpInfo.FollowUpStatus.ToString();
                    tm.FollowUpDateString = followUpInfo.FollowUpDate.ToString("yyyy-MM-dd");
                }
                
                data.Add(tm);
            }
            //var data = list.Data.Select(c => PatientModel.From(c)).ToList();
            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        public ActionResult Get(int id)
        {
            var patient = this.PatientRepository.Get(id);
            return JsonSuccess(patient ?? (new Patient()));
        }

        [Transaction]
        public ActionResult Delete(int id)
        {
            var item = PatientRepository.Get(id);
            if (item != null)
            {
                PatientRepository.Delete(item);
            }

            return JsonSuccess();
        }
        
        [Transaction]
        public ActionResult CreateOrUpdate(Patient patient)
        {
            try
            {
                if (patient.Id <= 0)
                {
                    if (patient.IdCard.IsNullOrEmpty())
                    {
                        throw new Exception("身份证号不能为空");
                    }
                    patient.IdCard = patient.IdCard.Trim();

                    if (patient.CodeNo.IsNullOrEmpty())
                    {
                        throw new Exception("编号不能为空");
                    }
                    patient.CodeNo = patient.CodeNo.Trim();

                    if (patient.RealName.IsNullOrEmpty())
                    {
                        throw new Exception("姓名不能为空");
                    }
                    patient.RealName = patient.RealName.Trim();

                    //糖尿病类型
                    if (patient.DiabetesType == DiabetesType.妊娠糖尿病)
                    {
                        patient.Diabetes = this.CommonCodeRepository.GetGdType();
                    }

                    //
                    //判断身份证号是否已经存在
                    var user = this.PatientRepository.Get(patient.IdCard);
                    if (user != null)
                    {
                        throw new Exception("身份证号已经存在");
                    }

                    //
                    //判断患者编号是否已经存在
                    user = this.PatientRepository.GetByCodeNo(patient.CodeNo);
                    if (user != null)
                    {
                        throw new Exception("患者编号已经存在");
                    }
                    
                    patient.CreateTime = DateTime.Now;
                }
                else
                {
                    patient = this.PatientRepository.Get(patient.Id);

                    TryUpdateModel(patient);
                }

                patient.Doctor = CurrentUser;

                patient = this.PatientRepository.SaveOrUpdate(patient);

                //
                //修改最新患者编号
                int nNewCode = int.Parse(patient.CodeNo);
                nNewCode++;
                var item = this.CommonCodeRepository.GetBy(CommonCodeType.最新患者编号);
                if (item == null)
                {
                    item = new CommonCode();
                    item.Type = CommonCodeType.最新患者编号;
                }
                item.Name = string.Format("{0}", nNewCode);
                this.CommonCodeRepository.SaveOrUpdate(item);

                return JsonSuccess(patient.Id);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult UpdateDoctor(int id,int doctorid)
        {
            try
            {
                var patient = this.PatientRepository.Get(id);

                patient.Doctor = this.DoctorRepository.Get(doctorid);

                this.PatientRepository.SaveOrUpdate(patient);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [Transaction]
        public ActionResult UpdateDiabetesType(int id, int diabetesid)
        {
            try
            {
                var destType = this.CommonCodeRepository.Get(diabetesid);

                var origType = this.CommonCodeRepository.GetGdType();

                if (origType.Id == destType.Id)
                {
                    return JsonError("无效转型");
                }

                var patient = this.PatientRepository.Get(id);

                patient.Diabetes = destType;
                patient.DiabetesType = DiabetesType.普通糖尿病;

                this.PatientRepository.SaveOrUpdate(patient);

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
    }

    public class PatientModel
    {

        public int Id { get; set; }

        /// <summary>
        /// 患者编号
        /// </summary>
        public String CodeNo { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public String RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public String SexString { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public String CreateTime { get; set; }
        
        /// <summary>
        /// 身份证号
        /// </summary>
        public String IdCard { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public String BirthdayString { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public String Email { get; set; }
        
        /// <summary>
        /// 诊断日期
        /// </summary>
        public String DiagnoseDateString { get; set; }

        /// <summary>
        /// 糖尿病类型|诊断分型
        /// </summary>
        public String DiabetesString { get; set; }

        /// <summary>
        /// 病程
        /// </summary>
        public String DiseaseCourse { get; set; }

        /// <summary>
        /// 病程阶段
        /// </summary>
        public String DiseaseStageString { get; set; }

        /// <summary>
        /// 随访状态
        /// </summary>
        public String FollowUpStatusString { get; set; }

        /// <summary>
        /// 随访时间
        /// </summary>
        public String FollowUpDateString { get; set; }

        /// <summary>
        /// 是否健在
        /// </summary>
        public String IsBreathingString { get; set; }

        public PatientModel()
        {

        }

        public PatientModel(Patient patient)
        {
            this.Id = patient.Id;

            this.CodeNo = patient.CodeNo;

            this.RealName = patient.RealName;

            this.SexString = patient.Sex.ToString();
            
            this.Note = patient.Note;

            this.CreateTime = patient.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");

            this.IdCard = patient.IdCard;

            this.BirthdayString = patient.Birthday.ToString("yyyy-MM-dd HH:mm:ss");

            this.Age = patient.Age;

            this.Email = patient.Email;

            //this.DiagnoseDateString = patient.DiagnoseDate.ToString("yyyy-MM-dd HH:mm:ss");

            this.DiabetesString = "";
            if (patient.Diabetes != null)
                this.DiabetesString = patient.Diabetes.Name;

            this.DiseaseCourse = patient.DiseaseCourse;

            this.DiseaseStageString = patient.DiseaseStage.ToString();

            this.FollowUpStatusString = "";
            this.FollowUpDateString = "未设置";
            //if (patient.FollowUpInfo != null)
            //{
            //    this.FollowUpStatusString = patient.FollowUpInfo.FollowUpStatus.ToString();
            //    this.FollowUpDateString = patient.FollowUpInfo.FollowUpDate.ToString("yyyy-MM-dd HH:mm:ss");
            //}

            //this.IsBreathingString = patient.IsBreathing.ToString();
        }

        public static PatientModel From(Patient patient)
        {
            return new PatientModel(patient);
        }
    }

}
