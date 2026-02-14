using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmPromotionWizardSetp1 : System.Web.UI.Page
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
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
            btnNext.Attributes.Add("onclick", "return ValidateForm()");

            string flow = (string)this.Session["Flow"];
            if (this.Session["IsEdit"] != null)
            {
                bool IsEditing = (bool)this.Session["IsEdit"];
                if (IsEditing == true)
                {
                    if (flow == "f")
                    {
                        this.FillClonePromotion();
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
                }
            }
        }
    }
    private void LoadPromotionCollection()
    {
        SchemeCollection_Controller SchCtrl = new SchemeCollection_Controller();
        SchCtrl = (SchemeCollection_Controller)this.Session["SchCtrl"];
        this.txtPromotionName.Text = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).Promotion_Code;
        this.txtPromotionDescription.Text = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).Promotion_Desc;
        this.txtStartDate.Text = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).Start_Date.ToString("dd/MM/yyyy");
        this.txtEndDate.Text = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).End_Date.ToString("dd/MM/yyyy");
        this.txtTimeFrom.Value = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).Start_Time;
        this.txtTimeTo.Value = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).End_Time;

        #region Load Prmotion Day
        for (int nInner = 0; nInner < this.cblDays.Items.Count; nInner++)
        {
            cblDays.Items[nInner].Selected = false;
        }

        for (int nInner = 0; nInner < this.cblDays2.Items.Count; nInner++)
        {
            cblDays2.Items[nInner].Selected = false;
        }
        PromotionDaysColl_Controller mPromotionDay = SchCtrl.Get(0).ObjPromotionCol_Cntrl.Get_PCol(0).ObjPromotionDayCol_Cntrl;
        for (int nOuter = 0; nOuter < mPromotionDay.Count; nOuter++)
        {
            PromotionDays_Collection mPromotionVolClassCollection = mPromotionDay.Get(nOuter);
            int DAY_ID = mPromotionVolClassCollection.DAY_ID;

            for (int nInner = 0; nInner < this.cblDays.Items.Count; nInner++)
            {
                if (int.Parse(cblDays.Items[nInner].Value) == DAY_ID)
                {
                    cblDays.Items[nInner].Selected = true;
                    break;
                }
            }
            for (int nInner = 0; nInner < this.cblDays2.Items.Count; nInner++)
            {
                if (int.Parse(cblDays2.Items[nInner].Value) == DAY_ID)
                {
                    cblDays2.Items[nInner].Selected = true;
                    break;
                }
            }
        }
        #endregion
    }
    private void FillClonePromotion()
    {
        string PromotionId = (string)this.Session["PromotionId"];
        PromotionController mPromotionContrl = new PromotionController();
        DataTable dtPromotion = mPromotionContrl.SelectPromotionWithSchemeInfo(int.Parse(this.Session["DISTRIBUTOR_ID"].ToString()), int.Parse(PromotionId));
        if (dtPromotion.Rows.Count > 0)
        {
            this.txtPromotionName.Text = dtPromotion.Rows[0]["Promotion_code"].ToString();
            this.txtPromotionDescription.Text = dtPromotion.Rows[0]["Promotion_Desc"].ToString();
            this.txtStartDate.Text = DateTime.Parse(dtPromotion.Rows[0]["Start_Date"].ToString()).ToString("dd-MMM-yyyy");
            this.txtEndDate.Text = DateTime.Parse(dtPromotion.Rows[0]["End_Date"].ToString()).ToString("dd-MMM-yyyy");
            this.txtTimeFrom.Value = dtPromotion.Rows[0]["START_TIME"];
            this.txtTimeTo.Value = dtPromotion.Rows[0]["END_TIME"];
        }

        #region Load Prmotion Days        
        for (int nInner = 0; nInner < this.cblDays.Items.Count; nInner++)
        {
            cblDays.Items[nInner].Selected = false;
        }

        for (int nInner = 0; nInner < this.cblDays2.Items.Count; nInner++)
        {
            cblDays2.Items[nInner].Selected = false;
        }
        DataTable dtDays = mPromotionContrl.GetPromotionDays(long.Parse(PromotionId));

        for (int nOuter = 0; nOuter < dtDays.Rows.Count; nOuter++)
        {
            int DAY_ID = int.Parse(dtDays.Rows[nOuter]["DAY_ID"].ToString());

            for (int nInner = 0; nInner < this.cblDays.Items.Count; nInner++)
            {
                if (int.Parse(cblDays.Items[nInner].Value) == DAY_ID)
                {
                    if (cblDays.Items[nInner].Selected == false)
                    {
                        cblDays.Items[nInner].Selected = true;
                        break;
                    }
                }
            }

            for (int nInner = 0; nInner < this.cblDays2.Items.Count; nInner++)
            {
                if (int.Parse(cblDays2.Items[nInner].Value) == DAY_ID)
                {
                    if (cblDays2.Items[nInner].Selected == false)
                    {
                        cblDays2.Items[nInner].Selected = true;
                        break;
                    }
                }
            }
        }

        #endregion
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmPromotionWizard.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString());
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        this.Collection();
    }

    protected void Collection()
    {
        try
        {
            SchemeCollection_Controller SchCtrl = new SchemeCollection_Controller();
            Scheme_Collection SchCollection = new Scheme_Collection();
            SchCollection.Scheme_ID = 1;
            SchCtrl.Add(SchCollection);
            Promotion_Collection PC = new Promotion_Collection();
            PC.Dist_ID = Convert.ToInt32(this.Session["DISTRIBUTOR_ID"]);
            PC.Promotion_For = false;
            PC.Promotion_Code = txtPromotionName.Text;
            PC.Promotion_Desc = txtPromotionDescription.Text;
            PC.Claimable = false;
            PC.Start_Date = Convert.ToDateTime(txtStartDate.Text);
            if (txtEndDate.Text == "")
            {
                PC.End_Date = DateTime.MaxValue;
            }
            else
            {
                PC.End_Date = Convert.ToDateTime(txtEndDate.Text);
            }
            PC.Is_Active = true;
            PC.Start_Time = Convert.ToDateTime(txtTimeFrom.Text);
            PC.End_Time = Convert.ToDateTime(txtTimeTo.Value);
            PC.Promotion_Type = 1;
            PC.Is_Scheme = false;

            PC.ObjPromotionDayCol_Cntrl = new PromotionDaysColl_Controller();

            for (int i = 0; i < this.cblDays.Items.Count; i++)
            {
                if (this.cblDays.Items[i].Selected)
                {
                    PromotionDays_Collection PDayCollection = new PromotionDays_Collection();
                    PDayCollection.DAY_ID = int.Parse(this.cblDays.Items[i].Value);
                    PC.ObjPromotionDayCol_Cntrl.Add(PDayCollection);                    
                }
            }
            for (int i = 0; i < this.cblDays2.Items.Count; i++)
            {
                if (this.cblDays2.Items[i].Selected)
                {
                    PromotionDays_Collection PDayCollection = new PromotionDays_Collection();
                    PDayCollection.DAY_ID = int.Parse(this.cblDays2.Items[i].Value);
                    PC.ObjPromotionDayCol_Cntrl.Add(PDayCollection);
                }
            }

            if (PC.ObjPromotionDayCol_Cntrl.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Must Select at least one Day.');", true);
                return;
            }

            SchCollection.ObjPromotionCol_Cntrl = new PromotionCollections_Controller();
            SchCollection.ObjPromotionCol_Cntrl.Add_PCol(PC);
            this.Session.Add("SchCtrl", SchCtrl);
            this.Session.Add("flow", "f");
        }
        catch (Exception)
        {
        }
        this.Session.Add("PrincipalId", 1);
        Response.Redirect("frmPromotionWizardSetp2.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString());
    }
}