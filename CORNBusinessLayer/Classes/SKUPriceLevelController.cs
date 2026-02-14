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
	public class SKUPriceLevelController
    {
        #region Constructors

        /// <summary>
        /// Constructor For SKUPriceLevelController
        /// </summary>
        public SKUPriceLevelController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion
        
        #region public Methods
        
        #region Select

        public DataTable GetItemPriceLevel(int p_SKU_ID, int p_TypeID, long p_PriceLevelID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetItemPriceLevel mspSelectSkuInfo = new uspGetItemPriceLevel();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SKU_ID = p_SKU_ID;
                mspSelectSkuInfo.TypeID = p_TypeID;
                mspSelectSkuInfo.SKU_PRICES_LEVEL_ID = p_PriceLevelID;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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

        #region Insert, Update

        public bool InsertItemPriceLevel(int p_DistributorId, string p_PriceLvelName,
            int p_UserId, bool p_isPercentWise, DataTable dtPriceDetail)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            DataControl DC = new DataControl();

            mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
            mConnection.Open();
            mTransaction = ProviderFactory.GetTransaction(mConnection);

            try
            {
                uspInsertPriceLevelMaster mSKUS = new uspInsertPriceLevelMaster();
                mSKUS.Connection = mConnection;
                mSKUS.Transaction = mTransaction;
                mSKUS.DISTRIBUTOR_ID = p_DistributorId;
                mSKUS.PriceLevelName = p_PriceLvelName;
                mSKUS.USER_ID = p_UserId;
                mSKUS.IsPercentWise = p_isPercentWise;
                mSKUS.ExecuteQuery();

                uspInsertPriceLeveDetail mSaleInvoiceDetail = new uspInsertPriceLeveDetail
                {
                    Connection = mConnection,
                    Transaction = mTransaction
                };
                foreach (DataRow dr in dtPriceDetail.Rows)
                {
                    mSaleInvoiceDetail.SKU_PRICES_LEVEL_ID = mSKUS.SKU_PRICES_LEVEL_ID;
                    mSaleInvoiceDetail.PRICE = decimal.Parse(dr["PRICE"].ToString());
                    mSaleInvoiceDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());                    
                    mSaleInvoiceDetail.ExecuteQuery();
                }

                mTransaction.Commit();
                return true;
            }
            catch (Exception)
            {
                mTransaction.Rollback();
                mConnection.Close();
                return false;
            }
            finally
            {
                mConnection.Close();
            }
        }

        public bool UpdateItemPriceLevel(long p_SKU_PRICES_LEVEL_ID,int p_DistributorId,
            string p_PriceLvelName, int p_UserId,int p_TypeID, bool p_isPercentWise, DataTable dtPriceDetail)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            DataControl DC = new DataControl();

            mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
            mConnection.Open();
            mTransaction = ProviderFactory.GetTransaction(mConnection);

            try
            {
                uspUpdatePriceLevelMaster mSKUS = new uspUpdatePriceLevelMaster();
                mSKUS.Connection = mConnection;
                mSKUS.Transaction = mTransaction;
                mSKUS.DISTRIBUTOR_ID = p_DistributorId;
                mSKUS.PriceLevelName = p_PriceLvelName;
                mSKUS.USER_ID = p_UserId;
                mSKUS.TypeID = p_TypeID;
                mSKUS.SKU_PRICES_LEVEL_ID = p_SKU_PRICES_LEVEL_ID;
                mSKUS.IsPercentWise = p_isPercentWise;
                mSKUS.ExecuteQuery();

                uspInsertPriceLeveDetail mSaleInvoiceDetail = new uspInsertPriceLeveDetail
                {
                    Connection = mConnection,
                    Transaction = mTransaction
                };
                foreach (DataRow dr in dtPriceDetail.Rows)
                {
                    mSaleInvoiceDetail.SKU_PRICES_LEVEL_ID = mSKUS.SKU_PRICES_LEVEL_ID;
                    mSaleInvoiceDetail.PRICE = decimal.Parse(dr["PRICE"].ToString());
                    mSaleInvoiceDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                    mSaleInvoiceDetail.ExecuteQuery();
                }

                mTransaction.Commit();
                return true;
            }
            catch (Exception)
            {
                mTransaction.Rollback();
                mConnection.Close();
                return false;
            }
            finally
            {
                mConnection.Close();
            }
        }

        public bool DeleteItemPriceLevel(long p_SKU_PRICES_LEVEL_ID,int p_UserId, int p_TypeID)
        {
            IDbConnection mConnection = null;
            DataControl DC = new DataControl();

            mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
            mConnection.Open();

            try
            {
                uspUpdatePriceLevelMaster mSKUS = new uspUpdatePriceLevelMaster();
                mSKUS.Connection = mConnection;
                mSKUS.USER_ID = p_UserId;
                mSKUS.TypeID = p_TypeID;
                mSKUS.SKU_PRICES_LEVEL_ID = p_SKU_PRICES_LEVEL_ID;
                mSKUS.ExecuteQuery();                
                return true;
            }
            catch (Exception)
            {
                mConnection.Close();
                return false;
            }
            finally
            {
                mConnection.Close();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
