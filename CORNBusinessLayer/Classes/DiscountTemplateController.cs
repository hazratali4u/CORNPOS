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
    public class DiscountTemplateController
    {
        #region Constructor

        /// <summary>
        /// Constructor for DiscountTemplateController
        /// </summary>
		public DiscountTemplateController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        public bool InsertDiscountTemplate(int p_LocationID,string p_DiscountTypeName,decimal p_DiscountValue, byte p_DiscountBehaviour,int p_UserID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspInsertDiscountTemplate objDiscount = new uspInsertDiscountTemplate();
                objDiscount.Connection = mConnection;
                objDiscount.DistributorID = p_LocationID;
                objDiscount.DiscountTypeName = p_DiscountTypeName;
                objDiscount.DiscountValue = p_DiscountValue;
                objDiscount.DiscountBehaviour = p_DiscountBehaviour;
                objDiscount.UserID = p_UserID;
                return objDiscount.ExecuteQuery();
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
        public bool UpdateDiscountTemplate(int p_EmployeeDiscountTypeID, string p_DiscountTypeName, decimal p_DiscountValue, byte p_DiscountBehaviour, int p_UserID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspUpdateDiscountTemplate objDiscount = new uspUpdateDiscountTemplate();
                objDiscount.Connection = mConnection;
                objDiscount.EmployeeDiscountTypeID = p_EmployeeDiscountTypeID;
                objDiscount.DiscountTypeName = p_DiscountTypeName;
                objDiscount.DiscountValue = p_DiscountValue;
                objDiscount.DiscountBehaviour = p_DiscountBehaviour;
                objDiscount.UserID = p_UserID;
                return objDiscount.ExecuteQuery();
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

        public DataTable GetDiscountTemplate(int p_TypeID,int p_DistributorID)
        {
            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspGetDiscountTemplate objBankDiscount = new uspGetDiscountTemplate();
                objBankDiscount.Connection = mConnection;
                objBankDiscount.TypeID = p_TypeID;
                objBankDiscount.DistributorID = p_DistributorID;
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

    }
}