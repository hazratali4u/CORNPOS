using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
	public class spInsertPurchaseOrderMaster
    {
		#region Private Members
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;


		private int m_Distributor_ID;
		private int m_PRINCIPAL_ID;
        private int m_Payment_Mode;
        private string m_Remarks;
        private DateTime? m_Delivery_Date;
        private DateTime? m_Expiry_Date;
        private decimal m_GST_Amount;
        private decimal m_Discount_Amount;
        private decimal m_Net_Amount;
        private decimal m_Gross_Amount;
        private decimal m_Freight_Charges;
        private DateTime m_DOCUMENT_DATE;
        private int m_USER_ID;
        private long m_ID;
        #endregion

        #region Public Properties
        public int Distributor_ID
		{
			set
			{
                m_Distributor_ID = value ;
			}
			get
			{
				return m_Distributor_ID;
			}
		}

		public int PRINCIPAL_ID
        {
			set
			{
                m_PRINCIPAL_ID = value;
			}
			get
			{
				return m_PRINCIPAL_ID;
			}
		}

        public int Payment_Mode
        {
            set
            {
                m_Payment_Mode = value;
            }

            get
            {
                return m_Payment_Mode;
            }
        }

        public string Remarks
        {
            set
            {
                m_Remarks = value;
            }

            get
            {
                return m_Remarks;
            }
        }

        public DateTime? Delivery_Date
        {
            set
            {
                m_Delivery_Date = value;
            }

            get
            {
                return m_Delivery_Date;
            }
        }

        public DateTime? Expiry_Date
        {
            set
            {
                m_Expiry_Date = value;
            }

            get
            {
                return m_Expiry_Date;
            }
        }

        public decimal GST_Amount
        {
            set
            {
                m_GST_Amount = value;
            }

            get
            {
                return m_GST_Amount;
            }
        }

        public decimal Discount_Amount
        {
            set
            {
                m_Discount_Amount = value;
            }

            get
            {
                return m_Discount_Amount;
            }
        }

        public decimal Net_Amount
        {
            set
            {
                m_Net_Amount = value;
            }

            get
            {
                return m_Net_Amount;
            }
        }

        public decimal Gross_Amount
        {
            set
            {
                m_Gross_Amount = value;
            }

            get
            {
                return m_Gross_Amount;
            }
        }

        public decimal Freight_Charges
        {
            set
            {
                m_Freight_Charges = value;
            }

            get
            {
                return m_Freight_Charges;
            }
        }

        public DateTime DOCUMENT_DATE
        {
            set
            {
                m_DOCUMENT_DATE = value;
            }

            get
            {
                return m_DOCUMENT_DATE;
            }
        }

        public int USER_ID
        {
            set
            {
                m_USER_ID = value;
            }

            get
            {
                return m_USER_ID;
            }
        }

        public long ID
        {
            get
            {
                return m_ID;
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
		public spInsertPurchaseOrderMaster()
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
				cmd.CommandText = "spInsertPurchaseOrderMaster";
				cmd.Connection =   m_connection;
				if(m_transaction!=null)
				{
					cmd.Transaction = m_transaction;
				}
				GetParameterCollection(ref cmd);
				cmd.ExecuteNonQuery();
				m_ID = (long)((IDataParameter)(cmd.Parameters["@ID"])).Value;
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
            try
            {
                IDataParameterCollection pparams = cmd.Parameters;
                IDataParameter parameter;

                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
                parameter.Direction = ParameterDirection.Output;
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@DISTRIBUTOR_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_Distributor_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Distributor_ID;
                }
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@PRINCIPAL_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_PRINCIPAL_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_PRINCIPAL_ID;
                }
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Payment_Mode";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_Payment_Mode == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Payment_Mode;
                }
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Remarks";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
                parameter.Value = m_Remarks;
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Delivery_Date";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
                if (m_Delivery_Date == Constants.DateNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Delivery_Date;
                }
                pparams.Add(parameter);




                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Expiry_Date";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
                if (m_Expiry_Date == Constants.DateNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Expiry_Date;
                }
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Freight_Charges";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_Freight_Charges == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Freight_Charges;
                }
                pparams.Add(parameter);




                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@GST_Amount";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_GST_Amount == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_GST_Amount;
                }
                pparams.Add(parameter);




                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Discount_Amount";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_Discount_Amount == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Discount_Amount;
                }
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Net_Amount";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_Net_Amount == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Net_Amount;
                }
                pparams.Add(parameter);




                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Gross_Amount";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_Gross_Amount == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Gross_Amount;
                }
                pparams.Add(parameter);




                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@DOCUMENT_DATE";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
                parameter.Value = m_DOCUMENT_DATE;
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@USER_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_USER_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_USER_ID;
                }
                pparams.Add(parameter);
            }
            catch (Exception)
            { }

        }
        #endregion
    }
}