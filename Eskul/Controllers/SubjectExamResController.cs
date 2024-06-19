using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class SubjectExamResController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: SubjectExamResController
        public SubjectExamResController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(SubjectAssignments model)
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
                if (!string.IsNullOrEmpty(model.SubjectCode))
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
                else
                {

                    var model1 = new Assignment();
                    model1.Class = model.Class;
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

        // GET: SubjectExamResController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubjectExamResController/Create
        public async Task<ActionResult> Create(Assignment model, int id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
                var l = (from p in await _myUtilities._LoadClasses(SessionData.UserId) select new ListItems { Value = p.Class.ToString(), Text = p.ClassName }).ToList();
                ViewBag.Classes = LoadListItems(l, true);
                l = (from p in await _myUtilities._LoadSubjects(SessionData.UserId, model.Class, model.Stream ?? "0") select new ListItems { Value = p.Subjectcode.ToString(), Text = p.Subject_name }).ToList();
                ViewBag.Subjects = LoadListItems(l, true);
                l = (from p in await _myUtilities._LoadSubjectPapers(SessionData.UserId, model.Class, model.Stream ?? "0", model.SubjectCode ?? "0") select new ListItems { Value = p.PaperCode.ToString(), Text = p.Paper_Name }).ToList();
                ViewBag.SubjectPapers = LoadListItems(l, true);
                l = (from p in await _myUtilities._LoadStreams(SessionData.UserId, model.Class) select new ListItems { Text = p.streamname, Value = p.streamcode.ToString() }).ToList();
                ViewBag.Streams = LoadListItems(l, true);


                model.Year = DateTime.Now.Year.ToString();
                if (model.Term == 0)
                {
                    model.Term=SessionData.Term;
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

        // POST: SubjectExamResController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Assignment model)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            Url = "Examination/AddAssignment";
            string resp = "";
            try
            {
                var Exists = await _myUtilities.LoadAssignment(model);
                if (Exists == null || Exists.Count == 0)
                {
                    Url = "Examination/AddAssignment";
                    model.Initials = SessionData.UserId;
                    resp = await request.Add<Assignment>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = resp;
                    }

                }
                else
                {
                    string EditUrl = "Examination/UpdateAssignment";
                    model.delete = false;
                    model.Initials = SessionData.UserId;
                    resp = await request.Update<Assignment>(model, EditUrl);
                    if (resp.Contains("successfully "))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured" + resp;
                    }
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

        // GET: SubjectExamResController/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string resp = "";
            string EditUrl = "Examination/Assignment/" + id + "";
            var model = new Assignment();
            try
            {
                var c = await request.Get<Assignment>(EditUrl);

                model.Year = c.FirstOrDefault().Year;
                model.Term = c.FirstOrDefault().Term;
                model.Class = c.FirstOrDefault().Class;
                model.SubjectCode = c.FirstOrDefault().SubjectCode;
                model.PaperCode = c.FirstOrDefault().PaperCode;
                model.AssignmentName = c.FirstOrDefault().AssignmentName;
                model.competancy = c.FirstOrDefault().competency;
                model.Description = c.FirstOrDefault().Description;
                model.PassMark = c.FirstOrDefault().PassMark;
                model.Initials = c.FirstOrDefault().Initials;
                model.StatusId = c.FirstOrDefault().StatusId;
                model.AssignmentId = c.FirstOrDefault().AssignmentId;
                model.delete = false;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured" + resp;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Create", model);
            
        }

        // POST: SubjectExamResController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            return View();
        }

        // GET: SubjectExamResController/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            var json = "";
            string resp = "";
            string UpdateUrl = "Examination/UpdateAssignmentScore";
            string EditUrl = "Examination/Assignments/" + id + "";
            var model = new Assignment();
            try
            {
                var c = await request.Get<Assignment>(EditUrl);

                model.Year = c.FirstOrDefault().Year;
                model.Term = c.FirstOrDefault().Term;
                model.Class = c.FirstOrDefault().Class;
                model.SubjectCode = c.FirstOrDefault().SubjectCode;
                model.PaperCode = c.FirstOrDefault().PaperCode;
                model.AssignmentName = c.FirstOrDefault().AssignmentName;
                model.competancy = c.FirstOrDefault().competency;
                model.Description = c.FirstOrDefault().Description;
                model.PassMark = c.FirstOrDefault().PassMark;
                model.Initials = c.FirstOrDefault().Initials;
                model.StatusId = c.FirstOrDefault().StatusId;
                model.AssignmentId = c.FirstOrDefault().AssignmentId;
                model.delete = true;

                resp = await request.Update<Assignment>(model, UpdateUrl);
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

        // POST: SubjectExamResController/Delete/5
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
        public async Task<ActionResult> AssignMarks(string id)
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
                model.StudentsList = (await _myUtilities.LoadMarkList(id)).StudentsList;
                model.assignments = await _myUtilities.LoadAssignmentById(id);
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
        public async Task<ActionResult> AssignMarks(IFormCollection form, string studentId, string score, string comment, decimal AssignmentId)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string resp = "";
            string UpUrl = "Examination/UpdateAssignmentScore";
            var model = new MarksUpdate();
            try
            {


                model.StudentId = form["studentId"];
                model.Score = int.Parse(form["score"]);
                model.Comment = form["comment"];
                if (string.IsNullOrEmpty(model.Comment))
                {
                    model.Comment = "NO COMMENT";
                }
                model.AssignmentId = decimal.Parse(form["AssignmentId"]);
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
        public async Task<ActionResult> SubPaperByLevel(string userid, int classs, string streamcode, string subjectcode)
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
        public async Task<ActionResult> SubjectByLevel(string userid,int classs, string streamcode)
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
                    model = await _myUtilities._LoadSubjects(userid, classs, streamcode??"0");
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
    }
}
