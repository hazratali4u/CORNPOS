using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace CORNPOSUtility
{
    public partial class frmDataManagement : Form
    {
        DataTable dtGridItem = new DataTable();
        DataTable dtPurchase = new DataTable();
        DataTable dtPurchase2 = new DataTable();
        string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
        public frmDataManagement(Main Parent)
        {
            InitializeComponent();
        }
        private void frmTruncateTran_Load(object sender, EventArgs e)
        {
            dtpDate.Value = System.DateTime.Now;
            dtpToDate.Value = System.DateTime.Now;
            List<string> strDatabases = Main.list;

            foreach (string db in strDatabases)
            {
                cbDatabase.Items.Add(db);
            }

            clbMenuItem.Items.Add("Sections", true);
            clbMenuItem.Items.Add("Categories", true);
            clbMenuItem.Items.Add("Items", true);
            gridItem.AutoGenerateColumns = false;
        }

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLocations();
            if (cblLocation.Items.Count > 0)
            {
                cblLocation.SelectedIndex = 0;
            }

            DataTable dtSupplier = GetData(0, 0, 2);
            if (dtSupplier.Rows.Count > 0)
            {
                cbNewSupplier.DataSource = dtSupplier;
                cbNewSupplier.DisplayMember = "SKU_HIE_NAME";
                cbNewSupplier.ValueMember = "SKU_HIE_ID"; ;
            }
            else
            {
                cbNewSupplier.DataSource = null;
                cbNewSupplier.DataSource = null;
            }
        }
        public void LoadLocations()
        {
            Dictionary<int, string> locationListDictionary = new Dictionary<int, string>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT D.DISTRIBUTOR_ID,D.DISTRIBUTOR_NAME FROM DISTRIBUTOR D WHERE D.ISDELETED = 0", cbDatabase.SelectedItem), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            locationListDictionary.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                        }
                    }
                }
                con.Close();
            }
            cblLocation.DataSource = new BindingSource(locationListDictionary, null);
            cblLocation.DisplayMember = "Value";
            cblLocation.ValueMember = "Key";

            cbToPriceLocation.DataSource = new BindingSource(locationListDictionary, null);
            cbToPriceLocation.DisplayMember = "Value";
            cbToPriceLocation.ValueMember = "Key";
        }

        public void LoadItem()
        {
            Dictionary<int, string> locationListDictionary = new Dictionary<int, string>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT SKU_ID AS ItemID,SKU_NAME AS ItemName FROM SKUS ", cbDatabase.SelectedItem), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                        }
                    }
                }
                con.Close();
            }

        }

        public void LoadPurchaseDocument2()
        {
            int SelectedLocation = 0;
            try
            {
                SelectedLocation = ((System.Collections.Generic.KeyValuePair<int, string>)cblLocation.SelectedValue).Key;
            }
            catch (Exception)
            {
                SelectedLocation = Convert.ToInt32(cblLocation.SelectedValue);
            }
            Dictionary<long, string> purchaseListDictionary = new Dictionary<long, string>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT PURCHASE_MASTER_ID,CASE PAYMENT_MODE WHEN 2 THEN 'Cash'ELSE 'Credit' END As PaymentMod FROM PURCHASE_MASTER WHERE PAYMENT_MODE = 2 AND DISTRIBUTOR_ID = {1} ORDER BY PURCHASE_MASTER_ID DESC", cbDatabase.SelectedItem, SelectedLocation), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            purchaseListDictionary.Add(Convert.ToInt64(dr[0]), dr[0].ToString() + "-" + dr[1].ToString());
                        }
                    }
                }
                con.Close();
            }
        }

        private void cblLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cblLocation.Items.Count > 0)
            {
                int SelectedLocation = 0;
                try
                {
                    SelectedLocation = ((System.Collections.Generic.KeyValuePair<int, string>)cblLocation.SelectedValue).Key;
                }
                catch (Exception)
                {
                    SelectedLocation = Convert.ToInt32(cblLocation.SelectedValue);
                }
                dtPurchase = GetData(SelectedLocation, 0, 1);
                if (dtPurchase.Rows.Count > 0)
                {
                    cbPurDocument.DataSource = dtPurchase;
                    cbPurDocument.DisplayMember = "PaymentMod";
                    cbPurDocument.ValueMember = "PURCHASE_MASTER_ID";
                }
                else
                {
                    cbPurDocument.DataSource = null;
                }
                DataTable dtItem = LoadItems(SelectedLocation, 1, null);
                if (dtItem.Rows.Count > 0)
                {
                    cblItem.DataSource = dtItem;
                    cblItem.DisplayMember = "SKU_NAME";
                    cblItem.ValueMember = "SKU_ID";
                    cbAllItem.Checked = true;
                    for (int i = 0; i < cblItem.Items.Count; i++)
                    {
                        cblItem.SetItemChecked(i, true);
                    }
                }
                dtPurchase2 = GetData(SelectedLocation, 0, 3);
                if (dtPurchase2.Rows.Count > 0)
                {
                    Dictionary<long, string> purchaseListDictionary = new Dictionary<long, string>();
                    foreach (DataRow dr in dtPurchase2.Rows)
                    {
                        purchaseListDictionary.Add(Convert.ToInt64(dr["PURCHASE_MASTER_ID"]), dr["SupplierName"].ToString());
                    }
                    cbPurDocument2.DataSource = new BindingSource(purchaseListDictionary, null);
                    cbPurDocument2.DisplayMember = "Value";
                    cbPurDocument2.ValueMember = "Key";

                }
                else
                {
                    cbPurDocument2.DataSource = null;
                }
                gridItem.DataSource = null;
                dtGridItem = LoadItems(SelectedLocation, 2, null);
                gridItem.DataSource = dtGridItem;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            int SelectedLocation = 0;
            try
            {
                SelectedLocation = ((KeyValuePair<int, string>)cblLocation.SelectedValue).Key;
            }
            catch (Exception)
            {
                SelectedLocation = Convert.ToInt32(cblLocation.SelectedValue);
            }
            if (btnConvert.Text == "Convert To Cash")
            {
                var confirmResult = ZKMessgeBox.Show("Are you sure to convert to cash??", "Confirm Convert To Cash!!", ZKMessgeBox.I8Buttons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    UpdatePurchaseMaster(SelectedLocation, Convert.ToInt64(cbPurDocument.SelectedValue), 2);
                    DataRow drPur = dtPurchase.Rows[0];
                    long VoucherNo = GetVoucherNo(SelectedLocation);
                    //Cash Purchase
                    InsertLedger(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToInt32(drPur["USER_ID"]), 25, 0, Convert.ToDecimal(drPur["NET_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]), Convert.ToDateTime(drPur["DOCUMENT_DATE"]), Convert.ToInt64(drPur["Inventoryatstore"]), VoucherNo, Convert.ToInt32(cbPurDocument.SelectedValue), cbPurDocument.SelectedValue.ToString(), "Purchase Cash, INV#: " + drPur["ORDER_NUMBER"].ToString() + " , Supplier: " + drPur["SupplierName"].ToString() + " ," + drPur["BUILTY_NO"].ToString(), "Purchase Cash");
                    InsertLedger(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToInt32(drPur["USER_ID"]), 25, Convert.ToDecimal(drPur["NET_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]), 0, Convert.ToDateTime(drPur["DOCUMENT_DATE"]), Convert.ToInt64(drPur["AccountPayable"]), VoucherNo, Convert.ToInt32(cbPurDocument.SelectedValue), cbPurDocument.SelectedValue.ToString(), "Purchase Cash, INV#: " + drPur["ORDER_NUMBER"].ToString() + " , Supplier: " + drPur["SupplierName"].ToString() + " ," + drPur["BUILTY_NO"].ToString(), "Purchase Cash");

                    //Cash Payment
                    InsertLedger(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToInt32(drPur["USER_ID"]), 25, 0, Convert.ToDecimal(drPur["NET_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]), Convert.ToDateTime(drPur["DOCUMENT_DATE"]), Convert.ToInt64(drPur["AccountPayable"]), VoucherNo, Convert.ToInt32(cbPurDocument.SelectedValue), cbPurDocument.SelectedValue.ToString(), "Purchase Cash, INV#: " + drPur["ORDER_NUMBER"].ToString() + " , Supplier: " + drPur["SupplierName"].ToString() + " ," + drPur["BUILTY_NO"].ToString(), "Purchase Cash");
                    InsertLedger(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToInt32(drPur["USER_ID"]), 25, Convert.ToDecimal(drPur["NET_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]), 0, Convert.ToDateTime(drPur["DOCUMENT_DATE"]), Convert.ToInt64(drPur["Inventoryatstore"]), VoucherNo, Convert.ToInt32(cbPurDocument.SelectedValue), cbPurDocument.SelectedValue.ToString(), "Purchase Cash, INV#: " + drPur["ORDER_NUMBER"].ToString() + " , Supplier: " + drPur["SupplierName"].ToString() + " ," + drPur["BUILTY_NO"].ToString(), "Purchase Cash");

                    if (drPur["IsFinanceIntegrate"].ToString() == "1")
                    {
                        string VoucherNo2 = GetVoucherNo(SelectedLocation, Convert.ToDateTime(drPur["DOCUMENT_DATE"]));

                        if (InsertGLMaster(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToInt32(drPur["USER_ID"]), Convert.ToInt64(cbPurDocument.SelectedValue), Convert.ToDateTime(drPur["DOCUMENT_DATE"]), VoucherNo2, "Purchase Cash, INV#: " + drPur["ORDER_NUMBER"].ToString() + " , Supplier: " + drPur["SupplierName"].ToString() + " ," + drPur["BUILTY_NO"].ToString()))
                        {
                            bool flag = false;
                            flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToDecimal(drPur["TOTAL_AMOUNT"]), 0, Convert.ToInt64(drPur["Inventoryatstore"]), VoucherNo2, "Inventory at Store Purchase Voucher");
                            flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), 0, Convert.ToDecimal(drPur["TOTAL_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]) - Convert.ToDecimal(drPur["DISCOUNT"]), Convert.ToInt64(drPur["AccountPayable"]), VoucherNo2, "Account(s) Payable Purchase Voucher");

                            flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToDecimal(drPur["TOTAL_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]) - Convert.ToDecimal(drPur["DISCOUNT"]), 0, Convert.ToInt64(drPur["AccountPayable"]), VoucherNo2, "Account(s) Payable Purchase Voucher");
                            flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), 0, Convert.ToDecimal(drPur["TOTAL_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]) - Convert.ToDecimal(drPur["DISCOUNT"]) + Convert.ToDecimal(drPur["GST_AMOUNT"]), Convert.ToInt64(drPur["CashInHand"]), VoucherNo2, "CashInHand Purchase Voucher");

                            if (Convert.ToDecimal(drPur["DISCOUNT"]) > 0)
                            {
                                flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), 0, Convert.ToDecimal(drPur["DISCOUNT"]), Convert.ToInt64(drPur["PurchaseDiscount"]), VoucherNo2, "PurchaseDiscount Purchase Voucher");
                            }
                            if (Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]) > 0)
                            {
                                flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]), 0, Convert.ToInt64(drPur["Freight"]), VoucherNo2, "Freight Purchase Voucher");
                            }
                            if (Convert.ToDecimal(drPur["GST_AMOUNT"]) > 0)
                            {
                                flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToDecimal(drPur["GST_AMOUNT"]), 0, Convert.ToInt64(drPur["PurchaseTax"]), VoucherNo2, "PurchaseTax Purchase Voucher");
                            }

                            if (flag)
                            {
                                ZKMessgeBox.Show("Record updated successfully");
                            }
                            else
                            {
                                ZKMessgeBox.Show("Some error occured.");
                            }
                        }
                    }
                }
            }
            else
            {
                var confirmResult = ZKMessgeBox.Show("Are you sure to convert to credit??", "Confirm Convert To Credit!!", ZKMessgeBox.I8Buttons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    UpdatePurchaseMaster(SelectedLocation, Convert.ToInt64(cbPurDocument.SelectedValue), 1);
                    DataRow drPur = dtPurchase.Rows[0];

                    long VoucherNo = GetVoucherNo(SelectedLocation);
                    InsertLedger(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToInt32(drPur["USER_ID"]), 24, 0, Convert.ToDecimal(drPur["NET_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]), Convert.ToDateTime(drPur["DOCUMENT_DATE"]), Convert.ToInt64(drPur["Inventoryatstore"]), VoucherNo, Convert.ToInt32(cbPurDocument.SelectedValue), cbPurDocument.SelectedValue.ToString(), "Purchase Voucher, INV#: " + drPur["ORDER_NUMBER"].ToString() + " , Supplier: " + drPur["SupplierName"].ToString() + " ," + drPur["BUILTY_NO"].ToString(), "Purchase");
                    InsertLedger(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToInt32(drPur["USER_ID"]), 24, Convert.ToDecimal(drPur["NET_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]), 0, Convert.ToDateTime(drPur["DOCUMENT_DATE"]), Convert.ToInt64(drPur["AccountPayable"]), VoucherNo, Convert.ToInt32(cbPurDocument.SelectedValue), cbPurDocument.SelectedValue.ToString(), "Purchase Voucher, INV#: " + drPur["ORDER_NUMBER"].ToString() + " , Supplier: " + drPur["SupplierName"].ToString() + " ," + drPur["BUILTY_NO"].ToString(), "Purchase");

                    if (drPur["IsFinanceIntegrate"].ToString() == "1")
                    {
                        string VoucherNo2 = GetVoucherNo(SelectedLocation, Convert.ToDateTime(drPur["DOCUMENT_DATE"]));
                        if (InsertGLMaster(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToInt32(drPur["USER_ID"]), Convert.ToInt64(cbPurDocument.SelectedValue), Convert.ToDateTime(drPur["DOCUMENT_DATE"]), VoucherNo2, "Purchase Cash, INV#: " + drPur["ORDER_NUMBER"].ToString() + " , Supplier: " + drPur["SupplierName"].ToString() + " ," + drPur["BUILTY_NO"].ToString()))
                        {
                            bool flag = false;
                            flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToDecimal(drPur["TOTAL_AMOUNT"]), 0, Convert.ToInt64(drPur["Inventoryatstore"]), VoucherNo2, "Purchase Voucher");
                            flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), 0, Convert.ToDecimal(drPur["TOTAL_AMOUNT"]) + Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]) - Convert.ToDecimal(drPur["DISCOUNT"]) + Convert.ToDecimal(drPur["GST_AMOUNT"]), Convert.ToInt64(drPur["AccountPayable"]), VoucherNo2, "Account(s) Payable Purchase Voucher");

                            if (Convert.ToDecimal(drPur["DISCOUNT"]) > 0)
                            {
                                flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), 0, Convert.ToDecimal(drPur["DISCOUNT"]), Convert.ToInt64(drPur["PurchaseDiscount"]), VoucherNo2, "Account(s) Payable Discount Purchase Voucher");
                            }
                            if (Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]) > 0)
                            {
                                flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToDecimal(drPur["FREIGHT_AMOUNT"]), 0, Convert.ToInt64(drPur["Freight"]), VoucherNo2, "Account(s) Payable Freight Purchase Voucher");
                            }
                            if (Convert.ToDecimal(drPur["GST_AMOUNT"]) > 0)
                            {
                                flag = InsertGLDetail(SelectedLocation, Convert.ToInt32(drPur["PRINCIPAL_ID"]), Convert.ToDecimal(drPur["GST_AMOUNT"]), 0, Convert.ToInt64(drPur["PurchaseTax"]), VoucherNo2, "GST on Purchase Voucher");
                            }
                            if (flag)
                            {
                                ZKMessgeBox.Show("Record updated successfully");
                            }
                            else
                            {
                                ZKMessgeBox.Show("Some error occured.");
                            }
                        }
                    }
                }
            }
            GetData(SelectedLocation, 0, 1);
            if (dtPurchase.Rows.Count > 0)
            {
                cbPurDocument.DataSource = dtPurchase;
                cbPurDocument.DisplayMember = "PaymentMod";
                cbPurDocument.ValueMember = "PURCHASE_MASTER_ID";
            }
            else
            {
                cbPurDocument.DataSource = null;
            }
        }

        private void cbPurDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPurDocument.Text.Contains("Credit"))
            {
                btnConvert.Text = "Convert To Cash";
            }
            else
            {
                btnConvert.Text = "Convert To Credit";
            }
        }

        public DataTable GetData(int SelectedLocation, long pDocumentID, int pTypeID)
        {
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {

                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetDataUtility";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TypeID", DbType = DbType.Int32, Value = pTypeID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DocumentID", DbType = DbType.Int64, Value = pDocumentID };
                        pparams.Add(parameter);

                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetVoucherNo(int SelectedLocation, DateTime mDate)
        {
            string MaxVoucherId = "";
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "UspSelectMaxVoucherNo";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@Document_TypeId", DbType = DbType.Int32, Value = 16 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@Distributor_id", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@Month", DbType = DbType.DateTime, Value = mDate };
                        pparams.Add(parameter);

                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        DataTable MaxId = ds.Tables[0];
                        MaxVoucherId = MaxId.Rows[0][0].ToString();

                        if (MaxVoucherId.Length == 1)
                        {
                            if (mDate.Month.ToString().Length == 1)
                            {
                                MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0000" + MaxVoucherId;
                            }
                            else
                            {
                                MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0000" + MaxVoucherId;
                            }

                        }
                        else if (MaxVoucherId.Length == 2)
                        {
                            if (mDate.Month.ToString().Length == 1)
                            {
                                MaxVoucherId = MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-000" + MaxVoucherId;
                            }
                            else
                            {
                                MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-000" + MaxVoucherId;
                            }

                        }
                        else if (MaxVoucherId.Length == 3)
                        {
                            if (mDate.Month.ToString().Length == 1)
                            {
                                MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-00" + MaxVoucherId;
                            }
                            else
                            {
                                MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-00" + MaxVoucherId;
                            }

                        }
                        else if (MaxVoucherId.Length == 4)
                        {
                            if (mDate.Month.ToString().Length == 1)
                            {
                                MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0" + MaxVoucherId;
                            }
                            else
                            {
                                MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0" + MaxVoucherId;
                            }

                        }
                        else
                        {
                            if (mDate.Month.ToString().Length == 1)
                            {
                                MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-" + MaxVoucherId;
                            }
                            else
                            {
                                MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-" + MaxVoucherId;
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return MaxVoucherId;
        }

        public long GetVoucherNo(int SelectedLocation)
        {
            long MaxVoucherId = 0;
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "UspSelectMaxDocumentNo";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@Document_TypeId", DbType = DbType.Int32, Value = 16 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@Distributor_id", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TypeId", DbType = DbType.Int32, Value = 1 };
                        pparams.Add(parameter);

                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        DataTable MaxId = ds.Tables[0];
                        MaxVoucherId = Convert.ToInt64(MaxId.Rows[0][0]);

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return MaxVoucherId;
        }

        public void InsertLedger(int SelectedLocation, int PRINCIPAL_ID, int USER_ID, int PAYMENT_MODE, decimal DEBIT, decimal CREDIT, DateTime LEDGER_DATE, long ACCOUNT_HEAD_ID, long VOUCHER_NO, long DOCUMENT_NO, string MANUAL_DOCUMENT_NO, string REMARKS, string PAYEE_NAME)
        {
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "spInsertVENDER_LEDGER";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@POSTING", DbType = DbType.Int32, Value = 0 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@IS_DELETED", DbType = DbType.Int32, Value = 0 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_TYPE_ID", DbType = DbType.Int32, Value = 16 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PRINCIPAL_ID", DbType = DbType.Int32, Value = PRINCIPAL_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DOCUMENT_TYPE_ID", DbType = DbType.Int32, Value = 2 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@USER_ID", DbType = DbType.Int32, Value = USER_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PAYMENT_MODE", DbType = DbType.Int32, Value = PAYMENT_MODE };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DEBIT", DbType = DbType.Decimal, Value = DEBIT };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CREDIT", DbType = DbType.Decimal, Value = CREDIT };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@LEDGER_DATE", DbType = DbType.DateTime, Value = LEDGER_DATE };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TIME_STAMP", DbType = DbType.DateTime, Value = DateTime.Now };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CHQUE_DATE", DbType = DbType.DateTime, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@ACCOUNT_HEAD_ID", DbType = DbType.Int64, Value = ACCOUNT_HEAD_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_NO", DbType = DbType.Int64, Value = VOUCHER_NO };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DOCUMENT_NO", DbType = DbType.Int64, Value = DOCUMENT_NO };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@MANUAL_DOCUMENT_NO", DbType = DbType.Decimal, Value = MANUAL_DOCUMENT_NO };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@LEDGER_ID", DbType = DbType.Int64, Direction = ParameterDirection.Output };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@REMARKS", DbType = DbType.String, Value = REMARKS };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CHEQUE_NO", DbType = DbType.String, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SLIP_NO", DbType = DbType.String, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PAYEE_NAME", DbType = DbType.String, Value = PAYEE_NAME };
                        pparams.Add(parameter);

                        cmd.ExecuteNonQuery();

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }

        }

        public bool InsertGLMaster(int SelectedLocation, int PRINCIPAL_ID, int USER_ID, long INVOICE_ID, DateTime VOUCHER_DATE, string VOUCHER_NO, string REMARKS)
        {
            bool flag = true;
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "spInsertGL_MASTER";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_TYPE_ID", DbType = DbType.Int32, Value = 16 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PRINCIPAL_ID", DbType = DbType.Int32, Value = PRINCIPAL_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PAYMENT_MODE", DbType = DbType.Int32, Value = 2 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@USER_ID", DbType = DbType.Int32, Value = USER_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@INVOICE_TYPE", DbType = DbType.Int32, Value = 2 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@INVOICE_ID", DbType = DbType.Int64, Value = INVOICE_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_DATE", DbType = DbType.DateTime, Value = VOUCHER_DATE };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CHEQUE_DATE", DbType = DbType.DateTime, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TIME_STAMP", DbType = DbType.DateTime, Value = DateTime.Now };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DUE_DATE", DbType = DbType.DateTime, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@IS_DELETED", DbType = DbType.Boolean, Value = false };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@IS_POSTED", DbType = DbType.Boolean, Value = false };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_NO", DbType = DbType.String, Value = VOUCHER_NO };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PAYEES_NAME", DbType = DbType.String, Value = INVOICE_ID.ToString() };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CHEQUE_NO", DbType = DbType.String, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@REMARKS", DbType = DbType.String, Value = REMARKS };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@Slip_No", DbType = DbType.String, Value = "Purchase" };
                        pparams.Add(parameter);

                        cmd.ExecuteNonQuery();

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

        public bool InsertGLMasterSale(int SelectedLocation, int PRINCIPAL_ID, int USER_ID, long INVOICE_ID, DateTime VOUCHER_DATE, string VOUCHER_NO, string REMARKS)
        {
            bool flag = true;
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "spInsertGL_MASTER";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_TYPE_ID", DbType = DbType.Int32, Value = 16 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PRINCIPAL_ID", DbType = DbType.Int32, Value = PRINCIPAL_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PAYMENT_MODE", DbType = DbType.Int32, Value = 13 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@USER_ID", DbType = DbType.Int32, Value = USER_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@INVOICE_TYPE", DbType = DbType.Int32, Value = 13 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@INVOICE_ID", DbType = DbType.Int64, Value = INVOICE_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_DATE", DbType = DbType.DateTime, Value = VOUCHER_DATE };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CHEQUE_DATE", DbType = DbType.DateTime, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TIME_STAMP", DbType = DbType.DateTime, Value = DateTime.Now };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DUE_DATE", DbType = DbType.DateTime, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@IS_DELETED", DbType = DbType.Boolean, Value = false };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@IS_POSTED", DbType = DbType.Boolean, Value = false };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_NO", DbType = DbType.String, Value = VOUCHER_NO };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PAYEES_NAME", DbType = DbType.String, Value = INVOICE_ID.ToString() };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CHEQUE_NO", DbType = DbType.String, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@REMARKS", DbType = DbType.String, Value = REMARKS };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@Slip_No", DbType = DbType.String, Value = "Sale" };
                        pparams.Add(parameter);

                        cmd.ExecuteNonQuery();

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

        public bool InsertGLDetail(int SelectedLocation, int PRINCIPAL_ID, decimal DEBIT, decimal CREDIT, long ACCOUNT_HEAD_ID, string VOUCHER_NO, string REMARKS)
        {
            bool flag = true;
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "spInsertGL_DETAIL";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PRINCIPAL_ID", DbType = DbType.Int32, Value = PRINCIPAL_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_TYPE_ID", DbType = DbType.Int32, Value = 16 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DEBIT", DbType = DbType.Decimal, Value = DEBIT };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CREDIT", DbType = DbType.Decimal, Value = CREDIT };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@IS_DELETED", DbType = DbType.Boolean, Value = false };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@ACCOUNT_HEAD_ID", DbType = DbType.Int64, Value = ACCOUNT_HEAD_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@VOUCHER_NO", DbType = DbType.String, Value = VOUCHER_NO };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@GL_REMARKS", DbType = DbType.String, Value = REMARKS };
                        pparams.Add(parameter);

                        cmd.ExecuteNonQuery();

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

        public bool UpdatePurchaseMaster(int SelectedLocation, long PURCHASE_MASTER_ID, int PAYMENT_MODE)
        {
            bool flag = true;
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspUpdatePurchaseMasterUtility";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PURCHASE_MASTER_ID", DbType = DbType.Int64, Value = PURCHASE_MASTER_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@PAYMENT_MODE", DbType = DbType.Int32, Value = PAYMENT_MODE };
                        pparams.Add(parameter);

                        cmd.ExecuteNonQuery();

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

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {

        }

        public DataTable LoadItems(int SelectedLocation, int TypeID, string BrandIDs)
        {
            DataTable dtItem = new DataTable();
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {

                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetItemsUtility";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TypeID", DbType = DbType.Int32, Value = TypeID };
                        pparams.Add(parameter);

                        if (BrandIDs == null)
                        {
                            parameter = new SqlParameter() { ParameterName = "@BrandIDs", DbType = DbType.String, Value = DBNull.Value };
                            pparams.Add(parameter);
                        }
                        else
                        {
                            parameter = new SqlParameter() { ParameterName = "@BrandIDs", DbType = DbType.String, Value = BrandIDs };
                            pparams.Add(parameter);
                        }

                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        dtItem = ds.Tables[0];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return dtItem;
        }

        private void btnRemoveMenu_Click(object sender, EventArgs e)
        {
            var confirmResult = ZKMessgeBox.Show("Did you took backup of " + txtDBName.Text.Trim() + " Database??", "Confirm Database Backup!!", ZKMessgeBox.I8Buttons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                List<string> list = new List<string>();

                if (clbMenuItem.CheckedItems.Count > 0)
                {
                    for (int x = 0; x < clbMenuItem.CheckedItems.Count; x++)
                    {
                        list.Add(clbMenuItem.CheckedItems[x].ToString());
                    }
                    if (list.Count > 0)
                    {
                        RemoveConstraints();
                        //Truncate Tables
                        StringBuilder truncateTables = new StringBuilder();
                        foreach (string tbl in list)
                        {
                            if (tbl == "Sections")
                            {
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE PRODUCT_SECTION");
                            }
                            else if (tbl == "Categories")
                            {
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE SKU_HIERARCHY");
                            }
                            else if (tbl == "Items")
                            {
                                //SKUS
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE SKUS");
                                //SKU_PRICES
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE SKU_PRICES");

                                //ITEM_DEAL_ASSIGNMENT
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE ITEM_DEAL_ASSIGNMENT");
                                //Item_Deal_Assignment_Detail
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE Item_Deal_Assignment_Detail");
                                //FINISHED_GOOD_MASTER
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE FINISHED_GOOD_MASTER");
                                //FINISHED_GOOD_DETAIL
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE FINISHED_GOOD_DETAIL");
                                //tblSKUModifier
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE tblSKUModifier");
                                //tblPackageMaterial
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE tblPackageMaterial");
                                //tblPackageMaterialDetail
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE tblPackageMaterialDetail");
                                //SKU_PRICES_RAW
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE SKU_PRICES_RAW");
                                //SKUS_CHILD
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE SKUS_CHILD");
                                //SKU_PRICES_LEVEL
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE SKU_PRICES_LEVEL");
                                //SKU_PRICES_LEVEL_DETAIL
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE SKU_PRICES_LEVEL_DETAIL");
                                //SKU_GROUP
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE SKU_GROUP");
                                //SKU_GROUP_DETAIL
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE SKU_GROUP_DETAIL");
                                //LOCATION_ITEM_ASSIGNMENT
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE LOCATION_ITEM_ASSIGNMENT");
                                //LOCATION_ITEM_ASSIGNMENT
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE LOCATION_ITEM_ASSIGNMENT");
                                //tblRecipeLocation
                                truncateTables.Append(Environment.NewLine);
                                truncateTables.Append("TRUNCATE TABLE tblRecipeLocation");
                            }
                        }
                        ExecuteScript(truncateTables.ToString());
                        AddConstraints();
                        txtDBName.Text = string.Empty;
                        ZKMessgeBox.Show("Menu removed successfully.");
                    }
                }
                else
                {
                    ZKMessgeBox.Show("Select any Item(s)");
                }
            }
        }
        protected void RemoveConstraints()
        {
            StringBuilder FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER = new StringBuilder();
            FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER.Append(Environment.NewLine);
            FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER.Append("IF (OBJECT_ID('FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER') IS NOT NULL) BEGIN ALTER TABLE[dbo].[SALE_INVOICE_CONSUMED] DROP CONSTRAINT[FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER] END ");
            ExecuteScript(FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER.ToString());

            StringBuilder FK_SALE_INVOICE_DETAIL_SALE_INVOICE_MASTER = new StringBuilder();
            FK_SALE_INVOICE_DETAIL_SALE_INVOICE_MASTER.Append(Environment.NewLine);
            FK_SALE_INVOICE_DETAIL_SALE_INVOICE_MASTER.Append("IF (OBJECT_ID('FK_SALE_INVOICE_DETAIL_SALE_INVOICE_MASTER') IS NOT NULL) BEGIN ALTER TABLE[dbo].[SALE_INVOICE_DETAIL] DROP CONSTRAINT[FK_SALE_INVOICE_DETAIL_SALE_INVOICE_MASTER] END");
            ExecuteScript(FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER.ToString());

            StringBuilder FK_tblModuleColInfo_tblDataType = new StringBuilder();
            FK_tblModuleColInfo_tblDataType.Append(Environment.NewLine);
            FK_tblModuleColInfo_tblDataType.Append("IF (OBJECT_ID('FK_tblModuleColInfo_tblDataType') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblModuleColInfo] DROP CONSTRAINT[FK_tblModuleColInfo_tblDataType] END");
            ExecuteScript(FK_tblModuleColInfo_tblDataType.ToString());

            StringBuilder FK_tblModuleColInfo_tblModuleParamInfo = new StringBuilder();
            FK_tblModuleColInfo_tblModuleParamInfo.Append(Environment.NewLine);
            FK_tblModuleColInfo_tblModuleParamInfo.Append("IF (OBJECT_ID('FK_tblModuleColInfo_tblModuleParamInfo') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblModuleColInfo] DROP CONSTRAINT[FK_tblModuleColInfo_tblModuleParamInfo] END");
            ExecuteScript(FK_tblModuleColInfo_tblModuleParamInfo.ToString());

            StringBuilder FK_tblModuleParamInfo_tblFinancialYear = new StringBuilder();
            FK_tblModuleParamInfo_tblFinancialYear.Append(Environment.NewLine);
            FK_tblModuleParamInfo_tblFinancialYear.Append("IF (OBJECT_ID('FK_tblModuleParamInfo_tblFinancialYear') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblModuleParamInfo] DROP CONSTRAINT[FK_tblModuleParamInfo_tblFinancialYear] END");
            ExecuteScript(FK_tblModuleParamInfo_tblFinancialYear.ToString());

            StringBuilder FK_SALE_INVOICE_CONSUMED_SKUS = new StringBuilder();
            FK_SALE_INVOICE_CONSUMED_SKUS.Append(Environment.NewLine);
            FK_SALE_INVOICE_CONSUMED_SKUS.Append("IF (OBJECT_ID('FK_SALE_INVOICE_CONSUMED_SKUS') IS NOT NULL) BEGIN ALTER TABLE[dbo].[SALE_INVOICE_CONSUMED] DROP CONSTRAINT[FK_SALE_INVOICE_CONSUMED_SKUS] END");
            ExecuteScript(FK_SALE_INVOICE_CONSUMED_SKUS.ToString());

            StringBuilder FK_SALE_INVOICE_DETAIL_SKUS = new StringBuilder();
            FK_SALE_INVOICE_DETAIL_SKUS.Append(Environment.NewLine);
            FK_SALE_INVOICE_DETAIL_SKUS.Append("IF (OBJECT_ID('FK_SALE_INVOICE_DETAIL_SKUS') IS NOT NULL) BEGIN ALTER TABLE[dbo].[SALE_INVOICE_DETAIL] DROP CONSTRAINT[FK_SALE_INVOICE_DETAIL_SKUS] END");
            ExecuteScript(FK_SALE_INVOICE_DETAIL_SKUS.ToString());

            StringBuilder FK_tblPackageMaterial_SKUS = new StringBuilder();
            FK_tblPackageMaterial_SKUS.Append(Environment.NewLine);
            FK_tblPackageMaterial_SKUS.Append("IF (OBJECT_ID('FK_tblPackageMaterial_SKUS') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblPackageMaterial] DROP CONSTRAINT[FK_tblPackageMaterial_SKUS] END");
            ExecuteScript(FK_tblPackageMaterial_SKUS.ToString());

            StringBuilder FK_tblPackageMaterialDetail_SKUS = new StringBuilder();
            FK_tblPackageMaterialDetail_SKUS.Append(Environment.NewLine);
            FK_tblPackageMaterialDetail_SKUS.Append("IF (OBJECT_ID('FK_tblPackageMaterialDetail_SKUS') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblPackageMaterialDetail] DROP CONSTRAINT[FK_tblPackageMaterialDetail_SKUS] END");
            ExecuteScript(FK_tblPackageMaterialDetail_SKUS.ToString());

            StringBuilder FK_tblSKUModifier_SKUS = new StringBuilder();
            FK_tblSKUModifier_SKUS.Append(Environment.NewLine);
            FK_tblSKUModifier_SKUS.Append("IF (OBJECT_ID('FK_tblSKUModifier_SKUS') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblSKUModifier] DROP CONSTRAINT[FK_tblSKUModifier_SKUS] END");
            ExecuteScript(FK_tblSKUModifier_SKUS.ToString());

            StringBuilder FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL = new StringBuilder();
            FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL.Append(Environment.NewLine);
            FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL.Append("IF (OBJECT_ID('FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblRecipeProductionDetail] DROP CONSTRAINT[FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL] END");
            ExecuteScript(FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL.ToString());

            StringBuilder FK_tblRecipeProductionDetail_tblRecipeProductionMaster = new StringBuilder();
            FK_tblRecipeProductionDetail_tblRecipeProductionMaster.Append(Environment.NewLine);
            FK_tblRecipeProductionDetail_tblRecipeProductionMaster.Append("IF (OBJECT_ID('FK_tblRecipeProductionDetail_tblRecipeProductionMaster') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblRecipeProductionDetail] DROP CONSTRAINT[FK_tblRecipeProductionDetail_tblRecipeProductionMaster] END");
            ExecuteScript(FK_tblRecipeProductionDetail_tblRecipeProductionMaster.ToString());

            StringBuilder FK_tblAppSetting_DISTRIBUTOR = new StringBuilder();
            FK_tblAppSetting_DISTRIBUTOR.Append(Environment.NewLine);
            FK_tblAppSetting_DISTRIBUTOR.Append("IF (OBJECT_ID('FK_tblAppSetting_DISTRIBUTOR') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblAppSetting] DROP CONSTRAINT[FK_tblAppSetting_DISTRIBUTOR] END");
            ExecuteScript(FK_tblAppSetting_DISTRIBUTOR.ToString());

            StringBuilder FK_tblAppSetting_tblDataType = new StringBuilder();
            FK_tblAppSetting_tblDataType.Append(Environment.NewLine);
            FK_tblAppSetting_tblDataType.Append("IF (OBJECT_ID('FK_tblAppSetting_tblDataType') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblAppSetting] DROP CONSTRAINT[FK_tblAppSetting_tblDataType] END");
            ExecuteScript(FK_tblAppSetting_tblDataType.ToString());

            StringBuilder FK_tblModuleColInfo_MODULE = new StringBuilder();
            FK_tblModuleColInfo_MODULE.Append(Environment.NewLine);
            FK_tblModuleColInfo_MODULE.Append("IF (OBJECT_ID('FK_tblModuleColInfo_MODULE') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblModuleColInfo] DROP CONSTRAINT[FK_tblModuleColInfo_MODULE] END");
            ExecuteScript(FK_tblModuleColInfo_MODULE.ToString());

            StringBuilder FK_tblModuleParamInfo_MODULE = new StringBuilder();
            FK_tblModuleParamInfo_MODULE.Append(Environment.NewLine);
            FK_tblModuleParamInfo_MODULE.Append("IF (OBJECT_ID('FK_tblModuleParamInfo_MODULE') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblModuleParamInfo] DROP CONSTRAINT[FK_tblModuleParamInfo_MODULE] END");
            ExecuteScript(FK_tblModuleParamInfo_MODULE.ToString());

            StringBuilder FK_tblReportDashboardInfo_MODULE = new StringBuilder();
            FK_tblReportDashboardInfo_MODULE.Append(Environment.NewLine);
            FK_tblReportDashboardInfo_MODULE.Append("IF (OBJECT_ID('FK_tblReportDashboardInfo_MODULE') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblReportDashboardInfo] DROP CONSTRAINT[FK_tblReportDashboardInfo_MODULE] END");
            ExecuteScript(FK_tblReportDashboardInfo_MODULE.ToString());

            StringBuilder FK_tblAppSettingDetail_tblAppSetting = new StringBuilder();
            FK_tblAppSettingDetail_tblAppSetting.Append(Environment.NewLine);
            FK_tblAppSettingDetail_tblAppSetting.Append("IF (OBJECT_ID('FK_tblAppSettingDetail_tblAppSetting') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblAppSettingDetail] DROP CONSTRAINT[FK_tblAppSettingDetail_tblAppSetting] END");
            ExecuteScript(FK_tblAppSettingDetail_tblAppSetting.ToString());

            StringBuilder FK_tblAppSettingDetail_tblDataType = new StringBuilder();
            FK_tblAppSettingDetail_tblDataType.Append(Environment.NewLine);
            FK_tblAppSettingDetail_tblDataType.Append("IF (OBJECT_ID('FK_tblAppSettingDetail_tblDataType') IS NOT NULL) BEGIN ALTER TABLE[dbo].[tblAppSettingDetail] DROP CONSTRAINT[FK_tblAppSettingDetail_tblDataType] END");
            ExecuteScript(FK_tblAppSettingDetail_tblDataType.ToString());
        }
        protected void AddConstraints()
        {
            StringBuilder FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER = new StringBuilder();
            FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER.Append(Environment.NewLine);
            FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER.Append("ALTER TABLE [dbo].[SALE_INVOICE_CONSUMED] ADD CONSTRAINT [FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER] FOREIGN KEY ([SALE_INVOICE_ID]) REFERENCES [dbo].[SALE_INVOICE_MASTER]([SALE_INVOICE_ID])");
            ExecuteScript(FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER.ToString());

            StringBuilder FK_SALE_INVOICE_DETAIL_SALE_INVOICE_MASTER = new StringBuilder();
            FK_SALE_INVOICE_DETAIL_SALE_INVOICE_MASTER.Append(Environment.NewLine);
            FK_SALE_INVOICE_DETAIL_SALE_INVOICE_MASTER.Append("ALTER TABLE [dbo].[SALE_INVOICE_DETAIL] ADD CONSTRAINT [FK_SALE_INVOICE_DETAIL_SALE_INVOICE_MASTER] FOREIGN KEY ([SALE_INVOICE_ID]) REFERENCES [dbo].[SALE_INVOICE_MASTER]([SALE_INVOICE_ID])");
            ExecuteScript(FK_SALE_INVOICE_CONSUMED_SALE_INVOICE_MASTER.ToString());

            StringBuilder FK_tblModuleColInfo_tblDataType = new StringBuilder();
            FK_tblModuleColInfo_tblDataType.Append(Environment.NewLine);
            FK_tblModuleColInfo_tblDataType.Append("ALTER TABLE [dbo].[tblModuleColInfo] ADD CONSTRAINT [FK_tblModuleColInfo_tblDataType] FOREIGN KEY ([sintDataTypeCode]) REFERENCES [dbo].[tblDataType]([sintCode])");
            ExecuteScript(FK_tblModuleColInfo_tblDataType.ToString());

            StringBuilder FK_tblModuleColInfo_tblModuleParamInfo = new StringBuilder();
            FK_tblModuleColInfo_tblModuleParamInfo.Append(Environment.NewLine);
            FK_tblModuleColInfo_tblModuleParamInfo.Append("ALTER TABLE [dbo].[tblModuleColInfo] ADD CONSTRAINT [FK_tblModuleColInfo_tblModuleParamInfo] FOREIGN KEY ([intParamInfoCode]) REFERENCES [dbo].[tblModuleParamInfo]([intCode])");
            ExecuteScript(FK_tblModuleColInfo_tblModuleParamInfo.ToString());

            StringBuilder FK_tblModuleParamInfo_tblFinancialYear = new StringBuilder();
            FK_tblModuleParamInfo_tblFinancialYear.Append(Environment.NewLine);
            FK_tblModuleParamInfo_tblFinancialYear.Append("ALTER TABLE [dbo].[tblModuleParamInfo] ADD CONSTRAINT [FK_tblModuleParamInfo_tblFinancialYear] FOREIGN KEY ([sintFinancialYearCode]) REFERENCES [dbo].[tblFinancialYear]([sintCode])");
            ExecuteScript(FK_tblModuleParamInfo_tblFinancialYear.ToString());

            StringBuilder FK_SALE_INVOICE_CONSUMED_SKUS = new StringBuilder();
            FK_SALE_INVOICE_CONSUMED_SKUS.Append(Environment.NewLine);
            FK_SALE_INVOICE_CONSUMED_SKUS.Append("ALTER TABLE [dbo].[SALE_INVOICE_CONSUMED] ADD CONSTRAINT [FK_SALE_INVOICE_CONSUMED_SKUS] FOREIGN KEY ([SKU_ID]) REFERENCES [dbo].[SKUS]([SKU_ID])");
            ExecuteScript(FK_SALE_INVOICE_CONSUMED_SKUS.ToString());

            StringBuilder FK_SALE_INVOICE_DETAIL_SKUS = new StringBuilder();
            FK_SALE_INVOICE_DETAIL_SKUS.Append(Environment.NewLine);
            FK_SALE_INVOICE_DETAIL_SKUS.Append("ALTER TABLE [dbo].[SALE_INVOICE_DETAIL] ADD CONSTRAINT [FK_SALE_INVOICE_DETAIL_SKUS] FOREIGN KEY ([SKU_ID]) REFERENCES [dbo].[SKUS]([SKU_ID])");
            ExecuteScript(FK_SALE_INVOICE_DETAIL_SKUS.ToString());

            StringBuilder FK_tblPackageMaterial_SKUS = new StringBuilder();
            FK_tblPackageMaterial_SKUS.Append(Environment.NewLine);
            FK_tblPackageMaterial_SKUS.Append("ALTER TABLE [dbo].[tblPackageMaterial] ADD CONSTRAINT [FK_tblPackageMaterial_SKUS] FOREIGN KEY ([SKU_ID]) REFERENCES [dbo].[SKUS]([SKU_ID])");
            ExecuteScript(FK_tblPackageMaterial_SKUS.ToString());

            StringBuilder FK_tblPackageMaterialDetail_SKUS = new StringBuilder();
            FK_tblPackageMaterialDetail_SKUS.Append(Environment.NewLine);
            FK_tblPackageMaterialDetail_SKUS.Append("ALTER TABLE [dbo].[tblPackageMaterialDetail] ADD CONSTRAINT [FK_tblPackageMaterialDetail_SKUS] FOREIGN KEY ([SKU_ID]) REFERENCES [dbo].[SKUS]([SKU_ID])");
            ExecuteScript(FK_tblPackageMaterialDetail_SKUS.ToString());

            StringBuilder FK_tblSKUModifier_SKUS = new StringBuilder();
            FK_tblSKUModifier_SKUS.Append(Environment.NewLine);
            FK_tblSKUModifier_SKUS.Append("ALTER TABLE [dbo].[tblSKUModifier] ADD CONSTRAINT [FK_tblSKUModifier_SKUS] FOREIGN KEY ([SKU_ID]) REFERENCES [dbo].[SKUS]([SKU_ID])");
            ExecuteScript(FK_tblSKUModifier_SKUS.ToString());

            StringBuilder FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL = new StringBuilder();
            FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL.Append(Environment.NewLine);
            FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL.Append("ALTER TABLE [dbo].[tblRecipeProductionDetail] ADD CONSTRAINT [FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL] FOREIGN KEY ([FINISHED_GOOD_DETAIL_ID]) REFERENCES [dbo].[FINISHED_GOOD_DETAIL]([FINISHED_GOOD_DETAIL_ID])");
            ExecuteScript(FK_tblRecipeProductionDetail_FINISHED_GOOD_DETAIL.ToString());

            StringBuilder FK_tblRecipeProductionDetail_tblRecipeProductionMaster = new StringBuilder();
            FK_tblRecipeProductionDetail_tblRecipeProductionMaster.Append(Environment.NewLine);
            FK_tblRecipeProductionDetail_tblRecipeProductionMaster.Append("ALTER TABLE [dbo].[tblRecipeProductionDetail] ADD CONSTRAINT [FK_tblRecipeProductionDetail_tblRecipeProductionMaster] FOREIGN KEY ([lngRecipeProductionCode]) REFERENCES [dbo].[tblRecipeProductionMaster]([lngRecipeProductionCode])");
            ExecuteScript(FK_tblRecipeProductionDetail_tblRecipeProductionMaster.ToString());

            StringBuilder FK_tblAppSetting_DISTRIBUTOR = new StringBuilder();
            FK_tblAppSetting_DISTRIBUTOR.Append(Environment.NewLine);
            FK_tblAppSetting_DISTRIBUTOR.Append("ALTER TABLE [dbo].[tblAppSetting] ADD CONSTRAINT [FK_tblAppSetting_DISTRIBUTOR] FOREIGN KEY ([intCompanyId],[intDistributorId]) REFERENCES [dbo].[DISTRIBUTOR]([COMPANY_ID],[DISTRIBUTOR_ID])");
            ExecuteScript(FK_tblAppSetting_DISTRIBUTOR.ToString());

            StringBuilder FK_tblAppSetting_tblDataType = new StringBuilder();
            FK_tblAppSetting_tblDataType.Append(Environment.NewLine);
            FK_tblAppSetting_tblDataType.Append("ALTER TABLE [dbo].[tblAppSetting] ADD CONSTRAINT [FK_tblAppSetting_tblDataType] FOREIGN KEY ([sintDataTypeCode]) REFERENCES [dbo].[tblDataType]([sintCode])");
            ExecuteScript(FK_tblAppSetting_tblDataType.ToString());

            StringBuilder FK_tblModuleColInfo_MODULE = new StringBuilder();
            FK_tblModuleColInfo_MODULE.Append(Environment.NewLine);
            FK_tblModuleColInfo_MODULE.Append("ALTER TABLE [dbo].[tblModuleColInfo] ADD CONSTRAINT [FK_tblModuleColInfo_MODULE] FOREIGN KEY ([MODULE_ID]) REFERENCES [dbo].[MODULE]([MODULE_ID])");
            ExecuteScript(FK_tblModuleColInfo_MODULE.ToString());

            StringBuilder FK_tblModuleParamInfo_MODULE = new StringBuilder();
            FK_tblModuleParamInfo_MODULE.Append(Environment.NewLine);
            FK_tblModuleParamInfo_MODULE.Append("ALTER TABLE [dbo].[tblModuleParamInfo] ADD CONSTRAINT [FK_tblModuleParamInfo_MODULE] FOREIGN KEY ([MODULE_ID]) REFERENCES [dbo].[MODULE]([MODULE_ID])");
            ExecuteScript(FK_tblModuleParamInfo_MODULE.ToString());

            StringBuilder FK_tblReportDashboardInfo_MODULE = new StringBuilder();
            FK_tblReportDashboardInfo_MODULE.Append(Environment.NewLine);
            FK_tblReportDashboardInfo_MODULE.Append("ALTER TABLE [dbo].[tblReportDashboardInfo] ADD CONSTRAINT [FK_tblReportDashboardInfo_MODULE] FOREIGN KEY ([MODULE_ID]) REFERENCES [dbo].[MODULE]([MODULE_ID])");
            ExecuteScript(FK_tblReportDashboardInfo_MODULE.ToString());

            StringBuilder FK_tblAppSettingDetail_tblDataType = new StringBuilder();
            FK_tblAppSettingDetail_tblDataType.Append(Environment.NewLine);
            FK_tblAppSettingDetail_tblDataType.Append("ALTER TABLE [dbo].[tblAppSettingDetail] ADD CONSTRAINT [FK_tblAppSettingDetail_tblDataType] FOREIGN KEY ([sintDataTypeCode]) REFERENCES [dbo].[tblDataType]([sintCode])");
            ExecuteScript(FK_tblAppSettingDetail_tblDataType.ToString());
        }

        private void ExecuteScript(string script)
        {
            try
            {
                string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USE " + txtDBName.Text + script, con))
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
            }
        }

        private void ExecuteScript(string script, string DatabaseName)
        {
            try
            {
                string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("USE " + DatabaseName + script, con))
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
            }
        }

        private void btnChangeSupplier_Click(object sender, EventArgs e)
        {
            var confirmResult = ZKMessgeBox.Show("Are you sure to Change Supplier Name??", "Confirm Supplir Name!!", ZKMessgeBox.I8Buttons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                StringBuilder PurchaseTable = new StringBuilder();
                PurchaseTable.Append(Environment.NewLine);
                PurchaseTable.Append("UPDATE PURCHASE_MASTER SET PRINCIPAL_ID = " + Convert.ToInt32(cbNewSupplier.SelectedValue) + " , SOLD_FROM = " + Convert.ToInt32(cbNewSupplier.SelectedValue) + " WHERE PURCHASE_MASTER_ID=" + Convert.ToInt64(cbPurDocument2.SelectedValue));
                ExecuteScript(PurchaseTable.ToString(), cbDatabase.SelectedItem.ToString());

                StringBuilder VendorLedgerTable = new StringBuilder();
                VendorLedgerTable.Append(Environment.NewLine);
                VendorLedgerTable.Append("UPDATE VENDOR_LEDGER SET PRINCIPAL_ID = " + Convert.ToInt32(cbNewSupplier.SelectedValue) + " WHERE DOCUMENT_NO=" + Convert.ToInt64(cbPurDocument2.SelectedValue));
                ExecuteScript(VendorLedgerTable.ToString(), cbDatabase.SelectedItem.ToString());

                //Get GL Master Data
                DataTable dtGLMaster = new DataTable();
                dtGLMaster.Columns.Add("VOUCHER_NO", typeof(string));
                dtGLMaster.Columns.Add("VOUCHER_TYPE_ID", typeof(int));
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT VOUCHER_NO,VOUCHER_TYPE_ID FROM GL_MASTER WHERE INVOICE_TYPE = 2 AND INVOICE_ID = " + cbPurDocument2.SelectedValue + " AND DISTRIBUTOR_ID=" + cblLocation.SelectedValue, cbDatabase.SelectedItem), con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                DataRow drNew = dtGLMaster.NewRow();
                                drNew["VOUCHER_NO"] = dr[0];
                                drNew["VOUCHER_TYPE_ID"] = dr[1];
                                dtGLMaster.Rows.Add(drNew);
                            }
                        }
                    }
                    con.Close();
                }
                if (dtGLMaster.Rows.Count > 0)
                {
                    StringBuilder GLMasterTable = new StringBuilder();
                    GLMasterTable.Append(Environment.NewLine);
                    GLMasterTable.Append("UPDATE GL_MASTER SET PRINCIPAL_ID = " + Convert.ToInt32(cbNewSupplier.SelectedValue) + " WHERE INVOICE_ID=" + Convert.ToInt64(cbPurDocument2.SelectedValue) + " AND INVOICE_TYPE=2 AND DISTRIBUTOR_ID=" + cblLocation.SelectedValue);
                    ExecuteScript(GLMasterTable.ToString(), cbDatabase.SelectedItem.ToString());

                    StringBuilder GLDetailTable = new StringBuilder();
                    GLDetailTable.Append(Environment.NewLine);
                    GLDetailTable.Append("UPDATE GL_DETAIL SET PRINCIPAL_ID = " + Convert.ToInt32(cbNewSupplier.SelectedValue) + " WHERE VOUCHER_NO='" + dtGLMaster.Rows[0]["VOUCHER_NO"].ToString() + "' AND VOUCHER_TYPE_ID=" + Convert.ToInt32(dtGLMaster.Rows[0]["VOUCHER_TYPE_ID"]) + " AND DISTRIBUTOR_ID =" + cblLocation.SelectedValue);
                    ExecuteScript(GLDetailTable.ToString(), cbDatabase.SelectedItem.ToString());
                }
                ZKMessgeBox.Show("Supplier Name Change successfully.");
                cblLocation_SelectedIndexChanged(null, null);
            }
        }

        private void cbPurDocument2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPurDocument2.Items.Count > 0)
            {
                long PurchaseID = 0;
                try
                {
                    PurchaseID = ((System.Collections.Generic.KeyValuePair<long, string>)cbPurDocument2.SelectedValue).Key;
                }
                catch (Exception)
                {
                    PurchaseID = Convert.ToInt64(cbPurDocument2.SelectedValue);
                }
                foreach (DataRow dr in dtPurchase2.Rows)
                {
                    if (PurchaseID.ToString() == dr["PURCHASE_MASTER_ID"].ToString())
                    {
                        lblAmount.Text = "Amount :" + dr["TOTAL_AMOUNT"].ToString();
                        break;
                    }
                }
            }
        }

        private void btnDeletePurchase_Click(object sender, EventArgs e)
        {
            int SelectedLocation = 0;
            try
            {
                SelectedLocation = ((KeyValuePair<int, string>)cblLocation.SelectedValue).Key;
            }
            catch (Exception)
            {
                SelectedLocation = Convert.ToInt32(cblLocation.SelectedValue);
            }
            if (SelectedLocation > 0)
            {
                if (txtDocumentNo.Text.Length > 0)
                {
                    var confirmResult = ZKMessgeBox.Show("Are you sure to Delete this Purchase Document??", "Confirm Purchase Delete!!", ZKMessgeBox.I8Buttons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        DataTable dtPurchaseDetail = GetData(SelectedLocation, Convert.ToInt64(txtDocumentNo.Text), 4);
                        StringBuilder sbPurchaseDetail = new StringBuilder();
                        foreach (DataRow dr in dtPurchaseDetail.Rows)
                        {
                            sbPurchaseDetail = new StringBuilder();
                            sbPurchaseDetail.Append(Environment.NewLine);
                            sbPurchaseDetail.Append("DELETE FROM PURCHASE_DETAIL WHERE PURCHASE_DETAIL_ID = " + Convert.ToInt64(dr["PURCHASE_DETAIL_ID"]));
                            ExecuteScript(sbPurchaseDetail.ToString(), cbDatabase.SelectedItem.ToString());
                        }

                        StringBuilder sbPurchaseMaster = new StringBuilder();
                        sbPurchaseMaster.Append(Environment.NewLine);
                        sbPurchaseMaster.Append("DELETE FROM PURCHASE_MASTER WHERE PURCHASE_MASTER_ID = " + Convert.ToInt64(txtDocumentNo.Text));
                        ExecuteScript(sbPurchaseMaster.ToString(), cbDatabase.SelectedItem.ToString());

                        StringBuilder sbVendorLedger = new StringBuilder();
                        sbVendorLedger.Append(Environment.NewLine);
                        sbVendorLedger.Append("UPDATE VENDOR_LEDGER SET IS_DELETED = 1 WHERE DOCUMENT_NO = " + Convert.ToInt64(txtDocumentNo.Text));
                        ExecuteScript(sbVendorLedger.ToString(), cbDatabase.SelectedItem.ToString());

                        StringBuilder sbGLMaster = new StringBuilder();
                        sbGLMaster.Append(Environment.NewLine);
                        sbGLMaster.Append("UPDATE GL_MASTER SET IS_DELETED = 1 WHERE INVOICE_TYPE = 2 AND INVOICE_ID = " + Convert.ToInt64(txtDocumentNo.Text));
                        ExecuteScript(sbGLMaster.ToString(), cbDatabase.SelectedItem.ToString());

                        cblLocation_SelectedIndexChanged(null, null);
                        txtDocumentNo.Text = string.Empty;
                        lblAmountDelete.Text = "Amount :0";
                        lblSupplierDelete.Text = "Supplier: ";
                        ZKMessgeBox.Show("Purchase Deleted Successfully");
                    }
                }
                else
                {
                    ZKMessgeBox.Show("Enter Document No.");
                    txtDocumentNo.Focus();
                }
            }
            else
            {
                ZKMessgeBox.Show("Select Database.");
                cbDatabase.Focus();
            }
        }

        private void txtDocumentNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtDocumentNo.Text.Length > 0)
                {
                    foreach (DataRow dr in dtPurchase2.Rows)
                    {
                        if (txtDocumentNo.Text == dr["PURCHASE_MASTER_ID"].ToString())
                        {
                            lblAmountDelete.Text = "Amount :" + dr["TOTAL_AMOUNT"].ToString();
                            lblSupplierDelete.Text = "Supplier: " + dr["SupplierName2"].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    ZKMessgeBox.Show("Enter Document No");
                }
            }
        }

        private void txtDocumentNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnUpdateGL_Click(object sender, EventArgs e)
        {
            int RowsEffected = 0;
            int SelectedLocation = 0;
            try
            {
                SelectedLocation = ((KeyValuePair<int, string>)cblLocation.SelectedValue).Key;
            }
            catch (Exception)
            {
                SelectedLocation = Convert.ToInt32(cblLocation.SelectedValue);
            }
            if (SelectedLocation > 0)
            {
                if (txtInvoiceNo.Text.Length > 0)
                {
                    var confirmResult = ZKMessgeBox.Show("Are you sure to Update GL??", "Confirm Update GL!!", ZKMessgeBox.I8Buttons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        string[] InvoiceNos = txtInvoiceNo.Text.Split(',');
                        foreach (string InvoiceNo in InvoiceNos)
                        {
                            DataTable dtInvocie = GetData(SelectedLocation, Convert.ToInt64(InvoiceNo), 5);
                            if (dtInvocie.Rows.Count > 0)
                            {
                                RowsEffected++;
                                DataRow dr = dtInvocie.Rows[0];
                                decimal pAmountDue = 0;
                                decimal DiscountAmount = 0;
                                decimal GST = 0;
                                decimal pServiceCharges = 0;
                                decimal pPaidin = 0;
                                decimal GLConsumption = 0;
                                int pPaymentModeId = 0;
                                pAmountDue = Convert.ToDecimal(dr["AMOUNTDUE"]);
                                GST = Convert.ToDecimal(dr["GST"]);
                                pServiceCharges = Convert.ToDecimal(dr["SERVICE_CHARGES"]);
                                pPaidin = Convert.ToDecimal(dr["PAIDIN"]);
                                GLConsumption = Convert.ToDecimal(dr["GLConsumption"]);
                                pPaymentModeId = Convert.ToInt32(dr["PAYMENT_MODE_ID"]);
                                if (dr["DISCOUNT_TYPE"].ToString() == "0")
                                {
                                    DiscountAmount = pAmountDue * Convert.ToDecimal(dr["DISCOUNT"]) / 100;
                                }
                                else
                                {
                                    DiscountAmount = Convert.ToDecimal(dr["DISCOUNT"]);
                                }
                                DiscountAmount += Convert.ToDecimal(dr["ITEM_DISCOUNT"]);

                                //GL
                                //Get GL Detail If exists
                                DataTable dtVoucher = GetData(SelectedLocation, Convert.ToInt64(InvoiceNo), 6);
                                if (dtVoucher.Rows.Count > 0)
                                {
                                    if (dtVoucher.Rows[0]["IsFinanceIntegrate"].ToString() == "1")
                                    {
                                        //Delete GL Master
                                        StringBuilder sbGL = new StringBuilder();
                                        sbGL = new StringBuilder();
                                        sbGL.Append(Environment.NewLine);
                                        sbGL.Append("DELETE FROM GL_MASTER WHERE VOUCHER_NO = '" + dtVoucher.Rows[0]["VOUCHER_NO"].ToString() + "' AND VOUCHER_TYPE_ID=" + Convert.ToInt32(dtVoucher.Rows[0]["VOUCHER_TYPE_ID"]) + " AND DISTRIBUTOR_ID =" + SelectedLocation);
                                        ExecuteScript(sbGL.ToString(), cbDatabase.SelectedItem.ToString());

                                        //Delete GL Detail
                                        StringBuilder sbGLDetail = new StringBuilder();
                                        sbGLDetail = new StringBuilder();
                                        sbGLDetail.Append(Environment.NewLine);
                                        sbGLDetail.Append("DELETE FROM GL_DETAIL WHERE VOUCHER_NO = '" + dtVoucher.Rows[0]["VOUCHER_NO"].ToString() + "' AND VOUCHER_TYPE_ID=" + Convert.ToInt32(dtVoucher.Rows[0]["VOUCHER_TYPE_ID"]) + " AND DISTRIBUTOR_ID =" + SelectedLocation);
                                        ExecuteScript(sbGLDetail.ToString(), cbDatabase.SelectedItem.ToString());
                                    }
                                    //Insert GL
                                    if (InsertGLMasterSale(SelectedLocation, 0, Convert.ToInt32(dr["USER_ID"]), Convert.ToInt64(InvoiceNo), Convert.ToDateTime(dr["DOCUMENT_DATE"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), dtVoucher.Rows[0]["REMARKS"].ToString()))
                                    {
                                        if (dr["CUSTOMER_TYPE_ID"].ToString() == "2" && Convert.ToInt32(dr["DELIVERY_CHANNEL"]) > 0)
                                        {
                                            if (pAmountDue + GST + pServiceCharges - DiscountAmount > 0)
                                            {
                                                InsertGLDetail(SelectedLocation, 0, pAmountDue + GST + pServiceCharges - DiscountAmount, 0, Convert.ToInt64(dr["DELIVERY_CHANNEL"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Third Party Delivery Sale Voucher");
                                                InsertGLDetail(SelectedLocation, 0, 0, pAmountDue - DiscountAmount, Convert.ToInt64(dtVoucher.Rows[0]["CreditCardSales"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Third Party Delivery Sale Voucher");
                                                if (DiscountAmount > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, DiscountAmount, 0, Convert.ToInt64(dtVoucher.Rows[0]["DiscountonSale"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Discount Sale Voucher");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (pPaymentModeId == 0)//Cash Sale
                                            {
                                                if (pAmountDue + GST + pServiceCharges - DiscountAmount > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, pAmountDue + GST + pServiceCharges - DiscountAmount, 0, Convert.ToInt64(dtVoucher.Rows[0]["CashInHand"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Cash In Hand Sale Voucher");
                                                    InsertGLDetail(SelectedLocation, 0, 0, pAmountDue, Convert.ToInt64(dtVoucher.Rows[0]["CashSale"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Sale Voucher");
                                                    if (DiscountAmount > 0)
                                                    {
                                                        InsertGLDetail(SelectedLocation, 0, DiscountAmount, 0, Convert.ToInt64(dtVoucher.Rows[0]["DiscountonSale"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Discount Sale Voucher");
                                                    }

                                                    if (GST > 0)
                                                    {
                                                        InsertGLDetail(SelectedLocation, 0, 0, GST, Convert.ToInt64(dtVoucher.Rows[0]["SalesTax"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "GST Sale Voucher");
                                                    }
                                                    if (pServiceCharges > 0)
                                                    {
                                                        InsertGLDetail(SelectedLocation, 0, pServiceCharges, 0, Convert.ToInt64(dtVoucher.Rows[0]["ServiceCharges"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Service Charges Voucher");
                                                    }
                                                }
                                            }
                                            else if (pPaymentModeId == 1)//Credit Card Sale
                                            {
                                                if (pPaidin > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, pPaidin, 0, Convert.ToInt64(dtVoucher.Rows[0]["CashInHand"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Cash In Hand Sale Voucher");
                                                }
                                                if (Convert.ToInt64(dr["BANK_ID"]) > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, (pAmountDue + GST + pServiceCharges - DiscountAmount) - pPaidin, 0, Convert.ToInt64(dr["BANK_ID"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Credit Card Sale Voucher");
                                                }
                                                else
                                                {
                                                    if ((pAmountDue + GST + pServiceCharges - DiscountAmount) - pPaidin > 0)
                                                    {
                                                        InsertGLDetail(SelectedLocation, 0, (pAmountDue + GST + pServiceCharges - DiscountAmount) - pPaidin, 0, Convert.ToInt64(dtVoucher.Rows[0]["CreditCardSaleReceivable"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Credit Card Sale Voucher");
                                                    }
                                                }

                                                InsertGLDetail(SelectedLocation, 0, 0, pAmountDue - pPaidin, Convert.ToInt64(dtVoucher.Rows[0]["CreditCardSales"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Credit Card Sale Voucher");
                                                InsertGLDetail(SelectedLocation, 0, 0, pPaidin, Convert.ToInt64(dtVoucher.Rows[0]["CashSale"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Cash In Hand Sale Voucher");
                                                if (DiscountAmount > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, DiscountAmount, 0, Convert.ToInt64(dtVoucher.Rows[0]["DiscountonSale"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Discount Sale Voucher");
                                                }

                                                if (GST > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, 0, GST, Convert.ToInt64(dtVoucher.Rows[0]["SalesTax"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "GST Sale Voucher");
                                                }
                                                if (pServiceCharges > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, pServiceCharges, 0, Convert.ToInt64(dtVoucher.Rows[0]["ServiceCharges"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Service Charges Voucher");
                                                }
                                            }
                                            else if (pPaymentModeId == 2)//Credit Sale
                                            {
                                                if (pPaidin > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, pPaidin, 0, Convert.ToInt64(dtVoucher.Rows[0]["CashInHand"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Cash In Hand Sale Voucher");
                                                }
                                                if ((pAmountDue + GST + pServiceCharges - DiscountAmount) - pPaidin > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, (pAmountDue + GST + pServiceCharges - DiscountAmount) - pPaidin, 0, Convert.ToInt64(dtVoucher.Rows[0]["CreditCardSaleReceivable"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Credit Sale Voucher");
                                                }
                                                InsertGLDetail(SelectedLocation, 0, 0, pAmountDue, Convert.ToInt64(dtVoucher.Rows[0]["CreditSales"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Credit Sale Voucher");
                                                if (DiscountAmount > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, DiscountAmount, 0, Convert.ToInt64(dtVoucher.Rows[0]["DiscountonSale"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Discount Sale Voucher");
                                                }

                                                if (GST > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, 0, GST, Convert.ToInt64(dtVoucher.Rows[0]["SalesTax"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "GST Sale Voucher");
                                                }
                                                if (pServiceCharges > 0)
                                                {
                                                    InsertGLDetail(SelectedLocation, 0, pServiceCharges, 0, Convert.ToInt64(dtVoucher.Rows[0]["ServiceCharges"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Service Charges Voucher");
                                                }
                                            }
                                        }
                                        if (GLConsumption > 0)
                                        {
                                            InsertGLDetail(SelectedLocation, 0, GLConsumption, 0, Convert.ToInt64(dtVoucher.Rows[0]["Consumption"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Consumption Sale Voucher");
                                            InsertGLDetail(SelectedLocation, 0, 0, GLConsumption, Convert.ToInt64(dtVoucher.Rows[0]["Inventoryatstore"]), dtVoucher.Rows[0]["VOUCHER_NO"].ToString(), "Inventoryatstore Sale Voucher");
                                        }
                                    }
                                }
                            }
                        }
                        txtInvoiceNo.Text = string.Empty;
                        ZKMessgeBox.Show(RowsEffected.ToString() + " Rows effected");
                    }
                }
                else
                {
                    ZKMessgeBox.Show("Enter Invoice No.");
                    txtDocumentNo.Focus();
                }
            }
            else
            {
                ZKMessgeBox.Show("Select Database.");
                cbDatabase.Focus();
            }
        }

        private void cbAll_CheckedChanged(object sender, EventArgs e)
        {
            int _rows = gridItem.Rows.Count;
            if (cbAll.Checked)
            {
                for (int i = 0; i < _rows; i++)
                {
                    gridItem.Rows[i].Cells["chkSelect"].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < _rows; i++)
                {
                    gridItem.Rows[i].Cells["chkSelect"].Value = false;
                }
            }
        }

        private void cbFinish_CheckedChanged(object sender, EventArgs e)
        {
            LoadItemGrid();
        }

        private void cbRaw_CheckedChanged(object sender, EventArgs e)
        {
            LoadItemGrid();
        }

        private void cbPackaging_CheckedChanged(object sender, EventArgs e)
        {
            LoadItemGrid();
        }

        private void LoadItemGrid()
        {
            int SelectedLocation = 0;
            try
            {
                SelectedLocation = ((System.Collections.Generic.KeyValuePair<int, string>)cblLocation.SelectedValue).Key;
            }
            catch (Exception)
            {
                SelectedLocation = Convert.ToInt32(cblLocation.SelectedValue);
            }
            string BrandIDs = "";
            if (cbFinish.Checked)
            {
                BrandIDs += "1,";
            }
            if (cbRaw.Checked)
            {
                BrandIDs += "2,";
            }
            if (cbPackaging.Checked)
            {
                BrandIDs += "3";
            }
            gridItem.DataSource = null;
            dtGridItem = LoadItems(SelectedLocation, 2, BrandIDs);
            gridItem.DataSource = dtGridItem;
        }

        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            DataTable dtItem2 = new DataTable();
            dtItem2.Columns.Add("ItemID", typeof(int));
            dtItem2.Columns.Add("ItemName", typeof(string));
            DataRow dr = dtItem2.NewRow();
            foreach (DataRow gvr in dtGridItem.Rows)
            {
                dr = dtItem2.NewRow();
                if (gvr["ItemName"].ToString().ToLower().Contains(txtSearchItem.Text.ToLower()))
                {
                    dr["ItemID"] = gvr["ItemID"];
                    dr["ItemName"] = gvr["ItemName"];
                    dtItem2.Rows.Add(dr);
                }
            }
            gridItem.DataSource = dtItem2;
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            var confirmResult = ZKMessgeBox.Show("Did you took backup of " + cbDatabase.SelectedItem.ToString() + " Database??", "Confirm Database Backup!!", ZKMessgeBox.I8Buttons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                StringBuilder spDeleteItemSALE_INVOICE_CONSUMED = new StringBuilder();
                StringBuilder spDeleteItemSALE_INVOICE_DETAIL = new StringBuilder();
                StringBuilder spDeleteItemtblPackageMaterial = new StringBuilder();
                StringBuilder spDeleteItemtblPackageMaterialDetail = new StringBuilder();
                StringBuilder spDeleteItemtblSKUModifier = new StringBuilder();
                StringBuilder spDeleteItemSKUS = new StringBuilder();
                int count = 0;
                foreach (DataGridViewRow gvr in gridItem.Rows)
                {
                    if (Convert.ToBoolean(gvr.Cells["chkSelect"].Value))
                    {
                        count++;
                        spDeleteItemSALE_INVOICE_CONSUMED = new StringBuilder();
                        spDeleteItemSALE_INVOICE_CONSUMED.Append(Environment.NewLine);
                        spDeleteItemSALE_INVOICE_CONSUMED.Append("DELETE FROM SALE_INVOICE_CONSUMED WHERE SKU_ID=" + Convert.ToInt32(gvr.Cells[1].Value));
                        ExecuteScript(spDeleteItemSALE_INVOICE_CONSUMED.ToString(), cbDatabase.SelectedItem.ToString());

                        spDeleteItemSALE_INVOICE_DETAIL = new StringBuilder();
                        spDeleteItemSALE_INVOICE_DETAIL.Append(Environment.NewLine);
                        spDeleteItemSALE_INVOICE_DETAIL.Append("DELETE FROM SALE_INVOICE_DETAIL WHERE SKU_ID=" + Convert.ToInt32(gvr.Cells[1].Value));
                        ExecuteScript(spDeleteItemSALE_INVOICE_DETAIL.ToString(), cbDatabase.SelectedItem.ToString());

                        spDeleteItemtblPackageMaterial = new StringBuilder();
                        spDeleteItemtblPackageMaterial.Append(Environment.NewLine);
                        spDeleteItemtblPackageMaterial.Append("DELETE FROM tblPackageMaterial WHERE SKU_ID=" + Convert.ToInt32(gvr.Cells[1].Value));
                        ExecuteScript(spDeleteItemtblPackageMaterial.ToString(), cbDatabase.SelectedItem.ToString());

                        spDeleteItemtblPackageMaterialDetail = new StringBuilder();
                        spDeleteItemtblPackageMaterialDetail.Append(Environment.NewLine);
                        spDeleteItemtblPackageMaterialDetail.Append("DELETE FROM tblPackageMaterialDetail WHERE SKU_ID=" + Convert.ToInt32(gvr.Cells[1].Value));
                        ExecuteScript(spDeleteItemtblPackageMaterialDetail.ToString(), cbDatabase.SelectedItem.ToString());

                        spDeleteItemtblSKUModifier = new StringBuilder();
                        spDeleteItemtblSKUModifier.Append(Environment.NewLine);
                        spDeleteItemtblSKUModifier.Append("DELETE FROM tblSKUModifier WHERE SKU_ID=" + Convert.ToInt32(gvr.Cells[1].Value));
                        ExecuteScript(spDeleteItemtblSKUModifier.ToString(), cbDatabase.SelectedItem.ToString());

                        spDeleteItemSKUS = new StringBuilder();
                        spDeleteItemSKUS.Append(Environment.NewLine);
                        spDeleteItemSKUS.Append("DELETE FROM SKUS WHERE SKU_ID=" + Convert.ToInt32(gvr.Cells[1].Value));
                        ExecuteScript(spDeleteItemSKUS.ToString(), cbDatabase.SelectedItem.ToString());
                    }
                }

                if (count > 0)
                {
                    LoadItemGrid();
                    ZKMessgeBox.Show(count.ToString() + " Items deleted");
                }
            }
        }

        private void txtShiftPrices_Click(object sender, EventArgs e)
        {
            var confirmResult = ZKMessgeBox.Show("Did you took backup of " + cbDatabase.SelectedItem.ToString() + " Database??", "Confirm Database Backup!!", ZKMessgeBox.I8Buttons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                int SelectedLocation = 0;
                int SelectedLocationTo = 0;
                try
                {
                    SelectedLocation = ((System.Collections.Generic.KeyValuePair<int, string>)cblLocation.SelectedValue).Key;
                    SelectedLocationTo = ((System.Collections.Generic.KeyValuePair<int, string>)cbToPriceLocation.SelectedValue).Key;
                }
                catch (Exception)
                {
                    SelectedLocation = Convert.ToInt32(cblLocation.SelectedValue);
                    SelectedLocationTo = Convert.ToInt32(cbToPriceLocation.SelectedValue);
                }

                if (SelectedLocation == SelectedLocationTo)
                {
                    ZKMessgeBox.Show("Price Shit To Location should not be same as Price Shift From Location");
                    return;
                }

                DateTime dtClosingDate = DateTime.MinValue;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT ISNULL(MAX(CLOSING_DATE),GETDATE()) FROM DAILY_CLOSE WHERE DISTRIBUTOR_ID = " + Convert.ToInt32(SelectedLocationTo), cbDatabase.SelectedItem), con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dtClosingDate = Convert.ToDateTime(dr[0]);
                            }
                        }
                    }
                    con.Close();
                }

                int count = 0;
                decimal TRADE_PRICE = 0;
                bool found = false;
                StringBuilder sbInsertPrice = new StringBuilder();
                foreach (DataGridViewRow gvr in gridItem.Rows)
                {
                    if (Convert.ToBoolean(gvr.Cells["chkSelect"].Value))
                    {
                        TRADE_PRICE = 0;
                        found = false;
                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT TOP 1 TRADE_PRICE FROM SKU_PRICES WHERE DISTRIBUTOR_ID = " + Convert.ToInt32(SelectedLocation) + " AND SKU_ID=" + Convert.ToInt32(gvr.Cells[1].Value), cbDatabase.SelectedItem), con))
                            {
                                using (IDataReader dr = cmd.ExecuteReader())
                                {
                                    while (dr.Read())
                                    {
                                        found = true;
                                        TRADE_PRICE = Convert.ToDecimal(dr[0]);
                                    }
                                }
                            }
                            con.Close();
                        }
                        if (found)
                        {
                            count++;
                            sbInsertPrice = new StringBuilder();
                            sbInsertPrice.Append(Environment.NewLine);
                            sbInsertPrice.Append("DELETE FROM SKU_PRICES WHERE DISTRIBUTOR_ID=" + SelectedLocationTo + " AND SKU_ID=" + Convert.ToInt32(gvr.Cells[1].Value));
                            sbInsertPrice.Append(Environment.NewLine);
                            sbInsertPrice.Append("INSERT INTO SKU_PRICES(DISTRIBUTOR_ID,SKU_ID,DISTRIBUTOR_PRICE,TRADE_PRICE,RETAIL_PRICE,DISTRIBUTOR_DISCOUNT,TAX_PRICE,SED_TAX,Commission,Discount,TIME_STAMP,LASTUPDATE_DATE,DATE_EFFECTED,USER_ID)");
                            sbInsertPrice.Append(Environment.NewLine);
                            sbInsertPrice.Append("VALUES(" + SelectedLocationTo + "," + Convert.ToInt32(gvr.Cells[1].Value) + ",0," + TRADE_PRICE + ",0,0,0,0,0,0,'" + DateTime.Now + "','" + DateTime.Now + "','" + dtClosingDate + "',1)");
                            ExecuteScript(sbInsertPrice.ToString(), cbDatabase.SelectedItem.ToString());
                        }
                    }
                }
                ZKMessgeBox.Show(count.ToString() + " Row(s) effected!");
            }
        }

        private void btnUpdateConsumption_Click(object sender, EventArgs e)
        {
            var confirmResult = ZKMessgeBox.Show("Are you sure to Update Consumption??", "Confirm Update Consumption!!", ZKMessgeBox.I8Buttons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                DateTime dtFrom = Convert.ToDateTime(dtpDate.Value.ToString("yyyy-MM-dd"));
                DateTime dtTo = Convert.ToDateTime(dtpToDate.Value.ToString("yyyy-MM-dd"));
                while (dtFrom <= dtTo)
                {
                    int RowIndex = 0;
                    bool flagClosingStock = false;
                    bool Is_Production = false;
                    int SelectedLocation = 0;
                    DataTable dtConsumptionOld = new DataTable();
                    dtConsumptionOld.Columns.Add("SKU_ID", typeof(int));
                    dtConsumptionOld.Columns.Add("QTY", typeof(decimal));
                    try
                    {
                        SelectedLocation = ((System.Collections.Generic.KeyValuePair<int, string>)cblLocation.SelectedValue).Key;
                    }
                    catch (Exception)
                    {
                        SelectedLocation = Convert.ToInt32(cblLocation.SelectedValue);
                    }
                    //Get Consumption Old Data
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT SKU_ID,SUM(QTY) AS QTY FROM SALE_INVOICE_CONSUMED WHERE SALE_INVOICE_ID IN(SELECT SALE_INVOICE_ID FROM SALE_INVOICE_MASTER WHERE DISTRIBUTOR_ID = " + SelectedLocation + " AND IS_HOLD = 0 AND DOCUMENT_DATE = '" + dtFrom + "') GROUP BY SKU_ID", cbDatabase.SelectedItem), con))
                        {
                            using (IDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    DataRow drNew = dtConsumptionOld.NewRow();
                                    drNew["SKU_ID"] = dr[0];
                                    drNew["QTY"] = dr[1];
                                    dtConsumptionOld.Rows.Add(drNew);
                                }
                            }
                        }
                        con.Close();
                    }

                    //Update Consumption Stock
                    if (dtConsumptionOld.Rows.Count > 0)
                    {
                        StringBuilder sbUpdateConsumtion = new StringBuilder();
                        StringBuilder sbUpdateConsumtion2 = new StringBuilder();
                        foreach (DataRow dr in dtConsumptionOld.Rows)
                        {
                            sbUpdateConsumtion = new StringBuilder();
                            sbUpdateConsumtion.Append(Environment.NewLine);
                            sbUpdateConsumtion.Append("UPDATE SKU_STOCK_REGISTER SET CONSUMED = CONSUMED - " + Convert.ToDecimal(dr["QTY"]) + ",CLOSING_STOCK = CLOSING_STOCK + " + Convert.ToDecimal(dr["QTY"]) + " WHERE SKU_ID =" + Convert.ToInt32(dr["SKU_ID"]) + " AND STOCK_DATE ='" + dtFrom + "' AND DISTRIBUTOR_ID =" + SelectedLocation);
                            ExecuteScript(sbUpdateConsumtion.ToString(), cbDatabase.SelectedItem.ToString());

                            sbUpdateConsumtion2 = new StringBuilder();
                            sbUpdateConsumtion2.Append(Environment.NewLine);
                            sbUpdateConsumtion2.Append("UPDATE SKU_STOCK_REGISTER SET OPENING_STOCK = OPENING_STOCK + " + Convert.ToDecimal(dr["QTY"]) + ",CLOSING_STOCK = CLOSING_STOCK + " + Convert.ToDecimal(dr["QTY"]) + " WHERE SKU_ID =" + Convert.ToInt32(dr["SKU_ID"]) + " AND STOCK_DATE >'" + dtFrom + "' AND DISTRIBUTOR_ID =" + SelectedLocation);
                            ExecuteScript(sbUpdateConsumtion2.ToString(), cbDatabase.SelectedItem.ToString());
                        }
                    }
                    //Delete Old Consumption Data
                    StringBuilder sbDeleteConsumtion = new StringBuilder();
                    sbDeleteConsumtion.Append(Environment.NewLine);
                    sbDeleteConsumtion.Append("DELETE FROM SALE_INVOICE_CONSUMED WHERE SALE_INVOICE_ID IN(SELECT SALE_INVOICE_ID FROM SALE_INVOICE_MASTER WHERE DISTRIBUTOR_ID = " + SelectedLocation + " AND IS_HOLD = 0 AND DOCUMENT_DATE = '" + dtFrom + "')");
                    ExecuteScript(sbDeleteConsumtion.ToString(), cbDatabase.SelectedItem.ToString());

                    //Get Data For Cosnumption
                    DataTable dtConsumption = new DataTable();
                    dtConsumption.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                    dtConsumption.Columns.Add("SALE_INVOICE_ID", typeof(long));
                    dtConsumption.Columns.Add("SKU_ID", typeof(int));
                    dtConsumption.Columns.Add("QTY", typeof(decimal));
                    dtConsumption.Columns.Add("UOM_ID", typeof(int));
                    dtConsumption.Columns.Add("intStockMUnitCode", typeof(int));
                    dtConsumption.Columns.Add("Stock_to_SaleOperator", typeof(string));
                    dtConsumption.Columns.Add("Stock_to_SaleFactor", typeof(string));
                    dtConsumption.Columns.Add("CAT_ID", typeof(int));
                    dtConsumption.Columns.Add("I_D_ID", typeof(int));
                    dtConsumption.Columns.Add("T_PRICE", typeof(decimal));
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} SELECT SALE_INVOICE_ID, SID.SALE_INVOICE_DETAIL_ID,SID.SKU_ID,SID.QTY,isnull(S.intSaleMUnitCode,0)  [UOM_ID],S.intStockMUnitCode,S.Stock_to_SaleOperator,S.Stock_to_SaleFactor,S.CATEGORY_ID AS CAT_ID, ITEM_DEAL_ID AS I_D_ID,PRICE AS T_PRICE FROM SALE_INVOICE_DETAIL SID INNER JOIN SKUS S ON S.SKU_ID = SID.SKU_ID WHERE SID.SALE_INVOICE_ID IN(SELECT SALE_INVOICE_ID FROM SALE_INVOICE_MASTER WHERE DISTRIBUTOR_ID = " + SelectedLocation + " AND IS_HOLD = 0 AND DOCUMENT_DATE = '" + dtFrom + "')", cbDatabase.SelectedItem), con))
                        {
                            using (IDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    DataRow drNew = dtConsumption.NewRow();
                                    drNew["SALE_INVOICE_ID"] = dr[0];
                                    drNew["SALE_INVOICE_DETAIL_ID"] = dr[1];
                                    drNew["SKU_ID"] = dr[2];
                                    drNew["QTY"] = dr[3];
                                    drNew["UOM_ID"] = dr[4];
                                    drNew["intStockMUnitCode"] = dr[5];
                                    drNew["Stock_to_SaleOperator"] = dr[6];
                                    drNew["Stock_to_SaleFactor"] = dr[7];
                                    drNew["CAT_ID"] = dr[8];
                                    drNew["I_D_ID"] = dr[9];
                                    drNew["T_PRICE"] = dr[10];
                                    dtConsumption.Rows.Add(drNew);
                                }
                            }
                        }
                        con.Close();
                    }
                    if (dtConsumption.Rows.Count > 0)
                    {
                        StringBuilder sbUpdateConsumtion = new StringBuilder();
                        StringBuilder sbUpdateConsumtion2 = new StringBuilder();
                        long SALE_INVOICE_DETAIL_ID = 0;
                        long SALE_INVOICE_ID = 0;
                        foreach (DataRow dr in dtConsumption.Rows)
                        {
                            SALE_INVOICE_DETAIL_ID = Convert.ToInt64(dr["SALE_INVOICE_DETAIL_ID"]);
                            SALE_INVOICE_ID = Convert.ToInt64(dr["SALE_INVOICE_ID"]);
                            DataTable dtRaw = GetConsumptionDetail(SelectedLocation, Convert.ToInt32(dr["SKU_ID"]), dtFrom, 4);
                            if (dtRaw != null)
                            {
                                decimal StockQty = 0;
                                decimal ConsumedQty = 0;
                                RowIndex = 0;
                                flagClosingStock = false;
                                Is_Production = false;

                                foreach (DataRow drRaw in dtRaw.Rows)
                                {
                                    RowIndex++;
                                    if (RowIndex == 1 && Convert.ToDecimal(drRaw["CLOSING_STOCK"]) <= 0)
                                    {
                                        flagClosingStock = true;
                                    }
                                    if (RowIndex == 1 && drRaw["Is_Production"].ToString() == "1")
                                    {
                                        Is_Production = true;
                                    }

                                    if (drRaw["Type"].ToString() == "Finish")
                                    {
                                        if (drRaw["IsModifierdItem"].ToString() == "1")
                                        {
                                            StockQty = decimal.Parse(dr["QTY"].ToString());
                                        }
                                        else
                                        {
                                            StockQty = Conversion(drRaw["Sale_to_StockOperator"].ToString(), decimal.Parse(drRaw["Sale_to_StockFactor"].ToString()), 1, decimal.Parse(dr["QTY"].ToString()), 0, "Finish");
                                        }

                                        if (drRaw["Is_Inventory"].ToString() == "True" && drRaw["CHECK"].ToString() == "EXIST" && drRaw["Is_Production"].ToString() == "1")
                                        {
                                            if (flagClosingStock)
                                            {
                                                ConsumedQty = StockQty;
                                            }
                                        }
                                    }
                                    else if (drRaw["Type"].ToString() == "Raw")
                                    {
                                        if (Is_Production)
                                        {
                                            if (drRaw["Is_Inventory"].ToString() == "True" && flagClosingStock)
                                            {
                                                if (drRaw["IsModifierdItem"].ToString() == "1")
                                                {
                                                    ConsumedQty = decimal.Parse(dr["QTY"].ToString());
                                                }
                                                else
                                                {
                                                    ConsumedQty = Conversion(drRaw["Sale_to_StockOperator"].ToString(), decimal.Parse(drRaw["Sale_to_StockFactor"].ToString()), 1, StockQty, decimal.Parse(drRaw["QUANTITY"].ToString()), "Raw");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (drRaw["Is_Inventory"].ToString() == "True")
                                            {
                                                if (drRaw["IsModifierdItem"].ToString() == "1")
                                                {
                                                    ConsumedQty = decimal.Parse(dr["QTY"].ToString());
                                                }
                                                else
                                                {
                                                    ConsumedQty = Conversion(drRaw["Sale_to_StockOperator"].ToString(), decimal.Parse(drRaw["Sale_to_StockFactor"].ToString()), 1, StockQty, decimal.Parse(drRaw["QUANTITY"].ToString()), "Raw");
                                                }
                                            }
                                        }
                                    }
                                    else if (drRaw["Type"].ToString() == "Package")
                                    {
                                        if (drRaw["Is_Inventory"].ToString() == "True")
                                        {
                                            if (drRaw["IsModifierdItem"].ToString() == "1")
                                            {
                                                ConsumedQty = decimal.Parse(dr["QTY"].ToString());
                                            }
                                            else
                                            {
                                                ConsumedQty = Conversion(drRaw["Sale_to_StockOperator"].ToString(), decimal.Parse(drRaw["Sale_to_StockFactor"].ToString()), 1, StockQty, decimal.Parse(drRaw["QUANTITY"].ToString()), "Raw");
                                            }
                                        }
                                    }
                                    sbUpdateConsumtion = new StringBuilder();
                                    sbUpdateConsumtion2 = new StringBuilder();
                                    if (Convert.ToBoolean(drRaw["IS_MODIFIER"]))
                                    {
                                        sbUpdateConsumtion.Append(Environment.NewLine);
                                        sbUpdateConsumtion.Append("UPDATE SKU_STOCK_REGISTER SET CONSUMED = CONSUMED + " + StockQty + ",CLOSING_STOCK = CLOSING_STOCK - " + StockQty + " WHERE SKU_ID =" + Convert.ToInt32(drRaw["SKU_ID"]) + " AND STOCK_DATE ='" + dtFrom + "' AND DISTRIBUTOR_ID =" + SelectedLocation);
                                        ExecuteScript(sbUpdateConsumtion.ToString(), cbDatabase.SelectedItem.ToString());

                                        sbUpdateConsumtion2 = new StringBuilder();
                                        sbUpdateConsumtion2.Append(Environment.NewLine);
                                        sbUpdateConsumtion2.Append("UPDATE SKU_STOCK_REGISTER SET OPENING_STOCK = OPENING_STOCK - " + StockQty + ",CLOSING_STOCK = CLOSING_STOCK - " + StockQty + " WHERE SKU_ID =" + Convert.ToInt32(drRaw["SKU_ID"]) + " AND STOCK_DATE >'" + dtFrom + "' AND DISTRIBUTOR_ID =" + SelectedLocation);
                                        ExecuteScript(sbUpdateConsumtion2.ToString(), cbDatabase.SelectedItem.ToString());
                                    }
                                    else
                                    {
                                        sbUpdateConsumtion.Append(Environment.NewLine);
                                        sbUpdateConsumtion.Append("UPDATE SKU_STOCK_REGISTER SET CONSUMED = CONSUMED + " + ConsumedQty + ",CLOSING_STOCK = CLOSING_STOCK - " + ConsumedQty + " WHERE SKU_ID =" + Convert.ToInt32(drRaw["SKU_ID"]) + " AND STOCK_DATE ='" + dtFrom + "' AND DISTRIBUTOR_ID =" + SelectedLocation);
                                        ExecuteScript(sbUpdateConsumtion.ToString(), cbDatabase.SelectedItem.ToString());

                                        sbUpdateConsumtion2 = new StringBuilder();
                                        sbUpdateConsumtion2.Append(Environment.NewLine);
                                        sbUpdateConsumtion2.Append("UPDATE SKU_STOCK_REGISTER SET OPENING_STOCK = OPENING_STOCK - " + ConsumedQty + ",CLOSING_STOCK = CLOSING_STOCK - " + ConsumedQty + " WHERE SKU_ID =" + Convert.ToInt32(drRaw["SKU_ID"]) + " AND STOCK_DATE >'" + dtFrom + "' AND DISTRIBUTOR_ID =" + SelectedLocation);
                                        ExecuteScript(sbUpdateConsumtion2.ToString(), cbDatabase.SelectedItem.ToString());
                                    }

                                    if (drRaw["Type"].ToString() == "Finish")
                                    {
                                        if (drRaw["Is_Inventory"].ToString() == "True")
                                        {
                                            if (flagClosingStock)
                                            {
                                                decimal price = Convert.ToDecimal(drRaw["LastPurPrice"]);
                                                decimal consumedqty = ConsumedQty;
                                                if (price == 0)
                                                {
                                                    price = decimal.Parse(dr["T_PRICE"].ToString());
                                                }
                                                if (consumedqty == 0)
                                                {
                                                    if (dr["UOM_ID"].ToString() != dr["intStockMUnitCode"].ToString())
                                                    {
                                                        consumedqty = Conversion(dr["Stock_to_SaleOperator"].ToString(), decimal.Parse(dr["Stock_to_SaleFactor"].ToString()), 1, decimal.Parse(dr["QTY"].ToString()), 0, "Finish");
                                                    }
                                                    else
                                                    {
                                                        consumedqty = decimal.Parse(dr["QTY"].ToString());
                                                    }
                                                }
                                                Consumption(SelectedLocation, SALE_INVOICE_ID, SALE_INVOICE_DETAIL_ID, Convert.ToInt32(drRaw["SKU_ID"]), int.Parse(dr["CAT_ID"].ToString()), price, consumedqty, consumedqty, false, "Finish", false, 0);
                                            }
                                        }
                                    }
                                    else if (drRaw["Type"].ToString() == "Raw")
                                    {
                                        if (Is_Production)
                                        {
                                            if (drRaw["Is_Inventory"].ToString() == "True" && flagClosingStock)
                                            {
                                                decimal SaleQty = Conversion(drRaw["Stock_to_SaleOperator"].ToString(), decimal.Parse(drRaw["Stock_to_SaleFactor"].ToString()), decimal.Parse(drRaw["QUANTITY"].ToString()), ConsumedQty, 0, "Raw");
                                                Consumption(SelectedLocation, SALE_INVOICE_ID, SALE_INVOICE_DETAIL_ID, Convert.ToInt32(drRaw["SKU_ID"]), int.Parse(drRaw["PRODUCT_CATEGORY_ID"].ToString()), decimal.Parse(drRaw["LastPurPrice"].ToString()), ConsumedQty, SaleQty, false, "Raw", false, 0);
                                            }
                                        }
                                        else
                                        {
                                            if (drRaw["Is_Inventory"].ToString() == "True")
                                            {
                                                decimal SaleQty = Conversion(drRaw["Stock_to_SaleOperator"].ToString(), decimal.Parse(drRaw["Stock_to_SaleFactor"].ToString()), decimal.Parse(drRaw["QUANTITY"].ToString()), ConsumedQty, 0, "Raw");
                                                Consumption(SelectedLocation, SALE_INVOICE_ID, SALE_INVOICE_DETAIL_ID, Convert.ToInt32(drRaw["SKU_ID"]), int.Parse(drRaw["PRODUCT_CATEGORY_ID"].ToString()), decimal.Parse(drRaw["LastPurPrice"].ToString()), ConsumedQty, SaleQty, false, "Raw", false, 0);
                                            }
                                        }
                                    }
                                    else if (drRaw["Type"].ToString() == "Package")
                                    {
                                        if (drRaw["Is_Inventory"].ToString() == "True")
                                        {
                                            decimal SaleQty = Conversion(drRaw["Stock_to_SaleOperator"].ToString(), decimal.Parse(drRaw["Stock_to_SaleFactor"].ToString()), decimal.Parse(drRaw["QUANTITY"].ToString()), ConsumedQty, 0, "Raw");
                                            Consumption(SelectedLocation, SALE_INVOICE_ID, SALE_INVOICE_DETAIL_ID, Convert.ToInt32(drRaw["SKU_ID"]), int.Parse(drRaw["PRODUCT_CATEGORY_ID"].ToString()), decimal.Parse(drRaw["LastPurPrice"].ToString()), ConsumedQty, SaleQty, false, "Package", false, 0);
                                        }
                                    }
                                }
                            }
                        }

                        DataView view = new DataView(dtConsumption);
                        DataTable distinctDealIDs = view.ToTable(true, "I_D_ID");
                        foreach (DataRow dr in distinctDealIDs.Rows)
                        {
                            if (dr["I_D_ID"].ToString() != "0")
                            {
                                int DealID = Convert.ToInt32(dr["I_D_ID"]);
                                DataTable dtRaw = GetConsumptionDetail(SelectedLocation, Convert.ToInt32(dr["I_D_ID"]), dtFrom, 3);
                                decimal StockQty = 0;
                                decimal ConsumedQty = 0;
                                RowIndex = 0;
                                flagClosingStock = false;
                                Is_Production = false;
                                if (dtRaw != null)
                                {
                                    RowIndex = 0;
                                    flagClosingStock = false;
                                    Is_Production = false;
                                    foreach (DataRow drRaw in dtRaw.Rows)
                                    {
                                        if (drRaw["Type"].ToString() == "Package")
                                        {
                                            if (drRaw["Is_Inventory"].ToString() == "True")
                                            {
                                                ConsumedQty = Convert.ToDecimal(drRaw["QUANTITY"]);
                                            }
                                        }
                                        sbUpdateConsumtion = new StringBuilder();
                                        sbUpdateConsumtion2 = new StringBuilder();

                                        sbUpdateConsumtion.Append(Environment.NewLine);
                                        sbUpdateConsumtion.Append("UPDATE SKU_STOCK_REGISTER SET CONSUMED = CONSUMED + " + ConsumedQty + ",CLOSING_STOCK = CLOSING_STOCK - " + ConsumedQty + " WHERE SKU_ID =" + Convert.ToInt32(drRaw["SKU_ID"]) + " AND STOCK_DATE ='" + dtFrom + "' AND DISTRIBUTOR_ID =" + SelectedLocation);
                                        ExecuteScript(sbUpdateConsumtion.ToString(), cbDatabase.SelectedItem.ToString());

                                        sbUpdateConsumtion2 = new StringBuilder();
                                        sbUpdateConsumtion2.Append(Environment.NewLine);
                                        sbUpdateConsumtion2.Append("UPDATE SKU_STOCK_REGISTER SET OPENING_STOCK = OPENING_STOCK - " + ConsumedQty + ",CLOSING_STOCK = CLOSING_STOCK - " + ConsumedQty + " WHERE SKU_ID =" + Convert.ToInt32(drRaw["SKU_ID"]) + " AND STOCK_DATE >'" + dtFrom + "' AND DISTRIBUTOR_ID =" + SelectedLocation);
                                        ExecuteScript(sbUpdateConsumtion2.ToString(), cbDatabase.SelectedItem.ToString());
                                        if (drRaw["Type"].ToString() == "Package")
                                        {
                                            if (drRaw["Is_Inventory"].ToString() == "True")
                                            {
                                                decimal SaleQty = Convert.ToDecimal(drRaw["QUANTITY"]);
                                                Consumption(SelectedLocation, SALE_INVOICE_ID, SALE_INVOICE_DETAIL_ID, Convert.ToInt32(drRaw["SKU_ID"]), int.Parse(drRaw["PRODUCT_CATEGORY_ID"].ToString()), decimal.Parse(drRaw["DISTRIBUTOR_PRICE"].ToString()), ConsumedQty, SaleQty, false, "Package", true, DealID);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dtFrom = dtFrom.AddDays(1);
                }
                ZKMessgeBox.Show("Consumption updated successfully.");
            }
        }

        public DataTable GetConsumptionDetail(int SelectedLocation, int pFINISHED_SKU_ID, DateTime pDATE,int TypeID)
        {
            string conString2 = "server=" + System.Configuration.ConfigurationManager.AppSettings["server"].ToString()
                + ";uid=" + System.Configuration.ConfigurationManager.AppSettings["uid"].ToString()
                + ";pwd=" + System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString()
                + ";database=" + cbDatabase.Text;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetFinishedDetail";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = SelectedLocation };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TYPE_ID", DbType = DbType.Int32, Value = TypeID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@FINISHED_SKU_ID", DbType = DbType.Int32, Value = pFINISHED_SKU_ID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DATE", DbType = DbType.DateTime, Value = pDATE };
                        pparams.Add(parameter);

                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public decimal Conversion(string Operator, decimal Factor, decimal DefaultQty, decimal Qty, decimal RawQty, string Type)
        {
            decimal STOCK_QTY = 0;
            if (Type == "Finish")
            {
                switch (Operator)
                {
                    case "+":
                        STOCK_QTY = Qty * (Factor + DefaultQty);
                        break;
                    case "-":
                        STOCK_QTY = Qty * (Factor - DefaultQty);
                        break;
                    case "/":
                        STOCK_QTY = Qty * (Factor / DefaultQty);
                        break;
                    case "%":
                        STOCK_QTY = Qty * (Factor % DefaultQty);
                        break;
                    default:
                        STOCK_QTY = Qty * (Factor * DefaultQty);
                        break;
                }
            }
            else
            {
                switch (Operator)
                {
                    case "+":
                        STOCK_QTY = (Factor + DefaultQty) * (Qty * RawQty);
                        break;
                    case "-":
                        STOCK_QTY = (Factor - DefaultQty) * (Qty * RawQty);
                        break;
                    case "/":
                        STOCK_QTY = (Factor / DefaultQty) * (Qty * RawQty);
                        break;
                    case "%":
                        STOCK_QTY = (Factor % DefaultQty) * (Qty * RawQty);
                        break;
                    default:
                        STOCK_QTY = (Factor * DefaultQty) * (Qty * RawQty);
                        break;
                }
            }
            return STOCK_QTY;
        }
        public void Consumption(int p_Distributor_id, long p_SALE_INVOICE_ID, long p_SALE_INVOICE_DETAIL_ID, int p_SKU_ID, int p_PRODUCT_CATEGORY_ID, decimal p_PRICE, decimal p_Qty, decimal p_SaleQty, bool p_IS_VOID, string p_Remarks, bool p_IS_DEAL, int p_DEAL_ID)
        {
            try
            {
                int IsVoid = 0;
                int IsDeal = 0;
                if(p_IS_VOID)
                {
                    IsVoid = 1;
                }
                if (p_IS_DEAL)
                {
                    IsDeal = 1;
                }
                StringBuilder sbInsertConsumtion = new StringBuilder();
                sbInsertConsumtion.Append(Environment.NewLine);
                sbInsertConsumtion.Append("INSERT INTO SALE_INVOICE_CONSUMED(SALE_INVOICE_ID,SKU_ID,PRODUCT_CATEGORY_ID,PRICE,QTY,SaleQty,REMARKS,IS_VOID,DISTRIBUTOR_ID,SALE_INVOICE_DETAIL_ID,IS_DEAL,DEAL_ID)");
                sbInsertConsumtion.Append(Environment.NewLine.ToString());
                sbInsertConsumtion.Append("VALUES(" + p_SALE_INVOICE_ID + "," + p_SKU_ID + "," + p_PRODUCT_CATEGORY_ID + "," + p_PRICE + "," + p_Qty + "," + p_SaleQty +",'" + p_Remarks + "'," + IsVoid + "," + p_Distributor_id + "," + p_SALE_INVOICE_DETAIL_ID + "," + IsDeal + "," + p_DEAL_ID + ")");
                ExecuteScript(sbInsertConsumtion.ToString(), cbDatabase.SelectedItem.ToString());

            }
            catch (Exception exp)
            {
                throw exp;
            }

        }
    }
}