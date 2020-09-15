using Microsoft.AspNetCore.SignalR;
using WhoIsFaster.Infrastructure.SignalRNotifications.Hubs;
using WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagers
{
    public class GameNotificationManager : IGameNotificationManager
    {
        private readonly IHubContext<WhoIsFasterSignalRHub> _hubContext;

        public GameNotificationManager(IHubContext<WhoIsFasterSignalRHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task AddConnectionToGroup(string connectionId, string groupName)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task SendMessage(string user, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendRoomInfoToGroup(string groupName, string roomObject)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveRoom", roomObject);
        }
    }
}
