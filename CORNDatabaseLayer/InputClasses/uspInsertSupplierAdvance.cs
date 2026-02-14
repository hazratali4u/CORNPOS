using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
namespace CORNDatabaseLayer.Classes
{
	public class uspInsertSupplierAdvance
	{
		#region Private Members
		private string sp_Name = "uspInsertSupplierAdvance" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
		private int m_LocationID;
		private int m_SupplierID;
		private int m_AdvanceType;
		private DateTime m_AdvanceDate;
		private int m_UserID;
		private long m_SupplierAdvanceID;
		private decimal m_AdvanceAmount;
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
		public int SupplierID
		{
			set
			{
				m_SupplierID = value ;
			}
			get
			{
				return m_SupplierID;
			}
		}
		public int AdvanceType
		{
			set
			{
				m_AdvanceType = value ;
			}
			get
			{
				return m_AdvanceType;
			}
		}
		public DateTime AdvanceDate
		{
			set
			{
				m_AdvanceDate = value ;
			}
			get
			{
				return m_AdvanceDate;
			}
		}
		public int UserID
		{
			set
			{
				m_UserID = value ;
			}
			get
			{
				return m_UserID;
			}
		}
		public long SupplierAdvanceID
		{
			get
			{
				return m_SupplierAdvanceID;
			}
		}
		public decimal  AdvanceAmount
		{
			set
			{
				m_AdvanceAmount = value ;
			}
			get
			{
				return m_AdvanceAmount;
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
		public uspInsertSupplierAdvance()
		{
		}
		#endregion
		#region public Methods
		public bool  ExecuteQuery()
		{
			try
			{
			    IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
				cmd.CommandType =  CommandType.StoredProcedure;
				cmd.CommandText = sp_Name;
				cmd.Connection =   m_connection;
				if(m_transaction!=null)
				{
					cmd.Transaction = m_transaction;
				}
				GetParameterCollection(ref cmd);
				cmd.ExecuteNonQuery();
				m_SupplierAdvanceID = (long)((IDataParameter)(cmd.Parameters["@SupplierAdvanceID"])).Value;
				return true;
			}
			catch(Exception e)
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
			parameter.ParameterName = "@SupplierID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_SupplierID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_SupplierID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@AdvanceType" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_AdvanceType==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_AdvanceType;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@AdvanceDate" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
			if(m_AdvanceDate==Constants.DateNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_AdvanceDate;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@UserID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_UserID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_UserID;
			}
			pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@SupplierAdvanceID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
			parameter.Direction = ParameterDirection.Output;
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@AdvanceAmount" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
			if(m_AdvanceAmount==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_AdvanceAmount;
			}
			pparams.Add(parameter);


		}
		#endregion
	}
}
