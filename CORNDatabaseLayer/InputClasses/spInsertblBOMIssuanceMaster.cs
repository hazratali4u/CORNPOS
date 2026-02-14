using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
	public class spInsertblBOMIssuanceMaster
	{
		#region Private Members
		private string sp_Name = "spInsertblBOMIssuanceMaster" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
		private long m_ProductionPlanID;
        private int m_DistributorID;
        private int m_FinishedSKUID;
		private int m_UserID;
		private DateTime m_DocumentDate;
		private decimal m_Issuance_Qty;
        private decimal m_FinishedSKUPrice;
        private long m_lngBOMIssuanceCode;
		#endregion
		#region Public Properties
		public long ProductionPlanID
		{
			set
			{
				m_ProductionPlanID = value ;
			}
			get
			{
				return m_ProductionPlanID;
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
		public decimal Issuance_Qty
		{
			set
			{
				m_Issuance_Qty = value ;
			}
			get
			{
				return m_Issuance_Qty;
			}
		}
        public decimal FinishedSKUPrice
        {
            set { m_FinishedSKUPrice = value; }
            get { return m_FinishedSKUPrice; }
        }
        public long lngBOMIssuanceCode
		{
			get
			{
				return m_lngBOMIssuanceCode;
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
		public spInsertblBOMIssuanceMaster()
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
				m_lngBOMIssuanceCode = (long)((IDataParameter)(cmd.Parameters["@lngBOMIssuanceCode"])).Value;
				return m_lngBOMIssuanceCode;
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
			parameter.ParameterName = "@ProductionPlanID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
			if(m_ProductionPlanID==Constants.LongNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_ProductionPlanID;
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
			parameter.ParameterName = "@Issuance_Qty" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
			if(m_Issuance_Qty==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_Issuance_Qty;
			}
			pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FinishedSKUPrice";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_FinishedSKUPrice == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FinishedSKUPrice;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@lngBOMIssuanceCode" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
			parameter.Direction = ParameterDirection.Output;
			pparams.Add(parameter);


		}
		#endregion
	}
}