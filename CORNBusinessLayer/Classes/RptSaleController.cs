using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Fetching Data Of Sales Reports
    /// </summary>
    public class RptSaleController
    {
        #region Constructor
        /// <summary>
        /// Constructor for RptSaleController
        /// </summary>
        public RptSaleController()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        #endregion
        #region Public Methods
        #region Customer List and Member List
        public DataSet SelectPrincipalWiseCustomer(int p_Distributor_ID, int p_Type_ID, int p_User_ID, int p_IsActive, bool p_isExcelQuery = false)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectPrincipalWiseCustomer ObjPrint = new UspSelectPrincipalWiseCustomer();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.TYPE_ID = p_Type_ID;
                ObjPrint.USER_ID = p_User_ID;
                ObjPrint.IS_ACTIVE = p_IsActive;
                ObjPrint.ForExcel = p_isExcelQuery;
                DataTable dt = ObjPrint.ExecuteTable();
                if (p_isExcelQuery == false)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["UspSelectPrincipalWiseCustomer"].ImportRow(dr);
                    }
                }
                else
                {
                    dt.TableName = "MembersListExcel";
                    ds.Tables.Add(dt.Copy());
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
        #region Daily Sales Report Month Restricted && Daily Sales Report
        public DataSet SelectSalePersonDSR(string pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId,int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspPrintSaleForceDSRNew objPrint = new UspPrintSaleForceDSRNew();
                Reports.DsReport2 ds = new Reports.DsReport2();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                objPrint.TYPE_ID = p_TYPE_ID;

                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspPrintSaleForceDSR"].ImportRow(dr);
                }

                uspGetServiceWiseCoverTables mLedgerSub = new uspGetServiceWiseCoverTables();

                mLedgerSub.Connection = mConnection;
                mLedgerSub.Rpt_Type = 1;
                mLedgerSub.DISTRIBUTOR_ID = pDistributorId;
                mLedgerSub.FROMDATE = pFromDate;
                mLedgerSub.TODATE = pToDate;
                mLedgerSub.USER_ID = pUserId;
                DataTable dt2 = mLedgerSub.ExecuteTable();

                foreach (DataRow dr in dt2.Rows)
                {
                    ds.Tables["uspGetServiceWiseCoverTables"].ImportRow(dr);
                }

                uspGetDailyDeliveryData mDelivery = new uspGetDailyDeliveryData();

                mDelivery.Connection = mConnection;
                mDelivery.USER_ID= pUserId;
                mDelivery.DISTRIBUTOR_ID = pDistributorId;
                mDelivery.FROMDATE = pFromDate;
                mDelivery.TODATE = pToDate;
                DataTable dtDelivery = mDelivery.ExecuteTable();

                foreach (DataRow dr in dtDelivery.Rows)
                {
                    ds.Tables["uspGetDailyDeliveryData"].ImportRow(dr);
                }

                UspCustomerLedger customerLedger = new UspCustomerLedger();

                customerLedger.Connection = mConnection;
                customerLedger.Distributor_id = pDistributorId;
                customerLedger.From_Date = pFromDate;
                customerLedger.To_Date = pToDate;
                customerLedger.TypeID = 3;
                DataTable dtCustomerLedger = customerLedger.ExecuteTable();

                foreach (DataRow dr in dtCustomerLedger.Rows)
                {
                    ds.Tables["RptVoucherView"].ImportRow(dr);
                }

                uspGSTPaymentModeBreakup gstBreakup = new uspGSTPaymentModeBreakup();
                gstBreakup.Connection = mConnection;
                gstBreakup.DISTRIBUTOR_ID = pDistributorId;
                gstBreakup.FROM_DATE= pFromDate;
                gstBreakup.TO_DATE = pToDate;
                gstBreakup.TYPE_ID = p_TYPE_ID;
                DataTable dtgstBreakup = gstBreakup.ExecuteTable();

                foreach (DataRow dr in dtgstBreakup.Rows)
                {
                    ds.Tables["dtDailySalesGSTBreakup"].ImportRow(dr);
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
        #region Daily Sales Report Month Restricted && Daily Sales Report Date Wise
        public DataSet PrintDailySalesReportDateWise(string pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId, int pTypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spPrintDailySalesReportDateWise objPrint = new spPrintDailySalesReportDateWise();
                Reports.DsReport2 ds = new Reports.DsReport2();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                objPrint.TYPE_ID = pTypeId;
                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spPrintDailySalesReportDateWise"].ImportRow(dr);
                }

                uspGSTPaymentModeBreakup gstBreakup = new uspGSTPaymentModeBreakup();
                gstBreakup.Connection = mConnection;
                gstBreakup.DISTRIBUTOR_ID = pDistributorId;
                gstBreakup.FROM_DATE = pFromDate;
                gstBreakup.TO_DATE = pToDate;
                gstBreakup.TYPE_ID = 1;
                DataTable dtgstBreakup = gstBreakup.ExecuteTable();

                foreach (DataRow dr in dtgstBreakup.Rows)
                {
                    ds.Tables["dtDailySalesGSTBreakup"].ImportRow(dr);
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
        public DataSet SalesReportDateWise(string pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spPrintDailySalesReportDateWise objPrint = new spPrintDailySalesReportDateWise();
                Reports.DsReport2 ds = new Reports.DsReport2();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                DataTable dt = objPrint.ExecuteTableForDateWiseRpt();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["DateWiseSaleSummary"].ImportRow(dr);
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
        #region Order Taker DSR Report
        public DataSet SelectBookerDSR(int pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId, int p_TYPE_ID, int p_Booker_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspPrintBookerDSR objPrint = new UspPrintBookerDSR();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                //objPrint.TYPE_ID = p_TYPE_ID;
                objPrint.BOOKER_ID = p_Booker_ID;
                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspPrintBookerDSR"].ImportRow(dr);
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
        #region Order Taker DSR Report Invoice Wise
        public DataSet SelectBookerInvoiceWiseDSR(int pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId, int p_TYPE_ID, int p_Booker_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspPrintBookerWiseInvoiceDetail objPrint = new UspPrintBookerWiseInvoiceDetail();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                //objPrint.TYPE_ID = p_TYPE_ID;
                objPrint.BOOKER_ID = p_Booker_ID;
                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspPrintSaleForceDSR"].ImportRow(dr);
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
        #region Order Taker DSR Report Item Wise
        public DataSet SelectBookerSkuWiseDSR(int pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId, int p_TYPE_ID, int p_Booker_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspPrintBookerSkuDetail objPrint = new UspPrintBookerSkuDetail();
                Reports.LatestDataSet ds = new Reports.LatestDataSet();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                //objPrint.TYPE_ID = p_TYPE_ID;
                objPrint.BOOKER_ID = p_Booker_ID;
                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspPrintBookerSkuDetail"].ImportRow(dr);
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
        #region Void Order & Void Invoice Report
        public DataSet PrintVoidOrderSaleReport(string pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId, int pTypeId, int pSubTypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spVoidOrderSaleReport objPrint = new spVoidOrderSaleReport();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                objPrint.TYPE_ID = pTypeId;
                objPrint.SubType_ID = pSubTypeId;
                DataTable dt = objPrint.ExecuteTable();

                if (pSubTypeId == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["UspBillWiseInvoiceReport"].ImportRow(dr);
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["spVoidOrderSaleReport"].ImportRow(dr);
                    }
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
        public DataTable PrintVoidOrderSaleReportExcel(string pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId, int pTypeId, int pSubTypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spVoidOrderSaleReport objPrint = new spVoidOrderSaleReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                objPrint.TYPE_ID = pTypeId;
                objPrint.SubType_ID = pSubTypeId;
                DataTable dt = objPrint.ExecuteTable();

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
        #endregion
        #region Reports on POS Form
        public DataTable SelectSaleReport(int p_Distributor_Id, int p_UserId, DateTime p_StartDate, int pTypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                sp_SelectSaleReport mOrder = new sp_SelectSaleReport
                {
                    Connection = mConnection,
                    DISTRIBUTOR_ID = p_Distributor_Id,
                    typeId = pTypeId,
                    UserID = p_UserId,
                    Date = p_StartDate
                };
                return mOrder.ExecuteTable();
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
        public DataTable SelectSaleReport(int p_Distributor_Id, int p_UserId, DateTime p_StartDate, int pTypeId,bool p_HiddenReport)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                sp_SelectSaleReport mOrder = new sp_SelectSaleReport
                {
                    Connection = mConnection,
                    DISTRIBUTOR_ID = p_Distributor_Id,
                    typeId = pTypeId,
                    UserID = p_UserId,
                    Date = p_StartDate,
                    HiddenReport = p_HiddenReport
                };
                return mOrder.ExecuteTable();
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
        public DataTable SelectSaleReport(int p_Distributor_Id, int p_UserId, DateTime p_StartDate, DateTime p_EndDate, int pTypeId, bool p_HiddenReport)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                sp_SelectSaleReport mOrder = new sp_SelectSaleReport
                {
                    Connection = mConnection,
                    DISTRIBUTOR_ID = p_Distributor_Id,
                    typeId = pTypeId,
                    UserID = p_UserId,
                    Date = p_StartDate,
                    EndDate = p_EndDate,
                    HiddenReport = p_HiddenReport
                };
                return mOrder.ExecuteTable();
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
        public DataSet SelectSaleReportDataSet(int p_Distributor_Id, int p_UserId, DateTime p_StartDate, int pTypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                sp_SelectSaleReport mOrder = new sp_SelectSaleReport
                {
                    Connection = mConnection,
                    DISTRIBUTOR_ID = p_Distributor_Id,
                    typeId = pTypeId,
                    UserID = p_UserId,
                    Date = p_StartDate
                };
                return mOrder.ExecuteDataSet();
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
        public DataSet SelectSaleReportDataSet(int p_Distributor_Id, int p_UserId, DateTime p_StartDate, int pTypeId,bool p_HiddenReport)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                sp_SelectSaleReport mOrder = new sp_SelectSaleReport
                {
                    Connection = mConnection,
                    DISTRIBUTOR_ID = p_Distributor_Id,
                    typeId = pTypeId,
                    UserID = p_UserId,
                    Date = p_StartDate,
                    HiddenReport = p_HiddenReport
                };
                return mOrder.ExecuteDataSet();
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
        public DataSet SelectSaleReportDataSet(int p_Distributor_Id, int p_UserId, DateTime p_StartDate,DateTime p_EndDate, int pTypeId, bool p_HiddenReport)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                sp_SelectSaleReport mOrder = new sp_SelectSaleReport
                {
                    Connection = mConnection,
                    DISTRIBUTOR_ID = p_Distributor_Id,
                    typeId = pTypeId,
                    UserID = p_UserId,
                    Date = p_StartDate,
                    EndDate = p_EndDate,
                    HiddenReport = p_HiddenReport
                };
                return mOrder.ExecuteDataSet();
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
        #region Section Wise Sale && Section Wise Purchase && Item Wise Sale && Item Contribution
        public DataSet GetRegionItemSaleDetail(int pDistributorId, int pCategory, int pUserId, DateTime pFrmDate, DateTime pToDate, int p_RptType, string p_SECTIONIDs)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var ds = new Reports.dsSalesPurchaseRegister();
                var objRegionwise = new uspRegionWiseItemSales
                {
                    Connection = mConnection,
                    CATEGORY_ID = pCategory,
                    distributor_id = pDistributorId,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    RptType = p_RptType,
                    SECTIONIDs = p_SECTIONIDs,
                    user_id = pUserId

                };

                DataTable dt = objRegionwise.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    if (p_RptType == 1)
                    {
                        ds.Tables["uspRegionWiseItemSales1"].ImportRow(dr);
                    }
                    else
                    {
                        ds.Tables["uspRegionWiseItemSales"].ImportRow(dr);
                    }
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        public DataSet GetMonthWiseContribution(int pDistributorId, int pCategory, int pUserId, DateTime pFrmDate, DateTime pToDate, int p_RptType, string p_SECTIONIDs)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var ds = new Reports.dsSalesPurchaseRegister();
                var objRegionwise = new uspRegionWiseItemSales
                {
                    Connection = mConnection,
                    CATEGORY_ID = pCategory,
                    distributor_id = pDistributorId,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    RptType = p_RptType,
                    SECTIONIDs = p_SECTIONIDs,
                    user_id = pUserId

                };

                DataTable dt = objRegionwise.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspRegionWiseItemSales2"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        #region Item Wise Sale
        public DataSet GetItemWiseSales(string pDISTRIBUTOR_IDs,string pSKU_IDs,string pServiceTypeIDs, int pCashierID, DateTime pFrmDate, DateTime pToDate, int p_RptType)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var ds = new Reports.dsSalesPurchaseRegister();
                var objRegionwise = new uspItemWiseSales
                {
                    Connection = mConnection,
                    DISTRIBUTOR_IDs = pDISTRIBUTOR_IDs,
                    SKU_IDs= pSKU_IDs,
                    ServiceTypeIDs = pServiceTypeIDs,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    RptType = p_RptType,
                    CashierID = pCashierID
                };

                DataTable dt = objRegionwise.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    if (p_RptType == 1)
                    {
                        ds.Tables["uspRegionWiseItemSales1"].ImportRow(dr);
                    }
                    else
                    {
                        ds.Tables["uspRegionWiseItemSales"].ImportRow(dr);
                    }
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        public DataTable GetdtItemWiseSales(string pDISTRIBUTOR_IDs, string pSKU_IDs, DateTime pFrmDate, DateTime pToDate, int p_RptType)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var objRegionwise = new uspItemWiseSales
                {
                    Connection = mConnection,
                    DISTRIBUTOR_IDs = pDISTRIBUTOR_IDs,
                    SKU_IDs = pSKU_IDs,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    RptType = p_RptType
                };

                DataTable dt = objRegionwise.ExecuteTable();                
                return dt;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        #region Category Wise Sales
        public DataSet GetCategorySales(string p_Distributor_Ids,string p_ServiceTypeIDs, DateTime p_StartDate,DateTime p_EndDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var ds = new Reports.dsSalesPurchaseRegister();
                uspGetCategorySales mOrder = new uspGetCategorySales
                {
                    Connection = mConnection,
                    DISTRIBUTOR_IDs = p_Distributor_Ids,
                    ServiceTypeIDs = p_ServiceTypeIDs,
                    FromDate = p_StartDate,
                    ToDate = p_EndDate
                };
                DataTable dt= mOrder.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetCategorySales"].ImportRow(dr);
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
        #region Hourly Sales Report && Meal Time Wise Sales Report
        public DataSet GethourlySaleDetail(int pDistributorId, int pCategory, int pUserId, DateTime pFrmDate, DateTime pToDate, string pHour, int pType)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var ds = new Reports.dsSalesPurchaseRegister();
                var objRegionwise = new uspHourlyItemSales
                {
                    Connection = mConnection,
                    CATEGORY_ID = pCategory,
                    distributor_id = pDistributorId,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    Type = pType,
                    user_id = pUserId,
                    Hour = pHour
                };

                DataTable dt = objRegionwise.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspHourlyItemSales"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        #region Horly Sales Summary Report
        public DataSet GethourlySaleDetailSummary(int pDistributorId, int pCategory, int pUserId, DateTime pFrmDate, DateTime pToDate, string pHour, int pType)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var ds = new Reports.DsReport2();
                var objRegionwise = new uspHourlyItemSalesSummary
                {
                    Connection = mConnection,
                    CATEGORY_ID = pCategory,
                    distributor_id = pDistributorId,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    Type = pType,
                    Hour = pHour,
                    user_id = pUserId
                };

                DataTable dt = objRegionwise.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspHourlyItemSalesSummary"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        public DataSet GethourlySalesItemWise(int pDistributorId, int pUserId, DateTime pFrmDate, DateTime pToDate, string pSKU_ID, int pType)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var ds = new Reports.DsReport2();
                var objRegionwise = new uspHourlyItemWiseSales
                {
                    Connection = mConnection,
                    distributor_id = pDistributorId,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    Type = pType,
                    SKU_ID = pSKU_ID,
                    user_id = pUserId
                };

                DataTable dt = objRegionwise.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspHourlyItemWiseSales"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        public DataSet GetPRASynchSaleDetail(int pDistributorId, int pCategory, int pUserId,DateTime pFrmDate, DateTime pToDate, int pType, int pRecordType)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var ds = new Reports.dsSalesPurchaseRegister();
                var objRegionwise = new uspPraSyncReport
                {
                    Connection = mConnection,
                    CATEGORY_ID = pCategory,
                    distributor_id = pDistributorId,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    Type = pType,
                    user_id = pUserId,
                    RecordType = pRecordType
                };

                DataTable dt = objRegionwise.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspSelectRollBackDocument"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        /// Gets Data For SKU Price List Report
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Catagory_ID">Category</param>
        /// <param name="p_DistributorId">Location</param>
        /// <returns>DataSet</returns>
        public DataSet PriceList(int p_Principal_ID, int p_Catagory_ID, string p_DistributorId, int p_status,int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DsReport ds = new Reports.DsReport();

                rptPriceList mPriceList = new rptPriceList();

                mPriceList.Connection = mConnection;
                mPriceList.PRINCIPAL_ID = p_Principal_ID;
                mPriceList.CATEGORY_ID = p_Catagory_ID;
                mPriceList.DISTRIBUTOR_ID = p_DistributorId;
                mPriceList.STATUS = p_status;
                mPriceList.TYPE_ID = p_TYPE_ID;
                DataTable DT = mPriceList.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["PriceList"].ImportRow(dr);
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
        public DataSet SelectRptGrossProfit(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspGrossProfitReport ObjPrint = new UspGrossProfitReport();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspGrossProfitReport2"].ImportRow(dr);
                }

                uspGetExpenseDetail Expense = new uspGetExpenseDetail();
                Expense.Connection = mConnection;
                Expense.DISTRIBUTOR_ID = p_Distributor_ID;
                Expense.PRINCIPAL_ID = p_Principal_Id;
                Expense.FROM_DATE = p_FromDate;
                Expense.TO_DATE = p_To_Date;
                DataTable dtPro = Expense.ExecuteTable();

                foreach (DataRow dr in dtPro.Rows)
                {
                    ds.Tables["uspGetExpenseDetail"].ImportRow(dr);
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
        public DataSet SelectRptGrossProfitDelish(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspGrossProfitReportDelish ObjPrint = new UspGrossProfitReportDelish();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspGrossProfitReport2"].ImportRow(dr);
                }

                uspGetExpenseDetail Expense = new uspGetExpenseDetail();
                Expense.Connection = mConnection;
                Expense.DISTRIBUTOR_ID = p_Distributor_ID;
                Expense.PRINCIPAL_ID = p_Principal_Id;
                Expense.FROM_DATE = p_FromDate;
                Expense.TO_DATE = p_To_Date;
                DataTable dtPro = Expense.ExecuteTable();

                foreach (DataRow dr in dtPro.Rows)
                {
                    ds.Tables["uspGetExpenseDetail"].ImportRow(dr);
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
        public DataSet SelectRptGrossProfitBeirut(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspGrossProfitReportBeirut ObjPrint = new UspGrossProfitReportBeirut();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspGrossProfitReport2"].ImportRow(dr);
                }

                uspGetExpenseDetailBeirut Expense = new uspGetExpenseDetailBeirut();
                Expense.Connection = mConnection;
                Expense.DISTRIBUTOR_ID = p_Distributor_ID;
                Expense.PRINCIPAL_ID = p_Principal_Id;
                Expense.FROM_DATE = p_FromDate;
                Expense.TO_DATE = p_To_Date;
                DataTable dtPro = Expense.ExecuteTable();

                foreach (DataRow dr in dtPro.Rows)
                {
                    ds.Tables["uspGetExpenseDetail"].ImportRow(dr);
                }

                uspGetConsumptionAccountHeadWise Expense2 = new uspGetConsumptionAccountHeadWise();
                Expense2.Connection = mConnection;
                Expense2.DISTRIBUTOR_ID = p_Distributor_ID;
                Expense2.USER_ID = p_Principal_Id;
                Expense2.FROM_DATE = p_FromDate;
                Expense2.TO_DATE = p_To_Date;
                DataTable dtExpense = Expense2.ExecuteTable();

                foreach (DataRow dr in dtExpense.Rows)
                {
                    ds.Tables["uspGetConsumptionAccountHeadWise"].ImportRow(dr);
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
        public DataSet SelectRptGrossProfitCrossTab(string p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspGrossProfitReportCrossTab ObjPrint = new UspGrossProfitReportCrossTab();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspGrossProfitReport"].ImportRow(dr);
                }

                uspGetExpenseDetailCrossTab Expense = new uspGetExpenseDetailCrossTab();
                Expense.Connection = mConnection;
                Expense.DISTRIBUTOR_ID = p_Distributor_ID;
                Expense.PRINCIPAL_ID = p_Principal_Id;
                Expense.FROM_DATE = p_FromDate;
                Expense.TO_DATE = p_To_Date;
                DataTable dtPro = Expense.ExecuteTable();

                foreach (DataRow dr in dtPro.Rows)
                {
                    ds.Tables["uspGetExpenseDetailCrossTab"].ImportRow(dr);
                }

                uspGetOtherIncomCrossTab OtherIncome = new uspGetOtherIncomCrossTab();
                OtherIncome.Connection = mConnection;
                OtherIncome.DISTRIBUTOR_ID = p_Distributor_ID;
                OtherIncome.FROM_DATE = p_FromDate;
                OtherIncome.TO_DATE = p_To_Date;
                DataTable dtOtherIncome = OtherIncome.ExecuteTable();

                foreach (DataRow dr in dtOtherIncome.Rows)
                {
                    ds.Tables["uspGetOtherIncomCrossTab"].ImportRow(dr);
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

        public DataSet SelectRptGrossProfitCrossTabAKFood(string p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspGrossProfitReportCrossTabAKFood ObjPrint = new UspGrossProfitReportCrossTabAKFood();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["UspGrossProfitReport"].ImportRow(dr);
                }

                uspGetExpenseDetailCrossTab Expense = new uspGetExpenseDetailCrossTab();
                Expense.Connection = mConnection;
                Expense.DISTRIBUTOR_ID = p_Distributor_ID;
                Expense.PRINCIPAL_ID = p_Principal_Id;
                Expense.FROM_DATE = p_FromDate;
                Expense.TO_DATE = p_To_Date;
                DataTable dtPro = Expense.ExecuteTable();

                foreach (DataRow dr in dtPro.Rows)
                {
                    ds.Tables["uspGetExpenseDetailCrossTab"].ImportRow(dr);
                }

                uspGetOtherIncomCrossTab OtherIncome = new uspGetOtherIncomCrossTab();
                OtherIncome.Connection = mConnection;
                OtherIncome.DISTRIBUTOR_ID = p_Distributor_ID;
                OtherIncome.FROM_DATE = p_FromDate;
                OtherIncome.TO_DATE = p_To_Date;
                DataTable dtOtherIncome = OtherIncome.ExecuteTable();

                foreach (DataRow dr in dtOtherIncome.Rows)
                {
                    ds.Tables["uspGetOtherIncomCrossTab"].ImportRow(dr);
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
        public DataSet GetDailyReconciliation(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();

                uspGetDailyReconciliation Expense = new uspGetDailyReconciliation();
                Expense.Connection = mConnection;
                Expense.DISTRIBUTOR_ID = p_Distributor_ID;
                Expense.PRINCIPAL_ID = p_Principal_Id;
                Expense.FROM_DATE = p_FromDate;
                Expense.TO_DATE = p_To_Date;
                DataTable dtPro = Expense.ExecuteTable();

                foreach (DataRow dr in dtPro.Rows)
                {
                    ds.Tables["uspGetExpenseDetail"].ImportRow(dr);
                }

                uspGetThirdPartySale Expense2 = new uspGetThirdPartySale();
                Expense2.Connection = mConnection;
                Expense2.DISTRIBUTOR_ID = p_Distributor_ID;
                Expense2.PRINCIPAL_ID = p_Principal_Id;
                Expense2.FROM_DATE = p_FromDate;
                Expense2.TO_DATE = p_To_Date;
                DataSet dtPro2 = Expense2.ExecuteTable();

                foreach (DataRow dr in dtPro2.Tables[0].Rows)
                {
                    ds.Tables["uspGetThirdPartySale"].ImportRow(dr);
                }
                foreach (DataRow dr in dtPro2.Tables[1].Rows)
                {
                    ds.Tables["uspGetDailyReconciliation"].ImportRow(dr);
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
        /// Gets Data For Monthly Sale Report (Trade Price And Purchase Price)
        /// </summary>
        /// <param name="P_DateType">DateType</param>
        /// <param name="P_PrincipalId">Principal</param>
        /// <param name="P_DistributorId">Location</param>
        /// <param name="P_frmDate">DateFrom</param>
        /// <param name="P_toDate">DateTo</param>
        /// <param name="p_UserId">User</param>
        /// <param name="p_Column">ReprotFor</param>
        /// <param name="p_PriceType">PriceType</param>
        /// <returns>DataSet</returns>
        public DataSet GetDistributorReconcilation(byte P_DateType, int P_PrincipalId, int P_DistributorId, DateTime P_frmDate, DateTime P_toDate, int p_UserId, byte p_Column, byte p_PriceType)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                DataTable DT;
                UspPrintSaleAnalysisSummary obj_regionwise = new UspPrintSaleAnalysisSummary();
                obj_regionwise.Connection = mConnection;
                obj_regionwise.DATETYPE = P_DateType;
                obj_regionwise.DISTRIBUTOR_ID = P_DistributorId;
                obj_regionwise.PRINCIPAL_ID = p_UserId;
                obj_regionwise.FROM_DATE = P_frmDate;
                obj_regionwise.TO_DATE = P_toDate;
                obj_regionwise.COLUMN = p_Column;
                DT = obj_regionwise.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptMonthSaleValues"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        /// Gets Data For Monthly Sale Report (Trade Price And Purchase Price)
        /// </summary>
        /// <param name="P_DateType">DateType</param>
        /// <param name="P_PrincipalId">Principal</param>
        /// <param name="P_DistributorId">Location</param>
        /// <param name="P_frmDate">DateFrom</param>
        /// <param name="P_toDate">DateTo</param>
        /// <param name="p_UserId">User</param>
        /// <param name="p_Column">ReprotFor</param>
        /// <param name="p_PriceType">PriceType</param>
        /// <returns>DataSet</returns>
        public DataSet GetDistributorReconcilation2(byte P_DateType, int P_PrincipalId, int P_DistributorId, DateTime P_frmDate, DateTime P_toDate, int p_UserId, byte p_Column, byte p_PriceType,int p_Month)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                DataTable DT;

                UspPrintSaleAnalysisSummary obj_regionwise = new UspPrintSaleAnalysisSummary();
                obj_regionwise.Connection = mConnection;
                obj_regionwise.DATETYPE = P_DateType;
                obj_regionwise.DISTRIBUTOR_ID = P_DistributorId;
                obj_regionwise.PRINCIPAL_ID = p_UserId;
                obj_regionwise.FROM_DATE = P_frmDate;
                obj_regionwise.TO_DATE = P_toDate;
                obj_regionwise.COLUMN = p_Column;
                obj_regionwise.MONTH = p_Month;
                DT = obj_regionwise.ExecuteTable();


                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptMonthSaleValues"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        #region Added by Hazrat Ali
        
        /// <summary>
        /// Gets Data For User Login History Report
        /// </summary>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <param name="p_User_ID">User</param>
        /// <param name="p_User_Log_ID">UserLog</param>
        /// <returns>DataSet</returns>
        public DataSet GetUserLoginDetail(DateTime p_FromDate, DateTime p_To_Date, string p_User_IDs, long p_User_Log_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                uspGetUserLoginDetail ObjLogin = new uspGetUserLoginDetail();
                ObjLogin.Connection = mConnection;
                ObjLogin.DateFrom = p_FromDate;
                ObjLogin.DateTo = p_To_Date;
                ObjLogin.USER_IDs = p_User_IDs;
                ObjLogin.User_Log_ID = p_User_Log_ID;
                DataTable dt = ObjLogin.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetUserLoginDetail"].ImportRow(dr);
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
        public DataSet GetDashboardFile(int p_intMODULE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetDashboardFile mDashboardFile = new uspGetDashboardFile();
                mDashboardFile.Connection = mConnection;
                mDashboardFile.intMODULE_ID = p_intMODULE_ID;
                return mDashboardFile.ExecuteDataSet();
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
        public DataSet GetDashboardData(DateTime p_DATE_TO, string p_DISTRIBUTOR_IDs)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetDashboardData mDashboard = new uspGetDashboardData();
                mDashboard.Connection = mConnection;
                mDashboard.DATE_TO = p_DATE_TO;
                mDashboard.DISTRIBUTOR_IDs = p_DISTRIBUTOR_IDs;

                return mDashboard.ExecuteDataSet();


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
        public DataSet SelectClosedShift(int pUserId, DateTime pDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                USPCLOSEDSHIFTS objPrint = new USPCLOSEDSHIFTS();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DATE = pDate;
                objPrint.USER_ID = pUserId;

                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["USPCLOSEDSHIFTS"].ImportRow(dr);
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
        public DataSet GetDayEndData(int pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetDayEndDate objPrint = new uspGetDayEndDate();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;

                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetDayEndDate"].ImportRow(dr);
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
        public DataSet GetItemCanelDeleteLog(int pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetItemCancelDeleteLog objPrint = new uspGetItemCancelDeleteLog();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                objPrint.TYPE_ID = p_TYPE_ID;

                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetItemCancelDeleteLog"].ImportRow(dr);
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
        public DataTable GetSalesRawData(int pDistributorId, int pUserId, DateTime pFrmDate, DateTime pToDate, int p_RptType)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var objRegionwise = new uspGetSalesRawData
                {
                    Connection = mConnection,
                    DISTRIBUTOR_ID = pDistributorId,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    RptType = p_RptType,
                    USER_ID = pUserId

                };

                DataTable dt = objRegionwise.ExecuteTable();
                return dt;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        public DataSet GetComplimentaryItemReport(int p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_ToDate, int p_type)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spDateWiseItemConsumption objPrint = new spDateWiseItemConsumption();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                objPrint.FROM_DATE = p_FromDate;
                objPrint.TO_DATE = p_ToDate;
                objPrint.TYPE_ID = p_type;
                DataTable dt = objPrint.ExecuteTableForComplimentaryItemReport();
                if (p_type == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["spDateWiseItemConsumption"].ImportRow(dr);
                    }
                }
                else if (p_type == 2)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["RptPurchaseDocument"].ImportRow(dr);
                    }
                }
                else if (p_type == 3)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["RptComplimentaryItem"].ImportRow(dr);
                    }
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
        public DataSet SelectDateWiseSaleReport(int p_Distributor_Id, int p_UserId, DateTime p_StartDate,DateTime p_EndDate, int p_SKU_ID, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                sp_SelectSaleReport1 mOrder = new sp_SelectSaleReport1();
                mOrder.Connection = mConnection;
                mOrder.DISTRIBUTOR_ID = p_Distributor_Id;
                mOrder.UserID = p_UserId;
                mOrder.STARTDATE = p_StartDate;
                mOrder.ENDDATE = p_EndDate;
                mOrder.SHIFT_ID = p_SKU_ID;
                mOrder.TYPE = p_TypeId;
                DataTable dt = mOrder.ExecuteTableForDateWiseSaleRpt();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spDateWiseSale"].ImportRow(dr);
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
        #region Item Sale Margin
        public DataSet GetItemSaleMargin(int pDistributorId, int pCategory,int pUserId, DateTime pFrmDate, DateTime pToDate, int p_RptType)
        {

            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                var ds = new Reports.dsSalesPurchaseRegister();
                var objRegionwise = new uspRegionWiseItemSales
                {
                    Connection = mConnection,
                    CATEGORY_ID = pCategory,
                    distributor_id = pDistributorId,
                    FROM_DATE = pFrmDate,
                    TO_DATE = pToDate,
                    RptType = p_RptType,
                    user_id = pUserId

                };

                DataTable dt = objRegionwise.ExecuteTableForItemSaleMargin();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspRegionWiseItemSales"].ImportRow(dr);
                }
                return ds;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        public DataSet SelectItemWiseCreditSale(string pDistributorId, DateTime pFromDate, DateTime pToDate, int pUserId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spPrintDailySalesReportDateWise objPrint = new spPrintDailySalesReportDateWise();
                Reports.DsReport2 ds = new Reports.DsReport2();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                DataTable dt = objPrint.ExecuteTableForItemWiseCreditSale();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["rptItemWiseCreditSale"].ImportRow(dr);
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
        public DataSet SelectSaleInvoiceReport(int p_UserID, string p_Sale_Invoice_Id,int p_DistributorId, DateTime p_From_Date, DateTime p_ToDate, int p_OrderBookerID,Byte p_showAll, Byte p_PaymentModeID, long p_customer_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                UspBillWiseInvoiceReportNew mInvoice = new UspBillWiseInvoiceReportNew();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                mInvoice.Connection = mConnection;

                mInvoice.PRINCIPAL_ID = p_UserID;
                mInvoice.DISTRIBUTOR_ID = p_DistributorId;
                mInvoice.SALE_INVOICE_ID = p_Sale_Invoice_Id;
                mInvoice.FROM_DATE = p_From_Date;
                mInvoice.TO_DATE = p_ToDate;
                mInvoice.ShowAll = p_showAll;
                mInvoice.ORDERBOOKER_ID = p_OrderBookerID;
                mInvoice.PaymentModeID = p_PaymentModeID;
                mInvoice.CUSTOMER_ID = p_customer_Id;
                DataTable DT = mInvoice.ExecuteTableForSaleInvoicePOS();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptFranchiseSaleInvoice"].ImportRow(dr);
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
        public DataSet GetBankDiscountReport(string p_BankDiscountIDs,int p_TypeID, string p_LocationID, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspGetBankDiscount mInvoice = new uspGetBankDiscount();
                CORNBusinessLayer.Reports.DSReportNew ds = new CORNBusinessLayer.Reports.DSReportNew();
                mInvoice.Connection = mConnection;

                mInvoice.TypeID = p_TypeID;
                mInvoice.BankDiscountID = p_BankDiscountIDs;
                mInvoice.LocationID = p_LocationID;
                mInvoice.FromDate = p_FromDate;
                mInvoice.ToDate = p_ToDate;
                DataTable DT = mInvoice.ExecuteTable();
                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["dtBankDiscount"].ImportRow(dr);
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
        public DataTable GetBankDiscountReportExcel(string p_BankDiscountIDs, int p_TypeID, string p_LocationID, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspGetBankDiscount mInvoice = new uspGetBankDiscount();
                CORNBusinessLayer.Reports.DSReportNew ds = new CORNBusinessLayer.Reports.DSReportNew();
                mInvoice.Connection = mConnection;

                mInvoice.TypeID = p_TypeID;
                mInvoice.BankDiscountID = p_BankDiscountIDs;
                mInvoice.LocationID = p_LocationID;
                mInvoice.FromDate = p_FromDate;
                mInvoice.ToDate = p_ToDate;
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
        public DataSet GetNetCashSalevsCashSubmitted(string pDistributorId, int pUserId, DateTime pFromDate, DateTime pToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spPrintDailySalesReportDateWise objPrint = new spPrintDailySalesReportDateWise();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_IDs = pDistributorId;
                objPrint.FROM_DATE = pFromDate;
                objPrint.TO_DATE = pToDate;
                objPrint.USER_ID = pUserId;
                DataTable dt = objPrint.ExecuteTableForNetCashSalevsCashSubmitted();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["USPCLOSEDSHIFTS"].ImportRow(dr);
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
        public DataSet GetCallCenterInvoices(string p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_ToDate, int p_PAYMENT_MODE_ID,long p_CUSTOMER_ID,int p_DELIVERYMAN_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetCallCenterInvoices objPrint = new uspGetCallCenterInvoices();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                objPrint.FROM_DATE = p_FromDate;
                objPrint.TO_DATE = p_ToDate;
                objPrint.PAYMENT_MODE_ID = p_PAYMENT_MODE_ID;
                objPrint.CUSTOMER_ID = p_CUSTOMER_ID;
                objPrint.DELIVERYMAN_ID = p_DELIVERYMAN_ID;

                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetCallCenterInvoices"].ImportRow(dr);
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
        public DataSet GetKDSOrderTim(int p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetKDSOrderTime objPrint = new uspGetKDSOrderTime();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                objPrint.FROM_DATE = p_FromDate;
                objPrint.TO_DATE = p_ToDate;
                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetKDSOrderTime"].ImportRow(dr);
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
        #region Table Reservation Report
        public DataSet GetTableReservationReport(int p_DISTRIBUTOR_ID,long p_CustomerID,int p_BookingSlot, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetTableReservation objPrint = new uspGetTableReservation();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                objPrint.Connection = mConnection;
                objPrint.DistributorID = p_DISTRIBUTOR_ID;
                objPrint.BookingSlot = p_BookingSlot;
                objPrint.CustomerID = p_CustomerID;
                objPrint.FromDate = p_FromDate;
                objPrint.ToDate = p_ToDate;
                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetTableReservation"].ImportRow(dr);
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