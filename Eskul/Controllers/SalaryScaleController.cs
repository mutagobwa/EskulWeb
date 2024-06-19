using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class SalaryScaleController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public SalaryScaleController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(SalaryScale model)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.salaryScales = await _myUtilities.LoadSalaryScales(true);
                var l = (from p in await _myUtilities.LoadDesignations(true) select new ListItems { Text = p.DesignationName, Value = p.DesignationCode.ToString() }).ToList();
                ViewBag.Designations = LoadListItems(l, true);
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: SalaryScaleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SalaryScaleController/Create
        public async Task<ActionResult> Create(SalaryScale model,int id)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";
                string Url = "StaffManagement/GenerateSalaryCode";
                resp = await request.GetB(Url);
                model.Code = resp.Split('\"')[3];
                return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: SalaryScaleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SalaryScale model)
        {
            Url = "StaffManagement/AddSalaryScale";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadSalaryScale(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "StaffManagement/UpdateSalaryScale";
                    resp = await request.Update<SalaryScale>(model, EditUrl);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured" + " " + resp;
                    }
                }
                else
                {
                    Url = "StaffManagement/AddSalaryScale";
                    resp = await request.Add<SalaryScale>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured" + " " + resp;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: SalaryScaleController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string resp = "";
            string EditUrl = "StaffManagement/SalaryScale/" + id + "";
            var model = new SalaryScale();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<SalaryScale>(EditUrl);
                model.Code = c.FirstOrDefault().Code;
                model.ScaleDesc = c.FirstOrDefault().ScaleDesc;
                model.Designation = c.FirstOrDefault().Designation;
                model.Monthly = c.FirstOrDefault().Monthly;
                model.Annually = c.FirstOrDefault().Annual;
                model.delete = false;
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), model);
        }

        // POST: SalaryScaleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SalaryScaleController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            string resp = "";
            string UpdateUrl = "StaffManagement/UpdateSalaryScale";
            string EditUrl = "StaffManagement/SalaryScale/" + id + "";
            var model = new SalaryScale();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<SalaryScale>(EditUrl);
                model.Code = c.FirstOrDefault().Code;
                model.ScaleDesc = c.FirstOrDefault().ScaleDesc;
                model.Designation = c.FirstOrDefault().Designation;
                model.Monthly = c.FirstOrDefault().Monthly;
                model.Annually = c.FirstOrDefault().Annual;
                model.delete = true;
                resp = await request.Update<SalaryScale>(model, UpdateUrl);
                var data = new { status = 200, res = resp };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");

            }
            catch (Exception ex)
            {

                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");

            }
        }

        // POST: SalaryScaleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
       
    }
}
