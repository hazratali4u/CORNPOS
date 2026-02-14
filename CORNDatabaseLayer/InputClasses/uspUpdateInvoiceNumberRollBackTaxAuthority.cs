using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
	public class uspUpdateInvoiceNumberRollBackTaxAuthority
	{
		#region Private Members
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;


		private long m_SALE_INVOICE_ID;
		private string m_InvoiceNumberRollBackTaxAuthority;
		#endregion

		#region Public Properties
		public long SALE_INVOICE_ID
		{
			set
			{
				m_SALE_INVOICE_ID = value ;
			}
			get
			{
				return m_SALE_INVOICE_ID;
			}
		}


		public string InvoiceNumberRollBackTaxAuthority
		{
			set
			{
				m_InvoiceNumberRollBackTaxAuthority = value ;
			}
			get
			{
				return m_InvoiceNumberRollBackTaxAuthority;
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
		public uspUpdateInvoiceNumberRollBackTaxAuthority()
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
				cmd.CommandText = "uspUpdateInvoiceNumberRollBackTaxAuthority";
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
			parameter.ParameterName = "@SALE_INVOICE_ID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
			if(m_SALE_INVOICE_ID==Constants.LongNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_SALE_INVOICE_ID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@InvoiceNumberRollBackTaxAuthority" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_InvoiceNumberRollBackTaxAuthority == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_InvoiceNumberRollBackTaxAuthority;
            }
			pparams.Add(parameter);

		}
		#endregion
	}
}