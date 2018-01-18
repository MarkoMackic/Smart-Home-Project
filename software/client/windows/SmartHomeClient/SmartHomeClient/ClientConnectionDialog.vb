Imports System.Windows.Forms

Public Class ClientConnectionDialog
    Public Property Host As String = Nothing
    Public Property Port As Integer = Nothing
    Public Property Username As String = Nothing
    Public Property Password As String = Nothing

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim host As String = txtHost.Text.Trim()
        Dim port As String = txtPort.Text.Trim()
        Dim username As String = txtUname.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()
        If Not String.IsNullOrEmpty(host) And
            Not String.IsNullOrEmpty(port) And IsNumeric(port) And
            Not String.IsNullOrEmpty(username) And
            Not String.IsNullOrEmpty(password) Then

            Me.Host = host
            Me.Port = Int(port)
            Me.Username = username
            Me.Password = password
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MsgBox("Please fill data correctly and try again.")
        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ClientConnectionDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
