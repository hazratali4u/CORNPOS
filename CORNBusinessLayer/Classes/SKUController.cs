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
	public class SkuController
    {
        #region Constructors

        /// <summary>
        /// Constructor For SkuController
        /// </summary>
        public SkuController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region public Methods

        #region Select
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_UOM_ID"></param>
        /// <param name="p_UOM_TYPE_ID"></param>
        /// <returns></returns>
        public DataTable SelectUOM(int p_UOM_ID, int p_UOM_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectUOM mspSelectSkuInfo = new spSelectUOM();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.UOM_ID = p_UOM_ID;
                mspSelectSkuInfo.UOM_TYPE_ID = p_UOM_TYPE_ID;

                DataTable dt = mspSelectSkuInfo.ExecuteTable();
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
        /// Gets SKUS Data
        /// </summary>
        /// <remarks>
        /// Returns SKUS Data as Datatable
        /// </remarks>
        /// <param name="p_company_id">Principal</param>
        /// <param name="p_division_id">Dicision</param>
        /// <param name="p_category_id">Category</param>
        /// <param name="p_brand_id">Brand</param>
        /// <param name="Companyid">Company</param>
        /// <returns>SKUS Data as Datatable</returns>
        public DataTable SelectSkuInfo(int p_company_id, int p_division_id, int p_category_id, int p_brand_id, int Companyid,string SectionID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectSkuInfo mspSelectSkuInfo = new uspSelectSkuInfo();
                mspSelectSkuInfo.Connection = mConnection;

                mspSelectSkuInfo.brand_id = p_brand_id;
                mspSelectSkuInfo.category_id = p_category_id;
                mspSelectSkuInfo.Principal_id = p_company_id;
                mspSelectSkuInfo.division_id = p_division_id;
                mspSelectSkuInfo.Company_id = Companyid;
                mspSelectSkuInfo.SECTION_ID = SectionID;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        public DataTable SelectSkuInfo(int p_company_id, int p_division_id, int p_category_id, int p_brand_id, int Companyid, int p_Type_id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectSkuInfo mspSelectSkuInfo = new uspSelectSkuInfo();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.brand_id = p_brand_id;
                mspSelectSkuInfo.category_id = p_category_id;
                mspSelectSkuInfo.Principal_id = p_company_id;
                mspSelectSkuInfo.division_id = p_division_id;
                mspSelectSkuInfo.Company_id = Companyid;
                mspSelectSkuInfo.Type_id = p_Type_id;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();
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
        public DataTable GetSKUInfo(int p_Distributor_ID,DateTime p_StockDate, int p_Type_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetSKUInfo mspSelectSkuInfo = new uspGetSKUInfo();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.Distributor_ID = p_Distributor_ID;
                mspSelectSkuInfo.Type_ID = p_Type_ID;
                mspSelectSkuInfo.StockDate = p_StockDate;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();
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
        public DataTable GetSKUInfo(int p_Distributor_ID, DateTime p_StockDate,string p_CategoryIDs, int p_Type_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetSKUInfo mspSelectSkuInfo = new uspGetSKUInfo();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.Distributor_ID = p_Distributor_ID;
                mspSelectSkuInfo.Type_ID = p_Type_ID;
                mspSelectSkuInfo.StockDate = p_StockDate;
                mspSelectSkuInfo.Category_IDs = p_CategoryIDs;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();
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
        public DataTable SelectModifier(int p_Type_id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSKU_Modifier mSkuModifier = new spSelectSKU_Modifier();
                mSkuModifier.Connection = mConnection;

                mSkuModifier.Typeid =p_Type_id;

                DataTable dt = mSkuModifier.ExecuteTable();

                return dt;

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
        public DataSet SelectModifierRpt(int p_Type_id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSKU_ModifierReport mSkuModifier = new spSelectSKU_ModifierReport();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                mSkuModifier.Connection = mConnection;

                mSkuModifier.SKU_ID = p_Type_id;

                DataTable dt = mSkuModifier.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectSKU_ModifierReport"].ImportRow(dr);
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
        public DataTable SelectSkuConsumption(int p_Typeid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectHasModifier mspSelectSkuInfo = new spSelectHasModifier();
                mspSelectSkuInfo.Connection = mConnection;

                mspSelectSkuInfo.Type_id = p_Typeid;

                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        public DataSet SelectSKUConsumption(int p_DISTRIBUTOR_ID, int p_SKU_id, int pReportType, int p_Type_id, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                Reports.DsReport ds = new Reports.DsReport();
                spSelectSKU_Consumption mSkuModifier = new spSelectSKU_Consumption();
                mSkuModifier.Connection = mConnection;
                mSkuModifier.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mSkuModifier.SKU_ID = p_SKU_id;
                mSkuModifier.Type_ID = p_Type_id;
                mSkuModifier.REPORT_TYPE = pReportType;
                mSkuModifier.FROM_DATE = p_FromDate;
                mSkuModifier.TO_DATE = p_ToDate;
                DataTable dt = mSkuModifier.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectSKU_Consumption"].ImportRow(dr);
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
        public DataSet GetTopSellingItems(string p_DISTRIBUTOR_IDs, DateTime p_FromDate,DateTime p_ToDate, int p_Type_ID, int p_OrderBy)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                RptTopSellingItems objPrint = new RptTopSellingItems();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_IDs = p_DISTRIBUTOR_IDs;
                objPrint.FROM_DATE = p_FromDate;
                objPrint.TO_DATE = p_ToDate;
                objPrint.TYPE_ID = p_Type_ID;
                objPrint.OrderBy = p_OrderBy;
                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectSKU_Consumption"].ImportRow(dr);
                }

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
        public DataSet SelectINvoiceItemWiseSales(int p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_ToDate, int p_User_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                Reports.DsReport ds = new Reports.DsReport();
                spSelectSKU_Consumption mSkuModifier = new spSelectSKU_Consumption();

                mSkuModifier.Connection = mConnection;
                mSkuModifier.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mSkuModifier.FROM_DATE = p_FromDate;
                mSkuModifier.TO_DATE = p_ToDate;
                mSkuModifier.SKU_ID = p_User_ID;

                DataTable dt = mSkuModifier.ExecuteTableInvoiceItemWiseSales();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["rptInvoiceItemWiseSales"].ImportRow(dr);
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
        public DataSet SelectDiscountedInvoices(string p_distributer_id, int p_Cashier_id, DateTime p_FromDate, DateTime p_ToDate,int p_EmployeeDiscountTypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                SpSelectDiscountedSaleInvoices mDiscountSale = new SpSelectDiscountedSaleInvoices();
                mDiscountSale.Connection = mConnection;
                mDiscountSale.DISTRIBUTOR_ID = p_distributer_id;
                mDiscountSale.CASHIER_ID = p_Cashier_id;
                mDiscountSale.FROMDATE = p_FromDate;
                mDiscountSale.TODATE = p_ToDate;
                mDiscountSale.EmployeeDiscountTypeID = p_EmployeeDiscountTypeID;
                DataTable dt = mDiscountSale.ExecuteTable();
                CORNBusinessLayer.Reports.DSReportNew ds = new CORNBusinessLayer.Reports.DSReportNew();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["SpSelectDiscountedSaleInvoices"].ImportRow(dr);
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
        public DataTable SelectDiscountedInvoicesExcel(string p_distributer_id, int p_Cashier_id, DateTime p_FromDate, DateTime p_ToDate, int p_EmployeeDiscountTypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                SpSelectDiscountedSaleInvoices mDiscountSale = new SpSelectDiscountedSaleInvoices();
                mDiscountSale.Connection = mConnection;
                mDiscountSale.DISTRIBUTOR_ID = p_distributer_id;
                mDiscountSale.CASHIER_ID = p_Cashier_id;
                mDiscountSale.FROMDATE = p_FromDate;
                mDiscountSale.TODATE = p_ToDate;
                mDiscountSale.EmployeeDiscountTypeID = p_EmployeeDiscountTypeID;
                DataTable dt = mDiscountSale.ExecuteTable();               
                return dt;
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

        public DataSet SelectCustomerWiseSales(int p_Rpt_Type, int p_distributer_id, int p_Customer_id, int p_SortBy, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                SpSelectCustomerWiseSaleReport mSkuCustomer = new SpSelectCustomerWiseSaleReport();

                mSkuCustomer.Connection = mConnection;

                mSkuCustomer.Rpt_Type = p_Rpt_Type;
                mSkuCustomer.DISTRIBUTOR_ID = p_distributer_id;
                mSkuCustomer.CUSTOMER_ID = p_Customer_id;
                mSkuCustomer.SORT_BY = p_SortBy;
                mSkuCustomer.FROMDATE = p_FromDate;
                mSkuCustomer.TODATE = p_ToDate;

                DataTable dt = mSkuCustomer.ExecuteTable();

                CORNBusinessLayer.Reports.DSReportNew ds = new CORNBusinessLayer.Reports.DSReportNew();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["SpSelectCustomerWiseSaleReport"].ImportRow(dr);
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

        public DataSet SelectServiceWiseSales(int p_Rpt_Type, string p_distributer_id, int p_Service_Type_id, int p_SortBy, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                SpSelectServiceWiseSaleReport mSkuCustomer = new SpSelectServiceWiseSaleReport();
                Reports.DsReport2 ds = new Reports.DsReport2();

                mSkuCustomer.Connection = mConnection;

                mSkuCustomer.Rpt_Type = p_Rpt_Type;
                mSkuCustomer.DISTRIBUTOR_ID = p_distributer_id;
                mSkuCustomer.SERVICE_ID = p_Service_Type_id;
                mSkuCustomer.SORT_BY = p_SortBy;
                mSkuCustomer.FROMDATE = p_FromDate;
                mSkuCustomer.TODATE = p_ToDate;

                if (p_Rpt_Type == 2)
                {
                    DataTable dt = mSkuCustomer.ExecuteTableForSKUWise();

                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["RptDISTSKUWiseQtySale"].ImportRow(dr);
                    }
                }
                else
                {
                    DataTable dt = mSkuCustomer.ExecuteTable();

                    foreach (DataRow dr in dt.Rows)
                    {
                        ds.Tables["SpSelectServiceWiseSaleReport"].ImportRow(dr);
                    }
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

        public DataSet SelectServiceWiseSales(int p_Rpt_Type, string p_distributer_id, int p_Service_Type_id, int p_SortBy, DateTime p_FromDate, DateTime p_ToDate,string p_INVOICE_NO)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                SpSelectServiceWiseSaleReport mSkuCustomer = new SpSelectServiceWiseSaleReport();

                mSkuCustomer.Connection = mConnection;

                mSkuCustomer.Rpt_Type = p_Rpt_Type;
                mSkuCustomer.DISTRIBUTOR_ID = p_distributer_id;
                mSkuCustomer.SERVICE_ID = p_Service_Type_id;
                mSkuCustomer.SORT_BY = p_SortBy;
                mSkuCustomer.FROMDATE = p_FromDate;
                mSkuCustomer.TODATE = p_ToDate;
                mSkuCustomer.INVOICE_NO = p_INVOICE_NO;
                DataTable dt = mSkuCustomer.ExecuteTable();

                Reports.DsReport2 ds = new Reports.DsReport2();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["SpSelectServiceWiseSaleReport"].ImportRow(dr);
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
        public DataTable SelectServiceWiseSalesExcel(int p_Rpt_Type, string p_distributer_id, int p_Service_Type_id, int p_SortBy, DateTime p_FromDate, DateTime p_ToDate, string p_INVOICE_NO)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                SpSelectServiceWiseSaleReport mSkuCustomer = new SpSelectServiceWiseSaleReport();

                mSkuCustomer.Connection = mConnection;

                mSkuCustomer.Rpt_Type = p_Rpt_Type;
                mSkuCustomer.DISTRIBUTOR_ID = p_distributer_id;
                mSkuCustomer.SERVICE_ID = p_Service_Type_id;
                mSkuCustomer.SORT_BY = p_SortBy;
                mSkuCustomer.FROMDATE = p_FromDate;
                mSkuCustomer.TODATE = p_ToDate;
                mSkuCustomer.INVOICE_NO = p_INVOICE_NO;
                DataTable dt = mSkuCustomer.ExecuteTable();                
                return dt;

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
        public DataSet SelectMonthalyAttendance(int p_distributer_id, int p_dept_id, int p_Emp_id, DateTime p_FromDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                MONTHALY_ATTENDANCE mattendance = new MONTHALY_ATTENDANCE();

                mattendance.Connection = mConnection;

                mattendance.DISTRIBUTOR_ID = p_distributer_id;
                mattendance.DEPARTMENT_ID = p_dept_id;
                mattendance.Emp_ID = p_Emp_id;
                mattendance.DATE = p_FromDate;

                DataTable dt = mattendance.ExecuteTable();

                Reports.DsReport2 ds = new Reports.DsReport2();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["MONTHALY_ATTENDANCE"].ImportRow(dr);
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

        public string InsertModifier(int p_Sku_Id, DataTable p_dt_Modifier)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertSKU_Modifier mSkuModifier = new spInsertSKU_Modifier();
                mSkuModifier.Connection = mConnection;

                DataTable dt = p_dt_Modifier;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    mSkuModifier.SKU_ID = p_Sku_Id;
                    mSkuModifier.ModifierSKU_ID =Convert.ToInt32( dt.Rows[i]["IsModifierID"]);
                    mSkuModifier.intStockMUnitCode = Convert.ToInt32(dt.Rows[i]["StockUnitID"]); 
                    mSkuModifier.Default_Qty = Convert.ToDecimal(dt.Rows[i]["Default_Qty"]); 
                    mSkuModifier.IS_Manadatory = Convert.ToBoolean( dt.Rows[i]["IS_Manadatory"]);
                    mSkuModifier.ExecuteQuery();
                }
      
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

        public string InsertModifierBulk(int p_Sku_Id, DataTable p_dt_Modifier)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertSKU_ModifierBulk mSkuModifier = new spInsertSKU_ModifierBulk();
                mSkuModifier.Connection = mConnection;

                DataTable dt = p_dt_Modifier;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    mSkuModifier.SKU_ID = p_Sku_Id;
                    mSkuModifier.ModifierSKU_ID = Convert.ToInt32(dt.Rows[i]["IsModifierID"]);
                    mSkuModifier.intStockMUnitCode = Convert.ToInt32(dt.Rows[i]["StockUnitID"]);
                    mSkuModifier.Default_Qty = Convert.ToDecimal(dt.Rows[i]["Default_Qty"]);
                    mSkuModifier.IS_Manadatory = Convert.ToBoolean(dt.Rows[i]["IS_Manadatory"]);
                    mSkuModifier.Status = dt.Rows[i]["Status"].ToString();
                    mSkuModifier.ExecuteQuery();
                }

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
        public string UpdateModifier(int p_Sku_ModifierCode,int p_Sku_Id,int p_ModifierSKU_ID,int p_intStockMUnitCode,decimal p_Default_Qty, bool p_IS_Manadatory)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateSKU_Modifier mSkuModifier = new spUpdateSKU_Modifier();
                mSkuModifier.Connection = mConnection;

                    mSkuModifier.lngSKUModifierCode = p_Sku_ModifierCode;
                    mSkuModifier.SKU_ID = p_Sku_Id;
                    mSkuModifier.ModifierSKU_ID = p_ModifierSKU_ID;
                    mSkuModifier.intStockMUnitCode = p_intStockMUnitCode;
                    mSkuModifier.Default_Qty = p_Default_Qty;
                    mSkuModifier.IS_Manadatory = p_IS_Manadatory;
                    mSkuModifier.ExecuteQuery();

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

        public bool DeleteModifier(long p_Sku_ModifierCode)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spDeleteSKU_Modifier mSkuModifier = new spDeleteSKU_Modifier();
                mSkuModifier.Connection = mConnection;

                mSkuModifier.lngSKUModifierCode = p_Sku_ModifierCode;
                mSkuModifier.ExecuteQuery();

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

        public DataTable SelectSkuHasModifier(int p_Typeid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectHasModifier mspSelectSkuInfo = new spSelectHasModifier();
                mspSelectSkuInfo.Connection = mConnection;

                mspSelectSkuInfo.Type_id = p_Typeid;

                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        
        public DataTable SelectSkusforOrder(int pCompanyId, int pCategoryId, int pBrandId, int pSkuId, int pDistributorId,DateTime pCLOSING_DATE, int pTypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSkusforOrder mspSelectSkuInfo = new spSelectSkusforOrder();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SKU_ID = pSkuId;
                mspSelectSkuInfo.Category_Id = pCategoryId;
                mspSelectSkuInfo.DISTRIBUTOR_ID = pDistributorId;
                mspSelectSkuInfo.BRAND_ID = pBrandId;
                mspSelectSkuInfo.company_id = pCompanyId;
                mspSelectSkuInfo.TYPE_ID = pTypeId;
                mspSelectSkuInfo.CLOSING_DATE = pCLOSING_DATE;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        public DataTable GetTodayMenuItems(int pDistributorId, DateTime pCLOSING_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetTodayMenuItems mspSelectSkuInfo = new uspGetTodayMenuItems();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.DISTRIBUTOR_ID = pDistributorId;
                mspSelectSkuInfo.CLOSING_DATE = pCLOSING_DATE;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        public DataTable SpGetPendingBill(long pSaleInvoiceId, int pTypeId, int pLOCKED_BY)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spGetPendingBill mspSelectSkuInfo = new spGetPendingBill();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SALE_INVOICE_ID = pSaleInvoiceId;
                mspSelectSkuInfo.LOCKED_BY = pLOCKED_BY;
                mspSelectSkuInfo.TYPE_ID = pTypeId;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        /// Gets SKU Data
        /// </summary>
        /// <remarks>
        /// Returns SKUS Data as Datatable
        /// </remarks>
        /// <param name="p_SKU_Id">SKU</param>
        /// <param name="Companyid">Company</param>
        /// <returns>SKUS Data as Datatable</returns>
        public DataTable SelectSkuData(int p_SKU_Id, int Companyid)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectSKUS mSkuInfo = new spSelectSKUS();
                mSkuInfo.Connection = mConnection;
                mSkuInfo.SKU_ID = p_SKU_Id;
                mSkuInfo.COMPANY_ID = Companyid;
                DataTable dt = mSkuInfo.ExecuteTable();
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
public string InsertSKUS(bool p_IsExempted, bool p_IsActive, char p_Gst_On, int p_Company_Id, int p_Division_Id, int p_Category_Id, int p_Brand_Id
            , int p_Variant_Id, decimal p_GST_Rate_Reg, decimal p_GST_Rate_Unreg, short p_Units_In_Case, string p_Sku_Code, string p_Sku_Name, string p_Ip_Address
            , string p_packSize, int p_UserId, int Companyid, string p_DESCRIPTION, int pSectionId, bool p_IsDeal, bool p_IsModifier
            , decimal minlevel, decimal reorderlevel, bool p_IsRecipe, bool p_IS_HasMODIFIER, int p_intSaleMUnitCode, int p_intPurchaseMUnitCode, int p_intStockMUnitCode
            , float p_fltFEDPercentage, float p_fltWHTPercentage, float p_fltAgeInDays, bool p_IsMarketItem
            , bool p_IsReplaceable, bool p_IsFEDItem, bool p_IsWHTItem, bool p_IsSerialized
            , bool p_IsHazardous, bool p_IsBatchItem, bool p_IsOverSaleAllowed, bool p_IsExpiryAllowed, bool p_IsWarehouseItem
            , decimal p_MAX_LEVEL, decimal p_Sale_to_PurchaseFactor, decimal p_Purchase_to_SaleFactor, decimal p_Sale_to_StockFactor
            , decimal p_Purchase_to_StockFactor, decimal p_Default_Qty, decimal p_Stock_to_SaleFactor, decimal p_Stock_to_PurchaseFactor
            , string p_Sale_to_PurchaseOperator, string p_Purchase_to_SaleOperator, string p_Sale_to_StockOperator
            , string p_Purchase_to_StockOperator, string p_Stock_to_SaleOperator
            , string p_Stock_to_PurchaseOperator, string p_strDescription, string p_strSerialCode, string p_strStatus, string p_strERPCode
            , float p_ShelfAgeInDays, int p_MUnitLifeCode, bool p_IsInventoryWeight,
            string p_BUTTON_COLOR, string p_SKU_IMAGE,bool p_IsSaleWeight,bool p_IsUnGroup,bool p_IsPackage
            , string p_SortOrder, decimal p_PreparationTime,int p_UOM,bool p_ValidateStockOnSplitItem,bool p_isStickerPrint,string p_strKOTDescription
            ,bool p_IsRunOut)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertSKUS mSkus = new spInsertSKUS();

                mSkus.Connection = mConnection;
                mSkus.PRINCIPAL_ID = p_Company_Id;
                mSkus.ISEXEMPTED = p_IsExempted;
                mSkus.ISACTIVE = p_IsActive;
                mSkus.GST_ON = p_Gst_On;
                mSkus.COMPANY_ID = Companyid;
                mSkus.DIVISION_ID = p_Division_Id;
                mSkus.BRAND_ID = p_Brand_Id;
                mSkus.CATEGORY_ID = p_Category_Id;
                if (!p_IsExempted)
                {
                    mSkus.GST_RATE_REG = p_GST_Rate_Reg;
                    mSkus.GST_RATE_UNREG = p_GST_Rate_Unreg;
                }
                else
                {
                    mSkus.GST_RATE_REG = 0;
                    mSkus.GST_RATE_UNREG = 0;
                }
                mSkus.UNITS_IN_CASE = p_Units_In_Case;
                mSkus.SKU_NAME = p_Sku_Name;
                mSkus.SKU_CODE = p_Sku_Code;
                mSkus.TIME_STAMP = System.DateTime.Now;
                mSkus.LASTUPDATE_DATE = System.DateTime.Now;
                mSkus.IP_ADDRESS = p_Ip_Address;
                mSkus.PACKSIZE = p_packSize;
                mSkus.USER_ID = p_UserId;
                mSkus.DESCRIPTION = p_DESCRIPTION;
                mSkus.SECTION_ID = pSectionId;
                mSkus.IsInventoryWeight = p_IsInventoryWeight;
                mSkus.IS_DEAL = p_IsDeal;
                mSkus.IS_MODIFIER = p_IsModifier;
                mSkus.IS_Recipe = p_IsRecipe;
                mSkus.MIN_LEVEL = minlevel;
                mSkus.REORDER_LEVEL = reorderlevel;
                mSkus.IS_HasMODIFIER = p_IS_HasMODIFIER;
                mSkus.intSaleMUnitCode = p_intSaleMUnitCode;
                mSkus.intPurchaseMUnitCode = p_intPurchaseMUnitCode;
                mSkus.intStockMUnitCode = p_intStockMUnitCode;
                mSkus.fltFEDPercentage = p_fltFEDPercentage;
                mSkus.fltWHTPercentage = p_fltWHTPercentage;
                mSkus.fltAgeInDays = p_fltAgeInDays;
                mSkus.IsMarketItem = p_IsMarketItem;
                mSkus.IsReplaceable = p_IsReplaceable;
                mSkus.IsFEDItem = p_IsFEDItem;
                mSkus.IsWHTItem = p_IsWHTItem;
                mSkus.IsSerialized = p_IsSerialized;
                mSkus.IsHazardous = p_IsHazardous;
                mSkus.IsBatchItem = p_IsBatchItem;
                mSkus.IsOverSaleAllowed = p_IsOverSaleAllowed;
                mSkus.IsExpiryAllowed = p_IsExpiryAllowed;
                mSkus.IsWarehouseItem = p_IsWarehouseItem;
                mSkus.MAX_LEVEL = p_MAX_LEVEL;
                mSkus.Sale_to_PurchaseFactor = p_Sale_to_PurchaseFactor;
                mSkus.Purchase_to_SaleFactor = p_Purchase_to_SaleFactor;
                mSkus.Sale_to_StockFactor = p_Sale_to_StockFactor;
                mSkus.Purchase_to_StockFactor = p_Purchase_to_StockFactor;
                mSkus.Default_Qty = p_Default_Qty;
                mSkus.Stock_to_SaleFactor = p_Stock_to_SaleFactor;
                mSkus.Stock_to_PurchaseFactor = p_Stock_to_PurchaseFactor;
                mSkus.Sale_to_PurchaseOperator = p_Sale_to_PurchaseOperator;
                mSkus.Purchase_to_SaleOperator = p_Purchase_to_SaleOperator;
                mSkus.Sale_to_StockOperator = p_Sale_to_StockOperator;
                mSkus.Purchase_to_StockOperator = p_Purchase_to_StockOperator;
                mSkus.Stock_to_SaleOperator = p_Stock_to_SaleOperator;
                mSkus.Stock_to_PurchaseOperator = p_Stock_to_PurchaseOperator;
                mSkus.strDescription = p_strDescription;
                mSkus.strSerialCode = p_strSerialCode;
                mSkus.strStatus = p_strStatus;
                mSkus.strERPCode = p_strERPCode;
                mSkus.fltShelfAgeInDays = p_ShelfAgeInDays;
                mSkus.intMUnitLifeCode = p_MUnitLifeCode;
                mSkus.BUTTON_COLOR = p_BUTTON_COLOR;
                mSkus.SKU_IMAGE = p_SKU_IMAGE;
                mSkus.IsSaleWeight = p_IsSaleWeight;
                mSkus.IsUnGroup = p_IsUnGroup;
                mSkus.IsPackage = p_IsPackage;
                mSkus.SortOrder = !string.IsNullOrEmpty(p_SortOrder) ? Convert.ToInt32(p_SortOrder) : 1;
                mSkus.PreparationTime = p_PreparationTime;
                mSkus.UOM = p_UOM;
                mSkus.ValidateStockOnSplitItem = p_ValidateStockOnSplitItem;
                mSkus.Is_StickerPrint = p_isStickerPrint;
                mSkus.strKOTDescription = p_strKOTDescription;
                mSkus.IsRunOut = p_IsRunOut;
                mSkus.ExecuteQuery();

                return mSkus.SKU_ID.ToString();

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
        /// Updates SKU
        /// </summary>
        /// <remarks>
        /// Returns "Record Updated" On Success And Null On Failure
        /// </remarks>
        /// <param name="p_IsExempted">IsExempted</param>
        /// <param name="p_IsActive">IsActive</param>
        /// <param name="p_Gst_On">GSTOn</param>
        /// <param name="p_Company_Id">Principal</param>
        /// <param name="p_Division_Id">Division</param>
        /// <param name="p_Category_Id">Category</param>
        /// <param name="p_Brand_Id">Brand</param>
        /// <param name="p_Variant_Id">Variant</param>
        /// <param name="p_GST_Rate_Reg">GSTReg</param>
        /// <param name="p_GST_Rate_Unreg">GSTUnReg</param>
        /// <param name="p_Units_In_Case">Units</param>
        /// <param name="p_Sku_Id">SKU</param>
        /// <param name="p_Sku_Code">Code</param>
        /// <param name="p_Sku_Name">Name</param>
        /// <param name="p_Ip_Address">Address</param>
        /// <param name="p_packSize">Packing</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="CompanyId">Company</param>
        /// <returns>"Record Updated" On Success And Null On Failure</returns>
        public string UpdateSKUS(bool p_IsExempted, bool p_IsActive, char p_Gst_On, int p_Company_Id, int p_Division_Id, int p_Category_Id,
                int p_Brand_Id, int p_Variant_Id, decimal p_GST_Rate_Reg, decimal p_GST_Rate_Unreg, short p_Units_In_Case, int p_Sku_Id, string p_Sku_Code,
                string p_Sku_Name, string p_Ip_Address, string p_packSize, int p_UserId, int CompanyId, string p_DESCRIPTION, int pSectionId, bool p_IsDeal, bool p_IsModifier,
                decimal minlevel, decimal reorderlevel, bool p_IsRecipe, bool p_IS_HasMODIFIER, int p_intSaleMUnitCode, int p_intPurchaseMUnitCode, int p_intStockMUnitCode,
                float p_fltFEDPercentage, float p_fltWHTPercentage, float p_fltAgeInDays, bool p_IsMarketItem,
                bool p_IsReplaceable, bool p_IsFEDItem, bool p_IsWHTItem, bool p_IsSerialized,
                bool p_IsHazardous, bool p_IsBatchItem, bool p_IsOverSaleAllowed, bool p_IsExpiryAllowed, bool p_IsWarehouseItem,
                decimal p_MAX_LEVEL, decimal p_Sale_to_PurchaseFactor, decimal p_Purchase_to_SaleFactor, decimal p_Sale_to_StockFactor,
                decimal p_Purchase_to_StockFactor, decimal p_Default_Qty, decimal p_Stock_to_SaleFactor, decimal p_Stock_to_PurchaseFactor,
                string p_Sale_to_PurchaseOperator, string p_Purchase_to_SaleOperator, string p_Sale_to_StockOperator,
                string p_Purchase_to_StockOperator, string p_Stock_to_SaleOperator,
                string p_Stock_to_PurchaseOperator, string p_strDescription, string p_strSerialCode, string p_strStatus, string p_strERPCode,
                float p_ShelfAgeInDays, int p_MUnitLifeCode, bool p_IsInventoryWeight, string p_BUTTON_COLOR, string p_SKU_IMAGE
            ,bool p_IsSaleWeight,bool p_IsUnGroup,bool p_IsPackage, string p_SortOrder,decimal p_PreparationTime,
                int p_UOM,bool p_ValidateStockOnSplitItem, bool p_isStickerPrint,string p_strKOTDescription,bool p_IsRunOut)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateSKUS mSkus = new spUpdateSKUS();

                mSkus.Connection = mConnection;
                mSkus.ISEXEMPTED = p_IsExempted;
                mSkus.ISACTIVE = p_IsActive;
                mSkus.GST_ON = p_Gst_On;
                mSkus.COMPANY_ID = CompanyId;
                mSkus.PRINCIPAL_ID = p_Company_Id;
                mSkus.DIVISION_ID = p_Division_Id;
                mSkus.BRAND_ID = p_Brand_Id;
                mSkus.CATEGORY_ID = p_Category_Id;
                if (!p_IsExempted)
                {
                    mSkus.GST_RATE_REG = p_GST_Rate_Reg;
                    mSkus.GST_RATE_UNREG = p_GST_Rate_Unreg;
                }
                else
                {
                    mSkus.GST_RATE_REG = 0;
                    mSkus.GST_RATE_UNREG = 0;
                }
                mSkus.UNITS_IN_CASE = p_Units_In_Case;
                mSkus.SKU_ID = p_Sku_Id;
                mSkus.SKU_NAME = p_Sku_Name;
                mSkus.SKU_CODE = p_Sku_Code;
                mSkus.TIME_STAMP = DateTime.Now;
                mSkus.LASTUPDATE_DATE = DateTime.Now;
                mSkus.IP_ADDRESS = p_Ip_Address;
                mSkus.PACKSIZE = p_packSize;
                mSkus.USER_ID = p_UserId;
                mSkus.DESCRIPTION = p_DESCRIPTION;
                mSkus.SECTION_ID = pSectionId;
                mSkus.IS_DEAL = p_IsDeal;
                mSkus.IS_MODIFIER = p_IsModifier;
                mSkus.IS_Recipe = p_IsRecipe;
                mSkus.IsInventoryWeight = p_IsInventoryWeight;
                mSkus.MIN_LEVEL = minlevel;
                mSkus.REORDER_LEVEL = reorderlevel;
                mSkus.IS_HasMODIFIER = p_IS_HasMODIFIER;
                mSkus.intSaleMUnitCode = p_intSaleMUnitCode;
                mSkus.intPurchaseMUnitCode = p_intPurchaseMUnitCode;
                mSkus.intStockMUnitCode = p_intStockMUnitCode;
                mSkus.fltFEDPercentage = p_fltFEDPercentage;
                mSkus.fltWHTPercentage = p_fltWHTPercentage;
                mSkus.fltAgeInDays = p_fltAgeInDays;
                mSkus.IsMarketItem = p_IsMarketItem;
                mSkus.IsReplaceable = p_IsReplaceable;
                mSkus.IsFEDItem = p_IsFEDItem;
                mSkus.IsWHTItem = p_IsWHTItem;
                mSkus.IsSerialized = p_IsSerialized;
                mSkus.IsHazardous = p_IsHazardous;
                mSkus.IsBatchItem = p_IsBatchItem;
                mSkus.IsOverSaleAllowed = p_IsOverSaleAllowed;
                mSkus.IsExpiryAllowed = p_IsExpiryAllowed;
                mSkus.IsWarehouseItem = p_IsWarehouseItem;
                mSkus.MAX_LEVEL = p_MAX_LEVEL;
                mSkus.Sale_to_PurchaseFactor = p_Sale_to_PurchaseFactor;
                mSkus.Purchase_to_SaleFactor = p_Purchase_to_SaleFactor;
                mSkus.Sale_to_StockFactor = p_Sale_to_StockFactor;
                mSkus.Purchase_to_StockFactor = p_Purchase_to_StockFactor;
                mSkus.Default_Qty = p_Default_Qty;
                mSkus.Stock_to_SaleFactor = p_Stock_to_SaleFactor;
                mSkus.Stock_to_PurchaseFactor = p_Stock_to_PurchaseFactor;
                mSkus.Sale_to_PurchaseOperator = p_Sale_to_PurchaseOperator;
                mSkus.Purchase_to_SaleOperator = p_Purchase_to_SaleOperator;
                mSkus.Sale_to_StockOperator = p_Sale_to_StockOperator;
                mSkus.Purchase_to_StockOperator = p_Purchase_to_StockOperator;
                mSkus.Stock_to_SaleOperator = p_Stock_to_SaleOperator;
                mSkus.Stock_to_PurchaseOperator = p_Stock_to_PurchaseOperator;
                mSkus.strDescription = p_strDescription;
                mSkus.strSerialCode = p_strSerialCode;
                mSkus.strStatus = p_strStatus;
                mSkus.strERPCode = p_strERPCode;
                mSkus.fltShelfAgeInDays = p_ShelfAgeInDays;
                mSkus.intMUnitLifeCode = p_MUnitLifeCode;
                mSkus.BUTTON_COLOR = p_BUTTON_COLOR;
                mSkus.SKU_IMAGE = p_SKU_IMAGE;
                mSkus.IsSaleWeight = p_IsSaleWeight;
                mSkus.IsUnGroup = p_IsUnGroup;
                mSkus.IsPackage = p_IsPackage;
                mSkus.SortOrder = !string.IsNullOrEmpty(p_SortOrder) ? Convert.ToInt32(p_SortOrder) : 1;
                mSkus.PreparationTime = p_PreparationTime;
                mSkus.UOM = p_UOM;
                mSkus.ValidateStockOnSplitItem = p_ValidateStockOnSplitItem;
                mSkus.Is_StickerPrint = p_isStickerPrint;
                mSkus.strKOTDescription = p_strKOTDescription;
                mSkus.IsRunOut = p_IsRunOut;
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
        
        #endregion

        public int InsertProductSection(string SECTION_CODE, string SECTION_NAME, string PRINTER_NAME, bool IS_FULL_KOT,int NO_OF_PRINTS)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertPRODUCT_SECTION mDepartment = new spInsertPRODUCT_SECTION();
                mDepartment.Connection = mConnection;
                mDepartment.SECTION_CODE = SECTION_CODE;
                mDepartment.SECTION_NAME = SECTION_NAME;
                mDepartment.PRINTER_NAME = PRINTER_NAME;
                mDepartment.IS_FULL_KOT = IS_FULL_KOT;
                mDepartment.NO_OF_PRINTS = NO_OF_PRINTS;
                mDepartment.ExecuteQuery();
                return mDepartment.SECTION_ID;
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
        public bool InsertUserSECTION(int p_SECTION_ID, int p_USER_ID, bool p_is_Selected)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspUSER_SECTION mDepartment = new uspUSER_SECTION();
                mDepartment.Connection = mConnection;
                mDepartment.SECTION_ID = p_SECTION_ID;
                mDepartment.USER_ID = p_USER_ID;
                mDepartment.IsChecked = p_is_Selected;
                mDepartment.ExecuteQuery();
                return true;
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
        public bool UpdateProductSection(int SECTION_ID, string SECTION_CODE, string SECTION_NAME, string PRINTER_NAME, bool IS_ACTIVE, bool IS_FULL_KOT,int NO_OF_PRINTS)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdatePRODUCT_SECTION mSERVICE_TYPE = new spUpdatePRODUCT_SECTION();

                mSERVICE_TYPE.Connection = mConnection;
                mSERVICE_TYPE.IS_ACTIVE = IS_ACTIVE;
                mSERVICE_TYPE.PRINTER_NAME = PRINTER_NAME;
                mSERVICE_TYPE.SECTION_CODE = SECTION_CODE;
                mSERVICE_TYPE.SECTION_ID = SECTION_ID;
                mSERVICE_TYPE.SECTION_NAME = SECTION_NAME;
                mSERVICE_TYPE.IS_FULL_KOT = IS_FULL_KOT;
                mSERVICE_TYPE.NO_OF_PRINTS = NO_OF_PRINTS;
                bool a = mSERVICE_TYPE.ExecuteQuery();
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
        public DataTable SelectUserInSection(int SECTION_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspUSER_SECTION mSERVICE_TYPE = new uspUSER_SECTION();
                mSERVICE_TYPE.Connection = mConnection;

                mSERVICE_TYPE.SECTION_ID = SECTION_ID;
                return mSERVICE_TYPE.ExecuteTable();
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
        public DataTable SelectProductSection(int SECTION_ID, string SECTION_CODE, string SECTION_NAME)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectPRODUCT_SECTION mSERVICE_TYPE = new spSelectPRODUCT_SECTION();
                mSERVICE_TYPE.Connection = mConnection;

                mSERVICE_TYPE.SECTION_CODE = SECTION_CODE;
                mSERVICE_TYPE.SECTION_ID = SECTION_ID;
                mSERVICE_TYPE.SECTION_NAME = SECTION_NAME;

                //   mPAYMENT_TYPE.IS_ACTIVE = p_IS_ACTIVE;


                return mSERVICE_TYPE.ExecuteTable();

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
        public DataTable GetFinshedDetail(int p_FINISHED_SKU_ID,int p_DISTRIBUTOR_ID, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetFinishedDetail mSkuInfo = new uspGetFinishedDetail();
                mSkuInfo.Connection = mConnection;
                mSkuInfo.FINISHED_SKU_ID = p_FINISHED_SKU_ID;
                mSkuInfo.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mSkuInfo.TYPE_ID = p_TYPE_ID;
                DataTable dt = mSkuInfo.ExecuteTable();
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

        public int InsertFinishedSKU(int p_FINISHED_SKU_ID,decimal p_RecipeQty, int p_RecipeUnit, 
            DateTime p_DOCUMENT_DATE, int p_USER_ID,bool p_Is_Production,
            DataTable dtFinishedDetail, DataTable deletedFinishedDetail)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                uspInsertFinishedMaster mFinishedMaster = new uspInsertFinishedMaster();
                mFinishedMaster.Connection = mConnection;
                mFinishedMaster.Transaction = mTransaction;
                //------------Insert Finished Goods Master----------
                if (dtFinishedDetail.Rows.Count > 0)
                {
                    mFinishedMaster.FINISHED_SKU_ID = p_FINISHED_SKU_ID;
                    mFinishedMaster.DOCUMENT_DATE = p_DOCUMENT_DATE;
                    mFinishedMaster.USER_ID = p_USER_ID;
                    mFinishedMaster.Recipe_Qty = p_RecipeQty;
                    mFinishedMaster.intRecipeMUnitCode = p_RecipeUnit;
                    mFinishedMaster.Is_Production = p_Is_Production;
                    mFinishedMaster.ExecuteQuery();
                    //----------------Insert Finished Goods Detail-------------
                    uspInsertFinishedDetail mFinishedDetail = new uspInsertFinishedDetail();
                    mFinishedDetail.Connection = mConnection;
                    mFinishedDetail.Transaction = mTransaction;
                    foreach (DataRow dr in dtFinishedDetail.Rows)
                    {
                        mFinishedDetail.FINISHED_GOOD_MASTER_ID = mFinishedMaster.FINISHED_GOOD_MASTER_ID;
                        mFinishedDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mFinishedDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString());
                        mFinishedDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                        mFinishedDetail.ExecuteQuery();
                    }

                    foreach (DataRow dr in deletedFinishedDetail.Rows)
                    {
                        mFinishedDetail.FINISHED_GOOD_MASTER_ID = mFinishedMaster.FINISHED_GOOD_MASTER_ID;
                        mFinishedDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mFinishedDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString());
                        mFinishedDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                        mFinishedDetail.ExecuteQueryForDelete();
                    }

                    mTransaction.Commit();                    
                }
                return mFinishedMaster.FINISHED_GOOD_MASTER_ID;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return Constants.IntNullValue;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public DataTable GetRecipeLocation(int DistributorID, int TypeiD)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetRecipeLocation mUserAssing = new uspGetRecipeLocation();
                mUserAssing.Connection = mConnection;
                mUserAssing.DISTRIBUTOR_ID = DistributorID;
                mUserAssing.TYPE_ID = TypeiD;
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
        public bool InsertRecipeLocation(int p_LocationID, int p_RecipeID,int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspInsertRecipeLocation mFinishedMaster = new uspInsertRecipeLocation();
                mFinishedMaster.Connection = mConnection;
                mFinishedMaster.LocationID = p_LocationID;
                mFinishedMaster.RecipeID = p_RecipeID;
                mFinishedMaster.TypeID = p_TypeID;
                mFinishedMaster.ExecuteQuery();
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

        public DataSet GetSKUClosingStockLastPrice(int p_SKU_ID, int p_DISTRIBUTOR_ID,DateTime p_StockDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetClosingStockLastPrice mspSelectSkuInfo = new uspGetClosingStockLastPrice();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SKU_ID = p_SKU_ID;
                mspSelectSkuInfo.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mspSelectSkuInfo.StockDate = p_StockDate;
                return mspSelectSkuInfo.ExecuteTable();                
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
        public DataTable GetSKULastPurchasePrice(int p_SKU_ID, int p_DISTRIBUTOR_ID, bool p_IS_Recipe)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetLastPurchasePrice mspSelectSkuInfo = new uspGetLastPurchasePrice();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SKU_ID = p_SKU_ID;
                mspSelectSkuInfo.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mspSelectSkuInfo.IS_Recipe = p_IS_Recipe;
                return mspSelectSkuInfo.ExecuteTable();
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

        public DataSet GetSKUClosingStock(int p_SKU_ID, int p_DISTRIBUTOR_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetClosingStockLastPrice mspSelectSkuInfo = new uspGetClosingStockLastPrice();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SKU_ID = p_SKU_ID;
                mspSelectSkuInfo.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                return mspSelectSkuInfo.ExecuteTableForAllSKUS();
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

        public DataTable CheckSKU(int p_SKU_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spCheckSKU mSkuInfo = new spCheckSKU();
                mSkuInfo.Connection = mConnection;
                mSkuInfo.SKU_ID = p_SKU_ID;
                DataTable dt = mSkuInfo.ExecuteTable();
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

        public int InsertOpenItem(string p_SKU_NAME,int p_CATEGORY_ID,int p_SECTION_ID,int p_USER_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspInsertOpenItem mDepartment = new uspInsertOpenItem();
                mDepartment.Connection = mConnection;
                mDepartment.SKU_NAME = p_SKU_NAME;
                mDepartment.CATEGORY_ID = p_CATEGORY_ID;
                mDepartment.SECTION_ID = p_SECTION_ID;
                mDepartment.USER_ID = p_USER_ID;
                mDepartment.ExecuteQuery();
                return mDepartment.SKU_ID;
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

        public DataTable SelectCategoryType()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectCategoryType mspSelectCategoryType = new spSelectCategoryType();
                mspSelectCategoryType.Connection = mConnection;


                DataTable ds = mspSelectCategoryType.ExecuteTable();

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

        public DataTable GetSKUByName(string p_SKU_NAME,int p_CATEGORY_ID,int p_BRAND_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spGetSKUSByName mSKU = new spGetSKUSByName();
                mSKU.Connection = mConnection;
                mSKU.SKU_NAME = p_SKU_NAME;
                mSKU.CATEGORY_ID = p_CATEGORY_ID;
                mSKU.BRAND_ID = p_BRAND_ID;
                DataTable ds = mSKU.ExecuteTable();

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

        public DataTable GetERPCode(string p_ERPCode)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spGetERPCode mSKU = new spGetERPCode();
                mSKU.Connection = mConnection;
                mSKU.strERPCode = p_ERPCode;
                DataTable ds = mSKU.ExecuteTable();
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

        #region Recipe Production

        public DataTable SelectRecipeInfo(int p_finish_id, long p_lngRecipeProductionCode, int DistributorID, DateTime Date,int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelecttblRecipeProductionMaster mspSelectSkuInfo = new spSelecttblRecipeProductionMaster();
                mspSelectSkuInfo.Connection = mConnection;

                mspSelectSkuInfo.FINISHED_SKU_ID = p_finish_id;
                mspSelectSkuInfo.lngRecipeProductionCode = p_lngRecipeProductionCode;
                mspSelectSkuInfo.DISTRIBUTOR_ID = DistributorID;
                mspSelectSkuInfo.DATE = Date;
                mspSelectSkuInfo.TYPE_ID = p_TYPE_ID;
                DataTable ds = mspSelectSkuInfo.ExecuteTable();

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
        public bool InsertRecipeProduction(int p_FINISHED_GOOD_MASTER_ID, int p_DISTRIBUTOR_ID,
            int p_FINISHED_SKU_ID, decimal p_RecipeQty, decimal p_ActualQty,
            DateTime p_Production_DATE, int p_RecipeUnit, DateTime p_DOCUMENT_DATE,
            int p_USER_ID, string p_ConsumptionFromProductionPlan, DataTable dtFinishedDetail,
            bool IsFinanceIntegrate, DataTable dtCOAConfig, string p_remarks)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertblRecipeProductionMaster mRecipeMaster = new spInsertblRecipeProductionMaster();
                mRecipeMaster.Connection = mConnection;
                mRecipeMaster.Transaction = mTransaction;

                //------------Insert Recipe Master----------\\

                if (dtFinishedDetail.Rows.Count > 0)
                {
                    mRecipeMaster.FINISHED_SKU_ID = p_FINISHED_SKU_ID;
                    mRecipeMaster.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                    mRecipeMaster.DOCUMENT_DATE = p_DOCUMENT_DATE;
                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.ExpectedProduction_Qty = p_RecipeQty;
                    mRecipeMaster.ActualProduction_Qty = p_ActualQty;
                    mRecipeMaster.FINISHED_GOOD_MASTER_ID = p_FINISHED_GOOD_MASTER_ID;
                    mRecipeMaster.intProductionMUnitCode = p_RecipeUnit;
                    mRecipeMaster.Production_DATE = p_Production_DATE;
                    mRecipeMaster.TIME_STAMP = DateTime.Now;
                    mRecipeMaster.LASTUPDATE_DATE = DateTime.Now;
                    mRecipeMaster.IS_ACTIVE = true;
                    mRecipeMaster.REMARKS = p_remarks;

                    mRecipeMaster.ExecuteQuery();

                    UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                    mStockUpdate.Connection = mConnection;
                    mStockUpdate.Transaction = mTransaction;
                    mStockUpdate.PRINCIPAL_ID = 0;
                    mStockUpdate.TYPE_ID = 17;
                    mStockUpdate.DISTRIBUTOR_ID = mRecipeMaster.DISTRIBUTOR_ID;
                    mStockUpdate.STOCK_DATE = mRecipeMaster.DOCUMENT_DATE;
                    mStockUpdate.SKU_ID = mRecipeMaster.FINISHED_SKU_ID;
                    mStockUpdate.STOCK_QTY = mRecipeMaster.ActualProduction_Qty;
                    mStockUpdate.PRICE = 0;
                    mStockUpdate.FREE_QTY = 0;
                    mStockUpdate.BATCHNO = "NA";
                    mStockUpdate.UOM_ID = mRecipeMaster.intProductionMUnitCode;
                    mStockUpdate.ExecuteQuery();

                    decimal GLConsumption = 0;
                    //----------------Insert Recipe Production Detail-------------
                    if (p_ConsumptionFromProductionPlan != "1")
                    {
                        spInserttblRecipeProductionDetail mRecipeDetail = new spInserttblRecipeProductionDetail();
                        mRecipeDetail.Connection = mConnection;
                        mRecipeDetail.Transaction = mTransaction;

                        UspProcessStockRegister mStockDetail = new UspProcessStockRegister();
                        mStockDetail.Connection = mConnection;
                        mStockDetail.Transaction = mTransaction;
                        foreach (DataRow dr in dtFinishedDetail.Rows)
                        {
                            mRecipeDetail.lngRecipeProductionCode = mRecipeMaster.lngRecipeProductionCode;
                            mRecipeDetail.FINISHED_GOOD_DETAIL_ID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                            mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                            mRecipeDetail.QUANTITY = decimal.Parse(dr["ActulQty"].ToString());
                            mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                            mRecipeDetail.sintRowNo = 0;
                            mRecipeDetail.ActulQty = decimal.Parse(dr["ActulQty"].ToString());
                            mRecipeDetail.OrignalQty = decimal.Parse(dr["OrignalQty"].ToString());
                            mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                            mRecipeDetail.ExecuteQuery();

                            mStockDetail.PRINCIPAL_ID = 0;
                            mStockDetail.TYPE_ID = 18;
                            mStockDetail.DISTRIBUTOR_ID = mRecipeMaster.DISTRIBUTOR_ID;
                            mStockDetail.STOCK_DATE = mRecipeMaster.DOCUMENT_DATE;
                            mStockDetail.SKU_ID = mRecipeDetail.SKU_ID;
                            mStockDetail.STOCK_QTY = mRecipeDetail.ActulQty;
                            mStockDetail.PRICE = 0;
                            mStockDetail.FREE_QTY = 0;
                            mStockDetail.BATCHNO = "NA";
                            mStockDetail.UOM_ID = mRecipeDetail.intStockMUnitCode;
                            mStockDetail.ExecuteQuery();
                            GLConsumption += mRecipeDetail.ActulQty * mRecipeDetail.Price;
                        }

                        if (IsFinanceIntegrate && GLConsumption > 0)
                        {
                            //Dr Consumption
                            //Cr Stock in Trade
                            DataRow[] drConfig = null;
                            LedgerController LController = new LedgerController();
                            string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, p_DOCUMENT_DATE);
                            if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, 0, VoucherNo, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.CashPayment, Convert.ToString(mRecipeMaster.lngRecipeProductionCode), "Recipe Production Voucher, Prod#: " + mRecipeMaster.lngRecipeProductionCode.ToString(), p_USER_ID, "RecipeProd", Constants.Document_RecipeProduction, mRecipeMaster.lngRecipeProductionCode))
                            {
                                drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                                LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), GLConsumption, 0, "Consumption Sale Voucher");

                                drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                                LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, GLConsumption, "Inventoryatstore Sale Voucher");
                            }
                        }
                    }
                    else
                    {
                        spInserttblRecipeProductionDetail mRecipeDetail = new spInserttblRecipeProductionDetail();
                        mRecipeDetail.Connection = mConnection;
                        mRecipeDetail.Transaction = mTransaction;
                        foreach (DataRow dr in dtFinishedDetail.Rows)
                        {
                            mRecipeDetail.lngRecipeProductionCode = mRecipeMaster.lngRecipeProductionCode;
                            mRecipeDetail.FINISHED_GOOD_DETAIL_ID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                            mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                            mRecipeDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString()) * mRecipeMaster.ActualProduction_Qty;
                            mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                            mRecipeDetail.sintRowNo = 0;
                            mRecipeDetail.ActulQty = decimal.Parse(dr["ActulQty"].ToString());
                            mRecipeDetail.OrignalQty = decimal.Parse(dr["OrignalQty"].ToString());
                            mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                            mRecipeDetail.ExecuteQuery();
                        }
                    }
                                        
                    mTransaction.Commit();
                    return true;
                }
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
            return true;
        }

        public bool UpdateRecipeProduction(long MASTER_ID, int p_DISTRIBUTOR_ID, decimal p_RecipeQty, 
            int p_FINISHED_SKU_ID, decimal p_ActualQty, DateTime p_Production_DATE, int p_RecipeUnit, 
            DateTime p_DOCUMENT_DATE, int p_USER_ID, string p_ConsumptionFromProductionPlan,
            DataTable dtRecipeDetail, bool IsFinanceIntegrate, DataTable dtCOAConfig, string p_remarks)
        {
            DataTable dtRecipeDetail2 = new DataTable();
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spUpdatetblRecipeProductionMaster mRecipeMaster = new spUpdatetblRecipeProductionMaster();
                mRecipeMaster.Connection = mConnection;

                //------------Insert Recipe Master----------\\
                if (p_ConsumptionFromProductionPlan == "1")
                {
                    if (dtRecipeDetail.Rows.Count > 0)
                    {
                        mRecipeMaster.USER_ID = p_USER_ID;
                        mRecipeMaster.ActualProduction_Qty = p_ActualQty;
                        mRecipeMaster.Production_DATE = p_Production_DATE;
                        mRecipeMaster.LASTUPDATE_DATE = DateTime.Now;
                        mRecipeMaster.IS_ACTIVE = true;
                        mRecipeMaster.TypeID = 2;
                        mRecipeMaster.lngRecipeProductionCode = MASTER_ID;
                        mRecipeMaster.REMARKS = p_remarks;

                        mRecipeMaster.ExecuteQuery();

                        UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                        mStockUpdate.Connection = mConnection;

                        mStockUpdate.PRINCIPAL_ID = 0;
                        mStockUpdate.TYPE_ID = 17;
                        mStockUpdate.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mStockUpdate.STOCK_DATE = p_DOCUMENT_DATE;
                        mStockUpdate.SKU_ID = p_FINISHED_SKU_ID;
                        mStockUpdate.STOCK_QTY = mRecipeMaster.ActualProduction_Qty;
                        mStockUpdate.PRICE = 0;
                        mStockUpdate.FREE_QTY = 0;
                        mStockUpdate.BATCHNO = "NA";
                        mStockUpdate.UOM_ID = p_RecipeUnit;
                        mStockUpdate.ExecuteQuery();


                        //----------------Insert Finished Goods Detail-------------
                        spInserttblRecipeProductionDetail mRecipeDetail = new spInserttblRecipeProductionDetail();
                        mRecipeDetail.Connection = mConnection;

                        foreach (DataRow dr in dtRecipeDetail.Rows)
                        {
                            mRecipeDetail.lngRecipeProductionCode = mRecipeMaster.lngRecipeProductionCode;
                            mRecipeDetail.FINISHED_GOOD_DETAIL_ID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                            mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                            mRecipeDetail.QUANTITY = decimal.Parse(dr["ActulQty"].ToString());
                            mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                            mRecipeDetail.sintRowNo = 0;
                            mRecipeDetail.ActulQty = decimal.Parse(dr["ActulQty"].ToString());
                            mRecipeDetail.OrignalQty = decimal.Parse(dr["OrignalQty"].ToString());
                            mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                            mRecipeDetail.ExecuteQuery();
                        }
                        return true;
                    }
                }
                else
                {
                    if (dtRecipeDetail.Rows.Count > 0)
                    {
                        spSelecttblRecipeProductionMaster mSelectRecipe = new spSelecttblRecipeProductionMaster();
                        mSelectRecipe.Connection = mConnection;
                        
                        mSelectRecipe.FINISHED_SKU_ID = p_FINISHED_SKU_ID;
                        mSelectRecipe.lngRecipeProductionCode = MASTER_ID;
                        mSelectRecipe.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mSelectRecipe.DATE = p_DOCUMENT_DATE;
                        mSelectRecipe.TYPE_ID = 3;
                        dtRecipeDetail2 = mSelectRecipe.ExecuteTable();

                        UspUpdatePurchaseDetailStock mRecipeStock = new UspUpdatePurchaseDetailStock();
                        mRecipeStock.Connection = mConnection;

                        foreach (DataRow dr in dtRecipeDetail2.Rows)
                        {
                            mRecipeStock.TYPEID = 21;
                            mRecipeStock.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                            mRecipeStock.PURCHASE_DETAIL_ID = 0;
                            mRecipeStock.PURCHASE_MASTER_ID = MASTER_ID;
                            mRecipeStock.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                            mRecipeStock.DATE = p_DOCUMENT_DATE;
                            mRecipeStock.QTY = decimal.Parse(dr["ActulQty"].ToString());
                            mRecipeStock.ExecuteQuery();
                        }

                        mRecipeMaster.USER_ID = p_USER_ID;
                        mRecipeMaster.ActualProduction_Qty = p_ActualQty;
                        mRecipeMaster.Production_DATE = p_Production_DATE;
                        mRecipeMaster.LASTUPDATE_DATE = DateTime.Now;
                        mRecipeMaster.TypeID = 1;
                        mRecipeMaster.IS_ACTIVE = true;
                        mRecipeMaster.lngRecipeProductionCode = MASTER_ID;

                        mRecipeMaster.ExecuteQuery();

                        UspProcessStockRegister mStockUpdate = new UspProcessStockRegister();
                        mStockUpdate.Connection = mConnection;

                        mStockUpdate.PRINCIPAL_ID = 0;
                        mStockUpdate.TYPE_ID = 17;
                        mStockUpdate.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mStockUpdate.STOCK_DATE = p_DOCUMENT_DATE;
                        mStockUpdate.SKU_ID = p_FINISHED_SKU_ID;
                        mStockUpdate.STOCK_QTY = mRecipeMaster.ActualProduction_Qty;
                        mStockUpdate.PRICE = 0;
                        mStockUpdate.FREE_QTY = 0;
                        mStockUpdate.BATCHNO = "NA";
                        mStockUpdate.UOM_ID = p_RecipeUnit;
                        mStockUpdate.ExecuteQuery();


                        //----------------Insert Finished Goods Detail-------------
                        spInserttblRecipeProductionDetail mRecipeDetail = new spInserttblRecipeProductionDetail();
                        mRecipeDetail.Connection = mConnection;

                        UspProcessStockRegister mStockDetail = new UspProcessStockRegister();
                        mStockDetail.Connection = mConnection;

                        decimal GLConsumption = 0;
                        foreach (DataRow dr in dtRecipeDetail.Rows)
                        {
                            mRecipeDetail.lngRecipeProductionCode = mRecipeMaster.lngRecipeProductionCode;
                            mRecipeDetail.FINISHED_GOOD_DETAIL_ID = int.Parse(dr["FINISHED_GOOD_DETAIL_ID"].ToString());
                            mRecipeDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                            mRecipeDetail.QUANTITY = decimal.Parse(dr["ActulQty"].ToString());
                            mRecipeDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                            mRecipeDetail.sintRowNo = 0;
                            mRecipeDetail.ActulQty = decimal.Parse(dr["ActulQty"].ToString());
                            mRecipeDetail.OrignalQty = decimal.Parse(dr["OrignalQty"].ToString());
                            mRecipeDetail.Price = decimal.Parse(dr["Price"].ToString());
                            mRecipeDetail.ExecuteQuery();

                            mStockDetail.PRINCIPAL_ID = 0;
                            mStockDetail.TYPE_ID = 18;
                            mStockDetail.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                            mStockDetail.STOCK_DATE = p_DOCUMENT_DATE;
                            mStockDetail.SKU_ID = mRecipeDetail.SKU_ID;
                            mStockDetail.STOCK_QTY = mRecipeDetail.ActulQty;
                            mStockDetail.PRICE = 0;
                            mStockDetail.FREE_QTY = 0;
                            mStockDetail.BATCHNO = "NA";
                            mStockDetail.UOM_ID = mRecipeDetail.intStockMUnitCode;
                            mStockDetail.ExecuteQuery();
                            GLConsumption += mRecipeDetail.ActulQty * mRecipeDetail.Price;
                        }

                        if (IsFinanceIntegrate && GLConsumption > 0)
                        {
                            //Dr Consumption
                            //Cr Stock in Trade
                            DataRow[] drConfig = null;
                            LedgerController LController = new LedgerController();

                            string VoucherNo = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, p_DOCUMENT_DATE);

                            if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, 0, VoucherNo, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.CashPayment, Convert.ToString(mRecipeMaster.lngRecipeProductionCode), "Recipe Production Voucher, Prod#: " + mRecipeMaster.lngRecipeProductionCode.ToString(), p_USER_ID, "RecipeProd", Constants.Document_RecipeProduction, mRecipeMaster.lngRecipeProductionCode))
                            {
                                drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                                LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), GLConsumption, 0, "Consumption Sale Voucher");

                                drConfig = dtCOAConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                                LController.PostingGLDetail(p_DISTRIBUTOR_ID, 0, Constants.Journal_Voucher, VoucherNo, Convert.ToInt64(drConfig[0]["VALUE"].ToString()), 0, GLConsumption, "Inventoryatstore Sale Voucher");
                            }
                        }

                        return true;
                    }
                }
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
            return true;
        }

        public bool DeleteRecipeProduction(long p_MASTER_ID,int p_DISTRIBUTOR_ID,DateTime p_DOCUMENT_DATE,
            int p_FINISHED_SKU_ID, int p_USER_ID)
        {
            DataTable dtRecipeDetail = new DataTable();
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spUpdatetblRecipeProductionMaster mRecipeMaster = new spUpdatetblRecipeProductionMaster();
                mRecipeMaster.Connection = mConnection;
                mRecipeMaster.Transaction = mTransaction;

                spSelecttblRecipeProductionMaster mSelectRecipe = new spSelecttblRecipeProductionMaster();
                mSelectRecipe.Connection = mConnection;
                mSelectRecipe.Transaction = mTransaction;

                mSelectRecipe.FINISHED_SKU_ID = p_FINISHED_SKU_ID;
                mSelectRecipe.lngRecipeProductionCode = p_MASTER_ID;
                mSelectRecipe.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mSelectRecipe.DATE = p_DOCUMENT_DATE;
                mSelectRecipe.TYPE_ID = 3;
                dtRecipeDetail = mSelectRecipe.ExecuteTable();

                if (dtRecipeDetail.Rows.Count > 0)
                {
                    UspUpdatePurchaseDetailStock mRecipeStock = new UspUpdatePurchaseDetailStock();
                    mRecipeStock.Connection = mConnection;
                    mRecipeStock.Transaction = mTransaction;

                    foreach (DataRow dr in dtRecipeDetail.Rows)
                    {

                        mRecipeStock.TYPEID = 21;
                        mRecipeStock.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                        mRecipeStock.PURCHASE_DETAIL_ID = 0;
                        mRecipeStock.PURCHASE_MASTER_ID = p_MASTER_ID;
                        mRecipeStock.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mRecipeStock.DATE = p_DOCUMENT_DATE;
                        mRecipeStock.QTY = decimal.Parse(dr["ActulQty"].ToString());
                        mRecipeStock.ExecuteQuery();
                    }


                    mRecipeMaster.USER_ID = p_USER_ID;
                    mRecipeMaster.ActualProduction_Qty = Convert.ToDecimal(dtRecipeDetail.Rows[0]["ActualProduction_Qty"]);
                    mRecipeMaster.Production_DATE = p_DOCUMENT_DATE;
                    mRecipeMaster.LASTUPDATE_DATE = DateTime.Now;
                    mRecipeMaster.IS_ACTIVE = false;
                    mRecipeMaster.TypeID = 1;
                    mRecipeMaster.lngRecipeProductionCode = p_MASTER_ID;

                    mRecipeMaster.ExecuteQuery();

                    mTransaction.Commit();
                    return true;
                }
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
            return true;
        }

        public bool DeleteRecipeProduction(long p_MASTER_ID, int p_USER_ID)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spUpdatetblRecipeProductionMaster mRecipeMaster = new spUpdatetblRecipeProductionMaster();
                mRecipeMaster.Connection = mConnection;
                mRecipeMaster.Transaction = mTransaction;
                mRecipeMaster.USER_ID = p_USER_ID;
                mRecipeMaster.TypeID = 3;
                mRecipeMaster.lngRecipeProductionCode = p_MASTER_ID;

                mRecipeMaster.ExecuteQuery();

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
        #endregion

        #region Package Material

        public DataTable GetPackageDetail(int p_SKU_ID, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetPackageDetail mSkuInfo = new uspGetPackageDetail();
                mSkuInfo.Connection = mConnection;
                mSkuInfo.SKU_ID = p_SKU_ID;
                mSkuInfo.TYPE_ID = p_TYPE_ID;
                DataTable dt = mSkuInfo.ExecuteTable();
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

        public bool InsertPackagedSKU(int p_SKU_ID, decimal p_PackageQty, int p_Unit, DateTime p_DOCUMENT_DATE
           , int p_USER_ID, DataTable dtPackageDetail)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                uspInsertPackageMaster mPackageMaster = new uspInsertPackageMaster();
                mPackageMaster.Connection = mConnection;
                mPackageMaster.Transaction = mTransaction;

                //------------Insert Finished Goods Master----------

                if (dtPackageDetail.Rows.Count > 0)
                {
                    mPackageMaster.SKU_ID = p_SKU_ID;
                    mPackageMaster.DOCUMENT_DATE = p_DOCUMENT_DATE;
                    mPackageMaster.USER_ID = p_USER_ID;
                    mPackageMaster.Package_Qty = p_PackageQty;
                    mPackageMaster.intMUnitCode = p_Unit;

                    mPackageMaster.ExecuteQuery();

                    //----------------Insert Finished Goods Detail-------------
                    uspInsertPackageDetail mPackageDetail = new uspInsertPackageDetail();
                    mPackageDetail.Connection = mConnection;
                    mPackageDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtPackageDetail.Rows)
                    {
                        mPackageDetail.intPackageMaterialID = mPackageMaster.intPackageMaterialID;
                        mPackageDetail.SKU_ID = int.Parse(dr["SKU_ID"].ToString());
                        mPackageDetail.QUANTITY = decimal.Parse(dr["QUANTITY"].ToString());
                        mPackageDetail.intStockMUnitCode = int.Parse(dr["UOM_ID"].ToString());
                        mPackageDetail.DineIn_CUSTOMER_TYPE_ID = int.Parse(dr["DINE_IN"].ToString());
                        mPackageDetail.Delivery_CUSTOMER_TYPE_ID = int.Parse(dr["DELIVERY"].ToString());
                        mPackageDetail.TakeAway_CUSTOMER_TYPE_ID = int.Parse(dr["TAKEAWAY"].ToString());

                        mPackageDetail.ExecuteQuery();
                    }
                    mTransaction.Commit();
                    return true;
                }
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return false;// exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
            return true;
        }

        #endregion

        public DataTable GetItemList(int p_TYPE_ID, int p_CATEGORY_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetItemList mSKU = new uspGetItemList();
                mSKU.Connection = mConnection;
                mSKU.TYPE_ID = p_TYPE_ID;
                mSKU.CATEGORY_ID = p_CATEGORY_ID;
                DataTable ds = mSKU.ExecuteTable();

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

        public SKUResponse GetMenuCardDetails(SKURequest request)
        {
            SKUResponse response = new SKUResponse();
            response.SKUInfoList = new List<SKUInfo>();
            response.CategoryList = new List<SKUCategory>();

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.SKU_GetMenuCardDetails";
                command.Connection = mConnection;

                IDataParameterCollection pparams = command.Parameters;
                IDataParameter parameter;

                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@categoryTypeId";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                parameter.Value = request.CategoryTypeId;
                pparams.Add(parameter);

                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@companyId";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                parameter.Value = request.CompanyId;
                pparams.Add(parameter);

                IDbDataAdapter da = ProviderFactory.GetAdapter(EnumProviders.SQLClient);
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    response.CategoryList = ds.Tables[0].ToCollection<SKUCategory>();
                    response.SKUInfoList = ds.Tables[1].ToCollection<SKUInfo>();
                }
                response.IsException = false;

                return response;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                response.IsException = true;
                response.Message = exp.Message;
                return response;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public ProductSectionResponse GetProductSectionsWithPrinters(ProductSectionRequest request)
        {
            ProductSectionResponse response = new ProductSectionResponse();
            response.ProductSectionList = new List<ProductSection>();

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.CAT_GetProductSections";
                command.Connection = mConnection;

                IDbDataAdapter da = ProviderFactory.GetAdapter(EnumProviders.SQLClient);
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    response.ProductSectionList = ds.Tables[0].ToCollection<ProductSection>();
                }
                response.IsException = false;

                return response;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                response.IsException = true;
                response.Message = exp.Message;
                return response;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public DataTable GetIRawtemPrice(int p_category_id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetRawItemPrice mspSelectSkuInfo = new uspGetRawItemPrice();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.CATEGORY_ID = p_category_id;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        public DataTable GetKOTNo(int pDISTRIBUTOR_ID, DateTime pDOCUMENT_DATE, string pMANUAL_ORDER_NO,long pOldOrderID,int pTYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetManualKOTNo mKOTNo = new uspGetManualKOTNo() { Connection = mConnection, DISTRIBUTOR_ID = pDISTRIBUTOR_ID, MANUAL_ORDER_NO = pMANUAL_ORDER_NO, DOCUMENT_DATE = pDOCUMENT_DATE, OldOrderID= pOldOrderID, TYPE_ID= pTYPE_ID };
                DataTable dt = mKOTNo.ExecuteTable();

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

        public DataTable GetSKUConsumption(int p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                Reports.DsReport ds = new Reports.DsReport();
                spSelectSKU_Consumption mSkuModifier = new spSelectSKU_Consumption();
                mSkuModifier.Connection = mConnection;
                mSkuModifier.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mSkuModifier.SKU_ID = Constants.IntNullValue;
                mSkuModifier.Type_ID = Constants.IntNullValue;
                mSkuModifier.REPORT_TYPE = 8;
                mSkuModifier.FROM_DATE = p_FromDate;
                mSkuModifier.TO_DATE = p_ToDate;
                DataTable dt = mSkuModifier.ExecuteTable();
                return dt;
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

        #region Items Against ECommerce Categories
        public DataTable GetAssignUnAssignItemsINECommCategory(int p_CATEGORY_ID, int p_pos_Category_ID, int p_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spSelectEcommAssignedUnAssignedItem ObjSelect = new spSelectEcommAssignedUnAssignedItem();
                ObjSelect.Connection = mConnection;
                ObjSelect.CategoryID = p_CATEGORY_ID;
                ObjSelect.PosCategoryID = p_pos_Category_ID;
                ObjSelect.TYPE = p_TYPE;
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

        public bool InsertItemAgainstCategory(int p_Category_ID, int p_Item_ID, int p_Unassign)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                //to be continued... add sp class and SP.
                spInsertItemAgainstCategory ObjInsert = new spInsertItemAgainstCategory();
                ObjInsert.Connection = mConnection;
                ObjInsert.SKU_ID = p_Item_ID;
                ObjInsert.Category_ID = p_Category_ID;
                ObjInsert.Unassign = p_Unassign;
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

        public bool UpdateSizeAndCrust(int p_Category_ID, int p_Item_ID, string p_size, string p_crust)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                //to be continued... add sp class and SP.
                spUpdateSizeAndCrustForEcommerceItem ObjInsert = new spUpdateSizeAndCrustForEcommerceItem();
                ObjInsert.Connection = mConnection;
                ObjInsert.SKU_ID = p_Item_ID;
                ObjInsert.Category_ID = p_Category_ID;
                ObjInsert.Size = p_size;
                ObjInsert.Crust = p_crust;
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

        public bool InsertECommerceCategory(string p_categoryName, int p_parentCategoryId, 
            int sortOrder, string p_ImagePath, int p_CategoryId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                //to be continued... add sp class and SP.
                spInsertECommerceCategory ObjInsert = new spInsertECommerceCategory();
                ObjInsert.Connection = mConnection;
                ObjInsert.Category_Name = p_categoryName;
                ObjInsert.Parent_Category_ID = p_parentCategoryId;
                ObjInsert.SortOrder = sortOrder;
                ObjInsert.ImagePath = p_ImagePath;
                ObjInsert.Category_ID = p_CategoryId;
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

        #region Item Assignment
        public bool InsertUpdateItemAssignment(int p_TYPE_ID,int p_SKU_ID,int p_CHILD_SKU_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                //to be continued... add sp class and SP.
                uspItemAssignment ObjInsert = new uspItemAssignment();
                ObjInsert.Connection = mConnection;
                ObjInsert.TYPE_ID = p_TYPE_ID;
                ObjInsert.SKU_ID = p_SKU_ID;
                ObjInsert.CHILD_SKU_ID = p_CHILD_SKU_ID;
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
        public DataTable GetItemAssignment(int p_TYPE_ID,int p_SKU_ID, int p_CHILD_SKU_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspItemAssignment mKOTNo = new uspItemAssignment() { Connection = mConnection, TYPE_ID = p_TYPE_ID,SKU_ID = p_SKU_ID ,CHILD_SKU_ID=p_CHILD_SKU_ID};
                DataTable dt = mKOTNo.ExecuteTable();

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

        #region Items Against Location

        public bool InsertItemAgainstLocation(int p_Location_ID, int p_Item_ID, int p_Unassign, int p_User_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                //to be continued... add sp class and SP.
                spInsertItemAgainstCategory ObjInsert = new spInsertItemAgainstCategory();
                ObjInsert.Connection = mConnection;
                ObjInsert.SKU_ID = p_Item_ID;
                ObjInsert.Category_ID = p_Location_ID;
                ObjInsert.Unassign = p_Unassign;
                ObjInsert.USER_ID = p_User_ID;
                bool Bvalue = ObjInsert.ExecuteQueryForItemAgainstLocation();
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

        #region BarCode Printing
        public bool InsertBarcode(string p_Company_Name, string p_Product_Name,
            string p_Product_price, string size, string color, byte[] p_image)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertBARCODE mSkus = new spInsertBARCODE();

                mSkus.Connection = mConnection;
                mSkus.COMPANY_NAME = p_Company_Name;
                mSkus.PRODUCT_NAME = p_Product_Name;
                mSkus.PRODUCT_PRICE = p_Product_price;
                mSkus.PRODUCT_SIZE = size;
                mSkus.PRODUCT_COLOR = color;
                mSkus.BARCODE_IMAGE = p_image;

                mSkus.ExecuteQuery();
                return true;

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                //return exp.Message;
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
        public DataTable SelectSkuBarcode()
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spSelectBARCODE mBarcode = new spSelectBARCODE();
                mBarcode.Connection = mConnection;
                //mBarcode.PRODUCT_NAME = p_ROWNO;
                DataTable dt = mBarcode.ExecuteTable();
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
        public DataTable SearchProduct(string pSearchText)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSearchProduct mspSelectSkuInfo = new uspSearchProduct();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SEARCH_TEXT = pSearchText;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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
        public DataTable SearchProduct(string pSearchText, int pDistributorId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSearchProduct mspSelectSkuInfo = new uspSearchProduct();
                mspSelectSkuInfo.Connection = mConnection;
                mspSelectSkuInfo.SEARCH_TEXT = pSearchText;
                mspSelectSkuInfo.DistributorId = pDistributorId;
                DataTable dt = mspSelectSkuInfo.ExecuteTable();

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

        public DataSet GetConsumptionMonthWise(string p_DISTRIBUTOR_ID, DateTime p_FromDate,
           DateTime p_ToDate, int p_Type_ID, int p_SubType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spDateWiseItemConsumption objPrint = new spDateWiseItemConsumption();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_IDs = p_DISTRIBUTOR_ID;
                objPrint.FROM_DATE = p_FromDate;
                objPrint.TO_DATE = p_ToDate;
                objPrint.TYPE_ID = p_Type_ID;
                objPrint.DELIVERY_CHANNEL = p_SubType;
                DataTable dt = objPrint.ExecuteTableForMonthWiseConsumption();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectSKU_Consumption"].ImportRow(dr);
                }

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
    }


    public class ProductSection
    {
        public int SectionId { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string PrinterName { get; set; }
        public bool IsPrint { get; set; }
        public bool IsActive { get; set; }
    }

    public class SKUInfo
    {
        public int CATEGORY_ID { get; set; }
        public int SKU_ID { get; set; }
        public string SKU_CODE { get; set; }
        public string SKU_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string SKU_IMAGE { get; set; }
        public decimal PRICE { get; set; }
    }

    public class SKUCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class SKUResponse
    {
        public List<SKUInfo> SKUInfoList { get; set; }
        public List<SKUCategory> CategoryList { get; set; }

        public bool IsException { get; set; }
        public string Message { get; set; }
    }

    public class SKURequest
    {
        public int CategoryTypeId { get; set; }
        public int CompanyId { get; set; }
    }

    public class ProductSectionResponse
    {
        public List<ProductSection> ProductSectionList { get; set; }

        public bool IsException { get; set; }
        public string Message { get; set; }
    }

    public class ProductSectionRequest
    {
        public ProductSection ProductSectionInfo { get; set; }
    }

}