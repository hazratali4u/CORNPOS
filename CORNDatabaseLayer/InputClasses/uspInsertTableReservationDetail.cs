using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class uspInsertTableReservationDetail
    {
        #region Private Members
        private string sp_Name = "uspInsertTableReservationDetail";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private long m_lngTableReservationMasterCode;
        public int m_TableID;
        #endregion
        #region Public Properties
        public long lngTableReservationMasterCode
        {
            get
            {
               return m_lngTableReservationMasterCode;
            }
            set
            {
                m_lngTableReservationMasterCode = value;
            }
        }
        public int TableID
        {
            set { m_TableID = value; }
            get { return m_TableID; }
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
        public uspInsertTableReservationDetail()
        {
        }
        #endregion
        #region public Methods
        public long ExecuteQuery()
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
                return m_lngTableReservationMasterCode= (long)((IDataParameter)(cmd.Parameters["@lngTableReservationMasterCode"])).Value;
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
            parameter.ParameterName = "@lngTableReservationMasterCode";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_lngTableReservationMasterCode == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_lngTableReservationMasterCode;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TableID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_TableID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TableID;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}