using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using System;
using System.Data;


namespace CORNDatabaseLayer.Classes
{
    public class spInsertCUSTOMER_FEEDBACK
    {
        #region Private Members
        private string sp_Name = "spInsertCUSTOMER_FEEDBACK";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_LOCATION_ID;
        private int m_SERVICE_RATE;
        private int m_FOOD_RATE;
        private int m_ENVIRONMENT_RATE;
        private int m_OVERALL_RATE;
        private int m_HEAR_MEDIUM;
        private int m_FEEDBACK_ID;
        private DateTime m_TIME_STAMP;
        private string m_COMMENTS;
        private string m_OTHER_MEDIUM;
        private string m_NAME;
        private string m_CONTACT_NO;
        private string m_EMAIL;
        private string m_ADDRESS;
        private int m_RETURN_SUGGESTION;
        private int m_VISIT_SUGGESTION;
        private string m_LIKE_SUGGESTION;
        private string m_IMPROVE_SUGGESTION;
        private string m_MENU_SUGGESTION;
        private string m_CITY;

        private DateTime m_FROM_DATE;
        private DateTime m_TO_DATE;
        private DateTime m_DOCUMENT_DATE;

        #endregion
        #region Public Properties
        public int LOCATION_ID
        {
            set
            {
                m_LOCATION_ID = value;
            }
            get
            {
                return m_LOCATION_ID;
            }
        }
        public int SERVICE_RATE
        {
            set
            {
                m_SERVICE_RATE = value;
            }
            get
            {
                return m_SERVICE_RATE;
            }
        }
        public int FOOD_RATE
        {
            set
            {
                m_FOOD_RATE = value;
            }
            get
            {
                return m_FOOD_RATE;
            }
        }
        public int ENVIRONMENT_RATE
        {
            set
            {
                m_ENVIRONMENT_RATE = value;
            }
            get
            {
                return m_ENVIRONMENT_RATE;
            }
        }
        public int OVERALL_RATE
        {
            set
            {
                m_OVERALL_RATE = value;
            }
            get
            {
                return m_OVERALL_RATE;
            }
        }
        public int HEAR_MEDIUM
        {
            set
            {
                m_HEAR_MEDIUM = value;
            }
            get
            {
                return m_HEAR_MEDIUM;
            }
        }
        public int RETURN_SUGGESTION
        {
            set
            {
                m_RETURN_SUGGESTION = value;
            }
            get
            {
                return m_RETURN_SUGGESTION;
            }
        }
        public int FEEDBACK_ID
        {
            get
            {
                return m_FEEDBACK_ID;
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
        public DateTime DOCUMENT_DATE
        {
            set
            {
                m_DOCUMENT_DATE = value;
            }
            get
            {
                return m_DOCUMENT_DATE;
            }
        }
        public string COMMENTS
        {
            set
            {
                m_COMMENTS = value;
            }
            get
            {
                return m_COMMENTS;
            }
        }
        public string OTHER_MEDIUM
        {
            set
            {
                m_OTHER_MEDIUM = value;
            }
            get
            {
                return m_OTHER_MEDIUM;
            }
        }
        public string NAME
        {
            set
            {
                m_NAME = value;
            }
            get
            {
                return m_NAME;
            }
        }
        public string CONTACT_NO
        {
            set
            {
                m_CONTACT_NO = value;
            }
            get
            {
                return m_CONTACT_NO;
            }
        }
        public string EMAIL
        {
            set
            {
                m_EMAIL = value;
            }
            get
            {
                return m_EMAIL;
            }
        }
        public string ADDRESS
        {
            set
            {
                m_ADDRESS = value;
            }
            get
            {
                return m_ADDRESS;
            }
        }
        public int VISIT_SUGGESTION
        {
            set
            {
                m_VISIT_SUGGESTION = value;
            }
            get
            {
                return m_VISIT_SUGGESTION;
            }
        }
        public string LIKE_SUGGESTION
        {
            set
            {
                m_LIKE_SUGGESTION = value;
            }
            get
            {
                return m_LIKE_SUGGESTION;
            }
        }
        public string IMPROVE_SUGGESTION
        {
            set
            {
                m_IMPROVE_SUGGESTION = value;
            }
            get
            {
                return m_IMPROVE_SUGGESTION;
            }
        }
        public string MENU_SUGGESTION
        {
            set
            {
                m_MENU_SUGGESTION = value;
            }
            get
            {
                return m_MENU_SUGGESTION;
            }
        }
        public string CITY
        {
            set
            {
                m_CITY = value;
            }
            get
            {
                return m_CITY;
            }
        }

        public DateTime FROM_DATE
        {
            set
            {
                m_FROM_DATE = value;
            }
            get
            {
                return m_FROM_DATE;
            }
        }
        public DateTime TO_DATE
        {
            set
            {
                m_TO_DATE = value;
            }
            get
            {
                return m_TO_DATE;
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
        public spInsertCUSTOMER_FEEDBACK()
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
                m_FEEDBACK_ID = (int)((IDataParameter)(cmd.Parameters["@FEEDBACK_ID"])).Value;
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
        public DataTable ExecuteTableForCustomerFeedbackReport()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CustomerFeedbackReport";
                command.Connection = m_connection;
                if (m_transaction != null)
                {
                    command.Transaction = m_transaction;
                }
                GetParameterCollectionForFeedbackReport(ref command);
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
            parameter.ParameterName = "@LOCATION_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_LOCATION_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LOCATION_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SERVICE_RATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SERVICE_RATE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SERVICE_RATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FOOD_RATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_FOOD_RATE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FOOD_RATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ENVIRONMENT_RATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ENVIRONMENT_RATE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ENVIRONMENT_RATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@OVERALL_RATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_OVERALL_RATE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_OVERALL_RATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@HEAR_MEDIUM";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_HEAR_MEDIUM == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_HEAR_MEDIUM;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@RETURN_SUGGESTION";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_RETURN_SUGGESTION == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_RETURN_SUGGESTION;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@VISIT_SUGGESTION";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_VISIT_SUGGESTION == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_VISIT_SUGGESTION;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FEEDBACK_ID";
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
            parameter.ParameterName = "@DOCUMENT_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_DOCUMENT_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DOCUMENT_DATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@COMMENTS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_COMMENTS == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_COMMENTS;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@OTHER_MEDIUM";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_OTHER_MEDIUM == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_OTHER_MEDIUM;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@NAME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_NAME == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_NAME;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CONTACT_NO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_CONTACT_NO == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CONTACT_NO;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@EMAIL";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_EMAIL == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_EMAIL;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ADDRESS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_ADDRESS == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ADDRESS;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CITY";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_CITY))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CITY;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LIKE_SUGGESTION";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_LIKE_SUGGESTION))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LIKE_SUGGESTION;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IMPROVE_SUGGESTION";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_IMPROVE_SUGGESTION))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_IMPROVE_SUGGESTION;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@MENU_SUGGESTION";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_MENU_SUGGESTION))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_MENU_SUGGESTION;
            }
            pparams.Add(parameter);
        }

        public void GetParameterCollectionForFeedbackReport(ref IDbCommand cmd)
        {
            IDataParameterCollection pparams = cmd.Parameters;
            IDataParameter parameter;
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISTRIBUTOR_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_LOCATION_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LOCATION_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FROM_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_FROM_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FROM_DATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TO_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_TO_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TO_DATE;
            }
            pparams.Add(parameter);
        }
        #endregion
    }
}