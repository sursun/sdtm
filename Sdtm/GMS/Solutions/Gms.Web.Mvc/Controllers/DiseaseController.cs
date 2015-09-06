using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gms.Domain;
using SharpArch.NHibernate.Web.Mvc;

namespace Gms.Web.Mvc.Controllers
{
    public class DiseaseController : BaseController
    {
        public ActionResult Index()
        {
            return View(new Disease());
        }

        public ActionResult List(DiseaseQuery query)
        {
            var list = this.DiseaseRepository.GetList(query);
            var data = list.Data.Select(disease => DiseaseModel.From(disease)).ToList();
           
            return Json(data);
        }

        [Transaction]
        public ActionResult Delete(int id)
        {
            var item = DiseaseRepository.Get(id);
            if (item != null)
            {
                DiseaseRepository.Delete(item);
            }

            return JsonSuccess();
        }

        public ActionResult Get(int id)
        {
            var disease = this.DiseaseRepository.Get(id);
            return JsonSuccess(disease == null ? (new Disease()) : disease);
        }

        [Transaction]
        public ActionResult SaveOrUpdate(Disease disease)
        {
            if (disease.Id > 0)
            {
                disease = this.DiseaseRepository.Get(disease.Id);

                TryUpdateModel(disease);
            }

            this.DiseaseRepository.SaveOrUpdate(disease);

            return JsonSuccess(disease);
        }

        public ActionResult GetDisease(int id)
        {
            var disease = this.DiseaseRepository.Get(id);
            if (disease != null)
            {
                return JsonSuccess(DiseaseModel.From(disease));
            }

            return JsonError("未找到疾病信息");
        }

        public ActionResult AutocompleteGetDisease(string q)
        {
            var list = this.DiseaseRepository.GetList(new DiseaseQuery() {PinYin = q,PageIndex = 1,PageSize = 10});

            IList<AutocompleteItem> data = list.Data.Select(disease => new AutocompleteItem()
            {
                label = disease.Name, 
                value = disease.Id.ToString(),
                param1 = disease.CodeNo,
                param2 = disease.Type==null?"":disease.Type.Name
            }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        } 

    }

    public class DiseaseModel
    {
        public DiseaseModel()
        {
        }

        public DiseaseModel(Disease disease)
        {
            this.Id = disease.Id;
            this.TypeString = disease.Type==null?"":disease.Type.Name;
            this.Name = disease.Name;
            this.CodeNo = disease.CodeNo;
            this.PinYin = disease.PinYin;
        }

        public int Id { get; set; }

        /// <summary>
        /// 疾病类型
        /// </summary>
        public String TypeString { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 国际码
        /// </summary>
        public String CodeNo { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        public String PinYin { get; set; }

        public static DiseaseModel From(Disease disease)
        {
            return new DiseaseModel(disease);
        }

    }
    
}