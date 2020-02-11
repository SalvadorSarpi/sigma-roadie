using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sigma.Roadie.Server.Orquestrator
{

    public class OrchestratorHub : Hub
    {

        public override Task OnConnectedAsync()
        {
            //Clients.All.SendAsync("broadcastMessage", "system", $"{Context.ConnectionId} joined the conversation");
            return base.OnConnectedAsync();
        }


        public void PlayScene(Guid sceneId)
        {
            Clients.All.SendAsync("PlayScene", sceneId);
        }


        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            //Clients.All.SendAsync("broadcastMessage", "system", $"{Context.ConnectionId} left the conversation");
            return base.OnDisconnectedAsync(exception);
        }
    }

}
