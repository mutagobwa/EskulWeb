using Microsoft.AspNetCore.SignalR;

namespace Eskul.Hubs
{
    public class StudentCount:Hub
    {
        public async Task NotifyStudentCountChanged(int newCount)
        {
            await Clients.All.SendAsync("StudentCountChanged", newCount);
        }
    }
}
