using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sigma.Roadie.Server.Orquestrator
{

    public class Orchestrator
    {

        public EventCallback ClientsStatusUpdated { get; set; }

        IHubContext<OrchestratorHub> hub;


        public Orchestrator(IHubContext<OrchestratorHub> hub)
        {
            this.hub = hub;
        }


        public async Task SendPlayMediaCommand(MediaFile media)
        {
            var json = JsonConvert.SerializeObject(media);
            await hub.Clients.All.SendAsync("PlayMediaCommand", json);
        }


        public async Task SendStopMediaCommand(Guid mediaFileId)
        {
            await hub.Clients.All.SendAsync("PlayMediaCommand", mediaFileId);
        }


        public async Task SendStopAllCommand()
        {
            await hub.Clients.All.SendAsync("StopAll");
        }



    }

}
