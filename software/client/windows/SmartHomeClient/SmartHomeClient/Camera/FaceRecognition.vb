Imports SmartHomeClient.Globals
Imports Emgu.CV
Imports System.Data.SqlServerCe
Imports System.IO
Imports System.Threading
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports Emgu.CV.Face

Public Class FaceRecognition
    'EMGU components
    Private cap As Capture
    Private cc As New CascadeClassifier(Path.Combine(Application.StartupPath, "Res/Haarcascade/haarcascade_frontalface_default.xml"))
    Private recognizer As LBPHFaceRecognizer = Nothing

    'Hashtable for matching results from recognizer
    Private matches As Hashtable = New Hashtable()

    'Data from DB
    Private tempDBData As Hashtable = New Hashtable()
    Private idx As Integer = 0

    'Camera thread and its state
    Private thrCamera As Thread
    Private thrCameraRunning As Boolean

    'Indicate is camera started
    Private cameraStarted As Boolean = False

    'Bitmap of grayscale face image
    Private grayBitmap As Bitmap

    'Automated procedures (this form state)
    Public state As Integer = 0

    'How many faces do we have in db
    Private differentFaces = 0

    'Initial form width set in UI designer
    Private expandedWidth As Integer

    Private supressedWidth As Integer

    'Form events
    Event faceRecognized(ByVal username As String)

    'Delegates needed for camThread to communicate to UI
    Private Delegate Sub _fRecognized(ByVal text As String)
    Private Delegate Sub SetTextDelegate(ByVal text As String)


    Public Sub New(Optional ByVal state As Integer = 0)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.state = state
    End Sub
    Private Sub FaceRecognition_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'Dipsose objects, and return to inital form
        Try
            thrCameraRunning = False
            thrCamera.Abort()
        Catch ex As Exception
        End Try

        stopCamera()
        Globals.faceRecognizer = Nothing
        MainUI.Show()

    End Sub


    Private Sub FaceRecognition_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Get face data
        dbAdapter.openConnection()
        Dim reader As SqlCeDataReader = dbAdapter.GetData("SELECT * FROM faces ")

        While reader.Read()
            Dim face As Bitmap = New Bitmap(New MemoryStream(CType(reader("face_sample"), Byte())))
            tempDBData(idx) = New Object() {reader("id"), reader("username"), face, reader("user_id")}
            idx += 1
        End While

        reader = dbAdapter.GetData("SELECT user_id FROM faces GROUP BY user_id")
        While reader.Read()
            differentFaces += 1
        End While

        reader.Close()
        dbAdapter.closeConnection()

        'Save inital needed UI properties
        expandedWidth = Me.Width

        'Set UI properties
        faceBox.Image = Bitmap.FromFile(Path.Combine(Application.StartupPath, "Res/FaceRecognition/user.png"))
        txtUname.Enabled = False
        Me.Width = txtUname.Right + btnPrev.Left + 10

        supressedWidth = Me.Width

        'Initalize needed objects 
        grayBitmap = New Bitmap(facebox1.Width, facebox1.Height)


        'Automated procedures
        Select Case state
            Case 1 'just recognize
                recognizeFace()




        End Select

    End Sub

    Private Sub recognizeFace()
        If (tempDBData.Count = 0) Then
            MsgBox("Nothing in DB")
            Globals.faceRecognizer = Nothing
            Me.Close()
        End If
        If Me.state <> 1 Then ' Thread is dependant on this
            Me.state = 1
        End If
        'Try to train recognizer, set ui and start camera thread
        If trainRecognizer() Then
            btnNew.Enabled = False
            btnNext.Enabled = False
            btnPrev.Enabled = False
            saveBtn.Enabled = False
            btnRecognize.Enabled = False

            Label2.Visible = True
            Label3.Visible = True

            startCamera()
            thrCamera = New Thread(AddressOf faceDetect)
            thrCamera.Start()
            thrCameraRunning = True
        End If

    End Sub

    Private Function trainRecognizer() As Boolean

        If tempDBData.Count = 0 Then
            Return False
        End If
        Dim faceImages(tempDBData.Count - 1) As Image(Of [Structure].Gray, Byte)
        Dim faceLables(tempDBData.Count - 1) As Integer

        For i As Integer = 0 To tempDBData.Count - 1
            Dim img As New Image(Of [Structure].Gray, Byte)(CType(tempDBData(i)(2), Bitmap))
            Dim label As Integer = CType(tempDBData(i)(3), Integer)
            faceImages(i) = img.Resize(100, 100, CvEnum.Inter.Cubic)
            faceLables(i) = label
        Next
        recognizer = New LBPHFaceRecognizer()
        recognizer.Train(faceImages, faceLables)


        Return True

    End Function
    Private Function getUserId(ByVal username As String)
        Dim result As Integer = 0
        Dim sqlCommand As SqlCeCommand
        Dim sqlResult As SqlCeDataReader

        dbAdapter.openConnection()
        sqlCommand = New SqlCeCommand("SELECT TOP(1) * FROM faces WHERE username=@Username")
        sqlCommand.Parameters.Add("@Username", username)
        sqlResult = dbAdapter.GetData(sqlCommand)
        While sqlResult.Read()
            result = sqlResult("user_id")
        End While

        If result = 0 Then
            sqlCommand = New SqlCeCommand("SELECT MAX(user_id) FROM faces")
            sqlResult = dbAdapter.GetData(sqlCommand)
            While sqlResult.Read()

                If TypeOf sqlResult(0) Is DBNull Then
                    result = 1
                Else
                    result = sqlResult(0) + 1
                End If

            End While
        End If

        Return result
    End Function
    Private Function insertFaceToDB(ByVal username As String, ByVal pic As Bitmap)
        dbAdapter.openConnection()
        Dim command As New SqlCeCommand("INSERT INTO faces(username,face_sample,user_id) VALUES(@Username,@Picture,@userId)")

        command.Parameters.Add("@Username", username)
        command.Parameters.Add("@UserId", getUserId(username))
        'Create an Image object.'
        Using picture As Image = pic
            'Create an empty stream in memory.'
            Using stream As New IO.MemoryStream
                'Fill the stream with the binary data from the Image.'
                picture.Save(stream, ImageFormat.Png)
                'Get an array of Bytes from the stream and assign to the parameter.'
                command.Parameters.Add("@Picture", SqlDbType.Image).Value = stream.GetBuffer()
            End Using
        End Using

        MsgBox(dbAdapter.Query(command))



        dbAdapter.closeConnection()
        Return True
    End Function
    Private Sub loadViewToFaceRecognizer(ByVal values() As Object)
        faceBox.Image = CType(tempDBData(idx)(2), Bitmap)
        txtUname.Text = tempDBData(idx)(1)
    End Sub

    Private Sub faceDetect()
        While thrCameraRunning


            Dim imageFrame As Emgu.CV.Image(Of [Structure].Bgr, Byte) = Me.cap.QueryFrame().ToImage(Of [Structure].Bgr, Byte)()
            Dim grayscale As Image(Of [Structure].Gray, Byte) = imageFrame.Convert(Of [Structure].Gray, Byte)()
            grayscale._EqualizeHist()
            grayscale._SmoothGaussian(1)
            Dim comparators() As Rectangle = cc.DetectMultiScale(grayscale, 1.1, 10, New Size(100, 100))
            If comparators.Length > 0 Then
                Dim face As Rectangle = comparators(0)
                Dim bmp As Bitmap = New Bitmap(face.Width, face.Height)
                Dim g As Graphics = Graphics.FromImage(bmp)
                Dim dst_rect As New Rectangle(0, 0, face.Width, face.Height)
                Dim src_rect As New Rectangle(face.Left, face.Top, face.Width, face.Height)
                Using g
                    g.DrawImage(grayscale.ToBitmap(), dst_rect, src_rect, GraphicsUnit.Pixel)
                End Using

                grayBitmap = bmp.Clone()
                imageFrame.Draw(face, New Emgu.CV.Structure.Bgr(Color.BurlyWood), 2)
            Else
                Dim g As Graphics = Graphics.FromImage(grayBitmap)
                Using g
                    g.FillRectangle(Brushes.Black, New Rectangle(0, 0, grayBitmap.Width, grayBitmap.Height))
                End Using

            End If
            facebox1.Image = grayBitmap.Clone()
            faceBox.Image = imageFrame.ToBitmap()
            If state = 1 Then

                Dim res As EigenFaceRecognizer.PredictionResult = recognizer.Predict(New Image(Of [Structure].Gray, Byte)(grayBitmap).Resize(100, 100, CvEnum.Inter.Cubic))
                changeDistance(res.Distance)

                If (res.Distance < 95) Then
                    If matches.ContainsKey(res.Label) And matches.Count = 1 Then
                        matches(res.Label) += 1
                    Else
                        matches.Clear()
                        matches(res.Label) = 1
                    End If
                    If matches(res.Label) > 20 Then
                        For Each i() As Object In tempDBData.Values
                            If Int(i(3)) = res.Label Then
                                changeText(i(1))
                                fRecognized(i(1))
                                thrCameraRunning = False
                            End If

                        Next

                        matches.Clear()
                    End If

                Else
                    changeText("")
                End If




            End If






        End While
    End Sub
    Private Sub fRecognized(ByVal username As String)
        If InvokeRequired Then
            Invoke(New _fRecognized(AddressOf fRecognized), username)
            Return

        End If
        stopCamera()
        RaiseEvent faceRecognized(username)

    End Sub

    Private Sub changeText(ByVal text As String)

        If InvokeRequired Then
            Invoke(New SetTextDelegate(AddressOf changeText), text)
            Return
        End If
        Me.txtUname.Text = text
    End Sub

    Private Sub changeDistance(ByVal text As String)

        If InvokeRequired Then
            Invoke(New SetTextDelegate(AddressOf changeDistance), text)
            Return

        End If
        Me.Label2.Text = Int(text)
    End Sub


    Private Sub startCamera()
        Me.Width = expandedWidth
        cap = New Capture(0)
        cameraStarted = True
    End Sub

    Private Sub stopCamera()
        Me.Width = supressedWidth
        If cameraStarted = True Then
            cap.Stop()
            cap.Dispose()
            cameraStarted = False
        End If

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If tempDBData.Count = 0 Then
            MsgBox("No content in DB")
            Return
        End If
        If (idx > 0) Then
            idx -= 1
            loadViewToFaceRecognizer(tempDBData(idx))
        End If
    End Sub

    Private Sub btnPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        If tempDBData.Count = 0 Then
            MsgBox("No content in DB")
            Return
        End If
        If (idx < tempDBData.Count - 1) Then
            idx += 1
            loadViewToFaceRecognizer(tempDBData(idx))
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        MsgBox("When you see rectangle around face enter your username and can click 'Save face' button. " + vbNewLine + "If you have face in DB please enter same username")
        startCamera()

        btnNext.Enabled = False
        btnPrev.Enabled = False
        btnNew.Enabled = False
        btnRecognize.Enabled = False
        saveBtn.Enabled = True
        txtUname.Enabled = True

        thrCamera = New Thread(AddressOf faceDetect)
        thrCamera.Start()
        thrCameraRunning = True

    End Sub

    Private Sub saveBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveBtn.Click
        If Not String.IsNullOrEmpty(txtUname.Text) Then
            insertFaceToDB(txtUname.Text, grayBitmap)
            tempDBData(tempDBData.Count) = New Object() {0, txtUname.Text, grayBitmap, getUserId(txtUname.Text)}
            thrCameraRunning = False
            stopCamera()

            txtUname.Clear()
            txtUname.Enabled = False
            saveBtn.Enabled = False
            btnPrev.Enabled = True
            btnNext.Enabled = True
            btnNew.Enabled = True
            btnRecognize.Enabled = True
        Else
            MsgBox("Please enter a username")
        End If


    End Sub


    Private Sub btnRecognize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecognize.Click

        recognizeFace()
    End Sub
End Class