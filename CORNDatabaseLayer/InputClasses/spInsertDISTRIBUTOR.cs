using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;


namespace CORNDatabaseLayer.Classes
{
    public class spInsertDISTRIBUTOR
    {
        #region Private Members
        private string sp_Name = "spInsertDISTRIBUTOR";
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;
        private int m_COMPANY_ID;
        private int m_REGION_ID;
        private int m_ZONE_ID;
        private int m_SUBZONE_ID;
        private int m_DIVISION_ID;
        private int m_DIST_CLASS_ID;
        private int m_USER_ID;
        private int m_CREDIT_LEVEL;
        private int m_DISTRIBUTOR_ID;
        private int m_DELIVERY_CHARGES_TYPE;
        private DateTime m_TIME_STAMP;
        private DateTime m_LASTUPDATE_DATE;
        private bool m_ISDELETED;
        private bool m_SERVICE_CHARGES;
        private bool m_IS_REGISTERED;
        private bool m_PrintKOT;
        private bool m_PrintKOTDelivery;
        private bool m_PrintKOTTakeaway;
        private bool m_AutoPromotion;
        private decimal m_GST;
        private decimal m_GST_CREDIT_CARD;
        private decimal m_DELIVERY_CHARGES_VALUE;
        private string m_DISTRIBUTOR_CODE;
        private string m_DISTRIBUTOR_NAME;
        private string m_PASSWORD;
        private string m_ADDRESS1;
        private string m_ADDRESS2;
        private string m_CONTACT_PERSON;
        private string m_CONTACT_NUMBER;
        private string m_GST_NUMBER;
        private string m_IP_ADDRESS;
        private string m_PIC;
        private bool m_SHOW_LOGO;
        private string m_smsDelivery;
        private string m_smsTakeAway;
        private string m_smsDayClose;
        private string m_smsContact;
        private bool m_IssmsDelivery;
        private bool m_IssmsTakeAway;
        private bool m_IssmsDayClose;
        private bool m_IsDeliveryCharges;
        private int m_SERVICE_CHARGES_TYPE;
        private decimal m_SERVICE_CHARGES_VALUE;
        private string m_STRN;
        private string m_Latitude;
        private string m_Longitude;
        private string m_ServiceChargesLabel;
        private decimal m_POS_FEE;
        #endregion
        #region Public Properties

        public bool SERVICE_CHARGES
        {
            set
            {
                m_SERVICE_CHARGES = value;
            }
            get
            {
                return m_SERVICE_CHARGES;
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
        public int REGION_ID
        {
            set
            {
                m_REGION_ID = value;
            }
            get
            {
                return m_REGION_ID;
            }
        }
        public int ZONE_ID
        {
            set
            {
                m_ZONE_ID = value;
            }
            get
            {
                return m_ZONE_ID;
            }
        }
        public int SUBZONE_ID
        {
            set
            {
                m_SUBZONE_ID = value;
            }
            get
            {
                return m_SUBZONE_ID;
            }
        }
        public int DIVISION_ID
        {
            set
            {
                m_DIVISION_ID = value;
            }
            get
            {
                return m_DIVISION_ID;
            }
        }
        public int DIST_CLASS_ID
        {
            set
            {
                m_DIST_CLASS_ID = value;
            }
            get
            {
                return m_DIST_CLASS_ID;
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
        public int CREDIT_LEVEL
        {
            set
            {
                m_CREDIT_LEVEL = value;
            }
            get
            {
                return m_CREDIT_LEVEL;
            }
        }
        public int DISTRIBUTOR_ID
        {
            get
            {
                return m_DISTRIBUTOR_ID;
            }
        }
        public int DELIVERY_CHARGES_TYPE
        {
            set { m_DELIVERY_CHARGES_TYPE = value; }
            get { return m_DELIVERY_CHARGES_TYPE; }
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
        public bool ISDELETED
        {
            set
            {
                m_ISDELETED = value;
            }
            get
            {
                return m_ISDELETED;
            }
        }
        public bool PrintKOT
        {
            set { m_PrintKOT = value; }
            get { return m_PrintKOT; }
        }
        public bool PrintKOTDelivery
        {
            set { m_PrintKOTDelivery = value; }
            get { return m_PrintKOTDelivery; }
        }
        public bool PrintKOTTakeaway
        {
            set { m_PrintKOTTakeaway = value; }
            get { return m_PrintKOTTakeaway; }
        }
        public bool AutoPromotion
        {
            set { m_AutoPromotion = value; }
            get { return m_AutoPromotion; }
        }
        public bool IS_REGISTERED
        {
            set
            {
                m_IS_REGISTERED = value;
            }
            get
            {
                return m_IS_REGISTERED;
            }
        }
        public bool IsDeliveryCharges
        {
            set { m_IsDeliveryCharges = value; }
            get { return m_IsDeliveryCharges; }
        }
        public decimal GST
        {
            set
            {
                m_GST = value;
            }
            get
            {
                return m_GST;
            }
        }
        public decimal GST_CREDIT_CARD
        {
            set { m_GST_CREDIT_CARD = value; }
            get { return m_GST_CREDIT_CARD; }
        }
        public decimal DELIVERY_CHARGES_VALUE
        {
            set { m_DELIVERY_CHARGES_VALUE = value; }
            get { return m_DELIVERY_CHARGES_VALUE; }
        }
        public string DISTRIBUTOR_CODE
        {
            set
            {
                m_DISTRIBUTOR_CODE = value;
            }
            get
            {
                return m_DISTRIBUTOR_CODE;
            }
        }
        public string DISTRIBUTOR_NAME
        {
            set
            {
                m_DISTRIBUTOR_NAME = value;
            }
            get
            {
                return m_DISTRIBUTOR_NAME;
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
        public string ADDRESS1
        {
            set
            {
                m_ADDRESS1 = value;
            }
            get
            {
                return m_ADDRESS1;
            }
        }
        public string ADDRESS2
        {
            set
            {
                m_ADDRESS2 = value;
            }
            get
            {
                return m_ADDRESS2;
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
        public string IP_ADDRESS
        {
            set
            {
                m_IP_ADDRESS = value;
            }
            get
            {
                return m_IP_ADDRESS;
            }
        }
        public string PIC
        {
            set
            {
                m_PIC = value;
            }
            get
            {
                return m_PIC;
            }
        }
        public bool SHOW_LOGO
        {
            set
            {
                m_SHOW_LOGO = value;
            }
            get
            {
                return m_SHOW_LOGO;
            }
        }
        public string SMSDELIVERY
        {
            set
            {
                m_smsDelivery = value;
            }
            get
            {
                return m_smsDelivery;
            }
        }
        public string SMSTAKEAWAY
        {
            set
            {
                m_smsTakeAway = value;
            }
            get
            {
                return m_smsTakeAway;
            }
        }
        public string SMSDAYCLOSE
        {
            set
            {
                m_smsDayClose = value;
            }
            get
            {
                return m_smsDayClose;
            }
        }
        public string SMSCONTACT
        {
            set
            {
                m_smsContact = value;
            }
            get
            {
                return m_smsContact;
            }
        }
        public bool IsSMSDELIVERY
        {
            set
            {
                m_IssmsDelivery = value;
            }
            get
            {
                return m_IssmsDelivery;
            }
        }
        public bool IsSMSTAKEAWAY
        {
            set
            {
                m_IssmsTakeAway = value;
            }
            get
            {
                return m_IssmsTakeAway;
            }
        }
        public bool IsSMSDAYCLOSE
        {
            set
            {
                m_IssmsDayClose = value;
            }
            get
            {
                return m_IssmsDayClose;
            }
        }
        public int SERVICE_CHARGES_TYPE
        {
            set { m_SERVICE_CHARGES_TYPE = value; }
            get { return m_SERVICE_CHARGES_TYPE; }
        }
        public decimal SERVICE_CHARGES_VALUE
        {
            set { m_SERVICE_CHARGES_VALUE = value; }
            get { return m_SERVICE_CHARGES_VALUE; }
        }
        public string STRN
        {
            set { m_STRN = value; }
            get { return m_STRN; }
        }
        public string Latitude
        {
            set { m_Latitude = value; }
            get { return m_Latitude; }
        }
        public string Longitude
        {
            set { m_Longitude = value; }
            get { return m_Longitude; }
        }
        public int CITY_ID { get; set; }
        public string ServiceChargesLabel {
            set { m_ServiceChargesLabel = value; }
            get { return m_ServiceChargesLabel; }
        }
        public decimal POS_FEE
        {
            set { m_POS_FEE = value; }
            get { return m_POS_FEE; }
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
        public spInsertDISTRIBUTOR()
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
                m_DISTRIBUTOR_ID = (int)((IDataParameter)(cmd.Parameters["@DISTRIBUTOR_ID"])).Value;
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
            parameter.ParameterName = "@REGION_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_REGION_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_REGION_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ZONE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ZONE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ZONE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SUBZONE_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SUBZONE_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SUBZONE_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DIVISION_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DIVISION_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DIVISION_ID;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DIST_CLASS_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DIST_CLASS_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DIST_CLASS_ID;
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


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CREDIT_LEVEL";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_CREDIT_LEVEL == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_CREDIT_LEVEL;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DELIVERY_CHARGES_TYPE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_DELIVERY_CHARGES_TYPE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DELIVERY_CHARGES_TYPE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISTRIBUTOR_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            parameter.Direction = ParameterDirection.Output;
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
            parameter.ParameterName = "@ISDELETED";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_ISDELETED;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IS_REGISTERED";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IS_REGISTERED;
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SERVICE_CHARGES";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_SERVICE_CHARGES;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PrintKOT";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_PrintKOT;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PrintKOTDelivery";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_PrintKOTDelivery;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PrintKOTTakeaway";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_PrintKOTTakeaway;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@AutoPromotion";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_AutoPromotion;
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
            parameter.ParameterName = "@GST_CREDIT_CARD";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_GST_CREDIT_CARD == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_GST_CREDIT_CARD;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DELIVERY_CHARGES_VALUE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_DELIVERY_CHARGES_VALUE == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DELIVERY_CHARGES_VALUE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISTRIBUTOR_CODE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_DISTRIBUTOR_CODE == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DISTRIBUTOR_CODE;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DISTRIBUTOR_NAME";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_DISTRIBUTOR_NAME == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DISTRIBUTOR_NAME;
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
            parameter.ParameterName = "@ADDRESS1";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ADDRESS1 == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ADDRESS1;
            }
            pparams.Add(parameter);


            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ADDRESS2";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ADDRESS2 == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ADDRESS2;
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
            parameter.ParameterName = "@IP_ADDRESS";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_IP_ADDRESS == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_IP_ADDRESS;
            }
            pparams.Add(parameter);
            
            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@PIC";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_PIC == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_PIC;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SHOW_LOGO";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_SHOW_LOGO;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@strMessageonDeliveryHoldOrder";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_smsDelivery == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_smsDelivery;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@strMessageonTakeAway";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_smsTakeAway == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_smsTakeAway;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@strMessageDayClose";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_smsDayClose == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_smsDayClose;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@strContactNo";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_smsContact == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_smsContact;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsSMSonDeliveryHoldOrder";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IssmsDelivery;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsSMSonTakeAway";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IssmsTakeAway;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsDeliveryCharges";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IsDeliveryCharges;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@IsSMSonDayClose";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Bit);
            parameter.Value = m_IssmsDayClose;
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SERVICE_CHARGES_TYPE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SERVICE_CHARGES_TYPE == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SERVICE_CHARGES_TYPE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SERVICE_CHARGES_VALUE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_SERVICE_CHARGES_VALUE == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SERVICE_CHARGES_VALUE;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@STRN";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_STRN == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_STRN;
            }
            pparams.Add(parameter);




            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Latitude";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_Latitude == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Latitude;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Longitude";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_Longitude == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Longitude;
            }
            pparams.Add(parameter);



            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@CITY_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (CITY_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = CITY_ID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ServiceChargesLabel";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
            if (m_ServiceChargesLabel == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ServiceChargesLabel;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@POS_FEE";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Money);
            if (m_POS_FEE == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_POS_FEE;
            }
            pparams.Add(parameter);
        }
        #endregion
    }
}