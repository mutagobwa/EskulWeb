using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eskul.Controllers
{
    public class TestController : Controller
    {
        // GET: TestController
        public ActionResult Index(_student model)
        
        {
            List<_student> students = new List<_student>
            {
                new _student { Name = "Nabulobi Alice", Scores = new Dictionary<string, int> { { "mtc", 80 }, { "sci", 75 }, { "eng", 90 }, { "CRE", 90 }, { "Phy", 90 }, { "Bio", 90 }, { "Chem", 90 }, { "Agr", 90 }, { "Art", 90 }, { "His", 90 }, { "Ent", 90 }, { "Sub", 90 }, { "Comm", 90 }, { "Comp", 90 }, { "Lit", 90 }, { "Isl", 90 } } },
                new _student { Name = "Kazungu Bob", Scores = new Dictionary<string, int> { { "mtc", 90 }, { "sci", 85 }, { "eng", 80 }, { "CRE", 90 }, { "Phy", 90 }, { "Bio", 90 }, { "Chem", 90 }, { "Agr", 90 }, { "Art", 90 }, { "His", 90 }, { "Ent", 90 }, { "Sub", 90 }, { "Comm", 90 }, { "Comp", 90 }, { "Lit", 90 }, { "Isl", 90 } } },
                new _student { Name = "Dhabangi Charlie", Scores = new Dictionary<string, int> { { "mtc", 70 }, { "sci", 80 }, { "eng", 75 }, { "CRE", 90 }, { "Phy", 90 }, { "Bio", 90 }, { "Chem", 90 }, { "Agr", 90 }, { "Art", 90 }, { "His", 90 }, { "Ent", 90 }, { "Sub", 90 }, { "Comm", 90 }, { "Comp", 90 }, { "Lit", 90 }, { "Isl", 90 } } },
                new _student { Name = "Muhumuza David", Scores = new Dictionary<string, int> { { "mtc", 60 }, { "sci", 70 }, { "eng", 80 }, { "CRE", 90 }, { "Phy", 90 }, { "Bio", 90 }, { "Chem", 90 }, { "Agr", 90 }, { "Art", 90 }, { "His", 90 }, { "Ent", 90 }, { "Sub", 90 }, { "Comm", 90 }, { "Comp", 90 }, { "Lit", 90 }, { "Isl", 90 } } },
                new _student { Name = "Nambilo Eve", Scores = new Dictionary<string, int> { { "mtc", 75 }, { "sci", 90 }, { "eng", 85 }, { "CRE", 90 }, { "Phy", 90 }, { "Bio", 90 }, { "Chem", 90 }, { "Agr", 90 }, { "Art", 90 }, { "His", 90 }, { "Ent", 90 }, { "Sub", 90 }, { "Comm", 90 }, { "Comp", 90 }, { "Lit", 90 }, { "Isl", 90 } } }
            };

            var subjects = students.SelectMany(s => s.Scores.Keys).Distinct().ToList();
            var scores = students.Select(s => subjects.Select(subject => s.Scores[subject]).ToList()).ToList();

            ViewBag.Subjects = subjects;
            ViewBag.Scores = scores;
            ViewBag.StudentNames = students.Select(s => s.Name).ToList();
            model._students= students;
            //model._subjects = subjects.ToList();
            return View(model);
        }

        // GET: TestController/Details/5
        public ActionResult Subass()
        {
            var classes = new List<string> { "Class 1", "Class 2", "Class 3" };
            var streams = new List<string> { "Stream 1", "Stream 2", "Stream 3" };
            var students = new List<string> { "Student 1", "Student 2", "Student 3" };

            // Simulate data for the table
            var subjects = new List<_Subject>
{
    new _Subject { Name = "Subject 1", Papers = 3, PaperStatus = 2 },
    new _Subject { Name = "Subject 2", Papers = 2, PaperStatus = 3 },
    new _Subject { Name = "Subject 3", Papers = 4, PaperStatus = 1 },
    new _Subject { Name = "Subject 4", Papers = 1, PaperStatus = 1 },
    new _Subject { Name = "Subject 5", Papers = 2, PaperStatus = 2 },
    new _Subject { Name = "Subject 6", Papers = 3, PaperStatus = 3 },
    new _Subject { Name = "Subject 7", Papers = 2, PaperStatus = 1 },
    new _Subject { Name = "Subject 8", Papers = 4, PaperStatus = 2 },
    new _Subject { Name = "Subject 9", Papers = 1, PaperStatus = 3 },
    new _Subject { Name = "Subject 10", Papers = 3, PaperStatus = 1 }
};

            var model = new FormModel
            {
                Classes = classes,
                Streams = streams,
                Students = students,
                Subjects = subjects
            };

            return View(model);
        
    }

        // GET: TestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestController/Create
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

        // GET: TestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestController/Edit/5
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

        // GET: TestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestController/Delete/5
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
