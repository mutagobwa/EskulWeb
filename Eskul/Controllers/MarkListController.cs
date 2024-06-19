using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using SmartPaperEdms.Web.App_Code;
using System.Text;
using System.Text.Json;

namespace Eskul.Controllers
{
    public class MarkListController : BaseController
    {
        RequestHandler request;
        private readonly IConfiguration configuration;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: MarkListController
        public MarkListController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _logger = logger;
            _sd = new SessionDetail();
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult >Index(ExamMarksList model)
                {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse? resp = null;
                resp = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (resp != null && resp.ResponseCode == 100)
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
                    if (string.IsNullOrEmpty(model.Stream))
                    {
                        model.Stream = "0";
                    }
                    if (model.TermCode == 0)
                    {
                        model.TermCode = SessionData.Term;
                    }

                    ApiResponse response = await _myUtilities._LoadMarkListStudents(model);
                    if (response.Success && response.PayLoad != null)
                    {
                        model = JsonConvert.DeserializeObject<ExamMarksList>(response.PayLoad);

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

                    //var document = JsonDocument.Parse(resp2);
                    //var root = document.RootElement;

                    //var offeredPapersArray = root.GetProperty("OfferedPapers").ToString();
                    //var offeredPapers = System.Text.Json.JsonSerializer.Deserialize<List<OfferedPapersModel>>(offeredPapersArray);
                    //var studentMarksArray = model.StudentMarks.ToList();
                    //var studentMarks = System.Text.Json.JsonSerializer.Deserialize<List<StudentMarksModel>>(studentMarksArray);
                    var subjectCodes = model.StudentMarks.SelectMany(m => m.MarksLists.Select(ml => ml.SubjectCode)).Distinct().ToList();

                    ViewBag.Subjects = subjectCodes;
                    //model.OfferedPapers = offeredPapers;
                    //model.StudentMarks = studentMarks;
                    return View(model);


                }
                else
                {

                    var model1 = new ExamMarksList();
                    model1.Year = DateTime.Now.Year.ToString();
                    //model1.Branch = SessionData.UserBranchId;
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
                    TempData["error"] = "Error Occured" + " " + "The Search had no Results.";
                }
                else
                {
                    TempData["error"] = "Error Occured Contact Admin" ;
                }

                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
        }
     
        // GET: MarkListController/Create
        public async Task<IActionResult> PrintMatkList(MarksList model)
       {
            try
            {

                ApiResponse response = await _myUtilities._GenerateMarklist(model);
                if (response.Success && response.PayLoad != null)
                {
                    HttpContext.Session.Set("Filenames", Encoding.UTF8.GetBytes(response.PayLoad));
                    var dictionaryBytes = HttpContext.Session.Get("Filenames");
                    HttpContext.Session.Remove("Filenames");

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
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            
            TempData["error"] = "No Report To Show";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PrintMarkSheet(MarksList model)
        {
            try
            {
                ApiResponse response = await _myUtilities._GenerateMarksheet(model);
                if (response.Success && response.PayLoad != null)
                {
                    HttpContext.Session.Set("Filenamem", Encoding.UTF8.GetBytes(response.PayLoad));
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
