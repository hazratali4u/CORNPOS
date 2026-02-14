using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class usp_GetDistributorHierarchy
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_DistributorId;
        #endregion

        #region Public Properties
        public int DistributorId
        {
            set
            {
                m_DistributorId = value;
            }
            get
            {
                return m_DistributorId;
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
        public usp_GetDistributorHierarchy()
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
                command.CommandText = "usp_GetDistributorHierarchy";
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
            parameter.ParameterName = "@DistributorId";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DistributorId == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DistributorId;
            }
            pparams.Add(parameter);


        }
        #endregion
    }
}