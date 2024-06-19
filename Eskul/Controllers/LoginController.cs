using ASEEncryptorDecryptorTool;
using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Net;
using System.Security.Principal;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class LoginController : BaseController
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly ILoggerErr _logger;
        private readonly IConfiguration configuration;
        private readonly MyUtilities _myUtilities;
        public LoginController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            this.configuration = configuration;
            _sd = new SessionDetail();
            request = new RequestHandler(configuration);
            _logger = logger;
            _myUtilities = myUtilities;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginModel model)
         {
            try
            {
                if (ModelState.IsValid)
                {
                    model.ClientCode = model?.UserName?.Split('-').Last();
                    Url = $"Users/SignIn/{model?.ClientCode}/{model?.UserName}/{ model?.Password}";
                    var res = await request.GetAsync(Url);
                    if (res!=null)
                    {
                        if (res.ResponseCode==100)
                        {
                            GetRequestDetails(this.HttpContext, out string WindowsUser, out string IpAdress, out string WorkStation);

                            ApiResponse response = await _myUtilities.GetUserByUsername(model);
                            if (response.Success)
                            {
                                var u = JsonConvert.DeserializeObject<UsersDet>(response.PayLoad);
                                _sd.IsSignedIn = true;
                                _sd.UserId = u.UserId;
                                _sd.SchoolLogo = u.SchoolLogo;
                                _sd.Username = u.UserName;
                                _sd.BranchId = u.BranchId;
                                _sd.UserBranchCode = u.BranchCode;
                                _sd.UserBranchName = u.BranchName;
                                _sd.ProfileId = u.ProfileCode1;
                                _sd.ProfileName = u.ProfileName;
                                var dt = DateTime.Now;
                                _sd.LcyDate = dt;
                                _sd.UserIPAddress = IpAdress;
                                _sd.UserWorkStation = WorkStation;
                                _sd.WindowsUser = WindowsUser;
                                _sd.CanAdd = u.Can_Add;
                                _sd.CanDelete = u.Can_Delete;
                                _sd.CanUpdate = u.Can_Update;
                                _sd.Current_Stp = u.CurrentStp;
                                _sd.Previous_Stp = u.PreviousStp;
                                _sd.Next_Stp = u.NextStp;
                                _sd.Mult_Institutional = u.MultInstitutional;
                                _sd.Term = u.Term;
                                _sd.UserrefNo = u.UserrefNo;
                                _sd.UserProfileCode = u.ProfileCode;
                                _sd.Licence = u.LicenceCode;
                                _sd.ClientCode = u.ClientCode;
                                _sd.ProductCode = u.ProductCode;
                                _sd.IsAdmin = u.IsAdmin;    
                                var userJson = JsonConvert.SerializeObject(_sd);
                                HttpContext.Session.SetString("user", userJson);

                                await LoadMenusAsync();
                                var menuJson = HttpContext.Session.GetString("Menus");
                                var menus = JsonConvert.DeserializeObject<List<Menu>>(menuJson);
                                return RedirectToAction("Index", "Home");
                            }
                            else if (response.ResponseCode == 101)
                            {
                                ViewBag.ErrorMessage = response.ResponseMessage;
                            }
                            else if (response.ResponseCode == 500)
                            {
                                ViewBag.ErrorMessage = "Error Occured, Contact Admin";
                            }

                        }
                        else if (res.ResponseCode ==101)
                        {
                            ViewBag.ErrorMessage = res.ResponseMessage;
                        }
                        else if (res.ResponseCode == 500)
                        {
                            ViewBag.ErrorMessage = res.ResponseMessage;
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt. Contact Admin.");
                        ViewBag.ErrorMessage = res.ResponseMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);

                ViewBag.ErrorMessage = "Error occured Contact Admin";
            }


            return View(model);
        }

        public async Task<ActionResult> Create([FromQuery] string Username)
        {
            try
            {
                
                ApiResponse resp  = await _myUtilities.SignOutUser(Username);
                if (resp.ResponseCode==100)
                {
                    if (resp.ResponseCode ==100)
                    {
                        SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                        sd.IsSignedIn = false;
                        var userJson = JsonConvert.SerializeObject(sd);
                        HttpContext.Session.SetString("user", userJson);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = "Error Occured " + resp.ResponseMessage;
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
       
        
        private async Task<ActionResult> LoadMenusAsync()
        {
            Url = $"Menu/Licence/Validate/{SessionData.LicenceCode}/{SessionData.ProductCode}/{SessionData.ClientCode}";
            var menus = new MenuViewModel();
            var ApiResp = await request.GetAsync(Url);
            if (ApiResp.ResponseCode ==100)
            {
                var menuresp = (await _myUtilities.LoadMenus(SessionData.UserProfileCode, SessionData.ClientCode));
                menus.Menuss=JsonConvert.DeserializeObject<List<Menu>>(menuresp.PayLoad);
            }
            HttpContext.Session.SetString("LicenceMsg",ApiResp.ResponseMessage);
            HttpContext.Session.SetString("Menus", JsonConvert.SerializeObject(menus.Menuss));
            return Ok();
        }
    }
}
