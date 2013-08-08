using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Controls;

namespace GizMaker.forms
{
    public partial class Mapper : Form
    {
        // Global Variables.
        #region "global variables"
        // Colors
        Color clrBlank = Color.Gray;
        //Color clrBlankNoGrid = Color.DimGray;
        Color clrBlankBorder = Color.Black;
        //Color clrBlankBorderNoGrid = Color.DimGray;
        Color clrDefault = Color.DarkSlateGray;
        Color clrCurrent = Color.DarkSlateGray;
        Color clrUp = Color.Yellow;
        Color clrDown = Color.MediumSpringGreen;
        Color clrDoorDefault = Color.DarkGoldenrod;
        Color clrDoorLocked = Color.DarkRed;

        // Positioning 
        int iCurrentArea = 0;
        int iCurrentRoom = 0;
        int iCurrentX = 0;
        int iCurrentY = 0;
        int iCurrentZ = 0;

        // Toggles
        bool blnAutoColor = false;
        bool blnAutoLink = false;
        #endregion"

        // On Load Event.
        #region "contructors"
        public Mapper()
        {
            InitializeComponent();

            // Set the default room color.
            clrCurrent = clrDefault;

            // Set Legend Colors
            SetLegendColors();

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
            txtAreaName.Text = oArea.areaName;
            txtZoneNumber.Text = oArea.zoneNumber.ToString();
            txtStartingVNUM.Text = oArea.startingVNUM.ToString();
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
        #endregion

        // Movement around the grid.
        #region "movement"
        // Move North on the Grid.
        void MoveNorth()
        {
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

                    // If Autolink is enabled, link the respective rooms.
                    if (blnAutoLink)
                    {
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

                    // If AutoColor is enabled, save the new room to the map.
                    if (blnAutoColor)
                    {
                        if (room.BackColor != clrCurrent)
                        {
                            room.BackColor = clrCurrent;
                            blnRoomUpdated = true;
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
        }

        // Move West on the Grid.
        void MoveWest()
        {
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

                // If Autolink is enabled, link the respective rooms.
                if (blnAutoLink)
                {
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

                // If AutoColor is enabled, save the new room to the map.
                if (blnAutoColor)
                {
                    if (room.BackColor != clrCurrent)
                    {
                        room.BackColor = clrCurrent;
                        blnRoomUpdated = true;
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
        }

        // Move East on the Grid.
        void MoveEast()
        {
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

                // If Autolink is enabled, link the respective rooms.
                if (blnAutoLink)
                {
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

                // If AutoColor is enabled, save the new room to the map.
                if (blnAutoColor)
                {
                    if (room.BackColor != clrCurrent)
                    {
                        room.BackColor = clrCurrent;
                        blnRoomUpdated = true;
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
        }

        // Move South on the Grid.
        void MoveSouth()
        {
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
            if (iModSouthBorder != 0 && iCurrentRoom != 1)
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

                    // If Autolink is enabled, link the respective rooms.
                    if (blnAutoLink)
                    {
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

                    // If AutoColor is enabled, save the new room to the map.
                    if (blnAutoColor)
                    {
                        if (room.BackColor != clrCurrent)
                        {
                            room.BackColor = clrCurrent;
                            blnRoomUpdated = true;
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

                if (blnAutoColor || blnAutoLink)
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

                if (blnAutoColor || blnAutoLink)
                {
                    // Save the Room Location
                    SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), true);
                }
            }

            NewMap_Down();
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

                // If AutoColor is enabled, save the new room to the map.
                if (blnAutoColor)
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

                // If AutoColor is enabled, save the new room to the map.
                if (blnAutoColor)
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

                // If AutoColor is enabled, save the new room to the map.
                if (blnAutoColor)
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

                // If AutoColor is enabled, save the new room to the map.
                if (blnAutoColor)
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

                // If AutoColor is enabled, save the new room to the map.
                if (blnAutoColor)
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

                // If AutoColor is enabled, save the new room to the map.
                if (blnAutoColor)
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
            iCurrentRoom = 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room1";
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
            iCurrentRoom = 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room1";
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
            iCurrentRoom = 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room1";
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
            iCurrentRoom = 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room1";
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
            iCurrentRoom = 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room1";
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
            iCurrentRoom = 1;

            // Clear Map Rooms.
            ClearRooms();

            // Get New Room Control.
            Button room = new Button();
            string strRoomName = "room1";
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

        // Use single button click event to route all button clicks. 
        public void RoomClick(object sender, EventArgs e)
        {
            string strRoomName = string.Empty;

            // Get the sender button object.
            Button btnButton = (Button)sender;
            // Get the name of the button.
            strRoomName = btnButton.Name;

            iCurrentRoom = Convert.ToInt32(strRoomName.Replace("room", ""));
            SetCurrentRoom();

            if (blnAutoColor)
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
            using (forms.MobManagement mobDetail = new forms.MobManagement())
            {
                // Display the form.
                mobDetail.ShowDialog();

                // Get the saved Mob ID from the child form and refresh.
                int iMob = mobDetail.iMobID;

                if (iMob > 0)
                {
                    // Refresh Mob Detail.
                }
            }
        }

        // Open Mob Detail and Populate for selected Mob.
        private void btnFullDetail_Click(object sender, EventArgs e)
        {
            int iCurrentMob = -1;

            // Check which row is selected.
            foreach (DataGridViewRow dgvRow in dgMobsInRoom.Rows)
            {
                if (dgvRow.Selected)
                {
                    iCurrentMob = dgvRow.Index;
                }
            }

            if (iCurrentMob > -1)
            {
                // Get the Mob ID
                string strMobID = dgMobsInRoom.Rows[iCurrentMob].Cells["MobID"].Value.ToString();

                // Display the Form to allow the User to Edit a Mob.
                using (forms.MobManagement mobDetail = new forms.MobManagement(Convert.ToInt32(strMobID)))
                {
                    // Display the form.
                    mobDetail.ShowDialog();

                    // Get the saved Mob ID from the child form and refresh.
                    int iMob = mobDetail.iMobID;

                    if (iMob > 0)
                    {
                        // Refresh Mob Detail.
                    }
                }
            }
        }

        // Open Blank Object Detail Form.
        private void btnAddObject_Click(object sender, EventArgs e)
        {
            // Display the Form to allow the User to select an Area.
            using (forms.ObjectManagement objDetail = new forms.ObjectManagement())
            {
                // Display the form.
                objDetail.ShowDialog();
            }
        }

        // Open Object Detail and Populate for selected Mob.
        private void btnFullObjDetail_Click(object sender, EventArgs e)
        {
            int iCurrentObj = -1;

            // Check which row is selected.
            foreach (DataGridViewRow dgvRow in dgLoadingItems.Rows)
            {
                if (dgvRow.Selected)
                {
                    iCurrentObj = dgvRow.Index;
                }
            }

            if (iCurrentObj > -1)
            {
                // Get the Object ID
                string strObjectID = dgLoadingItems.Rows[iCurrentObj].Cells["ObjectID"].Value.ToString();

                // Display the Form to allow the User to Edit an Object.
                using (forms.ObjectManagement objDetail = new forms.ObjectManagement(Convert.ToInt32(strObjectID)))
                {
                    // Display the form.
                    objDetail.ShowDialog();

                    // Get the saved Mob ID from the child form and refresh.
                    int iObjectID = objDetail.iObjectID;

                    if (iObjectID > 0)
                    {
                        // Refresh Mob Detail.
                    }
                }
            }
        }

        // Set Autocolor Flag.
        private void chkAutocolor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutocolor.Checked == true)
            {
                blnAutoColor = true;
            }
            else
            {
                blnAutoColor = false;
            }
        }

        // Set Autolink Flag.
        private void chkAutolink_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutolink.Checked == true)
            {
                blnAutoLink = true;
            }
            else
            {
                blnAutoLink = false;
            }
        }

        // Mob Selected in "Mobs Currently Loading in Room" grid.
        private void dgMobsInRoom_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int iCurrentMob = -1;

            // Check which row is selected.
            foreach (DataGridViewRow dgvRow in dgMobsInRoom.Rows)
            {
                if (dgvRow.Selected)
                {
                    iCurrentMob = dgvRow.Index;
                }
            }

            if (iCurrentMob > -1)
            {
                // Get the Mob ID
                string strMobID = dgMobsInRoom.Rows[iCurrentMob].Cells["MobID"].Value.ToString();

                // Populate Objects associated with selected Mob. 
                PopulateObjectsLoadingOnMob(Convert.ToInt32(strMobID));
                // Populate Objects not associated with selected Mob.
                PopulateObjectsNotLoadingOnMob(Convert.ToInt32(strMobID));
            }
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

            // Draw a each column on the form.
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
                    }

                    // Color the Room.
                    if (btnRoom.BackColor != clrCurrent)
                    {
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
                picUp.Visible = true;
            }
            else
            {
                picUp.Visible = false;
            }

            // ################
            // Set Zoom Down.
            // ################
            bool blnHasDownLink = false;
            blnHasDownLink = HasDownLink(iCurrentRoom);

            // Display or Hide the Down Link depending on Current Room links.
            if (blnHasDownLink)
            {
                picDown.Visible = true;
            }
            else
            {
                picDown.Visible = false;
            }

            if (blnAutoColor && btnCurrentRoom.BackColor != clrBlank)
            {
                SaveRoom(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ, HasUpLink(iCurrentRoom), HasDownLink(iCurrentRoom));
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
            // Populate the Grid to Display Mobs Spawning in Current Room.
            dgMobsInRoom.AutoGenerateColumns = true;
            dgMobsInRoom.DataSource = classes.mob.GetMobSpawns_ByRoomID(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ);
            dgMobsInRoom.DataMember = "Mobs";
            dgMobsInRoom.Update();
        }

        // Populate Mobs NOT Loading in Room.
        private void PopulateMobsNotLoadingInRoom()
        {
            // Populate the Grid to Display Mobs NOT Spawning in Current Room.
            dgMobsNotInRoom.AutoGenerateColumns = true;
            dgMobsNotInRoom.DataSource = classes.mob.GetMobSpawnsNotInRoom_ByRoomID(iCurrentArea, iCurrentRoom, iCurrentX, iCurrentY, iCurrentZ);
            dgMobsNotInRoom.DataMember = "Mobs";
            dgMobsNotInRoom.Update();
        }

        // Populate Objects Loading on Mob.
        private void PopulateObjectsLoadingOnMob(int iCurrentMob)
        {
            // Populate the Grid to Objects Loading on the Current Mob.
            dgLoadingItems.AutoGenerateColumns = true;
            dgLoadingItems.DataSource = classes.c_object.GetLoadingObjects_ByMobID(iCurrentArea, iCurrentMob, iCurrentX, iCurrentY, iCurrentZ);
            dgLoadingItems.DataMember = "Objs";
            dgLoadingItems.Update();
        }

        // Populate Objects NOT Loading on Mob.
        private void PopulateObjectsNotLoadingOnMob(int iCurrentMob)
        {
            // Populate the Grid to Objects NOT Loading on the Current Mob.
            dgNonLoadingItems.AutoGenerateColumns = true;
            dgNonLoadingItems.DataSource = classes.c_object.GetNonLoadingObjects_ByMobID(iCurrentArea, iCurrentMob, iCurrentX, iCurrentY, iCurrentZ);
            dgNonLoadingItems.DataMember = "Objs";
            dgNonLoadingItems.Update();
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
