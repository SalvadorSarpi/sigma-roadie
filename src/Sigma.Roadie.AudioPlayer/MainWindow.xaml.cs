using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Sigma.Roadie.Domain.DataModels;
using Sigma.Roadie.Domain.Models;

namespace Sigma.Roadie.AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        HubConnection hub;

        public MainWindow()
        {
            InitializeComponent();

            hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/orchestratorhub")
                .WithAutomaticReconnect()
                .Build();

            hub.KeepAliveInterval = TimeSpan.FromSeconds(3);

            hub.StartAsync();

            hub.On<SceneModel>("PlayScene", PlayScene);
            hub.On("StopAudio", () => PlayScene(null));
        }


        void PlayScene(SceneModel scene)
        {
            if (scene != null)
            {
                LogMessage("Comenzar: " + scene.Name);
            }
            else
            {
                LogMessage("Detener");
            }
        }


        void LogMessage(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                txt.Text += message + Environment.NewLine;
            });
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            /*

            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                //outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(@"D:\Media\Música\MP3\Pink Floyd\1987 - A Momentary Lapse Of Reason\07 - A New Machine (Part 1).mp3");
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();*/
        }
    }
}
