using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spSelectFranchiseSaleInvoice_Detail
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
        public int m_USER_ID { get; set; }
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
        public spSelectFranchiseSaleInvoice_Detail()
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
        public DataTable ExecuteTable()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spSELECTFRANCHISE_SALE_INVOICE_DETAIL";
                command.Connection = m_connection;
                if (m_transaction != null)
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
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {


            }
        }
        public DataTable ExecuteTableForSaleDetail()
        {
            try
            {
                IDbCommand command = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spSelectSALE_INVOICE_DETAIL";
                command.Connection = m_connection;
                if (m_transaction != null)
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
            catch (Exception exp)
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
            IDataParameter parameter;

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

        }
        #endregion
    }
}