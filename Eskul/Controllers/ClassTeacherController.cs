using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class ClassTeacherController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ClassTeacherController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        // GET: ClassTeacherController
        public async Task<ActionResult> Index(Classteacher model,decimal id)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                if (id != 0)
                {
                    var coid = Convert.ToInt32(id);
                    ApiResponse respons = await _myUtilities.LoadClassTeacherAsync(coid);
                    if (respons.Success)
                    {
                        model = JsonConvert.DeserializeObject<Classteacher>(respons.PayLoad);
                    }
                }

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
                resp = await _myUtilities.LoadStaffsByCategory("T");

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<StaffList> StaffList = JsonConvert.DeserializeObject<List<StaffList>>(resp.PayLoad);
                    var l = (from p in StaffList select new ListItems { Text = p.FullName, Value = p.StaffId.ToString() }).ToList();

                    ViewBag.Teachers = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Teachers = LoadListItems(l, true);
                }
                model.year = DateTime.Now.Year.ToString();
                ApiResponse respon = await _myUtilities.LoadClassTeachersAsync();
                if (respon.Success && respon.ResponseCode == 100 && respon.PayLoad != null)
                {
                    model.classteachers = JsonConvert.DeserializeObject<List<Classteacher>>(respon.PayLoad);
                }
                else if (respon.ResponseCode == 101)
                {
                    TempData["info"] = respon.ResponseMessage;
                }
                else
                {
                    TempData["error"] = respon.ResponseMessage;
                }
            }
            catch (Exception ex)
            {

                TempData["error"] = "Error Occured Please Contact Admin";
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }
        // POST: ClassTeacherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Classteacher model)
        {
            Url = "Academics/SaveClassTeacher";
            ApiResponse resp;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                model.SchoolCode = SessionData.ClientCode;
                resp = await request.AddAsync<Classteacher>(model, Url);
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

        public async Task<ActionResult> Edit(decimal id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            var model = new Classteacher();
            string resp = "";
            try
            {
                var coid = Convert.ToInt32(id);
                ApiResponse respons = await _myUtilities.LoadClassTeacherAsync(coid);
                if (respons.Success && respons.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<Classteacher>(respons.PayLoad);
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

        public async Task<ActionResult> Delete(decimal id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            var coid = Convert.ToInt32(id);
            Url = $"Academics/ClassTeacher/{SessionData.ClientCode}/{coid}";
            try
            {
                var myresp = await request.DeleteAsync(Url);
                var data = new { status = 200, res = myresp.ResponseMessage };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
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
        public async Task<ActionResult> SubjectByLevel(string Level)
        {
            try
            {
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
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");

            }

        }
    }
}
