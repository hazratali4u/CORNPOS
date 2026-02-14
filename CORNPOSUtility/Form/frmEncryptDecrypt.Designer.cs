namespace CORNPOSUtility
{
    partial class frmEncryptDecrypt
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
            this.pCORNEncryptDecrtype = new System.Windows.Forms.TabPage();
            this.lblEncryptionFor = new System.Windows.Forms.Label();
            this.cbFor = new System.Windows.Forms.ComboBox();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.lblInput = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnEncrypt = new System.Windows.Forms.Button();
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
            this.pCORNInsightEncryptDecrypt = new System.Windows.Forms.TabPage();
            this.btnDecryptInsight = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnEncryptInsight = new System.Windows.Forms.Button();
            this.TabTerminal.SuspendLayout();
            this.pCORNEncryptDecrtype.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).BeginInit();
            this.pCORNInsightEncryptDecrypt.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabTerminal
            // 
            this.TabTerminal.Controls.Add(this.pCORNEncryptDecrtype);
            this.TabTerminal.Controls.Add(this.pCORNInsightEncryptDecrypt);
            this.TabTerminal.Dock = System.Windows.Forms.DockStyle.Top;
            this.TabTerminal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TabTerminal.Location = new System.Drawing.Point(0, 0);
            this.TabTerminal.Name = "TabTerminal";
            this.TabTerminal.SelectedIndex = 0;
            this.TabTerminal.Size = new System.Drawing.Size(934, 385);
            this.TabTerminal.TabIndex = 81;
            // 
            // pCORNEncryptDecrtype
            // 
            this.pCORNEncryptDecrtype.Controls.Add(this.lblEncryptionFor);
            this.pCORNEncryptDecrtype.Controls.Add(this.cbFor);
            this.pCORNEncryptDecrtype.Controls.Add(this.btnDecrypt);
            this.pCORNEncryptDecrtype.Controls.Add(this.txtOutput);
            this.pCORNEncryptDecrtype.Controls.Add(this.lblOutput);
            this.pCORNEncryptDecrtype.Controls.Add(this.lblInput);
            this.pCORNEncryptDecrtype.Controls.Add(this.txtInput);
            this.pCORNEncryptDecrtype.Controls.Add(this.btnEncrypt);
            this.pCORNEncryptDecrtype.ImageIndex = 1;
            this.pCORNEncryptDecrtype.Location = new System.Drawing.Point(4, 25);
            this.pCORNEncryptDecrtype.Name = "pCORNEncryptDecrtype";
            this.pCORNEncryptDecrtype.Size = new System.Drawing.Size(926, 356);
            this.pCORNEncryptDecrtype.TabIndex = 0;
            this.pCORNEncryptDecrtype.Text = "CORN Encrypt Decrypt";
            this.pCORNEncryptDecrtype.UseVisualStyleBackColor = true;
            // 
            // lblEncryptionFor
            // 
            this.lblEncryptionFor.AutoSize = true;
            this.lblEncryptionFor.Location = new System.Drawing.Point(20, 28);
            this.lblEncryptionFor.Name = "lblEncryptionFor";
            this.lblEncryptionFor.Size = new System.Drawing.Size(100, 17);
            this.lblEncryptionFor.TabIndex = 13;
            this.lblEncryptionFor.Text = "Encryption For";
            // 
            // cbFor
            // 
            this.cbFor.FormattingEnabled = true;
            this.cbFor.Items.AddRange(new object[] {
            "Default",
            "Mehran",
            "Adams License"});
            this.cbFor.Location = new System.Drawing.Point(98, 26);
            this.cbFor.Name = "cbFor";
            this.cbFor.Size = new System.Drawing.Size(339, 24);
            this.cbFor.TabIndex = 12;
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(177, 109);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(75, 23);
            this.btnDecrypt.TabIndex = 8;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(98, 81);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(339, 22);
            this.txtOutput.TabIndex = 7;
            this.txtOutput.Enter += new System.EventHandler(this.txtOutput_Enter);
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(36, 81);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(51, 17);
            this.lblOutput.TabIndex = 10;
            this.lblOutput.Text = "Output";
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(36, 57);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(77, 17);
            this.lblInput.TabIndex = 9;
            this.lblInput.Text = "Enter Input";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(98, 54);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(339, 22);
            this.txtInput.TabIndex = 6;
            this.txtInput.Enter += new System.EventHandler(this.txtInput_Enter);
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(258, 109);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncrypt.TabIndex = 11;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
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
            this.textBox22.Size = new System.Drawing.Size(100, 22);
            this.textBox22.TabIndex = 64;
            // 
            // textBox23
            // 
            this.textBox23.Location = new System.Drawing.Point(544, 61);
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new System.Drawing.Size(100, 22);
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
            // pCORNInsightEncryptDecrypt
            // 
            this.pCORNInsightEncryptDecrypt.Controls.Add(this.btnDecryptInsight);
            this.pCORNInsightEncryptDecrypt.Controls.Add(this.textBox1);
            this.pCORNInsightEncryptDecrypt.Controls.Add(this.label1);
            this.pCORNInsightEncryptDecrypt.Controls.Add(this.label2);
            this.pCORNInsightEncryptDecrypt.Controls.Add(this.textBox2);
            this.pCORNInsightEncryptDecrypt.Controls.Add(this.btnEncryptInsight);
            this.pCORNInsightEncryptDecrypt.Location = new System.Drawing.Point(4, 25);
            this.pCORNInsightEncryptDecrypt.Name = "pCORNInsightEncryptDecrypt";
            this.pCORNInsightEncryptDecrypt.Size = new System.Drawing.Size(926, 356);
            this.pCORNInsightEncryptDecrypt.TabIndex = 1;
            this.pCORNInsightEncryptDecrypt.Text = "CORN Insight Encrypt Decrtypt";
            this.pCORNInsightEncryptDecrypt.UseVisualStyleBackColor = true;
            // 
            // btnDecryptInsight
            // 
            this.btnDecryptInsight.Location = new System.Drawing.Point(149, 75);
            this.btnDecryptInsight.Name = "btnDecryptInsight";
            this.btnDecryptInsight.Size = new System.Drawing.Size(75, 23);
            this.btnDecryptInsight.TabIndex = 14;
            this.btnDecryptInsight.Text = "Decrypt";
            this.btnDecryptInsight.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(85, 47);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(339, 22);
            this.textBox1.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Output";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Enter Input";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(85, 20);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(339, 22);
            this.textBox2.TabIndex = 12;
            // 
            // btnEncryptInsight
            // 
            this.btnEncryptInsight.Location = new System.Drawing.Point(230, 75);
            this.btnEncryptInsight.Name = "btnEncryptInsight";
            this.btnEncryptInsight.Size = new System.Drawing.Size(75, 23);
            this.btnEncryptInsight.TabIndex = 17;
            this.btnEncryptInsight.Text = "Encrypt";
            this.btnEncryptInsight.UseVisualStyleBackColor = true;
            // 
            // frmEncryptDecrypt
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(934, 392);
            this.Controls.Add(this.TabTerminal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmEncryptDecrypt";
            this.Text = "frmEncryptDecrypt";
            this.TabTerminal.ResumeLayout(false);
            this.pCORNEncryptDecrtype.ResumeLayout(false);
            this.pCORNEncryptDecrtype.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).EndInit();
            this.pCORNInsightEncryptDecrypt.ResumeLayout(false);
            this.pCORNInsightEncryptDecrypt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabTerminal;
        private System.Windows.Forms.TabPage pCORNEncryptDecrtype;
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
        private System.Windows.Forms.Label lblEncryptionFor;
        private System.Windows.Forms.ComboBox cbFor;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.TabPage pCORNInsightEncryptDecrypt;
        private System.Windows.Forms.Button btnDecryptInsight;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnEncryptInsight;
    }
}