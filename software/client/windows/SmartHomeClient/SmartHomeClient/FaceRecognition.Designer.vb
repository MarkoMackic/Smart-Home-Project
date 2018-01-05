<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FaceRecognition
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
        Me.faceBox = New System.Windows.Forms.PictureBox()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.EventLog1 = New System.Diagnostics.EventLog()
        Me.saveBtn = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtUname = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.facebox1 = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.faceBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.facebox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'faceBox
        '
        Me.faceBox.Location = New System.Drawing.Point(12, 12)
        Me.faceBox.Name = "faceBox"
        Me.faceBox.Size = New System.Drawing.Size(274, 275)
        Me.faceBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.faceBox.TabIndex = 0
        Me.faceBox.TabStop = False
        '
        'btnPrev
        '
        Me.btnPrev.Location = New System.Drawing.Point(12, 318)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(75, 23)
        Me.btnPrev.TabIndex = 1
        Me.btnPrev.Text = "Prev"
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(211, 318)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(75, 23)
        Me.btnNext.TabIndex = 2
        Me.btnNext.Text = "Next"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(106, 318)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(75, 23)
        Me.btnNew.TabIndex = 3
        Me.btnNew.Text = "New face"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'EventLog1
        '
        Me.EventLog1.SynchronizingObject = Me
        '
        'saveBtn
        '
        Me.saveBtn.Location = New System.Drawing.Point(316, 318)
        Me.saveBtn.Name = "saveBtn"
        Me.saveBtn.Size = New System.Drawing.Size(75, 23)
        Me.saveBtn.TabIndex = 4
        Me.saveBtn.Text = "Save face"
        Me.saveBtn.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(301, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Username : "
        '
        'txtUname
        '
        Me.txtUname.Location = New System.Drawing.Point(304, 39)
        Me.txtUname.Name = "txtUname"
        Me.txtUname.Size = New System.Drawing.Size(168, 20)
        Me.txtUname.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(405, 119)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Label2"
        Me.Label2.Visible = False
        '
        'facebox1
        '
        Me.facebox1.Location = New System.Drawing.Point(564, 26)
        Me.facebox1.Name = "facebox1"
        Me.facebox1.Size = New System.Drawing.Size(274, 275)
        Me.facebox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.facebox1.TabIndex = 8
        Me.facebox1.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(313, 119)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Eigen distance:"
        Me.Label3.Visible = False
        '
        'FaceRecognition
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(859, 367)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.facebox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtUname)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.saveBtn)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnPrev)
        Me.Controls.Add(Me.faceBox)
        Me.Name = "FaceRecognition"
        Me.Text = "Facial scanner"
        CType(Me.faceBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.facebox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents faceBox As System.Windows.Forms.PictureBox
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents EventLog1 As System.Diagnostics.EventLog
    Friend WithEvents saveBtn As System.Windows.Forms.Button
    Friend WithEvents txtUname As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents facebox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
