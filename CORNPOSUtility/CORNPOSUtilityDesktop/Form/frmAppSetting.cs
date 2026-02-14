using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace CORNPOSUtilityDesktop
{
    public partial class frmAppSetting : Form
    {
        DataTable dtAppSeting = new DataTable();
        string conString = System.Configuration.ConfigurationSettings.AppSettings["connString"].ToString();
        public frmAppSetting(Main Parent)
        {
            InitializeComponent();
        }

        private void frmAppSetting_Load(object sender, EventArgs e)
        {
            List<string> strDatabases = Main.list;
            foreach (string db in strDatabases)
            {
                cbDatabase.Items.Add(db);
            }
            CreateTable();
        }
        
        private void CreateTable()
        {
            dtAppSeting.Columns.Add("intCode", typeof(int));
            dtAppSeting.Columns.Add("strColumnName", typeof(string));
            dtAppSeting.Columns.Add("strColumnValue", typeof(string));
            dtAppSeting.Columns.Add("strDescription", typeof(string));
        }

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadColumns();
        }

        public void LoadColumns()
        {
            dtAppSeting.Rows.Clear();
            cbColumnName.Items.Clear();            
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT DISTINCT intCode,strColumnName,strColumnValue,strDescription FROM tblAppSettingDetail ORDER BY strColumnName DESC ", cbDatabase.SelectedItem), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drNew = dtAppSeting.NewRow();
                            drNew["intCode"] = dr[0];
                            drNew["strColumnName"] = dr[1];
                            drNew["strColumnValue"] = dr[2];
                            drNew["strDescription"] = dr[3];
                            dtAppSeting.Rows.Add(drNew);
                            cbColumnName.Items.Insert(0, new MyListBoxItem { Text = string.Format("{0}", dr[1]), Value = dr[0].ToString() });                            
                        }
                    }
                }
                con.Close();
            }
            cbColumnName.DisplayMember = "Text";
            cbColumnName.ValueMember = "Value";
        }

        private void cbColumnName_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(DataRow dr in dtAppSeting.Rows)
            {
                MyListBoxItem mItem = (MyListBoxItem)cbColumnName.SelectedItem;
                
                if (dr["intCode"].ToString() == mItem.Value.ToString())
                {
                    txtColumnValue.Text = dr["strColumnValue"].ToString();
                    txtDescription.Text = dr["strDescription"].ToString();
                    break;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool flag = false;
            MyListBoxItem mItem = (MyListBoxItem)cbColumnName.SelectedItem;            
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} UPDATE tblAppSettingDetail SET strColumnValue ='" + txtColumnValue.Text + "' WHERE intCode =" + mItem.Value , cbDatabase.SelectedItem), con))
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        flag = true;
                    }
                }
                con.Close();
            }
            if(flag)
            {
                LoadColumns();
                ZKMessgeBox.Show("App Setting updated successfully.");
            }
        }
    }
}