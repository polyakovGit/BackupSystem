using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using System.Threading;
namespace PgStore
{
    public static class Control
    {
        public static event System.EventHandler ResultChanged;
        public static event System.EventHandler Terminated;
        const string PGPASSWORD = "PGPASSWORD";
        const string PGHOST = "PGHOST";
        const string PGDATABASE = "PGDATABASE";
        const string PGUSER = "PGUSER";
        const string PGPORT = "PGPORT";
        static string dbName = "";
        static string _result = "";

        #region Public members
        public static Npgsql.NpgsqlConnection Cn = new NpgsqlConnection();
        #endregion

        #region Public methods
        public static void Backup(string directoryPath, string filename, bool createDirectory)
        {
            try
            {
                string str = "";
                if (CurrentConfig == null)
                    throw new Exception("missing config");
                if (!CurrentConfig.AllISOK(ref str))
                    throw new Exception(str);
                if (directoryPath == "")
                    throw new Exception("invalid directory path");
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    if (createDirectory)
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(directoryPath);
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                    else
                        throw new Exception("invalid directoru path");

                }

                if (filename == "")
                    throw new Exception("filename could not be empty");
                if (!filename.EndsWith(".backup"))
                    filename = filename += ".backup";
                string cmd = "-h " + CurrentConfig.ServerName +
                    " -p " + CurrentConfig.Port +
                    " -U " + CurrentConfig.UserName +
                    " -F c " + GetOptions(CurrentConfig.Parametres) + " -v -f " + directoryPath + "\\" + filename + " " +
                    CurrentConfig.DataBase;

                ExecuteCommand("pg_dump", cmd, CurrentConfig);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static void Restaure(string filePath, bool ReplaceExistant)
        {
            try
            {
                string str = "";
                if (CurrentConfig == null)
                    throw new Exception("missing config");
                if (!CurrentConfig.AllISOK(ref str))
                    throw new Exception(str);
                if (filePath == "")
                    throw new Exception("invalid file path");
                if (!System.IO.File.Exists(filePath))
                {
                    throw new Exception("invalid file path");
                }

                if (ReplaceExistant)
                {
                    dbName = CurrentConfig.DataBase;
                    CreateNewDatabase();
                }
                string cmd = "-h " + CurrentConfig.ServerName + " -p " + CurrentConfig.Port +
                    " -U " + CurrentConfig.UserName + " -d " +
                    CurrentConfig.DataBase + " -v " + filePath;
                if (ReplaceExistant)
                    Terminated += Control_Terminated;
                Result += "\nStart Restore";
                ExecuteCommand("pg_restore", cmd, CurrentConfig);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> GetDataBases()
        {

            try
            {
                Cn.ConnectionString = GetSqlConnexionString();
                NpgsqlCommand cmd = new NpgsqlCommand("select datname from pg_database");
                cmd.Connection = Cn;
                Cn.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<string> lst = new List<string>();
                while (reader.Read())
                {
                    if (reader[0] != null)
                        lst.Add(reader[0].ToString());
                }
                return lst;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                Cn.Close();
            }
        }

        public static string GetSqlConnexionString()
        {
            if (CurrentConfig == null)
                throw new Exception("Config not set");
            return @"Server=" + CurrentConfig.ServerName + ";" +
                     "Database=" + CurrentConfig.DataBase + ";" +
                    "User ID=" + CurrentConfig.UserName + ";" +
                    "Password=" + CurrentConfig.Password + ";" +
                    "Port =" + CurrentConfig.Port.ToString() + ";";
        }

        public static void CreateNewDatabase()
        {
            Npgsql.NpgsqlConnection cc = new NpgsqlConnection();
            cc.ConnectionString = @"Server=" + CurrentConfig.ServerName + ";" +
    "User ID=" + CurrentConfig.UserName + ";" +
    "Password=" + CurrentConfig.Password + ";" +
    "Port =" + CurrentConfig.Port.ToString() + ";";
            try
            {

                Result += "\nStoping database process...";
                ///Permet de déconnecter les bases de données
                string req = "SELECT procpid, (SELECT pg_terminate_backend(procpid)) as killed from pg_stat_activity   WHERE current_query LIKE '<IDLE>'";
                NpgsqlCommand cmd = new NpgsqlCommand(req);
                cmd.Connection = cc;
                cc.Open();
                cmd.ExecuteNonQuery();

                Result += "\nDeleting original database...";
                req = "DROP DATABASE " + CurrentConfig.DataBase;
                cmd.CommandText = req;
                cmd.ExecuteNonQuery();

                Result += "\nOriginal database deleted...";
                cmd = new NpgsqlCommand("create database " + CurrentConfig.DataBase);
                cmd.Connection = cc;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "ALTER DATABASE " + CurrentConfig.DataBase + " SET bytea_output='escape'";
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                cc.Close();
            }

        }
        #endregion

        #region Private methods
        static string GetOptions(StoreParameters parametres)
        {
            string ss = "";
            ss += parametres.DataOnlyCode;
            ss += parametres.BlobsCode;
            ss += parametres.CleanCode;
            ss += parametres.OidsCode;
            ss += parametres.NoOwnerCode;
            ss += parametres.SchemaOnlyCode;
            ss += parametres.NoPrivilegesCode;
            return ss;
        }
        private static void ExecuteCommand(string cmd, string commandSentence, Config cnf)
        {
            try
            {
                _result = "";
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                info.FileName = cmd + ".exe ";
                info.Arguments = commandSentence;
                info.CreateNoWindow = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.UseShellExecute = false;
                try { info.EnvironmentVariables.Add(PGHOST, cnf.ServerName); }
                catch (Exception) { }
                try { info.EnvironmentVariables.Add(PGDATABASE, cnf.DataBase); ;}
                catch (Exception) { }
                try { info.EnvironmentVariables.Add(PGUSER, cnf.UserName); ;}
                catch (Exception) { }
                try { info.EnvironmentVariables.Add(PGPASSWORD, cnf.Password); ;}
                catch (Exception) { }
                try { info.EnvironmentVariables.Add(PGPORT, cnf.Port.ToString()); ;}
                catch (Exception) { }

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = info;
                proc.Start();

                CancellationTokenSource cTokenSource = new CancellationTokenSource();
                CancellationToken cToken = cTokenSource.Token;


                if (cmd == "pg_dump")
                {
                    //Result = await proc.StandardError.ReadToEndAsync();
                    Result = Task.Factory.StartNew(() => proc.StandardError.ReadToEnd(), cToken).Result;
                    proc.WaitForExit();
                    if (proc.ExitCode == 0)
                        Result += "\nBackup terminated successfully";
                    else
                        Result += "\nError Occured";

                }
                else
                    Result = Task.Factory.StartNew(() => proc.StandardError.ReadToEnd(), cToken).Result;
                //Result = await proc.StandardError.ReadToEndAsync();

                if (Terminated != null)
                    Terminated(proc.ExitCode, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Events
        static void Control_Terminated(object sender, EventArgs e)
        {
            CurrentConfig.DataBase = dbName;
            Result += "Restore terminated";
        }
        #endregion

        #region Public properties
        public static string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                if (ResultChanged != null) ResultChanged(value, null);
            }
        }
        public static Config CurrentConfig { get; set; }
        #endregion
    }
}
