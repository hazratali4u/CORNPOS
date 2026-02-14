using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmSKUAssignment : System.Web.UI.Page
{
    UserController mUserController = new UserController();
    SkuController dController = new SkuController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadItem();
            LoadAssingned();
            LoadUnAssingned();            
        }
    }    
    
    
    /// <summary>
    /// Loads LocationTypes To LocationType Combo
    /// </summary>
    private void LoadItem()
    {
        DataTable dt = dController.GetItemList(0,Constants.IntNullValue);
        clsWebFormUtil.FillDxComboBoxList(ddlItem, dt, "SKU_ID", "SKU_NAME");
        if (dt.Rows.Count > 0)
        {
            ddlItem.SelectedIndex = 0;
        }
        Session.Add("dtItem", dt);
    }

    /// <summary>
    /// Loads UnAssigned Locations To UnAssigned ListBox on Location Tab
    /// </summary>
    private void LoadUnAssingned()
    {
        DataTable dt = (DataTable)Session["dtItem"];
        DataTable dt2 = new DataTable();
        dt2.Columns.Add("SKU_ID", typeof(int));
        dt2.Columns.Add("SKU_NAME", typeof(string));
        
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["SKU_ID"].ToString() != ddlItem.SelectedItem.Value.ToString())
            {
                bool add = true;
                for (int i = 0; i < lstAssignDistributor.Items.Count; i++)
                {
                    if (lstAssignDistributor.Items[i].Value.ToString() == dr["SKU_ID"].ToString())
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                {
                    DataRow dr2 = dt2.NewRow();
                    dr2["SKU_ID"] = dr["SKU_ID"];
                    dr2["SKU_NAME"] = dr["SKU_NAME"];
                    dt2.Rows.Add(dr2);
                }
            }
        }
        clsWebFormUtil.FillListBox(lstUnAssignDistributor, dt2, "SKU_ID", "SKU_NAME", true);
    }

    /// <summary>
    /// Loads Assigned Locations To Assigned ListBox on Location Tab
    /// </summary>o
    private void LoadAssingned()
    {
        DataTable dt = dController.GetItemAssignment(3, int.Parse(ddlItem.SelectedItem.Value.ToString()), Constants.IntNullValue);
        clsWebFormUtil.FillListBox(lstAssignDistributor, dt, "SKU_ID", "SKU_NAME", true);
    }
    
    /// <summary>
    /// Inserts Location Assignment To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnAssignLocation_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstUnAssignDistributor.Items.Count; i++)
        {
            if (lstUnAssignDistributor.Items[i].Selected == true)
            {
                dController.InsertUpdateItemAssignment(1, int.Parse(ddlItem.SelectedItem.Value.ToString()), int.Parse(lstUnAssignDistributor.Items[i].Value.ToString()));
            }
        }
        this.LoadAssingned();
        this.LoadUnAssingned();        
    }

    /// <summary>
    /// Deletes Location Assignment To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnUnAssignLocation_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstAssignDistributor.Items.Count; i++)
        {
            if (lstAssignDistributor.Items[i].Selected == true)
            {
                dController.InsertUpdateItemAssignment(2, int.Parse(ddlItem.SelectedItem.Value.ToString()), int.Parse(lstAssignDistributor.SelectedItem.Value.ToString()));
            }
        }
        this.LoadAssingned();
        this.LoadUnAssingned();        
    }
    
    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUnAssingned();
        LoadAssingned();
    }
}