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

namespace Sigma.Roadie.MediaPlayerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILocalLogger
    {

        public MainWindow()
        {
            InitializeComponent();

            txt.Text = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + Environment.NewLine + Environment.NewLine;
        }


        public void LogMessage(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                txt.Text += message + Environment.NewLine;
            });
        }

    }
}
