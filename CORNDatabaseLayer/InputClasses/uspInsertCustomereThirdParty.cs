using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;



namespace CORNDatabaseLayer.Classes
{
    public class uspInsertCustomereThirdParty:IDisposable
    {
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here, if any
                    // e.g., managedResource?.Dispose();
                }
                // Mark as disposed
                disposed = true;
            }
        }
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;

        private int m_ThirdPartyDeliveryID;
        private int m_LocationID;
        private int m_UserID;
        private string m_Name;
        private string m_ContactNo;
        private string m_Address;

        #endregion

        #region Public Properties

        public int ThirdPartyDeliveryID
        {
            set
            {
                m_ThirdPartyDeliveryID = value;
            }
            get
            {
                return m_ThirdPartyDeliveryID;
            }
        }

        public int LocationID
        {
            set
            {
                m_LocationID = value;
            }
            get
            {
                return m_LocationID;
            }
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

        public string Name
        {
            set
            {
                m_Name = value;
            }
            get
            {
                return m_Name;
            }
        }

        public string ContactNo
        {
            set { m_ContactNo = value; }
            get { return m_ContactNo; }
        }

        public string Address
        {
            set { m_Address = value; }
            get { return m_Address; }
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
        public uspInsertCustomereThirdParty()
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
                cmd.CommandText = "uspInsertCustomereThirdParty";
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
                command.CommandText = "uspInsertCustomereThirdParty";
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
            parameter.ParameterName = "@ThirdPartyDeliveryID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ThirdPartyDeliveryID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ThirdPartyDeliveryID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LocationID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_LocationID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LocationID;
            }
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
            parameter.ParameterName = "@Name";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_Name == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Name;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ContactNo";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ContactNo == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ContactNo;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Address";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_Address == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Address;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}