Imports System.Drawing.Drawing2D
Public Class SplashScreen
    Dim sec As Integer = 0
    Dim i As New Hashtable
    Dim rnd_brushses() As Brush = New Brush() {Brushes.Red, Brushes.Green, Brushes.Wheat, Brushes.Bisque}


    Private Sub SplashScreen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Dim t As New System.Timers.Timer()
        t.Interval = 200
        t.AutoReset = True
        AddHandler t.Elapsed, AddressOf MoveClock
        t.Start()

    End Sub

    Private Sub changeText(ByVal text As String)
        If InvokeRequired Then
            Me.Invoke(Sub() changeText(text))
            Return
        End If
        Label1.Text = text
    End Sub

    Private Sub closeMe()
        If InvokeRequired Then
            Me.Invoke(Sub() closeMe())
            Return
        End If
        Me.Close()
    End Sub
    Private Sub MoveClock()
        sec += 10
        Me.PictureBox1.Invalidate()
        sec = sec Mod 370
        If sec = 0 Then
            changeText("Finished loading....")
            Threading.Thread.Sleep(1000)
            closeMe()
        End If
    End Sub

   

    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint

        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Graphics.FillEllipse(Brushes.Brown, Me.PictureBox1.ClientRectangle)
        Dim linGrBrush As LinearGradientBrush = New LinearGradientBrush(Me.PictureBox1.ClientRectangle, Color.Lime, Color.DarkBlue, sec)
        linGrBrush.Transform = New Matrix()


        i(sec) = linGrBrush

        For Each intgr As Integer In i.Keys
            If (intgr - 10 < 10) Then
                e.Graphics.FillPie(i(intgr), Me.PictureBox1.ClientRectangle, 0, 10)
            Else
                e.Graphics.FillPie(i(intgr), Me.PictureBox1.ClientRectangle, intgr - 10, 10)
            End If
        Next

        'e.Graphics.FillEllipse(Brushes.Red, Me.PictureBox1.ClientRectangle)
        'Dim ugao As Double = 3165
        'ugao = ugao Mod 360
        'Dim ugaorad = ugao * Math.PI / 180
        'Dim poluprecnik As Integer = Me.PictureBox1.ClientRectangle.Width / 2
        'Debug.WriteLine(Math.Sin(ugaorad))
        'Dim y As Integer = Math.Sin(ugaorad) * poluprecnik
        'Debug.WriteLine(y)
        'Dim x As Integer = Math.Sqrt(poluprecnik * poluprecnik - y * y)
        'Debug.WriteLine(x)
        'y = poluprecnik - y
        'If ugao >= 0 And ugao <= 90 Or ugao >= 270 And ugao < 360 Then
        '    x = x
        'Else
        '    x = -x
        'End If
        'x = poluprecnik + x
        'e.Graphics.DrawLine(Pens.Blue, New Point(Me.PictureBox1.ClientRectangle.Width / 2, Me.PictureBox1.ClientRectangle.Height / 2), New Point(x, y))


    End Sub
End Class