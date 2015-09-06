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
    public class DoctorController : BaseController
    {
        //
        // GET: /Doctor/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Select()
        {
            return View();
        }

        public ActionResult List(DoctorQuery query)
        {
            var list = this.DoctorRepository.GetList(query);
            var data = list.Data.Select(c => DoctorModel.From(c)).ToList();
            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        public ActionResult Get(int id)
        {
            var doctor = this.DoctorRepository.Get(id);
            return JsonSuccess(doctor ?? (new Doctor()));
        }

        [Transaction]
        public ActionResult Delete(int id)
        {
            var item = DoctorRepository.Get(id);
            if (item != null)
            {
                Membership.DeleteUser(item.LoginName);

                DoctorRepository.Delete(item);
            }

            return JsonSuccess();
        }

        [Transaction]
        public ActionResult CreateOrUpdate(Doctor doctor, string psw)
        {
            try
            {
                if (doctor.Id <= 0)
                {
                    string strUserName = doctor.CodeNo.Trim();

                    MembershipUser membershipuser = Membership.GetUser(strUserName);

                    if (membershipuser != null)
                    {
                        throw new Exception("工号[" + strUserName + "]已经存在!");
                    }

                    membershipuser = Membership.CreateUser(strUserName, psw);

                    doctor.MemberShipId = (Guid)membershipuser.ProviderUserKey;
                    doctor.CreateTime = membershipuser.CreationDate;
                    doctor.LoginName = doctor.CodeNo;
                }
                else
                {
                    doctor = this.DoctorRepository.Get(doctor.Id);

                    TryUpdateModel(doctor);
                }

                doctor = this.DoctorRepository.SaveOrUpdate(doctor);

                return JsonSuccess(doctor);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        public ActionResult ResetPassword(int id)
        {
            var user = this.DoctorRepository.Get(id);
            if (user == null)
                return JsonError("没有找到用户，请刷新后再试！");
            MembershipUser membershipUser = Membership.GetUser(user.LoginName);
            string tempPsw = membershipUser.ResetPassword();
            string defaultPassword = "123";
            try
            {
                membershipUser.ChangePassword(tempPsw, defaultPassword);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess(defaultPassword);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ChangePsw(string oldpsw,string newpsw)
        {
            MembershipUser membershipUser = Membership.GetUser(MembershipId);

            try
            {
                if (membershipUser == null)
                {
                    throw new Exception("没有找到用户");
                }

                bool bFlag = membershipUser.ChangePassword(oldpsw, newpsw);

                if (!bFlag)
                {
                    return JsonError("原密码不正确");
                }
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }

            return JsonSuccess();
        }

        #region 权限设置

        public ActionResult Scope(int id)
        {
            var doctor = this.DoctorRepository.Get(id);

            return View(doctor);
        }

        [Transaction]
        public ActionResult ScopeAdd(int id, int scopeid)
        {
            var doctor = this.DoctorRepository.Get(id);

            if (doctor == null)
            {
                return JsonError("医生不存在");
            }

            String strNew = String.Format("{0}|", scopeid);

            bool bFlag = false;

            if (doctor.Scope.IsNullOrEmpty())
            {
                doctor.Scope = String.Format("|{0}|", doctor.Id);
                bFlag = true;
            }

            if (!doctor.Scope.Contains(strNew))
            {
                doctor.Scope += strNew;
                bFlag = true;
            }

            if (bFlag)
            {
                this.DoctorRepository.SaveOrUpdate(doctor);
            }

            return JsonSuccess();
        }

        [Transaction]
        public ActionResult ScopeDelete(int id,int scopeid)
        {
            var doctor = this.DoctorRepository.Get(id);

            if (doctor == null)
            {
                return JsonError("医生不存在");
            }

            String strOld = String.Format("|{0}|", scopeid);

            doctor.Scope = doctor.Scope.Replace(strOld, "|");


            this.DoctorRepository.SaveOrUpdate(doctor);

            return JsonSuccess();
        }

        [Transaction]
        public ActionResult UpdateScopeType(int id, ScopeType type)
        {
            var doctor = this.DoctorRepository.Get(id);

            doctor.ScopeType = type;

            this.DoctorRepository.SaveOrUpdate(doctor);

            return JsonSuccess();
        }
  
        public ActionResult ScopeList(int id)
        {
            var list = this.DoctorRepository.GetSope(id);
            
            var data = list.Select(c => DoctorModel.From(c)).ToList();
            var result = new { total = list.Count, rows = data };
            return Json(result);
        }

        #endregion

    }

    public class DoctorModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 所属科室
        /// </summary>
        public String Department { get; set; }

        /// <summary>
        /// 职工编号
        /// </summary>
        public String CodeNo { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public String ProfessionalLevel { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public String Duty { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public String LoginName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public String RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public String SexString { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public String Mobile { get; set; }

        /// <summary>
        /// 启用|禁用
        /// </summary>
        public String EnabledString { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public String CreateTime { get; set; }

        public DoctorModel(Doctor doctor) 
        {
            this.Id = doctor.Id;

            this.Department = "";
            if (doctor.Department != null)
            {
                this.Department = doctor.Department.ParentString() + doctor.Department.Name;
            }
            
            this.CodeNo = doctor.CodeNo;
            this.ProfessionalLevel = doctor.ProfessionalLevel;
            this.Duty = doctor.Duty;
            this.LoginName = doctor.LoginName;
            this.RealName = doctor.RealName;
            this.SexString = doctor.Sex.ToString();
            this.Mobile = doctor.Mobile;
            this.EnabledString = doctor.Enabled.ToString();
            this.Note = doctor.Note;
            this.CreateTime = doctor.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static DoctorModel From(Doctor doctor)
        {
            return new DoctorModel(doctor);
        }
    }


}
