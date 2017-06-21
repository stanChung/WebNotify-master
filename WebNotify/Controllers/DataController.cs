using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;


namespace WebNotify.Controllers
{
    public class DataController : Controller
    {


        // GET: Data
        public ActionResult Index(string uid)
        {

            JobRepository job = new JobRepository();

            var data = job.GetData();
            //var rpt = data.Where(x => x.IsComplete && !x.IsNotified && x.ReportOwner == uid).FirstOrDefault();


            //if (rpt != null)
            //{

            //    NotificationHub.ShowCompleteNotify(rpt.ReportOwner, rpt.ReportName, rpt.DataID);
            //}

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}