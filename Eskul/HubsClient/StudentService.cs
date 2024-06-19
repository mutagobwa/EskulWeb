using Eskul.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Eskul.HubsClient
{
    public class StudentService
    {
        private readonly IHubContext<StudentCount> _hubContext;
        private int _studentCount;
        public StudentService(IHubContext<StudentCount> hubContext)
        {
            _hubContext = hubContext;
            _studentCount =0;
        }
        public void AddStudent()
        {
            // Add student logic here
            _studentCount++;
            NotifyStudentCountChanged();
        }
        public void RemoveStudent()
        {
            // Remove student logic here
            _studentCount--;
            NotifyStudentCountChanged();
        }

        private async Task NotifyStudentCountChanged()
        {
            await _hubContext.Clients.All.SendAsync("StudentCountChanged", _studentCount);
        }
    }
}
