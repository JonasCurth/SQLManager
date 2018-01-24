using SQLogin.View;
using SQLogin.Viewmodel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SQLogin
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void OnApplicationStart(object sender, StartupEventArgs e)
        {
            LoginViewmodel vm = new LoginViewmodel();
            LoginView view = new LoginView();

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            view.DataContext = vm;
            this.MainWindow = view;
            view.ShowDialog();

        }
    }
}
