using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class FeesGroupController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public FeesGroupController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger= logger; 
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;    
        }
        public async Task<ActionResult> Index(FeesGroupViewModel model)
        {
            try
            {
                if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}

                ApiResponse response = await _myUtilities.LoadFeesGroups(true);
                
                if(response.Success)
                {
                    model.feesGroups = JsonConvert.DeserializeObject<List<FeesGroup>>(response.PayLoad);
                }
                else if(response.ResponseCode == 101) 
                {
                    TempData["error"] = response.ResponseMessage;
                }
                else if(response.ResponseCode == 500)
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
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: FeesGroupController/Create
        public ActionResult Create(FeesGroup model,int id)
        {
            return View();
        }

        // POST: FeesGroupController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FeesGroup model)
        {
            Url = "FeesCollection/FeesGroup/Save";
            ApiResponse resp =null;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }

                model.SchoolCode = SessionData.ClientCode;
                if (string.IsNullOrEmpty(model.GroupCode)){model.GroupCode = "00000";}
                if (model.StatusId==0){ model.StatusId =3;}
                resp = await request.AddAsync<FeesGroup>(model, Url);
                    if (resp.ResponseCode==100)
                    {
                        TempData["success"] = resp.ResponseMessage;

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

        // GET: FeesGroupController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string resp = "";
            var model = new FeesGroup();
            try
            {
                if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}
                ApiResponse response = await _myUtilities.LoadFeesGroup(id);

                if (response.Success)
                {
                    model= JsonConvert.DeserializeObject<FeesGroup>(response.PayLoad);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Create), model);
        }
        public async Task<ActionResult> Delete(string id)
        {
            if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}
            string resp = "";
            Url = $"FeesCollection/FeesGroup/Delete/{SessionData.ClientCode}/{id}";
            try
            {
                var myresp = await request.DeleteAsync(Url);
                var data = new { status = 200, res = myresp.ResponseMessage};
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
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
