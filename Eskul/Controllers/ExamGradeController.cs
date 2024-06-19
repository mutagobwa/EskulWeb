using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class ExamGradeController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ExamGradeController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        // GET: ExamGradeController
        public async Task<ActionResult> Index(ExamGradeAdd model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.examGrades = await _myUtilities.  LoadExamGrades(true);
                //var l = (from p in await _myUtilities.LoadClasses(true) select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();
                //ViewBag.Classes = LoadListItems(l, true);
            }
            catch (Exception ex)
            {


                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }

        // GET: ExamGradeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExamGradeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExamGradeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExamGradeAdd model)
        {
            Url = "Examination/ExamGrade/Add";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadExamGrade(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "Examination/ExamGrade/Update";
                    model.delete = false;
                    model.PercentageTo = model.PercentagefTo;
                    resp = await request.Update<ExamGradeAdd>(model, EditUrl);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] =resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured"+resp;
                    }
                }
                else
                {
                    Url = "Examination/ExamGrade/Add";
                    model.PercentageTo=model.PercentagefTo;
                    resp = await request.Add<ExamGradeAdd>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured" + resp;
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

        // GET: ExamGradeController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string EditUrl = "Examination/ExamGrade/Get/ByCode/" + id + "";
            var model = new ExamGradeAdd();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<ExamGradeList>(EditUrl);

                model.Class = c.FirstOrDefault().Classcode;
                model.GradeCode = c.FirstOrDefault().GradeCode;
                model.GradePoints = c.FirstOrDefault().GradePoints;
                model.PercentageFrom= c.FirstOrDefault().PercentageFrom;
                model.PercentageTo = c.FirstOrDefault().PercentagefTo;
                model.PercentagefTo = c.FirstOrDefault().PercentagefTo;
                model.GradeRank = c.FirstOrDefault().GradeRank;
                model.Comment = c.FirstOrDefault().Comment;
                model.GradeDescriptor = c.FirstOrDefault().GradeDescriptor;
                model.ExamGradeId = c.FirstOrDefault().ExamGradeId;
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
            // POST: ExamGradeController/Edit/5
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

        // GET: ExamGradeController/Delete/5
        public async Task< ActionResult> Delete(string id)
        {
            string resp = "";
            string UpUrl = "Academics/UpdateExamGrade";
            string EditUrl = "Academics/ExamGrade/" + id + "";
            var model = new ExamGradeAdd();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<ExamGradeList>(EditUrl);

                model.Class = c.FirstOrDefault().Classcode;
                model.GradeCode = c.FirstOrDefault().GradeCode;
                model.GradePoints = c.FirstOrDefault().GradePoints;
                model.PercentageFrom = c.FirstOrDefault().PercentageFrom;
                model.PercentageTo = c.FirstOrDefault().PercentageFrom;
                model.GradeRank = c.FirstOrDefault().GradeRank;
                model.Comment = c.FirstOrDefault().Comment;
                model.GradeDescriptor = c.FirstOrDefault().GradeDescriptor;
                model.ExamGradeId = c.FirstOrDefault().ExamGradeId;
                model.delete = true;
                resp = await request.Update<ExamGradeAdd>(model, UpUrl);
                var data = new { status = 200, res = resp };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);

                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: ExamGradeController/Delete/5
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
