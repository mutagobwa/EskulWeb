using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using SmartPaperEdms.Web.App_Code;
using System.Configuration;
using System.Reflection;

namespace Eskul.Controllers
{
    public class MarksController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public MarksController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        // GET: MarksController
        public async Task<ActionResult> Index(MarkVm model1)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                //var l = (from p in await _myUtilities.LoadSubjects(model1.Level??"O") select new ListItems { Text = p.Subject_name, Value = p.Subject_code.ToString() }).ToList();
                //ViewBag.Subjects = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadSubjectPapers(model1.StudentSubject) select new ListItems { Text = p.Paper_Name, Value = p.Paper_Code.ToString() }).ToList();
                //ViewBag.SubPaper = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadClasses(true) select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();
                //ViewBag.Classs = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadStreams(true) select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();
                //ViewBag.Streams = LoadListItems(l, true);
                var l = (from p in await _myUtilities.LoadStudents(model1.StudentClass, model1.Stream ?? "0", SessionData.UserBranchId) select new ListItems { Text = p.Firstname, Value = p.Studentid.ToString() }).ToList();
                ViewBag.Students = LoadListItems(l, true);
    
                if (model1.StudentClass>0)
                {

                    model1.StudentList = (await LoadStudentList(true)).Where(p => p.Class == model1.StudentClass && p.Stream == model1.Stream).ToList();
                }
                else
                {

                    return View(new MarkVm());
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model1);
        }

        // GET: MarksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MarksController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MarksController/Create
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

        // GET: MarksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MarksController/Edit/5
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

        // GET: MarksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MarksController/Delete/5
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
        private async Task<List<Students>> LoadStudentList(bool fromDb)
        {
            Url = "StudentManagement/Students";

            if (fromDb)
                return await request.GetAll<Students>(Url);

            return await request.GetAll<Students>(Url);
        }
        
    }
}
