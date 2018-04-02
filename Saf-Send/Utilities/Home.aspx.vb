Imports System.Data
Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Drawing


Partial Class Utilities_Home
    Inherits System.Web.UI.Page
    Private UserID As String
    Private SelectedID As String = 0
    Private SelectedUserName As String
    Private Reader As SqlDataReader
    Private SelectedTransactionID As String

    Public ThisSqlDataSource As String = "Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename='C:\Users\Abey\Documents\Visual Studio 2017\WebSites\Saf-Send\App_Data\aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96.mdf';Initial Catalog=aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96;Integrated Security=True"
    Private Connection As SqlConnection

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        UserID = CType(Session.Item("UserID"), String)

        SelectedTransactionID = -2
    End Sub
    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged
        Dim Gindex As Integer = -1
        Gindex = GridView1.SelectedIndex

        Dim Connection As SqlConnection


        If Gindex <> -1 Then

            Connection = New SqlConnection(ThisSqlDataSource)
            Dim Command As New SqlCommand("Select Id from UserTable where Name ='" + GridView1.Rows(Gindex).Cells(1).Text.ToString +
                                          "'and Surname = '" + GridView1.Rows(Gindex).Cells(2).Text.ToString +
                                          "' and Location ='" + GridView1.Rows(Gindex).Cells(3).Text.ToString + "'", Connection)
            Connection.Open()
            Command.ExecuteNonQuery()
            Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
            If Reader.HasRows Then
                Do While Reader.Read
                    SelectedID = CInt(Reader("ID"))
                    Session("TellerBID") = SelectedID
                Loop
            End If
        End If
        SelectedUserName = GridView1.Rows(Gindex).Cells(1).Text + " " + GridView1.Rows(Gindex).Cells(2).Text '+ " ID number is " + Session("TellerBID")
        Session("tempNameofTellerB") = SelectedUserName
        GridView1.Rows(GridView1.SelectedIndex).CssClass = "warning"
        Response.Redirect("~/utilities/Transaction")
    End Sub
    Protected Sub GridView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView2.SelectedIndexChanged
        Dim Gindex As Integer = GridView2.SelectedIndex

        If Gindex <> -1 Then
            Session("SelectedTransactionID") = GetTransactionID(UserID, GridView2.Rows(Gindex).Cells(1).Text.ToString, GridView2.Rows(Gindex).Cells(2).Text.ToString, GridView2.Rows(Gindex).Cells(3).Text.ToString, GridView2.Rows(Gindex).Cells(4).Text.ToString, GridView2.Rows(Gindex).Cells(6).Text.ToString)

        End If

        GridView2.Rows(Gindex).CssClass = "danger"

    End Sub
    Protected Sub btnTransactAccept_Click(sender As Object, e As EventArgs) Handles btnTransactAccept.Click
        Dim transaction(4) As String
        Dim network(5) As String
        Dim SenderTellerID As String
        Dim ReceiverTellerID As String
        Dim NetworkID As String
        Dim DebtTransfer As Double
        Dim amount As Double

        For i As Integer = 0 To 4
            transaction(i) = "0"
            network(i) = "0"
        Next


        If GridView2.SelectedIndex = -1 Then
            PHTransactionError.Visible = True
            ltlTerror.Text = "Transaction not Selected. Please Refresh this page and try again."
        Else
            GetTransaction(Session("SelectedTransactionID").ToString, transaction)
            SenderTellerID = transaction(1)
            ReceiverTellerID = transaction(2)


            If CInt(SenderTellerID) > 0 And CInt(ReceiverTellerID) > 0 Then

                NetworkID = getNetworkID(SenderTellerID, ReceiverTellerID)
                getNetwork(NetworkID, network)

                If CInt(network(1)) > 0 And CInt(network(2)) > 0 Then

                    amount = CDbl(transaction(3))
                    DebtTransfer = CalcDebt(transaction, network, amount)

                    'Update transaction to isRecieved = 1
                    Dim msg1 As String = UpdateTransactions(1, transaction(0))
                    'Insert Transaction Table with new successful transaction ID and NetworkID
                    Dim msg2 As String = UpdateTransactionHistory(network(0), transaction(0), 1, DebtTransfer)

                    'Network Table is updated with the latest Credit and trust Levels.
                    Dim msg3 As String = UpdateNetworkAfterTransactionResponse(1, transaction(0), network(0))
                    If msg1 = "Transaction updated as recieved transaction." And msg2 = "History is updated." And msg3 = "Debt and Trust Management Complete." Then
                        PHTransactionSuccess.Visible = True
                        ltlTsuccess.Text = msg1 + msg2 + msg3
                    Else
                        PHTransactionError.Visible = True
                        ltlTerror.Text = msg1 + msg2 + msg3
                    End If
                Else
                    PHTransactionError.Visible = True
                    ltlTerror.Text = "Selected tellers in the selected transaction are not networked. Please Report this to System Adminstrator." + " Network ID: " + NetworkID + " NTellerA: " + network(1) + " NTellerB : " + network(2) + " TTellerA: " + SenderTellerID + " TTellerB: " + ReceiverTellerID
                End If


            Else
                PHTransactionError.Visible = True
                ltlTerror.Text = "Selected Transaction could not be found. Please Contact the System Adminstrator. Sender: " + transaction(1) + "ReceiverTellerID" + transaction(2)
            End If

        End If

    End Sub
    Protected Sub BtnTranFailed_Click(sender As Object, e As EventArgs) Handles BtnTranFailed.Click
        If GridView2.SelectedIndex = -1 Then
            PHTransactionError.Visible = True
            ltlTerror.Text = "Transaction not Selected. Please Refresh this page and try again."
        Else
            Dim msg1 As String = UpdateTransactions(-1, Session("SelectedTransactionID").ToString)
            PHTransactionSuccess.Visible = True
            ltlTsuccess.Text = msg1
        End If
    End Sub
    Protected Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Response.Redirect("~/utilities/Home")
    End Sub


    'Delagated Functions
    Private Function GetTransactionID(ByVal UserID As String, ByVal SelectedName As String, SelectedSurname As String, ByVal SelectedAmount As String, ByVal SelectedDescription As String, ByVal Selectedtime As String) As String
        Dim TransactionID As String = "0"
        Dim SDatetime As DateTime = Convert.ToDateTime(Selectedtime)

        Connection = New SqlConnection(ThisSqlDataSource)
        Dim Command As New SqlCommand("Select TransactionTable.Id from TransactionTable INNER JOIN UserTable ON TransactionTable.TellerAID = UserTable.Id " +
                                      "where UserTable.Name = @SelectedName and UserTable.Surname = @SelectedSurname " +
                                      " and TransactionTable.Amount = @SelectedAmount" +' and TransactionTable.Time = @Selectedtime" +
                                      " and TransactionTable.Description = @SelectedDescription " +
                                      " and TransactionTable.TellerBID = @UserID ", Connection)
        Command.Parameters.AddWithValue("@SelectedName", SelectedName)
        Command.Parameters.AddWithValue("@SelectedSurname", SelectedSurname)
        Command.Parameters.AddWithValue("@SelectedAmount", SelectedAmount)
        Command.Parameters.AddWithValue("@Selectedtime", Selectedtime)
        Command.Parameters.AddWithValue("@SelectedDescription", SelectedDescription)
        Command.Parameters.AddWithValue("@UserID", UserID)

        Connection.Open()
        Command.ExecuteNonQuery()
        Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
        If Reader.HasRows Then
            Do While Reader.Read
                TransactionID = Reader("ID")
            Loop
        Else
            PHTransactionError.Visible = True
            ltlTerror.Text = "The transaction could not be found. Please Refresh this page and try again: " + TransactionID
        End If
        Connection.Close()
        Return TransactionID

    End Function

    'Return(0) = NetworkID Return(1) = TellerAID Return(2) = TellerBID
    Private Sub getNetwork(ByVal NetworkID As String, ByRef Network As String())

        For i As Integer = 0 To 5
            Network(i) = "0"
        Next

        Connection = New SqlConnection(ThisSqlDataSource)
        Dim Command As New SqlCommand("Select Id, TellerAID , TellerBID , Credit , TrustLevel , IsAccept from NetworkTable " +
                                      " Where Id =  @NetworkID ", Connection)
        Command.Parameters.AddWithValue("@NetworkID", NetworkID)


        Connection.Open()
        Command.ExecuteNonQuery()
        Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
        If Reader.HasRows Then
            Do While Reader.Read
                Network(0) = Reader("ID")
                Network(1) = Reader("TellerAID")
                Network(2) = Reader("TellerBID")
                Network(3) = Reader("Credit")
                Network(4) = Reader("TrustLevel")
                Network(5) = Reader("IsAccept")
            Loop
        Else
            PHTransactionError.Visible = True
            ltlTerror.Text = "The users are Not Networked. This Is a system violation. Pleases report the error."
        End If
        Connection.Close()

    End Sub

    Private Function getNetworkID(ByVal SenderTellerID As String, ByVal ReceiverTellerID As String) As String
        Dim NetworkID As String = "-1"


        Connection = New SqlConnection(ThisSqlDataSource)
        Dim Command As New SqlCommand("Select Id from NetworkTable " +
                                      " Where (TellerBID = @SenderTellerID) and (TellerAID = @ReceiverTellerID ) AND (IsAccept = 1) " +
                                      " OR (TellerAID = @SenderTellerID) and (TellerBID = @ReceiverTellerID ) AND (IsAccept = 1)", Connection)



        Command.Parameters.AddWithValue("@SenderTellerID", SenderTellerID)
        Command.Parameters.AddWithValue("@ReceiverTellerID", ReceiverTellerID)

        Connection.Open()
        Try
            Command.ExecuteNonQuery()
            Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                If Reader.HasRows Then
                    Do While Reader.Read
                        NetworkID = Reader("Id")
                        Return NetworkID
                    Loop
                Else
                    PHTransactionError.Visible = True
                    ltlTerror.Text = "The users are not Networked! This is a system violation. Pleases report the error."
                End If

        Catch ex As Exception
            PHTransactionError.Visible = True
            ltlTerror.Text = "Can not find the network associated because " & ex.Message

        End Try


        Connection.Close()

    End Function
    'Return(0) = TransactionID  Return(1) = Teller A and Return(2) = Teller B Return(3) = Amount and Return(4) = Profit
    Private Sub GetTransaction(ByVal TransactionID As String, ByRef transaction As String())

        Try
            Connection = New SqlConnection(ThisSqlDataSource)
            Dim Command2 As New SqlCommand("Select ID , TellerAID , TellerBID , Amount , Profit from TransactionTable where ID like @TransactionID", Connection)
            Command2.Parameters.AddWithValue("@TransactionID", TransactionID)
            Connection.Open()
            Command2.ExecuteNonQuery()
            Reader = Command2.ExecuteReader(CommandBehavior.CloseConnection)

            If Reader.HasRows Then
                Do While Reader.Read
                    transaction(0) = Reader("ID")
                    transaction(1) = Reader("TellerAID")
                    'Session("TTellerAID") = Reader("TellerAID")
                    transaction(2) = Reader("TellerBID")
                    ' Session("TTellerBID") = Reader("TellerBID")
                    transaction(3) = Reader("Amount")
                    transaction(4) = Reader("Profit")
                Loop
            Else
                PHTransactionError.Visible = True
                ltlTerror.Text = "No transaction with the ID no. :" + TransactionID
            End If

        Catch ex As Exception
            PHTransactionError.Visible = True
            ltlTerror.Text = "Transacton was not found because " & ex.Message
        End Try


    End Sub



    Private Function UpdateTransactions(ByVal Isreceive As Integer, ByVal TransactionId As String) As String
        Dim msg As String = ""

        Try
            Connection = New SqlConnection(ThisSqlDataSource)

            Dim Command As New SqlCommand("Update TransactionTable SET IsReceived = @Isreceive where Id= @TransactionId ", Connection)
            Command.Parameters.AddWithValue("@Isreceive", Isreceive)
            Command.Parameters.AddWithValue("@TransactionId", TransactionId)
            Connection.Open()
            If Command.ExecuteNonQuery() = 1 Then
                msg = "Transaction updated as recieved transaction."



            Else
                msg = "The transaction was unsuccessful. Please Refresh this page and try again."
            End If
        Catch ex As Exception
            msg = "The transaction respond failed because " & ex.Message
        End Try
        Connection.Close()
        Return msg
    End Function
    'Update Transaction History Table
    Private Function UpdateTransactionHistory(ByVal NetworkID As String, ByVal TransactionID As String, ByVal TrustRewards As Double, ByVal DebtTransfer As Double) As String
        Dim msg As String = ""

        Try
            Connection = New SqlConnection(ThisSqlDataSource)

            Dim Command As New SqlCommand("INSERT INTO TransactionHistory(NetworkID, TransactionID,TrustRewards,DebtTransfer) values (@NetworkID,@TransactionID,@TrustRewards,@DebtTransfer) ", Connection)
            Command.Parameters.AddWithValue("@NetworkID", NetworkID)
            Command.Parameters.AddWithValue("@TransactionID", TransactionID)
            Command.Parameters.AddWithValue("@TrustRewards", TrustRewards)
            Command.Parameters.AddWithValue("@DebtTransfer", DebtTransfer)
            Connection.Open()
            If Command.ExecuteNonQuery() = 1 Then

                msg = "History is updated."

            Else
                msg = "The transaction history was unsuccessful."
            End If
        Catch ex As Exception
            msg = "The transaction history failed because " & ex.Message
        End Try
        Connection.Close()

        Return msg
    End Function


    'Increments trust on Every Successfull transaction and decreaments trust on rejects.
    'Debt and Credit are calculated based upon who has requested the transaction
    Private Function UpdateNetworkAfterTransactionResponse(ByVal Response As Integer, ByVal TransactionID As String, ByVal NetworkID As String) As String
        Dim Returnmsg As String
        Dim Command, CmdSelectTHisotry As SqlCommand
        Dim Credit, Reward As Double


        Connection = New SqlConnection(ThisSqlDataSource)

        If Response = 1 Then
            Connection.Open()
            CmdSelectTHisotry = New SqlCommand("Select TrustRewards , DebtTransfer from TransactionHistory where TransactionID = @TransactionID and NetworkID = @NetworkID", Connection)
            CmdSelectTHisotry.Parameters.AddWithValue("@TransactionID", TransactionID)
            CmdSelectTHisotry.Parameters.AddWithValue("@NetworkID", NetworkID)
            CmdSelectTHisotry.ExecuteNonQuery()
            Reader = CmdSelectTHisotry.ExecuteReader(CommandBehavior.CloseConnection)

            If Reader.HasRows Then
                Do While Reader.Read
                    Reward = CDbl(Reader("TrustRewards"))
                    Credit = CDbl(Reader("DebtTransfer"))

                    Command = New SqlCommand("Update NetworkTable SET Credit = Credit + @Credit, TrustLevel = TrustLevel + @trustreward  where Id= @NetworkID ", Connection)
                    Command.Parameters.AddWithValue("@NetworkID", NetworkID)
                    Command.Parameters.AddWithValue("@Credit", Credit)
                    Command.Parameters.AddWithValue("@trustreward", Reward)
                Loop
            End If



        ElseIf Response = -1 Then
            Connection.Open()
            Command = New SqlCommand("Update NetworkTable SET trust = trust-1  where Id= @NetworkID ", Connection)
            Command.Parameters.AddWithValue("@NetworkID", NetworkID)

        End If

        Connection.Close()
        Connection.Open()

        Try

            If Command.ExecuteNonQuery() = 1 Then
                Returnmsg = "Debt and Trust Management Complete."
            Else
                Returnmsg = "The Debt and Trust management has failed"
            End If
        Catch ex As Exception
            Returnmsg = "Debt and trust management has stopped because " & ex.Message
        End Try
        Connection.Close()
        Return Returnmsg
    End Function


    'Calculate Debt Transafer

    Private Function CalcDebt(ByRef transaction() As String, ByRef Network() As String, ByVal amount As Double) As Double

        If Network(1) = transaction(1) And Network(2) = transaction(2) Then

            Return amount
        ElseIf (Network(2) = transaction(1) And Network(1) = transaction(2)) Then
            Return amount * -1
        End If

    End Function
End Class
