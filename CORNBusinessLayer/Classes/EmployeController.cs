using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{    
    public class EmployeController
    {
        #region Constructor
        public EmployeController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Public Methods
        public DataTable GetAttendance(long p_AttendanceID, long p_EmployeeID, int p_DepartmentID, int p_EmployeeLocationID, int p_TYPE_ID, DateTime p_Month)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spGetAttendance objLeave = new spGetAttendance();
                objLeave.Connection = mConnection;
                objLeave.AttendanceID = p_AttendanceID;
                objLeave.EmployeeID = p_EmployeeID;
                objLeave.DepartmentID = p_DepartmentID;
                objLeave.EmployeeLocationID = p_EmployeeLocationID;
                objLeave.TYPE_ID = p_TYPE_ID;
                objLeave.MONTH = p_Month;
                DataTable dt = objLeave.ExecuteTable();
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

        public bool UpdateAttendeance(long p_AttendanceID,DateTime p_DayofMonth,int p_EmployeeID,string p_Note,string p_TimeOfDay,int p_TimeSheetID,int p_AttendanceType
            ,bool p_IsLate,int p_User_ID,bool p_IS_ACTIVE,bool p_IS_DELETED)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateAttendance Attendance = new spUpdateAttendance();
                Attendance.Connection = mConnection;
                Attendance.AttendanceID = p_AttendanceID;
                Attendance.DayofMonth = p_DayofMonth;
                Attendance.EmployeeID = p_EmployeeID;
                Attendance.Note = p_Note;
                Attendance.TimeOfDay = p_TimeOfDay;
                Attendance.TimeSheetID = p_TimeSheetID;
                Attendance.AttendanceType = p_AttendanceType;
                Attendance.IsLate = p_IsLate;
                Attendance.User_ID = p_User_ID;
                Attendance.IS_ACTIVE = p_IS_ACTIVE;
                Attendance.IS_DELETED = p_IS_DELETED;
                return Attendance.ExecuteQuery();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw exp;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }

        }

        public bool InsertAttendeance(int p_EmployeeID, int p_TimeSheetID, int p_AttendanceType, DateTime p_DayofMonth, string p_TimeOfDay, string p_Note
            , bool p_IsLate, int p_User_ID,DateTime p_Document_Date)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertAttendance Attendance = new spInsertAttendance();
                Attendance.Connection = mConnection;
                Attendance.DayofMonth = p_DayofMonth;
                Attendance.EmployeeID = p_EmployeeID;
                Attendance.Note = p_Note;
                Attendance.TimeOfDay = p_TimeOfDay;
                Attendance.TimeSheetID = p_TimeSheetID;
                Attendance.AttendanceType = p_AttendanceType;
                Attendance.IsLate = p_IsLate;
                Attendance.User_ID = p_User_ID;
                Attendance.Document_Date = p_Document_Date;
                return Attendance.ExecuteQuery();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw exp;
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
    }
}