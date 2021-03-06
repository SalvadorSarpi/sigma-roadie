﻿using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Sigma.Roadie.Domain.DataModels;
using Sigma.Roadie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sigma.Roadie.MediaPlayerClient
{

    public class HubClient
    {

        HubConnection hub;
        MediaPlayer mediaPlayer;
        ILocalLogger log;
        AppSettings settings;

        Timer statusTimer;

        public HubClient(MediaPlayer mediaPlayer, ILocalLogger log, AppSettings settings)
        {
            this.mediaPlayer = mediaPlayer;
            this.log = log;
            this.settings = settings;

            hub = new HubConnectionBuilder()
                .WithUrl($"http://{settings.HubEndpoint}/orchestratorhub")
                .WithAutomaticReconnect()
                .Build();

            hub.KeepAliveInterval = TimeSpan.FromSeconds(3);

            hub.On<string>("PlayMedia", PlayMedia);
            hub.On<Guid>("StopMedia", mediaPlayer.StopMedia);
            hub.On("StopAll", mediaPlayer.StopAll);

            hub.Reconnecting += (df) =>
              {
                  

                  return Task.CompletedTask;
              };

            hub.StartAsync();


            statusTimer = new Timer((e) =>
            {
                SendStatus();
            }, null, 1000, 1000);
        }


        void SendStatus()
        {
            if (hub == null || hub.State != HubConnectionState.Connected) return;

            MediaPlayerStatus status = new MediaPlayerStatus()
            {
                Hostname = Environment.MachineName,
                LocalDateTime = DateTime.Now,
                MediaFiles = mediaPlayer.GetPlayingMediaStatus()
            };

            var json = JsonConvert.SerializeObject(status);

            hub.InvokeAsync("StatusUpdate", json);
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


    }

}
