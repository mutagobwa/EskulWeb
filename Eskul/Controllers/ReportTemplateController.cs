using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.Session;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Drawing;
using System.Reflection;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class ReportTemplateController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ReportTemplateController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        // GET: ReportTemplateController
        public async Task<ActionResult> Index(ReportTemplate model,string option)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string selectedLevel = @TempData["MembershipId"] as string;
                

                string Level1 = selectedLevel ?? "O";
                

                var l = (from p in await _myUtilities.LoadCurriculum(true) select new ListItems { Text = p.Name, Value = p.Id.ToString() }).ToList();
                ViewBag.Curriculum = LoadListItems(l, true);

                l = (from p in await _myUtilities.LoadClasses(Level1) select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();
                ViewBag.Classes = LoadListItems(l, true);

                //l = (from p in await _myUtilities.LoadSubjects(Level1) select new ListItems { Text = p.Subject_name, Value = p.Subject_code.ToString() }).ToList();
                //ViewBag.Subjects = LoadListItems(l, true);

                model.reportTemplates = await _myUtilities.LoadReportTemplates(model.Class);

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: ReportTemplateController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReportTemplateController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportTemplateController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Create(ReportTemplate model)
        {
            Url = "Examination/Save/ReportTemplate";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                if (model.CurriculumType == 1)
                {
                    model.CurriculumName= "OLD";
                }
                else
                {
                    model.CurriculumName = "NEW";
                }
                model.SubjectName = "";
                    resp = await request.Add<ReportTemplate>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = resp;
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

        // GET: ReportTemplateController/Edit/5
        public async Task<ActionResult> Edit(decimal Id)
        {
            string EditUrl = "Examination/ReportTemplate/GetById/" + Id + "";
            var model = new ReportTemplate();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<ReportTemplate>(EditUrl);
                model.Class = c.FirstOrDefault().Class;
                model.SubjectCode = c.FirstOrDefault().SubjectCode;
                model.SubjectName = c.FirstOrDefault().SubjectName;
                model.CurriculumType = c.FirstOrDefault().CurriculumType;
                model.CurriculumName = c.FirstOrDefault().CurriculumName;
                model.OrderLevel = c.FirstOrDefault().OrderLevel;
                model.TemplateId = c.FirstOrDefault().TemplateId;
                model.CoreElective = c.FirstOrDefault().CoreElective;
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

        // POST: ReportTemplateController/Edit/5
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

        // GET: ReportTemplateController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var json = "";
            string resp = "";
            string UpdateUrl = "Academics/Save/ReportTemplate";
            string EditUrl = "Academics/ReportTemplate/GetById/" + id + "";
            var model = new ReportTemplate();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<ReportTemplate>(EditUrl);
                model.Class = c.FirstOrDefault().Class;
                model.SubjectCode = c.FirstOrDefault().SubjectCode;
                model.SubjectName = c.FirstOrDefault().SubjectName;
                model.CurriculumType = c.FirstOrDefault().CurriculumType;
                model.CurriculumName = c.FirstOrDefault().CurriculumName;
                model.OrderLevel = c.FirstOrDefault().OrderLevel;
                model.TemplateId = c.FirstOrDefault().TemplateId;
                model.CoreElective = c.FirstOrDefault().CoreElective;
                model.delete = true;
                if (model.CurriculumType == 1)
                {
                    model.CurriculumName = "OLD";
                }
                else
                {
                    model.CurriculumName = "NEW";
                }
                model.SubjectName = "";

                resp = await request.Add<ReportTemplate>(model, UpdateUrl);
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
                //var data = new { status = 201, message = ex };
                //var json = JsonConvert.SerializeObject(data);
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: ReportTemplateController/Delete/5
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
        public async Task<ActionResult> ClassByLevel(string Level)
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
                    
                    model = await _myUtilities.LoadClasses(Level);
                    
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
