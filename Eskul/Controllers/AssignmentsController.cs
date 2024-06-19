using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using Nancy.Json;
using Newtonsoft.Json;
using NPOI.Util;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;
using System.Text;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{

    public class AssignmentsController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public AssignmentsController(IConfiguration configuration,ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        // GET: AssignmentsController
        public async Task<ActionResult> Index(SubjectAssignments model)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse resp = null;
                resp = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (resp != null && resp.ResponseCode==100)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Classes = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadSubjects(model.Level ?? "O");

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<subjectList> SubjectLists = JsonConvert.DeserializeObject<List<subjectList>>(resp.PayLoad);
                    var l = (from p in SubjectLists select new ListItems { Text = p.Subjectname, Value = p.Subjectcode.ToString() }).ToList();

                    ViewBag.Subjects = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Subjects = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadSubjectPapers(model.SubjectCode ?? "0");

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<subjectpaperList> classLists = JsonConvert.DeserializeObject<List<subjectpaperList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.PaperCode, Value = p.PaperCode.ToString() }).ToList();

                    ViewBag.SubjectPapers = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.SubjectPapers = LoadListItems(l, true);
                }
                
                resp = await _myUtilities.LoadTopics(model.Year ?? "0", model.Class, model.SubjectCode ?? "0");

                if (resp != null && resp.ResponseCode==100)
                {
                    SubjectTopicsModel TopicsLists = JsonConvert.DeserializeObject<SubjectTopicsModel>(resp.PayLoad);
                    var l = (from p in TopicsLists.Topics select new ListItems { Text = p.TopicName, Value = p.TopicId.ToString() }).ToList();

                    ViewBag.Topics = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Topics = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadTopicComps(model.TopicId);

                if (resp != null && resp.Success)
                {
                    TopicMaster TopicCompsLists = JsonConvert.DeserializeObject<TopicMaster>(resp.PayLoad);
                    var l = (from p in TopicCompsLists.CompetencyList select new ListItems { Text = p.Competency, Value = p.CompetencyId.ToString() }).ToList();

                    ViewBag.Topics = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.TopicComps = LoadListItems(l, true);
                }

                model.Term = SessionData.Term;
                if (string.IsNullOrEmpty(model.Year)) { model.Year = DateTime.Now.Year.ToString(); }
                   
                if (model.Class > 0)
                {
                    ApiResponse response = await _myUtilities.LoadAssignments(model);

                    if (response.Success)
                    {
                        model = JsonConvert.DeserializeObject<SubjectAssignments>(response.PayLoad);
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
                    if (string.IsNullOrEmpty(model.Year)) { model.Year = DateTime.Now.Year.ToString(); }
                }
                else
                {
                    model.Year = DateTime.Now.Year.ToString();
                    model.Term = SessionData.Term;
                    return View(model);
                }
                if (string.IsNullOrEmpty(model.Year)) { model.Year = DateTime.Now.Year.ToString(); }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        // GET: AssignmentsController/Create
        public async Task<ActionResult> Create(Assignment model,int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {
                ApiResponse resp = null;
                //var l = (from p in _da.BankList() select new ItemList { Text = p.Name, Value = p.Id.ToString() }).ToList();
                //_da.LoadLisItems(cboDBank, l, true);
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

                    ViewBag.SubjectPapers = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadTopics(model.Year ?? "0", model.Class, model.SubjectCode ?? "0");

                if (resp != null && resp.Success)
                {
                    List<TopicMaster> TopicsLists = JsonConvert.DeserializeObject<List<TopicMaster>>(resp.PayLoad);
                    var l = (from p in TopicsLists select new ListItems { Text = p.TopicName, Value = p.TopicId.ToString() }).ToList();

                    ViewBag.Topics = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadTopicComps(model.TopicId);

                if (resp != null && resp.Success)
                {
                    TopicMaster TopicCompsLists = JsonConvert.DeserializeObject<TopicMaster>(resp.PayLoad);
                    //var l = (from p in TopicCompsLists select new ListItems { Text = p.CompetencyList.FirstOrDefault().Competency, Value = p.CompetencyList.FirstOrDefault().CompetencyId.ToString() }).ToList();

                    //ViewBag.Topics = LoadListItems(l, true);
                }

                model.Year= DateTime.Now.Year.ToString();
                if (model.Term == 0)
                {
                    model.Term = SessionData.Term;
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

        // POST: AssignmentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignmentAdd model)
        {
            Url = "Assignments";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }

                if (model.AssignmentId!=0) {
                    Url = $"Assignments/{model.AssignmentId}";
                    model.schoolCode = SessionData.ClientCode;
                    model.subject = model.SubjectCode;
                    model.year = DateTime.Now.Year.ToString();
                    resp = await request.UpdateAsync<AssignmentAdd>(model, Url);
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
                }
                else
                {
                    model.schoolCode = SessionData.ClientCode;
                    model.subject = model.SubjectCode;
                    model.year = DateTime.Now.Year.ToString();
                    resp = await request.AddAsync<AssignmentAdd>(model, Url);
                    if (resp != null && resp.ResponseCode == 100) { TempData["success"] = resp.ResponseMessage;  }
                    else if (resp?.ResponseCode == 101) { TempData["info"] = resp.ResponseMessage; }
                    else { TempData["error"] = resp?.ResponseMessage; }
                }
                
                return RedirectToAction(nameof(Index), new {model.term,model.SubjectCode,model.Class,model.Stream});
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AssignmentsController/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            ApiResponse response = null;
            var model = new Assignment();
            try
            {
                response = await _myUtilities.LoadAssignment(id);

                if (response.Success)
                {
                    model = JsonConvert.DeserializeObject<Assignment>(response.PayLoad);
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
                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
            //return RedirectToAction(nameof(Index),model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoadAsignments(SubjectAssignments model)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {
               // model.Year = DateTime.Now.Year.ToString();
                ApiResponse response = await _myUtilities.LoadAssignments(model);

                if (response.ResponseCode == 100)
                {
                    SubjectAssignments model1 = JsonConvert.DeserializeObject<SubjectAssignments>(response.PayLoad);

                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = response.ResponseMessage;
                }

                //model = await _myUtilities.LoadAssignments(model);
                //model.assignments = model.assignments ?? null;
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), new {model.Year,model.Term,model.Class,model.SubjectCode});

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoadAsignment(SubjectAssignments model)
        {
            if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login"); }
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
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AssignmentsController/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            var json = "";
            string resp = "";
            string UpdateUrl = "Examination/Assignment/Save";
            string EditUrl = "Examination/Assignment/Get/" + id + "";
            var model = new Assignment();
            try
            {
                var c = await request.GetSingle<Assignment>(EditUrl);
                model.Year = c.Year;
                model.Term = c.Term;
                model.Class = c.Class;
                model.SubjectCode = c.SubjectCode;
                model.competencyId = c.competencyId;
                model.Description = c.CompetencyDesc;
                model.StatusId = c.StatusId;
                model.AssignmentId = c.AssignmentId;
                model.delete = true;

                resp = await request.Add<Assignment>(model, UpdateUrl);
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
                TempData["error"] = "Error Occured Contact Admin";
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }
        public async Task< ActionResult> AssignMarks(AssignmentScoreViewModel model, SubjectAssignments model1,decimal assignmentId, int classId, string streamCode)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {
                ApiResponse resp1 = null;
                resp1 = await _myUtilities.LoadSkillDefinitions();

                if (resp1 != null && resp1.ResponseCode == 100)
                {
                    List<GSkillDefinition> GSLists = JsonConvert.DeserializeObject<List<GSkillDefinition>>(resp1.PayLoad);
                    var l = (from p in GSLists select new ListItems { Text = p.SkillDefinition, Value = p.DefinitionId.ToString() }).ToList();

                    ViewBag.GsCat = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.GsCat = LoadListItems(l, true);
                }
                resp1 = await _myUtilities.LoadClassStream(classId);

                if (resp1 != null && resp1.ResponseCode == 100)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(resp1.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }

                Url = $"Assignments/Scores?school={SessionData.ClientCode}&assignmentId={assignmentId}&classId={classId}";
                ApiResponse resp= await request.GetAsync(Url);

                model = JsonConvert.DeserializeObject<AssignmentScoreViewModel>(resp.PayLoad);

                if (!string.IsNullOrEmpty(streamCode) && streamCode!="0")
                {
                    model.StudentsList = model.StudentsList.Where(x=>x.StreamCode==streamCode).ToList();
                }
                return View("AssignMarks", model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message,ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            } 
            
           
        }
        [HttpPost]
        public async Task<ActionResult> AssignMarks(IFormCollection form, string studentId, double score, string comment,decimal AssignmentId,int genericSkillId)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            
            var model = new MarksUpdate();
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                Url = "Assignments/Scores";
                model.SchoolCode = SessionData.ClientCode; 
                model.StudentId=studentId;
                model.Comment = comment ?? "NO COMMENT";
                model.GenericSkillId = genericSkillId;
                model.AssignmentId = AssignmentId;
                model.Score = score;
                ApiResponse resp = await request.AddAsync<MarksUpdate>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<MarksUpdate>(resp.PayLoad);
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
                //return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin";
                _logger.Error(ex.Message, ex);
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);

                TempData["error"] = "Error Occured Contact Admin";
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
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
                TempData["error"] = "Error Occured Contact Admin";
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
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");

            }

        }
            public async Task<ActionResult> SubjectByLevel(string Level)
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                try
                {
                    ApiResponse resp =null;

                    object model = "";

                    if (Level != null)
                    {
                        resp=await _myUtilities.LoadSubjects(Level);
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
        public async Task<ActionResult> CompetencyByTopic(decimal id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {
                string resp = "";


                if (id != 0)
                {

                    var req = await _myUtilities.LoadTopicComps(id);
                    SubjectTopicComp model = JsonConvert.DeserializeObject<SubjectTopicComp>(req.PayLoad);

                    var data = new { status = 200, res = resp };
                    var json = JsonConvert.SerializeObject(model.Competencies);
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
        public async Task<ActionResult> TopicBySubject(string Year,int ClassId,string SubCode)
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

                if (SubCode != null)
                {
                    var req = await _myUtilities.LoadTopics(Year, ClassId, SubCode);
                    SubjectTopicsModel model1 = JsonConvert.DeserializeObject<SubjectTopicsModel>(req.PayLoad);
                    var data = new { status = 200, res = resp };
                    var json = JsonConvert.SerializeObject(model1.Topics);
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

    }
}
