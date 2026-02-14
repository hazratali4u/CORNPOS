using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Sale Force Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Sale Force, Sale Force Area, Sale Force Principal
    /// </item>
    /// <term>
    /// Update Sale Force, Sale Force Area, Sale Force Principal
    /// </term>
    /// <item>
    /// Get Sale Force, Sale Force Area, Sale Force Principal
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class SaleForceController
    {
        #region Constructor

        /// <summary>
        /// Constructor for SaleForceController
        /// </summary>
        public SaleForceController()
        {

        }

        #endregion

        #region Public Methods

        #region Select 
        public DataTable SelectSaleForceTrackingDetails(int p_orderBookerId, int locationId,
        DateTime? fromDate = null, DateTime? toDate = null)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetSaleForceTrackingDetails mSaleForceCash = new uspGetSaleForceTrackingDetails();
                mSaleForceCash.Connection = mConnection;

                mSaleForceCash.DELIVERYMAN_ID = p_orderBookerId;
                mSaleForceCash.DISTRIBUTOR_ID = locationId;
                mSaleForceCash.FROM_DATE = fromDate;
                mSaleForceCash.TO_DATE = toDate;
                DataTable dt = mSaleForceCash.ExecuteTable();
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
        /// Gets Assigned Sale Force To a Route
        /// </summary>
        /// <remarks>
        /// Returns Assigned Sale Force To a Route as Datatable
        /// </remarks>
        /// <param name="p_TYPE">Type</param>
        /// <param name="p_DISTRIBUTOR_ID">Location</param>
        /// <param name="p_AREA_ID">Route</param>
        /// <param name="p_COMPANY_ID">Company</param>
        /// <param name="p_PRINCIPAL_ID">Principal</param>
        /// <returns>Assigned Sale Force To a Route as Datatable</returns>
        public DataTable SelectSaleForceAssignedArea(int p_TYPE, int p_DISTRIBUTOR_ID, int p_AREA_ID, int p_COMPANY_ID, int p_PRINCIPAL_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectPrincipalAreaWiseSaleForce mDistUser = new uspSelectPrincipalAreaWiseSaleForce();
                mDistUser.Connection = mConnection;
                mDistUser.TYPE = p_TYPE;
                mDistUser.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mDistUser.COMPANY_ID = p_COMPANY_ID;
                mDistUser.AREA_ID = p_AREA_ID;
                mDistUser.PRINCIPAL_ID = p_PRINCIPAL_ID;
                DataTable dt = mDistUser.ExecuteTable();
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

        public DataSet SelectSaleForceUsers(int p_TYPE, int p_DISTRIBUTOR_ID, int p_COMPANY_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectPrincipalAreaWiseSaleForce mDistUser = new uspSelectPrincipalAreaWiseSaleForce();
                mDistUser.Connection = mConnection;
                mDistUser.TYPE = p_TYPE;
                mDistUser.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mDistUser.COMPANY_ID = p_COMPANY_ID;
                mDistUser.AREA_ID = Constants.IntNullValue;
                mDistUser.PRINCIPAL_ID = Constants.IntNullValue;
                DataSet ds = mDistUser.ExecuteTableSet();
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

        public DataTable GetDepartmentWiseEmployees(int p_DISTRIBUTOR_ID, int p_COMPANY_ID,int p_USER_ID,string p_DepartmentIDs,  int p_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetDepartment_Employee mDistUser = new uspGetDepartment_Employee();
                mDistUser.Connection = mConnection;
                mDistUser.TYPE = p_TYPE;
                mDistUser.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mDistUser.COMPANY_ID = p_COMPANY_ID;
                mDistUser.USER_ID = p_USER_ID;
                mDistUser.DepartmentIDs = p_DepartmentIDs;
                mDistUser.TYPE = p_TYPE;
                DataTable dt = mDistUser.ExecuteTable();
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

        public DataSet GetTimeCardData(int p_DISTRIBUTOR_ID, string p_DepartmentIDs, string p_EmployeeIDs, DateTime p_DATE_FROM, DateTime p_DATE_TO)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetTimeCard mattendance = new uspGetTimeCard();

                mattendance.Connection = mConnection;

                mattendance.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mattendance.DepartmentIDs = p_DepartmentIDs;
                mattendance.EmployeeIDs = p_EmployeeIDs;
                mattendance.DATE_FROM = p_DATE_FROM;
                mattendance.DATE_TO = p_DATE_TO;
                DataTable dt = mattendance.ExecuteTable();

                Reports.DSReportNew ds = new Reports.DSReportNew();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetTimeCard"].ImportRow(dr);
                }
                return ds;

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

        #endregion

        #endregion
    }
}