using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System;
using System.Data;

namespace CORNBusinessLayer.Classes
{
    public class AppSettingDetail
    {
        public DataTable GetAppSettingDetail(int TypeID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetAppSettingDetail mdtConfig = new uspGetAppSettingDetail();
                mdtConfig.Connection = mConnection;
                mdtConfig.TYPEID = TypeID;
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
    }
}