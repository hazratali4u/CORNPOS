using System;
using System.Data;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;

namespace CORNBusinessLayer.Classes
{

    public class SkimmingController
    {

        public SkimmingController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string InsertSkimming(int distributor_id, int CashierId, decimal amount,
            string REMARKS,DateTime SKIM_DATE, long p_account_head_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spInsertCASH_SKIMMING mCashSkimming = new spInsertCASH_SKIMMING();
                mCashSkimming.Connection = mConnection;

                mCashSkimming.DISTRIBUTOR_ID = distributor_id;
                mCashSkimming.CASHIER_ID = CashierId;
                mCashSkimming.AMOUNT = amount;
                mCashSkimming.TIME_STAMP = System.DateTime.Now;
                mCashSkimming.LAST_UPDATE = System.DateTime.Now;
                mCashSkimming.IS_ACTIVE = true;
                mCashSkimming.REMARKS = REMARKS;
                mCashSkimming.SKIM_DATE = SKIM_DATE;
                mCashSkimming.ACCOUNT_HEAD_ID = p_account_head_Id;

                mCashSkimming.ExecuteQuery();

                return mCashSkimming.SKIMMING_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        public string UpdateSkimming(int skimming_Id, int distributor_id, int CashierId,
            decimal amount, string REMARKS, long p_account_head_Id)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spUpdateCASH_SKIMMING mCashSkimming = new spUpdateCASH_SKIMMING();
                mCashSkimming.Connection = mConnection;

                mCashSkimming.SKIMMING_ID = skimming_Id;
                mCashSkimming.DISTRIBUTOR_ID = distributor_id;
                mCashSkimming.CASHIER_ID = CashierId;
                mCashSkimming.AMOUNT = amount;
                mCashSkimming.TIME_STAMP = Constants.DateNullValue;
                mCashSkimming.LAST_UPDATE = System.DateTime.Now;
                mCashSkimming.IS_ACTIVE = true;
                mCashSkimming.REMARKS = REMARKS;
                mCashSkimming.ACCOUNT_HEAD_ID = p_account_head_Id;

                mCashSkimming.ExecuteQuery();

                return mCashSkimming.SKIMMING_ID.ToString();
            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        public DataTable SelectCashSkimming(int p_Distributor_Id, int p_CashierId, int p_TYPEID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectCASH_SKIMMING mCashSkimming = new spSelectCASH_SKIMMING();
                mCashSkimming.Connection = mConnection;
                mCashSkimming.DISTRIBUTOR_ID = p_Distributor_Id;
                mCashSkimming.CASHIER_ID = p_CashierId;
                mCashSkimming.TYPEID = p_TYPEID;
                DataTable dt = mCashSkimming.ExecuteTable();
                return dt;

            }
            catch (Exception exp)
            {
                ExceptionPublisher.PublishException(exp);
                throw;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }

        #region Cash Skimming Report
        public DataSet GetSkimmingReport(int p_DISTRIBUTOR_ID, DateTime p_FromDate, DateTime p_ToDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                spSelectCASH_SKIMMING objPrint = new spSelectCASH_SKIMMING();
                Reports.DsReport ds = new Reports.DsReport();
                objPrint.Connection = mConnection;
                objPrint.DISTRIBUTOR_ID = p_DISTRIBUTOR_ID;
                objPrint.FROM_DATE = p_FromDate;
                objPrint.TO_DATE = p_ToDate;
                DataTable dt = objPrint.ExecuteTableForCashSkimmingReport();
                foreach (DataRow dr in dt.Rows)
                {
                    ds.Tables["RptCashSkimming"].ImportRow(dr);
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
