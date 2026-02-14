namespace CORNPOSUtility
{
    partial class frmTruncateData
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
            this.TabTerminal = new System.Windows.Forms.TabControl();
            this.tabTransaction = new System.Windows.Forms.TabPage();
            this.lblDayClose = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblDBName = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.btnTruncate = new System.Windows.Forms.Button();
            this.tabSingleLocation = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTruncateSingleLocationData = new System.Windows.Forms.Button();
            this.cblLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbDatabase = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label31 = new System.Windows.Forms.Label();
            this.dataGridView5 = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox22 = new System.Windows.Forms.TextBox();
            this.textBox23 = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridView6 = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabTerminal.SuspendLayout();
            this.tabTransaction.SuspendLayout();
            this.tabSingleLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).BeginInit();
            this.SuspendLayout();
            // 
            // TabTerminal
            // 
            this.TabTerminal.Controls.Add(this.tabTransaction);
            this.TabTerminal.Controls.Add(this.tabSingleLocation);
            this.TabTerminal.Dock = System.Windows.Forms.DockStyle.Top;
            this.TabTerminal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TabTerminal.Location = new System.Drawing.Point(0, 0);
            this.TabTerminal.Name = "TabTerminal";
            this.TabTerminal.SelectedIndex = 0;
            this.TabTerminal.Size = new System.Drawing.Size(934, 385);
            this.TabTerminal.TabIndex = 81;
            // 
            // tabTransaction
            // 
            this.tabTransaction.Controls.Add(this.lblDayClose);
            this.tabTransaction.Controls.Add(this.dtpDate);
            this.tabTransaction.Controls.Add(this.lblDBName);
            this.tabTransaction.Controls.Add(this.txtDBName);
            this.tabTransaction.Controls.Add(this.btnTruncate);
            this.tabTransaction.ImageIndex = 1;
            this.tabTransaction.Location = new System.Drawing.Point(4, 22);
            this.tabTransaction.Name = "tabTransaction";
            this.tabTransaction.Size = new System.Drawing.Size(926, 359);
            this.tabTransaction.TabIndex = 0;
            this.tabTransaction.Text = "Truncate Transactions";
            this.tabTransaction.UseVisualStyleBackColor = true;
            // 
            // lblDayClose
            // 
            this.lblDayClose.AutoSize = true;
            this.lblDayClose.Location = new System.Drawing.Point(283, 19);
            this.lblDayClose.Name = "lblDayClose";
            this.lblDayClose.Size = new System.Drawing.Size(88, 13);
            this.lblDayClose.TabIndex = 13;
            this.lblDayClose.Text = "Select Day Close";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "";
            this.dtpDate.Location = new System.Drawing.Point(286, 39);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 20);
            this.dtpDate.TabIndex = 12;
            // 
            // lblDBName
            // 
            this.lblDBName.AutoSize = true;
            this.lblDBName.Location = new System.Drawing.Point(21, 19);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(84, 13);
            this.lblDBName.TabIndex = 11;
            this.lblDBName.Text = "Database Name";
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(21, 39);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(259, 20);
            this.txtDBName.TabIndex = 10;
            // 
            // btnTruncate
            // 
            this.btnTruncate.Location = new System.Drawing.Point(506, 40);
            this.btnTruncate.Name = "btnTruncate";
            this.btnTruncate.Size = new System.Drawing.Size(85, 23);
            this.btnTruncate.TabIndex = 9;
            this.btnTruncate.Text = "Truncate Tran";
            this.btnTruncate.UseVisualStyleBackColor = true;
            this.btnTruncate.Click += new System.EventHandler(this.btnTruncate_Click);
            // 
            // tabSingleLocation
            // 
            this.tabSingleLocation.Controls.Add(this.label1);
            this.tabSingleLocation.Controls.Add(this.btnTruncateSingleLocationData);
            this.tabSingleLocation.Controls.Add(this.cblLocation);
            this.tabSingleLocation.Controls.Add(this.lblLocation);
            this.tabSingleLocation.Controls.Add(this.cbDatabase);
            this.tabSingleLocation.Location = new System.Drawing.Point(4, 22);
            this.tabSingleLocation.Name = "tabSingleLocation";
            this.tabSingleLocation.Size = new System.Drawing.Size(926, 359);
            this.tabSingleLocation.TabIndex = 1;
            this.tabSingleLocation.Text = "Delete Single Location Data";
            this.tabSingleLocation.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Select Database";
            // 
            // btnTruncateSingleLocationData
            // 
            this.btnTruncateSingleLocationData.Location = new System.Drawing.Point(11, 127);
            this.btnTruncateSingleLocationData.Name = "btnTruncateSingleLocationData";
            this.btnTruncateSingleLocationData.Size = new System.Drawing.Size(139, 26);
            this.btnTruncateSingleLocationData.TabIndex = 19;
            this.btnTruncateSingleLocationData.Text = "Truncate Single Location Data";
            this.btnTruncateSingleLocationData.UseVisualStyleBackColor = true;
            this.btnTruncateSingleLocationData.Click += new System.EventHandler(this.btnTruncateSingleLocationData_Click);
            // 
            // cblLocation
            // 
            this.cblLocation.FormattingEnabled = true;
            this.cblLocation.Location = new System.Drawing.Point(11, 84);
            this.cblLocation.Name = "cblLocation";
            this.cblLocation.Size = new System.Drawing.Size(254, 21);
            this.cblLocation.TabIndex = 18;
            this.cblLocation.Text = "Select Location";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(8, 68);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(81, 13);
            this.lblLocation.TabIndex = 17;
            this.lblLocation.Text = "Select Location";
            // 
            // cbDatabase
            // 
            this.cbDatabase.FormattingEnabled = true;
            this.cbDatabase.Location = new System.Drawing.Point(8, 24);
            this.cbDatabase.Name = "cbDatabase";
            this.cbDatabase.Size = new System.Drawing.Size(254, 21);
            this.cbDatabase.TabIndex = 15;
            this.cbDatabase.Text = "Select Database";
            this.cbDatabase.SelectedIndexChanged += new System.EventHandler(this.cbDatabase_SelectedIndexChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(758, 127);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 66;
            this.button5.Text = "Add";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(758, 53);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 65;
            this.button4.Text = "Search";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(466, 127);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(34, 13);
            this.label31.TabIndex = 63;
            this.label31.Text = "Value";
            // 
            // dataGridView5
            // 
            this.dataGridView5.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView5.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn4,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.dataGridView5.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView5.Location = new System.Drawing.Point(26, 7);
            this.dataGridView5.MultiSelect = false;
            this.dataGridView5.Name = "dataGridView5";
            this.dataGridView5.RowHeadersVisible = false;
            this.dataGridView5.RowTemplate.Height = 23;
            this.dataGridView5.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView5.Size = new System.Drawing.Size(234, 320);
            this.dataGridView5.TabIndex = 57;
            // 
            // dataGridViewCheckBoxColumn4
            // 
            this.dataGridViewCheckBoxColumn4.HeaderText = "";
            this.dataGridViewCheckBoxColumn4.Name = "dataGridViewCheckBoxColumn4";
            this.dataGridViewCheckBoxColumn4.Width = 30;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Name";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Value";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // textBox22
            // 
            this.textBox22.Location = new System.Drawing.Point(544, 124);
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new System.Drawing.Size(100, 20);
            this.textBox22.TabIndex = 64;
            // 
            // textBox23
            // 
            this.textBox23.Location = new System.Drawing.Point(544, 61);
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new System.Drawing.Size(100, 20);
            this.textBox23.TabIndex = 62;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(466, 64);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(35, 13);
            this.label32.TabIndex = 61;
            this.label32.Text = "Name";
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Controls.Add(this.tabPage4);
            this.tabControl3.Location = new System.Drawing.Point(38, 18);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(318, 369);
            this.tabControl3.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(310, 343);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "NMS Param";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridView6);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(310, 343);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Main Param";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView6
            // 
            this.dataGridView6.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView6.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView6.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn5,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10});
            this.dataGridView6.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView6.Location = new System.Drawing.Point(38, 11);
            this.dataGridView6.MultiSelect = false;
            this.dataGridView6.Name = "dataGridView6";
            this.dataGridView6.RowHeadersVisible = false;
            this.dataGridView6.RowTemplate.Height = 23;
            this.dataGridView6.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView6.Size = new System.Drawing.Size(234, 320);
            this.dataGridView6.TabIndex = 58;
            // 
            // dataGridViewCheckBoxColumn5
            // 
            this.dataGridViewCheckBoxColumn5.HeaderText = "";
            this.dataGridViewCheckBoxColumn5.Name = "dataGridViewCheckBoxColumn5";
            this.dataGridViewCheckBoxColumn5.Width = 30;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Name";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Value";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // frmTruncateData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(934, 392);
            this.Controls.Add(this.TabTerminal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmTruncateData";
            this.Text = "frmUpdateLicense";
            this.Load += new System.EventHandler(this.frmTruncateTran_Load);
            this.TabTerminal.ResumeLayout(false);
            this.tabTransaction.ResumeLayout(false);
            this.tabTransaction.PerformLayout();
            this.tabSingleLocation.ResumeLayout(false);
            this.tabSingleLocation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabTerminal;
        private System.Windows.Forms.TabPage tabTransaction;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.DataGridView dataGridView5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.TextBox textBox22;
        private System.Windows.Forms.TextBox textBox23;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dataGridView6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.Button btnTruncate;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.Label lblDBName;
        private System.Windows.Forms.Label lblDayClose;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TabPage tabSingleLocation;
        private System.Windows.Forms.ComboBox cbDatabase;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cblLocation;
        private System.Windows.Forms.Button btnTruncateSingleLocationData;
        private System.Windows.Forms.Label label1;
    }
}