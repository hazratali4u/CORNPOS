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
	public class CommonController
    {
        /// <summary>
        /// Constructor for UserController
        /// </summary>
        public CommonController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region public Methods

        #region Select

        /// <summary>
        /// Gets Login User Data
        /// </summary>
        /// Reutns Login User Data as Datatable
        /// <param name="p_LoginId">Login</param>
        /// <param name="p_Password">Password</param>
        /// <returns>Login User Data as Datatable</returns>
        public DataTable SelectWhatsNew()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spWhatNew mSlashUser = new spWhatNew();
                mSlashUser.Connection = mConnection;

                DataTable dt = mSlashUser.ExecuteTable();
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
        public bool InsertOrUpdate_WhatsNew(int ID, string title, string link, string remarks,
            int userId, bool isActive)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spWhatNew mUserAssing = new spWhatNew();
                mUserAssing.Connection = mConnection;
                mUserAssing.USER_ID = userId;
                mUserAssing.MenuTitle = title;
                mUserAssing.MenuLink = link;
                mUserAssing.Description = remarks;
                mUserAssing.ID = ID;
                mUserAssing.Is_Active = isActive;
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
        #endregion

        #endregion
    }
}