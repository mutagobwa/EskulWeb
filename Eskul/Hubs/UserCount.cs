using Eskul.Controllers;
using Eskul.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Eskul.Hubs
{
    public class UserCount:Hub
    {
        private readonly MyUtilities _myUtilities;
        public static int TotalViews { get; set; } = 0;
        public UserCount(MyUtilities myUtilities)
        {
            _myUtilities = myUtilities;
        }
        public async Task NewWindowLoaded()
        {
            TotalViews++;
            await Clients.All.SendAsync("UpdateTotalViews", TotalViews);

        }
        //public async Task<int> LoadStudents(bool fromDb)
        //{
        //    var students = await _myUtilities.LoadStaff(true);
        //    int totalCount = students.Count; // Get the total count of students
        //    return totalCount;
        //}
        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}
    }
}
