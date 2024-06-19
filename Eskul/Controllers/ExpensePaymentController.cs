using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eskul.Controllers
{
    public class ExpensePaymentController : Controller
    {
        // GET: ExpensePaymentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ExpensePaymentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExpensePaymentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpensePaymentController/Create
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

        // GET: ExpensePaymentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExpensePaymentController/Edit/5
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

        // GET: ExpensePaymentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExpensePaymentController/Delete/5
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
