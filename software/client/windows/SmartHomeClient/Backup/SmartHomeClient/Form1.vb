Imports SmartHomeClient.Globals

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialize all the resources
        hardwareChannel = New HardwareComm("COM5", 115200)
        msgHandler = New MessageHandler()

        hardwareChannel.startCommunication()
        hardwareChannel.sendData("authmessage", True, Me)
        hardwareChannel.sendData("pas", True, Me)
    End Sub
    Private Function doDeviceAuth(ByVal authStr As String)
        hardwareChannel.sendData("authmessage", True, Me)
    End Function
    Public Sub serialDataRecieved(ByVal msg As String)
        MsgBox(msg)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        hardwareChannel()
    End Sub
End Class
