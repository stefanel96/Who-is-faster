using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhoIsFaster.BlazorApp.EventServices
{
    public interface IEventService
    {
        Task AddConnectionToSignalRGroup(string connectionId, string groupName);
    }
}
