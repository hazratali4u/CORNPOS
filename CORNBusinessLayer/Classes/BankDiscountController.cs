using System;
using System.Data;
using CORNDatabaseLayer.Classes;
using CORNDataAccessLayer.Classes;
using CORNCommon.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Cheque Related Tasks
    /// <example>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// Insert Cheque
    /// </item>
    /// <item>
    /// Update Cheque
    /// </item>
    /// <item>
    /// Get Cheque Data
    /// </item>
    /// <item>
    /// Rollback Cheque etc.
    /// </item>
    /// </list>
    /// </remarks>
    /// </example>
    /// </summary>
    public class BankDiscountController
    {
        #region Constructor

        /// <summary>
        /// Constructor for BankDiscountController
        /// </summary>
		public BankDiscountController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        public bool InsertBankDiscount(int p_LocationID,string p_DiscountName,decimal p_DiscountPer,decimal p_BankPortion, decimal p_Limit,string p_CardNo, string p_Description,DateTime p_FromDate,DateTime p_ToDate,int p_UserID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspInsertBankDiscount objBankDiscount = new uspInsertBankDiscount();
                objBankDiscount.Connection = mConnection;
                objBankDiscount.LocationID = p_LocationID;
                objBankDiscount.DiscountName = p_DiscountName;
                objBankDiscount.CardNo = p_CardNo;
                objBankDiscount.DiscountPer = p_DiscountPer;
                objBankDiscount.BankPortion = p_BankPortion;
                objBankDiscount.Limit = p_Limit;
                objBankDiscount.Description = p_Description;
                objBankDiscount.FromDate = p_FromDate;
                objBankDiscount.ToDate = p_ToDate;
                objBankDiscount.UserID = p_UserID;
                return objBankDiscount.ExecuteQuery();
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

        public bool UpdateBankDiscount(long p_BankDiscountID, int p_LocationID, string p_DiscountName, decimal p_DiscountPer,decimal p_BankPortion, decimal p_Limit,string p_CardNo, string p_Description, DateTime p_FromDate, DateTime p_ToDate, int p_UserID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspUpdateBankDiscount objBankDiscount = new uspUpdateBankDiscount();
                objBankDiscount.Connection = mConnection;
                objBankDiscount.BankDiscountID = p_BankDiscountID;
                objBankDiscount.LocationID = p_LocationID;
                objBankDiscount.DiscountName = p_DiscountName;
                objBankDiscount.DiscountPer = p_DiscountPer;
                objBankDiscount.BankPortion = p_BankPortion;
                objBankDiscount.Limit = p_Limit;
                objBankDiscount.CardNo = p_CardNo;
                objBankDiscount.Description = p_Description;
                objBankDiscount.FromDate = p_FromDate;
                objBankDiscount.ToDate = p_ToDate;
                objBankDiscount.UserID = p_UserID;
                return objBankDiscount.ExecuteQuery();
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

        public DataTable GetBankDiscount(string p_BankDiscountID,int p_TypeID)
        {
            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspGetBankDiscount objBankDiscount = new uspGetBankDiscount();
                objBankDiscount.Connection = mConnection;
                objBankDiscount.BankDiscountID = p_BankDiscountID;
                objBankDiscount.TypeID = p_TypeID;
                DataTable dt = objBankDiscount.ExecuteTable();
                return dt;

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
                //return exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public DataTable GetBankDiscount(string p_BankDiscountID, int p_TypeID,string p_LocationID,DateTime p_FromDate,DateTime p_ToDate)
        {
            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspGetBankDiscount objBankDiscount = new uspGetBankDiscount();
                objBankDiscount.Connection = mConnection;
                objBankDiscount.BankDiscountID = p_BankDiscountID;
                objBankDiscount.TypeID = p_TypeID;
                objBankDiscount.LocationID = p_LocationID;
                objBankDiscount.FromDate = p_FromDate;
                objBankDiscount.ToDate = p_ToDate;
                DataTable dt = objBankDiscount.ExecuteTable();
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
    }
}