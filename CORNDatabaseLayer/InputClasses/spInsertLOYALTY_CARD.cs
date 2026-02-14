using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using System;
using System.Data;


namespace CORNDatabaseLayer.Classes
{
    public class spInsertLOYALTY_CARD
    {
        #region Private Members
        private string sp_Name = "spInsertLOYALTY_CARD";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_DISTIBUTOR_ID;
        private int m_CARD_TYPE_ID;
        private int m_USER_ID;
        private int m_ID;
        private DateTime m_LAST_UPDATE;
        private DateTime m_TIME_STAMP;
        private bool m_IS_ACTIVE;
        private decimal m_DISCOUNT;
        private decimal m_PURCHASING;
        private decimal m_POINTS;
        private decimal m_AMOUNT_LIMIT;
        private DateTime m_DATE_CREATED;
        private string m_strCardNo;
        private string m_CARD_NAME;
        #endregion
        #region Public Properties
        public string strCardNo
        {
            set
            {
                m_strCardNo = value;
            }
            get
            {
                return m_strCardNo;
            }
        }

        public int DISTIBUTOR_ID
        {
            set
            {
                m_DISTIBUTOR_ID = value;
            }
            get
            {
                return m_DISTIBUTOR_ID;
            }
        }
        public int CARD_TYPE_ID
        {
            set
            {
                m_CARD_TYPE_ID = value;
            }
            get
            {
                return m_CARD_TYPE_ID;
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
        public int ID
        {
            set { m_ID = value; }
            get
            {
                return m_ID;
            }
        }
        public DateTime LAST_UPDATE
        {
            set
            {
                m_LAST_UPDATE = value;
            }
            get
            {
                return m_LAST_UPDATE;
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
        public decimal DISCOUNT
        {
            set
            {
                m_DISCOUNT = value;
            }
            get
            {
                return m_DISCOUNT;
            }
        }
        public decimal PURCHASING
        {
            set
            {
                m_PURCHASING = value;
            }
            get
            {
                return m_PURCHASING;
            }
        }
        public decimal POINTS
        {
            set
            {
                m_POINTS = value;
            }
            get
            {
                return m_POINTS;
            }
        }
        public decimal AMOUNT_LIMIT
        {
            set
            {
                m_AMOUNT_LIMIT = value;
            }
            get
            {
                return m_AMOUNT_LIMIT;
            }
        }
        public DateTime DATE_CREATED
        {
            set
            {
                m_DATE_CREATED = value;
            }
            get
            {
                return m_DATE_CREATED;
            }
        }
        public string CARD_NAME
        {
            set { m_CARD_NAME = value; }
            get { return m_CARD_NAME; }
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
        public spInsertLOYALTY_CARD()
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
            parameter.ParameterName = "@strCardNo";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_strCardNo == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_strCardNo;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISTIBUTOR_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DISTIBUTOR_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DISTIBUTOR_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CARD_TYPE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CARD_TYPE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CARD_TYPE_ID;
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
            parameter.ParameterName = "@ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ID;
            }
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
            parameter.ParameterName = "@LAST_UPDATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_LAST_UPDATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LAST_UPDATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_ACTIVE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_ACTIVE;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISCOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_DISCOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DISCOUNT;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PURCHASING";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_PURCHASING == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PURCHASING;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@POINTS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_POINTS == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_POINTS;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@AMOUNT_LIMIT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_AMOUNT_LIMIT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_AMOUNT_LIMIT;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DATE_CREATED";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_DATE_CREATED == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DATE_CREATED;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CARD_NAME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_CARD_NAME == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CARD_NAME;
            }
            pparams.Add(parameter);
        }
        #endregion
    }
}