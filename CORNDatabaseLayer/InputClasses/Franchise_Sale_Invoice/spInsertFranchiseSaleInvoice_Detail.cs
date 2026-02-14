using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spInsertFranchiseSaleInvoice_Detail
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_DISTRIBUTOR_ID;
        private int m_SKU_ID;
        private decimal m_QUANTITY;
        private bool m_IS_DELETED;
        private decimal m_PRICE;
        private decimal m_AMOUNT;
        private DateTime m_TIME_STAMP;
        private long m_FRANCHISE_MASTER_ID;
        private long m_FRANCHISE_DETAIL_ID;
        private int m_UOM_ID;
        private decimal m_SALES_PER;
        public int m_USER_ID { get; set; }
        public decimal m_PRICE_LEVEL;
        public decimal m_PRICE_LEVEL_FACTOR;
        public long m_SKU_PRICES_LEVEL_ID;
        #endregion

        #region Public Properties
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

        public int DISTRIBUTOR_ID
        {
            set
            {
                m_DISTRIBUTOR_ID = value;
            }
            get
            {
                return m_DISTRIBUTOR_ID;
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


        public DateTime TIME_STAMP
        {
            set
            {
                m_TIME_STAMP = value;
            }
            get
            {
                return m_TIME_STAMP;
            }
        }


        public long FRANCHISE_MASTER_ID
        {
            set
            {
                m_FRANCHISE_MASTER_ID = value;
            }
            get
            {
                return m_FRANCHISE_MASTER_ID;
            }
        }
        public long FRANCHISE_DETAIL_ID
        {
            set
            {
                m_FRANCHISE_DETAIL_ID = value;
            }
            get
            {
                return m_FRANCHISE_DETAIL_ID;
            }
        }
        public bool IS_DELETED
        {
            set
            {
                m_IS_DELETED = value;
            }
            get
            {
                return m_IS_DELETED;
            }
        }

        public decimal SALES_PER
        {
            set { m_SALES_PER = value; }
            get { return m_SALES_PER; }
        }

        public decimal PRICE_LEVEL
        {
            set { m_PRICE_LEVEL = value; }
            get { return m_PRICE_LEVEL; }
        }
        public long SKU_PRICES_LEVEL_ID
        {
            set { m_SKU_PRICES_LEVEL_ID = value; }
            get { return m_SKU_PRICES_LEVEL_ID; }
        }
        public decimal PRICE_LEVEL_FACTOR
        {
            set { m_PRICE_LEVEL_FACTOR = value; }
            get { return m_PRICE_LEVEL_FACTOR; }
        }
        public IDbConnection Connection
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
        public IDbTransaction Transaction
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
        public spInsertFranchiseSaleInvoice_Detail()
        {
            m_DISTRIBUTOR_ID = Constants.IntNullValue;
            m_SKU_ID = Constants.IntNullValue;
            m_QUANTITY = Constants.DecimalNullValue;
            PRICE = Constants.DecimalNullValue;
            AMOUNT = Constants.DecimalNullValue;
            m_FRANCHISE_MASTER_ID = Constants.LongNullValue;
            m_FRANCHISE_DETAIL_ID = Constants.LongNullValue;
            m_UOM_ID = Constants.IntNullValue;
            m_IS_DELETED = false;
            TIME_STAMP = DateTime.Now;
        }
        #endregion

        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spInsertFRANCHISE_SALE_INVOICE_DETAIL";
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                m_FRANCHISE_DETAIL_ID = (long)((IDataParameter)(cmd.Parameters["@FRANCHISE_DETAIL_ID"])).Value;
                return true;
            }
            catch (Exception e)
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
            IDataParameter parameter;
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISTRIBUTOR_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DISTRIBUTOR_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DISTRIBUTOR_ID;
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
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
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
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
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
            parameter.ParameterName = "@TIME_STAMP";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_TIME_STAMP == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TIME_STAMP;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FRANCHISE_MASTER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (FRANCHISE_MASTER_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = FRANCHISE_MASTER_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FRANCHISE_DETAIL_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            parameter.Direction = ParameterDirection.Output;
            parameter.Value = FRANCHISE_DETAIL_ID;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@USER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            parameter.Value = USER_ID;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_DELETED";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = IS_DELETED;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SALES_PER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_SALES_PER == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SALES_PER;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PRICE_LEVEL";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_PRICE_LEVEL == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PRICE_LEVEL;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SKU_PRICES_LEVEL_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_SKU_PRICES_LEVEL_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SKU_PRICES_LEVEL_ID;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PRICE_LEVEL_FACTOR";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_PRICE_LEVEL_FACTOR == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PRICE_LEVEL_FACTOR;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}