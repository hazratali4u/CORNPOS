using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using System.Data;
using CORNCommon.Classes;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

/// <summary>
/// Form To Change User Password
/// </summary>
public partial class Forms_frmOrderingAppsIntegration : System.Web.UI.Page
{

   private readonly GeoHierarchyController _gCtl = new GeoHierarchyController();

    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
    }

    protected void btnImportMenu_Click(object sender, EventArgs e)
    {

    }
}