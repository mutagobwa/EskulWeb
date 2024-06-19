using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class StreamController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public StreamController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        // GET: StreamController
        public async Task<ActionResult> Index(stream model,string id)
         {

            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                var resp = await _myUtilities.LoadClassess();

                if (resp != null && resp.Success)
                {
                    List<ClassList> classLists = JsonConvert.DeserializeObject<List<ClassList>>(resp.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();

                    ViewBag.Classes = LoadListItems(l, true);
                }

                if (!string.IsNullOrEmpty(id))
                {
                    ApiResponse response1 = await _myUtilities.LoadStream(id);

                    if (response1.ResponseCode==100)
                    {
                        model = JsonConvert.DeserializeObject<streamList>(response1.PayLoad);
                    }
                    else if(response1.ResponseCode == 101) { TempData["info"] = response1.ResponseMessage; }
                    else { TempData["error"] = response1.ResponseMessage; }
                }

                ApiResponse response = await _myUtilities.LoadStreamList();

                if (response.Success)
                {
                    model.streams = JsonConvert.DeserializeObject<List<streamList>>(response.PayLoad);
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
        // POST: StreamController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(stream model)
        {
            Url = "Settings/Stream/Save";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }
                model.SchoolCode = SessionData.ClientCode;
                if (model.StatusId == 0) { model.StatusId = 3; }
                if (string.IsNullOrEmpty(model.streamId)) { model.streamId ="000"; }
                resp = await request.AddAsync<stream>(model, Url);
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

        // GET: StreamController/Edit/5
        public async Task<ActionResult> Edit(string id)
        { 
            return RedirectToAction(nameof(Index),new {id=id});
        }
        // GET: StreamController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            string resp = "";
            Url = $"Settings/Stream/Delete/{SessionData.ClientCode}/{id}";
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
