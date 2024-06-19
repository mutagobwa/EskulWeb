using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class BranchController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public BranchController(IConfiguration configuration , ILoggerErr logger, MyUtilities myUtilities)
        {
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _logger = logger;
            _sd = new SessionDetail();
            _myUtilities = myUtilities;
        }
        // GET: BranchController
        public async Task<ActionResult> Index(Branch model,int id)
        {
            try
            {
                if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}
                if (id!=0)
                {
                    ApiResponse respons = await _myUtilities.LoadBranch(id);
                    if (respons.Success)
                    {
                        model = JsonConvert.DeserializeObject<List<Branch>>(respons.PayLoad);
                    }
                }
                ApiResponse response = await _myUtilities.LoadBranches();
                if (response.Success)
                {
                    model.Branches = JsonConvert.DeserializeObject<List<Branch>>(response.PayLoad);
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
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }

        // GET: BranchController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BranchController/Create
        [HttpPost]
        
        public async Task<ActionResult> Create(Branch model)
        {
            Url = "Settings/School/Branch/Save";
            ApiResponse resp;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                model.SchoolCode = SessionData.ClientCode;
                if (model.StatusId == 0) { model.StatusId = 3; }
                resp = await request.AddAsync<Branch>(model, Url);
                if (resp.Success && resp.ResponseCode == 100 && resp.PayLoad!=null)
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

        // GET: BranchController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}
            return RedirectToAction(nameof(Index),new {id=id});
        }
        public async Task<ActionResult> Delete(int id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            string resp = "";
            Url = $"Settings/School/Branch/Delete/{SessionData.ClientCode}/{id}";
            try
            {
                var myresp = await request.DeleteAsync(Url);
                var data = new { status = 200, res = myresp.ResponseMessage };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }
    }
}
