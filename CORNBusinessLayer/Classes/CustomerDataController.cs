using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Customer Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Customer
    /// </item>
    /// <term>
    /// Update Customer
    /// </term>
    /// <item>
    /// Get Customer
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class CustomerDataController
    {
        #region Constructor

        /// <summary>
        /// Constructor For CustomerDataController
        /// </summary>
        public CustomerDataController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region Public Methods

        #region Select
        public DataTable GetCustomerDropDown(int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetProfession ObjSelect = new uspGetProfession();
                ObjSelect.Connection = mConnection;
                ObjSelect.TypeID = p_TypeID;
                DataTable dt = ObjSelect.ExecuteTableForCustomerDropDown();
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
        
        public DataTable SelectAllCustomer(int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetAllCUSTOMER mCustData = new uspGetAllCUSTOMER();
                mCustData.Connection = mConnection;

                mCustData.TypeID = p_TypeID;
                DataTable dt = mCustData.ExecuteTable();
                DataView dv = new DataView(dt);
                dv.Sort = "CUSTOMER_NAME";
                dt = dv.ToTable();
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
        /// Gets Customer Opening Balance
        /// </summary>
        /// <remarks>
        /// Returns Customer Opening Balance as Datatable
        /// </remarks>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_CustomerId">Customer</param>
        /// <param name="p_fromDate">DateFrom</param>
        /// <param name="p_PrincipalId">Principal</param>
        /// <returns>Customer Opening Balance as Datatable</returns>
        public DataTable SelectPrincipalCustomerOpBalance(int p_Distributor_Id, int p_CustomerId, DateTime p_fromDate, int p_PrincipalId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspCustomerOpBalance mCustData = new UspCustomerOpBalance();
                mCustData.Connection = mConnection;

                mCustData.Distributor_id = p_Distributor_Id;
                mCustData.principal_id = p_PrincipalId;
                mCustData.From_Date = p_fromDate;
                mCustData.customer_id = p_CustomerId;
                DataTable dt = mCustData.ExecuteTable();
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
        public DataTable SelectPrincipalCustomerOpBalanceFranchise(int p_Distributor_Id, int p_CustomerId, DateTime p_fromDate, int p_PrincipalId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspCustomerOpBalanceFranchise mCustData = new UspCustomerOpBalanceFranchise();
                mCustData.Connection = mConnection;

                mCustData.Distributor_id = p_Distributor_Id;
                mCustData.principal_id = p_PrincipalId;
                mCustData.From_Date = p_fromDate;
                mCustData.customer_id = p_CustomerId;
                DataTable dt = mCustData.ExecuteTable();
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
        /// Gets Customer For Different Search Criterias
        /// </summary>
        /// <remarks>
        /// Returns Customer as Datatable
        /// </remarks>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_FIELDNAME">FieldToSearchFor</param>
        /// <param name="p_PARAMETERNAME">ValueToSearch</param>
        /// <returns>Customer as Datatable</returns>
        public DataTable UspSelectCustomer(int p_Distributor_Id, string p_FIELDNAME, string p_PARAMETERNAME, DateTime pDOCUMENT_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectCUSTOMER mCustData = new UspSelectCUSTOMER();
                mCustData.Connection = mConnection;
                mCustData.FEILDNAME = p_FIELDNAME;
                mCustData.PARAMETER = p_PARAMETERNAME;
                mCustData.DISTRIBUTOR_ID = p_Distributor_Id;
                mCustData.DOCUMENT_DATE = pDOCUMENT_DATE;
                DataTable dt = mCustData.ExecuteTable();
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
        public DataTable UspGetCustomerThirdPartyDelivery(int p_Distributor_Id, string p_FIELDNAME, string p_PARAMETERNAME, DateTime pDOCUMENT_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectCUSTOMER mCustData = new UspSelectCUSTOMER();
                mCustData.Connection = mConnection;
                mCustData.FEILDNAME = p_FIELDNAME;
                mCustData.PARAMETER = p_PARAMETERNAME;
                mCustData.DISTRIBUTOR_ID = p_Distributor_Id;
                mCustData.DOCUMENT_DATE = pDOCUMENT_DATE;
                DataTable dt = mCustData.ExecuteTableThirdPartyDelivery();
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
        public DataTable SelectCustomerLocationWise(int p_Type_id, int p_Distributor_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                SpSelectLocationWiseCustomer mCustData = new SpSelectLocationWiseCustomer();
                mCustData.Connection = mConnection;
                mCustData.TYPE_ID = p_Type_id;
                mCustData.DISTRIBUTOR_ID = p_Distributor_Id;

                DataTable dt = mCustData.ExecuteTable();
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

        public DataTable GetCustomerGroup(int p_CustomerGroupCode)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetCustomerGroup mCustData = new uspGetCustomerGroup();
                mCustData.Connection = mConnection;
                mCustData.CustomerGroupCode = p_CustomerGroupCode;

                DataTable dt = mCustData.ExecuteTable();
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

        public DataTable SelectCreditCustomer(string p_Distributor_Id, int p_Type_id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectPendingCreditInvoice mCust = new UspSelectPendingCreditInvoice();
                mCust.Connection = mConnection;
                mCust.DISTRIBUTOR_IDS = p_Distributor_Id;
                mCust.AREA_ID = p_Type_id;
                DataTable dt = mCust.ExecuteTable();
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

        /// <summary>
        /// Inserts Customer
        /// </summary>
        /// <remarks>
        /// Returns "Record Updated Customer Code " + Inserted Customer ID On Success And "Some Error Update Record" On Failure
        /// </remarks>
        /// <param name="p_Customer_Id">Customer</param>
        /// <param name="p_Is_Gst_Registered">IsGSTReg</param>
        /// <param name="p_Is_Active">IsActive</param>
        /// <param name="p_Channel_Type_Id">Type</param>
        /// <param name="p_Volume_Class_Id">Class</param>
        /// <param name="p_Business_Type_Id">BusinessType</param>
        /// <param name="p_Area_Id">Market</param>
        /// <param name="p_Route_Id">Route</param>
        /// <param name="p_Town_Id">Town</param>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Gst_Number">GSTNumber</param>
        /// <param name="p_Contact_Person">ContactPerson</param>
        /// <param name="p_Contact_Number">ContactNo</param>
        /// <param name="p_Email_Address">Email</param>
        /// <param name="p_Customer_Code">Code</param>
        /// <param name="p_Customer_Name">Name</param>
        /// <param name="p_Address">Address</param>
        /// <param name="p_RegDate">RegDate</param>
        /// <param name="p_Stand">Stand</param>
        /// <param name="p_Cooler">Cooler</param>
        /// <param name="p_CNIC">CNIC</param>
        /// <param name="p_NTN">NTN</param>
        /// <returns>"Record Updated Customer Code " + Inserted Customer ID On Success And "Some Error Update Record" On Failure</returns>

        public static bool InsertCustomer(int p_Distributor_Id, string p_CardId, string p_CNIC, string p_DOB,string p_Contact_Number, string p_Email_Address, string p_Customer_Name, string p_Address, string Barcode, decimal OpeningAmount, string Nature, string contact2, int p_CARD_TYPE_ID, decimal p_DISCOUNT, decimal p_PURCHASING, decimal p_POINTS, decimal p_AMOUNT_LIMIT, int p_Gender, int p_Occupation)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                using (spInsertCUSTOMER mCustomer = new spInsertCUSTOMER())
                {
                    mCustomer.Connection = mConnection;

                    mCustomer.ADDRESS = p_Address;
                    mCustomer.AREA_ID = 0;
                    mCustomer.BUSINESS_TYPE_ID = 0;
                    mCustomer.CHANNEL_TYPE_ID = 0;
                    mCustomer.CONTACT_NUMBER = p_Contact_Number;
                    mCustomer.CONTACT2 = contact2;
                    mCustomer.CONTACT_PERSON = "";
                    mCustomer.CUSTOMER_CODE = p_CardId;
                    mCustomer.CUSTOMER_NAME = p_Customer_Name;
                    mCustomer.EMAIL_ADDRESS = p_Email_Address;
                    mCustomer.CNIC = p_CNIC;
                    mCustomer.GST_NUMBER = "";
                    mCustomer.NTN = "";
                    mCustomer.IS_GST_REGISTERED = true;
                    mCustomer.TOWN_ID = 0;
                    mCustomer.PROMOTION_CLASS = 0;
                    mCustomer.DISTRIBUTOR_ID = p_Distributor_Id;
                    mCustomer.TOWN_ID = 0;
                    mCustomer.ROUTE_ID = 0;
                    mCustomer.IS_ACTIVE = true;
                    mCustomer.TIME_STAMP = System.DateTime.Now;
                    mCustomer.LASTUPDATE_DATE = System.DateTime.Now;
                    mCustomer.GENDER = p_Gender;
                    mCustomer.OCCUPATION = p_Occupation;
                    if (p_DOB != "")
                    {
                        mCustomer.REGDATE = Convert.ToDateTime(p_DOB);
                    }
                    mCustomer.IS_COOLER = 0;
                    mCustomer.IS_STAND = 0;

                    mCustomer.BARCODE = Barcode;
                    mCustomer.OPENING_AMOUNT = OpeningAmount;
                    mCustomer.NATURE = Nature;
                    mCustomer.CARD_TYPE_ID = p_CARD_TYPE_ID;
                    mCustomer.DISCOUNT = p_DISCOUNT;
                    mCustomer.PURCHASING = p_PURCHASING;
                    mCustomer.POINTS = p_POINTS;
                    mCustomer.AMOUNT_LIMIT = p_AMOUNT_LIMIT;
                    mCustomer.strCardNo = p_CardId;
                    mCustomer.FROM_DATE = Constants.DateNullValue;
                    mCustomer.TO_DATE = Constants.DateNullValue;
                    mCustomer.Membership_Date = Constants.DateNullValue;
                    mCustomer.ExecuteQuery();
                    return true;
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
        public static DataTable InsertCustomer(int p_Distributor_Id, string p_CardId, string p_CNIC, string p_DOB, string p_Contact_Number, string p_Email_Address, string p_Customer_Name, string p_Address, decimal OpeningAmount, string Nature, string contact2, int p_Gender, int p_Occupation)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                using (spInsertCUSTOMER mCustomer = new spInsertCUSTOMER())
                {
                    mCustomer.Connection = mConnection;

                    mCustomer.ADDRESS = p_Address;
                    mCustomer.AREA_ID = 0;
                    mCustomer.BUSINESS_TYPE_ID = 0;
                    mCustomer.CHANNEL_TYPE_ID = 0;
                    mCustomer.CONTACT_NUMBER = p_Contact_Number;
                    mCustomer.CONTACT2 = contact2;
                    mCustomer.CONTACT_PERSON = "";
                    mCustomer.CUSTOMER_CODE = p_CardId;
                    mCustomer.CUSTOMER_NAME = p_Customer_Name;
                    mCustomer.EMAIL_ADDRESS = p_Email_Address;
                    mCustomer.CNIC = p_CNIC;
                    mCustomer.GST_NUMBER = "";
                    mCustomer.NTN = "";
                    mCustomer.IS_GST_REGISTERED = true;
                    mCustomer.TOWN_ID = 0;
                    mCustomer.PROMOTION_CLASS = 0;
                    mCustomer.DISTRIBUTOR_ID = p_Distributor_Id;
                    mCustomer.TOWN_ID = 0;
                    mCustomer.ROUTE_ID = 0;
                    mCustomer.IS_ACTIVE = true;
                    mCustomer.TIME_STAMP = System.DateTime.Now;
                    mCustomer.LASTUPDATE_DATE = System.DateTime.Now;
                    mCustomer.GENDER = p_Gender;
                    mCustomer.OCCUPATION = p_Occupation;
                    if (p_DOB != "")
                    {
                        mCustomer.REGDATE = Convert.ToDateTime(p_DOB);
                    }
                    mCustomer.IS_COOLER = 0;
                    mCustomer.IS_STAND = 0;

                    mCustomer.BARCODE = null;
                    mCustomer.OPENING_AMOUNT = OpeningAmount;
                    mCustomer.NATURE = Nature;
                    mCustomer.CARD_TYPE_ID = 0;
                    mCustomer.DISCOUNT = 0;
                    mCustomer.PURCHASING = 0;
                    mCustomer.POINTS = 0;
                    mCustomer.AMOUNT_LIMIT = 0;
                    mCustomer.strCardNo = p_CardId;
                    mCustomer.FROM_DATE = Constants.DateNullValue;
                    mCustomer.TO_DATE = Constants.DateNullValue;
                    mCustomer.Membership_Date = Constants.DateNullValue;
                    DataTable dt = mCustomer.ExecuteTable();
                    return dt;
                }
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
        public static DataTable InsertCustomer(int p_Distributor_Id, string p_CardId, string p_CNIC, string p_DOB,string p_Contact_Number, string p_Email_Address, string p_Customer_Name, string p_Address,string Barcode, decimal OpeningAmount, string Nature, string contact2, int p_CARD_TYPE_ID,decimal p_DISCOUNT, decimal p_PURCHASING, decimal p_POINTS,decimal p_AMOUNT_LIMIT,int p_Gender,int p_Occupation,decimal p_SalePer, DateTime p_membership_Date,string p_spouse, int p_profession,string p_membership_No, int p_Category, int p_Status, bool p_golfer, DateTime p_fromDate, DateTime p_toDate,int p_GroupID,int p_LoyaltyCard_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertCUSTOMER mCustomer = new spInsertCUSTOMER();
                mCustomer.Connection = mConnection;

                mCustomer.ADDRESS = p_Address;
                mCustomer.AREA_ID = 0;
                mCustomer.BUSINESS_TYPE_ID = 0;
                mCustomer.CHANNEL_TYPE_ID = 0;
                mCustomer.CONTACT_NUMBER = p_Contact_Number;
                mCustomer.CONTACT2 = contact2;
                mCustomer.CONTACT_PERSON = "";
                mCustomer.CUSTOMER_CODE = p_CardId;
                mCustomer.CUSTOMER_NAME = p_Customer_Name;
                mCustomer.EMAIL_ADDRESS = p_Email_Address;
                mCustomer.CNIC = p_CNIC;
                mCustomer.GST_NUMBER = "";
                mCustomer.NTN = "";
                mCustomer.IS_GST_REGISTERED = true;
                mCustomer.TOWN_ID = 0;  
                mCustomer.PROMOTION_CLASS = 0;
                mCustomer.DISTRIBUTOR_ID = p_Distributor_Id;
                mCustomer.TOWN_ID = 0;
                mCustomer.ROUTE_ID = 0;
                mCustomer.IS_ACTIVE = true;
                mCustomer.TIME_STAMP = System.DateTime.Now;
                mCustomer.LASTUPDATE_DATE = System.DateTime.Now;
                mCustomer.GENDER = p_Gender;
                mCustomer.OCCUPATION = p_Occupation;
                mCustomer.Membership_Date = p_membership_Date;
                mCustomer.Spouse_Name = p_spouse;
                mCustomer.Profession = p_profession;
                mCustomer.Membership_No = p_membership_No;
                mCustomer.CUSTOMER_CATEGORY_ID = p_Category;
                mCustomer.CUSTOMER_STATUS_ID = p_Status;
                mCustomer.IsGolfer = p_golfer;
                mCustomer.FROM_DATE = p_fromDate;
                mCustomer.TO_DATE = p_toDate;
                if (p_DOB != "")
                {
                    mCustomer.REGDATE = Convert.ToDateTime(p_DOB);
                }
                mCustomer.IS_COOLER = 0;
                mCustomer.IS_STAND = 0;

                mCustomer.BARCODE = Barcode;
                mCustomer.OPENING_AMOUNT = OpeningAmount;
                mCustomer.NATURE = Nature;
                mCustomer.CARD_TYPE_ID = p_CARD_TYPE_ID;
                mCustomer.DISCOUNT = p_DISCOUNT;
                mCustomer.PURCHASING = p_PURCHASING;
                mCustomer.POINTS = p_POINTS;
                mCustomer.AMOUNT_LIMIT = p_AMOUNT_LIMIT;
                mCustomer.strCardNo = p_CardId;
                mCustomer.SALES_PER = p_SalePer;
                mCustomer.GroupID = p_GroupID;
                mCustomer.LOYALTY_CARD_ID = p_LoyaltyCard_ID;
                DataTable dt = mCustomer.ExecuteTable();
                return dt;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return new DataTable();
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }

        }

        public static long InsertCustomer2(int p_Distributor_Id, string p_CardId, string p_CNIC, string p_DOB,
               string p_Contact_Number, string p_Email_Address, string p_Customer_Name, string p_Address, string Barcode, decimal OpeningAmount, string Nature, string contact2
              , int p_CARD_TYPE_ID, decimal p_DISCOUNT, decimal p_PURCHASING, decimal p_POINTS, decimal p_AMOUNT_LIMIT)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                using (spInsertCUSTOMER2 mCustomer = new spInsertCUSTOMER2())
                {
                    mCustomer.Connection = mConnection;

                    mCustomer.ADDRESS = p_Address;
                    mCustomer.AREA_ID = 0;
                    mCustomer.BUSINESS_TYPE_ID = 0;
                    mCustomer.CHANNEL_TYPE_ID = 0;
                    mCustomer.CONTACT_NUMBER = p_Contact_Number;
                    mCustomer.CONTACT2 = contact2;
                    mCustomer.CONTACT_PERSON = "";
                    mCustomer.CUSTOMER_CODE = p_CardId;
                    mCustomer.CUSTOMER_NAME = p_Customer_Name;
                    mCustomer.EMAIL_ADDRESS = p_Email_Address;
                    mCustomer.CNIC = p_CNIC;
                    mCustomer.GST_NUMBER = "";
                    mCustomer.NTN = "";
                    mCustomer.IS_GST_REGISTERED = true;
                    mCustomer.TOWN_ID = 0;
                    mCustomer.PROMOTION_CLASS = 0;
                    mCustomer.DISTRIBUTOR_ID = p_Distributor_Id;
                    mCustomer.TOWN_ID = 0;
                    mCustomer.ROUTE_ID = 0;
                    mCustomer.IS_ACTIVE = true;
                    mCustomer.TIME_STAMP = DateTime.Now;
                    mCustomer.LASTUPDATE_DATE = DateTime.Now;
                    mCustomer.REGDATE = DateTime.Now;
                    mCustomer.IS_COOLER = 0;
                    mCustomer.IS_STAND = 0;

                    mCustomer.BARCODE = Barcode;
                    mCustomer.OPENING_AMOUNT = OpeningAmount;
                    mCustomer.NATURE = Nature;
                    mCustomer.CARD_TYPE_ID = p_CARD_TYPE_ID;
                    mCustomer.DISCOUNT = p_DISCOUNT;
                    mCustomer.PURCHASING = p_PURCHASING;
                    mCustomer.POINTS = p_POINTS;
                    mCustomer.AMOUNT_LIMIT = p_AMOUNT_LIMIT;
                    mCustomer.strCardNo = p_CardId;
                    mCustomer.ExecuteQuery();
                    return mCustomer.CUSTOMER_ID;
                }
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return Constants.LongNullValue;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public static long InsertCustomer3(int p_Distributor_Id,string p_Contact_Number, string p_Email_Address, string p_Customer_Name,string p_WhatsApp_No,DateTime p_dtDOB)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertCUSTOMER3 mCustomer = new spInsertCUSTOMER3();
                mCustomer.Connection = mConnection;

                mCustomer.CONTACT_NUMBER = p_Contact_Number;
                mCustomer.CUSTOMER_NAME = p_Customer_Name;
                mCustomer.EMAIL_ADDRESS = p_Email_Address;
                mCustomer.DISTRIBUTOR_ID = p_Distributor_Id;
                mCustomer.WHATSAPP_NO = p_WhatsApp_No;
                mCustomer.dtDOB = p_dtDOB;
                mCustomer.ExecuteQuery();
                return mCustomer.CUSTOMER_ID;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return Constants.LongNullValue;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public static bool InsertCustomerThirdPartyDelivery(int p_ThirdPartyDeliveryID,int p_LocationID,int p_UserID,string p_Name,string p_Address, string p_ContactNo)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                using (uspInsertCustomereThirdParty mCustomer = new uspInsertCustomereThirdParty())
                {
                    mCustomer.Connection = mConnection;

                    mCustomer.ThirdPartyDeliveryID = p_ThirdPartyDeliveryID;
                    mCustomer.LocationID = p_LocationID;
                    mCustomer.UserID = p_UserID;
                    mCustomer.Name = p_Name;
                    mCustomer.Address = p_Address;
                    mCustomer.ContactNo = p_ContactNo;
                    mCustomer.ExecuteQuery();
                    return true;
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

        public static bool UpdateCustomerAddress(long p_Customer_Id, string p_Address)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateCustomerAddress mCustomer = new spUpdateCustomerAddress();
                mCustomer.Connection = mConnection;
                mCustomer.ADDRESS = p_Address;
                mCustomer.CUSTOMER_ID = p_Customer_Id;
                mCustomer.ExecuteQuery();
                return true;
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
        /// Updates Customer
        /// </summary>
        /// <remarks>
        /// Returns "Record Updated Customer Code " + Inserted Customer ID On Success And "Some Error Update Record" On Failure
        /// </remarks>
        /// <param name="p_Customer_Id">Customer</param>
        /// <param name="p_Is_Gst_Registered">GSTReg</param>
        /// <param name="p_Is_Active">IsActive</param>
        /// <param name="p_Channel_Type_Id">Channel</param>
        /// <param name="p_Volume_Class_Id">Class</param>
        /// <param name="p_Business_Type_Id">BusinessType</param>
        /// <param name="p_Area_Id">Market</param>
        /// <param name="p_Route_Id">Route</param>
        /// <param name="p_Town_Id">Town</param>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Gst_Number">GSTNo</param>
        /// <param name="p_Contact_Person">ContactPerson</param>
        /// <param name="p_Contact_Number">ContactNo</param>
        /// <param name="p_Email_Address">Email</param>
        /// <param name="p_Customer_Code">Code</param>
        /// <param name="p_Customer_Name">Name</param>
        /// <param name="p_Address">Address</param>
        /// <param name="p_RegDate">RegDate</param>
        /// <param name="p_Stand">Stand</param>
        /// <param name="p_Cooler">Cooler</param>
        /// <param name="p_CNIC">CNIC</param>
        /// <param name="p_NTN">NTN</param>
        /// <returns>"Record Updated Customer Code " + Inserted Customer ID On Success And "Some Error Update Record" On Failure</returns>
        public string UpdateCustomer(long p_Customer_Id, bool p_Is_Active, int p_Distributor_Id, string p_Contact_Number,
            string p_Email_Address, string p_Customer_Code, string p_Customer_Name, string p_Address, 
            DateTime p_RegDate,string p_CNIC, string barcode, string nature, decimal openingAmount,
            string contact2, int p_CARD_TYPE_ID, decimal p_DISCOUNT, decimal p_PURCHASING, decimal p_POINTS,
            decimal p_AMOUNT_LIMIT,decimal p_SalePer, DateTime p_membership_Date, string p_spouse, int p_profession,
            string p_membership_No, int p_Category, int p_Status, bool p_golfer, DateTime p_fromDate, DateTime p_toDate,int p_GroupID,int p_LoyaltyCard_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateCUSTOMER mCustomer = new spUpdateCUSTOMER();
                mCustomer.Connection = mConnection;

                mCustomer.CUSTOMER_ID = p_Customer_Id;
                mCustomer.ADDRESS = p_Address;
                mCustomer.AREA_ID = 0;
                mCustomer.BUSINESS_TYPE_ID = 0;
                mCustomer.CHANNEL_TYPE_ID = 0;
                mCustomer.CONTACT_NUMBER = p_Contact_Number;
                mCustomer.CONTACT_PERSON = null;
                mCustomer.CUSTOMER_CODE = p_Customer_Code;
                mCustomer.CUSTOMER_NAME = p_Customer_Name;
                mCustomer.EMAIL_ADDRESS = p_Email_Address;
                mCustomer.CNIC = p_CNIC;
                mCustomer.GST_NUMBER = null;
                mCustomer.NTN = null;
                mCustomer.REGDATE = p_RegDate;
                mCustomer.IS_GST_REGISTERED = true;
                mCustomer.TOWN_ID = 0;
                mCustomer.ROUTE_ID = 0;
                mCustomer.PROMOTION_CLASS = 0;
                mCustomer.DISTRIBUTOR_ID = Constants.IntNullValue;
                mCustomer.IS_ACTIVE = p_Is_Active;
                mCustomer.TIME_STAMP = Constants.DateNullValue;
                mCustomer.LASTUPDATE_DATE = Constants.DateNullValue;
                mCustomer.IS_COOLER = 0;
                mCustomer.IS_STAND = 0;
                mCustomer.BARCODE = barcode;
                mCustomer.CONTACT2 = contact2;
                mCustomer.NATURE = nature;
                mCustomer.OPENING_AMOUNT = openingAmount;
                mCustomer.REGDATE = p_RegDate;

                mCustomer.CARD_TYPE_ID = p_CARD_TYPE_ID;
                mCustomer.DISCOUNT = p_DISCOUNT;
                mCustomer.PURCHASING = p_PURCHASING;
                mCustomer.POINTS = p_POINTS;
                mCustomer.AMOUNT_LIMIT = p_AMOUNT_LIMIT;
                mCustomer.strCardNo = p_Customer_Code;
                mCustomer.SALES_PER = p_SalePer;

                mCustomer.Membership_Date = p_membership_Date;
                mCustomer.Spouse_Name = p_spouse;
                mCustomer.Profession = p_profession;
                mCustomer.Membership_No = p_membership_No;
                mCustomer.CUSTOMER_CATEGORY_ID = p_Category;
                mCustomer.CUSTOMER_STATUS_ID = p_Status;
                mCustomer.IsGolfer = p_golfer;
                mCustomer.FROM_DATE = p_fromDate;
                mCustomer.TO_DATE = p_toDate;
                mCustomer.GroupID = p_GroupID;
                mCustomer.LOYALTY_CARD_ID = p_LoyaltyCard_ID;
                mCustomer.ExecuteQuery();
                return "Record Updated Customer Code " + p_Customer_Code;

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
        /// Inserts Customer From Excel File
        /// </summary>
        /// <remarks>
        /// Returns True On Success And False On Failure
        /// </remarks>
        /// <param name="p_TownId">Town</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="pFileName">ExcelFile</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool ImportCustomer(spInsertCUSTOMER mDistRoute)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;

            mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
            mConnection.Open();
            mTransaction = ProviderFactory.GetTransaction(mConnection);

            try
            {
                spInsertCUSTOMER newConn = new spInsertCUSTOMER();
                newConn = mDistRoute;
                newConn.Connection = mConnection;
                newConn.Transaction = mTransaction;
                newConn.ExecuteQuery();
                mTransaction.Commit();
                return true;
            }
            catch (Exception excp)
            {
                mTransaction.Rollback();
                mConnection.Close();
                ExceptionPublisher.PublishException(excp);
                throw;
            }
            finally
            {
                mConnection.Close();
            }
        }

        #endregion

        #endregion
        #region Family Details
        public DataTable GetFamilyDetail(long p_Customer_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectCUSTOMER mCustData = new spSelectCUSTOMER();
                mCustData.Connection = mConnection;

                mCustData.CUSTOMER_ID = p_Customer_Id;
                DataTable dt = mCustData.ExecuteTableForFamilyDetails();
                //DataView dv = new DataView(dt);
                //dv.Sort = "CUSTOMER_NAME";
                //dt = dv.ToTable();
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
        public long InsertFamilyDetails(long p_Customer_Id, string p_Child_Name, DateTime p_dob, string p_gender)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertCUSTOMER3 mCustomer = new spInsertCUSTOMER3();
                mCustomer.Connection = mConnection;

                mCustomer.CUST_ID = p_Customer_Id;
                mCustomer.CHILD_NAME = p_Child_Name;
                mCustomer.CHILD_DOB = p_dob;
                mCustomer.CHILD_GENDER = p_gender;
                mCustomer.ExecuteQueryForCustomerFamily();
                return mCustomer.CUSTOMER_ID;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return Constants.LongNullValue;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public DataTable DeleteCUSTOMER_FAMILY(long p_Customer_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectCUSTOMER mCustData = new spSelectCUSTOMER();
                mCustData.Connection = mConnection;

                mCustData.CUSTOMER_ID = p_Customer_Id;
                DataTable dt = mCustData.ExecuteTableForFamilyDetailsDelete();
                //DataView dv = new DataView(dt);
                //dv.Sort = "CUSTOMER_NAME";
                //dt = dv.ToTable();
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
        #region Invoice Calculation
        public long InsertCustomerInvoiceCalculation(long p_CUSTOMER_ID, decimal p_GST, int p_GST_Type, decimal p_DISCOUNT,int p_DISCOUNT_Type,decimal p_SERVICE_CHARGES,int p_SERVICE_CHARGES_Type)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspInsertCustomerInvoiceCalculation mCustomer = new uspInsertCustomerInvoiceCalculation();
                mCustomer.Connection = mConnection;

                mCustomer.CUSTOMER_ID = p_CUSTOMER_ID;
                mCustomer.GST = p_GST;
                mCustomer.GST_Type = p_GST_Type;
                mCustomer.DISCOUNT = p_DISCOUNT;
                mCustomer.DISCOUNT_Type = p_DISCOUNT_Type;
                mCustomer.SERVICE_CHARGES = p_SERVICE_CHARGES;
                mCustomer.SERVICE_CHARGES_Type = p_SERVICE_CHARGES_Type;
                mCustomer.ExecuteQuery();
                return mCustomer.CUSTOMER_ID;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return Constants.LongNullValue;
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
        #region Address Details
        public DataTable GetAddressDetail(long p_Customer_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectCUSTOMER mCustData = new spSelectCUSTOMER();
                mCustData.Connection = mConnection;

                mCustData.CUSTOMER_ID = p_Customer_Id;
                DataTable dt = mCustData.ExecuteTableForAddressDetails();
                //DataView dv = new DataView(dt);
                //dv.Sort = "CUSTOMER_NAME";
                //dt = dv.ToTable();
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
        public long InsertAddressDetails(long p_Customer_Id, string p_Address)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertCUSTOMER3 mCustomer = new spInsertCUSTOMER3();
                mCustomer.Connection = mConnection;

                mCustomer.CUST_ID = p_Customer_Id;
                mCustomer.CHILD_NAME = p_Address;
                mCustomer.ExecuteQueryForCustomerAddress();
                return mCustomer.CUSTOMER_ID;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return Constants.LongNullValue;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public DataTable DeleteCUSTOMER_Address(long p_Customer_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectCUSTOMER mCustData = new spSelectCUSTOMER();
                mCustData.Connection = mConnection;

                mCustData.CUSTOMER_ID = p_Customer_Id;
                DataTable dt = mCustData.ExecuteTableForAddressDelete();
                //DataView dv = new DataView(dt);
                //dv.Sort = "CUSTOMER_NAME";
                //dt = dv.ToTable();
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
    }
}