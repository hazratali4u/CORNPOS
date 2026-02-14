using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spInsert_USER
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;

        private int m_USER_ID;
        private int m_COMPANY_ID;
        private int m_DISTRIBUTOR_ID;
        private int m_ROLE_ID;
        private DateTime m_LASTUPDATE_DATE;
        private bool m_IS_ACTIVE;
        private string m_LOGIN_ID;
        private string m_PASSWORD;
        private bool m_IsDelRight;
        private bool m_IsLessRight;
        private bool m_IS_CAHSIER;
        private bool m_IS_CanGiveDiscount;
        private bool m_IS_CanReverseDayClose;
        private bool m_Can_DineIn;
        private bool m_Can_Delivery;
        private bool m_Can_TakeAway;
        private bool m_Can_ComplimentaryItem;
        private bool m_Can_PrintOrder;
        private bool m_CanAlterServiceCharges;
        private int m_DefaultServiceType;
        private string m_PRINTER_NAME;
        private bool m_PrintInvoiceFromWS;
        private bool m_AutoStockAdjustment;
        private bool m_CanAlterDeliveryCharges;
        private bool m_CanCancelTableReservation;
        private int m_USER_UPDATE_BY;
        private bool m_IsSplitBill;
        #endregion

        #region Public Properties

        public bool Can_DineIn
        {
            set
            {
                m_Can_DineIn = value;
            }
            get
            {
                return m_Can_DineIn;
            }
        }
        public bool Can_Delivery
        {
            set
            {
                m_Can_Delivery = value;
            }
            get
            {
                return m_Can_Delivery;
            }
        }
        public bool Can_TakeAway
        {
            set
            {
                m_Can_TakeAway = value;
            }
            get
            {
                return m_Can_TakeAway;
            }
        }
        public bool IS_CanReverseDayClose
        {
            set
            {
                m_IS_CanReverseDayClose = value;
            }
            get
            {
                return m_IS_CanReverseDayClose;
            }
        }
        public bool IS_CanGiveDiscount
        {
            set
            {
                m_IS_CanGiveDiscount = value;
            }
            get
            {
                return m_IS_CanGiveDiscount;
            }
        }
        public bool IsLessRight
        {
            set
            {
                m_IsLessRight = value;
            }
            get
            {
                return m_IsLessRight;
            }
        }
        public bool IsDelRight
        {
            set
            {
                m_IsDelRight = value;
            }
            get
            {
                return m_IsDelRight;
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
        public int COMPANY_ID
        {
            set
            {
                m_COMPANY_ID = value;
            }
            get
            {
                return m_COMPANY_ID;
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
        public int ROLE_ID
        {
            set
            {
                m_ROLE_ID = value;
            }
            get
            {
                return m_ROLE_ID;
            }
        }
        public DateTime LASTUPDATE_DATE
        {
            set
            {
                m_LASTUPDATE_DATE = value;
            }
            get
            {
                return m_LASTUPDATE_DATE;
            }
        }
        public bool IS_ACTIVE
        {
            set
            {
                m_IS_ACTIVE = value;
            }
            get
            {
                return m_IS_ACTIVE;
            }
        }
        public string LOGIN_ID
        {
            set
            {
                m_LOGIN_ID = value;
            }
            get
            {
                return m_LOGIN_ID;
            }
        }
        public string PASSWORD
        {
            set
            {
                m_PASSWORD = value;
            }
            get
            {
                return m_PASSWORD;
            }
        }
        public bool IS_CASHIER
        {
            set
            {
                m_IS_CAHSIER = value;
            }
            get
            {
                return m_IS_CAHSIER;
            }
        }
        public bool Can_ComplimentaryItem
        {
            set { m_Can_ComplimentaryItem = value; }
            get { return m_Can_ComplimentaryItem; }
        }
        public bool Can_PrintOrder
        {
            set { m_Can_PrintOrder = value; }
            get { return m_Can_PrintOrder; }
        }
        public bool CanAlterServiceCharges
        {
            set { m_CanAlterServiceCharges = value; }
            get { return m_CanAlterServiceCharges; }
        }
        public int DefaultServiceType
        {
            set { m_DefaultServiceType = value; }
            get { return m_DefaultServiceType; }
        }
        public string PRINTER_NAME
        {
            set { m_PRINTER_NAME = value; }
            get { return m_PRINTER_NAME; }
        }
        public bool PrintInvoiceFromWS
        {
            set { m_PrintInvoiceFromWS = value; }
            get { return m_CanAlterServiceCharges; }
        }
        public bool AutoStockAdjustment
        {
            set { m_AutoStockAdjustment = value; }
            get { return m_AutoStockAdjustment; }
        }
        public bool CanAlterDeliveryCharges
        {
            set { m_CanAlterDeliveryCharges = value; }
            get { return m_CanAlterDeliveryCharges; }
        }
        public bool CanCancelTableReservation
        {
            set { m_CanCancelTableReservation = value; }
            get { return m_CanCancelTableReservation; }
        }
        public int USER_UPDATE_BY
        {
            set { m_USER_UPDATE_BY = value; }
            get { return m_USER_UPDATE_BY; }
        }
        public bool IsSplitBill
        {
            set { m_IsSplitBill = value; }
            get { return m_IsSplitBill; }
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
        public spInsert_USER()
        {
            m_USER_ID = Constants.IntNullValue;
            m_COMPANY_ID = Constants.IntNullValue;
            m_DISTRIBUTOR_ID = Constants.IntNullValue;
            m_ROLE_ID = Constants.IntNullValue;
            m_LASTUPDATE_DATE = Constants.DateNullValue;
            m_IS_ACTIVE = true;
            m_LOGIN_ID = null;
            m_PASSWORD = null;
            m_IsDelRight = false;
            m_IsLessRight = false;
            m_IS_CAHSIER = false;
            m_IS_CanGiveDiscount = false;
            m_IS_CanReverseDayClose = false;
        }
        #endregion

        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spInsert_USER";
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


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@COMPANY_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_COMPANY_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_COMPANY_ID;
            }
            pparams.Add(parameter);


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
            parameter.ParameterName = "@ROLE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ROLE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ROLE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LASTUPDATE_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_LASTUPDATE_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LASTUPDATE_DATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_ACTIVE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_ACTIVE;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LOGIN_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_LOGIN_ID == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LOGIN_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PASSWORD";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_PASSWORD == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PASSWORD;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsDelRight";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IsDelRight;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsLessRight";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IsLessRight;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_CASHIER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_CAHSIER;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_CanGiveDiscount";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_CanGiveDiscount;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_CanReverseDayClose";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_CanReverseDayClose;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Can_DineIn";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_Can_DineIn;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Can_Delivery";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_Can_Delivery;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Can_TakeAway";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_Can_TakeAway;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Can_ComplimentaryItem";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_Can_ComplimentaryItem;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Can_PrintOrder";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_Can_PrintOrder;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CanAlterServiceCharges";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_CanAlterServiceCharges;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DefaultServiceType";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DefaultServiceType == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DefaultServiceType;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PRINTER_NAME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_PRINTER_NAME == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PRINTER_NAME;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PrintInvoiceFromWS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_PrintInvoiceFromWS;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@AutoStockAdjustment";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_AutoStockAdjustment;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CanAlterDeliveryCharges";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_CanAlterDeliveryCharges;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CanCancelTableReservation";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_CanCancelTableReservation;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@USER_UPDATE_BY";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_USER_UPDATE_BY == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_USER_UPDATE_BY;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsSplitBill";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IsSplitBill;
            pparams.Add(parameter);
        }
        #endregion
    }
}