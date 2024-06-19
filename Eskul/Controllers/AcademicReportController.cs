using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Text;

namespace Eskul.Controllers
{
    public class AcademicReportController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: AcademicReportController
        public AcademicReportController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _logger = logger;
            _sd = new SessionDetail();
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(reportfil model)
        {

            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse resp;
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
                //resp = await _myUtilities.LoadBranches();

                //if (resp != null && resp.ResponseCode == 100)
                //{
                //    List<Branch> BranchLists = JsonConvert.DeserializeObject<List<Branch>>(resp.PayLoad);
                //    var l = (from p in BranchLists select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();

                //    ViewBag.Branches = LoadListItems(l, true);
                //}
                //else
                //{
                //    var l = new List<ListItems>();
                //    ViewBag.Branches = LoadListItems(l, true);
                //}
                model.Term=SessionData.Term;
                    model.Year = DateTime.Now.Year.ToString();
                
                if (model.Class != 0)
                {
                    if (model.Branch == 0) { model.Branch = SessionData.UserBranchId; }
                    if (model.Term == 0) { model.Term = SessionData.Term; }
                    if (model.Stream == null) { model.Stream = "0"; }
                   ApiResponse response= await _myUtilities.LoadList(model);
                    if (response.Success && response.PayLoad!=null )
                    {
                        model.Studs = JsonConvert.DeserializeObject<List<BioInfo>>(response.PayLoad);

                    }
                    else if (response.ResponseCode == 101)
                    {
                        TempData["Info"] = response.ResponseMessage;
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
                    if (model.Branch == 0)
                    {
                        model.Branch = SessionData.UserBranchId;
                    }
                    var model1 = new reportfil();

                    return View(model);
                }



            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }
        public ActionResult OpenPdf(string path)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }

            var dictionaryJson = "";

            var dictionaryBytes = HttpContext.Session.Get("Filename");
            if (dictionaryBytes != null)
            {
                dictionaryJson = Encoding.UTF8.GetString(dictionaryBytes).Replace("\"", "");
                HttpContext.Session.Remove("Filename"); // Remove the session value for "Filename"
            }

            string fileName = Path.GetFileName(dictionaryJson);
            if (!string.IsNullOrEmpty(dictionaryJson))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(dictionaryJson);

                // Set the response headers
                Response.Headers.Clear();
                Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);
                Response.Headers.Add("Content-Length", fileBytes.Length.ToString());
                Response.Headers.Add("Content-Type", "application/pdf");

                // Write the file content to the response stream
                Response.Body.WriteAsync(fileBytes);

                HttpContext.Session.CommitAsync(); // Commit the session changes to clear the session value
            }
            else
            {
                TempData["infor"] = "No Report To Show";
                return RedirectToAction("Index");
            }
            return new EmptyResult();
        }
            
        public ActionResult Details(int id)
        {
            return View();
        }
        public async Task<ActionResult<string>> GenerateReport([FromBody] GenerateReport model)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }

            try
            {
                string resp = "";

                if (string.IsNullOrEmpty(model.studentId))
                {
                    model.studentId = "0";
                }
                else
                {
                    model.studentId = model.studentId.Trim();
                }

                string url = "Examination/Student/Report/Generate/" + model.branch + "/" + model.year + "/" + model.term + "/" + model.classs + "/" + model.stream + "/" + model.reportType + "/" + model.checkAll + "/" + model.studentId;

                resp = await request.GetB(url);

                if (resp.Contains("ERROR"))
                {
                    TempData["error"] = "Error Occured" + " " + resp;
                    return RedirectToAction(nameof(Index));
                }
                else
                {

                    HttpContext.Session.Set("Filename", Encoding.UTF8.GetBytes(resp));
                    var data = new { status = 200, res = resp, message = "Report Successfully Generated" };
                    TempData["success"] = "Report Successfully Generated";
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> PrintReport(GenerateReport model)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }


                if (string.IsNullOrEmpty(model.studentId))
                {
                    model.studentId = "0";
                }
                else
                {
                    model.studentId = model.studentId.Trim();
                }

                ApiResponse resp = await _myUtilities.GenerateReport(model);
                if (resp.Success && resp!=null && resp.ResponseCode==100 && resp.PayLoad!=null)
                {
                    HttpContext.Session.Set("Filename", Encoding.UTF8.GetBytes(resp.PayLoad));
                    var dictionaryBytes = HttpContext.Session.Get("Filename");
                    HttpContext.Session.Remove("Filename");

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
                    else {
                        TempData["error"] = resp.ResponseMessage;
                    }
                }
                else if (resp.ResponseCode == 101)
                {
                    TempData["Info"] = resp.ResponseMessage;
                }
                else if (resp.ResponseCode == 500)
                {
                    TempData["error"] = resp.ResponseMessage;

                    
                }


            }
            catch (Exception ex)
            {

                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
            }
            return RedirectToAction("Index");
        }
    }
}
