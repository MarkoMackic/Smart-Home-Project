Imports SmartHomeClient.Globals
Imports System.IO
Imports ZXing
Imports Emgu.CV

Imports System.Data.SqlServerCe



Public Class MainUI


    Private testIdx As Integer = 0
    Private timeSent As DateTime
    Private state1 As Integer = 0

    Private Delegate Sub _addLog(ByVal text As String)

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mainForm = Me

        Dim comport As String = ""

        Dim spD As New SerialPortDialog()
        If spD.ShowDialog() = DialogResult.OK Then
            comport = spD.Result
        End If


        'Initialize all the resources
        If resLoaded = False Then
            hardwareChannel = New HardwareComm(comport, 115200)
            msgHandler = New MessageHandler()
            dbAdapter = New DBDriver("Data Source = DB.sdf")
            masterCont = New MasterController(MasterController.States.Login)
            AddHandler masterCont.LoggedIn, AddressOf continueInitialization
       
        End If
 
        logInstantiation(Me)

    End Sub

    Private Sub continueInitialization()

        devManager = New DeviceManager()
        devManager.addDevice("Marko", New Integer() {13}, 1)

        resLoaded = True
    End Sub


    Public Sub SerialDataRecieved(ByVal msg As String)
        Return
    End Sub
    Private Sub faceRecognized(ByVal username As String)
        MsgBox(username)
      
        faceRecognizer.Close()
    End Sub

    Public Sub addLog(ByVal text As String)
        Try
            If InvokeRequired Then
                Invoke(New _addLog(AddressOf addLog), text)
                Return

            End If
            txtLog.AppendText(text + vbNewLine)
        Catch ex As ObjectDisposedException
            Debug.WriteLine("Disposed object accessed")
        End Try

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        faceRecognizer = New FaceRecognition()
        faceRecognizer.Show()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim dev As Device = devManager.GetDevice(0)
        dev.UpdateState(state1)

        If state1 = 0 Then
            state1 = 1
        Else
            state1 = 0
        End If

    End Sub
End Class
