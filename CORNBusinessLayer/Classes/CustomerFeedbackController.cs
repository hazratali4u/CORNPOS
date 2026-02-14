using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using System;
using System.Data;

namespace CORNBusinessLayer.Classes
{
    public class CustomerFeedbackController
    {
        public string InsertFeedBack(int LocationId, int ServiceRate, int FoodRate, int EnvRate,
            int OverallRate, string Comments, int HearMedium, string OtherMeduim,
            string Name, string ContactNo, string Email, string Address, DateTime p_documentDate,
            int returnSuggest, int visitSuggest, string likeSuggest,string improveSuggest, string menuSuggest, 
            string city)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertCUSTOMER_FEEDBACK mFeedback = new spInsertCUSTOMER_FEEDBACK();
                mFeedback.Connection = mConnection;

                mFeedback.LOCATION_ID = LocationId;
                mFeedback.SERVICE_RATE = ServiceRate;
                mFeedback.FOOD_RATE = FoodRate;
                mFeedback.ENVIRONMENT_RATE = EnvRate;
                mFeedback.OVERALL_RATE = OverallRate;
                mFeedback.COMMENTS = Comments;
                mFeedback.HEAR_MEDIUM = HearMedium;
                mFeedback.OTHER_MEDIUM = OtherMeduim;
                mFeedback.NAME = Name;
                mFeedback.CONTACT_NO = ContactNo;
                mFeedback.EMAIL = Email;
                mFeedback.ADDRESS = Address;
                mFeedback.RETURN_SUGGESTION = returnSuggest;
                mFeedback.VISIT_SUGGESTION = visitSuggest;
                mFeedback.LIKE_SUGGESTION = likeSuggest;
                mFeedback.IMPROVE_SUGGESTION = improveSuggest;
                mFeedback.MENU_SUGGESTION = menuSuggest;
                mFeedback.CITY = city;
                mFeedback.TIME_STAMP = System.DateTime.Now;
                mFeedback.DOCUMENT_DATE = p_documentDate;

                mFeedback.ExecuteQuery();

                return mFeedback.FEEDBACK_ID.ToString();
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

        #region Feedback Report
        public DataSet GetCustomerFeedbackReport(int p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertCUSTOMER_FEEDBACK objPrint = new spInsertCUSTOMER_FEEDBACK();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.LOCATION_ID = p_DISTRIBUTOR_ID;
                objPrint.FROM_DATE = p_FromDate;
                objPrint.TO_DATE = p_ToDate;
                DataTable dt = objPrint.ExecuteTableForCustomerFeedbackReport();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["rptCustomerFeedback"].ImportRow(dr);
                }

                return ds;
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
    }
}
