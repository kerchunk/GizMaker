﻿using System;
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
    public partial class Mapper : Form
    {
        // Global Variables.
        #region "global variables"
        // Colors
        Color clrBlank = Color.Gray;
        Color clrBlankNoGrid = Color.DimGray;
        Color clrBlankBorder = Color.Black;
        Color clrBlankBorderNoGrid = Color.DimGray;
        Color clrDefault = Color.DarkSlateGray;
        Color clrCurrent = Color.DarkSlateGray;
        Color clrUp = Color.Yellow;
        Color clrDown = Color.MediumSpringGreen;
        Color clrDoorDefault = Color.BurlyWood;
        Color clrDoorLocked = Color.DarkRed;
        Color clrHighlightSpawn = Color.OrangeRed;

        // Positioning 
        int iCurrentArea = 0;
        int iCurrentRoom = 0;
        int iCurrentX = 0;
        int iCurrentY = 0;
        int iCurrentZ = 0;

        // Toggles
        bool blnAutoMap = false;
        bool blnHighlightMobs = false;

        // String Messages
        string strErrorHeaderMsg = "You blew it.";
        string strCurrentRoomIndicator = "¥";
        #endregion"

        // On Load Event.
        #region "contructors"
        public Mapper()
        {
            InitializeComponent();

            // Set the default room color.
            clrCurrent = clrDefault;

            // Add Key Capture for Numpad.
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(KeyDown_Click);

            // Display Empty Grid.
            if (iCurrentArea > 0)
            {
                // Display the empty grid.
                DisplayGrid();

                // Display the Current Map.
                DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
            }
        }

        // Populate Area to the Grid.
        public void PopulateArea(int iAreaID)
        {
            // Create a new area object.
            classes.area oArea = new classes.area();

            // Populate the object by Area ID.
            oArea = classes.area.GetAreaByID(iAreaID);

            // Set the form controls with the Area Object details.
            lblAreaHeader.Text = oArea.areaName.ToString() + " [ #" + oArea.startingVNUM.ToString() + " ]";
            //lblRoomCount.Text = classes.area.GetRoomCountByID(iAreaID).ToString();

            // Populate Door Key List.
            PopulateKeyList();
        }

        // Populate Door Key List. 
        public void PopulateKeyList()
        {
            // Create a DataTable to source the Combobox.
            DataTable dtKeys = new DataTable();
            dtKeys.Columns.Add("ObjectID", typeof(string));
            dtKeys.Columns.Add("ShortDesc", typeof(string));

            // Get the list of Key Objects to Populate the Combobox.
            DataSet dsKeys = classes.c_object.GetAreaKeys(iCurrentArea);

            // Fill the DataTable with the list of Key Objects.
            dtKeys = dsKeys.Tables[0];

            // Add blank row to the top of the ComboBox.
            DataRow row = dtKeys.NewRow();
            row["ObjectID"] = "";
            row["ShortDesc"] = " < Add Key >";
            dtKeys.Rows.InsertAt(row, 0);

            // Push the List to the ComboBox.
            cboDoorKey.ValueMember = "ObjectID";
            cboDoorKey.DisplayMember = "ShortDesc";
            cboDoorKey.DataSource = dtKeys;
        }

        // Get Key Name.
        public string GetKeyName(int iKeyID)
        {
            string strKeyName = "";

            strKeyName = classes.door.GetKeyName_ByID(iCurrentArea, iKeyID);

            return strKeyName;
        }
        #endregion

        // Respond to selections from the MenuItem.
        #region "menu items"
        // Open Area Window
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display the Form to allow the User to select an Area.
            using (forms.AreaManagement selectArea = new forms.AreaManagement())
            {
                // Display the form.
                selectArea.ShowDialog();
                // Get the selected Area from the child form.
                int iArea = selectArea.iAreaID;

                if (iArea > 0)
                {
                    // Hide the Child form.
                    selectArea.Hide();

                    // Set the Area to the Page.
                    iCurrentArea = iArea;
                    lblAreaID.Text = iArea.ToString();

                    // Refresh the page and load the new Area content.
                    LoadArea(iArea);

                    // Open all Area Detail tabs.
                    tabControl.Enabled = true;
                }
            }
        }

        // Close Application
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Open "New Mob" Window.
        private void newMobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (iCurrentArea > 0)
            {
                // Display the Form to allow the User to select an Area.
                using (forms.MobManagement mobDetail = new forms.MobManagement(iCurrentArea))
                {
                    // Display the form.
                    mobDetail.ShowDialog();

                    // Get the saved Mob ID from the child form and refresh.
                    int iMob = mobDetail.iMobID;

                    if (iMob > 0)
                    {
                        // Refresh Mob Detail.
                    }

                    // Refresh Available Mob List.
                    PopulateMobsNotLoadingInRoom();
                }
            }
        }

        // Open "New Object" Windows.
        private void newObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (iCurrentArea > 0)
            {
                // Display the Form to allow the User to select an Area.
                using (forms.ObjectManagement objDetail = new forms.ObjectManagement(iCurrentArea))
                {
                    // Display the form.
                    objDetail.ShowDialog();
                }
            }
        }
        #endregion

        // Movement around the grid.
        #region "movement"
        // Move North on the Grid.
        void MoveNorth()
        {
            DisplayCurrentRoom(false);

            string strRoomName = string.Empty;
            int iNextButton = iCurrentRoom - 1;
            int iModNorthBorder = 0;
            bool blnRoomUpdated = false;
            bool blnLinkUpdated = false;

            // ## Move Room ##
            strRoomName = "room" + iNextButton;
            Button room = new Button();
            Button vPath = new Button();

            // Check if the Current Square is on the North border of the map.
            iModNorthBorder = (iCurrentRoom - 1) % 25;
            if (iModNorthBorder != 0 && iCurrentRoom != 1)
            {
                try
                {
                    // Get the Room Control and set focus.
                    Control[] btnRoom = this.Controls.Find(strRoomName, true);
                    if (btnRoom != null)
                    {
                        room = (Button)btnRoom[0];
                    }
                    room.Focus();

                    // If AutoMap is enabled, save the new room to the map.
                    if (blnAutoMap)
                    {
                        if (room.BackColor != clrCurrent)
                        {
                            room.BackColor = clrCurrent;
                            blnRoomUpdated = true;
                        }

                        // If AutoMap is enabled, Link the respective rooms.
                        Control[] btnVPath = this.Controls.Find("vpath" + iNextButton, true);
                        if (btnVPath != null)
                        {
                            vPath = (Button)btnVPath[0];

                            if (!vPath.Visible)
                            {
                                vPath.Visible = true;
                                blnLinkUpdated = true;
                            }
                        }
                    
                    }

                    // Save the Room Location
                    if (blnRoomUpdated || blnLinkUpdated)
                    {
                        SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
                    }

                    // Set the room Details to the form.
                    lblButtonName.Text = strRoomName.Replace("room", "");
                    iCurrentRoom = Convert.ToInt32(strRoomName.Replace("room", ""));
                    SetCurrentRoom();

                    // Set new Current Room.
                    iCurrentRoom = iNextButton;

                    // Update the Room Zoom View for the current room.
                    UpdateZoomView(room);
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                }
            }
            else
            {
                NewMap_North();
            }

            // Set the Current Room indicator.
            DisplayCurrentRoom(true);
        }

        // Move West on the Grid.
        void MoveWest()
        {
            // Set the Current Room indicator.
            DisplayCurrentRoom(false);

            string strRoomName = string.Empty;
            int iNextButton = iCurrentRoom - 25;
            bool blnRoomUpdated = false;
            bool blnLinkUpdated = false;

            // ## Move Room ##
            strRoomName = "room" + iNextButton;
            Button room = new Button();
            Button hPath = new Button();

            try
            {
                // Get the Room Control and set focus.
                Control[] btnRoom = this.Controls.Find(strRoomName, true);
                if (btnRoom != null)
                {
                    room = (Button)btnRoom[0];
                }
                room.Focus();

                // If AutoMap is enabled, save the new room to the map.
                if (blnAutoMap)
                {
                    if (room.BackColor != clrCurrent)
                    {
                        room.BackColor = clrCurrent;
                        blnRoomUpdated = true;
                    }

                    // If AutoMap is enabled, link the respective rooms.
                    Control[] btnHPath = this.Controls.Find("hpath" + iNextButton, true);
                    if (btnHPath != null)
                    {
                        hPath = (Button)btnHPath[0];

                        if (!hPath.Visible)
                        {
                            hPath.Visible = true;
                            blnLinkUpdated = true;
                        }
                    }                
                }

                // Save the Room Location
                if (blnRoomUpdated || blnLinkUpdated)
                {
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
                }

                // Set the room Details to the form.
                lblButtonName.Text = strRoomName.Replace("room", "");
                iCurrentRoom = Convert.ToInt32(strRoomName.Replace("room", ""));
                SetCurrentRoom();

                // Set new Current Room.
                iCurrentRoom = iNextButton;

                // Update the Room Zoom View for the current room.
                UpdateZoomView(room);

            }
            catch (Exception ex)
            {
                string strError = ex.Message;

                NewMap_West();
            }

            // Set the Current Room indicator.
            DisplayCurrentRoom(true);
        }

        // Move East on the Grid.
        void MoveEast()
        {
            // Set the Current Room indicator.
            DisplayCurrentRoom(false);

            string strRoomName = string.Empty;
            int iNextButton = iCurrentRoom + 25;
            bool blnRoomUpdated = false;
            bool blnLinkUpdated = false;

            // ## Move Room ##
            strRoomName = "room" + iNextButton;
            Button room = new Button();
            Button hPath = new Button();

            try
            {
                // Get the Room Control and set focus.
                Control[] btnRoom = this.Controls.Find(strRoomName, true);
                if (btnRoom != null)
                {
                    room = (Button)btnRoom[0];
                }
                room.Focus();

                // If AutoMap is enabled, save the new room to the map.
                if (blnAutoMap)
                {
                    if (room.BackColor != clrCurrent)
                    {
                        room.BackColor = clrCurrent;
                        blnRoomUpdated = true;
                    }

                    // If AutoMap is enabled, link the respective rooms.
                    Control[] btnHPath = this.Controls.Find("hpath" + iCurrentRoom, true);
                    if (btnHPath != null)
                    {
                        hPath = (Button)btnHPath[0];

                        if (!hPath.Visible)
                        {
                            hPath.Visible = true;
                            blnLinkUpdated = true;
                        }
                    }
                }

                // Save the Room Location
                if (blnRoomUpdated || blnLinkUpdated)
                {
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
                }

                // Set the room Details to the form.
                lblButtonName.Text = strRoomName.Replace("room", "");
                iCurrentRoom = Convert.ToInt32(strRoomName.Replace("room", ""));
                SetCurrentRoom();

                // Set new Current Room.
                iCurrentRoom = iNextButton;

                // Update the Room Zoom View for the current room.
                UpdateZoomView(room);

            }
            catch (Exception ex)
            {
                string strError = ex.Message;
                
                NewMap_East();
            }

            // Set the Current Room indicator.
            DisplayCurrentRoom(true);
        }

        // Move South on the Grid.
        void MoveSouth()
        {
            // Set the Current Room indicator.
            DisplayCurrentRoom(false);

            string strRoomName = string.Empty;
            int iNextButton = iCurrentRoom + 1;
            int iModSouthBorder = 0;
            bool blnRoomUpdated = false;
            bool blnLinkUpdated = false;

            // ## Move Room ##
            strRoomName = "room" + iNextButton;
            Button room = new Button();
            Button vPath = new Button();

            // Check if the Current Square is on the South border of the map.
            iModSouthBorder = (iCurrentRoom) % 25;
            if (iModSouthBorder != 0)
            {
                try
                {
                    // Get the Room Control and set focus.
                    Control[] btnRoom = this.Controls.Find(strRoomName, true);
                    if (btnRoom != null)
                    {
                        room = (Button)btnRoom[0];
                    }
                    room.Focus();

                    // If AutoMap is enabled, save the new room to the map.
                    if (blnAutoMap)
                    {
                        if (room.BackColor != clrCurrent)
                        {
                            room.BackColor = clrCurrent;
                            blnRoomUpdated = true;
                        }

                        // If AutoMap is enabled, link the respective rooms.
                        Control[] btnVPath = this.Controls.Find("vpath" + iCurrentRoom, true);
                        if (btnVPath != null)
                        {
                            vPath = (Button)btnVPath[0];

                            if (!vPath.Visible)
                            {
                                vPath.Visible = true;
                                blnLinkUpdated = true;
                            }
                        }
                    }
                    // Save the Room Location
                    if (blnRoomUpdated || blnLinkUpdated)
                    {
                        SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
                    }

                    // Set the room Details to the form.
                    lblButtonName.Text = strRoomName.Replace("room", "");
                    iCurrentRoom = Convert.ToInt32(strRoomName.Replace("room", ""));
                    SetCurrentRoom();

                    // Set new Current Room.
                    iCurrentRoom = iNextButton;

                    // Update the Room Zoom View for the current room.
                    UpdateZoomView(room);

                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                }
            }
            else
            {
                NewMap_South();
            }

            // Set the Current Room indicator.
            DisplayCurrentRoom(true);
        }

        // Move Up to a new Grid.
        void MoveUp()
        {
            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom.ToString();
            Control[] btnRoom = this.Controls.Find(strRoomName, true);

            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();

                if (blnAutoMap)
                {
                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, true, HasDownLink(iCurrentRoom));
                }
            }

            NewMap_Up();
        }

        // Move Down to a new Grid. 
        void MoveDown()
        {
            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom.ToString();
            Control[] btnRoom = this.Controls.Find(strRoomName, true);

            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();

                if (blnAutoMap)
                {
                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), true);
                }
            }

            NewMap_Down();
        }

        // Before Moving, determine if Textbox has focus (NumPad numerals)
        private bool TextBoxHasFocus()
        {
            bool blnHasFocus = false;

            // Loop through panel controls and check if a TextBox has focus.
            foreach (Control con in this.pnlRoomDetail.Controls)
            {
                if (con.Name.Contains("txt") || con.Name.Contains("cbo"))
                {
                    // Every control has a Focused property.
                    if (con.Focused == true)
                    {
                        blnHasFocus = true;
                    }
                }
            }

            // Loop through panel controls and check if a TextBox has focus.
            foreach (Control con in this.pnlGeneral.Controls)
            {
                if (con.Name.Contains("txt") || con.Name.Contains("cbo"))
                {
                    // Every control has a Focused property.
                    if (con.Focused == true)
                    {
                        blnHasFocus = true;
                    }
                }
            }

            // Loop through panel controls and check if a TextBox has focus.
            foreach (Control con in this.pnlDoor.Controls)
            {
                if (con.Name.Contains("txt") || con.Name.Contains("cbo"))
                {
                    // Every control has a Focused property.
                    if (con.Focused == true)
                    {
                        blnHasFocus = true;
                    }
                }
            }

            return blnHasFocus;
        }
        #endregion

        // Panel Movement.
        #region "movement panel"
        // User moved out of the East border.
        public void NewMap_East()
        {
            // Set new Coordinates.
            iCurrentX += 1;
            iCurrentRoom -= 725;

            // Clear Map Rooms.
            ClearRooms();

            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom.ToString();
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();

                // If AutoMap is enabled, save the new room to the map.
                if (blnAutoMap)
                {
                    room.BackColor = clrCurrent;

                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
                }
            }

            // Set Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }

        // User moved out of the North border.
        public void NewMap_North()
        {
            // Set new Coordinates.
            iCurrentY += 1;
            iCurrentRoom += 24;

            // Clear Map Rooms.
            ClearRooms();

            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom.ToString();
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();

                // If AutoMap is enabled, save the new room to the map.
                if (blnAutoMap)
                {
                    room.BackColor = clrCurrent;

                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
                }
            }

            // Set Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }

        // User moved out of the South border.
        public void NewMap_South()
        {
            // Set new Coordinates.
            iCurrentY -= 1;
            iCurrentRoom -= 24;

            // Clear Map Rooms.
            ClearRooms();

            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom.ToString();
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();

                // If AutoMap is enabled, save the new room to the map.
                if (blnAutoMap)
                {
                    room.BackColor = clrCurrent;

                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
                }
            }

            // Set Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }

        // User moved out of the West border.
        public void NewMap_West()
        {
            // Set new Coordinates.
            iCurrentX -= 1;
            iCurrentRoom += 725;

            // Clear Map Rooms.
            ClearRooms();

            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom.ToString();
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();

                // If AutoMap is enabled, save the new room to the map.
                if (blnAutoMap)
                {
                    room.BackColor = clrCurrent;

                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
                }
            }

            // Set Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }

        // User moved down.
        public void NewMap_Down()
        {
            // Set new Coordinates.
            iCurrentZ -= 1;

            // Clear Map Rooms.
            ClearRooms();

            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom.ToString();
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();

                // If AutoMap is enabled, save the new room to the map.
                if (blnAutoMap)
                {
                    room.BackColor = clrCurrent;

                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, true, HasDownLink(iCurrentRoom));
                }
            }

            // Set Current Room Details.
            SetCurrentRoom();

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);
        }

        // User moved up.
        public void NewMap_Up()
        {
            // Set new Coordinates.
            iCurrentZ += 1;

            // Clear Map Rooms.
            ClearRooms();

            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom.ToString();
            Control[] btnRoom = this.Controls.Find(strRoomName, true);

            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();

                // If AutoMap is enabled, save the new room to the map.
                if (blnAutoMap)
                {
                    room.BackColor = clrCurrent;

                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), true);
                }
            }

            // Set Current Room Details.
            SetCurrentRoom();

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);
        }

        // Shift Panel to the North.
        public void MovePanelNorth()
        {
            // Set new Coordinates.
            iCurrentY += 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom;
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();
            }

            // Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }

        // Shift Panel to the South.
        public void MovePanelSouth()
        {
            // Set new Coordinates.
            iCurrentY -= 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom; 
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();
            }

            // Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }

        // Shift Panel to the East.
        public void MovePanelEast()
        {
            // Set new Coordinates.
            iCurrentX += 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom; 
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();
            }

            // Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }

        // Shift Panel to the West.
        public void MovePanelWest()
        {
            // Set new Coordinates.
            iCurrentX -= 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom; 
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();
            }

            // Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }

        // Shift Panel Up.
        public void MovePanelUp()
        {
            // Set new Coordinates.
            iCurrentZ += 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom; 
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();
            }

            // Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }

        // Shift Panel Down.
        public void MovePanelDown()
        {
            // Set new Coordinates.
            iCurrentZ -= 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room" + iCurrentRoom; 
            Control[] btnRoom = this.Controls.Find(strRoomName, true);
            if (btnRoom != null)
            {
                room = (Button)btnRoom[0];
                room.Focus();
            }

            // Current Room Details.
            SetCurrentRoom();

            // Update the Room Zoom View for the current room.
            UpdateZoomView(room);

            // Display the Current Map.
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
        }
        #endregion

        // Event handling.
        #region "event handling"
        // Respond to Numpad directional keys and move around the grid.
        private void KeyDown_Click(object sender, KeyEventArgs e)
        {
            bool blnTXTHasFocus = false;
            blnTXTHasFocus = TextBoxHasFocus();
            if (!blnTXTHasFocus)
            {
                // North
                if (!e.Control && e.KeyCode == Keys.NumPad8)
                {
                    MoveNorth();
                }
                // Panel North
                if (e.Control && e.KeyCode == Keys.NumPad8)
                {
                    MovePanelNorth();
                }

                // West
                if (!e.Control && e.KeyCode == Keys.NumPad4)
                {
                    MoveWest();
                }
                // Panel West
                if (e.Control && e.KeyCode == Keys.NumPad4)
                {
                    MovePanelWest();
                }

                // East
                if (!e.Control && e.KeyCode == Keys.NumPad6)
                {
                    MoveEast();
                }
                // Panel East
                if (e.Control && e.KeyCode == Keys.NumPad6)
                {
                    MovePanelEast();
                }

                // South
                if (!e.Control && e.KeyCode == Keys.NumPad2)
                {
                    MoveSouth();
                }
                // Panel South
                if (e.Control && e.KeyCode == Keys.NumPad2)
                {
                    MovePanelSouth();
                }

                // Up
                if (!e.Control && e.KeyCode == Keys.NumPad9)
                {
                    MoveUp();
                }
                // Panel Up
                if (e.Control && e.KeyCode == Keys.NumPad9)
                {
                    MovePanelUp();
                }

                // Down
                if (!e.Control && e.KeyCode == Keys.NumPad3)
                {
                    MoveDown();
                }
                // Panel Down
                if (e.Control && e.KeyCode == Keys.NumPad3)
                {
                    MovePanelDown();
                }
            }
        }

        // Use single button click event to route all button clicks. 
        public void RoomClick(object sender, EventArgs e)
        {
            // Set the Current Room indicator.
            DisplayCurrentRoom(false);

            string strRoomName = string.Empty;

            // Get the sender button object.
            Button btnButton = (Button)sender;
            // Get the name of the button.
            strRoomName = btnButton.Name;

            iCurrentRoom = Convert.ToInt32(strRoomName.Replace("room", ""));
            SetCurrentRoom();

            if (blnAutoMap)
            {
                if (btnButton.BackColor == clrCurrent)
                {
                    // btnButton.BackColor = clrBlank;

                    // Remove the Room Location - don't do this on click
                    // RemoveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ);
                }
                else
                {
                    btnButton.BackColor = clrCurrent;

                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, false, false);
                }
            }

            // Update the Room Zoom View for the current room.
            UpdateZoomView(btnButton);

            // Set the Current Room indicator.
            DisplayCurrentRoom(true);
        }

        // Use single button click event to route all button clicks. 
        public void PathClick(object sender, EventArgs e)
        {
            string strRoomName = string.Empty;
            int iRoomNumber = 0;

            // Get the sender button object.
            Button btnButton = (Button)sender;
            // Get the name of the button.
            strRoomName = btnButton.Name;

            // ################################
            // Path On-Click Toggle:
            // ################################

            /*
            // Remove Link
            if (rdoRemoveLink.Checked)
            {
                // Hide Room Link.
                if (btnButton.Visible == true)
                {
                    btnButton.Visible = false;
                }
                else
                {
                    btnButton.Visible = true;
                }

                // Get Room Number from Control Name.
                iRoomNumber = Convert.ToInt32(strRoomName.Replace("hpath", "").Replace("vpath", ""));

                // Push to the East of the Removed Path.
                if (strRoomName.Contains("hpath"))
                {
                    iRoomNumber += 25;
                    strRoomName = "room" + iRoomNumber.ToString();

                    Button btnEast = (Button)sender;
                    Control[] btnRoomEast = this.Controls.Find(strRoomName, true);
                    if (btnRoomEast != null)
                    {
                        btnEast = (Button)btnRoomEast[0];
                    }
                    btnEast.Focus();
                    iCurrentRoom = iRoomNumber;

                    UpdateZoomView(btnEast);
                }

                // Push to the North of the Removed Path.
                if (strRoomName.Contains("vpath"))
                {
                    strRoomName = "room" + iRoomNumber.ToString();

                    Button btnNorth = (Button)sender;
                    Control[] btnRoomNorth = this.Controls.Find(strRoomName, true);
                    if (btnRoomNorth != null)
                    {
                        btnNorth = (Button)btnRoomNorth[0];
                    }
                    btnNorth.Focus();
                    iCurrentRoom = iRoomNumber;

                    UpdateZoomView(btnNorth);
                }

                lblButtonName.Text = strRoomName.Replace("room", "");
                Refresh();
            }

            // Create Door
            if (rdoCreateDoor.Checked)
            {
                // Turn Door to default door color.
                Button btnDoor = (Button)sender;
                Control[] btnDoorControl = this.Controls.Find(strRoomName, true);
                if (btnDoorControl != null)
                {
                    btnDoor = (Button)btnDoorControl[0];
                }
                btnDoor.BackColor = clrDoorLocked;            

                // Create Door Object at selected location.

            }
            */
        }

        // Set the Paint Color to Default.
        private void btnDefault_Click(object sender, EventArgs e)
        {
            clrCurrent = clrDefault;
        }

        // Open Blank Mob Detail Form.
        private void btnNewMob_Click(object sender, EventArgs e)
        {
            // Display the Form to allow the User to select an Area.
            using (forms.MobManagement mobDetail = new forms.MobManagement(iCurrentArea))
            {
                // Display the form.
                mobDetail.ShowDialog();

                // Get the saved Mob ID from the child form and refresh.
                int iMob = mobDetail.iMobID;

                if (iMob > 0)
                {
                    // Refresh Mob Detail.
                }

                // Refresh Available Mob List.
                PopulateMobsNotLoadingInRoom();
            }
        }

        // Open Mob Detail and Populate for selected Mob.
        private void btnFullDetail_Click(object sender, EventArgs e)
        {
            int iCurrentMob = -1;

            // Check which row is selected.
            iCurrentMob = Convert.ToInt32(lbSpawns.SelectedValue.ToString());

            if (iCurrentMob > -1)
            {
                // Display the Form to allow the User to Edit a Mob.
                using (forms.MobManagement mobDetail = new forms.MobManagement(iCurrentArea, iCurrentMob))
                {
                    // Display the form.
                    mobDetail.ShowDialog();

                    // Get the saved Mob ID from the child form and refresh.
                    int iMob = mobDetail.iMobID;

                    if (iMob > 0)
                    {
                        // Refresh Mob Detail.
                    }

                    // Refresh Mob Lists.
                    PopulateMobsLoadingInRoom();
                    PopulateMobsNotLoadingInRoom();
                }
            }
        }

        // Open Blank Object Detail Form.
        private void btnAddObject_Click(object sender, EventArgs e)
        {
            // Display the Form to allow the User to select an Area.
            using (forms.ObjectManagement objDetail = new forms.ObjectManagement(iCurrentArea))
            {
                // Display the form.
                objDetail.ShowDialog();
            }
        }

        // Highlight selected mob on the current panel.
        private void btnHighlightSpawns_Click(object sender, EventArgs e)
        {
            HighlightMobSpawns();
        }

        // Add the selected Mob to the Room Spawn list.
        private void btnAddSpawn_Click(object sender, EventArgs e)
        {
            int iCurrentMob = -1;

            // Get the selected Mob.
            if (cboAllMobs.SelectedIndex > 0)
                iCurrentMob = Convert.ToInt32(cboAllMobs.SelectedValue.ToString());

            if (iCurrentMob > 0)
            {
                // Call Add Method.
                classes.room.AddRoomSpawn(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, iCurrentMob);

                // Reset ComboBox.
                PopulateMobsLoadingInRoom();
                PopulateMobsNotLoadingInRoom();
            }
        }

        // Add the selected Object to the Mob Load list.
        private void btnAddLoad_Click(object sender, EventArgs e)
        {
            int iCurrentObj = -1;
            int iCurrentMob = -1;

            // Get the selected Mob.
            if (lbSpawns.SelectedIndex > -1)
                iCurrentMob = Convert.ToInt32(lbSpawns.SelectedValue.ToString());

            // Get the selected Object.
            if (cboAllObjects.SelectedIndex > 0)
                iCurrentObj = Convert.ToInt32(cboAllObjects.SelectedValue.ToString());

            if (iCurrentMob > 0 && iCurrentObj > 0)
            {
                // Call Add Method.
                classes.mob.AddMobLoad(iCurrentArea, iCurrentMob, iCurrentObj, iCurrentX, iCurrentY, iCurrentZ);

                // Reset ComboBox.
                PopulateObjectsLoadingOnMob(iCurrentMob);
                PopulateObjectsNotLoadingOnMob(iCurrentMob);
            }
        }

        // Open Object Detail and Populate for selected Mob.
        private void btnFullObjDetail_Click(object sender, EventArgs e)
        {
            int iCurrentObj = -1;
            int iCurrentMob = -1;

            // Check which row is selected.
            iCurrentObj = Convert.ToInt32(lbLoads.SelectedValue.ToString());

            if (iCurrentObj > -1)
            {
                // Display the Form to allow the User to Edit an Object.
                using (forms.ObjectManagement objDetail = new forms.ObjectManagement(iCurrentArea, iCurrentObj))
                {
                    // Display the form.
                    objDetail.ShowDialog();

                    // Get the saved Mob ID from the child form and refresh.
                    int iObjectID = objDetail.iObjectID;

                    if (iObjectID > 0)
                    {
                        // Refresh Mob Detail.
                    }

                    if (lbSpawns.SelectedValue.ToString() != "")
                    {
                        // Check which row is selected.
                        iCurrentMob = Convert.ToInt32(lbSpawns.SelectedValue.ToString());

                        PopulateObjectsLoadingOnMob(iCurrentMob);
                        PopulateObjectsNotLoadingOnMob(iCurrentMob);
                    }
                }
            }
        }

        // Set AutoMap Flag.
        private void chkAutoMap_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoMap.Checked == true)
            {
                blnAutoMap = true;
            }
            else
            {
                blnAutoMap = false;
            }
        }

        // Mob Selected in "Mobs Currently Loading in Room" grid.
        private void dgMobsInRoom_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int iCurrentMob = -1;

            // Check which row is selected.
            iCurrentMob = Convert.ToInt32(lbSpawns.SelectedValue.ToString());

            if (iCurrentMob > -1)
            {
                // Populate Objects associated with selected Mob. 
                PopulateObjectsLoadingOnMob(iCurrentMob);
                // Populate Objects not associated with selected Mob.
                PopulateObjectsNotLoadingOnMob(iCurrentMob);
                // Set Mob Details on Mob Tab.
                classes.mob oMob = classes.mob.GetMob(iCurrentMob);
                lblQuickMobName.Text = oMob.ShortDesc;
                lblQuickMobVNUM.Text = oMob.mobVNUM.ToString();
                lblQuickMobID.Text = oMob.mobID.ToString();
            }
        }

        // Update Selected Mob from Spawn List.
        private void lbSpawns_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCurrentMob = -1;

            // Check which row is selected.
            iCurrentMob = Convert.ToInt32(lbSpawns.SelectedValue.ToString());

            if (iCurrentMob > -1)
            {
                // Populate Objects associated with selected Mob. 
                PopulateObjectsLoadingOnMob(iCurrentMob);
                // Populate Objects not associated with selected Mob.
                PopulateObjectsNotLoadingOnMob(iCurrentMob);
                // Set Mob Details on Mob Tab.
                classes.mob oMob = classes.mob.GetMob(iCurrentMob);
                lblQuickMobName.Text = oMob.ShortDesc;
                lblQuickMobVNUM.Text = oMob.mobVNUM.ToString();
                lblQuickMobID.Text = oMob.mobID.ToString();

                lbLoads.Enabled = true;
            }
        }

        // Update Selected Object from Load List.
        private void lbLoads_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Remove Currently-Selected Spawn from the Selected Room.
        private void btnRemoveSpawn_Click(object sender, EventArgs e)
        {
            int iCurrentMob = -1;

            // Check which row is selected.
            iCurrentMob = Convert.ToInt32(lbSpawns.SelectedValue.ToString());

            if (iCurrentMob > -1)
            {
                // Remove the Spawn from the Room.
                classes.mob.RemoveMobLoads(iCurrentArea, iCurrentMob, iCurrentX, iCurrentY, iCurrentZ);
                // Remove the Spawn from the Room.
                classes.room.RemoveRoomSpawn(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, iCurrentMob);
                // Populate Spawns associated with selected Room. 
                PopulateMobsLoadingInRoom();
                // Populate Spawns not associated with selected Room.
                PopulateMobsNotLoadingInRoom();
            }
        }

        // Remove Currently-Selected Load from the Selected Mob.
        private void btnRemoveLoad_Click(object sender, EventArgs e)
        {
            int iCurrentMob = -1;
            int iCurrentObj = -1;

            // Check which row is selected.
            iCurrentMob = Convert.ToInt32(lbSpawns.SelectedValue.ToString());
            // Check which row is selected.
            iCurrentObj = Convert.ToInt32(lbLoads.SelectedValue.ToString());

            if (iCurrentMob > 0 && iCurrentObj > 0)
            {
                // Remove the Spawn from the Room.
                classes.mob.RemoveMobLoad(iCurrentArea, iCurrentMob, iCurrentObj, iCurrentX, iCurrentY, iCurrentZ);
                // Populate Spawns associated with selected Room. 
                PopulateMobsLoadingInRoom();
                // Populate Spawns not associated with selected Room.
                PopulateMobsNotLoadingInRoom();
                // Populate Loads associated with selected Mob. 
                PopulateObjectsLoadingOnMob(iCurrentMob);
                // Populate Loads not associated with selected Mob.
                PopulateObjectsNotLoadingOnMob(iCurrentMob);
            }
        }

        // Save Door Click
        private void btnSaveDoor_Click(object sender, EventArgs e)
        {
            SaveDoor();
        }

        // Delete Door Click
        private void btnDeleteDoor_Click(object sender, EventArgs e)
        {
            DeleteDoor();
        }

        // Door North Click
        private void btnDoorNorth_Click(object sender, EventArgs e)
        {
            classes.door oDoor = classes.door.GetDoor_ByRoomAndDirection(iCurrentArea, iCurrentRoom, "north", iCurrentX, iCurrentY, iCurrentZ);

            lblDoorID.Text = oDoor.doorID.ToString();
            txtDoorVNUM.Text = oDoor.VNUM.ToString();
            txtDoorKeywords.Text = oDoor.keywords;
            cboDirection.SelectedIndex = cboDirection.FindString(oDoor.direction);
            cboDoorType.SelectedIndex = cboDoorType.FindString(oDoor.doorType);
            cboDoorKey.SelectedIndex = cboDoorKey.FindString(GetKeyName(oDoor.keyVNUM));
        }

        // Door South Click
        private void btnDoorSouth_Click(object sender, EventArgs e)
        {
            classes.door oDoor = classes.door.GetDoor_ByRoomAndDirection(iCurrentArea, iCurrentRoom, "south", iCurrentX, iCurrentY, iCurrentZ);

            lblDoorID.Text = oDoor.doorID.ToString();
            txtDoorVNUM.Text = oDoor.VNUM.ToString();
            txtDoorKeywords.Text = oDoor.keywords;
            cboDirection.SelectedIndex = cboDirection.FindString(oDoor.direction);
            cboDoorType.SelectedIndex = cboDoorType.FindString(oDoor.doorType);
            cboDoorKey.SelectedIndex = cboDoorKey.FindString(GetKeyName(oDoor.keyVNUM));
        }

        // Door East Click
        private void btnDoorEast_Click(object sender, EventArgs e)
        {
            classes.door oDoor = classes.door.GetDoor_ByRoomAndDirection(iCurrentArea, iCurrentRoom, "east", iCurrentX, iCurrentY, iCurrentZ);

            lblDoorID.Text = oDoor.doorID.ToString();
            txtDoorVNUM.Text = oDoor.VNUM.ToString();
            txtDoorKeywords.Text = oDoor.keywords;
            cboDirection.SelectedIndex = cboDirection.FindString(oDoor.direction);
            cboDoorType.SelectedIndex = cboDoorType.FindString(oDoor.doorType);
            cboDoorKey.SelectedIndex = cboDoorKey.FindString(GetKeyName(oDoor.keyVNUM));
        }

        // Door West Click
        private void btnDoorWest_Click(object sender, EventArgs e)
        {
            classes.door oDoor = classes.door.GetDoor_ByRoomAndDirection(iCurrentArea, iCurrentRoom, "west", iCurrentX, iCurrentY, iCurrentZ);

            lblDoorID.Text = oDoor.doorID.ToString();
            txtDoorVNUM.Text = oDoor.VNUM.ToString();
            txtDoorKeywords.Text = oDoor.keywords;
            cboDirection.SelectedIndex = cboDirection.FindString(oDoor.direction);
            cboDoorType.SelectedIndex = cboDoorType.FindString(oDoor.doorType);
            cboDoorKey.SelectedIndex = cboDoorKey.FindString(GetKeyName(oDoor.keyVNUM));
        }

        // Door Up Click
        private void btnDoorUp_Click(object sender, EventArgs e)
        {
            classes.door oDoor = classes.door.GetDoor_ByRoomAndDirection(iCurrentArea, iCurrentRoom, "up", iCurrentX, iCurrentY, iCurrentZ);

            lblDoorID.Text = oDoor.doorID.ToString();
            txtDoorVNUM.Text = oDoor.VNUM.ToString();
            txtDoorKeywords.Text = oDoor.keywords;
            cboDirection.SelectedIndex = cboDirection.FindString(oDoor.direction);
            cboDoorType.SelectedIndex = cboDoorType.FindString(oDoor.doorType);
            cboDoorKey.SelectedIndex = cboDoorKey.FindString(GetKeyName(oDoor.keyVNUM));
        }

        // Door Down Click
        private void btnDoorDown_Click(object sender, EventArgs e)
        {
            classes.door oDoor = classes.door.GetDoor_ByRoomAndDirection(iCurrentArea, iCurrentRoom, "down", iCurrentX, iCurrentY, iCurrentZ);

            lblDoorID.Text = oDoor.doorID.ToString();
            txtDoorVNUM.Text = oDoor.VNUM.ToString();
            txtDoorKeywords.Text = oDoor.keywords;
            cboDirection.SelectedIndex = cboDirection.FindString(oDoor.direction);
            cboDoorType.SelectedIndex = cboDoorType.FindString(oDoor.doorType);
            cboDoorKey.SelectedIndex = cboDoorKey.FindString(GetKeyName(oDoor.keyVNUM));
        }

        // Save Room Details
        private void btnSaveRoom_Click(object sender, EventArgs e)
        {
            SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, this.HasUpLink(iCurrentRoom), this.HasDownLink(iCurrentRoom));
        }
        #endregion

        // Current room details.
        #region "current room properties"
        // Determine if Current Room has a North Room Link.
        bool HasNorthLink(int iCurrentRoom)
        {
            bool blnHasNorthLink = false;

            int iNorth = 0;
            iNorth = iCurrentRoom - 1;
            Button btnNorth = null;

            // Get the Link Image north fo the Current Room links.
            Control[] btnNorthLink = this.Controls.Find("vPath" + iNorth.ToString(), true);
            if (btnNorthLink != null)
            {
                try
                {
                    btnNorth = (Button)btnNorthLink[0];

                    // Display or Hide the North Link depending on Current Room links.
                    if (btnNorth.Visible)
                    {
                        blnHasNorthLink = true;
                    }
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                }
            }

            return blnHasNorthLink;
        }

        // Determine if Current Room has a South Room Link.
        bool HasSouthLink(int iCurrentRoom)
        {
            bool blnHasSouthLink = false;

            int iSouth = 0;
            iSouth = iCurrentRoom;
            Button btnSouth = null;

            // Get the Link Image South fo the Current Room links.
            Control[] btnSouthLink = this.Controls.Find("vPath" + iSouth.ToString(), true);
            if (btnSouthLink != null)
            {
                try
                {
                    btnSouth = (Button)btnSouthLink[0];

                    // Display or Hide the North Link depending on Current Room links.
                    if (btnSouth.Visible)
                    {
                        blnHasSouthLink = true;
                    }
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                }
            }

            return blnHasSouthLink;
        }

        // Determine if Current Room has a East Room Link.
        bool HasEastLink(int iCurrentRoom)
        {
            bool blnHasEastLink = false;

            int iEast = 0;
            iEast = iCurrentRoom;
            Button btnEast = null;

            // Get the Link Image East fo the Current Room links.
            Control[] btnEastLink = this.Controls.Find("hPath" + iEast.ToString(), true);
            if (btnEastLink != null)
            {
                try
                {
                    btnEast = (Button)btnEastLink[0];

                    // Display or Hide the North Link depending on Current Room links.
                    if (btnEast.Visible)
                    {
                        blnHasEastLink = true;
                    }
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                }
            }

            return blnHasEastLink;
        }

        // Determine if Current Room has a West Room Link.
        bool HasWestLink(int iCurrentRoom)
        {
            bool blnHasWestLink = false;

            int iWest = 0;
            iWest = iCurrentRoom - 25;
            Button btnWest = null;

            // Get the Link Image West fo the Current Room links.
            Control[] btnWestLink = this.Controls.Find("hPath" + iWest.ToString(), true);
            if (btnWestLink != null)
            {
                try
                {
                    btnWest = (Button)btnWestLink[0];

                    // Display or Hide the North Link depending on Current Room links.
                    if (btnWest.Visible)
                    {
                        blnHasWestLink = true;
                    }
                }
                catch(Exception ex)
                {
                    string strError = ex.Message;
                }
            }

            return blnHasWestLink;
        }

        // Determine if Current Room has an Up Room Link.
        bool HasUpLink(int iCurrentRoom)
        {
            bool blnHasUpLink = false;

            int iUp = 0;
            iUp = iCurrentRoom;

            classes.room oRoom = classes.room.GetRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ);
            if (!blnHasUpLink && oRoom.hasExitUp)
            {
                blnHasUpLink = true;
            }

            return blnHasUpLink;
        }

        // Determine if Current Room has a Down Room Link.
        bool HasDownLink(int iCurrentRoom)
        {
            bool blnHasDownLink = false;

            int iDown = 0;
            iDown = iCurrentRoom;

            classes.room oRoom = classes.room.GetRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ);
            if (!blnHasDownLink && oRoom.hasExitDown)
            {
                blnHasDownLink = true;
            }

            return blnHasDownLink;
        }
        #endregion

        // Room DB-related functions.
        #region "room functions"
        // Clear all rooms.
        private void ClearRooms()
        {
            int iRoom = 1;
            string strRoomName = "";
            Button room = new Button();
            Button hpath = new Button();
            Button vpath = new Button();

            // Walk each room and clear the display.
            while (iRoom <= 750)
            {
                strRoomName = "room" + iRoom;
                Control[] btnRoom = this.Controls.Find(strRoomName, true);
                if (btnRoom != null)
                {
                    room = (Button)btnRoom[0];
                }
                room.BackColor = clrBlank;
                room.FlatAppearance.BorderColor = clrBlankBorder;

                strRoomName = "hpath" + iRoom;
                Control[] btnHPath = this.Controls.Find(strRoomName, true);
                if (btnHPath != null)
                {
                    hpath = (Button)btnHPath[0];
                }
                hpath.Visible = false;

                strRoomName = "vpath" + iRoom;
                Control[] btnVPath = this.Controls.Find(strRoomName, true);
                if (btnVPath != null)
                {
                    vpath = (Button)btnVPath[0];
                }
                vpath.Visible = false;

                iRoom++;
            }
        }

        // Display the empty background grid to the form.
        public void DisplayGrid()
        {
            int iRoomLeft = 1;
            int iRoomTop = 0;
            int iButtonName = 1;

            // Draw a each column on the form. //30
            for (int col = 0; col < 30; col++)
            {
                // Draw a single column on the form. //26
                for (iRoomTop = 1; iRoomTop < 26; iRoomTop++)
                {
                    // ## MAIN ROOMS ##
                    // Create a new Default Button to draw to the form.
                    Button room = new Button();
                    room.Location = new Point(30 * iRoomLeft, 30 * iRoomTop + 25);
                    room.Width = 20;
                    room.Height = 20;
                    room.BackColor = clrBlank;
                    room.FlatAppearance.BorderColor = clrBlankBorder;
                    room.FlatStyle = FlatStyle.Flat;
                    room.FlatAppearance.BorderSize = 1;
                    room.Name = "room" + iButtonName.ToString();
                    room.Click += new EventHandler(RoomClick);
                    // Add the button to the form.
                    this.Controls.Add(room);

                    // ## HORIZONTAL PATHS ##
                    Button hPath = new Button();
                    hPath.Location = new Point(30 * iRoomLeft + 20, 30 * iRoomTop + 33);
                    hPath.Width = 10;
                    hPath.Height = 5;
                    hPath.BackColor = Color.Black;
                    hPath.FlatStyle = FlatStyle.Flat;
                    hPath.FlatAppearance.BorderSize = 0;
                    hPath.Visible = false;
                    hPath.Name = "hpath" + iButtonName.ToString();
                    hPath.Click += new EventHandler(PathClick);
                    // Add the button to the form.
                    this.Controls.Add(hPath);

                    // ## Verticle PATHS ##
                    Button vPath = new Button();
                    vPath.Location = new Point(30 * iRoomLeft + 8, 30 * iRoomTop + 45);
                    vPath.Width = 5;
                    vPath.Height = 10;
                    vPath.BackColor = Color.Black;
                    vPath.FlatStyle = FlatStyle.Flat;
                    vPath.FlatAppearance.BorderSize = 0;
                    vPath.Visible = false;
                    vPath.Name = "vpath" + iButtonName.ToString();
                    vPath.Click += new EventHandler(PathClick);
                    // Add the button to the form.
                    this.Controls.Add(vPath);

                    iButtonName++;
                }

                // Increment position counters.
                iRoomTop = 0;
                iRoomLeft += 1;
            }
        }

        // Display all of the saved rooms on a Panel.
        public void DisplayPanelRooms(int iAreaID, int iCoordX, int iCoordY, int iCoordZ)
        {
            classes.room[] rooms = new classes.room[800];
            Button btnRoom = new Button();

            // Get a collection of the rooms on the current panel.
            rooms = classes.room.GetPanelRooms(iAreaID, iCoordX, iCoordY, iCoordZ);

            foreach (classes.room oRoom in rooms)
            {
                if (oRoom != null)
                {
                    // Get the Button at the specified location.
                    Control[] ctrlRoom = this.Controls.Find("room" + oRoom.roomNumber, true);
                    if (btnRoom != null)
                    {
                        btnRoom = (Button)ctrlRoom[0];
                        btnRoom.BackColor = clrCurrent;
                        btnRoom.FlatAppearance.BorderColor = clrBlankBorder;
                    }

                    // Color Exits where appropriate.
                    // ## North ##
                    if (oRoom.hasExitNorth)
                    {
                        // Get the Button at the specified location.
                        Control[] ctrlNorth = this.Controls.Find("vpath" + (oRoom.roomNumber - 1).ToString(), true);
                        Button btnNorth = new Button();
                        if (btnNorth != null)
                        {
                            btnNorth = (Button)ctrlNorth[0];
                            btnNorth.Visible = true;
                            btnNorth.BackColor = Color.Black;
                        }
                    }

                    // ## South ##
                    if (oRoom.hasExitSouth)
                    {
                        // Get the Button at the specified location.
                        Control[] ctrlSouth = this.Controls.Find("vpath" + (oRoom.roomNumber).ToString(), true);
                        Button btnSouth = new Button();
                        if (btnSouth != null)
                        {
                            btnSouth = (Button)ctrlSouth[0];
                            btnSouth.Visible = true;
                            btnSouth.BackColor = Color.Black;
                        }
                    }

                    // ## East ##
                    if (oRoom.hasExitEast)
                    {
                        // Get the Button at the specified location.
                        Control[] ctrlEast = this.Controls.Find("hpath" + (oRoom.roomNumber).ToString(), true);
                        Button btnEast = new Button();
                        if (btnEast != null)
                        {
                            btnEast = (Button)ctrlEast[0];
                            btnEast.Visible = true;
                            btnEast.BackColor = Color.Black;
                        }
                    }

                    // ## West ##
                    if (oRoom.hasExitWest)
                    {
                        // Get the Button at the specified location.
                        Control[] ctrlWest = this.Controls.Find("hpath" + (oRoom.roomNumber - 25).ToString(), true);
                        Button btnWest = new Button();
                        if (btnWest != null)
                        {
                            btnWest = (Button)ctrlWest[0];
                            btnWest.Visible = true;
                            btnWest.BackColor = Color.Black;
                        }
                    }
                }
            }

            // Display Up Rooms. 
            classes.room[] RoomsUp = new classes.room[750];
            RoomsUp = classes.room.GetRoomsWithUpExit(iAreaID, iCoordX, iCoordY, iCoordZ);
            // Loop through Up Rooms and change border colors on map. 
            foreach (classes.room RoomUp in RoomsUp)
            {
                if (RoomUp != null)
                {
                    Control[] ctrlUp = this.Controls.Find("room" + RoomUp.roomNumber.ToString(), true);
                    Button btnUp = new Button();
                    if (btnUp != null)
                    {
                        btnUp = (Button)ctrlUp[0];
                        btnUp.FlatAppearance.BorderColor = clrUp;
                    }
                }
            }

            // Display Down Rooms. 
            classes.room[] RoomsDown = new classes.room[750];
            RoomsDown = classes.room.GetRoomsWithDownExit(iAreaID, iCoordX, iCoordY, iCoordZ);
            // Loop through Down Rooms and change border colors on map. 
            foreach (classes.room RoomDown in RoomsDown)
            {
                if (RoomDown != null)
                {
                    Control[] ctrlDown = this.Controls.Find("room" + RoomDown.roomNumber.ToString(), true);
                    Button btnDown = new Button();
                    if (btnDown != null)
                    {
                        btnDown = (Button)ctrlDown[0];
                        btnDown.FlatAppearance.BorderColor = clrDown;
                    }
                }
            }

            // Display Doors.
            classes.door[] doors = new classes.door[250];
            doors = classes.door.GetPanelDoors(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
            // Loop through Doors and Display on Panel.
            foreach (classes.door oDoor in doors)
            {
                if (oDoor != null)
                {
                    switch (oDoor.direction.ToLower())
                    {
                        case "north":
                            // Get the Button at the specified location.
                            Control[] ctrlNorth = this.Controls.Find("vpath" + (oDoor.roomNumber - 1).ToString(), true);
                            Button btnNorth = new Button();
                            if (btnNorth != null)
                            {
                                btnNorth = (Button)ctrlNorth[0];
                                btnNorth.Visible = true;
                                if (oDoor.doorType == "Not Pickable")
                                    btnNorth.BackColor = clrDoorLocked;
                                else
                                    btnNorth.BackColor = clrDoorDefault;
                            }
                            break;
                        case "south":
                            // Get the Button at the specified location.
                            Control[] ctrlSouth = this.Controls.Find("vpath" + (oDoor.roomNumber).ToString(), true);
                            Button btnSouth = new Button();
                            if (btnSouth != null)
                            {
                                btnSouth = (Button)ctrlSouth[0];
                                btnSouth.Visible = true;
                                if (oDoor.doorType == "Not Pickable")
                                    btnSouth.BackColor = clrDoorLocked;
                                else
                                    btnSouth.BackColor = clrDoorDefault;
                            }
                            break;
                        case "east":
                            // Get the Button at the specified location.
                            Control[] ctrlEast = this.Controls.Find("hpath" + (oDoor.roomNumber).ToString(), true);
                            Button btnEast = new Button();
                            if (btnEast != null)
                            {
                                btnEast = (Button)ctrlEast[0];
                                btnEast.Visible = true;
                                btnEast.BackColor = clrDoorDefault;
                                if (oDoor.doorType == "Not Pickable")
                                    btnEast.BackColor = clrDoorLocked;
                                else
                                    btnEast.BackColor = clrDoorDefault;
                            }
                            break;
                        case "west":
                            // Get the Button at the specified location.
                            Control[] ctrlWest = this.Controls.Find("hpath" + (oDoor.roomNumber - 25).ToString(), true);
                            Button btnWest = new Button();
                            if (btnWest != null)
                            {
                                btnWest = (Button)ctrlWest[0];
                                btnWest.Visible = true;
                                if (oDoor.doorType == "Not Pickable")
                                    btnWest.BackColor = clrDoorLocked;
                                else
                                    btnWest.BackColor = clrDoorDefault;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            //Display Current Room.
            DisplayCurrentRoom(true);
        }

        // Fully load a new (0, 0, 0) Mapper screen for an AreaID.
        public void LoadArea(int iAreaID)
        {
            iCurrentRoom = 1;

            lblAreaID.Text = iAreaID.ToString();
            if (iAreaID > 0)
            {
                PopulateArea(iAreaID);
            }

            // Set the default room color.
            clrCurrent = clrDefault;

            // Set Legend Colors
            SetLegendColors();

            // Display Empty Grid.
            if (iCurrentArea > 0)
            {
                DisplayGrid();

                // Display the Current Map.
                DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
            }
        }
        
        // Save the current room.
        public void SaveRoom(int iAreaID, int iRoomNumber, int iCoordX, int iCoordY, int iCoordZ, bool HasExitUp, bool HasExitDown)
        {
            if (iRoomNumber > 0)
            {
                // Create a Room Object.
                classes.room oRoom = new classes.room();

                // Populate the Room Object from the form.
                oRoom.roomAreaID = iAreaID;
                oRoom.roomNumber = iCurrentRoom;

                oRoom.roomName = txtRoomName.Text;
                if (txtVNUM.Text != "")
                    oRoom.VNUM = Convert.ToInt32(txtVNUM.Text);
                oRoom.sector = cboSector.Items[cboSector.SelectedIndex].ToString();
                oRoom.description = txtRoomDescription.Text;
                oRoom.extraKeywords = txtExtraDescKeywords.Text;
                oRoom.extraDescription = txtExtraDescription.Text;
                oRoom.exitNorthDesc = txtNorthExit.Text;
                oRoom.exitSouthDesc = txtSouthExit.Text;
                oRoom.exitEastDesc = txtEastExit.Text;
                oRoom.exitWestDesc = txtWestExit.Text;
                oRoom.exitUpDesc = txtUpExit.Text;
                oRoom.exitDownDesc = txtDownExit.Text;
                oRoom.coordX = iCoordX;
                oRoom.coordY = iCoordY;
                oRoom.coordZ = iCoordZ;
                oRoom.hasExitNorth = HasNorthLink(iCurrentRoom);
                oRoom.hasExitSouth = HasSouthLink(iCurrentRoom);
                oRoom.hasExitEast = HasEastLink(iCurrentRoom);
                oRoom.hasExitWest = HasWestLink(iCurrentRoom);
                oRoom.hasExitUp = HasExitUp;
                oRoom.hasExitDown = HasExitDown;

                // Insert or Update the room.
                if (oRoom.Exists())
                {
                    oRoom.UpdateRoom();
                }
                else
                {
                    oRoom.AddRoom();
                }
            }

            // Refresh Room Count.
            //lblRoomCount.Text = classes.area.GetRoomCountByID(iAreaID).ToString();
        }

        // Remove the current room.
        public void RemoveRoom(int iAreaID, int iRoomNumber, int iCoordX, int iCoordY, int iCoordZ)
        {
            // Create a Room Object.
            classes.room oRoom = new classes.room();

            // Populate the Room from the Form.
            oRoom.roomAreaID = iAreaID;
            oRoom.roomNumber = iCurrentRoom;
            oRoom.coordX = iCoordX;
            oRoom.coordY = iCoordY;
            oRoom.coordZ = iCoordZ;

            // Delete the room.
            oRoom.RemoveRoom();

            // Refresh Room Count.
            //lblRoomCount.Text = classes.area.GetRoomCountByID(iAreaID).ToString();
        }

        // Save current room door.
        public void SaveDoor()
        {
            if (txtDoorVNUM.Text != "" && txtDoorKeywords.Text != "" && cboDirection.SelectedIndex > -1 && cboDoorType.SelectedIndex > -1)
            {
                // Populate a Door Object with data from the Form.
                classes.door oDoor = new classes.door();
                if (lblDoorID.Text != "")
                    oDoor.doorID = Convert.ToInt32(lblDoorID.Text);
                oDoor.areaID = iCurrentArea;
                oDoor.roomNumber = iCurrentRoom;
                oDoor.VNUM = Convert.ToInt32(txtDoorVNUM.Text);
                oDoor.keywords = txtDoorKeywords.Text;
                oDoor.direction = cboDirection.Items[cboDirection.SelectedIndex].ToString();
                oDoor.doorType = cboDoorType.Items[cboDoorType.SelectedIndex].ToString();
                if (cboDoorKey.SelectedIndex > 0)
                    oDoor.keyVNUM = Convert.ToInt32(cboDoorKey.SelectedValue.ToString());
                oDoor.coordX = iCurrentX;
                oDoor.coordY = iCurrentY;
                oDoor.coordZ = iCurrentZ;

                // Populate the Door Mirror Object with data from the Door.
                classes.door oDoorMirror = new classes.door();
                oDoorMirror = classes.door.GetDoorMirror(oDoor, iCurrentRoom);

                // Check if Door is a Duplicate (by Room and Direction)
                if (!oDoor.DoorIsDuplicate())
                {
                    // If Door is not a Duplicate, Insert/Update.
                    if (lblDoorID.Text != "")
                    {
                        // Update Door.
                        oDoor.doorID = Convert.ToInt32(lblDoorID.Text);
                        oDoor.UpdateDoor();

                        // Get Door Mirror.
                        int iDoorMirrorID = 0;
                        iDoorMirrorID = classes.door.GetDoorMirrorID(oDoorMirror, iCurrentRoom);
                        if (iDoorMirrorID > 0)
                        {
                            oDoorMirror.doorID = iDoorMirrorID;
                            oDoorMirror.UpdateDoor();
                        }
                    }
                    else
                    {
                        // Add Door.
                        oDoor.AddDoor();
                        // Add Door Mirror.
                        oDoorMirror.AddDoor();
                    }
                }
                else
                {
                    MessageBox.Show("A door already exists in that direction!", strErrorHeaderMsg, MessageBoxButtons.OK);
                }

                // Get the Current Room in Zoom View.
                Control[] ctrlRoom = this.Controls.Find("room" + iCurrentRoom.ToString(), true);
                Button btnRoom = new Button();
                btnRoom = (Button)ctrlRoom[0];

                // Redraw the Panel and refresh Zoom View to reflect the Door Save.
                ClearDoorSection();
                DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
                UpdateZoomView(btnRoom);
            }
            else
                MessageBox.Show("The door needs more info!", strErrorHeaderMsg, MessageBoxButtons.OK);
        }

        // Delete the current door and its mirror.
        public void DeleteDoor()
        {
            if (lblDoorID.Text != "")
            {
                // Populate a Door Object with data from the Form.
                classes.door oDoor = new classes.door();
                oDoor = classes.door.GetDoorByID(Convert.ToInt32(lblDoorID.Text));

                // Populate the Door Mirror Object with data from the Door.
                classes.door oDoorMirror = new classes.door();
                oDoorMirror = classes.door.GetDoorMirror(oDoor, iCurrentRoom);
                oDoorMirror.doorID = classes.door.GetDoorMirrorID(oDoorMirror, iCurrentRoom);

                // Delete Doors. 
                oDoor.DeleteDoor();
                oDoorMirror.DeleteDoor();
            }
            else
                MessageBox.Show("You don't have a door selected!", strErrorHeaderMsg, MessageBoxButtons.OK);

            // Get the Current Room in Zoom View.
            Control[] ctrlRoom = this.Controls.Find("room" + iCurrentRoom.ToString(), true);
            Button btnRoom = new Button();
            btnRoom = (Button)ctrlRoom[0];

            // Redraw the Panel and refresh Zoom View to reflect the Door Save.
            ClearDoorSection();
            DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
            UpdateZoomView(btnRoom);
        }

        // Show or Hide "Current Room" Indicator.
        public void DisplayCurrentRoom(bool blnDisplay)
        {
            // Set the Current Room indicator.
            Control[] currRoom = this.Controls.Find("room" + iCurrentRoom, true);
            Button btnRoom = new Button();
            if (btnRoom != null)
            {
                btnRoom = (Button)currRoom[0];

                if (blnDisplay)
                {
                    btnRoom.Text = strCurrentRoomIndicator;
                    btnRoom.ForeColor = Color.White;
                }
                else
                {
                    btnRoom.Text = "";
                }
            }
        }
        #endregion

        // Zoom view window painting.
        #region "zoom view"
        // Update all listings of current room.
        private void SetCurrentRoom()
        {
            // Populate Room Number in all tabs. 
            lblButtonName.Text = iCurrentRoom.ToString();
            lblRoomRoomNumber.Text = iCurrentRoom.ToString();
            lblAttributesRoomNumber.Text = iCurrentRoom.ToString();
            lblMobCurrentRoom.Text = iCurrentRoom.ToString();

            // Populate Mobs Loading in Room.
            PopulateMobsLoadingInRoom();
            // Populate Mobs Not Loading in Room.
            PopulateMobsNotLoadingInRoom();

            // Disable Spawn selection if Room is not part of Map.
            classes.room oRoom = new classes.room();
            oRoom = classes.room.GetRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ);

            // Toggle Panels and allow Add/Edit if Room is part of the Map.
            if (oRoom.Exists())
            {
                // Set Room Details.
                if (oRoom.roomName == "" || oRoom.roomName == null) lblCurrentRoomName.Text = "< Not Set >"; else lblCurrentRoomName.Text = oRoom.roomName;
                if (oRoom.VNUM <= 0) lblVNUM.Text = "< Not Set >"; else lblVNUM.Text = oRoom.VNUM.ToString();
                txtRoomName.Text = oRoom.roomName;
                txtVNUM.Text = oRoom.VNUM.ToString();
                cboSector.SelectedIndex = cboSector.FindString(oRoom.sector);
                txtRoomDescription.Text = oRoom.description;
                txtExtraDescKeywords.Text = oRoom.extraKeywords;
                txtExtraDescription.Text = oRoom.extraDescription;
                txtNorthExit.Text = oRoom.exitNorthDesc;
                txtSouthExit.Text = oRoom.exitSouthDesc;
                txtEastExit.Text = oRoom.exitEastDesc;
                txtWestExit.Text = oRoom.exitWestDesc;
                txtUpExit.Text = oRoom.exitUpDesc;
                txtDownExit.Text = oRoom.exitDownDesc;

                // Enable "Mob Tab" controls.
                cboAllMobs.Enabled = true;
                btnAddSpawn.Enabled = true;
                btnMobDetail.Enabled = true;
                btnRemoveSpawn.Enabled = true;

                // Enable "Door" controls.
                txtDoorKeywords.Enabled = true;
                txtDoorVNUM.Enabled = true;
                cboDirection.Enabled = true;
                cboDoorType.Enabled = true;
                cboDoorKey.Enabled = true;
            }
            else
            {
                // Disable "Mob Tab" controls.
                cboAllMobs.Enabled = false;
                btnAddSpawn.Enabled = false;
                btnMobDetail.Enabled = false;
                btnRemoveSpawn.Enabled = false;

                // Disable "Door" controls.
                txtDoorKeywords.Enabled = false;
                txtDoorVNUM.Enabled = false;
                cboDirection.Enabled = false;
                cboDoorType.Enabled = false;
                cboDoorKey.Enabled = false;
            }

            // Disable Load selection if Mob is not Selected.
            if (lbSpawns.SelectedIndex > -1)
            {
                PopulateObjectsNotLoadingOnMob(Convert.ToInt32(lbSpawns.SelectedValue.ToString()));
                cboAllObjects.Enabled = true;
                btnAddLoad.Enabled = true;
                btnObjDetail.Enabled = true;
                btnRemoveLoad.Enabled = true;
            }
            else
            {
                cboAllObjects.Enabled = false;
                btnAddLoad.Enabled = false;
                btnObjDetail.Enabled = false;
                btnRemoveLoad.Enabled = false;
            }
        }

        // Update Zoom View window.
        private void UpdateZoomView(Button btnCurrentRoom)
        {
            // Set Background Color to Current Room.
            btnCurrentZoom.BackColor = btnCurrentRoom.BackColor;
            // Set Zoom Button to visible.
            btnCurrentZoom.Visible = true;

            // Update Coordinates.
            lblX.Text = iCurrentX.ToString();
            lblY.Text = iCurrentY.ToString();
            lblZ.Text = iCurrentZ.ToString();

            // Hide Doors.
            HideDoors();

            // ################
            // Set Zoom North.
            // ################
            bool blnHasNorthLink = false;
            blnHasNorthLink = HasNorthLink(iCurrentRoom);

            // Display or Hide the North Link depending on Current Room links.
            if (blnHasNorthLink)
            {
                btnZoomNorth.Visible = true;
            }
            else
            {
                btnZoomNorth.Visible = false;
            }

            // ################
            // Set Zoom South.
            // ################
            bool blnHasSouthLink = false;
            blnHasSouthLink = HasSouthLink(iCurrentRoom);

            // Display or Hide the South Link depending on Current Room links.
            if (blnHasSouthLink)
            {
                btnZoomSouth.Visible = true;
            }
            else
            {
                btnZoomSouth.Visible = false;
            }

            // ################
            // Set Zoom East.
            // ################
            bool blnHasEastLink = false;
            blnHasEastLink = HasEastLink(iCurrentRoom);

            // Display or Hide the East Link depending on Current Room links.
            if (blnHasEastLink)
            {
                btnZoomEast.Visible = true;
            }
            else
            {
                btnZoomEast.Visible = false;
            }

            // ################
            // Set Zoom West.
            // ################
            bool blnHasWestLink = false;
            blnHasWestLink = HasWestLink(iCurrentRoom);

            // Display or Hide the West Link depending on Current Room links.
            if (blnHasWestLink)
            {
                btnZoomWest.Visible = true;
            }
            else
            {
                btnZoomWest.Visible = false;
            }

            // ################
            // Set Zoom Up.
            // ################
            bool blnHasUpLink = false;
            blnHasUpLink = HasUpLink(iCurrentRoom);

            // Display or Hide the Up Link depending on Current Room links.
            if (blnHasUpLink)
            {
                btnZoomUp.Visible = true;
            }
            else
            {
                btnZoomUp.Visible = false;
            }

            // ################
            // Set Zoom Down.
            // ################
            bool blnHasDownLink = false;
            blnHasDownLink = HasDownLink(iCurrentRoom);

            // Display or Hide the Down Link depending on Current Room links.
            if (blnHasDownLink)
            {
                btnZoomDown.Visible = true;
            }
            else
            {
                btnZoomDown.Visible = false;
            }

            // ################
            // Display Doors.
            // ################
            DisplayRoomDoors();

            if (blnAutoMap && btnCurrentRoom.BackColor != clrBlank)
            {
                SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
            }
        }

        // Hide all Doors.
        private void HideDoors()
        {
            ClearDoorSection();

            btnDoorNorth.Visible = false;
            btnDoorSouth.Visible = false;
            btnDoorEast.Visible = false;
            btnDoorWest.Visible = false;
            btnDoorUp.Visible = false;
            btnDoorDown.Visible = false;
        }

        // Clear Door Fields.
        private void ClearDoorSection()
        {
            lblDoorID.Text = "";
            txtDoorKeywords.Text = "";
            txtDoorVNUM.Text = "";
            cboDirection.SelectedIndex = -1;
            cboDoorType.SelectedIndex = -1;
            cboDoorKey.SelectedIndex = 0;
        }

        // Set Door to Display as Exit.
        private void DisplayDoor(string strDirection)
        {
            switch (strDirection.ToLower()) 
            {
                case "north":
                    btnDoorNorth.Visible = true;
                    btnZoomNorth.Visible = false;
                    break;
                case "south":
                    btnDoorSouth.Visible = true;
                    btnZoomSouth.Visible = false;
                    break;
                case "east":
                    btnDoorEast.Visible = true;
                    btnZoomEast.Visible = false;
                    break;
                case "west":
                    btnDoorWest.Visible = true;
                    btnZoomWest.Visible = false;
                    break;
                case "down":
                    btnDoorDown.Visible = true;
                    btnZoomDown.Visible = false;
                    break;
                case "up":
                    btnDoorUp.Visible = true;
                    btnZoomUp.Visible = false;
                    break;
                default:
                    break;
            }
        }

        // Display All Doors for a Room.
        private void DisplayRoomDoors()
        {
            // Display Room Doors. 
            classes.door[] doors = new classes.door[6];
            doors = classes.door.GetRoomDoors(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ);
            // Loop through doors and display on the zoom view map. 
            foreach (classes.door oDoor in doors)
            {
                if (oDoor != null)
                {
                    DisplayDoor(oDoor.direction);
                }
            }
        }
        #endregion

        // Map Terrain color selector.
        #region "map terrain"
        // Set Terrain Colors. 
        void SetLegendColors()
        {
            btnDefault.BackColor = clrDefault;
            btnDefault.FlatStyle = FlatStyle.Flat;
            btnDefault.FlatAppearance.BorderSize = 1;
        }
        #endregion

        // Mob Tab
        #region "mob tab"
        // Populate Mobs Loading in Room.
        private void PopulateMobsLoadingInRoom()
        {
            // Create a DataTable to source the Combobox.
            DataTable dtMobs = new DataTable();
            dtMobs.Columns.Add("MobID", typeof(string));
            dtMobs.Columns.Add("ShortDesc", typeof(string));

            // Get the list of Mobs to Populate the Combobox.
            DataSet dsMobs = classes.mob.GetMobSpawns_ByRoomID(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ);

            // Fill the DataTable with the list of Mobs.
            dtMobs = dsMobs.Tables[0];

            // Push the List to the ComboBox.
            lbSpawns.ValueMember = "MobID";
            lbSpawns.DisplayMember = "ShortDesc";
            lbSpawns.DataSource = dtMobs;
        }

        // Populate Mobs NOT Loading in Room.
        private void PopulateMobsNotLoadingInRoom()
        {
            // Create a DataTable to source the Combobox.
            DataTable dtMobs = new DataTable();
            dtMobs.Columns.Add("MobID", typeof(string));
            dtMobs.Columns.Add("ShortDesc", typeof(string));

            // Get the list of Mobs to Populate the Combobox.
            DataSet dsMobs = classes.mob.GetMobSpawnsNotInRoom_ByRoomID(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ);

            // Fill the DataTable with the list of Mobs.
            dtMobs = dsMobs.Tables[0];

            // Add blank row to the top of the ComboBox.
            DataRow row = dtMobs.NewRow();
            row["MobID"] = "";
            row["ShortDesc"] = " < Add New Spawn >";
            dtMobs.Rows.InsertAt(row, 0);

            // Push the List to the ComboBox.
            cboAllMobs.ValueMember = "MobID";
            cboAllMobs.DisplayMember = "ShortDesc";
            cboAllMobs.DataSource = dtMobs;
        }

        // Populate Objects Loading on Mob.
        private void PopulateObjectsLoadingOnMob(int iCurrentMob)
        {
            // Create a DataTable to source the Combobox.
            DataTable dtObjects = new DataTable();
            dtObjects.Columns.Add("ObjectID", typeof(string));
            dtObjects.Columns.Add("ShortDesc", typeof(string));

            // Get the list of Objects to Populate the Combobox.
            DataSet dsObjects = classes.c_object.GetLoadingObjects_ByMobID(iCurrentArea, iCurrentMob, iCurrentX, iCurrentY, iCurrentZ);

            // Fill the DataTable with the list of Mobs.
            dtObjects = dsObjects.Tables[0];

            // Push the List to the ComboBox.
            lbLoads.ValueMember = "ObjectID";
            lbLoads.DisplayMember = "ShortDesc";
            lbLoads.DataSource = dtObjects;
        }

        // Populate Objects NOT Loading on Mob.
        private void PopulateObjectsNotLoadingOnMob(int iCurrentMob)
        {
            // Create a DataTable to source the Combobox.
            DataTable dtObjects = new DataTable();
            dtObjects.Columns.Add("ObjectID", typeof(string));
            dtObjects.Columns.Add("ShortDesc", typeof(string));

            // Get the list of Objects to Populate the Combobox.
            DataSet dsObjects = classes.c_object.GetNonLoadingObjects_ByMobID(iCurrentArea, iCurrentMob, iCurrentX, iCurrentY, iCurrentZ);

            // Fill the DataTable with the list of Mobs.
            dtObjects = dsObjects.Tables[0];

            // Add blank row to the top of the ComboBox.
            DataRow row = dtObjects.NewRow();
            row["ObjectID"] = "";
            row["ShortDesc"] = " < Add New Load >";
            dtObjects.Rows.InsertAt(row, 0);

            // Push the List to the ComboBox.
            cboAllObjects.ValueMember = "ObjectID";
            cboAllObjects.DisplayMember = "ShortDesc";
            cboAllObjects.DataSource = dtObjects;
        }

        // Display Currently Mob Spawns to the current Panel.
        private void HighlightMobSpawns()
        {
            if (lblMobVNUM.Text.Trim() != "")
            {
                // If turning "Spawn Highlight" off, redisplay the panel.
                if (blnHighlightMobs)
                {
                    DisplayPanelRooms(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ);
                }
                else
                {
                    // Display Spawn Rooms for Current Mob. 
                    classes.room[] MobSpawns = new classes.room[750];
                    MobSpawns = classes.room.GetRoomsWithMobSpawn(iCurrentArea, iCurrentX, iCurrentY, iCurrentZ, Convert.ToInt32(lblQuickMobID.Text.Trim()));
                    // Loop through Down Rooms and change border colors on map. 
                    foreach (classes.room MobSpawn in MobSpawns)
                    {
                        if (MobSpawn != null)
                        {
                            Control[] ctrlSpawn = this.Controls.Find("room" + MobSpawn.roomNumber.ToString(), true);
                            Button btnRoom = new Button();
                            if (btnRoom != null)
                            {
                                btnRoom = (Button)ctrlSpawn[0];
                                btnRoom.BackColor = clrHighlightSpawn;
                            }
                        }
                    }
                }
                
                // Set the Highlight Mob Toggle;
                if (blnHighlightMobs)
                {
                    btnHighlightSpawns.Text = "Show Spawns";
                    blnHighlightMobs = false;
                }
                else
                {
                    btnHighlightSpawns.Text = "Hide Spawns";
                    blnHighlightMobs = true;
                }
            }
        }
        #endregion

        // Testing - Onclick Sandbox.
        #region "testing"
        private void btnTesting_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
