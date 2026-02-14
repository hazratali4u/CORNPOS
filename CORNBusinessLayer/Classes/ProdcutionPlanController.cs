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
	public class ProdcutionPlanController
    {
        #region Constructors

        /// <summary>
        /// Constructor For ProdcutionPlanController
        /// </summary>
        public ProdcutionPlanController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region Select
        public DataTable SelectProdcutionPlanInfo(int p_finish_id, long p_lngProductionPlanCode, int DistributorID, DateTime p_Date,DateTime p_TO_DATE, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelecttblProductionPlanMaster mspSelectSkuInfo = new spSelecttblProductionPlanMaster();
                mspSelectSkuInfo.Connection = mConnection;

                mspSelectSkuInfo.FINISHED_SKU_ID = p_finish_id;
                mspSelectSkuInfo.lngProductionPlanCode = p_lngProductionPlanCode;
                mspSelectSkuInfo.DISTRIBUTOR_ID = DistributorID;
                mspSelectSkuInfo.DATE = p_Date;
                mspSelectSkuInfo.TO_DATE = p_TO_DATE;
                mspSelectSkuInfo.TYPE_ID = p_TYPE_ID;
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

        public long InsertProductionPlan(int p_FINISHED_GOOD_MASTER_ID, int p_DISTRIBUTOR_ID, int p_FINISHED_SKU_ID, decimal p_ProdQty
            , DateTime p_DOCUMENT_DATE, int p_USER_ID,string p_ConsumptionFromProductionPlan, DataTable dtFinishedDetail, bool IsFinanceIntegrate, DataTable dtCOAConfig)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertblProductionPlanMaster mRecipeMaster = new spInsertblProductionPlanMaster ();
                mRecipeMaster.Connection = mConnection;
                mRecipeMaster.Transaction = mTransaction;

                //------------Insert Recipe Master----------\\

                if (dtFinishedDetail.Rows.Count > 0)
                {
                    mRecipeMaster.FinishedSKUID = p_FINISHED_SKU_ID;
                    mRecipeMaster.DistributorID = p_DISTRIBUTOR_ID;
                    mRecipeMaster.DocumentDate = p_DOCUMENT_DATE;
                    mRecipeMaster.UserID = p_USER_ID;
                    mRecipeMaster.Production_Qty = p_ProdQty;
                    mRecipeMaster.FINISHED_GOOD_MASTER_ID = p_FINISHED_GOOD_MASTER_ID;
                    
                    mRecipeMaster.ExecuteQuery();

                    //----------------Insert Production Plan Detail-------------
                    decimal GLConsumption = 0;
                    if (p_ConsumptionFromProductionPlan == "1")
                    {
                        spInserttblProductionPlanDetail mRecipeDetail = new spInserttblProductionPlanDetail();
                        mRecipeDetail.Connection = mConnection;
                        mRecipeDetail.Transaction = mTransaction;

                        UspProcessStockRegister mStockDetail = new UspProcessStockRegister();
                        mStockDetail.Connection = mConnection;
                        mStockDetail.Transaction = mTransaction;                        
                        foreach (DataRow dr in dtFinishedDetail.Rows)
                        {
                            mRecipeDetail.lngProductionPlanCode = mRecipeMaster.lngProductionPlanCode;
                            mRecipeDetail.FINISHED_GOOD_DETAIL_ID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                            mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                            mRecipeDetail.Quantity = decimal.Parse(dr["Quantity"].ToString()) * mRecipeMaster.Production_Qty;
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
                    }
                    else
                    {
                        spInserttblProductionPlanDetail mRecipeDetail = new spInserttblProductionPlanDetail();
                        mRecipeDetail.Connection = mConnection;
                        mRecipeDetail.Transaction = mTransaction;

                        foreach (DataRow dr in dtFinishedDetail.Rows)
                        {
                            mRecipeDetail.lngProductionPlanCode = mRecipeMaster.lngProductionPlanCode;
                            mRecipeDetail.FINISHED_GOOD_DETAIL_ID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                            mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                            mRecipeDetail.Quantity = decimal.Parse(dr["Quantity"].ToString()) * mRecipeMaster.Production_Qty;
                            mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                            mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                            mRecipeDetail.ExecuteQuery();
                        }
                    }

                    if (IsFinanceIntegrate && GLConsumption > 0 && p_ConsumptionFromProductionPlan=="1")
                    {
                        //Dr Consumption
                        //Cr Stock in Trade
                        DataRow[] drConfig = null;
                        LedgerController LController = new LedgerController();
                        string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, p_DOCUMENT_DATE);
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, 0, VoucherNo, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.CashPayment, Convert.ToString(mRecipeMaster.lngProductionPlanCode), "Production Plan Voucher, Plan#: " + mRecipeMaster.lngProductionPlanCode.ToString(), p_USER_ID, "ProdPlan", Constants.Document_ProductionPlan, mRecipeMaster.lngProductionPlanCode))
                        {
                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Consumption + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), GLConsumption, 0, "Consumption Sale Voucher");

                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, GLConsumption, "Inventoryatstore Sale Voucher");
                        }
                    }
                    mTransaction.Commit();                    
                }
                return mRecipeMaster.lngProductionPlanCode;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                return Constants.LongNullValue;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public bool UpdateProductionPlan(long MASTER_ID, decimal p_ActualQty, int p_USER_ID, DataTable dtRecipeDetail)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdatetblProductionPlanMaster mRecipeMaster = new spUpdatetblProductionPlanMaster();
                mRecipeMaster.Connection = mConnection;

                //------------Insert Recipe Master----------\\

                if (dtRecipeDetail.Rows.Count > 0)
                {
                   
                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.Production_Qty = p_ActualQty;                    
                    mRecipeMaster.lngProductionPlanCode = MASTER_ID;
                    mRecipeMaster.TypeID = 1;
                    mRecipeMaster.ExecuteQuery();

                    //----------------Insert Finished Goods Detail-------------
                    spInserttblProductionPlanDetail mRecipeDetail = new spInserttblProductionPlanDetail();
                    mRecipeDetail.Connection = mConnection;
                    
                    foreach (DataRow dr in dtRecipeDetail.Rows)
                    {
                        mRecipeDetail.lngProductionPlanCode = mRecipeMaster.lngProductionPlanCode;
                        mRecipeDetail.FINISHED_GOOD_DETAIL_ID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                        mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeDetail.Quantity = decimal.Parse(dr["Quantity"].ToString()) * mRecipeMaster.Production_Qty;
                        mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                        mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                        mRecipeDetail.ExecuteQuery();

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

        public bool UpdateProductionPlan(long MASTER_ID, decimal p_ActualQty, int p_USER_ID, DataTable dtRecipeDetail,int p_DISTRIBUTOR_ID,DateTime p_DOCUMENT_DATE, string p_ConsumptionFromProductionPlan, bool IsFinanceIntegrate, DataTable dtCOAConfig)
        {
            DataTable dtRecipeDetail2 = new DataTable();
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdatetblProductionPlanMaster mRecipeMaster = new spUpdatetblProductionPlanMaster();
                mRecipeMaster.Connection = mConnection;

                //------------Insert Recipe Master----------\\

                if (dtRecipeDetail.Rows.Count > 0)
                {
                    UspUpdatePurchaseDetailStock mRecipeStock = new UspUpdatePurchaseDetailStock();
                    mRecipeStock.Connection = mConnection;

                    spSelecttblProductionPlanMaster mSelectRecipe = new spSelecttblProductionPlanMaster();
                    mSelectRecipe.Connection = mConnection;
                                        
                    mSelectRecipe.lngProductionPlanCode = MASTER_ID;
                    mSelectRecipe.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mSelectRecipe.DATE = p_DOCUMENT_DATE;
                    mSelectRecipe.TYPE_ID = 3;
                    dtRecipeDetail2 = mSelectRecipe.ExecuteTable();

                    foreach (DataRow dr in dtRecipeDetail2.Rows)
                    {

                        mRecipeStock.TYPEID = 23;
                        mRecipeStock.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mRecipeStock.PURCHASE_DETAIL_ID = 0;
                        mRecipeStock.PURCHASE_MASTER_ID = MASTER_ID;
                        mRecipeStock.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeStock.DATE = p_DOCUMENT_DATE;
                        mRecipeStock.QTY = decimal.Parse(dr["ActulQty"].ToString());
                        mRecipeStock.ExecuteQuery();
                    }


                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.Production_Qty = p_ActualQty;
                    mRecipeMaster.lngProductionPlanCode = MASTER_ID;
                    mRecipeMaster.IsActive = true;
                    mRecipeMaster.TypeID = 3;
                    mRecipeMaster.ExecuteQuery();

                    //----------------Insert Finished Goods Detail-------------
                    spInserttblProductionPlanDetail mRecipeDetail = new spInserttblProductionPlanDetail();
                    mRecipeDetail.Connection = mConnection;

                    UspProcessStockRegister mStockDetail = new UspProcessStockRegister();
                    mStockDetail.Connection = mConnection;

                    decimal GLConsumption = 0;
                    foreach (DataRow dr in dtRecipeDetail.Rows)
                    {
                        mRecipeDetail.lngProductionPlanCode = mRecipeMaster.lngProductionPlanCode;
                        mRecipeDetail.FINISHED_GOOD_DETAIL_ID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                        mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeDetail.Quantity = decimal.Parse(dr["Quantity"].ToString()) * mRecipeMaster.Production_Qty;
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
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, 0, VoucherNo, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.CashPayment, Convert.ToString(mRecipeMaster.lngProductionPlanCode), "Production Plan Voucher, Plan#: " + mRecipeMaster.lngProductionPlanCode.ToString(), p_USER_ID, "ProdPlan", Constants.Document_ProductionPlan, mRecipeMaster.lngProductionPlanCode))
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

        public bool DeleteProductionPlan(long MASTER_ID, int p_USER_ID)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdatetblProductionPlanMaster mRecipeMaster = new spUpdatetblProductionPlanMaster();
                mRecipeMaster.Connection = mConnection;
                mRecipeMaster.USER_ID = p_USER_ID;
                mRecipeMaster.lngProductionPlanCode = MASTER_ID;
                mRecipeMaster.TypeID = 2;
                mRecipeMaster.ExecuteQuery();

                return true;
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
        }
        public bool DeleteProductionPlan(long MASTER_ID, int p_USER_ID, int p_DISTRIBUTOR_ID, DateTime p_DOCUMENT_DATE)
        {
            DataTable dtRecipeDetail = new DataTable();
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spUpdatetblProductionPlanMaster mRecipeMaster = new spUpdatetblProductionPlanMaster();
                mRecipeMaster.Connection = mConnection;
                mRecipeMaster.Transaction = mTransaction;

                spSelecttblProductionPlanMaster mSelectRecipe = new spSelecttblProductionPlanMaster();
                mSelectRecipe.Connection = mConnection;
                mSelectRecipe.Transaction = mTransaction;

                mSelectRecipe.lngProductionPlanCode = MASTER_ID;
                mSelectRecipe.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mSelectRecipe.DATE = p_DOCUMENT_DATE;
                mSelectRecipe.TO_DATE = Constants.DateNullValue;
                mSelectRecipe.TYPE_ID = 3;
                dtRecipeDetail = mSelectRecipe.ExecuteTable();

                if (dtRecipeDetail.Rows.Count > 0)
                {
                    UspUpdatePurchaseDetailStock mRecipeStock = new UspUpdatePurchaseDetailStock();
                    mRecipeStock.Connection = mConnection;
                    mRecipeStock.Transaction = mTransaction;

                    foreach (DataRow dr in dtRecipeDetail.Rows)
                    {
                        mRecipeStock.TYPEID = 23;
                        mRecipeStock.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mRecipeStock.PURCHASE_DETAIL_ID = 0;
                        mRecipeStock.PURCHASE_MASTER_ID = MASTER_ID;
                        mRecipeStock.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeStock.DATE = p_DOCUMENT_DATE;
                        mRecipeStock.QTY = decimal.Parse(dr["ActulQty"].ToString());
                        mRecipeStock.ExecuteQuery();
                    }


                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.lngProductionPlanCode = MASTER_ID;
                    mRecipeMaster.IsActive = false;
                    mRecipeMaster.TypeID = 3;
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