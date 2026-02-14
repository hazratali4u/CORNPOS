<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="rptDigitalFeedback.aspx.cs" Inherits="Forms_rptDigitalFeedback" Title="CORN :: Customer Feedback" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
     Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="cphPage">

    <script type="text/javascript" src="../AjaxLibrary/jquery-1.7.1.min.js"></script>

    <script language="JavaScript" type="text/javascript">

        $(document).load(function () {
            $.ajax({
                type: "POST", //HTTP method
                url: "frmCustomerFeedback.aspx/GetDistributorInfo", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: SetDistributorPic
            });

            function SetDistributorPic(data) {
                data = JSON.parse(data.d);

                var url = window.location.origin + '/Pics/';
                var picPath = url + data[0].PIC;
                $("#pic").attr('src', picPath);
            }

        });

    </script>
    
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel3" runat="server" BackColor="White">
                        <div class="row">
                            <div class="col-md-7" style="text-align: center;">
                                <label for="ctl00_ctl00_mainCopy_cphPage_fuPic" id="lbl"
                                    runat="server" style="cursor: pointer;">
                                    <img src="../images/watch.png" id="pic" name="pic" width="100" height="100" />
                                </label>
                            </div>
                        </div>
                        
                        <div class="row" style="margin-top: 15px">
                            <div class="col-md-7">
                                <div class="col-md-12" style="margin-top: 10px">
                                    <asp:GridView ID="Grdfeedback" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                            AllowPaging="true" AutoGenerateColumns="False" 
                            EmptyDataText="No Record exist"
                            PageSize="8">
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Question" ReadOnly="true">
                                    <ItemStyle Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Excellent" HeaderText="Excellent" ReadOnly="true" DataFormatString="{0}">
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="VeryGood" HeaderText="Very Good" ReadOnly="true" DataFormatString="{0}">
                                    <ItemStyle Width="10%" /> 
                                </asp:BoundField>
                                <asp:BoundField DataField="Good" HeaderText="Good" ReadOnly="true" DataFormatString="{0}">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fair" HeaderText="Fair" ReadOnly="true" DataFormatString="{0}">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="Poor" HeaderText="Poor" ReadOnly="true" DataFormatString="{0}">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="cf head"></HeaderStyle>
                            <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                            <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                        </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <div class="row" style="margin-top: 10px">
                            <div class="col-md-12">
                                <asp:PlaceHolder ID="ChartPlaceHolder" runat="server"></asp:PlaceHolder>

                                <asp:Chart ID="Chart1" runat="server" Height="400px">
                                </asp:Chart>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
