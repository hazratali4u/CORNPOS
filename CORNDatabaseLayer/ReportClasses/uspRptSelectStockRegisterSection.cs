using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
	public class uspRptSelectStockRegisterSection
	{
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_distributor_id;
        private int m_category_id;
        private int m_Company_Id;
        private int m_UOM_ID;
        private int m_USER_ID;
        private int m_PRICE_TYPE;
        private DateTime m_DateFrom;
        private DateTime m_dateto;
        private string m_SECTION_IDS;
        private string m_IsLocationWiseItem;
        #endregion

        #region Public Properties
        public int distributor_id
        {
            set
            {
                m_distributor_id = value;
            }
            get
            {
                return m_distributor_id;
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


        public int Company_Id
        {
            set
            {
                m_Company_Id = value;
            }
            get
            {
                return m_Company_Id;
            }
        }


        public int UOM_ID
        {
            set
            {
                m_UOM_ID = value;
            }
            get
            {
                return m_UOM_ID;
            }
        }


        public int USER_ID
        {
            set
            {
                m_USER_ID = value;
            }
            get
            {
                return m_USER_ID;
            }
        }

        public int PRICE_TYPE
        {
            set { m_PRICE_TYPE = value; }
            get { return m_PRICE_TYPE; }
        }

        public DateTime DateFrom
        {
            set
            {
                m_DateFrom = value;
            }
            get
            {
                return m_DateFrom;
            }
        }


        public DateTime dateto
        {
            set
            {
                m_dateto = value;
            }
            get
            {
                return m_dateto;
            }
        }

        public string SECTION_IDS
        {
            set { m_SECTION_IDS = value; }
            get { return m_SECTION_IDS; }
        }
        public string IsLocationWiseItem
        {
            set { m_IsLocationWiseItem = value; }
            get { return m_IsLocationWiseItem; }
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
        public uspRptSelectStockRegisterSection()
        {
            m_distributor_id = Constants.IntNullValue;
            m_category_id = Constants.IntNullValue;
            m_Company_Id = Constants.IntNullValue;
            m_UOM_ID = Constants.IntNullValue;
            m_USER_ID = Constants.IntNullValue;
            m_DateFrom = Constants.DateNullValue;
            m_dateto = Constants.DateNullValue;
            m_PRICE_TYPE = Constants.IntNullValue;
            m_SECTION_IDS = null;
        }
        #endregion

        #region public Methods
        public DataTable ExecuteTable()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "uspRptSelectStockRegisterSection";
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
            parameter.ParameterName = "@distributor_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_distributor_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_distributor_id;
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
            parameter.ParameterName = "@Company_Id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Company_Id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Company_Id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@UOM_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_UOM_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_UOM_ID;
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

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PRICE_TYPE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PRICE_TYPE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PRICE_TYPE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DateFrom";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_DateFrom == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DateFrom;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@dateto";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_dateto == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_dateto;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SECTION_IDS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_SECTION_IDS == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SECTION_IDS;
            }
            pparams.Add(parameter);


        }
        #endregion
	}
}