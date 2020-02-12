using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Sigma.Roadie.Domain.DataModels;
using Sigma.Roadie.Domain.DataModels.Enums;

namespace Sigma.Roadie.AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        HubConnection hub;
        MediaPlayer mediaPlayer;

        public MainWindow(MediaPlayer mediaPlayer)
        {
            InitializeComponent();

            this.mediaPlayer = mediaPlayer;

            hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/orchestratorhub")
                .WithAutomaticReconnect()
                .Build();

            hub.KeepAliveInterval = TimeSpan.FromSeconds(3);

            hub.StartAsync();

            hub.On<string>("PlayMedia", PlayMedia);
            hub.On<Guid>("StopMedia", mediaPlayer.StopMedia);
            hub.On("StopAll", mediaPlayer.StopAll);

            txt.Text = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + Environment.NewLine + Environment.NewLine;
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
                LogMessage("ERROR: " + e.Message);
            }
        }


        public void LogMessage(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                txt.Text += message + Environment.NewLine;
            });
        }
        

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await hub.StopAsync();
        }

    }
}
