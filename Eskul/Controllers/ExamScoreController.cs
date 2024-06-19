using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;
using System.Text;

namespace Eskul.Controllers
{
    public class ExamScoreController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly IConfiguration configuration;
        private readonly MyUtilities _myUtilities;
        public ExamScoreController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(ExamScore model)
        {
            try
            { 
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
                resp = await _myUtilities.LoadSubjectPapers(model.SubjectCode ?? "0");

                if (resp != null && resp.Success)
                {
                    List<subjectpaperList> PaperLists = JsonConvert.DeserializeObject<List<subjectpaperList>>(resp.PayLoad);
                    var l = (from p in PaperLists select new ListItems { Text = p.PaperName, Value = p.PaperCode.ToString() }).ToList();

                    ViewBag.SubjectPapers = LoadListItems(l,true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.SubjectPapers = LoadListItems(l, true);
                }

                resp = await _myUtilities.LoadClassStream(model.Class);

                if (resp != null && resp.Success)
                {
                    List<streamList> StreamLists = JsonConvert.DeserializeObject<List<streamList>>(resp.PayLoad);
                    var l = (from p in StreamLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }

                resp = await _myUtilities.LoadExams();

                if (resp != null && resp.Success)
                {
                    List<ExamTypeVm> classLists = JsonConvert.DeserializeObject<List<ExamTypeVm>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.ExamName, Value = p.ExamCode.ToString() }).ToList();

                    ViewBag.ExamType = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.ExamType = LoadListItems(l, true);
                }
                if (model.Class > 0)
                {
                    if (string.IsNullOrEmpty(model.Stream)) { model.Stream = "0"; }
                    if (model.TermCode==0) { model.TermCode = SessionData.Term; }
                    if (string.IsNullOrEmpty(model.YearCode)) { model.YearCode = DateTime.Now.Year.ToString(); }
                    
                   model.examScores = await _myUtilities._LoadExamss(model);
                    if (!string.IsNullOrEmpty(model.Stream) && model.Stream != "0") { model.examScores = model.examScores.Where(x => x.Stream == model.Stream).ToList(); }
                }
                else
                {

                    var model1 = new ExamScore();
                    model1.YearCode = DateTime.Now.Year.ToString();
                    if (model1.TermCode == 0)
                    {
                        model1.TermCode = SessionData.Term;
                    }
                    return View(model1);
                }


            }
            catch (Exception ex)
            {
                if (ex.Message== "Object reference not set to an instance of an object.")
                {
                    TempData["error"] = "Error Occured" + " " + "The Search had no Results.";
                }
                else
                {
                    TempData["error"] = "Error Occured Contact Admin" ;
                }
               
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ExamScoreController/Details/5
        public async Task<ActionResult> ExamScoreT(ExamScore model)
        {
            try
            {
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
                resp = await _myUtilities.LoadExams();

                if (resp != null && resp.Success)
                {
                    List<ExamTypeVm> classLists = JsonConvert.DeserializeObject<List<ExamTypeVm>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.ExamName, Value = p.ExamCode.ToString() }).ToList();

                    ViewBag.ExamType = LoadListItems(l, true);
                }
                
                if (model.Class > 0)
                {
                    if (model.Stream == null)
                    {
                        model.Stream = "0";
                    }
                    if (model.TermCode == 0)
                    {
                        model.TermCode = SessionData.Term;
                    }
                    model.YearCode = DateTime.Now.Year.ToString();
                    ApiResponse apiResponse = await _myUtilities.LoadExamss(model);
                    model.examScores = JsonConvert.DeserializeObject<List<ExamScore>>(resp.PayLoad);
                }
                else
                {

                    var model1 = new ExamScore();
                    model1.YearCode = DateTime.Now.Year.ToString();
                    if (model.TermCode == 0)
                    {
                        model.TermCode = SessionData.Term;
                        
                    }
                    resp = await _myUtilities.LoadClassess(model.Level ?? "O");

                    if (resp != null && resp.Success)
                    {
                        List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(resp.PayLoad);
                        var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                        ViewBag.Classes = LoadListItems(l, true);
                    }
                    return View(model1);
                }


            }
            catch (Exception ex)
            {
                if (ex.Message == "Object reference not set to an instance of an object.")
                {
                    TempData["error"] = "Error Occured" + " " + "The Search had no Results.";
                }
                else
                {
                    TempData["error"] = "Error Occured Contact Admin" ;
                }

                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(ExamScoreT));
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> AssignMarks(ExamScoreSaveResponse model)
        {
            model.Comment = string.IsNullOrEmpty(model.Comment) ? "NO COMMENT" : model.Comment;
            model.Grade = ""; 
            if (model.Score == 0) { model.Score = -1; }
            model.SchoolCode = SessionData.ClientCode;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                Url = "Examinations/Scores";
               ApiResponse  resp = await request.AddAsync<ExamScoreSaveResponse>(model, Url);
                if (resp.ResponseCode==100)
                {
                    model = JsonConvert.DeserializeObject<ExamScoreSaveResponse>(resp.PayLoad);
                    var data = new { status = resp.ResponseCode, res = model.Comment,grade=model.Grade};
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
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);

                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
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
                    var req=await _myUtilities.LoadClassStream(classs);
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
                TempData["error"] = "Error Occured Contact Admin" ;
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
        public async Task<ActionResult> SubPaperByLevel(string paper)
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

                if (paper != null)
                {
                    var req = await _myUtilities.LoadSubjectPapers(paper);
                    model = JsonConvert.DeserializeObject<List<subjectpaperList>>(req.PayLoad);
                    

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

        public async Task<IActionResult> PrintSubjectMarkList(SubmarkVm model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Year))
                {
                    model.Year = DateTime.Now.Year.ToString();
                }
               
                model.PaperCode = model.PaperCode.Replace("/", "-");
                ApiResponse resp = await _myUtilities.GenerateMarkList(model);
                if (resp!=null)
                {
                    HttpContext.Session.Set("Filenamesbm", Encoding.UTF8.GetBytes(resp.PayLoad));
                    var dictionaryBytes = HttpContext.Session.Get("Filenamesbm");
                    HttpContext.Session.Remove("Filenamesbm");

                    if (dictionaryBytes != null)
                    {
                        string dictionaryJson = Encoding.UTF8.GetString(dictionaryBytes).Replace("\"", "");
                        string fileName = Path.GetFileName(dictionaryJson);

                        if (!string.IsNullOrEmpty(dictionaryJson))
                        {
                            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(dictionaryJson);
                            return new FileContentResult(fileBytes, "application/pdf");
                        }
                        //return View();
                    }
                    
                }
                else
                {
                    TempData["error"] = "Error Occured Contact Admin";
                    return RedirectToAction("Index");
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }

            TempData["error"] = "No Report To Show";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PrintSubjectMarkSheet(SubmarkVm model)
        {
            try
            {
                model.PaperCode = model.PaperCode.Replace("/", "-");
                model.Year = DateTime.Now.Year.ToString();
                ApiResponse resp = await _myUtilities.GenerateMarkSheet(model);

                HttpContext.Session.Set("Filenamesbm", Encoding.UTF8.GetBytes(resp.PayLoad));
                var dictionaryBytes = HttpContext.Session.Get("Filenamesbm");
                HttpContext.Session.Remove("Filenamesbm");

                if (dictionaryBytes != null)
                {
                    string dictionaryJson = Encoding.UTF8.GetString(dictionaryBytes).Replace("\"", "");
                    string fileName = Path.GetFileName(dictionaryJson);

                    if (!string.IsNullOrEmpty(dictionaryJson))
                    {
                        byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(dictionaryJson);
                        return new FileContentResult(fileBytes, "application/pdf");
                    }
                    //return View();
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }

            TempData["error"] = "No Report To Show";
            return RedirectToAction("Index");
        }
        
        public async Task<ActionResult> SubjectByLevels(string userid, int classs, string streamcode)
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

                if (userid != null)
                {
                    model = await _myUtilities._LoadSubjects(userid, classs, streamcode ?? "0");
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
        public async Task<ActionResult> StreamsByClasss(string userid, int classs)
        {
            try
            {
                string resp = "";

                object model = "";

                if (classs != 0)
                {
                    model = await _myUtilities._LoadStreams(userid, classs);
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
        public async Task<ActionResult> SubPaperByLevels(string userid, int classs, string streamcode, string subjectcode)
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

                if (userid != null)
                {

                    model = await _myUtilities._LoadSubjectPapers(userid, classs, streamcode, subjectcode);

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
