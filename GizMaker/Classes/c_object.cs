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
    class c_object
    {
        #region "object properties"
        // Properties
        public int objectID { get; set; }
        public int objAreaID { get; set; }
        public int objVNUM { get; set; }
        public string ShortDesc { get; set; }
        #endregion

        #region "object methods"
        // Add a new Object.
        public void AddObject()
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
                strSQL += " insert into [Object] ([ObjectAreaID], [VNUM], [ShortDesc]) ";
                strSQL += " values (@ObjectAreaID, @VNUM, @ShortDesc) ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@ObjectAreaID", OleDbType.Integer, 10, "ObjectAreaID").Value = this.objAreaID;
                da.InsertCommand.Parameters.Add("@VNUM", OleDbType.Integer, 10, "VNUM").Value = this.objVNUM;
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

        // Update an existing object.
        public void UpdateObject()
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
                strSQL += " update  [Object] ";
                strSQL += " set     [VNUM] =  @VNUM,";
                strSQL += "         [ShortDesc] =  @ShortDesc";
                strSQL += " where  ObjectID = " + this.objectID + " ";

                da.InsertCommand = new OleDbCommand(strSQL);
                da.InsertCommand.Connection = connection;

                da.InsertCommand.Parameters.Add("@VNUM", OleDbType.Integer, 10, "VNUM").Value = this.objVNUM;
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

        #region "object public methods"
        // Get an object by Object ID. 
        public static c_object GetObject(int ObjectID)
        {
            c_object oObject = new c_object();

            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("object");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select [ObjectID], [VNUM], [ShortDesc] ";
                strSQL += " from   [Object] ";
                strSQL += " where  ObjectID = " + ObjectID + " ";

                da = new OleDbDataAdapter(strSQL, connection);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    oObject.objectID = (int)ds.Tables[0].Rows[0]["ObjectID"];
                    oObject.objVNUM = (int)ds.Tables[0].Rows[0]["VNUM"];
                    oObject.ShortDesc = (string)ds.Tables[0].Rows[0]["ShortDesc"];
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            connection.Close();
            connection.Dispose();

            return oObject;
        }
        
        // Get a dataset of objects loading on a mob.
        public static DataSet GetLoadingObjects_ByMobID(int ObjAreaID, int MobID, int CoordX, int CoordY, int CoordZ)
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("objs");

            DataTable dtObjects = new DataTable("Objs");
            dtObjects.Columns.Add("ObjectID");
            dtObjects.Columns.Add("VNUM");
            dtObjects.Columns.Add("ShortDesc");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select  [ObjectID], [VNUM], [ShortDesc] ";
                strSQL += " from    [Object]  ";
                strSQL += " where   [ObjectID] in ( ";
                strSQL += "         select  [ObjectID] ";
                strSQL += "         from    [MobObjects] ";
                strSQL += "         where   [LoadAreaID] = " + ObjAreaID.ToString() + " ";
                strSQL += "                 and [MobID] = " + MobID.ToString() + " ";
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
                    dtObjects.Rows.Add((int)ds.Tables[0].Rows[iRow]["ObjectID"], (int)ds.Tables[0].Rows[iRow]["VNUM"], (string)ds.Tables[0].Rows[iRow]["ShortDesc"]);

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
            DataSet set = new DataSet("SetObj");
            set.Tables.Add(dtObjects);

            return set;
        }

        // Get a dataset of objects loading on a mob.
        public static DataSet GetNonLoadingObjects_ByMobID(int ObjAreaID, int MobID, int CoordX, int CoordY, int CoordZ)
        {
            // Configure database connection elements.
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = database.getConnectionString();
            DataSet ds = new DataSet("objs");

            DataTable dtObjects = new DataTable("Objs");
            dtObjects.Columns.Add("ObjectID");
            dtObjects.Columns.Add("VNUM");
            dtObjects.Columns.Add("ShortDesc");

            try
            {
                connection.Open();

                // Create query. 
                string strSQL = string.Empty;
                strSQL += " select  [ObjectID], [VNUM], [ShortDesc] ";
                strSQL += " from    [Object]  ";
                strSQL += " where   [ObjectID] not in ( ";
                strSQL += "         select  [ObjectID] ";
                strSQL += "         from    [MobObjects] ";
                strSQL += "         where   [LoadAreaID] = " + ObjAreaID.ToString() + " ";
                strSQL += "                 and [MobID] = " + MobID.ToString() + " ";
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
                    dtObjects.Rows.Add((int)ds.Tables[0].Rows[iRow]["ObjectID"], (int)ds.Tables[0].Rows[iRow]["VNUM"], (string)ds.Tables[0].Rows[iRow]["ShortDesc"]);

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
            DataSet set = new DataSet("SetObj");
            set.Tables.Add(dtObjects);

            return set;
        }
        #endregion
    }
}
