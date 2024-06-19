using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Security;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class ScoreRankController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ScoreRankController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _logger = logger;
            _sd = new SessionDetail();
            _myUtilities = myUtilities;
        }
        // GET: ScoreRankController
        public async Task<ActionResult> Index(ScoreRank model)
        
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }

                var l = (from p in await _myUtilities.LoadClasses(model.Level ?? "O") select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();
                ViewBag.Classes = LoadListItems(l, true);
                if (model.ClassCode != 0)
                {
                    ApiResponse response = await _myUtilities.LoadScoreRanksAsync(model);
                    if (response.Success)
                    {
                        model.ScoreRanks = JsonConvert.DeserializeObject<List<ScoreRank>>(response.PayLoad);
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
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured Contact Admin" ;
                _logger.Error(ex.Message, ex);
            }
            return View(model);
        }
        public async Task<ActionResult> Delete(decimal id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            string resp = "";
            Url = $"FeesCollection/FeesGroup/Delete/{SessionData.ClientCode}/{id}";
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

        // GET: ScoreRankController/Details/5
        public async Task<ActionResult> Details(decimal id, ScoreRank model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    return RedirectToAction("Index", "Login");
                }

                ApiResponse response = await _myUtilities.LoadScoreRankByIdAsync(id);
                if (response.Success)
                {
                    model = JsonConvert.DeserializeObject<ScoreRank>(response.PayLoad);
                    if (model?.ScoreRanks != null && model.ScoreRanks.Count() > 1)
                    {
                        model.Comments = model.ScoreRanks.FirstOrDefault()?.Comments;
                    }
                    else
                    {
                        model.Comments = new List<CommentMaster> { new CommentMaster() };
                        string resp = "This Particular ScoreRank has no comments";
                        TempData["success"] = resp;
                        return View(model);
                    }
                    return View(model);
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
                    TempData["error"] = "Response Unknown";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occurred Contact Admin";
                _logger.Error(ex.Message, ex);
            }

            return View();
        }


        // GET: ScoreRankController/Create
        public ActionResult Create(CommentMaster model)
        {
            return RedirectToAction();
        }

        // POST: ScoreRankController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ScoreRank model)
        {
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                Url = "Examination/ScoreRank/Save";
                resp = await request.Add<ScoreRank>(model, Url);
                if (resp.Contains("successfully"))
                {
                    TempData["success"] = resp;

                }
                else
                {
                    TempData["error"] = "Error Occured" + " " + resp;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured" + "" + resp;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ScoreRankController/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            string EditUrl = "Examination/ScoreRank/SearchById/" + id + "";
            var model = new ScoreRank();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<ScoreRank>(EditUrl);
                model.ScoreRankId = c.FirstOrDefault().ScoreRankId;
                model.ClassCode = c.FirstOrDefault().ClassCode;
                model.LowerLimit = c.FirstOrDefault().LowerLimit;
                model.UpperLimit = c.FirstOrDefault().UpperLimit;
                model.RankCode = c.FirstOrDefault().RankCode;
                model.Points = c.FirstOrDefault().Points;
                model.StatusId = c.FirstOrDefault().StatusId;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), model);
        }

        // POST: ScoreRankController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ScoreComment(CommentMaster model,decimal ScoreRankId)
        {
            ApiResponse resp;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                Url = "Examinations/Comment/Save";
                model.ScoreRankId = (int)ScoreRankId;
                resp = await request.AddAsync<CommentMaster>(model, Url);
                if (resp.Success && resp.ResponseCode == 100 && resp.PayLoad != null)
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
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ScoreRankController/Delete/5
        public async Task<ActionResult> EditComment(CommentMaster model)
        {

            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                Url = "Examination/Comment/Save";
                resp = await request.Add<CommentMaster>(model, Url);
                if (resp.Contains("successfully"))
                {
                    TempData["success"] = resp;

                }
                else
                {
                    TempData["error"] = "Error Occured" + " " + resp;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured" + "" + resp;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ScoreRankController/Delete/5
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
        public async Task<ActionResult> ClassByLevel(string Level)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";

                object model = "";

                if (Level != null)
                {

                    model = await _myUtilities.LoadClasses(Level);

                    var data = new { status = 200, res = resp };
                    var json = JsonConvert.SerializeObject(model);
                    return Content(json, "application/json");
                }

                else
                {
                    return Content(JsonConvert.SerializeObject(new { status = 404, res = "Error" }), "application/json");
                }
            }
            catch (Exception ex)
            {

                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");

            }

        }
        public async Task<ActionResult> SubjectByLevel(string Level)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";

                object model = "";

                if (Level != null)
                {
                    model = await _myUtilities.LoadSubjects(Level);
                    var data = new { status = 200, res = resp };
                    var json = JsonConvert.SerializeObject(model);
                    return Content(json, "application/json");
                }

                else
                {
                    return Content(JsonConvert.SerializeObject(new { status = 404, res = "Error" }), "application/json");
                }
            }
            catch (Exception ex)
            {

                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");

            }

        }
    }
}
