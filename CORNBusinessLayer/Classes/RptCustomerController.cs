using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Fetching Data Of Customer Reports
    /// </summary>
    public class RptCustomerController
    {
        #region Constructor

        /// <summary>
        /// Constructor for RptCustomerController
        /// </summary>
        public RptCustomerController()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

        #region Public Methods
        
        #region Customer Invoice Wise Sales Report        

        /// <summary>
        /// Gets Data For Customer Invoice Wise Sales Report
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Customer_Id">Customer</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="p_From_Date">DateFrom</param>
        /// <param name="p_ToDate">DateTo</param>
        /// <param name="p_showAll">Type</param>
        /// <returns>DataSet</returns>
        public DataSet SelectInvoiceDetail(int p_Principal_ID, string p_Sale_Invoice_Id,
            int p_DistributorId, DateTime p_From_Date, DateTime p_ToDate, Byte p_showAll,
            int p_ORDERBOOKER_ID, int p_Cashier_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspBillWiseInvoiceReportNew mInvoice = new UspBillWiseInvoiceReportNew();

                mInvoice.Connection = mConnection;
                mInvoice.PRINCIPAL_ID = p_Principal_ID;
                mInvoice.DISTRIBUTOR_ID = p_DistributorId;
                mInvoice.SALE_INVOICE_ID = p_Sale_Invoice_Id;
                mInvoice.FROM_DATE = p_From_Date;
                mInvoice.TO_DATE = p_ToDate;
                mInvoice.ShowAll = p_showAll;
                mInvoice.ORDERBOOKER_ID = p_ORDERBOOKER_ID;
                mInvoice.CASHIER_ID = p_Cashier_ID;
                mInvoice.PaymentModeID = Constants.ByteNullValue;
                DataTable DT = mInvoice.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["UspBillWiseInvoiceReport"].ImportRow(dr);
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
        public DataTable SelectInvoiceDetail2(int p_UserID, string p_Sale_Invoice_Id, 
            int p_DistributorId, DateTime p_From_Date, DateTime p_ToDate, Byte p_showAll)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
              

                UspBillWiseInvoiceReportNew mInvoice = new UspBillWiseInvoiceReportNew();

                mInvoice.Connection = mConnection;

                mInvoice.PRINCIPAL_ID = p_UserID;
                mInvoice.DISTRIBUTOR_ID = p_DistributorId;
                mInvoice.SALE_INVOICE_ID = p_Sale_Invoice_Id;
                mInvoice.FROM_DATE = p_From_Date;
                mInvoice.TO_DATE = p_ToDate;
                mInvoice.ShowAll = p_showAll;
                mInvoice.PaymentModeID = Constants.ByteNullValue;
                DataTable DT = mInvoice.ExecuteTable();
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
        public DataTable SelectInvoiceDetail2(int p_UserID, string p_Sale_Invoice_Id, int p_DistributorId, 
            DateTime p_From_Date, DateTime p_ToDate, int p_OrderBookerID, Byte p_showAll
            ,int p_PaymentModeID, int p_Cashier_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();


                UspBillWiseInvoiceReportNew mInvoice = new UspBillWiseInvoiceReportNew();

                mInvoice.Connection = mConnection;

                mInvoice.PRINCIPAL_ID = p_UserID;
                mInvoice.DISTRIBUTOR_ID = p_DistributorId;
                mInvoice.SALE_INVOICE_ID = p_Sale_Invoice_Id;
                mInvoice.FROM_DATE = p_From_Date;
                mInvoice.TO_DATE = p_ToDate;
                mInvoice.ShowAll = p_showAll;
                mInvoice.ORDERBOOKER_ID = p_OrderBookerID;
                mInvoice.PaymentModeID = p_PaymentModeID;
                mInvoice.CASHIER_ID = p_Cashier_ID;
                DataTable DT = mInvoice.ExecuteTable();
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
        #endregion        

        #region Customer Ledger Report

        /// <summary>
        /// Gets Data For Customer Ledger Report
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="p_CustomerId">Customer</param>
        /// <param name="p_From_Date">DateFrom</param>
        /// <param name="p_ToDate">DateTo</param>
        /// <returns>DataSet</returns>
        public DataSet CustomerLedger_View(int p_Principal_ID, string p_DistributorId, int p_CustomerId, DateTime p_From_Date, DateTime p_ToDate,int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                UspCustomerLedger mLedger = new UspCustomerLedger();
                mLedger.Connection = mConnection;
                mLedger.principal_id = p_Principal_ID;
                mLedger.Distributor_id = p_DistributorId;
                mLedger.customer_id = p_CustomerId;
                mLedger.From_Date = p_From_Date;
                mLedger.To_Date = p_ToDate;
                mLedger.TypeID = p_TypeID;
                DataTable DT = mLedger.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptCustomerLedgerView"].ImportRow(dr);
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

        #endregion
    }
}