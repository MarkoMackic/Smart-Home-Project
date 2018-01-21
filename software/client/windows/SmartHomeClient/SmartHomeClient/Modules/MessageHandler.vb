Imports SmartHomeClient.Globals
Public Class MessageHandler
    Public Sub New()
        logInstantiation(Me)
    End Sub
    Public Function SerialDataRecieved(ByVal msg As String)
        Log("Unhandled serial message : " + msg, Color.OrangeRed)
        Return 0
    End Function
    Public Function handleServerMessage(ByVal msg As String)

        Return 0
    End Function
End Class
