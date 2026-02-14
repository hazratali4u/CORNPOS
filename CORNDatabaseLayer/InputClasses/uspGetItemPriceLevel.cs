using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using System;
using System.Data;


namespace CORNDatabaseLayer.Classes
{
    public class uspGetItemPriceLevel
    {
        #region Private Members
        private string sp_Name = "uspGetItemPriceLevel";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;        
        private int m_TypeID;
        private long m_SKU_PRICES_LEVEL_ID;
        private int m_SKU_ID;
        #endregion
        #region Public Properties
        public int TypeID
        {
            set
            {
                m_TypeID = value;
            }
            get
            {
                return m_TypeID;
            }
        }
        public int SKU_ID
        {
            set
            {
                m_SKU_ID = value;
            }
            get
            {
                return m_SKU_ID;
            }
        }
        public long SKU_PRICES_LEVEL_ID
        {
            set { m_SKU_PRICES_LEVEL_ID = value; }
            get { return m_SKU_PRICES_LEVEL_ID; }
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
        public uspGetItemPriceLevel()
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
            parameter.ParameterName = "@TypeID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_TypeID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TypeID;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SKU_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SKU_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SKU_ID;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SKU_PRICES_LEVEL_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_SKU_PRICES_LEVEL_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SKU_PRICES_LEVEL_ID;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}