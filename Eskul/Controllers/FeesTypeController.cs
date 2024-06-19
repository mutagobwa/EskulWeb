using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
//using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class FeesTypeController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;

        public FeesTypeController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(FeesType model)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }

                ApiResponse response = await _myUtilities.LoadFeeTypes(true);

                if (response.Success)
                {
                    model.Feestypes = JsonConvert.DeserializeObject<List<FeesType>>(response.PayLoad);
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
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }
        // GET: FeesTypeController/Create
        public async Task<ActionResult> Create(FeesType model,int Id)
        {
            try
            {
                var response = await _myUtilities.LoadFeesGroups(true);

                if (response != null && response.Success)
                {
                    IEnumerable<dynamic> feeGroups = JsonConvert.DeserializeObject<List<FeesGroup>>(response.PayLoad);
                    var feeGroupsListItems = feeGroups.Select(p => new ListItems { Text = p.GroupName, Value = p.GroupCode.ToString() }).ToList();

                    ViewBag.FeesGroups = LoadListItems(feeGroupsListItems, true);
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: FeesTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FeesType model)
        {
            Url = "AccountsAndFinance/AddFeesType";
            string resp = "";
            try
            {
                    Url = "AccountsAndFinance/AddFeesType";
                    resp = await request.Add<FeesType>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

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

        // GET: FeesTypeController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string resp = "";
            string EditUrl = "AccountsAndFinance/FeesType/" + id + "";
            var model = new FeesType();
            try
            {
                var c = await request.Get<FeesType>(EditUrl);
                model.GroupCode = c.FirstOrDefault().GroupCode;
                model.TypeCode = c.FirstOrDefault().TypeCode;
                model.TypeName = c.FirstOrDefault().TypeName;
                model.TypeDescription = c.FirstOrDefault().TypeDescription;
                model.GlAccount = c.FirstOrDefault().GlAccount;
                model.Statusid = c.FirstOrDefault().Statusid;
                model.ApplyBursary = c.FirstOrDefault().ApplyBursary;
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

        // POST: FeesTypeController/Edit/5
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

        // GET: FeesTypeController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            string resp = "";
            string UpdateUrl = "AccountsAndFinance/UpdateFeesType";
            string EditUrl = "AccountsAndFinance/FeesType/" + id + "";
            var model = new FeesType();
            try
            {
                var c = await request.Get<FeesType>(EditUrl);
                model.GroupCode = c.FirstOrDefault().GroupCode;
                model.TypeCode = c.FirstOrDefault().TypeCode;
                model.TypeName = c.FirstOrDefault().TypeName;
                model.TypeDescription = c.FirstOrDefault().TypeDescription;
                model.GlAccount = c.FirstOrDefault().GlAccount;
                model.Statusid = c.FirstOrDefault().Statusid;
                model.ApplyBursary = c.FirstOrDefault().ApplyBursary;
                model.delete = true;
                resp = await request.Update<FeesType>(model, UpdateUrl);
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

        // POST: FeesTypeController/Delete/5
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
