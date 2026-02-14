using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
	public class spInsertPurchaseOrderDetail
    {
		#region Private Members
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;


		private long m_Purchase_Order_Master_ID;
		private int m_SKU_ID;
        private decimal m_QUANTITY;
        private decimal m_PRICE;
        private decimal m_AMOUNT;
        private int m_UOM_ID;
        private decimal m_Tax_Percentage;
        private decimal m_Discount_Percentage;
        #endregion

        #region Public Properties
        public long Purchase_Order_Master_ID
        {
			set
			{
                m_Purchase_Order_Master_ID = value ;
			}
			get
			{
				return m_Purchase_Order_Master_ID;
			}
		}

		public int SKU_ID
        {
			set
			{
                m_SKU_ID = value;
			}
			get
			{
				return m_SKU_ID;
			}
		}

        public decimal QUANTITY
        {
            set
            {
                m_QUANTITY = value;
            }

            get
            {
                return m_QUANTITY;
            }
        }

        public decimal PRICE
        {
            set
            {
                m_PRICE = value;
            }

            get
            {
                return m_PRICE;
            }
        }

        public decimal AMOUNT
        {
            set
            {
                m_AMOUNT = value;
            }

            get
            {
                return m_AMOUNT;
            }
        }

        public int UOM_ID
        {
            set
            {
                m_UOM_ID = value;
            }

            get
            {
                return m_UOM_ID;
            }
        }

        public decimal Tax_Percentage
        {
            set
            {
                m_Tax_Percentage = value;
            }

            get
            {
                return m_Tax_Percentage;
            }
        }

        public decimal Discount_Percentage
        {
            set
            {
                m_Discount_Percentage = value;
            }

            get
            {
                return m_Discount_Percentage;
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
		public spInsertPurchaseOrderDetail()
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
				cmd.CommandText = "spInsertPurchaseOrderDetail";
				cmd.Connection =   m_connection;
				if(m_transaction!=null)
				{
					cmd.Transaction = m_transaction;
				}
				GetParameterCollection(ref cmd);
				cmd.ExecuteNonQuery();
				//m_ROLE_DETAIL_ID = (int)((IDataParameter)(cmd.Parameters["@ROLE_DETAIL_ID"])).Value;
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
                parameter.ParameterName = "@Purchase_Order_Master_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
                if (m_Purchase_Order_Master_ID == Constants.LongNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Purchase_Order_Master_ID;
                }
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@SKU_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_SKU_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_SKU_ID;
                }
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@QUANTITY";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_QUANTITY == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_QUANTITY;
                }
                pparams.Add(parameter);

               


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@PRICE";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_PRICE == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_PRICE;
                }
                pparams.Add(parameter);




                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@AMOUNT";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_AMOUNT == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_AMOUNT;
                }
                pparams.Add(parameter);




                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@UOM_ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_UOM_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_UOM_ID;
                }
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Tax_Percentage";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_Tax_Percentage == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Tax_Percentage;
                }
                pparams.Add(parameter);




                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@Discount_Percentage";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
                if (m_Discount_Percentage == Constants.DecimalNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Discount_Percentage;
                }
                pparams.Add(parameter);

            }
            catch (Exception)
            { }

        }
        #endregion
    }
}