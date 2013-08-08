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
    class area
    {
        public int areaID { get; set; }
        public string areaName { get; set; }
        public int zoneNumber { get; set; }
        public int startingVNUM { get; set; }

        // Add new Area. 
        public void AddArea()
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
                strSQL += " insert into [Area] ([AreaName], [ZoneNumber], [StartingVNUM]) ";
                strSQL += " values (@AreaName, @ZoneNumber, @StartingVNUM) ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@AreaName", OleDbType.VarChar, 100, "AreaName").Value = this.areaName;
                da.InsertCommand.Parameters.Add("@ZoneNumber", OleDbType.Integer, 10, "ZoneNumber").Value = this.zoneNumber;
                da.InsertCommand.Parameters.Add("@StartingVNUM", OleDbType.Integer, 10, "StartingVNUM").Value = this.startingVNUM;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();
        }

        public static DataSet GetAreas()
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("areas");

            DataTable dtTest = new DataTable("Areas");
            dtTest.Columns.Add("AreaID");
            dtTest.Columns.Add("AreaName");
            dtTest.Columns.Add("ZoneNumber");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [AreaID], [AreaName], [ZoneNumber]";
                strSQL += " from   [Area] ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                // Populate a DataTable with the query results.
                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {
                    dtTest.Rows.Add((int)ds.Tables[0].Rows[iRow]["AreaID"], (string)ds.Tables[0].Rows[iRow]["AreaName"], (int)ds.Tables[0].Rows[iRow]["ZoneNumber"]);

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
            DataSet set = new DataSet("SetArea");
            set.Tables.Add(dtTest);

            return set;
        }

        public static area GetAreaByID(int iAreaID)
        {
            area oArea = new area();

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("areas");

            // Create query. 
            string strSQL = string.Empty;
            strSQL += " select [AreaID], [AreaName], [ZoneNumber]";
            strSQL += " from   [Area] ";
            strSQL += " where  [AreaID] = " + iAreaID.ToString() + " ";

            try
            {
                connection.Open();

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                int iRow = 0;
                while (iRow <= ds.Tables[0].Rows.Count - 1)
                {

                    oArea.areaID = (int)ds.Tables[0].Rows[iRow]["AreaID"];
                    oArea.areaName = (string)ds.Tables[0].Rows[iRow]["AreaName"];
                    oArea.zoneNumber = (int)ds.Tables[0].Rows[iRow]["ZoneNumber"];
                    oArea.startingVNUM = (int)ds.Tables[0].Rows[iRow]["StartingVNUM"];

                    iRow++;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return oArea;
        }

        // Completely Remove an Area from the database.
        public static void DeleteArea(int iAreaID)
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();

            // Delete All Rooms in the Area.
            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " delete ";
                strSQL += " from   [Room] ";
                strSQL += " where  [RoomAreaID] = " + iAreaID.ToString() + " ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }
            connection.Close();


            // Delete the Area.
            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " delete ";
                strSQL += " from   [Area] ";
                strSQL += " where  [AreaID] = " + iAreaID.ToString() + " ";

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
    }
}
