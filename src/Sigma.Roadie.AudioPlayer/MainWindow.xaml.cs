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
using Sigma.Roadie.Domain.DataModels;
using Sigma.Roadie.Domain.Models;

namespace Sigma.Roadie.AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        HubConnection hub;

        List<Player> players = new List<Player>();

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

            txt.Text = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }


        void PlayScene(SceneModel model)
        {
            if (model != null)
            {
                LogMessage("Comenzar: " + model.Name);
                this.Dispatcher.Invoke(() =>
                {
                    PlayAudio(model);
                });
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


        private void PlayAudio(SceneModel model)
        {
            foreach (var file in model.MediaFiles)
            {
                players.Add(new Player(file));
            }
        }


        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await hub.StopAsync();
        }

    }
}
