using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using System;
using System.Data;


namespace CORNDatabaseLayer.Classes
{
    public class uspUpdateBankDiscount
    {
        #region Private Members
        private string sp_Name = "uspUpdateBankDiscount";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private long m_BankDiscountID;
        private int m_LocationID;
        private string m_DiscountName;
        private string m_CardNo;
        private string m_Description;
        private decimal m_DiscountPer;
        private decimal m_BankPortion;
        private decimal m_Limit;
        private DateTime m_FromDate;
        private DateTime m_ToDate;
        private int m_UserID;
        #endregion
        #region Public Properties
        public long BankDiscountID
        {
            set { m_BankDiscountID = value; }
            get { return m_BankDiscountID; }
        }
        public int LocationID
        {
            set
            {
                m_LocationID = value;
            }
            get
            {
                return m_LocationID;
            }
        }
        public string DiscountName
        {
            set
            {
                m_DiscountName = value;
            }
            get
            {
                return m_DiscountName;
            }
        }
        public string CardNo
        {
            set { m_CardNo = value; }
            get { return m_CardNo; }
        }
        public string Description
        {
            set
            {
                m_Description = value;
            }
            get
            {
                return m_Description;
            }
        }
        public decimal DiscountPer
        {
            set
            {
                m_DiscountPer = value;
            }
            get
            {
                return m_DiscountPer;
            }
        }
        public decimal BankPortion
        {
            set { m_BankPortion = value; }
            get { return m_BankPortion; }
        }
        public decimal Limit
        {
            set { m_Limit = value; }
            get { return m_Limit; }
        }
        public DateTime FromDate
        {
            set
            {
                m_FromDate = value;
            }
            get
            {
                return m_FromDate;
            }
        }
        public DateTime ToDate
        {
            set
            {
                m_ToDate = value;
            }
            get
            {
                return m_ToDate;
            }
        }
        public int UserID
        {
            set
            {
                m_UserID = value;
            }
            get
            {
                return m_UserID;
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
        public uspUpdateBankDiscount()
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
            parameter.ParameterName = "@BankDiscountID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_BankDiscountID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_BankDiscountID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LocationID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_LocationID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LocationID;
            }
            pparams.Add(parameter);            


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DiscountName";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_DiscountName))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DiscountName;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CardNo";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_CardNo))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CardNo;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Description";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_Description))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Description;
            }
            pparams.Add(parameter);
            

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DiscountPer";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_DiscountPer == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DiscountPer;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@BankPortion";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_BankPortion == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_BankPortion;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Limit";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_Limit == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Limit;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FromDate";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_FromDate == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FromDate;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ToDate";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_ToDate == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ToDate;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@UserID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_UserID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_UserID;
            }
            pparams.Add(parameter);
            
        }
        #endregion
    }
}