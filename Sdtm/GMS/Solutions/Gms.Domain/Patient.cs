using System;
using Gms.Common;
using Gms.Domain.Attribute;
using Gms.Domain.FollowUp;
using SharpArch.Domain.DomainModel;

namespace Gms.Domain
{
    public class Patient:Entity
    {
        public Patient()
        {
            this.Birthday = DateTimeEx.Default();
            this.CreateTime = DateTime.Now;
            this.DiagnoseDate = DateTimeEx.Default();
        }

        /// <summary>
        /// 患者编号
        /// </summary>
        public virtual String CodeNo { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual String RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual Sex Sex { get; set; }
        
        /// <summary>
        /// 身份证号
        /// </summary>
        public virtual String IdCard { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public virtual DateTime Birthday { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [NotMap]
        public virtual int Age
        {
            get
            {
                int year = DateTime.Now.Year - Birthday.Year;
                //int month = DateTime.Now.m
                return year;
            }
        }

        /// <summary>
        /// 省/市（籍贯）
        /// </summary>
        public virtual CommonCode Area { get; set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual String Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public virtual String Mobile1 { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public virtual String Mobile2 { get; set; }

        /// <summary>
        /// 糖尿病大类
        /// </summary>
        public virtual DiabetesType DiabetesType { get; set; }

        /// <summary>
        /// 糖尿病类型|诊断分型
        /// </summary>
        [FieldNeed]
        public virtual CommonCode Diabetes { get; set; }

        /// <summary>
        /// 诊断日期
        /// </summary>
        [FieldNeed(9999)]
        public virtual DateTime DiagnoseDate { get; set; }

        /// <summary>
        /// 病程
        /// </summary>
        [NotMap]
        public virtual String DiseaseCourse
        {
            get
            {
                String str = "";
                int nYear = DateTime.Now.Year - DiagnoseDate.Year;
                int nMonth = DateTime.Now.Month - DiagnoseDate.Month;
                if (nMonth < 0)
                {
                    nYear -= 1;

                    nMonth = DateTime.Now.Month + 12 - DiagnoseDate.Month;
                }

                if (nYear > 0)
                {
                    str = String.Format("{0}年", nYear);
                }

                if (nMonth > 0)
                {
                    str += String.Format("{0}个月", nMonth);
                }


                return str;
            }
        }

        /// <summary>
        /// 病程阶段
        /// </summary>
        [FieldNeed]
        public virtual DiseaseStage DiseaseStage { get; set; }

        /// <summary>
        /// 主治医生
        /// </summary>
        public virtual Doctor Doctor { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Note { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }

    public class PatientQuery : QueryBase
    {
        /// <summary>
        /// 患者 ID
        /// </summary>
        public int? PatientId { get; set; }

        /// <summary>
        /// 患者 IDs
        /// </summary>
        public String PatientIds { get; set; }

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
        public Sex? Sex { get; set; }
        
        /// <summary>
        /// 手机号码
        /// </summary>
        public String Mobile { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public String IdCard { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public Range<DateTime?> Birthday { get; set; }

        /// <summary>
        /// 省/市（籍贯）
        /// </summary>
        public int? AreaId { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public int? NationId { get; set; }
        
        /// <summary>
        /// 诊断日期
        /// </summary>
        public Range<DateTime?> DiagnoseDate { get; set; }

        /// <summary>
        /// 糖尿病大类
        /// </summary>
        public DiabetesType? DiabetesType { get; set; }

        /// <summary>
        /// 糖尿病类型|诊断分型
        /// </summary>
        public int? DiabetesId { get; set; }

        /// <summary>
        /// 病程阶段
        /// </summary>
        public virtual DiseaseStage? DiseaseStage { get; set; }

        /// <summary>
        /// 是否健在
        /// </summary>
        public virtual YesNo? IsBreathing { get; set; }

        /// <summary>
        /// 主治医生
        /// </summary>
        public virtual int? DoctorId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public Range<DateTime?> CreateTime { get; set; }
    }
}
