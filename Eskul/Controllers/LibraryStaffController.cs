

using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Session;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

namespace Eskul.Controllers
{
    public class LibraryStaffController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public LibraryStaffController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(StaffList model)
        {
            //object model = null;
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                model.staffLists = await _myUtilities.LoadStaffs(true);
                
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }

        // GET: LibraryStaffController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> IssueBook(IssueBook model)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";
                Url = "Library/AddIssuedBook";
                resp = await request.Add<IssueBook>(model, Url);
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
            }
            
            return RedirectToAction(nameof(IssueReturn));
        }
        
        public async Task<ActionResult> ReturnBook(IssueBook model,int Id,IFormCollection f)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";
                Url = "Library/AddReturnedBook";
                model.BookReturn.IssuedId = (int)model.BookId;
                model.BookReturn.ReturnDate = model.BookReturn.ReturnDate;
                model.BookReturn.Comment =f["Comment"].ToString();
                if (model.BookReturn.Comment==null)
                {
                    model.BookReturn.Comment = "";
                }
                resp = await request.Add<BookReturn>(model.BookReturn, Url);
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
            }
            
            return RedirectToAction(nameof(IssueReturn));

        }
        
        public async Task<ActionResult> IssueReturn(IssueBook model,string id)
         {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            var l = (from p in await _myUtilities.LoadBooks(true) select new ListItems { Text = p.BookTittle, Value = p.BookId.ToString() }).ToList();
            ViewBag.Books = LoadListItems(l, true);
            
            if (id==null)
            {
                id = @TempData["MembershipId"] as string;
            }
            string EditUrl = "Library/LibraryMemberShipByMembershipNo/" + id + "";

            

            try
            {
                model.libraryMembers = await request.Get<LibraryMembers>(EditUrl);
                try
                {
                    
                    string IssuedBooksUrl = "Library/IssuedBooksByMembershipNo/" + id + "";
  
                    model.issuedBooks = await request.Get<IssuedBooks>(IssuedBooksUrl);
                    TempData["MembershipId"] = id; 
                }
                catch (Exception ex)
                {

                    TempData["error"] = "Error Occured Contact Admin" ;

                }


                //model.delete = false;


            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return View();
            }
            return View("IssueReturn", model);
        }
        public async Task<ActionResult> AddLibStaff(StaffList model,string id)
        {
            string resp = "";
            string EditUrl = "StaffManagement/Staff/" + model.Staffid + "";
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var Url = "Library/AddLibraryMemberShip";
                var model1 = new LibraryMember();
                var c = await request.Get<StaffList>(EditUrl);
                model1.AdmissionNo = c.FirstOrDefault().Staffid;
                model1.MembershipId = c.FirstOrDefault().Staffid;
                model1.MemberType = "T";//c.FirstOrDefault().MemberType;
                model1.LibraryCardNo = model.libraryMember.LibraryCardNo;
                resp = await request.Add<LibraryMember>(model1, Url);
                if (resp.Contains("successfully"))
                {
                    TempData["success"] = resp;
                }
                else
                {
                    TempData["error"] = "Error Occured"+" "+resp;
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
        // GET: LibraryStaffController/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult GetPartial(int id)
        {
            
            return PartialView("Views_Shared__BookReturn");
        }
        public async Task<ActionResult> Revock(string id)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                string resp = "";
                string Url = "Library/SurrenderLibraryMembership/" + id;
                resp = await request.GetA(Url);
                if (resp.Contains("successfully"))
                {
                    TempData["success"] = resp;
                }
                else
                {
                    TempData["error"] = "Error Occured" + " " + resp;
                }
                return RedirectToAction(nameof(AllLibMembers));
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(AllLibMembers));
            }
        }
        // POST: LibraryStaffController/Create
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

        // GET: LibraryStaffController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibraryStaffController/Edit/5
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

        // GET: LibraryStaffController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibraryStaffController/Delete/5
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
