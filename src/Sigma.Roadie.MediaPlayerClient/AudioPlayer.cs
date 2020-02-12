using NAudio.Wave;
using Sigma.Roadie.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Sigma.Roadie.AudioPlayer
{

    public class AudioPlayer
    {

        public MediaFile CurrentMediaFile { get; set; }

        public bool IsBusy { get; set; } = false;


        WaveOutEvent outputDevice;
        AudioFileReader audioFile;


        public AudioPlayer()
        {
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += (e, r) =>
            {
                IsBusy = false;
                CurrentMediaFile = null;
            };
        }


        public void PlayAudio(MediaFile model)
        {
            string fileUri = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "mediafiles", model.LocalUri);

            if (!File.Exists(fileUri)) return;

            this.CurrentMediaFile = model;
            IsBusy = true;

            audioFile = new AudioFileReader(fileUri);
            outputDevice.Init(audioFile);

            if (model.PlayAt.HasValue == false || model.PlayAt?.Ticks == 0)
            {
                PlayNow();
            }
            else if (model.PlayAt?.Ticks > 0)
            {
                var timer = new Timer(e =>
                {
                    PlayNow();
                }, null, Convert.ToInt32(model.PlayAt?.TotalMilliseconds), Timeout.Infinite);
            }
        }


        void PlayNow()
        {
            if (IsBusy == true)
            {
                outputDevice.Play();
            }
        }


        public void StopAudio()
        {
            outputDevice.Stop();
        }


        public void StopMedia(Guid mediaFileId)
        {
            if (mediaFileId == CurrentMediaFile?.MediaFileId)
            {
                StopAudio();
            }
        }

    }

}
