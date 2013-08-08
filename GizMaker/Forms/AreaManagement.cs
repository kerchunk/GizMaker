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
    public partial class AreaManagement : Form
    {
        // Global Variables
        #region "global variables"
        public int iAreaID { get; set; }
        #endregion

        // Constructors
        #region "constructors"
        public AreaManagement()
        {
            InitializeComponent();

            PopulateAreas();
        }
        #endregion

        // Population
        #region "population"
        public void PopulateAreas()
        {
            // Populate the List of Areas.
            dgAreas.AutoGenerateColumns = true;
            dgAreas.DataSource = classes.area.GetAreas();
            dgAreas.DataMember = "Areas";
            dgAreas.Update();
        }
        #endregion

        // Events
        #region "button events"
        // Open Area Selection, return the Area ID of the selected row back to the main window.
        private void btnOpenArea_Click(object sender, EventArgs e)
        {
            int iSelected = 0;

            // Check which row is selected.
            foreach (DataGridViewRow dgvRow in dgAreas.Rows)
            {
                if (dgvRow.Selected)
                {
                    iSelected = dgvRow.Index;
                }
            }

            // Get the Area ID
            string strAreaID = dgAreas.Rows[iSelected].Cells["AreaID"].Value.ToString();

            iAreaID = Convert.ToInt32(strAreaID);

            // Hide Window.
            this.Close();
        }

        // Confirm, then Delete Area completely.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Really? Completely ERASE the Area?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int iSelected = 0;

                // Check which row is selected.
                foreach (DataGridViewRow dgvRow in dgAreas.Rows)
                {
                    if (dgvRow.Selected)
                    {
                        iSelected = dgvRow.Index;
                    }
                }

                // Get the Area ID
                string strAreaID = dgAreas.Rows[iSelected].Cells["AreaID"].Value.ToString();

                // Delete the Area.
                classes.area.DeleteArea(Convert.ToInt32(strAreaID));

                // Repopulate the List.
                PopulateAreas();
            }
        }

        // Close the window.
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Create a new area from the input field values.
        private void btnCreateArea_Click(object sender, EventArgs e)
        {
            if (txtAreaName.Text != "")
            {
                classes.area oArea = new classes.area();
                oArea.areaName = txtAreaName.Text.Trim();
                oArea.zoneNumber = Convert.ToInt32(txtZoneNumber.Text);
                oArea.startingVNUM = Convert.ToInt32(txtStartingVNUM.Text);
                oArea.AddArea();

                // Clear Controls.
                txtAreaName.Text = "";
                txtZoneNumber.Text = "";
                txtStartingVNUM.Text = "";

                // Repopulate the List.
                PopulateAreas();
            }
        }        
        #endregion
    }
}
