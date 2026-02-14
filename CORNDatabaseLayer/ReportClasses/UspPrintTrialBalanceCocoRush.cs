using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class UspPrintTrialBalanceCocoRush
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        
        private int m_DISTRIBUTOR_ID;
        private int m_PRINCIPAL_ID;
        private int m_ACCOUNT_TYPE_ID;
        private int m_LEAVEL_ID;
        private int m_POSTING;
        private DateTime m_FROM_DATE;
        private DateTime m_TO_DATE;
        private string m_FROM_CODE;
        private string m_TO_CODE;
        private int m_USER_ID;
        #endregion

        #region Public Properties
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
        
        public int PRINCIPAL_ID
        {
            set
            {
                m_PRINCIPAL_ID = value;
            }
            get
            {
                return m_PRINCIPAL_ID;
            }
        }
        
        public int ACCOUNT_TYPE_ID
        {
            set
            {
                m_ACCOUNT_TYPE_ID = value;
            }
            get
            {
                return m_ACCOUNT_TYPE_ID;
            }
        }
        
        public int LEAVEL_ID
        {
            set
            {
                m_LEAVEL_ID = value;
            }
            get
            {
                return m_LEAVEL_ID;
            }
        }
        
        public int POSTING
        {
            set
            {
                m_POSTING = value;
            }
            get
            {
                return m_POSTING;
            }
        }
        
        public DateTime FROM_DATE
        {
            set
            {
                m_FROM_DATE = value;
            }
            get
            {
                return m_FROM_DATE;
            }
        }
        
        public DateTime TO_DATE
        {
            set
            {
                m_TO_DATE = value;
            }
            get
            {
                return m_TO_DATE;
            }
        }
        
        public string FROM_CODE
        {
            set
            {
                m_FROM_CODE = value;
            }
            get
            {
                return m_FROM_CODE;
            }
        }
        
        public string TO_CODE
        {
            set
            {
                m_TO_CODE = value;
            }
            get
            {
                return m_TO_CODE;
            }
        }
        
        public int USER_ID
        {
            set { m_USER_ID = value; }
            get { return m_USER_ID; }
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
        public UspPrintTrialBalanceCocoRush()
        {
            m_USER_ID = Constants.IntNullValue;
        }
        #endregion

        #region public Methods
        public DataTable ExecuteTableFinancialRptCocoRush()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spFinancialRptCocoRush";
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                GetParameterCollectionFinancialRptCocoRush(ref command);
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
        public DataTable ExecuteTableDayBookRptCocoRush()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spDayBookRptCocoRush";
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                GetParameterCollectionFinancialRptCocoRush(ref command);
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

        public void GetParameterCollectionFinancialRptCocoRush(ref IDbCommand cmd)
        {
            IDataParameterCollection pparams = cmd.Parameters;
            IDataParameter parameter;
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
            parameter.ParameterName = "@FROM_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_FROM_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FROM_DATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TO_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_TO_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TO_DATE;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@USER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_USER_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_USER_ID;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}