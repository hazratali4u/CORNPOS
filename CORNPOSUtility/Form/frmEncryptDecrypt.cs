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

namespace CORNPOSUtility
{
    public partial class frmEncryptDecrypt : Form
    {
        string strKey = "";
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

    }
}
