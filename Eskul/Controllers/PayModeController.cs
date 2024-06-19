using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class PayModeController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public PayModeController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(PaymentMode model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.paymentModes = await _myUtilities.LoadPayModes(true);
                

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: PayModeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PayModeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PayModeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PaymentMode model)
        {
            Url = "AccountsAndFinance/AddPaymentMode";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadPayMode(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "AccountsAndFinance/UpdatePaymentMode";
                    resp = await request.Update<PaymentMode>(model, EditUrl);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured" + " " + resp;
                    }
                }
                else
                {
                    Url = "AccountsAndFinance/AddPaymentMode";
                    resp = await request.Add<PaymentMode>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

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

        // GET: PayModeController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string resp = "";
            string EditUrl = "AccountsAndFinance/PaymentMode/" + id + "";
            var model = new PaymentMode();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<PaymentMode>(EditUrl);
                model.ModeCode = c.FirstOrDefault().ModeCode;
                model.ModeName = c.FirstOrDefault().ModeName;
                model.ModeDesc = c.FirstOrDefault().ModeDesc;
                model.delete = false;
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), model);
        }

        // POST: PayModeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PayModeController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            string resp = "";
            string UpdateUrl = "AccountsAndFinance/UpdatePaymentMode";
            string EditUrl = "AccountsAndFinance/PaymentMode/" + id + "";
            var model = new PaymentMode();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<PaymentMode>(EditUrl);
                model.ModeCode = c.FirstOrDefault().ModeCode;
                model.ModeName = c.FirstOrDefault().ModeName;
                model.ModeDesc = c.FirstOrDefault().ModeDesc;
                model.delete = true;
                resp = await request.Update<PaymentMode>(model, UpdateUrl);
                var data = new { status = 200, res = resp };
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

        // POST: PayModeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
    }
}
