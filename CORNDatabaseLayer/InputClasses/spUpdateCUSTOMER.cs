using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spUpdateCUSTOMER
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;

        private byte m_IS_STAND;
        private byte m_IS_COOLER;
        private int m_DISTRIBUTOR_ID;
        private int m_TOWN_ID;
        private int m_AREA_ID;
        private int m_ROUTE_ID;
        private int m_CHANNEL_TYPE_ID;
        private int m_BUSINESS_TYPE_ID;
        private int m_PROMOTION_CLASS;
        private DateTime m_REGDATE;
        private DateTime m_TIME_STAMP;
        private DateTime m_LASTUPDATE_DATE;
        private bool m_IS_GST_REGISTERED;
        private bool m_IS_ACTIVE;
        private long m_CUSTOMER_ID;
        private string m_CUSTOMER_CODE;
        private string m_CUSTOMER_NAME;
        private string m_ADDRESS;
        private string m_CONTACT_PERSON;
        private string m_CONTACT_NUMBER;
        private string m_EMAIL_ADDRESS;
        private string m_CNIC;
        private string m_GST_NUMBER;
        private string m_NTN;

        private string m_BARCODE;
        private decimal m_OPENING_AMOUNT;
        private string m_NATURE;
        private string m_CONTACT2;

        private int m_CARD_TYPE_ID;
        private decimal m_DISCOUNT;
        private decimal m_PURCHASING;
        private decimal m_POINTS;
        private decimal m_AMOUNT_LIMIT;
        private string m_strCardNo;
        private decimal m_SALES_PER;
        private DateTime m_Membership_Date;
        private string m_Spouse_Name;
        private int m_Profession;
        private string m_Membership_No;
        private int m_CUSTOMER_CATEGORY_ID;
        private int m_CUSTOMER_STATUS_ID;
        private bool m_IsGolfer;
        private DateTime m_TO_DATE;
        private DateTime m_FROM_DATE;
        private int m_GroupID;
        private int m_LOYALTY_CARD_ID;
        #endregion

        #region Public Properties
        public int CUSTOMER_CATEGORY_ID
        {
            set
            {
                m_CUSTOMER_CATEGORY_ID = value;
            }
            get
            {
                return m_CUSTOMER_CATEGORY_ID;
            }
        }
        public int CUSTOMER_STATUS_ID
        {
            set
            {
                m_CUSTOMER_STATUS_ID = value;
            }
            get
            {
                return m_CUSTOMER_STATUS_ID;
            }
        }
        public bool IsGolfer
        {
            set
            {
                m_IsGolfer = value;
            }
            get
            {
                return m_IsGolfer;
            }
        }
        public DateTime FROM_DATE
        {
            set
            {
                m_FROM_DATE = value;
            }
            get
            {
                return m_FROM_DATE;
            }
        }
        public DateTime TO_DATE
        {
            set
            {
                m_TO_DATE = value;
            }
            get
            {
                return m_TO_DATE;
            }
        }
        public DateTime Membership_Date
        {
            set
            {
                m_Membership_Date = value;
            }
            get
            {
                return m_Membership_Date;
            }
        }
        public string Spouse_Name
        {
            set
            {
                m_Spouse_Name = value;
            }
            get
            {
                return m_Spouse_Name;
            }
        }
        public string Membership_No
        {
            set
            {
                m_Membership_No = value;
            }
            get
            {
                return m_Membership_No;
            }
        }
        public int Profession
        {
            set
            {
                m_Profession = value;
            }
            get
            {
                return m_Profession;
            }
        }
        public int CARD_TYPE_ID
        {
            set
            {
                m_CARD_TYPE_ID = value;
            }
            get
            {
                return m_CARD_TYPE_ID;
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
        public decimal PURCHASING
        {
            set
            {
                m_PURCHASING = value;
            }
            get
            {
                return m_PURCHASING;
            }
        }
        public decimal POINTS
        {
            set
            {
                m_POINTS = value;
            }
            get
            {
                return m_POINTS;
            }
        }
        public decimal AMOUNT_LIMIT
        {
            set
            {
                m_AMOUNT_LIMIT = value;
            }
            get
            {
                return m_AMOUNT_LIMIT;
            }
        }

        public string strCardNo
        {
            set
            {
                m_strCardNo = value;
            }
            get
            {
                return m_strCardNo;
            }
        }

        public byte IS_STAND
        {
            set
            {
                m_IS_STAND = value;
            }
            get
            {
                return m_IS_STAND;
            }
        }


        public byte IS_COOLER
        {
            set
            {
                m_IS_COOLER = value;
            }
            get
            {
                return m_IS_COOLER;
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


        public int TOWN_ID
        {
            set
            {
                m_TOWN_ID = value;
            }
            get
            {
                return m_TOWN_ID;
            }
        }


        public int AREA_ID
        {
            set
            {
                m_AREA_ID = value;
            }
            get
            {
                return m_AREA_ID;
            }
        }


        public int ROUTE_ID
        {
            set
            {
                m_ROUTE_ID = value;
            }
            get
            {
                return m_ROUTE_ID;
            }
        }


        public int CHANNEL_TYPE_ID
        {
            set
            {
                m_CHANNEL_TYPE_ID = value;
            }
            get
            {
                return m_CHANNEL_TYPE_ID;
            }
        }


        public int BUSINESS_TYPE_ID
        {
            set
            {
                m_BUSINESS_TYPE_ID = value;
            }
            get
            {
                return m_BUSINESS_TYPE_ID;
            }
        }


        public int PROMOTION_CLASS
        {
            set
            {
                m_PROMOTION_CLASS = value;
            }
            get
            {
                return m_PROMOTION_CLASS;
            }
        }


        public DateTime REGDATE
        {
            set
            {
                m_REGDATE = value;
            }
            get
            {
                return m_REGDATE;
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


        public bool IS_GST_REGISTERED
        {
            set
            {
                m_IS_GST_REGISTERED = value;
            }
            get
            {
                return m_IS_GST_REGISTERED;
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


        public string CUSTOMER_CODE
        {
            set
            {
                m_CUSTOMER_CODE = value;
            }
            get
            {
                return m_CUSTOMER_CODE;
            }
        }


        public string CUSTOMER_NAME
        {
            set
            {
                m_CUSTOMER_NAME = value;
            }
            get
            {
                return m_CUSTOMER_NAME;
            }
        }


        public string ADDRESS
        {
            set
            {
                m_ADDRESS = value;
            }
            get
            {
                return m_ADDRESS;
            }
        }


        public string CONTACT_PERSON
        {
            set
            {
                m_CONTACT_PERSON = value;
            }
            get
            {
                return m_CONTACT_PERSON;
            }
        }


        public string CONTACT_NUMBER
        {
            set
            {
                m_CONTACT_NUMBER = value;
            }
            get
            {
                return m_CONTACT_NUMBER;
            }
        }


        public string EMAIL_ADDRESS
        {
            set
            {
                m_EMAIL_ADDRESS = value;
            }
            get
            {
                return m_EMAIL_ADDRESS;
            }
        }

        public string CNIC
        {
            set
            {
                m_CNIC = value;
            }
            get
            {
                return m_CNIC;
            }
        }

        public string GST_NUMBER
        {
            set
            {
                m_GST_NUMBER = value;
            }
            get
            {
                return m_GST_NUMBER;
            }
        }

        public string NTN
        {
            set
            {
                m_NTN = value;
            }
            get
            {
                return m_NTN;
            }
        }

        public string BARCODE
        {
            set
            {
                m_BARCODE = value;
            }
            get
            {
                return m_BARCODE;
            }
        }
        public decimal OPENING_AMOUNT
        {
            set
            {
                m_OPENING_AMOUNT = value;
            }
            get
            {
                return m_OPENING_AMOUNT;
            }
        }
        public string NATURE
        {
            set
            {
                m_NATURE = value;
            }
            get
            {
                return m_NATURE;
            }
        }

        public string CONTACT2
        {
            set
            {
                m_CONTACT2 = value;
            }
            get
            {
                return m_CONTACT2;
            }
        }

        public decimal SALES_PER
        {
            set { m_SALES_PER = value; }
            get { return m_SALES_PER; }
        }

        public int GroupID
        {
            set { m_GroupID = value; }
            get { return m_GroupID; }
        }
        public int LOYALTY_CARD_ID
        {
            set { m_LOYALTY_CARD_ID = value; }
            get { return m_LOYALTY_CARD_ID; }
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
        public spUpdateCUSTOMER()
        {
            m_POINTS = Constants.DecimalNullValue;
            m_PURCHASING = Constants.DecimalNullValue;
            m_DISCOUNT = Constants.DecimalNullValue;
            m_AMOUNT_LIMIT = Constants.DecimalNullValue;
            m_CARD_TYPE_ID = Constants.IntNullValue;
            m_strCardNo = null;


        }
        #endregion

        #region public Methods
        public bool ExecuteQuery()
        {
            try
            {
                IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spUpdateCUSTOMER";
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
            parameter.ParameterName = "@IS_STAND";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.TinyInt);
            if (m_IS_STAND == Constants.ByteNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_IS_STAND;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_COOLER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.TinyInt);
            if (m_IS_COOLER == Constants.ByteNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_IS_COOLER;
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
            parameter.ParameterName = "@TOWN_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_TOWN_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TOWN_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@AREA_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_AREA_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_AREA_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ROUTE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ROUTE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ROUTE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CHANNEL_TYPE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CHANNEL_TYPE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CHANNEL_TYPE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@BUSINESS_TYPE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_BUSINESS_TYPE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_BUSINESS_TYPE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PROMOTION_CLASS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_PROMOTION_CLASS == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PROMOTION_CLASS;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@REGDATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_REGDATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_REGDATE;
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
            parameter.ParameterName = "@IS_GST_REGISTERED";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_GST_REGISTERED;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_ACTIVE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_ACTIVE;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CUSTOMER_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_CUSTOMER_ID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CUSTOMER_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CUSTOMER_CODE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_CUSTOMER_CODE == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CUSTOMER_CODE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CUSTOMER_NAME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_CUSTOMER_NAME == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CUSTOMER_NAME;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ADDRESS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ADDRESS == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ADDRESS;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CONTACT_PERSON";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_CONTACT_PERSON == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CONTACT_PERSON;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CONTACT_NUMBER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_CONTACT_NUMBER == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CONTACT_NUMBER;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@EMAIL_ADDRESS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_EMAIL_ADDRESS == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_EMAIL_ADDRESS;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CNIC";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_CNIC == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CNIC;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@GST_NUMBER";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_GST_NUMBER == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_GST_NUMBER;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@NTN";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_NTN == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_NTN;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@BARCODE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_BARCODE == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_BARCODE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@OPENING_AMOUNT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_OPENING_AMOUNT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_OPENING_AMOUNT;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@NATURE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_NATURE == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_NATURE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CONTACT2";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_CONTACT2 == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CONTACT2;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CARD_TYPE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CARD_TYPE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CARD_TYPE_ID;
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
            parameter.ParameterName = "@PURCHASING";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_PURCHASING == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PURCHASING;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@POINTS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_POINTS == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_POINTS;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@AMOUNT_LIMIT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_AMOUNT_LIMIT == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_AMOUNT_LIMIT;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@strCardNo";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_strCardNo == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_strCardNo;
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
            parameter.ParameterName = "@Membership_Date";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_Membership_Date == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Membership_Date;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Membership_No";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_Membership_No))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Membership_No;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Spouse_Name";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (string.IsNullOrEmpty(m_Spouse_Name))
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Spouse_Name;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Profession";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_Profession == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Profession;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CUSTOMER_STATUS_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CUSTOMER_STATUS_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CUSTOMER_STATUS_ID;
            }
            pparams.Add(parameter);




            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CUSTOMER_CATEGORY_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CUSTOMER_CATEGORY_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CUSTOMER_CATEGORY_ID;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsGolfer";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IsGolfer;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@FROM_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_FROM_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_FROM_DATE;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TO_DATE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.DateTime);
            if (m_TO_DATE == Constants.DateNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TO_DATE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@GroupID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_GroupID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_GroupID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LOYALTY_CARD_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_LOYALTY_CARD_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LOYALTY_CARD_ID;
            }
            pparams.Add(parameter);

        }
        #endregion
    }
}