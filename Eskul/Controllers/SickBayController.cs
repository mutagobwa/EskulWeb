using Eskul.APIClient;
using Eskul.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class SickBayController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public SickBayController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        // GET: SickBayController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SickBayController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SickBayController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SickBayController/Create
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

        // GET: SickBayController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SickBayController/Edit/5
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

        // GET: SickBayController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SickBayController/Delete/5
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
        public async Task<ActionResult> DashBoard()
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
