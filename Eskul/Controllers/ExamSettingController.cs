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
using System.Configuration;
using System.Reflection;

namespace Eskul.Controllers
{
    public class ExamSettingController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ExamSettingController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;   
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        // GET: ExamSettingController
        public async Task<ActionResult> Index(ExamSettingAdd model,int id)
        {
            try
            {
                if (!SessionData.IsSignedIn) {  return RedirectToAction("Index", "Login"); }
                if (id!=0)
                {
                    Url = $"Examination/ExamSetting/Get/ByCode/{SessionData.ClientCode}/{id}";
                    var c = await request.Get<ExamSetting>(Url);
                    model.ExamsettingId = c.FirstOrDefault().ExamsettingId;
                    model.Class = c.FirstOrDefault().Class;
                    model.Term = c.FirstOrDefault().Term;
                    model.Year = c.FirstOrDefault().Year;
                    model.Exam = c.FirstOrDefault().ExamCode;
                    model.PassMark = c.FirstOrDefault().PassMark;
                    model.delete = false;
                    model.SchoolCode = SessionData.ClientCode;
                    model.ApplyToAllClasses = c.FirstOrDefault().ApplyToAllClasses;
                }
               ApiResponse res = await _myUtilities.LoadExamSettings();
                if (res != null && res.ResponseCode == 100)
                { model.examSettings = JsonConvert.DeserializeObject<List<ExamSetting>>(res.PayLoad); }
                else if (res != null && res.ResponseCode == 101) { TempData["info"] = res.ResponseMessage; }
                else { TempData["error"] = "Error Occured Contact Admin";}

                    
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
                response = await _myUtilities.LoadExams();

                if (response != null && response.ResponseCode == 100)
                {
                    List<ExamTypeVm> classLists = JsonConvert.DeserializeObject<List<ExamTypeVm>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.ExamName, Value = p.ExamCode.ToString() }).ToList();

                    ViewBag.ExamType = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Classes = LoadListItems(l, true);
                }
                if (string.IsNullOrEmpty(model.Year)) { model.Year = DateTime.Now.Year.ToString(); }
                model.Term=SessionData.Term;
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // POST: ExamSettingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExamSettingAdd model)
        {
            Url = "Examinations/ExamSetting/Save";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                model.SchoolCode = SessionData.ClientCode;
                
                //if (string.IsNullOrEmpty(model.Code)) { model.Code = "00000"; }
                resp = await request.AddAsync<ExamSettingAdd>(model, Url);
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

        // GET: ExamSettingController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            
            return RedirectToAction(nameof(Index), new {id=id});
        }
        // GET: ExamSettingController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            var json = "";
            string resp = "";
            string UpUrl = "Examination/ExamSetting/Update";
            string EditUrl = $"Examination/ExamSetting/Get/ByCode/{SessionData.ClientCode}/{id}";
            var model = new ExamSetting();
            try
            {
                

                var c = await request.Get<ExamSetting>(EditUrl);
                model.ExamsettingId = c.FirstOrDefault().ExamsettingId;
                model.Class = c.FirstOrDefault().Class;
                model.Term = c.FirstOrDefault().Term;
                model.Year = c.FirstOrDefault().Year;
                model.Exam = c.FirstOrDefault().ExamCode;
                model.PassMark = c.FirstOrDefault().PassMark;
                model.ApplyToAllClasses = c.FirstOrDefault().ApplyToAllClasses;
                model.delete = true;
                model.SchoolCode = SessionData.ClientCode;
                resp = await request.Update<ExamSetting>(model, UpUrl);
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
                TempData["error"] = "Error Occured Con";
                return Content(json, "application/json");
                
            }
        }
       
    }
}
