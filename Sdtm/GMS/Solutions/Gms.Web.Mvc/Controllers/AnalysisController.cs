using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gms.Common;
using Gms.Domain;

namespace Gms.Web.Mvc.Controllers
{
    public class AnalysisController : BaseController
    {
        #region 指标检查分析
        public ActionResult Examine()
        {
            return View();
        }
        
        public ActionResult ExamineDatas()
        {
            //
            //当数据比较大时，此部分再进行优化
            //

            IList<ChartModel> list = new List<ChartModel>();

            var tnbBasicDatas = this.TnbBasicRepository.GetAll();
            var chartModel = new ChartModel();
            chartModel.Name = Exam.HbA1c.ToString();
            chartModel.Int1 = (int)Exam.HbA1c;
            chartModel.Float1 = tnbBasicDatas.Count;
            chartModel.Float2 = tnbBasicDatas.Count(c => (!c.HbA1c.IsNullOrEmpty()));
            chartModel.Float3 = (chartModel.Float2*100)/chartModel.Float1;
            //var datas1 = tnbBasicDatas.Where(c => (!c.HbA1c.IsNullOrEmpty())).ToList();
            //chartModel.Float2 = datas1.Count();
            //chartModel.Float3 = datas1.Average(c => (float.Parse(c.HbA1c)));
            list.Add(chartModel);
            
            chartModel = new ChartModel();
            chartModel.Name = Exam.舒张压.ToString();
            chartModel.Int1 = (int) Exam.舒张压;
            chartModel.Float1 = tnbBasicDatas.Count;
            chartModel.Float2 = tnbBasicDatas.Count(c => (!c.BloodPressureLow.IsNullOrEmpty()));
            chartModel.Float3 = (chartModel.Float2 * 100) / chartModel.Float1;
            list.Add(chartModel);

            chartModel = new ChartModel();
            chartModel.Name = Exam.收缩压.ToString();
            chartModel.Int1 = (int) Exam.收缩压;
            chartModel.Float1 = tnbBasicDatas.Count;
            chartModel.Float2 = tnbBasicDatas.Count(c => (!c.BloodPressureHigh.IsNullOrEmpty()));
            chartModel.Float3 = (chartModel.Float2 * 100) / chartModel.Float1;
            list.Add(chartModel);

            var datas2 = this.BloodRtRepository.GetAll();
            chartModel = new ChartModel();
            chartModel.Name = Exam.LDL.ToString();
            chartModel.Int1 = (int) Exam.LDL;
            chartModel.Float1 = datas2.Count;
            chartModel.Float2 = datas2.Count(c => (!c.LDL.IsNullOrEmpty()));
            chartModel.Float3 = (chartModel.Float2 * 100) / chartModel.Float1;
            list.Add(chartModel);

            var datas3 = this.UroscopyRepository.GetAll();
            chartModel = new ChartModel();
            chartModel.Name = Exam.尿微量白蛋白.ToString();
            chartModel.Int1 = (int) Exam.尿微量白蛋白;
            chartModel.Float1 = datas3.Count;
            datas3 = datas3.Where(c => (!c.mALB.IsNullOrEmpty())).ToList();
            chartModel.Float2 = datas3.Count();
            chartModel.Float3 = (chartModel.Float2 * 100) / chartModel.Float1;
            list.Add(chartModel);

            var eyeDatas = this.EyeRepository.GetAll();
            chartModel = new ChartModel();
            chartModel.Name = Exam.眼睛.ToString();
            chartModel.Int1 = (int) Exam.眼睛;
            chartModel.Float1 = eyeDatas.Count;
            chartModel.Float2 = eyeDatas.Count(c => (c.CataractL != YesNoUncheck.未查));
            chartModel.Float3 = (chartModel.Float2 * 100) / chartModel.Float1;
            list.Add(chartModel);
            
            var legsDatas = this.LegsRepository.GetAll();
            chartModel = new ChartModel();
            chartModel.Name = Exam.下肢血管及神经病变.ToString();
            chartModel.Int1 = (int) Exam.下肢血管及神经病变;
            chartModel.Float1 = legsDatas.Count;
            chartModel.Float2 = legsDatas.Count(c => (!c.UpperBaspL.IsNullOrEmpty() || !c.XianhAsbpL.IsNullOrEmpty() || !c.DpaspL.IsNullOrEmpty()));
            chartModel.Float3 = (chartModel.Float2 * 100) / chartModel.Float1;
            list.Add(chartModel);
            
            chartModel = new ChartModel();
            chartModel.Name = Exam.感觉阈值.ToString();
            chartModel.Int1 = (int) Exam.感觉阈值;
            chartModel.Float1 = legsDatas.Count;
            chartModel.Float2 = legsDatas.Count(c => (!c.TouchFeelingL.IsNullOrEmpty() || !c.TouchFeelingR.IsNullOrEmpty()));
            chartModel.Float3 = (chartModel.Float2 * 100) / chartModel.Float1;
            list.Add(chartModel);


            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExamineItem(Exam exam)
        {
            return View(exam);
        }

        public ActionResult ExamineItemDatas(Exam exam)
        {
            IList<ChartModel> list = new List<ChartModel>();
            
            switch (exam)
            {
                case Exam.HbA1c:
                {
                    var tnbBasicDatas = this.TnbBasicRepository.GetAll();
                    var data =
                        tnbBasicDatas.Where(c => (!c.HbA1c.IsNullOrEmpty()))
                            .Select(c => (float.Parse(c.HbA1c)))
                            .ToList();

                    float fLen = data.Count;
                    if (fLen < 1)
                        break;

                    var chartModel = new ChartModel();
                    chartModel.Name = "<7%";
                    chartModel.Float1 = data.Count(c => (c < 7));
                    chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                    list.Add(chartModel);

                    chartModel = new ChartModel();
                    chartModel.Name = "7%～8%";
                    chartModel.Float1 = data.Count(c => (c >= 7 && c<=8));
                    chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                    list.Add(chartModel);

                    chartModel = new ChartModel();
                    chartModel.Name = ">8%";
                    chartModel.Float1 = data.Count(c => (c > 7));
                    chartModel.Float2 = (chartModel.Float1 * 100) / fLen;

                    list.Add(chartModel);

                }
                    break;
                case Exam.收缩压:
                    {
                        var datas1 = this.TnbBasicRepository.GetAll();
                        var datas = datas1.Where(c => (!c.BloodPressureHigh.IsNullOrEmpty()))
                                .Select(c => (float.Parse(c.BloodPressureHigh)))
                                .ToList();

                        float fLen = datas.Count;
                        if (fLen < 1)
                            break;

                        var chartModel = new ChartModel();
                        chartModel.Name = "达标（<130）";
                        chartModel.Float1 = datas.Count(c => (c < 130));
                        chartModel.Float2 = chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                        list.Add(chartModel);

                        chartModel = new ChartModel();
                        chartModel.Name = "未达标（>=130）";
                        chartModel.Float1 = datas.Count(c => (c >= 130));
                        chartModel.Float2 = chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                        list.Add(chartModel);
                    }
                    break;
                case Exam.舒张压:
                    {
                        var datas1 = this.TnbBasicRepository.GetAll();
                        var datas = datas1.Where(c => (!c.BloodPressureLow.IsNullOrEmpty()))
                                .Select(c => (float.Parse(c.BloodPressureLow)))
                                .ToList();

                        float fLen = datas.Count;
                        if (fLen < 1)
                            break;

                        var chartModel = new ChartModel();
                        chartModel.Name = "达标（<80）";
                        chartModel.Float1 = datas.Count(c => (c < 80));
                        chartModel.Float2 = chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                        list.Add(chartModel);

                        chartModel = new ChartModel();
                        chartModel.Name = "未达标（>=80）";
                        chartModel.Float1 = datas.Count(c => (c >= 80));
                        chartModel.Float2 = chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                        list.Add(chartModel);
                    }
                    break;
                case Exam.LDL:
                {
                    var datas1 = this.BloodRtRepository.GetAll();
                    var datas = datas1.Where(c => (!c.LDL.IsNullOrEmpty()))
                            .Select(c => (float.Parse(c.LDL)))
                            .ToList();

                    float fLen = datas.Count;
                    if (fLen < 1)
                        break;

                    var chartModel = new ChartModel();
                    chartModel.Name = "达标";
                    chartModel.Float1 = datas.Count(c => ( c < 2.6));
                    chartModel.Float2 = chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                    list.Add(chartModel);

                    chartModel = new ChartModel();
                    chartModel.Name = "未达标";
                    chartModel.Float1 = datas.Count(c => (c >= 2.6));
                    chartModel.Float2 = chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                    list.Add(chartModel);
                }
                    break;
                case Exam.尿微量白蛋白:
                    {
                        var datas1 = this.UroscopyRepository.GetAll();
                        var datas = datas1.Where(c => (!c.mALB.IsNullOrEmpty()))
                                .Select(c => (float.Parse(c.mALB)))
                                .ToList();

                        float fLen = datas.Count;
                        if (fLen < 1)
                            break;

                        var chartModel = new ChartModel();
                        chartModel.Name = "达标（<37）";
                        chartModel.Float1 = datas.Count(c => (c < 37));
                        chartModel.Float2 = chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                        list.Add(chartModel);

                        chartModel = new ChartModel();
                        chartModel.Name = "未达标（>=37）";
                        chartModel.Float1 = datas.Count(c => (c >= 37));
                        chartModel.Float2 = chartModel.Float2 = (chartModel.Float1 * 100) / fLen;
                        list.Add(chartModel);
                    }
                    break;
                default: break;
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExamineItemDatas2(Exam exam)
        {
            IList<ChartModel> list = new List<ChartModel>();

            //最高值，平均值，参考值，最低值，
            

            switch (exam)
            {
                case Exam.HbA1c:
                {
                    var tnbBasicDatas = this.TnbBasicRepository.GetAll();
                    var data =
                        tnbBasicDatas.Where(c => (!c.HbA1c.IsNullOrEmpty()))
                            .Select(c => (float.Parse(c.HbA1c)))
                            .ToList();
                    data.Sort();
                    int nLen = data.Count;
                    if (nLen > 0)
                    {
                        var chartModel = new ChartModel();
                        chartModel.Name = Exam.HbA1c.ToString();
                        chartModel.Float1 = data[nLen - 1];
                        chartModel.Float2 = data.Average();
                        chartModel.Float3 = 7;
                        chartModel.Float4 = data[0];

                        list.Add(chartModel);
                    }
                }
                    break;
                case Exam.收缩压:
                    {
                        var datas1 = this.TnbBasicRepository.GetAll();
                        var datas =
                            datas1.Where(c => (!c.BloodPressureHigh.IsNullOrEmpty()))
                                .Select(c => (float.Parse(c.BloodPressureHigh)))
                                .ToList();
                        datas.Sort();
                        int nLen = datas.Count;
                        if (nLen > 0)
                        {
                            var chartModel = new ChartModel();
                            chartModel.Name = Exam.收缩压.ToString();
                            chartModel.Float1 = datas[nLen - 1];
                            chartModel.Float2 = datas.Average();
                            chartModel.Float3 = 130;
                            chartModel.Float4 = datas[0];

                            list.Add(chartModel);
                        }
                    }
                    break;
                case Exam.舒张压:
                    {
                        var datas1 = this.TnbBasicRepository.GetAll();
                        var datas =
                            datas1.Where(c => (!c.BloodPressureLow.IsNullOrEmpty()))
                                .Select(c => (float.Parse(c.BloodPressureLow)))
                                .ToList();
                        datas.Sort();
                        int nLen = datas.Count;
                        if (nLen > 0)
                        {
                            var chartModel = new ChartModel();
                            chartModel.Name = Exam.舒张压.ToString();
                            chartModel.Float1 = datas[nLen - 1];
                            chartModel.Float2 = datas.Average();
                            chartModel.Float3 = 80;
                            chartModel.Float4 = datas[0];

                            list.Add(chartModel);
                        }
                    }
                    break;
                case Exam.LDL:
                    {
                        var datas1 = this.BloodRtRepository.GetAll();
                        var datas =
                            datas1.Where(c => (!c.LDL.IsNullOrEmpty()))
                                .Select(c => (float.Parse(c.LDL)))
                                .ToList();
                        datas.Sort();
                        int nLen = datas.Count;
                        if (nLen > 0)
                        {
                            var chartModel = new ChartModel();
                            chartModel.Name = Exam.LDL.ToString();
                            chartModel.Float1 = datas[nLen - 1];
                            chartModel.Float2 = datas.Average();
                            chartModel.Float3 = (float)2.6;
                            chartModel.Float4 = datas[0];

                            list.Add(chartModel);
                        }
                    }
                    break;
                case Exam.尿微量白蛋白:
                    {
                        var datas1 = this.UroscopyRepository.GetAll();
                        var datas =
                            datas1.Where(c => (!c.mALB.IsNullOrEmpty()))
                                .Select(c => (float.Parse(c.mALB)))
                                .ToList();
                        datas.Sort();
                        int nLen = datas.Count;
                        if (nLen > 0)
                        {
                            var chartModel = new ChartModel();
                            chartModel.Name = Exam.尿微量白蛋白.ToString();
                            chartModel.Float1 = datas[nLen - 1];
                            chartModel.Float2 = datas.Average();
                            chartModel.Float3 = 37;
                            chartModel.Float4 = datas[0];

                            list.Add(chartModel);
                        }
                    }
                    break;
                default:break;
            }
            
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 管理尽责度

        public ActionResult FollowUp()
        {
            ViewData["PatientTotal"] =this.PatientRepository.GetAll().Count;//患者总数
            var datas = this.FollowUpInfoRepository.GetAll();
            ViewData["FollowUpTotal"] = datas.Count;//参加随访人数
            ViewData["FollowUp_Normal"] = datas.Count(c=>c.FollowUpStatus == FollowUpStatus.参加随访);//正常随访
            ViewData["FollowUp_Pause"] = datas.Count(c => c.FollowUpStatus == FollowUpStatus.暂时离开超过六个月);//暂停
            ViewData["FollowUp_Refuse"] = datas.Count(c => c.FollowUpStatus == FollowUpStatus.拒绝随访);//拒绝访问
            ViewData["FollowUp_Off"] = datas.Count(c => c.FollowUpStatus == FollowUpStatus.失访);//失访
            return View();
        }

        #endregion
    }
}