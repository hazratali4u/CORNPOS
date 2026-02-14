using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web.UI.WebControls;
using System.IO;

/// <summary>
/// From to Assign, UnAssign Forms To Users
/// </summary>
public partial class frmAttributeMapping : System.Web.UI.Page
{
    protected string allowed_extensions = "gif|jpeg|jpg|png";
    readonly SkuController mController = new SkuController();
    readonly DataControl dc = new DataControl();

    /// <summary>
    /// Page_Load Function Populates All Combos and ListBox On The Page
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
            SetInitialRow();
            LoadCategories();
            btnSaveCategory.Attributes.Add("onclick", "return ValidateForm();");
        }
    }

    /// <summary>
    /// Loads Categories
    /// </summary>
    private void LoadCategories()
    {
        var categoryController = new SkuHierarchyController();
        DataTable dt = categoryController.SelectECommerceCategories(Constants.IntNullValue,1);
        dt.DefaultView.Sort = "SortOrder";
       
        clsWebFormUtil.FillDxComboBoxList(ddlCategory, dt, "CategoryID", "Name", true);

        if (dt.Rows.Count > 0)
        {
            ddlCategory.SelectedIndex = 0;
            GetAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue);
        }
    }
    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(new DataColumn("SKU_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("SKU_Name", typeof(string)));
        dt.Columns.Add(new DataColumn("txtSize", typeof(string)));
        dt.Columns.Add(new DataColumn("txtCrust", typeof(string)));

        dr = dt.NewRow();
        dr["SKU_ID"] = string.Empty;
        dr["SKU_Name"] = string.Empty;
        dr["txtSize"] = string.Empty;
        dr["txtCrust"] = string.Empty;

        dt.Rows.Add(dr);

        Session["CurrentTable"] = dt;
        Gridview1.DataSource = dt;
        Gridview1.DataBind();
    }

    /// <summary>
    /// Loads Fourth Layers Modules Assigned To Role To ListBox
    /// </summary>
    /// <param name="RoleID"></param>
    private void GetAssignedItemsINCategories(int eCommerceCategoryID, int posCategoryID)
    {
        DataTable dt = mController.GetAssignUnAssignItemsINECommCategory(int.Parse(dc.chkNull(ddlCategory.SelectedItem.Value.ToString())), Constants.IntNullValue, 1);
        //clsWebFormUtil.FillDxComboBoxList(ddlSKU, dt, "SKU_ID", "SKU_Name", true);
        DataTable dtCurrentTable = (DataTable)Session["CurrentTable"];
        DataRow drCurrentRow = null;

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCurrentRow = dtCurrentTable.NewRow();
                dtCurrentTable.Rows.Add(drCurrentRow);

                dtCurrentTable.Rows[i]["SKU_ID"] = dt.Rows[i]["SKU_ID"].ToString();
                dtCurrentTable.Rows[i]["SKU_Name"] = dt.Rows[i]["SKU_Name"].ToString();

                dtCurrentTable.Rows[i]["txtSize"] = dt.Rows[i]["Size"].ToString();
                dtCurrentTable.Rows[i]["txtCrust"] = dt.Rows[i]["Crust"].ToString();
            }

            dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1].Delete();
            Gridview1.DataSource = dtCurrentTable;
            Gridview1.DataBind();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)Gridview1.Rows[i].Cells[2].FindControl("txtSize");
                TextBox box2 = (TextBox)Gridview1.Rows[i].Cells[3].FindControl("txtCrust");

                box1.Text = dt.Rows[i]["Size"].ToString();
                box2.Text = dt.Rows[i]["Crust"].ToString();
            }
        }
    }

    /// <summary>
    /// Loads Fourth Layers Modules Assigned To Role To ListBox,Fourth Layers Modules Not Assigned To Role To ListBox,
    /// Loads Second Layer Module To Second Layer Module Combo And Loads Third Layer Module To Third Layer Module Combo
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetInitialRow();
        this.GetAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue);
    }
    protected void btnSaveCategory_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSaveCategory.Text == "Save")
            {
                foreach (GridViewRow dr in Gridview1.Rows)
                {
                    TextBox box1 = (TextBox)dr.Cells[2].FindControl("txtSize");
                    TextBox box2 = (TextBox)dr.Cells[3].FindControl("txtCrust");

                    if (dr.Cells[0].Text != "&nbsp;" && (box1.Text != "" || box2.Text != ""))
                    {
                        mController.UpdateSizeAndCrust(int.Parse(dc.chkNull(ddlCategory.SelectedItem.Value.ToString())),
                        int.Parse(dc.chkNull(dr.Cells[0].Text)), box1.Text, box2.Text);
                    }
                }                
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                SetInitialRow();
                GetAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        //this.LoadLookupGrid("filter");
    }
}