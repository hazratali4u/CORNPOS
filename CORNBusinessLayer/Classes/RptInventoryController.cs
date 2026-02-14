using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Fetching Data Of Inventory Reports
    /// </summary>
    public class RptInventoryController
    {
        #region Constructor

        /// <summary>
        /// Constructor for RptInventoryController
        /// </summary>
        public RptInventoryController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets Data For Stock Reconciliation Report
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <param name="p_USER_ID">User</param>
        /// <returns>DataSet</returns>
        public DataSet SelectPrincipalStockReconcilation(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, 
            DateTime p_To_Date, int p_USER_ID, int p_UOM_ID, int p_PRICE_TYPE, 
            string distributorIds, string IsLocationWiseItem, string sectionIds,int p_ZERO_ELIMINATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspRptSelectStockRegister ObjPrint = new uspRptSelectStockRegister();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.distributor_id = p_Distributor_ID;
                ObjPrint.Company_Id = p_Principal_Id;
                ObjPrint.DateFrom = p_FromDate;
                ObjPrint.dateto = p_To_Date;
                ObjPrint.USER_ID = p_USER_ID;
                ObjPrint.UOM_ID = p_UOM_ID;
                ObjPrint.PRICE_TYPE = p_PRICE_TYPE;
                ObjPrint.DISTRIBUTOR_Ids = distributorIds;
                ObjPrint.SECTION_IDs = sectionIds;
                ObjPrint.ZERO_ELIMINATE = p_ZERO_ELIMINATE;
                DataTable dt = new DataTable();
                if (IsLocationWiseItem == "1")
                {
                    dt = ObjPrint.ExecuteTableForLocationWiseItem();
                }
                else
                {
                    dt = ObjPrint.ExecuteTable();
                }
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["StockRegister"].ImportRow(dr);
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

        public DataSet SelectPrincipalStockReconcilation(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate,
            DateTime p_To_Date, int p_USER_ID, int p_UOM_ID, int p_PRICE_TYPE, string p_SectionIDs)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspRptSelectStockRegisterSection ObjPrint = new uspRptSelectStockRegisterSection();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.distributor_id = p_Distributor_ID;
                ObjPrint.Company_Id = p_Principal_Id;
                ObjPrint.DateFrom = p_FromDate;
                ObjPrint.dateto = p_To_Date;
                ObjPrint.USER_ID = p_USER_ID;
                ObjPrint.UOM_ID = p_UOM_ID;
                ObjPrint.PRICE_TYPE = p_PRICE_TYPE;
                ObjPrint.SECTION_IDS = p_SectionIDs;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["StockRegister"].ImportRow(dr);
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

        public DataSet SelectSkuStockStatus(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_USER_ID, int p_UOM_ID, int p_PRICE_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspRptSelectStockStatus ObjPrint = new uspRptSelectStockStatus();
                CORNBusinessLayer.Reports.dsSalesPurchaseRegister ds = new CORNBusinessLayer.Reports.dsSalesPurchaseRegister();
                ObjPrint.Connection = mConnection;
                ObjPrint.distributor_id = p_Distributor_ID;
                ObjPrint.Company_Id = p_Principal_Id;
                ObjPrint.DateFrom = p_FromDate;
                ObjPrint.dateto = p_To_Date;
                ObjPrint.USER_ID = p_USER_ID;
                ObjPrint.UOM_ID = p_UOM_ID;
                ObjPrint.PRICE_TYPE = p_PRICE_TYPE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspRptSelectStockStatus"].ImportRow(dr);
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

        /// <summary>
        /// Gets Data For Date Wise Stock Report
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <param name="p_TypeId">Type</param>
        /// <returns>DataSet</returns>
        public DataSet SelectPurchaseTransferStock(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_TypeId, int p_RATE_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspDailyPurchaseTransfer ObjPrint = new UspDailyPurchaseTransfer();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.TYPEID = p_TypeId;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.RATE_TYPE = p_RATE_TYPE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["DailyPurchaseTransferReport"].ImportRow(dr);
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

        /// <summary>
        /// Gets Data For Transfer In/Out Report (In Quantity And Carton)
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_FromTime">DateFrom</param>
        /// <param name="p_ToDate">DateTo</param>
        /// <param name="p_TransferType">Type</param>
        /// <param name="p_type">ReportType</param>
        /// <returns>DataSet</returns>
        public DataSet TransferIn(int p_Principal_ID, int p_Distributor_ID, DateTime p_FromTime, DateTime p_ToDate, string p_TransferType, int p_type)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspTransferInrpt mTransferIn = new UspTransferInrpt();
                mTransferIn.Connection = mConnection;

                mTransferIn.PRINCIPAL_ID = p_Principal_ID;
                mTransferIn.DISTRIBUTOR_ID = p_Distributor_ID;
                mTransferIn.FromDate = p_FromTime;
                mTransferIn.ToDate = p_ToDate;
                mTransferIn.TransferType = p_TransferType;
                mTransferIn.ReportType = p_type;
                DataTable DT = mTransferIn.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["TransferIn"].ImportRow(dr);
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

        /// <summary>
        /// Gets Data For Transfer In/Out Report (In Value)
        /// </summary>
        /// <param name="p_Principal_ID">Principal</param>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_FromTime">DateFrom</param>
        /// <param name="p_ToDate">DateTo</param>
        /// <param name="p_TransferType">Type</param>
        /// <param name="p_type">ReportTyp</param>
        /// <returns>DataSet</returns>
        public DataSet TransferInOutValue(int p_Principal_ID, int p_Distributor_ID, DateTime p_FromTime, DateTime p_ToDate, string p_TransferType, int p_type)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();

                UspTransferInOutValue mTransferIn = new UspTransferInOutValue();
                mTransferIn.Connection = mConnection;

                mTransferIn.PRINCIPAL_ID = p_Principal_ID;
                mTransferIn.DISTRIBUTOR_ID = p_Distributor_ID;
                mTransferIn.FromDate = p_FromTime;
                mTransferIn.ToDate = p_ToDate;
                mTransferIn.TransferType = p_TransferType;
                mTransferIn.ReportType = p_type;
                DataTable DT = mTransferIn.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["RptTransferInOutValueWise"].ImportRow(dr);
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

        /// <summary>
        /// Gets Data For Purchase Document Report
        /// </summary>
        /// <param name="p_Distributor_ID">Location</param>
        /// <param name="p_Principal_Id">Principal</param>
        /// <param name="p_FromDate">DateFrom</param>
        /// <param name="p_To_Date">DateTo</param>
        /// <param name="p_TypeId">Type</param>
        /// <returns>DataSet</returns>
        public DataSet SelectPurchaseDocument(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate,DateTime p_To_Date, int p_TypeId, string p_Purchase_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocument ObjPrint = new GetPurchasedocument();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.TYPE_ID = p_TypeId;
                ObjPrint.DOCUMENT_ID = !string.IsNullOrEmpty(p_Purchase_Id) ? Convert.ToInt64(p_Purchase_Id) : Constants.LongNullValue;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptPurchaseDocument"].ImportRow(dr);
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

        public DataSet selectSupllierWisePurchase(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, 
            DateTime p_To_Date, int p_UserId, int p_reportType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSupllierWisePurchase ObjPrint = new spSupllierWisePurchase();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.USER_ID = p_UserId;
                ObjPrint.TYPE_ID = p_reportType;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSupllierWisePurchase"].ImportRow(dr);
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

        public DataSet SelectPurchaseDocumentPopUp(int p_Purchase_ID, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocumentPopUp ObjPrint = new GetPurchasedocumentPopUp();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DOCUMENT_ID = p_Purchase_ID;
                ObjPrint.TYPE_ID = p_TypeId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptPurchasedocument"].ImportRow(dr);
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
        public DataSet SelectPurchaseDocumentPopUp2(long p_Purchase_ID, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocumentPopUp ObjPrint = new GetPurchasedocumentPopUp();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DOCUMENT_ID = p_Purchase_ID;
                ObjPrint.TYPE_ID = p_TypeId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptPurchasedocument"].ImportRow(dr);
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

        public DataSet SelectTransferDocument(int p_Distributor_ID, int p_Principal_Id,DateTime p_FromDate, DateTime p_To_Date, int p_TypeId, int p_to_Location)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocument ObjPrint = new GetPurchasedocument();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.TO_LOCATION = p_to_Location;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.TYPE_ID = p_TypeId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptTransferocument"].ImportRow(dr);
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
        public DataSet SelectTransferDocumentSummary(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate,
            DateTime p_To_Date, int p_TypeId, int p_to_Location, int p_priceType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocument ObjPrint = new GetPurchasedocument();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.TO_LOCATION = p_to_Location;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.TYPE_ID = p_TypeId;
                ObjPrint.DOCUMENT_ID = p_priceType;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptTransferocumentSummary"].ImportRow(dr);
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
        public DataSet SelectTransferDocumentPopUp(long p_Purchase_ID, int p_TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocumentPopUp ObjPrint = new GetPurchasedocumentPopUp();
                CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
                ObjPrint.Connection = mConnection;
                ObjPrint.DOCUMENT_ID = p_Purchase_ID;
                ObjPrint.TYPE_ID = p_TypeId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptTransferocument"].ImportRow(dr);
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

        public DataSet SelectIssueDocument(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate, DateTime p_To_Date, int p_TypeId, long pPurchaseId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocument ObjPrint = new GetPurchasedocument();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.TYPE_ID = p_TypeId;
                ObjPrint.DOCUMENT_ID = pPurchaseId;

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptPurchaseDocument"].ImportRow(dr);
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

        public DataSet SelectRawIssuenceReport(int DistributorId, DateTime FromDate, DateTime ToDate, string SKU_ID, 
            int TypeId, int priceType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectRawMaterialIssuence ObjPrint = new spSelectRawMaterialIssuence();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;
                ObjPrint.TYPE_ID = TypeId;
                ObjPrint.SKU_ID = SKU_ID;
                ObjPrint.PRICE_TYPE = priceType;

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectRawMaterialIssuence"].ImportRow(dr);
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

        public DataSet SelectDateWiseStokReport(int DistributorId, DateTime FromDate, DateTime ToDate, string SKU_ID, int TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectdatewisestocktaking ObjPrint = new uspSelectdatewisestocktaking();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;
                ObjPrint.TYPE_ID = TypeId;
                ObjPrint.SKU_ID = SKU_ID;

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspSelectdatewisestocktaking"].ImportRow(dr);
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


        public DataSet selectStockDemandReport(string  DistributorId, DateTime FromDate, DateTime ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectStockDemandReprot ObjPrint = new spSelectStockDemandReprot();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;
                

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectStockDemandReprot"].ImportRow(dr);
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
        public DataSet selectStockDemandvsTransferOutReport(string DistributorId, string ToDistributorId, DateTime FromDate, DateTime ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectStockDemandReprot ObjPrint = new spSelectStockDemandReprot();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.ToDISTRIBUTOR_ID = ToDistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;


                DataTable dt = ObjPrint.ExecuteTableForDemandVsTransferOut();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectStockDemandReprot"].ImportRow(dr);
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

        public DataSet StockDemandPrint(int p_DemandId,int p_DISTRIBUTOR_ID,DateTime p_FROM_DATE,DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectStockDemandForPrint ObjPrint = new spSelectStockDemandForPrint();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.STOCK_DEMAND_ID = p_DemandId;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.FROM_DATE = p_FROM_DATE;
                ObjPrint.TO_DATE = p_TO_DATE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectStockDemandForPrint"].ImportRow(dr);
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

        public DataSet StockDemandPrint(int p_DemandId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectStockDemandForPrint ObjPrint = new spSelectStockDemandForPrint();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.STOCK_DEMAND_ID = p_DemandId;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectStockDemandForPrint"].ImportRow(dr);
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

        public DataSet GetDemandFinishGood(int p_DemandId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectStockDemandForPrint ObjPrint = new spSelectStockDemandForPrint();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.STOCK_DEMAND_ID = p_DemandId;
                ObjPrint.TYPE_ID = 1;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spSelectStockDemandForPrint"].ImportRow(dr);
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

        public DataSet GetPhysicalStockTaking(int DistributorId, DateTime FromDate, DateTime ToDate,int TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspSelectdatewisestocktaking ObjPrint = new uspSelectdatewisestocktaking();
                Reports.DsReport ds = new Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;
                ObjPrint.TYPE_ID = TypeId;
                ObjPrint.SKU_ID = null;

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["PhysicalStockTaking"].ImportRow(dr);
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

        public DataSet GetPhysicalStockTaking(string p_DISTRIBUTOR_IDs, DateTime p_FROM_DATE, DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                Reports.DSReportNew ds = new Reports.DSReportNew();

                uspGetPhysicalStockTaking mLedger = new uspGetPhysicalStockTaking();

                mLedger.Connection = mConnection;
                mLedger.DISTRIBUTOR_IDs = p_DISTRIBUTOR_IDs;
                mLedger.TO_DATE = p_TO_DATE;
                mLedger.FROM_DATE = p_FROM_DATE;
                DataTable DT = mLedger.ExecuteTable();

                foreach (DataRow dr in DT.Rows)
                {
                    ds.Tables["uspGetPhysicalStockTaking"].ImportRow(dr);

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
        
        public DataSet GetStockVariation(int p_DISTRIBUTOR_ID,DateTime p_STOCK_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetStockVariation ObjPrint = new uspGetStockVariation();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.STOCK_DATE = p_STOCK_DATE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetStockVariation"].ImportRow(dr);
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

        public DataSet GetStockValuation2(int p_DISTRIBUTOR_ID, DateTime p_STOCK_DATE,string p_cetegoryIDs,int p_TypeId,int p_userID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetStockValuation2 ObjPrint = new uspGetStockValuation2();
                Reports.DSReportNew ds = new CORNBusinessLayer.Reports.DSReportNew();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.STOCK_DATE = p_STOCK_DATE;
                ObjPrint.CETEGORYIDS = p_cetegoryIDs;
                ObjPrint.TYPE = p_TypeId;
                ObjPrint.USER_ID = p_userID;
                ObjPrint.PRINCIPAL_ID = Constants.IntNullValue;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetStockValuation2"].ImportRow(dr);
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
        public DataSet GetItemWisePurchase(string p_DISTRIBUTOR_ID, string p_CATEGORY_ID, DateTime p_FROM_DATE, DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetItemWisePurchase ObjPrint = new uspGetItemWisePurchase();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.CATEGORY_ID = p_CATEGORY_ID;
                ObjPrint.SKU_ID = null;
                ObjPrint.FROM_DATE = p_FROM_DATE;
                ObjPrint.TO_DATE = p_TO_DATE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetPurchaseVariance"].ImportRow(dr);
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
        public DataSet GetItemWisePurchase(string p_DISTRIBUTOR_ID, string p_CATEGORY_ID,string p_SKU_ID,DateTime p_FROM_DATE,DateTime p_TO_DATE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetItemWisePurchase ObjPrint = new uspGetItemWisePurchase();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                ObjPrint.CATEGORY_ID = p_CATEGORY_ID;
                ObjPrint.SKU_ID = p_SKU_ID;
                ObjPrint.FROM_DATE = p_FROM_DATE;
                ObjPrint.TO_DATE = p_TO_DATE;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetItemWisePurchase"].ImportRow(dr);
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
        
        public DataSet GetSKULedgerData(DateTime p_FromDate, DateTime p_ToDate, int p_PRINCIPAL_ID, string p_Distributor_IDs, string p_SKU_ID, int p_CATEGORY_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetSKULedgerData ObjPrint = new uspGetSKULedgerData();
                CORNBusinessLayer.Reports.dsSalesPurchaseRegister ds = new CORNBusinessLayer.Reports.dsSalesPurchaseRegister();
                ObjPrint.Connection = mConnection;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_ToDate;
                ObjPrint.PRINCIPAL_ID = p_PRINCIPAL_ID;
                ObjPrint.DISTRIBUTOR_IDs = p_Distributor_IDs;
                ObjPrint.SKU_ID = p_SKU_ID;
                ObjPrint.CATEGORY_ID = p_CATEGORY_ID;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetSKULedgerData"].ImportRow(dr);
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
        public DataTable GetSKULedgerDataOpening(DateTime p_FromDate, DateTime p_ToDate, int p_PRINCIPAL_ID, string p_Distributor_IDs, int p_SKU_ID, int p_CATEGORY_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetSKULedgerDataOpening ObjPrint = new uspGetSKULedgerDataOpening();
                CORNBusinessLayer.Reports.dsSalesPurchaseRegister ds = new CORNBusinessLayer.Reports.dsSalesPurchaseRegister();
                ObjPrint.Connection = mConnection;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_ToDate;
                ObjPrint.PRINCIPAL_ID = p_PRINCIPAL_ID;
                ObjPrint.DISTRIBUTOR_IDs = p_Distributor_IDs;
                ObjPrint.SKU_ID = p_SKU_ID;
                ObjPrint.CATEGORY_ID = p_CATEGORY_ID;
                DataTable dt = ObjPrint.ExecuteTable();                
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
        public DataSet GetDateWiseProductionINReport(int p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_ToDate, int p_finished_SKU_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectRawMaterialIssuence objPrint = new spSelectRawMaterialIssuence();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                objPrint.FROM_DATE = p_FromDate;
                objPrint.TO_DATE = p_ToDate;
                objPrint.TYPE_ID = 1;
                objPrint.FINISHED_SKU_ID = p_finished_SKU_ID;
                DataTable dt = objPrint.ExecuteTableForProductionINDateWiseReport();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["spDateWiseItemConsumption"].ImportRow(dr);
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

        public DataSet GetProductionINSummaryReport(int p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_ToDate, int p_finished_SKU_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectRawMaterialIssuence objPrint = new spSelectRawMaterialIssuence();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                objPrint.FROM_DATE = p_FromDate;
                objPrint.TO_DATE = p_ToDate;
                objPrint.TYPE_ID = 2;
                objPrint.FINISHED_SKU_ID = p_finished_SKU_ID;
                DataTable dt = objPrint.ExecuteTableForProductionINDateWiseReport();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptPurchaseDocument"].ImportRow(dr);
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
        public DataSet SelectPurchaseOrderDocument(int p_Distributor_ID, int p_Principal_Id, DateTime p_FromDate,
            DateTime p_To_Date, int p_TypeId, string p_PurchaseOrder_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                GetPurchasedocument ObjPrint = new GetPurchasedocument();
                CORNBusinessLayer.Reports.DsReport ds = new CORNBusinessLayer.Reports.DsReport();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = p_Distributor_ID;
                ObjPrint.PRINCIPAL_ID = p_Principal_Id;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.TYPE_ID = p_TypeId;
                ObjPrint.DOCUMENT_ID = !string.IsNullOrEmpty(p_PurchaseOrder_Id) ? Convert.ToInt64(p_PurchaseOrder_Id) : Constants.LongNullValue;
                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    //ds.Tables["RptPurchaseDocument"].ImportRow(dr);
                    ds.Tables["RptPurchaseOrder"].ImportRow(dr);
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

        public DataSet GetLowStock(int p_DISTRIBUTOR_ID, DateTime p_StockDate, int p_TYPE_ID,string p_CATEGORY_IDS)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspLowStockList objPrint = new uspLowStockList();
                Reports.dsSalesPurchaseRegister ds = new Reports.dsSalesPurchaseRegister();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                objPrint.STOCK_DATE = p_StockDate;
                objPrint.TYPE_ID = p_TYPE_ID;
                objPrint.CATEGORY_IDS = p_CATEGORY_IDS;

                DataTable dt = objPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspLowStockList"].ImportRow(dr);
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
        public DataTable GetLowStock(int p_DISTRIBUTOR_ID, DateTime p_StockDate, int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspLowStockList objPrint = new uspLowStockList();
                Reports.dsSalesPurchaseRegister ds = new Reports.dsSalesPurchaseRegister();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                objPrint.STOCK_DATE = p_StockDate;
                objPrint.TYPE_ID = p_TYPE_ID;

                DataTable dt = objPrint.ExecuteTable();
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

        public DataTable GetItemRecipe(string p_FINISHED_SKU_ID, int p_DISTRIBUTOR_ID, int p_TYPE_ID, bool p_isActive)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetItemRecipe mSkuInfo = new uspGetItemRecipe();
                mSkuInfo.Connection = mConnection;
                mSkuInfo.FINISHED_SKU_ID = p_FINISHED_SKU_ID;
                mSkuInfo.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mSkuInfo.TYPE_ID = p_TYPE_ID;
                mSkuInfo.IsActive = p_isActive;
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
        public DataSet SelectRptConsumptionvsPhysicalStock(int p_Distributor_ID, DateTime p_FromDate,
            DateTime p_To_Date, int p_USER_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspRptSelectStockRegister ObjPrint = new uspRptSelectStockRegister();
                CORNBusinessLayer.Reports.DSReportNew ds = new CORNBusinessLayer.Reports.DSReportNew();
                ObjPrint.Connection = mConnection;
                ObjPrint.distributor_id = p_Distributor_ID;
                ObjPrint.DateFrom = p_FromDate;
                ObjPrint.dateto = p_To_Date;
                ObjPrint.USER_ID = p_USER_ID;

                DataTable dt = ObjPrint.ExecuteTableConsumptionvsPhysicalStock();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptConsumptionVsPhysicalStock"].ImportRow(dr);
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

        public DataSet SelectInvoiceBookingDetail(string p_ContractType, long p_PURCHASE_MASTER_ID, int p_TypeID)
        {
            IDbConnection mConnection = null;
            try
            {                
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectInvBooking_DETAIL mPurchaseDetail = new spSelectInvBooking_DETAIL();
                CORNBusinessLayer.Reports.dsSalesPurchaseRegister ds = new CORNBusinessLayer.Reports.dsSalesPurchaseRegister();
                mPurchaseDetail.Connection = mConnection;
                mPurchaseDetail.BATCH_NO = p_ContractType;
                mPurchaseDetail.PURCHASE_MASTER_ID = p_PURCHASE_MASTER_ID;
                mPurchaseDetail.TYPE_ID = p_TypeID;
                DataTable dt = mPurchaseDetail.ExecuteTable();

                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["dbInvBooking"].ImportRow(dr);
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

        public DataSet GetIssuanceVSSalesData(int DistributorId, DateTime FromDate, DateTime ToDate, string SECTION_IDs,int TypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetIssuenceVSSales ObjPrint = new uspGetIssuenceVSSales();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_ID = DistributorId;
                ObjPrint.FROM_DATE = FromDate;
                ObjPrint.TO_DATE = ToDate;
                ObjPrint.TYPE_ID = TypeId;
                ObjPrint.SECTION_IDs = SECTION_IDs;

                DataTable dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetIssuenceVSSales"].ImportRow(dr);
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

        public DataSet GetClosingStockSummary(string p_Distributor_IDs, DateTime p_FromDate,DateTime p_To_Date, string sectionIds)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetClosingStockSummary ObjPrint = new uspGetClosingStockSummary();
                Reports.DSReportNew ds = new Reports.DSReportNew();
                ObjPrint.Connection = mConnection;
                ObjPrint.DISTRIBUTOR_IDs = p_Distributor_IDs;
                ObjPrint.FROM_DATE = p_FromDate;
                ObjPrint.TO_DATE = p_To_Date;
                ObjPrint.SECTION_IDs = sectionIds;
                DataTable dt = new DataTable();
                
                dt = ObjPrint.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["uspGetClosingStockSummary"].ImportRow(dr);
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
        #endregion
    }
}