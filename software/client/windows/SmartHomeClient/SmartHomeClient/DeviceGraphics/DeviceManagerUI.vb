Imports SmartHomeClient.Globals

Public Class DeviceManagerUI
    Public viewMapper As New Hashtable
    Private maxHeight As Integer = 0
    Private Sub DeviceManagerUI_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        mainForm.Show()
    End Sub

    Private Sub DeviceManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not devManager Is Nothing Then
            devManager.attachUI(Me)
            For Each dev As Device In devManager.GetAllDevices()
                Dim ctl As DeviceView = New DeviceView(dev)
                Me.mainContainter.Controls.Add(ctl)
                viewMapper(dev.ID) = ctl

            Next
        End If



    End Sub
  
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub mainContainter_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles mainContainter.Paint

    End Sub
    Public Sub deviceStateChanged(ByVal devId As Integer, ByVal state As String)

        If viewMapper.ContainsKey(devId) Then
            Dim devView As DeviceView = viewMapper(devId)
            devView.deviceStateChanged(state)
        End If

    End Sub



End Class