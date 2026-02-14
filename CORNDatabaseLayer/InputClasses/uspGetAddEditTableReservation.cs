using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class uspGetAddEditTableReservation
    {
        #region Private Members
        private string sp_Name = "uspGetAddEditTableReservation";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_DistributorID;
        private long m_lngTableReservationMasterCode;
        private long m_TableReservationMasterCode;
        private DateTime m_DocumentDate;
        private DateTime m_ReservationDate;
        private DateTime m_ReservationTime;
        private int m_UserID;
        private int m_NoOfGuest;
        private int m_BooingSlot;
        private long m_CustomerID;
        private bool m_IsActive;
        private int m_TypeID;
        private int m_SourceID;
        private int m_CustomerTypeID;
        private string m_Remarks;
        private string m_CancelationReason;
        private int m_CancelledBy;
        #endregion
        #region Public Properties
        public int DistributorID
        {
            set
            {
                m_DistributorID = value;
            }
            get
            {
                return m_DistributorID;
            }
        }
        public long lngTableReservationMasterCode
        {
            get
            {
               return m_lngTableReservationMasterCode;
            }
        }
        public long TableReservationMasterCode
        {
            set { m_TableReservationMasterCode = value; }
            get { return m_TableReservationMasterCode; }
        }
        public int UserID
        {
            set
            {
                m_UserID = value;
            }
            get
            {
                return m_UserID;
            }
        }
        public DateTime DocumentDate
        {
            set
            {
                m_DocumentDate = value;
            }
            get
            {
                return m_DocumentDate;
            }
        }
        public DateTime ReservationDate
        {
            set { m_ReservationDate = value; }
            get { return m_ReservationDate; }
        }
        public DateTime ReservationTime
        {
            set { m_ReservationTime = value; }
            get { return m_ReservationTime; }
        }
        public int NoOfGuest
        {
            get
            {
                return m_NoOfGuest;
            }
            set { m_NoOfGuest = value; }
        }
        public int BooingSlot
        {
            set { m_BooingSlot = value; }
            get { return m_BooingSlot; }
        }
        public long CustomerID
        {
            set { m_CustomerID = value; }
            get { return m_CustomerID; }
        }
        public bool IsActive
        {
            set { m_IsActive = value; }
            get { return m_IsActive; }
        }
        public int TypeID
        {
            set { m_TypeID = value; }
            get { return m_TypeID; }
        }
        public int SourceID
        {
            set { m_SourceID = value; }
            get { return m_SourceID; }
        }
        public int CustomerTypeID
        {
            set { m_CustomerTypeID = value; }
            get { return m_CustomerTypeID; }
        }
        public string Remarks
        {
            set { m_Remarks = value; }
            get { return m_Remarks; }
        }
        public string CancelationReason
        {
            set { m_CancelationReason = value; }
            get { return m_CancelationReason; }
        }
        public int CancelledBy
        {
            set { m_CancelledBy = value; }
            get { return m_CancelledBy; }
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
        public uspGetAddEditTableReservation()
        {
            m_DistributorID = Constants.IntNullValue;
            m_lngTableReservationMasterCode = Constants.LongNullValue;
            m_DocumentDate = Constants.DateNullValue;
            m_ReservationDate = Constants.DateNullValue;
            m_ReservationTime = Constants.DateNullValue;
            m_UserID = Constants.IntNullValue;
            m_NoOfGuest = Constants.IntNullValue;
            m_BooingSlot = Constants.IntNullValue;
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
        public bool ExecuteQueryUpdate()
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
        public DataTable ExecuteTable()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sp_Name;
                command.Connection = m_connection;
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
            parameter.ParameterName = "@DistributorID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DistributorID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DistributorID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@lngTableReservationMasterCode";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            parameter.Direction = ParameterDirection.Output;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@UserID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_UserID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_UserID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DocumentDate";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_DocumentDate == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DocumentDate;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ReservationDate";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_ReservationDate == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ReservationDate;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ReservationTime";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_ReservationTime == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ReservationTime;
            }
            pparams.Add(parameter);
            
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@NoOfGuest";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_NoOfGuest == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_NoOfGuest;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@BooingSlot";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_BooingSlot == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_BooingSlot;
            }
            pparams.Add(parameter);
            
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CustomerID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_CustomerID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CustomerID;
            }
            pparams.Add(parameter);
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsActive";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);

            parameter.Value = m_IsActive;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TypeID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_TypeID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TypeID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SourceID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SourceID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SourceID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CustomerTypeID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CustomerTypeID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CustomerTypeID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TableReservationMasterCode";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_TableReservationMasterCode == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TableReservationMasterCode;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Remarks";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_Remarks == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Remarks;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CancelationReason";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_CancelationReason == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CancelationReason;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CancelledBy";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CancelledBy == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CancelledBy;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}