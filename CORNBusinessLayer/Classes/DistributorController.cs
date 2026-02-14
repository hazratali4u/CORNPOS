using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System.Collections;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Location Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Location
    /// </item>
    /// <term>
    /// Update Location
    /// </term>
    /// <item>
    /// Get Location
    /// </item>
    /// <item>
    /// DayClose
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class DistributorController
    {
        #region Constructors

        /// <summary>
        /// Constructor for DistributorController
        /// </summary>
        public DistributorController()
        {
            //
            // TODO: Add constructor logic here
            //

        }
        #endregion

        #region public Methods

        #region Select
        public DataTable SelectCities()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectDistributorInfo mDistributorInfo = new uspSelectDistributorInfo();
                mDistributorInfo.Connection = mConnection;

                DataTable dt = mDistributorInfo.ExecuteTableForCity();
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
        /// Gets Location Data
        /// </summary>
        /// <remarks>
        /// Returns Location Data as Datatable
        /// </remarks>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_User_Id">InsertedBy</param>
        /// <param name="Companyid">Company</param>
        /// <returns>Location Data as Datatable</returns>
        public DataTable SelectDistributorInfo(int p_Distributor_ID, int p_User_Id, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectDistributorInfo mDistributorInfo = new uspSelectDistributorInfo();
                mDistributorInfo.Connection = mConnection;
                mDistributorInfo.DISTRIBUTOR_ID = p_Distributor_ID;
                mDistributorInfo.USER_ID = p_User_Id;
                mDistributorInfo.COMPANY_ID = Companyid;

                DataTable dt = mDistributorInfo.ExecuteTable();
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
        /// Gets Location Data
        /// </summary>
        /// <remarks>
        /// Returns Location Data as Datatable
        /// </remarks>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_User_Id">InsertedBy</param>
        /// <param name="p_DistributorType">Type</param>
        /// <param name="Companyid">Company</param>
        /// <returns>Location Data as Datatable</returns>
        public DataTable SelectDistributorInfo(int p_Distributor_ID, int p_User_Id, int p_DistributorType, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectDistributorInfo mDistributorInfo = new uspSelectDistributorInfo();
                mDistributorInfo.Connection = mConnection;
                mDistributorInfo.DISTRIBUTOR_ID = p_Distributor_ID;
                mDistributorInfo.USER_ID = p_User_Id;
                mDistributorInfo.DISTRIBUTOR_TYPE = p_DistributorType;
                mDistributorInfo.COMPANY_ID = Companyid;
                DataTable dt = mDistributorInfo.ExecuteTable();
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
        public DataTable GetDistributorWithMaxDayClose(int p_Distributor_ID, int p_User_Id, int Companyid, int p_Type_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectDistributorInfo mDistributorInfo = new uspSelectDistributorInfo();
                mDistributorInfo.Connection = mConnection;
                mDistributorInfo.DISTRIBUTOR_ID = p_Distributor_ID;
                mDistributorInfo.USER_ID = p_User_Id;
                mDistributorInfo.COMPANY_ID = Companyid;
                mDistributorInfo.TYPE_ID = p_Type_ID;
                DataTable dt = mDistributorInfo.ExecuteTable();
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
        /// Gets Location Type Data
        /// </summary>
        /// <remarks>
        /// Returns Location Data Type as Datatable
        /// </remarks>
        /// <param name="p_DistributorType_ID">Type</param>
        /// <returns>Location Type Data as Datatable</returns>
        public DataTable SelectDistributorTypeInfo(int p_DistributorType_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectDISTRIBUTOR_TYPE mDistributorType = new spSelectDISTRIBUTOR_TYPE();
                mDistributorType.Connection = mConnection;

                mDistributorType.DISTRIBUTOR_TYPE_ID = p_DistributorType_ID;
                DataTable dt = mDistributorType.ExecuteTable();
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
        /// Gets Location Data
        /// </summary>
        /// <remarks>
        /// Returns Location Data as Datatable
        /// </remarks>
        /// <param name="p_Distributor_Id">Location</param>
        /// <returns>Location Data as Datatable</returns>
        public DataTable SelectDistributor(int p_Distributor_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectDISTRIBUTOR mDistributorInfo = new uspSelectDISTRIBUTOR();
                mDistributorInfo.Connection = mConnection;
                mDistributorInfo.DIST_CLASS_ID = Constants.IntNullValue;
                mDistributorInfo.ISDELETED = false;
                mDistributorInfo.TIME_STAMP = Constants.DateNullValue;
                mDistributorInfo.LASTUPDATE_DATE = Constants.DateNullValue;
                mDistributorInfo.DIVISION_ID = Constants.IntNullValue;
                mDistributorInfo.USER_ID = Constants.IntNullValue;
                mDistributorInfo.REGION_ID = Constants.IntNullValue;
                mDistributorInfo.ZONE_ID = Constants.IntNullValue;
                mDistributorInfo.SUBZONE_ID = Constants.IntNullValue;
                mDistributorInfo.CONTACT_PERSON = null;
                mDistributorInfo.CONTACT_NUMBER = null;
                mDistributorInfo.GST_NUMBER = null;
                mDistributorInfo.PASSWORD = null;
                mDistributorInfo.ADDRESS1 = null;
                mDistributorInfo.ADDRESS2 = null;
                mDistributorInfo.DISTRIBUTOR_ID = p_Distributor_Id;
                mDistributorInfo.DISTRIBUTOR_CODE = null;
                mDistributorInfo.DISTRIBUTOR_NAME = null;
                mDistributorInfo.IP_ADDRESS = null;
                //mDistributorInfo.IS_REGISTERED = Constants.IntNullValue;
                DataTable dt = mDistributorInfo.ExecuteTable();
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
        /// Gets Location Data
        /// </summary>
        /// <remarks>
        /// Returns Location Data as Datatable
        /// </remarks>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_distributor_type">Type</param>
        /// <param name="p_Company_Id">Company</param>
        /// <returns>Location Data as Datatable</returns>
        public DataTable SelectDistributor(int p_Distributor_Id, int p_distributor_type, int p_Company_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectDISTRIBUTOR mDistributorInfo = new uspSelectDISTRIBUTOR();
                mDistributorInfo.Connection = mConnection;


                mDistributorInfo.DIST_CLASS_ID = Constants.IntNullValue;
                mDistributorInfo.ISDELETED = false;
                mDistributorInfo.TIME_STAMP = Constants.DateNullValue;
                mDistributorInfo.LASTUPDATE_DATE = Constants.DateNullValue;
                mDistributorInfo.DIVISION_ID = Constants.IntNullValue;
                mDistributorInfo.USER_ID = Constants.IntNullValue;
                mDistributorInfo.REGION_ID = Constants.IntNullValue;
                mDistributorInfo.ZONE_ID = Constants.IntNullValue;
                mDistributorInfo.SUBZONE_ID = p_distributor_type;
                mDistributorInfo.CONTACT_PERSON = null;
                mDistributorInfo.CONTACT_NUMBER = null;
                mDistributorInfo.GST_NUMBER = null;
                mDistributorInfo.PASSWORD = null;
                mDistributorInfo.ADDRESS1 = null;
                mDistributorInfo.ADDRESS2 = null;
                mDistributorInfo.DISTRIBUTOR_ID = p_Distributor_Id;
                mDistributorInfo.DISTRIBUTOR_CODE = null;
                mDistributorInfo.DISTRIBUTOR_NAME = null;
                mDistributorInfo.IP_ADDRESS = null;
                mDistributorInfo.IS_REGISTERED = true;
                mDistributorInfo.COMPANY_ID = p_Company_Id;
                DataTable dt = mDistributorInfo.ExecuteTable();
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
        /// Gets Max DayClose
        /// </summary>
        /// <remarks>
        /// Returns Max DayClose as Datatable
        /// </remarks>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Status">Status</param>
        /// <returns>Max DayClose as Datatable</returns>
        public DataTable MaxDayClose(int p_Distributor_ID, int p_Status)
        {
            DataTable dt = new DataTable();

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectMaxDayCloseDistributor obj = new UspSelectMaxDayCloseDistributor();
                obj.Connection = mConnection;
                obj.DISTRIBUTOR_ID = p_Distributor_ID;
                obj.STATUS = p_Status;
                dt = obj.ExecuteTable();
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
        /// Gets Max DayClose
        /// </summary>
        /// <remarks>
        /// Returns Max DayClose as Datatable
        /// </remarks>
        /// <param name="UserId">InsertedBy</param>
        /// <param name="Distributor_id">Location</param>
        /// <returns>Max DayClose as Datatable</returns>
        public DataTable SelectMaxDayClose(int UserId, int Distributor_id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectMaxDayClose mMaxClose = new uspSelectMaxDayClose();
                mMaxClose.Connection = mConnection;
                mMaxClose.USER_ID = UserId;
                mMaxClose.DISTRIBUTOR_ID = Distributor_id;
                DataTable mLastClose = mMaxClose.ExecuteTable();
                return mLastClose;
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

        public DataTable GetDistributorHierarchy(int pDistributorId)
        {
            IDbConnection mconnection = null;
            DataTable mDistHierarchy = new DataTable();
            try
            {
                mconnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mconnection.Open();
                usp_GetDistributorHierarchy mGetDistHierarchy = new usp_GetDistributorHierarchy();
                mGetDistHierarchy.Connection = mconnection;
                mGetDistHierarchy.DistributorId = pDistributorId;
                mGetDistHierarchy.ExecuteTable();
                mDistHierarchy = mGetDistHierarchy.ExecuteTable();
                return mDistHierarchy;
            }
            catch (Exception ex)
            {
                ExceptionPublisher.PublishException(ex);
                return null;
            }
            finally
            {
                if (mconnection != null && mconnection.State == ConnectionState.Open)
                {
                    mconnection.Close();
                }
            }
        }

        #region Added By Hazrat Ali        

        /// <summary>
        /// Gets Location Data
        /// </summary>
        /// <remarks>
        /// Returns Location Data as Datatable
        /// </remarks>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_distributor_type">Type</param>
        /// <param name="p_Company_Id">Company</param>
        /// <returns>Location Data as Datatable</returns>
        public DataTable SelectAllDistributors(int p_Distributor_Id, int p_distributor_type, int p_Company_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectAllDISTRIBUTOR mDistributorInfo = new uspSelectAllDISTRIBUTOR();
                mDistributorInfo.Connection = mConnection;
                mDistributorInfo.DIST_CLASS_ID = Constants.IntNullValue;
                mDistributorInfo.ISDELETED = false;
                mDistributorInfo.TIME_STAMP = Constants.DateNullValue;
                mDistributorInfo.LASTUPDATE_DATE = Constants.DateNullValue;
                mDistributorInfo.DIVISION_ID = Constants.IntNullValue;
                mDistributorInfo.USER_ID = Constants.IntNullValue;
                mDistributorInfo.REGION_ID = Constants.IntNullValue;
                mDistributorInfo.ZONE_ID = Constants.IntNullValue;
                mDistributorInfo.SUBZONE_ID = p_distributor_type;
                mDistributorInfo.CONTACT_PERSON = null;
                mDistributorInfo.CONTACT_NUMBER = null;
                mDistributorInfo.GST_NUMBER = null;
                mDistributorInfo.PASSWORD = null;
                mDistributorInfo.ADDRESS1 = null;
                mDistributorInfo.ADDRESS2 = null;
                mDistributorInfo.DISTRIBUTOR_ID = p_Distributor_Id;
                mDistributorInfo.DISTRIBUTOR_CODE = null;
                mDistributorInfo.DISTRIBUTOR_NAME = null;
                mDistributorInfo.IP_ADDRESS = null;
                mDistributorInfo.IS_REGISTERED = true;
                mDistributorInfo.COMPANY_ID = p_Company_Id;
                DataTable dt = mDistributorInfo.ExecuteTable();
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
        /// Gets Menu Hierarchy For Bread Crumb
        /// </summary>
        /// <param name="p_MODULE_ID">ModuleID</param>
        /// <returns>Menu Hierarchy as Datatable</returns>
        public DataTable GetBreadCrumb(int p_MODULE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spGetBreadCrumb mBreadCrumb = new spGetBreadCrumb();
                mBreadCrumb.Connection = mConnection;


                mBreadCrumb.MODULE_ID = p_MODULE_ID;

                DataTable dt = mBreadCrumb.ExecuteTable();
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

        public DataTable GetLicenseData(int p_DISTRIBUTOR_ID)
        {
            DataTable dt = new DataTable();

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionStringEncrypt, EnumProviders.SQLClient);
                mConnection.Open();

                uspGetLicenseData obj = new uspGetLicenseData();
                obj.Connection = mConnection;
                obj.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                dt = obj.ExecuteTable();
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

        public DataTable GetReference(string p_TABLE_NAME,long p_COLUMN_VALUE)
        {
            IDbConnection mconnection = null;
            DataTable dtReference = new DataTable();

            try
            {
                mconnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mconnection.Open();

                uspGetReference mReference = new uspGetReference();
                mReference.Connection = mconnection;
                mReference.TABLE_NAME = p_TABLE_NAME;
                mReference.COLUMN_VALUE = p_COLUMN_VALUE;
                mReference.ExecuteTable();
                dtReference = mReference.ExecuteTable();
                return dtReference;
            }
            catch (Exception ex)
            {
                ExceptionPublisher.PublishException(ex);
                return null;
            }
            finally
            {
                if (mconnection != null && mconnection.State == ConnectionState.Open)
                {
                    mconnection.Close();
                }
            }
        }

        public DataTable GetDeliveryChannel(int pDistributorId)
        {
            IDbConnection mconnection = null;
            DataTable mDistHierarchy = new DataTable();

            try
            {
                mconnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mconnection.Open();

                uspGetDeliveryChannel mGetDistHierarchy = new uspGetDeliveryChannel();
                mGetDistHierarchy.Connection = mconnection;
                mGetDistHierarchy.DistributorID = pDistributorId;
                mGetDistHierarchy.ExecuteTable();
                mDistHierarchy = mGetDistHierarchy.ExecuteTable();
                return mDistHierarchy;
            }
            catch (Exception ex)
            {
                ExceptionPublisher.PublishException(ex);
                return null;
            }
            finally
            {
                if (mconnection != null && mconnection.State == ConnectionState.Open)
                {
                    mconnection.Close();
                }
            }
        }

        #endregion

        public DataTable GetWarehouseMapping(int p_TypeID, int p_WarehouseID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspWarehouseMapping mDistributorInfo = new uspWarehouseMapping();
                mDistributorInfo.Connection = mConnection;
                mDistributorInfo.TypeID = p_TypeID;
                mDistributorInfo.WarehouseID = p_WarehouseID;
                DataTable dt = mDistributorInfo.ExecuteTable();
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

        #endregion

        #region Insert, Update, And Delete

        /// <summary>
        /// Inserts Location
        /// </summary>
        /// <remarks>
        /// Returns Inserted Location ID as String
        /// </remarks>
        /// <param name="p_Company">Company</param>
        /// <param name="p_Dist_Class_Id">Class</param>
        /// <param name="p_IsDeleted">IsDeleted</param>
        /// <param name="p_Time_Stamp">CreatedOn</param>
        /// <param name="p_LastUpdate_Date">LastUpdateDate</param>
        /// <param name="p_Division_Id">Division</param>
        /// <param name="p_User_Id">InsertedBy</param>
        /// <param name="p_Region_Id">Region</param>
        /// <param name="p_Zone_Id">Zone</param>
        /// <param name="p_SubZone_Id">SubZone</param>
        /// <param name="p_Contact_Person">ContactPerson</param>
        /// <param name="p_Contact_Number">ContactNo</param>
        /// <param name="p_Gst_Number">GSTNo</param>
        /// <param name="p_Password">Password</param>
        /// <param name="p_Address1">Address1</param>
        /// <param name="p_Address2">Address2</param>
        /// <param name="p_Distributor_Code">Code</param>
        /// <param name="p_Distributor_Name">Name</param>
        /// <param name="p_Ip_Address">IP</param>
        /// <param name="p_Is_Registered">IsRegistered</param>
        /// <param name="p_CREDIT_LEVEL">CreditLevel</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <returns>Inserted Location ID as String</returns>
        public string InsertDistributor(int p_Company, int pCoverTable, bool p_IsDeleted, DateTime p_Time_Stamp, DateTime p_LastUpdate_Date, int p_Division_Id, int p_User_Id, int p_Region_Id, int p_Zone_Id, int p_SubZone_Id, string p_Contact_Person, string p_Contact_Number, string p_Gst_Number, string p_Password, string p_Address1, string p_Address2, string p_Distributor_Code, string p_Distributor_Name, string p_Ip_Address, bool p_Is_Registered, int p_CREDIT_LEVEL, int p_UserId, decimal pGst,decimal pGstCreditCard, bool p_ServiceCharges, string pic, bool showlogo, string smsDelivery, string smsTakeAway, string smsDayClose, string smscontact, bool IssmsDelivery, bool IssmsTakeAway, bool issmsDayClose,bool p_PrintKOT, bool p_PrintKOTDelivery, bool p_PrintKOTTakeaway,int p_SERVICE_CHARGES_TYPE,decimal p_SERVICE_CHARGES_VALUE,string p_STRN, string p_Latitude, string p_Longitude, int p_CityId,bool p_IsDeliveryCharges,int p_DELIVERY_CHARGES_TYPE,decimal p_DELIVERY_CHARGES_VALUE,bool p_AutoPromotion,string p_ServiceChargesLabel,decimal p_POS_FEE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertDISTRIBUTOR mDistributorInfo = new spInsertDISTRIBUTOR();
                mDistributorInfo.Connection = mConnection;
                mDistributorInfo.COMPANY_ID = p_Company;
                mDistributorInfo.USER_ID = p_UserId;
                mDistributorInfo.DIST_CLASS_ID = pCoverTable;
                mDistributorInfo.TIME_STAMP = p_Time_Stamp;
                mDistributorInfo.LASTUPDATE_DATE = p_LastUpdate_Date;
                mDistributorInfo.DIVISION_ID = p_Division_Id;
                mDistributorInfo.USER_ID = p_User_Id;
                mDistributorInfo.REGION_ID = p_Region_Id;
                mDistributorInfo.ZONE_ID = p_Zone_Id;
                mDistributorInfo.SUBZONE_ID = p_SubZone_Id;
                mDistributorInfo.CREDIT_LEVEL = p_CREDIT_LEVEL;
                mDistributorInfo.CONTACT_PERSON = p_Contact_Person;
                mDistributorInfo.CONTACT_NUMBER = p_Contact_Number;
                mDistributorInfo.GST_NUMBER = p_Gst_Number;
                mDistributorInfo.PASSWORD = p_Password;
                mDistributorInfo.ADDRESS1 = p_Address1;
                mDistributorInfo.ADDRESS2 = p_Address2;
                mDistributorInfo.DISTRIBUTOR_CODE = p_Distributor_Code;
                mDistributorInfo.DISTRIBUTOR_NAME = p_Distributor_Name;
                mDistributorInfo.USER_ID = p_UserId;
                mDistributorInfo.IP_ADDRESS = p_Ip_Address;
                mDistributorInfo.ISDELETED = p_IsDeleted;
                mDistributorInfo.IS_REGISTERED = p_Is_Registered;
                mDistributorInfo.GST = pGst;
                mDistributorInfo.GST_CREDIT_CARD = pGstCreditCard;
                mDistributorInfo.SERVICE_CHARGES = p_ServiceCharges;
                mDistributorInfo.PIC = pic;
                mDistributorInfo.SHOW_LOGO = showlogo;
                mDistributorInfo.IsSMSDELIVERY = IssmsDelivery;
                mDistributorInfo.SMSDELIVERY = smsDelivery;
                mDistributorInfo.IsSMSTAKEAWAY = IssmsTakeAway;
                mDistributorInfo.SMSTAKEAWAY = smsTakeAway;
                mDistributorInfo.IsSMSDAYCLOSE = issmsDayClose;
                mDistributorInfo.SMSDAYCLOSE = smsDayClose;
                mDistributorInfo.SMSCONTACT = smscontact;
                mDistributorInfo.PrintKOT = p_PrintKOT;
                mDistributorInfo.PrintKOTDelivery = p_PrintKOTDelivery;
                mDistributorInfo.PrintKOTTakeaway = p_PrintKOTTakeaway;
                mDistributorInfo.SERVICE_CHARGES_TYPE = p_SERVICE_CHARGES_TYPE;
                mDistributorInfo.SERVICE_CHARGES_VALUE = p_SERVICE_CHARGES_VALUE;
                mDistributorInfo.STRN = p_STRN;
                mDistributorInfo.Latitude = p_Latitude;
                mDistributorInfo.Longitude = p_Longitude;
                mDistributorInfo.CITY_ID = p_CityId;
                mDistributorInfo.IsDeliveryCharges = p_IsDeliveryCharges;
                mDistributorInfo.DELIVERY_CHARGES_TYPE = p_DELIVERY_CHARGES_TYPE;
                mDistributorInfo.DELIVERY_CHARGES_VALUE = p_DELIVERY_CHARGES_VALUE;
                mDistributorInfo.AutoPromotion = p_AutoPromotion;
                mDistributorInfo.ServiceChargesLabel = p_ServiceChargesLabel;
                mDistributorInfo.POS_FEE = p_POS_FEE;
                mDistributorInfo.ExecuteQuery();

                return mDistributorInfo.DISTRIBUTOR_ID.ToString();

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
        /// Updates Location
        /// </summary>
        /// <remarks>
        /// Returns Inserted Location ID as String
        /// </remarks>
        /// <param name="p_CompanyId">Company</param>
        /// <param name="p_Dist_Class_Id">Class</param>
        /// <param name="p_IsDeleted">IsDeleted</param>
        /// <param name="p_Time_Stamp">CreatedOn</param>
        /// <param name="p_LastUpdate_Date">LastUpdateDate</param>
        /// <param name="p_Division_Id">Division</param>
        /// <param name="p_User_Id">InsertedBy</param>
        /// <param name="p_Region_Id">Region</param>
        /// <param name="p_Zone_Id">Zone</param>
        /// <param name="p_SubZone_Id">SubZone</param>
        /// <param name="p_Contact_Person">ContactPerson</param>
        /// <param name="p_Contact_Number">ContactNo</param>
        /// <param name="p_Gst_Number">GSTNo</param>
        /// <param name="p_Password">Password</param>
        /// <param name="p_Address1">Address1</param>
        /// <param name="p_Address2">Address2</param>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Distributor_Code">Code</param>
        /// <param name="p_Distributor_Name">Name</param>
        /// <param name="p_Ip_Address">IP</param>
        /// <param name="p_Is_Registered">IsRegistered</param>
        /// <param name="p_CREDIT_LEVEL">CreditLevel</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="pGst"></param>
        /// <returns>"Record Updated." On Success and Exception.Message on Failure</returns>
        public string UpdateDistributor(int p_CompanyId, int pCoverTable, bool p_IsDeleted, DateTime p_Time_Stamp, DateTime p_LastUpdate_Date, int p_Division_Id, int p_User_Id, int p_Region_Id, int p_Zone_Id, int p_SubZone_Id, string p_Contact_Person, string p_Contact_Number, string p_Gst_Number, string p_Password, string p_Address1, string p_Address2, int p_Distributor_Id, string p_Distributor_Code, string p_Distributor_Name, string p_Ip_Address, bool p_Is_Registered, int p_CREDIT_LEVEL, int p_UserId, decimal pGst,decimal pGstCreditCard, bool p_ServiceCharges, string pic, bool showlogo, string smsDelivery, string smsTakeAway, string smsDayClose, string smscontact, bool IssmsDelivery, bool IssmsTakeAway, bool issmsDayClose,bool p_PrintKOT,bool p_PrintKOTDelivery,bool p_PrintKOTTakeaway, int p_SERVICE_CHARGES_TYPE,decimal p_SERVICE_CHARGES_VALUE,string p_STRN, string p_Latitude, string p_Longitude, int p_CityId, bool p_IsDeliveryCharges, int p_DELIVERY_CHARGES_TYPE, decimal p_DELIVERY_CHARGES_VALUE,bool p_AutoPromotion,string p_ServiceChargesLabel,decimal p_POS_FEE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateDISTRIBUTOR mDistributorInfo = new spUpdateDISTRIBUTOR();
                mDistributorInfo.Connection = mConnection;
                mDistributorInfo.COMPANY_ID = p_CompanyId;
                mDistributorInfo.DIST_CLASS_ID = pCoverTable;
                mDistributorInfo.DIVISION_ID = p_Division_Id;
                mDistributorInfo.USER_ID = p_User_Id;
                mDistributorInfo.REGION_ID = p_Region_Id;
                mDistributorInfo.ZONE_ID = p_Zone_Id;
                mDistributorInfo.SUBZONE_ID = p_SubZone_Id;
                mDistributorInfo.CREDIT_LEVEL = p_CREDIT_LEVEL;
                mDistributorInfo.CONTACT_PERSON = p_Contact_Person;
                mDistributorInfo.CONTACT_NUMBER = p_Contact_Number;
                mDistributorInfo.GST_NUMBER = p_Gst_Number;
                mDistributorInfo.PASSWORD = p_Password;
                mDistributorInfo.ADDRESS1 = p_Address1;
                mDistributorInfo.ADDRESS2 = p_Address2;
                mDistributorInfo.DISTRIBUTOR_ID = p_Distributor_Id;
                mDistributorInfo.DISTRIBUTOR_CODE = p_Distributor_Code;
                mDistributorInfo.DISTRIBUTOR_NAME = p_Distributor_Name;
                mDistributorInfo.IP_ADDRESS = p_Ip_Address;
                mDistributorInfo.IS_REGISTERED = p_Is_Registered;
                mDistributorInfo.USER_ID = p_UserId;
                mDistributorInfo.ISDELETED = p_IsDeleted;
                mDistributorInfo.GST = pGst;
                mDistributorInfo.GST_CREDIT_CARD = pGstCreditCard;
                mDistributorInfo.SERVICE_CHARGES = p_ServiceCharges;
                mDistributorInfo.PIC = pic;
                mDistributorInfo.SHOW_LOGO = showlogo;
                mDistributorInfo.IsSMSDELIVERY = IssmsDelivery;
                mDistributorInfo.SMSDELIVERY = smsDelivery;
                mDistributorInfo.IsSMSTAKEAWAY = IssmsTakeAway;
                mDistributorInfo.SMSTAKEAWAY = smsTakeAway;
                mDistributorInfo.IsSMSDAYCLOSE = issmsDayClose;
                mDistributorInfo.SMSDAYCLOSE = smsDayClose;
                mDistributorInfo.SMSCONTACT = smscontact;
                mDistributorInfo.PrintKOT = p_PrintKOT;
                mDistributorInfo.PrintKOTDelivery = p_PrintKOTDelivery;
                mDistributorInfo.PrintKOTTakeaway = p_PrintKOTTakeaway;
                mDistributorInfo.SERVICE_CHARGES_TYPE = p_SERVICE_CHARGES_TYPE;
                mDistributorInfo.SERVICE_CHARGES_VALUE = p_SERVICE_CHARGES_VALUE;
                mDistributorInfo.STRN = p_STRN;
                mDistributorInfo.Latitude = p_Latitude;
                mDistributorInfo.Longitude = p_Longitude;
                mDistributorInfo.CITY_ID = p_CityId;
                mDistributorInfo.IsDeliveryCharges = p_IsDeliveryCharges;
                mDistributorInfo.DELIVERY_CHARGES_TYPE = p_DELIVERY_CHARGES_TYPE;
                mDistributorInfo.DELIVERY_CHARGES_VALUE = p_DELIVERY_CHARGES_VALUE;
                mDistributorInfo.AutoPromotion = p_AutoPromotion;
                mDistributorInfo.ServiceChargesLabel = p_ServiceChargesLabel;
                mDistributorInfo.POS_FEE = p_POS_FEE;
                mDistributorInfo.ExecuteQuery();
                return "Record Updated.";

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
        /// Closes Day Transactions And Insert Relevant Data To GL
        /// </summary>
        /// <remarks>
        /// This Function Performs Following Tasks
        /// <list type="bullet">
        /// <item>
        /// Closes Day Transactions
        /// </item>
        /// <item>
        /// Inserts Cash Data To GL Through PostCashDeposit() Function
        /// </item>
        /// <item>
        /// Inserts Cheques Data To GL Through PostCashDeposit() Function
        /// </item>
        /// <item>
        /// Inserts Expenses Data To GL Through PettyDeposit() Function
        /// </item>
        /// <item>
        /// Inserts Sales And Sales Return Data To GL Through PostSaleVoucher() Function
        /// </item>
        /// <iterm>
        /// Inserts Purchase Data To GL Through PostPurchaseVoucher() Function
        /// </iterm>
        /// <item>
        /// Inserts Purchase Return Data To GL Through PostPurchaseReturnVoucher() Function
        /// </item>
        /// <item>
        /// Inserts Rate Difference Data To GL Through PostRateDifferenceVoucher() Function
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="p_DayClose">ClosingDate</param>
        /// <param name="Distributor_id">Location</param>
        /// <param name="UserId">InsertedBy</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool UspDayClose(DateTime p_DayClose, int Distributor_id, int UserId)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                UspDayClosing mMaxClose = new UspDayClosing();
                mMaxClose.Connection = mConnection;
                mMaxClose.Transaction = mTransaction;
                mMaxClose.DAYCLOSE = p_DayClose;
                mMaxClose.DISTRIBUTOR_ID = Distributor_id;
                mMaxClose.USER_ID = UserId;
                bool mLastClose = mMaxClose.ExecuteQuery();
                if (mLastClose == true)
                {
                    mTransaction.Commit();
                }
                else
                {
                    mTransaction.Rollback();
                }
                return mLastClose;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
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
        public bool UspDayReverse(DateTime p_DayReverse, int Distributor_id, int UserId)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                UspReverseDayClosing mMaxClose = new UspReverseDayClosing();
                mMaxClose.Connection = mConnection;
                mMaxClose.Transaction = mTransaction;
                mMaxClose.DAYCLOSE = p_DayReverse;
                mMaxClose.DISTRIBUTOR_ID = Distributor_id;
                mMaxClose.USER_ID = UserId;
                bool mLastClose = mMaxClose.ExecuteQuery();
                if (mLastClose == true)
                {
                    mTransaction.Commit();
                }
                else
                {
                    mTransaction.Rollback();
                }
                return mLastClose;
            }
            catch (Exception exp)
            {
               
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
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

        public bool InsertWarehouseMapping(int p_TypeID, int p_WarehouseID, int p_DistributorID, int p_UserID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspWarehouseMapping mUserAssing = new uspWarehouseMapping();
                mUserAssing.Connection = mConnection;
                mUserAssing.TypeID = p_TypeID;
                mUserAssing.WarehouseID = p_WarehouseID;
                mUserAssing.DistributorID = p_DistributorID;
                mUserAssing.UserID = p_UserID;
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