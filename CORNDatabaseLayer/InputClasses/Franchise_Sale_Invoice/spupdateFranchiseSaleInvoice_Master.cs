using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spupdateFranchiseSaleInvoice_Master
    {
        #region Private Members
        private string sp_Name = "spUpdateFRANCHISE_SALE_INVOICE_MASTER";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;

        private int m_DISTRIBUTOR_ID;
        private decimal m_TOTAL_AMOUNT;
        private decimal m_CREDIT_AMOUNT;
        private DateTime m_DOCUMENT_DATE;
        private DateTime m_TIME_STAMP;
        private DateTime m_LAST_UPDATE;
        private long m_FRANCHISE_MASTER_ID;
        private long m_CUSTOMER_ID;
        private string m_REF_NO;
        private string m_REMARKS;
        private int m_USER_ID;
        private bool m_POSTING;
        private bool m_IS_DELETED;
        private decimal m_SALES_PER;
        private int m_STOCK_DEMAND_ID;
        #endregion
        #region Public Properties

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
        public decimal TOTAL_AMOUNT
        {
            set
            {
                m_TOTAL_AMOUNT = value;
            }
            get
            {
                return m_TOTAL_AMOUNT;
            }
        }
        public decimal CREDIT_AMOUNT
        {
            set { m_CREDIT_AMOUNT = value; }
            get { return m_CREDIT_AMOUNT; }
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
        public int STOCK_DEMAND_ID
        {
            set
            {
                m_STOCK_DEMAND_ID = value;
            }
            get
            {
                return m_STOCK_DEMAND_ID;
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
        public DateTime LAST_UPDATE
        {
            set
            {
                m_LAST_UPDATE = value;
            }
            get
            {
                return m_LAST_UPDATE;
            }
        }
        public long FRANCHISE_MASTER_ID
        {
            get
            {
                return m_FRANCHISE_MASTER_ID;
            }
            set
            {
                m_FRANCHISE_MASTER_ID = value;
            }
        }
        public string REMARKS
        {
            set
            {
                m_REMARKS = value;
            }
            get
            {
                return m_REMARKS;
            }
        }
        public long CUSTOMER_ID
        {
            set
            {
                m_CUSTOMER_ID = value;
            }
            get
            {
                return m_CUSTOMER_ID;
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
        public bool POSTING
        {
            set
            {
                m_POSTING = value;
            }
            get
            {
                return m_POSTING;
            }
        }
        public string REF_NO
        {
            set
            {
                m_REF_NO = value;
            }
            get
            {
                return m_REF_NO;
            }
        }
        public decimal SALES_PER
        {
            set { m_SALES_PER = value; }
            get { return m_SALES_PER; }
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
        public spupdateFranchiseSaleInvoice_Master()
        {
            REMARKS = "";
            LAST_UPDATE = DateTime.Now;
            TIME_STAMP = DateTime.Now;
            DOCUMENT_DATE = DateTime.Now;
        }
        #endregion
        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp_Name;
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                m_FRANCHISE_MASTER_ID = (long)((IDataParameter)(cmd.Parameters["@FRANCHISE_MASTER_ID"])).Value;
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
            parameter.ParameterName = "@CUSTOMER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_CUSTOMER_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CUSTOMER_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DOCUMENT_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_DOCUMENT_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DOCUMENT_DATE;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@REF_NO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_REF_NO == "")
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_REF_NO;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@REMARKS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_REMARKS == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_REMARKS;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TOTAL_AMOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_TOTAL_AMOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TOTAL_AMOUNT;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CREDIT_AMOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_CREDIT_AMOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CREDIT_AMOUNT;
            }
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


            //parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            //parameter.ParameterName = "@TIME_STAMP";
            //parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            //if (m_TIME_STAMP == Constants.DateNullValue)
            //{
            //    parameter.Value = DBNull.Value;
            //}
            //else
            //{
            //    parameter.Value = m_TIME_STAMP;
            //}
            //pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LAST_UPDATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_LAST_UPDATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LAST_UPDATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@POSTING";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);

            parameter.Value = m_POSTING;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_DELETED";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);

            parameter.Value = m_IS_DELETED;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FRANCHISE_MASTER_ID";
            // parameter.Direction = ParameterDirection.Output;
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_FRANCHISE_MASTER_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FRANCHISE_MASTER_ID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SALES_PER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
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
            parameter.ParameterName = "@STOCK_DEMAND_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_STOCK_DEMAND_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_STOCK_DEMAND_ID;
            }
            pparams.Add(parameter);
        }
        #endregion
    }
}