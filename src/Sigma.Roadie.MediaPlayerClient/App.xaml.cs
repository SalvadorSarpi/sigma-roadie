using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sigma.Roadie.MediaPlayerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();


            _ = ServiceProvider.GetRequiredService<HubClient>();

            var mainWindow = (MainWindow)ServiceProvider.GetRequiredService<ILocalLogger>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILocalLogger, MainWindow>();
            services.AddSingleton(typeof(VideoPlayer));
            services.AddSingleton(typeof(MediaPlayer));
            services.AddSingleton(typeof(HubClient));
        }
    }
}
