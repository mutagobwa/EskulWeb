using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;
using System.Runtime.CompilerServices;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class GSCatController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public GSCatController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        // GET: GSCatController
        public async Task<ActionResult> Index(GsCat model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction(nameof(Index), "Login");
                }
                ApiResponse response = await _myUtilities.LoadGSCategories();

                if (response.Success)
                {
                    model.gsCats = JsonConvert.DeserializeObject<List<GsCat>>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
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
        public async Task<ActionResult> Definitions(Definitions model, int id, int id2)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            var resp = await _myUtilities.LoadGSCategories();

            if (resp != null && resp.Success)
            {
                List<GsCat> classLists = JsonConvert.DeserializeObject<List<GsCat>>(resp.PayLoad);
                var l = (from p in classLists select new ListItems { Text = p.categoryName, Value = p.categoryId.ToString() }).ToList();

                ViewBag.GSCategories = LoadListItems(l, true);
            }
            if (id2 != 0)
            {
                ApiResponse response = await _myUtilities.LoadDefinitions(id2);

                if (response.Success)
                {
                    model.definitions = JsonConvert.DeserializeObject<List<Definitions>>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = response.ResponseMessage;
                }
            }
            else
            {
                ApiResponse response = await _myUtilities.LoadDefinitions(id);

                if (response.Success)
                {
                    model.definitions = JsonConvert.DeserializeObject<List<Definitions>>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = response.ResponseMessage;
                }
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Definitions(Definitions model)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            Url = "Academics/GenericSkill/Definition/Save";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }
                model.SchoolCode = SessionData.ClientCode;
                if (model.StatusId == 0) { model.StatusId = 3; }
                if (string.IsNullOrEmpty(model.CategoryName)) { model.CategoryName = ""; }
                resp = await request.AddAsync<Definitions>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    TempData["success"] = resp.ResponseMessage;

                }
                else if ((resp.ResponseCode == 101))
                {
                    TempData["info"] = resp.ResponseMessage;
                }
                else { TempData["error"] = resp.ResponseMessage; }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GsCat model)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            Url = "Academics/GenericSkill/Category/Save";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }
                model.schoolCode = SessionData.ClientCode;
                resp = await request.AddAsync<GsCat>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    TempData["success"] = resp.ResponseMessage;

                }
                else if ((resp.ResponseCode == 101))
                {
                    TempData["info"] = resp.ResponseMessage;
                }
                else { TempData["error"] = resp.ResponseMessage; }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> _Edit(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            var model = new Definitions();
            string resp = "";
            try
            {
                ApiResponse response = await _myUtilities.LoadDefinition(id);

                if (response.Success)
                {
                    model = JsonConvert.DeserializeObject<Definitions>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = response.ResponseMessage;
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            var model = new GsCat();
            string resp = "";
            try
            {
                ApiResponse response = await _myUtilities.LoadGSCat(id);

                if (response.Success)
                {
                    model = JsonConvert.DeserializeObject<GsCat>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["info"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = response.ResponseMessage;
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            string resp = "";
            Url = $"SysSettings/Stream/Delete/{SessionData.ClientCode}/{id}";
            try
            {
                var myresp = await request.DeleteAsync(Url);
                var data = new { status = 200, res = myresp.ResponseMessage };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");
            }


        }

        public async Task<ActionResult> _Delete(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            string resp = "";
            Url = $"SysSettings/Stream/Delete/{SessionData.ClientCode}/{id}";
            try
            {
                var myresp = await request.DeleteAsync(Url);
                var data = new { status = 200, res = myresp.ResponseMessage };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");
            }
        }
    }
}
