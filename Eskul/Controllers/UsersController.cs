using ASEEncryptorDecryptorTool;
using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class UsersController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public UsersController( IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        // GET: UsersController/
        public async Task<ActionResult> Index(UserS model)
        {

            try
            {

                if (!SessionData.IsSignedIn) {  return RedirectToAction("Index", "Login"); }
                ApiResponse resp;
                resp = await _myUtilities.LoadProfiles();

                if (resp != null && resp.ResponseCode == 100)
                {
                    List<ProfileList> classLists = JsonConvert.DeserializeObject<List<ProfileList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.Code.ToString() }).ToList();

                    ViewBag.Profiles = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Profiles = LoadListItems(l, true);
                }
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
                if (!string.IsNullOrEmpty(model.UserType))
                {

                    ApiResponse response = await _myUtilities.LoadUsers(model);
                    if (response.Success && response.ResponseCode == 100 && response.PayLoad != null)
                    {
                       model.userS= JsonConvert.DeserializeObject<List<UserS>>(response.PayLoad);

                    }
                    else if (response.ResponseCode == 101)
                    {
                        TempData["info"] = response.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = response.ResponseMessage;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: UsersController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var model=new UserS();
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse resp = await _myUtilities.LoadUser(id);
                if (resp.Success && resp.ResponseCode == 100 && resp.PayLoad != null)
                {
                    model = JsonConvert.DeserializeObject<UserS>(resp.PayLoad);

                }
                else if (resp.ResponseCode == 101)
                {
                    TempData["info"] = resp.ResponseMessage;
                }
                else
                {
                    TempData["error"] = resp.ResponseMessage;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return View();
            }
           
        }

        // GET: UsersController/Create
        public async Task<ActionResult> Create(UserAdd model,string id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            ApiResponse resp;
            resp = await _myUtilities.LoadProfiles();

            if (resp != null && resp.ResponseCode == 100)
            {
                List<ProfileList> classLists = JsonConvert.DeserializeObject<List<ProfileList>>(resp.PayLoad);
                var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.Code.ToString() }).ToList();

                ViewBag.Profiles = LoadListItems(l, true);
            }
            else
            {
                var l = new List<ListItems>();
                ViewBag.Profiles = LoadListItems(l, true);
            }

            if (id != null)
            {
                ApiResponse response1 = await _myUtilities.LoadUser(id);

                if (response1.Success)
                {
                    model = JsonConvert.DeserializeObject<UserAdd>(response1.PayLoad);
                }
                else { TempData["info"] = response1.ResponseMessage; }

                return View(model);
            }
            model.UserName = "";
            model.Password = "";
            return View(model);
        }
        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserAdd model, IFormCollection form)
        {
            model.ReferenceNo = form["ReferenceNo"].ToString();
            ApiResponse resp;
            
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse resps = await _myUtilities.LoadUser(model.ReferenceNo);
                if (resps.Success && resps.ResponseCode == 100 && resps.PayLoad != null)
                {
                    Url = "Users";
                    model.SchoolCode = SessionData.ClientCode;
                    model.UserSession = "";
                    resp = await request.UpdateAsync<UserAdd>(model, Url);
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
                    Url = "Users";
                    model.UserSession = "";
                    model.SchoolCode = SessionData.ClientCode;
                    model.Password = Crypto.EncryptString(model.Password);
                    resp = await request.AddAsync<UserAdd>(model, Url);
                    if (resp != null && resp.ResponseCode == 100)
                    {
                        TempData["success"] = resp.ResponseMessage;
                    }
                    else if (resp != null && resp.ResponseCode == 101) { TempData["info"] = resp.ResponseMessage; }
                    else { TempData["error"] = "Error Occured Contact Admin"; }
                }
                return RedirectToAction(nameof(Index), new {model.UserType,model.ProfileCode});
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index), new { model.UserType, model.ProfileCode });
            }
        }
        public async Task<ActionResult> ResetUser(string Id)
        {
            try
            {
                ApiResponse resp;
                UserAdd userAdd = new UserAdd();
                Url = $"Users/Reset/{SessionData.ClientCode}/{Id}";
                resp = await request.UpdateAsync(userAdd, Url);
                if (resp.Success && resp.ResponseCode == 100 && resp.PayLoad != null)
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
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
        }

       

        // GET: UsersController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create), new {id=id});
        }
        // GET: UsersController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            Url = $"Users/Delete/{SessionData.ClientCode}/{id}";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoadUserss(UserS model)
        {
            try
            {

                ApiResponse res = await _myUtilities.LoadUsers(model);
                if (res.Success) {
                    model.userS= JsonConvert.DeserializeObject<List<UserS>>(res.PayLoad);
                }
                
            }
            catch (Exception ex)
            {

                
            }
            return RedirectToAction(nameof(Index), model);

        }
        public async Task<ActionResult> GetUserCodes(string UserType)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }

                ApiResponse resp;
                object model;

                switch (UserType)
                {
                    case "Parent":
                        resp = await _myUtilities.LoadParents();
                        break;

                    case "Staff":
                        resp = await _myUtilities.LoadStaffsByCategory("T");
                        break;

                    default:
                        return Content(JsonConvert.SerializeObject(new { status = 404, res = "Error" }), "application/json");
                }

                if (resp.Success)
                {
                    model = UserType == "Parent"
                        ? JsonConvert.DeserializeObject<List<ParentInfo>>(resp.PayLoad)
                        : JsonConvert.DeserializeObject<List<StaffModel>>(resp.PayLoad);

                    var data = new { status = 200, res = resp.ResponseMessage };
                    var json = JsonConvert.SerializeObject(model);
                    return Content(json, "application/json");
                }
                else
                {
                    TempData["error"] = resp.ResponseMessage ?? "Response Unknown";
                    var data = new { status = resp.ResponseCode, res = resp.ResponseMessage };
                    var json = JsonConvert.SerializeObject(data);
                    return Content(json, "application/json");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occurred. Contact Admin";
                var data = new { status = 201, message = ex.Message };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
            }
        }
        public async Task<ActionResult> LoadStudentList(string Level, int classs, string Stream)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }

                if (classs == 0)
                {
                    var errorResponse = new
                    {
                        status = 404,
                        res = "Error: Invalid class parameter"
                    };
                    return Content(JsonConvert.SerializeObject(errorResponse), "application/json");
                }

                ApiResponse resp = await _myUtilities.LoadStudentsAsync(Level, classs, Stream ?? "0", false, false);

                if (resp.Success)
                {
                    var students = JsonConvert.DeserializeObject<List<StudentViewDTO>>(resp.PayLoad);
                    var successResponse = new
                    {
                        status = 200,
                        res = resp.ResponseMessage
                    };
                    var json = JsonConvert.SerializeObject(students);
                    return Content(json, "application/json");
                }
                else
                {
                    TempData["error"] = resp.ResponseMessage ?? "Response Unknown";
                    var data = new
                    {
                        status = resp.ResponseCode,
                        res = resp.ResponseMessage
                    };
                    var json = JsonConvert.SerializeObject(data);
                    return Content(json, "application/json");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occurred. Contact Admin";
                var data = new
                {
                    status = 500,
                    message = "Internal server error",
                    detail = ex.Message
                };
                var json = JsonConvert.SerializeObject(data);
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

    }
}
