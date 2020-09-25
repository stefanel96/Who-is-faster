using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using WhoIsFaster.Domain.Interfaces;
using WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagerInterfaces;

namespace WhoIsFaster.BlazorApp.GameDefinitions
{
    public class GameLoop
    {

        public GameLoop(int loopIntervalMiliseconds)
        {
            Game = Game.Instance;
            LoopIntervalMiliseconds = loopIntervalMiliseconds;

        }
        public bool Running { get; private set; }
        public int LoopIntervalMiliseconds { get; private set; }
        public Game Game { get; set; }

        public async Task Start(IServiceProvider serviceProvider, IGameNotificationManager gameNotificationManager)
        {
            Game.SetServiceProvider(serviceProvider);
            Game.SetNotificationManager(gameNotificationManager);
            Running = true;
            DateTime previouseLoopTime = DateTime.Now;
            DateTime time;while (Running)
            {
                time = DateTime.Now;
                if ((time - previouseLoopTime).TotalMilliseconds > LoopIntervalMiliseconds)
                {
                    previouseLoopTime = time;
                    await Game.Instance.Update();
                }
            }
        }

        public void Stop()
        {
            Running = false;
        }

    }
}