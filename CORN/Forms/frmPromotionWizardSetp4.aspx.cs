using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmPromotionWizardSetp2 : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            SkuController mSkuController = new SkuController();
            DataTable m_dt_Sku = mSkuController.SelectSkuData(Constants.IntNullValue, int.Parse(this.Session["CompanyId"].ToString()));
            btnHome.Attributes.Add("onclick", "return ConfirmCancel()");
            clsListItems[] clsSKUItems = new clsListItems[m_dt_Sku.Rows.Count + 1];
            clsListItems[] clsSKUGroupItems = (clsListItems[])this.Session["clsSKUGroupItems"];
            clsListItems[] clsUOMItems = (clsListItems[])this.Session["clsUOMItems"];
            clsDetailListItems[] DistributorListItems = (clsDetailListItems[])this.Session["DistributorListItems"];
            clsDetailListItems[] CustTypeListItems = (clsDetailListItems[])this.Session["CustTypeListItems"];
            clsDetailListItems[] CustListItems = (clsDetailListItems[])this.Session["CustListItems"];
            clsSKUItems[0] = new clsListItems("ALL", "0");
            for (int i = 0; i < m_dt_Sku.Rows.Count; i++)
            {
                DataRow dRow = m_dt_Sku.Rows[i];
                clsSKUItems[i + 1] = new clsListItems(dRow[2].ToString(), dRow[0].ToString());
            }
            this.Session.Add("clsSKUItems", clsSKUItems);
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("BASKET NO", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("SKU", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("UOM", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("Basket On", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("From", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("To", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("Discount", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("SKU Offer", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("SKU Quantity", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("Is_Multiple", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            dc = new DataColumn("Is_Bundled", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);
            // Scheme Loading
            SchemeCollection_Controller SchCtrl = new SchemeCollection_Controller();
            SchCtrl = (SchemeCollection_Controller)this.Session["SchCtrl"];
            string summary = "Promotion Name : " + SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).Promotion_Code + "\n\nPromotion For : ";
            for (int i = 0; i < SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjPromotionForCol_Cntrl.Count; i++)
            {
                long id = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjPromotionForCol_Cntrl.Get(i).Assigned_Dist_ID;
                DistributorController mDistController = new DistributorController();
                DataTable m_dt_Dist = mDistController.SelectDistributor(int.Parse(id.ToString()));
                if (m_dt_Dist.Rows.Count > 0)
                {
                    summary = summary + m_dt_Dist.Rows[0]["DISTRIBUTOR_NAME"].ToString() + ",";
                }
            }
            BasketCollection_Controller BColCntrl = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjBasketCol_Cntrl;
            txtSummary.Text = summary.Remove(summary.Length - 1, 1);
            if (BColCntrl.Get(0).Is_Basket == true)
            {
                for (int n = 0; n < BColCntrl.Count; n++)
                {
                    Basket_Collection BCol = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjBasketCol_Cntrl.Get(n);
                    int numBasket = n + 1;
                    DataRow dr1 = dt.NewRow();
                    dr1["BASKET NO"] = "Basket # " + numBasket;
                    dr1["SKU"] = "";
                    dr1["UOM"] = "";
                    dr1["Basket On"] = "";
                    dr1["From"] = "";
                    dr1["To"] = "";
                    if (BCol.ObjPromotionOfferCol_Cntrl.Get(0).Discount != Constants.FloatNullValue)
                    {
                        dr1["Discount"] = BCol.ObjPromotionOfferCol_Cntrl.Get(0).Discount.ToString() + " %";
                    }
                    else if (BCol.ObjPromotionOfferCol_Cntrl.Get(0).Offer_Value > 0)
                    {
                        dr1["Discount"] = BCol.ObjPromotionOfferCol_Cntrl.Get(0).Offer_Value.ToString() + " /-";
                    }
                    else
                    {
                        dr1["Discount"] = "";
                    }
                    if (BCol.ObjPromotionOfferCol_Cntrl.Get(0).SKU_ID != Constants.IntNullValue)
                    {
                        SkuController mSKUController = new SkuController();
                        DataTable dtSKUs = mSKUController.SelectSkuData(BCol.ObjPromotionOfferCol_Cntrl.Get(0).SKU_ID, int.Parse(this.Session["CompanyId"].ToString()));
                        if (dtSKUs.Rows.Count > 0)
                        {
                            dr1["SKU Offer"] = dtSKUs.Rows[0]["SKU_Code"].ToString() + " - " + dtSKUs.Rows[0]["SKU_NAME"].ToString();
                        }
                    }
                    else
                    {
                        dr1["SKU Offer"] = "";
                    }
                    if (BCol.ObjPromotionOfferCol_Cntrl.Get(0).Quantity != Constants.IntNullValue)
                    {
                        dr1["SKU Quantity"] = BCol.ObjPromotionOfferCol_Cntrl.Get(0).Quantity.ToString();
                    }
                    else
                    {
                        dr1["SKU Quantity"] = "";
                    }
                    dr1["Is_Multiple"] = BCol.Is_Multiple;
                    dr1["Is_Bundled"] = BCol.Is_Bundled;
                    dt.Rows.Add(dr1);
                    for (int i = 0; i < BCol.ObjBasketDtlCol_Cntrlr.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["BASKET NO"] = "";
                        if (BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKU_ID == Constants.IntNullValue)
                        {
                            SKUGroupController mGroupController = new SKUGroupController();
                            DataTable dtSkus = mGroupController.SelectSKUGroup(BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKUGroup_ID, null, int.Parse(this.Session["CompanyId"].ToString()));
                            if (dtSkus.Rows.Count > 0)
                            {
                                dr["SKU"] = dtSkus.Rows[0]["GROUP_NAME"].ToString();
                            }
                        }
                        else if (BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKUGroup_ID == Constants.IntNullValue)
                        {
                            SkuController mSkUController = new SkuController();
                            DataTable dtSKu = mSkUController.SelectSkuData(BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKU_ID, int.Parse(this.Session["CompanyId"].ToString()));
                            if (dtSKu.Rows.Count > 0)
                            {
                                dr["SKU"] = dtSKu.Rows[0]["SKU_NAME"].ToString();
                            }
                        }
                        SkuController mSkController = new SkuController();
                        DataTable dtUOMs = mSkController.SelectUOM(BCol.ObjBasketDtlCol_Cntrlr.Get(i).UOM_ID, Constants.IntNullValue);
                        if (dtUOMs.Rows.Count > 0)
                        {
                            dr["UOM"] = dtUOMs.Rows[0]["UOM_DESC"].ToString();
                        }
                        if (BCol.Basket_On == Constants.Basket_On_Amount)
                        {
                            dr["Basket On"] = "Amount";
                        }
                        else if (BCol.Basket_On == Constants.Basket_On_Quantity)
                        {
                            dr["Basket On"] = "Quantity";
                        }
                        dr["From"] = BCol.ObjBasketDtlCol_Cntrlr.Get(i).Min_Val.ToString();
                        dr["To"] = BCol.ObjBasketDtlCol_Cntrlr.Get(i).Max_Val.ToString();
                        dr["Discount"] = "";
                        dr["SKU Offer"] = "";
                        dr["SKU Quantity"] = "";
                        dt.Rows.Add(dr);
                    }
                    //Inner Loop
                }
                //Outer Loop
                grdPromotion.DataSource = dt;
                grdPromotion.DataBind();
            }
            else
            {
                for (int n = 0; n < BColCntrl.Count; n++)
                {
                    Basket_Collection BCol = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjBasketCol_Cntrl.Get(n);
                    int numSlab = n + 1;
                    DataRow dr1 = dt.NewRow();
                    dr1["BASKET NO"] = "Slab # " + numSlab;
                    dr1["SKU"] = "";
                    dr1["UOM"] = "";
                    dr1["Basket On"] = "";
                    dr1["From"] = "";
                    dr1["To"] = "";
                    dr1["Discount"] = "";
                    dr1["SKU Offer"] = "";
                    dr1["SKU Quantity"] = "";
                    dr1["Is_Multiple"] = BCol.Is_Multiple;
                    dr1["Is_Bundled"] = BCol.Is_And;
                    dt.Rows.Add(dr1);
                    for (int i = 0; i < BCol.ObjBasketDtlCol_Cntrlr.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["BASKET NO"] = "";
                        if (BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKU_ID == Constants.IntNullValue)
                        {
                            SKUGroupController mGroupController = new SKUGroupController();
                            DataTable dtSkus = mGroupController.SelectSKUGroup(BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKUGroup_ID, null, int.Parse(this.Session["CompanyId"].ToString()));
                            if (dtSkus.Rows.Count > 0)
                            {
                                dr["SKU"] = dtSkus.Rows[0]["GROUP_NAME"].ToString();
                            }
                        }
                        else if (BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKUGroup_ID == Constants.IntNullValue)
                        {
                            SkuController mSKUController = new SkuController();
                            DataTable dtSKu = mSKUController.SelectSkuData(BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKU_ID, int.Parse(this.Session["CompanyId"].ToString()));
                            if (dtSKu.Rows.Count > 0)
                            {
                                dr["SKU"] = dtSKu.Rows[0]["SKU_NAME"].ToString();
                            }
                        }
                        SkuController mSkController = new SkuController();
                        DataTable dtUoms = mSkController.SelectUOM(BCol.ObjBasketDtlCol_Cntrlr.Get(i).UOM_ID, Constants.IntNullValue);
                        if (dtUoms.Rows.Count > 0)
                        {
                            dr["UOM"] = dtUoms.Rows[0]["UOM_DESC"].ToString();
                        }
                        if (BCol.Basket_On == Constants.Basket_On_Amount)
                        {
                            dr["Basket On"] = "Amount";
                        }
                        else if (BCol.Basket_On == Constants.Basket_On_Quantity)
                        {
                            dr["Basket On"] = "Quantity";
                        }
                        dr["From"] = BCol.ObjBasketDtlCol_Cntrlr.Get(i).Min_Val.ToString();
                        dr["To"] = BCol.ObjBasketDtlCol_Cntrlr.Get(i).Max_Val.ToString();
                        if (BCol.ObjPromotionOfferCol_Cntrl.Get(i).Discount > 0)
                        {
                            dr["Discount"] = BCol.ObjPromotionOfferCol_Cntrl.Get(i).Discount.ToString() + " %";
                        }
                        else if (BCol.ObjPromotionOfferCol_Cntrl.Get(i).Offer_Value > 0)
                        {
                            dr["Discount"] = BCol.ObjPromotionOfferCol_Cntrl.Get(i).Offer_Value.ToString() + "/-";
                        }
                        else
                        {
                            dr["Discount"] = "";
                        }
                        if (BCol.ObjPromotionOfferCol_Cntrl.Get(i).SKU_ID != Constants.IntNullValue)
                        {
                            SkuController mSKuController = new SkuController();
                            DataTable dtSku = mSKuController.SelectSkuData(BCol.ObjPromotionOfferCol_Cntrl.Get(i).SKU_ID, int.Parse(this.Session["CompanyId"].ToString()));
                            if (dtSku.Rows.Count > 0)
                            {
                                dr["SKU Offer"] = dtSku.Rows[0]["SKU_Code"].ToString() + " - " + dtSku.Rows[0]["SKU_NAME"].ToString();
                            }
                        }
                        else
                        {
                            dr["SKU Offer"] = "";
                        }
                        if (BCol.ObjPromotionOfferCol_Cntrl.Get(i).Quantity != Constants.IntNullValue)
                        {
                            dr["SKU Quantity"] = BCol.ObjPromotionOfferCol_Cntrl.Get(i).Quantity.ToString();
                        }
                        else
                        {
                            dr["SKU Quantity"] = "";
                        }
                        dt.Rows.Add(dr);
                    }
                    //Inner Loop
                }
                //Outer Loop
                grdPromotion.Columns[0].HeaderText = "Slab No";
                grdPromotion.Columns[5].HeaderText = "Slab On";
                grdPromotion.Columns[2].Visible = false;
                grdPromotion.DataSource = dt;
                grdPromotion.DataBind();
            }
        }
    }
    protected void btnHome_Click(object sender, EventArgs e)
    {
        Session.Remove("Flow");
        if (this.Session["IsEdit"] != null)
        {
            Session.Remove("IsEdit");
            Session.Remove("PromotionId");
        }
        Response.Redirect("frmPromotionWizard.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString());

    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Session.Add("Flow", "b");
        Response.Redirect("frmPromotionWizardSetp3.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString());
    }
    protected void btnFinish_Click(object sender, EventArgs e)
    {
        try
        {
            #region Loading Scheme
            SchemeCollection_Controller SchCtrl = new SchemeCollection_Controller();
            SchCtrl = (SchemeCollection_Controller)this.Session["SchCtrl"];
            int SchemeID = 1;
            #endregion

            #region Insert Promotion
            Promotion_Collection Pro = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0);
            PromotionController mPromotionController = new PromotionController();
            Pro.Promotion_Date = System.DateTime.Now;
            long PromotionID = long.Parse(mPromotionController.InsertPromotion(SchemeID, Pro.Dist_ID, Pro.Promotion_Code, Pro.Promotion_Desc, Pro.Promotion_Date, Pro.Claimable, Pro.Start_Date, Pro.End_Date,Pro.Start_Time,Pro.End_Time, Pro.Is_Active, Pro.Promotion_Type, Pro.Promotion_Selection, Pro.Is_Scheme, Pro.Promotion_For, int.Parse(this.Session["UserId"].ToString())));
            #endregion

            #region Promotion for Distributor 
            PromotionController PCtl = new PromotionController();
            for (int i = 0; i < Pro.ObjPromotionForCol_Cntrl.Count; i++)
            {
                PromotionFor_Collection ProDist = Pro.ObjPromotionForCol_Cntrl.Get(i);
                PCtl.InsertPromotionDist(PromotionID, SchemeID, ProDist.Dist_ID, ProDist.Assigned_Dist_ID);
            }
            #endregion            

            #region Promotion for Service Type

            for (int i = 0; i < Pro.ObjPromotionCustTypeCol_Cntrl.Count; i++)
            {
                PromotionCustomerType_Collection ProServiceType = Pro.ObjPromotionCustTypeCol_Cntrl.Get(i);
                PCtl.InsertPromotionServiceType(PromotionID, SchemeID, ProServiceType.Dist_ID, ProServiceType.Customer_Type_ID);
            }
            #endregion

            #region Promotion for Customer Volume Class

            for (int i = 0; i < Pro.ObjPromotionVolClassCol_Cntrl.Count; i++)
            {
                PromotionCustomerVolClass_Collection ProVolClass = Pro.ObjPromotionVolClassCol_Cntrl.Get(i);
                PCtl.InsertPromotionVolClass(PromotionID, SchemeID, ProVolClass.Dist_ID, ProVolClass.Customer_VolClass_ID);
            }
            #endregion

            #region Promotion for Days

            for (int i = 0; i < Pro.ObjPromotionDayCol_Cntrl.Count; i++)
            {
                PromotionDays_Collection ProDay = Pro.ObjPromotionDayCol_Cntrl.Get(i);
                PCtl.InsertPromotionDay(PromotionID, SchemeID, ProDay.DAY_ID);
            }
            #endregion

            #region Promotion Basket Master & Detail

            for (int i = 0; i < Pro.ObjBasketCol_Cntrl.Count; i++)
            {
                Basket_Collection Bask = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjBasketCol_Cntrl.Get(i);
                BasketController mBasketController = new BasketController();
                long BasketID = long.Parse(mBasketController.InsertBasket(PromotionID, SchemeID, Bask.Dist_ID, Bask.Is_Basket, Bask.Is_Bundled, Bask.Is_Multiple, Bask.Basket_On, Bask.Basket_Selection));
                if (Bask.Is_Basket == true)
                {
                    PromotionOffer_Collection ProOffer = Bask.ObjPromotionOfferCol_Cntrl.Get(0);
                    if ((ProOffer.Discount != 0) && (ProOffer.Discount != Constants.FloatNullValue))
                    {
                        ProOffer.Offer_Value = decimal.Parse(ProOffer.Discount.ToString());
                    }
                    PCtl.InsertPromotionOffer(BasketID, PromotionID, SchemeID, ProOffer.Dist_ID, ProOffer.BasketDetail_ID, ProOffer.Quantity, ProOffer.Offer_Value, ProOffer.Discount,
                        ProOffer.Is_And, ProOffer.SKU_ID, ProOffer.UOM_ID);
                }
                for (int j = 0; j < Bask.ObjBasketDtlCol_Cntrlr.Count; j++)
                {
                    Basket_Detail_Collection BaskDetail = Bask.ObjBasketDtlCol_Cntrlr.Get(j);
                    long BasketDetailID = long.Parse(mBasketController.InsertBasketDetail(BasketID, PromotionID, SchemeID, BaskDetail.Dist_ID, BaskDetail.Min_Val, BaskDetail.Max_Val, BaskDetail.Multiple_Of, BaskDetail.SKU_ID,
                        BaskDetail.SKUBrand_ID, BaskDetail.SKUDiv_ID, BaskDetail.SKUCatg_ID, BaskDetail.SKUProductLine_ID, BaskDetail.SKUGroup_ID, BaskDetail.UOM_ID, BaskDetail.SKUCompany_ID));
                    if (Bask.Is_Basket == false)
                    {
                        PromotionOffer_Collection ProOffer = Bask.ObjPromotionOfferCol_Cntrl.Get(j);
                        PCtl.InsertPromotionOffer(BasketID, PromotionID, SchemeID, ProOffer.Dist_ID, BasketDetailID, ProOffer.Quantity, ProOffer.Offer_Value, ProOffer.Discount,
                            ProOffer.Is_And, ProOffer.SKU_ID, ProOffer.UOM_ID);
                    }
                }
            }
            #endregion

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Job Has Completed Successfully');", true);
            btnFinish.Enabled = false;
            Session.Remove("Flow");
            Session.Remove("SchCtrl");
            txtSummary.Text = "";
            btnFinish.Enabled = false;
            btnBack.Enabled = false;
            if (this.Session["IsEdit"] != null)
            {
                Session.Remove("IsEdit");
                Session.Remove("PromotionId");
            }
        }
        catch (Exception e1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + e1.ToString() + "');", true);
        }
    }
}