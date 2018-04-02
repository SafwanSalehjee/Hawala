Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Data
Imports System.Data.SqlClient

Public Partial Class Account_Register
    Inherits Page

    Protected Sub CreateUser_Click(sender As Object, e As EventArgs)
        Dim manager = New UserManager()
        Dim user = New ApplicationUser() With {.UserName = UserName.Text}
        Dim result = manager.Create(user, Password.Text)
        If result.Succeeded Then
            CreateNewUserinTBLUser()
            IdentityHelper.SignIn(manager, user, isPersistent:=False)
            IdentityHelper.RedirectToReturnUrl(Request.QueryString("ReturnUrl"), Response)
        Else
            ErrorMessage.Text = result.Errors.FirstOrDefault()
        End If


    End Sub

    Private Sub CurrentUserInSession(ByVal Connection As SqlConnection, Username As String, Password As String)
        Dim Command As New SqlCommand("Select ID from UserTable where Username ='" + Username + "' and Password='" + Password + "'", Connection)
        Dim Reader As SqlDataReader

        Connection.Open()
        Command.ExecuteNonQuery()
        Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
        If Reader.HasRows Then
            Do While Reader.Read
                Session("UserID") = Reader("ID")
            Loop

        End If
    End Sub

    Private Sub CreateNewUserinTBLUser()
        Dim teller = New clsTeller(UserName.Text, Password.Text, FirstName.Text, Surname.Text, "", "", CurrentLocation.Text)
        Dim Hawalasystem = New clsHawalaSystem()
        Dim Connection As SqlConnection


        Try
            Connection = New SqlConnection("Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename='C:\Users\Abey\Documents\Visual Studio 2017\WebSites\Saf-Send\App_Data\aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96.mdf';Initial Catalog=aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96;Integrated Security=True")
            Dim Command As New SqlCommand("INSERT INTO UserTable (Name,Surname,[Home Language],Nation, Location, UserName,Password, isAdmin) VALUES ('" + teller.Name1() + "','" + teller.Surname1 + "','" + "" + "','" + "" + "','" + teller.Location1 + "','" + teller.getUserName + "','" + teller.getPassword + "'," + CStr(0) + ")", Connection)

            Connection.Open()
            Command.ExecuteNonQuery()
            CurrentUserInSession(Connection, teller.Name1(), teller.getPassword)
            Connection.Close()
        Catch ex As Exception
            ErrorMessage.Text = "Error while inserting record on table..." & ex.Message

        End Try

    End Sub
End Class
