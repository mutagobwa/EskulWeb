using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Configuration;
using System.Reflection;

namespace Eskul.Controllers
{
    public class DesciplinaryController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: DesciplinaryController
        public DesciplinaryController( IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(Disciplinary model)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                //model.Disciplinaries = await _myUtilities.LoadDisciplinaryCases(true);

            }
            catch (Exception ex)
            {

                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }

        // GET: DesciplinaryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DesciplinaryController/Create
        public async Task<ActionResult> Create(Disciplinary model,int id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
                string resp = "";
                string Url = "BehaviourManagement/GenerateDisciplinaryCaseCode";
                resp = await request.GetB(Url);
                model.Code = resp.Split('\"')[3];
                return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DesciplinaryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Disciplinary model)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            Url = "BehaviourManagement/AddDisciplinaryCase";
            string resp = "";
            try
            {
                //var Exists = await _myUtilities.LoadDisciplinaryCase(model);
                //if (Exists == null || Exists.Count == 0)
                //{

                //    Url = "BehaviourManagement/AddDisciplinaryCase";
                //    resp = await request.Add<Disciplinary>(model, Url);
                //    if (resp.Contains("successfully"))
                //    {
                //        TempData["success"] = resp;

                //    }
                //    else
                //    {
                //        TempData["error"] = resp;
                //    }

                //}
                //else
                //{
                //    string EditUrl = "BehaviourManagement/UpdateDisciplinaryCase";
                //    model.delete = false;
                //    resp = await request.Update<Disciplinary>(model, EditUrl);
                //    if (resp.Contains("successfully"))
                //    {
                //        TempData["success"] = "Updated Successfully";

                //    }
                //    else
                //    {
                //        TempData["error"] = "Error Occured";
                //    }
                //}

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: DesciplinaryController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string EditUrl = "BehaviourManagement/DisciplinaryCase/" + id + "";
            var model = new Disciplinary();
            try
            {
                var c = await request.Get<DisciplinaryList>(EditUrl);

                model.Code = c.FirstOrDefault().CaseCode;
                model.Name = c.FirstOrDefault().CaseName;
                model.delete = false;
            }
            catch (Exception ex)
            {

                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), model);
        }

        // POST: DesciplinaryController/Edit/5
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

        // GET: DesciplinaryController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            var json = "";
            string resp = "";
            string Urlu = "BehaviourManagement/UpdateDisciplinaryCase";
            string EditUrl = "BehaviourManagement/DisciplinaryCase/" + id + "";
            var model = new Disciplinary();
            try
            {
                var c = await request.Get<DisciplinaryList>(EditUrl);

                model.Code = c.FirstOrDefault().CaseCode;
                model.Name = c.FirstOrDefault().CaseName;
                model.delete = true;
                resp = await request.Update<Disciplinary>(model, Urlu);
                if (resp.Contains("successfully"))
                {
                    var data = new { status = 200, res = resp };
                    json = JsonConvert.SerializeObject(data);

                }
                else
                {
                    var data = new { status = 201, res = resp };
                    json = JsonConvert.SerializeObject(data);
                }
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                //   TempData["error"] = "Error Occured" + " " + resp;
                
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: DesciplinaryController/Delete/5
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
