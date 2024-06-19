using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Text;
using System.Text.Json;

namespace Eskul.Controllers
{
    public class SubjectMarkListController : BaseController
    {
        RequestHandler request;
        private readonly IConfiguration configuration;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: SubjectMarkListController
        public SubjectMarkListController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _logger = logger;
            _sd = new SessionDetail();
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(SubmarkVm model)
        {
            try



            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var l = (from p in await _myUtilities._LoadClasses(SessionData.UserId) select new ListItems { Value = p.Class.ToString(), Text = p.ClassName }).ToList();
                ViewBag.Classes = LoadListItems(l, true);
                l = (from p in await _myUtilities._LoadSubjects(SessionData.UserId, model.Class, model.Stream ?? "0") select new ListItems { Value = p.Subjectcode.ToString(), Text = p.Subject_name }).ToList();
                ViewBag.Subjects = LoadListItems(l, true);
                l = (from p in await _myUtilities._LoadSubjectPapers(SessionData.UserId, model.Class, model.Stream ?? "0", model.SubjectCode ?? "0") select new ListItems { Value = p.PaperCode.ToString(), Text = p.Paper_Name }).ToList();
                ViewBag.SubjectPapers = LoadListItems(l, true);
                l = (from p in await _myUtilities._LoadStreams(SessionData.UserId,model.Class) select new ListItems { Text = p.streamname, Value = p.streamcode.ToString() }).ToList();
                ViewBag.Streams = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadExams(true) select new ListItems { Text = p.ExamName, Value = p.ExamCode.ToString() }).ToList();
                //ViewBag.ExamType = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadBranches(true) select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();
                //ViewBag.Branches = LoadListItems(l, true);
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
                    model.PaperCode = model.PaperCode.Replace("/", "-");
                    string Url = "Examination/MarksList/Subject/Get/" + model.Year + "/" +  model.TermCode + "/" + model.Class + "/" + model.Stream + "/" + model.SubjectCode + "/" + model.PaperCode + "/" + model.ExamCode;
                    
                    model.submarklists= await request.GetAll<Submarklist>(Url);
                    return View(model);
                }
                else
                {

                    var model1 = new SubmarkVm();
                    model1.Year = DateTime.Now.Year.ToString();
                    //model1.Branch = SessionData.UserBranchId;
                    if (model1.TermCode == 0)
                    {
                        model.TermCode= SessionData.Term;
                    }
                    return View(model1);
                }


            }
            catch (Exception ex)
            {
                if (ex.Message == "Object reference not set to an instance of an object.")
                {
                    TempData["error"] = "The Search had no Results.";
                }
                else
                {
                    TempData["error"] = "Error Occured Contact Admin" ;
                }

                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> PrintSubjectMarkList(SubmarkVm model)
        {
            try
            {
                model.PaperCode = model.PaperCode.Replace("/","-");
                string url = $"Examination/MarksList/Subject/WritePDF/{model.Year}/{model.TermCode}/{model.Class}/{model.Stream}/{model.SubjectCode}/{model.PaperCode}/{model.ExamCode}";
                var resp = await request.GetB(url);

                HttpContext.Session.Set("Filenamesbm", Encoding.UTF8.GetBytes(resp));
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
        public async Task<ActionResult> SubjectByLevel(string userid, int classs, string streamcode)
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
        public async Task<ActionResult> StreamsByClass(string userid, int classs)
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
        public async Task<ActionResult> SubPaperByLevel(string userid, int classs, string streamcode,string subjectcode)
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

                    model = await _myUtilities._LoadSubjectPapers(userid, classs, streamcode,subjectcode);

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
