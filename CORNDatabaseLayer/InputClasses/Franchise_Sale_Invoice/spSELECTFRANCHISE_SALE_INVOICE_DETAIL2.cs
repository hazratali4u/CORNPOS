using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spSELECTFRANCHISE_SALE_INVOICE_DETAIL2
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;

        private long m_FRANCHISE_MASTER_ID;
        #endregion

        #region Public Properties
        public long FRANCHISE_MASTER_ID
        {
            set
            {
                m_FRANCHISE_MASTER_ID = value;
            }
            get
            {
                return m_FRANCHISE_MASTER_ID;
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
        public spSELECTFRANCHISE_SALE_INVOICE_DETAIL2()
        {
        }
        #endregion

        #region public Methods

        public DataTable ExecuteTable()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spSELECTFRANCHISE_SALE_INVOICE_DETAIL2";
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
            parameter.ParameterName = "@FRANCHISE_MASTER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (FRANCHISE_MASTER_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = FRANCHISE_MASTER_ID;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}