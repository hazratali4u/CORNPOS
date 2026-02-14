using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spUpdateFRANCHISE_SALE_INVOICE_MASTER2
    {
        #region Private Members
        private string sp_Name = "spUpdateFRANCHISE_SALE_INVOICE_MASTER2";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;

        private long m_FRANCHISE_MASTER_ID;
        #endregion
        #region Public Properties

        public long FRANCHISE_MASTER_ID
        {
            get
            {
                return m_FRANCHISE_MASTER_ID;
            }
            set
            {
                m_FRANCHISE_MASTER_ID = value;
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
        public spUpdateFRANCHISE_SALE_INVOICE_MASTER2()
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
                cmd.CommandText = sp_Name;
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                m_FRANCHISE_MASTER_ID = (long)((IDataParameter)(cmd.Parameters["@FRANCHISE_MASTER_ID"])).Value;
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
            parameter.ParameterName = "@FRANCHISE_MASTER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_FRANCHISE_MASTER_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FRANCHISE_MASTER_ID;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}