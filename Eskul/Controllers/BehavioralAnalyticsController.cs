using Eskul.APIClient;
using Eskul.Custom;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Security;
using SmartPaperEdms.Web.App_Code;

namespace Eskul.Controllers
{
    public class BehavioralAnalyticsController : Controller
    {
        private static string Url = "";
        RequestHandler request;
        SessionDetail _sd;
        private readonly IConfiguration configuration;
        private readonly ILoggerErr _logger;
        private readonly MyUtilities _myUtilities;
        public BehavioralAnalyticsController(IConfiguration configuration, ILoggerErr logger, MyUtilities myUtilities)
        {
            _sd = new SessionDetail();
            _logger = logger;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<ActionResult> Index()
        {
            if (!SessionData.IsSignedIn) { return RedirectToAction("Index", "Login"); }
            ApiResponse respons = await _myUtilities.LoadStudentCount();
            if (respons.Success)
            {
                var Scount = respons.PayLoad;
                TempData["TotalIncidentsByTerm"] = Scount;
            }

            ApiResponse respon = await _myUtilities.LoadStaffsByCategory("T");
            if (respon.Success)
            {
                var Teachingcount = JsonConvert.DeserializeObject<List<StaffModel>>(respon.PayLoad);
                TempData["UnreviedByTerm"] = Teachingcount.Count;
            }
            ApiResponse res = await _myUtilities.LoadStaffsByCategory("NT");
            if (res.Success)
            {
                var Noncount = JsonConvert.DeserializeObject<List<StaffModel>>(res.PayLoad);
                TempData["Noncount"] = Noncount.Count;
            }
            ApiResponse respo = await _myUtilities.LoadParents();
            if (respo.Success)
            {
                var Parents = JsonConvert.DeserializeObject<List<ParentViewDTO>>(respo.PayLoad);
                TempData["ParentCount"] = Parents.Count;
            }

            return View();
        }
    }
}
