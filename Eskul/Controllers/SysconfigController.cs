using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartPaperEdms.Web.App_Code;
using System.Configuration;
using System.Reflection;

namespace Eskul.Controllers
{
    public class SysconfigController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        // GET: SysconfigController
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public SysconfigController( IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(SysConfigVm model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.Sysconfigs = await _myUtilities.LoadSysconfigarations(true);

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: SysconfigController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SysconfigController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysconfigController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SysConfigVm model)
        {
            Url = "Settings/SystemConfigs";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadSysconfigaration(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "Settings/UpdateSystemConfig";
                    model.delete = false;
                    resp = await request.Update<SysConfigVm>(model, EditUrl);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = "Updated Successfully";

                    }
                    else
                    {
                        TempData["error"] = "Error Occured";
                    }
                }
                else
                {
                    Url = "AddSystemParameter";
                    resp = await request.Add<SysConfigVm>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = "Subject Added Successfully";

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

        // GET: SysconfigController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            string EditUrl = "Settings/SystemConfig/" + id + "";
            var model = new SysConfigVm();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<SysConfigVm>(EditUrl);

                model.SysconfigName = c.FirstOrDefault().SysconfigName;
                model.SysconfigValue = c.FirstOrDefault().SysconfigValue;
                model.SysconfigDesc = c.FirstOrDefault().SysconfigDesc;
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

        // POST: SysconfigController/Edit/5
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

        // GET: SysconfigController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SysconfigController/Delete/5
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
