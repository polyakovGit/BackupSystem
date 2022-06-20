using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PgStore
{
    /// <summary>
    /// This class is used to indicate parameters used to launch pg_dump aund pg_restore commnds
    /// the parameters are initialized with the most common parameters
    /// its could be changed if necessary to do a specific tasks
    /// </summary>
    public class StoreParameters
    {
        public StoreParameters()
        {
            DataOnly = false;
            Blobs = true;
            Clean = false;      
            Oids = false;
            NoOwner = false;
            SchemaOnly = false;
            NoPrivileges = false;
    
        }
        public bool DataOnly { get; set; }
        public bool Blobs { get; set; }
        public bool Clean { get; set; }    
        public bool Oids { get; set; }
        public bool NoOwner { get; set; }
        public bool SchemaOnly { get; set; }
        public bool NoPrivileges { get; set; }
 

        public string DataOnlyCode { get { if (DataOnly)  return " -a"; return ""; } }
        public string BlobsCode { get { if (Blobs) return " -b"; return ""; } }
        public string CleanCode { get { if (Clean && !DataOnly)   return " -c"; return ""; } } 
        public string OidsCode { get { if (Oids) return " -o"; return ""; } }
        public string NoOwnerCode { get { if (NoOwner) return " -O"; return ""; } }
        public string SchemaOnlyCode { get { if (SchemaOnly) return " -s"; return ""; } }
        public string NoPrivilegesCode { get { if (NoPrivileges) return " -x"; return ""; } }
    }
}
