using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using System;
using System.Data;


namespace CORNDatabaseLayer.Classes
{
    public class spPromotionVoucher
    {
        #region Private Members
        private string sp_Name = "";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_DISTRIBUTOR_ID;
        private long m_CUSTOMER_ID;
        private string m_VOUCHER_CODE;
        private string m_VOUCHER_NAME;
        private DateTime m_EXPIRY_DATE;
        private long m_SERIAL_FROM;
        private long m_SERIAL_TO;
        private string m_PREFIX;
        private string m_REMARKS;
        private long m_PROMOTION_VOUCHER_ID;
        #endregion
        #region Public Properties
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
        public string VOUCHER_CODE
        {
            set
            {
                m_VOUCHER_CODE = value;
            }
            get
            {
                return m_VOUCHER_CODE;
            }
        }
        public string VOUCHER_NAME
        {
            set
            {
                m_VOUCHER_NAME = value;
            }
            get
            {
                return m_VOUCHER_NAME;
            }
        }
        public DateTime EXPIRY_DATE
        {
            set
            {
                m_EXPIRY_DATE = value;
            }
            get
            {
                return m_EXPIRY_DATE;
            }
        }
        public long SERIAL_FROM
        {
            set
            {
                m_SERIAL_FROM = value;
            }
            get
            {
                return m_SERIAL_FROM;
            }
        }
        public long SERIAL_TO
        {
            set
            {
                m_SERIAL_TO = value;
            }
            get
            {
                return m_SERIAL_TO;
            }
        }
        public string PREFIX
        {
            set
            {
                m_PREFIX = value;
            }
            get
            {
                return m_PREFIX;
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
        public long PROMOTION_VOUCHER_ID
        {
            set
            {
                m_PROMOTION_VOUCHER_ID = value;
            }
            get
            {
                return m_PROMOTION_VOUCHER_ID;
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
        public spPromotionVoucher()
        {
        }
        #endregion
        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spInsertUPDATE_PROMOTION_VOUCHER";
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
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
        public DataTable ExecuteTable()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spSELECT_PROMOTION_VOUCHER";
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                //GetParameterCollection(ref command);
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
            parameter.ParameterName = "@VOUCHER_CODE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_VOUCHER_CODE))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_VOUCHER_CODE;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@VOUCHER_NAME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_VOUCHER_NAME))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_VOUCHER_NAME;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@REMARKS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (string.IsNullOrEmpty(m_REMARKS))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_REMARKS;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@EXPIRY_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_EXPIRY_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_EXPIRY_DATE;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PREFIX";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_PREFIX))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PREFIX;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SERIAL_FROM";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_SERIAL_FROM == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SERIAL_FROM;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SERIAL_TO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_SERIAL_TO == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SERIAL_TO;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PROMOTION_VOUCHER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_PROMOTION_VOUCHER_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PROMOTION_VOUCHER_ID;
            }
            pparams.Add(parameter);
            
        }
        #endregion
    }
}