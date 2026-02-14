using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using System;
using System.Data;


namespace CORNDatabaseLayer.Classes
{
    public class uspInsertTaxAuthorityIntegration
    {
        #region Private Members
        private string sp_Name = "uspInsertTaxAuthorityIntegration";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;        
        private int m_DistributorID;
        private int mUserID;
        private string m_POSID;
        private string m_FBRURL;
        private string m_Token;
        private string m_TaxAuthorityLabel;
        #endregion
        #region Public Properties
        public int DistributorID
        {
            set
            {
                m_DistributorID = value;
            }
            get
            {
                return m_DistributorID;
            }
        }
        public int UserID
        {
            set { mUserID = value; }
            get { return mUserID; }
        }
        public string POSID
        {
            set { m_POSID = value; }
            get { return m_POSID; }
        }
        public string Token
        {
            set { m_Token = value; }
            get { return m_Token; }
        }
        public string FBRURL
        {
            set { m_FBRURL = value; }
            get { return m_FBRURL; }
        }
        public string TaxAuthorityLabel
        {
            set { m_TaxAuthorityLabel = value; }
            get { return m_TaxAuthorityLabel; }
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
        public uspInsertTaxAuthorityIntegration()
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
            parameter.ParameterName = "@DistributorID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DistributorID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DistributorID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@UserID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (mUserID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = mUserID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@POSID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_POSID == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_POSID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FBRURL";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_FBRURL == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FBRURL;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Token";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_Token == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Token;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TaxAuthorityLabel";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_TaxAuthorityLabel == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TaxAuthorityLabel;
            }
            pparams.Add(parameter);
        }
        #endregion
    }
}