using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Configuration;
using System.Reflection;

namespace Eskul.Controllers
{
    public class DepartmentsController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: DepartmentsController
        public DepartmentsController( IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(DepartmentVm model,string id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }

            try
            {
                ApiResponse response;
                if (!string.IsNullOrEmpty(id))
                {
                    string resp = "";
                    
                        if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                         response = await _myUtilities.LoadDepartment(id);

                        if (response.Success)
                        {
                            model = JsonConvert.DeserializeObject<DepartmentVm>(response.PayLoad);
                        }
                    }

                 response = await _myUtilities.LoadDepartments();

                if (response.Success)
                {
                    model.Departments = JsonConvert.DeserializeObject<List<DepartmentVm>>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["error"] = response.ResponseMessage;
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
                TempData["error"] = "Error Occured Contact Admin";
            }
            return View(model);
        }

        // POST: DepartmentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DepartmentVm model)
        {
            Url = "Settings/Department/Save";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}
                model.SchoolCode = SessionData.ClientCode;
                if (model.StatusId == 0) { model.StatusId = 3; }
                if (string.IsNullOrEmpty(model.Code)) { model.Code = "00000"; }
                resp = await request.AddAsync<DepartmentVm>(model, Url);
                if (resp.ResponseCode == 100){TempData["success"] = resp.ResponseMessage;}
                else if (resp.ResponseCode == 101) { TempData["info"] = resp.ResponseMessage; }
                else{TempData["error"] = resp.ResponseMessage;}

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }

       
        // GET: DepartmentsController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            string resp = "";
            Url = $"Settings/Department/Delete/{SessionData.ClientCode}/{id}";
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
        
    }
}
