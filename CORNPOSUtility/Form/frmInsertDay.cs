using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace CORNPOSUtility
{
    public partial class frmInsertDay : Form
    {
        public frmInsertDay(Main Parent)
        {
            InitializeComponent();
        }
        
        private void frmInsertDay_Load(object sender, EventArgs e)
        {
            dtpDate.Value = System.DateTime.Now.AddDays(-1);
            List<string> strDatabases = Main.list;
            foreach (string db in strDatabases)
            {
                cbDatabase.Items.Add(db);
            }
        }

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLocations();
        }

        public void LoadLocations()
        {
            cblLocation.Items.Clear();
            string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT D.DISTRIBUTOR_ID,D.DISTRIBUTOR_NAME,(SELECT MAX(CLOSING_DATE) FROM DAILY_CLOSE WHERE DISTRIBUTOR_ID = D.DISTRIBUTOR_ID) AS WorkingDate FROM DISTRIBUTOR D WHERE D.ISDELETED = 0", cbDatabase.SelectedItem), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cblLocation.Items.Insert(0, new MyListBoxItem { Text = string.Format("{0}", dr[1].ToString() + '-' + dr[2].ToString()), Value = dr[0].ToString() });                            
                        }
                    }
                }
                con.Close();
            }
            cblLocation.DisplayMember = "Text";
            cblLocation.ValueMember = "Value";
        }

        private void btnDayClose_Click(object sender, EventArgs e)
        {
            if (InsertDayClose())
            {
                if(UpdatePrices())
                {
                    ZKMessgeBox.Show("Day entered successfully!.");
                    LoadLocations();
                }
                else
                {
                    ZKMessgeBox.Show("Some error occured!.");
                }
            }
            else
            {
                ZKMessgeBox.Show("Some error occured!.");
            }
        }
        private bool InsertDayClose()
        {
            bool flag = true;
            int count = 0;
            for (int i = 0; i < cblLocation.Items.Count; i++)
            {
                if (cblLocation.GetItemChecked(i))
                {
                    try
                    {
                        string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} INSERT INTO DAILY_CLOSE(CLOSING_DATE,DISTRIBUTOR_ID,OPENING_CASH,TIME_STAMP) VALUES('" + dtpDate.Value.ToString("yyyy - MM - dd") + "'," + (cblLocation.Items[i] as MyListBoxItem).Value + ",0,'" + System.DateTime.Now + "') ", cbDatabase.SelectedItem), con))
                            {
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    count++;
                                }
                            }
                            con.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
        private bool UpdatePrices()
        {
            bool flag = true;
            int count = 0;
            for (int i = 0; i < cblLocation.Items.Count; i++)
            {
                if (cblLocation.GetItemChecked(i))
                {
                    try
                    {
                        string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} UPDATE SKU_PRICES SET DATE_EFFECTED = '" + dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "' WHERE DISTRIBUTOR_ID =" + (cblLocation.Items[i] as MyListBoxItem).Value, cbDatabase.SelectedItem), con))
                            {
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    count++;
                                }
                            }
                            con.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
    }
}