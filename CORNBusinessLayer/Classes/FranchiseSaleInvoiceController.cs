using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System.IO;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Purchase, TranferOut, Purchase Return, TranferIn And Damage Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Purchase, TranferOut, Purchase Return, TranferIn And Damage
    /// </item>
    /// <term>
    /// Update Purchase, TranferOut, Purchase Return, TranferIn And Damage
    /// </term>
    /// <item>
    /// Get Purchase, TranferOut, Purchase Return, TranferIn And Damage
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class FranchiseSaleInvoiceController
    {
        #region Constructor

        /// <summary>
        /// Constructor for PurchaseController
        /// </summary>
        public FranchiseSaleInvoiceController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Private Variables

        IDbConnection mConnection;

        #endregion

        #region Public Methods

        #region Select
        public static DataTable SelectFranchise_Invoice_Details(long p_FRANCHISE_MASTER_ID, IDbConnection PConnection, IDbTransaction PTransaction)
        {
            try
            {
                spSelectFranchiseSaleInvoice_Detail mFranchiseDeatil = new spSelectFranchiseSaleInvoice_Detail();
                mFranchiseDeatil.Connection = PConnection;
                mFranchiseDeatil.Transaction = PTransaction;
                mFranchiseDeatil.FRANCHISE_MASTER_ID = p_FRANCHISE_MASTER_ID;
                DataTable dt = mFranchiseDeatil.ExecuteTable();
                return dt;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
        }

        public DataTable SelectFranchise_Invoice_Lookup(int? p_DISTRIBUTOR_ID, int p_USER_ID,DateTime p_DOCUMENT_DATE, int p_TYPE_ID)
        {
            try
            {
                spSelectFRANCHISE_INVOICE_MASTER mFranchiseInvoiceMaster = new spSelectFRANCHISE_INVOICE_MASTER();
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mFranchiseInvoiceMaster.Connection = mConnection;
                mFranchiseInvoiceMaster.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mFranchiseInvoiceMaster.USER_ID = p_USER_ID;
                mFranchiseInvoiceMaster.DOCUMENT_DATE = p_DOCUMENT_DATE;
                mFranchiseInvoiceMaster.TYPE_ID = p_TYPE_ID;
                DataTable dt = mFranchiseInvoiceMaster.ExecuteTable();
                return dt;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
        }

        public DataTable SelectFranchise_Invoice_Details(long p_FRANCHISE_MASTER_ID)
        {
            try
            {
                spSelectFranchiseSaleInvoice_Detail mFranchiseDeatil = new spSelectFranchiseSaleInvoice_Detail();
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mFranchiseDeatil.Connection = mConnection;
                mFranchiseDeatil.FRANCHISE_MASTER_ID = p_FRANCHISE_MASTER_ID;
                DataTable dt = mFranchiseDeatil.ExecuteTable();
                return dt;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
        }

        public DataTable SelectFranchise_Invoice_Details2(long p_FRANCHISE_MASTER_ID)
        {
            try
            {
                spSELECTFRANCHISE_SALE_INVOICE_DETAIL2 mFranchiseDeatil = new spSELECTFRANCHISE_SALE_INVOICE_DETAIL2();
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mFranchiseDeatil.Connection = mConnection;
                mFranchiseDeatil.FRANCHISE_MASTER_ID = p_FRANCHISE_MASTER_ID;
                DataTable dt = mFranchiseDeatil.ExecuteTable();
                return dt;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
        }


        public DataTable SelectROLLBACK_REASON(Int16? p_TYPE_ID)
        {
            try
            {
                spSelectROLLBACK_REASON reason = new spSelectROLLBACK_REASON();
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                reason.Connection = mConnection;
                reason.TYPE_ID = p_TYPE_ID;
                DataTable dt = reason.ExecuteTable();
                return dt;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
        }

        #endregion

        #region Insert, Update


        /// <summary>
        /// Inserts Purchase, TranferOut, Purchase Return, TranferIn And Damage Document
        /// </summary>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_ORDER_NUMBER">DocumentNo</param>
        /// <param name="p_TYPE_ID">Type</param>
        /// <param name="p_DOCUMENT_DATE">Date</param>
        /// <param name="p_SOLD_TO">SoldTo</param>
        /// <param name="p_SOLD_FROM">SoldFrom</param>
        /// <param name="p_TOTAL_AMOUNT">Amount</param>
        /// <param name="p_IS_DELETE">IsDeleted</param>
        /// <param name="dtPurchaseDetail">PurchaseDetailDatatable</param>
        /// <param name="p_Posting">Posting</param>
        /// <param name="p_BuiltyNo">Builty</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_PrincipalId">Principal</param>
        /// <returns>True On Success And False On Failure</returns>
        /// 
        public static long Add_FranchiseSaleInvoice(int distributorId, long customerId, string refNo, string remarks,
            decimal totalAmount, int pUserId, DateTime pDocumentDate, bool IsFinanceSetting, bool posting, bool isDeleted, decimal p_SalePer,
            DataTable franchiseInvoiceDetail, bool IsFinanceIntegrate, DataTable dtCOAConfig, int p_STOCK_DEMAND_ID)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                if (franchiseInvoiceDetail.Rows.Count > 0)
                {
                    mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                    mConnection.Open();
                    mTransaction = ProviderFactory.GetTransaction(mConnection);
                    spInsertFranchiseSaleInvoice_Master mISom = new spInsertFranchiseSaleInvoice_Master
                    {
                        Connection = mConnection,
                        Transaction = mTransaction,
                        TOTAL_AMOUNT = totalAmount,
                        CREDIT_AMOUNT = totalAmount,
                        CUSTOMER_ID = customerId,
                        DISTRIBUTOR_ID = distributorId,
                        DOCUMENT_DATE = pDocumentDate,
                        TIME_STAMP = DateTime.Now,
                        REF_NO = refNo,
                        REMARKS = remarks,
                        USER_ID = pUserId,
                        SALES_PER = p_SalePer,
                        STOCK_DEMAND_ID = p_STOCK_DEMAND_ID
                    };
                    mISom.ExecuteQuery();
                    //----------------Insert into sale order detail-------------
                    spInsertFranchiseSaleInvoice_Detail mFranchiseDetail = new spInsertFranchiseSaleInvoice_Detail
                    {
                        Connection = mConnection,
                        Transaction = mTransaction
                    };
                    foreach (DataRow dr in franchiseInvoiceDetail.Rows)
                    {
                        mFranchiseDetail.FRANCHISE_MASTER_ID = mISom.FRANCHISE_MASTER_ID;
                        mFranchiseDetail.PRICE = decimal.Parse(dr["PRICE"].ToString());
                        mFranchiseDetail.QUANTITY = decimal.Parse(dr["Quantity"].ToString());
                        mFranchiseDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mFranchiseDetail.UOM_ID = int.Parse(dr["UOM_ID"].ToString());
                        mFranchiseDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mFranchiseDetail.USER_ID = mISom.USER_ID;
                        mFranchiseDetail.IS_DELETED = false;
                        mFranchiseDetail.TIME_STAMP = DateTime.Now;
                        mFranchiseDetail.DISTRIBUTOR_ID = Convert.ToInt32(distributorId);
                        mFranchiseDetail.SALES_PER = p_SalePer;
                        mFranchiseDetail.PRICE_LEVEL = decimal.Parse(dr["PRICE_LEVEL"].ToString());
                        mFranchiseDetail.SKU_PRICES_LEVEL_ID = long.Parse(dr["SKU_PRICES_LEVEL_ID"].ToString());
                        mFranchiseDetail.PRICE_LEVEL_FACTOR = decimal.Parse(dr["PRICE_LEVEL_FACTOR"].ToString());
                        mFranchiseDetail.ExecuteQuery();

                        //Update Stock
                        UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                        mStockUpdate.Connection = mConnection;
                        mStockUpdate.Transaction = mTransaction;
                        mStockUpdate.PRINCIPAL_ID = 0;
                        mStockUpdate.TYPE_ID = Constants.Document_Invoice;
                        mStockUpdate.DISTRIBUTOR_ID = mISom.DISTRIBUTOR_ID;
                        mStockUpdate.STOCK_DATE = mISom.DOCUMENT_DATE;
                        mStockUpdate.SKU_ID = mFranchiseDetail.SKU_ID;
                        mStockUpdate.STOCK_QTY = mFranchiseDetail.QUANTITY;
                        mStockUpdate.PRICE = mFranchiseDetail.PRICE;
                        mStockUpdate.FREE_QTY = 0;
                        mStockUpdate.BATCHNO = "N/A";
                        mStockUpdate.UOM_ID = mFranchiseDetail.UOM_ID;
                        mStockUpdate.ExecuteQuery();
                    }

                    #region Credit Invoice
                    DataRow[] drConfig2 = null;

                    LedgerController LController2 = new LedgerController();
                    string VoucherNo2 = LController2.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, distributorId, 0);

                    drConfig2 = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
                    LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), distributorId, totalAmount, 0, pDocumentDate, "Credit Sale Default", DateTime.Now, 0, int.Parse(customerId.ToString()), mISom.FRANCHISE_MASTER_ID, "", Constants.Document_FranchiseSale, pUserId, mTransaction, mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                    drConfig2 = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSales + "'");
                    LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), distributorId, 0, totalAmount, pDocumentDate, "Credit Sale Default", DateTime.Now, 0, int.Parse(customerId.ToString()), mISom.FRANCHISE_MASTER_ID, "", Constants.Document_FranchiseSale, pUserId, mTransaction, mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                    #endregion

                    if (IsFinanceIntegrate)

                    {
                        #region GL Master, Detail

                        LedgerController LController = new LedgerController();

                        string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, distributorId, pDocumentDate);

                        if (LController.PostingGLMaster(distributorId, 0, VoucherNo, Constants.Journal_Voucher, pDocumentDate, Constants.Document_FranchiseSale, Convert.ToString(mISom.FRANCHISE_MASTER_ID), "Sale Voucher, Inv#: " + mISom.FRANCHISE_MASTER_ID.ToString(), pUserId, "FranchiseSale", Constants.Document_FranchiseSale, mISom.FRANCHISE_MASTER_ID, mTransaction, mConnection))
                        {
                            DataRow[] drConfig = null;

                            //Dr  Credit Sale Receivable
                            //Cr  Credit Sales

                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
                            LController.PostingGLDetail(distributorId, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), totalAmount, 0, "Credit Card Sale Voucher", mTransaction, mConnection);
                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSales + "'");
                            LController.PostingGLDetail(distributorId, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, totalAmount, "Credit Card Sale Voucher", mTransaction, mConnection);

                        }

                        #endregion
                    }

                    mTransaction.Commit();
                    return mISom.FRANCHISE_MASTER_ID;
                }
            }
            catch (Exception exp)
            {

                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                throw;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
            return 0;
        }

        public static long Update_FranchiseSaleInvoice(long masterId, int distributorId, long customerId, string refNo, string remarks,
        decimal totalAmount, int pUserId, DateTime pDocumentDate, bool IsFinanceSetting, bool posting, bool isDeleted
            , decimal p_SalePer, DataTable franchiseInvoiceDetail, bool IsFinanceIntegrate, DataTable dtCOAConfig, int p_STOCK_DEMAND_ID)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {

                if (franchiseInvoiceDetail.Rows.Count > 0)
                {
                    mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                    mConnection.Open();

                    spupdateFranchiseSaleInvoice_Master mISom = new spupdateFranchiseSaleInvoice_Master
                    {
                        Connection = mConnection,
                        Transaction = null,
                        TOTAL_AMOUNT = totalAmount,
                        CREDIT_AMOUNT = totalAmount,
                        CUSTOMER_ID = customerId,
                        DISTRIBUTOR_ID = distributorId,
                        FRANCHISE_MASTER_ID = masterId,
                        DOCUMENT_DATE = pDocumentDate,
                        REF_NO = refNo,
                        REMARKS = remarks,
                        USER_ID = pUserId,
                        SALES_PER = p_SalePer,
                        STOCK_DEMAND_ID = p_STOCK_DEMAND_ID
                    };
                    mISom.ExecuteQuery();

                    DataTable dt = SelectFranchise_Invoice_Details(masterId, mConnection, mTransaction);
                    foreach (DataRow dr in dt.Rows)
                    {
                        UspUpdatePurchaseDetailStock mPurchaseStock = new UspUpdatePurchaseDetailStock();
                        mPurchaseStock.Connection = mConnection;
                        mPurchaseStock.Transaction = null;
                        mPurchaseStock.TYPEID = Constants.Document_FranchiseSale;
                        mPurchaseStock.DISTRIBUTOR_ID = distributorId;
                        mPurchaseStock.PURCHASE_DETAIL_ID = 0;
                        mPurchaseStock.DATE = pDocumentDate;
                        mPurchaseStock.QTY = Convert.ToDecimal(dr["Quantity"]);
                        mPurchaseStock.PURCHASE_MASTER_ID = masterId;
                        mPurchaseStock.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mPurchaseStock.ExecuteQuery();
                    }
                    //----------------Insert into sale order detail-------------
                    mTransaction = ProviderFactory.GetTransaction(mConnection);
                    spInsertFranchiseSaleInvoice_Detail mFranchiseDetail = new spInsertFranchiseSaleInvoice_Detail
                    {
                        Connection = mConnection,
                        Transaction = mTransaction
                    };
                    foreach (DataRow dr in franchiseInvoiceDetail.Rows)
                    {
                        mFranchiseDetail.FRANCHISE_MASTER_ID = mISom.FRANCHISE_MASTER_ID;
                        mFranchiseDetail.PRICE = decimal.Parse(dr["PRICE"].ToString());
                        mFranchiseDetail.QUANTITY = decimal.Parse(dr["Quantity"].ToString());
                        mFranchiseDetail.AMOUNT = decimal.Parse(dr["AMOUNT"].ToString());
                        mFranchiseDetail.UOM_ID = int.Parse(dr["UOM_ID"].ToString());
                        mFranchiseDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mFranchiseDetail.USER_ID = mISom.USER_ID;
                        mFranchiseDetail.IS_DELETED = false;
                        mFranchiseDetail.TIME_STAMP = DateTime.Now;
                        mFranchiseDetail.DISTRIBUTOR_ID = Convert.ToInt32(distributorId);
                        mFranchiseDetail.SALES_PER = p_SalePer;
                        mFranchiseDetail.PRICE_LEVEL = decimal.Parse(dr["PRICE_LEVEL"].ToString());
                        mFranchiseDetail.SKU_PRICES_LEVEL_ID = long.Parse(dr["SKU_PRICES_LEVEL_ID"].ToString());
                        mFranchiseDetail.PRICE_LEVEL_FACTOR = decimal.Parse(dr["PRICE_LEVEL_FACTOR"].ToString());
                        mFranchiseDetail.ExecuteQuery();

                        //Update Stock
                        UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                        mStockUpdate.Connection = mConnection;
                        mStockUpdate.Transaction = mTransaction;
                        mStockUpdate.PRINCIPAL_ID = 0;
                        mStockUpdate.TYPE_ID = Constants.Document_Invoice;
                        mStockUpdate.DISTRIBUTOR_ID = mISom.DISTRIBUTOR_ID;
                        mStockUpdate.STOCK_DATE = mISom.DOCUMENT_DATE;
                        mStockUpdate.SKU_ID = mFranchiseDetail.SKU_ID;
                        mStockUpdate.STOCK_QTY = mFranchiseDetail.QUANTITY;
                        mStockUpdate.PRICE = mFranchiseDetail.PRICE;
                        mStockUpdate.FREE_QTY = 0;
                        mStockUpdate.BATCHNO = "N/A";
                        mStockUpdate.UOM_ID = mFranchiseDetail.UOM_ID;
                        mStockUpdate.ExecuteQuery();
                    }

                    #region Credit Invoice
                    DataRow[] drConfig2 = null;

                    LedgerController LController2 = new LedgerController();
                    string VoucherNo2 = LController2.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, distributorId, 0);

                    drConfig2 = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
                    LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), distributorId, totalAmount, 0, pDocumentDate, "Credit Sale Default", DateTime.Now, 0, int.Parse(customerId.ToString()), mISom.FRANCHISE_MASTER_ID, "", Constants.Document_FranchiseSale, pUserId, mTransaction, mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                    drConfig2 = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSales + "'");
                    LController2.PostingInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo2), long.Parse(drConfig2[0]["VALUE"].ToString()), distributorId, 0, totalAmount, pDocumentDate, "Credit Sale Default", DateTime.Now, 0, int.Parse(customerId.ToString()), mISom.FRANCHISE_MASTER_ID, "", Constants.Document_FranchiseSale, pUserId, mTransaction, mConnection, Constants.CreditSale, mISom.USER_ID.ToString());

                    #endregion

                    if (IsFinanceIntegrate)

                    {
                        #region GL Master, Detail

                        LedgerController LController = new LedgerController();

                        string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, distributorId, pDocumentDate);

                        if (LController.PostingGLMaster(distributorId, 0, VoucherNo, Constants.Journal_Voucher, pDocumentDate, Constants.Document_FranchiseSale, Convert.ToString(mISom.FRANCHISE_MASTER_ID), "Sale Voucher, Inv#: " + mISom.FRANCHISE_MASTER_ID.ToString(), pUserId, "FranchiseSale", Constants.Document_FranchiseSale, mISom.FRANCHISE_MASTER_ID))
                        {
                            DataRow[] drConfig = null;

                            //Dr  Credit Sale Receivable
                            //Cr  Credit Sales

                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSaleReceivable + "'");
                            LController.PostingGLDetail(distributorId, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), totalAmount, 0, "Credit Card Sale Voucher");
                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.CreditSales + "'");
                            LController.PostingGLDetail(distributorId, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, totalAmount, "Credit Card Sale Voucher");

                        }

                        #endregion
                    }
                    mTransaction.Commit();
                    return mISom.FRANCHISE_MASTER_ID;
                }
            }
            catch (Exception exp)
            {

                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                throw;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
            return 0;
        }

        public bool Update_FranchiseSaleInvoice(long masterId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdateFRANCHISE_SALE_INVOICE_MASTER2 mISom = new spUpdateFRANCHISE_SALE_INVOICE_MASTER2
                {
                    Connection = mConnection,
                    FRANCHISE_MASTER_ID = masterId
                };
                mISom.ExecuteQuery();
                return true;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw;
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

        #region Franchise Sale Invoice Report
        public DataSet SelectFranchiseReport(long p_franchise_Master_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectFRANCHISE_INVOICE_MASTER ObjPrint = new spSelectFRANCHISE_INVOICE_MASTER();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.FRANCHISE_MASTER_ID = p_franchise_Master_ID;
                DataTable dt = ObjPrint.ExecuteTableFranchiseSaleInvoiceReport();
                foreach (DataRow dr in dt.Rows)
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

        public DataSet SelectFranchiseReportByLocationDate(int p_Distributor_ID, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectFRANCHISE_INVOICE_MASTER ObjPrint = new spSelectFRANCHISE_INVOICE_MASTER();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataTable dt = ObjPrint.ExecuteTableForFranchiseSaleRptByLocationANDDate();
                foreach (DataRow dr in dt.Rows)
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
        public DataSet SelectFranchiseReportByPriceLevel(int p_Distributor_ID, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectFRANCHISE_INVOICE_MASTER ObjPrint = new spSelectFRANCHISE_INVOICE_MASTER();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataTable dt = ObjPrint.ExecuteTableForFranchiseSaleRptByPriceFactor();
                foreach (DataRow dr in dt.Rows)
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
        public DataSet GetFranchiseSummary(int p_Distributor_ID,long p_Customer_ID, DateTime p_FromDate, DateTime p_To_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetFranchiseSummary ObjPrint = new uspGetFranchiseSummary();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.CUSTOMER_ID = p_Customer_ID;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetFranchiseSummary"].ImportRow(dr);
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