using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spUpdateCafeTimeSchedule
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;

        private int m_ID;
        private int m_Distributor_ID;
        private bool m_IsTemporaryClosed;
        private string m_Day;
        private string m_Message;
        private DateTime? m_FromDate;
        private DateTime? m_ToDate;
        #endregion

        #region Public Properties
        public int ID
        {
            set
            {
                m_ID = value;
            }
            get
            {
                return m_ID;
            }
        }
        public int Distributor_ID
        {
            set
            {
                m_Distributor_ID = value;
            }
            get
            {
                return m_Distributor_ID;
            }
        }


        public bool IsTemporaryClosed
        {
            set
            {
                m_IsTemporaryClosed = value;
            }
            get
            {
                return m_IsTemporaryClosed;
            }
        }

        public string Day
        {
            set
            {
                m_Day = value;
            }

            get
            {
                return m_Day;
            }
        }

        public string Message
        {
            set
            {
                m_Message = value;
            }

            get
            {
                return m_Message;
            }
        }

        public DateTime? FromDate
        {
            set
            {
                m_FromDate = value;
            }

            get
            {
                return m_FromDate;
            }
        }

        public DateTime? ToDate
        {
            set
            {
                m_ToDate = value;
            }

            get
            {
                return m_ToDate;
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
        public spUpdateCafeTimeSchedule()
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
                cmd.CommandText = "spUpdateCafeTimeSchedule";
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
            try
            {
                IDataParameterCollection pparams = cmd.Parameters;
                IDataParameter parameter;

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
                parameter.ParameterName = "@DISTRIBUTOR_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_Distributor_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Distributor_ID;
                }
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@IsTemporaryClosed";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
                parameter.Value = m_IsTemporaryClosed;
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Day";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
                parameter.Value = m_Day;
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Message";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
                parameter.Value = m_Message;
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@FromDate";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
                if (m_FromDate == Constants.DateNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_FromDate;
                }
                pparams.Add(parameter);




                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@ToDate";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
                if (m_ToDate == Constants.DateNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_ToDate;
                }
                pparams.Add(parameter);
            }
            catch (Exception)
            { }

        }
        #endregion
    }
}