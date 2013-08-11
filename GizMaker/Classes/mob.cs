using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using ADODB;

namespace GizMaker.classes
{
    class mob
    {
        #region "mob properties"
        // Properties
        public int mobID { get; set; }
        public int mobVNUM { get; set; }
        public string ShortDesc { get; set; }
        #endregion

        #region "mob methods"
        #endregion

        #region "mob public methods"
        // Get a mob by MobID. 
        public static mob GetMob(int MobID)
        {
            mob oMob = new mob();

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("mob");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [MobID], [VNUM], [ShortDesc] ";
                strSQL += " from   [Mob] ";
                strSQL += " where  MobID = " + MobID + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    oMob.mobID = (int)ds.Tables[0].Rows[0]["MobID"];
                    oMob.mobVNUM = (int)ds.Tables[0].Rows[0]["VNUM"];
                    oMob.ShortDesc = (string)ds.Tables[0].Rows[0]["ShortDesc"];
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return oMob;
        }

        // Get a collection of mobs in the room.
        public static mob[] GetRoomMobs(int MobAreaID, int intRoomID, int CoordX, int CoordY, int CoordZ)
        {
            mob[] mobs = new mob[250];

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("mobs");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;

                strSQL += " select  [MobID], [RoomID], [ShortDesc] ";
                strSQL += " from    [mob]  ";
                strSQL += " where   [MobID] in ( ";
                strSQL += "         select  [MobID] ";
                strSQL += "         from    [RoomMobs] ";
                strSQL += "         where   SpawnAreaID = " + MobAreaID + " ";
                strSQL += "                 and RoomNumber = " + intRoomID + " ";
                strSQL += "                 and CoordX = " + CoordX + " ";
                strSQL += "                 and CoordY = " + CoordY + " ";
                strSQL += "                 and CoordZ = " + CoordZ + " ";
                strSQL += "         ) ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    mob oMob = new mob();

                    oMob.mobID = (int)ds.Tables[0].Rows[iRow]["MobID"];
                    oMob.mobVNUM = (int)ds.Tables[0].Rows[iRow]["VNUM"];
                    oMob.ShortDesc = (string)ds.Tables[0].Rows[iRow]["ShortDesc"];

                    mobs[iRow] = oMob;

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return mobs;
        }

        // Get a dataset of mobs spawning in room.
        public static DataSet GetMobSpawns_ByRoomID(int MobAreaID, int intRoomID, int CoordX, int CoordY, int CoordZ)
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("mobs");

            DataTable dtMobs = new DataTable("Mobs");
            dtMobs.Columns.Add("MobID");
            dtMobs.Columns.Add("VNUM");
            dtMobs.Columns.Add("ShortDesc");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select  [MobID], [VNUM], [ShortDesc] ";
                strSQL += " from    [mob]  ";
                strSQL += " where   [MobID] in ( ";
                strSQL += "         select  [MobID] ";
                strSQL += "         from    [RoomMobs] ";
                strSQL += "         where   [SpawnAreaID] = " + MobAreaID.ToString() + " ";
                strSQL += "                 and [RoomNumber] = " + intRoomID.ToString() + " ";
                strSQL += "                 and [CoordX] = " + CoordX.ToString() + " ";
                strSQL += "                 and [CoordY] = " + CoordY.ToString() + " ";
                strSQL += "                 and [CoordZ] = " + CoordZ.ToString() + " ";
                strSQL += "         ) ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                // Populate a DataTable with the query results.
                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    dtMobs.Rows.Add((int)ds.Tables[0].Rows[iRow]["MobID"], (int)ds.Tables[0].Rows[iRow]["VNUM"], (string)ds.Tables[0].Rows[iRow]["ShortDesc"]);

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            // Create a DataSet to return to the DataGridView.
            DataSet set = new DataSet("SetMob");
            set.Tables.Add(dtMobs);

            return set;
        }

        // Get a dataset of mobs not spawning in room.
        public static DataSet GetMobSpawnsNotInRoom_ByRoomID(int MobAreaID, int intRoomID, int CoordX, int CoordY, int CoordZ)
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("mobs");

            DataTable dtMobs = new DataTable("Mobs");
            dtMobs.Columns.Add("MobID");
            dtMobs.Columns.Add("VNUM");
            dtMobs.Columns.Add("ShortDesc");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select  [MobID], [VNUM], [ShortDesc] ";
                strSQL += " from    [mob]  ";
                strSQL += " where   [MobID] not in ( ";
                strSQL += "         select  [MobID] ";
                strSQL += "         from    [RoomMobs] ";
                strSQL += "         where   [SpawnAreaID] = " + MobAreaID.ToString() + " ";
                strSQL += "                 and [RoomNumber] = " + intRoomID.ToString() + " ";
                strSQL += "                 and [CoordX] = " + CoordX.ToString() + " ";
                strSQL += "                 and [CoordY] = " + CoordY.ToString() + " ";
                strSQL += "                 and [CoordZ] = " + CoordZ.ToString() + " ";
                strSQL += "         ) ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                // Populate a DataTable with the query results.
                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    dtMobs.Rows.Add((int)ds.Tables[0].Rows[iRow]["MobID"], (int)ds.Tables[0].Rows[iRow]["VNUM"], (string)ds.Tables[0].Rows[iRow]["ShortDesc"]);

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            // Create a DataSet to return to the DataGridView.
            DataSet set = new DataSet("SetMob");
            set.Tables.Add(dtMobs);

            return set;
        }        
        #endregion
    }
}
