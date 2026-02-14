namespace CORNPOSUtilityDesktop
{
    partial class frmUpdateBuild
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
            this.pUpdateBuild = new System.Windows.Forms.TabPage();
            this.richStatus = new System.Windows.Forms.RichTextBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.cblBuilds = new System.Windows.Forms.CheckedListBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.lblCopyTo = new System.Windows.Forms.Label();
            this.btnBrowseTo = new System.Windows.Forms.Button();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.btnBrowseFrom = new System.Windows.Forms.Button();
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.cbFile = new System.Windows.Forms.CheckBox();
            this.TabTerminal.SuspendLayout();
            this.pUpdateBuild.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).BeginInit();
            this.SuspendLayout();
            // 
            // TabTerminal
            // 
            this.TabTerminal.Controls.Add(this.pUpdateBuild);
            this.TabTerminal.Dock = System.Windows.Forms.DockStyle.Top;
            this.TabTerminal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TabTerminal.Location = new System.Drawing.Point(0, 0);
            this.TabTerminal.Name = "TabTerminal";
            this.TabTerminal.SelectedIndex = 0;
            this.TabTerminal.Size = new System.Drawing.Size(934, 385);
            this.TabTerminal.TabIndex = 81;
            // 
            // pUpdateBuild
            // 
            this.pUpdateBuild.Controls.Add(this.cbFile);
            this.pUpdateBuild.Controls.Add(this.richStatus);
            this.pUpdateBuild.Controls.Add(this.lblTotal);
            this.pUpdateBuild.Controls.Add(this.cblBuilds);
            this.pUpdateBuild.Controls.Add(this.btnCopy);
            this.pUpdateBuild.Controls.Add(this.txtTo);
            this.pUpdateBuild.Controls.Add(this.lblCopyTo);
            this.pUpdateBuild.Controls.Add(this.btnBrowseTo);
            this.pUpdateBuild.Controls.Add(this.txtFrom);
            this.pUpdateBuild.Controls.Add(this.lblFrom);
            this.pUpdateBuild.Controls.Add(this.btnBrowseFrom);
            this.pUpdateBuild.ImageIndex = 1;
            this.pUpdateBuild.Location = new System.Drawing.Point(4, 22);
            this.pUpdateBuild.Name = "pUpdateBuild";
            this.pUpdateBuild.Size = new System.Drawing.Size(926, 359);
            this.pUpdateBuild.TabIndex = 0;
            this.pUpdateBuild.Text = "Update Build";
            this.pUpdateBuild.UseVisualStyleBackColor = true;
            // 
            // richStatus
            // 
            this.richStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richStatus.Location = new System.Drawing.Point(412, 54);
            this.richStatus.Name = "richStatus";
            this.richStatus.Size = new System.Drawing.Size(488, 286);
            this.richStatus.TabIndex = 11;
            this.richStatus.Text = "";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(344, 156);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(53, 15);
            this.lblTotal.TabIndex = 10;
            this.lblTotal.Text = "Total: 0";
            // 
            // cblBuilds
            // 
            this.cblBuilds.FormattingEnabled = true;
            this.cblBuilds.Location = new System.Drawing.Point(71, 55);
            this.cblBuilds.Name = "cblBuilds";
            this.cblBuilds.Size = new System.Drawing.Size(256, 289);
            this.cblBuilds.TabIndex = 9;
            this.cblBuilds.SelectedIndexChanged += new System.EventHandler(this.cblBuilds_SelectedIndexChanged);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(335, 196);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 8;
            this.btnCopy.Text = "Copy Build";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(70, 30);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(331, 20);
            this.txtTo.TabIndex = 7;
            this.txtTo.Text = "D:\\My App\\CORN Builds\\Cafe Customers";
            this.txtTo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTo_KeyUp);
            // 
            // lblCopyTo
            // 
            this.lblCopyTo.AutoSize = true;
            this.lblCopyTo.Location = new System.Drawing.Point(9, 33);
            this.lblCopyTo.Name = "lblCopyTo";
            this.lblCopyTo.Size = new System.Drawing.Size(46, 13);
            this.lblCopyTo.TabIndex = 6;
            this.lblCopyTo.Text = "Build To";
            // 
            // btnBrowseTo
            // 
            this.btnBrowseTo.Location = new System.Drawing.Point(406, 28);
            this.btnBrowseTo.Name = "btnBrowseTo";
            this.btnBrowseTo.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTo.TabIndex = 5;
            this.btnBrowseTo.Text = "...";
            this.btnBrowseTo.UseVisualStyleBackColor = true;
            this.btnBrowseTo.Click += new System.EventHandler(this.btnBrowseTo_Click);
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(70, 7);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(331, 20);
            this.txtFrom.TabIndex = 3;
            this.txtFrom.Text = "D:\\My App\\CORN Builds\\Cafe Customers\\Mozzarella 27\\MOZ\\Build";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(9, 10);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(55, 13);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "Build Path";
            // 
            // btnBrowseFrom
            // 
            this.btnBrowseFrom.Location = new System.Drawing.Point(406, 4);
            this.btnBrowseFrom.Name = "btnBrowseFrom";
            this.btnBrowseFrom.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFrom.TabIndex = 1;
            this.btnBrowseFrom.Text = "...";
            this.btnBrowseFrom.UseVisualStyleBackColor = true;
            this.btnBrowseFrom.Click += new System.EventHandler(this.btnBrowse_Click);
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
            // cbFile
            // 
            this.cbFile.AutoSize = true;
            this.cbFile.Location = new System.Drawing.Point(488, 5);
            this.cbFile.Name = "cbFile";
            this.cbFile.Size = new System.Drawing.Size(74, 17);
            this.cbFile.TabIndex = 12;
            this.cbFile.Text = "Single File";
            this.cbFile.UseVisualStyleBackColor = true;
            this.cbFile.CheckedChanged += new System.EventHandler(this.cbFile_CheckedChanged);
            // 
            // frmUpdateBuild
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(934, 392);
            this.Controls.Add(this.TabTerminal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmUpdateBuild";
            this.Text = "frmUpdateBuild";
            this.TabTerminal.ResumeLayout(false);
            this.pUpdateBuild.ResumeLayout(false);
            this.pUpdateBuild.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabTerminal;
        private System.Windows.Forms.TabPage pUpdateBuild;
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
        private System.Windows.Forms.Button btnBrowseFrom;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblCopyTo;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.Button btnBrowseTo;
        private System.Windows.Forms.CheckedListBox cblBuilds;
        private System.Windows.Forms.Label lblTotal;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RichTextBox richStatus;
        private System.Windows.Forms.CheckBox cbFile;
    }
}