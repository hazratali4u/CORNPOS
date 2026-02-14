using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For SKU Group Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert SKU Group
    /// </item>
    /// <term>
    /// Update SKU Group
    /// </term>
    /// <item>
    /// Get SKU Group
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class SKUGroupController
    {
        #region Constructors

        /// <summary>
        /// Constructor For SKUGroupController
        /// </summary>
        public SKUGroupController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region Public Methods

        #region Select
        
        /// <summary>
        /// Gets SKU Group Detail
        /// </summary>
        /// <remarks>
        /// Returns SKU Group Detail Data as Datatable
        /// </remarks>
        /// <param name="Companyid">Company</param>
        /// <returns>SKU Group Detail Data as Datatable</returns>
        public DataTable GET_SKUGroupDetail(int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                UspGetGroup_Detail ObjSelect = new UspGetGroup_Detail();
                ObjSelect.Connection = mConnection;
                ObjSelect.COMPANY_ID = Companyid;
                DataTable dt = ObjSelect.ExecuteTable();
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
        public DataTable GET_Group_ID(int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                UspGetGroup_ID ObjSelect = new UspGetGroup_ID();
                ObjSelect.Connection = mConnection;
                ObjSelect.COMPANY_ID = Companyid;
                DataTable dt = ObjSelect.ExecuteTable();
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
        public DataTable GET_UniqueGroupName(int p_SKU_GROUP_ID, string p_GROUP_NAME, DateTime p_TIMESTAMP, bool p_ISACTIVE, int p_UserId, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSKU_GROUP ObjSelect = new spSelectSKU_GROUP();
                ObjSelect.Connection = mConnection;
                ObjSelect.SKU_GROUP_ID = p_SKU_GROUP_ID;
                ObjSelect.GROUP_NAME = p_GROUP_NAME;
                ObjSelect.COMPANY_ID = p_UserId;
                ObjSelect.USER_ID = Companyid;
                DataTable dt = ObjSelect.ExecuteTable();
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
        public DataTable SelectSKUGroup(int p_Sku_Group_Id, string p_Sku_Group_Name, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSKU_GROUP mSkus = new spSelectSKU_GROUP();
                mSkus.Connection = mConnection;
                mSkus.SKU_GROUP_ID = p_Sku_Group_Id;
                mSkus.GROUP_NAME = p_Sku_Group_Name;
                mSkus.COMPANY_ID = Companyid;
                DataTable dt = mSkus.ExecuteTable();
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

        public DataTable GetSKUGroupDetail()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetGroupDetal mSkus = new uspGetGroupDetal();
                mSkus.Connection = mConnection;
                DataTable dt = mSkus.ExecuteTable();
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

        public string Insert_SKUGroup(string p_GROUP_NAME, DateTime p_TIMESTAMP, bool p_ISACTIVE, int p_UserId, int p_PrincipalId, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spInsertSKU_GROUP ObjInsert = new spInsertSKU_GROUP();
                ObjInsert.Connection = mConnection;
                ObjInsert.GROUP_NAME = p_GROUP_NAME;
                ObjInsert.TIME_STAMP = p_TIMESTAMP;
                ObjInsert.COMPANY_ID = Companyid;
                ObjInsert.USER_ID = p_UserId;
                ObjInsert.LASTUPDATE_DATE = System.DateTime.Now;
                ObjInsert.PRINCIPAL_ID = p_PrincipalId;
                ObjInsert.ExecuteQuery();
                return ObjInsert.SKU_GROUP_ID.ToString();
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
        public bool Insert_SKU_GroupDetail(int p_SKU_GROUP_ID, int p_SKU_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertSKU_GROUP_DETAIL ObjInsert = new spInsertSKU_GROUP_DETAIL();
                ObjInsert.Connection = mConnection;
                ObjInsert.SKU_GROUP_ID = p_SKU_GROUP_ID;
                ObjInsert.SKU_ID = p_SKU_ID;

                bool Bvalue = ObjInsert.ExecuteQuery();
                return Bvalue;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
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
        public bool UpdateSKUinGroup(int p_SKU_GROUP_ID, int p_SKU_ID, string p_GROUP_NAME, bool p_ISACTIVE, int p_PrincipalId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspUpdateSKUinGroup ObjInsert = new UspUpdateSKUinGroup();
                ObjInsert.Connection = mConnection;
                ObjInsert.SKU_GROUP_ID = p_SKU_GROUP_ID;
                ObjInsert.SKU_ID = p_SKU_ID;
                ObjInsert.GROUP_NAME = p_GROUP_NAME;
                ObjInsert.ISACTIVE = p_ISACTIVE;
                ObjInsert.PRINCIPAL_ID = p_PrincipalId;
                bool Bvalue = ObjInsert.ExecuteQuery();
                return Bvalue;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
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