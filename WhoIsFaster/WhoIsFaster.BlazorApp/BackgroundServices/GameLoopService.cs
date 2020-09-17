using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WhoIsFaster.BlazorApp.GameDefinitions;
using WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagerInterfaces;
using WhoIsFaster.Domain.Interfaces;

namespace WhoIsFaster.BlazorApp.BackgroundServices
{
    public class GameLoopService : IHostedService
    {
        public bool Running { get; private set; }
        public int LoopIntervalMiliseconds { get; private set; }
        private readonly IServiceProvider _serviceProvider;
        private GameLoop _gameLoop;

        public GameLoopService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            LoopIntervalMiliseconds = int.Parse(configuration["LoopIntervalMiliseconds"]);
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            IUnitOfWork unitOfWork = _serviceProvider.CreateScope().ServiceProvider.GetService<IUnitOfWork>();
            IGameNotificationManager gameNotificationManager = _serviceProvider.GetService<IGameNotificationManager>();
            _gameLoop = new GameLoop(100);
            Thread t = new Thread(() => _gameLoop.Start(unitOfWork, gameNotificationManager).ConfigureAwait(false));
            t.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _gameLoop?.Stop();
            return Task.CompletedTask;
        }
    }
}