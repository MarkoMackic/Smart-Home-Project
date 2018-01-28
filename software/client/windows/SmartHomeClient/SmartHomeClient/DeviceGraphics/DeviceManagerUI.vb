Imports SmartHomeClient.Globals

Public Class DeviceManagerUI

    Private Sub DeviceManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not devManager Is Nothing Then
            devManager.attachUI(Me)
            For Each dev As Device In devManager.GetAllDevices()
                Me.mainContainter.Controls.Add(New DeviceView(dev))
            Next
        End If



    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub mainContainter_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles mainContainter.Paint

    End Sub
End Class