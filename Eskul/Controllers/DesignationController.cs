using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
//using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class DesignationController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public DesignationController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(Designation model,string id)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }
                if (!string.IsNullOrEmpty(id))
                {
                    Url = $"StaffManagement/Designation/GetByCode/{SessionData.ClientCode}/{id}";
                    var c = await request.Get<Designation>(Url);
                    model.DesignationCode = c.FirstOrDefault().DesignationCode;
                    model.DesignationName = c.FirstOrDefault().DesignationName;
                    model.delete = false;
                }
                model.designations = await _myUtilities.LoadDesignations(true);
            }
            catch (Exception ex)
            {

                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }
        // GET: DesignationController/Create
        public async Task<ActionResult> Create(Designation model,int Id)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";
                string Url = "StaffManagement/GenerateDesignationCode";
                resp = await request.GetB(Url);
                model.DesignationCode = resp.Split('\"')[3];
                return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: DesignationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Designation model)
        {
            Url = "StaffManagement/AddDesignation";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadDesignation(model);
                if (Exists.Count != 0)
                {
                    string EditUrl = "StaffManagement/UpdateDesignation";
                    model.SchoolCode = SessionData.ClientCode;
                    resp = await request.Update<Designation>(model, EditUrl);
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
                    Url = "StaffManagement/AddDesignation";
                    model.SchoolCode = SessionData.ClientCode;
                    resp = await request.Add<Designation>(model, Url);
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
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: DesignationController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
           
            return RedirectToAction(nameof(Index),new {id=id});
        }

    
        public async Task<ActionResult> Delete(string id)
        {
            string resp = "";
            string UpdateUrl = "StaffManagement/UpdateDesignation";
            Url = $"StaffManagement/Designation/GetByCode/{SessionData.ClientCode}/{id}";
            var model = new Designation();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<Designation>(Url);
                model.DesignationCode = c.FirstOrDefault().DesignationCode;
                model.DesignationName = c.FirstOrDefault().DesignationName;
                model.delete = true;
                model.SchoolCode = SessionData.ClientCode;
                resp = await request.Update<Designation>(model, UpdateUrl);
                var data = new { status = 200, res = resp };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
            }
        }
    }
}
