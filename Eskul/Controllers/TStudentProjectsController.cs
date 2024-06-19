using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class TStudentProjectsController : BaseController
    {
        // GET: StudentProjectsController
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public TStudentProjectsController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(SubjectProjects model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                ApiResponse response;

                response = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (response != null && response.ResponseCode == 100)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Classes = LoadListItems(l, true);
                }

                //response = await _myUtilities.LoadDisTypesAsync();

                //if (response != null && response.ResponseCode == 100)
                //{
                //    List<disabileReason> classLists = JsonConvert.DeserializeObject<List<disabileReason>>(response.PayLoad);
                //    var l = (from p in classLists select new ListItems { Text = p.reasonName, Value = p.reasonId.ToString() }).ToList();

                //    ViewBag.Classes = LoadListItems(l, true);
                //}
                //else
                //{
                //    var l = new List<ListItems>();
                //    ViewBag.Classes = LoadListItems(l, true);
                //}
                if (model.Class > 0)
                {
                    ApiResponse respons = await _myUtilities.LoadProjects(model);

                    if (respons.Success)
                    {
                        model.Projects = JsonConvert.DeserializeObject<List<ProjectAddModel>>(respons.PayLoad);
                    }
                    else if (respons.ResponseCode == 101)
                    {
                        TempData["info"] = respons.ResponseMessage;
                    }
                    else if (respons.ResponseCode == 500)
                    {
                        TempData["error"] = respons.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = "Response Unkown";
                    }
                }
                else
                {

                    var model1 = new ProjectAddModel();
                    model1.Year = DateTime.Now.Year.ToString();
                    return View(model1);
                }


            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: StudentProjectsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StudentProjectsController/Create
        public async Task<ActionResult> Create(Assignment model, int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {
                ApiResponse response;

                response = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (response != null && response.ResponseCode == 100)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Classes = LoadListItems(l, true);
                }
               model.Year = DateTime.Now.Year.ToString();
                if (model.Term == 0)
                {
                    model.Term = SessionData.Term;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST: StudentProjectsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectAddModel model)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string resp = "";
            try
            {
                    Url = "Examination/Project/Save";
                model.Year=DateTime.Now.Year.ToString();    
                    resp = await request.Add<ProjectAddModel>(model, Url);
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

        // GET: StudentProjectsController/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }

            string EditUrl = "Examination/Project/Get/" + id + "";
            var model = new ProjectAddModel();
            try
            {
                var c = await request.GetSingle<ProjectAddModel>(EditUrl);

                model.Year = c.Year;
                model.Term = c.Term;
                model.Class = c.Class;
                model.SubjectCode = c.SubjectCode;
                model.ProjectName = c.ProjectName;
                model.Description = c.Description;
                model.Delete = false;

                return Json(model); // Return the model as JSON response
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured";
                return RedirectToAction(nameof(Index));
            }
        }

        //public async Task<ActionResult> Edit(decimal id)
        //{
        //    if (!SessionData.IsSignedIn)
        //    {
        //        // Redirect the user to the login page
        //        return RedirectToAction("Index", "Login");
        //    }
        //    string resp = "";
        //    string EditUrl = "Examination/Project/Get/" + id + "";
        //    var model = new ProjectAddModel();
        //    try
        //    {
        //        var c = await request.GetSingle<ProjectAddModel>(EditUrl);

        //        model.Year = c.Year;
        //        model.Term = c.Term;
        //        model.Class = c.Class;
        //       model.SubjectCode = c.SubjectCode;
        //        model.ProjectName = c.ProjectName;
        //        model.Description = c.Description;
        //        model.Delete = false;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex.Message, ex);
        //        TempData["error"] = "Error Occured" + resp;
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction("Create", model);
        //}

        // POST: StudentProjectsController/Edit/5
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

        // GET: StudentProjectsController/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            var json = "";
            string resp = "";
            string EditUrl = "Examination/Project/Get/" + id + "";
            var model = new ProjectAddModel();
            try
            {
                var c = await request.GetSingle<ProjectAddModel>(EditUrl);

                model.Year = c.Year;
                model.Term = c.Term;
                model.Class = c.Class;
                model.SubjectCode = c.SubjectCode;
                model.ProjectName = c.ProjectName;
                model.Description = c.Description;
                model.ProjectId= c.ProjectId;
                model.Delete = true;
                Url = "Examination/Project/Save";
                resp = await request.Add<ProjectAddModel>(model, Url);
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
                var data = new { status = 201, message = ex };
                json = JsonConvert.SerializeObject(data);
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: StudentProjectsController/Delete/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoadAsignments(SubjectAssignments model)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
                ApiResponse response = await _myUtilities.LoadAssignments(model);

                if (response.Success)
                {
                    model.Assignments = JsonConvert.DeserializeObject<SubjectAssignments>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else if (response.ResponseCode == 500)
                {
                    TempData["error"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = "Response Unkown";
                }

                //model = await _myUtilities.LoadAssignments(model);
                //model.assignments = model.assignments ?? new List<Assignment>();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoadAsignment(SubjectAssignments model)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
                ApiResponse response = await _myUtilities.LoadAssignments(model);

                if (response.Success)
                {
                    model.Assignments = JsonConvert.DeserializeObject<SubjectAssignments>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else if (response.ResponseCode == 500)
                {
                    TempData["error"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = "Response Unkown";
                }

                //model = await _myUtilities.LoadAssignments(model);
                //model.assignments = model.assignments ?? new List<Assignment>();

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> SubPaperByLevel(string paper)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
                string resp = "";

                object model = "";

                if (paper != null)
                {

                    model = await _myUtilities.LoadSubjectPapers(paper);

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
        public async Task<ActionResult> ClassByLevel(string Level)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
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
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
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
        public async Task<ActionResult> AssignProject(string id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
                var model = new MarksAddList();
                //Url = "Academics/AssignmentScore/" + id;
                //model.StudentsList = await _myUtilities._LoadMarkList(id);
                //model.projs = await _myUtilities.LoadProjectById(id);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }


        }
        [HttpPost]
        public async Task<ActionResult> AssignProject(IFormCollection form, string studentId, string score, string comment, decimal AssignmentId)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string resp = "";
            string UpUrl = "Examination/ProjectScore/Update/";
            var model = new MarksUpdate();
            try
            {


                model.StudentId = form["studentId"];
                //model.score = int.Parse(form["score"]);
                model.Comment = form["comment"];
                if (string.IsNullOrEmpty(model.Comment))
                {
                    model.Comment = "NO COMMENT";
                }
                if (string.IsNullOrEmpty(score))
                {
                    score = "-1";
                }
                model.ProjectsId = decimal.Parse(form["AssignmentId"]);
                UpUrl = "Examination/ProjectScore/Update/" + score + "/" + model.Comment + "/" + studentId + "/" + AssignmentId;
                resp = await request.Update<MarksUpdate>(model, UpUrl);
                var data = new { status = 200, res = resp };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }

        }
    }
}
