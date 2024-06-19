using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nancy;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using SmartPaperEdms.Web.App_Code;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Eskul.Controllers
{
    public class StaffController : BaseController
    {
        // GET: StaffController
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public StaffController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;   
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(StaffModel model)
        
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {

                if (!string.IsNullOrEmpty(model.Category))
                {
                    ApiResponse response = await _myUtilities.LoadStaffsByCategory(model.Category);
                    

                    if (response.ResponseCode == 100)
                    {
                        model.Staffs = JsonConvert.DeserializeObject<List<StaffModel>>(response.PayLoad);
                        model.Category = model.Category;
                    }
                    else if (response.ResponseCode == 101)
                    {
                        TempData["info"] = response.ResponseMessage;
                    }
                    else if (response.ResponseCode == 500)
                    {
                        TempData["error"] = response.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = "Response Unkown";
                    }
                    return View(model);
                }

            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
            }
            return View(model);
        }

        // GET: StaffController/Details/5
        public async Task<ActionResult> Details(StaffProfile model, String id)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                if (!string.IsNullOrEmpty(id))
                {
                    ApiResponse response = await _myUtilities.LoadStaffProfile(id);

                    if (response.ResponseCode == 100)
                    {
                        model = JsonConvert.DeserializeObject<StaffProfile>(response.PayLoad);
                    }
                    else if (response.ResponseCode == 101)
                    {
                        TempData["info"] = response.ResponseMessage;
                    }
                    else if (response.ResponseCode == 500)
                    {
                        TempData["error"] = response.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = "Response Unkown";
                    }
                    //string StaffImageUrl = $"StaffManagement/RetrieveStaffImage/{model.Staffno}/{"1"}";
                    //var model2 = new StaffImageList();
                    //var resp = await request.GetB(StaffImageUrl);
                    //model2.Image64String = resp.Split('\"')[7];
                    //ViewBag.StaffImg = "data:image/png;base64," + model2.Image64String;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return View();
            }
            return View(model);
        }

        // GET: StaffController/Create
        public async Task<ActionResult> Create(StaffModel model, string id)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                ApiResponse response = null;
                response = await _myUtilities.LoadBranches();

                if (response != null && response.Success)
                {
                    List<Branch> classLists = JsonConvert.DeserializeObject<List<Branch>>(response.PayLoad);
                    var l = (from p in classLists select new ListItems { Text = p.BranchName, Value = p.BranchId.ToString() }).ToList();

                    ViewBag.Branches = LoadListItems(l, true);
                }
                else
                {
                    var l = new List<ListItems>();
                    ViewBag.Branches = LoadListItems(l, true);
                }

                if (model.BranchId == 0) { model.BranchId = SessionData.UserBranchId; }
                //if (model.Citizentype==0) { model.Citizentype.; }
                if (string.IsNullOrEmpty(model.Nationality)) { model.Nationality = "UG"; }
                if (model.TermofEntry==0) { model.TermofEntry = SessionData.Term; }
                if (!string.IsNullOrEmpty(id))
                {
                    ApiResponse resp = await _myUtilities.LoadStaff(id);
                    if (resp.Success && resp != null)
                    {
                        model = JsonConvert.DeserializeObject<StaffModel>(resp.PayLoad);
                    }
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
           
           return View(model);  
        }
        public async Task<ActionResult> Attendance(StaffList model)
        {

            //ApiResponse response = null;

            //response = await _myUtilities.LoadStaffsByCategory(model.Category??"T");

            //if (response != null && response.ResponseCode == 100)
            //{
            //    List<staffcategory> classLists = JsonConvert.DeserializeObject<List<staffcategory>>(response.PayLoad);
            //    var l = (from p in classLists select new ListItems { Text = p.categoryname, Value = p.categorycode.ToString() }).ToList();

            //    ViewBag.StaffCats = LoadListItems(l, true);
            //}
            //else
            //{
            //    var l = new List<ListItems>();
            //    ViewBag.StaffCats = LoadListItems(l, true);
            //}
            
            try
            {

                if (!SessionData.IsSignedIn)
                {

                    return RedirectToAction("Index", "Login");
                }
                if (!string.IsNullOrEmpty(model.Category))
                {
                    ApiResponse respons = await _myUtilities.LoadStaffsByCategory(model.Category);

                    if (respons.Success)
                    {
                        model.staffLists = JsonConvert.DeserializeObject<List<StaffList>>(respons.PayLoad);
                    }
                    else if (respons.ResponseCode == 101)
                    {
                        TempData["info"] = respons.ResponseMessage;
                    }
                    else if (respons.ResponseCode == 500)
                    {
                        TempData["error"] = respons.ResponseMessage;
                    }
                    else
                    {
                        TempData["error"] = "Response Unkown";
                    }                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateStatus(string Staffno, string status)
        {
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                var stude = Staffno.Trim();
                var sts = status;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
            }
            return Json(new { success = true });
        }
        // POST: StaffController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StaffModel model, StaffImage model2, IFormFileCollection files, IFormCollection collection)
        {
            Url = "Staffs";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                model.Initial = _myUtilities.GetInitials(model.FirstName, model.MiddleName, model.LastName);
                string base64String = "";
                //if (model.Citizentype == 0)
                //{
                //  model.Citizentype = 1;
                //}
                if (model.Nationality == null)
                {
                    model.Nationality = "UG";
                }
                if (model.BranchId == 0)
                {
                    model.BranchId = SessionData.UserBranchId;
                }
                if (model.StaffNo == null)
                {

                    model.StaffNo = collection["Staffno"].ToString();
                }
                if (model.TermofEntry == 0)
                {
                    model.TermofEntry = SessionData.Term;
                }
                model.StaffId = string.IsNullOrEmpty(model.StaffId) ? "" : model.StaffId;
                model.StaffNo = string.IsNullOrEmpty(model.StaffNo) ? "" : model.StaffNo;
                model.PositionTitle = string.IsNullOrEmpty(model.PositionTitle) ? "" : model.PositionTitle;
                model.SchoolCode = SessionData.ClientCode;
                byte[] imageBytes = null;
                if (files.Count > 0)
                {
                    BinaryReader reader = new BinaryReader(files.First().OpenReadStream());
                    imageBytes = reader.ReadBytes((int)files.First().Length);
                    base64String = Convert.ToBase64String(imageBytes);
                    model2.StaffNo = model.StaffNo;
                    model2.ImageType = 1;
                    model2.Image64String = base64String;
                }
                if (!string.IsNullOrEmpty(model.StaffId))
                {
                    Url = $"Staffs/{model.StaffId}";
                    resp = await request.UpdateAsync<StaffModel>(model, Url);
                    if (resp.ResponseCode == 100)
                    {
                        TempData["success"] = resp.ResponseMessage;
                    }
                    else if (resp.ResponseCode == 101) { TempData["info"] = resp.ResponseMessage; }
                    else { TempData["error"] = resp.ResponseMessage; }
                }
                else
                {
                    resp = await request.AddAsync<StaffModel>(model, Url);
                    if (resp.ResponseCode == 100)
                    {
                        TempData["success"] = resp.ResponseMessage;
                    }
                    else if (resp.ResponseCode == 101) { TempData["info"] = resp.ResponseMessage; }
                    else { TempData["error"] = resp.ResponseMessage; }
                }
                
                return RedirectToAction(nameof(Index), new { model.Category});
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: StaffController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ApiResponse resp = null;
            var model = new StaffModel();
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Create", new {id=id});
        }

        // GET: StaffController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {

            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            Url = $"Staffs?id={id}";
            try
            {
                var myresp = await request.DeleteAsync(Url);
                var data = new { status = 200, res = myresp.ResponseMessage };
                var json = JsonConvert.SerializeObject(data);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }

        }
        public async Task<ActionResult> BulkStaff(BulkImport model)
        {
            if (!SessionData.IsSignedIn)
            {
                return RedirectToAction("Index", "Login");
            }

            var contentRootPath = "";
            ApiResponse res = await _myUtilities.LoadSysparamByName("BULK_FILES_LOCATION");
            if (res.Success && res.ResponseCode == 100)
            {
                var model1 = new SysParamVm();
                model1 = JsonConvert.DeserializeObject<SysParamVm>(res.PayLoad);
                contentRootPath = model1.Value;
            }
            else if (res.ResponseCode == 101)
            {
                TempData["info"] = res.ResponseMessage;
            }
            else
            {
                TempData["error"] = res.ResponseMessage;
            }

            // Get all files in the content root path
            var files = Directory.GetFiles(contentRootPath);

            // Create a list of BulkImport objects to store the file information
            var bulkImports = new List<BulkImport>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var fileExtension = Path.GetExtension(file);
                var filePath = Path.GetDirectoryName(file);

                // Create a new BulkImport object and set its properties
                if (fileName.IndexOf("staff", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var bulkImport = new BulkImport
                    {
                        FileName = fileName,
                        FileExt = fileExtension,
                        FilePath = filePath
                    };

                    // Add the BulkImport object to the list
                    bulkImports.Add(bulkImport);
                }
            }

            model.ImportFiles = bulkImports;
            ViewBag.ContentRootPath = contentRootPath;
            ViewBag.BulkImports = bulkImports; // Set the list of BulkImport objects to ViewBag

            return View(model);
        }

        public async Task<ActionResult> BulkStaffImport(BulkImport model, string fileName)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {
                ApiResponse resp;
                model.FileName = fileName + model.FileExt;
                Url = $"Staffs/{SessionData.ClientCode}/{model.FileName}";
                
                resp = await request.AddAsync<BulkImport>(model, Url);
                if (resp.Success && resp.ResponseCode == 100 && resp.PayLoad != null)
                {
                    TempData["success"] = resp.ResponseMessage;

                }
                else if (resp.ResponseCode == 101)
                {
                    TempData["info"] = resp.ResponseMessage;
                }
                else
                {
                    TempData["error"] = resp.ResponseMessage;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }


            return RedirectToAction(nameof(BulkStaff));
        }
    }
}
