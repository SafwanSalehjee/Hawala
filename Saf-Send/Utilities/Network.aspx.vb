Imports System.Data
Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports clsHawalaSystem

Partial Class Utilities_Network
    Inherits System.Web.UI.Page
    Private HS As clsHawalaSystem
    Private Reader As SqlDataReader
    Private ThisSqlDataSource As String = "Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename='C:\Users\Abey\Documents\Visual Studio 2017\WebSites\Saf-Send\App_Data\aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96.mdf';Initial Catalog=aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96;Integrated Security=True"
    Private Connection As SqlConnection



    Protected Sub btnInvite_Click(sender As Object, e As EventArgs) Handles btnInvite.Click
        PHInvite.Visible = True
        Reader = Nothing
        Dim InvitedUserID As String = getUserID(txtUsername.Text)
        Dim Validate As String = CheckIfInviteExsit(Session("UserID").ToString, InvitedUserID)
        If Validate = "Valid Invitation: No previous Invitation or network." Or Validate = "Valid Invitation: Invitation between the two teller was previous rejected." Then
            InsertInviteinTblNetwork(InvitedUserID, txtPaskey.Text)
        Else
            PHInvite.Visible = True
            ltlError.Text = Validate
        End If

        'Response.Redirect("~/utilities/Network")
    End Sub

    Private Function getUserID(ByVal Username As String) As String
        Dim temp As String = "0"

        Connection = New SqlConnection(ThisSqlDataSource)
        Dim Command As New SqlCommand("Select Id from UserTable where UserName = '" + Username + "'", Connection)
        Connection.Open()

        Command.ExecuteNonQuery()
        Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
        If Reader.HasRows Then
            Do While Reader.Read
                temp = Reader("ID")
            Loop

        Else
            ltlError.Text = "Username is incorrect"
        End If
        Connection.Close()
        Return temp
    End Function

    Private Sub InsertInviteinTblNetwork(ByVal InviteUserID As String, ByVal PassKey As String)


        Try
            Connection = New SqlConnection(ThisSqlDataSource)
            Dim Command As New SqlCommand("INSERT INTO NetworkTable (TellerAID, TellerBID, passKey) VALUES ('" + Session("UserID").ToString + "','" + InviteUserID + "','" + PassKey + "')", Connection)

            Connection.Open()
            Command.ExecuteNonQuery()
            PHInviteSuccess.Visible = True
            ltlInviteSucess.Text = "Invite to " + txtUsername.Text + " Has been sent succesfully. Please wait for user to respond."
            Connection.Close()
        Catch ex As Exception
            PHInvite.Visible = True
            ltlError.Text = "Invite failed because " & ex.Message

        End Try

    End Sub
    Protected Sub BtnInvAccept_Click(sender As Object, e As EventArgs) Handles BtnInvAccept.Click
        If ListBox1.SelectedIndex > -1 Then
            Dim UserName As String = ListBox1.SelectedItem.ToString
            Dim SelectedUserID As String = getUserID(UserName)
            Dim PassKey As String = InputBox("Please Provide the shared Pass Key", "Thank you.")
            Dim NetworkId As String = getNetworkID(SelectedUserID, Session("UserID").ToString, PassKey)
            InviteRespond("1", NetworkId, SelectedUserID)

        Else
            PHResponse.Visible = True
            ltlRerror.Text = "Please select an Appropriate option"
        End If

    End Sub
    Protected Sub BtnInvReject_Click(sender As Object, e As EventArgs) Handles BtnInvReject.Click
        If ListBox1.SelectedIndex > -1 Then
            Dim UserName As String = ListBox1.SelectedItem.ToString
            Dim SelectedUserID As String = getUserID(UserName)
            Dim NetworkId As String = getNetworkID(SelectedUserID, Session("UserID").ToString)
            InviteRespond("-1", NetworkId, SelectedUserID)
            ListBox1.Items.Clear()
        Else
            PHResponse.Visible = True
            ltlRerror.Text = "Please select an Appropriate option"
        End If
    End Sub

    Private Sub InviteRespond(ByVal Respond As String, ByVal networkID As String, ByVal SelectedUserID As String)
        Dim SQL As String
        SQL = "Update NetworkTable SET IsAccept = '" + Respond + "' where Id= '" + networkID + "'"
        Try
            Connection = New SqlConnection(ThisSqlDataSource)
            Dim Command As New SqlCommand(SQL, Connection)

            Connection.Open()
            Command.ExecuteNonQuery()

            Connection.Close()
            PHResonseSuccess.Visible = True
            ltlRsuccess.Text = "Your Respond was Successful."
        Catch ex As Exception
            PHResponse.Visible = True
            ltlRerror.Text = "Invite failed because " & ex.Message

        End Try

    End Sub

    Private Function getNetworkID(ByVal InviterID As String, InvitedID As String) As String
        Dim temp As String = "0"

        Connection = New SqlConnection(ThisSqlDataSource)
        Dim Command As New SqlCommand("Select Id from NetworkTable where TellerAID = '" + InviterID + "' and TellerBID = '" + InvitedID + "' and IsAccept = 0", Connection)
        Connection.Open()

        Command.ExecuteNonQuery()
        Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
        If Reader.HasRows Then
            Do While Reader.Read
                temp = Reader("ID")
            Loop

        Else
            ltlRerror.Text = "Please select an Appropriate option"
        End If
        Connection.Close()
        Return temp
    End Function

    Private Function getNetworkID(ByVal InviterID As String, InvitedID As String, ByVal PassKey As String) As String
        Dim temp As String = "0"

        Connection = New SqlConnection(ThisSqlDataSource)
        Dim Command As New SqlCommand("Select Id from NetworkTable where TellerAID = '" + InviterID + "' and TellerBID = '" + InvitedID + "' and PassKey = '" + PassKey + "' and IsAccept = 0", Connection)
        Connection.Open()

        Command.ExecuteNonQuery()
        Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
        If Reader.HasRows Then
            Do While Reader.Read
                temp = Reader("ID")
            Loop

        Else
            ltlRerror.Text = "Your Password was incorrect"
        End If
        Connection.Close()
        Return temp
    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim i As Integer = 0
        For Each Item In ListBox1.Items
            i = i + 1
        Next
        InviteH3.Text = "Invites <span class='badge'>" + CStr(i) + "</span>"
    End Sub

    Protected Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Response.Redirect("~/utilities/Network")
    End Sub

    Private Function CheckIfInviteExsit(ByVal InviterID As String, ByVal InvitedID As String) As String
        Dim InviteStatus = "Invalid Invite"
        Connection = New SqlConnection(ThisSqlDataSource)

        Dim Command4 As New SqlCommand("Select Id from NetworkTable where ((TellerAID = '" + InviterID + "' and TellerBID = '" + InvitedID + "' ) OR ( TellerBID = '" + InviterID + "' and TellerAID = '" + InvitedID + "' )) and IsAccept = -1", Connection)
        Dim Command2 As New SqlCommand("Select Id from NetworkTable where  ((TellerAID = '" + InviterID + "' and TellerBID = '" + InvitedID + "' ) OR ( TellerBID = '" + InviterID + "' and TellerAID = '" + InvitedID + "' )) AND IsAccept = 1 ", Connection)
        Dim Command3 As New SqlCommand("Select Id from NetworkTable where ((TellerAID = '" + InviterID + "' and TellerBID = '" + InvitedID + "' ) OR ( TellerBID = '" + InviterID + "' and TellerAID = '" + InvitedID + "' )) and IsAccept = 0", Connection)
        Dim Command As New SqlCommand("Select Id from NetworkTable where (TellerAID = '" + InviterID + "' and TellerBID = '" + InvitedID + "' ) OR ( TellerBID = '" + InviterID + "' and TellerAID = '" + InvitedID + "' )", Connection)



        'If there are no invitation or networked establish

        Connection.Open()

        Command.ExecuteNonQuery()
        Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
        If Not Reader.HasRows Then
            InviteStatus = "Valid Invitation: No previous Invitation or network."
        Else

            'If there are networked establish
            Connection.Close()
            Connection.Open()

            Command3.ExecuteNonQuery()
            Reader = Command3.ExecuteReader(CommandBehavior.CloseConnection)
            If Reader.HasRows Then
                InviteStatus = "Invalid Invitation: Invitation Already Exist. Please Respond from your Respond section or wait for respond"
            Else
                'If there is a previous Invitation
                Connection.Close()
                Connection.Open()



                Command2.ExecuteNonQuery()
                Reader = Command2.ExecuteReader(CommandBehavior.CloseConnection)
                If Reader.HasRows Then
                    InviteStatus = "Invalid Invitation: Network Already Exist. Please Check your Networked Tellers."
                Else
                    'If the invitation was rejected
                    Connection.Close()
                    Connection.Open()

                    Command4.ExecuteNonQuery()
                    Reader = Command4.ExecuteReader(CommandBehavior.CloseConnection)
                    If Reader.HasRows Then
                        InviteStatus = "Valid Invitation: Invitation between the two teller was previous rejected."
                    End If
                End If
            End If
        End If




        Connection.Close()


        Return InviteStatus
    End Function
End Class
