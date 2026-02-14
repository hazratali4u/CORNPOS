using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For User Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert User
    /// </item>
    /// <term>
    /// Update User
    /// </term>
    /// <item>
    /// Get User
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class UserController
    {
        #region Constructors

        /// <summary>
        /// Constructor for UserController
        /// </summary>
        public UserController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region public Methods

        #region Select

        /// <summary>
        /// Gets Login User Data
        /// </summary>
        /// Reutns Login User Data as Datatable
        /// <param name="p_LoginId">Login</param>
        /// <param name="p_Password">Password</param>
        /// <returns>Login User Data as Datatable</returns>
        public DataTable SelectSlashUser(string p_LoginId, string p_Password)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelect_USER mUser = new spSelect_USER();
                mUser.Connection = mConnection;
                mUser.LOGIN_ID = p_LoginId;
                mUser.PASSWORD = p_Password;
                mUser.IS_ACTIVE = true;

                DataTable dt = mUser.ExecuteTable();
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
        /// Gets User Data To Change Password
        /// </summary>
        /// Returns User Data as Datatable
        /// <param name="p_User_ID">User</param>
        /// <returns>User Data as Datatable</returns>
        public DataTable SelectSlashUser(int p_User_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelect_USER mUser = new spSelect_USER();
                mUser.Connection = mConnection;
                mUser.USER_ID = p_User_ID;
                mUser.IS_ACTIVE = true;

                DataTable dt = mUser.ExecuteTable();
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

        public DataTable SelectSlashUserView(string p_LoginId, string p_Password,int p_USER_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelect_USERView mUser = new spSelect_USERView();
                mUser.Connection = mConnection;
                mUser.LOGIN_ID = p_LoginId;
                mUser.PASSWORD = p_Password;
                mUser.IS_ACTIVE = true;
                mUser.USER_ID = p_USER_ID;
                DataTable dt = mUser.ExecuteTable();
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

        public DataTable SelectSlashUserEncrypt(string p_LoginId, string p_Password)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionStringEncrypt, EnumProviders.SQLClient);
                mConnection.Open();
                spSelect_USER mUser = new spSelect_USER();
                mUser.Connection = mConnection;
                mUser.LOGIN_ID = p_LoginId;
                mUser.PASSWORD = p_Password;
                mUser.IS_ACTIVE = true;

                DataTable dt = mUser.ExecuteTable();
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
        public DataTable SelectValidateUser(int pDistributorId, string p_LoginId, string p_Password)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelect_USER mUser = new spSelect_USER();
                mUser.Connection = mConnection;

                mUser.DISTRIBUTOR_ID = pDistributorId;
                mUser.LOGIN_ID = p_LoginId;
                mUser.PASSWORD = p_Password;
                mUser.IS_ACTIVE = true;
                //mUser.ROLE_ID = 1;//admin
                DataTable dt = mUser.ExecuteTable();
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
        public DataTable SelectValidateUserActive(int pDistributorId, string p_LoginId, string p_Password)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelect_USERActive mUser = new spSelect_USERActive();
                mUser.Connection = mConnection;

                mUser.DISTRIBUTOR_ID = pDistributorId;
                mUser.LOGIN_ID = p_LoginId;
                mUser.PASSWORD = p_Password;
                mUser.IS_ACTIVE = true;
                //mUser.ROLE_ID = 1;//admin
                DataTable dt = mUser.ExecuteTable();
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
        /// Gets Locations Assigned To a User
        /// </summary>
        /// <remarks>
        /// Returns Locations Assigned To a User as Datatable
        /// </remarks>
        /// <param name="UserId">User</param>
        /// <param name="DistributorTypeid">LocationType</param>
        /// <param name="TypeiD">Type</param>
        /// <param name="Companyid">Company</param>
        /// <returns>Locations Assigned To a User as Datatable</returns>
        public DataTable SelectUserAssignment(int UserId, int DistributorTypeid, int TypeiD, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectUnAssignDistributor mUserAssing = new UspSelectUnAssignDistributor();
                mUserAssing.Connection = mConnection;
                mUserAssing.USER_ID = UserId;
                mUserAssing.DISTRIBUTOR_TYPE = DistributorTypeid;
                mUserAssing.TYPE_ID = TypeiD;
                mUserAssing.COMPANY_ID = Companyid;
                DataTable dt = mUserAssing.ExecuteTable();
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

        public DataTable SelectUserAssignmentActive(int UserId, int DistributorTypeid, int TypeiD, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectUnAssignDistributorActive mUserAssing = new UspSelectUnAssignDistributorActive();
                mUserAssing.Connection = mConnection;
                mUserAssing.USER_ID = UserId;
                mUserAssing.DISTRIBUTOR_TYPE = DistributorTypeid;
                mUserAssing.TYPE_ID = TypeiD;
                mUserAssing.COMPANY_ID = Companyid;
                DataTable dt = mUserAssing.ExecuteTable();
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

        public string ActiveInactive(bool p_IsActive, int p_MASTER_ID, int p_UserId, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spActiveInactive mSkus = new spActiveInactive();
                mSkus.Connection = mConnection;
                mSkus.ISACTIVE = p_IsActive;
                mSkus.MASTER_ID = p_MASTER_ID;
                mSkus.USER_ID = p_UserId;
                mSkus.TYPE_ID = p_TypeId;
                mSkus.ExecuteQuery();
                return "Record Updated";
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

        public bool IsExist(string pParameter, int p_DistributorId, int p_RecordId, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spIsExist mSkus = new spIsExist();

                mSkus.Connection = mConnection;

                mSkus.PARAMETER = pParameter;
                mSkus.DISTRIBUTOR_ID = p_DistributorId;
                mSkus.RECORD_ID = p_RecordId;
                mSkus.TYPE_ID = p_TypeId;

                string exist = mSkus.ExecuteScalar();
                if (exist == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
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
        /// Gets User Data Type 1 for All, Else for Active
        /// </summary>
        /// <remarks>
        /// Returns User Data as Datatable
        /// </remarks>
        /// <returns>User Data as Datatable</returns>
        public DataTable SelectAllUser(int pType, int pDistributorId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectAllUsers mUser = new uspSelectAllUsers();
                mUser.Connection = mConnection;
                mUser.TYPE = pType;
                mUser.DISTRIBUTOR_ID = pDistributorId;

                DataTable dt = new DataTable();
                dt = mUser.ExecuteTable();

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

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Inserts User
        /// </summary>
        /// <remarks>
        /// Returns Inserted User ID as String
        /// </remarks>
        /// <param name="User_Id">User</param>
        /// <param name="p_CompanyId">Company</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="p_LoginId">Login</param>
        /// <param name="p_Password">Password</param>
        /// <param name="p_RoleId">Role</param>
        /// <returns>Inserted User ID as String</returns>
        public string InsertUser(int User_Id, int p_CompanyId, int p_DistributorId, string p_LoginId,
            string p_Password, int p_RoleId, bool pIsDelRight, bool pIsLessRight, bool isCashier,
            bool p_IS_CanGiveDiscount, bool p_IS_CanReverseDayClose, bool p_Can_DineIn, bool p_Can_Delivery,
            bool p_Can_TakeAway,bool p_Can_ComplimentaryItem,bool p_Can_PrintOrder,bool p_CanAlterServiceCharges,
            int p_DefaultServiceType,string p_PRINTER_NAME,bool p_PrintInvoiceFromWS,bool p_autoStockAdjustment,bool p_CanAlterDeliveryCharges
            ,bool p_CanCancelTableReservation,int p_USER_UPDATE_BY,bool p_IsSplitBill)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsert_USER mUser = new spInsert_USER();
                mUser.Connection = mConnection;
                mUser.USER_ID = User_Id;
                mUser.DISTRIBUTOR_ID = p_DistributorId;
                mUser.COMPANY_ID = p_CompanyId;
                mUser.LOGIN_ID = p_LoginId;
                mUser.PASSWORD = p_Password;
                mUser.ROLE_ID = p_RoleId;
                mUser.LASTUPDATE_DATE = System.DateTime.Today;
                mUser.IsDelRight = pIsDelRight;
                mUser.IsLessRight = pIsLessRight;
                mUser.IS_CASHIER = isCashier;
                mUser.IS_CanGiveDiscount = p_IS_CanGiveDiscount;
                mUser.IS_CanReverseDayClose = p_IS_CanReverseDayClose;
                mUser.Can_DineIn = p_Can_DineIn;
                mUser.Can_Delivery = p_Can_Delivery;
                mUser.Can_TakeAway = p_Can_TakeAway;
                mUser.Can_ComplimentaryItem = p_Can_ComplimentaryItem;
                mUser.Can_PrintOrder = p_Can_PrintOrder;
                mUser.CanAlterServiceCharges = p_CanAlterServiceCharges;
                mUser.DefaultServiceType = p_DefaultServiceType;
                mUser.PRINTER_NAME = p_PRINTER_NAME;
                mUser.PrintInvoiceFromWS = p_PrintInvoiceFromWS;
                mUser.AutoStockAdjustment = p_autoStockAdjustment;
                mUser.CanAlterDeliveryCharges = p_CanAlterDeliveryCharges;
                mUser.CanCancelTableReservation = p_CanCancelTableReservation;
                mUser.USER_UPDATE_BY = p_USER_UPDATE_BY;
                mUser.IsSplitBill = p_IsSplitBill;
                mUser.ExecuteQuery();
                return mUser.USER_ID.ToString();
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
        /// Inserts Locations Assigned To User
        /// </summary>
        /// <remarks>
        /// Returns True on Success And False on Failure
        /// </remarks>
        /// <param name="UserId">User</param>
        /// <param name="DistributorTypeid">LocationType</param>
        /// <param name="DistributorId">Location</param>
        /// <param name="Companyid">Company</param>
        /// <returns>True on Success And False on Failure</returns>
        public bool InsertUserAssignment(int UserId, int DistributorTypeid, int DistributorId, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertDISTRIBUTOR_ASSIGNMENT mUserAssing = new spInsertDISTRIBUTOR_ASSIGNMENT();
                mUserAssing.Connection = mConnection;
                mUserAssing.USER_ID = UserId;
                mUserAssing.DISTRIBUTOR_TYPE = DistributorTypeid;
                mUserAssing.DISTRIBUTOR_ID = DistributorId;
                mUserAssing.COMPANY_ID = Companyid;
                bool dt = mUserAssing.ExecuteQuery();
                return dt;
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

        /// <summary>
        /// Updates User
        /// </summary>
        /// <remarks>
        /// Returns "Record Updated" On Success And Exception.Message on Failure
        /// </remarks>
        /// <param name="p_UserId">User</param>
        /// <param name="p_CompanyId">Company</param>
        /// <param name="p_LoginId">Login</param>
        /// <param name="p_Password">Password</param>
        /// <param name="p_RoleId">Role</param>
        /// <param name="p_IsActive">IsActive</param>
        /// <param name="p_DistributorID">Location</param>
        /// <returns>"Record Updated" On Success And Exception.Message on Failure</returns>
        public string UpdateUser(int p_UserId, int p_CompanyId, string p_LoginId, string p_Password, int p_RoleId,
            bool p_IsActive, int p_DistributorID, bool pIsDelRight, bool pIsLessRight, bool isCashier,
            bool p_IS_CanGiveDiscount, bool p_IS_CanReverseDayClose, bool p_Can_DineIn, bool p_Can_Delivery,
            bool p_Can_TakeAway,bool p_Can_ComplimentaryItem,bool p_Can_PrintOrder,bool p_CanAlterServiceCharges,
            int p_DefaultServiceType,string p_PRINTER_NAME,bool p_PrintInvoiceFromWS, bool p_autoStockAdjustment,bool p_CanAlterDeliveryCharges
            ,bool p_CanCancelTableReservation,int p_USER_UPDATE_BY,bool p_IsSplitBill)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdateUSER mUser = new spUpdateUSER();
                mUser.Connection = mConnection;
                mUser.USER_ID = p_UserId;
                mUser.DISTRIBUTOR_ID = p_DistributorID;
                mUser.COMPANY_ID = p_CompanyId;
                mUser.LOGIN_ID = p_LoginId;
                mUser.PASSWORD = p_Password;
                mUser.ROLE_ID = p_RoleId;
                mUser.IS_ACTIVE = p_IsActive;
                mUser.LASTUPDATE_DATE = System.DateTime.Today;
                mUser.IsDelRight = pIsDelRight;
                mUser.IsLessRight = pIsLessRight;
                mUser.IS_CASHIER = isCashier;
                mUser.IS_CanGiveDiscount = p_IS_CanGiveDiscount;
                mUser.IS_CanReverseDayClose = p_IS_CanReverseDayClose;
                mUser.Can_DineIn = p_Can_DineIn;
                mUser.Can_Delivery = p_Can_Delivery;
                mUser.Can_TakeAway = p_Can_TakeAway;
                mUser.Can_ComplimentaryItem = p_Can_ComplimentaryItem;
                mUser.Can_PrintOrder = p_Can_PrintOrder;
                mUser.CanAlterServiceCharges = p_CanAlterServiceCharges;
                mUser.DefaultServiceType = p_DefaultServiceType;
                mUser.PRINTER_NAME = p_PRINTER_NAME;
                mUser.PrintInvoiceFromWS = p_PrintInvoiceFromWS;
                mUser.AutoStockAdjustment = p_autoStockAdjustment;
                mUser.CanAlterDeliveryCharges = p_CanAlterDeliveryCharges;
                mUser.CanCancelTableReservation = p_CanCancelTableReservation;
                mUser.USER_UPDATE_BY = p_USER_UPDATE_BY;
                mUser.IsSplitBill = p_IsSplitBill;
                mUser.ExecuteQuery();

                return "true";
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

        public string UpdatePassword(int p_UserId, string p_LoginId, string p_Password)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdatePassword mUser = new spUpdatePassword();
                mUser.Connection = mConnection;

                mUser.USER_ID = p_UserId;
                mUser.LOGIN_ID = p_LoginId;
                mUser.PASSWORD = p_Password;
                mUser.LASTUPDATE_DATE = System.DateTime.Today;

                mUser.ExecuteQuery();

                return "true";
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


        /// <summary>
        /// Deletes Location Assignment To User
        /// </summary>
        /// <remarks>
        /// Returns True on Success And False on Failure
        /// </remarks>
        /// <param name="UserId">User</param>
        /// <param name="DistributorTypeid">LocationType</param>
        /// <param name="DistributorId">Location</param>
        /// <param name="Companyid">Company</param>
        /// <returns>True on Success And False on Failure</returns>
        public bool DeleteUserAssignment(int UserId, int DistributorTypeid, int DistributorId, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spDeleteDISTRIBUTOR_ASSIGNMENT mUserAssing = new spDeleteDISTRIBUTOR_ASSIGNMENT();
                mUserAssing.Connection = mConnection;
                mUserAssing.USER_ID = UserId;
                mUserAssing.DISTRIBUTOR_TYPE = DistributorTypeid;
                mUserAssing.DISTRIBUTOR_ID = DistributorId;
                mUserAssing.COMPANY_ID = Companyid;
                bool dt = mUserAssing.ExecuteQuery();
                return dt;
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

        #region Added By Hazrat Ali

        /// <summary>
        /// Inserts User Login Time
        /// </summary>
        /// <remarks>
        /// Returns Inserted User Login ID as Long
        /// </remarks>
        /// <param name="User_ID">User</param>
        /// <returns>Inserted User Login ID as Long</returns>
        public long InsertUserLoginTime(int User_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertUSER_LOG mSlashUserLog = new spInsertUSER_LOG();
                mSlashUserLog.Connection = mConnection;
                mSlashUserLog.User_ID = User_ID;
                string User_Log_ID = mSlashUserLog.ExecuteScalar();
                return Convert.ToInt64(User_Log_ID);

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
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

        /// <summary>
        /// Inserts User Logout Time
        /// </summary>
        /// <remarks>
        /// Returns "Logout Time Inserted" on Success And Exception.Message on Failure
        /// </remarks>
        /// <param name="p_User_Log_ID">UserLog</param>
        /// <param name="p_UserID">User</param>
        /// <returns>"Logout Time Inserted" on Success And Exception.Message on Failure</returns>
        public string InsertUserLogoutTime(long p_User_Log_ID, int p_UserID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdateUSER_LOG mSlashUserLog = new spUpdateUSER_LOG();
                mSlashUserLog.Connection = mConnection;

                mSlashUserLog.User_Log_ID = p_User_Log_ID;
                mSlashUserLog.User_ID = p_UserID;
                mSlashUserLog.ExecuteQuery();
                return "Logout Time Inserted";
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

        #endregion
    }
}