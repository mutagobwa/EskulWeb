using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Net.WebSockets;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class BehaviorIncidentController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public BehaviorIncidentController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index(BehaviorIncident model,int id)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                if (id != 0)
                {
                    ApiResponse respons = await _myUtilities.LoadBehaviorIncidentAsync(id);
                    if (respons.Success)
                    {
                        model = JsonConvert.DeserializeObject<BehaviorIncident>(respons.PayLoad);
                    }
                }

                ApiResponse response = await _myUtilities.LoadBehaviorIncidentsAsync();
                if (response.Success)
                {
                    model.behaviorIncidents = JsonConvert.DeserializeObject<List<BehaviorIncident>>(response.PayLoad);
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
        // GET: BehaviorIncidentController/Create
        public async Task<ActionResult> Create(ReportedIncident model)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }
            var l = (from p in await _myUtilities.LoadClasses(model.Level ?? "O") select new ListItems { Value = p.classcode.ToString(), Text = p.Name }).ToList();
            ViewBag.Classes = LoadListItems(l, true);
            //l = (from p in await _myUtilities.LoadStreams(model.Class) select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();
            //ViewBag.Streams = LoadListItems(l, true);
            return View();
        }

        // POST: BehaviorIncidentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BehaviorIncident model)
        {
            Url = "Behaviours/Incidents";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                model.SchoolCode = SessionData.ClientCode;
                //if (model.StatusId == 0) { model.StatusId = 3; }
                ////if (string.IsNullOrEmpty(model.Code)) { model.Code = "00000"; }
                resp = await request.AddAsync<BehaviorIncident>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    TempData["success"] = resp.ResponseMessage;

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

        // GET: BehaviorIncidentController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            return RedirectToAction(nameof(Index), new { id = id });
        }
        // GET: BehaviorIncidentController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction(nameof(Index), "Login"); }
            string json = "";
            string UpdateUrl = "BehaviourManagement/BehaviorIncident/Save";
            var model = new BehaviorIncident();
            try
            {
                model = await _myUtilities.LoadBehaviorIncident(id);
                model.SchoolCode = SessionData.ClientCode;
                model.StatusId = 5;
                var resp = await request.Add<BehaviorIncident>(model, UpdateUrl);
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
                TempData["error"] = "Error Occured Please Contact Admin";
                _logger.Error(ex.Message, ex);
                return Content(json, "application/json");
            }
        
    }
    }
}
