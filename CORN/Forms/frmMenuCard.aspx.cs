using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_frmMenuCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadOrderBooker();
    }

    private void LoadOrderBooker()
    {
        SKURequest request = new SKURequest();
        request.CategoryTypeId = 1;
        request.CompanyId = int.Parse(Session["CompanyId"].ToString());

        SKUResponse response = new SkuController().GetMenuCardDetails(request);

        clsWebFormUtil.FillDxComboBoxList(ddlCategory, response.CategoryList.ToDataTable(), "CategoryId", "CategoryName", false);

        if (response.CategoryList.ToDataTable().Rows.Count > 0)
        {
            ddlCategory.SelectedIndex = 0;
            hfCategoryID.Value = ddlCategory.SelectedItem.Value.ToString();
        }

    }
}