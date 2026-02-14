using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spInsertSALE_INVOICE_MASTER3
    {
        #region Private Members
        private string sp_Name = "spInsertSALE_INVOICE_MASTER3";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private long m_CUSTOMER_ID;
        private int m_PAYMENT_MODE_ID;
        private int m_CUSTOMER_TYPE_ID;
        private int m_TABLE_ID;
        private int m_USER_ID;
        private int m_DISTRIBUTOR_ID;
        private DateTime m_DOCUMENT_DATE;
        private bool m_IS_HOLD;
        private bool m_CreditCard_Impact;
        private decimal m_AMOUNTDUE;
        private decimal m_PAIDIN;
        private decimal m_BALANCE;
        private int m_orderBookerId;
        private string m_approvedby;
        private string m_ORDER_NO;
        private string m_TAKEAWAY_CUSTOMER;
        private string m_MANUAL_ORDER_NO;
        private string m_REMARKS;
        private int m_DeliveryType;
        private decimal m_GST;
        private int m_DELIVERY_CHANNEL;
        private int m_SERVICE_CHARGES_TYPE;
        private decimal m_ITEM_DISCOUNT;
        private byte m_FORM_ID;
        private bool m_DELIVERY_CHANNEL_CASH_IMPACT;
        private decimal m_GSTPER;
        private int m_Order_Delivery_Status_ID;
        private decimal m_EXTRA_DISCOUNT;
        private decimal m_AdvanceAmount;
        private decimal m_CustomerGST;
        private decimal m_CustomerDiscount;
        private byte m_CustomerDiscountType;
        private decimal m_CustomerServiceCharges;
        private byte m_CustomerServiceType;
        private short m_TakeawayType;
        private decimal m_POS_FEE;
        #endregion
        #region Public Properties

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
            set { m_MANUAL_ORDER_NO = value;}
            get { return m_MANUAL_ORDER_NO; }
        }
        public string REMARKS
        {
            set { m_REMARKS = value; }
            get { return m_REMARKS; }
        }
        public string ORDER_NO
        {
            set
            {
                m_ORDER_NO = value;
            }
            get
            {
                return m_ORDER_NO;
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
        
        public string approvedby
        {
            set
            {
                m_approvedby = value;
            }
            get
            {
                return m_approvedby;
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
        public DateTime DOCUMENT_DATE
        {
            set
            {
                m_DOCUMENT_DATE = value;
            }
            get
            {
                return m_DOCUMENT_DATE;
            }
        }
        public bool IS_HOLD
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
        public bool CreditCard_Impact
        {
            set { m_CreditCard_Impact = value; }
            get { return m_CreditCard_Impact; }
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
        public decimal GST
        {
            set { m_GST = value; }
            get { return m_GST; }
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

        public int DeliveryType
        {
            set { m_DeliveryType = value; }
            get { return m_DeliveryType; }
        }
        public int DELIVERY_CHANNEL
        {
            set { m_DELIVERY_CHANNEL = value; }
            get { return m_DELIVERY_CHANNEL; }
        }
        public int SERVICE_CHARGES_TYPE
        {
            set { m_SERVICE_CHARGES_TYPE = value; }
            get { return m_SERVICE_CHARGES_TYPE; }
        }
        public decimal ITEM_DISCOUNT
        {
            set { m_ITEM_DISCOUNT = value; }
            get { return m_ITEM_DISCOUNT; }
        }
        public decimal EXTRA_DISCOUNT
        {
            set { m_EXTRA_DISCOUNT = value; }
            get { return m_EXTRA_DISCOUNT; }
        }
        public byte FORM_ID
        {
            set { m_FORM_ID = value; }
            get { return m_FORM_ID; }
        }
        public bool DELIVERY_CHANNEL_CASH_IMPACT
        {
            set { m_DELIVERY_CHANNEL_CASH_IMPACT = value; }
            get { return m_DELIVERY_CHANNEL_CASH_IMPACT; }
        }
        public decimal GSTPER
        {
            set { m_GSTPER = value; }
            get { return m_GSTPER; }
        }
        public int Order_Delivery_Status_ID
        {
            set { m_Order_Delivery_Status_ID = value; }
            get { return m_Order_Delivery_Status_ID; }
        }
        public decimal AdvanceAmount
        {
            set { m_AdvanceAmount = value; }
            get { return m_AdvanceAmount; }
        }
        public decimal CustomerGST
        {
            set { m_CustomerGST = value; }
            get { return m_CustomerGST; }
        }
        public decimal CustomerDiscount
        {
            set { m_CustomerDiscount = value; }
            get { return m_CustomerDiscount; }
        }
        public byte CustomerDiscountType
        {
            set { m_CustomerDiscountType = value; }
            get { return m_CustomerDiscountType; }
        }
        public decimal CustomerServiceCharges
        {
            set { m_CustomerServiceCharges = value; }
            get { return m_CustomerServiceCharges; }
        }
        public byte CustomerServiceType
        {
            set { m_CustomerServiceType = value; }
            get { return m_CustomerServiceType; }
        }
        public short TakeawayType
        {
            set { m_TakeawayType = value; }
            get { return m_TakeawayType; }
        }
        public decimal POS_FEE
        {
            set { m_POS_FEE = value; }
            get { return m_POS_FEE; }
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
        public spInsertSALE_INVOICE_MASTER3()
        {
            m_CUSTOMER_ID = Constants.LongNullValue;
            m_TAKEAWAY_CUSTOMER = null;
            m_MANUAL_ORDER_NO = null;
            m_REMARKS = null;
            m_ITEM_DISCOUNT = 0;
        }
        #endregion
        #region public Methods
        public DataTable ExecuteTable()
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
                IDbDataAdapter da = ProviderFactory.GetAdapter(EnumProviders.SQLClient);
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
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
            parameter.ParameterName = "@DOCUMENT_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_DOCUMENT_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DOCUMENT_DATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_HOLD";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_HOLD;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CreditCard_Impact";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_CreditCard_Impact;
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
            parameter.ParameterName = "@approvedby";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_approvedby == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_approvedby;
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
            parameter.ParameterName = "@ORDER_NO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ORDER_NO == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ORDER_NO;
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
            parameter.ParameterName = "@DeliveryType";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DeliveryType == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DeliveryType;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DELIVERY_CHANNEL";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DELIVERY_CHANNEL == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DELIVERY_CHANNEL;
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
            parameter.ParameterName = "@ITEM_DISCOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_ITEM_DISCOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ITEM_DISCOUNT;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@EXTRA_DISCOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_EXTRA_DISCOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_EXTRA_DISCOUNT;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FORM_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.SmallInt);
            if (m_FORM_ID == Constants.ByteNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FORM_ID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DELIVERY_CHANNEL_CASH_IMPACT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_DELIVERY_CHANNEL_CASH_IMPACT;
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
            parameter.ParameterName = "@Order_Delivery_Status_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Order_Delivery_Status_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Order_Delivery_Status_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@AdvanceAmount";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_AdvanceAmount == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_AdvanceAmount;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CustomerGST";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_CustomerGST == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CustomerGST;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CustomerDiscount";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_CustomerDiscount == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CustomerDiscount;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CustomerDiscountType";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.SmallInt);
            if (m_CustomerDiscountType < 0)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CustomerDiscountType;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CustomerServiceCharges";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_CustomerServiceCharges == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CustomerServiceCharges;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CustomerServiceType";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.SmallInt);
            if (m_CustomerServiceType < 0)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CustomerServiceType;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TakeawayType";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.SmallInt);
            if (m_TakeawayType == Constants.ShortNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TakeawayType;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@POS_FEE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_POS_FEE == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_POS_FEE;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}