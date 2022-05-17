using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using StudyMaterialShare.Desktop.Models;
using StudyMaterialShare.Desktop.ViewModels;
using System;
using System.Windows;

namespace StudyMaterialShare.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            Startup += App_Startup;

            var services = new ServiceCollection();

            //Services
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IStudyMaterialShareApiService,StudyMaterialShareApiService>();

            //ViewModels
            services.AddTransient<BrowseViewModel>();
            services.AddTransient<SubjectTabViewModel>();

            services.AddTransient(s =>
            {
                var loginViewModel = new LoginViewModel(s.GetRequiredService<IStudyMaterialShareApiService>());

                loginViewModel.LoginSucceeded += LoginViewModel_LoginSucceeded;
                loginViewModel.MessageApplication += OnMessageApplication;

                return loginViewModel;
            });

            services.AddTransient(s =>
            {
                var mainViewModel = new MainViewModel(s, 
                    s.GetRequiredService<IStudyMaterialShareApiService>());

                mainViewModel.LogoutEvent += MainViewModel_LogoutEvent;
                mainViewModel.MessageApplication += OnMessageApplication;

                return mainViewModel;
            });
            
            //Windows
            services.AddTransient(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            services.AddTransient(s => new LoginWindow()
            {
                DataContext = s.GetRequiredService<LoginViewModel>(),
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        private void OnMessageApplication(object? sender, string e)
        {
            MessageBox.Show(e, "StudyMaterialShare", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModel_LogoutEvent(object? sender, EventArgs e)
        {
            var currentWindow = MainWindow;

            MainWindow = _serviceProvider.GetRequiredService<LoginWindow>();

            currentWindow.Close();

            MainWindow.Show();
        }

        private void LoginViewModel_LoginSucceeded()
        {
            /*MainWindow = _serviceProvider.GetService<MainWindow>();
            MainWindow?.Show();

            var loginWindow = _serviceProvider.GetService<LoginWindow>();
            loginWindow?.Close();*/

            var currentWindow = MainWindow;

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            currentWindow.Close();

            MainWindow.Show();
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            MainWindow.Show();
        }
    }
}
