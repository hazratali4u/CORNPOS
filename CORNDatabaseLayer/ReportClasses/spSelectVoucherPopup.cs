using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;



namespace CORNDatabaseLayer.Classes
{
    public class spSelectVoucherPopup
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;

        private string m_UniqueID;
        #endregion

        #region Public Properties

        public string UniqueID
        {
            set
            {
                m_UniqueID = value;
            }
            get
            {
                return m_UniqueID;
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
        public spSelectVoucherPopup()
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
                command.CommandText = "spSelectVoucherPopup";
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
            parameter.ParameterName = "@UniqueID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_UniqueID == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_UniqueID;
            }
            pparams.Add(parameter);
        }
        #endregion
    }
}