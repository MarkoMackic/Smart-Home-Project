Imports System.Windows.Forms

Public Class SerialPortDialog
    Public Property Result As String

    Sub RefreshSerialNames()
        cbSP.Items.Clear() '' clear combo box portNames 
        For Each sp As String In My.Computer.Ports.SerialPortNames ''add all the 
            cbSP.Items.Add(sp)
        Next
        cbSP.Refresh()
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not String.IsNullOrEmpty(cbSP.SelectedItem) Then
            Me.Result = cbSP.SelectedItem
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MsgBox("Please choose COM port for device")
        End If

    End Sub



    Private Sub SerialPortDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RefreshSerialNames()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        RefreshSerialNames()
    End Sub
End Class
