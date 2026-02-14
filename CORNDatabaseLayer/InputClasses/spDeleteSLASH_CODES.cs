using System;
using System.Data;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spDeleteSAMS_CODES
    {
        #region Private Members

        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private long m_REF_ID;

        #endregion

        #region Public Properties

        public long REF_ID
        {
            set
            {
                m_REF_ID = value;
            }
            get
            {
                return m_REF_ID;
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

        public spDeleteSAMS_CODES()
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
                cmd.CommandText = "spDeleteSAMS_CODES";
                cmd.Connection = m_connection;
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
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
            parameter.ParameterName = "@REF_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            parameter.Value = m_REF_ID;
            pparams.Add(parameter);
        }


        #endregion

    }
}