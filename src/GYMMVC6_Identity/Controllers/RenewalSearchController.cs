using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using GYMONE.Models;
using GYMONE.Repository;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;

namespace GYMONE.Controllers
{
    public class RenewalSearchController : Controller
    {
        IlReports objIRecepit;

        private IHostingEnvironment _hostingEnvironment;
        public RenewalSearchController(IConfiguration configuration,IHostingEnvironment hostingEnvironment)
        {
            objIRecepit = new ReportsMaster(configuration);
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult RenewalReport()
        {
            TempData["Message"] = null;
            return View(new RenewalSearch());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenewalReport(RenewalSearch obj, string actionType)
        {

            if (string.Equals(actionType, "Download"))
            {
                if (!string.IsNullOrEmpty(obj.Exactdate))
                {
                    obj.Fromdate = null;
                    obj.Todate = null;
                }
                else if (!string.IsNullOrEmpty(obj.Fromdate) && !string.IsNullOrEmpty(obj.Todate))
                {
                    obj.Exactdate = "1990-01-01 00:00:00.000";
                }
                if (string.IsNullOrEmpty(obj.RenewalSearchID))
                {
                    ModelState.AddModelError("Error", "Please select By Exact Date or By Between Date !");
                }
                else if (string.IsNullOrEmpty(obj.Exactdate) && obj.RenewalSearchID == "1")
                {
                    ModelState.AddModelError("Error", "Please enter Exact date!");
                }
                else if (string.IsNullOrEmpty(obj.Fromdate) && obj.RenewalSearchID == "2")
                {
                    ModelState.AddModelError("Error", "Please enter From date!");
                }
                else if (string.IsNullOrEmpty(obj.Todate) && obj.RenewalSearchID == "2")
                {
                    ModelState.AddModelError("Error", "Please enter To date!");
                }
                else
                {




                    DataSet ds = objIRecepit.Get_RenewalReport(obj);
                    ds.Tables[0].TableName = "RecepitDataset";

                    if (ds.Tables[0].Rows.Count > 0)
                    {


                        ReportClass rptH = new ReportClass();
                        rptH.FileName = _hostingEnvironment.MapPath("~/Reports/ExactDate.rpt");
                        rptH.Load();
                        rptH.SetDataSource(ds.Tables[0]);
                        ////Response.Buffer = false;
                        ////Response.ClearContent();
                        ////Response.ClearHeaders();

                        Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream, "application/pdf", "RenewalReport.pdf");
                    }
                }
            }

            return View(obj);
        }

    }
}
