using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
namespace CORNDatabaseLayer.Classes
{
	public class uspUpdateLoyaltyCardSalab
	{
		#region Private Members
		private string sp_Name = "uspUpdateLoyaltyCardSalab" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
		private int m_intLoyaltyCardTypeID;
		private int m_intLoyaltyCardSalabTypeID;
		private int m_intItemID;
		private int m_intUserID;
		private bool m_IsActive;
		private bool m_IsDeleted;
		private decimal m_decPointsFrom;
		private decimal m_decPointsTo;
		private decimal m_decCash;
		private decimal m_decDiscount;
		private decimal m_decFreeUnit;
		private decimal m_decPointsDeduction;
		private long m_lngLoyaltyCardSalabID;
        private int m_intMultipleOf;
        private int m_intLoyaltyCardID;
        #endregion
        #region Public Properties
        public int intLoyaltyCardTypeID
		{
			set
			{
				m_intLoyaltyCardTypeID = value ;
			}
			get
			{
				return m_intLoyaltyCardTypeID;
			}
		}
		public int intLoyaltyCardSalabTypeID
		{
			set
			{
				m_intLoyaltyCardSalabTypeID = value ;
			}
			get
			{
				return m_intLoyaltyCardSalabTypeID;
			}
		}
		public int intItemID
		{
			set
			{
				m_intItemID = value ;
			}
			get
			{
				return m_intItemID;
			}
		}
		public int intUserID
		{
			set
			{
				m_intUserID = value ;
			}
			get
			{
				return m_intUserID;
			}
		}
		public bool IsActive
		{
			set
			{
				m_IsActive = value ;
			}
			get
			{
				return m_IsActive;
			}
		}
		public bool IsDeleted
		{
			set
			{
				m_IsDeleted = value ;
			}
			get
			{
				return m_IsDeleted;
			}
		}
		public decimal decPointsFrom
		{
			set
			{
				m_decPointsFrom = value ;
			}
			get
			{
				return m_decPointsFrom;
			}
		}
		public decimal decPointsTo
		{
			set
			{
				m_decPointsTo = value ;
			}
			get
			{
				return m_decPointsTo;
			}
		}
		public decimal decCash
		{
			set
			{
				m_decCash = value ;
			}
			get
			{
				return m_decCash;
			}
		}
		public decimal decDiscount
		{
			set
			{
				m_decDiscount = value ;
			}
			get
			{
				return m_decDiscount;
			}
		}
		public decimal decFreeUnit
		{
			set
			{
				m_decFreeUnit = value ;
			}
			get
			{
				return m_decFreeUnit;
			}
		}
		public decimal decPointsDeduction
		{
			set
			{
				m_decPointsDeduction = value ;
			}
			get
			{
				return m_decPointsDeduction;
			}
		}
		public long lngLoyaltyCardSalabID
		{
			set
			{
				m_lngLoyaltyCardSalabID = value ;
			}
			get
			{
				return m_lngLoyaltyCardSalabID;
			}
		}
        public int intMultipleOf
        {
            set { m_intMultipleOf = value; }
            get { return m_intMultipleOf; }
        }
        public int intLoyaltyCardID
        {
            set { m_intLoyaltyCardID = value; }
            get { return m_intLoyaltyCardID; }
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
		public uspUpdateLoyaltyCardSalab()
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
			parameter.ParameterName = "@intLoyaltyCardTypeID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_intLoyaltyCardTypeID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_intLoyaltyCardTypeID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@intLoyaltyCardSalabTypeID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_intLoyaltyCardSalabTypeID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_intLoyaltyCardSalabTypeID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@intItemID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_intItemID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_intItemID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@intUserID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_intUserID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_intUserID;
			}
			pparams.Add(parameter);
            
			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@IsActive" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
				parameter.Value = m_IsActive;
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@IsDeleted" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
				parameter.Value = m_IsDeleted;
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@decPointsFrom" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
			if(m_decPointsFrom==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_decPointsFrom;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@decPointsTo" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
			if(m_decPointsTo==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_decPointsTo;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@decCash" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
			if(m_decCash==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_decCash;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@decDiscount" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
			if(m_decDiscount==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_decDiscount;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@decFreeUnit" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
			if(m_decFreeUnit==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_decFreeUnit;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@decPointsDeduction" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
			if(m_decPointsDeduction==Constants.DecimalNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_decPointsDeduction;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@lngLoyaltyCardSalabID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
			if(m_lngLoyaltyCardSalabID==Constants.LongNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_lngLoyaltyCardSalabID;
			}
			pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@intMultipleOf";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_intMultipleOf == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_intMultipleOf;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@intLoyaltyCardID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_intLoyaltyCardID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_intLoyaltyCardID;
            }
            pparams.Add(parameter);


        }
		#endregion
	}
}