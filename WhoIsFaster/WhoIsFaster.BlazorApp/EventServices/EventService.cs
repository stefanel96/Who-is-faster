using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagerInterfaces;

namespace WhoIsFaster.BlazorApp.EventServices
{
    public class EventService : IEventService
    {
        private readonly IGameNotificationManager _NotificationManager;

        public EventService(IGameNotificationManager NotificationManager)
        {
            _NotificationManager = NotificationManager;
        }
        public async Task AddConnectionToSignalRGroup(string connectionId, string groupName)
        {
            await _NotificationManager.AddConnectionToGroup(connectionId, groupName);
        }
    }
}
