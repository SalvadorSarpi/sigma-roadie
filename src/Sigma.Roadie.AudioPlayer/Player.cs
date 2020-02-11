using NAudio.Wave;
using Sigma.Roadie.Domain.Models;
using System;
using System.Collections.Generic;
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

            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += (e, r) =>
            {
                
            };

            audioFile = new AudioFileReader(model.LocalUri);
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
                }, null, model.PlayAt.Milliseconds, Timeout.Infinite);
            }
        }


        public void Stop()
        {
            timer = null;
            outputDevice.Stop();
        }

    }

}
