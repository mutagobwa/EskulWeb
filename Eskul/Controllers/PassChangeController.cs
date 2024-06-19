using ASEEncryptorDecryptorTool;
using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartPaperEdms.Web.App_Code;
using System.Web;

namespace Eskul.Controllers
{
    public class PassChangeController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public PassChangeController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        // GET: PassChangeController
        public async Task<ActionResult> Index(ChangePass model)
        {
            ApiResponse resp;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                if (!string.IsNullOrEmpty(model.RawPassword))
                {
                    //var rawpass = HttpUtility.UrlEncode(model.RawPassword);
                    resp= await _myUtilities.ChangePassword(model.RawPassword);
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
                }
                else
                {
                    model = new ChangePass();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View();
        }
    }
}
