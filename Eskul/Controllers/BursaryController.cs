using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
//using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Configuration;
using System.Reflection;

namespace Eskul.Controllers
{
    public class BursaryController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public BursaryController( IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            _sd = new SessionDetail();
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(BursaryVm model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.Bursaries = await _myUtilities.LoadBursaries(true);

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);

            }
            return View(model);
        }

        // GET: BursaryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BursaryController/Create
        public async Task<ActionResult> Create(BursaryVm model,int Id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            try
            {
                string resp = "";
                string Url = "AccountsAndFinance/GenerateBursaryCode";
                resp = await request.GetB(Url);
                model.BursaryId = resp.Split('\"')[3];
                return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: BursaryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BursaryVm model)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            Url = "AccountsAndFinance/AddBursary";
            string resp = "";
            try
            {
                var Exists = await _myUtilities.LoadBursary(model);
                if (Exists==null|| Exists.Count==0)
                {
                    Url = "AccountsAndFinance/AddBursary";
                    resp = await request.Add<BursaryVm>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = resp;
                    }
                    
                }
                else
                {
                    string EditUrl = "AccountsAndFinance/UpdateBursary";
                    model.delete = false;
                    resp = await request.Update<BursaryVm>(model, EditUrl);
                    if (resp.Contains("successfully "))
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

        // GET: BursaryController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            string EditUrl = "AccountsAndFinance/Bursary/" + id + "";
            var model = new BursaryVm();
            try
            {
                var c = await request.Get<BursaryList>(EditUrl);

                model.Name = c.FirstOrDefault().BursaryName;
                model.BursaryDesc = c.FirstOrDefault().BursaryDesc;
                model.DiscountRate = c.FirstOrDefault().DiscountRate;
                model.BursaryId = c.FirstOrDefault().BursaryCode;
                model.DiscountType = c.FirstOrDefault().DiscountType;
                model.delete = false;
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), model);
        }

        // POST: BursaryController/Edit/5
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

        // GET: BursaryController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            var json = "";
            string resp = "";
            string UpUrl = "AccountsAndFinance/UpdateBursary";
            string EditUrl = "AccountsAndFinance/Bursary/" + id + "";
            var model = new BursaryVm();
            try
            {
                var c = await request.Get<BursaryList>(EditUrl);

                model.Name = c.FirstOrDefault().BursaryName;
                model.BursaryDesc = c.FirstOrDefault().BursaryDesc;
                model.DiscountRate = c.FirstOrDefault().DiscountRate;
                model.BursaryId = c.FirstOrDefault().BursaryCode;
                
                model.delete = true;
                resp = await request.Update<BursaryVm>(model, UpUrl);
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
                
                //var data = new { status = 201, message = ex };
                //var json = JsonConvert.SerializeObject(data);
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: BursaryController/Delete/5
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
