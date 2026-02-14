using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System.IO;
using CORNBusinessLayer.Reports;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Ledger And GL Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Ledger Data And GL Data
    /// </item>
    /// <term>
    /// Update Ledger Data And GL Data
    /// </term>
    /// <item>
    /// Get Ledger Data And GL Data
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class LedgerController
    {
        #region Constuctor
        
        /// <summary>
        /// Constructor for LedgerController
        /// </summary>
        public LedgerController()
		{
			//
			// TODO: Add constructor logic here
			//
        }

        #endregion

        #region Public Methods

        #region Select

        /// <summary>
        /// Gets Vouchers
        /// </summary>
        /// <remarks>
        /// Returns Vouchers as Datatable
        /// </remarks>
        /// <param name="p_Voucherdate">Date</param>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_VoucherTypeId">Type</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_VoucherNo">No</param>
        /// <returns>Vouchers as Datatable</returns>
        public DataTable SelectVoucherNo(DateTime p_Voucherdate, int p_Distributor_id, int p_VoucherTypeId, int p_UserId, string p_VoucherNo)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectGL_MASTER objSelect = new spSelectGL_MASTER();
                objSelect.Connection = mConnection;
                objSelect.VOUCHER_DATE = p_Voucherdate;
                objSelect.DISTRIBUTOR_ID = p_Distributor_id;
                objSelect.USER_ID = p_UserId;
                objSelect.VOUCHER_TYPE_ID = p_VoucherTypeId;
                objSelect.VOUCHER_NO = p_VoucherNo;
                objSelect.IS_POSTED = true;
                objSelect.IS_DELETED = false;
                DataTable dt = objSelect.ExecuteTable();
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
        public DataTable GetPettyExpenseVoucher(DateTime p_Voucherdate, int p_Distributor_id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spGetPeetyExpenseVooucher objSelect = new spGetPeetyExpenseVooucher();
                objSelect.Connection = mConnection;
                objSelect.VOUCHER_DATE = p_Voucherdate;
                objSelect.DISTRIBUTOR_ID = p_Distributor_id;                
                DataTable dt = objSelect.ExecuteTable();
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
        /// Gets Vouchers Detail
        /// </summary>
        /// <remarks>
        /// Returns Vouchers Detail as Datatable
        /// </remarks>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_VoucherNo">No</param>
        /// <param name="p_VoucherTypeId">Type</param>
        /// <returns>Vouchers Detail as Datatable</returns>
        public DataTable SelectVoucherDetail(int p_Distributor_id, string p_VoucherNo, int p_VoucherTypeId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectGL_DETAIL objSelect = new spSelectGL_DETAIL();
                objSelect.Connection = mConnection;
                objSelect.DISTRIBUTOR_ID = p_Distributor_id;
                objSelect.VOUCHER_NO = p_VoucherNo;
                objSelect.VOUCHER_TYPE_ID = p_VoucherTypeId;
                objSelect.IS_DELETED = false;
                DataTable dt = objSelect.ExecuteTable();
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
        /// Gets Customer Balance
        /// </summary>
        /// <remarks>
        /// Returns Customer Balance Data as Datatable
        /// </remarks>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Account_Head_Id">AccountHead</param>
        /// <param name="p_DocumentType_id">Type</param>
        /// <param name="FDate">DateFrom</param>
        /// <param name="TDate">DateTo</param>
        /// <returns>Customer Balance Data as Datatable</returns>
        public DataTable SelectBankCashTransction(int p_Distributor_Id, int p_Account_Head_Id, int p_DocumentType_id, 
            DateTime FDate, DateTime TDate,int p_TypeID, string p_slipNo)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectBankCashDetail mAccountHead = new UspSelectBankCashDetail();
                mAccountHead.Connection = mConnection;
                mAccountHead.Distributor_id = p_Distributor_Id;
                mAccountHead.Account_Head_id = p_Account_Head_Id;
                mAccountHead.Document_Type_Id = p_DocumentType_id;
                mAccountHead.SLIP_NO = p_slipNo;
                mAccountHead.From_Date = DateTime.Parse(FDate.ToShortDateString() + " 00:00:00");
                mAccountHead.To_Date = DateTime.Parse(TDate.ToShortDateString() + " 23:59:59");
                mAccountHead.TypeID = p_TypeID;

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
        /// Gets UnPosted Voucher Nos
        /// </summary>
        /// <remarks>
        /// Returns UnPosted Voucher Nos as Datatable
        /// </remarks>
        /// <param name="p_VoucherType_id">VoucherType</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="p_PrincipalId">Principal</param>
        /// <param name="p_IS_POST">IsPost</param>
        /// <param name="p_FROM_DATE">DateFrom</param>
        /// <param name="p_TO_DATE">DateTo</param>
        /// <param name="p_TYPE">Type</param>
        /// <returns>UnPosted Voucher Nos as Datatable</returns>
        public DataTable SelectUnPostVoucherNo(int p_VoucherType_id, int p_DistributorId, int p_PrincipalId, bool p_IS_POST, DateTime p_FROM_DATE, DateTime p_TO_DATE, int p_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectVoucherNo mMaxDNo = new UspSelectVoucherNo();
                mMaxDNo.Connection = mConnection;
                mMaxDNo.VOUCHER_TYPE_ID = p_VoucherType_id;
                mMaxDNo.DISTRIBUTOR_ID = p_DistributorId;
                mMaxDNo.IS_POST = p_IS_POST;
                mMaxDNo.FROM_DATE = p_FROM_DATE;
                mMaxDNo.TO_DATE = p_TO_DATE;
                mMaxDNo.PRINCIPAL_ID = p_PrincipalId;
                mMaxDNo.VOUCHER_TYPE_ID2 = p_TYPE;

                DataTable VouhcerId = mMaxDNo.ExecuteTable();
                return VouhcerId;
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

        public DataTable SelectUnPostVoucherNoEdit(int p_VoucherType_id, int p_DistributorId, int p_PrincipalId, bool p_IS_POST, DateTime p_FROM_DATE, DateTime p_TO_DATE, int p_TYPE)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectVoucherNoEdit mMaxDNo = new UspSelectVoucherNoEdit();
                mMaxDNo.Connection = mConnection;
                mMaxDNo.VOUCHER_TYPE_ID = p_VoucherType_id;
                mMaxDNo.DISTRIBUTOR_ID = p_DistributorId;
                mMaxDNo.IS_POST = p_IS_POST;
                mMaxDNo.FROM_DATE = p_FROM_DATE;
                mMaxDNo.TO_DATE = p_TO_DATE;
                mMaxDNo.PRINCIPAL_ID = p_PrincipalId;
                mMaxDNo.VOUCHER_TYPE_ID2 = p_TYPE;

                DataTable VouhcerId = mMaxDNo.ExecuteTable();
                return VouhcerId;
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

        public DataTable SelectUnPostVoucherNo(int p_VoucherType_id, int p_DistributorId, int p_PrincipalId, bool p_IS_POST, DateTime p_FROM_DATE, DateTime p_TO_DATE, int p_TYPE,int p_TYPE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectVoucherNo mMaxDNo = new UspSelectVoucherNo();
                mMaxDNo.Connection = mConnection;
                mMaxDNo.VOUCHER_TYPE_ID = p_VoucherType_id;
                mMaxDNo.DISTRIBUTOR_ID = p_DistributorId;
                mMaxDNo.IS_POST = p_IS_POST;
                mMaxDNo.FROM_DATE = p_FROM_DATE;
                mMaxDNo.TO_DATE = p_TO_DATE;
                mMaxDNo.PRINCIPAL_ID = p_PrincipalId;
                mMaxDNo.VOUCHER_TYPE_ID2 = p_TYPE;
                mMaxDNo.TYPE_ID = p_TYPE_ID;
                DataTable VouhcerId = mMaxDNo.ExecuteTable();
                return VouhcerId;
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
        /// Gets UnPosted Voucher Detail
        /// </summary>
        /// <remarks>
        /// Returns UnPosted Voucher Detail as Datatable
        /// </remarks>
        /// <param name="p_Voucher_id">Voucher</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="p_VoucherType">Type</param>
        /// <returns>UnPosted Voucher Detail as Datatable</returns>
        public DataTable SelectUnPostLedger(string p_Voucher_id, int p_DistributorId, int p_VoucherType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectVoucher ObjLedger = new spSelectVoucher();
                ObjLedger.Connection = mConnection;
                ObjLedger.VOUCHER_NO = p_Voucher_id;
                ObjLedger.DISTRIBUTOR_ID = p_DistributorId;
                ObjLedger.VOUCHER_TYPE_ID = p_VoucherType;

                DataTable dt = ObjLedger.ExecuteTable();
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
        public DataTable SelectUnPostLedger(int p_DistributorId, int p_VoucherType,DateTime p_dtFrom, DateTime p_dtTo)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetVoucherSummary ObjLedger = new uspGetVoucherSummary();
                ObjLedger.Connection = mConnection;                
                ObjLedger.DISTRIBUTOR_ID = p_DistributorId;
                ObjLedger.VOUCHER_TYPE_ID = p_VoucherType;
                ObjLedger.DateFrom = p_dtFrom;
                ObjLedger.DateTo = p_dtTo;
                DataTable dt = ObjLedger.ExecuteTable();
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

        public DataTable GetVoucherPopup(string p_UniqueID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectVoucherPopup ObjLedger = new spSelectVoucherPopup();
                ObjLedger.Connection = mConnection;
                ObjLedger.UniqueID = p_UniqueID;
                DataTable dt = ObjLedger.ExecuteTable();
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
        /// Gets Voucher Types
        /// </summary>
        /// <remarks>
        /// Returns Voucher Types as Datatable
        /// </remarks>
        /// <param name="p_User_Id">User</param>
        /// <returns>Voucher Types as Datatable</returns>
        public DataTable SelectVoucherType(int p_User_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspGetVoucherTypeRights mAccountHead = new UspGetVoucherTypeRights();
                mAccountHead.Connection = mConnection;
                mAccountHead.user_id = p_User_Id;

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
        /// Gets Max Document ID
        /// </summary>
        /// <remarks>
        /// Returns Max Document ID as String
        /// </remarks>
        /// <param name="p_Document_id">Document</param>
        /// <param name="p_DistributorId">Location</param>
        /// <returns>Max Document ID as String</returns>
        public string SelectLedgerMaxDocumentId(int p_Document_id, int p_DistributorId, int pType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectMaxDocumentNo mMaxDNo = new UspSelectMaxDocumentNo();
                mMaxDNo.Connection = mConnection;
                mMaxDNo.Document_TypeId = p_Document_id;
                mMaxDNo.Distributor_id = p_DistributorId;
                mMaxDNo.TypeId = pType;
                DataTable MaxId = mMaxDNo.ExecuteTable();
                return MaxId.Rows[0][0].ToString();
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
        /// Gets Max Voucher No
        /// </summary>
        /// <remarks>
        /// Returns Max Voucher No as String
        /// </remarks>
        /// <param name="p_Document_id">Document</param>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="mDate">Date</param>
        /// <returns>Max Voucher No as String</returns>
        public string SelectMaxVoucherId(int p_Document_id, int p_DistributorId, DateTime mDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectMaxVoucherNo mMaxDNo = new UspSelectMaxVoucherNo();
                mMaxDNo.Connection = mConnection;
                mMaxDNo.Document_TypeId = p_Document_id;
                mMaxDNo.Distributor_id = p_DistributorId;
                mMaxDNo.Month = mDate;
                DataTable MaxId = mMaxDNo.ExecuteTable();
                string MaxVoucherId = MaxId.Rows[0][0].ToString();

                if (MaxVoucherId.Length == 1)
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0000" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0000" + MaxVoucherId;
                    }

                }
                else if (MaxVoucherId.Length == 2)
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-000" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-000" + MaxVoucherId;
                    }

                }
                else if (MaxVoucherId.Length == 3)
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-00" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-00" + MaxVoucherId;
                    }

                }
                else if (MaxVoucherId.Length == 4)
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0" + MaxVoucherId;
                    }

                }
                else
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-" + MaxVoucherId;
                    }
                }
                return MaxVoucherId;
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

        public string SelectMaxVoucherIdTest(int p_Document_id, int p_DistributorId, DateTime mDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectMaxVoucherNo mMaxDNo = new UspSelectMaxVoucherNo();
                mMaxDNo.Connection = mConnection;
                mMaxDNo.Document_TypeId = p_Document_id;
                mMaxDNo.Distributor_id = p_DistributorId;
                mMaxDNo.Month = mDate;
                DataTable MaxId = mMaxDNo.ExecuteTable();
                string MaxVoucherId = MaxId.Rows[0][0].ToString();

                if (MaxVoucherId.Length == 1)
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0000" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0000" + MaxVoucherId;
                    }

                }
                else if (MaxVoucherId.Length == 2)
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-000" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-000" + MaxVoucherId;
                    }

                }
                else if (MaxVoucherId.Length == 3)
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-00" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-00" + MaxVoucherId;
                    }

                }
                else if (MaxVoucherId.Length == 4)
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-0" + MaxVoucherId;
                    }

                }
                else
                {
                    if (mDate.Month.ToString().Length == 1)
                    {
                        MaxVoucherId = "0" + mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-" + MaxVoucherId;
                    }
                    else
                    {
                        MaxVoucherId = mDate.Month.ToString() + mDate.Year.ToString().Substring(2, 2) + "-" + MaxVoucherId;
                    }
                }
                return MaxVoucherId;
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
        /// Gets Customer Pending Credit Invoices
        /// </summary>
        /// <remarks>
        /// Returns Pending Credit Invoices as Datatable
        /// </remarks>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_PRINCIPAL">Principal</param>
        /// <param name="p_CUSTOMER_ID">Customer</param>
        /// <param name="P_AREA_ID">Area</param>
        /// <returns>Pending Credit Invoices as Datatable</returns>
        public DataTable SelectCreditPendingInvoice(int p_Distributor_Id, int p_PRINCIPAL, long p_CUSTOMER_ID, int P_AREA_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectPendingCreditInvoice mCust = new UspSelectPendingCreditInvoice();
                mCust.Connection = mConnection;
                mCust.PRINCIPAL_ID = p_PRINCIPAL;
                mCust.DISTRIBUTOR_ID = p_Distributor_Id;
                mCust.CUSTOMER_ID = p_CUSTOMER_ID;
                mCust.AREA_ID = P_AREA_ID;
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

        public DataTable SelectCreditPendingInvoice2(int p_Distributor_Id, int p_PRINCIPAL, int P_AREA_ID,int p_DocumentTypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectPendingCreditInvoice2 mCust = new UspSelectPendingCreditInvoice2();
                mCust.Connection = mConnection;
                mCust.PRINCIPAL_ID = p_PRINCIPAL;
                mCust.DISTRIBUTOR_ID = p_Distributor_Id;
                mCust.AREA_ID = P_AREA_ID;
                mCust.DOCUMENT_TYPE_ID = p_DocumentTypeID;
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

        public DataTable GetPendingPurchaseInvoices(string p_Distributor_Id, int p_PRINCIPAL, int P_AREA_ID, int p_DocumentTypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetPendingPurchaseInvoices mCust = new uspGetPendingPurchaseInvoices();
                mCust.Connection = mConnection;
                mCust.PRINCIPAL_ID = p_PRINCIPAL;
                mCust.DISTRIBUTOR_ID = p_Distributor_Id;
                mCust.AREA_ID = P_AREA_ID;
                mCust.DOCUMENT_TYPE_ID = p_DocumentTypeID;
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
        public DataTable VendorBankCashTransction(int p_Distributor_Id, int p_Account_Head_Id, int p_DocumentType_id, DateTime FDate, DateTime TDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectVendorBankCashDetail mAccountHead = new UspSelectVendorBankCashDetail();
                mAccountHead.Connection = mConnection;
                mAccountHead.Distributor_id = p_Distributor_Id;
                mAccountHead.Account_Head_id = p_Account_Head_Id;
                mAccountHead.Document_Type_Id = p_DocumentType_id;
                mAccountHead.From_Date = DateTime.Parse(FDate.ToShortDateString() + " 00:00:00");
                mAccountHead.To_Date = DateTime.Parse(TDate.ToShortDateString() + " 23:59:59");

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

        public DataSet SelectBankCashTransction(int p_Distributor_Id, int p_Customerid, int p_DocumentType_id, DateTime FDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectBankCashDetail mAccountHead = new UspSelectBankCashDetail();
                mAccountHead.Connection = mConnection;
                mAccountHead.Distributor_id = p_Distributor_Id;
                mAccountHead.Account_Head_id = p_Customerid;
                mAccountHead.Document_Type_Id = p_DocumentType_id;
                mAccountHead.From_Date = FDate;


                DataSet ds = mAccountHead.ExecuteDataSet();
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

        public DataTable SelectUnPostLedger(string p_Voucher_id, int p_DistributorId, int p_VoucherType, int pType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectVoucher ObjLedger = new spSelectVoucher();
                ObjLedger.Connection = mConnection;
                ObjLedger.VOUCHER_NO = p_Voucher_id;
                ObjLedger.DISTRIBUTOR_ID = p_DistributorId;
                ObjLedger.VOUCHER_TYPE_ID = p_VoucherType;
                ObjLedger.TYPE_ID = pType;

                DataTable dt = ObjLedger.ExecuteTable();
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

        public DataTable GetUnPostLedger(string p_Voucher_id, string p_DistributorId, int p_VoucherType, int pType,string p_PayeeName)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectVoucher2 ObjLedger = new spSelectVoucher2();
                ObjLedger.Connection = mConnection;
                ObjLedger.VOUCHER_NO = p_Voucher_id;
                ObjLedger.DISTRIBUTOR_ID = p_DistributorId;
                ObjLedger.VOUCHER_TYPE_ID = p_VoucherType;
                ObjLedger.PAYEES_NAME = p_PayeeName;
                ObjLedger.TYPE_ID = pType;

                DataTable dt = ObjLedger.ExecuteTable();
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
        public DataTable SelectPetyCashSummary(int p_Distributor_ID, DateTime p_FromDate, DateTime p_To_Date, int p_USER_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspRptPettyCashSummery ObjPrint = new UspRptPettyCashSummery();
                ObjPrint.Connection = mConnection;
                ObjPrint.DistributorId = p_Distributor_ID;
                ObjPrint.FromDate = p_FromDate;
                ObjPrint.ToDate = p_To_Date;
                ObjPrint.USER_ID = p_USER_ID;

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

        /// <summary>
        /// Gets Claim Data
        /// </summary>
        /// <remarks>
        /// Returns Claim Data as Datatable
        /// </remarks>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_ClaimType_id">Type</param>
        /// <param name="FDate">DateFrom</param>
        /// <param name="TDate">DateTo</param>
        /// <returns>Claim Data as Datatable</returns>
        public DataTable SelectClaimDetail(int p_Distributor_Id, int p_ClaimType_id, DateTime FDate, DateTime TDate, int pType)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspSelectClaimDetail mAccountHead = new UspSelectClaimDetail();
                mAccountHead.Connection = mConnection;
                mAccountHead.Distributor_id = p_Distributor_Id;
                mAccountHead.Claim_Type_Id = p_ClaimType_id;
                mAccountHead.From_Date = DateTime.Parse(FDate.ToShortDateString() + " 00:00:00");
                mAccountHead.To_Date = DateTime.Parse(TDate.ToShortDateString() + " 23:59:59");
                mAccountHead.typeId = pType;
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
        public bool DeletePrincipalOpening(int p_DISTRIBUTOR_ID, int p_PRINCIPAL_ID, long p_MASTER_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspDeletePrincipalOpening mOpening = new uspDeletePrincipalOpening();
                mOpening.Connection = mConnection;
                mOpening.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mOpening.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mOpening.MASTER_ID = p_MASTER_ID;
                mOpening.ExecuteQuery();

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
        public bool DeleteSupplierPayment(int p_DISTRIBUTOR_ID, int p_PRINCIPAL_ID, int p_VoucherNo,int p_TransactionType,DateTime p_DocumentDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspDeleteSupplierPayment mDel = new uspDeleteSupplierPayment();
                mDel.Connection = mConnection;
                mDel.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mDel.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mDel.VoucherNo = p_VoucherNo;
                mDel.TransactionType = p_TransactionType;
                mDel.DocumentDate = p_DocumentDate;
                mDel.ExecuteQuery();

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

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Inserts Credit And Advance Invoices
        /// </summary>
        /// <param name="p_VoucherType">Type</param>
        /// <param name="p_VoucherNo">No</param>
        /// <param name="p_ACCOUNT_HEAD_ID">AccountHead</param>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Debit">Debit</param>
        /// <param name="p_Credit">Credit</param>
        /// <param name="p_Ledger_Date">Date</param>
        /// <param name="p_Remarks">Remarks</param>
        /// <param name="p_TimeStamp">CreatedOn</param>
        /// <param name="p_PRINCIPAL_ID">Principal</param>
        /// <param name="p_CUSTOMER_ID">Customer</param>
        /// <param name="p_DOCUMENT_NO">DocumentNo</param>
        /// <param name="p_MANUAL_DOCUMENT_NO">ManualualDocumentNo</param>
        /// <param name="p_DOCUMENT_TYPE_ID">DocumentType</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="mTransaction">Transaction</param>
        /// <param name="mConnection">Connection</param>
        /// <param name="p_Paymode">Mode</param>
        /// <param name="p_PayeesName">Payee</param>
        public void PostingInvoiceAccount(int p_VoucherType, long p_VoucherNo, long p_ACCOUNT_HEAD_ID, int p_Distributor_Id, decimal p_Debit, decimal p_Credit, DateTime p_Ledger_Date, string p_Remarks, DateTime p_TimeStamp, int p_PRINCIPAL_ID, int p_CUSTOMER_ID, long p_DOCUMENT_NO, string p_MANUAL_DOCUMENT_NO, int p_DOCUMENT_TYPE_ID, int p_UserId, IDbTransaction mTransaction, IDbConnection mConnection, int p_Paymode, string p_PayeesName)
        {
            try
            {
                spInsertLEDGER mspInsertLedger = new spInsertLEDGER()
                {
                    Connection = mConnection
                    , Transaction = mTransaction
                    , VOUCHER_TYPE_ID = p_VoucherType
                    , VOUCHER_NO = p_VoucherNo
                    , ACCOUNT_HEAD_ID = p_ACCOUNT_HEAD_ID
                    , DISTRIBUTOR_ID = p_Distributor_Id
                    , DEBIT = p_Debit
                    , CREDIT = p_Credit
                    , LEDGER_DATE = p_Ledger_Date
                    , REMARKS = p_Remarks
                    , TIME_STAMP = p_TimeStamp
                    , PRINCIPAL_ID = p_PRINCIPAL_ID
                    , CUSTOMER_ID = p_CUSTOMER_ID
                    , DOCUMENT_TYPE_ID = p_DOCUMENT_TYPE_ID
                    , DOCUMENT_NO = p_DOCUMENT_NO
                    , MANUAL_DOCUMENT_NO = p_MANUAL_DOCUMENT_NO
                    , USER_ID = p_UserId
                    , PAYMENT_MODE = p_Paymode
                    , PAYEE_NAME = p_PayeesName
                    , IS_DELETED = 0
                };
                mspInsertLedger.ExecuteQuery();
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
            }
        }
        
        /// <summary>
        /// Inserts Cheque And Cash Realizations
        /// </summary>
        /// <param name="p_VOUCHER_TYPE_ID">Type</param>
        /// <param name="p_VOUCHER_NO">No</param>
        /// <param name="p_ACCOUNT_HEAD_ID">AccountHead</param>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Debit">Debit</param>
        /// <param name="p_Credit">Credit</param>
        /// <param name="p_Ledger_Date">Date</param>
        /// <param name="p_Remarks">Remarks</param>
        /// <param name="p_TimeStamp">CreatedOn</param>
        /// <param name="p_Customer_ID">Customer</param>
        /// <param name="p_Principal">Principal</param>
        /// <param name="p_Cheque_NO">Cheque</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_Document_Id">DocumentNo</param>
        /// <param name="p_Manual_Document_ID">ManualDocumentNo</param>
        /// <param name="p_DocumentTypeId">DocumentType</param>
        /// <param name="SlipNo">Slip</param>
        /// <param name="p_ChequeDate">ChequeDate</param>
        /// <param name="p_PaymentMode">Mode</param>
        /// <param name="p_PayeesName">Payee</param>
        public long PostingCash_Bank_Account(int p_VOUCHER_TYPE_ID, long p_VOUCHER_NO,long p_ACCOUNT_HEAD_ID, int p_Distributor_Id, decimal p_Debit, decimal p_Credit,DateTime p_Ledger_Date, string p_Remarks, DateTime p_TimeStamp, int p_Customer_ID,int p_Principal, string p_Cheque_NO, int p_UserId, long p_Document_Id,string p_Manual_Document_ID, int p_DocumentTypeId, string SlipNo,DateTime p_ChequeDate,int p_PaymentMode,string p_PayeesName)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertLEDGER mspInsertLedger = new spInsertLEDGER();
                mspInsertLedger.Connection = mConnection;
                mspInsertLedger.ACCOUNT_HEAD_ID = p_ACCOUNT_HEAD_ID;
                mspInsertLedger.DISTRIBUTOR_ID = p_Distributor_Id;
                mspInsertLedger.VOUCHER_NO = p_VOUCHER_NO;
                mspInsertLedger.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                mspInsertLedger.DEBIT = p_Debit;
                mspInsertLedger.CREDIT = p_Credit;
                mspInsertLedger.LEDGER_DATE = p_Ledger_Date;
                mspInsertLedger.DOCUMENT_NO = p_Document_Id;
                mspInsertLedger.MANUAL_DOCUMENT_NO = p_Manual_Document_ID;
                mspInsertLedger.DOCUMENT_TYPE_ID = p_DocumentTypeId;
                mspInsertLedger.REMARKS = p_Remarks;
                mspInsertLedger.TIME_STAMP = p_TimeStamp;
                mspInsertLedger.CUSTOMER_ID = p_Customer_ID;
                mspInsertLedger.PRINCIPAL_ID = p_Principal;
                mspInsertLedger.CHEQUE_NO = p_Cheque_NO;
                mspInsertLedger.USER_ID = p_UserId;
                mspInsertLedger.SLIP_NO = SlipNo;
                mspInsertLedger.CHQUE_DATE = p_ChequeDate;
                mspInsertLedger.PAYMENT_MODE = p_PaymentMode;
                mspInsertLedger.PAYEE_NAME = p_PayeesName;  
                mspInsertLedger.POSTING = 0;
                mspInsertLedger.IS_DELETED = 0; 
                mspInsertLedger.ExecuteQuery();
                return mspInsertLedger.LEDGER_ID;
            }

            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        public long SelectVoucherNoByLedgerId(long p_ledger_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertLEDGER mspInsertLedger = new spInsertLEDGER();
                mspInsertLedger.Connection = mConnection;
                mspInsertLedger.LEDGER_ID1 = p_ledger_Id;
                DataTable dt = mspInsertLedger.ExecuteTableForVoucherByLedgerId();
                return Convert.ToInt64(dt.Rows[0]["VOUCHER_NO"].ToString());
            }

            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
                return 0;
            }

        }
        
        
        /// <summary>
        /// Updates Invoice Credit Amount After Realization
        /// </summary>
        /// <param name="p_SaleInvoice_ID">Invoice</param>
        /// <param name="p_Distributor_Id">Location</param>
        /// <param name="p_Current_Amount">Amount</param>
        public void UpdateSaleInvoice(long p_SaleInvoice_ID, int p_Distributor_Id,decimal p_Current_Amount)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateSALE_INVOICE_MASTER mSaleMaster = new spUpdateSALE_INVOICE_MASTER();
                mSaleMaster.Connection = mConnection;
                mSaleMaster.DISTRIBUTOR_ID = p_Distributor_Id;
                mSaleMaster.SALE_INVOICE_ID = p_SaleInvoice_ID;

                mSaleMaster.ExecuteQuery();
            }

            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
                //return null;

            }

        }
        public void UpdateLedgerInvoice(long p_SaleInvoice_ID, int p_Distributor_Id, decimal p_RealizeAmount,int p_DocumentTypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateLedgerInvoice mSaleMaster = new spUpdateLedgerInvoice();
                mSaleMaster.Connection = mConnection;
                mSaleMaster.RealizeAmount = p_RealizeAmount;
                mSaleMaster.SALE_INVOICE_ID = p_SaleInvoice_ID;
                mSaleMaster.DOCUMENT_TYPE_ID = p_DocumentTypeID;
                mSaleMaster.ExecuteQuery();
            }

            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
                //return null;

            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        
        //uPDATE  VENDOR lEDGER,gl MASTER DETAIL
        public void UpdateVendorLedger(int p_DistributorId, int p_Document_Type, long p_Voucher_no, string pRemarks, decimal TotalAmount, int p_TYPE)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspUpdateVendorLedger mSaleMaster = new UspUpdateVendorLedger();
                mSaleMaster.Connection = mConnection;
                mSaleMaster.Distributor_id = p_DistributorId;
                mSaleMaster.Document_Type_Id = p_Document_Type;
                mSaleMaster.VOUCHER_NO = p_Voucher_no;
                mSaleMaster.REMARKS = pRemarks;
                mSaleMaster.AMOUNT = TotalAmount;
                mSaleMaster.TYPE = p_TYPE;

                mSaleMaster.ExecuteQuery();
            }

            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
            }

        }

        public void UpdateVendorLedgerClaim(int pDistributor, int p_DOCUMENT_NO, string p_VOUCHER_NO, long p_ACCOUNT_HEAD_ID, decimal p_AMOUNT, string p_REMARKS, int p_ClaimType)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspUpdateVendorLedger mSaleMaster = new UspUpdateVendorLedger();
                mSaleMaster.Connection = mConnection;

                mSaleMaster.Distributor_id = pDistributor;
                mSaleMaster.DOCUMENT_NO = p_DOCUMENT_NO;
                mSaleMaster.GMVOUCHER_NO = p_VOUCHER_NO;
                mSaleMaster.ACCOUNT_HEAD_ID = p_ACCOUNT_HEAD_ID;
                mSaleMaster.AMOUNT = p_AMOUNT;
                mSaleMaster.REMARKS = p_REMARKS;
                mSaleMaster.TYPE = p_ClaimType;
                mSaleMaster.ExecuteQuery();
            }

            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
            }

        }

        /// <summary>
        /// Posts Voucher
        /// </summary>
        /// <param name="p_DistributorId">Location</param>
        /// <param name="p_Voucher_no">Voucher</param>
        /// <param name="p_Voucher_type_Id">VoucherType</param>
        /// <param name="p_TypeId">Type</param>
        /// <param name="p_VoucherDate">Date</param>
        public void PostSelectVoucher(int p_DistributorId,string p_Voucher_no,int p_Voucher_type_Id,int p_TypeId,DateTime p_VoucherDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                UspPostSelectVoucher mPostVoucher = new UspPostSelectVoucher();
                mPostVoucher.Connection = mConnection;
                mPostVoucher.DISTRIBUTOR_ID  = p_DistributorId;
                mPostVoucher.VOUCHER_NO = p_Voucher_no;
                mPostVoucher.VOUCHER_TYPE_ID = p_Voucher_type_Id;
                mPostVoucher.VOUCHER_DATE = p_VoucherDate;
                mPostVoucher.TYPE_ID = p_TypeId;  
                mPostVoucher.ExecuteQuery();
            }

            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
                throw;
            }
        }
        
        /// <summary>
        /// Inserts Voucher
        /// </summary>
        /// <remarks>
        /// Returns True On Success And False On Failure
        /// </remarks>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_PRINCIPAL_ID">Principal</param>
        /// <param name="p_VOUCHER_NO">Voucher</param>
        /// <param name="p_VOUCHER_TYPE_ID">VoucherType</param>
        /// <param name="p_VOUCHER_DATE">Date</param>
        /// <param name="p_PAYMENT_MODE">Mode</param>
        /// <param name="p_PayeesName">Payee</param>
        /// <param name="p_Remarks">Remarks</param>
        /// <param name="ChequeDate">ChequeDate</param>
        /// <param name="ChequeNo">ChequeNo</param>
        /// <param name="dtVoucherDetail">VoucherDataTable</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_SlipNo">Slip</param>
        /// <param name="pDueDate">DueDate</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool Edit_Voucher(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID, DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks, DateTime ChequeDate, string ChequeNo, DataTable dtVoucherDetail, int p_UserId, string p_SlipNo,DateTime pDueDate)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            
            try
            {
         
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spDeleteGL_MASTER mDelete = new spDeleteGL_MASTER();
                
                mDelete.Connection = mConnection;
                mDelete.Transaction = mTransaction;
                mDelete.DISTRIBUTOR_ID = p_Distributor_id;
                mDelete.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                mDelete.VOUCHER_NO = p_VOUCHER_NO;
                mDelete.ExecuteQuery();

                spInsertGL_MASTERVoucher mISom = new spInsertGL_MASTERVoucher();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;
                decimal MDebitAmount = 0;
                decimal MCreditAmount = 0;

                //------------Insert into Voucher Master----------
            
                if (dtVoucherDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.PRINCIPAL_ID = Constants.IntNullValue;
                    mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                    mISom.VOUCHER_NO = p_VOUCHER_NO;
                    mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                    mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                    mISom.CHEQUE_DATE = ChequeDate;
                    mISom.CHEQUE_NO = ChequeNo;  
                    mISom.PAYEES_NAME = p_PayeesName;
                    mISom.REMARKS = p_Remarks;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.USER_ID = p_UserId;
                    mISom.Slip_No = p_SlipNo; 
                    mISom.IS_POSTED = false;
                    mISom.IS_DELETED = false;
                    mISom.DUE_DATE = pDueDate;
                    mISom.ExecuteQuery();


                    //----------------Insert into Voucher detail-------------
                    spInsertGL_DETAILVoucher mVoucherDetail = new spInsertGL_DETAILVoucher();
                    mVoucherDetail.Connection = mConnection;
                    mVoucherDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtVoucherDetail.Rows)
                    {
                        mVoucherDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mVoucherDetail.PRINCIPAL_ID = int.Parse(dr["PRINCIPAL_ID"].ToString());
                        mVoucherDetail.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                        mVoucherDetail.VOUCHER_NO = p_VOUCHER_NO;
                        mVoucherDetail.ACCOUNT_HEAD_ID = long.Parse(dr["ACCOUNT_HEAD_ID"].ToString());
                        mVoucherDetail.DEBIT = decimal.Parse(dr["DEBIT"].ToString());
                        mVoucherDetail.CREDIT = decimal.Parse(dr["CREDIT"].ToString());
                        mVoucherDetail.GL_REMARKS = dr["REMARKS"].ToString();
                        mVoucherDetail.IS_DELETED = false;
                        mVoucherDetail.ExecuteQuery();

                        MDebitAmount += decimal.Parse(dr["DEBIT"].ToString());
                        MCreditAmount += decimal.Parse(dr["CREDIT"].ToString());
                    }
        
                }

                if (MDebitAmount > 0 && MCreditAmount > 0 && MDebitAmount == MCreditAmount)
                {
                    spUpdateChequeStatus mdtCheck1 = new spUpdateChequeStatus();
                    mdtCheck1.Connection = mConnection;
                    mdtCheck1.Transaction = mTransaction;
                    mdtCheck1.STATUS = 529;
                    mdtCheck1.CHEQUE_NO = ChequeNo;
                    mdtCheck1.TYPEID = 1;
                    mdtCheck1.ExecuteQuery();

                    mTransaction.Commit();
                    return true;
                }
                else
                {
                    mTransaction.Rollback();
                    return false;
                }
            }
            catch (Exception exp)
            {
                mTransaction.Rollback(); 
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
        public bool AddVoucherCashSkimming(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID, DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks, DateTime ChequeDate, string ChequeNo, DataTable dtVoucherDetail, int p_UserId, string p_SlipNo, DateTime pDueDate, int p_INVOICE_TYPE, long p_INVOICE_ID)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spInsertGL_MASTERVoucher mISom = new spInsertGL_MASTERVoucher();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;
                decimal MDebitAmount = 0;
                decimal MCreditAmount = 0;
                //------------Insert into Voucher Master----------

                if (dtVoucherDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.PRINCIPAL_ID = Constants.IntNullValue;
                    mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                    mISom.VOUCHER_NO = p_VOUCHER_NO;
                    mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                    mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                    mISom.CHEQUE_DATE = ChequeDate;
                    mISom.CHEQUE_NO = ChequeNo;
                    mISom.PAYEES_NAME = p_PayeesName;
                    mISom.REMARKS = p_Remarks;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.USER_ID = p_UserId;
                    mISom.Slip_No = p_SlipNo;
                    mISom.IS_POSTED = false;
                    mISom.IS_DELETED = false;
                    mISom.DUE_DATE = pDueDate;
                    mISom.INVOICE_TYPE = p_INVOICE_TYPE;
                    mISom.INVOICE_ID = p_INVOICE_ID;
                    mISom.ExecuteQuery();


                    //----------------Insert into Voucher detail-------------
                    spInsertGL_DETAILVoucher mVoucherDetail = new spInsertGL_DETAILVoucher();
                    mVoucherDetail.Connection = mConnection;
                    mVoucherDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtVoucherDetail.Rows)
                    {
                        mVoucherDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mVoucherDetail.PRINCIPAL_ID = int.Parse(dr["PRINCIPAL_ID"].ToString());
                        mVoucherDetail.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                        mVoucherDetail.VOUCHER_NO = p_VOUCHER_NO;
                        mVoucherDetail.ACCOUNT_HEAD_ID = long.Parse(dr["ACCOUNT_HEAD_ID"].ToString());
                        mVoucherDetail.DEBIT = decimal.Parse(dr["DEBIT"].ToString());
                        mVoucherDetail.CREDIT = decimal.Parse(dr["CREDIT"].ToString());
                        mVoucherDetail.GL_REMARKS = dr["REMARKS"].ToString();
                        mVoucherDetail.IS_DELETED = false;
                        mVoucherDetail.ExecuteQuery();

                        MDebitAmount += decimal.Parse(dr["DEBIT"].ToString());
                        MCreditAmount += decimal.Parse(dr["CREDIT"].ToString());
                    }

                }

                if (MDebitAmount > 0 && MCreditAmount > 0 && MDebitAmount == MCreditAmount)
                {
                    spUpdateChequeStatus mdtCheck1 = new spUpdateChequeStatus();
                    mdtCheck1.Connection = mConnection;
                    mdtCheck1.Transaction = mTransaction;
                    mdtCheck1.STATUS = 529;
                    mdtCheck1.CHEQUE_NO = ChequeNo;
                    mdtCheck1.TYPEID = 1;
                    mdtCheck1.ExecuteQuery();

                    mTransaction.Commit();
                    return true;
                }
                else
                {
                    mTransaction.Rollback();
                    return false;
                }
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
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
        public bool AddVoucher(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID, DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks, DateTime ChequeDate, string ChequeNo, DataTable dtVoucherDetail, int p_UserId, string p_SlipNo, DateTime pDueDate)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);                
                spInsertGL_MASTERVoucher mISom = new spInsertGL_MASTERVoucher();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;
                decimal MDebitAmount = 0;
                decimal MCreditAmount = 0;
                //------------Insert into Voucher Master----------

                if (dtVoucherDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.PRINCIPAL_ID = Constants.IntNullValue;
                    mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                    mISom.VOUCHER_NO = p_VOUCHER_NO;
                    mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                    mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                    mISom.CHEQUE_DATE = ChequeDate;
                    mISom.CHEQUE_NO = ChequeNo;
                    mISom.PAYEES_NAME = p_PayeesName;
                    mISom.REMARKS = p_Remarks;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.USER_ID = p_UserId;
                    mISom.Slip_No = p_SlipNo;
                    mISom.IS_POSTED = false;
                    mISom.IS_DELETED = false;
                    mISom.DUE_DATE = pDueDate;
                    mISom.ExecuteQuery();


                    //----------------Insert into Voucher detail-------------
                    spInsertGL_DETAILVoucher mVoucherDetail = new spInsertGL_DETAILVoucher();
                    mVoucherDetail.Connection = mConnection;
                    mVoucherDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtVoucherDetail.Rows)
                    {
                        mVoucherDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mVoucherDetail.PRINCIPAL_ID = int.Parse(dr["PRINCIPAL_ID"].ToString());
                        mVoucherDetail.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                        mVoucherDetail.VOUCHER_NO = p_VOUCHER_NO;
                        mVoucherDetail.ACCOUNT_HEAD_ID = long.Parse(dr["ACCOUNT_HEAD_ID"].ToString());
                        mVoucherDetail.DEBIT = decimal.Parse(dr["DEBIT"].ToString());
                        mVoucherDetail.CREDIT = decimal.Parse(dr["CREDIT"].ToString());
                        mVoucherDetail.GL_REMARKS = dr["REMARKS"].ToString();
                        mVoucherDetail.IS_DELETED = false;
                        mVoucherDetail.ExecuteQuery();

                        MDebitAmount += decimal.Parse(dr["DEBIT"].ToString());
                        MCreditAmount += decimal.Parse(dr["CREDIT"].ToString());
                    }

                }

                if (MDebitAmount > 0 && MCreditAmount > 0 && MDebitAmount == MCreditAmount)
                {
                    spUpdateChequeStatus mdtCheck1 = new spUpdateChequeStatus();
                    mdtCheck1.Connection = mConnection;
                    mdtCheck1.Transaction = mTransaction;
                    mdtCheck1.STATUS = 529;
                    mdtCheck1.CHEQUE_NO = ChequeNo;
                    mdtCheck1.TYPEID = 1;
                    mdtCheck1.ExecuteQuery();

                    mTransaction.Commit();
                    return true;
                }
                else
                {
                    mTransaction.Rollback();
                    return false;
                }
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
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
        public bool AddVoucherNew(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID, DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks, DateTime ChequeDate, string ChequeNo, DataTable dtVoucherDetail, int p_UserId, string p_SlipNo, DateTime pDueDate,string p_UniqueID)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spInsertGL_MASTERVoucherNew mISom = new spInsertGL_MASTERVoucherNew();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;
                decimal MDebitAmount = 0;
                decimal MCreditAmount = 0;
                //------------Insert into Voucher Master----------

                if (dtVoucherDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.PRINCIPAL_ID = Constants.IntNullValue;
                    mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                    mISom.VOUCHER_NO = p_VOUCHER_NO;
                    mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                    mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                    mISom.CHEQUE_DATE = ChequeDate;
                    mISom.CHEQUE_NO = ChequeNo;
                    mISom.PAYEES_NAME = p_PayeesName;
                    mISom.REMARKS = p_Remarks;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.USER_ID = p_UserId;
                    mISom.Slip_No = p_SlipNo;
                    mISom.IS_POSTED = false;
                    mISom.IS_DELETED = false;
                    mISom.DUE_DATE = pDueDate;
                    mISom.UniqueID = p_UniqueID;
                    mISom.ExecuteQuery();


                    //----------------Insert into Voucher detail-------------
                    spInsertGL_DETAILVoucher mVoucherDetail = new spInsertGL_DETAILVoucher();
                    mVoucherDetail.Connection = mConnection;
                    mVoucherDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtVoucherDetail.Rows)
                    {
                        mVoucherDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mVoucherDetail.PRINCIPAL_ID = int.Parse(dr["PRINCIPAL_ID"].ToString());
                        mVoucherDetail.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                        mVoucherDetail.VOUCHER_NO = p_VOUCHER_NO;
                        mVoucherDetail.ACCOUNT_HEAD_ID = long.Parse(dr["ACCOUNT_HEAD_ID"].ToString());
                        mVoucherDetail.DEBIT = decimal.Parse(dr["DEBIT"].ToString());
                        mVoucherDetail.CREDIT = decimal.Parse(dr["CREDIT"].ToString());
                        mVoucherDetail.GL_REMARKS = dr["REMARKS"].ToString();
                        mVoucherDetail.IS_DELETED = false;
                        mVoucherDetail.ExecuteQuery();

                        MDebitAmount += decimal.Parse(dr["DEBIT"].ToString());
                        MCreditAmount += decimal.Parse(dr["CREDIT"].ToString());
                    }

                }

                if (MDebitAmount > 0 && MCreditAmount > 0 && MDebitAmount == MCreditAmount)
                {
                    spUpdateChequeStatus mdtCheck1 = new spUpdateChequeStatus();
                    mdtCheck1.Connection = mConnection;
                    mdtCheck1.Transaction = mTransaction;
                    mdtCheck1.STATUS = 529;
                    mdtCheck1.CHEQUE_NO = ChequeNo;
                    mdtCheck1.TYPEID = 1;
                    mdtCheck1.ExecuteQuery();

                    mTransaction.Commit();
                    return true;
                }
                else
                {
                    mTransaction.Rollback();
                    return false;
                }
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
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
        /// Inserts Voucher
        /// </summary>
        /// <remarks>
        /// Returns True On Success And False On Failure
        /// </remarks>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_PRINCIPAL_ID">Principal</param>
        /// <param name="p_VOUCHER_NO">No</param>
        /// <param name="p_VOUCHER_TYPE_ID">VoucherType</param>
        /// <param name="p_VOUCHER_DATE">Date</param>
        /// <param name="p_PAYMENT_MODE">Mode</param>
        /// <param name="p_PayeesName">Payee</param>
        /// <param name="p_Remarks">Remarks</param>
        /// <param name="ChequeDate">ChequeDate</param>
        /// <param name="ChequeNo">ChequeNo</param>
        /// <param name="dtVoucherDetail">VoucherDataTable</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <param name="p_SlipNo">Slip</param>
        /// <returns>True On Success And False On Failure</returns>
        public bool Add_Voucher(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID,
            DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks,
            DateTime ChequeDate, string ChequeNo, DataTable dtVoucherDetail, int p_UserId, string p_SlipNo)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;

            try
            {

                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                
                spInsertGL_MASTER mISom = new spInsertGL_MASTER();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;
                decimal MDebitAmount = 0;
                decimal MCreditAmount = 0;

                //------------Insert into Sale Order Master----------

                if (dtVoucherDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                    mISom.VOUCHER_NO = p_VOUCHER_NO;
                    mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                    mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                    mISom.CHEQUE_DATE = ChequeDate;
                    mISom.CHEQUE_NO = ChequeNo;
                    mISom.PAYEES_NAME = p_PayeesName;
                    mISom.REMARKS = p_Remarks;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.USER_ID = p_UserId;
                    mISom.Slip_No = p_SlipNo;
                    mISom.IS_POSTED = false;
                    mISom.IS_DELETED = false;
                    mISom.DUE_DATE = Constants.DateNullValue;                    
                    mISom.ExecuteQuery();


                    //----------------Insert into sale order detail-------------
                    spInsertGL_DETAIL mVoucherDetail = new spInsertGL_DETAIL();
                    mVoucherDetail.Connection = mConnection;
                    mVoucherDetail.Transaction = mTransaction;



                    foreach (DataRow dr in dtVoucherDetail.Rows)
                    {
                        mVoucherDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mVoucherDetail.PRINCIPAL_ID = int.Parse(dr["PRINCIPAL_ID"].ToString());
                        mVoucherDetail.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                        mVoucherDetail.VOUCHER_NO = p_VOUCHER_NO;
                        mVoucherDetail.ACCOUNT_HEAD_ID = long.Parse(dr["ACCOUNT_HEAD_ID"].ToString());
                        mVoucherDetail.DEBIT = decimal.Parse(dr["DEBIT"].ToString());
                        mVoucherDetail.CREDIT = decimal.Parse(dr["CREDIT"].ToString());
                        mVoucherDetail.GL_REMARKS = dr["REMARKS"].ToString();
                        mVoucherDetail.IS_DELETED = false;
                        mVoucherDetail.ExecuteQuery();

                        MDebitAmount += decimal.Parse(dr["DEBIT"].ToString());
                        MCreditAmount += decimal.Parse(dr["CREDIT"].ToString());
                    }
                }

                if (MDebitAmount > 0 && MCreditAmount > 0 && MDebitAmount == MCreditAmount)
                {
                    mTransaction.Commit();
                    return true;
                }
                else
                {
                    mTransaction.Rollback();
                    return false;
                }
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
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

        public bool Add_VoucherUpdateExpense(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID,
            DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks,
            DateTime ChequeDate, string ChequeNo, DataTable dtVoucherDetail, int p_UserId, string p_SlipNo)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;

            try
            {

                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spDeleteGL_MASTER mDelete = new spDeleteGL_MASTER();

                mDelete.Connection = mConnection;
                mDelete.Transaction = mTransaction;
                mDelete.DISTRIBUTOR_ID = p_Distributor_id;
                mDelete.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                mDelete.VOUCHER_NO = p_VOUCHER_NO;
                mDelete.ExecuteQuery();

                spInsertGL_MASTER mISom = new spInsertGL_MASTER();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;
                decimal MDebitAmount = 0;
                decimal MCreditAmount = 0;

                //------------Insert into Sale Order Master----------

                if (dtVoucherDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                    mISom.VOUCHER_NO = p_VOUCHER_NO;
                    mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                    mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                    mISom.CHEQUE_DATE = ChequeDate;
                    mISom.CHEQUE_NO = ChequeNo;
                    mISom.PAYEES_NAME = p_PayeesName;
                    mISom.REMARKS = p_Remarks;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.USER_ID = p_UserId;
                    mISom.Slip_No = p_SlipNo;
                    mISom.IS_POSTED = false;
                    mISom.IS_DELETED = false;
                    mISom.DUE_DATE = Constants.DateNullValue;
                    mISom.ExecuteQuery();


                    //----------------Insert into sale order detail-------------
                    spInsertGL_DETAIL mVoucherDetail = new spInsertGL_DETAIL();
                    mVoucherDetail.Connection = mConnection;
                    mVoucherDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtVoucherDetail.Rows)
                    {
                        mVoucherDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mVoucherDetail.PRINCIPAL_ID = int.Parse(dr["PRINCIPAL_ID"].ToString());
                        mVoucherDetail.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                        mVoucherDetail.VOUCHER_NO = p_VOUCHER_NO;
                        mVoucherDetail.ACCOUNT_HEAD_ID = long.Parse(dr["ACCOUNT_HEAD_ID"].ToString());
                        mVoucherDetail.DEBIT = decimal.Parse(dr["DEBIT"].ToString());
                        mVoucherDetail.CREDIT = decimal.Parse(dr["CREDIT"].ToString());
                        mVoucherDetail.GL_REMARKS = dr["REMARKS"].ToString();
                        mVoucherDetail.IS_DELETED = false;
                        mVoucherDetail.ExecuteQuery();

                        MDebitAmount += decimal.Parse(dr["DEBIT"].ToString());
                        MCreditAmount += decimal.Parse(dr["CREDIT"].ToString());
                    }
                }

                if (MDebitAmount > 0 && MCreditAmount > 0 && MDebitAmount == MCreditAmount)
                {
                    mTransaction.Commit();
                    return true;
                }
                else
                {
                    mTransaction.Rollback();
                    return false;
                }
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
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

        public bool Add_Voucher(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID,
            DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks,
            DateTime ChequeDate, string ChequeNo, DataTable dtVoucherDetail, int p_UserId,
            string p_SlipNo, DateTime pDueDate, bool pIsPost,int p_INVOICE_TYPE, long p_voucherNo)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);

                spInsertGL_MASTER mISom = new spInsertGL_MASTER();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;
                decimal MDebitAmount = 0;
                decimal MCreditAmount = 0;

                //------------Insert into Sale Order Master----------

                if (dtVoucherDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                    mISom.VOUCHER_NO = p_VOUCHER_NO;
                    mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                    mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                    mISom.CHEQUE_DATE = ChequeDate;
                    mISom.CHEQUE_NO = ChequeNo;
                    mISom.PAYEES_NAME = p_PayeesName;
                    mISom.REMARKS = p_Remarks;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.USER_ID = p_UserId;
                    mISom.Slip_No = p_SlipNo;
                    mISom.IS_DELETED = false;
                    mISom.DUE_DATE = pDueDate;
                    mISom.IS_POSTED = pIsPost;
                    mISom.INVOICE_TYPE = p_INVOICE_TYPE;
                    mISom.INVOICE_ID = p_voucherNo;          
                    mISom.ExecuteQuery();

                    //----------------Insert into sale order detail-------------
                    spInsertGL_DETAIL mVoucherDetail = new spInsertGL_DETAIL();
                    mVoucherDetail.Connection = mConnection;
                    mVoucherDetail.Transaction = mTransaction;

                    foreach (DataRow dr in dtVoucherDetail.Rows)
                    {
                        mVoucherDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mVoucherDetail.PRINCIPAL_ID = int.Parse(dr["PRINCIPAL_ID"].ToString());
                        mVoucherDetail.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                        mVoucherDetail.VOUCHER_NO = p_VOUCHER_NO;
                        mVoucherDetail.ACCOUNT_HEAD_ID = long.Parse(dr["ACCOUNT_HEAD_ID"].ToString());
                        mVoucherDetail.DEBIT = decimal.Parse(dr["DEBIT"].ToString());
                        mVoucherDetail.CREDIT = decimal.Parse(dr["CREDIT"].ToString());
                        mVoucherDetail.GL_REMARKS = dr["REMARKS"].ToString();
                        mVoucherDetail.IS_DELETED = false;
                        mVoucherDetail.ExecuteQuery();

                        MDebitAmount += decimal.Parse(dr["DEBIT"].ToString());
                        MCreditAmount += decimal.Parse(dr["CREDIT"].ToString());
                    }
                }
                
                mTransaction.Commit();
                return true;
            }

            catch (Exception exp)
            {
                mTransaction.Rollback();
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

        public bool Add_Voucher2(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID, DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks, DateTime ChequeDate, string ChequeNo, long p_INVOICE_ID, int p_INVOICE_TYPE, DataTable dtVoucherDetail, int p_UserId, string p_SlipNo, DateTime pDueDate)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spInsertGL_MASTER2 mISom = new spInsertGL_MASTER2();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;
                decimal MDebitAmount = 0;
                decimal MCreditAmount = 0;

                //------------Insert into Sale Order Master----------

                if (dtVoucherDetail.Rows.Count > 0)
                {
                    mISom.DISTRIBUTOR_ID = p_Distributor_id;
                    mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                    mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                    mISom.VOUCHER_NO = p_VOUCHER_NO;
                    mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                    mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                    mISom.CHEQUE_DATE = ChequeDate;
                    mISom.CHEQUE_NO = ChequeNo;
                    mISom.PAYEES_NAME = p_PayeesName;
                    mISom.REMARKS = p_Remarks;
                    mISom.TIME_STAMP = DateTime.Now;
                    mISom.USER_ID = p_UserId;
                    mISom.Slip_No = p_SlipNo;
                    mISom.IS_POSTED = false;
                    mISom.IS_DELETED = false;
                    mISom.DUE_DATE = pDueDate;
                    mISom.INVOICE_ID = p_INVOICE_ID;
                    mISom.INVOICE_TYPE = p_INVOICE_TYPE;
                    mISom.ExecuteQuery();


                    //----------------Insert into sale order detail-------------
                    spInsertGL_DETAIL mVoucherDetail = new spInsertGL_DETAIL();
                    mVoucherDetail.Connection = mConnection;
                    mVoucherDetail.Transaction = mTransaction;



                    foreach (DataRow dr in dtVoucherDetail.Rows)
                    {
                        mVoucherDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mVoucherDetail.PRINCIPAL_ID = int.Parse(dr["PRINCIPAL_ID"].ToString());
                        mVoucherDetail.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                        mVoucherDetail.VOUCHER_NO = p_VOUCHER_NO;
                        mVoucherDetail.ACCOUNT_HEAD_ID = long.Parse(dr["ACCOUNT_HEAD_ID"].ToString());
                        mVoucherDetail.DEBIT = decimal.Parse(dr["DEBIT"].ToString());
                        mVoucherDetail.CREDIT = decimal.Parse(dr["CREDIT"].ToString());
                        mVoucherDetail.GL_REMARKS = dr["REMARKS"].ToString();
                        mVoucherDetail.IS_DELETED = false;
                        mVoucherDetail.ExecuteQuery();

                        MDebitAmount += decimal.Parse(dr["DEBIT"].ToString());
                        MCreditAmount += decimal.Parse(dr["CREDIT"].ToString());
                    }

                }

                if (MDebitAmount > 0 && MCreditAmount > 0 && MDebitAmount == MCreditAmount)
                {
                    mTransaction.Commit();
                    return true;
                }
                else
                {
                    mTransaction.Rollback();
                    return false;
                }
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
                ExceptionPublisher.PublishException(exp);
                return false;
            }

        }

        /// <summary>
        /// Inserts Voucher
        /// </summary>
        /// <param name="p_Distributor_id">Location</param>
        /// <param name="p_PRINCIPAL_ID">Principal</param>
        /// <param name="p_VOUCHER_NO">No</param>
        /// <param name="p_VOUCHER_TYPE_ID">VoucherType</param>
        /// <param name="p_VOUCHER_DATE">Date</param>
        /// <param name="p_ChequeDate">ChequeDate</param>
        /// <param name="p_PAYMENT_MODE">Mode</param>
        /// <param name="p_PayeesName">Payee</param>
        /// <param name="p_Remarks">Remarks</param>
        /// <param name="FileName">File</param>
        /// <param name="p_UserId">InsertedBy</param>
        /// <returns>int</returns>
        public int  Import_Voucher(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID, DateTime p_VOUCHER_DATE,DateTime p_ChequeDate, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks,string FileName,int p_UserId)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            FileStream Sourcefile = null;
            StreamReader ReadSourceFile = null;
            DataControl dc = new DataControl();
            int pReturnCode = 0;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                
                spInsertGL_MASTER mISom = new spInsertGL_MASTER();

                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;

                mISom.DISTRIBUTOR_ID = p_Distributor_id;
                mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                mISom.VOUCHER_NO = p_VOUCHER_NO;
                mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                mISom.CHEQUE_DATE = Constants.DateNullValue ;
                mISom.CHEQUE_NO = null;
                mISom.PAYEES_NAME = null;
                mISom.REMARKS = p_Remarks;
                mISom.TIME_STAMP = DateTime.Now;
                mISom.USER_ID = p_UserId;
                mISom.IS_POSTED = false;
                mISom.IS_DELETED = false;
                mISom.ExecuteQuery();

                Sourcefile = new FileStream(FileName, FileMode.Open);
                ReadSourceFile = new StreamReader(Sourcefile);
                string FileContents = "";
                
                
                while ((FileContents = ReadSourceFile.ReadLine()) != null)
                {
                    string[] ParametersArr = FileContents.Split(Constants.File_Delimiter);

                    spSelectACCOUNT_HEAD Selectdata = new spSelectACCOUNT_HEAD();
                    Selectdata.Connection = mConnection;
                    Selectdata.Transaction = mTransaction;
                    Selectdata.ACCOUNT_CODE = ParametersArr[0].ToString();
                    Selectdata.IS_ACTIVE = true;
                    Selectdata.TIME_STAMP = Constants.DateNullValue;
                    Selectdata.LASTUPDATE_DATE = Constants.DateNullValue;   
                    DataTable  AccountHead  = Selectdata.ExecuteTable();
                    pReturnCode += 1;
                    if (AccountHead.Rows.Count > 0)
                    {
                        spInsertGL_DETAIL mVoucherDetail = new spInsertGL_DETAIL();
                        mVoucherDetail.Connection = mConnection;
                        mVoucherDetail.Transaction = mTransaction;

                        mVoucherDetail.ACCOUNT_HEAD_ID = long.Parse(AccountHead.Rows[0][0].ToString());
                        mVoucherDetail.DEBIT = decimal.Parse(dc.chkNull_0(ParametersArr[2].ToString()));
                        mVoucherDetail.CREDIT = decimal.Parse(dc.chkNull_0(ParametersArr[3].ToString()));
                        mVoucherDetail.DISTRIBUTOR_ID = p_Distributor_id;
                        mVoucherDetail.PRINCIPAL_ID = p_PRINCIPAL_ID;
                        mVoucherDetail.VOUCHER_NO = p_VOUCHER_NO;
                        mVoucherDetail.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                        mVoucherDetail.IS_DELETED = false;
                        mVoucherDetail.GL_REMARKS = ParametersArr[1].ToString();
                        mVoucherDetail.ExecuteQuery();
                        
                    }
                    else
                    {
                        mTransaction.Rollback();
                        return pReturnCode; 
  
                    }
                    
                }
                mTransaction.Commit();
                return pReturnCode = 0; 
                            
                
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
                ExceptionPublisher.PublishException(exp);
                return pReturnCode;
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
        public void UpdatePurchaseMaster(long p_INVOICE_ID, int p_Distributor_Id, decimal p_Current_Amount)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdatePURCHASE_MASTER mSaleMaster = new spUpdatePURCHASE_MASTER();
                mSaleMaster.Connection = mConnection;
                mSaleMaster.DISTRIBUTOR_ID = p_Distributor_Id;
                mSaleMaster.PURCHASE_MASTER_ID = p_INVOICE_ID;
                mSaleMaster.DEBIT_AMOUNT = p_Current_Amount;

                mSaleMaster.ExecuteQuery();
            }

            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
                //return null;

            }

        }

        public bool PostingGLMaster(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID, DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks, int p_UserId, string pDocumentType,int p_INVOICE_TYPE,long p_INVOICE_ID, IDbTransaction mTransaction, IDbConnection mConnection)
        {
            try
            {
                spInsertGL_MASTER mISom = new spInsertGL_MASTER();
                mISom.Connection = mConnection;
                mISom.Transaction = mTransaction;

                //------------Insert into GL Master----------

                mISom.DISTRIBUTOR_ID = p_Distributor_id;
                mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                mISom.VOUCHER_NO = p_VOUCHER_NO;
                mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                mISom.CHEQUE_DATE = Constants.DateNullValue;
                mISom.CHEQUE_NO = null;
                mISom.PAYEES_NAME = p_PayeesName;
                mISom.REMARKS = p_Remarks;
                mISom.TIME_STAMP = DateTime.Now;
                mISom.USER_ID = p_UserId;
                mISom.Slip_No = pDocumentType;
                mISom.IS_POSTED = true;
                mISom.IS_DELETED = false;
                mISom.INVOICE_TYPE = p_INVOICE_TYPE;
                mISom.INVOICE_ID = p_INVOICE_ID;
                mISom.DUE_DATE = Constants.DateNullValue;
                return mISom.ExecuteQuery();
            }

            catch (Exception exp)
            {
                 ExceptionPublisher.PublishException(exp);
                throw;
            }

        }
        public bool PostingGLMaster(int p_Distributor_id, int p_PRINCIPAL_ID, string p_VOUCHER_NO, int p_VOUCHER_TYPE_ID, DateTime p_VOUCHER_DATE, int p_PAYMENT_MODE, string p_PayeesName, string p_Remarks, int p_UserId, string pDocumentType, int p_INVOICE_TYPE, long p_INVOICE_ID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertGL_MASTER mISom = new spInsertGL_MASTER();
                mISom.Connection = mConnection;

                //------------Insert into GL Master----------

                mISom.DISTRIBUTOR_ID = p_Distributor_id;
                mISom.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mISom.VOUCHER_TYPE_ID = p_VOUCHER_TYPE_ID;
                mISom.VOUCHER_NO = p_VOUCHER_NO;
                mISom.VOUCHER_DATE = p_VOUCHER_DATE;
                mISom.PAYMENT_MODE = p_PAYMENT_MODE;
                mISom.CHEQUE_DATE = Constants.DateNullValue;
                mISom.CHEQUE_NO = null;
                mISom.PAYEES_NAME = p_PayeesName;
                mISom.REMARKS = p_Remarks;
                mISom.TIME_STAMP = DateTime.Now;
                mISom.USER_ID = p_UserId;
                mISom.Slip_No = pDocumentType;
                mISom.IS_POSTED = true;
                mISom.IS_DELETED = false;
                mISom.INVOICE_TYPE = p_INVOICE_TYPE;
                mISom.INVOICE_ID = p_INVOICE_ID;
                mISom.DUE_DATE = Constants.DateNullValue;
                return mISom.ExecuteQuery();
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
        public void PostingGLDetail(int pDistributorId, int pPrincpald, int pVoucherType, string pVoucherNo, long pAccountId, decimal pDebit, decimal pCredit, string pRemarks, IDbTransaction mTransaction, IDbConnection mConnection)
        {

            try
            {
                spInsertGL_DETAIL mISom2 = new spInsertGL_DETAIL();
                mISom2.Connection = mConnection;
                mISom2.Transaction = mTransaction;

                mISom2.DISTRIBUTOR_ID = pDistributorId;
                mISom2.PRINCIPAL_ID = pPrincpald;
                mISom2.VOUCHER_TYPE_ID = pVoucherType;
                mISom2.VOUCHER_NO = pVoucherNo;
                mISom2.ACCOUNT_HEAD_ID = pAccountId;
                mISom2.DEBIT = pDebit;
                mISom2.CREDIT = pCredit;
                mISom2.GL_REMARKS = pRemarks;
                mISom2.IS_DELETED = false;

                mISom2.ExecuteQuery();

            }

            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw;
            }
        }
        public bool PostingGLDetail(int pDistributorId, int pPrincpald, int pVoucherType, string pVoucherNo, long pAccountId, decimal pDebit, decimal pCredit, string pRemarks)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertGL_DETAIL mISom2 = new spInsertGL_DETAIL();
                mISom2.Connection = mConnection;

                mISom2.DISTRIBUTOR_ID = pDistributorId;
                mISom2.PRINCIPAL_ID = pPrincpald;
                mISom2.VOUCHER_TYPE_ID = pVoucherType;
                mISom2.VOUCHER_NO = pVoucherNo;
                mISom2.ACCOUNT_HEAD_ID = pAccountId;
                mISom2.DEBIT = pDebit;
                mISom2.CREDIT = pCredit;
                mISom2.GL_REMARKS = pRemarks;
                mISom2.IS_DELETED = false;
                mISom2.ExecuteQuery();
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

        public bool PostingPrinvipalInvoiceAccount(int p_VoucherType, long p_VoucherNo, long p_ACCOUNT_HEAD_ID, int p_Distributor_Id, decimal p_Debit, decimal p_Credit, DateTime p_Ledger_Date, string p_Remarks, DateTime p_TimeStamp, int p_PRINCIPAL_ID, long p_DOCUMENT_NO, string p_MANUAL_DOCUMENT_NO, int p_DOCUMENT_TYPE_ID, int p_UserId, IDbTransaction mTransaction, IDbConnection mConnection, int p_Paymode, string p_PayeesName)
        {

            try
            {

                spInsertVENDER_LEDGER mspInsertLedger = new spInsertVENDER_LEDGER();
                mspInsertLedger.Connection = mConnection;
                mspInsertLedger.Transaction = mTransaction;
                mspInsertLedger.VOUCHER_TYPE_ID = p_VoucherType;
                mspInsertLedger.VOUCHER_NO = p_VoucherNo;
                mspInsertLedger.ACCOUNT_HEAD_ID = p_ACCOUNT_HEAD_ID;
                mspInsertLedger.DISTRIBUTOR_ID = p_Distributor_Id;
                mspInsertLedger.DEBIT = p_Debit;
                mspInsertLedger.CREDIT = p_Credit;
                mspInsertLedger.LEDGER_DATE = p_Ledger_Date;
                mspInsertLedger.REMARKS = p_Remarks;
                mspInsertLedger.TIME_STAMP = p_TimeStamp;
                mspInsertLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;

                mspInsertLedger.DOCUMENT_TYPE_ID = p_DOCUMENT_TYPE_ID;
                mspInsertLedger.DOCUMENT_NO = p_DOCUMENT_NO;
                mspInsertLedger.MANUAL_DOCUMENT_NO = p_MANUAL_DOCUMENT_NO;
                mspInsertLedger.USER_ID = p_UserId;
                mspInsertLedger.PAYMENT_MODE = p_Paymode;
                mspInsertLedger.PAYEE_NAME = p_PayeesName;
                mspInsertLedger.IS_DELETED = 0;
                mspInsertLedger.ExecuteQuery();
                return true;
            }

            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
                throw;

            }

        }

        public long PostingPrinvipalInvoiceAccount(int p_VoucherType, long p_VoucherNo, long p_ACCOUNT_HEAD_ID, int p_Distributor_Id, decimal p_Debit, decimal p_Credit, DateTime p_Ledger_Date, string p_Remarks, DateTime p_TimeStamp, int p_PRINCIPAL_ID, int p_DOCUMENT_TYPE_ID, string pChequeNo, int p_UserId, long p_DOCUMENT_NO, string p_MANUAL_DOCUMENT_NO, int p_Paymode, string pSlipNo, string p_PayeesName, DateTime pChqDate,bool p_IsAdvance)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spInsertVENDER_LEDGER mspInsertLedger = new spInsertVENDER_LEDGER();
                mspInsertLedger.Connection = mConnection;
                mspInsertLedger.VOUCHER_TYPE_ID = p_VoucherType;
                mspInsertLedger.VOUCHER_NO = p_VoucherNo;
                mspInsertLedger.ACCOUNT_HEAD_ID = p_ACCOUNT_HEAD_ID;
                mspInsertLedger.DISTRIBUTOR_ID = p_Distributor_Id;
                mspInsertLedger.DEBIT = p_Debit;
                mspInsertLedger.CREDIT = p_Credit;
                mspInsertLedger.LEDGER_DATE = p_Ledger_Date;
                mspInsertLedger.REMARKS = p_Remarks;
                mspInsertLedger.TIME_STAMP = p_TimeStamp;
                mspInsertLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mspInsertLedger.DOCUMENT_TYPE_ID = p_DOCUMENT_TYPE_ID;
                mspInsertLedger.DOCUMENT_NO = p_DOCUMENT_NO;
                mspInsertLedger.MANUAL_DOCUMENT_NO = p_MANUAL_DOCUMENT_NO;
                mspInsertLedger.USER_ID = p_UserId;
                mspInsertLedger.PAYMENT_MODE = p_Paymode;
                mspInsertLedger.PAYEE_NAME = p_PayeesName;
                mspInsertLedger.CHEQUE_NO = pChequeNo;
                mspInsertLedger.SLIP_NO = pSlipNo;
                mspInsertLedger.IS_DELETED = 0;
                mspInsertLedger.CHQUE_DATE = pChqDate;
                mspInsertLedger.IS_ADVANCE = p_IsAdvance;
                mspInsertLedger.ExecuteQuery();
                return mspInsertLedger.LEDGER_ID;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw exp;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }

        }

        public bool PostingPrinvipalInvoiceAccount(int p_VoucherType, long p_VoucherNo, long p_ACCOUNT_HEAD_ID, int p_Distributor_Id, decimal p_Debit, decimal p_Credit, DateTime p_Ledger_Date, string p_Remarks, DateTime p_TimeStamp, int p_PRINCIPAL_ID, long p_DOCUMENT_NO, string p_MANUAL_DOCUMENT_NO, int p_DOCUMENT_TYPE_ID, int p_UserId, int p_Paymode, string p_PayeesName)
        {
            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spInsertVENDER_LEDGER mspInsertLedger = new spInsertVENDER_LEDGER();
                mspInsertLedger.Connection = mConnection;
                mspInsertLedger.VOUCHER_TYPE_ID = p_VoucherType;
                mspInsertLedger.VOUCHER_NO = p_VoucherNo;
                mspInsertLedger.ACCOUNT_HEAD_ID = p_ACCOUNT_HEAD_ID;
                mspInsertLedger.DISTRIBUTOR_ID = p_Distributor_Id;
                mspInsertLedger.DEBIT = p_Debit;
                mspInsertLedger.CREDIT = p_Credit;
                mspInsertLedger.LEDGER_DATE = p_Ledger_Date;
                mspInsertLedger.REMARKS = p_Remarks;
                mspInsertLedger.TIME_STAMP = p_TimeStamp;
                mspInsertLedger.PRINCIPAL_ID = p_PRINCIPAL_ID;
                mspInsertLedger.DOCUMENT_TYPE_ID = p_DOCUMENT_TYPE_ID;
                mspInsertLedger.DOCUMENT_NO = p_DOCUMENT_NO;
                mspInsertLedger.MANUAL_DOCUMENT_NO = p_MANUAL_DOCUMENT_NO;
                mspInsertLedger.USER_ID = p_UserId;
                mspInsertLedger.PAYMENT_MODE = p_Paymode;
                mspInsertLedger.PAYEE_NAME = p_PayeesName;
                mspInsertLedger.IS_DELETED = 0;
                mspInsertLedger.ExecuteQuery();
                return true;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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

        public bool InsertVendorOpening(int p_DISTRIBUTOR_ID, string pRemarks, int p_TYPE_ID, DateTime p_DOCUMENT_DATE, int p_PrincipalId, decimal p_TOTAL_AMOUNT, int p_UserId, ref long MasterId,DataTable dtConfig, bool IsFinanceSetting)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spInsertPURCHASE_MASTER mPurchaseMaster = new spInsertPURCHASE_MASTER();
                mPurchaseMaster.Connection = mConnection;
                mPurchaseMaster.Transaction = mTransaction;
                mPurchaseMaster.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPurchaseMaster.TYPE_ID = p_TYPE_ID;
                mPurchaseMaster.ORDER_NUMBER = "opng";

                mPurchaseMaster.DOCUMENT_DATE = p_DOCUMENT_DATE;
                mPurchaseMaster.SOLD_TO = 0;
                mPurchaseMaster.SOLD_FROM = p_PrincipalId;
                mPurchaseMaster.TOTAL_AMOUNT = p_TOTAL_AMOUNT;
                mPurchaseMaster.USER_ID = p_UserId;
                mPurchaseMaster.TIME_STAMP = DateTime.Now;
                mPurchaseMaster.LAST_UPDATE = DateTime.Now;
                mPurchaseMaster.POSTING = 0;
                mPurchaseMaster.BUILTY_NO = pRemarks;
                mPurchaseMaster.DEBIT_AMOUNT = p_TOTAL_AMOUNT;
                mPurchaseMaster.FranchiseMasterID = 0;
                mPurchaseMaster.ExecuteQuery();

                MasterId = mPurchaseMaster.PURCHASE_MASTER_ID;

                LedgerController LController = new LedgerController();
                
                string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, 1);

                DataRow[] drConfig = null;
                
                drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                long Inventoryatstore = Convert.ToInt64(drConfig[0]["VALUE"].ToString());

                drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.AccountPayable + "'");
                long AccountPayable = Convert.ToInt64(drConfig[0]["VALUE"].ToString());

                if (p_TYPE_ID == 0)
                {
                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), Inventoryatstore, p_DISTRIBUTOR_ID, 0, p_TOTAL_AMOUNT, mPurchaseMaster.DOCUMENT_DATE, pRemarks, DateTime.Now, p_PrincipalId, MasterId, "opng", Constants.Document_Purchase, p_UserId, mTransaction, mConnection, Constants.Document_Opening, "0");
                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), AccountPayable, p_DISTRIBUTOR_ID, p_TOTAL_AMOUNT, 0, mPurchaseMaster.DOCUMENT_DATE, pRemarks, DateTime.Now, p_PrincipalId, MasterId, "opng", Constants.Document_Purchase, p_UserId, mTransaction, mConnection, Constants.Document_Opening, "0");
                }
                else
                {
                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), Inventoryatstore, p_DISTRIBUTOR_ID, p_TOTAL_AMOUNT, 0, mPurchaseMaster.DOCUMENT_DATE, pRemarks, DateTime.Now, p_PrincipalId, MasterId, "opng", Constants.Document_Purchase, p_UserId, mTransaction, mConnection, Constants.Document_Opening, "1");
                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), AccountPayable, p_DISTRIBUTOR_ID, 0, p_TOTAL_AMOUNT, mPurchaseMaster.DOCUMENT_DATE, pRemarks, DateTime.Now,  p_PrincipalId, MasterId, "opng", Constants.Document_Purchase, p_UserId, mTransaction, mConnection, Constants.Document_Opening, "1");
                }

                if (IsFinanceSetting)
                {
                    #region GL Master, Detail

                    string VoucherNo2 = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, p_DOCUMENT_DATE);

                    if (p_TYPE_ID == 0) //Credit
                    {
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, p_PrincipalId, VoucherNo2, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.Document_SupplierOpening, Convert.ToString(mPurchaseMaster.PURCHASE_MASTER_ID), "Supplier Opening Balance, Doc#: " + mPurchaseMaster.PURCHASE_MASTER_ID.ToString() + " , " + pRemarks, p_UserId, "OpeningBalance", p_TYPE_ID, mPurchaseMaster.PURCHASE_MASTER_ID, mTransaction, mConnection))
                        {

                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, p_PrincipalId, Constants.Journal_Voucher, VoucherNo2, Inventoryatstore, p_TOTAL_AMOUNT, 0, "Supplier Opening Voucher", mTransaction, mConnection);
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, p_PrincipalId, Constants.Journal_Voucher, VoucherNo2, AccountPayable, 0, p_TOTAL_AMOUNT, "Supplier Opening Voucher", mTransaction, mConnection);

                        }
                    }
                    else if (p_TYPE_ID == 1)//Debit
                    {
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, p_PrincipalId, VoucherNo2, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.Document_SupplierOpening, Convert.ToString(mPurchaseMaster.PURCHASE_MASTER_ID), "Supplier Opening Balance, Doc#: " + mPurchaseMaster.PURCHASE_MASTER_ID.ToString() + " , " + pRemarks, p_UserId, "OpeningBalance", p_TYPE_ID, mPurchaseMaster.PURCHASE_MASTER_ID, mTransaction, mConnection))
                        {

                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, p_PrincipalId, Constants.Journal_Voucher, VoucherNo2, Inventoryatstore, 0, p_TOTAL_AMOUNT, "Supplier Opening Voucher", mTransaction, mConnection);
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, p_PrincipalId, Constants.Journal_Voucher, VoucherNo2, AccountPayable, p_TOTAL_AMOUNT, 0,"Supplier Opening Voucher", mTransaction, mConnection);
                        }
                    }
                    #endregion
                }

                mTransaction.Commit();
                return true;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                return false;// exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public bool InsertVendorOpeningFranchise(int p_DISTRIBUTOR_ID, string pRemarks, int p_TYPE_ID, DateTime p_DOCUMENT_DATE, int p_PrincipalId, decimal p_TOTAL_AMOUNT, int p_UserId, ref long MasterId, DataTable dtConfig, bool IsFinanceSetting)
        {

            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
                spInsertPURCHASE_MASTER mPurchaseMaster = new spInsertPURCHASE_MASTER();
                mPurchaseMaster.Connection = mConnection;
                mPurchaseMaster.Transaction = mTransaction;
                mPurchaseMaster.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                mPurchaseMaster.TYPE_ID = p_TYPE_ID;
                mPurchaseMaster.ORDER_NUMBER = "opng";

                mPurchaseMaster.DOCUMENT_DATE = p_DOCUMENT_DATE;
                mPurchaseMaster.SOLD_TO = 0;
                mPurchaseMaster.SOLD_FROM = p_PrincipalId;
                mPurchaseMaster.TOTAL_AMOUNT = p_TOTAL_AMOUNT;
                mPurchaseMaster.USER_ID = p_UserId;
                mPurchaseMaster.TIME_STAMP = DateTime.Now;
                mPurchaseMaster.LAST_UPDATE = DateTime.Now;
                mPurchaseMaster.POSTING = 0;
                mPurchaseMaster.BUILTY_NO = pRemarks;
                mPurchaseMaster.DEBIT_AMOUNT = p_TOTAL_AMOUNT;
                mPurchaseMaster.FranchiseMasterID = 1;
                mPurchaseMaster.ExecuteQuery();
                MasterId = mPurchaseMaster.PURCHASE_MASTER_ID;
                LedgerController LController = new LedgerController();

                string VoucherNo = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, 1);

                DataRow[] drConfig = null;

                drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.Inventoryatstore + "'");
                long Inventoryatstore = Convert.ToInt64(drConfig[0]["VALUE"].ToString());

                drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.AccountPayable + "'");
                long AccountPayable = Convert.ToInt64(drConfig[0]["VALUE"].ToString());

                if (p_TYPE_ID == 0)
                {
                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), Inventoryatstore, p_DISTRIBUTOR_ID, 0, p_TOTAL_AMOUNT, mPurchaseMaster.DOCUMENT_DATE, pRemarks, DateTime.Now, p_PrincipalId, MasterId, "opng", Constants.Document_FranchisePurchase, p_UserId, mTransaction, mConnection, Constants.Document_Opening, "0");
                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), AccountPayable, p_DISTRIBUTOR_ID, p_TOTAL_AMOUNT, 0, mPurchaseMaster.DOCUMENT_DATE, pRemarks, DateTime.Now, p_PrincipalId, MasterId, "opng", Constants.Document_FranchisePurchase, p_UserId, mTransaction, mConnection, Constants.Document_Opening, "0");
                }
                else
                {
                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), Inventoryatstore, p_DISTRIBUTOR_ID, p_TOTAL_AMOUNT, 0, mPurchaseMaster.DOCUMENT_DATE, pRemarks, DateTime.Now, p_PrincipalId, MasterId, "opng", Constants.Document_FranchisePurchase, p_UserId, mTransaction, mConnection, Constants.Document_Opening, "1");
                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(VoucherNo), AccountPayable, p_DISTRIBUTOR_ID, 0, p_TOTAL_AMOUNT, mPurchaseMaster.DOCUMENT_DATE, pRemarks, DateTime.Now, p_PrincipalId, MasterId, "opng", Constants.Document_FranchisePurchase, p_UserId, mTransaction, mConnection, Constants.Document_Opening, "1");
                }

                if (IsFinanceSetting)
                {
                    #region GL Master, Detail

                    string VoucherNo2 = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_DISTRIBUTOR_ID, p_DOCUMENT_DATE);

                    if (p_TYPE_ID == 0) //Credit
                    {
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, p_PrincipalId, VoucherNo2, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.Document_SupplierOpening, Convert.ToString(mPurchaseMaster.PURCHASE_MASTER_ID), "Supplier Opening Balance, Doc#: " + mPurchaseMaster.PURCHASE_MASTER_ID.ToString() + " , " + pRemarks, p_UserId, "FranchiseOpeningBalance", Constants.Document_FranchisePurchase, mPurchaseMaster.PURCHASE_MASTER_ID, mTransaction, mConnection))
                        {

                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, p_PrincipalId, Constants.Journal_Voucher, VoucherNo2, Inventoryatstore, p_TOTAL_AMOUNT, 0, "Supplier Opening Voucher", mTransaction, mConnection);
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, p_PrincipalId, Constants.Journal_Voucher, VoucherNo2, AccountPayable, 0, p_TOTAL_AMOUNT, "Supplier Opening Voucher", mTransaction, mConnection);

                        }
                    }
                    else if (p_TYPE_ID == 1)//Debit
                    {
                        if (LController.PostingGLMaster(p_DISTRIBUTOR_ID, p_PrincipalId, VoucherNo2, Constants.Journal_Voucher, p_DOCUMENT_DATE, Constants.Document_SupplierOpening, Convert.ToString(mPurchaseMaster.PURCHASE_MASTER_ID), "Supplier Opening Balance, Doc#: " + mPurchaseMaster.PURCHASE_MASTER_ID.ToString() + " , " + pRemarks, p_UserId, "FranchiseOpeningBalance", Constants.Document_FranchisePurchase, mPurchaseMaster.PURCHASE_MASTER_ID, mTransaction, mConnection))
                        {

                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, p_PrincipalId, Constants.Journal_Voucher, VoucherNo2, Inventoryatstore, 0, p_TOTAL_AMOUNT, "Supplier Opening Voucher", mTransaction, mConnection);
                            LController.PostingGLDetail(p_DISTRIBUTOR_ID, p_PrincipalId, Constants.Journal_Voucher, VoucherNo2, AccountPayable, p_TOTAL_AMOUNT, 0, "Supplier Opening Voucher", mTransaction, mConnection);
                        }
                    }
                    #endregion
                }

                mTransaction.Commit();
                return true;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                mTransaction.Rollback();
                return false;// exp.Message;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public bool DeleteCustomerLedger(long p_document_ID, int p_VoucherType, int p_Distributor_Id, long p_Customer_ID, int p_DocumentTypeId, string p_SlipNo, int p_PaymentMode)
        {
            IDbConnection mConnection = null;

            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                spInsertLEDGER mspInsertLedger = new spInsertLEDGER();
                mspInsertLedger.Connection = mConnection;
                mspInsertLedger.VOUCHER_TYPE_ID = p_VoucherType;
                mspInsertLedger.DISTRIBUTOR_ID = p_Distributor_Id;
                mspInsertLedger.DOCUMENT_TYPE_ID = p_DocumentTypeId;
                mspInsertLedger.DOCUMENT_NO = p_document_ID;
                mspInsertLedger.CUSTOMER_ID = Convert.ToInt32(p_Customer_ID);
                mspInsertLedger.SLIP_NO = p_SlipNo;
                mspInsertLedger.PAYMENT_MODE = p_PaymentMode;
                mspInsertLedger.ExecuteQueryDeleteLedger();
                return true;
            }
            catch (Exception excp)
            {
                ExceptionPublisher.PublishException(excp);
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
        public DataSet SelectCreditPendingInvoice(string p_Distributor_Id, string p_CUSTOMER_ID, int P_AREA_ID, DateTime p_to_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                DSReportNew ds = new DSReportNew();
                UspSelectPendingCreditInvoice mCust = new UspSelectPendingCreditInvoice();
                mCust.Connection = mConnection;
                mCust.DISTRIBUTOR_IDS = p_Distributor_Id;
                mCust.CUSTOMER_IDS = p_CUSTOMER_ID;
                mCust.TO_DATE = p_to_Date;
                mCust.AREA_ID = P_AREA_ID;
                DataTable dt = mCust.ExecuteTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptCustomerCredit"].ImportRow(dr);
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