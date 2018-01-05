Imports SmartHomeClient.Globals
Imports System.IO
Imports ZXing
Imports Emgu.CV

Imports System.Data.SqlServerCe



Public Class Form1
  

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialize all the resources
        hardwareChannel = New HardwareComm("COM5", 115200)
        msgHandler = New MessageHandler()
        dbAdapter = New DBDriver("Data Source = DB.sdf")


   


        'QrCode()
        'PictureBox1.Image = Image.FromFile(Path.Combine(Application.StartupPath, "Res/QRtest/barcode.jpg"))
        'Dim reader As New barcodereader()
        'Try
        '    Dim result As Result = reader.Decode(PictureBox1.Image)
        '    Dim decodedstring As String = result.Text
        '    MsgBox(decodedstring)
        'Catch ex As Exception

        'End Try
        ' hardwareChannel.startCommunication()
        'hardwareChannel.sendData("authmessage", True, Me)
        'hardwareChannel.sendData("pas", True, Me)
      
    End Sub


    Private Function doDeviceAuth(ByVal authStr As String)
        hardwareChannel.sendData("authmessage", True, Me)
        Return 0
    End Function
    Public Sub serialDataRecieved(ByVal msg As String)
        MsgBox(msg)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Me.Hide()
        Try
            faceRecognizer.Show()
        Catch exc As Exception
            faceRecognizer = New FaceRecognition()
            faceRecognizer.Show()
        End Try

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
        Try
            faceRecognizer.state = 1
        Catch exc As Exception
            faceRecognizer = New FaceRecognition()
            faceRecognizer.state = 1
        End Try
        AddHandler faceRecognizer.faceRecognized, AddressOf faceRecognized
        faceRecognizer.Show()

    End Sub
    Private Sub faceRecognized(ByVal username As String)
        MsgBox(username)
        faceRecognizer.Close()
    End Sub
End Class
