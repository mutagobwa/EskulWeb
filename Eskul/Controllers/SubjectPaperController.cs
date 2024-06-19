using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Configuration;
using System.Reflection;

namespace Eskul.Controllers
{
    public class SubjectPaperController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public SubjectPaperController( IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;  
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;

        }
        // GET: SubjectPaperController
        public async Task<ActionResult> Index(subjectpaper model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.subjectPaperLists = await _myUtilities.LoadSubjectPapers(true);
                var l = (from p in await _myUtilities.LoadOfferedSubjects() select new ListItems { Text = p.Subject_name, Value = p.Subject_code.ToString() }).ToList();
                ViewBag.Subjects = LoadListItems(l, true);
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            
            return View(model);
        }
        
        // GET: SubjectPaperController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubjectPaperController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubjectPaperController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(subjectpaper model)
        {
            Url = "Academics/AddSubjectPaper";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadSubjectPaper(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "Academics/UpdateSubjectPaper";
                    resp = await request.Update<subjectpaper>(model, EditUrl);
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
                    Url = "Academics/AddSubjectPaper";
                    resp = await request.Add<subjectpaper>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured" + "" + resp;
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

        // GET: SubjectPaperController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string resp = "";
            var id2 = id.Replace("%2F", "-");

            string EditUrl = "Academics/PaperByCode/" + id2 + "";
            var model = new subjectpaper();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<subjectpaperList>(EditUrl);

                model.SubjectCode = c.FirstOrDefault().Subject_Code;
                model.PaperCode = c.FirstOrDefault().Paper_Code;
                model.PaperName = c.FirstOrDefault().Paper_Name;
                model.Compulsory = c.FirstOrDefault().Compulsory;
                model.Offered = c.FirstOrDefault().Offered;
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

        // POST: SubjectPaperController/Edit/5
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

        // GET: SubjectPaperController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var json = "";
            string resp = "";
            var id2 = id.Replace("/", "-");
            string UpUrl = "Academics/UpdateSubjectPaper";
            string EditUrl = "Academics/PaperByCode/" + id2 + "";
            var model = new subjectpaper();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<subjectpaperList>(EditUrl);

                model.SubjectCode = c.FirstOrDefault().Subject_Code;
                model.PaperCode = c.FirstOrDefault().Paper_Code;
                model.PaperName = c.FirstOrDefault().Paper_Name;
                model.Compulsory = c.FirstOrDefault().Compulsory;
                model.Offered = c.FirstOrDefault().Offered;
                model.delete = true;
                resp = await request.Update<subjectpaper>(model, UpUrl);
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

                var data = new { status = 201, message = ex };
                 json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                //TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");

            }
        }

        // POST: SubjectPaperController/Delete/5
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
        
       
        public async Task<ActionResult> SubjectByLevel(string Level)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";

                object model = "";

                if (Level != null)
                {
                    model = await _myUtilities.LoadSubjects(Level);
                    var data = new { status = 200, res = resp };
                    var json = JsonConvert.SerializeObject(model);
                    return Content(json, "application/json");
                }

                else
                {
                    return Content(JsonConvert.SerializeObject(new { status = 404, res = "Error" }), "application/json");
                }
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
    }
}
