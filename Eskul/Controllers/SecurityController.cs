using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class SecurityController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public SecurityController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        // GET: SecurityController
        public async Task<ActionResult> Index(SecuritySettings model,string id)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                if (!string.IsNullOrEmpty(id))
                {
                  
                }
                ApiResponse resp= await _myUtilities.LoadSecparameters();
                model.securityparamSettings = JsonConvert.DeserializeObject<SecuritySettings>(resp.PayLoad);

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }
        // POST: SecurityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(securitypram model)
        {
            Url = "Settings/SecurityParameters";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadSecparameter(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "Settings/UpdateSecurityParameter";
                    var Model = new securitypramEdit();
                    Model.name = Request.Form["parametername"];//Exists.FirstOrDefault().parametername;
                    Model.value = Request.Form["parametervalue"];
                    Model.description = Request.Form["parameterdesc"];
                    Model.id = Exists.FirstOrDefault().parameterid;
                    resp = await request.Update<securitypramEdit>(Model, EditUrl);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured"+resp;
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

        // GET: SecurityController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            return RedirectToAction(nameof(Index), new {id=id});
        }
    }
}
