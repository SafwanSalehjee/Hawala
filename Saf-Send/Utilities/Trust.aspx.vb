Imports System.Data
Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient

Partial Class Utilities_Trust
    Inherits System.Web.UI.Page

    Private Reader As SqlDataReader
    Private ThisSqlDataSource As String = "Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename='C:\Users\Abey\Documents\Visual Studio 2017\WebSites\Saf-Send\App_Data\aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96.mdf';Initial Catalog=aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96;Integrated Security=True"
    Private Connection As SqlConnection

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Function GetTrustDatatoJason() As String
        Dim msg As String = ""

        Connection = New SqlConnection(ThisSqlDataSource)
        Dim Command As New SqlCommand("Select UserName from UserTable", Connection)
        Connection.Open()

        Command.ExecuteNonQuery()

        Connection.Close()

        Return msg
    End Function
End Class
