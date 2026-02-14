using System;
using System.Data;
using CORNCommon.Classes;
using CORNDatabaseLayer.Classes;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections;
using CORNDataAccessLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    public class VenderEntryController
    {
        #region Constructor

        /// <summary>
        /// Constructor for OrderEntryController
        /// </summary>
        public VenderEntryController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Insert
        public long InsertSupplierAdvance(int p_LocationID, int p_SupplierID,DateTime p_AdvanceDate, int p_AdvanceType, decimal p_AdvanceAmount, int p_UserID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspInsertSupplierAdvance suppAdvance = new uspInsertSupplierAdvance();
                suppAdvance.Connection = mConnection;
                suppAdvance.LocationID = p_LocationID;
                suppAdvance.SupplierID = p_SupplierID;
                suppAdvance.AdvanceDate = p_AdvanceDate;
                suppAdvance.AdvanceType = p_AdvanceType;
                suppAdvance.AdvanceAmount = p_AdvanceAmount;
                suppAdvance.UserID = p_UserID;
                suppAdvance.ExecuteQuery();
                return suppAdvance.SupplierAdvanceID;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return 0;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        #endregion

        #region Select

        #region Vendoer Ledger

        public DataSet GetVendorLedger(string p_PRINCIPAL_ID, string p_DISTRIBUTOR_ID, DateTime p_FROM_DATE, DateTime p_TO_DATE,string SupplierName,int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DsReport ds = new Reports.DsReport();
                uspGetVendorLedgerReport mLedger = new uspGetVendorLedgerReport();
                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.FROM_DATE = p_FROM_DATE;
                mLedger.TO_DATE = p_TO_DATE;
                mLedger.TypeID = p_TypeID;
                DataTable DT = mLedger.ExecuteTable();
                if (DT.Rows.Count == 0)
                {
                    DataRow dr = DT.NewRow();
                    dr["account_head_id"] = 0;
                    dr["account_code"] = "";
                    dr["account_name"] = "";
                    dr["voucher_no"] = 0;
                    dr["ledger_date"] = DateTime.Now;
                    dr["debit"] = 0;
                    dr["credit"] = 0;
                    dr["remarks"] = "";
                    dr["Invoice_No"] = 0;
                    dr["Manual_Invoice_No"] = 0;
                    dr["Supplier"] = SupplierName;
                    dr["Op_Balance"] = 0;
                    DT.Rows.Add(dr);
                }
            
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptCustomerLedgerView"].ImportRow(dr);
                }
                spSelectCHEQUE_PROCESSReceiveDeposit mLedgerSub = new spSelectCHEQUE_PROCESSReceiveDeposit();
                mLedgerSub.Connection = mConnection;
                mLedgerSub.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedgerSub.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedgerSub.STATUS_ID = 527528;// Recieved and deposit Cheques
                DataTable dtPro = mLedgerSub.ExecuteTable();
                foreach (DataRow dr in dtPro.Rows)
                {
                    ds.Tables["spSelectCHEQUE_PROCESS2"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public DataTable GetVendorOpening(int p_PRINCIPAL_ID, int p_DISTRIBUTOR_ID, DateTime p_FROM_DATE,int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetVendorLedgerOpening mLedger = new uspGetVendorLedgerOpening();
                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.FROM_DATE = p_FROM_DATE;
                mLedger.TypeID = p_TypeID;
                DataTable DT = mLedger.ExecuteTable();
                return DT;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public DataSet GetSupplierClosingSummary(string p_PRINCIPAL_ID, string p_DISTRIBUTOR_ID, DateTime p_TO_DATE,int TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DSReportNew ds = new Reports.DSReportNew();

                uspGetSupplierClosingSummary mLedger = new uspGetSupplierClosingSummary();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.TO_DATE = p_TO_DATE;
                mLedger.TYPE_ID = TypeID;
                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["uspGetSupplierClosingSummary"].ImportRow(dr);

                }
                return ds;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public DataSet GetSupplierStatments(int p_PRINCIPAL_ID, int p_DISTRIBUTOR_ID, DateTime p_FROM_DATE, DateTime p_TO_DATE, int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DsReport ds = new Reports.DsReport();

                uspGetSupplierStatmentReport mLedger = new uspGetSupplierStatmentReport();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.FROM_DATE = p_FROM_DATE;
                mLedger.TO_DATE = p_TO_DATE;
                mLedger.TypeID = p_TypeID;
                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["uspGetSupplierStatmentReport"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public DataSet GetSupplierPaymentDetail(int p_DISTRIBUTOR_ID, DateTime p_FROM_DATE,int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DsReport ds = new Reports.DsReport();
                uspGetSupplierStatmentReport mLedger = new uspGetSupplierStatmentReport();
                mLedger.Connection = mConnection;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.FROM_DATE = p_FROM_DATE;
                mLedger.TypeID = p_TypeID;
                DataTable DT = mLedger.ExecuteTableForPaymentDetail();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["uspGetSupplierStatmentReport"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        #endregion

        #region Supplier Payable Ageing
        public DataSet GetSupplierAgeing(string p_PRINCIPAL_ID, string p_DISTRIBUTOR_IDs,DateTime p_TO_DATE,int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DSReportNew ds = new Reports.DSReportNew();

                uspGetSupplierAging mLedger = new uspGetSupplierAging();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_IDs = p_DISTRIBUTOR_IDs;
                mLedger.TO_DATE = p_TO_DATE;
                mLedger.TypeID = p_TypeID;
                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["uspGetSupplierAging"].ImportRow(dr);

                }
                return ds;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public DataSet GetSupplierAgeingInvoiceWise(string p_PRINCIPAL_IDs, string p_DISTRIBUTOR_IDs, DateTime p_TO_DATE,int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                uspGetSupplierAging mLedger = new uspGetSupplierAging();
                mLedger.Connection = mConnection;
                mLedger.DISTRIBUTOR_IDs = p_DISTRIBUTOR_IDs;
                mLedger.PRINCIPAL_IDs = p_PRINCIPAL_IDs;
                mLedger.TO_DATE = p_TO_DATE;
                mLedger.TypeID = p_TypeID;
                DataTable DT = mLedger.ExecuteTableInvoiceWise();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptSupplierWiseCreditAging"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        #endregion

        public DataSet GetSupplierPaymentDetailDateWise(int p_PRINCIPAL_ID, int p_DISTRIBUTOR_ID, DateTime p_FROM_DATE, DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DSReportNew ds = new Reports.DSReportNew();

                uspGetSupplierStatmentReport mLedger = new uspGetSupplierStatmentReport();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mLedger.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mLedger.FROM_DATE = p_FROM_DATE;
                mLedger.TO_DATE = p_TO_DATE;
                DataTable DT = mLedger.ExecuteTableForPaymentDetailDateWise();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptSupplierPaymentDetailDateWise"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        #endregion
    }
}