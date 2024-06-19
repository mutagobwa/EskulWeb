using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class LibraryStudentController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public LibraryStudentController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;   
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;

        }
        public async Task<ActionResult> Index(StudentMember model1)
        {
            //object model = null;
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                //var l = (from p in await _myUtilities.LoadClasses(true) select new ListItems { Text = p.Name, Value = p.classcode.ToString() }).ToList();
                //ViewBag.Classes = LoadListItems(l, true);
                //l = (from p in await _myUtilities.LoadStreams(true) select new ListItems { Text = p.Name, Value = p.StreamId.ToString() }).ToList();
                //ViewBag.Streams = LoadListItems(l, true);
                
                if (model1.ClassId > 0)
                {

                    model1.Libstudents = await _myUtilities.LoadLibStudents(model1); 
                }
                else
                {
                    
                    return View(new StudentMember());
                }
                

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model1);
        }
        public async Task<ActionResult> AddLibStaff(string id, StudentMember model)
        {
            string resp = "";
            string EditUrl = "StudentManagement/GetStudent/" + model.StudentId + "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Url = "Library/AddLibraryMemberShip";
                
                var c = await request.Get<Students>(EditUrl);
                model.libraryMember.AdmissionNo = c.FirstOrDefault().Studentid;
                model.libraryMember.MembershipId = c.FirstOrDefault().Studentid;
                model.libraryMember.MemberType = "S";//c.FirstOrDefault().MemberType;
                model.libraryMember.LibraryCardNo = model.libraryMember.LibraryCardNo;// c.FirstOrDefault().MemberType;
                resp = await request.Add<LibraryMember>(model.libraryMember, Url);
                if (resp.Contains("successfully"))
                {
                    TempData["success"] = resp;
                }
                else
                {
                    TempData["error"] = "Error Occured" + " " + resp;
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: LibraryStudentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LibraryStudentController/Create
        
        public async Task<ActionResult> Create(StudentMember model1,int id)
        {
            
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }

                model1.Libstudents = await _myUtilities.LoadLibStudents(model1);

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return RedirectToAction(nameof(Index),model1.Libstudents);
        }

        // POST: LibraryStudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StudentMember model1)
        {
            object model = null;
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model = await _myUtilities.LoadLibStudents(model1);

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return RedirectToAction(nameof(Index), model1);
        }

        // GET: LibraryStudentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibraryStudentController/Edit/5
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

        // GET: LibraryStudentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibraryStudentController/Delete/5
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
