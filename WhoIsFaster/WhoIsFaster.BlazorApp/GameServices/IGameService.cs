using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsFaster.BlazorApp.GameServices
{
    public interface IGameService
    {
        void UpdateRoomPlayerInput(int roomId, string userName, string input);
        Task AddRoomToGame(int roomId);
    }
}
