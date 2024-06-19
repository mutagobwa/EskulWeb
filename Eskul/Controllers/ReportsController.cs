using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartPaperEdms.Web.App_Code;
using System.Linq;

namespace Eskul.Controllers
{
    public class ReportsController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ReportsController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        [HttpGet]
        [Route("Reports/{id}")]
        public async Task<ActionResult> Index(reportfil model, string id)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                //var l = (from p in await _myUtilities.LoadBranches(true) select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();
                //ViewBag.Branches = LoadListItems(l, true);
                if (model.Branch == 0)
                {
                    model.Branch = SessionData.UserBranchId;
                }
                var allowedValues = new[] { "1", "2", "3", "4", "5", "6", "7" };

                if (allowedValues.Contains(id))
                {
                    var type = int.Parse(id);
                    switch (type)
                    {
                        case 1:
                            ViewBag.Rep = "Academic Reports";
                            break;
                        case 2:
                            ViewBag.Rep = "Finance Reports";
                            break;
                        case 3:
                            ViewBag.Rep = "Library Reports";
                            break;
                        case 4:
                            ViewBag.Rep = "Attendance Reports";
                            break;
                        case 5:

                            break;
                        case 6:

                            break;
                        case 7:

                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            



            return View();
        }
        public ActionResult Academic()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            } 
            return View();
        }
        // GET: ReportsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReportsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ReportsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReportsController/Edit/5
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

        // GET: ReportsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReportsController/Delete/5
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
