using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
	public class spInsertblProductionPlanMaster
	{
		#region Private Members
		private string sp_Name = "spInsertblProductionPlanMaster" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
		private int m_FINISHED_GOOD_MASTER_ID;
        private int m_DistributorID;
        private int m_FinishedSKUID;
		private int m_UserID;
		private DateTime m_DocumentDate;
		private decimal m_Production_Qty;
		private long m_lngProductionPlanCode;
		#endregion
		#region Public Properties
		public int FINISHED_GOOD_MASTER_ID
		{
			set
			{
				m_FINISHED_GOOD_MASTER_ID = value ;
			}
			get
			{
				return m_FINISHED_GOOD_MASTER_ID;
			}
		}
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
        public int FinishedSKUID
		{
			set
			{
				m_FinishedSKUID = value ;
			}
			get
			{
				return m_FinishedSKUID;
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
		public DateTime DocumentDate
		{
			set
			{
				m_DocumentDate = value ;
			}
			get
			{
				return m_DocumentDate;
			}
		}
		public decimal Production_Qty
		{
			set
			{
				m_Production_Qty = value ;
			}
			get
			{
				return m_Production_Qty;
			}
		}
		public long lngProductionPlanCode
		{
			get
			{
				return m_lngProductionPlanCode;
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
		public spInsertblProductionPlanMaster()
		{
		}
		#endregion
		#region public Methods
		public long  ExecuteQuery()
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
				m_lngProductionPlanCode = (long)((IDataParameter)(cmd.Parameters["@lngProductionPlanCode"])).Value;
				return m_lngProductionPlanCode;
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
			parameter.ParameterName = "@FINISHED_GOOD_MASTER_ID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_FINISHED_GOOD_MASTER_ID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_FINISHED_GOOD_MASTER_ID;
			}
			pparams.Add(parameter);

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
			parameter.ParameterName = "@FinishedSKUID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_FinishedSKUID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_FinishedSKUID;
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
			parameter.ParameterName = "@DocumentDate" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
			if(m_DocumentDate==Constants.DateNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_DocumentDate;
			}
			pparams.Add(parameter);


			

			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@Production_Qty" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
			if(m_Production_Qty==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_Production_Qty;
			}
			pparams.Add(parameter);            

			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@lngProductionPlanCode" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
			parameter.Direction = ParameterDirection.Output;
			pparams.Add(parameter);


		}
		#endregion
	}
}