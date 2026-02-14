using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spUpdateInvoice
    {
        #region Private Members
        private string sp_Name = "spUpdateInvoice";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private long m_SALE_INVOICE_ID;
        private decimal m_AMOUNTDUE;
        private decimal m_DISCOUNT;
        private decimal m_GST;
        private decimal m_SERVICE_CHARGES;
        private decimal m_ITEM_DISCOUNT;
        private int m_PaymentMode;
        #endregion
        #region Public Properties

        public decimal AMOUNTDUE
        {
            set
            {
                m_AMOUNTDUE = value;
            }
            get
            {
                return m_AMOUNTDUE;
            }
        }
        public decimal DISCOUNT
        {
            set
            {
                m_DISCOUNT = value;
            }
            get
            {
                return m_DISCOUNT;
            }
        }
        public decimal GST
        {
            set { m_GST = value; }
            get { return m_GST; }
        }
        public decimal SERVICE_CHARGES
        {
            set { m_SERVICE_CHARGES = value; }
            get { return m_SERVICE_CHARGES; }
        }
        public decimal ITEM_DISCOUNT
        {
            set { m_ITEM_DISCOUNT = value; }
            get { return m_ITEM_DISCOUNT; }
        }
        public long SALE_INVOICE_ID
        {
            set
            {
                m_SALE_INVOICE_ID = value;
            }
            get
            {
                return m_SALE_INVOICE_ID;
            }
        }
        public int PaymentMode
        {
            set { m_PaymentMode = value; }
            get { return m_PaymentMode; }
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
        public spUpdateInvoice()
        {
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
            parameter.ParameterName = "@AMOUNTDUE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_AMOUNTDUE == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_AMOUNTDUE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISCOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_DISCOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DISCOUNT;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@GST";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_GST == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_GST;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SERVICE_CHARGES";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_SERVICE_CHARGES == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SERVICE_CHARGES;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ITEM_DISCOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_ITEM_DISCOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ITEM_DISCOUNT;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SALE_INVOICE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_SALE_INVOICE_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SALE_INVOICE_ID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PaymentMode";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PaymentMode == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PaymentMode;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}