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
    public partial class MobManagement : Form
    {
        // Global Variables
        #region "global variables"
        public int iMobID { get; set; }
        int iCurrentAreaID = 0;
        #endregion

        // Constructors
        #region "constructors"
        public MobManagement(int iAreaID)
        {
            InitializeComponent();

            // Set Current Area ID.
            iCurrentAreaID = iAreaID;
        }

        public MobManagement(int iAreaID, int MobID)
        {
            InitializeComponent();

            // Set MobID to the local form value.
            iMobID = MobID;
            // Set Current Area ID.
            iCurrentAreaID = iAreaID;

            PopulateMobDetails(iMobID);
        }
        #endregion

        // Populate Mob Details to the Form
        #region "population"
        public void PopulateMobDetails(int iMobID)
        {
            classes.mob oMob = new classes.mob();
            oMob = classes.mob.GetMob(iMobID);

            txtVNUM.Text = oMob.mobVNUM.ToString();
            txtShortDescription.Text = oMob.ShortDesc;
        }
        #endregion

        // Form Events
        #region "events"
        // Close the Mob Detail Window.
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Save the Mob Details.
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save Mob.
            SaveMob();

            // Close Window.
            Close();
        }
        #endregion

        #region "methods"
        private void SaveMob()
        {
            // Create a new Mob object.
            classes.mob oMob = new classes.mob();

            // Populate Mob Details from Form.
            int iVUM = 0;
            if (txtVNUM.Text != "")
                iVUM = Convert.ToInt32(txtVNUM.Text);

            oMob.mobAreaID = iCurrentAreaID;
            oMob.mobVNUM = iVUM;
            oMob.ShortDesc = txtShortDescription.Text;

            // Insert or Update the room.
            if (iMobID > 0)
            {
                oMob.mobID = iMobID;
                oMob.UpdateMob();
            }
            else
            {
                oMob.AddMob();
            }
        }
        #endregion
    }
}
