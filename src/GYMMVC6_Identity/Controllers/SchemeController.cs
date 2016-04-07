using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GYMONE.Filters;
using GYMONE.Models;
using GYMONE.Repository;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;

// INC53846 
namespace GYMONE.Controllers
{
   
    /////[MyExceptionHandler]
    [Authorize(Roles = "Admin")]
    public class SchemeController : Controller
    {
        ISchemeMaster objISchemeMaster;

        public SchemeController(IConfiguration configuration)
        {
            objISchemeMaster = new SchemeMaster(configuration);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new SchemeMasterDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SchemeMasterDTO objSchemeMasterDTO)
        {
            if (ModelState.IsValid)
            {

                objSchemeMasterDTO.Createdby = TempData["UserID"].ToString();
                objISchemeMaster.InsertScheme(objSchemeMasterDTO);
                TempData["Message"] = "Scheme Create Successfully.";
                return RedirectToAction("Create");
            }
            else
            {
                ModelState.AddModelError("Error", "Please enter Scheme Name ");
            }

            ModelState.Remove("SchemeName");

            return View(objSchemeMasterDTO);
        }

        public ActionResult SchemeNameExists(string SchemeName)
        {
            var result = objISchemeMaster.SchemeNameExists(SchemeName);
            return Json(!result);
        }

        [HttpGet]
        public ActionResult Details()
        {
            return View(objISchemeMaster.GetSchemes());
        }

        [HttpGet]
        public ActionResult Edit(string ID)
        {
            var Model = objISchemeMaster.GetSchemeByID(ID);
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SchemeMasterDTO objSchemeMasterDTO)
        {

            objISchemeMaster.UpdateScheme(objSchemeMasterDTO);
            TempData["MessageUpdate"] = "Scheme Updated Successfully.";
            return RedirectToAction("Details");
        }


        [HttpGet]
        public ActionResult Delete(string ID)
        {
            objISchemeMaster.DeleteScheme(ID);
            return RedirectToAction("Details");
        }
    }
}
