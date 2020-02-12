using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sigma.Roadie.MediaPlayerClient
{

    public class HubClient
    {

        HubConnection hub;
        MediaPlayer mediaPlayer;
        ILocalLogger log;

        public HubClient(MediaPlayer mediaPlayer, ILocalLogger log)
        {
            this.mediaPlayer = mediaPlayer;
            this.log = log;

            hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/orchestratorhub")
                .WithAutomaticReconnect()
                .Build();

            hub.KeepAliveInterval = TimeSpan.FromSeconds(3);

            hub.On<string>("PlayMedia", PlayMedia);
            hub.On<Guid>("StopMedia", mediaPlayer.StopMedia);
            hub.On("StopAll", mediaPlayer.StopAll);

            //hub.Closed += Hub_Closed;

            hub.StartAsync();
        }



        void PlayMedia(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                MediaFile media = JsonConvert.DeserializeObject<MediaFile>(json);

                mediaPlayer.PlayMedia(media);
            }
            catch (Exception e)
            {
                log.LogMessage("ERROR: " + e.Message);
            }
        }


        Task StopAsync()
        {
            return hub.StopAsync();
        }


    }

}
