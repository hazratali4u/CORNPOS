using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Fetching Data Of Account Reports
    /// </summary>
    public class RptAccountController
    {
        #region Constructor

        /// <summary>
        /// Constructor for RptAccountController
        /// </summary>
        public RptAccountController()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

        #region Public Methods

        /// <summary>
        /// Gets Data For Voucher View Report
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_VoucherNo">Voucher</param>
        /// <param name="p_Voucher_Type">Type</param>
        /// <returns>DataSet</returns>
        public DataSet SelectUnpostVoucherForPrint(int p_Distributor_ID, string p_VoucherNo, int p_Voucher_Type)
        {
            IDbConnection mConnection = null;
            try
            {
                LedgerController LControler = new LedgerController();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                DataTable dt = LControler.SelectUnPostLedger(p_VoucherNo, p_Distributor_ID, p_Voucher_Type);
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
        public DataSet SelectUnpostVoucherForPrint(int p_Distributor_ID, int p_Voucher_Type,DateTime p_dtFrom,DateTime p_dtTo)
        {
            IDbConnection mConnection = null;
            try
            {
                LedgerController LControler = new LedgerController();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                DataTable dt = LControler.SelectUnPostLedger(p_Distributor_ID, p_Voucher_Type,p_dtFrom,p_dtTo);
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
        public DataTable SelectUnpostVoucherForPrintExcel(int p_Distributor_ID, int p_Voucher_Type, DateTime p_dtFrom, DateTime p_dtTo)
        {
            IDbConnection mConnection = null;
            try
            {
                LedgerController LControler = new LedgerController();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                DataTable dt = LControler.SelectUnPostLedger(p_Distributor_ID, p_Voucher_Type, p_dtFrom, p_dtTo);                
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
        public DataSet GetVoucherPopup(string p_UniqueID)
        {
            IDbConnection mConnection = null;
            try
            {
                LedgerController LControler = new LedgerController();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                DataTable dt = LControler.GetVoucherPopup(p_UniqueID);
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptVoucherViewUniqueID"].ImportRow(dr);
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
        public DataSet SelectUnpostVoucherForPrint(int p_Distributor_ID, string p_VoucherNo, int p_Voucher_Type, int pType)
        {
            IDbConnection mConnection = null;
            try
            {
                LedgerController LControler = new LedgerController();
                Reports.DsReport2 ds = new Reports.DsReport2();
                DataTable dt = LControler.SelectUnPostLedger(p_VoucherNo, p_Distributor_ID, p_Voucher_Type, pType);
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
        public DataSet GetUnpostVoucherForPrint(string p_Distributor_ID, string p_VoucherNo, int p_Voucher_Type, int pType,string p_PayeeName)
        {
            IDbConnection mConnection = null;
            try
            {
                LedgerController LControler = new LedgerController();
                Reports.DsReport2 ds = new Reports.DsReport2();
                DataTable dt = LControler.GetUnPostLedger(p_VoucherNo, p_Distributor_ID, p_Voucher_Type, pType, p_PayeeName);
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
        #region Petty Expense Report

        /// <summary>
        /// Gets Data For Petty Expense Report (Petty Expense Statament)
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <param name="p_USER_ID">User</param>
        /// <param name="p_ParentAccountId">ParentAccountHead</param>
        /// <returns>DataSet</returns>
        public DataSet SelectPetyCashStatment(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_USER_ID, int p_ParentAccountId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                RptPetyCashStatment ObjPrint = new RptPetyCashStatment();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.USER_ID = p_USER_ID;
                ObjPrint.ACCOUNT_PARENT_ID = p_ParentAccountId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["dbo_PetyCashSummary"].ImportRow(dr);
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
        /// Gets Data For Petty Expense Report (Petty Cash Statment)
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <param name="p_USER_ID">User</param>
        /// <returns>DataSet</returns>
        public DataSet SelectPetyCashSummary(int p_Distributor_ID, DateTime p_FromDate, DateTime p_To_Date, int p_USER_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspRptPettyCashSummery ObjPrint = new UspRptPettyCashSummery();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DistributorId = p_Distributor_ID;
                ObjPrint.FromDate = p_FromDate;
                ObjPrint.ToDate = p_To_Date;
                ObjPrint.USER_ID = p_USER_ID;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["PettyCashSummery"].ImportRow(dr);
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
        
        /// <summary>
        /// Gets Data For General Ledger Report
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Account_Head_ID">AccountHead</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="p_From_Date">DateFrom</param>
        /// <param name="p_ToDate">DateTo</param>
        /// <param name="p_Posted">Post</param>
        /// <returns>DataSet</returns>
        public DataSet GeneralLedger_View(int pUserID, long p_Account_Head_ID, string p_DistributorId, DateTime p_From_Date, DateTime p_ToDate, int p_Posted,string p_FROM_CODE,string p_TO_CODE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspLedgerReport mLedger = new UspLedgerReport();

                mLedger.Connection = mConnection;
                mLedger.USER_ID = pUserID;
                mLedger.DISTRIBUTOR_ID = p_DistributorId;
                mLedger.ACCOUNT_HEAD_ID = p_Account_Head_ID;
                mLedger.FROM_DATE = p_From_Date;
                mLedger.TO_DATE = p_ToDate;
                mLedger.POSTED = p_Posted;
                mLedger.FROM_CODE = p_FROM_CODE;
                mLedger.TO_CODE = p_TO_CODE;
                DataTable DT = mLedger.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptLedgerView"].ImportRow(dr);
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
        public DataSet GetCashPaymentVoucher(int p_DistributorId, DateTime p_From_Date, DateTime p_ToDate, long p_Account_Head_ID, int p_Voucher_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();

                spSelectCashPaymentVoucher mLedger = new spSelectCashPaymentVoucher();

                mLedger.Connection = mConnection;
                mLedger.DISTRIBUTOR_ID = p_DistributorId;
                mLedger.ACCOUNT_HEAD_ID = p_Account_Head_ID;
                mLedger.FROM_DATE = p_From_Date;
                mLedger.TO_DATE = p_ToDate;
                mLedger.USER_ID = p_Voucher_TYPE_ID;
                DataTable DT = mLedger.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["spSelectCashPaymentVoucher"].ImportRow(dr);
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
        /// Gets Data For Petty Expense Summary Report
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_StartDate">DateFrom</param>
        /// <param name="p_EndDate">DateTo</param>
        /// <param name="p_CatagoryIDS">Categories</param>
        /// <param name="p_ReportType">ReportType</param>
        /// <returns>DataSet</returns>
        public DataSet PrincipalWiseSale(int p_Principal_ID, int p_Distributor_ID, DateTime p_StartDate, DateTime p_EndDate, string p_CatagoryIDS, int p_ReportType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspPrincipalWiseExp mOutletwiseSale = new UspPrincipalWiseExp();
                mOutletwiseSale.Connection = mConnection;

                mOutletwiseSale.PRINCIPAL_ID = p_Principal_ID;
                mOutletwiseSale.DISTRIBUTOR_ID = p_Distributor_ID;
                mOutletwiseSale.FROM_DATE = p_StartDate;
                mOutletwiseSale.TO_DATE = p_EndDate;
                mOutletwiseSale.ACCOUNT_IDs = p_CatagoryIDS;
                mOutletwiseSale.TYPE_ID = p_ReportType;

                DataTable DT = mOutletwiseSale.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptPrincipalWiseExp"].ImportRow(dr);
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
        /// Gets Data For Trial Balance Report
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="p_Account_Type_ID">AccountType</param>
        /// <param name="p_From_Date">DateFrom</param>
        /// <param name="p_ToDate">DateTo</param>
        /// <param name="p_Level">Level</param>
        /// <param name="p_FromCode">CodeFrom</param>
        /// <param name="p_ToCode">CodeTo</param>
        /// <param name="p_Posted">Post</param>
        /// <returns>DataSet</returns>
        public DataSet TrialBalance(int p_Principal_ID, string p_DistributorId, int p_Account_Type_ID, DateTime p_From_Date, DateTime p_ToDate, int p_Level, string p_FromCode, string p_ToCode, int p_Posted,int p_USER_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspPrintTrialBalance mLedger = new UspPrintTrialBalance();

                mLedger.Connection = mConnection;
                mLedger.PRINCIPAL_ID = p_Principal_ID;
                mLedger.DISTRIBUTOR_ID = p_DistributorId;
                mLedger.ACCOUNT_TYPE_ID = p_Account_Type_ID;
                mLedger.FROM_DATE = p_From_Date;
                mLedger.TO_DATE = p_ToDate;
                mLedger.LEAVEL_ID = p_Level;
                mLedger.FROM_CODE = p_FromCode;
                mLedger.TO_CODE = p_ToCode;
                mLedger.POSTING = p_Posted;
                mLedger.USER_ID = p_USER_ID;
                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptTrialBalance"].ImportRow(dr);
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
        /// Gets Data For  Chart of Account Report
        /// </summary>
        /// <param name="p_account_category">Category</param>
        /// <param name="p_account_typeid">Type</param>
        /// <param name="p_accountsub_typeid">SubType</param>
        /// <param name="p_AccountDetail_TypeId">DetailType</param>
        /// <returns>DataSet</returns>
        public DataSet SelectRptChartofAccount(int p_account_category, int p_account_typeid, 
            int p_accountsub_typeid, int p_AccountDetail_TypeId, int p_DISTRIBUTOR_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspPrintChartofAccount ObjPrint = new UspPrintChartofAccount();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.account_category = p_account_category;
                ObjPrint.account_typeid = p_account_typeid;
                ObjPrint.accountsub_typeid = p_accountsub_typeid;
                ObjPrint.AccountDetail_TypeId = p_AccountDetail_TypeId;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                DataTable dt = ObjPrint.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["Account_Head_View"].ImportRow(dr);
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
        /// Gets Data For Gross Profit Report
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <returns>DataSet</returns>
        public DataSet GetBalanceSheet(int p_ACCOUNT_CATEGORY_ID, string p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetBalanceSheet ObjPrint = new uspGetBalanceSheet();
                Reports.DsReport2 ds = new Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.ACCOUNT_CATEGORY_ID = p_ACCOUNT_CATEGORY_ID;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataSet ds2 = ObjPrint.ExecuteTable();
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    ds.Tables["IncomeStatement"].ImportRow(dr);
                }

                foreach (DataRow dr in ds2.Tables[1].Rows)
                {
                    ds.Tables["IncomeStatement1"].ImportRow(dr);
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

        public DataSet GetGrossProfitItemWise(int p_Distributor_ID, int p_User_Id,string p_CategoryIDs, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetGrossProfitItemWise ObjPrint = new uspGetGrossProfitItemWise();
                CORNBusinessLayer.Reports.DSReportNew ds = new CORNBusinessLayer.Reports.DSReportNew();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.USER_ID = p_User_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.CATEGORY_ID = p_CategoryIDs;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["dtGrossProfitItemWise"].ImportRow(dr);
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
        public DataSet FinancialRptCocoRush(int p_DistributorId, DateTime p_From_Date, 
            DateTime p_ToDate, int p_USER_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspPrintTrialBalanceCocoRush mLedger = new UspPrintTrialBalanceCocoRush();

                mLedger.Connection = mConnection;
                mLedger.DISTRIBUTOR_ID = p_DistributorId;
                mLedger.FROM_DATE = p_From_Date;
                mLedger.TO_DATE = p_ToDate;
                mLedger.USER_ID = p_USER_ID;
                DataTable DT = mLedger.ExecuteTableFinancialRptCocoRush();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptTrialBalance"].ImportRow(dr);
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
        public DataSet DayBookRptCocoRush(int p_DistributorId, DateTime p_From_Date,
            DateTime p_ToDate, int p_USER_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspPrintTrialBalanceCocoRush mLedger = new UspPrintTrialBalanceCocoRush();

                mLedger.Connection = mConnection;
                mLedger.DISTRIBUTOR_ID = p_DistributorId;
                mLedger.FROM_DATE = p_From_Date;
                mLedger.TO_DATE = p_ToDate;
                mLedger.USER_ID = p_USER_ID;
                DataTable DT = mLedger.ExecuteTableDayBookRptCocoRush();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["spDayBookRptCocoRush"].ImportRow(dr);
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
        public DataSet GeneralLedger_ViewCashAndBank(int pUserID, int p_DistributorId, 
            DateTime p_From_Date, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspCashBankBook mLedger = new UspCashBankBook();

                mLedger.Connection = mConnection;
                mLedger.USER_ID = pUserID;
                mLedger.DISTRIBUTOR_ID = p_DistributorId;
                mLedger.FROM_DATE = p_From_Date;
                mLedger.TO_DATE = p_ToDate;
                DataTable DT = mLedger.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptLedgerView"].ImportRow(dr);
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