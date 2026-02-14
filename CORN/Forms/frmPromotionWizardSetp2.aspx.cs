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
            this.GetDistributorType();
            this.Populate_CustomerGroup();
            string flow = (string)this.Session["Flow"];
            bool IsEditing = (bool)this.Session["IsEdit"];
            if (IsEditing == true)
            {
                if (flow == "f")
                {
                    this.FillPromotionInfo();
                }
                else if (flow == "b")
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
            }
        }
    }
    private void GetDistributorType()
    {
        DistributorController mController = new DistributorController();
        DataTable dt = mController.SelectDistributorTypeInfo(Constants.IntNullValue);
        clsWebFormUtil.FillListBox(this.ChbDistributorType, dt, 0, 2);
    }
    private void GetDistributor()
    {
        chklDistributors.Items.Clear();
        DistributorController mController = new DistributorController();
        foreach (ListItem item in this.ChbDistributorType.Items)
        {
            if (item.Selected && int.Parse(item.Value.ToString()) > 0)
            {
                DataTable dt = mController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(item.Value.ToString()), int.Parse(this.Session["CompanyId"].ToString()));
                clsWebFormUtil.FillListBox(this.chklDistributors, dt, 0, 2);
            }
        }
    }
    private void Populate_CustomerGroup()
    {
        CustomerDataController cController = new CustomerDataController();
        DataTable dtGroup = cController.GetCustomerGroup(Constants.IntNullValue);
        clsWebFormUtil.FillListBox(this.ChbVolumClass, dtGroup, 0, 1);
    }
    private void FillPromotionInfo()
    {
        #region datatable for Distributor Info
        DataTable dtDistHirerchy = new DataTable();
        dtDistHirerchy.Columns.Add("distributor_id", System.Type.GetType("System.String"));
        dtDistHirerchy.Columns.Add("subzone_id", System.Type.GetType("System.String"));
        #endregion

        #region Fill Datatable of Distributor Info
        string PromotionId = (string)this.Session["PromotionId"];
        PromotionController mPromotionCtrl = new PromotionController();
        DistributorController mDistCtl = new DistributorController();
        DataTable dtDist = mPromotionCtrl.GetPromotionDistributors(long.Parse(PromotionId));
        for (int nCount = 0; nCount < dtDist.Rows.Count; nCount++)
        {
            int AssignDistId = int.Parse(dtDist.Rows[nCount]["ASSIGNED_DISTRIBUTOR_ID"].ToString());
            DataTable dtDistHierarchy = mDistCtl.GetDistributorHierarchy(AssignDistId);
            for (int nRow = 0; nRow < dtDistHierarchy.Rows.Count; nRow++)
            {
                DataRow dr = dtDistHirerchy.NewRow();
                dr["subzone_id"] = dtDistHierarchy.Rows[nRow]["subzone_id"].ToString();
                dr["distributor_id"] = dtDistHierarchy.Rows[nRow]["distributor_id"].ToString();
                dtDistHirerchy.Rows.Add(dr);
            }
        }
        #endregion        

        #region Load Customer Volume Class

        DataTable dtVolume = mPromotionCtrl.GetPromotionCustomerVolumeClass(long.Parse(PromotionId));

        for (int nOuter = 0; nOuter < dtVolume.Rows.Count; nOuter++)
        {
            int Customer_Type_Id = int.Parse(dtVolume.Rows[nOuter]["CUSTOMER_VOLUMECLASS_ID"].ToString());

            for (int nInner = 0; nInner < this.ChbVolumClass.Items.Count; nInner++)
            {
                if (int.Parse(ChbVolumClass.Items[nInner].Value) == Customer_Type_Id)
                {
                    if (ChbVolumClass.Items[nInner].Selected == false)
                    {
                        ChbVolumClass.Items[nInner].Selected = true;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Load Service Type

        for (int nInner = 0; nInner < this.chbServiceType.Items.Count; nInner++)
        {
            chbServiceType.Items[nInner].Selected = false;
        }

        DataTable dtServiceType = mPromotionCtrl.GetPromotionServiceType(long.Parse(PromotionId));

        for (int nOuter = 0; nOuter < dtServiceType.Rows.Count; nOuter++)
        {
            int Customer_Type_Id = int.Parse(dtServiceType.Rows[nOuter]["CUSTOMER_TYPE_ID"].ToString());

            for (int nInner = 0; nInner < this.chbServiceType.Items.Count; nInner++)
            {
                if (int.Parse(chbServiceType.Items[nInner].Value) == Customer_Type_Id)
                {
                    chbServiceType.Items[nInner].Selected = true;
                    break;
                }
            }
        }

        #endregion

        #region Select Distributor Type

        for (int Outer = 0; Outer < dtDistHirerchy.Rows.Count; Outer++)
        {
            int distributor_type = Convert.ToInt32(dtDistHirerchy.Rows[Outer]["subzone_id"].ToString());

            for (int Inner = 0; Inner < this.ChbDistributorType.Items.Count; Inner++)
            {
                if (int.Parse(ChbDistributorType.Items[Inner].Value) == distributor_type)
                {
                    if (ChbDistributorType.Items[Inner].Selected == false)
                    {
                        ChbDistributorType.Items[Inner].Selected = true;
                        ChbDistributorType_SelectedIndexChanged(null, null);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Select Distributors

        for (int Outer = 0; Outer < dtDistHirerchy.Rows.Count; Outer++)
        {
            int distributor_id = Convert.ToInt32(dtDistHirerchy.Rows[Outer]["distributor_id"].ToString());

            for (int Inner = 0; Inner < this.chklDistributors.Items.Count; Inner++)
            {
                if (int.Parse(chklDistributors.Items[Inner].Value) == distributor_id)
                {
                    if (chklDistributors.Items[Inner].Selected == false)
                    {
                        chklDistributors.Items[Inner].Selected = true;
                        break;
                    }
                }
            }
        }
        #endregion        
    }
    private void LoadPromotionCollection()
    {
        #region	Get Scheme Controller
        SchemeCollection_Controller SchCtrl = new SchemeCollection_Controller();
        SchCtrl = (SchemeCollection_Controller)this.Session["SchCtrl"];
        #endregion

        #region datatable for Distributor Info

        DataTable dtDistHirerchy = new DataTable();
        dtDistHirerchy.Columns.Add("distributor_id", System.Type.GetType("System.String"));
        dtDistHirerchy.Columns.Add("subzone_id", System.Type.GetType("System.String"));

        #endregion

        #region Fill Datatable of Distributor Info

        DistributorController mDistCtl = new DistributorController();
        PromotionForCollection_Controller mPromotionForColl = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjPromotionForCol_Cntrl;

        for (int nCount = 0; nCount < mPromotionForColl.Count; nCount++)
        {
            PromotionFor_Collection mPromotionforColl = mPromotionForColl.Get(nCount);
            int Distributor_Id = mPromotionforColl.Assigned_Dist_ID;
            DataTable dt = mDistCtl.GetDistributorHierarchy(Distributor_Id);
            if (dt.Rows.Count > 0)
            {
                for (int nItem = 0; nItem < dt.Rows.Count; nItem++)
                {
                    DataRow dr = dtDistHirerchy.NewRow();
                    dr["subzone_id"] = dt.Rows[nItem]["subzone_id"].ToString();
                    dr["distributor_id"] = dt.Rows[nItem]["distributor_id"].ToString();
                    dtDistHirerchy.Rows.Add(dr);
                }
            }
        }

        #endregion        

        #region Load Customer Volum Class

        PromotionCustVolclassColl_Controller mPromotionVolClass = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjPromotionVolClassCol_Cntrl;
        for (int nOuter = 0; nOuter < mPromotionVolClass.Count; nOuter++)
        {
            PromotionCustomerVolClass_Collection mPromotionVolClassCollection = mPromotionVolClass.Get(nOuter);
            int CustomerVolClass = mPromotionVolClassCollection.Customer_VolClass_ID;

            for (int nInner = 0; nInner < this.ChbVolumClass.Items.Count; nInner++)
            {
                if (int.Parse(ChbVolumClass.Items[nInner].Value) == CustomerVolClass)
                {
                    ChbVolumClass.Items[nInner].Selected = true;
                    break;
                }
            }
        }
        #endregion

        #region Load Service Type

        PromotionCustTypeColl_Controller mPromotionServiceType = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjPromotionCustTypeCol_Cntrl;
        for (int nOuter = 0; nOuter < mPromotionServiceType.Count; nOuter++)
        {
            PromotionCustomerType_Collection mPromotionServiceTypeCollection = mPromotionServiceType.Get(nOuter);
            int ServiceType = mPromotionServiceTypeCollection.Customer_Type_ID;

            for (int nInner = 0; nInner < this.chbServiceType.Items.Count; nInner++)
            {
                if (int.Parse(chbServiceType.Items[nInner].Value) == ServiceType)
                {
                    chbServiceType.Items[nInner].Selected = true;
                    break;
                }
            }
        }
        #endregion

        #region Select Distributor Type

        for (int Outer = 0; Outer < dtDistHirerchy.Rows.Count; Outer++)
        {
            int distributor_type = Convert.ToInt32(dtDistHirerchy.Rows[Outer]["subzone_id"].ToString());

            for (int Inner = 0; Inner < this.ChbDistributorType.Items.Count; Inner++)
            {
                if (int.Parse(ChbDistributorType.Items[Inner].Value) == distributor_type)
                {
                    if (ChbDistributorType.Items[Inner].Selected == false)
                    {
                        ChbDistributorType.Items[Inner].Selected = true;
                        ChbDistributorType_SelectedIndexChanged(null, null);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Select Distributors

        for (int Outer = 0; Outer < dtDistHirerchy.Rows.Count; Outer++)
        {
            int distributor_id = Convert.ToInt32(dtDistHirerchy.Rows[Outer]["distributor_id"].ToString());

            for (int Inner = 0; Inner < this.chklDistributors.Items.Count; Inner++)
            {
                if (int.Parse(chklDistributors.Items[Inner].Value) == distributor_id)
                {
                    if (chklDistributors.Items[Inner].Selected == false)
                    {
                        chklDistributors.Items[Inner].Selected = true;
                        break;
                    }
                }
            }
        }
        #endregion
    }
    protected void ChbAllLocationType_CheckedChanged(object sender, EventArgs e)
    {
        bool AllChecked = (ChbAllLocationType.Checked) ? true : false;

        foreach (ListItem item in ChbDistributorType.Items)
        {
            item.Selected = AllChecked;
            this.GetDistributor();
        }
    }

    protected void ChbDistributorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetDistributor();
    }

    protected void chkSelectAllDistributors_CheckedChanged(object sender, EventArgs e)
    {
        bool AllChecked = (chkSelectAllDistributors.Checked) ? true : false;

        foreach (ListItem item in chklDistributors.Items)
        {
            item.Selected = AllChecked;
        }
    }

    protected void ChbAllCustomerGroup_CheckedChanged(object sender, EventArgs e)
    {
        bool AllChecked = (ChbAllCustomerGroup.Checked) ? true : false;

        foreach (ListItem item in ChbVolumClass.Items)
        {
            item.Selected = AllChecked;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmPromotionWizard.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString());
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            SchemeCollection_Controller SchCtrl = new SchemeCollection_Controller();
            SchCtrl = (SchemeCollection_Controller)this.Session["SchCtrl"];
            Promotion_Collection pc = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0);
            pc.ObjPromotionForCol_Cntrl = new PromotionForCollection_Controller();
            for (int i = 0; i < this.chklDistributors.Items.Count; i++)
            {
                if (this.chklDistributors.Items[i].Selected)
                {
                    PromotionFor_Collection PForCollection = new PromotionFor_Collection();
                    PForCollection.Dist_ID = Convert.ToInt32(this.Session["DISTRIBUTOR_ID"]);
                    PForCollection.Assigned_Dist_ID = int.Parse(this.chklDistributors.Items[i].Value);
                    pc.ObjPromotionForCol_Cntrl.Add(PForCollection);
                }
            }
            if (pc.ObjPromotionForCol_Cntrl.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Must Select at least one Distributors.');", true);
                return;
            }
            pc.ObjPromotionCustTypeCol_Cntrl = new PromotionCustTypeColl_Controller();

            for (int i = 0; i < this.chbServiceType.Items.Count; i++)
            {
                if (this.chbServiceType.Items[i].Selected)
                {
                    PromotionCustomerType_Collection PCustTypeColllection = new PromotionCustomerType_Collection();
                    PCustTypeColllection.Dist_ID = Convert.ToInt32(this.Session["DISTRIBUTOR_ID"]);
                    PCustTypeColllection.Customer_Type_ID = int.Parse(this.chbServiceType.Items[i].Value);
                    pc.ObjPromotionCustTypeCol_Cntrl.Add(PCustTypeColllection);
                }
            }
            if (pc.ObjPromotionCustTypeCol_Cntrl.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Must select at least one Service Type.');", true);
                return;
            }

            pc.ObjPromotionVolClassCol_Cntrl = new PromotionCustVolclassColl_Controller();
            for (int i = 0; i < this.ChbVolumClass.Items.Count; i++)
            {
                if (this.ChbVolumClass.Items[i].Selected)
                {
                    PromotionCustomerVolClass_Collection PVolClassColllection = new PromotionCustomerVolClass_Collection();
                    PVolClassColllection.Dist_ID = Convert.ToInt32(this.Session["DISTRIBUTOR_ID"]);
                    PVolClassColllection.Customer_VolClass_ID = int.Parse(this.ChbVolumClass.Items[i].Value);
                    pc.ObjPromotionVolClassCol_Cntrl.Add(PVolClassColllection);

                }
            }
            if (pc.ObjPromotionVolClassCol_Cntrl.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Must select at least one Customer Group.');", true);
                return;
            }
            pc.ObjPromotionForCustCol_Cntrl = new PromotionForCustColl_Controller();
            this.Session.Add("SchCtrl", SchCtrl);
            this.Session.Add("Flow", "f");
            Response.Redirect("frmPromotionWizardSetp3.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString(), true);
        }
        catch (Exception e1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + e1.ToString() + "');", true);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Session.Add("Flow", "b");
        Response.Redirect("frmPromotionWizardSetp1.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString());
    }

    protected void chbAllServiceType_CheckedChanged(object sender, EventArgs e)
    {
        bool AllChecked = (chbAllServiceType.Checked) ? true : false;

        foreach (ListItem item in chbServiceType.Items)
        {
            item.Selected = AllChecked;
        }
    }
}