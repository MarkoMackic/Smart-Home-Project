Imports System.Drawing.Graphics
Public Class PWMCtl
    Implements IStateChanger

    Public State As Integer
    Private ctlDev As Device
    Private percentage As Integer = 0


    Private Sub PWMCtl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False

      
    End Sub

    Public Sub New(ByVal dev As Device, ByVal min As Integer, ByVal max As Integer)
        InitializeComponent()
        ctlDev = dev

        TrackBar1.Minimum = min
        TrackBar1.Maximum = max
        Me.ForeColor = Color.Black
    End Sub


    Public Function StateUpdate(ByVal state As Object) Implements IStateChanger.StateUpdate

        If TypeOf state Is String And IsNumeric(state) Then
            Dim tmpState As Integer = Int(state)
            If tmpState >= TrackBar1.Minimum And tmpState <= TrackBar1.Maximum Then
                percentage = ((tmpState - TrackBar1.Minimum) * 100) / (TrackBar1.Maximum - TrackBar1.Minimum)
                lblCurState.Text = tmpState
                PictureBox1.Invalidate()
            End If

            Return True

        End If
        Return False
    End Function

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint


        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.High
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        Dim p As New Pen(Brushes.Green, 10)
        Dim p1 As New Pen(Brushes.Red, 10)
        Dim angle As Single = 360 * percentage / 100
        Dim leftOverAngle As Single = 360 - angle
        e.Graphics.DrawArc(p, New Rectangle(PictureBox1.ClientRectangle.Left + 5, PictureBox1.ClientRectangle.Top + 5, PictureBox1.ClientRectangle.Width - 10, PictureBox1.ClientRectangle.Height - 10), -90, angle)
        e.Graphics.DrawArc(p1, New Rectangle(PictureBox1.ClientRectangle.Left + 5, PictureBox1.ClientRectangle.Top + 5, PictureBox1.ClientRectangle.Width - 10, PictureBox1.ClientRectangle.Height - 10), -89 + angle, leftOverAngle - 1)
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        lblSValue.Text = TrackBar1.Value
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ctlDev.ChangeState(New Object() {TrackBar1.Value})
    End Sub
End Class
