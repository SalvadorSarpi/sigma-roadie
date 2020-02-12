using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Sigma.Roadie.Domain.DataModels;

namespace Sigma.Roadie.AudioPlayer
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {

        public MediaFile CurrentMediaFile { get; set; }

        public bool IsBusy { get; set; }


        public VideoPlayer()
        {
            InitializeComponent();
        }



        public void PlayVideo(MediaFile model)
        {
            string fileUri = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "mediafiles", model.LocalUri);

            if (!File.Exists(fileUri)) return;

            StopVideo();

            CurrentMediaFile = model;

            Dispatcher.Invoke(() =>
            {
                video.Source = new Uri(fileUri);
                video.Volume = 0;
            });
        }


        public void StopVideo()
        {
            Dispatcher.Invoke(() =>
            {
                video.Source = null;
            });
        }


        public void StopMedia(Guid mediaFileId)
        {
            if (mediaFileId == CurrentMediaFile.MediaFileId)
            {
                StopVideo();
            }
        }


    }
}
