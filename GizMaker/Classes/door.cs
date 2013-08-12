using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using ADODB;

namespace GizMaker.Classes
{
    class door
    {
        public int doorID { get; set; }
        public int areaID { get; set; }
        public int roomID { get; set; }
        public int VNUM { get; set; }
        public string keywords { get; set; }
        public string direction { get; set; }
        public string doorType { get; set; }
        public int keyVNUM { get; set; }

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
                strSQL += " insert into [RoomDoors] ([DoorAreaID], [RoomID], [VNUM], [Keywords], [Direction], [DoorType], [KeyVNUM]) ";
                strSQL += " values (@DoorAreaID, @RoomID, @VNUM, @Keywords, @Direction, @DoorType, @KeyVNUM) ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@DoorAreaID", OleDbType.VarChar, 100, "DoorAreaID").Value = this.areaID;
                da.InsertCommand.Parameters.Add("@RoomID", OleDbType.Integer, 10, "RoomID").Value = this.roomID;
                da.InsertCommand.Parameters.Add("@VNUM", OleDbType.Integer, 10, "VNUM").Value = this.VNUM;
                da.InsertCommand.Parameters.Add("@Keywords", OleDbType.VarChar, 100, "Keywords").Value = this.keywords;
                da.InsertCommand.Parameters.Add("@Direction", OleDbType.Integer, 10, "Direction").Value = this.direction;
                da.InsertCommand.Parameters.Add("@DoorType", OleDbType.Integer, 10, "DoorType").Value = this.doorType;
                da.InsertCommand.Parameters.Add("@KeyVNUM", OleDbType.Integer, 10, "KeyVNUM").Value = this.keyVNUM;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

    }
}
