using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace DesktopClient
{
    class Database
    {
        private static Database instance;
        private Database() { }
        public static Database getInstance()
        {
            if (instance == null)
                instance = new Database();
            return instance;
        }
        public ServerConnection srvConn;
        public Server srv;
        public string serverName;
        public bool connectionStatus;

        public string FullBackup(string nameBase, string directory)
        {
            string fileName = string.Format($"{nameBase}_{DateTime.Now.ToString("dd.MM.yyyy_HH-mm-ss")}");
            string fullPath = directory + "\\" + fileName;
            string sqlExpression = $"BACKUP DATABASE [{nameBase}] " +
                                   $"TO DISK = N'{fullPath}' " +
                                   $"WITH FORMAT, NAME = N'{fileName} Database Full Backup'";
            return sqlExpression;
        }
    }
}
