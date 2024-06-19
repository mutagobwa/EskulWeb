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
    public class GLAccountController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public GLAccountController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(GLAccount model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.gLAccounts = await _myUtilities.LoadGLAccounts(true);
                //var l = (from p in await _myUtilities.LoadBranches(true) select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();
                //ViewBag.Branches = LoadListItems(l, true);
                


            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: GLAccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GLAccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GLAccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GLAccount model)
        {
            Url = "AccountsAndFinance/AddGLAccountSetting";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadGLAccount(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "AccountsAndFinance/UpdateGLAccountSetting";
                    resp = await request.Update<GLAccount>(model, EditUrl);
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
                    Url = "AccountsAndFinance/AddGLAccountSetting";
                    resp = await request.Add<GLAccount>(model, Url);
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

        // GET: GLAccountController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            string resp = "";
            string EditUrl = "AccountsAndFinance/GLAccountSetting/" + id + "";
            var model = new GLAccount();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<GLAccount>(EditUrl);
                model.BranchId = c.FirstOrDefault().BranchId;
                model.GlAccount = c.FirstOrDefault().GlAccount;
                model.GlAccountName = c.FirstOrDefault().GlAccountName;
                model.SettingId = c.FirstOrDefault().SettingId;
                
                model.delete = false;
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index),model);
        }

        // POST: GLAccountController/Edit/5
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

        // GET: GLAccountController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            string resp = "";
            string UpdateUrl = "AccountsAndFinance/UpdateGLAccountSetting";
            string EditUrl = "AccountsAndFinance/GLAccountSetting/" + id + "";
            var model = new GLAccount();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<GLAccount>(EditUrl);
                model.BranchId = c.FirstOrDefault().BranchId;
                model.GlAccount = c.FirstOrDefault().GlAccount;
                model.GlAccountName = c.FirstOrDefault().GlAccountName;
                model.SettingId = c.FirstOrDefault().SettingId;
                model.delete = true;
                resp = await request.Update<GLAccount>(model, UpdateUrl);
                var data = new { status = 200, res = resp };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: GLAccountController/Delete/5
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
