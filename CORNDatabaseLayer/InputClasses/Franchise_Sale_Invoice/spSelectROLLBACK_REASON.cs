using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spSelectROLLBACK_REASON
    {
        #region Private Members
        private string sp_Name = "spSelectROLLBACK_REASON";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_REASON_ID;
        private string m_REASON_DESC;
        private Int16? m_TYPE_ID;
        private bool m_IS_DELETED;

        #endregion
        #region Public Properties

        public int REASON_ID
        {
            set
            {
                m_REASON_ID = value;
            }
            get
            {
                return m_REASON_ID;
            }
        }
        public string REASON_DESC
        {
            set
            {
                m_REASON_DESC = value;
            }
            get
            {
                return m_REASON_DESC;
            }
        }
        public Int16? TYPE_ID
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

        public bool IS_DELETED
        {
            set
            {
                m_IS_DELETED = value;
            }
            get
            {
                return m_IS_DELETED;
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
        public spSelectROLLBACK_REASON()
        {
            REASON_DESC = null;
            TYPE_ID = null;
            IS_DELETED = false;
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

        }
        #endregion
    }
}