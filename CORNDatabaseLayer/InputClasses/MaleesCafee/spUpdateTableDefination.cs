using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spUpdateTableDefination
    {
        #region Private Members
        private string sp_Name = "spUpdateTableDefination";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_p_TableDefination_ID;
        private int m_p_USER_ID;
        private DateTime m_P_TIME_STAMP;
        private string m_p_TableDefination_No;
        private string m_p_TableDefination_Description;
        private string m_p_TableDefination_Capacity;
        private string m_p_TableDefination_Abbrivation;
        private bool m_p_Is_Active;
        private int m_p_distributorId;
        private int m_FloorID;
        private int m_SortID;
        private int m_ParetnCategoryID;
        #endregion
        #region Public Properties
        public bool p_Is_Active
        {
            set
            {
                m_p_Is_Active = value;
            }
            get
            {
                return m_p_Is_Active;
            }
        }
        public int p_TableDefination_ID
        {
            set
            {
                m_p_TableDefination_ID = value;
            }
            get
            {
                return m_p_TableDefination_ID;
            }
        }
        public int p_USER_ID
        {
            set
            {
                m_p_USER_ID = value;
            }
            get
            {
                return m_p_USER_ID;
            }
        }
        public DateTime P_TIME_STAMP
        {
            set
            {
                m_P_TIME_STAMP = value;
            }
            get
            {
                return m_P_TIME_STAMP;
            }
        }
        public string p_TableDefination_No
        {
            set
            {
                m_p_TableDefination_No = value;
            }
            get
            {
                return m_p_TableDefination_No;
            }
        }
        public string p_TableDefination_Description
        {
            set
            {
                m_p_TableDefination_Description = value;
            }
            get
            {
                return m_p_TableDefination_Description;
            }
        }
        public string p_TableDefination_Capacity
        {
            set
            {
                m_p_TableDefination_Capacity = value;
            }
            get
            {
                return m_p_TableDefination_Capacity;
            }
        }
        public string p_TableDefination_Abbrivation
        {
            set
            {
                m_p_TableDefination_Abbrivation = value;
            }
            get
            {
                return m_p_TableDefination_Abbrivation;
            }
        }
        public int p_distributorId
        {
            set
            {
                m_p_distributorId = value;
            }
            get
            {
                return m_p_distributorId;
            }
        }
        public int FloorID
        {
            set { m_FloorID = value; }
            get { return m_FloorID; }
        }
        public int SortID
        {
            set { m_SortID = value; }
            get { return m_SortID; }
        }
        public int ParetnCategoryID
        {
            set { m_ParetnCategoryID = value; }
            get { return m_ParetnCategoryID; }
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
        public spUpdateTableDefination()
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
            parameter.ParameterName = "@p_TableDefination_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_p_TableDefination_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_p_TableDefination_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@p_USER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_p_USER_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_p_USER_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@P_TIME_STAMP";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_P_TIME_STAMP == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_P_TIME_STAMP;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@p_TableDefination_No";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_p_TableDefination_No == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_p_TableDefination_No;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@p_TableDefination_Description";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_p_TableDefination_Description == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_p_TableDefination_Description;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@p_TableDefination_Capacity";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_p_TableDefination_Capacity == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_p_TableDefination_Capacity;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@p_TableDefination_Abbrivation";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_p_TableDefination_Abbrivation == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_p_TableDefination_Abbrivation;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@p_Is_Active";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_p_Is_Active;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@distributorId";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_p_distributorId == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_p_distributorId;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FloorID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_FloorID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FloorID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SortID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SortID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SortID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ParetnCategoryID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ParetnCategoryID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ParetnCategoryID;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}