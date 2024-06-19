using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using static iText.Signatures.LtvVerification;

namespace Eskul.Controllers
{
    public class StudentSubjectsController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: StudentHouseController
        public StudentSubjectsController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        // GET: StudentSubjectsController
        public async Task<ActionResult> Index(StudentSubjectsViewDTO model)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse resp = null;
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
                resp = await _myUtilities.LoadStudentsAsync(model.Level??"O",model.Class,model.Stream,false,false);

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<StudentViewDTO> studLists = JsonConvert.DeserializeObject<List<StudentViewDTO>>(resp.PayLoad);
                    var l = (from p in studLists select new ListItems { Text = p.FullName, Value = p.StudentId.ToString() }).ToList();

                    ViewBag.Students = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Students = LoadListItems(l, true);
                }
                if (!string.IsNullOrEmpty(model.StudentId))
                {
                    ApiResponse res = await _myUtilities.LoadStudentSubjectsAsync(model.StudentId ?? "0");
                    if (res.ResponseCode == 100 && res.PayLoad != null)
                    {
                        model  = JsonConvert.DeserializeObject<StudentSubjectsViewDTO>(res.PayLoad);
                    }
                     
                    
                }
                else
                {

                    var model1 = new StudentSubjectsViewDTO();

                    return View(model1);
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;

            }
            return View(model);
        }

        // GET: StudentSubjectsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StudentSubjectsController/Create
        public async Task<ActionResult> Create(List<StudentSubject> subjects)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            return RedirectToAction(nameof(Index));
        }

        // POST: StudentSubjectsController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            ApiResponse resp = null;
            List<OfferedSubject> subjects = new List<OfferedSubject>();
            var studentId = collection["sid"];

            foreach (var key in collection.Keys)
            {
                var value = collection[key];
                if (key.StartsWith("paper_")) // Check if it's a paper checkbox input
                {
                    string paperCode = key.Substring("paper_".Length);
                    string subjectCode = paperCode.Split('/')[0];

                    // Check if the OfferedSubject object exists in the subjects list
                    var subject = subjects.FirstOrDefault(s => s.SubjectCode == subjectCode);
                    if (subject == null)
                    {
                        subject = new OfferedSubject
                        {
                            SubjectCode = subjectCode,
                            IsOffered = collection["statuss_" + subjectCode] == "on",
                            Papers = new Dictionary<string, bool>()
                        };
                        subjects.Add(subject);
                    }

                    bool isChecked = !string.IsNullOrEmpty(value);

                    subject.Papers.Add(paperCode, isChecked);
                }
            }

            var model = new StudentSubjectsApiModel
            {
                StudentId = studentId,
                Subjects = subjects
                
            };

            var json = "";
            string Url = "Students/Subjects";
            model.SchoolCode = SessionData.ClientCode;
            
            resp = await request.UpdateAsync<StudentSubjectsApiModel>(model, Url);

            if (resp!=null && resp.ResponseCode==100)
            {
                var response = new
                {
                    status = resp.ResponseCode,
                    res = resp.ResponseMessage
                };

                json = JsonConvert.SerializeObject(response);
            }
            else
            {
                var response = new
                {
                    status = 101,
                    res = resp.ResponseMessage
                };

                json = JsonConvert.SerializeObject(response);
            }

            return Content(json, "application/json");
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentSubjectsController/Edit/5
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

        // GET: StudentSubjectsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StudentSubjectsController/Delete/5
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

        public async Task<ActionResult> LoadStudentList(string level,int classs,string Stream)
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

                if (classs != 0)
                {
                    ApiResponse res = await _myUtilities.LoadStudentsAsync(level,classs, Stream,false,false);
                    if (res.ResponseCode == 100 && res.PayLoad != null)
                    {
                        model = JsonConvert.DeserializeObject<List<StudentViewDTO>>(res.PayLoad);
                    }
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
    }
}
