using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sigma.Roadie.Server.Orquestrator
{

    public class OrchestratorClient
    {

        HubConnection hub;

        public OrchestratorClient()
        {
            RunAsync();
        }

        public async Task RunAsync()
        {
            hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/orchestratorhub")
                .WithAutomaticReconnect()
                .Build();

            hub.KeepAliveInterval = TimeSpan.FromSeconds(3);

            await hub.StartAsync();
        }


        public async Task SendPlaySceneCommand(Guid sceneId)
        {
            await hub.InvokeAsync<string>("PlayScene", sceneId).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    
                }
            });
        }

    }

}
