using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes

{
    /// <summary>
    /// Class For Account Head Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Account Head
    /// </item>
    /// <term>
    /// Update Account Head
    /// </term>
    /// <item>
    /// Get Account Head
    /// </item>
    /// <item>
    /// Assigns/UnAssings Account Head To Principal
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class AccountHeadController
    {
        #region Constructor

        /// <summary>
        /// Constructor for AccountHeadController
        /// </summary>
        public AccountHeadController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Public Methods

        #region Select

        /// <summary>
        /// Gets Account Head Data
        /// </summary>
        /// <remarks>
        /// Returns Account Head Data as Datatable
        /// </remarks>
        /// <param name="p_Account_Type_Id">Type</param>
        /// <param name="p_Account_ParentId">Parent</param>
        /// <returns>Account Head Data as Datatable</returns>
        public DataTable SelectAccountHead(int p_Account_Type_Id, long p_Account_ParentId, int typeid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectACCOUNT_HEAD mAccountHead = new spSelectACCOUNT_HEAD();
                mAccountHead.Connection = mConnection;
                mAccountHead.DISTRIBUTOR_ID = Constants.IntNullValue;
                mAccountHead.ACCOUNT_HEAD_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_PARENT_ID = p_Account_ParentId;
                mAccountHead.COMPANY_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_TYPE_ID = p_Account_Type_Id;
                mAccountHead.IS_ACTIVE = true;
                mAccountHead.TYPEID = typeid;
                mAccountHead.TIME_STAMP = Constants.DateNullValue;
                mAccountHead.LASTUPDATE_DATE = Constants.DateNullValue;
                mAccountHead.ACCOUNT_CATEGORY = Constants.IntNullValue;
                DataTable dt = mAccountHead.ExecuteTable();
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
        /// <summary>
        /// Gets Account Head Data
        /// </summary>
        /// <remarks>
        /// Returns Account Head Data as Datatable
        /// </remarks>
        /// <param name="p_Account_Type_Id">Type</param>
        /// <param name="p_Account_ParentId">Parent</param>
        /// <param name="p_Category">Category</param>
        /// <returns>Account Head Data as Datatable</returns>
        public DataTable SelectAccountHead(int p_Account_Type_Id, long p_Account_ParentId, int p_Category, int TYPEID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectACCOUNT_HEAD mAccountHead = new spSelectACCOUNT_HEAD();
                mAccountHead.Connection = mConnection;
                mAccountHead.DISTRIBUTOR_ID = Constants.IntNullValue;
                mAccountHead.ACCOUNT_HEAD_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_PARENT_ID = p_Account_ParentId;
                mAccountHead.COMPANY_ID = Constants.LongNullValue;
                mAccountHead.TYPEID = TYPEID;
                mAccountHead.ACCOUNT_TYPE_ID = p_Account_Type_Id;
                mAccountHead.IS_ACTIVE = true;
                mAccountHead.TIME_STAMP = Constants.DateNullValue;
                mAccountHead.LASTUPDATE_DATE = Constants.DateNullValue;
                mAccountHead.ACCOUNT_CATEGORY = p_Category;
                DataTable dt = mAccountHead.ExecuteTable();
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
        public DataTable SelectAccountHeadLocation(int p_Account_Type_Id, int p_DISTRIBUTOR_ID, int typeid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectACCOUNT_HEAD mAccountHead = new spSelectACCOUNT_HEAD();
                mAccountHead.Connection = mConnection;
                mAccountHead.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mAccountHead.ACCOUNT_HEAD_ID = Constants.LongNullValue;
                mAccountHead.COMPANY_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_TYPE_ID = p_Account_Type_Id;
                mAccountHead.IS_ACTIVE = true;
                mAccountHead.TYPEID = typeid;
                mAccountHead.TIME_STAMP = Constants.DateNullValue;
                mAccountHead.LASTUPDATE_DATE = Constants.DateNullValue;
                mAccountHead.ACCOUNT_CATEGORY = Constants.IntNullValue;
                DataTable dt = mAccountHead.ExecuteTable();
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
        public DataTable SelectAccountHeadByMapping(int p_Account_Type_Id, long AccountParentID, int typeid, string MapText)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectACCOUNT_HEAD mAccountHead = new spSelectACCOUNT_HEAD();
                mAccountHead.Connection = mConnection;
                mAccountHead.DISTRIBUTOR_ID = Constants.IntNullValue;
                mAccountHead.ACCOUNT_HEAD_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_PARENT_ID = Constants.LongNullValue;
                mAccountHead.COMPANY_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_TYPE_ID = p_Account_Type_Id;
                mAccountHead.IS_ACTIVE = true;
                mAccountHead.TYPEID = typeid;
                mAccountHead.ACCOUNT_NAME = MapText;
                mAccountHead.TIME_STAMP = Constants.DateNullValue;
                mAccountHead.LASTUPDATE_DATE = Constants.DateNullValue;
                mAccountHead.ACCOUNT_CATEGORY = Constants.IntNullValue;
                DataTable dt = mAccountHead.ExecuteTable();
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
        public DataTable SelectAccountHeadByMapping(int p_Account_Type_Id, long AccountParentID, int typeid, string MapText,int p_DISTRIBUTOR_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectACCOUNT_HEAD mAccountHead = new spSelectACCOUNT_HEAD();
                mAccountHead.Connection = mConnection;
                mAccountHead.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mAccountHead.ACCOUNT_HEAD_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_PARENT_ID = Constants.LongNullValue;
                mAccountHead.COMPANY_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_TYPE_ID = p_Account_Type_Id;
                mAccountHead.IS_ACTIVE = true;
                mAccountHead.TYPEID = typeid;
                mAccountHead.ACCOUNT_NAME = MapText;
                mAccountHead.TIME_STAMP = Constants.DateNullValue;
                mAccountHead.LASTUPDATE_DATE = Constants.DateNullValue;
                mAccountHead.ACCOUNT_CATEGORY = Constants.IntNullValue;
                DataTable dt = mAccountHead.ExecuteTable();
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
        public DataTable GetAccountHeadIDbyNameOrCode(int p_Account_Type_Id, string Code, string Title, int typeid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectACCOUNT_HEAD mAccountHead = new spSelectACCOUNT_HEAD();
                mAccountHead.Connection = mConnection;
                mAccountHead.DISTRIBUTOR_ID = Constants.IntNullValue;
                mAccountHead.ACCOUNT_HEAD_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_PARENT_ID = Constants.IntNullValue; ;
                mAccountHead.COMPANY_ID = Constants.LongNullValue;
                mAccountHead.ACCOUNT_TYPE_ID = p_Account_Type_Id;
                mAccountHead.ACCOUNT_CODE = Code;
                mAccountHead.ACCOUNT_NAME = Title;
                mAccountHead.IS_ACTIVE = true;
                mAccountHead.TYPEID = typeid;
                mAccountHead.TIME_STAMP = Constants.DateNullValue;
                mAccountHead.LASTUPDATE_DATE = Constants.DateNullValue;
                mAccountHead.ACCOUNT_CATEGORY = Constants.IntNullValue;
                DataTable dt = mAccountHead.ExecuteTable();
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

        /// <summary>
        /// Gets Account Head Data
        /// </summary>
        /// <remarks>
        /// Returns Account Head Data as Datatable
        /// </remarks>
        /// <param name="p_Account_MainType_Id">MainType</param>
        /// <param name="p_SubType_Id">SubType</param>
        /// <param name="p_DetailTypeId">DetailType</param>
        /// <param name="p_Category">Category</param>
        /// <param name="pType">Type</param>
        /// <returns>Account Head Data as Datatable</returns>
        public DataTable GetAccountHead(int p_Account_MainType_Id, int p_SubType_Id, int p_DetailTypeId, int p_Category, int pType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspGetACCOUNT_HEAD mAccountHead = new UspGetACCOUNT_HEAD();
                mAccountHead.Connection = mConnection;
                mAccountHead.ACCOUNT_HEAD_ID = Constants.LongNullValue;
                mAccountHead.Account_TypeId = p_Account_MainType_Id;
                mAccountHead.AccountSub_TypeId = p_SubType_Id;
                mAccountHead.AccountDetail_TypeId = p_DetailTypeId;
                mAccountHead.ACCOUNT_CATEGORY_ID = p_Category;
                mAccountHead.TypeId = pType;
                DataTable dt = mAccountHead.ExecuteTable();
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
        
        /// <summary>
        /// Gets General Ledger Data for Account Head ID
        /// </summary>
        /// <remarks>
        /// Returns General Ledger Data for Account Head ID as Datatable
        /// </remarks>
        /// <param name="p_Account_Head_Id">AccountHead</param>
        /// <returns>General Ledger Data for Account Head ID as Datable</returns>
        public DataTable SelectGlTranscton(long p_Account_Head_Id,int p_LevelID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspCheckGLTransction mAccountHead = new UspCheckGLTransction();
                mAccountHead.Connection = mConnection;
                mAccountHead.Account_Head_id = p_Account_Head_Id;
                mAccountHead.LevelID = p_LevelID;
                DataTable dt = mAccountHead.ExecuteTable();
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
        
        #region Added By Hazrat Ali


        /// <summary>
        /// Gets Assigned Account Heads To Principal
        /// </summary>
        /// <remarks>
        /// Returns Assigned Account Heads To Principal as Datatable
        /// </remarks>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_ACCOUNT_PARENT_ID">Parent</param>
        /// <returns>Assigned Account Heads To Principal as Datatable</returns>
        public DataTable GetAssignAccountHead(int p_TypeID, int p_ACCOUNT_PARENT_ID,int p_DISTRIBUTOR_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                usp_GetAssign_AccountHead mAccountHead = new usp_GetAssign_AccountHead();
                mAccountHead.Connection = mConnection;
                mAccountHead.TypeID = p_TypeID;
                mAccountHead.ACCOUNT_PARENT_ID = p_ACCOUNT_PARENT_ID;
                mAccountHead.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                DataTable dt = mAccountHead.ExecuteTable();
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

        public DataTable GetMinMaxAccountCode(int p_ACCOUNT_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetMinMaxAccountCode mAccountHead = new uspGetMinMaxAccountCode();
                mAccountHead.Connection = mConnection;
                mAccountHead.ACCOUNT_TYPE_ID = p_ACCOUNT_TYPE_ID;
                DataTable dt = mAccountHead.ExecuteTable();
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

        public DataTable GetCOALocation(int p_LocationID, int p_ACCOUNT_PARENT_ID, int p_ACCOUNT_CATEGORY_ID, int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspGetCOALocation ObjSelect = new uspGetCOALocation();
                ObjSelect.Connection = mConnection;
                ObjSelect.LocationID = p_LocationID;
                ObjSelect.ACCOUNT_PARENT_ID = p_ACCOUNT_PARENT_ID;
                ObjSelect.ACCOUNT_CATEGORY_ID = p_ACCOUNT_CATEGORY_ID;
                ObjSelect.TypeID = p_TypeID;
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

        public bool IsChildExist(long p_ACCOUNT_HEAD_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspGetAccountChildren ObjSelect = new uspGetAccountChildren();
                ObjSelect.Connection = mConnection;
                ObjSelect.ACCOUNT_HEAD_ID= p_ACCOUNT_HEAD_ID;
                DataTable dt = ObjSelect.ExecuteTable();
                if(dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

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

        #region Insert, Update

        /// <summary>
        /// Inserts Account Head
        /// <remarks>
        /// Returns Inserted Account Head ID as String
        /// </remarks>
        /// </summary>
        /// <param name="p_Company_id">Company</param>
        /// <param name="p_Is_Active">Active/InActive</param>
        /// <param name="p_Time_Stamp">CreatedOn</param>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Account_Type_Id">Type</param>
        /// <param name="p_Account_Parent_id">Parent</param>
        /// <param name="p_Account_Name">Name</param>
        /// <param name="p_Account_Code">Code</param>
        /// <param name="p_Index">Category</param>
        /// <returns>Inserted Account Head ID as String</returns>
      	public string InsertAccountHead(int p_Company_id, bool p_Is_Active, DateTime p_Time_Stamp, int p_Distributor_Id, int p_Account_Type_Id, long p_Account_Parent_id, string p_Account_Name, string p_Account_Code, int p_Index)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertACCOUNT_HEAD mAccountHead = new spInsertACCOUNT_HEAD();
                mAccountHead.Connection = mConnection;
                mAccountHead.COMPANY_ID = p_Company_id;
                mAccountHead.DISTRIBUTOR_ID = p_Distributor_Id;
                mAccountHead.ACCOUNT_TYPE_ID = p_Account_Type_Id;
                mAccountHead.ACCOUNT_PARENT_ID = p_Account_Parent_id;
                mAccountHead.ACCOUNT_NAME = p_Account_Name;
                mAccountHead.ACCOUNT_CODE = p_Account_Code;
                mAccountHead.IS_ACTIVE = p_Is_Active;
                mAccountHead.TIME_STAMP = p_Time_Stamp;
                mAccountHead.ACCOUNT_CATEGORY = p_Index;
                mAccountHead.TIME_STAMP = DateTime.Now;
                mAccountHead.LASTUPDATE_DATE = DateTime.Now;
                mAccountHead.ExecuteQuery();
                return mAccountHead.ACCOUNT_HEAD_ID.ToString();

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

        /// <summary>
        /// Updates Account Head
        /// </summary>
        /// <remarks>
        /// Returns Updated Account Head ID as String
        /// </remarks>
        /// <param name="p_Account_Head_Id">AccountHead</param>
        /// <param name="p_Company_Id">Company</param>
        /// <param name="p_Is_Active">Active/InActive</param>
        /// <param name="p_Time_Stamp">CreatedOn</param>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Account_Type_Id">Type</param>
        /// <param name="p_Account_ParentId">Parent</param>
        /// <param name="p_Account_Name">Name</param>
        /// <param name="p_Account_Code">Code</param>
        /// <param name="p_Index">Category</param>
        /// <returns>Updated Account Head ID as String</returns>
        public string UpdateAccountHead(long p_Account_Head_Id, long p_Company_Id, bool p_Is_Active, DateTime p_Time_Stamp, int p_Distributor_Id, int p_Account_Type_Id, long p_Account_ParentId, string p_Account_Name, string p_Account_Code, int p_Index)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateACCOUNT_HEAD mAccountHead = new spUpdateACCOUNT_HEAD();
                mAccountHead.Connection = mConnection;
                mAccountHead.ACCOUNT_HEAD_ID = p_Account_Head_Id;
                mAccountHead.ACCOUNT_NAME = p_Account_Name;
                mAccountHead.ACCOUNT_CODE = p_Account_Code;
                mAccountHead.ACCOUNT_TYPE_ID = p_Account_Type_Id;
                mAccountHead.DISTRIBUTOR_ID = p_Distributor_Id;
                mAccountHead.COMPANY_ID = p_Company_Id;
                mAccountHead.ACCOUNT_PARENT_ID = p_Account_ParentId;
                mAccountHead.IS_ACTIVE = p_Is_Active;
                mAccountHead.TIME_STAMP = p_Time_Stamp;
                mAccountHead.LASTUPDATE_DATE = p_Time_Stamp;
                mAccountHead.ACCOUNT_CATEGORY = p_Index;
                mAccountHead.ExecuteQuery();
                return mAccountHead.ACCOUNT_HEAD_ID.ToString();

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

        public bool InsertCOALocation(int p_LocationID, int p_COAID, int p_UserID, int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspInsertCOALocation ObjInsert = new uspInsertCOALocation();
                ObjInsert.Connection = mConnection;
                ObjInsert.LocationID = p_LocationID;
                ObjInsert.COAID = p_COAID;
                ObjInsert.UserID = p_UserID;
                ObjInsert.TypeID = p_TypeID;
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