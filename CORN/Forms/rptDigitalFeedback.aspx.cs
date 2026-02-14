using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Services;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

public partial class Forms_rptDigitalFeedback : System.Web.UI.Page
{
    readonly CustomerFeedbackController cCustomerFeedback = new CustomerFeedbackController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        string distributorID = Request["DistributorID"];
        string FromDate = Request["FromDate"];
        string ToDate = Request["ToDate"];

        LoadPercentGrid(distributorID, FromDate, ToDate);

        if (!Page.IsPostBack)
        {
        }
    }
    public void LoadPercentGrid(string distributorID, string FromDate, string ToDate)
    {
        CreateTable();

        DataTable dt = new DataTable();
        dt.Columns.Add("Type", typeof(string));
        dt.Columns.Add("Excellent", typeof(decimal));
        dt.Columns.Add("VeryGood", typeof(decimal));
        dt.Columns.Add("Good", typeof(decimal));
        dt.Columns.Add("Fair", typeof(decimal));
        dt.Columns.Add("Poor", typeof(decimal));

        DataSet ds = cCustomerFeedback.GetCustomerFeedbackReport(
            int.Parse(distributorID),
            DateTime.Parse(FromDate), 
            DateTime.Parse(ToDate));

        DataTable newDt = ds.Tables["rptCustomerFeedback"];
        decimal serviceGood = 0;
        decimal serviceVeryGood = 0;
        decimal serviceExcellent = 0;
        decimal serviceFair = 0;
        decimal servicePoor = 0;

        decimal foodGood = 0;
        decimal foodVeryGood = 0;
        decimal foodExcellent = 0;
        decimal foodFair = 0;
        decimal foodPoor = 0;

        decimal environmentGood = 0;
        decimal environmentVeryGood = 0;
        decimal environmentExcellent = 0;
        decimal environmentFair = 0;
        decimal environmentPoor = 0;

        decimal overallExperGood = 0;
        decimal overallExperVeryGood = 0;
        decimal overallExperExcellent = 0;
        decimal overallExperFair = 0;
        decimal overallExperPoor = 0;

        if (newDt != null && newDt.Rows.Count > 0)
        {
            //Service
            serviceGood = newDt.Select("Type='Serivce' AND Quality = 'Good'").Length;
            serviceVeryGood = newDt.Select("Type='Serivce' AND Quality = 'Very good'").Length;
            serviceExcellent = newDt.Select("Type='Serivce' AND Quality = 'Excellent'").Length;
            serviceFair = newDt.Select("Type='Serivce' AND Quality = 'Fair'").Length;
            servicePoor = newDt.Select("Type='Serivce' AND Quality = 'Poor'").Length;

            DataRow dr = dt.NewRow();
            dr["Type"] = "Service";
            dr["Excellent"] = (serviceExcellent / 100) * 100;
            dr["VeryGood"] = (serviceVeryGood / 100) * 100;
            dr["Good"] = (serviceGood / 100) * 100;
            dr["Fair"] = (serviceFair / 100) * 100;
            dr["Poor"] = (servicePoor / 100) * 100;

            dt.Rows.Add(dr);


            //Food
            foodGood = newDt.Select("Type='Food' AND Quality = 'Good'").Length;
            foodVeryGood = newDt.Select("Type='Food' AND Quality = 'Very good'").Length;
            foodExcellent = newDt.Select("Type='Food' AND Quality = 'Excellent'").Length;
            foodFair = newDt.Select("Type='Food' AND Quality = 'Fair'").Length;
            foodPoor = newDt.Select("Type='Food' AND Quality = 'Poor'").Length;

            dr = dt.NewRow();
            dr["Type"] = "Food";
            dr["Excellent"] = (foodExcellent / 100) * 100;
            dr["VeryGood"] = (foodVeryGood / 100) * 100;
            dr["Good"] = (foodGood / 100) * 100;
            dr["Fair"] = (foodFair / 100) * 100;
            dr["Poor"] = (foodPoor / 100) * 100;

            dt.Rows.Add(dr);


            //Environment
            environmentGood = newDt.Select("Type='Environment' AND Quality = 'Good'").Length;
            environmentVeryGood = newDt.Select("Type='Environment' AND Quality = 'Very good'").Length;
            environmentExcellent = newDt.Select("Type='Environment' AND Quality = 'Excellent'").Length;
            environmentFair = newDt.Select("Type='Environment' AND Quality = 'Fair'").Length;
            environmentPoor = newDt.Select("Type='Environment' AND Quality = 'Poor'").Length;

            dr = dt.NewRow();
            dr["Type"] = "Environment";
            dr["Excellent"] = (environmentExcellent / 100) * 100;
            dr["VeryGood"] = (environmentVeryGood / 100) * 100;
            dr["Good"] = (environmentGood / 100) *100;
            dr["Fair"] = (environmentFair / 100) * 100;
            dr["Poor"] = (environmentPoor / 100) * 100;

            dt.Rows.Add(dr);


            //Overall Experience
            overallExperGood = newDt.Select("Type='Overall Experience' AND Quality = 'Good'").Length;
            overallExperVeryGood = newDt.Select("Type='Overall Experience' AND Quality = 'Very good'").Length;
            overallExperExcellent = newDt.Select("Type='Overall Experience' AND Quality = 'Excellent'").Length;
            overallExperFair = newDt.Select("Type='Overall Experience' AND Quality = 'Fair'").Length;
            overallExperPoor = newDt.Select("Type='Overall Experience' AND Quality = 'Poor'").Length;

            dr = dt.NewRow();
            dr["Type"] = "Overall Experience";
            dr["Excellent"] = (overallExperExcellent / 100) * 100;
            dr["VeryGood"] = (overallExperVeryGood / 100) * 100;
            dr["Good"] = (overallExperGood / 100) * 100;
            dr["Fair"] = (overallExperFair / 100) * 100;
            dr["Poor"] = (overallExperPoor / 100) * 100;

            dt.Rows.Add(dr);

            if (dt != null && dt.Rows.Count > 0)
            {
                Grdfeedback.DataSource = dt;
                Grdfeedback.DataBind();

                Chart chart1 = new Chart();

                // Add a chart area.
                ChartArea chartArea = new ChartArea();
                chart1.ChartAreas.Add(chartArea);

                Legend legend = new Legend();
                legend.Name = "MyLegend";
                legend.Docking = Docking.Top;
                legend.Alignment = StringAlignment.Center;

                // Add the legend to the chart.
                chart1.Legends.Add(legend);

                // Loop through the columns (Value1, Value2, Value3) to create series.
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName != "Type") // Skip the "Type" column.
                    {
                        Series series = new Series();
                        series.Name = column.ColumnName;
                        series.ChartType = SeriesChartType.Column;

                        // Add data points for this series based on the Type.
                        foreach (DataRow row in dt.Rows)
                        {
                            DataPoint dataPoint = new DataPoint();
                            dataPoint.AxisLabel = row["Type"].ToString();
                            dataPoint.YValues = new double[] { Convert.ToDouble(row[column]) };
                            series.Points.Add(dataPoint);
                        }

                        chart1.Series.Add(series);
                    }
                }

                // Bind the chart to a Chart control in your ASP.NET page.
                ChartPlaceHolder.Controls.Add(chart1);
            }
        }
    }
    public void CreateTable()
    {

    }

    [WebMethod]
    public static string GetDistributorInfo()
    {
        DistributorController mDController = new DistributorController();
        DataTable dt = mDController.SelectDistributor(Convert.ToInt32(HttpContext.Current.Session["DISTRIBUTOR_ID"]));
        return GetJson(dt);
    }

    public static string GetJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dt.Rows)
        {
            row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }

}
