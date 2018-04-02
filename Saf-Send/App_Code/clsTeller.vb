Imports Microsoft.VisualBasic

Public Class clsTeller
    Inherits clsUser

    Private UserName As String
    Private Password As String


    Public Sub New(vUserName As String, vPassword As String)
        UserName = vUserName
        Password = vPassword

    End Sub



    Public Sub New(vUserName As String, vPassword As String, vName As String,
                   vSurname As String,
                   vNationality As String,
                   vHomeLanguage As String,
                   vLocation As String)

        UserName = vUserName
        Password = vPassword
        Name = vName
        Surname1 = vSurname
        Nationality1 = vNationality
        HomeLanguage1 = vHomeLanguage
        Location1 = vLocation

    End Sub
    Public Function getUserName() As String
        Return UserName

    End Function
    Public Function getPassword() As String
        Return Password
    End Function

    Public Function Iscorrect(ByVal vUserName As String, ByVal vPass As String) As Boolean

        If (UserName = vUserName) Then
            If (Password = vPass) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function

End Class

