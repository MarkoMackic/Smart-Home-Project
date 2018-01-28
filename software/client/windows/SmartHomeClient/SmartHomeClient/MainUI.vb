Imports SmartHomeClient.Globals
Imports System.IO
Imports ZXing
Imports Emgu.CV

Imports System.Data.SqlServerCe
Imports System.Security

Imports System.Threading

Public Class MainUI


    Private testIdx As Integer = 0
    Private timeSent As DateTime
    Private state1 As Integer = 0
    Private state2 As Integer = 0

    Private thr As Thread
    Private sendAgain As Boolean = True
    Private Delegate Sub _addLog(ByVal text As String, ByVal color As Color)
    Private Delegate Sub _changeText(ByVal text As String, ByVal ctl As Control)

    Private Sub MainUI_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        If Not masterCont Is Nothing Then masterCont.Destroy()
        If Not cliManager Is Nothing Then cliManager.Destroy()
        If Not devManager Is Nothing Then devManager.Destroy()


    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'SplashScreen.ShowDialog()
        'Set instance
        mainForm = Me

        'Hardware comport 
        Dim comport As String = ""

        Dim spD As New SerialPortDialog()
        If spD.ShowDialog() = DialogResult.OK Then
            comport = spD.Result
        End If

        'Net connection
        Dim port As Integer, ipaddress As String, username As String, password As String, makeConnection As Boolean
        Dim netD As New ClientConnectionDialog()
        If netD.ShowDialog() = DialogResult.OK Then
            port = netD.Port
            ipaddress = netD.Host
            username = netD.Username
            password = netD.Password
            makeConnection = True
        Else
            port = Nothing
            ipaddress = Nothing
            username = Nothing
            password = Nothing
            makeConnection = False
        End If




        'Initialize all the resources
        If resLoaded = False Then
            hardwareChannel = New HardwareComm(comport, 115200)
            msgHandler = New MessageHandler()
            dbAdapter = New DBDriver("Data Source = DB.sdf")
            masterCont = New MasterController(MasterController.States.Login)
            AddHandler masterCont.LoggedIn, AddressOf continueInitialization
            If makeConnection Then
                'this will instantiate tcpClient for us
                cliManager = New ClientMiddleware.ClientManager(ipaddress, port, username, password)
            End If
        End If

        logInstantiation(Me)

    End Sub

    Private Sub continueInitialization()
        If InvokeRequired Then
            Invoke(Sub() continueInitialization())
            Return
        End If

        devManager = New DeviceManager()
        devManager.addDevice("TLC5940", New Integer() {9, 11, 12, 51, 52}, 5, 1)
        devManager.addDevice("LED GREEN", New Integer() {13}, 2, 2, 1)
        devManager.addDevice("LED PLAVA", New Integer() {13}, 1, 3)

        resLoaded = True
    End Sub


    Public Sub SerialDataRecieved(ByVal msg As String)
        Return
    End Sub
    Private Sub faceRecognized(ByVal username As String)
        MsgBox(username)

        faceRecognizer.Close()
    End Sub

    Public Sub addLog(ByVal text As String, Optional ByVal color As Color = Nothing)

        Try
            If InvokeRequired Then
                Invoke(New _addLog(AddressOf addLog), text, color)
                Return

            End If

            If color = Nothing Or color = color.Black Then
                color = color.White
            End If

            txtLog.SelectionColor = color
            txtLog.AppendText(text + vbNewLine)
            txtLog.ScrollToCaret()
        Catch ex As ObjectDisposedException
            Debug.WriteLine("Disposed object accessed")

        End Try

    End Sub

    Public Sub ChangeText(ByVal text As String, ByVal ctl As Control)
        Try
            If InvokeRequired Then

                Me.Invoke(New _changeText(AddressOf ChangeText), text, ctl)
                Return
     

            End If
            ctl.Text = text
        Catch ex As ObjectDisposedException
        End Try
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        faceRecognizer = New FaceRecognition()
        faceRecognizer.Show()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim dev As Device = devManager.GetDevice(0)
     
        dev.ChangeState(New Object() {15, state1})

        If state1 = 0 Then
            state1 = 4095
        Else
            state1 = 0
        End If

    End Sub

    Private Sub Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim dev As Device = devManager.GetDevice(0)
        dev.ChangeState(New Object() {0, state2})

        If state2 = 0 Then
            state2 = 4095
        Else
            state2 = 0
        End If

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Dim dev As Device = devManager.GetDeviceById(2)
        MsgBox(dev.ID)
        dev.ChangeState(New Object() {TextBox1.Text})



    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim dev As Device = devManager.GetDevice(2)
        dev.ChangeState(New Object() {TextBox2.Text})
    End Sub



    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Me.Hide()
        devManagerUI = New DeviceManagerUI()
        devManagerUI.Show()
    End Sub
End Class
