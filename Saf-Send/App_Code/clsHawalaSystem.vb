Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class clsHawalaSystem
    Private ThisSqlDataSource As String = "Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename='C:\Users\Abey\Documents\Visual Studio 2017\WebSites\Saf-Send\App_Data\aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96.mdf';Initial Catalog=aspnet-Saf_Send-419da3c2-2292-43d0-aa34-26b14ec0be96;Integrated Security=True"
    Dim Connection As SqlConnection


    Public Sub RegisterTeller(ByVal teller As clsTeller)
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand

        Try
            con.ConnectionString = "Data Source=atisource;Initial Catalog=BillingSys;Persist Security Info=True;User ID=sa;Password=12345678"

            con.Open()
            cmd.Connection = con
            cmd.CommandText = "INSERT INTO UserTable (UserName,Password) VALUES (" + teller.getUserName() + "," + teller.getPassword() + ")"

        Catch ex As Exception
            ' MessageBox.Show("Error while inserting record on table..." & ex.Message, "Insert Records")
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub ReadFromDataBase(ByVal SQLstatement As String, ByRef Reader As SqlDataReader)
        Connection = New SqlConnection(ThisSqlDataSource)
        Dim Command As New SqlCommand(SQLstatement)

        Connection.Open()
        Command.ExecuteNonQuery()
        Reader = Command.ExecuteReader(CommandBehavior.CloseConnection)
    End Sub

End Class
