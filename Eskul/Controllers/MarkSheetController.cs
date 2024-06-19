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
    public class MarkSheetController : BaseController
    {
        RequestHandler request;
        private readonly IConfiguration configuration;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: MarkSheetController
        public MarkSheetController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _logger = logger;
            _sd = new SessionDetail();
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(MarksList model)
        {
            try



            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var l = (from p in await _myUtilities.LoadClasses(model.Level ?? "O") select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();
                ViewBag.Classes = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadSubjects(model.Level ?? "O") select new ListItems { Text = p.Subject_name, Value = p.Subject_code.ToString() }).ToList();
                //ViewBag.Subjects = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadSubjectPapers(model.SubjectCode ?? "0") select new ListItems { Text = p.Paper_Name, Value = p.Paper_Code.ToString() }).ToList();
                //ViewBag.SubjectPapers = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadStreams(model.Class) select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();
                //ViewBag.Streams = LoadListItems(l, true);
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

                    string Url = "Examination/MarkSheet/All/Get/" + model.Year + "/" + model.Branch + "/" + model.TermCode + "/" + model.Class + "/" + model.Stream + "/" + model.ExamCode;
                    var resp = await request.GetB(Url);
                    var document = JsonDocument.Parse(resp);
                    var root = document.RootElement;

                    var offeredPapersArray = root.GetProperty("OfferedPapers").ToString();
                    var offeredPapers = System.Text.Json.JsonSerializer.Deserialize<List<OfferedPapersModel>>(offeredPapersArray);
                    var studentMarksArray = root.GetProperty("StudentMarks").ToString();

                    // Deserialize the "StudentMarks" array into a List<StudentMarksModel>
                    var studentMarks = System.Text.Json.JsonSerializer.Deserialize<List<StudentMarksModel>>(studentMarksArray);
                    // Extract unique subject codes from studentMarks
                    var subjectCodes = studentMarks.SelectMany(m => m.MarksLists.Select(ml => ml.SubjectCode)).Distinct().ToList();

                    ViewBag.Subjects = subjectCodes;
                    model.OfferedPapers = offeredPapers;
                    model.StudentMarks = studentMarks;
                    return View(model);


                }
                else
                {

                    var model1 = new MarksList();
                    model1.Year = DateTime.Now.Year.ToString();
                    model1.Branch = SessionData.UserBranchId;
                    if (model1.TermCode == 0)
                    {
                        model.TermCode = SessionData.Term;
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

        public async Task<IActionResult> PrintMarkSheet(MarksList model)
        {
            try
            {
                string url = $"Examination/MarkSheet/WritePDF/{model.Year}/{model.Branch}/{model.TermCode}/{model.Class}/{model.Stream}/{model.ExamCode}";
                var resp = await request.GetB(url);

                HttpContext.Session.Set("Filenamem", Encoding.UTF8.GetBytes(resp));
                var dictionaryBytes = HttpContext.Session.Get("Filenamem");
                HttpContext.Session.Remove("Filenamem");

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
                    //model = await _myUtilities.LoadStreams(classs);
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
