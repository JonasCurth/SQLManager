using SQLogin.Viewmodel;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SQLogin.View
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class LoginView : ChromelessWindow
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void OnAbortClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            ((LoginViewmodel)this.DataContext).ClosingRequest += (s, ee) =>
            {
                this.DialogResult = true;
                this.Close();
            };
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((LoginViewmodel)this.DataContext).Connection.Password = this.Password.Password;
        }
    }
}
