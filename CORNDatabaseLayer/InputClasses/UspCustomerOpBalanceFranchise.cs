using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class UspCustomerOpBalanceFranchise
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_Distributor_id;
        private int m_principal_id;
        private int m_customer_id;
        private DateTime m_From_Date;
        #endregion

        #region Public Properties
        public int Distributor_id
        {
            set
            {
                m_Distributor_id = value;
            }
            get
            {
                return m_Distributor_id;
            }
        }


        public int principal_id
        {
            set
            {
                m_principal_id = value;
            }
            get
            {
                return m_principal_id;
            }
        }


        public int customer_id
        {
            set
            {
                m_customer_id = value;
            }
            get
            {
                return m_customer_id;
            }
        }


        public DateTime From_Date
        {
            set
            {
                m_From_Date = value;
            }
            get
            {
                return m_From_Date;
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
        public UspCustomerOpBalanceFranchise()
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
                command.CommandText = "UspCustomerOpBalanceFranchise";
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
            parameter.ParameterName = "@Distributor_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Distributor_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Distributor_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@principal_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_principal_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_principal_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@customer_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_customer_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_customer_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@From_Date";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_From_Date == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_From_Date;
            }
            pparams.Add(parameter);


        }
        #endregion
    }
}