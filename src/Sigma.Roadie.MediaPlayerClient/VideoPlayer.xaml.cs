using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Sigma.Roadie.Domain.DataModels;
using Sigma.Roadie.Domain.Models;

namespace Sigma.Roadie.MediaPlayerClient
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {

        ILocalLogger log;

        public MediaFile CurrentMediaFile { get; set; }

        public bool IsBusy { get; set; }

        bool IsFullScreen { get; set; } = false;


        public VideoPlayer(ILocalLogger log)
        {
            InitializeComponent();

            this.log = log;

            video.MediaEnded += (e, r) =>
              {
                  StopVideo();
              };

            LoadDefaultVideo();
        }


        public MediaFileStatus GetPlayingMediaStatus()
        {
            if (IsBusy == false || CurrentMediaFile != null) return null;

            var status = new MediaFileStatus()
            {
                MediaFileId = CurrentMediaFile.MediaFileId,
                PlayingFor = video.Position,
                PlaysIn = TimeSpan.Zero
            };

            return status;
        }


        void LoadDefaultVideo()
        {
            string fileUri = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "mediafiles", "default.mp4");

            if (!File.Exists(fileUri)) return;

            defaultVideo.Source = new Uri(fileUri);
            defaultVideo.Volume = 0;
        }


        DispatcherTimer dt;
        public void PlayVideo(MediaFile model)
        {
            string fileUri = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "mediafiles", model.LocalUri);
            if (!File.Exists(fileUri)) return;

            StopVideo();

            CurrentMediaFile = model;
            IsBusy = true;


            if (model.PlayAt.HasValue == false || model.PlayAt?.Ticks == 0)
            {
                PlayNow();
            }
            else if (model.PlayAt.Value.Ticks > 0)
            {
                if (dt != null) dt.Stop();

                dt = new DispatcherTimer(model.PlayAt.Value, DispatcherPriority.Normal, (ee, rr) =>
                {
                    PlayNow();
                    dt.Stop();
                }, this.Dispatcher);

                /*
                var timer = new Timer(e =>
                {
                    PlayNow();
                }, null, Convert.ToInt32(model.PlayAt.TotalMilliseconds), Timeout.Infinite);
                */
            }
        }


        void PlayNow()
        {
            if (CurrentMediaFile == null) return;

            string fileUri = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "mediafiles", CurrentMediaFile.LocalUri);
            if (!File.Exists(fileUri)) return;

            Dispatcher.Invoke(() =>
            {
                video.Source = new Uri(fileUri);
                video.Volume = 0;

                DoubleAnimation animation = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(3)));
                video.BeginAnimation(MediaElement.OpacityProperty, animation);
            });
        }


        public void StopVideo()
        {
            Dispatcher.Invoke(() =>
            {
                if (dt != null) dt.Stop();
                DoubleAnimation animationdef = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(2)));
                defaultVideo.BeginAnimation(MediaElement.OpacityProperty, animationdef);
                DoubleAnimation animation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(2)));
                video.BeginAnimation(MediaElement.OpacityProperty, animation);

                video.Source = null;
                CurrentMediaFile = null;
                IsBusy = false;
            });
        }


        public void StopMedia(Guid mediaFileId)
        {
            if (mediaFileId == CurrentMediaFile?.MediaFileId)
            {
                log.LogMessage($"Deteniendo {CurrentMediaFile.Name}");
                StopVideo();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F)
            {
                IsFullScreen = !IsFullScreen;

                if (IsFullScreen)
                {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Normal;
                }
            }
        }
    }
}
