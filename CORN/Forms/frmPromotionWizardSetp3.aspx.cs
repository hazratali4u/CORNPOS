using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmPromotionWizardSetp3 : System.Web.UI.Page
{
    public int SlabNo;
    public static int RowNo;
    public DataTable dt;

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
            txtMultipleOf.Enabled = false;
            this.LoadCategory();
            this.LoadSKUS();
            this.LoadGroups();
            this.LoadSKUS2();
            this.CreateSlabTable();
            btnAddtoSlab.Attributes.Add("onclick", "return ValidateForm()");
            string flow = (string)this.Session["Flow"];
            if (this.Session["IsEdit"] != null)
            {
                bool IsEditing = (bool)this.Session["IsEdit"];

                if (IsEditing == true)
                {
                    if (flow == "f")
                    {
                        this.LoadCloneSlabData();
                        btnNext.Enabled = true;
                    }
                    else
                        if (flow == "b")
                    {
                        this.LoadPromotionCollection();
                    }
                }
                else
                {
                    if (flow == "b")
                    {
                        this.LoadPromotionCollection();
                    }
                    else
                    {
                        this.CreatNewSlab();
                    }
                }
            }
        }
        this.LoadGrid();
    }
    private void LoadCategory()
    {
        SkuHierarchyController mHer_Controller = new SkuHierarchyController();
        DataTable m_dt = mHer_Controller.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()),Constants.IntNullValue);
        clsWebFormUtil.FillDxComboBoxList(this.ddSKUCatagory, m_dt, 0, 3, true);
        clsWebFormUtil.FillDxComboBoxList(this.ddPromotionCatagory, m_dt, 0, 3, true);
        if (m_dt.Rows.Count > 0)
        {
            ddSKUCatagory.SelectedIndex = 0;
            ddPromotionCatagory.SelectedIndex = 0;
        }
    }
    private void LoadSKUS()
    {
        if (ddSKUCatagory.Items.Count > 0)
        {
            SkuController mSKUController = new SkuController();
            if (ddSKUCatagory.SelectedItem.Value != null)
            {
                DataTable dt = new DataTable();
                dt = mSKUController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 21, Constants.IntNullValue, ddSKUCatagory.Value.ToString());
                clsWebFormUtil.FillDxComboBoxList(ddSKU, dt, "SKU_ID", "SKU_NAME", true);   
                if(dt.Rows.Count>0)
                {
                    ddSKU.SelectedIndex = 0;
                }
            }
        }
    }
    private void LoadSKUS2()
    {
        if (ddSKUCatagory.Items.Count > 0)
        {
            SkuController mSKUController = new SkuController();
            if (ddSKUCatagory.SelectedItem.Value != null)
            {
                DataTable dt = new DataTable();
                dt = mSKUController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 21, Constants.IntNullValue, ddPromotionCatagory.Value.ToString());
                clsWebFormUtil.FillDxComboBoxList(ddPromotionSKU, dt, "SKU_ID", "SKU_NAME", true);
                if (dt.Rows.Count > 0)
                {
                    ddPromotionSKU.SelectedIndex = 0;
                }
            }
        }
    }
    private void LoadGroups()
    {
        SKUGroupController grpSku = new SKUGroupController();
        DataTable m_dt = grpSku.SelectSKUGroup(Constants.IntNullValue, null, int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(this.ddSKUSelectedGroup, m_dt, "SKU_GROUP_ID", "GROUP_NAME");
        if(m_dt.Rows.Count>0)
        {
            ddSKUSelectedGroup.SelectedIndex = 0;
        }
    }
    protected void rblSKUType_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlGroup.Visible = false;
        pnlHierarchy.Visible = false;
        if(rblSKUType.SelectedValue == "1")
        {
            pnlHierarchy.Visible = true;
        }
        else
        {
            pnlGroup.Visible = true;
        }
    }

    protected void ddSKUCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadSKUS();
    }

    protected void ddSlabOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblfromquantity.Text = "From " + ddSlabOn.SelectedItem.Text;
        lblToQuantity.Text = "To " + ddSlabOn.SelectedItem.Text;
    }

    protected void chIsMultiple_CheckedChanged(object sender, EventArgs e)
    {
        if (chIsMultiple.Checked == true)
        {
            txtMultipleOf.Enabled = true;
            txtMultipleOf.Focus();
        }
        else
        {
            txtMultipleOf.Enabled = false;
            txtMultipleOf.Text = "";
        }
    }

    protected void btnAddtoSlab_Click(object sender, EventArgs e)
    {
        DataRow dr1;
        dt = (DataTable)this.Session["dt"];
        if (btnAddtoSlab.Text == "Add To Slab")
        {
            #region Add New Slab Detail on Last Index
            SlabNo = Convert.ToInt32(this.Session["SlabNo"]);
            dr1 = dt.NewRow();
            dr1["SLAB_NO"] = SlabNo.ToString();
            if (rblSKUType.SelectedValue == "1")
            {
                dr1["SKU"] = ddSKU.SelectedItem.Text;
                dr1["SKUID"] = ddSKU.SelectedItem.Value.ToString();
                dr1["SKUGROUPID"] = Constants.IntNullValue;
            }
            else if (rblSKUType.SelectedValue == "2")
            {
                dr1["SKU"] = ddSKUSelectedGroup.SelectedItem.Text;
                dr1["SKUGROUPID"] = ddSKUSelectedGroup.SelectedItem.Value.ToString();
                dr1["SKUID"] = Constants.IntNullValue;
            }
            dr1["UOM"] = "Piece";
            dr1["Slab On"] = ddSlabOn.SelectedItem.Text;
            dr1["From"] = txtFrom.Text;
            dr1["To"] = txtTo.Text;
            dr1["UOMID"] = "10";
            if (ddSlabOn.SelectedItem.Text == "Quantity")
            {
                dr1["Basket_on"] = Constants.Basket_On_Quantity;
            }
            else
            {
                dr1["Basket_on"] = Constants.Basket_On_Amount;
            }
            if (chDiscount.Checked == true)
            {
                if (rblDiscountType.SelectedValue == "1")
                {
                    dr1["Discount"] = txtDiscount.Text + " %";
                    dr1["DiscountPercentage"] = txtDiscount.Text;
                    dr1["DiscountAmount"] = Constants.DecimalNullValue;
                }
                else
                {
                    dr1["Discount"] = txtDiscount.Text + "/-";
                    dr1["DiscountAmount"] = txtDiscount.Text;
                    dr1["DiscountPercentage"] = Constants.FloatNullValue;
                }
            }
            if (chSKU.Checked == true)
            {
                dr1["SKUOfferID"] = ddPromotionSKU.SelectedItem.Value;
                dr1["SKU Offer"] = ddPromotionSKU.SelectedItem.Text;
                dr1["SKU Quantity"] = txtPromotionQuantity.Text;

            }
            if (chIsMultiple.Checked == true)
            {
                dr1["Multiple_of"] = int.Parse(txtMultipleOf.Text).ToString();
            }
            else
            {
                dr1["Multiple_of"] = "0";
            }
            dt.Rows.Add(dr1);
            grdSlab.DataSource = dt;
            grdSlab.DataBind();
            this.Session.Add("dt", dt);
            this.OfferEmpty();
            #endregion

            #region Update Priviouse Slab Detail aginst New Record
            bool flag = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == dr1[0].ToString())
                {
                    DataRow dr2 = dt.Rows[i];
                    if (flag != true)
                    {
                        dr2["Is_Multiple"] = chIsMultiple.Checked.ToString();
                        flag = true;
                    }
                    else
                    {
                        dr2["Is_Multiple"] = "";
                        if (rblSKUType.SelectedValue == "1")
                        {
                            dr2["SKU"] = ddSKU.SelectedItem.Text;
                        }
                        else
                        {
                            dr2["SKU"] = ddSKUSelectedGroup.SelectedItem.Text;
                        }
                        dr2["Slab On"] = dr1["Slab On"];
                        dr2["UOM"] = dr1["UOM"].ToString();
                    }
                    if (rblSKUType.SelectedValue == "1")
                    {
                        dr2["SKUID"] = ddSKU.SelectedItem.Value.ToString();
                        dr2["SKUGROUPID"] = Constants.IntNullValue;
                    }
                    else if (rblSKUType.SelectedValue == "2")
                    {
                        dr2["SKUGROUPID"] = ddSKUSelectedGroup.SelectedItem.Value.ToString();
                        dr2["SKUID"] = Constants.IntNullValue;
                    }
                    dr2["Basket_On"] = dr1["Basket_on"];
                    dr2["UOMID"] = dr1["UOMID"];

                }
            }
            dt.DefaultView.Sort = "Slab_NO";
            grdSlab.DataSource = dt.DefaultView;
            grdSlab.DataBind();
            this.Session.Add("dt", dt);
            this.OfferEmpty();
            btnAddtoSlab.Text = "Add To Slab";
            btnCreateNewSlab.Enabled = true;
            btnNext.Enabled = true;
            #endregion
        }
        else if (btnAddtoSlab.Text == "Update Slab")
        {
            #region Update Slab Detail
            string Id = dt.Rows[RowNo][0].ToString();
            dr1 = dt.Rows[RowNo];
            dr1["UOM"] = "Piece";
            dr1["Slab On"] = ddSlabOn.SelectedItem.Text;
            dr1["From"] = txtFrom.Text;
            dr1["To"] = txtTo.Text;
            dr1["UOMID"] = "10";
            if (ddSlabOn.Value.ToString() == "82")
            {
                dr1["Basket_on"] = Constants.Basket_On_Quantity;
            }
            else
            {
                dr1["Basket_on"] = Constants.Basket_On_Amount;
            }
            if (chDiscount.Checked == true)
            {
                if (rblDiscountType.SelectedValue== "1")
                {
                    dr1["Discount"] = txtDiscount.Text + " %";
                    dr1["DiscountPercentage"] = txtDiscount.Text;
                    dr1["DiscountAmount"] = Constants.DecimalNullValue;
                }
                else
                {
                    dr1["DiscountAmount"] = txtDiscount.Text;
                    dr1["Discount"] = txtDiscount.Text + "/-";
                    dr1["DiscountPercentage"] = Constants.FloatNullValue;
                }
            }
            else
            {
                dr1["Discount"] = DBNull.Value;
                dr1["DiscountPercentage"] = DBNull.Value;
                dr1["DiscountAmount"] = DBNull.Value;
            }
            if (chSKU.Checked == true)
            {
                dr1["SKUOfferID"] = ddPromotionSKU.SelectedItem.Value;
                dr1["SKU Offer"] = ddPromotionSKU.SelectedItem.Text;
                dr1["SKU Quantity"] = txtPromotionQuantity.Text;
            }
            else
            {
                dr1["SKUOfferID"] = DBNull.Value;
                dr1["SKU Offer"] = DBNull.Value;
                dr1["SKU Quantity"] = DBNull.Value;
            }
            if (chIsMultiple.Checked == true)
            {
                dr1["Multiple_of"] = int.Parse(txtMultipleOf.Text).ToString();
            }
            else
            {
                dr1["Multiple_of"] = "0";
            }
            grdSlab.DataSource = dt;
            grdSlab.DataBind();
            this.Session.Add("dt", dt);
            this.OfferEmpty();
            btnAddtoSlab.Text = "Add To Slab";
            btnCreateNewSlab.Enabled = true;
            btnNext.Enabled = true;
            #endregion
        }
        else
        {
            #region Add New Existing Slab at Meddle
            dr1 = dt.NewRow();
            dr1["SLAB_NO"] = int.Parse(btnAddtoSlab.Text.Substring(12));
            if (rblSKUType.SelectedValue == "1")
            {
                dr1["SKU"] = ddSKU.SelectedItem.Text;
                dr1["SKUID"] = ddSKU.SelectedItem.Value.ToString();
                dr1["SKUGROUPID"] = Constants.IntNullValue;
            }
            else if (rblSKUType.SelectedValue == "2")
            {
                dr1["SKU"] = ddSKUSelectedGroup.SelectedItem.Text;
                dr1["SKUGROUPID"] = ddSKUSelectedGroup.SelectedItem.Value.ToString();
                dr1["SKUID"] = Constants.IntNullValue;
            }

            dr1["UOM"] = "Piece";
            dr1["Slab On"] = ddSlabOn.Value.ToString();
            dr1["From"] = txtFrom.Text;
            dr1["To"] = txtTo.Text;
            dr1["UOMID"] = "10";
            if (ddSlabOn.Value.ToString() == "82")
            {
                dr1["Basket_on"] = Constants.Basket_On_Quantity;
            }
            else
            {
                dr1["Basket_on"] = Constants.Basket_On_Amount;
            }
            if (chDiscount.Checked == true)
            {
                if (rblDiscountType.SelectedValue == "1")
                {
                    dr1["Discount"] = txtDiscount.Text + " %";
                    dr1["DiscountPercentage"] = txtDiscount.Text;
                    dr1["DiscountAmount"] = Constants.DecimalNullValue;
                }
                else
                {
                    dr1["Discount"] = txtDiscount.Text + "/-";
                    dr1["DiscountAmount"] = txtDiscount.Text;
                    dr1["DiscountPercentage"] = Constants.FloatNullValue;
                }
            }
            if (chSKU.Checked == true)
            {
                dr1["SKUOfferID"] = ddPromotionSKU.SelectedItem.Value;
                dr1["SKU Offer"] = ddPromotionSKU.SelectedItem.Text;
                dr1["SKU Quantity"] = txtPromotionQuantity.Text;
            }
            if (chIsMultiple.Checked == true)
            {
                dr1["Multiple_of"] = int.Parse(txtMultipleOf.Text).ToString();
            }
            else
            {
                dr1["Multiple_of"] = "0";
            }
            dt.Rows.Add(dr1);
            grdSlab.DataSource = dt;
            grdSlab.DataBind();
            this.Session.Add("dt", dt);
            this.OfferEmpty();
            #endregion

            #region Update Priviouse Slab Detail aginst New Record
            bool flag = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == dr1[0].ToString())
                {
                    DataRow dr2 = dt.Rows[i];
                    if (flag != true)
                    {
                        dr2["Is_Multiple"] = chIsMultiple.Checked.ToString();
                        flag = true;
                    }
                    else
                    {
                        dr2["Is_Multiple"] = "";
                        if (rblSKUType.SelectedValue == "1")
                        {
                            dr2["SKU"] = ddSKU.SelectedItem.Text;
                        }
                        else
                        {
                            dr2["SKU"] = ddSKUSelectedGroup.SelectedItem.Text;
                        }
                        dr2["Slab On"] = dr1["Slab On"];
                        dr2["UOM"] = dr1["UOM"].ToString();
                    }
                    if (rblSKUType.SelectedValue == "1")
                    {
                        dr2["SKUID"] = ddSKU.SelectedItem.Value.ToString();
                        dr2["SKUGROUPID"] = Constants.IntNullValue;
                    }
                    else if (rblSKUType.SelectedValue == "2")
                    {
                        dr2["SKUGROUPID"] = ddSKUSelectedGroup.SelectedItem.Value.ToString();
                        dr2["SKUID"] = Constants.IntNullValue;
                    }
                    dr2["Basket_On"] = dr1["Basket_on"];
                    dr2["UOMID"] = dr1["UOMID"];
                }
            }
            dt.DefaultView.Sort = "Slab_NO";
            grdSlab.DataSource = dt.DefaultView;
            grdSlab.DataBind();
            this.Session.Add("dt", dt);
            this.OfferEmpty();
            btnAddtoSlab.Text = "Add To Slab";
            btnCreateNewSlab.Enabled = true;
            btnNext.Enabled = true;
            #endregion
        }
    }

    protected void btnCreateNewSlab_Click(object sender, EventArgs e)
    {
        this.CreatNewSlab();
    }

    protected void ddPromotionCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSKUS2();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Session.Add("Flow", "b");
        Response.Redirect("frmPromotionWizardSetp2.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString());
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        DataControl dc = new DataControl();
        SchemeCollection_Controller SchCtrl = new SchemeCollection_Controller();
        SchCtrl = (SchemeCollection_Controller)this.Session["SchCtrl"];
        Promotion_Collection pc = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0);
        pc.ObjBasketCol_Cntrl = new BasketCollection_Controller();
        Basket_Collection BCol = null;
        dt = (DataTable)this.Session["dt"];
        foreach (DataRowView drv in dt.DefaultView)
        {
            if (!drv[1].ToString().Trim().Equals(""))
            {
                BCol = new Basket_Collection();
                BCol.ObjPromotionOfferCol_Cntrl = new PromotionOfferColl_Controller();
                BCol.ObjBasketDtlCol_Cntrlr = new BasketDetailCollection_Controller();
                BCol.Is_Basket = false;
                BCol.Is_And = false;
                BCol.Dist_ID = Convert.ToInt32(this.Session["DISTRIBUTOR_ID"]);
                BCol.Basket_On = int.Parse(drv["Basket_on"].ToString());
                BCol.Basket_ID = Constants.LongNullValue;
                BCol.Basket_Selection = Constants.IntNullValue;
                if (!drv["Is_Multiple"].ToString().Equals(""))
                {
                    BCol.Is_Multiple = bool.Parse(drv["Is_Multiple"].ToString());
                }
                pc.ObjBasketCol_Cntrl.Add(BCol);
            }
            else
            {
                Basket_Detail_Collection BDCol = new Basket_Detail_Collection();
                BDCol.Dist_ID = Convert.ToInt32(this.Session["DISTRIBUTOR_ID"]);
                BDCol.Min_Val = int.Parse(drv["From"].ToString());
                BDCol.Max_Val = int.Parse(drv["To"].ToString());
                BDCol.Multiple_Of = int.Parse(dc.chkNull(drv["Multiple_of"].ToString()));
                BDCol.SKU_ID = int.Parse(dc.chkNull(drv["SKUID"].ToString()));
                BDCol.SKUGroup_ID = int.Parse(dc.chkNull(drv["SKUGROUPID"].ToString()));
                BDCol.UOM_ID = int.Parse(dc.chkNull(drv["UOMID"].ToString()));
                BDCol.SKUCompany_ID = Constants.IntNullValue;
                BDCol.SKUDiv_ID = Constants.IntNullValue;
                BDCol.SKUCatg_ID = Constants.IntNullValue;
                BDCol.SKUBrand_ID = Constants.IntNullValue;
                BDCol.SKUProductLine_ID = Constants.IntNullValue;
                BCol.ObjBasketDtlCol_Cntrlr.Add(BDCol);
                PromotionOffer_Collection POfferCol = new PromotionOffer_Collection();
                POfferCol.Dist_ID = Convert.ToInt32(this.Session["DISTRIBUTOR_ID"]);
                POfferCol.Is_And = false;
                if (int.Parse(dc.chkNull(drv["SKUOfferID"].ToString())) > 0)
                {
                    POfferCol.SKU_ID = int.Parse(drv["SKUOfferID"].ToString());
                }
                else
                {
                    POfferCol.SKU_ID = Constants.IntNullValue;
                }
                POfferCol.UOM_ID = int.Parse(dc.chkNull(drv["UOMID"].ToString()));
                if (int.Parse(dc.chkNull(drv["SKU Quantity"].ToString())) > 0)
                {
                    POfferCol.Quantity = int.Parse(dc.chkNull(drv["SKU Quantity"].ToString()));
                }
                else
                {
                    POfferCol.Quantity = Constants.IntNullValue;
                }
                if (float.Parse(dc.chkNull_0(drv["DiscountPercentage"].ToString())) > 0)
                {
                    POfferCol.Discount = float.Parse(dc.chkNull(drv["DiscountPercentage"].ToString()));
                    POfferCol.Offer_Value = Constants.DecimalNullValue;
                }
                else
                {
                    POfferCol.Offer_Value = decimal.Parse(dc.chkNull(drv["DiscountAmount"].ToString()));
                    POfferCol.Discount = Constants.FloatNullValue;
                }
                BCol.ObjPromotionOfferCol_Cntrl.Add(POfferCol);
            }
        }
        Response.Redirect("frmPromotionWizardSetp4.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString());
        this.Session.Add("Flow", "f");
    }

    #region Slab
    private void CreateSlabTable()
    {
        dt = new DataTable();
        DataColumn dc = new DataColumn("SLAB_NO", System.Type.GetType("System.Int32"));
        dt.Columns.Add(dc);
        dc = new DataColumn("SLAB NO", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("SKU", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("SKUID", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("SKUGROUPID", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("UOM", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("UOMID", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("Slab On", System.Type.GetType("System.String"));
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
        dc = new DataColumn("Multiple_of", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("Basket_on", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("Edit", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("Remove", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("SKUOfferID", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("DiscountPercentage", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        dc = new DataColumn("DiscountAmount", System.Type.GetType("System.String"));
        dt.Columns.Add(dc);
        this.Session.Add("dt", dt);
        this.Session.Add("SlabNo", SlabNo);
    }
    private void LoadCloneSlabData()
    {
        DataControl dc = new DataControl();
        string PromotionId = (string)this.Session["PromotionId"];
        BasketController BCtl = new BasketController();
        dt = (DataTable)this.Session["dt"];
        SlabNo = Convert.ToInt32(this.Session["SlabNo"]);
        DataTable dtBMaster = BCtl.GetBasketMaster(long.Parse(PromotionId));
        for (int nCount = 0; nCount < dtBMaster.Rows.Count; nCount++)
        {
            SlabNo = Convert.ToInt32(this.Session["SlabNo"]);
            SlabNo++;
            DataRow dr1 = dt.NewRow();
            dr1["SLAB NO"] = "SLab # " + SlabNo;
            dr1["SLAB_NO"] = SlabNo.ToString();
            if (int.Parse(dc.chkNull_0(dtBMaster.Rows[nCount]["Basket_On"].ToString())) == Constants.Basket_On_Quantity)
            {
                dr1["Basket_on"] = Constants.Basket_On_Quantity;

            }
            else
            {
                dr1["Basket_on"] = Constants.Basket_On_Amount;
            }

            if (bool.Parse(dtBMaster.Rows[nCount]["IS_MULTIPLE"].ToString()) == true)
            {
                dr1["Is_Multiple"] = "true";
            }
            else
            {
                dr1["Is_Multiple"] = "false";
            }

            dr1["SKU"] = "";
            dr1["UOM"] = "";
            dr1["Slab On"] = "";
            dr1["From"] = "";
            dr1["To"] = "";
            dr1["Discount"] = "";
            dr1["SKU Offer"] = "";
            dr1["SKU Quantity"] = "";
            dr1["Remove"] = "";
            dr1["Edit"] = "";
            dt.Rows.Add(dr1);
            this.Session.Add("SlabNo", SlabNo);
            DataTable dtBasketDetail = BCtl.GetBasketDetailForSlab(int.Parse(dtBMaster.Rows[nCount]["BASKET_ID"].ToString()));
            this.CloneSlabDetailData(dtBasketDetail);
        }
        grdSlab.DataSource = dt;
        grdSlab.DataBind();
        this.Session.Add("dt", dt);
    }
    private void CloneSlabDetailData(DataTable dt1)
    {
        DataControl dc = new DataControl();
        DataTable dt = (DataTable)this.Session["dt"];
        for (int nCount = 0; nCount < dt1.Rows.Count; nCount++)
        {
            SlabNo = Convert.ToInt32(this.Session["SlabNo"]);
            DataRow dr1 = dt.NewRow();
            dr1["SLAB_NO"] = SlabNo.ToString();
            if (int.Parse(dc.chkNull_0(dt1.Rows[nCount]["SKU_ID"].ToString())) > 0)
            {
                dr1["SKU"] = dt1.Rows[nCount]["sku_name"].ToString();
                dr1["SKUID"] = Convert.ToInt64(dt1.Rows[nCount]["sku_Id"].ToString());
                dr1["SKUGROUPID"] = Constants.IntNullValue;
            }
            else
            {
                dr1["SKU"] = dt1.Rows[nCount]["GROUP_NAME"].ToString(); ;
                dr1["SKUGROUPID"] = dt1.Rows[nCount]["SKU_GROUP_ID"].ToString();
                dr1["SKUID"] = Constants.IntNullValue;
            }
            dr1["UOM"] = dt1.Rows[nCount]["UOM_DESC"].ToString();
            dr1["UOMID"] = dt1.Rows[nCount]["UOM_ID"].ToString();
            if (int.Parse(dt1.Rows[nCount]["BASKET_ON"].ToString()) == Constants.Basket_On_Quantity)
            {
                dr1["Slab On"] = "Quantity";
            }
            else
            {
                dr1["Slab On"] = "Amount";
            }
            dr1["From"] = Convert.ToInt64(dt1.Rows[nCount]["MIN_VAL"].ToString());
            dr1["To"] = Convert.ToInt64(dt1.Rows[nCount]["MAX_VAL"].ToString());
            if (int.Parse(dt1.Rows[nCount]["BASKET_ON"].ToString()) == Constants.Basket_On_Quantity)
            {
                dr1["Basket_on"] = Constants.Basket_On_Quantity;
            }
            else
            {
                dr1["Basket_on"] = Constants.Basket_On_Amount;
            }
            if (decimal.Parse(dc.chkNull_0(dt1.Rows[nCount]["DISCOUNT"].ToString())) > 0)
            {
                dr1["Discount"] = dt1.Rows[nCount]["DISCOUNT"].ToString() + "%";
                dr1["DiscountPercentage"] = decimal.Parse(dt1.Rows[nCount]["DISCOUNT"].ToString());
                dr1["DiscountAmount"] = "";
            }
            else if (decimal.Parse(dc.chkNull_0(dt1.Rows[nCount]["OFFER_VALUE"].ToString())) > 0)
            {
                dr1["Discount"] = Math.Round(decimal.Parse(dt1.Rows[nCount]["OFFER_VALUE"].ToString()), 2) + "/-";
                dr1["DiscountAmount"] = decimal.Parse(dt1.Rows[nCount]["OFFER_VALUE"].ToString());
                dr1["DiscountPercentage"] = "";
            }
            if (decimal.Parse(dc.chkNull_0(dt1.Rows[nCount]["OFFERSKUID"].ToString())) > 0)
            {
                dr1["SKUOfferID"] = dc.chkNull_0(dt1.Rows[nCount]["OFFERSKUID"].ToString());
                dr1["SKU Offer"] = dc.chkNull_0(dt1.Rows[nCount]["OFFERSKUNAME"].ToString());
                dr1["SKU Quantity"] = dc.chkNull_0(dt1.Rows[nCount]["QUANTITY"].ToString());
            }
            dr1["MULTIPLE_OF"] = dt1.Rows[nCount]["MULTIPLE_OF"].ToString();
            dt.Rows.Add(dr1);
        }
        this.Session.Add("dt", dt);
    }
    private void LoadPromotionCollection()
    {
        SchemeCollection_Controller SchCtrl = new SchemeCollection_Controller();
        SchCtrl = (SchemeCollection_Controller)this.Session["SchCtrl"];
        SlabNo = Convert.ToInt32(this.Session["SlabNo"]);
        BasketCollection_Controller BColCntrl = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjBasketCol_Cntrl;
        dt = (DataTable)this.Session["dt"];
        for (int n = 0; n < BColCntrl.Count; n++)
        {
            Basket_Collection BCol = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjBasketCol_Cntrl.Get(n);
            DataRow dr1 = dt.NewRow();
            SlabNo++;
            dr1["SLAB NO"] = "SLab # " + SlabNo;
            dr1["SLAB_NO"] = SlabNo.ToString();
            dr1["Basket_on"] = BCol.Basket_On;
            dr1["Is_Multiple"] = BCol.Is_Multiple;
            dr1["SKU"] = "";
            dr1["UOM"] = "";
            dr1["Slab On"] = "";
            dr1["From"] = "";
            dr1["To"] = "";
            dr1["Discount"] = "";
            dr1["SKU Offer"] = "";
            dr1["SKU Quantity"] = "";
            dr1["Remove"] = "";
            dr1["Edit"] = "";
            dt.Rows.Add(dr1);
            for (int i = 0; i < BCol.ObjBasketDtlCol_Cntrlr.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["SLAB NO"] = "";
                dr["SLAB_NO"] = SlabNo.ToString();
                if (BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKU_ID == Constants.IntNullValue)
                {
                    SKUGroupController mGroupController = new SKUGroupController();
                    DataTable dtSkus = mGroupController.SelectSKUGroup(BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKUGroup_ID, null, int.Parse(this.Session["CompanyId"].ToString()));
                    if (dtSkus.Rows.Count > 0)
                    {
                        dr["SKU"] = dtSkus.Rows[0]["GROUP_NAME"].ToString();
                        dr["SKUGROUPID"] = BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKUGroup_ID;
                        dr["SKUID"] = Constants.IntNullValue;
                    }
                }
                else if (BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKUGroup_ID == Constants.IntNullValue)
                {
                    SkuController mSKUController = new SkuController();
                    DataTable dtSKu = mSKUController.SelectSkuData(BCol.ObjBasketDtlCol_Cntrlr.Get(i).SKU_ID, int.Parse(this.Session["CompanyId"].ToString()));
                    if (dtSKu.Rows.Count > 0)
                    {
                        dr["SKU"] = dtSKu.Rows[0]["SKU_CODE"].ToString() + " - " + dtSKu.Rows[0]["SKU_NAME"].ToString();
                        dr["SKUID"] = dtSKu.Rows[0]["SKU_ID"].ToString();
                        dr["SKUGROUPID"] = Constants.IntNullValue;
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
                    dr["Slab On"] = "Amount";
                }
                else if (BCol.Basket_On == Constants.Basket_On_Quantity)
                {
                    dr["Slab On"] = "Quantity";
                }
                dr["From"] = BCol.ObjBasketDtlCol_Cntrlr.Get(i).Min_Val.ToString();
                dr["To"] = BCol.ObjBasketDtlCol_Cntrlr.Get(i).Max_Val.ToString();
                dr["Multiple_Of"] = BCol.ObjBasketDtlCol_Cntrlr.Get(i).Multiple_Of.ToString();
                if (BCol.ObjPromotionOfferCol_Cntrl.Get(i).Discount > 0)
                {
                    dr["Discount"] = BCol.ObjPromotionOfferCol_Cntrl.Get(i).Discount.ToString() + " %";
                    dr["DiscountPercentage"] = BCol.ObjPromotionOfferCol_Cntrl.Get(i).Discount.ToString();
                    dr["DiscountAmount"] = Constants.DecimalNullValue;
                }
                else if (BCol.ObjPromotionOfferCol_Cntrl.Get(i).Offer_Value > 0)
                {
                    dr["Discount"] = BCol.ObjPromotionOfferCol_Cntrl.Get(i).Offer_Value.ToString() + "/-";
                    dr["DiscountAmount"] = BCol.ObjPromotionOfferCol_Cntrl.Get(i).Offer_Value.ToString();
                    dr["DiscountPercentage"] = Constants.FloatNullValue;
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
                        dr["SKUOfferID"] = dtSku.Rows[0]["SKU_ID"].ToString();
                        dr["SKU Offer"] = dtSku.Rows[0]["SKU_CODE"].ToString() + " - " + dtSku.Rows[0]["SKU_NAME"].ToString();
                    }
                }
                else
                {
                    dr["SKUOfferID"] = Constants.IntNullValue;
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
            this.Session.Add("dt", dt);
            this.Session.Add("SlabNo", SlabNo);
        }
    }
    private void CreatNewSlab()
    {
        chIsMultiple.Enabled = true;
        ddSlabOn.Enabled = true;
        txtMultipleOf.Enabled = false;
        txtMultipleOf.Text = "";
        chIsMultiple.Checked = false;        
        SlabNo = Convert.ToInt32(this.Session["SlabNo"]);
        dt = (DataTable)this.Session["dt"];
        SlabNo++;
        DataRow dr1 = dt.NewRow();
        dr1["SLAB NO"] = "SLab # " + SlabNo;
        dr1["SLAB_NO"] = SlabNo.ToString();
        if (ddSlabOn.Value.ToString() == "82")
        {
            dr1["Basket_on"] = Constants.Basket_On_Quantity;
        }
        else
        {
            dr1["Basket_on"] = Constants.Basket_On_Amount;
        }
        if (chIsMultiple.Checked == true)
        {
            dr1["Is_Multiple"] = "true";
        }
        else
        {
            dr1["Is_Multiple"] = "false";
        }
        dr1["SKU"] = "";
        dr1["UOM"] = "";
        dr1["Slab On"] = "";
        dr1["From"] = "";
        dr1["To"] = "";
        dr1["Discount"] = "";
        dr1["SKU Offer"] = "";
        dr1["SKU Quantity"] = "";
        dr1["Remove"] = "";
        dr1["Edit"] = "";
        dt.Rows.Add(dr1);
        grdSlab.DataSource = dt;
        grdSlab.DataBind();
        this.Session.Add("dt", dt);
        this.Session.Add("SlabNo", SlabNo);
    }
    private void LoadGrid()
    {
        dt = (DataTable)this.Session["dt"];
        grdSlab.DataSource = dt.DefaultView;
        grdSlab.DataBind();
    }
    private void OfferEmpty()
    {
        this.chDiscount.Checked = false;
        this.txtDiscount.Text = "";
        this.chSKU.Checked = false;
        this.ddPromotionCatagory.SelectedIndex = 0;
        this.txtPromotionQuantity.Text = "";
        this.txtMultipleOf.Text = "";
        this.txtFrom.Text = "";
        this.txtTo.Text = "";
        this.txtMultipleOf.Text = "";
    }
    protected void grdSlab_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int RowPosition = e.RowIndex;
            dt = (DataTable)this.Session["dt"];
            DataRowView drv = dt.DefaultView[RowPosition];
            string SlabValue = drv[0].ToString();
            string Column2 = drv[1].ToString();
            int RowCount = dt.Rows.Count - 1;
            if (Column2.Trim() != "")
            {
                for (int i = RowCount; i > 0; i--)
                {
                    if (dt.Rows[i][0].ToString().Trim() == SlabValue)
                    {
                        dt.Rows[i].Delete();
                    }
                }
            }
            else
            {
                drv.Delete();
            }
            dt.AcceptChanges();
            this.Session.Add("dt", dt);
            this.LoadGrid();
        }
        catch (Exception)
        {
        }
    }
    protected void grdSlab_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        DataControl dc = new DataControl();
        dt = (DataTable)this.Session["dt"];
        DataRowView drv = dt.DefaultView[e.NewEditIndex];
        RowNo = e.NewEditIndex;
        if (drv["Slab No"].ToString().Trim().Equals(""))
        {
            if (int.Parse(dc.chkNull_0(drv["SKUID"].ToString())) > 0)
            {
                for (int i = 0; i < ddSKU.Items.Count; i++)
                {
                    if (int.Parse(ddSKU.Items[i].Value.ToString()) == int.Parse(dc.chkNull_0(drv["SKUID"].ToString())))
                    {
                        ddSKU.SelectedIndex = i;
                        break;
                    }
                }                
            }
            else
            {
                for (int i = 0; i < ddSKUSelectedGroup.Items.Count; i++)
                {
                    if (int.Parse(ddSKUSelectedGroup.Items[i].Value.ToString()) == int.Parse(dc.chkNull_0(drv["SKUGROUPID"].ToString())))
                    {
                        ddSKUSelectedGroup.SelectedIndex = i;
                        break;
                    }
                }
            }
            this.txtMultipleOf.Text = drv["Multiple_of"].ToString();
            if (int.Parse(dc.chkNull_0(this.txtMultipleOf.Text)) > 0)
            {
                chIsMultiple.Checked = true;
                txtMultipleOf.Enabled = true;
            }
            else
            {
                chIsMultiple.Checked = false;
                txtMultipleOf.Enabled = false;
            }
            this.txtFrom.Text = drv["From"].ToString();
            this.txtTo.Text = drv["To"].ToString();
            this.ddSlabOn.Value = drv["Basket_on"].ToString();

            if (decimal.Parse(dc.chkNull_0(drv["DiscountAmount"].ToString())) == 0)
            {
                this.txtDiscount.Text = dc.chkNull_0(drv["DiscountPercentage"].ToString());
            }
            else
            {
                this.txtDiscount.Text = dc.chkNull_0(drv["DiscountAmount"].ToString());
            }
            if (!drv["Discount"].ToString().Trim().Equals(""))
            {
                chDiscount.Checked = true;
            }
            if (int.Parse(dc.chkNull_0(drv["SKU Quantity"].ToString())) > 0)
            {
                chSKU.Checked = true;
            }
            for (int i = 0; i < ddPromotionSKU.Items.Count; i++)
            {
                if (int.Parse(ddPromotionSKU.Items[i].Value.ToString()) == int.Parse(dc.chkNull_0(drv["SKUOfferID"].ToString())))
                {
                    ddPromotionSKU.SelectedIndex = i;
                    break;
                }
            }
            this.txtPromotionQuantity.Text = dc.chkNull_0(drv["SKU Quantity"].ToString());
            btnAddtoSlab.Text = "Update Slab";
            btnCreateNewSlab.Enabled = false;
            btnNext.Enabled = false;
        }
        else
        {
            btnAddtoSlab.Text = "Add To Slab#" + drv["Slab_No"].ToString();
            btnCreateNewSlab.Enabled = false;
            btnNext.Enabled = false;
        }
    }
    #endregion    
}