using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class UspSelectPendingCreditInvoice
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_DISTRIBUTOR_ID;
        private int m_PRINCIPAL_ID;
        private int m_AREA_ID;
        private long m_CUSTOMER_ID;
        private long m_SALE_INVOICE_ID;
        #endregion

        #region Public Properties
        public string DISTRIBUTOR_IDS { get; set; }
        public string CUSTOMER_IDS { get; set; }
        public DateTime? TO_DATE { get; set; }
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


        public int AREA_ID
        {
            set
            {
                m_AREA_ID = value;
            }
            get
            {
                return m_AREA_ID;
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
        public UspSelectPendingCreditInvoice()
        {
            m_DISTRIBUTOR_ID = Constants.IntNullValue;
            m_PRINCIPAL_ID = Constants.IntNullValue;
            m_AREA_ID = Constants.IntNullValue;
            m_CUSTOMER_ID = Constants.LongNullValue;
            m_SALE_INVOICE_ID = Constants.LongNullValue;
        }
        #endregion

        #region public Methods
        public DataTable ExecuteTable()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UspSelectPendingCreditInvoice";
                command.Connection = m_connection;
                command.CommandTimeout = 300;
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
            parameter.ParameterName = "@AREA_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_AREA_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_AREA_ID;
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
            parameter.ParameterName = "@DISTRIBUTOR_IDS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(DISTRIBUTOR_IDS))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = DISTRIBUTOR_IDS;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CUSTOMER_IDS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(CUSTOMER_IDS))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = CUSTOMER_IDS;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TO_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (TO_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = TO_DATE;
            }
            pparams.Add(parameter);
        }
        #endregion
    }
}