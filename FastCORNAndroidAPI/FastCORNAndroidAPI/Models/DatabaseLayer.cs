using System;
using System.Data;
using System.Data.SqlClient;
using FastAndroidAPI.Models;
using System.Collections.Generic;

namespace CORNAttendanceApi.Models
{
    public class DatabaseLayer
    {
        public DataTable GetClientConnection(string ClientPIN)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["myConnectionString"].ToString());
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UspGetClientConnectionInsight";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@ClientPIN", ClientPIN);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }


        public DataTable GetAndoridUser(string LoginID, string Password, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetUSERSAndroid";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@LOGIN_ID", LoginID);
                cmd.Parameters.AddWithValue("@PASSWORD", Password);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetArea(int p_Distributor_Id, int p_SaleForce_Id, int p_Area_Id, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetSaleForceArea";
                cmd.Connection = con;
                if (p_Distributor_Id == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", p_Distributor_Id);
                }
                if (p_SaleForce_Id == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@SALEFORCE_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SALEFORCE_ID", p_SaleForce_Id);
                }
                if (p_Area_Id == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@AREA_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AREA_ID", p_Area_Id);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetRoute(int p_DISTRIBUTOR_ID, int p_USER_ID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetAssignedDISTRIBUTOR_ROUTE";
                cmd.Connection = con;
                if (p_USER_ID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@USER_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@USER_ID", p_USER_ID);
                }
                if (p_DISTRIBUTOR_ID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", p_DISTRIBUTOR_ID);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetCustomer(int p_DISTRIBUTOR_ID, int p_USER_ID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetSaleForceCustomerSync";
                cmd.Connection = con;
                cmd.CommandTimeout = 0;
                if (p_USER_ID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@SALEFORCE_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SALEFORCE_ID", p_USER_ID);
                }
                if (p_DISTRIBUTOR_ID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", p_DISTRIBUTOR_ID);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetChannel(int p_ParentID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spSelectCODESAndroid";
                cmd.Connection = con;
                if (p_ParentID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@PARENT_REF_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PARENT_REF_ID", p_ParentID);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetSKUHierarchy(int p_Sku_Type_Id, int p_Sku_Hie_Id, int p_Parent_Sku_Hie_Id, int Companyid, int p_USER_ID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spSelectSKU_HIERARCHYAssigned";
                cmd.Connection = con;
                if (p_Sku_Hie_Id == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@SKU_HIE_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SKU_HIE_ID", p_Sku_Hie_Id);
                }
                if (Companyid == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@COMPANY_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Companyid);
                }
                if (p_Parent_Sku_Hie_Id == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@PARENT_SKU_HIE_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PARENT_SKU_HIE_ID", p_Parent_Sku_Hie_Id);
                }
                if (p_Sku_Type_Id == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@SKU_HIE_TYPE_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SKU_HIE_TYPE_ID", p_Sku_Type_Id);
                }
                if (p_USER_ID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@USER_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@USER_ID", p_USER_ID);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetSKU(int p_Distributor_ID, int p_Principal_Id, int p_User_Id, DateTime p_ClosedDate, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetSKUAndroid";
                cmd.Connection = con;
                if (p_User_Id == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@USER_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@USER_ID", p_User_Id);
                }
                if (p_Distributor_ID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", p_Distributor_ID);
                }
                if (p_Principal_Id == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@PRINCIPAL_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PRINCIPAL_ID", p_Principal_Id);
                }
                if (p_ClosedDate == Constants.DateNullValue)
                {
                    cmd.Parameters.AddWithValue("@DAYCLOSED", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DAYCLOSED", p_ClosedDate);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetSKUGroup(string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspSelectSKU_Groups";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Dist_ID", DBNull.Value);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetPromotion(int p_DISTRIBUTOR_ID, DateTime pDate, int p_TypeID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetPromotionAndroid";
                cmd.Connection = con;
                if (p_DISTRIBUTOR_ID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", p_DISTRIBUTOR_ID);
                }
                if (p_TypeID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@TypeID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TypeID", p_TypeID);
                }
                if (pDate == Constants.DateNullValue)
                {
                    cmd.Parameters.AddWithValue("@DATE", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DATE", pDate);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetUnOrderStatus(string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetUnOrderStatus";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetAndoridMenu(int p_UserID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetAndroidMenu";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@USER_ID", p_UserID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetComplaintReason(int p_DistributorID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetComplaintReasonAndroid";
                cmd.Connection = con;
                if (p_DistributorID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@DistributorID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DistributorID", p_DistributorID);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetImagePath(string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspGetImagePath";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable InsertOrderBookerStartDay(int p_DISTRIBUTOR_ID, int p_USER_ID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            try
            {
                con.Open();

                SqlCommand cmdDetail = new SqlCommand();
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "uspInsertAndroidOBDailyProcess";
                cmdDetail.Connection = con;

                if (p_DISTRIBUTOR_ID == Constants.IntNullValue)
                {
                    cmdDetail.Parameters.AddWithValue("@DistributorID", DBNull.Value);
                }
                else
                {
                    cmdDetail.Parameters.AddWithValue("@DistributorID", p_DISTRIBUTOR_ID);
                }

                if (p_USER_ID == Constants.IntNullValue)
                {
                    cmdDetail.Parameters.AddWithValue("@OrderBookerID", DBNull.Value);
                }
                else
                {
                    cmdDetail.Parameters.AddWithValue("@OrderBookerID", p_USER_ID);
                }

                if (p_USER_ID == Constants.IntNullValue)
                {
                    cmdDetail.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                }
                else
                {
                    cmdDetail.Parameters.AddWithValue("@CreatedBy", p_USER_ID);
                }

                cmdDetail.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmdDetail);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertOrderBookerEndDay(int p_DISTRIBUTOR_ID, int p_USER_ID, DateTime p_StartDayDateTime, string ConnectionString)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            try
            {
                con.Open();

                SqlCommand cmdDetail = new SqlCommand();
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "uspUpdateAndroidOBDailyProcess";
                cmdDetail.Connection = con;

                if (p_DISTRIBUTOR_ID == Constants.IntNullValue)
                {
                    cmdDetail.Parameters.AddWithValue("@DistributorID", DBNull.Value);
                }
                else
                {
                    cmdDetail.Parameters.AddWithValue("@DistributorID", p_DISTRIBUTOR_ID);
                }

                if (p_USER_ID == Constants.IntNullValue)
                {
                    cmdDetail.Parameters.AddWithValue("@OrderBookerID", DBNull.Value);
                }
                else
                {
                    cmdDetail.Parameters.AddWithValue("@OrderBookerID", p_USER_ID);
                }

                if (p_StartDayDateTime == Constants.DateNullValue)
                {
                    cmdDetail.Parameters.AddWithValue("@StartDayDateTime", DBNull.Value);
                }
                else
                {
                    cmdDetail.Parameters.AddWithValue("@StartDayDateTime", p_StartDayDateTime);
                }

                if (p_USER_ID == Constants.IntNullValue)
                {
                    cmdDetail.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                }
                else
                {
                    cmdDetail.Parameters.AddWithValue("@CreatedBy", p_USER_ID);
                }

                cmdDetail.ExecuteNonQuery();

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable ExecGetStoreProcedure(SpData data, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = data.SpName;
                cmd.Connection = con;
                if (data.Parameters.Count > 0)
                {
                    foreach (var item in data.Parameters)
                        cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataSet ExecGetStoreProcedureDataSet(SpData data, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = data.SpName;
                cmd.Connection = con;
                if (data.Parameters.Count > 0)
                {
                    foreach (var item in data.Parameters)
                        cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public long AddInvoice(int p_LocationID, decimal p_GST, long p_CUSTOMER_ID,int p_UserID,string p_Instructions,string p_DeliveryAddress, List<OrderDetail> dtOrderDetail, string ConnectionString)
        {
            long m_SALE_INVOICE_ID = 0;
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                con.Open();
                objTrans = con.BeginTransaction();
                SqlCommand cmd = new SqlCommand() { Transaction = objTrans, CommandType = CommandType.StoredProcedure, CommandText = "spInsertSALE_INVOICE_MASTERCORNOrderAPI", Connection = con };
                if (p_LocationID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", p_LocationID);
                }
                decimal AMOUNTDUE = 0;
                foreach (var dr in dtOrderDetail)
                {
                    AMOUNTDUE += dr.Quantity * dr.Price;
                }
                cmd.Parameters.AddWithValue("@AMOUNTDUE", AMOUNTDUE);
                cmd.Parameters.AddWithValue("@BALANCE", 0);
                
                cmd.Parameters.AddWithValue("@IS_HOLD", true);
                if (p_GST == Constants.DecimalNullValue)
                {
                    cmd.Parameters.AddWithValue("@GST", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@GST", p_GST);
                }
                cmd.Parameters.AddWithValue("@PAYMENT_MODE_ID", DBNull.Value);
                cmd.Parameters.AddWithValue("@PAIDIN", 0);
                cmd.Parameters.AddWithValue("@CUSTOMER_TYPE_ID", 2);
                cmd.Parameters.AddWithValue("@TABLE_ID", 0);
                if (p_CUSTOMER_ID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@USER_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@USER_ID", p_UserID);
                }
                cmd.Parameters.AddWithValue("@orderBookerId", 0);
                cmd.Parameters.AddWithValue("@approvedby", 0);
                cmd.Parameters.AddWithValue("@CUSTOMER_ID", p_CUSTOMER_ID);
                cmd.Parameters.AddWithValue("@ORDER_NO", DBNull.Value);
                cmd.Parameters.AddWithValue("@TAKEAWAY_CUSTOMER", DBNull.Value);
                cmd.Parameters.AddWithValue("@MANUAL_ORDER_NO", DBNull.Value);
                cmd.Parameters.AddWithValue("@REMARKS", p_Instructions);
                cmd.Parameters.AddWithValue("@DeliveryType", 1);
                cmd.Parameters.AddWithValue("@DELIVERY_CHANNEL", 1);
                cmd.Parameters.AddWithValue("@SERVICE_CHARGES_TYPE", 0);
                cmd.Parameters.AddWithValue("@DeliveryAddress", p_DeliveryAddress);

                SqlParameter parm = new SqlParameter("@SALE_INVOICE_ID", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parm);

                cmd.ExecuteNonQuery();
                m_SALE_INVOICE_ID = (long)((IDataParameter)(cmd.Parameters["@SALE_INVOICE_ID"])).Value;

                if (m_SALE_INVOICE_ID > 0)
                {
                    foreach (var dr in dtOrderDetail)
                    {
                        SqlCommand cmdDetail = new SqlCommand() { Transaction = objTrans, CommandType = CommandType.StoredProcedure, CommandText = "spInsertSALE_INVOICE_DETAILCORNOrderAPI", Connection = con };

                        cmdDetail.Parameters.AddWithValue("@SALE_INVOICE_ID", m_SALE_INVOICE_ID);
                        cmdDetail.Parameters.AddWithValue("@IS_VOID", false);
                        if (dr.Price == Constants.DecimalNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@PRICE", DBNull.Value);
                        }
                        else
                        {
                            cmdDetail.Parameters.AddWithValue("@PRICE", dr.Price);
                        }
                        if (dr.PRODUCT_CATEGORY_ID == Constants.IntNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@PRODUCT_CATEGORY_ID", DBNull.Value);
                        }
                        else
                        {
                            cmdDetail.Parameters.AddWithValue("@PRODUCT_CATEGORY_ID", dr.PRODUCT_CATEGORY_ID);
                        }
                        if (dr.Quantity == Constants.IntNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@QTY", DBNull.Value);
                            cmdDetail.Parameters.AddWithValue("@ORIGINAL_QTY", DBNull.Value);
                        }
                        else
                        {
                            cmdDetail.Parameters.AddWithValue("@QTY", dr.Quantity);
                            cmdDetail.Parameters.AddWithValue("@ORIGINAL_QTY", dr.Quantity);
                        }
                        if (dr.ITEM_DEAL_ID == Constants.IntNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@ITEM_DEAL_ID", 0);
                        }
                        else
                        {
                            cmdDetail.Parameters.AddWithValue("@ITEM_DEAL_ID", dr.ITEM_DEAL_ID);
                        }

                        if (dr.Deal_Price == Constants.DecimalNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@DEAL_PRICE", 0);
                        }
                        else
                        {
                            cmdDetail.Parameters.AddWithValue("@DEAL_PRICE", dr.Deal_Price);
                        }
                        cmdDetail.Parameters.AddWithValue("@REMARKS", "H");
                        cmdDetail.Parameters.AddWithValue("@C1", 0);
                        cmdDetail.Parameters.AddWithValue("@C2", 0);
                        if (dr.SKUID == Constants.IntNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@SKU_ID", DBNull.Value);
                        }
                        else
                        {
                            cmdDetail.Parameters.AddWithValue("@SKU_ID", dr.SKUID);
                        }
                        if (dr.Deal_Item_Qty == Constants.DecimalNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@DealDetailQTY", 0);
                        }
                        else
                        {
                            cmdDetail.Parameters.AddWithValue("@DealDetailQTY", dr.Deal_Item_Qty);
                        }
                        if (dr.Deal_Qty == Constants.DecimalNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@DealQTY", 0);
                        }
                        else
                        {
                            cmdDetail.Parameters.AddWithValue("@DealQTY", dr.Deal_Qty);
                        }
                        cmdDetail.Parameters.AddWithValue("@DISTRIBUTOR_ID", p_LocationID);
                        cmdDetail.Parameters.AddWithValue("@VOID_BY", DBNull.Value);
                        if (dr.Deal_Assignment_ID == Constants.IntNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@intDealID", 0);
                        }
                        else
                        {
                            cmdDetail.Parameters.AddWithValue("@intDealID", dr.Deal_Assignment_ID);
                        }
                        if (dr.Deal_Assignment_Detail_ID == Constants.LongNullValue)
                        {
                            cmdDetail.Parameters.AddWithValue("@lngDealDetailID", 0);
                        }
                        else {
                            cmdDetail.Parameters.AddWithValue("@lngDealDetailID", dr.Deal_Assignment_Detail_ID);
                        }
                        cmdDetail.Parameters.AddWithValue("@IS_FREE", false);
                        cmdDetail.ExecuteNonQuery();
                    }

                    objTrans.Commit();
                }
                else
                {
                    objTrans.Rollback();
                }
                return m_SALE_INVOICE_ID;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return -1;
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertCustomerAndroid(int p_DISTRIBUTOR_ID, int p_USER_ID, List<Outlet> dtDetail, string ConnectionString)
        {
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmdFree = new SqlCommand();
            try
            {
                con.Open();
                objTrans = con.BeginTransaction();

                foreach (var dr in dtDetail)
                {
                    cmdFree = new SqlCommand();
                    cmdFree.Transaction = objTrans;
                    cmdFree.CommandType = CommandType.StoredProcedure;
                    cmdFree.CommandText = "uspSelectSETTINGS_TABLE";
                    cmdFree.Connection = con;
                    if (p_DISTRIBUTOR_ID == Constants.IntNullValue)
                    {
                        cmdFree.Parameters.AddWithValue("@DistributorId", DBNull.Value);
                    }
                    else
                    {
                        cmdFree.Parameters.AddWithValue("@DistributorId", p_DISTRIBUTOR_ID);
                    }
                    cmdFree.Parameters.AddWithValue("@TableName", "CUSTOMERTEMPORARY");
                    cmdFree.Parameters.AddWithValue("@FieldName", "CUSTOMER_ID");
                    SqlDataAdapter da = new SqlDataAdapter(cmdFree);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    long CustomerId = 0;
                    string StrCode = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        CustomerId = long.Parse(ds.Tables[0].Rows[0]["Value"].ToString()) + 1;

                        if (CustomerId.ToString().Length == 1)
                        {
                            StrCode = "OT0000" + CustomerId.ToString();
                        }
                        else if (CustomerId.ToString().Length == 2)
                        {
                            StrCode = "OT000" + CustomerId.ToString();
                        }
                        else if (CustomerId.ToString().Length == 3)
                        {
                            StrCode = "OT00" + CustomerId.ToString();
                        }
                        else if (CustomerId.ToString().Length == 4)
                        {
                            StrCode = "OT0" + CustomerId.ToString();
                        }
                        else if (CustomerId.ToString().Length == 5)
                        {
                            StrCode = "OT" + CustomerId.ToString();
                        }
                    }

                    SqlCommand cmdDetail = new SqlCommand();
                    cmdDetail.Transaction = objTrans;
                    cmdDetail.CommandType = CommandType.StoredProcedure;
                    cmdDetail.CommandText = "uspInsertCUSTOMER_ANDROID";
                    cmdDetail.Connection = con;

                    cmdDetail.Parameters.AddWithValue("@CUSTOMER_ID", CustomerId);

                    if (p_DISTRIBUTOR_ID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@DISTRIBUTOR_ID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@DISTRIBUTOR_ID", p_DISTRIBUTOR_ID);
                    }

                    if (p_USER_ID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@USER_ID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@USER_ID", p_USER_ID);
                    }

                    cmdDetail.Parameters.AddWithValue("@CUSTOMER_CODE", StrCode);

                    if (dr.OutletName == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@CUSTOMER_NAME", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@CUSTOMER_NAME", dr.OutletName);
                    }

                    if (dr.OwnerName == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@CONTACT_PERSON", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@CONTACT_PERSON", dr.OwnerName);
                    }

                    if (dr.PhoneNumber == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@MOBILE_NUMBER", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@MOBILE_NUMBER", dr.PhoneNumber);
                    }
                    if (dr.OutletAddress == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@ADDRESS", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@ADDRESS", dr.OutletAddress);
                    }

                    if (dr.SectionID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@AREA_ID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@AREA_ID", dr.SectionID);
                    }

                    cmdDetail.Parameters.AddWithValue("@ROUTE_ID", 0);

                    if (dr.ChannelID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@CHANNEL_TYPE_ID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@CHANNEL_TYPE_ID", dr.ChannelID);
                    }
                    if (dr.TownID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@TOWN_ID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@TOWN_ID", dr.TownID);
                    }

                    if (dr.AreaTypeId == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@AreaTypeId", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@AreaTypeId", dr.AreaTypeId);
                    }

                    if (dr.SubChannelID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@SUB_CHANNEL_TYPE_ID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@SUB_CHANNEL_TYPE_ID", dr.SubChannelID);
                    }

                    if (dr.Comments == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@REMARKS", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@REMARKS", dr.Comments);
                    }

                    if (dr.LandMark == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@LandMark", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@LandMark", dr.LandMark);
                    }

                    if (dr.Latitude == Constants.DecimalNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@Latitude", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Latitude", dr.Latitude);
                    }

                    if (dr.Longtidue == Constants.DecimalNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@Longitdue", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Longitdue", dr.Longtidue);
                    }

                    if (dr.PhotoPath1 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath1", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath1", dr.PhotoPath1);
                    }

                    if (dr.PhotoPath2 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath2", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath2", dr.PhotoPath2);
                    }

                    if (dr.PhotoPath3 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath3", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath3", dr.PhotoPath3);
                    }

                    if (dr.PhotoPath4 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath4", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath4", dr.PhotoPath4);
                    }

                    if (dr.PhotoPath5 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath5", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@PhotoPath5", dr.PhotoPath5);
                    }

                    cmdDetail.Parameters.AddWithValue("@WHATSAPP_NO", "");
                    cmdDetail.Parameters.AddWithValue("@CNIC", "");

                    cmdDetail.ExecuteNonQuery();
                }
                objTrans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertCustomerComplaint(int p_DISTRIBUTOR_ID, int p_USER_ID, List<OutletCompalint> dtDetail, string ConnectionString)
        {
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmdFree = new SqlCommand();
            try
            {
                con.Open();
                objTrans = con.BeginTransaction();

                foreach (var dr in dtDetail)
                {

                    SqlCommand cmdDetail = new SqlCommand();
                    cmdDetail.Transaction = objTrans;
                    cmdDetail.CommandType = CommandType.StoredProcedure;
                    cmdDetail.CommandText = "uspInsertCustomerComplaint";
                    cmdDetail.Connection = con;

                    if (p_DISTRIBUTOR_ID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@DistributorID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@DistributorID", p_DISTRIBUTOR_ID);
                    }

                    if (dr.CustomerID == Constants.LongNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@CustomerID", dr.CustomerID);
                    }

                    if (dr.DocumentDate == Constants.DateNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@DocumentDate", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@DocumentDate", dr.DocumentDate);
                    }

                    if (dr.ComplaintReasonID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@ComplaintReasonID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@ComplaintReasonID", dr.ComplaintReasonID);
                    }

                    if (dr.Remarks == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Remarks", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Remarks", dr.Remarks);
                    }

                    if (p_USER_ID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@CreatedBy", p_USER_ID);
                    }

                    cmdDetail.ExecuteNonQuery();
                }
                objTrans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertMarchandise(int p_DISTRIBUTOR_ID, int p_USER_ID, List<Marchandise> dtDetail, string ConnectionString)
        {
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmdFree = new SqlCommand();
            try
            {
                con.Open();
                objTrans = con.BeginTransaction();

                foreach (var dr in dtDetail)
                {
                    SqlCommand cmdDetail = new SqlCommand();
                    cmdDetail.Transaction = objTrans;
                    cmdDetail.CommandType = CommandType.StoredProcedure;
                    cmdDetail.CommandText = "uspInsertMarchandise";
                    cmdDetail.Connection = con;

                    if (p_DISTRIBUTOR_ID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@DistributorID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@DistributorID", p_DISTRIBUTOR_ID);
                    }

                    if (dr.OutletID == Constants.LongNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@CustomerID", dr.OutletID);
                    }

                    if (dr.DocumentDate == Constants.DateNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@DocumentDate", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@DocumentDate", dr.DocumentDate);
                    }

                    if (dr.Remarks == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Remarks", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Remarks", dr.Remarks);
                    }

                    if (dr.BFMarchanImg1 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path1", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path1", dr.BFMarchanImg1);
                    }

                    if (dr.BFMarchanImg2 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path2", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path2", dr.BFMarchanImg2);
                    }

                    if (dr.BFMarchanImg3 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path3", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path3", dr.BFMarchanImg3);
                    }

                    if (dr.BFMarchanImg4 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path4", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path4", dr.BFMarchanImg4);
                    }

                    if (dr.BFMarchanImg5 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path5", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image1Path5", dr.BFMarchanImg5);
                    }

                    if (dr.AFMarchanImg1 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path1", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path1", dr.AFMarchanImg1);
                    }

                    if (dr.AFMarchanImg2 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path2", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path2", dr.AFMarchanImg2);
                    }

                    if (dr.AFMarchanImg3 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path3", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path3", dr.AFMarchanImg3);
                    }

                    if (dr.AFMarchanImg4 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path4", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path4", dr.AFMarchanImg4);
                    }

                    if (dr.AFMarchanImg5 == null)
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path5", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@Image2Path5", dr.AFMarchanImg5);
                    }

                    if (p_USER_ID == Constants.IntNullValue)
                    {
                        cmdDetail.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                    }
                    else
                    {
                        cmdDetail.Parameters.AddWithValue("@CreatedBy", p_USER_ID);
                    }

                    cmdDetail.ExecuteNonQuery();
                }
                objTrans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertReplacement(int p_DistributorID, long p_CustomerID, DateTime p_DocumentDate, int p_ReplacementTypeID, int p_CreatedBy, List<Replacement> dtDetail, string ConnectionString)
        {
            long mReplacementID = 0;
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                if (dtDetail.Count > 0)
                {
                    con.Open();
                    objTrans = con.BeginTransaction();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Transaction = objTrans;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "uspInsertReplacementMaster";
                    cmd.Connection = con;
                    if (p_DistributorID == Constants.IntNullValue)
                    {
                        cmd.Parameters.AddWithValue("@DistributorID", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DistributorID", p_DistributorID);
                    }

                    if (p_CustomerID == Constants.LongNullValue)
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", p_CustomerID);
                    }

                    if (p_DocumentDate == Constants.DateNullValue)
                    {
                        cmd.Parameters.AddWithValue("@DocumentDate", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DocumentDate", p_DocumentDate);
                    }

                    if (p_ReplacementTypeID == Constants.IntNullValue)
                    {
                        cmd.Parameters.AddWithValue("@ReplacementTypeID", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ReplacementTypeID", p_ReplacementTypeID);
                    }

                    if (p_CreatedBy == Constants.IntNullValue)
                    {
                        cmd.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CreatedBy", p_CreatedBy);
                    }


                    SqlParameter parm = new SqlParameter("@ReplacementID", SqlDbType.BigInt);
                    parm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parm);


                    cmd.ExecuteNonQuery();
                    mReplacementID = (long)((IDataParameter)(cmd.Parameters["@ReplacementID"])).Value;

                    if (mReplacementID > 0)
                    {
                        foreach (var dr in dtDetail)
                        {
                            SqlCommand cmdDetail = new SqlCommand();
                            cmdDetail.Transaction = objTrans;
                            cmdDetail.CommandType = CommandType.StoredProcedure;
                            cmdDetail.CommandText = "uspInsertReplacementDetail";
                            cmdDetail.Connection = con;

                            cmdDetail.Parameters.AddWithValue("@ReplacementID", mReplacementID);

                            if (p_DistributorID == Constants.IntNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@DistributorID", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@DistributorID", p_DistributorID);
                            }

                            if (dr.ReasonID == Constants.IntNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@ReasonID", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@ReasonID", dr.ReasonID);
                            }

                            if (dr.SKUID == Constants.IntNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@SKUID", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@SKUID", dr.SKUID);
                            }

                            if (dr.BATCHNO == null)
                            {
                                cmdDetail.Parameters.AddWithValue("BATCHNO", dr.BATCHNO);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("BATCHNO", DBNull.Value);
                            }

                            if (dr.Price == Constants.DecimalNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@Price", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@Price", dr.Price);
                            }

                            if (dr.Quantity == Constants.IntNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@Quantity", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@Quantity", dr.Quantity);
                            }

                            if (dr.InvoiceNo == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceNo", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceNo", dr.InvoiceNo);
                            }

                            if (dr.InvoiceDate == Constants.DateNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceDate", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceDate", dr.InvoiceDate);
                            }

                            if (dr.Description == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@Description", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@Description", dr.Description);
                            }

                            if (dr.StockImage1 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage1", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage1", dr.StockImage1);
                            }

                            if (dr.StockImage2 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage2", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage2", dr.StockImage2);
                            }

                            if (dr.StockImage3 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage3", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage3", dr.StockImage3);
                            }

                            if (dr.StockImage4 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage4", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage4", dr.StockImage4);
                            }

                            if (dr.StockImage5 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage5", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@StockImage5", dr.StockImage5);
                            }

                            if (dr.InvoiceImage1 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage1", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage1", dr.InvoiceImage1);
                            }

                            if (dr.InvoiceImage2 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage2", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage2", dr.InvoiceImage2);
                            }

                            if (dr.InvoiceImage3 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage3", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage3", dr.InvoiceImage3);
                            }

                            if (dr.InvoiceImage4 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage4", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage4", dr.InvoiceImage4);
                            }

                            if (dr.InvoiceImage5 == null)
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage5", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@InvoiceImage5", dr.InvoiceImage5);
                            }
                            cmdDetail.ExecuteNonQuery();
                        }
                        objTrans.Commit();
                    }
                    else
                    {
                        objTrans.Rollback();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertCustomerStock(int p_DistributorID, long p_CustomerID, DateTime p_DocumentDate, int p_CreatedBy, List<StockPosition> dtDetail, string ConnectionString)
        {
            long mCustomerStocktID = 0;
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                if (dtDetail.Count > 0)
                {
                    con.Open();
                    objTrans = con.BeginTransaction();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Transaction = objTrans;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "uspInsertCustomerStocktMaster";
                    cmd.Connection = con;
                    if (p_DistributorID == Constants.IntNullValue)
                    {
                        cmd.Parameters.AddWithValue("@DistributorID", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DistributorID", p_DistributorID);
                    }

                    if (p_CustomerID == Constants.LongNullValue)
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", p_CustomerID);
                    }

                    if (p_DocumentDate == Constants.DateNullValue)
                    {
                        cmd.Parameters.AddWithValue("@DocumentDate", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DocumentDate", p_DocumentDate);
                    }


                    if (p_CreatedBy == Constants.IntNullValue)
                    {
                        cmd.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CreatedBy", p_CreatedBy);
                    }


                    SqlParameter parm = new SqlParameter("@CustomerStocktID", SqlDbType.BigInt);
                    parm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parm);


                    cmd.ExecuteNonQuery();
                    mCustomerStocktID = (long)((IDataParameter)(cmd.Parameters["@CustomerStocktID"])).Value;

                    if (mCustomerStocktID > 0)
                    {
                        foreach (var dr in dtDetail)
                        {
                            SqlCommand cmdDetail = new SqlCommand();
                            cmdDetail.Transaction = objTrans;
                            cmdDetail.CommandType = CommandType.StoredProcedure;
                            cmdDetail.CommandText = "uspInsertCustomerStocktDetail";
                            cmdDetail.Connection = con;

                            cmdDetail.Parameters.AddWithValue("@CustomerStocktID", mCustomerStocktID);

                            if (p_DistributorID == Constants.IntNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@DistributorID", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@DistributorID", p_DistributorID);
                            }


                            if (dr.SKUID == Constants.IntNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@SKUID", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@SKUID", dr.SKUID);
                            }

                            if (dr.Quantity == Constants.IntNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@Quantity", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@Quantity", dr.Quantity);
                            }

                            if (dr.Unit_In_Case == Constants.IntNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@Unit_In_Case", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@Unit_In_Case", dr.Unit_In_Case);
                            }

                            if (dr.Price == Constants.DecimalNullValue)
                            {
                                cmdDetail.Parameters.AddWithValue("@Price", DBNull.Value);
                            }
                            else
                            {
                                cmdDetail.Parameters.AddWithValue("@Price", dr.Price);
                            }

                            cmdDetail.ExecuteNonQuery();
                        }
                        objTrans.Commit();
                    }
                    else
                    {
                        objTrans.Rollback();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        public long InsertCustomer(int pLocationID, string pEmail, string pPassword, string pFName, string pLName, string pAddress
            , string pCountry, string pProvince, string pCity, string pPhoneNo, string pZipCode, string ConnectionString)
        {
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            try
            {
                long m_CustomerID = 0;
                con.Open();
                objTrans = con.BeginTransaction();

                SqlCommand cmdDetail = new SqlCommand();
                cmdDetail.Transaction = objTrans;
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "uspInsertCustomerCORNOrderAPI";
                cmdDetail.Connection = con;
                cmdDetail.Parameters.AddWithValue("@LocationID", pLocationID);
                cmdDetail.Parameters.AddWithValue("@Email", pEmail.Trim());
                cmdDetail.Parameters.AddWithValue("@Password", pPassword);
                cmdDetail.Parameters.AddWithValue("@FName", pFName);
                cmdDetail.Parameters.AddWithValue("@LName", pLName);
                cmdDetail.Parameters.AddWithValue("@Address", pAddress);
                cmdDetail.Parameters.AddWithValue("@Country", pCountry);
                cmdDetail.Parameters.AddWithValue("@Province", pProvince);
                cmdDetail.Parameters.AddWithValue("@City", pCity);
                cmdDetail.Parameters.AddWithValue("@PhoneNo", pPhoneNo);
                cmdDetail.Parameters.AddWithValue("@ZipCode", pZipCode);
                SqlParameter parm = new SqlParameter("@CustomerID", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                cmdDetail.Parameters.Add(parm);

                cmdDetail.ExecuteNonQuery();
                m_CustomerID = (long)((IDataParameter)(cmdDetail.Parameters["@CustomerID"])).Value;
                objTrans.Commit();
                return m_CustomerID;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return 0;
            }
            finally
            {
                con.Close();
            }
        }

        public bool UpdateCustomer(long pCustomerID,int pLocationID, string pEmail, string pPassword, string pFName, string pLName, string pAddress
            , string pCountry, string pProvince, string pCity, string pZipCode, string ConnectionString)
        {
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            try
            {
                con.Open();
                objTrans = con.BeginTransaction();

                SqlCommand cmdDetail = new SqlCommand();
                cmdDetail.Transaction = objTrans;
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "uspUpdateCustomerCORNOrderAPI";
                cmdDetail.Connection = con;
                cmdDetail.Parameters.AddWithValue("@CustomerID", pCustomerID);
                cmdDetail.Parameters.AddWithValue("@LocationID", pLocationID);
                cmdDetail.Parameters.AddWithValue("@Email", pEmail.Trim());
                cmdDetail.Parameters.AddWithValue("@Password", pPassword);
                cmdDetail.Parameters.AddWithValue("@FName", pFName);
                cmdDetail.Parameters.AddWithValue("@LName", pLName);
                cmdDetail.Parameters.AddWithValue("@Address", pAddress);
                cmdDetail.Parameters.AddWithValue("@Country", pCountry);
                cmdDetail.Parameters.AddWithValue("@Province", pProvince);
                cmdDetail.Parameters.AddWithValue("@City", pCity);
                cmdDetail.Parameters.AddWithValue("@ZipCode", pZipCode);
                cmdDetail.ExecuteNonQuery();
                objTrans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetRiderOrderListing(int p_riderID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetRiderOrderListing";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@riderID", p_riderID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetRiderOrderDetails(int p_orderId, int p_riderID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetRiderOrderDetails";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@orderId", p_orderId);
                cmd.Parameters.AddWithValue("@riderID", p_riderID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetRiderRideStartedDetails(int p_orderId, int p_riderID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetRiderRideStartedDetails";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@orderId", p_orderId);
                cmd.Parameters.AddWithValue("@riderID", p_riderID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public bool UpdateDeliveryStatus(int p_orderId, int p_deliveryStatusId, string ConnectionString)
        {
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateDeliveryStatus";
                con.Open();
                
                if (p_orderId == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@orderId", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@orderId", p_orderId);
                }
                
               
                if (p_deliveryStatusId == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@deliveryStatusId", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@deliveryStatusId", p_deliveryStatusId);
                }

                int rows = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertRiderFeedback(long p_orderId, int p_riderId, int p_locationId, long p_customerId,
            int p_rate, string p_description, string ConnectionString)
        {
            //SqlTransaction objTrans = null;
            //SqlConnection con = new SqlConnection(ConnectionString);
            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Insert_RIDER_FEEDBACK";
                con.Open();

                if (p_orderId == Constants.LongNullValue)
                {
                    cmd.Parameters.AddWithValue("@SALE_INVOICE_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SALE_INVOICE_ID", p_orderId);
                }


                if (p_riderId == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@ORDER_BOOKER_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ORDER_BOOKER_ID", p_riderId);
                }

                if (p_locationId == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", p_locationId);
                }

                if (p_customerId == Constants.LongNullValue)
                {
                    cmd.Parameters.AddWithValue("@CUSTOMER_ID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CUSTOMER_ID", p_customerId);
                }

                if (p_rate == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@RATE", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RATE", p_rate);
                }

                cmd.Parameters.AddWithValue("@DESCRIPTION", p_description);


                int rows = cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                //objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertReturnedReason(long p_orderId, int p_riderId, string p_reason, string ConnectionString)
        {
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateDeliveryReturnedReson";
                con.Open();

                if (p_orderId == Constants.LongNullValue)
                {
                    cmd.Parameters.AddWithValue("@orderId", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@orderId", p_orderId);
                }


                if (p_riderId == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@riderID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@riderID", p_riderId);
                }

                cmd.Parameters.AddWithValue("@returnReason", p_reason);


                var result = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetRiderProfile(int p_riderID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetRiderProfile";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@userId", p_riderID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable InsertRiderTrackingMaster(long p_orderId, int p_riderId, int locationId, decimal p_distanceInKM, string ConnectionString)
        {
            //SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertRiderTrackingMaster";
                con.Open();

                if (p_orderId == Constants.LongNullValue)
                {
                    cmd.Parameters.AddWithValue("@orderId", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@orderId", p_orderId);
                }


                if (p_riderId == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@riderID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@riderID", p_riderId);
                }

                if (locationId == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@locationId", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@locationId", locationId);
                }

                if (p_distanceInKM == Constants.DecimalNullValue)
                {
                    cmd.Parameters.AddWithValue("@distanceInKM", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@distanceInKM", p_distanceInKM);
                }


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                //objTrans.Rollback();
                return new DataTable();
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertTrackingDetail(long p_trackingMasterId, decimal p_latitude, decimal p_longitude, string ConnectionString)
        {
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertRiderTrackingDetail";
                con.Open();

                if (p_trackingMasterId == Constants.LongNullValue)
                {
                    cmd.Parameters.AddWithValue("@trackingMasterId", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@trackingMasterId", p_trackingMasterId);
                }


                if (p_latitude == Constants.DecimalNullValue)
                {
                    cmd.Parameters.AddWithValue("@latitude", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@latitude", p_latitude);
                }

                if (p_longitude == Constants.DecimalNullValue)
                {
                    cmd.Parameters.AddWithValue("@longitude", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@longitude", p_longitude);
                }


                var result = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable InsertRiderTracking(long p_orderId, int p_riderId, int locationId, decimal p_latitude,decimal p_longitude, string ConnectionString)
        {
            //SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertRiderTracking";
                con.Open();

                if (p_orderId == Constants.LongNullValue)
                {
                    cmd.Parameters.AddWithValue("@orderId", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@orderId", p_orderId);
                }


                if (p_riderId == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@riderID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@riderID", p_riderId);
                }

                if (locationId == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@locationId", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@locationId", locationId);
                }

                if (p_latitude == Constants.DecimalNullValue)
                {
                    cmd.Parameters.AddWithValue("@latitude", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@latitude", p_latitude);
                }

                if (p_longitude == Constants.DecimalNullValue)
                {
                    cmd.Parameters.AddWithValue("@longitude", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@longitude", p_longitude);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                //objTrans.Rollback();
                return new DataTable();
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetRiderOrderHistory(int p_riderID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RiderOrderHistory";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@riderID", p_riderID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetCategoriesList(string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetCategoriesList";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetBranches(string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spSelectDISTRIBUTORForEcommerce";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetItemsINCategory(int categoryID, int typeID, int locationId, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                if (categoryID == Constants.IntNullValue)
                {
                    cmd.Parameters.AddWithValue("@CategoryID", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                }
                
                cmd.Parameters.AddWithValue("@TYPE", typeID);
                cmd.Parameters.AddWithValue("@PosCategoryID", DBNull.Value);
                cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", locationId);
                cmd.CommandText = "spGetItemsInECommerceCategory";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetCustomerProfile(long customerId, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.CommandText = "GetECommerceCustomerProfile";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetCustomerOrderHistory(long customerId, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.CommandText = "getCustomerOrderHistory";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetItemDeals(string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.CommandText = "GetDeals";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public long InsertCustomerAndUser(int pLocationID, string pEmail, string pPassword, string pFName, string pLName, string pAddress
            , string pCountry, string pProvince, string pCity, string pPhoneNo, string pZipCode, string ConnectionString)
        {
            SqlTransaction objTrans = null;
            SqlConnection con = new SqlConnection(ConnectionString);
            try
            {
                long m_CustomerID = 0;
                con.Open();
                objTrans = con.BeginTransaction();

                SqlCommand cmdDetail = new SqlCommand();
                cmdDetail.Transaction = objTrans;
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "createCustomerANDUser";
                cmdDetail.Connection = con;
                cmdDetail.Parameters.AddWithValue("@LocationID", pLocationID);
                cmdDetail.Parameters.AddWithValue("@Email", pEmail.Trim());
                cmdDetail.Parameters.AddWithValue("@Password", pPassword);
                cmdDetail.Parameters.AddWithValue("@FName", pFName);
                cmdDetail.Parameters.AddWithValue("@LName", pLName);
                cmdDetail.Parameters.AddWithValue("@Address", pAddress);
                cmdDetail.Parameters.AddWithValue("@Country", pCountry);
                cmdDetail.Parameters.AddWithValue("@Province", pProvince);
                cmdDetail.Parameters.AddWithValue("@City", pCity);
                cmdDetail.Parameters.AddWithValue("@PhoneNo", pPhoneNo);
                cmdDetail.Parameters.AddWithValue("@ZipCode", pZipCode);
                SqlParameter parm = new SqlParameter("@CustomerID", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                cmdDetail.Parameters.Add(parm);

                cmdDetail.ExecuteNonQuery();
                m_CustomerID = (long)((IDataParameter)(cmdDetail.Parameters["@CustomerID"])).Value;
                objTrans.Commit();
                return m_CustomerID;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                return 0;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetCafeStatus(DateTime dateAndTime, string day, int locationId, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dateAndTime", dateAndTime);
                cmd.Parameters.AddWithValue("@day", day);
                cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", locationId);
                cmd.CommandText = "CheckCafeStatus";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetECommerceItemById(string skuIds, int locationId, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SKU_ID", skuIds);
                cmd.Parameters.AddWithValue("@DISTRIBUTOR_ID", locationId);
                cmd.CommandText = "GetItemByID";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetLoginDetails(long CustomerID, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmd.CommandText = "GetEcommerceLoginINFO";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetLoginDetailsByLoginID(string username, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LOGIN_ID", username);
                cmd.CommandText = "GetEcommerceLoginINFOByLoginID";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetLoginDetailsByLoginID2(string username, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LOGIN_ID", username);
                cmd.CommandText = "uspGetUSERSAndroid";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetLoginDetailsByLoginID3(string username, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LOGIN_ID", username);
                cmd.CommandText = "uspGetUSERSAndroid2";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GetDealById(string dealId, string ConnectionString)
        {
            SqlConnection con = new SqlConnection();
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dealAssignnetId_AND_SKUID", dealId);
                cmd.CommandText = "GetDealById";
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
    }
}