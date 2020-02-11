using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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


        public void PlayScene(string json)
        {
            //var json = JsonConvert.SerializeObject(scene);
            Clients.All.SendAsync("PlayScene", json);
        }


        public void StopVideo()
        {
            Clients.All.SendAsync("StopVideo");
        }

        public void StopAudio()
        {
            Clients.All.SendAsync("StopAudio");
        }


        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            //Clients.All.SendAsync("broadcastMessage", "system", $"{Context.ConnectionId} left the conversation");
            return base.OnDisconnectedAsync(exception);
        }
    }

}
