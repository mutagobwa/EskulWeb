using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Linq;
using System.Reflection;

namespace Eskul.Controllers
{
    public class SubjectTeacherController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public SubjectTeacherController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        // GET: SubjectTeacherController
        public async Task<ActionResult> Index(Subjecteacher model, decimal id)
        {
            try
            {
                if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}
                ApiResponse resp;
                
                ApiResponse response = await _myUtilities.LoadSubjecteachersAsync();
                if (response.Success)
                {
                    model.subjecteachers = JsonConvert.DeserializeObject<List<Subjecteacher>>(response.PayLoad);
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
               

                //if (respons.Contains("successfully"))
                //{
                //    model.subjecteachers = respons; /*JsonConvert.DeserializeObject<List<Subjecteacher>>(respons.PayLoad);*/
                //}
                resp = await _myUtilities.LoadClassess();

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
                resp = await _myUtilities.LoadSubjectPapers(model.subjectcode ?? "0");

                if (resp != null && resp.Success)
                {
                    List<subjectpaperList> SubjectPaperLists = JsonConvert.DeserializeObject<List<subjectpaperList>>(resp.PayLoad);
                    var l = (from p in SubjectPaperLists select new ListItems { Text = p.PaperName, Value = p.PaperCode.ToString() }).ToList();

                    ViewBag.SubjectPapers = LoadListItems(l, true);
                }
                resp = await _myUtilities.LoadStaffsByCategory("T");

                if (resp != null && resp.Success)
                {
                    List<StaffList> SubjectPaperLists = JsonConvert.DeserializeObject<List<StaffList>>(resp.PayLoad);
                    var l = (from p in SubjectPaperLists select new ListItems { Text = p.FullName, Value = p.StaffId.ToString() }).ToList();

                    ViewBag.Teachers = LoadListItems(l, true);
                }
               
                model.year=DateTime.Now.Year.ToString();
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
            }
            return View(model);
        }

        // POST: SubjectTeacherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Subjecteacher model)
        {
            Url = "Academics/SaveSubjectTeacher";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                model.SchoolCode = SessionData.ClientCode;
                //if (model.StatusId == 0) { model.StatusId = 3; }
                ////if (string.IsNullOrEmpty(model.Code)) { model.Code = "00000"; }
                resp = await request.AddAsync<Subjecteacher>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    TempData["success"] = resp.ResponseMessage;

                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: SubjectTeacherController/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            var model = new Subjecteacher();
            string resp = "";
            try
            {
                var coid = Convert.ToInt32(id);
                ApiResponse respons = await _myUtilities.LoadSubjecteacherAsync(coid);
                if (respons.Success && respons.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<Subjecteacher>(respons.PayLoad);
                }
                else if (respons.ResponseCode == 101)
                {
                    TempData["info"] = respons.ResponseMessage;
                }
                else
                {
                    TempData["error"] = respons.ResponseMessage;
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction("Index");
            }


        }
        // GET: SubjectTeacherController/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            var json = "";
            string resp = "";
            string UpdateUrl = "Academics/SaveSubjectTeacher";
            string EditUrl = $"Academics/SubjectTeacher/GetById/{SessionData.ClientCode}/{(int)id}";
            var model = new Subjecteacher();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<Subjecteacher>(EditUrl);

                model.year = c.FirstOrDefault().year;
                model.Class = c.FirstOrDefault().Class;
                model.tearcherid = c.FirstOrDefault().tearcherid;
                model.subjectcode = c.FirstOrDefault().subjectcode;
                model.papercode = c.FirstOrDefault().papercode;
                model.Stream = c.FirstOrDefault().Stream;
                model.ApplyToAllStreams = c.FirstOrDefault().ApplyToAllStreams;
                model.delete = true;
                model.SchoolCode = SessionData.ClientCode;
                resp = await request.Add<Subjecteacher>(model, UpdateUrl);
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
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");

            }

        }
        public async Task<ActionResult> SubjectByLevel(string Level)
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
    }
}
