using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgStore
{
    public class Config
    {
        public Config()
        {
            Parametres = new StoreParameters();
            Port = 5432;
            ServerName = "localhost";
            UserName = "postgres";
            Password = "";
        }

        public string ServerName { get; set; }
        public string DataBase { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }

        public bool AllISOK(ref string message)
        {
            bool b = true;
            if (this.ServerName == "") { b = false; message = "missing server name"; }
            if (this.DataBase == "") { b = false; message = "Missing database"; }
            if (this.UserName == "") { b = false; message = "Missing username"; }
            return b;
        }
        public StoreParameters Parametres
        { get; set; }
    }
}