using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Configuration;
using System.Reflection;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class ParentsController : BaseController
    {
        // GET: ParentsController
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public ParentsController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities; 
        }
        public async Task<ActionResult> Index(ParentViewDTO model,string id)
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }

            try
            {
                ApiResponse response;
                if (!string.IsNullOrEmpty(id))
                {
                    string resp = "";

                    if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                    response = await _myUtilities.LoadParent(id);

                    if (response.Success)
                    {
                        model = JsonConvert.DeserializeObject<ParentViewDTO>(response.PayLoad);
                    }
                }

                response = await _myUtilities.LoadParents();

                if (response.Success)
                {
                    model.parents = JsonConvert.DeserializeObject<List<ParentViewDTO>>(response.PayLoad);
                }
                else if (response.ResponseCode == 101)
                {
                    TempData["error"] = response.ResponseMessage;
                }
                else if (response.ResponseCode == 500)
                {
                    TempData["error"] = response.ResponseMessage;
                }
                else
                {
                    TempData["error"] = "Response Unkown";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
            }
            return View(model);
        }

        // GET: ParentsController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var model = new ParentViewDTO();
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            try
            {
                ApiResponse res = await _myUtilities.LoadParent(id);

                if (res.ResponseCode == 100 && res.PayLoad != null)
                {
                    model = JsonConvert.DeserializeObject<ParentViewDTO>(res.PayLoad);
                    model.Image = new ParentImage();
                    if (!string.IsNullOrEmpty(model.Image.Image64String))
                    {
                        ViewBag.ParentImg = "data:image/png;base64," + model.Image.Image64String;
                    }
                    
                }
                else if (res.ResponseCode==101)
                {
                    TempData["Info"] = res.ResponseMessage;
                }
                else {
                    TempData["error"] = "Error Occured Contact Admin";
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

        // GET: ParentsController/Create
        public async Task<ActionResult> Create(ParentAdd model,int id)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                {
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

        // POST: ParentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateParentApiModel model,IFormFileCollection files)
        {
            Url = "Parents";
            ApiResponse resp = null;
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
                model.SchoolCode = SessionData.ClientCode;
                if (string.IsNullOrEmpty(model.ParentNo)) { model.ParentNo = ""; }
                if (string.IsNullOrEmpty(model.Contact2)) { model.Contact2 = ""; }
                if (string.IsNullOrEmpty(model.Nin)) { model.Nin = ""; }
                if (string.IsNullOrEmpty(model.Email)) { model.Email = ""; }
                if (string.IsNullOrEmpty(model.MiddleName)) { model.MiddleName = ""; }

                string base64String = "";
                byte[] imageBytes = null;
                if (files.Count > 0)
                {
                    BinaryReader reader = new BinaryReader(files.First().OpenReadStream());
                    imageBytes = reader.ReadBytes((int)files.First().Length);
                    base64String = Convert.ToBase64String(imageBytes);
                    model.Image = new ParentImage();
                    model.Image.Image64String = base64String;
                    model.Image.ImageType = 1;
                    model.Image.ParentNo = model.ParentNo ?? "";
                }
                if (!string.IsNullOrEmpty(model.ParentNo))
                {
                    Url = $"Parents/{model.ParentId}";
                    resp = await request.UpdateAsync<CreateParentApiModel>(model, Url);
                    if (resp.ResponseCode == 100)
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
                else
                {
                    resp = await request.AddAsync<CreateParentApiModel>(model, Url);
                    if (resp.ResponseCode == 100)
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
               

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ParentsController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var model = new ParentAdd();
            try
            {
                    if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }

                    ApiResponse res = await _myUtilities.LoadParent(id);

                    if (res.ResponseCode == 100 && res.PayLoad != null)
                    {
                        model = JsonConvert.DeserializeObject<ParentAdd>(res.PayLoad);
                    }

                }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Create", model);
        }

        // POST: ParentsController/Edit/5
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

        // GET: ParentsController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            string resp = "";
            string UpdateUrl = "ParentManagement/Save";
            string EditUrl = "ParentManagement/Parent/" + id + "";
            var model = new ParentAdd();
            try
            {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                //var c = await request.Get<ParentAdd>(EditUrl);

                //model.parentid = c.FirstOrDefault().parentid;
                //model.nin = c.FirstOrDefault().nin;
                //model.tribe = c.FirstOrDefault().tribe;
                //model.parentno = c.FirstOrDefault().parentno;
                //model.prefix = c.FirstOrDefault().prefix;
                //model.residence = c.FirstOrDefault().residence;
                //model.firstname = c.FirstOrDefault().firstname;
                //model.middlename = c.FirstOrDefault().middlename;
                //model.lastname = c.FirstOrDefault().lastname;
                //model.gender = c.FirstOrDefault().gender;
                //model.contact1 = c.FirstOrDefault().contact1;
                //model.contact2 = c.FirstOrDefault().contact2;
                //model.email = c.FirstOrDefault().email;
                //model.occupation = c.FirstOrDefault().occupation;
                //model.religion = c.FirstOrDefault().religion;
                //model.nationality = c.FirstOrDefault().nationality;
                //model.tribe = c.FirstOrDefault().tribe;
                //model.citizentype = c.FirstOrDefault().citizentype;
                //model.branchId = c.FirstOrDefault().branchId;
                //model.delete = true;
                //resp = await request.Add<ParentAdd>(model, UpdateUrl);
                //var data = new { status = 200, res = resp };
                //var json = JsonConvert.SerializeObject(data);
                //return Content(json, "application/json");
                return RedirectToAction(nameof(Index), model);
            }
            catch (Exception ex)
            {
                //   TempData["error"] = "Error Occured" + " " + resp;
                var data = new { status = 201, message = ex };
                var json = JsonConvert.SerializeObject(data);
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return Content(json, "application/json");
                //return RedirectToAction(nameof(Index));
            }
        }

        // POST: ParentsController/Delete/5
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
        public async Task<ActionResult> ParentDash()
        {
            if (!SessionData.IsSignedIn)
            {
                // Redirect the user to the login page
                return RedirectToAction("Index", "Login");
            }
            return View();

        }

    }
}
