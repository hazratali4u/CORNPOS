using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spSelectPROMOTION_CUSTOMER_VOLUMECLASS
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private long m_PROMOTION_VOLUMECLASS_ID;
        private long m_PROMOTION_ID;
        private int m_SCHEME_ID;
        private int m_DISTRIBUTOR_ID;
        private int m_CUSTOMER_VOLUMECLASS_ID;
        #endregion

        #region Public Properties
        public long PROMOTION_VOLUMECLASS_ID
        {
            set
            {
                m_PROMOTION_VOLUMECLASS_ID = value;
            }
            get
            {
                return m_PROMOTION_VOLUMECLASS_ID;
            }
        }


        public long PROMOTION_ID
        {
            set
            {
                m_PROMOTION_ID = value;
            }
            get
            {
                return m_PROMOTION_ID;
            }
        }


        public int SCHEME_ID
        {
            set
            {
                m_SCHEME_ID = value;
            }
            get
            {
                return m_SCHEME_ID;
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


        public int CUSTOMER_VOLUMECLASS_ID
        {
            set
            {
                m_CUSTOMER_VOLUMECLASS_ID = value;
            }
            get
            {
                return m_CUSTOMER_VOLUMECLASS_ID;
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
        public spSelectPROMOTION_CUSTOMER_VOLUMECLASS()
        {
            m_PROMOTION_VOLUMECLASS_ID = Constants.LongNullValue;
            m_PROMOTION_ID = Constants.LongNullValue;
            m_SCHEME_ID = Constants.IntNullValue;
            m_DISTRIBUTOR_ID = Constants.IntNullValue;
            m_CUSTOMER_VOLUMECLASS_ID = Constants.IntNullValue;
        }
        #endregion

        #region public Methods
        public DataTable ExecuteTable()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spSelectPROMOTION_CUSTOMER_VOLUMECLASS";
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
            parameter.ParameterName = "@PROMOTION_VOLUMECLASS_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_PROMOTION_VOLUMECLASS_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PROMOTION_VOLUMECLASS_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PROMOTION_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_PROMOTION_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PROMOTION_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SCHEME_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SCHEME_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SCHEME_ID;
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
            parameter.ParameterName = "@CUSTOMER_VOLUMECLASS_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CUSTOMER_VOLUMECLASS_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CUSTOMER_VOLUMECLASS_ID;
            }
            pparams.Add(parameter);


        }
        #endregion
    }
}