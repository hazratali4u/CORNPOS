using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using System;
using System.Data;


namespace CORNDatabaseLayer.Classes
{
    public class uspInsertDiscountTemplate
    {
        #region Private Members
        private string sp_Name = "uspInsertDiscountTemplate";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_DistributorID;
        private int m_UserID;
        private string m_DiscountTypeName;
        private decimal m_DiscountValue;
        private byte m_DiscountBehaviour;
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
            set
            {
                m_UserID = value;
            }
            get
            {
                return m_UserID;
            }
        }
        public string DiscountTypeName
        {
            set
            {
                m_DiscountTypeName = value;
            }
            get
            {
                return m_DiscountTypeName;
            }
        }
        public decimal DiscountValue
        {
            set
            {
                m_DiscountValue = value;
            }
            get
            {
                return m_DiscountValue;
            }
        }
        public byte DiscountBehaviour
        {
            set
            {
                m_DiscountBehaviour = value;
            }
            get
            {
                return m_DiscountBehaviour;
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
        public uspInsertDiscountTemplate()
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
            if (m_UserID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_UserID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DiscountTypeName";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_DiscountTypeName))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DiscountTypeName;
            }
            pparams.Add(parameter);
           

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DiscountValue";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_DiscountValue == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DiscountValue;
            }
            pparams.Add(parameter);
            
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DiscountBehaviour";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.TinyInt);
            if (m_DiscountBehaviour == Constants.ByteNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DiscountBehaviour;
            }
            pparams.Add(parameter);
            
        }
        #endregion
    }
}