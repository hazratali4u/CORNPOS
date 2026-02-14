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
    public partial class frmUpdateLicense : Form
    {
        string CryptographyKey = "b0tin@74";
        public frmUpdateLicense(Main Parent)
        {
            InitializeComponent();
        }
        private void frmUpdateLicense_Load(object sender, EventArgs e)
        {
            List<string> strDatabases = Main.list;
            foreach (string db in strDatabases)
            {
                cbDatabase.Items.Add(db);
            }

            DateTime dtMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime dtLicenseDate = dtMonthStart.AddMonths(1).AddDays(19);
            if (DateTime.Now < dtMonthStart.AddDays(20))
            {
                dtLicenseDate = dtMonthStart.AddDays(19);
            }
            dtpDate.Value = dtLicenseDate;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int count = 0;
            for (int i = 0; i < clbLocation.Items.Count; i++)
            {
                if (clbLocation.GetItemChecked(i))
                {
                    try
                    {
                        string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} UPDATE DISTRIBUTOR SET LICENSE_DATE = '" + Encrypt(dtpDate.Value.ToString("yyyy-MM-dd"), CryptographyKey) + "' WHERE DISTRIBUTOR_ID=" + (clbLocation.Items[i] as MyListBoxItem).Value, cbDatabase.SelectedItem), con))
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
            if(count>0)
            {
                ZKMessgeBox.Show("License updated successfully!.");
                LoadLocations();
            }
            else
            {
                ZKMessgeBox.Show("Select any location.");
            }
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

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLocations();
        }
        public void LoadLocations()
        {
            clbLocation.Items.Clear();
            string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT D.DISTRIBUTOR_ID,D.DISTRIBUTOR_NAME,LICENSE_DATE FROM DISTRIBUTOR D WHERE D.ISDELETED = 0", cbDatabase.SelectedItem), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[2].ToString().Length > 0)
                            {
                                clbLocation.Items.Insert(0, new MyListBoxItem { Text = string.Format("{0}", dr[1].ToString() + '-' + Convert.ToDateTime(CORNEncryptDecrypter.Cryptography.Decrypt(dr[2].ToString(), CryptographyKey)).ToString("dd-MMM-yyyy")), Value = dr[0].ToString() });
                            }
                            else
                            {
                                clbLocation.Items.Insert(0, new MyListBoxItem { Text = string.Format("{0}", dr[1].ToString() + '-' + DateTime.Now.ToString("dd-MMM-yyyy")), Value = dr[0].ToString() });
                            }
                        }
                    }
                }
                con.Close();
            }
            clbLocation.DisplayMember = "Text";
            clbLocation.ValueMember = "Value";
        }
    }
}
