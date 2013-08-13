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
    class door
    {
        #region "door properties"
        public int doorID { get; set; }
        public int areaID { get; set; }
        public int roomNumber { get; set; }
        public int VNUM { get; set; }
        public string keywords { get; set; }
        public string direction { get; set; }
        public string doorType { get; set; }
        public int keyVNUM { get; set; }
        public int coordX { get; set; }
        public int coordY { get; set; }
        public int coordZ { get; set; }
        #endregion

        #region "door methods"
        // Add new Door. 
        public void AddDoor()
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
                strSQL += " insert into [RoomDoors] ([DoorAreaID], [RoomNumber], [VNUM], [Keywords], [Direction], [DoorType], [KeyVNUM], [CoordX], [CoordY], [CoordZ]) ";
                strSQL += " values (@DoorAreaID, @RoomNumber, @VNUM, @Keywords, @Direction, @DoorType, @KeyVNUM, @CoordX, @CoordY, @CoordZ) ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@DoorAreaID", OleDbType.Integer, 100, "DoorAreaID").Value = this.areaID;
                da.InsertCommand.Parameters.Add("@RoomNumber", OleDbType.Integer, 10, "RoomNumber").Value = this.roomNumber;
                da.InsertCommand.Parameters.Add("@VNUM", OleDbType.Integer, 10, "VNUM").Value = this.VNUM;
                da.InsertCommand.Parameters.Add("@Keywords", OleDbType.VarChar, 100, "Keywords").Value = this.keywords;
                da.InsertCommand.Parameters.Add("@Direction", OleDbType.VarChar, 100, "Direction").Value = this.direction;
                da.InsertCommand.Parameters.Add("@DoorType", OleDbType.VarChar, 100, "DoorType").Value = this.doorType;
                da.InsertCommand.Parameters.Add("@KeyVNUM", OleDbType.Integer, 10, "KeyVNUM").Value = this.keyVNUM;
                da.InsertCommand.Parameters.Add("@CoordX", OleDbType.Integer, 10, "CoordX").Value = this.keyVNUM;
                da.InsertCommand.Parameters.Add("@CoordY", OleDbType.Integer, 10, "CoordY").Value = this.keyVNUM;
                da.InsertCommand.Parameters.Add("@CoordZ", OleDbType.Integer, 10, "CoordZ").Value = this.keyVNUM;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        // Update an existing door.
        public void UpdateDoor()
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
                strSQL += " update  [RoomDoors] ";
                strSQL += " set     [VNUM] =  @VNUM, ";
                strSQL += "         [Keywords] =  @Keywords, ";
                strSQL += "         [Direction] =  @Direction, ";
                strSQL += "         [KeyVNUM] =  @KeyVNUM, ";
                strSQL += "         [DoorType] = @DoorType ";
                strSQL += " where  DoorID = " + this.doorID + " ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@VNUM", OleDbType.Integer, 10, "VNUM").Value = this.VNUM;
                da.InsertCommand.Parameters.Add("@Keywords", OleDbType.VarChar, 100, "Keywords").Value = this.keywords;
                da.InsertCommand.Parameters.Add("@Direction", OleDbType.VarChar, 25, "Direction").Value = this.direction;
                da.InsertCommand.Parameters.Add("@KeyVNUM", OleDbType.Integer, 10, "KeyVNUM").Value = this.keyVNUM;
                da.InsertCommand.Parameters.Add("@DoorType", OleDbType.VarChar, 25, "DoorType").Value = this.doorType;

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

        #region "door public methods"
        // Get a room door Room ID and Direction.
        public static door GetDoor_ByRoomAndDirection(int DoorAreaID, int RoomNumber, string strDirection, int CoordX, int CoordY, int CoordZ)
        {
            door oDoor = new door();

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("doors");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [DoorID], [DoorAreaID], [RoomNumber], [VNUM], [Keywords], [Direction], [DoorType], [KeyVNUM], [CoordX], [CoordY], [CoordZ] ";
                strSQL += " from   [RoomDoors] ";
                strSQL += " where  DoorAreaID = " + DoorAreaID + " ";
                strSQL += "        and RoomNumber = " + RoomNumber + " ";
                strSQL += "        and Direction = '" + strDirection + "' ";
                strSQL += "        and CoordX = " + CoordX + " ";
                strSQL += "        and CoordY = " + CoordY + " ";
                strSQL += "        and CoordZ = " + CoordZ + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    oDoor.doorID = (int)ds.Tables[0].Rows[0]["DoorID"];
                    oDoor.areaID = (int)ds.Tables[0].Rows[0]["DoorAreaID"];
                    oDoor.roomNumber = (int)ds.Tables[0].Rows[0]["RoomNumber"];
                    oDoor.VNUM = (int)ds.Tables[0].Rows[0]["VNUM"];
                    oDoor.keywords = (string)ds.Tables[0].Rows[0]["Keywords"];
                    oDoor.direction = (string)ds.Tables[0].Rows[0]["Direction"];
                    oDoor.doorType = (string)ds.Tables[0].Rows[0]["DoorType"];
                    oDoor.keyVNUM = (int)ds.Tables[0].Rows[0]["KeyVNUM"]; ;
                    oDoor.coordX = (int)ds.Tables[0].Rows[0]["CoordX"]; ;
                    oDoor.coordY = (int)ds.Tables[0].Rows[0]["CoordY"]; ;
                    oDoor.coordZ = (int)ds.Tables[0].Rows[0]["CoordZ"]; ;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return oDoor;
        }

        // Get a collection of doors from the room.
        public static door[] GetRoomDoors(int DoorAreaID, int RoomNumber, int CoordX, int CoordY, int CoordZ)
        {
            door[] doors = new door[800];

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("doors");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [DoorID], [DoorAreaID], [RoomNumber], [VNUM], [Keywords], [Direction], [DoorType], [KeyVNUM], [CoordX], [CoordY], [CoordZ] ";
                strSQL += " from   [RoomDoors] ";
                strSQL += " where  DoorAreaID = " + DoorAreaID + " ";
                strSQL += "        and RoomNumber = " + RoomNumber + " ";
                strSQL += "        and CoordX = " + CoordX + " ";
                strSQL += "        and CoordY = " + CoordY + " ";
                strSQL += "        and CoordZ = " + CoordZ + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    door oDoor = new door();

                    oDoor.doorID = (int)ds.Tables[0].Rows[iRow]["DoorID"];
                    oDoor.areaID = (int)ds.Tables[0].Rows[iRow]["DoorAreaID"];
                    oDoor.roomNumber = (int)ds.Tables[0].Rows[iRow]["RoomNumber"];
                    oDoor.VNUM = (int)ds.Tables[0].Rows[iRow]["VNUM"];
                    oDoor.keywords = (string)ds.Tables[0].Rows[iRow]["Keywords"];
                    oDoor.direction = (string)ds.Tables[0].Rows[iRow]["Direction"];
                    oDoor.doorType = (string)ds.Tables[0].Rows[iRow]["DoorType"];
                    oDoor.keyVNUM = (int)ds.Tables[0].Rows[iRow]["KeyVNUM"]; ;
                    oDoor.coordX = (int)ds.Tables[0].Rows[iRow]["CoordX"]; ;
                    oDoor.coordY = (int)ds.Tables[0].Rows[iRow]["CoordY"]; ;
                    oDoor.coordZ = (int)ds.Tables[0].Rows[iRow]["CoordZ"]; ;

                    doors[iRow] = oDoor;

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return doors;
        }

        // Get a collection of doors from the panel.
        public static door[] GetPanelDoors(int DoorAreaID, int CoordX, int CoordY, int CoordZ)
        {
            door[] doors = new door[800];

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("doors");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [DoorID], [DoorAreaID], [RoomNumber], [VNUM], [Keywords], [Direction], [DoorType], [KeyVNUM], [CoordX], [CoordY], [CoordZ] ";
                strSQL += " from   [RoomDoors] ";
                strSQL += " where  DoorAreaID = " + DoorAreaID + " ";
                strSQL += "        and CoordX = " + CoordX + " ";
                strSQL += "        and CoordY = " + CoordY + " ";
                strSQL += "        and CoordZ = " + CoordZ + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    door oDoor = new door();

                    oDoor.doorID = (int)ds.Tables[0].Rows[iRow]["DoorID"];
                    oDoor.areaID = (int)ds.Tables[0].Rows[iRow]["DoorAreaID"];
                    oDoor.roomNumber = (int)ds.Tables[0].Rows[iRow]["RoomNumber"];
                    oDoor.VNUM = (int)ds.Tables[0].Rows[iRow]["VNUM"];
                    oDoor.keywords = (string)ds.Tables[0].Rows[iRow]["Keywords"];
                    oDoor.direction = (string)ds.Tables[0].Rows[iRow]["Direction"];
                    oDoor.doorType = (string)ds.Tables[0].Rows[iRow]["DoorType"];
                    oDoor.keyVNUM = (int)ds.Tables[0].Rows[iRow]["KeyVNUM"]; ;
                    oDoor.coordX = (int)ds.Tables[0].Rows[iRow]["CoordX"]; ;
                    oDoor.coordY = (int)ds.Tables[0].Rows[iRow]["CoordY"]; ;
                    oDoor.coordZ = (int)ds.Tables[0].Rows[iRow]["CoordZ"]; ;

                    doors[iRow] = oDoor;

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return doors;
        }

        // Get Key Name by ID.
        public static string GetKeyName_ByID(int ObjAreaID, int iKeyID)
        {
            string strName = string.Empty;

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("key");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [ShortDesc] ";
                strSQL += " from   [Object] ";
                strSQL += " where  ObjectAreaID = " + ObjAreaID + " ";
                strSQL += "        and ObjectID = " + iKeyID + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    strName = (string)ds.Tables[0].Rows[0]["ShortDesc"];
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return strName;
        }
        #endregion
    }
}
