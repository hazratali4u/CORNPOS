using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

//using System.Data.SQLite;

namespace CORNPOSUtilityDesktop
{
    public partial class frmEncryptDecrypt : Form
    {
        string strKey = "b0tin@74";
        string strKeyInsight = "Attendance2017";

        private const string _keyString = "pokjmngf76174g59";
        private const string _ivString = "3254rv72tyv22ygh";

        public frmEncryptDecrypt(Main Parent)
        {
            InitializeComponent();
            cbFor.SelectedIndex = 0;
            txtInput.Focus();
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (cbFor.SelectedItem.ToString() == "Mehran")
            {
                strKey = "na0rh#em";
            }
            else if (cbFor.SelectedItem.ToString() == "Adams License")
            {
                strKey = "Fast1234";
            }
            else
            {
                strKey = "b0tin@74";
            }
            txtOutput.Text = CORNEncryptDecrypter.Cryptography.Decrypt(txtInput.Text, strKey);
            txtOutput.Focus();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (cbFor.SelectedItem.ToString() == "Mehran")
            {
                strKey = "na0rh#em";
            }
            else if (cbFor.SelectedItem.ToString() == "Adams License")
            {
                strKey = "Fast1234";
            }
            else
            {
                strKey = "b0tin@74";
            }
            txtOutput.Text = CORNEncryptDecrypter.Cryptography.Encrypt(txtInput.Text, strKey);
            txtOutput.Focus();
        }

        private void txtInput_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtOutput_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void btnDecryptInsight_Click(object sender, EventArgs e)
        {
            if(txtInputInsight.Text.Length > 0)
            {
                bool flag = true;
                byte[] inputBuffer = Convert.FromBase64String(txtInputInsight.Text);
                byte[] numArray;
                if (flag)
                {
                    MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
                    numArray = cryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(strKeyInsight));
                    cryptoServiceProvider.Clear();
                }
                else
                    numArray = Encoding.UTF8.GetBytes(strKeyInsight);
                TripleDESCryptoServiceProvider cryptoServiceProvider1 = new TripleDESCryptoServiceProvider();
                cryptoServiceProvider1.Key = numArray;
                cryptoServiceProvider1.Mode = CipherMode.ECB;
                cryptoServiceProvider1.Padding = PaddingMode.PKCS7;
                byte[] bytes = cryptoServiceProvider1.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                cryptoServiceProvider1.Clear();
                txtOutputInsight.Text= Encoding.UTF8.GetString(bytes);
                txtOutputInsight.Focus();
            }
        }

        private void btnEncryptInsight_Click(object sender, EventArgs e)
        {
            if(txtInputInsight.Text.Length>0)
            {
                bool flag = true;
                byte[] bytes = Encoding.UTF8.GetBytes(txtInputInsight.Text);
                byte[] numArray;
                if (flag)
                {
                    MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
                    numArray = cryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(strKeyInsight));
                    cryptoServiceProvider.Clear();
                }
                else
                    numArray = Encoding.UTF8.GetBytes(strKeyInsight);
                TripleDESCryptoServiceProvider cryptoServiceProvider1 = new TripleDESCryptoServiceProvider();
                cryptoServiceProvider1.Key = numArray;
                cryptoServiceProvider1.Mode = CipherMode.ECB;
                cryptoServiceProvider1.Padding = PaddingMode.PKCS7;
                byte[] inArray = cryptoServiceProvider1.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
                cryptoServiceProvider1.Clear();
                txtOutputInsight.Text =  Convert.ToBase64String(inArray, 0, inArray.Length);
                txtOutputInsight.Focus();
            }
        }

        private void txtInputInsight_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtOutputInsight_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void btnDecryptNew_Click(object sender, EventArgs e)
        {
            if(txtInputNew.Text.Length > 0)
            {
                string combinedString = txtInputNew.Text;
                string plainText = string.Empty;
                try
                {
                    byte[] combinedData = Convert.FromBase64String(combinedString);
                    Aes aes = Aes.Create();
                    aes.Key = Encoding.UTF8.GetBytes(_keyString);
                    aes.IV = Encoding.UTF8.GetBytes(_ivString);

                    byte[] iv = aes.IV;
                    byte[] cipherText = new byte[combinedData.Length - iv.Length];

                    Array.Copy(combinedData, iv, iv.Length);
                    Array.Copy(combinedData, iv.Length, cipherText, 0, cipherText.Length);

                    aes.Mode = CipherMode.CBC;

                    ICryptoTransform decipher = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(cipherText))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decipher, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                plainText = sr.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception) { }
                txtOutputNew.Text = plainText;
                txtOutputNew.Focus();
            }
        }

        private void txtInputNew_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtOutputNew_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void btnEncryptNew_Click(object sender, EventArgs e)
        {
            if (txtInputNew.Text.Length > 0)
            {
                string plainText = txtInputNew.Text;
                byte[] cipherData;
                Aes aes = Aes.Create();

                aes.Key = Encoding.UTF8.GetBytes(_keyString);
                aes.IV = Encoding.UTF8.GetBytes(_ivString);
                aes.Mode = CipherMode.CBC;
                ICryptoTransform cipher = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, cipher, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }

                    cipherData = ms.ToArray();
                }
                byte[] combinedData = new byte[aes.IV.Length + cipherData.Length];
                Array.Copy(aes.IV, 0, combinedData, 0, aes.IV.Length);
                Array.Copy(cipherData, 0, combinedData, aes.IV.Length, cipherData.Length);
                txtOutputNew.Text = Convert.ToBase64String(combinedData);
                txtOutputNew.Focus();
            }
        }
    }
}