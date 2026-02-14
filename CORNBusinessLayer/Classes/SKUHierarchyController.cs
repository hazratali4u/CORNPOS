using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System.Collections.Generic;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For SKU Heirarchy Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert SKU Heirarchy
    /// </item>
    /// <term>
    /// Update SKU Heirarchy
    /// </term>
    /// <item>
    /// Get SKU Heirarchy
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class SkuHierarchyController
	{
        #region Contructors

        /// <summary>
        /// Constructor for SkuHierarchyController
        /// </summary>
        public SkuHierarchyController()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#endregion
        
		#region Public Method

        #region Select

        /// <summary>
        /// Gets SKU Hierarchy
        /// </summary>
        /// Returns SKU Hierarchy Data as Datatable
        /// <param name="p_Sku_Type_Id">Type</param>
        /// <param name="p_Sku_Hie_Id">Hierarchy</param>
        /// <param name="p_Parent_Sku_Hie_Id">Parent</param>
        /// <param name="p_Sku_Hie_Code">Code</param>
        /// <param name="p_Sku_Hie_Name">Name</param>
        /// <param name="p_Is_Active">IsActive</param>
        /// <param name="Companyid">Company</param>
        /// <returns>SKU Hierarchy Data as Datatable</returns>
        public DataTable SelectSkuHierarchy(int p_Sku_Type_Id, int p_Sku_Hie_Id,int p_Parent_Sku_Hie_Id, string p_Sku_Hie_Code, string p_Sku_Hie_Name,bool p_Is_Active, int Companyid, int p_Parent_sku_hie_TypeId)
		{
			IDbConnection mConnection = null;
			try
			{
				mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
				mConnection.Open();
				spSelectSKU_HIERARCHY mSkuHierarchy = new spSelectSKU_HIERARCHY();
				mSkuHierarchy.Connection = mConnection;
                
				mSkuHierarchy.SKU_HIE_TYPE_ID = p_Sku_Type_Id;
				mSkuHierarchy.SKU_HIE_ID = p_Sku_Hie_Id;
				mSkuHierarchy.PARENT_SKU_HIE_ID = p_Parent_Sku_Hie_Id;
				mSkuHierarchy.SKU_HIE_CODE = p_Sku_Hie_Code;
				mSkuHierarchy.SKU_HIE_NAME = p_Sku_Hie_Name;
				mSkuHierarchy.TIME_STAMP = Constants.DateNullValue ;
				mSkuHierarchy.LASTUPDATE_DATE = Constants .DateNullValue ;
				mSkuHierarchy.IP_ADDRESS = null;
				mSkuHierarchy.IS_ACTIVE = p_Is_Active;
                mSkuHierarchy.COMPANY_ID = Companyid;
                mSkuHierarchy.PARENT_SKU_HIE_TYPE_ID = p_Parent_sku_hie_TypeId;

                DataTable dt = mSkuHierarchy.ExecuteTable();
				return dt;
				
			}
			catch(Exception exp)
			{
				ExceptionPublisher.PublishException(exp);				
				return null;
			}
			finally
			{
				if(mConnection != null && mConnection.State == ConnectionState.Open)
				{
					mConnection.Close();
				}
			}
		}        
        
        public DataTable GetParentCategory(int p_DistributorID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetParentCategry mSkuHierarchy = new uspGetParentCategry();
                mSkuHierarchy.Connection = mConnection;

                mSkuHierarchy.DISTRIBUTOR_ID = p_DistributorID;

                DataTable dt = mSkuHierarchy.ExecuteTable();
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
        /// Gets SKU Hierarchy
        /// </summary>
        /// <remarks>
        /// Returns SKU Hierarchy Data as Datatable
        /// </remarks>
        /// <param name="p_type">Type</param>
        /// <param name="p_UserId">User</param>
        /// <returns>SKU Hierarchy Data as Datatable</returns>
        public DataTable SelectBrandAssignment(int p_type, int p_UserId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspGetBrandAssignment mSkuHierarchy = new UspGetBrandAssignment();
                mSkuHierarchy.Connection = mConnection;

                mSkuHierarchy.TYPE_ID = p_type;
                mSkuHierarchy.USER_ID = p_UserId;
                DataTable dt = new DataTable();
                dt = mSkuHierarchy.ExecuteTable();
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

        public DataTable SelectDealAssignment(int p_ItemDealId, int p_CategoryId, int p_distributorId, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spITEM_DEAL_ASSIGNMENT mInsertBrand = new spITEM_DEAL_ASSIGNMENT();
                mInsertBrand.Connection = mConnection;

                mInsertBrand.ITEM_DEAL_ID = p_ItemDealId;
                mInsertBrand.CATEGORY_ID = p_CategoryId;
                mInsertBrand.DISTRIBUTOR_ID = p_distributorId;
                mInsertBrand.TYPE_ID = p_TypeId;

                DataTable dt = mInsertBrand.ExecuteTable();
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

        public string SelectDealAssignment(int p_ItemDealId, int p_distributorId, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spITEM_DEAL_ASSIGNMENT mInsertBrand = new spITEM_DEAL_ASSIGNMENT();
                mInsertBrand.Connection = mConnection;

                mInsertBrand.ITEM_DEAL_ID = p_ItemDealId;
                mInsertBrand.CATEGORY_ID = 0;
                mInsertBrand.DISTRIBUTOR_ID = p_distributorId;
                mInsertBrand.TYPE_ID = p_TypeId;

                string dt = mInsertBrand.ExecuteScalar();
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

        public DataTable SelectECommerceCategories(int p_Category_ID, int p_typeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectECommerce_Category mSkuHierarchy = new spSelectECommerce_Category();
                mSkuHierarchy.Connection = mConnection;

                mSkuHierarchy.CategoryID = p_Category_ID;
                mSkuHierarchy.TYPE_ID = p_typeID;

                DataTable dt = mSkuHierarchy.ExecuteTable();
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
        /// Gets Principals
        /// </summary>
        /// <remarks>
        /// Returns Principal Data as Datatable
        /// </remarks>
        /// <param name="p_Sku_Type_Hie_Id">Type</param>
        /// <param name="Companyid">Company</param>
        /// <returns>Principal Data as Datatable</returns>
        public DataTable SelectPrincipal(int p_Sku_Type_Hie_Id, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectPRINCIPAL mPRINCIPAL = new spSelectPRINCIPAL();
                mPRINCIPAL.Connection = mConnection;
                mPRINCIPAL.SKU_HIE_TYPE_ID = p_Sku_Type_Hie_Id;
                mPRINCIPAL.COMPANY_ID = Companyid;
                DataTable dt = mPRINCIPAL.ExecuteTable();
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
        
        public string InsertHierarchy(int p_Sku_Type_Id, int p_Parent_Sku_Hie_Id, string p_Sku_Hie_Code,
            string p_Sku_Hie_Name, string p_Ip_Address, bool p_Is_Active, int Companyid,
            int p_Parent_sku_hie_TypeId,string p_ImagePath, int p_sortOrder,
            bool p_IsOpenItemCategory, bool p_multiSelectModifier)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertSKU_HIERARCHY mSkuHierarchy = new spInsertSKU_HIERARCHY();
                mSkuHierarchy.Connection = mConnection;
                mSkuHierarchy.SKU_HIE_TYPE_ID = p_Sku_Type_Id;
                mSkuHierarchy.PARENT_SKU_HIE_ID = p_Parent_Sku_Hie_Id;
                mSkuHierarchy.SKU_HIE_CODE = p_Sku_Hie_Code;
                mSkuHierarchy.SKU_HIE_NAME = p_Sku_Hie_Name;
                mSkuHierarchy.TIME_STAMP = System.DateTime.Now; ;
                mSkuHierarchy.LASTUPDATE_DATE = System.DateTime.Now; ;
                mSkuHierarchy.IP_ADDRESS = p_Ip_Address;
                mSkuHierarchy.IS_ACTIVE = p_Is_Active;
                mSkuHierarchy.COMPANY_ID = Companyid;
                mSkuHierarchy.PARENT_SKU_HIE_TYPE_ID = p_Parent_sku_hie_TypeId;
                mSkuHierarchy.IS_MANUALDISCOUNT = false;
                mSkuHierarchy.ImagePath = p_ImagePath;
                mSkuHierarchy.SORT_ORDER = p_sortOrder;
                mSkuHierarchy.IsOpenItemCategory = p_IsOpenItemCategory;
                mSkuHierarchy.MultiSelectModifier = p_multiSelectModifier;
                mSkuHierarchy.ExecuteQuery();
                return "Record Inserted";

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
        public string UpdateHierarchy(int p_Sku_Type_Id, int p_Sku_Hie_Id, int p_Parent_Sku_Hie_Id,
            string p_Sku_Hie_Code, string p_Sku_Hie_Name, string p_Ip_Address,bool p_Is_Active, int Companyid,
            int p_Parent_sku_hie_TypeId,string p_ImagePath, int p_sortOrder,bool p_IsOpenItemCategory,
            bool p_MultiSelectModifier)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateSKU_HIERARCHY mSkuHierarchy = new spUpdateSKU_HIERARCHY();
                mSkuHierarchy.Connection = mConnection;
                mSkuHierarchy.SKU_HIE_TYPE_ID = p_Sku_Type_Id;
                mSkuHierarchy.SKU_HIE_ID = p_Sku_Hie_Id;
                mSkuHierarchy.PARENT_SKU_HIE_ID = p_Parent_Sku_Hie_Id;
                mSkuHierarchy.SKU_HIE_CODE = p_Sku_Hie_Code;
                mSkuHierarchy.SKU_HIE_NAME = p_Sku_Hie_Name;
                mSkuHierarchy.TIME_STAMP = System.DateTime.Now; ;
                mSkuHierarchy.LASTUPDATE_DATE = System.DateTime.Now; ;
                mSkuHierarchy.IP_ADDRESS = p_Ip_Address;
                mSkuHierarchy.IS_ACTIVE = p_Is_Active;
                mSkuHierarchy.COMPANY_ID = Companyid;
                mSkuHierarchy.PARENT_SKU_HIE_TYPE_ID = p_Parent_sku_hie_TypeId;
                mSkuHierarchy.IS_MANUALDISCOUNT = false;
                mSkuHierarchy.ImagePath = p_ImagePath;
                mSkuHierarchy.SORT_ORDER = p_sortOrder;
                mSkuHierarchy.IsOpenItemCategory = p_IsOpenItemCategory;
                mSkuHierarchy.MultiSelectModifier = p_MultiSelectModifier;
                mSkuHierarchy.ExecuteQuery();
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

        /// <summary>
        /// Deletes PromotionType For a User
        /// </summary>
        /// <param name="p_companyId">Company</param>
        /// <param name="p_UserId">User</param>
        public void DeleteAssignBrand(int p_companyId,int p_UserId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spDeleteBRAND_ASSIGNMENT mDeleteBrand = new spDeleteBRAND_ASSIGNMENT();
                mDeleteBrand.Connection = mConnection;
                mDeleteBrand.PRINCIPAL_ID = p_companyId;
                mDeleteBrand.USER_ID = p_UserId;
                mDeleteBrand.ExecuteQuery();
                
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                
            }
            finally
            {
            }
        }
        
        /// <summary>
        /// Inserts PromotionType Manual Or Auto For A User
        /// </summary>
        /// <param name="p_companyId">Company</param>
        /// <param name="p_UserId">User</param>
        public void InsertAssignBrand(int p_companyId,int p_UserId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spInsertBRAND_ASSIGNMENT mInsertBrand = new spInsertBRAND_ASSIGNMENT();
                mInsertBrand.Connection = mConnection;
                mInsertBrand.PRINCIPAL_ID = p_companyId;
                mInsertBrand.USER_ID = p_UserId;
                mInsertBrand.ExecuteQuery();

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);

            }
            finally
            {
            }
        }

        /// <summary>
        /// Inserts Items in Item_deal_Assignment table TYPE_ID=1
        /// SELECT Unassign Items in Item_deal_Assignment table TYPE_ID=2
        /// </summary>
        /// <param name="p_skuId"></param>
        /// <param name="p_CategoryId"></param>
        /// <param name="p_distributorId"></param>
        public void InsertAssignItem(int p_ItemDealId, int p_distributorId,DateTime p_Document_date, int p_UserId, string pRemarks
            , DataTable Skus)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction=null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                List<int> Category = new List<int>();
                int mid = 0;

                spITEM_DEAL_ASSIGNMENT spITEM_DEAL_ASSIGNMENT = new spITEM_DEAL_ASSIGNMENT();
                spITEM_DEAL_ASSIGNMENT.Connection = mConnection;
                spITEM_DEAL_ASSIGNMENT.Transaction = mTransaction;

                spITEM_DEAL_ASSIGNMENT_DETAIL mInsertDetail = new spITEM_DEAL_ASSIGNMENT_DETAIL();
                mInsertDetail.Connection = mConnection;
                mInsertDetail.Transaction = mTransaction;

                foreach (DataRow dr in Skus.Rows)
                {
                    if (!Category.Contains(int.Parse(dr["CATEGORY_ID"].ToString())))
                    {
                        if (dr["Status"].ToString() == "New")
                        {
                            Category.Add(int.Parse(dr["CATEGORY_ID"].ToString()));
                        }
                        spITEM_DEAL_ASSIGNMENT.CATEGORY_DEAL_ID = int.Parse(dr["CATEGORY_DEAL_ID"].ToString());
                        spITEM_DEAL_ASSIGNMENT.ITEM_DEAL_ID = int.Parse(dr["ITEM_DEAL_ID"].ToString());
                        spITEM_DEAL_ASSIGNMENT.CATEGORY_ID = int.Parse(dr["CATEGORY_ID"].ToString());
                        spITEM_DEAL_ASSIGNMENT.DISTRIBUTOR_ID = p_distributorId;
                        spITEM_DEAL_ASSIGNMENT.IS_ChoiceBased = bool.Parse(dr["IS_CatChoiceBased"].ToString());
                        spITEM_DEAL_ASSIGNMENT.QUANTITY = int.Parse(dr["QUANTITY"].ToString());
                        spITEM_DEAL_ASSIGNMENT.TYPE_ID = 1;
                        spITEM_DEAL_ASSIGNMENT.TIME_STAMP = p_Document_date;
                        spITEM_DEAL_ASSIGNMENT.USER_ID = p_UserId;
                        spITEM_DEAL_ASSIGNMENT.Status = dr["Status"].ToString();
                        spITEM_DEAL_ASSIGNMENT.MAX_CATEGORY = Convert.ToInt32(dr["MAX_CATEGORY"]);
                        spITEM_DEAL_ASSIGNMENT.IS_CATEGORY_RESTRICTED = Convert.ToBoolean(dr["IS_CATEGORY_RESTRICTED"]);
                        mid = spITEM_DEAL_ASSIGNMENT.ExecuteQuery();
                    }                    

                    mInsertDetail.ITEM_DEAL_ID = int.Parse(dr["ITEM_DEAL_ID"].ToString());
                    mInsertDetail.intDealID = mid;
                    mInsertDetail.IS_ChoiceBased = bool.Parse(dr["IS_ChoiceBased"].ToString());
                    mInsertDetail.IS_Optional = bool.Parse(dr["IS_Optional"].ToString());
                    mInsertDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                    mInsertDetail.QUANTITY = int.Parse(dr["SKU_QUANTITY"].ToString());
                    mInsertDetail.TIME_STAMP = p_Document_date;
                    mInsertDetail.USER_ID = p_UserId;
                    mInsertDetail.TYPE_ID = 1;
                    mInsertDetail.IS_Active = bool.Parse(dr["IS_Active"].ToString());
                    mInsertDetail.Status = dr["Status"].ToString();


                    mInsertDetail.ExecuteQuery();
                }
                mTransaction.Commit();
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
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

        public void UpdateAssignItem(int p_ItemDealId, int p_distributorId, DateTime p_Document_date, int p_UserId, string pRemarks)
        {
            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spDELETE_DEAL_ASSIGNMENT spDEAL_ASSIGNMENT = new spDELETE_DEAL_ASSIGNMENT();
                spDEAL_ASSIGNMENT.Connection = mConnection;


                spDEAL_ASSIGNMENT.ITEM_DEAL_ID = p_ItemDealId;
                spDEAL_ASSIGNMENT.DISTRIBUTOR_ID = p_distributorId;
                spDEAL_ASSIGNMENT.TYPE_ID = 1;

                spDEAL_ASSIGNMENT.ExecuteQuery();

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
        
        #region Added By Hazrat Ali

        /// <summary>
        /// Inserts Principal
        /// </summary>
        /// <remarks>
        /// Returns "Record Inserted" On Success And Exception.Message On Failure
        /// </remarks>
        /// <param name="p_Sku_Type_Id">Type</param>
        /// <param name="p_Parent_Sku_Hie_Id">Hierarchy</param>
        /// <param name="p_Sku_Hie_Code">Code</param>
        /// <param name="p_Sku_Hie_Name">Name</param>
        /// <param name="p_Ip_Address">IP</param>
        /// <param name="p_Is_Active">IsActive</param>
        /// <param name="Companyid">Compnay</param>
        /// <param name="p_IsManualDiscount">PromotionType</param>
        /// <param name="p_Address">Address</param>
        /// <param name="p_NTN">NTN</param>
        /// <param name="p_STRN">STRN</param>
        /// <returns>"Record Inserted" On Success And Exception.Message On Failure</returns>
        public string InsertPrincipal(int p_Sku_Type_Id, int p_Parent_Sku_Hie_Id, string p_Sku_Hie_Code, string p_Sku_Hie_Name, string p_Ip_Address, bool p_Is_Active
            , int Companyid, bool p_IsManualDiscount, string p_Address,
            string P_ContactPerson, string p_EMAIL, string p_PHONE, string p_FAX,
            string p_NTN, string p_CreditDays, string p_CreditLimit)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertPRINCIPAL mPrincipal = new spInsertPRINCIPAL();
                mPrincipal.Connection = mConnection;

                mPrincipal.SKU_HIE_TYPE_ID = p_Sku_Type_Id;

                mPrincipal.PARENT_SKU_HIE_ID = p_Parent_Sku_Hie_Id;
                mPrincipal.SKU_HIE_CODE = p_Sku_Hie_Code;
                mPrincipal.SKU_HIE_NAME = p_Sku_Hie_Name;
                mPrincipal.TIME_STAMP = System.DateTime.Now; ;
                mPrincipal.LASTUPDATE_DATE = System.DateTime.Now; ;
                mPrincipal.IP_ADDRESS = p_Ip_Address;
                mPrincipal.IS_ACTIVE = p_Is_Active;
                mPrincipal.COMPANY_ID = Companyid;
                mPrincipal.IS_MANUALDISCOUNT = p_IsManualDiscount;
                mPrincipal.ADDRESS = p_Address;
                mPrincipal.CONTACT_PERSON = P_ContactPerson;
                mPrincipal.EMAIL = p_EMAIL;
                mPrincipal.FAX = p_FAX;
                mPrincipal.PHONE = p_PHONE;
                mPrincipal.NTN = p_NTN;
                mPrincipal.CREDIT_DAYS = p_CreditDays;
                mPrincipal.CREDIT_LIMIT = p_CreditLimit;
               // mPrincipal.CONTACT_PERSON = P_ContactPerson;
                mPrincipal.ExecuteQuery();
                return "Record Inserted";

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
        /// Updates Principal
        /// </summary>
        /// <remarks>
        /// Returns "Record Updated" On Success And Exception.Message On Failure
        /// </remarks>
        /// <param name="p_Sku_Type_Id">Type</param>
        /// <param name="p_Sku_Hie_Id">Hierarchy</param>
        /// <param name="p_Parent_Sku_Hie_Id">Parent</param>
        /// <param name="p_Sku_Hie_Code">Code</param>
        /// <param name="p_Sku_Hie_Name">Name</param>
        /// <param name="p_Ip_Address">IP</param>
        /// <param name="p_Is_Active">IsActive</param>
        /// <param name="Companyid">Company</param>
        /// <param name="p_IsManualDiscount">PromotionType</param>
        /// <param name="p_Address">Address</param>
        /// <param name="p_NTN">NTN</param>
        /// <param name="p_STRN">STRN</param>
        /// <returns>"Record Updated" On Success And Exception.Message On Failure</returns>
        public string UpdatePrincipal(int p_Sku_Type_Id, int p_Sku_Hie_Id, int p_Parent_Sku_Hie_Id, string p_Sku_Hie_Code, string p_Sku_Hie_Name, string p_Ip_Address
            , bool p_Is_Active, int Companyid, bool p_IsManualDiscount,
            string p_Address, string P_ContactPerson, string p_EMAIL,
            string p_PHONE, string p_FAX, string p_NTN, string p_CreditDays, string p_CreditLimit)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdatePRINCIPAL mPRINCIPAL = new spUpdatePRINCIPAL();
                mPRINCIPAL.Connection = mConnection;

                mPRINCIPAL.SKU_HIE_TYPE_ID = p_Sku_Type_Id;
                mPRINCIPAL.SKU_HIE_ID = p_Sku_Hie_Id;
                mPRINCIPAL.PARENT_SKU_HIE_ID = p_Parent_Sku_Hie_Id;
                mPRINCIPAL.SKU_HIE_CODE = p_Sku_Hie_Code;
                mPRINCIPAL.SKU_HIE_NAME = p_Sku_Hie_Name;
                mPRINCIPAL.TIME_STAMP = System.DateTime.Now; ;
                mPRINCIPAL.LASTUPDATE_DATE = System.DateTime.Now; ;
                mPRINCIPAL.IP_ADDRESS = p_Ip_Address;
                mPRINCIPAL.IS_ACTIVE = p_Is_Active;
                mPRINCIPAL.COMPANY_ID = Companyid;
                mPRINCIPAL.IS_MANUALDISCOUNT = p_IsManualDiscount;
                mPRINCIPAL.ADDRESS = p_Address;
                mPRINCIPAL.CONTACT_PERSON = P_ContactPerson;
                mPRINCIPAL.EMAIL = p_EMAIL;
                mPRINCIPAL.FAX = p_FAX;
                mPRINCIPAL.PHONE = p_PHONE;
                mPRINCIPAL.NTN = p_NTN;
                mPRINCIPAL.CREDIT_DAYS = p_CreditDays;
                mPRINCIPAL.CREDIT_LIMIT = p_CreditLimit;

                mPRINCIPAL.ExecuteQuery();
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

        /// <summary>
        /// Updates PromotionType For User
        /// </summary>
        /// <remarks>
        /// Returns "Record Updated" On Success And Exception.Message On Failure
        /// </remarks>
        /// <param name="p_USER_ID">User</param>
        /// <param name="p_PRINCIPAL_ID">Principal</param>
        /// <param name="p_Is_ManualDiscount">PromotionType</param>
        /// <returns>"Record Updated" On Success And Exception.Message On Failure</returns>
        public string UpdateBrandAssignment(int p_USER_ID, int p_PRINCIPAL_ID, bool p_Is_ManualDiscount)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateBRAND_ASSIGNMENT mBrand = new spUpdateBRAND_ASSIGNMENT();
                mBrand.Connection = mConnection;
                mBrand.USER_ID = p_USER_ID;
                mBrand.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mBrand.Is_ManualDiscount = p_Is_ManualDiscount;
                mBrand.ExecuteQuery();
                return "Record Updated";

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

        #region Location Wise Assigned Items for Raw & Packaging category
        public DataTable SelectAssignedUnAssignedItems(int p_distributor_ID, bool p_Assigned, int p_category_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectECommerce_Category mSkuHierarchy = new spSelectECommerce_Category();
                mSkuHierarchy.Connection = mConnection;

                mSkuHierarchy.DISTRIBUTOR_ID = p_distributor_ID;
                mSkuHierarchy.Is_ASSIGNED = p_Assigned;
                mSkuHierarchy.CategoryID = p_category_ID;

                DataTable dt = mSkuHierarchy.ExecuteTableForItemAssignmentAgainstLocation();
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