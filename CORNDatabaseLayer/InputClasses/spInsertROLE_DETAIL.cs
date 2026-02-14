using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spInsertROLE_DETAIL
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_ROLE_ID;
        private int m_MODULE_ID;
        private int m_ROLE_DETAIL_ID;
        private int m_Unassign;
        #endregion

        #region Public Properties
        public int ROLE_ID
        {
            set
            {
                m_ROLE_ID = value;
            }
            get
            {
                return m_ROLE_ID;
            }
        }


        public int MODULE_ID
        {
            set
            {
                m_MODULE_ID = value;
            }
            get
            {
                return m_MODULE_ID;
            }
        }


        public int ROLE_DETAIL_ID
        {
            get
            {
                return m_ROLE_DETAIL_ID;
            }
        }

        public int Unassign
        {
            set
            {
                m_Unassign = value;
            }

            get
            {
                return m_Unassign;
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
        public spInsertROLE_DETAIL()
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
                cmd.CommandText = "spInsertROLE_DETAIL";
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                //m_ROLE_DETAIL_ID = (int)((IDataParameter)(cmd.Parameters["@ROLE_DETAIL_ID"])).Value;
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
                parameter.ParameterName = "@ROLE_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_ROLE_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_ROLE_ID;
                }
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@MODULE_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_MODULE_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_MODULE_ID;
                }
                pparams.Add(parameter);

                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Unassign";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_Unassign == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Unassign;
                }
                pparams.Add(parameter);


                //parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                //parameter.ParameterName = "@ROLE_DETAIL_ID";
                //parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                //parameter.Direction = ParameterDirection.Output;
                //pparams.Add(parameter);
            }
            catch (Exception)
            { }

        }
        #endregion
    }
}