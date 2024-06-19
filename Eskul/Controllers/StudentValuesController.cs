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
using System.Runtime.CompilerServices;

namespace Eskul.Controllers
{
    public class StudentValuesController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public StudentValuesController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(GsCat model)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login");}
                model.gsCats = await _myUtilities.LoadStudentCatValues(true);
            }
            catch (Exception ex)
            {
                TempData["error"] ="Error Occured Please Contact Admin";
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }


        public async Task<ActionResult> Definitions(ValueDefinitions model, int id, int id2)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }
                var l = (from p in await _myUtilities.LoadStudentCatValues(true) select new ListItems { Text = p.categoryName, Value = p.categoryId.ToString() }).ToList();
                ViewBag.StudentCatValues = LoadListItems(l, true);
                if (id2 != 0) { model.valueDefinitions = await _myUtilities.LoadValueDefinitions(id2); }
                else { model.valueDefinitions = await _myUtilities.LoadValueDefinitions(id); }
                return View(model);
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Definitions(ValueDefinitions model)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }

            Url = "Academics/StudentValues/Definition/Save";
            string resp = "";
            try
            {
                model.SchoolCode = SessionData.ClientCode;
                model.StatusId = 3;
                model.CategoryName = "";
                resp = await request.Add<ValueDefinitions>(model,Url);
                if (resp.Contains("successfully"))
                    TempData["success"] =resp;
                else
                  TempData["error"] =resp;
                return RedirectToAction(nameof(Definitions),new{id2 = model.CategoryId});
            }
            catch (Exception ex)
            {
              _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GsCat model)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }
            Url = "Academics/StudentValues/Category/Save";
            string resp = "";
            try
            {
                model.schoolCode = SessionData.ClientCode;
                resp = await request.Add<GsCat>(model,Url);
                if (resp.Contains("successfully")) { TempData["success"] = resp; }
                else { TempData["error"] = resp; }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
               _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> _Edit(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }
            var model = new ValueDefinitions();
            try
            {
                ApiResponse response = await _myUtilities.LoadValueDefinition(id);
                if (response.Success)
                {
                    model = JsonConvert.DeserializeObject<ValueDefinitions>(response.PayLoad);
                    return Json(model);
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
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }
            var model = new GsCat();
            try
            {
                model = await _myUtilities._LoadGSCat(id);
                return Json(model);
            }
            catch (Exception ex)
            {
               _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }

            string json = "";
            Url = "Academics/StudentValues/Category/Save";
            var model= new GsCat();
            try
            {
                model = await _myUtilities._LoadGSCat(id);
                model.schoolCode = SessionData.ClientCode;
                model.statusId = 5;
                var resp= await request.Add<GsCat>(model,Url);
                if (resp.Contains("successfully"))
                {
                    var data = new { status = 200, res = resp };
                    json = JsonConvert.SerializeObject(data);
                }
                else
                {
                    var data = new { status = 201, res = resp };
                    json = JsonConvert.SerializeObject((object)data);
                }
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
               TempData["error"] = "Error Occured Please Contact Admin";
               _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
            }
        }

        public async Task<ActionResult> _Delete(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }

            string json = "";
            Url = "Academics/StudentValues/Definition/Save";
            var model = new ValueDefinitions();
            try
            {
               ApiResponse response= await _myUtilities.LoadValueDefinition(id);
                model.SchoolCode = SessionData.ClientCode;
                model.StatusId = 5;
                var resp= await request.Add<ValueDefinitions>(model,Url);
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
                TempData["error"] ="Error Occured Please Contact Admin";
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
            }
        }
    }
}
