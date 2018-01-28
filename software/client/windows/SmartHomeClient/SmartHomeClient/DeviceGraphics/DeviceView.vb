Imports System.Dynamic
Imports System.Drawing.Drawing2D

Public Class DeviceView
    Public Sub New(ByVal dev As Device)

        ' This call is required by the designer.
        InitializeComponent()
        Me.BackColor = Color.Transparent
        Me.BorderStyle = Windows.Forms.BorderStyle.None

        ' Add any initialization after the InitializeComponent() call.
        Dim devProps As Object = dev.SerializableObject()

        'tempObj.Name = Me.Name
        'tempObj.Type = Me.Type
        'tempObj.ID = Me.ID
        'If Not Me.Master Is Nothing Then
        '    tempObj.MasterName = Me.Master.Name
        'End If
        'tempObj.State = Driver.StateStr()
        'tempObj.Driver = Driver.GetType.ToString()

        'Return tempObj

        lblID.Text = devProps.ID
        lblName.Text = devProps.Name
        lblDriver.Text = devProps.Driver
        lblParent.Text = devProps.MasterName


        Select Case devProps.AcceptStateType
            Case "BOOLEAN"
                Dim ctl As OnOff = New OnOff(dev)
                ctl.Dock = DockStyle.Fill
                ctl.TabIndex = 5
                pnlStateChanger.Controls.Add(ctl)

        End Select




    End Sub
    Private Sub DeviceView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
    End Sub

    Private Sub DeviceView_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality
        e.Graphics.InterpolationMode = InterpolationMode.High

        Dim rect As Rectangle = Me.ClientRectangle 'Drawing Rounded Rectangle
        rect.X = rect.X + 1
        rect.Y = rect.Y + 1
        rect.Width -= 2
        rect.Height -= 2
        Using bb As GraphicsPath = GetPath(rect, 10)
            Using br As Brush = New SolidBrush(Color.FromArgb(255, Color.Blue))
                e.Graphics.FillPath(br, bb)
            End Using
        End Using
    End Sub


    Protected Function GetPath(ByVal rc As Rectangle, ByVal r As Int32) As GraphicsPath
        Dim x As Int32 = rc.X, y As Int32 = rc.Y, w As Int32 = rc.Width, h As Int32 = rc.Height
        r = r << 1
        Dim path As GraphicsPath = New GraphicsPath()
        If r > 0 Then
            If (r > h) Then r = h
            If (r > w) Then r = w
            path.AddArc(x, y, r, r, 180, 90)
            path.AddArc(x + w - r, y, r, r, 270, 90)
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90)
            path.AddArc(x, y + h - r, r, r, 90, 90)
            path.CloseFigure()
        Else
            path.AddRectangle(rc)
        End If
        Return path
    End Function

    Public Function deviceStateChanged(ByVal state As String)
        If pnlStateChanger.Controls.Count > 0 Then
            Dim devStateChanger = CType(pnlStateChanger.Controls(0), IStateChanger)
            Return devStateChanger.StateUpdate(state)

        End If
        Return False
    End Function
    Private Sub pnlStateChanger_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlStateChanger.Paint

    End Sub

    Private Sub lblDriver_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDriver.Click

    End Sub
End Class
