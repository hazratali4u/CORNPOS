using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spSelectMaxUOM_ID
    {
        #region Private Members

        private string sp_Name = "spSelectMaxUOM_ID";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;

        #endregion

        #region Public Properties


        public IDbConnection Connection
        {
            set { m_connection = value; }
            get { return m_connection; }
        }

        public IDbTransaction Transaction
        {
            set { m_transaction = value; }
            get { return m_transaction; }
        }

        #endregion

        #region Constructor

        public spSelectMaxUOM_ID()
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
                command.CommandText = sp_Name;
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
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
                
        #endregion
    }
}