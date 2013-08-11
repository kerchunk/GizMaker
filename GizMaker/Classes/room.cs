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
    class room
    {
        #region "room properties"
        // Properties
        public int roomID { get; set; }
        public int roomAreaID { get; set; }
        public int roomNumber { get; set; }
        public int coordX { get; set; }
        public int coordY { get; set; }
        public int coordZ { get; set; }
        public bool hasExitNorth { get; set; }
        public bool hasExitSouth { get; set; }
        public bool hasExitEast { get; set; }
        public bool hasExitWest { get; set; }
        public bool hasExitUp { get; set; }
        public bool hasExitDown { get; set; }
        #endregion

        #region "room methods"
        // Add a new room.
        public void AddRoom()
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
                strSQL += " insert into [Room] ([RoomAreaID], [RoomNumber], [CoordX], [CoordY], [CoordZ], [HasExitNorth], [HasExitSouth], [HasExitEast], ";
                strSQL += "             [HasExitWest], [HasExitUp], [HasExitDown]) ";
                strSQL += " values (@RoomAreaID, @RoomNumber, @CoordX, @CoordY, @CoordZ, @HasExitNorth, @HasExitSouth, @HasExitEast, ";
                strSQL += "             @HasExitWest, @HasExitUp, @HasExitDown) ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@RoomAreaID", OleDbType.Integer, 10, "RoomAreaID").Value = this.roomAreaID;
                da.InsertCommand.Parameters.Add("@RoomNumber", OleDbType.Integer, 10, "RoomNumber").Value = this.roomNumber;
                da.InsertCommand.Parameters.Add("@CoordX", OleDbType.Integer, 10, "CoordX").Value = this.coordX;
                da.InsertCommand.Parameters.Add("@CoordY", OleDbType.Integer, 10, "CoordY").Value = this.coordY;
                da.InsertCommand.Parameters.Add("@CoordZ", OleDbType.Integer, 10, "CoordZ").Value = this.coordZ;
                da.InsertCommand.Parameters.Add("@HasExitNorth", OleDbType.Boolean, 10, "HasExitNorth").Value = this.hasExitNorth;
                da.InsertCommand.Parameters.Add("@HasExitSouth", OleDbType.Boolean, 10, "HasExitSouth").Value = this.hasExitSouth;
                da.InsertCommand.Parameters.Add("@HasExitEast", OleDbType.Boolean, 10, "HasExitEast").Value = this.hasExitEast;
                da.InsertCommand.Parameters.Add("@HasExitWest", OleDbType.Boolean, 10, "HasExitWest").Value = this.hasExitWest;
                da.InsertCommand.Parameters.Add("@HasExitUp", OleDbType.Boolean, 10, "HasExitUp").Value = this.hasExitUp;
                da.InsertCommand.Parameters.Add("@HasExitDown", OleDbType.Boolean, 10, "HasExitDown").Value = this.hasExitDown;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        // Update an existing room.
        public void UpdateRoom()
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
                strSQL += " update  [Room] ";
                strSQL += " set     [HasExitNorth] =  @HasExitNorth,";
                strSQL += "         [HasExitSouth] =  @HasExitSouth,";
                strSQL += "         [HasExitEast] =  @HasExitEast,";
                strSQL += "         [HasExitWest] =  @HasExitWest,";
                strSQL += "         [HasExitUp] =  @HasExitUp,";
                strSQL += "         [HasExitDown] =  @HasExitDown";
                strSQL += " where  RoomAreaID = " + this.roomAreaID + " ";
                strSQL += "        and RoomNumber = " + this.roomNumber + " ";
                strSQL += "        and CoordX = " + this.coordX + " ";
                strSQL += "        and CoordY = " + this.coordY + " ";
                strSQL += "        and CoordZ = " + this.coordZ + " ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@HasExitNorth", OleDbType.Boolean, 10, "HasExitNorth").Value = this.hasExitNorth;
                da.InsertCommand.Parameters.Add("@HasExitSouth", OleDbType.Boolean, 10, "HasExitSouth").Value = this.hasExitSouth;
                da.InsertCommand.Parameters.Add("@HasExitEast", OleDbType.Boolean, 10, "HasExitEast").Value = this.hasExitEast;
                da.InsertCommand.Parameters.Add("@HasExitWest", OleDbType.Boolean, 10, "HasExitWest").Value = this.hasExitWest;
                da.InsertCommand.Parameters.Add("@HasExitUp", OleDbType.Boolean, 10, "HasExitUp").Value = this.hasExitUp;
                da.InsertCommand.Parameters.Add("@HasExitDown", OleDbType.Boolean, 10, "HasExitDown").Value = this.hasExitDown;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        // Remove an existing room.
        public void RemoveRoom()
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
                strSQL += " delete from [Room] ";
                strSQL += " where  RoomAreaID = " + this.roomAreaID + " ";
                strSQL += "        and RoomNumber = " + this.roomNumber + " ";
                strSQL += "        and CoordX = " + this.coordZ + " ";
                strSQL += "        and CoordY = " + this.coordY + " ";
                strSQL += "        and CoordZ = " + this.coordZ + " ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@RoomAreaID", OleDbType.Integer, 10, "RoomAreaID").Value = this.roomAreaID;
                da.InsertCommand.Parameters.Add("@RoomNumber", OleDbType.Integer, 10, "RoomNumber").Value = this.roomNumber;
                da.InsertCommand.Parameters.Add("@CoordX", OleDbType.Integer, 10, "CoordX").Value = this.coordX;
                da.InsertCommand.Parameters.Add("@CoordY", OleDbType.Integer, 10, "CoordY").Value = this.coordY;
                da.InsertCommand.Parameters.Add("@CoordZ", OleDbType.Integer, 10, "CoordZ").Value = this.coordZ;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        // Check if room exists.
        public bool Exists()
        {
            bool blnExists = false;

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("rooms");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [RoomID] from [Room] ";
                strSQL += " where  RoomAreaID = " + this.roomAreaID + " ";
                strSQL += "        and RoomNumber = " + this.roomNumber + " ";
                strSQL += "        and CoordX = " + this.coordX + " ";
                strSQL += "        and CoordY = " + this.coordY + " ";
                strSQL += "        and CoordZ = " + this.coordZ + " ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@RoomAreaID", OleDbType.Integer, 10, "RoomAreaID").Value = this.roomAreaID;
                da.InsertCommand.Parameters.Add("@RoomNumber", OleDbType.Integer, 10, "RoomNumber").Value = this.roomNumber;
                da.InsertCommand.Parameters.Add("@CoordX", OleDbType.Integer, 10, "CoordX").Value = this.coordX;
                da.InsertCommand.Parameters.Add("@CoordY", OleDbType.Integer, 10, "CoordY").Value = this.coordY;
                da.InsertCommand.Parameters.Add("@CoordZ", OleDbType.Integer, 10, "CoordZ").Value = this.coordZ;

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                int iRoomNumber = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    iRoomNumber = (int)ds.Tables[0].Rows[0]["RoomID"];

                    if (iRoomNumber > 0)
                    {
                        blnExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return blnExists;
        }
        #endregion

        #region "room public methods"
        // Get a room by Area, Room and X, Y, Z panel coordinates. 
        public static room GetRoom(int RoomAreaID, int RoomNumber, int CoordX, int CoordY, int CoordZ)
        {
            room oRoom = new room();

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("rooms");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [RoomAreaID], [RoomNumber], [CoordX], [CoordY], [CoordZ], [HasExitNorth], [HasExitSouth], [HasExitEast], ";
                strSQL += "             [HasExitWest], [HasExitUp], [HasExitDown] ";
                strSQL += " from   [Room] ";
                strSQL += " where  RoomAreaID = " + RoomAreaID + " ";
                strSQL += "        and RoomNumber = " + RoomNumber + " ";
                strSQL += "        and CoordX = " + CoordX + " ";
                strSQL += "        and CoordY = " + CoordY + " ";
                strSQL += "        and CoordZ = " + CoordZ + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    oRoom.roomAreaID = (int)ds.Tables[0].Rows[0]["RoomAreaID"];
                    oRoom.roomNumber = (int)ds.Tables[0].Rows[0]["RoomNumber"];
                    oRoom.hasExitNorth = (bool)ds.Tables[0].Rows[0]["HasExitNorth"];
                    oRoom.hasExitSouth = (bool)ds.Tables[0].Rows[0]["HasExitSouth"];
                    oRoom.hasExitEast = (bool)ds.Tables[0].Rows[0]["HasExitEast"];
                    oRoom.hasExitWest = (bool)ds.Tables[0].Rows[0]["HasExitWest"];
                    oRoom.hasExitUp = (bool)ds.Tables[0].Rows[0]["HasExitUp"];
                    oRoom.hasExitDown = (bool)ds.Tables[0].Rows[0]["HasExitDown"];
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return oRoom;
        }

        // Get a collection of rooms on the panel.
        public static room[] GetPanelRooms(int RoomAreaID, int CoordX, int CoordY, int CoordZ)
        {
            room[] rooms = new room[800];

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("rooms");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [RoomAreaID], [RoomNumber], [CoordX], [CoordY], [CoordZ], [HasExitNorth], [HasExitSouth], [HasExitEast], ";
                strSQL += "             [HasExitWest], [HasExitUp], [HasExitDown] ";
                strSQL += " from   [Room] ";
                strSQL += " where  RoomAreaID = " + RoomAreaID + " ";
                strSQL += "        and CoordX = " + CoordX + " ";
                strSQL += "        and CoordY = " + CoordY + " ";
                strSQL += "        and CoordZ = " + CoordZ + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    room oRoom = new room();

                    oRoom.roomNumber = (int)ds.Tables[0].Rows[iRow]["RoomNumber"];
                    oRoom.hasExitNorth = (bool)ds.Tables[0].Rows[iRow]["HasExitNorth"];
                    oRoom.hasExitSouth = (bool)ds.Tables[0].Rows[iRow]["HasExitSouth"];
                    oRoom.hasExitEast = (bool)ds.Tables[0].Rows[iRow]["HasExitEast"];
                    oRoom.hasExitWest = (bool)ds.Tables[0].Rows[iRow]["HasExitWest"];
                    oRoom.hasExitUp = (bool)ds.Tables[0].Rows[iRow]["HasExitUp"];
                    oRoom.hasExitDown = (bool)ds.Tables[0].Rows[iRow]["HasExitDown"];

                    rooms[iRow] = oRoom;

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return rooms;
        }

        // Get a collection of rooms with an "up" exit.
        public static room[] GetRoomsWithUpExit(int RoomAreaID, int CoordX, int CoordY, int CoordZ)
        {
            room[] rooms = new room[800];

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("rooms");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [RoomAreaID], [RoomNumber], [CoordX], [CoordY], [CoordZ], [HasExitNorth], [HasExitSouth], [HasExitEast], ";
                strSQL += "             [HasExitWest], [HasExitUp], [HasExitDown] ";
                strSQL += " from   [Room] ";
                strSQL += " where  RoomAreaID = " + RoomAreaID + " ";
                strSQL += "        and CoordX = " + CoordX + " ";
                strSQL += "        and CoordY = " + CoordY + " ";
                strSQL += "        and CoordZ = " + CoordZ + " ";
                strSQL += "        and HasExitUp = Yes ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    room oRoom = new room();

                    oRoom.roomNumber = (int)ds.Tables[0].Rows[iRow]["RoomNumber"];
                    oRoom.hasExitNorth = (bool)ds.Tables[0].Rows[iRow]["HasExitNorth"];
                    oRoom.hasExitSouth = (bool)ds.Tables[0].Rows[iRow]["HasExitSouth"];
                    oRoom.hasExitEast = (bool)ds.Tables[0].Rows[iRow]["HasExitEast"];
                    oRoom.hasExitWest = (bool)ds.Tables[0].Rows[iRow]["HasExitWest"];
                    oRoom.hasExitUp = (bool)ds.Tables[0].Rows[iRow]["HasExitUp"];
                    oRoom.hasExitDown = (bool)ds.Tables[0].Rows[iRow]["HasExitDown"];

                    rooms[iRow] = oRoom;

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return rooms;
        }

        // Get a collection of rooms with a "down" exit.
        public static room[] GetRoomsWithDownExit(int RoomAreaID, int CoordX, int CoordY, int CoordZ)
        {
            room[] rooms = new room[800];

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("rooms");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [RoomAreaID], [RoomNumber], [CoordX], [CoordY], [CoordZ], [HasExitNorth], [HasExitSouth], [HasExitEast], ";
                strSQL += "             [HasExitWest], [HasExitUp], [HasExitDown] ";
                strSQL += " from   [Room] ";
                strSQL += " where  RoomAreaID = " + RoomAreaID + " ";
                strSQL += "        and CoordX = " + CoordX + " ";
                strSQL += "        and CoordY = " + CoordY + " ";
                strSQL += "        and CoordZ = " + CoordZ + " ";
                strSQL += "        and HasExitDown = Yes ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    room oRoom = new room();

                    oRoom.roomNumber = (int)ds.Tables[0].Rows[iRow]["RoomNumber"];
                    oRoom.hasExitNorth = (bool)ds.Tables[0].Rows[iRow]["HasExitNorth"];
                    oRoom.hasExitSouth = (bool)ds.Tables[0].Rows[iRow]["HasExitSouth"];
                    oRoom.hasExitEast = (bool)ds.Tables[0].Rows[iRow]["HasExitEast"];
                    oRoom.hasExitWest = (bool)ds.Tables[0].Rows[iRow]["HasExitWest"];
                    oRoom.hasExitUp = (bool)ds.Tables[0].Rows[iRow]["HasExitUp"];
                    oRoom.hasExitDown = (bool)ds.Tables[0].Rows[iRow]["HasExitDown"];

                    rooms[iRow] = oRoom;

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return rooms;
        }

        // Get a collection of rooms with an "up" exit.
        public static room[] GetRoomsWithMobSpawn(int RoomAreaID, int CoordX, int CoordY, int CoordZ, int iMobID)
        {
            room[] rooms = new room[800];

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("rooms");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL = " select [RoomAreaID], [RoomNumber], [CoordX], [CoordY], [CoordZ], [HasExitNorth], [HasExitSouth], [HasExitEast], ";
                strSQL += "             [HasExitWest], [HasExitUp], [HasExitDown] ";
                strSQL += " from    [Room]  ";
                strSQL += " where   [RoomAreaID] = " + RoomAreaID.ToString() +" ";
                strSQL += "         and [RoomNumber] in ( ";
                strSQL += "         select  [RoomNumber] ";
                strSQL += "         from    [RoomMobs] ";
                strSQL += "         where   [SpawnAreaID] = " + RoomAreaID.ToString() + " ";
                strSQL += "                 and [MobID] = " + iMobID.ToString() + " ";
                strSQL += "                 and [CoordX] = " + CoordX.ToString() + " ";
                strSQL += "                 and [CoordY] = " + CoordY.ToString() + " ";
                strSQL += "                 and [CoordZ] = " + CoordZ.ToString() + " ";
                strSQL += "         ) ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    room oRoom = new room();

                    oRoom.roomNumber = (int)ds.Tables[0].Rows[iRow]["RoomNumber"];
                    oRoom.hasExitNorth = (bool)ds.Tables[0].Rows[iRow]["HasExitNorth"];
                    oRoom.hasExitSouth = (bool)ds.Tables[0].Rows[iRow]["HasExitSouth"];
                    oRoom.hasExitEast = (bool)ds.Tables[0].Rows[iRow]["HasExitEast"];
                    oRoom.hasExitWest = (bool)ds.Tables[0].Rows[iRow]["HasExitWest"];
                    oRoom.hasExitUp = (bool)ds.Tables[0].Rows[iRow]["HasExitUp"];
                    oRoom.hasExitDown = (bool)ds.Tables[0].Rows[iRow]["HasExitDown"];

                    rooms[iRow] = oRoom;

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return rooms;
        }

        // Add a Mob Spawn to the Selected Room.
        public static void AddRoomSpawn(int RoomAreaID, int RoomNumber, int CoordX, int CoordY, int CoordZ, int iMobID)
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
                strSQL += " insert into [RoomMobs] ([SpawnAreaID], [RoomNumber], [CoordX], [CoordY], [CoordZ], [MobID]) ";
                strSQL += " values (@SpawnAreaID, @RoomNumber, @CoordX, @CoordY, @CoordZ, @MobID)";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@SpawnAreaID", OleDbType.Integer, 10, "SpawnAreaID").Value = RoomAreaID;
                da.InsertCommand.Parameters.Add("@RoomNumber", OleDbType.Integer, 10, "RoomNumber").Value = RoomNumber;
                da.InsertCommand.Parameters.Add("@CoordX", OleDbType.Integer, 10, "CoordX").Value = CoordX;
                da.InsertCommand.Parameters.Add("@CoordY", OleDbType.Integer, 10, "CoordY").Value = CoordY;
                da.InsertCommand.Parameters.Add("@CoordZ", OleDbType.Integer, 10, "CoordZ").Value = CoordZ;
                da.InsertCommand.Parameters.Add("@MobID", OleDbType.Integer, 10, "MobID").Value = iMobID;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        // Remove a Mob Spawn to the Selected Room.
        public static void RemoveRoomSpawn(int RoomAreaID, int RoomNumber, int CoordX, int CoordY, int CoordZ, int iMobID)
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
                strSQL += " delete from [RoomMobs] ";
                strSQL += " where [SpawnAreaID] = @SpawnAreaID ";
                strSQL += "       and [RoomNumber] = @RoomNumber ";
                strSQL += "       and [CoordX] = @CoordX ";
                strSQL += "       and [CoordY] = @CoordY ";
                strSQL += "       and [CoordZ] = @CoordZ ";
                strSQL += "       and [MobID] = @MobID ";

                da.DeleteCommand = new OleDbCommand(strSQL);
                da.DeleteCommand.Connection = connection;

                da.DeleteCommand.Parameters.Add("@SpawnAreaID", OleDbType.Integer, 10, "SpawnAreaID").Value = RoomAreaID;
                da.DeleteCommand.Parameters.Add("@RoomNumber", OleDbType.Integer, 10, "RoomNumber").Value = RoomNumber;
                da.DeleteCommand.Parameters.Add("@CoordX", OleDbType.Integer, 10, "CoordX").Value = CoordX;
                da.DeleteCommand.Parameters.Add("@CoordY", OleDbType.Integer, 10, "CoordY").Value = CoordY;
                da.DeleteCommand.Parameters.Add("@CoordZ", OleDbType.Integer, 10, "CoordZ").Value = CoordZ;
                da.DeleteCommand.Parameters.Add("@MobID", OleDbType.Integer, 10, "MobID").Value = iMobID;

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
