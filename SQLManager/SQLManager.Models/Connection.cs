using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SQLManager.Models
{
    public class Connection : INotifyPropertyChanged
    {

        #region fields
        private string servername;
        private string database;
        private string username;
        private string password;

        private ObservableCollection<String> databases;
        private bool isSQLAuth;

        private static Connection instance;
        #endregion

        #region constructor
        public Connection() { }
        #endregion

        #region public properties

        public static Connection Default
        {
            get
            {
                if(instance == null)
                {
                    instance = new Connection();
                }

                return instance;
            }
        }

        public string Servername
        {
            get { return this.servername; }
            set
            {
                this.servername = value;
                this.databases = null;
                this.OnPropertyChanged();
            }
        }

        public string Database
        {
            get { return this.database; }
            set
            {
                this.database = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<String> Databases
        {
            get { return this.databases; }
            set
            {
                this.databases = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get { return this.username; }
            set
            {
                this.username = value;
                this.databases = null;
                this.OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return this.password; }
            set
            {
                this.password = value;
                this.databases = null;
                this.OnPropertyChanged();
            }
        }
        
        public bool IsSQLAuth
        {
            get { return this.isSQLAuth; }
            set
            {
                this.isSQLAuth = value;
                this.databases = null;
                this.OnPropertyChanged();
            }
        }
        #endregion

        #region private methods

        private string BuildConnectionString()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.DataSource = this.Servername;
            connectionStringBuilder.IntegratedSecurity = !this.IsSQLAuth;

            if (!connectionStringBuilder.IntegratedSecurity)
            {
                connectionStringBuilder.UserID = this.Username;
                connectionStringBuilder.Password = this.Password;
            }

            if (null != this.Database)
            {
                connectionStringBuilder.InitialCatalog = this.Database;
            }

            return connectionStringBuilder.ConnectionString;
        }



        #endregion


        #region public methods
        public bool TestConnection()
        {
            string connectionString = BuildConnectionString();

            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                success = true;
            }

            return success;
        }

        public List<string> GetAllDatabases()
        {
            List<String> result = null;

            string connectionString = BuildConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM sys.databases", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result = new List<string>();
                        while (reader.Read())
                        {
                            result.Add(Convert.ToString(reader[0]));
                        }
                    }
                }
                connection.Close();
            }

            return result;
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
