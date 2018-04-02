Imports Microsoft.VisualBasic

Public Class clsUser
    Protected Name As String
    Private Surname As String
    Private Nationality As String
    Private HomeLanguage As String
    Private Location As String

    Public Property Location1 As String
        Get
            Return Location
        End Get
        Set(value As String)
            Location = value
        End Set
    End Property

    Public Property HomeLanguage1 As String
        Get
            Return HomeLanguage
        End Get
        Set(value As String)
            HomeLanguage = value
        End Set
    End Property

    Public Property Nationality1 As String
        Get
            Return Nationality
        End Get
        Set(value As String)
            Nationality = value
        End Set
    End Property

    Public Property Surname1 As String
        Get
            Return Surname
        End Get
        Set(value As String)
            Surname = value
        End Set
    End Property

    Public Property Name1 As String
        Get
            Return Name
        End Get
        Set(value As String)
            Name = value
        End Set
    End Property

    Public Function ToString() As String
        Return "Name : " + Name1 + " Surname: " + Surname + " Nationality: " + Nationality +
             " Home Language : " + HomeLanguage + " Location: " + Location

    End Function



End Class

