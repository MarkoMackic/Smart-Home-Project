Public Class MessageHandler

    Public Function SerialDataRecieved(ByVal msg As String)
        Debug.WriteLine("MH : " + msg)
        Return 0
    End Function
    Public Function handleServerMessage(ByVal msg As String)

        Return 0
    End Function
End Class
