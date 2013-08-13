using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GizMaker.classes
{
    class database
    {
        public static string getConnectionString()
        {
            string strConnection = string.Empty;

            strConnection = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "data\\GizMaker.accdb";

            return strConnection;
        }
    }
}
