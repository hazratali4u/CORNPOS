using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spUpdateSALE_INVOICE_MASTERNew
    {
        #region Private Members
        private string sp_Name = "spUpdateSALE_INVOICE_MASTERNew";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private byte m_POSTING;
        private int m_PAYMENT_MODE_ID;
        private int m_DISCOUNT_TYPE;
        private int m_CUSTOMER_TYPE_ID;
        private int m_TABLE_ID;
        private int m_USER_ID;
        private int m_DISTRIBUTOR_ID;
        private long m_CUSTOMER_ID;
        private int m_orderBookerId;
        private bool? m_IS_HOLD;
        private bool? m_IS_ACTIVE;
        private decimal m_AMOUNTDUE;
        private decimal m_DISCOUNT;
        private decimal m_SERVICE_CHARGES;
        private decimal m_GST;
        private decimal m_PAIDIN;
        private decimal m_BALANCE;
        private long m_SALE_INVOICE_ID;
        private string m_coverTable;
        private string m_TAKEAWAY_CUSTOMER;
        private string m_MANUAL_ORDER_NO;
        private string m_REMARKS;
        private short m_EmpDiscountType;
        private int m_EMC_UserID;
        private int m_Manager_UserID;
        private string m_ManagerPWD;
        private string m_strCardNo;
        private decimal m_POINTS;
        private decimal m_PURCHASING;
        private int m_TYPE_ID;
        private int m_SERVICE_CHARGES_TYPE;
        private string m_InvoiceNumberFBR;
        private DateTime m_KDS_TIME;
        private long m_BANK_ID;
        private decimal m_GSTPER;
        private decimal m_CREDIT_AMOUNT;
        private int m_RecordType;
        #endregion
        #region Public Properties
        public int TYPE_ID
        {
            set
            {
                m_TYPE_ID = value;
            }
            get
            {
                return m_TYPE_ID;
            }
        }
        public short EmpDiscountType
        {
            set
            {
                m_EmpDiscountType = value;
            }
            get
            {
                return m_EmpDiscountType;
            }
        }
        public int EMC_UserID
        {
            set
            {
                m_EMC_UserID = value;
            }
            get
            {
                return m_EMC_UserID;
            }
        }
        public int Manager_UserID
        {
            set
            {
                m_Manager_UserID = value;
            }
            get
            {
                return m_Manager_UserID;
            }
        }
        public string ManagerPWD
        {
            set
            {
                m_ManagerPWD = value;
            }
            get
            {
                return m_ManagerPWD;
            }
        }
        public string strCardNo
        {
            set
            {
                m_strCardNo = value;
            }
            get
            {
                return m_strCardNo;
            }
        }
        public string TAKEAWAY_CUSTOMER
        {
            set
            {
                m_TAKEAWAY_CUSTOMER = value;
            }
            get
            {
                return m_TAKEAWAY_CUSTOMER;
            }
        }
        public string MANUAL_ORDER_NO
        {
            set { m_MANUAL_ORDER_NO = value; }
            get { return m_MANUAL_ORDER_NO; }
        }
        public string REMARKS
        {
            set { m_REMARKS = value; }
            get { return m_REMARKS; }
        }
        public byte POSTING
        {
            set
            {
                m_POSTING = value;
            }
            get
            {
                return m_POSTING;
            }
        }
        public int PAYMENT_MODE_ID
        {
            set
            {
                m_PAYMENT_MODE_ID = value;
            }
            get
            {
                return m_PAYMENT_MODE_ID;
            }
        }
        public int DISCOUNT_TYPE
        {
            set
            {
                m_DISCOUNT_TYPE = value;
            }
            get
            {
                return m_DISCOUNT_TYPE;
            }
        }
        public int CUSTOMER_TYPE_ID
        {
            set
            {
                m_CUSTOMER_TYPE_ID = value;
            }
            get
            {
                return m_CUSTOMER_TYPE_ID;
            }
        }
        public int TABLE_ID
        {
            set
            {
                m_TABLE_ID = value;
            }
            get
            {
                return m_TABLE_ID;
            }
        }
        public int USER_ID
        {
            set
            {
                m_USER_ID = value;
            }
            get
            {
                return m_USER_ID;
            }
        }
        public int DISTRIBUTOR_ID
        {
            set
            {
                m_DISTRIBUTOR_ID = value;
            }
            get
            {
                return m_DISTRIBUTOR_ID;
            }
        }
        public long CUSTOMER_ID
        {
            set
            {
                m_CUSTOMER_ID = value;
            }
            get
            {
                return m_CUSTOMER_ID;
            }
        }
        public int orderBookerId
        {
            set
            {
                m_orderBookerId = value;
            }
            get
            {
                return m_orderBookerId;
            }
        }
        public bool? IS_HOLD
        {
            set
            {
                m_IS_HOLD = value;
            }
            get
            {
                return m_IS_HOLD;
            }
        }
        public bool? IS_ACTIVE
        {
            set
            {
                m_IS_ACTIVE = value;
            }
            get
            {
                return m_IS_ACTIVE;
            }
        }
        public decimal AMOUNTDUE
        {
            set
            {
                m_AMOUNTDUE = value;
            }
            get
            {
                return m_AMOUNTDUE;
            }
        }
        public decimal DISCOUNT
        {
            set
            {
                m_DISCOUNT = value;
            }
            get
            {
                return m_DISCOUNT;
            }
        }
        public decimal GST
        {
            set
            {
                m_GST = value;
            }
            get
            {
                return m_GST;
            }
        }
        public decimal PAIDIN
        {
            set
            {
                m_PAIDIN = value;
            }
            get
            {
                return m_PAIDIN;
            }
        }
        public decimal BALANCE
        {
            set
            {
                m_BALANCE = value;
            }
            get
            {
                return m_BALANCE;
            }
        }
        public decimal SERVICE_CHARGES
        {
            set
            {
                m_SERVICE_CHARGES = value;
            }
            get
            {
                return m_SERVICE_CHARGES;
            }
        }
        public long SALE_INVOICE_ID
        {
            set
            {
                m_SALE_INVOICE_ID = value;
            }
            get
            {
                return m_SALE_INVOICE_ID;
            }
        }
        public DateTime KDS_TIME
        {
            set
            {
                m_KDS_TIME = value;
            }
            get
            {
                return m_KDS_TIME;
            }
        }
        public string coverTable
        {
            set
            {
                m_coverTable = value;
            }
            get
            {
                return m_coverTable;
            }
        }
        public decimal POINTS
        {
            set
            {
                m_POINTS = value;
            }
            get
            {
                return m_POINTS;
            }
        }
        public decimal PURCHASING
        {
            set { m_PURCHASING = value; }
            get { return m_PURCHASING; }
        }
        public int SERVICE_CHARGES_TYPE
        {
            set { m_SERVICE_CHARGES_TYPE = value; }
            get { return m_SERVICE_CHARGES_TYPE; }
        }
        public string InvoiceNumberFBR
        {
            set { m_InvoiceNumberFBR = value; }
            get { return m_InvoiceNumberFBR; }
        }
        public long BANK_ID
        {
            set { m_BANK_ID = value; }
            get { return m_BANK_ID; }
        }
        public decimal GSTPER
        {
            set { m_GSTPER = value; }
            get { return m_GSTPER; }
        }
        public decimal CREDIT_AMOUNT
        {
            set { m_CREDIT_AMOUNT = value; }
            get { return m_CREDIT_AMOUNT; }
        }
        public int RecordType
        {
            set { m_RecordType = value; }
            get { return m_RecordType; }
        }
        public IDbConnection Connection
        {
            set
            {
                m_connection = value;
            }
            get
            {
                return m_connection;
            }
        }
        public IDbTransaction Transaction
        {
            set
            {
                m_transaction = value;
            }
            get
            {
                return m_transaction;
            }
        }
        #endregion
        #region Constructor
        public spUpdateSALE_INVOICE_MASTERNew()
        {
            m_orderBookerId = Constants.IntNullValue;
            m_coverTable = null;
            m_MANUAL_ORDER_NO = null;
            m_REMARKS = null;
            m_PAYMENT_MODE_ID = Constants.IntNullValue;
            m_DISCOUNT_TYPE = Constants.IntNullValue;
            m_CUSTOMER_TYPE_ID = Constants.IntNullValue;
            m_TABLE_ID = Constants.IntNullValue;
            m_USER_ID = Constants.IntNullValue;
            m_DISTRIBUTOR_ID = Constants.IntNullValue;
            m_AMOUNTDUE = Constants.DecimalNullValue;
            m_DISCOUNT = Constants.DecimalNullValue;
            m_GST = Constants.DecimalNullValue;
            m_PAIDIN = Constants.DecimalNullValue;
            m_BALANCE = Constants.DecimalNullValue;
            m_SALE_INVOICE_ID = Constants.LongNullValue;
            m_SERVICE_CHARGES = Constants.DecimalNullValue;
            m_EmpDiscountType = Constants.ShortNullValue;
            m_EMC_UserID = Constants.IntNullValue;
            m_ManagerPWD = null;
            m_Manager_UserID = Constants.IntNullValue;
            m_strCardNo = null;
            m_POINTS = Constants.DecimalNullValue;
            m_PURCHASING = Constants.DecimalNullValue;
            m_CUSTOMER_ID = Constants.LongNullValue;
            m_TYPE_ID = Constants.IntNullValue;
        }
        #endregion
        #region public Methods
        public string ExecuteScalar()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sp_Name;
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                GetParameterCollection(ref command);
                object o;
                o = command.ExecuteScalar();


                return o.ToString();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
            }
        }
        public string ExecuteScalarForItemAsReady()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "MarkItemAsReadyForKDS";
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                GetParameterCollectionForItemReady(ref command);
                object o;
                o = command.ExecuteScalar();


                return o.ToString();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
            }
        }
        public string ExecuteScalarForItemAsServed()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "MarkItemAsServedForKDS";
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                GetParameterCollectionForItemReady(ref command);
                object o;
                o = command.ExecuteScalar();


                return o.ToString();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
            }
        }
        public void GetParameterCollection(ref IDbCommand cmd)
        {
            IDataParameterCollection pparams = cmd.Parameters;
            IDataParameter parameter;
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@POSTING";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.TinyInt);
            if (m_POSTING == Constants.ByteNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_POSTING;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TYPE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_TYPE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TYPE_ID;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PAYMENT_MODE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PAYMENT_MODE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PAYMENT_MODE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISCOUNT_TYPE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DISCOUNT_TYPE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DISCOUNT_TYPE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CUSTOMER_TYPE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CUSTOMER_TYPE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CUSTOMER_TYPE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TABLE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_TABLE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TABLE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@USER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_USER_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_USER_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISTRIBUTOR_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DISTRIBUTOR_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DISTRIBUTOR_ID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CUSTOMER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_CUSTOMER_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CUSTOMER_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@orderBookerId";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_orderBookerId == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_orderBookerId;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_HOLD";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            if (m_IS_HOLD == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_IS_HOLD;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_ACTIVE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            if (m_IS_ACTIVE == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_IS_ACTIVE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@AMOUNTDUE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_AMOUNTDUE == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_AMOUNTDUE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISCOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_DISCOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DISCOUNT;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@GST";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_GST == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_GST;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PAIDIN";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_PAIDIN == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PAIDIN;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@BALANCE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_BALANCE == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_BALANCE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SERVICE_CHARGES";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_SERVICE_CHARGES == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SERVICE_CHARGES;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SALE_INVOICE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_SALE_INVOICE_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SALE_INVOICE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@coverTable";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_coverTable == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_coverTable;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TAKEAWAY_CUSTOMER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_TAKEAWAY_CUSTOMER == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TAKEAWAY_CUSTOMER;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@MANUAL_ORDER_NO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_MANUAL_ORDER_NO == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_MANUAL_ORDER_NO;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@REMARKS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_REMARKS == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_REMARKS;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@EmpDiscountType";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.SmallInt);
            if (m_EmpDiscountType == Constants.ShortNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_EmpDiscountType;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@EMC_UserID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_EMC_UserID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_EMC_UserID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Manager_UserID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Manager_UserID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Manager_UserID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ManagerPWD";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ManagerPWD == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ManagerPWD;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@strCardNo";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_strCardNo == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_strCardNo;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@POINTS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_POINTS == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_POINTS;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PURCHASING";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_PURCHASING == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PURCHASING;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SERVICE_CHARGES_TYPE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SERVICE_CHARGES_TYPE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SERVICE_CHARGES_TYPE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@InvoiceNumberFBR";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_InvoiceNumberFBR == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_InvoiceNumberFBR;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@KDS_TIME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_KDS_TIME == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_KDS_TIME;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@BANK_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_BANK_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_BANK_ID;
            }
            pparams.Add(parameter);

            
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@GSTPER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_GSTPER == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_GSTPER;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CREDIT_AMOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_CREDIT_AMOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CREDIT_AMOUNT;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@RecordType";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_RecordType == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_RecordType;
            }
            pparams.Add(parameter);
            
        }

        public void GetParameterCollectionForItemReady(ref IDbCommand cmd)
        {
            IDataParameterCollection pparams = cmd.Parameters;
            IDataParameter parameter;
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SALE_INVOICE_DETAIL_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_SALE_INVOICE_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SALE_INVOICE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsReady";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            if (m_IS_HOLD == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_IS_HOLD;
            }
            pparams.Add(parameter);            
        }
        #endregion
    }
}