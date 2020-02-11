using NAudio.Wave;
using Sigma.Roadie.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Sigma.Roadie.AudioPlayer
{

    public class Player
    {

        WaveOutEvent outputDevice;
        AudioFileReader audioFile;

        MediaFileModel model;

        Timer timer;

        public Player(MediaFileModel model)
        {
            this.model = model;

            string fileUri = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "mediafiles", model.LocalUri);

            if (!File.Exists(fileUri)) return;


            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += (e, r) =>
            {
                
            };

            audioFile = new AudioFileReader(fileUri);
            outputDevice.Init(audioFile);

            if (model.PlayAt.Ticks == 0)
            {
                outputDevice.Play();
            }
            else
            {
                timer = new Timer(e =>
                {
                    outputDevice.Play();
                }, null, Convert.ToInt32(model.PlayAt.TotalMilliseconds), Timeout.Infinite);
            }
        }


        public void Stop()
        {
            timer = null;
            outputDevice.Stop();
        }

    }

}
