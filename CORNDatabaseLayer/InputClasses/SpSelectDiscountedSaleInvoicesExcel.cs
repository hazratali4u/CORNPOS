using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
	public class SpSelectDiscountedSaleInvoicesExcel
	{
		#region Private Members
		private string sp_Name = "SpSelectDiscountedSaleInvoicesExcel" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
		private string m_DISTRIBUTOR_ID;
		private int m_CASHIER_ID;
		private DateTime m_FROMDATE;
		private DateTime m_TODATE;
        private int m_EmployeeDiscountTypeID;
        #endregion
        #region Public Properties
        public string DISTRIBUTOR_ID
		{
			set
			{
				m_DISTRIBUTOR_ID = value ;
			}
			get
			{
				return m_DISTRIBUTOR_ID;
			}
		}
		public int CASHIER_ID
		{
			set
			{
				m_CASHIER_ID = value ;
			}
			get
			{
				return m_CASHIER_ID;
			}
		}
		public DateTime FROMDATE
		{
			set
			{
				m_FROMDATE = value ;
			}
			get
			{
				return m_FROMDATE;
			}
		}
		public DateTime TODATE
		{
			set
			{
				m_TODATE = value ;
			}
			get
			{
				return m_TODATE;
			}
		}
        public int EmployeeDiscountTypeID
        {
            set { m_EmployeeDiscountTypeID = value; }
            get { return m_EmployeeDiscountTypeID; }
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
		public SpSelectDiscountedSaleInvoicesExcel()
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
			parameter.ParameterName = "@DISTRIBUTOR_ID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
			if(m_DISTRIBUTOR_ID==null)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_DISTRIBUTOR_ID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@CASHIER_ID" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_CASHIER_ID==Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_CASHIER_ID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@FROMDATE" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
			if(m_FROMDATE==Constants.DateNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_FROMDATE;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@TODATE" ; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
			if(m_TODATE==Constants.DateNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_TODATE;
			}
			pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@EmployeeDiscountTypeID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_EmployeeDiscountTypeID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_EmployeeDiscountTypeID;
            }
            pparams.Add(parameter);


        }
		#endregion
	}
}