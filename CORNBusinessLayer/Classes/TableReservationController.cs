using System;
using System.Data;
using System.IO;
using CORNCommon.Classes;
using CORNDataAccessLayer.Classes;
using CORNDatabaseLayer.Classes;
using CORNBusinessLayer.Classes;
using System.Collections.Generic;

namespace CORNBusinessLayer.Classes
{
    /// <summary>
    /// Class For Table Reservation Related Tasks
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// Insert SKU
    /// </item>
    /// <term>
    /// Update SKU
    /// </term>
    /// <item>
    /// Get SKU
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
	public class TableReservationController
    {
        #region Constructors

        /// <summary>
        /// Constructor For TableReservationController
        /// </summary>
        public TableReservationController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region Select
        
        public DataTable GetTimeSlot(int p_TypeID,int p_DistributorID)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetAddEditTableReservation mSlot = new uspGetAddEditTableReservation();
                mSlot.Connection = mConnection;
                mSlot.DistributorID = p_DistributorID;
                mSlot.TypeID = p_TypeID;
                DataTable ds = mSlot.ExecuteTable();

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
        public DataTable GetTableReservation(int p_TypeID,int p_DistributorID, DateTime p_ReservationDate)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetAddEditTableReservation mtblReser = new uspGetAddEditTableReservation();
                mtblReser.Connection = mConnection;
                mtblReser.TypeID = p_TypeID;
                mtblReser.ReservationDate = p_ReservationDate;
                mtblReser.DistributorID = p_DistributorID;
                DataTable ds = mtblReser.ExecuteTable();

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
        public DataTable GetTableReservationDetail(int p_TypeID, long p_TableReservationMasterCode)
        {
            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                uspGetAddEditTableReservation mtblReser = new uspGetAddEditTableReservation();
                mtblReser.Connection = mConnection;
                mtblReser.TypeID = p_TypeID;
                mtblReser.TableReservationMasterCode = p_TableReservationMasterCode;
                DataTable ds = mtblReser.ExecuteTable();

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

        #region Insert/Update

        public bool InsertTableReservation(int p_DistributorID,DateTime p_DocumentDate,DateTime p_ReservationDate,DateTime p_ReservationTime,int p_NoOfGuest,int p_BooingSlot,long p_CustomerID,string p_Remarks,int p_SourceID,int p_CustomerTypeID, int p_UserID,int p_TypeID,DataTable dtDetail)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                if (dtDetail.Rows.Count > 0)
                {
                    uspGetAddEditTableReservation mTableRes = new uspGetAddEditTableReservation();
                    mTableRes.Connection = mConnection;

                    mTableRes.DistributorID = p_DistributorID;
                    mTableRes.DocumentDate = p_DocumentDate;
                    mTableRes.ReservationDate = p_ReservationDate;
                    mTableRes.ReservationTime = p_ReservationTime;
                    mTableRes.NoOfGuest = p_NoOfGuest;
                    mTableRes.BooingSlot = p_BooingSlot;
                    mTableRes.CustomerID = p_CustomerID;
                    mTableRes.SourceID = p_SourceID;
                    mTableRes.CustomerTypeID = p_CustomerTypeID;
                    mTableRes.UserID = p_UserID;
                    mTableRes.TypeID = p_TypeID;
                    mTableRes.Remarks = p_Remarks;
                    mTableRes.ExecuteQuery();

                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        uspInsertTableReservationDetail mTableResDetail = new uspInsertTableReservationDetail();
                        mTableResDetail.Connection = mConnection;
                        mTableResDetail.lngTableReservationMasterCode = mTableRes.lngTableReservationMasterCode;
                        mTableResDetail.TableID = Convert.ToInt32(dr["TableID"]);
                        mTableResDetail.ExecuteQuery();
                    }                    
                }
                return true;
            }
            catch (Exception exp)
            {
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

        public bool UpdateTableReservation(long p_TableReservationMasterCode,DateTime p_ReservationDate, DateTime p_ReservationTime, int p_NoOfGuest, int p_BooingSlot, string p_Remarks,int p_SourceID,int p_CustomerTypeID, int p_UserID, int p_TypeID, DataTable dtDetail)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();
                if (dtDetail.Rows.Count > 0)
                {
                    uspGetAddEditTableReservation mTableRes = new uspGetAddEditTableReservation();
                    mTableRes.Connection = mConnection;

                    mTableRes.TableReservationMasterCode = p_TableReservationMasterCode;
                    mTableRes.ReservationDate = p_ReservationDate;
                    mTableRes.ReservationTime = p_ReservationTime;
                    mTableRes.NoOfGuest = p_NoOfGuest;
                    mTableRes.BooingSlot = p_BooingSlot;
                    mTableRes.SourceID = p_SourceID;
                    mTableRes.CustomerTypeID = p_CustomerTypeID;
                    mTableRes.UserID = p_UserID;
                    mTableRes.TypeID = p_TypeID;
                    mTableRes.Remarks = p_Remarks;
                    mTableRes.ExecuteQueryUpdate();

                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        uspInsertTableReservationDetail mTableResDetail = new uspInsertTableReservationDetail();
                        mTableResDetail.Connection = mConnection;
                        mTableResDetail.lngTableReservationMasterCode = p_TableReservationMasterCode;
                        mTableResDetail.TableID = Convert.ToInt32(dr["TableID"]);
                        mTableResDetail.ExecuteQuery();
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
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

        public bool CancelReservation(long p_TableReservationMasterCode,string p_CancelationReason, int p_UserID,int p_CancelledBy, int p_TypeID)
        {

            IDbConnection mConnection = null;
            try
            {
                mConnection = ProviderFactory.GetConnection(Configuration.ConnectionString, EnumProviders.SQLClient);
                mConnection.Open();

                uspGetAddEditTableReservation mTableRes = new uspGetAddEditTableReservation();
                mTableRes.Connection = mConnection;
                mTableRes.TableReservationMasterCode = p_TableReservationMasterCode;
                mTableRes.CancelationReason = p_CancelationReason;
                mTableRes.UserID = p_UserID;
                mTableRes.CancelledBy = p_CancelledBy;
                mTableRes.TypeID = p_TypeID;                
                mTableRes.ExecuteQueryUpdate();
                return true;
            }
            catch (Exception exp)
            {
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

        #endregion
    }
}