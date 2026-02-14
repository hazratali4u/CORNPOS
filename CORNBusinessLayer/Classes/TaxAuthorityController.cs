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
	public class TaxAuthorityController
    {
        #region Constructors

        /// <summary>
        /// Constructor For TaxAuthorityController
        /// </summary>
        public TaxAuthorityController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region Select
        public DataTable GetTaxAuthority(int p_FBRIntegrationID, int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetTaxAuthority mTax = new uspGetTaxAuthority();
                mTax.Connection = mConnection;

                mTax.FBRIntegrationID = p_FBRIntegrationID;
                mTax.TypeID = p_TypeID;
                DataTable ds = mTax.ExecuteTable();

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
        public DataTable GetTaxAuthority(int p_FBRIntegrationID, int p_TypeID,int p_DistributorID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetTaxAuthority mTax = new uspGetTaxAuthority();
                mTax.Connection = mConnection;

                mTax.FBRIntegrationID = p_FBRIntegrationID;
                mTax.TypeID = p_TypeID;
                mTax.DistributorID = p_DistributorID;
                DataTable ds = mTax.ExecuteTable();

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

        public bool InsertTaxAuthority(int p_DistributorID,string p_POSID,string p_Token,string p_FBRURL,string p_TaxAuthorityLabel, int p_UserID)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                uspInsertTaxAuthorityIntegration mTax = new uspInsertTaxAuthorityIntegration();
                mTax.Connection = mConnection;
                mTax.Transaction = mTransaction;
                mTax.DistributorID = p_DistributorID;
                mTax.Token = p_Token;
                mTax.POSID = p_POSID;
                mTax.FBRURL = p_FBRURL;
                mTax.UserID = p_UserID;
                mTax.TaxAuthorityLabel = p_TaxAuthorityLabel;
                mTax.ExecuteQuery();
                mTransaction.Commit();
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

        public bool UpdateTaxAuthority(int p_FBRIntegrationID, int p_DistributorID, string p_POSID, string p_Token, string p_FBRURL,string p_TaxAuthorityLabel, int p_UserID)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspUpdateTaxAuthorityIntegration mTax = new uspUpdateTaxAuthorityIntegration();
                mTax.Connection = mConnection;

                mTax.FBRIntegrationID = p_FBRIntegrationID;
                mTax.DistributorID = p_DistributorID;
                mTax.POSID = p_POSID;
                mTax.FBRURL = p_FBRURL;
                mTax.Token = p_Token;
                mTax.UserID = p_UserID;
                mTax.TypeID = 1;
                mTax.TaxAuthorityLabel = p_TaxAuthorityLabel;
                mTax.ExecuteQuery();

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

        public bool DeleteTaxAuthority(int MASTER_ID, int p_USER_ID)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspUpdateTaxAuthorityIntegration mTax = new uspUpdateTaxAuthorityIntegration();
                mTax.Connection = mConnection;

                mTax.FBRIntegrationID = MASTER_ID;
                mTax.UserID = p_USER_ID;
                mTax.TypeID = 2;
                mTax.ExecuteQuery();

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
        #endregion
    }
}