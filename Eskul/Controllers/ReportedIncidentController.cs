using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NuGet.Packaging;
using SmartPaperEdms.Web.App_Code;
using System.Runtime.CompilerServices;

namespace Eskul.Controllers
{
    public class ReportedIncidentController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ReportedIncidentController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(ReportedIncident model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction(nameof(Index), "Login");
                }
               
                ApiResponse rescap = await _myUtilities.LoadReportedIncidentsCapturedAsync();
                if (rescap != null && rescap.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<ReportedIncident>(rescap.PayLoad);
                }
                //else if (rescap != null && rescap.ResponseCode == 101)
                //{
                //    TempData["info"] = rescap.ResponseMessage;
                //}
                //else if (rescap != null && rescap.ResponseCode == 500)
                //{
                //    TempData["error"] = rescap.ResponseMessage;
                //}
                //else
                //{
                //    TempData["error"] = "Response Unkown";
                //}
                ApiResponse resrev = await _myUtilities.LoadReportedIncidentsReviewedAsync();
                if (resrev != null && resrev.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<ReportedIncident>(resrev.PayLoad);
                }
                //else if (resrev != null && resrev.ResponseCode == 101)
                //{
                //    TempData["info"] = resrev.ResponseMessage;
                //}
                //else if (resrev != null && resrev.ResponseCode == 500)
                //{
                //    TempData["error"] = resrev.ResponseMessage;
                //}
                //else
                //{
                //    TempData["error"] = "Response Unkown";
                //}
                ApiResponse resconfirm = await _myUtilities.LoadReportedIncidentsConfirmedAsync();
                if (resconfirm != null && resconfirm.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<ReportedIncident>(resconfirm.PayLoad);
                }
                //else if (resconfirm != null && resconfirm.ResponseCode == 101)
                //{
                //    TempData["info"] = resconfirm.ResponseMessage;
                //}
                //else if (resconfirm != null && resconfirm.ResponseCode == 500)
                //{
                //    TempData["error"] = resconfirm.ResponseMessage;
                //}
                //else
                //{
                //    TempData["error"] = "Response Unkown";
                //}
                var allReportedIncidents = new List<ReportedIncident>();

                
                    allReportedIncidents.Add(model);
                if (allReportedIncidents != null) { model.reportedIncidents = allReportedIncidents; }
                else { model = new ReportedIncident(); }   
             
                
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the execution of the action method
                TempData["error"] = "An error occurred. Please contact the administrator.";
                _logger.Error(ex.Message, ex);
            }

            // Return the view with the populated model
            return View(model);
        }

        //GET: ReportedIncidentController/Create
         public async Task<ActionResult> Create(ReportedIncident model, decimal id)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }
               
                ApiResponse response = null;

                response = await _myUtilities.LoadClassess(model.Level ?? "O");

                if (response != null && response.ResponseCode == 100)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Classes = LoadListItems(l, true);
                }

                response = await _myUtilities.LoadClassStream(model.Class);

                if (response != null && response.Success)
                {
                    List<streamList> classLists = JsonConvert.DeserializeObject<List<streamList>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();

                    ViewBag.Streams = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Streams = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadBehaviorIncidentsAsync();

                if (response != null && response.Success)
                {
                    List<BehaviorIncident> classLists = JsonConvert.DeserializeObject<List<BehaviorIncident>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.BehaviorID.ToString() }).ToList();

                    ViewBag.BehaviorIncidents = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.BehaviorIncidents = LoadListItems(l, true);
                }
                response = await _myUtilities.LoadStaffsByCategory("T");

                if (response != null && response.Success)
                {
                    List<StaffList> StaffList = JsonConvert.DeserializeObject<List<StaffList>>(response.PayLoad);
                    var l = (from p in StaffList select new ListItems { Text = p.FullName, Value = p.StaffId.ToString() }).ToList();


                    ViewBag.Staffs = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Staffs = LoadListItems(l, true);
                }

                response = await _myUtilities.LoadBehaviorActionsAsync();

                if (response != null && response.Success)
                {
                    List<BehaviorAction> StaffList = JsonConvert.DeserializeObject<List<BehaviorAction>>(response.PayLoad);
                    var l = (from p in StaffList select new ListItems { Text = p.Name, Value = p.ActionID.ToString() }).ToList();


                    ViewBag.Actions = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Actions = LoadListItems(l, true);
                }
               

                response = await _myUtilities.LoadStudentsAsync(model.Level ?? "O", model.Class, model.Stream,false, false);

                if (response != null && response.ResponseCode == 100)
                {
                    List<StudentViewDTO> studLists = JsonConvert.DeserializeObject<List<StudentViewDTO>>(response.PayLoad);
                    var l = (from p in studLists select new ListItems { Text = p.FullName, Value = p.StudentId.ToString() }).ToList();

                    ViewBag.Students = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Students = LoadListItems(l, true);
                }
                if (model.Term == 0) { model.Term = SessionData.Term; }
                if (string.IsNullOrEmpty(model.IncidentStatus)) { model.IncidentStatus = "Open"; }

                int? actionId = model.ActionID;
                int num = 0;
                return actionId.GetValueOrDefault() == num & actionId.HasValue ? View() : View(model);
            }
            catch (Exception ex)
            {

                TempData["error"] = "Error Occured  Contact Admin";
                _logger.Error(ex.Message, ex);
                return RedirectToAction("Index");
            }
        }

        // POST: ReportedIncidentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ReportedIncident model)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }
                Url = "Behaviours/Incident/Reported";
                model.SchoolCode = SessionData.ClientCode;
                DateTime now;
                if (model.IncidentID == 0)
                {
                    model.CapturedBy = SessionData.UserId;
                    now = DateTime.Now;
                    string str = now.ToString("yyyy-MM-ddTHH:mm:ss");
                    model.DateCaptured = str;
                    model.Captured = true;
                }
                else
                    model.DateCaptured = model.DateCaptured;
                  model.Captured = true;
                if (model.IncidentStatus == "Closed")
                    model.Resolved = true;
                now = DateTime.Now;
                string str1 = now.Year.ToString();
                model.Year = str1;
                ApiResponse resp = await request.AddAsync<ReportedIncident>(model, Url);
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
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["error"] = "Error Occured Please Contact Admin";
                _logger.Error(ex.Message, ex);
                return RedirectToAction("Index");
            }
        }

        // GET: ReportedIncidentController/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            int id2 = (int)id;
           var model = new ReportedIncident();
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse response = await _myUtilities.LoadReportedIncidentByIdAsync(id2,true,false,false,false);
                if (response != null && response.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<ReportedIncident>(response.PayLoad);
                    //model.StudentId = model.StudentId;
                    //model.StatusId = 3;
                }
                else if (response != null && response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else if (response != null && response.ResponseCode == 500)
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
               TempData["error"] ="Error Occured Please Contact Admin";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create", (object)model);
        }
        public async Task<ActionResult> Verify(Decimal id)
        {
            int id2 = (int)id;
            var model = new ReportedIncident();
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse response = await _myUtilities.LoadReportedIncidentByIdAsync(id2, true, false, false, false);
                if (response != null && response.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<ReportedIncident>(response.PayLoad);
                    model.Reviewed = true;
                    DateTime now;
                    now = DateTime.Now;
                    string str = now.ToString("yyyy-MM-ddTHH:mm:ss");
                    model.DateReviewed = str;
                }
                else if (response != null && response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else if (response != null && response.ResponseCode == 500)
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
                TempData["error"] = "Error Occured Please Contact Admin";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create", model);
        }

        public async Task<ActionResult> Confirm(Decimal id)
        {
            int id2 = (int)id;
            var model = new ReportedIncident();
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse response = await _myUtilities.LoadReportedIncidentByIdAsync(id2, true, true, false, false);
                if (response != null && response.ResponseCode == 100)
                {
                    model = JsonConvert.DeserializeObject<ReportedIncident>(response.PayLoad);
                   model.Confirmed = true;
                    DateTime now;
                    now = DateTime.Now;
                    string str = now.ToString("yyyy-MM-ddTHH:mm:ss");
                    model.DateConfirmed = str;
                }
                else if (response != null && response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else if (response != null && response.ResponseCode == 500)
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
                TempData["error"] = "Error Occured Please Contact Admin";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create",model);
        }
        public async Task<ActionResult> Delete(Decimal id)
        {
            int id2 = (int)id;
            Url=$"BehaviourManagement/ReportedIncident/GetById/{SessionData.ClientCode}/{id2}/{true}/{false}/{false}/{false}";
            string UpdateUrl = "BehaviourManagement/ReportedIncident/Save";
            var model = new ReportedIncident();
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
               model = await request.GetSingle<ReportedIncident>(ReportedIncidentController.Url);
                model.StudentId = model.StudentId;
                model.StatusId = 5;
                var resp = await request.Add<ReportedIncident>(model, UpdateUrl);
                string content;
                if (resp.Contains("successfully"))
                {
                    var data = new { status = 200, res = resp };
                    content = JsonConvert.SerializeObject(data);
                }
                else
                {
                    var data = new { status = 201, res = resp };
                    content = JsonConvert.SerializeObject((object)data);
                }
                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Please Contact Admin";
                return RedirectToAction("Index");
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
        public async Task<ActionResult> LoadStudentList(string level,int classs, string Stream)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                string resp = "";

                object model = "";

                if (classs != 0)
                {
                    ApiResponse res = await _myUtilities.LoadStudentsAsync(level, classs, Stream,false, false);
                    model = JsonConvert.DeserializeObject<List<StudentViewDTO>>(res.PayLoad);
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
