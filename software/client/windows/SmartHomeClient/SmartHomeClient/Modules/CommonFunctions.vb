Module CommonFunctions
   

    Public Function trimSpaces(ByVal str As String)
        Return str.Trim(New Char() {" "})
    End Function


    Public Sub closeApplication()
        MainUI.Close()
    End Sub


End Module
