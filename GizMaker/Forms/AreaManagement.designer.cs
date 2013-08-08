namespace GizMaker.forms
{
    partial class AreaManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgAreas = new System.Windows.Forms.DataGridView();
            this.btnOpenArea = new System.Windows.Forms.Button();
            this.pnlNew = new System.Windows.Forms.Panel();
            this.lblNew = new System.Windows.Forms.Label();
            this.lblChooseArea = new System.Windows.Forms.Label();
            this.AreaID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AreaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZoneNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtAreaName = new System.Windows.Forms.TextBox();
            this.txtStartingVNUM = new System.Windows.Forms.TextBox();
            this.txtZoneNumber = new System.Windows.Forms.TextBox();
            this.lblTextArea = new System.Windows.Forms.Label();
            this.lblZoneNumber = new System.Windows.Forms.Label();
            this.lblStartingVNUM = new System.Windows.Forms.Label();
            this.btnCreateArea = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgAreas)).BeginInit();
            this.pnlNew.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgAreas
            // 
            this.dgAreas.AllowUserToAddRows = false;
            this.dgAreas.AllowUserToOrderColumns = true;
            this.dgAreas.AllowUserToResizeColumns = false;
            this.dgAreas.AllowUserToResizeRows = false;
            this.dgAreas.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.dgAreas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAreas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dgAreas.ColumnHeadersVisible = false;
            this.dgAreas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AreaID,
            this.AreaName,
            this.ZoneNumber});
            this.dgAreas.GridColor = System.Drawing.Color.DarkGray;
            this.dgAreas.Location = new System.Drawing.Point(10, 236);
            this.dgAreas.MultiSelect = false;
            this.dgAreas.Name = "dgAreas";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAreas.RowHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgAreas.RowHeadersVisible = false;
            this.dgAreas.RowHeadersWidth = 10;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.LightSlateGray;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.InactiveCaption;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            this.dgAreas.RowsDefaultCellStyle = dataGridViewCellStyle18;
            this.dgAreas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgAreas.Size = new System.Drawing.Size(510, 290);
            this.dgAreas.TabIndex = 0;
            // 
            // btnOpenArea
            // 
            this.btnOpenArea.BackColor = System.Drawing.Color.White;
            this.btnOpenArea.Location = new System.Drawing.Point(356, 536);
            this.btnOpenArea.Name = "btnOpenArea";
            this.btnOpenArea.Size = new System.Drawing.Size(75, 23);
            this.btnOpenArea.TabIndex = 6;
            this.btnOpenArea.Text = "Open";
            this.btnOpenArea.UseVisualStyleBackColor = false;
            this.btnOpenArea.Click += new System.EventHandler(this.btnOpenArea_Click);
            // 
            // pnlNew
            // 
            this.pnlNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNew.Controls.Add(this.btnCreateArea);
            this.pnlNew.Controls.Add(this.lblStartingVNUM);
            this.pnlNew.Controls.Add(this.lblZoneNumber);
            this.pnlNew.Controls.Add(this.lblTextArea);
            this.pnlNew.Controls.Add(this.txtZoneNumber);
            this.pnlNew.Controls.Add(this.txtStartingVNUM);
            this.pnlNew.Controls.Add(this.txtAreaName);
            this.pnlNew.Location = new System.Drawing.Point(10, 25);
            this.pnlNew.Name = "pnlNew";
            this.pnlNew.Size = new System.Drawing.Size(502, 163);
            this.pnlNew.TabIndex = 2;
            // 
            // lblNew
            // 
            this.lblNew.AutoSize = true;
            this.lblNew.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNew.Location = new System.Drawing.Point(12, 11);
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(91, 14);
            this.lblNew.TabIndex = 3;
            this.lblNew.Text = "Create New Area";
            // 
            // lblChooseArea
            // 
            this.lblChooseArea.AutoSize = true;
            this.lblChooseArea.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChooseArea.Location = new System.Drawing.Point(12, 219);
            this.lblChooseArea.Name = "lblChooseArea";
            this.lblChooseArea.Size = new System.Drawing.Size(99, 14);
            this.lblChooseArea.TabIndex = 4;
            this.lblChooseArea.Text = "Open Existing Area";
            // 
            // AreaID
            // 
            this.AreaID.DataPropertyName = "AreaID";
            this.AreaID.HeaderText = "Area ID";
            this.AreaID.Name = "AreaID";
            this.AreaID.Visible = false;
            this.AreaID.Width = 50;
            // 
            // AreaName
            // 
            this.AreaName.DataPropertyName = "AreaName";
            this.AreaName.HeaderText = "Area Name";
            this.AreaName.Name = "AreaName";
            this.AreaName.Width = 400;
            // 
            // ZoneNumber
            // 
            this.ZoneNumber.DataPropertyName = "ZoneNumber";
            this.ZoneNumber.HeaderText = "Zone Number";
            this.ZoneNumber.Name = "ZoneNumber";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.AliceBlue;
            this.btnDelete.Location = new System.Drawing.Point(10, 536);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Erase";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(437, 536);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtAreaName
            // 
            this.txtAreaName.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtAreaName.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAreaName.Location = new System.Drawing.Point(86, 22);
            this.txtAreaName.Name = "txtAreaName";
            this.txtAreaName.Size = new System.Drawing.Size(404, 20);
            this.txtAreaName.TabIndex = 1;
            // 
            // txtStartingVNUM
            // 
            this.txtStartingVNUM.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtStartingVNUM.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStartingVNUM.Location = new System.Drawing.Point(86, 80);
            this.txtStartingVNUM.Name = "txtStartingVNUM";
            this.txtStartingVNUM.Size = new System.Drawing.Size(86, 20);
            this.txtStartingVNUM.TabIndex = 3;
            // 
            // txtZoneNumber
            // 
            this.txtZoneNumber.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtZoneNumber.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZoneNumber.Location = new System.Drawing.Point(86, 51);
            this.txtZoneNumber.Name = "txtZoneNumber";
            this.txtZoneNumber.Size = new System.Drawing.Size(86, 20);
            this.txtZoneNumber.TabIndex = 2;
            // 
            // lblTextArea
            // 
            this.lblTextArea.AutoSize = true;
            this.lblTextArea.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextArea.Location = new System.Drawing.Point(11, 25);
            this.lblTextArea.Name = "lblTextArea";
            this.lblTextArea.Size = new System.Drawing.Size(70, 14);
            this.lblTextArea.TabIndex = 3;
            this.lblTextArea.Text = "Area Name:";
            // 
            // lblZoneNumber
            // 
            this.lblZoneNumber.AutoSize = true;
            this.lblZoneNumber.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoneNumber.Location = new System.Drawing.Point(34, 55);
            this.lblZoneNumber.Name = "lblZoneNumber";
            this.lblZoneNumber.Size = new System.Drawing.Size(47, 14);
            this.lblZoneNumber.TabIndex = 4;
            this.lblZoneNumber.Text = "Zone #:";
            // 
            // lblStartingVNUM
            // 
            this.lblStartingVNUM.AutoSize = true;
            this.lblStartingVNUM.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartingVNUM.Location = new System.Drawing.Point(11, 85);
            this.lblStartingVNUM.Name = "lblStartingVNUM";
            this.lblStartingVNUM.Size = new System.Drawing.Size(70, 14);
            this.lblStartingVNUM.TabIndex = 5;
            this.lblStartingVNUM.Text = "First VNUM:";
            // 
            // btnCreateArea
            // 
            this.btnCreateArea.Location = new System.Drawing.Point(409, 130);
            this.btnCreateArea.Name = "btnCreateArea";
            this.btnCreateArea.Size = new System.Drawing.Size(81, 23);
            this.btnCreateArea.TabIndex = 4;
            this.btnCreateArea.Text = "Create Area";
            this.btnCreateArea.UseVisualStyleBackColor = true;
            this.btnCreateArea.Click += new System.EventHandler(this.btnCreateArea_Click);
            // 
            // AreaManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.ClientSize = new System.Drawing.Size(524, 570);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lblChooseArea);
            this.Controls.Add(this.lblNew);
            this.Controls.Add(this.pnlNew);
            this.Controls.Add(this.btnOpenArea);
            this.Controls.Add(this.dgAreas);
            this.Name = "AreaManagement";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Area Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgAreas)).EndInit();
            this.pnlNew.ResumeLayout(false);
            this.pnlNew.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgAreas;
        private System.Windows.Forms.Button btnOpenArea;
        private System.Windows.Forms.Panel pnlNew;
        private System.Windows.Forms.Label lblNew;
        private System.Windows.Forms.Label lblChooseArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn AreaID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AreaName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZoneNumber;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblStartingVNUM;
        private System.Windows.Forms.Label lblZoneNumber;
        private System.Windows.Forms.Label lblTextArea;
        private System.Windows.Forms.TextBox txtZoneNumber;
        private System.Windows.Forms.TextBox txtStartingVNUM;
        private System.Windows.Forms.TextBox txtAreaName;
        private System.Windows.Forms.Button btnCreateArea;

    }
}