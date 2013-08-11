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
        public int mobAreaID { get; set; }
        public int mobVNUM { get; set; }
        public string ShortDesc { get; set; }
        #endregion

        #region "mob methods"
        // Add a new Mob.
        public void AddMob()
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " insert into [Mob] ([MobAreaID], [VNUM], [ShortDesc]) ";
                strSQL += " values (@MobAreaID, @VNUM, @ShortDesc) ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@MobAreaID", OleDbType.Integer, 10, "MobAreaID").Value = this.mobAreaID;
                da.InsertCommand.Parameters.Add("@VNUM", OleDbType.Integer, 10, "VNUM").Value = this.mobVNUM;
                da.InsertCommand.Parameters.Add("@ShortDesc", OleDbType.VarChar, 100, "ShortDesc").Value = this.ShortDesc;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        // Update an existing mob.
        public void UpdateMob()
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " update  [Mob] ";
                strSQL += " set     [VNUM] =  @VNUM,";
                strSQL += "         [ShortDesc] =  @ShortDesc";
                strSQL += " where  MobID = " + this.mobID + " ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@VNUM", OleDbType.Integer, 10, "VNUM").Value = this.mobVNUM;
                da.InsertCommand.Parameters.Add("@ShortDesc", OleDbType.VarChar, 100, "ShortDesc").Value = this.ShortDesc;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }
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

        // Add an Object Load to the Selected Mob.
        public static void AddMobLoad(int RoomAreaID, int iMobID, int iObjectID, int CoordX, int CoordY, int CoordZ)
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " insert into [MobObjects] ([LoadAreaID], [MobID], [ObjectID], [CoordX], [CoordY], [CoordZ]) ";
                strSQL += " values (@LoadAreaID, @MobID, @ObjectID, @CoordX, @CoordY, @CoordZ)";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@LoadAreaID", OleDbType.Integer, 10, "LoadAreaID").Value = RoomAreaID;
                da.InsertCommand.Parameters.Add("@MobID", OleDbType.Integer, 10, "MobID").Value = iMobID;
                da.InsertCommand.Parameters.Add("@ObjectID", OleDbType.Integer, 10, "ObjectID").Value = iObjectID;
                da.InsertCommand.Parameters.Add("@CoordX", OleDbType.Integer, 10, "CoordX").Value = CoordX;
                da.InsertCommand.Parameters.Add("@CoordY", OleDbType.Integer, 10, "CoordY").Value = CoordY;
                da.InsertCommand.Parameters.Add("@CoordZ", OleDbType.Integer, 10, "CoordZ").Value = CoordZ;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        // Remove an Object Load from the Selected Mob.
        public static void RemoveMobLoad(int MobAreaID, int iMobID, int iObjectID, int CoordX, int CoordY, int CoordZ)
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " delete from [MobObjects] ";
                strSQL += " where [LoadAreaID] = @LoadAreaID ";
                strSQL += "       and [MobID] = @MobID ";
                strSQL += "       and [ObjectID] = @ObjectID ";
                strSQL += "       and [CoordX] = @CoordX ";
                strSQL += "       and [CoordY] = @CoordY ";
                strSQL += "       and [CoordZ] = @CoordZ ";

                da.DeleteCommand = new OleDbCommand(strSQL);
                da.DeleteCommand.Connection = connection;

                da.DeleteCommand.Parameters.Add("@LoadAreaID", OleDbType.Integer, 10, "LoadAreaID").Value = MobAreaID;
                da.DeleteCommand.Parameters.Add("@MobID", OleDbType.Integer, 10, "MobID").Value = iMobID;
                da.DeleteCommand.Parameters.Add("@ObjectID", OleDbType.Integer, 10, "ObjectID").Value = iObjectID;
                da.DeleteCommand.Parameters.Add("@CoordX", OleDbType.Integer, 10, "CoordX").Value = CoordX;
                da.DeleteCommand.Parameters.Add("@CoordY", OleDbType.Integer, 10, "CoordY").Value = CoordY;
                da.DeleteCommand.Parameters.Add("@CoordZ", OleDbType.Integer, 10, "CoordZ").Value = CoordZ;

                da.DeleteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        // Remove an Object Load from the Selected Mob.
        public static void RemoveMobLoads(int MobAreaID, int iMobID, int CoordX, int CoordY, int CoordZ)
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " delete from [MobObjects] ";
                strSQL += " where [LoadAreaID] = @LoadAreaID ";
                strSQL += "       and [MobID] = @MobID ";
                strSQL += "       and [CoordX] = @CoordX ";
                strSQL += "       and [CoordY] = @CoordY ";
                strSQL += "       and [CoordZ] = @CoordZ ";

                da.DeleteCommand = new OleDbCommand(strSQL);
                da.DeleteCommand.Connection = connection;

                da.DeleteCommand.Parameters.Add("@LoadAreaID", OleDbType.Integer, 10, "LoadAreaID").Value = MobAreaID;
                da.DeleteCommand.Parameters.Add("@MobID", OleDbType.Integer, 10, "MobID").Value = iMobID;
                da.DeleteCommand.Parameters.Add("@CoordX", OleDbType.Integer, 10, "CoordX").Value = CoordX;
                da.DeleteCommand.Parameters.Add("@CoordY", OleDbType.Integer, 10, "CoordY").Value = CoordY;
                da.DeleteCommand.Parameters.Add("@CoordZ", OleDbType.Integer, 10, "CoordZ").Value = CoordZ;

                da.DeleteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }
        #endregion
    }
}
