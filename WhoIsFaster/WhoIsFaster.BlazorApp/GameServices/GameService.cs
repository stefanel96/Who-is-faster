using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.BlazorApp.GameDefinitions;

namespace WhoIsFaster.BlazorApp.GameServices
{
    class GameService : IGameService
    {
        private readonly Game game;

        public GameService()
        {
            game = Game.Instance;
        }
        public void UpdateRoomPlayerInput(int roomId, string userName, string input)
        {
            game.UpdateRoomPlayerInput(roomId, userName, input);
        }

        public async Task AddRoomToGame(int roomId)
        {
            await game.AddRoom(roomId);
        }
    }
}
