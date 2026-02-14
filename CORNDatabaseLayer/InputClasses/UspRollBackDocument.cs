using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class UspRollBackDocument
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_DOCUMENT_TYPE;
        private int m_LEGEND_ID;
        private int m_VOID_BY;
        private long m_DOCUMENT_ID;
        private Int16 m_ROLLBACK_REASON_ID;
        #endregion

        #region Public Properties
        public int DOCUMENT_TYPE
        {
            set
            {
                m_DOCUMENT_TYPE = value;
            }
            get
            {
                return m_DOCUMENT_TYPE;
            }
        }


        public int LEGEND_ID
        {
            set
            {
                m_LEGEND_ID = value;
            }
            get
            {
                return m_LEGEND_ID;
            }
        }

        public int VOID_BY
        {
            set { m_VOID_BY = value; }
            get { return m_VOID_BY; }
        }

        public long DOCUMENT_ID
        {
            set
            {
                m_DOCUMENT_ID = value;
            }
            get
            {
                return m_DOCUMENT_ID;
            }
        }
        public Int16 ROLLBACK_REASON_ID
        {
            set
            {
                m_ROLLBACK_REASON_ID = value;
            }
            get
            {
                return m_ROLLBACK_REASON_ID;
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
        public UspRollBackDocument()
        {
            m_DOCUMENT_TYPE = Constants.IntNullValue;
            m_LEGEND_ID = Constants.IntNullValue;
            m_DOCUMENT_ID = Constants.LongNullValue;
        }
        #endregion

        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UspRollBackDocument";
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
            parameter.ParameterName = "@DOCUMENT_TYPE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DOCUMENT_TYPE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DOCUMENT_TYPE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LEGEND_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_LEGEND_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LEGEND_ID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@VOID_BY";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_VOID_BY == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_VOID_BY;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DOCUMENT_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_DOCUMENT_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DOCUMENT_ID;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ROLLBACK_REASON_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.SmallInt);
            if (m_DOCUMENT_ID == Constants.ShortNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ROLLBACK_REASON_ID;
            }
            pparams.Add(parameter);


        }
        #endregion
    }
}