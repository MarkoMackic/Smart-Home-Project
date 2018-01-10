Imports SmartHomeClient.Globals
Imports System.Data.SqlServerCe

Public Class DBDriver
    Dim con As SqlCeConnection = Nothing
    Dim dataSource As String = Nothing

    Public Sub New(ByVal dSource As String)
        dataSource = dSource
        openConnection()
    End Sub

    Public Function openConnection() As Boolean
        If (con Is Nothing) Then
            con = New SqlCeConnection(dataSource)
        End If
        If (con.State = ConnectionState.Closed) Then
            con.Open()
            Return True
        End If
        Return False
    End Function


    Public Function closeConnection() As Boolean
        If (con.State <> ConnectionState.Closed) Then
            con.Close()
            Return True
        End If
        Return False
    End Function

    Public Function GetData(ByVal cmd As String) As SqlCeDataReader

        Dim result As SqlCeDataReader
        Dim sqlCmd As New SqlCeCommand(cmd, con)
        result = sqlCmd.ExecuteReader()

        Return result
    End Function

    Public Function GetData(ByVal sqlCmd As SqlCeCommand) As SqlCeDataReader
        Dim result As SqlCeDataReader

        sqlCmd.Connection = con
        result = sqlCmd.ExecuteReader()


        Return result
    End Function

    Public Function Query(ByVal cmd As String) As Integer
        Dim result As Integer
        Dim sqlCmd As New SqlCeCommand(cmd, con)
        result = sqlCmd.ExecuteNonQuery()
        Return result
    End Function

    Public Function Query(ByVal sqlCmd As SqlCeCommand) As Integer
        Dim result As Integer
        sqlCmd.Connection = con
        result = sqlCmd.ExecuteNonQuery()

        Return result
    End Function
End Class
