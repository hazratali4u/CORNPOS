using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spInsertSKU_GROUP_DETAIL
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_SKU_GROUP_ID;
        private int m_SKU_ID;
        private int m_SKU_GROUP_DETAIL_ID;
        #endregion

        #region Public Properties
        public int SKU_GROUP_ID
        {
            set
            {
                m_SKU_GROUP_ID = value;
            }
            get
            {
                return m_SKU_GROUP_ID;
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


        public int SKU_GROUP_DETAIL_ID
        {
            get
            {
                return m_SKU_GROUP_DETAIL_ID;
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
        public spInsertSKU_GROUP_DETAIL()
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
                cmd.CommandText = "spInsertSKU_GROUP_DETAIL";
                cmd.Connection = m_connection;
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                m_SKU_GROUP_DETAIL_ID = (int)((IDataParameter)(cmd.Parameters["@SKU_GROUP_DETAIL_ID"])).Value;
                return true;
            }
            catch (Exception)
            {
                return false;
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
            parameter.ParameterName = "@SKU_GROUP_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SKU_GROUP_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SKU_GROUP_ID;
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
            parameter.ParameterName = "@SKU_GROUP_DETAIL_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            parameter.Direction = ParameterDirection.Output;
            pparams.Add(parameter);


        }
        #endregion
    }
}