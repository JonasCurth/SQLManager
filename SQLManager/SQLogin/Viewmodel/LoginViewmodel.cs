using DidiDerDenker.BirdsEyeView.Command;
using SQLManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SQLogin.Viewmodel
{
    class LoginViewmodel : INotifyPropertyChanged
    {
        #region fields
        private Connection connection;

        private bool isEnabled;

        private DelegateCommand testConnectionCommand;
        private DelegateCommand connectCommand;
        #endregion

        #region constructor
        public LoginViewmodel() { }
                #endregion

        #region public properties
        public Connection Connection
        {
            get
            {
                if(this.connection == null)
                {
                    this.connection = new Connection();
                }

                return this.connection;
            }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set
            {
                this.isEnabled = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand TestConnectionCommand
        {
            get
            {
                if (null == this.testConnectionCommand)
                {
                    this.testConnectionCommand = new DelegateCommand(this.TestConnection, this.CanTestConnection);
                }

                return this.testConnectionCommand;
            }
        }

        public ICommand ConnectCommand
        {
            get
            {
                if (null == this.connectCommand)
                {
                    this.connectCommand = new DelegateCommand(this.Connect, this.CanConnect);
                }

                return this.connectCommand;
            }
        }
        #endregion

        #region private and protected methods
        private bool CanTestConnection()
        {
            return !String.IsNullOrWhiteSpace(this.Connection.Servername) &&
                    (!this.Connection.IsSQLAuth ||
                        (!String.IsNullOrWhiteSpace(this.Connection.Username) && !String.IsNullOrWhiteSpace(this.Connection.Password)));
        }

        private bool CanConnect()
        {
            return this.Connection.Database != null;
        }

        private void Connect()
        {
            bool success = this.Connection.TestConnection();

            if (!success)
            {
                //App.AppClient.ShowErrorWindow(Errorcode.DBCONNECTIONERROR, null);
            }

            this.OnClosingRequest();
        }

        private void TestConnection()
        {
            this.Connection.Databases = null;
            this.Connection.Database = null;
            try
            {
                this.Connection.Databases = new ObservableCollection<string>(this.Connection.GetAllDatabases());
            }
            catch (Exception ex)
            {
                //App.AppClient.ShowErrorWindow(Errorcode.DBCONNECTIONERROR, ex);
            }
            finally
            {
                this.IsEnabled = (this.connection.Databases != null) ? true : false;
                this.Connection.Database = this.Connection.Databases[0];
            }
        }
        #endregion

        #region public methods

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ClosingRequest;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnClosingRequest()
        {
            this.ClosingRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
