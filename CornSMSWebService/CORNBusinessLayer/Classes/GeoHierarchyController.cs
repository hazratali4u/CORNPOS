using System;
using CORNDatabaseLayer.Classes;
using CORNDataAccessLayer.Classes;
using CORNCommon.Classes;
using System.Data;
using CORNBusinessLayer.Classes;    

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Geo Hierarchy Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Geo Hierarchy
    /// </item>
    /// <term>
    /// Update Geo Hierarchy
    /// </term>
    /// <item>
    /// Get Geo Hierarchy
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class GeoHierarchyController
    {
        public DataTable GetSMSSetting(int pDistributerId)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetSMSSetting mtblDefination = new uspGetSMSSetting
                {
                    Connection = mConnection,
                    Distributer_ID = pDistributerId
                };
                return mtblDefination.ExecuteTable();
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
