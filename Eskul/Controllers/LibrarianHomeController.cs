using Eskul.APIClient;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class LibrarianHomeController : Controller
    {
        SessionDetail _sd;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly MyUtilities _myUtilities;
        // GET: LibrarianHomeController
        public LibrarianHomeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, MyUtilities myUtilities)
        {
            _httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index()
        {
            if (!SessionData.IsSignedIn)
            {
                return RedirectToAction("Index", "Login");
            }
            var Books = await _myUtilities.LoadBooks(true);//.Result.Take(7).ToList();
            ViewBag.Books = Books.Count;
            var Members = await _myUtilities.LoadLibraryMembers(true);
            ViewBag.Members = Members.Count;
            return View();
        }

        // GET: LibrarianHomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LibrarianHomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibrarianHomeController/Create
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

        // GET: LibrarianHomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibrarianHomeController/Edit/5
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

        // GET: LibrarianHomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibrarianHomeController/Delete/5
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
