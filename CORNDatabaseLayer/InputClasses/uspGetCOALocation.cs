using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
namespace CORNDatabaseLayer.Classes
{
	public class uspGetCOALocation
	{
		#region Private Members
		private string sp_Name = "uspGetCOALocation" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
		private int m_LocationID;
        private int m_ACCOUNT_PARENT_ID;
        private int m_ACCOUNT_CATEGORY_ID;
        private int m_TypeID;
		#endregion
		#region Public Properties
		public int LocationID
		{
			set
			{
				m_LocationID = value ;
			}
			get
			{
				return m_LocationID;
			}
		}
        public int ACCOUNT_PARENT_ID
        {
            set { m_ACCOUNT_PARENT_ID = value; }
            get { return m_ACCOUNT_PARENT_ID; }
        }
        public int ACCOUNT_CATEGORY_ID
        {
            set { m_ACCOUNT_CATEGORY_ID = value; }
            get { return m_ACCOUNT_CATEGORY_ID; }
        }
        public int TypeID
        {
			set
			{
                m_TypeID = value ;
			}
			get
			{
				return m_TypeID;
			}
		}


		public IDbConnection  Connection
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
		public IDbTransaction  Transaction
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
		public uspGetCOALocation()
		{
		}
		#endregion
		#region public Methods
		public DataTable ExecuteTable()
		{
			try
			{
				IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = sp_Name;
				command.Connection = m_connection;
				if(m_transaction!=null)
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
			catch(Exception exp)
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
			IDataParameter parameter ;
			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@LocationID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_LocationID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_LocationID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@TypeID"; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_TypeID == Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_TypeID;
			}
			pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ACCOUNT_PARENT_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ACCOUNT_PARENT_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ACCOUNT_PARENT_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ACCOUNT_CATEGORY_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ACCOUNT_CATEGORY_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ACCOUNT_CATEGORY_ID;
            }
            pparams.Add(parameter);


        }
		#endregion
	}
}