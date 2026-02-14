using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spInsertPROMOTION_DAYS
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private long m_PROMOTION_ID;
        private long m_PROMOTION_DAYS_ID;
        private int m_SCHEME_ID;
        private int m_DAY_ID;
        #endregion

        #region Public Properties
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


        public long PROMOTION_DAYS_ID
        {
            get
            {
                return m_PROMOTION_DAYS_ID;
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


        public int DAY_ID
        {
            set
            {
                m_DAY_ID = value;
            }
            get
            {
                return m_DAY_ID;
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
        public spInsertPROMOTION_DAYS()
        {
            m_PROMOTION_ID = Constants.LongNullValue;
            m_PROMOTION_DAYS_ID = Constants.LongNullValue;
            m_SCHEME_ID = Constants.IntNullValue;
            m_DAY_ID = Constants.IntNullValue;
        }
        #endregion

        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spInsertPROMOTION_DAYS";
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                m_PROMOTION_DAYS_ID = (long)((IDataParameter)(cmd.Parameters["@PROMOTION_DAYS_ID"])).Value;
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
            parameter.ParameterName = "@PROMOTION_DAYS_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            parameter.Direction = ParameterDirection.Output;
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
            parameter.ParameterName = "@DAY_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DAY_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DAY_ID;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}