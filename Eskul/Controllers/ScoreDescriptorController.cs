using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class ScoreDescriptorController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ScoreDescriptorController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(ScoreDescriptor model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                ApiResponse response = await _myUtilities.LoadScoreDescriptorsAsync();
                model.scoreDescriptors = JsonConvert.DeserializeObject<List<ScoreDescriptor>>(response.PayLoad);
                var l = (from p in await _myUtilities.LoadClasses(model.Level ?? "O") select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();
                ViewBag.Classes = LoadListItems(l, true);

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: ScoreDescriptorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ScoreDescriptorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScoreDescriptorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ScoreDescriptor model)
        {
            Url = "Examination/AddScoreDescriptor";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadScoreDescriptor(model);
                if (Exists.Count > 0)
                {
                    string EditUrl = "Examination/UpdateScoreDescriptor";
                    model.Identifier = model.scoreCode;
                    model.SchoolCode = SessionData.ClientCode;
                    resp = await request.Update<ScoreDescriptor>(model, EditUrl);
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
                    Url = "Examination/AddScoreDescriptor";
                    resp = await request.Add<ScoreDescriptor>(model, Url);
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

        // GET: ScoreDescriptorController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            string resp = "";
            string EditUrl = $"Examination/ScoreDescriptor/GetById/{SessionData.ClientCode}/{id}";
            var model = new ScoreDescriptor();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<ScoreDescriptor>(EditUrl);
                model.Class = c.FirstOrDefault().ClassCode;
                model.scoreCode = c.FirstOrDefault().Identifier;
                model.scorePoints = c.FirstOrDefault().scorePoints;
                model.lowerScore = c.FirstOrDefault().lowerScore;
                model.upperScore = c.FirstOrDefault().upperScore;
                model.scoreRank = c.FirstOrDefault().scoreRank;
                model.descriptorId = c.FirstOrDefault().descriptorId;
                model.comment = c.FirstOrDefault().comment;
                model.scoreDescriptor = c.FirstOrDefault().scoreDescriptor;
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

        // GET: ScoreDescriptorController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var json = "";
            string resp = "";
            string UpdateUrl = "Examination/UpdateScoreDescriptor";
            string EditUrl = $"Examination/ScoreDescriptor/GetById/{SessionData.ClientCode}/{id}";
            var model = new ScoreDescriptor();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<ScoreDescriptor>(EditUrl);
                model.Class = c.FirstOrDefault().ClassCode;
                model.scoreCode = c.FirstOrDefault().Identifier;
                model.scorePoints = c.FirstOrDefault().scorePoints;
                model.lowerScore = c.FirstOrDefault().lowerScore;
                model.upperScore = c.FirstOrDefault().upperScore;
                model.scoreRank = c.FirstOrDefault().scoreRank;
                model.comment = c.FirstOrDefault().comment;
                model.scoreDescriptor = c.FirstOrDefault().scoreDescriptor;
                model.delete = true;
                model.SchoolCode = SessionData.ClientCode;
                resp = await request.Update<ScoreDescriptor>(model, UpdateUrl);
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
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");

            }
        }
    }
}
