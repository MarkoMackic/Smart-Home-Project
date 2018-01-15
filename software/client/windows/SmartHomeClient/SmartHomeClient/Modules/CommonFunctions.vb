Imports SmartHomeClient.Globals
Module CommonFunctions

    Public Sub logInstantiation(ByVal obj As Object)
        mainForm.addLog(obj.GetType().Name + " is instantiated . ")
    End Sub

    Public Function trimSpaces(ByVal str As String)
        Return str.Trim(New Char() {" "})
    End Function


    Public Sub closeApplication()
        MainUI.Close()
    End Sub

    Public Sub DriverLog(ByVal msg As String)

    End Sub

    Public Function Map(ByVal x As Long, ByVal in_min As Long, ByVal in_max As Long, ByVal out_min As Long, ByVal out_max As Long) As Long
        Return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min

    End Function

End Module
