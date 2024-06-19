using Eskul.APIClient;
using Eskul.Controllers;
using Eskul.Hubs;
using Eskul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SmartPaperEdms.Web.App_Code;

namespace Eskul
{
    public class StudentCountHub:Hub
    {
        private static int _userCount = 0;
        private readonly MyUtilities _myUtilities;
        SessionDetail _sd;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static string Url = "";
        RequestHandler request;
        private readonly IConfiguration configuration;
        public StudentCountHub(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, MyUtilities myUtilities)
        {
            _httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            request = new RequestHandler(configuration);
            _myUtilities = myUtilities;
        }
        public override async Task OnConnectedAsync()
        {
            _userCount++;
           //var model = await _myUtilities.LoadStudentCount();
           // int studentCount = (model != null ? model.FirstOrDefault<StudentAnalytic>()?.TotalStudents : new int?()).GetValueOrDefault();
            //var Teachers = await _myUtilities.LoadStaff(true);
            //var TeacherCount = Teachers.Count;
            //var Parents = await _myUtilities.LoadParents(true);
            // //await LoadMenusAsync();
            //var ParentCount = Parents.Count;
            //Clients.All.SendAsync("UserCountChanged", _userCount);
            //Clients.All.SendAsync("StudentCountChanged", studentCount);
            //Clients.All.SendAsync("TeacherCountChanged", TeacherCount);
            //Clients.All.SendAsync("ParentCountChanged", ParentCount);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _userCount--;
            Clients.All.SendAsync("UserCountChanged", _userCount);
            await base.OnDisconnectedAsync(exception);
        }
        //private async Task LoadMenusAsync()
        //{
        //    Url = "Menu/Licence/Validate/" + SessionData.LicenceCode + "/" + SessionData.ProductCode + "/" + SessionData.ClientCode;
        //    var menus = new List<Menu>();
        //    var ApiResp = await request.GetB(Url);
        //    var LicenceResp = ApiResp.Replace("[", string.Empty).Replace("{", string.Empty).Replace("]", string.Empty).Replace("}", string.Empty);
        //    var respcode = LicenceResp.ToString().Split(',')[0].Split(':')[1];
        //    var respmsg = LicenceResp.ToString().Split(',')[1].Split(':')[1];

        //    if (respcode.Split('\"')[1] == "100")
        //    {
        //        menus = (await _myUtilities.LoadMenus(SessionData.UserProfileCode)).Where(x => x.Status == true).ToList();
        //    }

        //    var httpContext = _httpContextAccessor.HttpContext;
        //    httpContext.Session.SetString("LicenceMsg", JsonConvert.SerializeObject(respmsg.Split('\"')[1]).Replace("\"", string.Empty));
        //    httpContext.Session.SetString("Menus", JsonConvert.SerializeObject(menus));
        //}

    }
}
