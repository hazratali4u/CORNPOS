using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Fetching Data Of Different Crystal Reports
    /// </summary>
    public class DocumentPrintController
    {
        #region Constructor

        /// <summary>
        /// Constructor for DocumentPrintController
        /// </summary>
        public DocumentPrintController()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

        #region Public Methods

        /// <summary>
        /// Gets Company Name For All Reports
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <returns></returns>
        public DataTable SelectReportTitle(int p_Distributor_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectReportTitile ObjPrint = new UspSelectReportTitile();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                DataTable dt = ObjPrint.ExecuteTable();
                return dt;
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
        
        /// <summary>
        /// Gets Data For Role Management (View Report)
        /// </summary>
        /// <param name="p_Role_Id">Role</param>
        /// <returns>DataSet</returns>
        public DataSet SelectRoleManagmentReport(int p_Role_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspRolemanagementDetail ObjPrint = new UspRolemanagementDetail();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = Constants.IntNullValue;
                ObjPrint.ROLE_ID = p_Role_Id;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptRoleManagement"].ImportRow(dr);
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
        
        /// <summary>
        /// Gets Data For Voucher Editing (Print Voucher)
        /// </summary>
        /// <param name="p_Vouchertype_id">Type</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="p_Fromdate">DateFrom</param>
        /// <param name="p_ToDate">DateTo</param>
        /// <param name="Posted">Posted</param>
        /// <param name="p_UserId">User</param>
        /// <returns>DataSet</returns>
        public DataSet PrintVouchers(int p_Vouchertype_id, int p_DistributorId, DateTime p_Fromdate, DateTime p_ToDate, int Posted, int p_UserId)
        {
            IDbConnection mConnection = null;
            try
            {
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spPrintVouchers ObjLedger = new spPrintVouchers();
                ObjLedger.Connection = mConnection;
                ObjLedger.VOUCHER_TYPE_ID = p_Vouchertype_id;
                ObjLedger.DISTRIBUTOR_ID = p_DistributorId;
                ObjLedger.FROM_DATE = p_Fromdate;
                ObjLedger.TO_DATE = p_ToDate;
                ObjLedger.POSTED = Posted;
                ObjLedger.USER_ID = p_UserId;
                DataTable dt = ObjLedger.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptVoucherView"].ImportRow(dr);
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

        public DataSet PrintVouchers(int p_Vouchertype_id, int p_DistributorId, DateTime p_Fromdate, DateTime p_ToDate, int Posted, int p_UserId, int p_VOUCHER_TYPE_ID2)
        {
            IDbConnection mConnection = null;
            try
            {
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spPrintVouchers ObjLedger = new spPrintVouchers();
                ObjLedger.Connection = mConnection;
                ObjLedger.VOUCHER_TYPE_ID = p_Vouchertype_id;
                ObjLedger.DISTRIBUTOR_ID = p_DistributorId;
                ObjLedger.FROM_DATE = p_Fromdate;
                ObjLedger.TO_DATE = p_ToDate;
                ObjLedger.POSTED = Posted;
                ObjLedger.USER_ID = p_UserId;
                ObjLedger.VOUCHER_TYPE_ID2 = p_VOUCHER_TYPE_ID2;
                DataTable dt = ObjLedger.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptVoucherView"].ImportRow(dr);
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
