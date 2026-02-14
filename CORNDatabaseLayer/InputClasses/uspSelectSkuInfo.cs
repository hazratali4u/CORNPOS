using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using System;
using System.Data;


namespace CORNDatabaseLayer.Classes
{
    public class uspSelectSkuInfo
    {
        #region Private Members
        private string sp_Name = "uspSelectSkuInfo";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_Principal_id;
        private int m_division_id;
        private int m_category_id;
        private int m_brand_id;
        private int m_Company_id;
        private int m_Type_id;
        private string m_SECTION_ID;
        #endregion
        #region Public Properties
        public int Principal_id
        {
            set
            {
                m_Principal_id = value;
            }
            get
            {
                return m_Principal_id;
            }
        }
        public int division_id
        {
            set
            {
                m_division_id = value;
            }
            get
            {
                return m_division_id;
            }
        }
        public int category_id
        {
            set
            {
                m_category_id = value;
            }
            get
            {
                return m_category_id;
            }
        }
        public int brand_id
        {
            set
            {
                m_brand_id = value;
            }
            get
            {
                return m_brand_id;
            }
        }
        public int Company_id
        {
            set
            {
                m_Company_id = value;
            }
            get
            {
                return m_Company_id;
            }
        }
        public int Type_id
        {
            set
            {
                m_Type_id = value;
            }
            get
            {
                return m_Type_id;
            }
        }
        public string SECTION_ID
        {
            set
            {
                m_SECTION_ID = value;
            }
            get
            {
                return m_SECTION_ID;
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
        public uspSelectSkuInfo()
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
                command.CommandTimeout = 0;
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
            parameter.ParameterName = "@Principal_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Principal_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Principal_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@division_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_division_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_division_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@category_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_category_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_category_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@brand_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_brand_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_brand_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Company_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Company_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Company_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Type_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Type_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Type_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SECTION_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_SECTION_ID == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SECTION_ID;
            }
            pparams.Add(parameter);
            
        }
        #endregion
    }
}