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
    public partial class frmAllowInsight : Form
    {
        bool busy = false;
        public frmAllowInsight(Main Parent)
        {
            InitializeComponent();
        }

        private void frmAllowInsight_Load(object sender, EventArgs e)
        {
            List<string> strDatabases = Main.list;
            foreach (string db in strDatabases)
            {
                cbDatabase.Items.Add(db);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int count = 0;
            for (int i = 0; i < clbUser.Items.Count; i++)
            {
                if(clbUser.GetItemChecked(i))
                {
                    try
                    {
                        string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} UPDATE [USER] SET IsMobileInsightAllowed = 1 WHERE [USER_ID] = " + (clbUser.Items[i] as MyListBoxItem).Value, cbDatabase.SelectedItem), con))
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
                        break;
                    }
                }
                else
                {
                    try
                    {
                        string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} UPDATE [USER] SET IsMobileInsightAllowed = 0 WHERE [USER_ID] = " + (clbUser.Items[i] as MyListBoxItem).Value, cbDatabase.SelectedItem), con))
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
                        break;
                    }
                }
            }
            if (count > 0)
            {
                ZKMessgeBox.Show("User(s) updated successfully.");
            }
        }


        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }
        
        public void LoadUsers()
        {
            clbUser.Items.Clear();
            string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT USER_ID,LOGIN_ID,IsMobileInsightAllowed,PASSWORD FROM [USER]", cbDatabase.SelectedItem), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clbUser.Items.Insert(0, new MyListBoxItem { Text = string.Format("{0}-{1}", dr[1], dr[3]), Value = dr[0].ToString() });
                            if(dr[2].ToString().ToLower()=="true")
                            {
                                clbUser.SetItemChecked(0, true);
                            }
                        }
                    }
                }
                con.Close();
            }
            clbUser.DisplayMember = "Text";
            clbUser.ValueMember = "Value";
        }

        public static string Encrypt(string PlainText, string Key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(Key);

            byte[] buffer = Encoding.UTF8.GetBytes(PlainText);
            MemoryStream stream = new MemoryStream();
            try
            {
                DES des = new DESCryptoServiceProvider();
                CryptoStream stream2 = new CryptoStream(stream, des.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
            }
            catch (Exception ex)
            {

            }
            return Convert.ToBase64String(stream.ToArray());
        }        
    }    
}
