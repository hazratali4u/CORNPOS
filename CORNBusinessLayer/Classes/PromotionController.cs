using System;
using System.Data;
using CORNDatabaseLayer.Classes;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Promotion Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Promotion
    /// </item>
    /// <term>
    /// Update Promotion
    /// </term>
    /// <item>
    /// Get Promotion
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class PromotionController
	{
        #region Constructor

		public PromotionController()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        #endregion

        #region Public Methods

        #region Select
        public DataTable GetSalePromotion(long p_SalePromotionID, int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetSalePromotion mPromotionSale = new uspGetSalePromotion();
                mPromotionSale.Connection = mConnection;
                mPromotionSale.SalePromotionID = p_SalePromotionID;
                mPromotionSale.TypeID = p_TypeID;
                DataTable dt = mPromotionSale.ExecuteTable();
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
        public DataTable GetPromotionCustomerVolumeClass(long p_promotionId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectPROMOTION_CUSTOMER_VOLUMECLASS mPromotionCustomerType = new spSelectPROMOTION_CUSTOMER_VOLUMECLASS();
                mPromotionCustomerType.Connection = mConnection;
                mPromotionCustomerType.PROMOTION_ID = p_promotionId;
                DataTable dt = mPromotionCustomerType.ExecuteTable();
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
        public DataTable GetPromotionServiceType(long p_promotionId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectPROMOTION_CUSTOMER_TYPE mPromotionCustomerType = new spSelectPROMOTION_CUSTOMER_TYPE();
                mPromotionCustomerType.Connection = mConnection;
                mPromotionCustomerType.PROMOTION_ID = p_promotionId;
                DataTable dt = mPromotionCustomerType.ExecuteTable();
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
        public DataTable GetPromotionDays(long p_promotionId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectPROMOTION_DAYS mPromotionDay = new spSelectPROMOTION_DAYS();
                mPromotionDay.Connection = mConnection;
                mPromotionDay.PROMOTION_ID = p_promotionId;
                DataTable dt = mPromotionDay.ExecuteTable();
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
        public DataTable GetPromotionDistributors(long p_promotionId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectPROMOTION_FOR mSelectPromotionFor = new spSelectPROMOTION_FOR();
                mSelectPromotionFor.ASSIGNED_DISTRIBUTOR_ID = Constants.IntNullValue;
                mSelectPromotionFor.Connection = mConnection;
                mSelectPromotionFor.DISTRIBUTOR_ID = Configuration.DistributorId;
                mSelectPromotionFor.PROMOTION_FOR_ID = Constants.LongNullValue;
                mSelectPromotionFor.PROMOTION_ID = p_promotionId;
                mSelectPromotionFor.SCHEME_ID = Constants.IntNullValue;
                DataTable dt = mSelectPromotionFor.ExecuteTable();
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
        public DataTable SelectPromotionWithSchemeInfo(int p_Distributor_Id, int p_Promotion_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspSelectPromotionWithSchemes mPromotion = new uspSelectPromotionWithSchemes();
                mPromotion.Connection = mConnection;

                mPromotion.PROMOTION_ID = p_Promotion_Id;
                mPromotion.DISTRIBUTOR_ID = p_Distributor_Id;

                DataTable dt = mPromotion.ExecuteTable();
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
        public DataTable SelectPromotion(string P_start, string P_end, int p_Principal_ID, int P_UserId, bool p_PActive)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectPromotionsScheme mspscheme = new uspSelectPromotionsScheme();
                mspscheme.Connection = mConnection;
                mspscheme.FromDate = DateTime.Parse(P_start);
                mspscheme.To_Date = DateTime.Parse(P_end);
                mspscheme.PRINCIPAL_ID = p_Principal_ID;
                mspscheme.USER_ID = P_UserId;
                mspscheme.IS_ACTIVE = p_PActive;
                DataTable dt = mspscheme.ExecuteTable();
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
        public DataTable GetPromotion(int p_Distributor_Id, DateTime p_Working_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetPromotionDetail mPromotionDetail = new uspGetPromotionDetail();
                mPromotionDetail.Connection = mConnection;
                mPromotionDetail.DISTRIBUTOR_ID = p_Distributor_Id;
                mPromotionDetail.WORKING_DATE = p_Working_Date;
                DataTable dt = mPromotionDetail.ExecuteTable();
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

        #region Insert, Update

        public bool InsertSalePromotion(string p_PromotionName, string p_ImagePath, int p_LocationID, int p_UserID, DataTable dtSKUs)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {

                if (dtSKUs.Rows.Count > 0)
                {
                    mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                    mConnection.Open();
                    mTransaction = ProviderFactory.GetTransaction(mConnection);

                    uspInsertSalePromotion mSale = new uspInsertSalePromotion
                    {
                        Connection = mConnection,
                        Transaction = mTransaction,
                        PromotionName = p_PromotionName,
                        ImagePath = p_ImagePath,
                        LocationID = p_LocationID,
                        UserID = p_UserID
                    };
                    mSale.ExecuteQuery();
                    //----------------Insert into sale order detail-------------
                    uspInsertSalePromotionSKU mSaleDetail = new uspInsertSalePromotionSKU
                    {
                        Connection = mConnection,
                        Transaction = mTransaction
                    };
                    foreach (DataRow dr in dtSKUs.Rows)
                    {
                        mSaleDetail.SalePromotionID = mSale.SalePromotionID;
                        mSaleDetail.SKUID = int.Parse(dr["SKUID"].ToString());
                        mSaleDetail.ExecuteQuery();
                    }
                    mTransaction.Commit();
                    return true;
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
            return true;
        }
        public bool UpdateSalePromotion(long p_SalePromotionID, string p_PromotionName, string p_ImagePath, int p_LocationID, int p_UserID, DataTable dtSKUs)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {

                if (dtSKUs.Rows.Count > 0)
                {
                    mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                    mConnection.Open();
                    mTransaction = ProviderFactory.GetTransaction(mConnection);

                    uspUpdateSalePromotion mSale = new uspUpdateSalePromotion
                    {
                        Connection = mConnection,
                        Transaction = mTransaction,
                        SalePromotionID = p_SalePromotionID,
                        PromotionName = p_PromotionName,
                        ImagePath = p_ImagePath,
                        LocationID = p_LocationID,
                        UserID = p_UserID
                    };
                    mSale.ExecuteQuery();
                    //----------------Insert into sale order detail-------------
                    uspInsertSalePromotionSKU mSaleDetail = new uspInsertSalePromotionSKU
                    {
                        Connection = mConnection,
                        Transaction = mTransaction
                    };
                    foreach (DataRow dr in dtSKUs.Rows)
                    {
                        mSaleDetail.SalePromotionID = mSale.SalePromotionID;
                        mSaleDetail.SKUID = int.Parse(dr["SKUID"].ToString());
                        mSaleDetail.ExecuteQuery();
                    }
                    mTransaction.Commit();
                    return true;
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
            return true;
        }
        public string InsertPromotion(int p_Scheme_Id, int p_Distributor_Id, string p_Pro_Code, string p_Pro_Description, DateTime p_Promo_Date, bool p_Claimable, DateTime p_Start_Date, DateTime p_End_Date, DateTime p_START_TIME,DateTime p_END_TIME, bool p_Is_Active, int p_Promotion_Type, int p_Promotion_Selection, bool p_Is_Scheme, bool p_Promotion_For, int p_UserId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertPROMOTION mPromotion = new spInsertPROMOTION();
                mPromotion.Connection = mConnection;
                mPromotion.SCHEME_ID = p_Scheme_Id;
                mPromotion.DISTRIBUTOR_ID = p_Distributor_Id;
                mPromotion.PROMOTION_CODE = p_Pro_Code;
                mPromotion.PROMOTION_DESCRIPTION = p_Pro_Description;
                mPromotion.PROMO_DATE = p_Promo_Date;
                mPromotion.CLAIMABLE = p_Claimable;
                mPromotion.START_DATE = DateTime.Parse(p_Start_Date.ToShortDateString() + " 00:00:00");
                mPromotion.END_DATE = DateTime.Parse(p_End_Date.ToShortDateString() + " 23:59:59");
                mPromotion.START_TIME = p_START_TIME;
                mPromotion.END_TIME = p_END_TIME;
                mPromotion.IS_ACTIVE = p_Is_Active;
                mPromotion.PROMOTION_TYPE = p_Promotion_Type;
                mPromotion.PROMOTION_SELECTION = p_Promotion_Selection;
                mPromotion.IS_SCHEME = p_Is_Scheme;
                mPromotion.PROMOTION_FOR = p_Promotion_For;
                mPromotion.USER_ID = p_UserId;
                mPromotion.ExecuteQuery();
                return mPromotion.PROMOTION_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public string InsertPromotionDist(long p_Promotion_ID, int p_Scheme_ID, int p_Dist_ID, int p_Assigned_Dist_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertPROMOTION_FOR mPromotion = new spInsertPROMOTION_FOR();
                mPromotion.Connection = mConnection;
                mPromotion.PROMOTION_ID = p_Promotion_ID;
                mPromotion.DISTRIBUTOR_ID = p_Dist_ID;
                mPromotion.SCHEME_ID = p_Scheme_ID;
                mPromotion.ASSIGNED_DISTRIBUTOR_ID = p_Assigned_Dist_ID;
                mPromotion.ExecuteQuery();
                return mPromotion.PROMOTION_FOR_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public string InsertPromotionVolClass(long p_Promotion_ID, int p_Scheme_ID, int p_Dist_ID, int p_Customer_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertPROMOTION_CUSTOMER_VOLUMECLASS mPromotion = new spInsertPROMOTION_CUSTOMER_VOLUMECLASS();
                mPromotion.Connection = mConnection;
                mPromotion.SCHEME_ID = p_Scheme_ID;
                mPromotion.PROMOTION_ID = p_Promotion_ID;
                mPromotion.DISTRIBUTOR_ID = p_Dist_ID;
                mPromotion.CUSTOMER_VOLUMECLASS_ID = p_Customer_ID;
                mPromotion.ExecuteQuery();
                return mPromotion.PROMOTION_VOLUMECLASS_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public string InsertPromotionServiceType(long p_Promotion_ID, int p_Scheme_ID, int p_Dist_ID, int p_CustomerType_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertPROMOTION_CUSTOMER_TYPE mPromotion = new spInsertPROMOTION_CUSTOMER_TYPE();
                mPromotion.Connection = mConnection;
                mPromotion.SCHEME_ID = p_Scheme_ID;
                mPromotion.PROMOTION_ID = p_Promotion_ID;
                mPromotion.DISTRIBUTOR_ID = p_Dist_ID;
                mPromotion.CUSTOMER_TYPE_ID = p_CustomerType_ID;
                mPromotion.ExecuteQuery();
                return mPromotion.PROMOTION_CUSTOMER_TYPE_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public string InsertPromotionDay(long p_Promotion_ID, int p_Scheme_ID, int p_DAY_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertPROMOTION_DAYS mPromotion = new spInsertPROMOTION_DAYS();
                mPromotion.Connection = mConnection;
                mPromotion.SCHEME_ID = p_Scheme_ID;
                mPromotion.PROMOTION_ID = p_Promotion_ID;
                mPromotion.DAY_ID = p_DAY_ID;
                mPromotion.ExecuteQuery();
                return mPromotion.PROMOTION_DAYS_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public string InsertPromotionOffer(long p_Basket_ID, long p_Promotion_ID, int p_Scheme_ID, int p_Dist_ID, long p_BasketDetail_ID, int p_Quantity, decimal p_Offer_Value, float p_Discount, bool p_Is_And, int p_SKU_ID, int p_UOM_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertPROMOTION_OFFER mPromotion = new spInsertPROMOTION_OFFER();
                mPromotion.Connection = mConnection;
                mPromotion.IS_AND = p_Is_And;
                mPromotion.DISCOUNT = p_Discount;
                mPromotion.QUANTITY = p_Quantity;
                mPromotion.SKU_ID = p_SKU_ID;
                mPromotion.UOM_ID = p_UOM_ID;
                mPromotion.SCHEME_ID = p_Scheme_ID;
                mPromotion.DISTRIBUTOR_ID = p_Dist_ID;
                mPromotion.BASKET_DETAIL_ID = p_BasketDetail_ID;
                mPromotion.BASKET_ID = p_Basket_ID;
                mPromotion.PROMOTION_ID = p_Promotion_ID;
                mPromotion.OFFER_VALUE = p_Offer_Value;
                mPromotion.ExecuteQuery();
                return mPromotion.PROMOTION_OFFER_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public string UpdatePromotion(long p_Promotion_Id, int p_Scheme_Id, int p_Distributor_Id, string p_Pro_Code, string p_Pro_Description, DateTime p_Promo_Date, bool p_Claimable, DateTime p_Start_Date, DateTime p_End_Date, bool p_Is_Active, int p_Promotion_Type, int p_Promotion_Selection)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdatePROMOTION mPromotion = new spUpdatePROMOTION();
                mPromotion.Connection = mConnection;
                mPromotion.PROMOTION_ID = p_Promotion_Id;
                mPromotion.SCHEME_ID = p_Scheme_Id;
                mPromotion.DISTRIBUTOR_ID = p_Distributor_Id;
                mPromotion.PROMOTION_CODE = p_Pro_Code;
                mPromotion.PROMOTION_DESCRIPTION = p_Pro_Description;
                mPromotion.PROMO_DATE = p_Promo_Date;
                mPromotion.CLAIMABLE = p_Claimable;
                mPromotion.START_DATE = p_Start_Date;
                mPromotion.END_DATE = p_End_Date;
                mPromotion.IS_ACTIVE = p_Is_Active;
                mPromotion.PROMOTION_TYPE = p_Promotion_Type;
                mPromotion.PROMOTION_SELECTION = p_Promotion_Selection;
                mPromotion.ExecuteQuery();
                return "Record Updated";
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return exp.Message;
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