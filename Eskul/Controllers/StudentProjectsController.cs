using Eskul.APIClient;
using Eskul.Controllers;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Newtonsoft.Json;
using NPOI.Util;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class StudentProjectsController : BaseController
    {
        // GET: StudentProjectsController
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public StudentProjectsController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
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
                
                ApiResponse resp = null;
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
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
                resp = await _myUtilities.LoadSubjectPapers(model.SubjectCode ?? "0");

                if (resp != null && resp.Success)
                {
                    List<subjectpaperList> classLists = JsonConvert.DeserializeObject<List<subjectpaperList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.PaperCode, Value = p.PaperCode.ToString() }).ToList();

                    ViewBag.SubjectPapers = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadClassStream(model.Class);

                if (resp != null && resp.Success)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Stream = LoadListItems(l, true);
                }
                if (model.Class > 0)
                {
                    model.Year=DateTime.Now.Year.ToString();
                    ApiResponse response = await _myUtilities.LoadProjects(model);

                    if (response.Success)
                    {
                        model = JsonConvert.DeserializeObject<SubjectProjects>(response.PayLoad);
                       
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
                }
                else
                {

                    var model1 = new SubjectProjects();
                        model1.Term = SessionData.Term;
                    model1.Year = DateTime.Now.Year.ToString();
                    return View(model1);
                }


            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin";
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
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
                var l = (from p in await _myUtilities.LoadClasses(model.Level ?? "O") select new ListItems { Value = p.classcode.ToString(), Text = p.Name }).ToList();
                ViewBag.Classes = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadSubjects(model.Level ?? "O") select new ListItems { Value = p.Subject_code.ToString(), Text = p.Subject_name }).ToList();
                //ViewBag.Subjects = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadSubjectPapers(model.SubjectCode ?? "0") select new ListItems { Value = p.Paper_Code.ToString(), Text = p.Paper_Name }).ToList();
                //ViewBag.SubjectPapers = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadStreams(model.Class) select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();
                //ViewBag.Streams = LoadListItems(l, true);
               // l = (from p in await _myUtilities.LoadDisTypes(true) select new ListItems { Text = p.reasonName, Value = p.reasonId.ToString() }).ToList();
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
        public async Task<ActionResult> Create(SubjectProjects model)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            ApiResponse resp = null;
            try
            {
                Url = "Projects";
                model.Year = DateTime.Now.Year.ToString();
                model.SchoolCode = SessionData.ClientCode;
                resp = await request.AddAsync<SubjectProjects>(model, Url);
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

                return RedirectToAction(nameof(Index), new {model.Year,model.Class,model.Term,model.SubjectCode });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: StudentProjectsController/Edit/5
        public async Task<ActionResult> Edit(double id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }

            var model = new SubjectProjects();
            try
            {
                ApiResponse resp=await _myUtilities.LoadProject(id);
                model= JsonConvert.DeserializeObject<SubjectProjects>(resp.PayLoad);



                return Json(model); // Return the model as JSON response
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured";
                return RedirectToAction(nameof(Index));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoadAsignments(SubjectAssignments model)
        {
            if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}
            try
            {
                model.Year=DateTime.Now.Year.ToString();
                //ApiResponse response = await _myUtilities.LoadAssignments(model);

                //if (response.Success)
                //{
                //    model.Assignments = JsonConvert.DeserializeObject<List<SubjectAssignments>>(response.PayLoad);
                //}
                //else if (response.ResponseCode == 101)
                //{
                //    TempData["info"] = response.ResponseMessage;
                //}
                //else if (response.ResponseCode == 500)
                //{
                //    TempData["error"] = response.ResponseMessage;
                //}
                //else
                //{
                //    TempData["error"] = "Response Unkown";
                //}
                //model = await _myUtilities.LoadAssignments(model);
                //model.assignments = model.assignments ?? new List<Assignment>();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin";
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
                    model.Assignments = JsonConvert.DeserializeObject<List<SubjectAssignments>>(response.PayLoad);
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
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {
                ApiResponse resp = null;

                object model = "";

                if (Level != null)
                {
                    resp = await _myUtilities.LoadSubjects(Level);
                    model = JsonConvert.DeserializeObject<List<subjectList>>(resp.PayLoad);

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
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");

            }

        }
        public async Task<ActionResult> StreamsByClass(int classs)
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

                if (classs != 0)
                {
                    var req = await _myUtilities.LoadClassStream(classs);
                    model = JsonConvert.DeserializeObject<List<streamList>>(req.PayLoad);
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
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");

            }

        }
        public async Task<ActionResult> AssignProject(ProjectScoreViewModel model, string id,string StreamCode)
        {
            ApiResponse resp = null;
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {

                resp = await _myUtilities.LoadClassStream(model.Class);

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }

                resp = await _myUtilities._LoadMarkList(id);
                if (resp.Success && resp!=null && resp.ResponseCode==100) {
                     model = JsonConvert.DeserializeObject<ProjectScoreViewModel>(resp.PayLoad);
                    if (!string.IsNullOrEmpty(StreamCode) && StreamCode != "0")
                    {
                        model.StudentsList = model.StudentsList.Where(x => x.StreamCode == StreamCode).ToList();
                    }

                }
                else if (resp.ResponseCode == 101)
                {
                    TempData["info"] = resp.ResponseMessage;
                }
                else
                {
                    TempData["info"] = resp.ResponseMessage;
                }
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
        { if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            ApiResponse resp =null;
            var model = new ProjectScoreSaveRequest();
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
                model.ProjectId = decimal.Parse(form["AssignmentId"]);
                model.SchoolCode = SessionData.ClientCode;
                model.Score =int.Parse(score);
                Url = "Projects/Scores";
                 resp = await request.AddAsync<ProjectScoreSaveRequest>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<ProjectScoreSaveRequest>(resp.PayLoad);
                    var data = new { status = resp.ResponseCode, res = model.Comment};
                    var json = JsonConvert.SerializeObject(data);
                    return Content(json, "application/json");
                }
                else
                {
                    var data = new { status = 101, res = resp };
                    var json = JsonConvert.SerializeObject(data);
                    return Content(json, "application/json");
                }
      
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
