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
	public class SplitItemController
    {
        #region Constructors

        /// <summary>
        /// Constructor For SplitItemController
        /// </summary>
        public SplitItemController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region Select
        public DataTable SelectSplitItemInfo(long p_lngSplitItemCode, int DistributorID, DateTime Date, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelecttblSplitItemMaster mspSelectSkuInfo = new spSelecttblSplitItemMaster();
                mspSelectSkuInfo.Connection = mConnection;                
                mspSelectSkuInfo.lngSplitItemCode = p_lngSplitItemCode;
                mspSelectSkuInfo.DISTRIBUTOR_ID = DistributorID;
                mspSelectSkuInfo.DATE = Date;
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

        #region Insert/Update/Delete

        public bool InsertSplitItem(int p_DISTRIBUTOR_ID, int p_FINISHED_SKU_ID,int p_intProductionMUnitCode,decimal p_SplitQty, DateTime p_DOCUMENT_DATE, int p_USER_ID , DataTable dtFinishedDetail, bool IsFinanceIntegrate, DataTable dtCOAConfig)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertblSplitItemMaster mRecipeMaster = new spInsertblSplitItemMaster();
                mRecipeMaster.Connection = mConnection;
                mRecipeMaster.Transaction = mTransaction;

                //------------Insert Recipe Master----------\\

                if (dtFinishedDetail.Rows.Count > 0)
                {
                    mRecipeMaster.FINISHED_SKU_ID = p_FINISHED_SKU_ID;
                    mRecipeMaster.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mRecipeMaster.DOCUMENT_DATE = p_DOCUMENT_DATE;
                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.intProductionMUnitCode = p_intProductionMUnitCode;
                    mRecipeMaster.SplitQty = p_SplitQty;
                    mRecipeMaster.ExecuteQuery();

                    UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                    mStockUpdate.Connection = mConnection;
                    mStockUpdate.Transaction = mTransaction;
                    mStockUpdate.PRINCIPAL_ID = 0;
                    mStockUpdate.TYPE_ID = 18;
                    mStockUpdate.DISTRIBUTOR_ID = mRecipeMaster.DISTRIBUTOR_ID;
                    mStockUpdate.STOCK_DATE = mRecipeMaster.DOCUMENT_DATE;
                    mStockUpdate.SKU_ID = mRecipeMaster.FINISHED_SKU_ID;
                    mStockUpdate.STOCK_QTY = mRecipeMaster.SplitQty;
                    mStockUpdate.PRICE = 0;
                    mStockUpdate.FREE_QTY = 0;
                    mStockUpdate.BATCHNO = "NA";
                    mStockUpdate.UOM_ID = mRecipeMaster.intProductionMUnitCode;
                    mStockUpdate.ExecuteQuery();

                    //----------------Insert Finished Goods Detail-------------
                    spInserttblSplitItemDetail mRecipeDetail = new spInserttblSplitItemDetail();
                    mRecipeDetail.Connection = mConnection;
                    mRecipeDetail.Transaction = mTransaction;

                    UspProcessStockRegister mStockDetail = new UspProcessStockRegister();
                    mStockDetail.Connection = mConnection;
                    mStockDetail.Transaction = mTransaction;
                    decimal GLProduction = 0;
                    foreach (DataRow dr in dtFinishedDetail.Rows)
                    {
                        mRecipeDetail.lngSplitItemCode = mRecipeMaster.lngSplitItemCode;
                        mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString());
                        mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                        mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                        mRecipeDetail.ExecuteQuery();

                        mStockDetail.PRINCIPAL_ID = 0;
                        mStockDetail.TYPE_ID = 17;
                        mStockDetail.DISTRIBUTOR_ID = mRecipeMaster.DISTRIBUTOR_ID;
                        mStockDetail.STOCK_DATE = mRecipeMaster.DOCUMENT_DATE;
                        mStockDetail.SKU_ID = mRecipeDetail.SKU_ID;
                        mStockDetail.STOCK_QTY = mRecipeDetail.QUANTITY;
                        mStockDetail.PRICE = 0;
                        mStockDetail.FREE_QTY = 0;
                        mStockDetail.BATCHNO = "NA";
                        mStockDetail.UOM_ID = mRecipeDetail.intStockMUnitCode;
                        mStockDetail.ExecuteQuery();
                        GLProduction += mRecipeDetail.QUANTITY * mRecipeDetail.Price;
                    }

                    if (IsFinanceIntegrate)
                    {
                        //Dr Consumption
                        //Cr Stock in Trade
                        DataRow[] drConfig = null;
                        LedgerController LController = new LedgerController();
                        string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, p_DOCUMENT_DATE);
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, 0, VoucherNo, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.CashPayment, Convert.ToString(mRecipeMaster.lngSplitItemCode), "Split Item Voucher, SplitID#: " + mRecipeMaster.lngSplitItemCode.ToString(), p_USER_ID, "SplitItem", Constants.Document_SplitItem, mRecipeMaster.lngSplitItemCode))
                        {
                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Consumption + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), mRecipeMaster.SplitQty, 0, "Consumption Sale Voucher");

                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, mRecipeMaster.SplitQty, "Inventoryatstore Sale Voucher");
                        }

                        if(GLProduction>0)
                        {

                        }
                    }
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

        public bool UpdateSplitItem(long MASTER_ID,int p_DISTRIBUTOR_ID,DateTime p_DOCUMENT_DATE,int p_FinishedSKUID,int p_intProductionMUnitCode, decimal p_SplitQty, int p_USER_ID, DataTable dtRecipeDetail, bool IsFinanceIntegrate, DataTable dtCOAConfig)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdatetblSplitItemMaster mRecipeMaster = new spUpdatetblSplitItemMaster();
                mRecipeMaster.Connection = mConnection;

                //------------Insert Recipe Master----------\\

                if (dtRecipeDetail.Rows.Count > 0)
                {
                    UspUpdatePurchaseDetailStock mRecipeStock = new UspUpdatePurchaseDetailStock();
                    mRecipeStock.Connection = mConnection;

                    foreach (DataRow dr in dtRecipeDetail.Rows)
                    {
                        mRecipeStock.TYPEID = 22;
                        mRecipeStock.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mRecipeStock.PURCHASE_DETAIL_ID = 0;
                        mRecipeStock.PURCHASE_MASTER_ID = MASTER_ID;
                        mRecipeStock.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeStock.DATE = p_DOCUMENT_DATE;
                        mRecipeStock.QTY = decimal.Parse(dr["QUANTITY"].ToString());
                        mRecipeStock.ExecuteQuery();
                    }

                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.SplitQty = p_SplitQty;
                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.lngSplitItemCode = MASTER_ID;
                    mRecipeMaster.IsActive = true;

                    mRecipeMaster.ExecuteQuery();

                    UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                    mStockUpdate.Connection = mConnection;

                    mStockUpdate.PRINCIPAL_ID = 0;
                    mStockUpdate.TYPE_ID = 18;
                    mStockUpdate.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mStockUpdate.STOCK_DATE = p_DOCUMENT_DATE;
                    mStockUpdate.SKU_ID = p_FinishedSKUID;
                    mStockUpdate.STOCK_QTY = mRecipeMaster.SplitQty;
                    mStockUpdate.PRICE = 0;
                    mStockUpdate.FREE_QTY = 0;
                    mStockUpdate.BATCHNO = "NA";
                    mStockUpdate.UOM_ID = p_intProductionMUnitCode;
                    mStockUpdate.ExecuteQuery();


                    //----------------Insert Finished Goods Detail-------------
                    spInserttblSplitItemDetail mRecipeDetail = new spInserttblSplitItemDetail();
                    mRecipeDetail.Connection = mConnection;

                    UspProcessStockRegister mStockDetail = new UspProcessStockRegister();
                    mStockDetail.Connection = mConnection;

                    decimal GLProduction = 0;
                    foreach (DataRow dr in dtRecipeDetail.Rows)
                    {
                        mRecipeDetail.lngSplitItemCode = MASTER_ID;
                        mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString());
                        mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                        mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                        mRecipeDetail.ExecuteQuery();

                        mStockDetail.PRINCIPAL_ID = 0;
                        mStockDetail.TYPE_ID = 17;
                        mStockDetail.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mStockDetail.STOCK_DATE = p_DOCUMENT_DATE;
                        mStockDetail.SKU_ID = mRecipeDetail.SKU_ID;
                        mStockDetail.STOCK_QTY = mRecipeDetail.QUANTITY;
                        mStockDetail.PRICE = 0;
                        mStockDetail.FREE_QTY = 0;
                        mStockDetail.BATCHNO = "NA";
                        mStockDetail.UOM_ID = mRecipeDetail.intStockMUnitCode;
                        mStockDetail.ExecuteQuery();
                        GLProduction += mRecipeDetail.QUANTITY * mRecipeDetail.Price;
                    }

                    if (IsFinanceIntegrate)
                    {
                        //Dr Consumption
                        //Cr Stock in Trade
                        DataRow[] drConfig = null;
                        LedgerController LController = new LedgerController();
                        string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, p_DOCUMENT_DATE);
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, 0, VoucherNo, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.CashPayment, Convert.ToString(mRecipeMaster.lngSplitItemCode), "Split Item Voucher, SplitID#: " + mRecipeMaster.lngSplitItemCode.ToString(), p_USER_ID, "SplitItem", Constants.Document_SplitItem, mRecipeMaster.lngSplitItemCode))
                        {
                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Consumption + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), mRecipeMaster.SplitQty, 0, "Consumption Sale Voucher");

                            drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, mRecipeMaster.SplitQty, "Inventoryatstore Sale Voucher");
                        }

                        if(GLProduction>0)
                        {

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

        public bool DeleteSplitItem(long MASTER_ID, int p_DISTRIBUTOR_ID, DateTime p_DOCUMENT_DATE, int p_USER_ID)
        {

            IDbConnection mConnection = null;
            DataTable dtRecipeDetail = new DataTable();
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdatetblSplitItemMaster mRecipeMaster = new spUpdatetblSplitItemMaster();
                mRecipeMaster.Connection = mConnection;

                spSelecttblSplitItemMaster mSelectRecipe = new spSelecttblSplitItemMaster();
                mSelectRecipe.Connection = mConnection;
                
                mSelectRecipe.lngSplitItemCode = MASTER_ID;
                mSelectRecipe.TYPE_ID = 2;
                mSelectRecipe.DATE = Constants.DateNullValue;
                dtRecipeDetail = mSelectRecipe.ExecuteTable();

                //------------Insert Recipe Master----------\\

                if (dtRecipeDetail.Rows.Count > 0)
                {
                    UspUpdatePurchaseDetailStock mRecipeStock = new UspUpdatePurchaseDetailStock();
                    mRecipeStock.Connection = mConnection;
                    decimal SplitQty = 0;
                    foreach (DataRow dr in dtRecipeDetail.Rows)
                    {
                        mRecipeStock.TYPEID = 22;
                        mRecipeStock.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mRecipeStock.PURCHASE_DETAIL_ID = 0;
                        mRecipeStock.PURCHASE_MASTER_ID = MASTER_ID;
                        mRecipeStock.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeStock.DATE = p_DOCUMENT_DATE;
                        mRecipeStock.QTY = decimal.Parse(dr["QUANTITY"].ToString());
                        mRecipeStock.ExecuteQuery();
                        SplitQty += decimal.Parse(dr["QUANTITY"].ToString());
                    }

                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.SplitQty = SplitQty;
                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.lngSplitItemCode = MASTER_ID;
                    mRecipeMaster.IsActive = false;
                    mRecipeMaster.ExecuteQuery();

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