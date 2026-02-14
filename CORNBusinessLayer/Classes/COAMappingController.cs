using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System;
using System.Data;


namespace CORNBusinessLayer.Classes
{
    public class COAMappingController
    {
        public DataTable SelectCOAConfiguration(int Typeid, short Code, long ColumnValue, string pLevel)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spCOAConfiguration mdtConfig = new spCOAConfiguration();
                mdtConfig.Connection = mConnection;
                mdtConfig.TYPEID = Typeid;
                mdtConfig.CODE = Code;
                mdtConfig.COLUMN_VALUE = ColumnValue;
                mdtConfig.LEVEL = pLevel;

                DataTable dt = mdtConfig.ExecuteTable();
                return dt;
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public DataTable ChekCOAConfigurationExistance(int Typeid, long pAccountId, string pAccountLevel)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spCOAConfiguration mdtConfig = new spCOAConfiguration();
                mdtConfig.Connection = mConnection;
                mdtConfig.TYPEID = Typeid;
                mdtConfig.CODE = Constants.ShortNullValue;
                mdtConfig.COLUMN_VALUE = pAccountId;
                mdtConfig.LEVEL = pAccountLevel;
                return mdtConfig.ExecuteTable();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                return null;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public bool UpdateAppConfiguration(short Code, long ACCOUNT_ID)
        {
            IDbConnection mConnection = null;
            IDbTransaction mTransaction = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                mTransaction = ProviderFactory.GetTransaction(mConnection);
               
                    spCOAConfiguration mdtConfig = new spCOAConfiguration();
                    mdtConfig.Connection = mConnection;
                    mdtConfig.Transaction = mTransaction;
                    mdtConfig.TYPEID = 4;
                    mdtConfig.CODE = Code;
                    mdtConfig.COLUMN_VALUE = ACCOUNT_ID;
                    mdtConfig.ExecuteQuery();
                
                mTransaction.Commit();
                return true;
            }
            catch (Exception exp)
            {
                mTransaction.Rollback();
                ExceptionPublisher.PublishException(exp);
                return false;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
    }
}
