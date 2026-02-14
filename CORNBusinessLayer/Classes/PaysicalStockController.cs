using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Physical Stock Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Stock
    /// </item>
    /// <term>
    /// Update Stock
    /// </term>
    /// <item>
    /// Get Stock
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class PhaysicalStockController
    {

        #region Constructors

        /// <summary>
        /// Constructor For PhaysicalStockController
        /// </summary>
        public PhaysicalStockController()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        #endregion

        #region Private Variables

        IDbTransaction mTransaction;
        IDbConnection mConnection;

        #endregion

        #region Public Methods

        #region Select
        
        public DataTable SelectSKUClosingStock2(int p_DISTRIBUTOR_ID, int p_SKU_ID, string p_BatchNo, DateTime p_StockDate, int pType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                mStockUpdate.Connection = mConnection;
                mStockUpdate.TYPE_ID = pType;
                mStockUpdate.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mStockUpdate.SKU_ID = p_SKU_ID;
                mStockUpdate.BATCHNO = p_BatchNo;
                mStockUpdate.STOCK_DATE = p_StockDate;
                DataTable dt = mStockUpdate.ExecuteTable();
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

        public DataTable SelectPhysicalStockTakingDetail(int p_DISTRIBUTOR_ID, long p_PhysiclaStockTaking_ID,int p_TYPE_ID)
        {
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectPhysicalStockTakingDetail mPurchaseDetail = new spSelectPhysicalStockTakingDetail();
                mPurchaseDetail.Connection = mConnection;
                mPurchaseDetail.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPurchaseDetail.PhysiclaStockTaking_ID = p_PhysiclaStockTaking_ID;
                mPurchaseDetail.TYPE_ID = p_TYPE_ID;
                DataTable dt = mPurchaseDetail.ExecuteTable();
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

        public DataTable SelectPhysicalStock(DateTime p_WorkingDate, int p_UserID, int p_TYPE_ID)
        {
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectPhysicalStockTakingDetail mPurchaseDetail = new spSelectPhysicalStockTakingDetail();
                mPurchaseDetail.Connection = mConnection;
                mPurchaseDetail.TYPE_ID = p_TYPE_ID;
                mPurchaseDetail.UserID = p_UserID;
                mPurchaseDetail.WorkingDate = p_WorkingDate;
                DataTable dt = mPurchaseDetail.ExecuteTable();
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

        #region Insert, Update, Delete                
        public long InsertPhysicalStockTaking(int p_DISTRIBUTOR_ID, DateTime p_DOCUMENT_DATE, string p_REMARKS, int p_USER_ID, DataTable dtPurchaseDetail)
        {
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spInsertPhysicalStockTaking mPurchaseMaster = new spInsertPhysicalStockTaking();
                mPurchaseMaster.Connection = mConnection;
                mPurchaseMaster.Transaction = mTransaction;
                mPurchaseMaster.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPurchaseMaster.DOCUMENT_DATE = p_DOCUMENT_DATE;
                mPurchaseMaster.USER_ID = p_USER_ID;
                mPurchaseMaster.REMARKS = p_REMARKS;
                mPurchaseMaster.ExecuteQuery();

                spInsertPhysicalStockTakingDetail mPurchaseDetail = new spInsertPhysicalStockTakingDetail();
                mPurchaseDetail.Connection = mConnection;
                mPurchaseDetail.Transaction = mTransaction;

                foreach (DataRow dr in dtPurchaseDetail.Rows)
                {
                    mPurchaseDetail.PhysiclaStockTaking_ID = mPurchaseMaster.PhysiclaStockTaking_ID;
                    mPurchaseDetail.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mPurchaseDetail.STOCK_DATE = p_DOCUMENT_DATE;
                    mPurchaseDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                    mPurchaseDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString());
                    mPurchaseDetail.UNIT_RATE = decimal.Parse(dr["UNIT_RATE"].ToString());
                    mPurchaseDetail.ExecuteQuery();
                }
                mTransaction.Commit();
                return mPurchaseMaster.PhysiclaStockTaking_ID;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
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

        public long UpdatePhysicalStockTaking(long p_PhysiclaStockTaking_ID, int p_DISTRIBUTOR_ID,DateTime p_DOCUMENT_DATE, int p_USER_ID,string p_REMARKS, DataTable dtPurchaseDetail)
        {
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spUpdatePhysicalStockTaking mPurchaseMaster = new spUpdatePhysicalStockTaking();
                mPurchaseMaster.Connection = mConnection;
                mPurchaseMaster.Transaction = mTransaction;
                mPurchaseMaster.PhysiclaStockTaking_ID = p_PhysiclaStockTaking_ID;
                mPurchaseMaster.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPurchaseMaster.USER_ID = p_USER_ID;
                mPurchaseMaster.REMARKS = p_REMARKS;
                mPurchaseMaster.ExecuteQuery();
                
                spInsertPhysicalStockTakingDetail mPurchaseDetail = new spInsertPhysicalStockTakingDetail();
                mPurchaseDetail.Connection = mConnection;
                mPurchaseDetail.Transaction = mTransaction;

                foreach (DataRow dr in dtPurchaseDetail.Rows)
                {
                    mPurchaseDetail.PhysiclaStockTaking_ID = mPurchaseMaster.PhysiclaStockTaking_ID;
                    mPurchaseDetail.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mPurchaseDetail.STOCK_DATE = p_DOCUMENT_DATE;
                    mPurchaseDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                    mPurchaseDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString());
                    mPurchaseDetail.UNIT_RATE = decimal.Parse(dr["UNIT_RATE"].ToString());
                    mPurchaseDetail.ExecuteQuery();
                }
                mTransaction.Commit();
                return mPurchaseMaster.PhysiclaStockTaking_ID;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
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

        public bool DeletePhysicalStockTaking(long p_Physical_Stock_ID, int p_USER_ID)
        {
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spInsertPhysicalStockTaking mPurchaseMaster = new spInsertPhysicalStockTaking();
                mPurchaseMaster.Connection = mConnection;
                mPurchaseMaster.Transaction = mTransaction;
                mPurchaseMaster.ID = p_Physical_Stock_ID;
                mPurchaseMaster.USER_ID = p_USER_ID;
                mPurchaseMaster.ExecuteQueryForDelete();

                mTransaction.Commit();
                return true;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                return false;
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