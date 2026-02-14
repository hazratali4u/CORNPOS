using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spInsertPROMOTION
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private long m_PROMOTION_ID;
        private bool m_PROMOTION_FOR;
        private bool m_CLAIMABLE;
        private bool m_IS_ACTIVE;
        private bool m_IS_SCHEME;
        private DateTime m_PROMO_DATE;
        private DateTime m_START_DATE;
        private DateTime m_END_DATE;
        private DateTime m_START_TIME;
        private DateTime m_END_TIME;
        private int m_PROMOTION_SELECTION;
        private int m_USER_ID;
        private int m_SCHEME_ID;
        private int m_DISTRIBUTOR_ID;
        private int m_PROMOTION_TYPE;
        private string m_PROMOTION_CODE;
        private string m_PROMOTION_DESCRIPTION;
        #endregion

        #region Public Properties
        public long PROMOTION_ID
        {
            get
            {
                return m_PROMOTION_ID;
            }
        }


        public bool PROMOTION_FOR
        {
            set
            {
                m_PROMOTION_FOR = value;
            }
            get
            {
                return m_PROMOTION_FOR;
            }
        }


        public bool CLAIMABLE
        {
            set
            {
                m_CLAIMABLE = value;
            }
            get
            {
                return m_CLAIMABLE;
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


        public bool IS_SCHEME
        {
            set
            {
                m_IS_SCHEME = value;
            }
            get
            {
                return m_IS_SCHEME;
            }
        }


        public DateTime PROMO_DATE
        {
            set
            {
                m_PROMO_DATE = value;
            }
            get
            {
                return m_PROMO_DATE;
            }
        }


        public DateTime START_DATE
        {
            set
            {
                m_START_DATE = value;
            }
            get
            {
                return m_START_DATE;
            }
        }

        public DateTime END_DATE
        {
            set
            {
                m_END_DATE = value;
            }
            get
            {
                return m_END_DATE;
            }
        }

        public DateTime START_TIME
        {
            set { m_START_TIME = value; }
            get { return m_START_TIME; }
        }

        public DateTime END_TIME
        {
            set { m_END_TIME = value; }
            get { return m_END_TIME; }
        }

        public int PROMOTION_SELECTION
        {
            set
            {
                m_PROMOTION_SELECTION = value;
            }
            get
            {
                return m_PROMOTION_SELECTION;
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


        public int SCHEME_ID
        {
            set
            {
                m_SCHEME_ID = value;
            }
            get
            {
                return m_SCHEME_ID;
            }
        }


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


        public int PROMOTION_TYPE
        {
            set
            {
                m_PROMOTION_TYPE = value;
            }
            get
            {
                return m_PROMOTION_TYPE;
            }
        }


        public string PROMOTION_CODE
        {
            set
            {
                m_PROMOTION_CODE = value;
            }
            get
            {
                return m_PROMOTION_CODE;
            }
        }


        public string PROMOTION_DESCRIPTION
        {
            set
            {
                m_PROMOTION_DESCRIPTION = value;
            }
            get
            {
                return m_PROMOTION_DESCRIPTION;
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
        public spInsertPROMOTION()
        {
            m_PROMOTION_ID = Constants.LongNullValue;
            m_PROMOTION_FOR = true;
            m_CLAIMABLE = true;
            m_IS_ACTIVE = true;
            m_IS_SCHEME = true;
            m_PROMO_DATE = Constants.DateNullValue;
            m_START_DATE = Constants.DateNullValue;
            m_END_DATE = Constants.DateNullValue;
            m_START_TIME = Constants.DateNullValue;
            m_END_TIME = Constants.DateNullValue;
            m_PROMOTION_SELECTION = Constants.IntNullValue;
            m_USER_ID = Constants.IntNullValue;
            m_SCHEME_ID = Constants.IntNullValue;
            m_DISTRIBUTOR_ID = Constants.IntNullValue;
            m_PROMOTION_TYPE = Constants.IntNullValue;
            m_PROMOTION_CODE = null;
            m_PROMOTION_DESCRIPTION = null;
        }
        #endregion

        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spInsertPROMOTION";
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                m_PROMOTION_ID = (long)((IDataParameter)(cmd.Parameters["@PROMOTION_ID"])).Value;
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
            parameter.ParameterName = "@PROMOTION_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            parameter.Direction = ParameterDirection.Output;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PROMOTION_FOR";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_PROMOTION_FOR;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CLAIMABLE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_CLAIMABLE;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_ACTIVE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_ACTIVE;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_SCHEME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_SCHEME;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PROMO_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_PROMO_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PROMO_DATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@START_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_START_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_START_DATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@END_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_END_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_END_DATE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@START_TIME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_START_TIME == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_START_TIME;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@END_TIME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_END_TIME == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_END_TIME;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PROMOTION_SELECTION";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PROMOTION_SELECTION == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PROMOTION_SELECTION;
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
            parameter.ParameterName = "@SCHEME_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SCHEME_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SCHEME_ID;
            }
            pparams.Add(parameter);


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
            parameter.ParameterName = "@PROMOTION_TYPE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PROMOTION_TYPE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PROMOTION_TYPE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PROMOTION_CODE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_PROMOTION_CODE == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PROMOTION_CODE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PROMOTION_DESCRIPTION";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_PROMOTION_DESCRIPTION == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PROMOTION_DESCRIPTION;
            }
            pparams.Add(parameter);


        }
        #endregion
    }
}