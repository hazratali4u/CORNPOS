using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDataAccessLayer.Classes
{
    public class uspInsertSalePromotionSKU
    {
        #region Private Members
        private string sp_Name = "uspInsertSalePromotionSKU";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_SKUID;
        private long m_SalePromotionID;
        #endregion
        #region Public Properties
        public int SKUID
        {
            set
            {
                m_SKUID = value;
            }
            get
            {
                return m_SKUID;
            }
        }
        public long SalePromotionID
        {
            set
            {
                m_SalePromotionID = value;
            }
            get
            {
                return m_SalePromotionID;
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
        public uspInsertSalePromotionSKU()
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
            parameter.ParameterName = "@SKUID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SKUID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SKUID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SalePromotionID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_SalePromotionID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SalePromotionID;
            }
            pparams.Add(parameter);
           
        }
        #endregion
    }
}