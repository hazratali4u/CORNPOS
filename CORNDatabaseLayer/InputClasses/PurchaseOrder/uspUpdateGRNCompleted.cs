using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class uspUpdateGRNCompleted
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
                m_Distributor_ID = value;
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
            set
            {
                m_ID = value;
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
        public uspUpdateGRNCompleted()
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
                cmd.CommandText = "uspUpdateGRNCompleted";
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
            try
            {
                IDataParameterCollection pparams = cmd.Parameters;
                IDataParameter parameter;

                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@ID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
                if (m_ID == Constants.LongNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_ID;
                }
                pparams.Add(parameter);

            }
            catch (Exception)
            { }

        }
        #endregion
    }
}