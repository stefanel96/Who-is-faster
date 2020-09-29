using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.BlazorApp.EventServices;
using WhoIsFaster.BlazorApp.GameServices;
using WhoIsFaster.BlazorApp.StringEncryption;
using WhoIsFaster.BlazorApp.ViewModels;
using WhoIsFaster.Domain.Entities.RoomAggregate;
using WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagerInterfaces;

namespace WhoIsFaster.BlazorApp.Pages
{
    public partial class PartyRoom : IAsyncDisposable
    {
        public string Title { get; set; } = "Party Room";
        [Parameter]
        public string RoomId { get; set; }
        public bool FirstRender { get; set; }
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
        [Inject]
        IJSRuntime JSRuntime { get; set; }
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
        public DateTime StartClicked { get; set; }
        public int ChangeDurationOfToast { get; set; }
        public bool DisableStartButton { get; set; }
        public string JoinLink { get; set; }
        public bool EnableInitialize { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            string userName = HttpContextAccessor.HttpContext.User.Identity.Name;
            RoomResponseDTO roomResponse;
            if (RoomId == null)
            {
                roomResponse = await RoomService.CreateAndJoinPartyRoomAsync(userName);
                if (roomResponse.IsNew)
                {
                    await GameService.AddRoomToGame(roomResponse.Room.Id);
                }
                else
                {
                    if (roomResponse.Room.RoomType != RoomType.Party.ToString())
                    {
                        if (roomResponse.Room.RoomType == RoomType.Practice.ToString())
                        {
                            NavigationManager.NavigateTo("/practice");
                        }
                        else
                        {
                            NavigationManager.NavigateTo("/room");
                        }
                    }
                }
            }
            else
            {
                var urlDecoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(RoomId));
                var decrypted = StringCipher.Decrypt(urlDecoded, "gsdoifjdsoi");
                int.TryParse(decrypted, out var roomId);
                roomResponse = await RoomService.JoinPartyRoomAsync(userName, roomId);
                if (roomResponse.Room.RoomType != RoomType.Party.ToString())
                {
                    if (roomResponse.Room.RoomType == RoomType.Practice.ToString())
                    {
                        NavigationManager.NavigateTo("/practice");
                    }
                    else
                    {
                        NavigationManager.NavigateTo("/public");
                    }
                }
            }
            ChangeDurationOfToast = 5;
            ShowToastForStartingGame = true;
            Room = new RoomVM(roomResponse.Room);
            ShowToastForOnePlayer = Room.RoomPlayers.Count() == 1 ? true : false;
            Username = userName;
            RoomPlayer = Room.RoomPlayers.FirstOrDefault(rp => rp.UserName == userName);
            if (RoomPlayer.IsRoomAdmin)
            {
                var encrypted = StringCipher.Encrypt(Room.Id + "", "gsdoifjdsoi");
                var urlEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(encrypted));
                JoinLink = NavigationManager.Uri + "/" + urlEncoded;
                JoinLink = HtmlEncoder.Default.Encode(JoinLink);
            }
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
                var player = room.RoomPlayers.FirstOrDefault(rp => rp.UserName == Username);

                if (player != null)
                {
                    Room = new RoomVM(room);
                    RoomPlayer = new RoomPlayerVM(player);
                    if (Room.HasFinished)
                    {
                        hubConnection.DisposeAsync();
                    }
                    if (CurrentTextIndex == 0)
                    {
                        CurrentTextIndex = RoomPlayer.CurrentTextIndex;
                    }
                    StateHasChanged();
                }
            });

            hubConnection.On<string>("LeaveRoom", (message) =>
            {
                NavigationManager.NavigateTo("/");
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
            StartClicked = DateTime.Now;
            ChangeDurationOfToast = 1;
            DisableStartButton = true;
            await RoomService.StartRoom(Room.Id);
        }

        public async Task LeaveRoom()
        {
            await RoomService.LeavePartyRoom(Room.Id);
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

        public async void CopyToClipboard()
        {
            await JSRuntime.InvokeVoidAsync("clipboardCopy.copyText", JoinLink);
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