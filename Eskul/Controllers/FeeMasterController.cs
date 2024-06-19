using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class FeeMasterController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        public FeeMasterController(IConfiguration configuration, ILoggerErr logger)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
        }
        public async Task<ActionResult> Index(Feemaster model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.Feemasters = await LoadFeemasters(true);
                var l = (from p in await LoadClasses(true) select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();
                ViewBag.Classes = LoadListItems(l, true);
                l = (from p in await LoadBranches(true) select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();
                ViewBag.Branches = LoadListItems(l, true);
                l = (from p in await LoadFeeTypes(true) select new ListItems { Text = p.TypeCode, Value = p.TypeCode.ToString() }).ToList();
                ViewBag.FeeTypes = LoadListItems(l, true);
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: FeeMasterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FeeMasterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeeMasterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Feemaster model)
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            Url = "AccountsAndFinance/AddFeesMaster";
            string resp = "";
            try
            {
                var Exists = await LoadFeemaster(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "AccountsAndFinance/UpdateFeesMaster";
                    resp = await request.Update<Feemaster>(model, EditUrl);
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
                    Url = "AccountsAndFinance/AddFeesMaster";
                    resp = await request.Add<Feemaster>(model, Url);
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

        // GET: FeeMasterController/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            string resp = "";
            string EditUrl = "AccountsAndFinance/FeesMaster/" + id + "";
            var model = new Feemaster();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<Feemaster>(EditUrl);
                model.FeeMasterId = c.FirstOrDefault().FeeMasterId;
                model.BranchId = c.FirstOrDefault().BranchId;
                model.ClassCode = c.FirstOrDefault().ClassCode;
                model.TypeCode = c.FirstOrDefault().TypeCode;
                model.Amount = c.FirstOrDefault().Amount;
                model.FineType = c.FirstOrDefault().FineType;
                model.Percentage = c.FirstOrDefault().Percentage;
                model.FineAmount = c.FirstOrDefault().FineAmount;
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

        // POST: FeeMasterController/Edit/5
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

        // GET: FeeMasterController/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            string resp = "";
            string UpdateUrl = "AccountsAndFinance/UpdateFeesMaster";
            string EditUrl = "AccountsAndFinance/FeesMaster/" + id + "";
            var model = new Feemaster();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<Feemaster>(EditUrl);
                model.FeeMasterId = c.FirstOrDefault().FeeMasterId;
                model.BranchId = c.FirstOrDefault().BranchId;
                model.ClassCode = c.FirstOrDefault().ClassCode;
                model.TypeCode = c.FirstOrDefault().TypeCode;
                model.Amount = c.FirstOrDefault().Amount;
                model.FineType = c.FirstOrDefault().FineType;
                model.Percentage = c.FirstOrDefault().Percentage;
                model.FineAmount = c.FirstOrDefault().FineAmount;
                model.delete = true;
                resp = await request.Update<Feemaster>(model, UpdateUrl);
                if (resp.Contains("successfully"))
                {
                    TempData["success"] = resp;

                }
                else
                {
                    TempData["error"] = "Error Occured" + resp;
                }
            }
            catch (Exception ex)
            {


                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: FeeMasterController/Delete/5
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
        private async Task<List<ClassList>> LoadClasses(bool fromDb)
        {
            Url = "Settings/Class/GetAll";
            if (fromDb)
                return await request.GetAll<ClassList>(Url);
            return await request.GetAll<ClassList>(Url);
        }
        private async Task<List<Branch>> LoadBranches(bool fromDb)
        {
            Url = "Settings/Branches";

            if (fromDb)
                return await request.GetAll<Branch>(Url);

            return await request.GetAll<Branch>(Url);
        }
        private async Task<List<FeesType>> LoadFeeTypes(bool fromDb)
        {
            Url = "AccountsAndFinance/FeesType";

            if (fromDb)
                return await request.GetAll<FeesType>(Url);

            return await request.GetAll<FeesType>(Url);
        }
        private async Task<List<Feemaster>> LoadFeemasters(bool fromDb)
        {
            Url = "AccountsAndFinance/FeesMaster";

            if (fromDb)
                return await request.GetAll<Feemaster>(Url);

            return await request.GetAll<Feemaster>(Url);
        }
        private async Task<List<Feemaster>> LoadFeemaster(Feemaster model)
        {

            Url = "AccountsAndFinance/FeesMaster/" + model.FeeMasterId;
            return await request.Get<Feemaster>(Url);
        }
    }
}
