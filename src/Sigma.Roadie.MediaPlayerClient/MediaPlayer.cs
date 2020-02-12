using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sigma.Roadie.Domain.DataModels.Enums;

namespace Sigma.Roadie.MediaPlayerClient
{
    public class MediaPlayer
    {

        VideoPlayer video;
        ILocalLogger log;

        List<AudioPlayer> audios;

        public MediaPlayer(VideoPlayer video, ILocalLogger log)
        {
            this.video = video;
            this.log = log;
            audios = new List<AudioPlayer>();

            video.Show();
        }


        public void PlayMedia(MediaFile media)
        {
            log.LogMessage($"Reproducir: {media.Name}");

            if (media.TypeEnum == MediaFileType.Audio)
            {
                var player = GetAvailableAudioPlayer();
                player.PlayAudio(media);
            }
            else if (media.TypeEnum == MediaFileType.Video)
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
            video.StopVideo();
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
