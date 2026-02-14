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
    public partial class frmRemoveModule : Form
    {
        public frmRemoveModule(Main Parent)
        {
            InitializeComponent();
        }

        private void frmRemoveModule_Load(object sender, EventArgs e)
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
            System.Text.StringBuilder sbScript = new StringBuilder();
            for (int i = 0; i < clbModule.Items.Count; i++)
            {
                sbScript = new StringBuilder();
                if(clbModule.GetItemChecked(i))
                {
                    try
                    {
                        string conString = System.Configuration.ConfigurationSettings.AppSettings["connString"].ToString();
                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            string IDs = "";
                            if((clbModule.Items[i] as MyListBoxItem).Value == "28")
                            {
                                IDs = "IN (28,52)";
                            }
                            else if ((clbModule.Items[i] as MyListBoxItem).Value == "32")
                            {
                                IDs = "IN (32,53)";
                                sbScript.Append(Environment.NewLine);
                                sbScript.Append("UPDATE tblAppSettingDetail");
                                sbScript.Append(Environment.NewLine);
                                sbScript.Append("SET strColumnValue = '1'");
                                sbScript.Append(Environment.NewLine);
                                sbScript.Append("WHERE strColumnName = 'IsFinanceIntegrate'");
                            }
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} UPDATE MODULE SET IS_ACTIVE = 1 WHERE MODULE_ID " + IDs + sbScript.ToString(), cbDatabase.SelectedItem), con))
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
                        string conString = System.Configuration.ConfigurationSettings.AppSettings["connString"].ToString();
                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            string IDs = "";
                            if ((clbModule.Items[i] as MyListBoxItem).Value == "28")
                            {
                                IDs = "IN (28,52)";
                            }
                            else if ((clbModule.Items[i] as MyListBoxItem).Value == "32")
                            {
                                IDs = "IN (32,53)";
                                sbScript.Append(Environment.NewLine);
                                sbScript.Append("UPDATE tblAppSettingDetail");
                                sbScript.Append(Environment.NewLine);
                                sbScript.Append("SET strColumnValue = '0'");
                                sbScript.Append(Environment.NewLine);
                                sbScript.Append("WHERE strColumnName = 'IsFinanceIntegrate'");
                            }
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} UPDATE MODULE SET IS_ACTIVE = 0 WHERE [MODULE_ID] " + IDs + sbScript.ToString(), cbDatabase.SelectedItem), con))
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
            LoadModules();
        }
        
        public void LoadModules()
        {
            clbModule.Items.Clear();
            string conString = System.Configuration.ConfigurationSettings.AppSettings["connString"].ToString();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT MODULE_ID,MODULE_DESCRIPTION,IS_ACTIVE FROM [MODULE] WHERE MODULE_ID IN(28,32)", cbDatabase.SelectedItem), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clbModule.Items.Insert(0, new MyListBoxItem { Text = string.Format("{0}", dr[1]), Value = dr[0].ToString() });
                            if(dr[2].ToString().ToLower()=="true")
                            {
                                clbModule.SetItemChecked(0, true);
                            }
                        }
                    }
                }
                con.Close();
            }
            clbModule.DisplayMember = "Text";
            clbModule.ValueMember = "Value";
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

    public class MyListBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
