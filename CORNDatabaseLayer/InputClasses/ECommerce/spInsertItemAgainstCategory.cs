using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spInsertItemAgainstCategory
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_Category_ID;
        private int m_SKU_ID;
        private int m_Unassign;
        private int m_USER_ID;
        #endregion

        #region Public Properties
        public int Category_ID
        {
            set
            {
                m_Category_ID = value;
            }
            get
            {
                return m_Category_ID;
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

        public int SKU_ID
        {
            set
            {
                m_SKU_ID = value;
            }
            get
            {
                return m_SKU_ID;
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
        public spInsertItemAgainstCategory()
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
                cmd.CommandText = "spInsertItemAgainstECommCategory";
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

        public bool ExecuteQueryForItemAgainstLocation()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spInsertItemAgainstLocation";
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollectionForItemAgainstLocation(ref cmd);
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
                parameter.ParameterName = "@CategoryID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_Category_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Category_ID;
                }
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@SKU_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_SKU_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_SKU_ID;
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
            }
            catch (Exception)
            { }

        }
        public void GetParameterCollectionForItemAgainstLocation(ref IDbCommand cmd)
        {
            try
            {
                IDataParameterCollection pparams = cmd.Parameters;
                IDataParameter parameter;
                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@DISTRIBUTOR_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_Category_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Category_ID;
                }
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@SKU_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_SKU_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_SKU_ID;
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
            }
            catch (Exception)
            { }

        }
        #endregion
    }
}