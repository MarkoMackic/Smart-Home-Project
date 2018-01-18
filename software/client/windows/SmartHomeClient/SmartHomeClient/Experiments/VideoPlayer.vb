Imports Emgu.CV
Imports System.IO
Imports System.Threading

Public Class VideoPlayer
    Dim cap As Capture
    Private Sub VideoPlayer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cap = New Capture(Path.Combine(Application.StartupPath, "Res/Video/testvideo.mp4"))


        Dim thr As New Thread(AddressOf showVideo)
        thr.Start()
    End Sub
    Private Sub ShowVideo()
        Try
            While True

                Dim i As Bitmap = cap.QueryFrame().ToImage(Of [Structure].Gray, Byte).Bitmap

                PictureBox1.Image = i
                Thread.Sleep(1000 / 30)
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click

    End Sub
End Class