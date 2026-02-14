using System;
using System.Data;
using System.IO;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using CORNBusinessLayer.Classes;
using System.Collections.Generic;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For SKU Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert SKU
    /// </item>
    /// <term>
    /// Update SKU
    /// </term>
    /// <item>
    /// Get SKU
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class BOMIssuanceController
    {
        #region Constructors

        /// <summary>
        /// Constructor For BOMIssuanceController
        /// </summary>
        public BOMIssuanceController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region Select
        public DataTable SelectBOMIssuanceInfo(int p_finish_id, long p_lngProductionPlanCode, int DistributorID, DateTime p_Date,DateTime p_TO_DATE, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelecttblBOMIssuanceMaster mspSelectSkuInfo = new spSelecttblBOMIssuanceMaster();
                mspSelectSkuInfo.Connection = mConnection;

                mspSelectSkuInfo.FINISHED_SKU_ID = p_finish_id;
                mspSelectSkuInfo.lngBOMIssuanceCode = p_lngProductionPlanCode;
                mspSelectSkuInfo.DISTRIBUTOR_ID = DistributorID;
                mspSelectSkuInfo.DATE = p_Date;
                mspSelectSkuInfo.TYPE_ID = p_TYPE_ID;
                mspSelectSkuInfo.TO_DATE = p_TO_DATE;
                DataTable ds = mspSelectSkuInfo.ExecuteTable();

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

        #region Insert/Update

        public long InsertBOMIssuance(long p_ProdctionPanID, int p_DISTRIBUTOR_ID, int p_FINISHED_SKU_ID, decimal p_IssueQty
            , DateTime p_DOCUMENT_DATE, int p_USER_ID,string p_ConsumptionFromProductionPlan,decimal p_FinishedSKUPrice, DataTable dtFinishedDetail, bool IsFinanceIntegrate, DataTable dtCOAConfig)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertblBOMIssuanceMaster mRecipeMaster = new spInsertblBOMIssuanceMaster();
                mRecipeMaster.Connection = mConnection;
                mRecipeMaster.Transaction = mTransaction;

                //------------Insert BOM Issuane Master----------\\

                if (dtFinishedDetail.Rows.Count > 0)
                {
                    mRecipeMaster.FinishedSKUID = p_FINISHED_SKU_ID;
                    mRecipeMaster.DistributorID = p_DISTRIBUTOR_ID;
                    mRecipeMaster.DocumentDate = p_DOCUMENT_DATE;
                    mRecipeMaster.UserID = p_USER_ID;
                    mRecipeMaster.Issuance_Qty = p_IssueQty;
                    mRecipeMaster.ProductionPlanID = p_ProdctionPanID;
                    mRecipeMaster.FinishedSKUPrice = p_FinishedSKUPrice;
                    mRecipeMaster.ExecuteQuery();

                    //----------------Insert BOM Issuane Detail-------------
                    decimal GLConsumption = 0;

                    spInserttblBOMIssuanceDetail mRecipeDetail = new spInserttblBOMIssuanceDetail();
                        mRecipeDetail.Connection = mConnection;
                        mRecipeDetail.Transaction = mTransaction;

                        UspProcessStockRegister mStockDetail = new UspProcessStockRegister();
                        mStockDetail.Connection = mConnection;
                        mStockDetail.Transaction = mTransaction;                        
                        foreach (DataRow dr in dtFinishedDetail.Rows)
                        {
                            mRecipeDetail.lngBOMIssuanceCode = mRecipeMaster.lngBOMIssuanceCode;
                            mRecipeDetail.ProductionPlanDetailID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                            mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                            mRecipeDetail.Quantity = decimal.Parse(dr["Quantity"].ToString());
                            mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                            mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                            mRecipeDetail.ExecuteQuery();

                            mStockDetail.PRINCIPAL_ID = 0;
                            mStockDetail.TYPE_ID = 18;
                            mStockDetail.DISTRIBUTOR_ID = mRecipeMaster.DistributorID;
                            mStockDetail.STOCK_DATE = mRecipeMaster.DocumentDate;
                            mStockDetail.SKU_ID = mRecipeDetail.SKU_ID;
                            mStockDetail.STOCK_QTY = mRecipeDetail.Quantity;
                            mStockDetail.PRICE = 0;
                            mStockDetail.FREE_QTY = 0;
                            mStockDetail.BATCHNO = "NA";
                            mStockDetail.UOM_ID = mRecipeDetail.intStockMUnitCode;
                            mStockDetail.ExecuteQuery();
                            GLConsumption += mRecipeDetail.Quantity * mRecipeDetail.Price;
                        }

                    if (IsFinanceIntegrate && GLConsumption > 0)
                    {
                        //Dr Consumption
                        //Cr Stock in Trade
                        DataRow[] drConfig = null;
                        LedgerController LController = new LedgerController();
                        string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, p_DOCUMENT_DATE);
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, 0, VoucherNo, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.CashPayment, Convert.ToString(mRecipeMaster.lngBOMIssuanceCode), "BOM Issuance Voucher, Plan#: " + mRecipeMaster.lngBOMIssuanceCode.ToString(), p_USER_ID, "BOMIssue", Constants.Document_BOMIssuance, mRecipeMaster.lngBOMIssuanceCode))
                        {
                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Consumption + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), GLConsumption, 0, "Consumption Sale Voucher");

                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, GLConsumption, "Inventoryatstore Sale Voucher");
                        }
                    }
                    mTransaction.Commit();
                    return mRecipeMaster.lngBOMIssuanceCode;
                }
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return Constants.LongNullValue;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
            return Constants.LongNullValue;
        }

        public bool UpdateBOMIssuance(long MASTER_ID, decimal p_ActualQty, int p_USER_ID, DataTable dtRecipeDetail,int p_DISTRIBUTOR_ID,DateTime p_DOCUMENT_DATE,decimal p_FinishedSKUPrice, bool IsFinanceIntegrate, DataTable dtCOAConfig)
        {
            DataTable dtRecipeDetail2 = new DataTable();
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdatetblBOMIssuanceMaster mRecipeMaster = new spUpdatetblBOMIssuanceMaster();
                mRecipeMaster.Connection = mConnection;

                //------------Insert Recipe Master----------\\

                if (dtRecipeDetail.Rows.Count > 0)
                {
                    UspUpdatePurchaseDetailStock mRecipeStock = new UspUpdatePurchaseDetailStock();
                    mRecipeStock.Connection = mConnection;

                    spSelecttblBOMIssuanceMaster mSelectRecipe = new spSelecttblBOMIssuanceMaster();
                    mSelectRecipe.Connection = mConnection;
                                        
                    mSelectRecipe.lngBOMIssuanceCode = MASTER_ID;
                    mSelectRecipe.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mSelectRecipe.DATE = p_DOCUMENT_DATE;
                    mSelectRecipe.TO_DATE = p_DOCUMENT_DATE;
                    mSelectRecipe.TYPE_ID = 3;
                    dtRecipeDetail2 = mSelectRecipe.ExecuteTable();

                    foreach (DataRow dr in dtRecipeDetail2.Rows)
                    {
                        mRecipeStock.TYPEID = 24;
                        mRecipeStock.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mRecipeStock.PURCHASE_DETAIL_ID = 0;
                        mRecipeStock.PURCHASE_MASTER_ID = MASTER_ID;
                        mRecipeStock.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeStock.DATE = p_DOCUMENT_DATE;
                        mRecipeStock.QTY = decimal.Parse(dr["ActulQty"].ToString());
                        mRecipeStock.ExecuteQuery();
                    }


                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.Issuance_Qty = p_ActualQty;
                    mRecipeMaster.lngBOMIssuanceCode = MASTER_ID;
                    mRecipeMaster.IsActive = true;
                    mRecipeMaster.TypeID = 1;
                    mRecipeMaster.FinishedSKUPrice = p_FinishedSKUPrice;
                    mRecipeMaster.ExecuteQuery();

                    //----------------Insert Finished Goods Detail-------------
                    spInserttblBOMIssuanceDetail mRecipeDetail = new spInserttblBOMIssuanceDetail();
                    mRecipeDetail.Connection = mConnection;

                    UspProcessStockRegister mStockDetail = new UspProcessStockRegister();
                    mStockDetail.Connection = mConnection;

                    decimal GLConsumption = 0;
                    foreach (DataRow dr in dtRecipeDetail.Rows)
                    {
                        mRecipeDetail.lngBOMIssuanceCode = mRecipeMaster.lngBOMIssuanceCode;
                        mRecipeDetail.ProductionPlanDetailID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                        mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeDetail.Quantity = decimal.Parse(dr["Quantity"].ToString());
                        mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                        mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                        mRecipeDetail.ExecuteQuery();

                        mStockDetail.PRINCIPAL_ID = 0;
                        mStockDetail.TYPE_ID = 18;
                        mStockDetail.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mStockDetail.STOCK_DATE = p_DOCUMENT_DATE;
                        mStockDetail.SKU_ID = mRecipeDetail.SKU_ID;
                        mStockDetail.STOCK_QTY = mRecipeDetail.Quantity;
                        mStockDetail.PRICE = 0;
                        mStockDetail.FREE_QTY = 0;
                        mStockDetail.BATCHNO = "NA";
                        mStockDetail.UOM_ID = mRecipeDetail.intStockMUnitCode;
                        mStockDetail.ExecuteQuery();
                        GLConsumption += mRecipeDetail.Quantity * mRecipeDetail.Price;
                    }

                    if (IsFinanceIntegrate && GLConsumption > 0)
                    {
                        //Dr Consumption
                        //Cr Stock in Trade
                        DataRow[] drConfig = null;
                        LedgerController LController = new LedgerController();
                        string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, p_DOCUMENT_DATE);
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, 0, VoucherNo, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.CashPayment, Convert.ToString(mRecipeMaster.lngBOMIssuanceCode), "BOM Issuance Voucher, Plan#: " + mRecipeMaster.lngBOMIssuanceCode.ToString(), p_USER_ID, "BOMIssue", Constants.Document_BOMIssuance, mRecipeMaster.lngBOMIssuanceCode))
                        {
                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Consumption + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), GLConsumption, 0, "Consumption Sale Voucher");

                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, GLConsumption, "Inventoryatstore Sale Voucher");
                        }
                    }

                    return true;
                }
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw;// exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
            return true;
        }

        public bool DeleteBOMIssuance(long MASTER_ID, int p_USER_ID, int p_DISTRIBUTOR_ID, DateTime p_DOCUMENT_DATE)
        {
            DataTable dtRecipeDetail = new DataTable();
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spUpdatetblBOMIssuanceMaster mRecipeMaster = new spUpdatetblBOMIssuanceMaster();
                mRecipeMaster.Connection = mConnection;
                mRecipeMaster.Transaction = mTransaction;

                spSelecttblBOMIssuanceMaster mSelectRecipe = new spSelecttblBOMIssuanceMaster();
                mSelectRecipe.Connection = mConnection;
                mSelectRecipe.Transaction = mTransaction;

                mSelectRecipe.lngBOMIssuanceCode = MASTER_ID;
                mSelectRecipe.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mSelectRecipe.DATE = p_DOCUMENT_DATE;
                mSelectRecipe.TYPE_ID = 3;
                mSelectRecipe.TO_DATE = Constants.DateNullValue;
                dtRecipeDetail = mSelectRecipe.ExecuteTable();

                if (dtRecipeDetail.Rows.Count > 0)
                {
                    UspUpdatePurchaseDetailStock mRecipeStock = new UspUpdatePurchaseDetailStock();
                    mRecipeStock.Connection = mConnection;
                    mRecipeStock.Transaction = mTransaction;

                    foreach (DataRow dr in dtRecipeDetail.Rows)
                    {
                        mRecipeStock.TYPEID = 24;
                        mRecipeStock.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mRecipeStock.PURCHASE_DETAIL_ID = 0;
                        mRecipeStock.PURCHASE_MASTER_ID = MASTER_ID;
                        mRecipeStock.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeStock.DATE = p_DOCUMENT_DATE;
                        mRecipeStock.QTY = decimal.Parse(dr["ActulQty"].ToString());
                        mRecipeStock.ExecuteQuery();
                    }


                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.lngBOMIssuanceCode = MASTER_ID;
                    mRecipeMaster.IsActive = false;
                    mRecipeMaster.TypeID = 2;
                    mRecipeMaster.ExecuteQuery();

                    mTransaction.Commit();
                    return true;
                }
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw;// exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
            return true;
        }
        #endregion
    }
}