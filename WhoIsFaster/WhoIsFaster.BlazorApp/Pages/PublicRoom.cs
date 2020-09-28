using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.BlazorApp.EventServices;
using WhoIsFaster.BlazorApp.GameServices;
using WhoIsFaster.BlazorApp.ViewModels;
using WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagerInterfaces;

namespace WhoIsFaster.BlazorApp.Pages
{
    public partial class PublicRoom : IAsyncDisposable
    {
        public string Title { get; set; } = "Public Room";

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IRoomService RoomService { get; set; }

        [Inject]
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        [Inject]
        public IGameNotificationManager NotificationManager { get; set; }

        [Inject]
        public IGameService GameService { get; set; }

        [Inject]
        public IEventService EventService { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }
        public bool ShowToastForOnePlayer { get; set; }
        public bool ShowToastForStartingGame { get; set; }
        public string Input { get; set; } = "";
        public RoomVM Room { get; set; }
        public RoomPlayerVM RoomPlayer { get; set; }
        public string Username { get; set; }

        private HubConnection hubConnection;

        public int CorrectWordIndex { get; set; }
        public int IncorrectWordIndex { get; set; }
        public int CurrentTextIndex { get; set; }

        public bool EndOfText { get; set; }
        public int ChangeDurationOfToast { get; set; }
        protected override async Task OnInitializedAsync()
        {
            ChangeDurationOfToast = 5;
            ShowToastForStartingGame = true;
            string userName = HttpContextAccessor.HttpContext.User.Identity.Name;
            var roomResponse = await RoomService.JoinOrCreateRoomAsync(userName);
            if (roomResponse.IsNew)
            {
                await GameService.AddRoomToGame(roomResponse.RoomId);
            }
            Room = new RoomVM(await RoomService.GetRoomByUserNameAsync(userName));
            ShowToastForOnePlayer = Room.RoomPlayers.Count() == 1 ? true : false;
            Username = userName;
            RoomPlayer = Room.RoomPlayers.FirstOrDefault(rp => rp.UserName == userName);
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/whoIsFasterSignalRHub"), conf =>
                {
                    conf.HttpMessageHandlerFactory = (x) => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                    };
                })
                .Build();

            hubConnection.On<string>("ReceiveRoom", (roomObject) =>
            {
                RoomDTO room = JsonSerializer.Deserialize<RoomDTO>(roomObject);
                Room = new RoomVM(room);
                RoomPlayer = Room.RoomPlayers.FirstOrDefault(rp => rp.UserName == Username);
                if (Room.HasFinished)
                {
                    hubConnection.DisposeAsync();
                }
                StateHasChanged();
            });

            await hubConnection.StartAsync();

            await EventService.AddConnectionToSignalRGroup(hubConnection.ConnectionId, Room.Id.ToString());

        }

        public double EndingRoomSecondsReverse()
        {
            return Math.Round(Room.GameLengthSeconds + (Room.StartEventTime - DateTime.Now).TotalSeconds + Room.LengthOfStarting);
        }

        public int EndingRoomPercentage()
        {
            return (int)((Math.Round((DateTime.Now - Room.StartEventTime).TotalSeconds - Room.LengthOfStarting) / Room.GameLengthSeconds) * 100);
        }

        public int EndingRoomPercentageReverse()
        {
            return (int)((Math.Round(Room.GameLengthSeconds + (Room.StartEventTime - DateTime.Now).TotalSeconds + Room.LengthOfStarting) / Room.GameLengthSeconds) * 100);
        }

        public void OnInput()
        {
            CorrectWordIndex = GetCorrectlyTypedIndexOfWord();
            IncorrectWordIndex = Input.Trim().Length > CorrectWordIndex ? Input.Trim().Length : CorrectWordIndex;
            if (this.RoomPlayer.CurrentWord == this.Input.Trim() && Input[Input.Length - 1] == ' ')
            {
                GameService.UpdateRoomPlayerInput(Room.Id, RoomPlayer.UserName, Input);
                CurrentTextIndex += this.Input.Length;
                Input = "";
                CorrectWordIndex = 0;
                IncorrectWordIndex = 0;
                if (this.RoomPlayer.CurrentWord == Room.WordList[Room.WordList.Count - 1])
                {
                    CurrentTextIndex -= 1;
                    EndOfText = true;
                }
            }
        }

        public async Task StartRoom()
        {
            await RoomService.StartRoom(Room.Id);
        }

        public string addBGColor(int index)
        {
            switch (index)
            {
                case 1:
                    return "bg-success";
                case 2:
                    return "bg-warning";
                case 3:
                    return "bg-danger";
            }

            return "";
        }
        private int GetCorrectlyTypedIndexOfWord()
        {
            int n = RoomPlayer.CurrentWord.Length;
            int m = Input.Trim().Length;
            int i;
            for (i = 0; i < Math.Min(n, m); i++)
            {
                if (RoomPlayer.CurrentWord[i] != Input[i])
                    break;
            }
            return i;
        }

        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        }

    }
}