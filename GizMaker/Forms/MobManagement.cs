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
        #endregion

        // Constructors
        #region "constructors"
        public MobManagement()
        {
            InitializeComponent();
        }

        public MobManagement(int iMobID)
        {
            InitializeComponent();

            PopulateMobDetails(iMobID);
        }
        #endregion

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
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
    }
}
