using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using System.Data.SQLite;
using System.Threading;
using System.Management;
using System.IO;
using System.Data.SqlClient;

namespace CORNPOSUtility
{
    public partial class frmScript : Form
    {
        private BackgroundWorker backgroundWorker;
        
        public frmScript(Main Parent)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            InitializeComponent();
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            int counterror = 0;
            for (int i = 0; i < cblDb.Items.Count; i++)
            {
                if (cblDb.GetItemChecked(i))
                {                    
                    if (ExecuteScript(cblDb.Items[i].ToString(), Environment.NewLine + txtScript.Text))
                    {
                        count++;
                        this.Invoke(new MethodInvoker(delegate ()
                      {
                          richStatus.Text += cblDb.Items[i].ToString() + " script executed" + Environment.NewLine;
                      }));
                    }
                    else
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            counterror++;
                            richStatus.Text += cblDb.Items[i].ToString() + " script error!" + Environment.NewLine;
                        }));
                    }
                }
            }

            this.Invoke(new MethodInvoker(delegate ()
            {
                richStatus.Text += count.ToString() + " db(s) updated." + Environment.NewLine;
                richStatus.Text += counterror.ToString() + " db(s) error." + Environment.NewLine;
            }));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ZKMessgeBox.Show("Script executed.");
        }
        private void btnScript_Click(object sender, EventArgs e)
        {
            if (txtScript.Text.Length > 0)
            {
                int count = 0;
                for (int i = 0; i < cblDb.Items.Count; i++)
                {
                    if (cblDb.GetItemChecked(i))
                    {
                        count++;
                        break;
                    }
                }
                if (count > 0)
                {
                    var confirmResult = ZKMessgeBox.Show("Are you sure?", "Confirm Build Upload!!", ZKMessgeBox.I8Buttons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        richStatus.Text = string.Empty;
                        backgroundWorker.RunWorkerAsync();                        
                    }
                }
                else
                {
                    ZKMessgeBox.Show("Select database(s)!!");
                    cblDb.Focus();
                }
            }
            else
            {
                ZKMessgeBox.Show("Enter script!!");
                txtScript.Focus();
            }
        }

        private void frmScript_Load(object sender, EventArgs e)
        {
            List<string> strDatabases = Main.list;
            foreach (string db in strDatabases)
            {
                cblDb.Items.Add(db);
            }
            for (int i = 0; i < cblDb.Items.Count; i++)
            {
                cblDb.SetItemChecked(i, cbAll.Checked);

            }
        }

        private bool ExecuteScript(string DBName, string script)
        {
            bool flag = true;
            try
            {
                string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USE " + DBName.Trim() + script, con))
                    {
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        private void cbAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cblDb.Items.Count; i++)
            {
                cblDb.SetItemChecked(i, cbAll.Checked);
                
            }
        }
    }
}