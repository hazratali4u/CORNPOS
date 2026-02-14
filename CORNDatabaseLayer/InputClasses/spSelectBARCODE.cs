using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CORNDataAccessLayer.Classes;
using CORNCommon.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spSelectBARCODE
    {
        #region Private Members
        private string sp_Name = "spSelectBARCODE";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private string m_COMPANY_NAME;
        private int m_PRODUCT_NAME;
        private decimal m_PRODUCT_PRICE;
        private Byte[] m_BARCODE_IMAGE;
        #endregion
        #region Public Properties
        public string COMPANY_NAME
        {
            set
            {
                m_COMPANY_NAME = value;
            }
            get
            {
                return m_COMPANY_NAME;
            }
        }
        public int PRODUCT_NAME
        {
            set
            {
                m_PRODUCT_NAME = value;
            }
            get
            {
                return m_PRODUCT_NAME;
            }
        }
        public byte[] BARCODE_IMAGE
        {
            set
            {
                m_BARCODE_IMAGE = value;
            }
            get
            {
                return m_BARCODE_IMAGE;
            }
        }
        public decimal PRODUCT_PRICE
        {
            set
            {
                m_PRODUCT_PRICE = value;
            }
            get
            {
                return m_PRODUCT_PRICE;
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
        public spSelectBARCODE()
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

        }
        #endregion
    }
}