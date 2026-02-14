using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
	public class spUpdatetblRecipeProductionMaster
	{
		#region Private Members
		private string sp_Name = "spUpdatetblRecipeProductionMaster" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
		
		private int m_USER_ID;
		private DateTime m_Production_DATE;
		private DateTime m_LASTUPDATE_DATE;
		private bool m_IS_ACTIVE;
		private decimal m_ActualProduction_Qty;
		private long m_lngRecipeProductionCode;
        private int m_TypeID;
        #endregion
        #region Public Properties


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
		public DateTime Production_DATE
		{
			set
			{
				m_Production_DATE = value ;
			}
			get
			{
				return m_Production_DATE;
			}
		}
		public DateTime LASTUPDATE_DATE
		{
			set
			{
				m_LASTUPDATE_DATE = value ;
			}
			get
			{
				return m_LASTUPDATE_DATE;
			}
		}
		public bool IS_ACTIVE
		{
			set
			{
				m_IS_ACTIVE = value ;
			}
			get
			{
				return m_IS_ACTIVE;
			}
		}
		
		public decimal ActualProduction_Qty
		{
			set
			{
				m_ActualProduction_Qty = value ;
			}
			get
			{
				return m_ActualProduction_Qty;
			}
		}
		public long lngRecipeProductionCode
		{
			set
			{
				m_lngRecipeProductionCode = value ;
			}
			get
			{
				return m_lngRecipeProductionCode;
			}
		}
        public int TypeID
        {
            set { m_TypeID = value; }
            get { return m_TypeID; }
        }
        public string REMARKS { get; set; }

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
		public spUpdatetblRecipeProductionMaster()
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
			parameter.ParameterName = "@Production_DATE" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
			if(m_Production_DATE==Constants.DateNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_Production_DATE;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@LASTUPDATE_DATE" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
			if(m_LASTUPDATE_DATE==Constants.DateNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_LASTUPDATE_DATE;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@IS_ACTIVE" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
				parameter.Value = m_IS_ACTIVE;
			pparams.Add(parameter);

            

			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@ActualProduction_Qty" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
			if(m_ActualProduction_Qty==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_ActualProduction_Qty;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@lngRecipeProductionCode" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
			if(m_lngRecipeProductionCode==Constants.LongNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_lngRecipeProductionCode;
			}
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
            parameter.ParameterName = "@REMARKS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(REMARKS))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = REMARKS;
            }
            pparams.Add(parameter);
        }
		#endregion
	}
}