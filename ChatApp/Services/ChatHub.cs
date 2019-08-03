using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatApp.Services
{
    [HubName("ChatHubApi")]
    public class ChatHub: Hub
    {
        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            return base.OnConnected();
        }

        public void Send(string message, string toConnectionId)
        {
            var senderConnectionId = Clients.Caller.Connection.Identity; 
            Clients.Client(toConnectionId).sendMessage(message, senderConnectionId);
            Clients.Client(senderConnectionId).sendMessage(message, senderConnectionId);
        }

        public void SendGroup(string message, string groupName)
        {
            var senderConnectionId = Clients.Caller.ConnectionId;
            var otherConnectionIds = Clients.OthersInGroup(groupName);
            Clients.Clients(otherConnectionIds).sendMessage(message, senderConnectionId);
        }

        public void CreateGroup(string groupName, List<string> connectionIds)
        {
        }
    }
}