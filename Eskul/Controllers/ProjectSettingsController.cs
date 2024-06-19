using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eskul.Controllers
{
    public class ProjectSettingsController : Controller
    {
        // GET: ProjectSettingsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProjectSettingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectSettingsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectSettingsController/Create
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

        // GET: ProjectSettingsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectSettingsController/Edit/5
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

        // GET: ProjectSettingsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectSettingsController/Delete/5
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
