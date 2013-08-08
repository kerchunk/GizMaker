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
        #endregion

        // Constructors
        #region "constructors"
        public ObjectManagement()
        {
            InitializeComponent();
        }

        public ObjectManagement(int iObjectID)
        {
            InitializeComponent();

            PopulateObjectDetails(iObjectID);
        }
        #endregion

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
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
    }
}
