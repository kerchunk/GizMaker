using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GizMaker.forms
{
    public partial class ObjectManagement : Form
    {
        // Global Variables
        #region "global variables"
        public int iObjectID { get; set; }
        int iCurrentAreaID = 0;
        #endregion

        // Constructors
        #region "constructors"
        public ObjectManagement(int iAreaID)
        {
            InitializeComponent();

            // Set Current Area ID.
            iCurrentAreaID = iAreaID;
        }

        public ObjectManagement(int iAreaID, int ObjectID)
        {
            InitializeComponent();

            // Populate ObjectID to local form.
            iObjectID = ObjectID;
            // Set Current Area ID.
            iCurrentAreaID = iAreaID;

            PopulateObjectDetails(iObjectID);
        }
        #endregion

        // Populate Object Details to the Form
        #region "population"
        public void PopulateObjectDetails(int iObjectID)
        {
            classes.c_object oObject = new classes.c_object();
            oObject = classes.c_object.GetObject(iObjectID);

            txtVNUM.Text = oObject.objVNUM.ToString();
            txtShortDescription.Text = oObject.ShortDesc;
        }
        #endregion

        // Form Events
        #region "events"
        // Close the Window.
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Save the new Object.
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save Object.
            SaveObject();

            // Close the Window.
            Close();
        }
        #endregion

        #region "methods"
        private void SaveObject()
        {
            // Create a new Object object.
            classes.c_object oObject = new classes.c_object();

            // Populate Object Details from Form.
            int iVUM = 0;
            if (txtVNUM.Text != "")
                iVUM = Convert.ToInt32(txtVNUM.Text);

            oObject.objAreaID = iCurrentAreaID;
            oObject.objVNUM = iVUM;
            oObject.ShortDesc = txtShortDescription.Text;

            // Insert or Update the room.
            if (iObjectID > 0)
            {
                oObject.objectID = iObjectID;
                oObject.UpdateObject();
            }
            else
            {
                oObject.AddObject();
            }
        }
        #endregion
    }
}
