using Microsoft.AspNetCore.SignalR;
using Sigma.Roadie.Domain.DataModels;
using Sigma.Roadie.Domain.Models;
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


        public bool PlayScene(SceneModel scene)
        {
            Clients.All.SendAsync("PlayScene", scene);

            return true;
        }


        public bool StopVideo()
        {
            Clients.All.SendAsync("StopVideo");

            return true;
        }

        public bool StopAudio()
        {
            Clients.All.SendAsync("StopAudio");

            return true;
        }


        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            //Clients.All.SendAsync("broadcastMessage", "system", $"{Context.ConnectionId} left the conversation");
            return base.OnDisconnectedAsync(exception);
        }
    }

}
