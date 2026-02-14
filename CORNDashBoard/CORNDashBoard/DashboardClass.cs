using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CORNDashBoard
{
    public static class DashboardClass
    {
        public static string GetConnectionString()
        {
            try
            {
                string mConnectionString = string.Empty;
                string DatabseAuthentication = string.Empty;
                if (DatabseAuthentication.Equals("Windows")) // windows method
                {
                    mConnectionString = "Integrated Security=SSPI;Persist Security Info=False;";
                }
                else // MSDE mode
                {
                    mConnectionString = " Persist Security Info=False; ";
                    //mConnectionString += " User ID=" + System.Configuration.ConfigurationManager.AppSettings["DBUser"].ToString() + ";Password=" + System.Configuration.ConfigurationManager.AppSettings["DBPassword"].ToString() + ";";
                }
                //common settings
                //mConnectionString += " Initial Catalog = " + System.Configuration.ConfigurationManager.AppSettings["DBName"].ToString() + "; ";
                //mConnectionString += " Data Source= " + System.Configuration.ConfigurationManager.AppSettings["Server"].ToString() + " ";

                return mConnectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int SaveDashboardinMenu(Int32 intMenuCode, byte[] xmlByteArray)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@intMenuCode", intMenuCode));
                parameters.Add(new SqlParameter("@xmlByteArray", xmlByteArray));
                return SqlHelper.ExecuteNonQuery(GetConnectionString(), "procSaveDashboardinMenu", parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetDashboardMenuInfo(Int32 intMenuCode,string conn)
        {
            try
            {
                DataSet dsDashboard;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@MenuCode", intMenuCode));
                dsDashboard = (DataSet)SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "procMenuDashboard", parameters.ToArray());
                
                return dsDashboard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static DataSet GetDashboardDataSet(String strProcName, DateTime dtFrom, DateTime dtTo,string conn)
        {
            try
            {
                DataSet dsDashboard;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FinancialYearCode", 2));
                parameters.Add(new SqlParameter("@dtFrom", "2023-11-01"));
                parameters.Add(new SqlParameter("@dtTo", "2023-11-07"));
                parameters.Add(new SqlParameter("@dtFrom2", "2023-11-08"));
                parameters.Add(new SqlParameter("@dtTo2", "2023-11-16"));
                parameters.Add(new SqlParameter("@EmployeeCode", 1));
                parameters.Add(new SqlParameter("@CompanyCode", 1));
                return dsDashboard = (DataSet)SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, strProcName, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataSet GetDashboardDataSet(String strProcName, DateTime dtFrom, DateTime dtTo,int SKUID)
        {
            try
            {
                DataSet dsDashboard;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FinancialYearCode", SKUID));
                parameters.Add(new SqlParameter("@dtFrom", dtFrom.Date));
                parameters.Add(new SqlParameter("@dtTo", dtTo.Date));
                parameters.Add(new SqlParameter("@EmployeeCode", 1));
                parameters.Add(new SqlParameter("@CompanyCode", 1));
                return dsDashboard = (DataSet)SqlHelper.ExecuteDataset(GetConnectionString(), CommandType.StoredProcedure, strProcName, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
