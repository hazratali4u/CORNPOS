using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class UspUpdateVendorLedger
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_Distributor_id;
        private int m_Document_Type_Id;
        private int m_DOCUMENT_NO;
        private decimal m_AMOUNT;
        private long m_VOUCHER_NO;
        private int m_TYPE;
        private string m_REMARKS;
        private long m_ACCOUNT_HEAD_ID;
        private string m_GMVOUCHER_NO;

        #endregion

        #region Public Properties
        public int Distributor_id
        {
            set
            {
                m_Distributor_id = value;
            }
            get
            {
                return m_Distributor_id;
            }
        }


        public int Document_Type_Id
        {
            set
            {
                m_Document_Type_Id = value;
            }
            get
            {
                return m_Document_Type_Id;
            }
        }


        public int DOCUMENT_NO
        {
            set
            {
                m_DOCUMENT_NO = value;
            }
            get
            {
                return m_DOCUMENT_NO;
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


        public long VOUCHER_NO
        {
            set
            {
                m_VOUCHER_NO = value;
            }
            get
            {
                return m_VOUCHER_NO;
            }
        }
        public string GMVOUCHER_NO
        {
            set
            {
                m_GMVOUCHER_NO = value;
            }
            get
            {
                return m_GMVOUCHER_NO;
            }
        }

        public int TYPE
        {
            set
            {
                m_TYPE = value;
            }
            get
            {
                return m_TYPE;
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
        public long ACCOUNT_HEAD_ID
        {
            set
            {
                m_ACCOUNT_HEAD_ID = value;
            }
            get
            {
                return m_ACCOUNT_HEAD_ID;
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
        public UspUpdateVendorLedger()
        {
            m_Distributor_id = Constants.IntNullValue;
            m_Document_Type_Id = Constants.IntNullValue;
            m_DOCUMENT_NO = Constants.IntNullValue;
            m_AMOUNT = Constants.DecimalNullValue;
            m_VOUCHER_NO = Constants.LongNullValue;
            m_TYPE = Constants.IntNullValue;
            m_REMARKS = null;
            m_ACCOUNT_HEAD_ID = Constants.LongNullValue;
            m_GMVOUCHER_NO = null;
        }
        #endregion

        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UspUpdateVendorLedger";
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
            parameter.ParameterName = "@Distributor_id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Distributor_id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Distributor_id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Document_Type_Id";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Document_Type_Id == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Document_Type_Id;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DOCUMENT_NO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DOCUMENT_NO == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DOCUMENT_NO;
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
            parameter.ParameterName = "@VOUCHER_NO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_VOUCHER_NO == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_VOUCHER_NO;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TYPE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_TYPE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TYPE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@REMARKS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
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
            parameter.ParameterName = "@ACCOUNT_HEAD_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_ACCOUNT_HEAD_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ACCOUNT_HEAD_ID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@GMVOUCHER_NO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_GMVOUCHER_NO == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_GMVOUCHER_NO;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}