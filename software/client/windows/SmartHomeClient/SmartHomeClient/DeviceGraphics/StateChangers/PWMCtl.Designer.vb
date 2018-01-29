<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PWMCtl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.lblSValue = New System.Windows.Forms.Label()
        Me.lblCurState = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(14, 20)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(70, 70)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'TrackBar1
        '
        Me.TrackBar1.AutoSize = False
        Me.TrackBar1.Location = New System.Drawing.Point(103, 11)
        Me.TrackBar1.Margin = New System.Windows.Forms.Padding(0)
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(115, 33)
        Me.TrackBar1.TabIndex = 1
        Me.TrackBar1.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'lblSValue
        '
        Me.lblSValue.AutoSize = True
        Me.lblSValue.Location = New System.Drawing.Point(221, 20)
        Me.lblSValue.Name = "lblSValue"
        Me.lblSValue.Size = New System.Drawing.Size(13, 13)
        Me.lblSValue.TabIndex = 2
        Me.lblSValue.Text = "0"
        '
        'lblCurState
        '
        Me.lblCurState.AutoSize = True
        Me.lblCurState.Location = New System.Drawing.Point(100, 66)
        Me.lblCurState.Name = "lblCurState"
        Me.lblCurState.Size = New System.Drawing.Size(13, 13)
        Me.lblCurState.TabIndex = 4
        Me.lblCurState.Text = "0"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Button1.Location = New System.Drawing.Point(154, 55)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(113, 35)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Set state"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'PWMCtl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DarkOrange
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblCurState)
        Me.Controls.Add(Me.lblSValue)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "PWMCtl"
        Me.Size = New System.Drawing.Size(280, 104)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents TrackBar1 As System.Windows.Forms.TrackBar
    Friend WithEvents lblSValue As System.Windows.Forms.Label
    Friend WithEvents lblCurState As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
