using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.ViewEngines;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Buffers.Text;
using System.Configuration;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Eskul.Controllers
{
    public class SchInfoController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public SchInfoController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _logger = logger;   
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        // GET: SchInfoController

        public async Task<ActionResult> Index(GeneralSettingAdd model, IFormFile formFile)
        {
            try
            {
                if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }

                ApiResponse response = await _myUtilities.LoadSchoolDetails();

                if (response.Success)
                {
                    model = JsonConvert.DeserializeObject<GeneralSettingAdd>(response.PayLoad);
                    ViewBag.Logo= "data:image/png;base64,"+model.Logo;
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
       
        // POST: SchInfoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GeneralSettingAdd model, IFormFileCollection files)
        {

            
            try
                {
                if (!SessionData.IsSignedIn)
                {
                    // Redirect the user to the login page
                    return RedirectToAction("Index", "Login");
                }
                //ApiResponse response = await _myUtilities.LoadSchoolDetails();

                //if (response.Success)
                //{
                //    model = JsonConvert.DeserializeObject<GeneralSettingAdd>(response.PayLoad);
                //}
                string Url = "Settings/School/Config/Save";
                ApiResponse resp = null;
                string base64String = "";
                byte[] imageBytes = null; model.Logo = "";
                if (files.Count > 0)
                {
                    BinaryReader reader = new BinaryReader(files.First().OpenReadStream());
                    imageBytes = reader.ReadBytes((int)files.First().Length);
                    base64String = Convert.ToBase64String(imageBytes);
                    model.Logo = base64String;
                }
                else
                {
                    model.Logo = SessionData.SchoolLogo;
                }
                model.CountryCode = "UG";
                model.PostalAdress = model.LocationAddress;
                model.SchoolCode = SessionData.ClientCode;
                if (string.IsNullOrEmpty(model.UNEBNo))
                {
                    model.UNEBNo = "000000";
                }
                resp = await request.AddAsync<GeneralSettingAdd>(model, Url);
                if (resp.ResponseCode == 100)
                {
                    TempData["success"] = resp.ResponseMessage;
                }
                else if (resp.ResponseCode == 101)
                {
                    TempData["error"] = resp.ResponseMessage;
                }
                else if (resp.ResponseCode == 500)
                {
                    TempData["error"] = resp.ResponseMessage;
                }

                return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                _logger.Error(ex.Message, ex);
                TempData["error"] = "Error Occured Contact Admin" ;
                return View(nameof(Index));
                }
            }
    }
}
