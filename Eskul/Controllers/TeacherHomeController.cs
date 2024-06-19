using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eskul.Controllers
{
    public class TeacherHomeController : Controller
    {
        // GET: TeacherHomeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TeacherHomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TeacherHomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeacherHomeController/Create
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

        // GET: TeacherHomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TeacherHomeController/Edit/5
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

        // GET: TeacherHomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TeacherHomeController/Delete/5
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
