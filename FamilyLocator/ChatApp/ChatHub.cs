using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace FamilyLocator.ChatApp
{
    public class ChatHub
    {
        private IHubProxy hub;
        private HubConnection hubConnection;

        public ChatHub()
        {
            hubConnection = new HubConnection("https://chatapp20190804101109.azurewebsites.net/signalr", useDefaultUrl: false);
            IHubProxy chatHubApiProxy = hubConnection.CreateHubProxy("ChatHubApi");
            hub = chatHubApiProxy;
            RegisterEvents();
            hubConnection.Start().Wait();
        }

        private void RegisterEvents()
        {
            hub.On<string, string>("sendMessage", onData: async (message, sender) =>
            {
                var messageActivity = new MessageActivity();
                messageActivity.OnMessageReceived(message, sender);
            });
        }

        public async Task SendMessage(string message, string toConnection)
        {
            await hubConnection.Start();
            await hub.Invoke("Send", message, toConnection);
        }
    }
}