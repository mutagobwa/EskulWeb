using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class ProfilesController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        // GET: ProfilesController
        public ProfilesController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(ProfileVm model,string id)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }
                if (!string.IsNullOrEmpty(id))
                {
                    ApiResponse response1 = await _myUtilities.LoadProfile(id);

                    if (response1.Success)
                    {
                        model = JsonConvert.DeserializeObject<ProfileVm>(response1.PayLoad);
                    }
                    else if (response1.ResponseCode == 101)
                    {
                        TempData["info"] = response1.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = response1.ResponseMessage;
                    }
                }
                ApiResponse response = await _myUtilities.LoadProfiles();

                if (response.Success)
                {
                    model.Profiles = JsonConvert.DeserializeObject<List<ProfileList>>(response.PayLoad);
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
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: ProfilesController/Details/5
        public async Task<ActionResult> Rights(string id)
        {
            if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}
            var menus = await _myUtilities.LoadMenus(id, SessionData.ClientCode);
            List<MenuItem> menuCategories = JsonConvert.DeserializeObject<List<MenuItem>>(menus.PayLoad);
            //var menuCategories = JsonConvert.DeserializeObject<Menu>(menus.PayLoad);
            return View("Rights", menuCategories);
        }

        // POST: ProfilesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProfileVm model)
        {
            Url = "Users/Profiles";
            ApiResponse resp;
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }
                model.SchoolCode = SessionData.ClientCode;
                if (model.StatusId == 0) { model.StatusId = 3; }
                if (string.IsNullOrEmpty(model.Code)) { model.Code = "0000"; }
                resp = await request.AddAsync<ProfileVm>(model, Url);
                if (resp.ResponseCode == 100)
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

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ProfilesController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            
            return RedirectToAction(nameof(Index), new {id=id});
        }
        // GET: ProfilesController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            Url = $"Users/Profiles/{SessionData.ClientCode}/{id}";
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

        [HttpPost]
        public async Task<ActionResult> SaveMenuAccess(IFormCollection collection)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            string resp = "";

            var rightsSave = new RightsSave();

            foreach (var key in collection.Keys)
            {
                if (key.StartsWith("Menus["))
                {
                    string[] keyParts = key.Split('.');
                    if (keyParts.Length == 2)
                    {
                        int index = int.Parse(keyParts[0].Substring(6, keyParts[0].Length - 7));
                        bool status = collection[$"Menus[{index}].Status"] == "true";
                        decimal rightId = decimal.Parse(collection[$"Menus[{index}].RightId"]);

                        int convertedRightId = (int)rightId;

                        var menu = new RightsSave
                        {
                            RightId = convertedRightId,
                            Status = status
                        };
                        if (menu.Status)
                        {
                            menu.statusId = 3;
                        }
                        else
                        {
                            menu.statusId = 5;
                        }
                        Url = "Menu/AccessRight/Set/" + menu.RightId + "/" + menu.statusId;
                        resp = await request.Add<RightsSave>(menu, Url);
                    }
                }
            }
            resp = "Menu access saved successfully";

            var response = new
            {
                status = 200,
                res = resp
            };

            var json = JsonConvert.SerializeObject(response);

            return Content(json, "application/json");
        }





    }
}
