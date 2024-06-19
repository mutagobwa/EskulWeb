using Eskul.APIClient;
using Eskul.Hubs;
using Eskul.HubsClient;
using Eskul.Models;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;
using System.Diagnostics;
using System.Reflection;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Eskul.Controllers
{
    public class HomeController : Controller
    {
        //testing git
        int pluse = 0;
        SessionDetail _sd;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        private readonly MyUtilities _myUtilities;
        public HomeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, MyUtilities myUtilities)
        {
            _httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public async Task<IActionResult> Index()        
        {
            if (!SessionData.IsSignedIn){return RedirectToAction("Index", "Login");}
            ApiResponse respons = await _myUtilities.LoadStudentCount();
            if (respons.Success)
            {
                var Scount = respons.PayLoad;
                TempData["studentCount"] = Scount;
            }

            ApiResponse respon = await _myUtilities.LoadStaffsByCategory("T");
            if (respon.Success)
            {
                var Teachingcount = JsonConvert.DeserializeObject<List<StaffModel>>(respon.PayLoad);
                TempData["Teachingcount"] = Teachingcount.Count;
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
            ApiResponse re = await _myUtilities.LoadStudentPopByGender();
            if (re.Success)
            {
                var genderDistribution = JsonConvert.DeserializeObject<List<StudentGenderDistribution>>(re.PayLoad);
                TempData["GenderDistribution"] = JsonConvert.SerializeObject(genderDistribution);
            }
            else
            {
                TempData["GenderDistribution"] = "[]"; // or handle the error appropriately
            }

            ApiResponse studpop = await _myUtilities.LoadStudentPopByClass();
            if (studpop.Success)
            {
                var ClassDistribution = JsonConvert.DeserializeObject<List<StudentClassDistribution>>(studpop.PayLoad);
                TempData["ClassDistribution"] = JsonConvert.SerializeObject(ClassDistribution);
            }
            else
            {
                TempData["ClassDistribution"] = "[]"; // or handle the error appropriately
            }
            ApiResponse studpopclassStream = await _myUtilities.LoadStudentPopByClassStream();
            if (studpopclassStream.Success)
            {
                var classDistributionStream = JsonConvert.DeserializeObject<List<StudentCountByClassAndStream>>(studpopclassStream.PayLoad);
                TempData["ClassDistributionStream"] = JsonConvert.SerializeObject(classDistributionStream);
            }
            else
            {
                TempData["ClassDistributionStream"] = "[]"; // or handle the error appropriately
            }
            //ApiResponse studpopclassStream = await _myUtilities.LoadStudentPopByClassStream();
            //if (studpopclassStream.Success)
            //{
            //    var classDistributionStream = JsonConvert.DeserializeObject<List<StudentCountByClassAndStream>>(studpopclassStream.PayLoad);
            //    TempData["ClassDistributionStream"] = JsonConvert.SerializeObject(classDistributionStream);
            //}
            //else
            //{
            //    TempData["ClassDistributionStream"] = "[]"; // or handle the error appropriately
            //}

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}