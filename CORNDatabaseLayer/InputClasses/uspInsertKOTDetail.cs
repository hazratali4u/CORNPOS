using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
namespace CORNDatabaseLayer.Classes
{
	public class uspInsertKOTDetail
	{
		#region Private Members
		private string sp_Name = "uspInsertKOTDetail" ;
		private IDbConnection m_connection;
		private IDbTransaction m_transaction;
		private byte m_KOTType;
		private int m_LocationID;
        private int m_KOTNo;
        private int m_SKUID;
        private int m_ModifierParetn_Row_ID;
        private int m_UserID;
        private int m_OrderBookerID;
        private long m_SaleInvoiceID;
        private long m_SaleInvoiceDetailID;
        private string m_Section;
        private string m_ServiceType;
        private string m_TableName;
        private string m_DealName;
        private string m_ItemComments;
        private string m_Modifiers;
        private string m_Category;
        private string m_TimeStamp;
        private string m_LastUpdateDateTime;
        private string m_LastUpdateDateTimeDetail;
        private string m_OrderNotes;
        private decimal m_Quantity;
        private decimal m_DealQTY;
        #endregion
        #region Public Properties
        public byte KOTType
        {
			set
			{
                m_KOTType = value ;
			}
			get
			{
				return m_KOTType;
			}
		}
		public int LocationID
        {
			set
			{
                m_LocationID = value ;
			}
			get
			{
				return m_LocationID;
			}
		}
        public int KOTNo
        {
            set { m_KOTNo = value; }
            get { return m_KOTNo; }
        }
        public int SKUID
        {
            set { m_SKUID = value; }
            get { return m_SKUID; }
        }
        public int ModifierParetn_Row_ID
        {
            set { m_ModifierParetn_Row_ID = value; }
            get { return m_ModifierParetn_Row_ID; }
        }
        public int UserID
        {
            set { m_UserID = value; }
            get { return m_UserID; }
        }
        public int OrderBookerID
        {
            set { m_OrderBookerID = value; }
            get { return m_OrderBookerID; }
        }
        public long SaleInvoiceID
        {
			set
			{
                m_SaleInvoiceID = value ;
			}
			get
			{
				return m_SaleInvoiceID;
			}
		}
        public long SaleInvoiceDetailID
        {
            set { m_SaleInvoiceDetailID = value; }
            get { return m_SaleInvoiceDetailID; }
        }
        public string Section
        {
            set { m_Section = value; }
            get { return m_Section; }
        }
        public string ServiceType
        {
            set { m_ServiceType = value; }
            get { return m_ServiceType; }
        }
        public string TableName
        {
            set { m_TableName = value; }
            get { return m_TableName; }
        }
        public string DealName
        {
            set { m_DealName = value; }
            get { return m_DealName; }
        }
        public string ItemComments
        {
            set { m_ItemComments = value; }
            get { return m_ItemComments; }
        }
        public string Modifiers
        {
            set { m_Modifiers = value; }
            get { return m_Modifiers; }
        }
        public string Category
        {
            set { m_Category = value; }
            get { return m_Category; }
        }
        public string TimeStamp
        {
            set { m_TimeStamp = value; }
            get { return m_TimeStamp; }
        }
        public string LastUpdateDateTime
        {
            set { m_LastUpdateDateTime = value; }
            get { return m_LastUpdateDateTime; }
        }
        public string LastUpdateDateTimeDetail
        {
            set { m_LastUpdateDateTimeDetail = value; }
            get { return m_LastUpdateDateTimeDetail; }
        }
        public string OrderNotes
        {
            set { m_OrderNotes = value; }
            get { return m_OrderNotes; }
        }
        public decimal Quantity
        {
            set { m_Quantity = value; }
            get { return m_Quantity; }
        }
        public decimal DealQTY
        {
            set { m_DealQTY = value; }
            get { return m_DealQTY; }
        }

        public IDbConnection  Connection
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
		public IDbTransaction  Transaction
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
		public uspInsertKOTDetail()
		{
		}
		#endregion
		#region public Methods
		public bool  ExecuteQuery()
		{
			try
			{
			    IDbCommand cmd = ProviderFactory.GetCommand(EnumProviders.SQLClient);
				cmd.CommandType =  CommandType.StoredProcedure;
				cmd.CommandText = sp_Name;
				cmd.Connection =   m_connection;
				if(m_transaction!=null)
				{
					cmd.Transaction = m_transaction;
				}
				GetParameterCollection(ref cmd);
				cmd.ExecuteNonQuery();
				return true;
			}
			catch(Exception e)
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
			IDataParameter parameter ;
			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@KOTType"; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.TinyInt);
			if(m_KOTType == Constants.ByteNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_KOTType;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@LocationID"; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
			if(m_LocationID == Constants.IntNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_LocationID;
			}
			pparams.Add(parameter);


			parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
			parameter.ParameterName = "@SaleInvoiceID"; 
			parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
			if(m_SaleInvoiceID == Constants.LongNullValue)
			{
				parameter.Value = DBNull.Value;
			}
			else
			{
				parameter.Value = m_SaleInvoiceID;
			}
			pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SaleInvoiceDetailID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.BigInt);
            if (m_SaleInvoiceDetailID == Constants.LongNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SaleInvoiceDetailID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@KOTNo";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_KOTNo == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_KOTNo;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@SKUID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_SKUID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_SKUID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ModifierParetn_Row_ID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_ModifierParetn_Row_ID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ModifierParetn_Row_ID;
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

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@OrderBookerID";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
            if (m_OrderBookerID == Constants.IntNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_OrderBookerID;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Section";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_Section == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Section;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ServiceType";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ServiceType == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ServiceType;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TableName";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_TableName == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TableName;
            }
            pparams.Add(parameter);
            

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DealName";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_DealName == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DealName;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@ItemComments";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_ItemComments == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_ItemComments;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Modifiers";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_Modifiers == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Modifiers;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Category";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_Category == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Category;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@TimeStamp";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_TimeStamp == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_TimeStamp;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LastUpdateDateTime";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_LastUpdateDateTime == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LastUpdateDateTime;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@LastUpdateDateTimeDetail";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_LastUpdateDateTimeDetail == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_LastUpdateDateTimeDetail;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@OrderNotes";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.VarChar);
            if (m_OrderNotes == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_OrderNotes;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@Quantity";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_Quantity == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_Quantity;
            }
            pparams.Add(parameter);

            parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
            parameter.ParameterName = "@DealQTY";
            parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Decimal);
            if (m_DealQTY == Constants.DecimalNullValue)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = m_DealQTY;
            }
            pparams.Add(parameter);
        }
		#endregion
	}
}