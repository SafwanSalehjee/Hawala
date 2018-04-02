<%@ Page Title="Trust Levels of the Network" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Trust.aspx.vb" Inherits="Utilities_Trust" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2><%: Title %>.</h2>
    <asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource1">
        <Series>
            <asp:Series Name="Series1" XValueMember="UserName" YValueMembers="TrustLevel">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT UserTable.UserName, NetworkTable.TrustLevel FROM NetworkTable INNER JOIN UserTable ON NetworkTable.TellerBID = UserTable.Id WHERE (NetworkTable.TellerAID = @UserID) UNION SELECT UserTable_1.UserName, NetworkTable_1.TrustLevel FROM NetworkTable AS NetworkTable_1 INNER JOIN UserTable AS UserTable_1 ON NetworkTable_1.TellerAID = UserTable_1.Id WHERE (NetworkTable_1.TellerBID = @UserID)">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="UserID" />
        </SelectParameters>
    </asp:SqlDataSource>
    <hr />
    

                          
</asp:Content>

