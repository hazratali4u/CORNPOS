using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spInsertSKU_HIERARCHY
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_COMPANY_ID;
        private int m_PARENT_SKU_HIE_ID;
        private int m_SKU_HIE_TYPE_ID;
        private int m_SKU_HIE_ID;
        private DateTime m_TIME_STAMP;
        private DateTime m_LASTUPDATE_DATE;
        private bool m_IS_ACTIVE;
        private bool m_IS_MANUALDISCOUNT;
        private string m_SKU_HIE_CODE;
        private string m_SKU_HIE_NAME;
        private string m_IP_ADDRESS;
        private string m_ImagePath;
        private int m_PARENT_SKU_HIE_TYPE_ID;
        private int m_SORT_ORDER;
        private bool m_IsOpenItemCategory;
        #endregion

        #region Public Properties
        public int COMPANY_ID
        {
            set
            {
                m_COMPANY_ID = value;
            }
            get
            {
                return m_COMPANY_ID;
            }
        }


        public int PARENT_SKU_HIE_ID
        {
            set
            {
                m_PARENT_SKU_HIE_ID = value;
            }
            get
            {
                return m_PARENT_SKU_HIE_ID;
            }
        }


        public int SKU_HIE_TYPE_ID
        {
            set
            {
                m_SKU_HIE_TYPE_ID = value;
            }
            get
            {
                return m_SKU_HIE_TYPE_ID;
            }
        }


        public int SKU_HIE_ID
        {
            get
            {
                return m_SKU_HIE_ID;
            }
        }


        public DateTime TIME_STAMP
        {
            set
            {
                m_TIME_STAMP = value;
            }
            get
            {
                return m_TIME_STAMP;
            }
        }


        public DateTime LASTUPDATE_DATE
        {
            set
            {
                m_LASTUPDATE_DATE = value;
            }
            get
            {
                return m_LASTUPDATE_DATE;
            }
        }


        public bool IS_ACTIVE
        {
            set
            {
                m_IS_ACTIVE = value;
            }
            get
            {
                return m_IS_ACTIVE;
            }
        }


        public bool IS_MANUALDISCOUNT
        {
            set
            {
                m_IS_MANUALDISCOUNT = value;
            }
            get
            {
                return m_IS_MANUALDISCOUNT;
            }
        }


        public string SKU_HIE_CODE
        {
            set
            {
                m_SKU_HIE_CODE = value;
            }
            get
            {
                return m_SKU_HIE_CODE;
            }
        }


        public string SKU_HIE_NAME
        {
            set
            {
                m_SKU_HIE_NAME = value;
            }
            get
            {
                return m_SKU_HIE_NAME;
            }
        }


        public string IP_ADDRESS
        {
            set
            {
                m_IP_ADDRESS = value;
            }
            get
            {
                return m_IP_ADDRESS;
            }
        }

        public string ImagePath
        {
            set { m_ImagePath = value; }
            get { return m_ImagePath; }
        }

        public int PARENT_SKU_HIE_TYPE_ID
        {
            set
            {
                m_PARENT_SKU_HIE_TYPE_ID = value;
            }
            get
            {
                return m_PARENT_SKU_HIE_TYPE_ID;
            }
        }
        public int SORT_ORDER
        {
            set
            {
                m_SORT_ORDER = value;
            }
            get
            {
                return m_SORT_ORDER;
            }
        }

        public bool IsOpenItemCategory
        {
            set { m_IsOpenItemCategory = value; }
            get { return m_IsOpenItemCategory; }
        }
        public bool MultiSelectModifier { get; set; }

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
        public spInsertSKU_HIERARCHY()
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
                cmd.CommandText = "spInsertSKU_HIERARCHY";
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                m_SKU_HIE_ID = (int)((IDataParameter)(cmd.Parameters["@SKU_HIE_ID"])).Value;
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
            parameter.ParameterName = "@COMPANY_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_COMPANY_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_COMPANY_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PARENT_SKU_HIE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PARENT_SKU_HIE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PARENT_SKU_HIE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SKU_HIE_TYPE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SKU_HIE_TYPE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SKU_HIE_TYPE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SKU_HIE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            parameter.Direction = ParameterDirection.Output;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TIME_STAMP";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_TIME_STAMP == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TIME_STAMP;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LASTUPDATE_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_LASTUPDATE_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LASTUPDATE_DATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_ACTIVE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_ACTIVE;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_MANUALDISCOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_MANUALDISCOUNT;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SKU_HIE_CODE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_SKU_HIE_CODE == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SKU_HIE_CODE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SKU_HIE_NAME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_SKU_HIE_NAME == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SKU_HIE_NAME;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IP_ADDRESS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_IP_ADDRESS == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_IP_ADDRESS;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ImagePath";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ImagePath == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ImagePath;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PARENT_SKU_HIE_TYPE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PARENT_SKU_HIE_TYPE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PARENT_SKU_HIE_TYPE_ID;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SORT_ORDER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SORT_ORDER == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SORT_ORDER;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsOpenItemCategory";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IsOpenItemCategory;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@MultiSelectModifier";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = MultiSelectModifier;
            pparams.Add(parameter);
        }
        #endregion
    }
}