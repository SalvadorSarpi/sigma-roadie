﻿using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sigma.Roadie.Domain.DataModels.Enums;
using Sigma.Roadie.Domain.Models;

namespace Sigma.Roadie.MediaPlayerClient
{
    public class MediaPlayer
    {

        VideoPlayer video;
        ILocalLogger log;
        AppSettings settings;

        List<AudioPlayer> audios;

        public MediaPlayer(VideoPlayer video, ILocalLogger log, AppSettings settings)
        {
            this.video = video;
            this.log = log;
            this.settings = settings;
            audios = new List<AudioPlayer>();

            if (settings.VideoPlayer)
            {
                video.Show();
            }
        }


        public void PlayMedia(MediaFile media)
        {
            log.LogMessage($"Reproducir: {media.Name}");

            if (media.TypeEnum == MediaFileType.Audio && settings.AudioPlayer)
            {
                var player = GetAvailableAudioPlayer();
                player.PlayAudio(media);
            }
            else if (media.TypeEnum == MediaFileType.Video && settings.VideoPlayer)
            {
                video.PlayVideo(media);
            }
        }

        public void StopMedia(Guid mediaFileId)
        {
            audios.ForEach(q => q.StopMedia(mediaFileId));
            video.StopMedia(mediaFileId);
        }

        public void StopAll()
        {
            log.LogMessage($"Detener todo.");

            audios.ForEach(q => q.StopAudio());
            video.StopVideo(true);
        }


        public List<MediaFileStatus> GetPlayingMediaStatus()
        {
            List<MediaFileStatus> playing = new List<MediaFileStatus>();

            foreach(var audio in audios)
            {
                var status = audio.GetPlayingMediaStatus();
                if (status != null) playing.Add(status);
            }

            var statusv = video.GetPlayingMediaStatus();
            if (statusv != null) playing.Add(statusv);

            return playing;
        }

        AudioPlayer GetAvailableAudioPlayer()
        {
            var available = (from p in audios where p.IsBusy == false select p).FirstOrDefault();

            if (available == null)
            {
                available = new AudioPlayer(log);
                audios.Add(available);

                log.LogMessage($"Creando audio player.");
            }

            return available;
        }


    }
}
