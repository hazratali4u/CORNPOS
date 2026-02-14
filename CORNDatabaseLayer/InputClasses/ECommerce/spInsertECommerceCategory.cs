using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;

namespace CORNDatabaseLayer.Classes
{
    public class spInsertECommerceCategory
    {
        #region Private Members
        private IDbConnection m_connection;
        private IDbTransaction m_transaction;


        private int m_Parent_Category_ID;
        private string m_Category_Name;
        private int m_SortOrder;
        private string m_Image_Path;
        #endregion

        #region Public Properties
        public int Parent_Category_ID
        {
            set
            {
                m_Parent_Category_ID = value;
            }
            get
            {
                return m_Parent_Category_ID;
            }
        }


        public string Category_Name
        {
            set
            {
                m_Category_Name = value;
            }
            get
            {
                return m_Category_Name;
            }
        }

        public int SortOrder
        {
            set
            {
                m_SortOrder = value;
            }
            get
            {
                return m_SortOrder;
            }
        }

        public string ImagePath
        {
            set
            {
                m_Image_Path = value;
            }

            get
            {
                return m_Image_Path;
            }
        }
        public int Category_ID { get; set; }

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
        public spInsertECommerceCategory()
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
                cmd.CommandText = "spInsertECommCategory";
                cmd.Connection = m_connection;
                if (m_transaction != null)
                {
                    cmd.Transaction = m_transaction;
                }
                GetParameterCollection(ref cmd);
                cmd.ExecuteNonQuery();
                //m_ROLE_DETAIL_ID = (int)((IDataParameter)(cmd.Parameters["@ROLE_DETAIL_ID"])).Value;
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
                parameter.ParameterName = "@ParentCategoryId";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_Parent_Category_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_Parent_Category_ID;
                }
                pparams.Add(parameter);


                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@CategoryName";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
                parameter.Value = m_Category_Name;
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@SortOrder";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (m_SortOrder == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = m_SortOrder;
                }
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@ImgUrl";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.NVarChar);
                parameter.Value = m_Image_Path;
                pparams.Add(parameter);



                parameter = ProviderFactory.GetParameter(EnumProviders.SQLClient);
                parameter.ParameterName = "@CategoryID";
                parameter.DbType = ProviderFactory.GetDBType(EnumProviders.SQLClient, EnumDBTypes.Int);
                if (Category_ID == Constants.IntNullValue)
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = Category_ID;
                }
                pparams.Add(parameter);
            }
            catch (Exception)
            { }

        }
        #endregion
    }
}