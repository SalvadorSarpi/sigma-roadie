using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sigma.Roadie.Domain;
using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sigma.Roadie.Server.Orquestrator
{

    public class OrchestratorHub : Hub<IOrchestratorHub>
    {

        ILogger<OrchestratorHub> log;

        public OrchestratorHub(ILogger<OrchestratorHub> log)
        {
            this.log = log;
        }

        public Task StatusUpdate(string json)
        {
            return Clients.Others.StatusUpdate(json);
        }


        public override Task OnConnectedAsync()
        {
            log.LogInformation("Cliente conectado.");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            log.LogInformation("Cliente desconectado.");
            return base.OnDisconnectedAsync(exception);
        }

    }

}
