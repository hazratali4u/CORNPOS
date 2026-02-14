using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.IO;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Drawing;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using System.Net;
using System.Text;
using System.Xml;
using System.Linq;

namespace Forms
{
    public partial class frmCallCenterDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            Response.AppendHeader("pragma", "no-cache");


            if (!Page.IsPostBack)
            {
            }
        }

    }
}