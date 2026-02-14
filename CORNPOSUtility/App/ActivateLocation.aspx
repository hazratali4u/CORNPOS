<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="ActivateLocation.aspx.cs" Inherits="ActivateLocation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cDefault" ContentPlaceHolderID="cphChild" Runat="Server">    

      <script language="JavaScript" type="text/javascript">
        function CheckBoxListSelect() {
            var chkBoxList = document.getElementById('<%= cblLocation.ClientID %>');
            var chkBox = document.getElementById('<%= cbAll.ClientID %>');
            var chkBoxCount;
            var i;
            if (chkBox.checked == true) {
                chkBoxCount = chkBoxList.getElementsByTagName("input");
                for (i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            } else {
                chkBoxCount = chkBoxList.getElementsByTagName("input");
                for (i = 0; i < chkBoxCount.length; i++) {

                    chkBoxCount[i].checked = false;

                }
            }
        }
        function UnCheckSelectAll() {
            var chkBox = document.getElementById('<%= cbAll.ClientID %>');
            var chkBoxList = document.getElementById('<%= cblLocation.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var count = 0;
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked == false) {
                    count += 1;
                }
            }
            if (count > 0) {
                chkBox.checked = false;
            }
            else {
                chkBox.checked = true;
            }
        }
    </script>

    <style type="text/css">
    label { display: inline-block;margin-left:5px;padding-left:5px; }
</style>
    <section id="content">
      <div class="container">
        <div class="row demobtn">
          <div class="span12">
              <div class="row">
                  <div class="span6">
                      <strong><asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label> </strong>
                  </div>
              </div>
            <div class="row" style="margin-bottom: 0px;">
              <div class="span6">
                  <strong>Select Database</strong>
                  <br />
                  <asp:DropDownList ID="ddlDB" runat="server" AutoPostBack="true"
                      OnSelectedIndexChanged="ddlDB_SelectedIndexChanged"></asp:DropDownList>
                  <br />
                  <asp:CheckBox ID="cbAll" runat="server" Text="Select All" Checked="true" onclick="CheckBoxListSelect()"/>
                    <asp:Panel ID="pnlDb" runat="server" Height="250" ScrollBars="Vertical" BorderColor="Black" Width="100%" Style="overflow-y: scroll;">
                        <asp:CheckBoxList ID="cblLocation" runat="server" Width="100%" onclick="UnCheckSelectAll()">
                        </asp:CheckBoxList>
                    </asp:Panel>
              </div>
                <div class="span3">
                     <strong>Select Day Close</strong>
                        <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                        <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="txtDate"
                            TargetControlID="txtDate"></cc1:CalendarExtender>
                </div>
                <div class="span3">
                    <br />
                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Activate Location" class="btn btn-primary"/>
                </div>
            </div>              
          </div>
          <!-- end span12 -->
        </div>
      </div>
    </section>
</asp:Content>