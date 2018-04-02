Imports System.Data.SqlClient
Imports System.Data

Partial Class Utilities_Transaction
    Inherits System.Web.UI.Page


    Private SelectedUser As String
    Private InsertedAmount As Double
    Private Discription As String
    Private ThisSqlDataSource As String = "Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename='C:\Users\Abey\Documents\Visual Studio 2017\WebSites\Saf-Send\App_Data\aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96.mdf';Initial Catalog=aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96;Integrated Security=True"
    Private Connection As SqlConnection

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        SelectedUser = CType(Session.Item("tempNameofTellerB"), String)
        TransactionHeader.Text = "Transact with <strong>" + SelectedUser + "</strong>"
        LtlFrom.Text = "Pending Transaction from " + SelectedUser
        LtlTo.Text = "Pending Transaction to " + SelectedUser

    End Sub


    Protected Sub btnTransact_Click1(sender As Object, e As EventArgs) Handles btnTransact.Click
        Dim Profit As Double
        Profit = (CLng(txtAmount.Text) * 5) / 100

        InsertintoTransactionTable(Session("UserID").ToString, Session("TellerBID").ToString, CLng(txtAmount.text()), txtADescription.InnerText, Profit)

    End Sub

    Private Sub InsertintoTransactionTable(ByVal SenderTeller As String, ByVal RecieverTeller As String, ByVal Amount As Long, ByVal Description As String, ByVal Profit As Long)
        Try
            Connection = New SqlConnection(ThisSqlDataSource)
            Dim Command As New SqlCommand("INSERT INTO TransactionTable(TellerAID,TellerBID,Amount,Description,IsReceived,Time,Profit) VALUES ( @SenderTeller ,@RecieverTeller ,@Amount , @Description , 0 ,@DateTime, @Profit )", Connection)

            Command.Parameters.AddWithValue("@SenderTeller", SenderTeller)
            Command.Parameters.AddWithValue("@RecieverTeller", RecieverTeller)
            Command.Parameters.AddWithValue("@Amount", Amount)
            Command.Parameters.AddWithValue("@Description", Description)
            Command.Parameters.AddWithValue("@DateTime", DateTime.Now)
            Command.Parameters.AddWithValue("@Profit", Profit)
            Connection.Open()
            Command.ExecuteNonQuery()
            PHTransactionSuccess.Visible = True
            ltlTransactSucess.Text = "Transaction Successful. Please refresh this page."
            Connection.Close()
        Catch ex As Exception
            PHTransacterror.Visible = True
            ltlTransactError.Text = "Transaction failed because " & ex.Message

        End Try

    End Sub
    Protected Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Response.Redirect("~/utilities/Transaction")
    End Sub


End Class
