Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.Owin.Security
Imports System.Data
Imports System.Data.SqlClient

Public Partial Class Account_Login
    Inherits Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        RegisterHyperLink.NavigateUrl = "Register"

        Dim returnUrl = HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        If Not [String].IsNullOrEmpty(returnUrl) Then
            RegisterHyperLink.NavigateUrl += "?ReturnUrl=" & returnUrl
        End If
    End Sub

    Protected Sub LogIn(sender As Object, e As EventArgs)
        If IsValid Then
            ' Validate the user password
            Dim manager = New UserManager()
            Dim user As ApplicationUser = manager.Find(UserName.Text, Password.Text)
            Dim Connection As SqlConnection
            Dim Reader As SqlDataReader


            If user IsNot Nothing Then
                IdentityHelper.SignIn(manager, user, RememberMe.Checked)
                Connection = New SqlConnection("Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename='C:\Users\Abey\Documents\Visual Studio 2017\WebSites\Saf-Send\App_Data\aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96.mdf';Initial Catalog=aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96;Integrated Security=True")
                Dim Command As New SqlCommand("Select ID from UserTable where Username ='" + UserName.Text + "' and Password='" + Password.Text + "'", Connection)

                Try
                    Connection.Open()
                    Command.ExecuteNonQuery()
                Catch ex As Exception
                    ErrorMessage.Visible = True
                    FailureText.Text = ex.Message

                End Try

                Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)


                If Reader.HasRows Then
                    Do While Reader.Read
                        Session("UserID") = Reader("ID")
                        IdentityHelper.RedirectToReturnUrl("~/utilities/Home", Response)
                    Loop
                Else
                    FailureText.Text = "Invalid username or password."
                    ErrorMessage.Visible = True
                End If


                'Response.Redirect("~/utilities/Home")

            Else
                FailureText.Text = "Invalid username or password."
            ErrorMessage.Visible = True
        End If


        End If
    End Sub


End Class
