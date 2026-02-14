using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Forms
{
    public partial class frmTableReservation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            Response.AppendHeader("pragma", "no-cache");
            if (!Page.IsPostBack)
            {
                GetAppSettingDetail();
                lblUserName.Text = Session["UserName"].ToString();
                DateTime date = DateTime.Parse(Session["CurrentWorkDate"].ToString());
                lblDateTime.Text = date.ToString("dd-MMMM-yyyy");

                var mTableRes = new TableReservationController();
                DataTable dtTables = mTableRes.GetTableReservation(8, Convert.ToInt32(Session["UserID"]), date);
                hfReservationDate.Value = GetJson(dtTables);
            }
        }

        protected void lnkExit_OnClick(object sender, EventArgs e)
        {

            Response.Redirect("Home.aspx");

        }

        #region Location
        [WebMethod]
        public static string LoadLocation()
        {
            var mController = new DistributorController();
            try
            {
                DataTable dtLocations = mController.GetDistributorWithMaxDayClose(Constants.IntNullValue, Convert.ToInt32(HttpContext.Current.Session["UserId"]), Convert.ToInt32(HttpContext.Current.Session["CompanyId"]), 1);
                HttpContext.Current.Session.Add("dtLocationInfo", dtLocations);
                return GetJson(dtLocations);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion        

        #region Customer
        [WebMethod]
        public static string SearchCustomer(string FieldValue, string FieldName)
        {
            var mController = new CustomerDataController();
            try
            {
                DataTable dtCustomers = mController.UspSelectCustomer(int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()), FieldName, FieldValue, DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()));
                return GetJson(dtCustomers);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Tables

        [WebMethod]
        public static string GetFloors(string DistributorID)
        {
            var mTableController = new OrderEntryController();
            DataTable dtTables = mTableController.SelectPendingBills(Constants.LongNullValue, DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()), int.Parse(DistributorID), Constants.IntNullValue, Constants.IntNullValue, 15);
            return GetJson(dtTables);
        }

        [WebMethod]
        public static string GetTables(string DistributorID)
        {
            var mTableController = new OrderEntryController();
            DataTable dtTables = mTableController.SelectPendingBills(Constants.LongNullValue,DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()), int.Parse(DistributorID), Constants.IntNullValue, Constants.IntNullValue, 12);
            return GetJson(dtTables);
        }

        #endregion

        #region Time Slot

        [WebMethod]
        public static string GetTimeSlot(string DistributorID)
        {
            var mTableRes = new TableReservationController();
            DataTable dtTables = mTableRes.GetTimeSlot(2,Convert.ToInt32(DistributorID));
            return GetJson(dtTables);
        }

        #endregion

        #region Save/Update Reservation

        [WebMethod]
        public static void SaveReservation(string LocationID, string ReservationDate,string ReservatkionTime,string NoOfGuest,string BookinSlot,string Name,string MobileNo, string WhatsApp,string Email,string Remarks,string Source,string CustomerTypeID,string dtTableIDs,string CustomerID,string DOB)
        {
            if (Convert.ToInt64(CustomerID) > 0)
            {
                var mTableRes = new TableReservationController();
                DataTable dtTableID = (DataTable)JsonConvert.DeserializeObject(dtTableIDs, (typeof(DataTable)));
                mTableRes.InsertTableReservation(int.Parse(LocationID), DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()), Convert.ToDateTime(ReservationDate), Convert.ToDateTime(ReservatkionTime), Convert.ToInt32(NoOfGuest), Convert.ToInt32(BookinSlot), Convert.ToInt64(CustomerID), Remarks, Convert.ToInt32(Source), Convert.ToInt32(CustomerTypeID), Convert.ToInt32(HttpContext.Current.Session["UserID"]), 1, dtTableID);
            }
            else
            {
                DateTime dtDOB = Constants.DateNullValue;
                if(DOB.Length > 0)
                {
                    dtDOB = DateTime.ParseExact(DOB, "dd-MM-yyyy", null);
                }
                long Customer_ID = CustomerDataController.InsertCustomer3(Convert.ToInt32(LocationID), MobileNo, Email, Name, WhatsApp,dtDOB);
                if (Customer_ID > 0)
                {
                    var mTableRes = new TableReservationController();
                    DataTable dtTableID = (DataTable)JsonConvert.DeserializeObject(dtTableIDs, (typeof(DataTable)));
                    mTableRes.InsertTableReservation(int.Parse(LocationID), DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()), Convert.ToDateTime(ReservationDate), Convert.ToDateTime(ReservatkionTime), Convert.ToInt32(NoOfGuest), Convert.ToInt32(BookinSlot), Customer_ID, Remarks, Convert.ToInt32(Source), Convert.ToInt32(CustomerTypeID), Convert.ToInt32(HttpContext.Current.Session["UserID"]), 1, dtTableID);
                }
            }
        }

        [WebMethod]
        public static void UpdateReservation(string TableReservationMasterCode,string ReservationDate, string ReservatkionTime, string NoOfGuest, string BookinSlot, string Remarks, string Source, string CustomerTypeID, string dtTableIDs)
        {
            var mTableRes = new TableReservationController();
            DataTable dtTableID = (DataTable)JsonConvert.DeserializeObject(dtTableIDs, (typeof(DataTable)));
            mTableRes.UpdateTableReservation(Convert.ToInt64(TableReservationMasterCode),Convert.ToDateTime(ReservationDate), Convert.ToDateTime(ReservatkionTime), Convert.ToInt32(NoOfGuest), Convert.ToInt32(BookinSlot), Remarks,Convert.ToInt32(Source),Convert.ToInt32(CustomerTypeID), Convert.ToInt32(HttpContext.Current.Session["UserID"]), 5, dtTableID);
        }
        [WebMethod]
        public static void CancelReservation(string TableReservationMasterCode,string CancelationReason,string CancelledBy)
        {
            var mTableRes = new TableReservationController();
            mTableRes.CancelReservation(Convert.ToInt64(TableReservationMasterCode), CancelationReason, Convert.ToInt32(HttpContext.Current.Session["UserID"]),Convert.ToInt32(CancelledBy), 6);
        }
        #endregion

        #region Get Table Reservation
        
        [WebMethod]
        public static string GetTableReservation(string DistributorID, string ReservationDate)
        {
            var mTableRes = new TableReservationController();
            DataTable dtTables = mTableRes.GetTableReservation(3,Convert.ToInt32(DistributorID),Convert.ToDateTime(ReservationDate));
            return GetJson(dtTables);
        }

        [WebMethod]
        public static string GetTableReservationDetail(string TableReservationMasterCode)
        {
            var mTableRes = new TableReservationController();
            DataTable dtTables = mTableRes.GetTableReservationDetail(4,Convert.ToInt64(TableReservationMasterCode));
            return GetJson(dtTables);
        }

        [WebMethod]
        public static string GetReseveredTable(string DistributorID, string ReservationDate)
        {
            var mTableRes = new TableReservationController();
            DataTable dtTables = mTableRes.GetTableReservation(7, Convert.ToInt32(DistributorID), Convert.ToDateTime(ReservationDate));
            return GetJson(dtTables);
        }
        #endregion

        #region User Data
        [WebMethod]
        public static string ValidateUser(string UserId, string UserPass, string UserAction)
        {
            DataTable dtConfigDefault = (DataTable)HttpContext.Current.Session["dtAppSettingDetail"];
            var _mController = new UserController();
            int distributerId = int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString());
            bool IsEncrypted = Convert.ToBoolean(Convert.ToInt32(dtConfigDefault.Rows[0]["IsEncreptedCredentials"]));
            if (IsEncrypted)
            {
                UserPass = Cryptography.Encrypt(UserPass, "b0tin@74");
            }
            DataTable dt = _mController.SelectValidateUserActive(distributerId, UserId, UserPass);
            if (dt != null)
            {
                DataRow[] foundRow = null;
                if (UserAction == "CanCancelTableReservation")
                {
                    foundRow = dt.Select("CanCancelTableReservation=" + 1);
                }                
                if (foundRow.Length > 0)
                {
                    return GetJson(dt);
                }
                return null;
            }
            return null;
        }
        #endregion

        public void GetAppSettingDetail()
        {
            try
            {
                AppSettingDetail _cController = new AppSettingDetail();
                DataTable dtAppSetting = _cController.GetAppSettingDetail(1);
                if (dtAppSetting.Rows.Count > 0)
                {
                    Session.Add("dtAppSettingDetail", dtAppSetting);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
            }
        }

        public static string GetJson(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
    }
}