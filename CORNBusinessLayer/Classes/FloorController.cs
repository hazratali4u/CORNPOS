using System;
using CORNDatabaseLayer.Classes;
using CORNDataAccessLayer.Classes;
using CORNCommon.Classes;
using System.Data;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Menu Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert Menu Level
    /// </item>
    /// <item>
    /// Get Menu Level
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public class FloorController
    {
        #region Public Methods

        #region Select               
        public DataTable GetFloor(int p_TypeID,int p_DistributorID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetFloor ObjSelect = new uspGetFloor();
                ObjSelect.Connection = mConnection;                
                ObjSelect.TypeID = p_TypeID;
                ObjSelect.DistributorID = p_DistributorID;
                DataTable dt = ObjSelect.ExecuteTable();
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
        
        #endregion        

        #endregion
    }
}