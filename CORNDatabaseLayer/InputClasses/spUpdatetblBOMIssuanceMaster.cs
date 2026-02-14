using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
	public class spUpdatetblBOMIssuanceMaster
	{
		#region Private Members
		private string sp_Name = "spUpdatetblBOMIssuanceMaster" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
        private int m_TypeID;
        private int m_USER_ID;
		private decimal m_Issuance_Qty;
        private decimal m_FinishedSKUPrice;
        private long m_lngBOMIssuanceCode;
        private bool m_IsActive;
        #endregion
        #region Public Properties

        public int TypeID
        {
            set { m_TypeID = value; }
            get { return m_TypeID; }
        }
        public int USER_ID
		{
			set
			{
				m_USER_ID = value ;
			}
			get
			{
				return m_USER_ID;
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
			set
			{
				m_lngBOMIssuanceCode = value ;
			}
			get
			{
				return m_lngBOMIssuanceCode;
			}
		}
        public bool IsActive
        {
            set { m_IsActive = value; }
            get { return m_IsActive; }
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
		public spUpdatetblBOMIssuanceMaster()
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
			parameter.ParameterName = "@USER_ID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_USER_ID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_USER_ID;
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
			parameter.ParameterName = "@lngBOMIssuanceCode" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
			if(m_lngBOMIssuanceCode==Constants.LongNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_lngBOMIssuanceCode;
			}
			pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FinishedSKUPrice";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_FinishedSKUPrice == Constants.DebitClaim)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FinishedSKUPrice;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsActive";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IsActive;
            pparams.Add(parameter);


        }
		#endregion
	}
}