<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Transaction.aspx.vb" Inherits="Utilities_Transaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class ="row">
        <h4><asp:Literal id="TransactionHeader" runat=server /></h4>
    </div>
    <hr />
    <div class="row">
        <div class="col-md-6">
            <h4>Previous succussful Transactions</h4>
            <hr />
            <asp:GridView ID="GVTransaction" class="table table-striped table-hover " runat="server" DataSourceID="SqlDSTransaction" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="Time" HeaderText="Time" SortExpression="Time" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="Profit" HeaderText="Profit" SortExpression="Profit" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDSTransaction" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT Time, Amount, Description, Profit FROM TransactionTable WHERE (TellerAID = @UserID OR TellerAID = @TellerBID) AND (TellerBID = @UserID OR TellerBID = @TellerBID) AND (IsReceived &lt;&gt; 0) ORDER BY Time">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="8" Name="UserID" SessionField="UserID" />
                    <asp:SessionParameter DefaultValue="9" Name="TellerBID" SessionField="TellerBID" />
                </SelectParameters>
            </asp:SqlDataSource>

        </div>
        <div class="col-md-6">
            
     
                  <asp:PlaceHolder runat="server" ID="PHTransacterror" Visible="False">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="ltlTransactError" />
                        </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="PHTransactionSuccess" Visible="False">
                         <div class="alert alert-dismissible alert-success">
                            <button type="button"   class="close" data-dismiss="alert">&times;</button>
                            <asp:Literal runat="server" ID="ltlTransactSucess"  />
                             <asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
                        </div>
                     </asp:PlaceHolder>
    <fieldset>
    <legend>New Transaction</legend>
      <asp:ValidationSummary runat="server" CssClass="text-danger" />

    <div class="form-group">
      <label for="txtAmount" class="col-lg-2 control-label">Amount</label>
      <div class="col-lg-10">
        <asp:TextBox ID="txtAmount" class="form-control form-control-lg" runat="server"></asp:TextBox>
          
      </div>
    </div>
      <br />
      <br />
    <div class="form-group">
      <label for="txtADescription" class="col-lg-2 control-label">Description</label>
        <div class="col-lg-10">
            
            <textarea class="form-control" rows="3" id="txtADescription" runat ="server" ></textarea>
            
            <span class="help-block">Please Insert any reference of the Reciever for the understanding of your co teller.</span>
        </div>
    </div>
   
    
    <div class="form-group">
      <div class="col-lg-10 col-lg-offset-2">
        <button type="reset" class="btn btn-default">Cancel</button>
          
          <asp:Button ID="btnTransact" class="btn btn-primary" runat="server" Text="Transact" />
          
            <asp:SqlDataSource ID="SqlInsertTransaction" runat="server"></asp:SqlDataSource>

      </div>
    </div>
  </fieldset>

 
                
           
        </div>
        
    </div>
    <hr />
    <div class="row">
        <div class="col-md-6">

            <h4><asp:Literal id="LtlFrom" runat=server /></h4>
            <hr />
            <asp:GridView ID="GVfromRequesr"  class="table table-striped table-hover " runat="server" AutoGenerateColumns="False" DataSourceID="SQLdsRequestFrom" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="Time" HeaderText="Time" SortExpression="Time" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="Profit" HeaderText="Profit" SortExpression="Profit" />
                </Columns>
            </asp:GridView>

            <asp:SqlDataSource ID="SQLdsRequestFrom" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT Time, Amount, Description, Profit FROM TransactionTable WHERE (TellerAID = @TellerBID) AND (TellerBID = @UserID ) AND (IsReceived = 0) ORDER BY Time">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="0" Name="TellerBID" SessionField="TellerBID" />
                    <asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="UserID" />
                </SelectParameters>
            </asp:SqlDataSource>

        </div>
        <div class="col-md-6">
            <h4><asp:Literal id="LtlTo" runat=server /></h4>
            <hr />

            <asp:GridView ID="GVToRequest"  class="table table-striped table-hover " runat="server" AutoGenerateColumns="False" DataSourceID="SQLdsRequestTo" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="Time" HeaderText="Time" SortExpression="Time" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="Profit" HeaderText="Profit" SortExpression="Profit" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SQLdsRequestTo" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT Time, Amount, Description, Profit FROM TransactionTable WHERE (TellerAID = @UserID) AND (TellerBID = @TellerBID) AND (IsReceived = 0) ORDER BY Time">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="1" Name="UserID" SessionField="UserID" />
                    <asp:SessionParameter DefaultValue="0" Name="TellerBID" SessionField="TellerBID" />
                </SelectParameters>
            </asp:SqlDataSource>

        </div>
    </div>
</asp:Content>

