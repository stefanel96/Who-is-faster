using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagerInterfaces
{
    public interface IGameNotificationManager
    {
        Task SendMessage(string user, string message);

        Task AddConnectionToGroup(string connectionId, string groupName);

        Task SendRoomInfoToGroup(string groupName, string roomObject);

        Task SendLeaveRoomSignalToGroup(string groupName);
    }
}

