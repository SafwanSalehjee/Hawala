<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Home.aspx.vb" Inherits="Utilities_Home"   %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row">
        <div class="col-md-6">
                <h3>Network Users</h3>
                <hr />
                <div class="panel-body">   
                    <div class="list-group">
                        <asp:GridView ID="GridView1"  class="table table-striped table-hover " runat="server" AutoGenerateColumns="False" DataSourceID="SqlDBNetwork" AllowSorting="True" AllowPaging="True" SelectedIndex="1" >
                            <Columns >
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Surname" />
                                <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                                <asp:BoundField DataField="Credit" HeaderText="Credit" SortExpression="Credit" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDBNetwork" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT UserTable.Name, UserTable.Surname, UserTable.Location, NetworkTable.Credit FROM NetworkTable INNER JOIN UserTable ON NetworkTable.TellerAID = UserTable.Id WHERE (NetworkTable.TellerBID = @UserID) AND (NetworkTable.IsAccept = 1) UNION SELECT UserTable_1.Name, UserTable_1.Surname, UserTable_1.Location, NetworkTable_1.Credit * - 1 AS Expr1 FROM NetworkTable AS NetworkTable_1 INNER JOIN UserTable AS UserTable_1 ON NetworkTable_1.TellerBID = UserTable_1.Id WHERE (NetworkTable_1.TellerAID = @UserID) AND (NetworkTable_1.IsAccept = 1)">
                        <SelectParameters>
                            <asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="UserID" />
                        </SelectParameters>
                        </asp:SqlDataSource>
            
                </div>
             </div>
            
        </div>
        <div class="col-md-6">
           
                <h3>Pending Transactions</h3>
                <hr />
                <div class="panel-body">
                    <asp:PlaceHolder runat="server" ID="PHTransactionError" Visible="False">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="ltlTerror" />
                        </p>
                        
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="PHTransactionSuccess" Visible="False">
                         <div class="alert alert-dismissible alert-success">
                            <button type="button"   class="close" data-dismiss="alert">&times;</button>
                            <asp:Literal runat="server" ID="ltlTsuccess"  /><asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
                        </div>
                     </asp:PlaceHolder>
                    <asp:GridView ID="GridView2" class="table table-striped table-hover " runat="server" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlPendingTransaction" AllowPaging="True">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                            <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Surname" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                            <asp:BoundField DataField="Profit" HeaderText="Profit" SortExpression="Profit" />
                            <asp:BoundField DataField="Time" HeaderText="Time" SortExpression="Time" />
                           
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlPendingTransaction" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT UserTable.Name, UserTable.Surname, TransactionTable.Amount, TransactionTable.Description, TransactionTable.Profit, TransactionTable.Time FROM TransactionTable INNER JOIN UserTable ON TransactionTable.TellerAID = UserTable.Id WHERE (TransactionTable.IsReceived = 0) AND (TransactionTable.TellerBID = @UserID)


">
                        <SelectParameters>
                            <asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="UserID" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                    &nbsp; <asp:Button ID="btnTransactAccept" runat="server" class="btn btn-success" Text="Completed" />
                            <asp:Button ID="BtnTranFailed" runat="server" class="btn btn-danger" Text="Reject it" />
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                    <br />
                    <h6>
                        <asp:Literal ID="TransactionSelected" runat="server" />
                    </h6>
                </div>
         </div>

        
        

    </div>
    
</asp:Content>

