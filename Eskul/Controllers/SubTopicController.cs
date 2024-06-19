using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class SubTopicController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;

        public SubTopicController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _logger = logger;
            _sd = new SessionDetail();
            _myUtilities = myUtilities;
        }

        // GET: SubTopicController
        public async Task<ActionResult> Index(SubTopic model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
                var l = (from p in await _myUtilities.LoadClasses(model.Level ?? "O") select new ListItems { Value = p.classcode.ToString(), Text = p.Name }).ToList();
                ViewBag.Classes = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadSubjects(model.Level ?? "O") select new ListItems { Value = p.Subjectname.ToString(), Text = p.Subjectcode }).ToList();
                //ViewBag.Subjects = LoadListItems(l, true);
                model.year = DateTime.Now.Year.ToString();
                if (!string.IsNullOrEmpty(model.year) && !string.IsNullOrEmpty(model.SubCode) && model.ClassId != 0)
                {
                    model.SubTopics = await _myUtilities.LoadSubTopics(model.year, model.ClassId, model.SubCode ?? "0");

                }
            }
            
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }

        // GET: SubTopicController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubTopicController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubTopicController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SubTopic model)
        {
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                
                    Url = "Academics/LessonPlan/SubjectTopic/Save";
                    resp = await request.Add<SubTopic>(model,Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured" + " " + resp;
                    }
                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured" + "" + resp;
                return RedirectToAction(nameof(Index));
            }
        }
    // GET: SubTopicController/Edit/5
    public async Task<ActionResult> Edit(decimal id)
        {
            string resp = "";
            string EditUrl = "Academics/LessonPlan/SubjectTopic/SearchById/" + id + "";
            var model = new SubTopic();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<SubTopic>(EditUrl);
                model.TopicId = c.FirstOrDefault().TopicId;
                model.TopicName = c.FirstOrDefault().TopicName;
                model.Period = c.FirstOrDefault().Period;
                model.ClassId = c.FirstOrDefault().ClassId;
                model.SubCode = c.FirstOrDefault().SubCode;
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

        // POST: SubTopicController/Edit/5
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

        // GET: SubTopicController/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            string resp = "";
            string UpdateUrl = "Academics/LessonPlan/SubjectTopic/Save";
            string EditUrl = "Academics/LessonPlan/SubjectTopic/SearchById/" + id + "";
            var model = new SubTopic();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<SubTopic>(EditUrl);
                model.TopicId = c.FirstOrDefault().TopicId;
                model.TopicName = c.FirstOrDefault().TopicName;
                model.Period = c.FirstOrDefault().Period;
                model.ClassId = c.FirstOrDefault().ClassId;
                model.SubCode = c.FirstOrDefault().SubCode;
                model.year = c.FirstOrDefault().year;
                model.delete = true;
                resp = await request.Add<SubTopic>(model, UpdateUrl);
                var data = new { status = 200, res = resp };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");

            }
        }

        // POST: SubTopicController/Delete/5
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
