using System;
using CORNDatabaseLayer.Classes;
using CORNDataAccessLayer.Classes;
using CORNCommon.Classes;
using System.Data;
using CORNBusinessLayer.Classes;    

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Geo Hierarchy Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Geo Hierarchy
    /// </item>
    /// <term>
    /// Update Geo Hierarchy
    /// </term>
    /// <item>
    /// Get Geo Hierarchy
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class GeoHierarchyController
    {
        #region Table Defination

        public bool InsertTableDefination(int pDistributorId, string p_TableDefination_No, string p_TableDefination_Description,string p_TableDefination_Capacity, string p_TableDefination_Abbrivation, DateTime p_TIME_STAMP,int p_USER_ID,int p_FloorID,int p_SortID,int p_ParetnCategoryID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertTableDefination mTableDefination = new spInsertTableDefination();
                mTableDefination.Connection = mConnection;
                mTableDefination.distributorId = pDistributorId;
                mTableDefination.p_TableDefination_No = p_TableDefination_No;
                mTableDefination.p_TableDefination_Description = p_TableDefination_Description;
                mTableDefination.p_TableDefination_Capacity = p_TableDefination_Capacity;
                mTableDefination.p_TableDefination_Abbrivation = p_TableDefination_Abbrivation;
                mTableDefination.P_TIME_STAMP = p_TIME_STAMP;
                mTableDefination.p_USER_ID = p_USER_ID;
                mTableDefination.FloorID = p_FloorID;
                mTableDefination.SortID = p_SortID;
                mTableDefination.ParetnCategoryID = p_ParetnCategoryID;
                bool a = mTableDefination.ExecuteQuery();
                return a;
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
        public DataTable GetTableDefination(int p_TableDefination_ID, int pDistributorId, bool Is_Active, int pUserId)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectTableDefination mtblDefination = new spSelectTableDefination();
                mtblDefination.Connection = mConnection;
                mtblDefination.Active = Is_Active;
                mtblDefination.TableDefination_ID = p_TableDefination_ID;
                mtblDefination.distributorId = pDistributorId;
                mtblDefination.USER_ID = pUserId;
                return mtblDefination.ExecuteTable();

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

        public DataSet SelectDataForPosLoad(int pUserId,int pDistributerId, DateTime pDocumentDate,int pRoleId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                SpSelectDataForPOSLoad mtblDefination = new SpSelectDataForPOSLoad
                {
                    Connection = mConnection,
                    USER_ID = pUserId,
                    Distributer_ID = pDistributerId,
                    DOCUMENT_DATE = pDocumentDate,
                    ROLE_ID = pRoleId
                };
                return mtblDefination.ExecuteDataSet();
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
        
        public bool UnlockRecord(DateTime pDocumentDate, int pLOCKED_BY)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspUnlockRecord mOrderNo = new uspUnlockRecord
                {
                    Connection = mConnection,
                    DOCUMENT_DATE = pDocumentDate,
                    LOCKED_BY = pLOCKED_BY
                };
                return mOrderNo.ExecuteQuery();
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

        public bool UpdateTableDefination(int p_TableDefination_ID, string p_TableDefination_No, string p_TableDefination_Description, string p_TableDefination_Capacity, string p_TableDefination_Abbrivation, DateTime p_TIME_STAMP, int p_USER_ID, bool p_Is_Active, int location,int p_FloorID,int p_SortID,int p_ParetnCategoryID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateTableDefination mTableDefination = new spUpdateTableDefination();
                mTableDefination.Connection = mConnection;

                mTableDefination.p_TableDefination_ID = p_TableDefination_ID;
                mTableDefination.p_TableDefination_No = p_TableDefination_No;
                mTableDefination.p_TableDefination_Description = p_TableDefination_Description;
                mTableDefination.p_TableDefination_Capacity = p_TableDefination_Capacity;
                mTableDefination.p_TableDefination_Abbrivation = p_TableDefination_Abbrivation;
                mTableDefination.p_Is_Active = p_Is_Active;
                mTableDefination.p_distributorId = location;
                mTableDefination.p_USER_ID = p_USER_ID;
                mTableDefination.P_TIME_STAMP = p_TIME_STAMP;
                mTableDefination.FloorID = p_FloorID;
                mTableDefination.SortID = p_SortID;
                mTableDefination.ParetnCategoryID = p_ParetnCategoryID;
                bool a = mTableDefination.ExecuteQuery();
                return a;
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
        public bool DeleteTableDefination(int p_TableDefination_ID, bool p_Is_Active)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spDeleteTableDefination mTableDefination = new spDeleteTableDefination();
                mTableDefination.Connection = mConnection;

                mTableDefination.p_TableDefination_ID = p_TableDefination_ID;
                mTableDefination.p_Is_Active = p_Is_Active;

                bool a = mTableDefination.ExecuteQuery();
                return a;
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

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetMaxUOM_ID()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectMaxUOM_ID mdpt_id = new spSelectMaxUOM_ID();
                mdpt_id.Connection = mConnection;
                return mdpt_id.ExecuteTable();
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
        public bool InsertUOM(string p_UOM_CODE, string p_UOM_DESCRIPTION, string p_UOM_REMARKS, int p_UOM_TYPE_ID, DateTime p_TIME_STAMP, int p_USER_ID, bool p_IS_ACTIVE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spInsertUOM mDEPARTMENT = new spInsertUOM();
                mDEPARTMENT.Connection = mConnection;

                mDEPARTMENT.UOM_CODE = p_UOM_CODE;
                mDEPARTMENT.UOM_DESC = p_UOM_DESCRIPTION;
                mDEPARTMENT.UOM_REMARKS = p_UOM_REMARKS;
                mDEPARTMENT.UOM_TYPE_ID = p_UOM_TYPE_ID;
                mDEPARTMENT.TIME_STAMP = p_TIME_STAMP;
                mDEPARTMENT.USER_ID = p_USER_ID;
                mDEPARTMENT.IS_ACTIVE = p_IS_ACTIVE;

                bool a = mDEPARTMENT.ExecuteQuery();
                return a;
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
        public bool UpdateUOM(int p_UOM_ID, string p_UOM_DESCRIPTION, string p_UOM_REMARKS, int p_UOM_TYPE_ID, DateTime p_TIME_STAMP, int p_USER_ID, bool p_IS_ACTIVE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdateUOM mDEPARTMENT = new spUpdateUOM();
                mDEPARTMENT.Connection = mConnection;

                mDEPARTMENT.UOM_ID = p_UOM_ID;
                mDEPARTMENT.UOM_DESC = p_UOM_DESCRIPTION;
                mDEPARTMENT.UOM_REMARKS = p_UOM_REMARKS;
                mDEPARTMENT.UOM_TYPE_ID = p_UOM_TYPE_ID;
                mDEPARTMENT.TIME_STAMP = p_TIME_STAMP;
                mDEPARTMENT.USER_ID = p_USER_ID;
                mDEPARTMENT.IS_ACTIVE = p_IS_ACTIVE;
                //mDEPARTMENT.TYPE_ID = p_TYPE_ID;

                bool a = mDEPARTMENT.ExecuteQuery();
                return a;
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

        public DataTable GetUOM(int p_UOM_ID, int UomTypeId, int TypeID)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectUOM mGetDpt = new spSelectUOM();
                mGetDpt.Connection = mConnection;
                mGetDpt.UOM_TYPE_ID = UomTypeId;
                mGetDpt.UOM_ID = p_UOM_ID;
                mGetDpt.TYPEID = TypeID;
                return mGetDpt.ExecuteTable();

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

        public DataTable GetSMSSetting(int pDistributerId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetSMSSetting mtblDefination = new uspGetSMSSetting
                {
                    Connection = mConnection,
                    Distributer_ID = pDistributerId
                };
                return mtblDefination.ExecuteTable();
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