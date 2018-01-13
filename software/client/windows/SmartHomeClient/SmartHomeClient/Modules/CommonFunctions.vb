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

End Module
