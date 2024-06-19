using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using SmartPaperEdms.Web.App_Code;
using System.Collections.Generic;

namespace Eskul.Controllers
{
    public class TopicMasterController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public TopicMasterController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _logger = logger;
            _sd = new SessionDetail();
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(SubjectTopics model,decimal id)
        
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                if (id != 0)
                {
                    var res = await _myUtilities.LoadTopic(id);
                    if (res.Success)
                    {
                        model = JsonConvert.DeserializeObject<SubjectTopics>(res.PayLoad);

                    }
                    else if (res.ResponseCode == 101)
                    {
                        TempData["info"] = res.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = res.ResponseMessage;
                    }


                }
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse resp = null;
                resp = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (resp != null && resp.Success)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadSubjects(model.Level ?? "O");

                if (resp != null && resp.Success)
                {
                    List<subjectList> SubjectLists = JsonConvert.DeserializeObject<List<subjectList>>(resp.PayLoad);
                    var l = (from p in SubjectLists select new ListItems { Text = p.Subjectname, Value = p.Subjectcode.ToString() }).ToList();

                    ViewBag.Subjects = LoadListItems(l, true);
                }

                model.Year = DateTime.Now.Year.ToString();
                if (!string.IsNullOrEmpty(model.Year) && !string.IsNullOrEmpty(model.SubjectCode) && model.ClassId != 0)
                {
                   var res=await _myUtilities.LoadTopics(model.Year, model.ClassId, model.SubjectCode ?? "0");
                    if (res.Success)
                    {
                        model = JsonConvert.DeserializeObject<SubjectTopics>(res.PayLoad);

                    }
                    else if (res.ResponseCode == 101)
                    {
                        TempData["info"] = res.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = res.ResponseMessage;
                    }

                }
            }

            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin";
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }

        public async Task<ActionResult> TComp(TopicsCompetencies model,decimal id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
                if (id!=0||model.TopicId!=0)
                {
                    var response = await _myUtilities.LoadTopicComps(id==0 ? model.TopicId: id);
                    if (response.ResponseCode==100)
                    {
                        model = JsonConvert.DeserializeObject<TopicsCompetencies>(response.PayLoad);
                    }
                    else if (response.ResponseCode == 101)
                    {
                        TempData["info"] = response.ResponseMessage;
                    }
                    else if (response.ResponseCode == 500)
                    {
                        TempData["error"] = response.ResponseMessage;
                    }
                }
                

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        
        // POST: TopicMasterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TopicMasterModel model)
        {
            Url = "Academics/Topics";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                model.SchoolCode = SessionData.ClientCode;
                model.Year = DateTime.Now.Year.ToString();
                resp = await request.AddAsync<TopicMasterModel>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    TempData["success"] = resp.ResponseMessage;

                }
                else if (resp.ResponseCode == 101)
                {
                    TempData["info"] = resp.ResponseMessage;
                }
                else
                {
                    TempData["error"] = resp.ResponseMessage;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: TopicMasterController/Edit/5
        public async Task<IActionResult> CompEdit(decimal id)
        {
            string resp = "";
            string EditUrl = "Academics/LessonPlan/TopicCompetency/SearchById/" + id + "";
            var model = new TopicMaster();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<Compitence>(EditUrl);
                if (c != null && c.Any())
                {
                    var firstCompitence = c.FirstOrDefault();
                    if (firstCompitence != null)
                    {
                        model._compitence = new Compitence(); // Instantiate the Competence object
                        model._compitence.Year = firstCompitence.Year;
                        model._compitence.TopicName = firstCompitence.TopicName;
                        model._compitence.TopicId = firstCompitence.TopicId;
                        model._compitence.CompetencyId = firstCompitence.CompetencyId;
                        model._compitence.Competency = firstCompitence.Competency;
                        model._compitence.StatusId = firstCompitence.StatusId;
                        model.delete = false;
                    }
                }
                else
                {
                    // Handle the case when the collection is null or empty
                    TempData["error"] = "No data found.";
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }

            return PartialView("_Compitence", model);
        }
        public async Task<ActionResult> Edit(decimal id)
        {
            
            return RedirectToAction(nameof(Index), new {id=id});
        }
        // GET: TopicMasterController/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            string resp = "";
            string UpdateUrl = "Academics/LessonPlan/SubjectTopic/Save";
            string EditUrl = $"Academics/LessonPlan/SubjectTopic/SearchById/{SessionData.ClientCode}/{id}";
            var model = new TopicMaster();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.GetSingle<TopicMaster>(EditUrl);
                model= c;
                model.delete = true;
                model.SchoolCode = SessionData.ClientCode;
                resp = await request.Add<TopicMaster>(model, UpdateUrl);
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
        public async Task<ActionResult> AddTopiComp(Compitence model, decimal id)
        {
            string resp = "";
            Url = "Academics/Topics/Competency";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.SchoolCode = SessionData.ClientCode;
                resp = await request.Add<Compitence>(model, Url);
               
                if (resp.Contains("successfully"))
                {

                    //model.compitence = await _myUtilities.LoadTopicComps(id);
                    TempData["success"] = resp;
                    return RedirectToAction(nameof(TComp),model);
                }
                else
                {
                    TempData["error"] = "Error Occured" + " " + resp;
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
