using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spInsertPURCHASE_MASTER
    {
        #region Private Members
        private string sp_Name = "spInsertPURCHASE_MASTER";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_DISTRIBUTOR_ID;
        private int m_TYPE_ID;
        private int m_SOLD_TO;
        private int m_SOLD_FROM;
        private int m_USER_ID;
        private int m_POSTING;
        private int m_PRINCIPAL_ID;
        private decimal m_TOTAL_AMOUNT;
        private DateTime m_DOCUMENT_DATE;
        private DateTime m_TIME_STAMP;
        private DateTime m_LAST_UPDATE;
        private long m_PURCHASE_MASTER_ID;
        private string m_ORDER_NUMBER;
        private string m_BUILTY_NO;
        private decimal m_DEBIT_AMOUNT;
        private decimal m_DISCOUNT;
        private decimal m_NET_AMOUNT;
        private decimal m_GST_AMOUNT;
        private decimal m_GST_ADVANCE;
        private decimal m_FREIGHT_AMOUNT;
        private string m_REMARKS;
        private int m_PAYMENT_MODE;
        private long? m_Purchase_Order_Master_ID;
        private long m_PhysicalStock_Taking_ID;
        private long m_FranchiseMasterID;
        private int m_STOCK_DEMAND_ID;

        #endregion
        #region Public Properties
        public long PhysicalStock_Taking_ID
        {
            set
            {
                m_PhysicalStock_Taking_ID = value;
            }
            get
            {
                return m_PhysicalStock_Taking_ID;
            }
        }
        public int STOCK_DEMAND_ID
        {
            set
            {
                m_STOCK_DEMAND_ID = value;
            }
            get
            {
                return m_STOCK_DEMAND_ID;
            }
        }
        public decimal DEBIT_AMOUNT
        {
            set
            {
                m_DEBIT_AMOUNT = value;
            }
            get
            {
                return m_DEBIT_AMOUNT;
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
        public decimal NET_AMOUNT
        {
            set
            {
                m_NET_AMOUNT = value;
            }
            get
            {
                return m_NET_AMOUNT;
            }
        }
        public decimal GST_AMOUNT
        {
            set
            {
                m_GST_AMOUNT = value;
            }
            get
            {
                return m_GST_AMOUNT;
            }
        }
        public decimal GST_ADVANCE
        {
            set { m_GST_ADVANCE = value; }
            get { return m_GST_ADVANCE; }
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
        public int SOLD_TO
        {
            set
            {
                m_SOLD_TO = value;
            }
            get
            {
                return m_SOLD_TO;
            }
        }
        public int SOLD_FROM
        {
            set
            {
                m_SOLD_FROM = value;
            }
            get
            {
                return m_SOLD_FROM;
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
        public int POSTING
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
        public int PRINCIPAL_ID
        {
            set
            {
                m_PRINCIPAL_ID = value;
            }
            get
            {
                return m_PRINCIPAL_ID;
            }
        }
        public decimal TOTAL_AMOUNT
        {
            set
            {
                m_TOTAL_AMOUNT = value;
            }
            get
            {
                return m_TOTAL_AMOUNT;
            }
        }
        public decimal FREIGHT_AMOUNT
        {
            set { m_FREIGHT_AMOUNT = value; }
            get { return m_FREIGHT_AMOUNT; }
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
        public DateTime TIME_STAMP
        {
            set
            {
                m_TIME_STAMP = value;
            }
            get
            {
                return m_TIME_STAMP;
            }
        }
        public DateTime LAST_UPDATE
        {
            set
            {
                m_LAST_UPDATE = value;
            }
            get
            {
                return m_LAST_UPDATE;
            }
        }
        public long PURCHASE_MASTER_ID
        {
            get
            {
                return m_PURCHASE_MASTER_ID;
            }
        }
        public string ORDER_NUMBER
        {
            set
            {
                m_ORDER_NUMBER = value;
            }
            get
            {
                return m_ORDER_NUMBER;
            }
        }
        public string BUILTY_NO
        {
            set
            {
                m_BUILTY_NO = value;
            }
            get
            {
                return m_BUILTY_NO;
            }
        }
        public string REMARKS
        {
            set
            {
                m_REMARKS = value;
            }
            get
            {
                return m_REMARKS;
            }
        }
        public int PAYMENT_MODE
        {
            set { m_PAYMENT_MODE = value; }
            get { return m_PAYMENT_MODE; }
        }
        public long? Purchase_Order_Master_ID
        {
            set { m_Purchase_Order_Master_ID = value; }
            get { return m_Purchase_Order_Master_ID; }
        }
        public long FranchiseMasterID
        {
            set { m_FranchiseMasterID = value; }
            get { return m_FranchiseMasterID; }
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
        public spInsertPURCHASE_MASTER()
        {
            GST_AMOUNT = Constants.DecimalNullValue;
            DISCOUNT = Constants.DecimalNullValue;
            DEBIT_AMOUNT = Constants.DecimalNullValue;
            NET_AMOUNT = Constants.DecimalNullValue;
            REMARKS = null;
            m_FREIGHT_AMOUNT = 0;
            m_FranchiseMasterID = 0;
        }
        #endregion
        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp_Name;
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                m_PURCHASE_MASTER_ID = (long)((IDataParameter)(cmd.Parameters["@PURCHASE_MASTER_ID"])).Value;
                return true;
            }
            catch (Exception e)
            {
                throw e;
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
            parameter.ParameterName = "@SOLD_TO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SOLD_TO == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SOLD_TO;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SOLD_FROM";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SOLD_FROM == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SOLD_FROM;
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
            parameter.ParameterName = "@POSTING";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_POSTING == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_POSTING;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PRINCIPAL_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PRINCIPAL_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PRINCIPAL_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TOTAL_AMOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_TOTAL_AMOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TOTAL_AMOUNT;
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
            parameter.ParameterName = "@TIME_STAMP";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_TIME_STAMP == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TIME_STAMP;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LAST_UPDATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_LAST_UPDATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LAST_UPDATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PURCHASE_MASTER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            parameter.Direction = ParameterDirection.Output;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ORDER_NUMBER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ORDER_NUMBER == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ORDER_NUMBER;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@BUILTY_NO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_BUILTY_NO == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_BUILTY_NO;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DEBIT_AMOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_DEBIT_AMOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DEBIT_AMOUNT;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISCOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
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
            parameter.ParameterName = "@GST_AMOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_GST_AMOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_GST_AMOUNT;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@GST_ADVANCE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_GST_ADVANCE == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_GST_ADVANCE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@NET_AMOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_NET_AMOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_NET_AMOUNT;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@REMARKS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
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
            parameter.ParameterName = "@PAYMENT_MODE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PAYMENT_MODE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PAYMENT_MODE;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Purchase_Order_Master_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_Purchase_Order_Master_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Purchase_Order_Master_ID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FREIGHT_AMOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_FREIGHT_AMOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FREIGHT_AMOUNT;
            }
            pparams.Add(parameter);




            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PhysicalStock_Taking_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_PhysicalStock_Taking_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PhysicalStock_Taking_ID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FranchiseMasterID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_FranchiseMasterID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FranchiseMasterID;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@STOCK_DEMAND_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_STOCK_DEMAND_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_STOCK_DEMAND_ID;
            }
            pparams.Add(parameter);
        }
        #endregion
    }
}