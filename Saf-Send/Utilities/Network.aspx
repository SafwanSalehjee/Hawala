<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Network.aspx.vb" Inherits="Utilities_Network" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    
    
    <div class="row">
        <br />
        <div class="col-md-8">
        <legend>Invite Registered User</legend>
        <asp:PlaceHolder runat="server" ID="PHInvite" Visible="False">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="ltlError" />
                        </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="PHInviteSuccess" Visible="False">
                         <div class="alert alert-dismissible alert-success">
                            <button type="button"   class="close" data-dismiss="alert">&times;</button>
                            <asp:Literal runat="server" ID="ltlInviteSucess"  />
                        </div>
                     </asp:PlaceHolder>
        <div class="form-group">
        <asp:Label ID="lblUsername" class="col-form-label col-form-label-lg" for="txtUsername" runat="server" Text="Username"></asp:Label>
        &nbsp;&nbsp;&nbsp;
        <asp:TextBox class="form-control form-control-lg" ID="txtUsername" runat="server"></asp:TextBox>
        </div>
        <br />
        <br />
        <div class="form-group">
        <asp:Label ID="lblPassKey" class="col-form-label col-form-label-lg" for="txtPaskey" runat="server" Text="Passkey"></asp:Label>
&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtPaskey" class="form-control form-control-lg" runat="server"></asp:TextBox>
        </div>
        <br />
        <br />
        <asp:Button ID="btnInvite" Class=" btn btn-primary" runat="server" AutoPostBack="true" Text="Invite" />
        </div>
        <div class="col-md-4">
            
            <div class="panel panel-info">
                <div class="panel-heading">
                
                    <asp:Literal ID="InviteH3"  runat="server" />
                </div>
                
                <div class="panel-body">
                    <asp:PlaceHolder runat="server" ID="PHResponse" Visible="False">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="ltlRerror" />
                        </p>
                        
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="PHResonseSuccess" Visible="False">
                         <div class="alert alert-dismissible alert-success">
                            <button type="button"   class="close" data-dismiss="alert">&times;</button>
                            <asp:Literal runat="server" ID="ltlRsuccess"  /><asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
                        </div>
                     </asp:PlaceHolder>
                   
                    <asp:ListBox ID="ListBox1" runat="server" DataSourceID="SqlDSAllInvite" DataTextField="UserName" DataValueField="UserName" Height="114px" Width="279px"></asp:ListBox>
                    <asp:SqlDataSource ID="SqlDSAllInvite" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT UserTable.UserName FROM NetworkTable INNER JOIN UserTable ON NetworkTable.TellerAID = UserTable.Id WHERE (NetworkTable.TellerBID = @UserID) AND (NetworkTable.IsAccept = 0)">
                        <SelectParameters>
                            <asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="UserID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <br />
                    <br />
                    &nbsp;<asp:Button ID="BtnInvAccept" runat="server" AutoPostBack="true" class="btn btn-success" Text="Accept" />
                    &nbsp;&nbsp;
                    <asp:Button ID="BtnInvReject" runat="server" AutoPostBack="true" class="btn btn-default " Text="Reject" />
                </div>   
            </div>
     </div>
    </div>
</asp:Content>

