<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DeviceManagerUI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainContainter = New System.Windows.Forms.FlowLayoutPanel()
        Me.SuspendLayout()
        '
        'mainContainter
        '
        Me.mainContainter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mainContainter.Location = New System.Drawing.Point(0, 0)
        Me.mainContainter.Name = "mainContainter"
        Me.mainContainter.Size = New System.Drawing.Size(1092, 475)
        Me.mainContainter.TabIndex = 0
        '
        'DeviceManagerUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1092, 475)
        Me.Controls.Add(Me.mainContainter)
        Me.Name = "DeviceManagerUI"
        Me.Text = "DeviceManager"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mainContainter As System.Windows.Forms.FlowLayoutPanel
End Class
