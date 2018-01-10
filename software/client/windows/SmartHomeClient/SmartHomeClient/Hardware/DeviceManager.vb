Imports System.Threading.Tasks
Imports SmartHomeClient.Globals

Public Class DeviceManager


    Public Sub New()

    End Sub

    Public Sub updateDeviceStates(ByVal serialData As String)
        Dim t As New task(Sub()
                              mainForm.addLog(serialData)
                          End Sub)
        t.Start()
    End Sub
End Class
