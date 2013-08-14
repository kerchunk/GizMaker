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


        // Delete an existing door.
        public void DeleteDoor()
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
                strSQL += " delete from [RoomDoors] ";
                strSQL += " where  DoorID = " + this.doorID + " ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        // Check for Duplicate door.
        public bool DoorIsDuplicate()
        {
            bool blnIsDuplicate = false;
            string strDoorID = string.Empty;

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
                strSQL += " select [DoorID] ";
                strSQL += " from   [RoomDoors] ";
                strSQL += " where  DoorAreaID = " + this.areaID + " ";
                strSQL += "        and RoomNumber = " + this.roomNumber + " ";
                strSQL += "        and Direction = '" + this.direction + "' ";
                strSQL += "        and CoordX = " + this.coordX + " ";
                strSQL += "        and CoordY = " + this.coordY + " ";
                strSQL += "        and CoordZ = " + this.coordZ + " ";
                strSQL += "        and DoorID <> " + this.doorID + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    strDoorID = ds.Tables[0].Rows[0]["DoorID"].ToString();

                    if (strDoorID != "")
                        blnIsDuplicate = true;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return blnIsDuplicate;
        }
        #endregion

        #region "door public methods"
        // Get a room door by Room ID.
        public static door GetDoorByID(int DoorID)
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
                strSQL += " where  DoorID = " + DoorID + " ";

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

        // Get the Mirror Door of a Door by Direction.
        public static door GetDoorMirror(door oDoor, int RoomNumber)
        {
            int iModCheck = 0;
            bool blnNorthBorder = false;
            bool blnSouthBorder = false;
            bool blnEastBorder = false;
            bool blnWestBorder = false;

            // Create a new Door Object to hold the Mirror values.
            door oDoorMirror = new door();
            // Start with a copy of the New Door.
            oDoorMirror.doorID = oDoor.doorID;
            oDoorMirror.areaID = oDoor.areaID;
            oDoorMirror.roomNumber = oDoor.roomNumber;
            oDoorMirror.VNUM = oDoor.VNUM;
            oDoorMirror.keywords = oDoor.keywords;
            oDoorMirror.direction = oDoor.direction;
            oDoorMirror.doorType = oDoor.doorType;
            oDoorMirror.keyVNUM = oDoor.keyVNUM;
            oDoorMirror.coordX = oDoor.coordX;
            oDoorMirror.coordY = oDoor.coordY;
            oDoorMirror.coordZ = oDoor.coordZ;

            // Set the Door Mirror new Direction.
            oDoorMirror.direction = GetOppositeDirection(oDoor.direction);

            // Deterine if Room is on the North border of the map.
            iModCheck = (RoomNumber - 1) % 25;
            if (iModCheck == 0)
                blnNorthBorder = true;

            // Deterine if Room is on the South border of the map.
            iModCheck = (RoomNumber) % 25;
            if (iModCheck == 0)
                blnSouthBorder = true;

            // Deterine if Room is on the East border of the map.
            if ((RoomNumber + 25) > 750)
                blnEastBorder = true;

            // Deterine if Room is on the West border of the map.
            if ((RoomNumber - 25) < 0)
                blnWestBorder = true;

            // Determine North Mirror
            if (blnNorthBorder && oDoor.direction.ToLower() == "north")
            {
                oDoorMirror.roomNumber = RoomNumber + 24;
                oDoorMirror.coordY += 1;
            }
            else if (!blnNorthBorder && oDoor.direction.ToLower() == "north")
            {
                oDoorMirror.roomNumber = RoomNumber - 1;
            }

            // Determine South Mirror
            if (blnSouthBorder && oDoor.direction.ToLower() == "south")
            {
                oDoorMirror.roomNumber = RoomNumber - 24;
                oDoorMirror.coordY -= 1;
            }
            else if (!blnSouthBorder && oDoor.direction.ToLower() == "south")
            {
                oDoorMirror.roomNumber = RoomNumber + 1;
            }

            // Determine East Mirror
            if (blnEastBorder && oDoor.direction.ToLower() == "east")
            {
                oDoorMirror.roomNumber = RoomNumber - 725;
                oDoorMirror.coordX += 1;
            }
            else if (!blnEastBorder && oDoor.direction.ToLower() == "east")
            {
                oDoorMirror.roomNumber = RoomNumber + 25;
            }

            // Determine West Mirror
            if (blnWestBorder && oDoor.direction.ToLower() == "west")
            {
                oDoorMirror.roomNumber = RoomNumber + 725;
                oDoorMirror.coordX -= 1;
            }
            else if (!blnWestBorder && oDoor.direction.ToLower() == "west")
            {
                oDoorMirror.roomNumber = RoomNumber - 25;
            }

            // Determine West Mirror
            if (oDoor.direction.ToLower() == "up")
            {
                oDoorMirror.coordZ += 1;
            }

            // Determine West Mirror
            if (oDoor.direction.ToLower() == "down")
            {
                oDoorMirror.coordZ -= 1;
            }

            return oDoorMirror;
        }

        // Get the Mirror Door ID of a Door by Direction.
        public static int GetDoorMirrorID(door oDoorMirror, int RoomNumber)
        {
            int iDoorID = 0;

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
                strSQL += " where  DoorAreaID = " + oDoorMirror.areaID + " ";
                strSQL += "        and RoomNumber = " + oDoorMirror.roomNumber + " ";
                strSQL += "        and Direction = '" + oDoorMirror.direction + "' ";
                strSQL += "        and CoordX = " + oDoorMirror.coordX + " ";
                strSQL += "        and CoordY = " + oDoorMirror.coordY + " ";
                strSQL += "        and CoordZ = " + oDoorMirror.coordZ + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    iDoorID = (int)ds.Tables[0].Rows[0]["DoorID"];
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return iDoorID;
        }


        // Get the Mirror direction for a newly-created Door.
        public static string GetOppositeDirection(string direction)
        {
            string strOpposite = string.Empty;

            switch (direction.ToLower())
            {
                case "north":
                    strOpposite = "South";
                    break;
                case "south":
                    strOpposite = "North";
                    break;
                case "east":
                    strOpposite = "West";
                    break;
                case "west":
                    strOpposite = "East";
                    break;
                case "up":
                    strOpposite = "Down";
                    break;
                case "down":
                    strOpposite = "Up";
                    break;
                default:
                    break;
            }

            return strOpposite;
        }
        #endregion
    }
}
