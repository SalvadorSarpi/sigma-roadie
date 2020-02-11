using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Sigma.Roadie.Domain.DataModels;
using Sigma.Roadie.Domain.Models;
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

            /*
            hub.On<string>("PlayScene", UpdateEventHandler);
            hub.On("StopAudio", () => PlayScene(null));*/
        }


        public async Task SendPlaySceneCommand(Scene scene)
        {
            SceneModel model = null;

            if (scene != null)
            {
                model = new SceneModel()
                {
                    SceneId = scene.SceneId,
                    Name = scene.Name,
                    MediaFiles = (from p in scene.MediaFile
                                  select new MediaFileModel()
                                  {
                                      MediaFileId = p.MediaFileId,
                                      LocalUri = p.LocalUri,
                                      PlayAt = p.PlayAt,
                                      Type = p.Type
                                  }).ToList()
                };
            }

            var json = JsonConvert.SerializeObject(model);
            await hub.InvokeAsync<string>("PlayScene", json).ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {
                    
                }
            });
        }


        public async Task StopVideo()
        {
            await hub.InvokeAsync("StopVideo").ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {

                }
            });
        }


        public async Task StopAudio()
        {
            await hub.InvokeAsync("StopAudio").ContinueWith(task1 =>
            {
                if (task1.IsFaulted)
                {

                }
            });
        }

    }

}
