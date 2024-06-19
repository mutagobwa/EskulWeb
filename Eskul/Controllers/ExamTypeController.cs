using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Configuration;
using System.Reflection;

namespace Eskul.Controllers
{
    public class ExamTypeController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: ExamTypeController
        public ExamTypeController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;   
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(ExamTypeVm model)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse response = await _myUtilities.LoadExams();

                if (response.Success)
                {
                    model.Exams = JsonConvert.DeserializeObject<List<ExamTypeVm>>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = response.ResponseMessage;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: ExamTypeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExamTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExamTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExamTypeVm model)
        {
            Url = "Academics/AddExamType";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadExam(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "Academics/UpdateExamType";
                    model.delete = false;
                    resp = await request.Update<ExamTypeVm>(model, EditUrl);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] ="Error Occured"+ resp;
                    }
                }
                else
                {
                    Url = "Academics/AddExamType";
                    resp = await request.Add<ExamTypeVm>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

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

        // GET: ExamTypeController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string resp = "";
            string EditUrl = "Academics/ExamType/" + id + "";
            var model = new ExamTypeVm();
            try
            {
                var c = await request.Get<ExamTypeVm>(EditUrl);

                model.ExamCode = c.FirstOrDefault().ExamCode;
                model.ExamName = c.FirstOrDefault().ExamName;
                model.ExamDescription = c.FirstOrDefault().ExamDescription;
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

        // POST: ExamTypeController/Edit/5
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

        // GET: ExamTypeController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string resp = "";
            string UpUrl = "Academics/UpdateExamType";
            string EditUrl = "Academics/ExamType/" + id + "";
            var model = new ExamTypeVm();
            try
            {
                var c = await request.Get<ExamTypeVm>(EditUrl);

                model.ExamCode = c.FirstOrDefault().ExamCode;
                model.ExamName = c.FirstOrDefault().ExamName;
                model.ExamDescription = c.FirstOrDefault().ExamDescription;
                model.delete = true;
                resp = await request.Update<ExamTypeVm>(model, UpUrl);
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
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: ExamTypeController/Delete/5
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
