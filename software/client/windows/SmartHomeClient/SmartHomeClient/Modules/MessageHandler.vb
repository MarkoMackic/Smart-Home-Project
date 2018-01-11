Imports SmartHomeClient.Globals
Public Class MessageHandler
    Public Sub New()
        logInstantiation(Me)
    End Sub
    Public Function SerialDataRecieved(ByVal msg As String)
        mainForm.addLog("Unhandled serial message : " + msg)
        Return 0
    End Function
    Public Function handleServerMessage(ByVal msg As String)

        Return 0
    End Function
End Class
