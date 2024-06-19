using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class LibraryBooksController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public LibraryBooksController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index()
        {
            object model = null;
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model = await _myUtilities.LoadBooks(true);


            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: LibraryBooksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public async Task<ActionResult> AllLibMembers()
        {
            object model = null;
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model = await _myUtilities.LoadLibraryMembers(true);

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View("AllLibMembers", model);
        }
        // GET: LibraryBooksController/Create
        public async Task<ActionResult> Create(Book model)
        {
            //var l = (from p in await _myUtilities.LoadSubjects(model.Level ?? "O") select new ListItems { Text = p.Subject_name, Value = p.Subject_code.ToString() }).ToList();
            //ViewBag.Subjects = LoadListItems(l, true);

            if (model.BookId !=0)
            {
                
                return View(model);
            }
            return View();
        }

        // POST: LibraryBooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Book model, decimal id)
        {
            Url = "Library/AddLibraryBook";
            string resp = "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Exists = await _myUtilities.LoadBook(model);
                if (Exists == null || Exists.Count == 0)
                {
                    Url = "Library/AddLibraryBook";
                    //model.UserSession = "uhdeeuud3";
                    resp = await request.Add<Book>(model, Url);
                    if (resp.Contains("successfully"))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = resp;
                    }

                }
                else
                {
                    string EditUrl = "Library/UpdateLibraryBook";

                    resp = await request.Update<Book>(model, EditUrl);
                    if (resp.Contains("successfully "))
                    {
                        TempData["success"] = resp;

                    }
                    else
                    {
                        TempData["error"] = "Error Occured" + resp;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: LibraryBooksController/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            string resp = "";
            string EditUrl = "Library/LibraryBook/" + id + "";
            var model = new Book();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var c = await request.Get<Book>(EditUrl);

                model.BookTitle = c.FirstOrDefault().BookTittle;
                model.BookNumber = c.FirstOrDefault().BookNumber;
                model.IsbnNumber = c.FirstOrDefault().IsbnNumber;
                model.Author = c.FirstOrDefault().Author;
                model.Publisher = c.FirstOrDefault().Publisher;
                model.Subject = c.FirstOrDefault().Subject;
                model.Qty = c.FirstOrDefault().Qty;
                model.Price = c.FirstOrDefault().Price;
                model.BookDescription = c.FirstOrDefault().BookDescription;
                model.RackNumber = c.FirstOrDefault().RackNumber;
                model.Available = c.FirstOrDefault().Available;
                model.BookId = c.FirstOrDefault().BookId;
                model.delete = false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Create", model);
        }

        // POST: LibraryBooksController/Edit/5
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

        // GET: LibraryBooksController/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            string EditUrl = "Library/UpdateLibraryBook";
            string Getsub = "Library/LibraryBook/" + id + "";
            var model = new Book();
            //model.subjectLists = await LoadSubjects(true);
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";
                var c = await request.Get<Book>(Getsub);

                model.BookTitle = c.FirstOrDefault().BookTittle;
                model.BookNumber = c.FirstOrDefault().BookNumber;
                model.IsbnNumber = c.FirstOrDefault().IsbnNumber;
                model.Author = c.FirstOrDefault().Author;
                model.Publisher = c.FirstOrDefault().Publisher;
                model.Subject = c.FirstOrDefault().Subject;
                model.Qty = c.FirstOrDefault().Qty;
                model.Price = c.FirstOrDefault().Price;
                model.BookDescription = c.FirstOrDefault().BookDescription;
                model.RackNumber = c.FirstOrDefault().RackNumber;
                model.Available = c.FirstOrDefault().Available;
                model.BookId = c.FirstOrDefault().BookId;
                model.delete = true;
                resp = await request.Update<Book>(model, EditUrl);
                var data = new { status = 200, res = resp };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
                
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

        // POST: LibraryBooksController/Delete/5
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
