using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gms.Domain;
using SharpArch.NHibernate.Web.Mvc;

namespace Gms.Web.Mvc.Controllers
{
    public class MedicineController : BaseController
    {
        public ActionResult Index()
        {
            return View(new Medicine());
        }

        public ActionResult List(MedicineQuery query)
        {
            var list = this.MedicineRepository.GetList(query);
            var data = list.Data.Select(medicine => MedicineModel.From(medicine)).ToList();

            var result = new { total = list.RecordCount, rows = data };
            return Json(result);
        }

        [Transaction]
        public ActionResult Delete(int id)
        {
            var item = MedicineRepository.Get(id);
            if (item != null)
            {
                MedicineRepository.Delete(item);
            }

            return JsonSuccess();
        }

        public ActionResult Get(int id)
        {
            var medicine = this.MedicineRepository.Get(id);
            return JsonSuccess(medicine == null ? (new Medicine()) : medicine);
        }

        public ActionResult MedicineRecommend()
        {
            var list = this.MedicineRepository.GetAll(new MedicineQuery()
            {
                Recommend = YesNo.是
            });
            var data = list.OrderBy(c=>c.RecommendTime).ToList();

            return View(data);
        }

        [Transaction]
        public ActionResult SaveOrUpdate(Medicine medicine)
        {
            if (medicine.Id > 0)
            {
                medicine = this.MedicineRepository.Get(medicine.Id);

                TryUpdateModel(medicine);
            }

            this.MedicineRepository.SaveOrUpdate(medicine);

            return JsonSuccess(medicine);
        }

        [Transaction]
        public ActionResult UpdateRecommend(int id)
        {
            var medicine = this.MedicineRepository.Get(id);

            medicine.Recommend = medicine.Recommend == YesNo.是 ? YesNo.否 : YesNo.是;
            
            this.MedicineRepository.SaveOrUpdate(medicine);

            return JsonSuccess(medicine);
        }

        public ActionResult GetMedicine(int id)
        {
            var medicine = this.MedicineRepository.Get(id);
            if (medicine != null)
            {
                return JsonSuccess(MedicineModel.From(medicine));
            }

            return JsonError("未找到药品");
        }

        public ActionResult AutocompleteGetMedicine(string q)
        {
            var list = this.MedicineRepository.GetList(new MedicineQuery() { PinYin = q, PageIndex = 1, PageSize = 10 });

            IList<AutocompleteItem> data = list.Data.Select(medicine => new AutocompleteItem()
            {
                label = medicine.NormalName,
                value = medicine.Id.ToString()
            }).ToList();

   
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsage()
        {
            var data = (from Enum e in Enum.GetValues(typeof(Usage))
                    select new
                    {
                        UsageValue = e.ToString(),//Int32.Parse(Enum.Format(typeof(Usage), e, "d")),
                        UsageName = e.ToString()
                    }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        } 
    }

    public class MedicineModel
    {
        public MedicineModel()
        {
        }

        public MedicineModel(Medicine medicine)
        {
            this.Id = medicine.Id;
            this.MedicineTypeString = medicine.MedicineType == null ? "" : medicine.MedicineType.Name;
            this.NormalName = medicine.NormalName;
            this.ChemicalName = medicine.ChemicalName;
            this.PinYin = medicine.PinYin;
            this.Model = medicine.Model;
            this.IsRecommend = (int) medicine.Recommend;
            this.Note = medicine.Note;
            this.EnabledString = Enum.GetName(typeof (Enabled), medicine.Enabled);
        }

        public int Id { get; set; }

        /// <summary>
        /// 药物类型
        /// </summary>
        public String MedicineTypeString { get; set; }

        /// <summary>
        /// 常用名称
        /// </summary>
        public String NormalName { get; set; }

        /// <summary>
        /// 化学名称
        /// </summary>
        public String ChemicalName { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        public String PinYin { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public String Model { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public int IsRecommend { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 禁用|启用
        /// </summary>
        public String EnabledString { get; set; }

        public static MedicineModel From(Medicine medicine)
        {
            return new MedicineModel(medicine);
        }

    }
    
}