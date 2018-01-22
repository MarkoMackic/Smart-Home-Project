Imports SmartHomeClient.Globals
Imports System.Threading

Public Class MasterController
    'Abstract functions for hardwareChannel ( serial port ) 
    '
    '
    '

    Public Enum States
        Login = 1
        NormOp = 2
    End Enum

    Public state As Integer
    Public IsConnected As Boolean = False
    Public IsLoggedIn As Boolean = False

    Private GPSResetEvent As ManualResetEvent = New ManualResetEvent(True)
    Private GPSThreadRunning As Boolean = False
    Private GPSTimeout As Integer = 400
    Private GPSByteCount As Integer = 0

    Private HardwareAverageSpeed As Double = 0
    Private UpdateUIState As DateTime

    Public digitalPins, analogPins As String

    Private timeSent As DateTime


    Public Event LoggedIn()


    Sub New(Optional ByVal st As Integer = 1)
        logInstantiation(Me)
        If hardwareChannel.startCommunication() Then
            IsConnected = True
            addLog("Connected to device .")
        End If
        hardwareChannel.sendData("lo")
        Me.state = st
        Select Case state
            Case States.Login
                DeviceLogin(authmessage)
        End Select

        UpdateUIState = Now
    End Sub

    Public Sub DeviceLogin(ByVal authmessage As String)
        Me.state = States.Login
        hardwareChannel.sendData(authmessage, True, Me)
    End Sub

    Public Sub SendData(ByVal data As String, Optional ByVal waitForData As Boolean = False, Optional ByVal caller As Object = Nothing, Optional ByVal callback As String = "SerialDataRecieved")
        'Interface to hardwareChannel 

        If IsConnected And IsLoggedIn Then
            addLog("Sending data to controller : " + data)
            hardwareChannel.sendData(data, waitForData, caller, callback)
        End If
    End Sub

    Public Sub GetPinStates()
        While GPSThreadRunning


            timeSent = Now
            hardwareChannel.sendData("pds", True, Me, "pinStateRecv")
            hardwareChannel.sendData("pas", True, Me, "pinStateRecv")
            If Not GPSResetEvent.WaitOne(GPSTimeout) Then
                addLog("Recieving data timeout")
                hardwareChannel.CleanUpTimedOutWaiters()
            Else
                GPSResetEvent.Reset()
            End If
        End While
    End Sub

    Private Sub LoginDataArrived(ByRef data As String)

        Select Case data
            Case "OK"
                state = States.NormOp
                IsLoggedIn = True
                addLog("Logged in to device")
                'Raise event, so procedures can go on

                'Start listening for pin states
                GPSThreadRunning = True
                Dim thr As New Threading.Thread(AddressOf GetPinStates)
                thr.IsBackground = True
                thr.Start()


                RaiseEvent LoggedIn()
            Case "ERROR"
                addLog("Authentication with hardware failed")
                Dim res As MsgBoxResult = MsgBox(String.Format("Authentication with the hardware failed, do you want to try again? ({0})", data), MsgBoxStyle.YesNo, "Authentication failed")
                If res = MsgBoxResult.Yes Then
                    DeviceLogin(InputBox("Enter your device password", "Pass"))
                Else
                    closeApplication()
                End If
            Case Else
                MsgBox(ErrorDescriptors.DEV_RESP_ERR)
                closeApplication()
        End Select

    End Sub
    Public Sub SerialDataRecieved(ByVal data As String)
        Select Case state
            Case States.Login
                LoginDataArrived(data)
            Case States.NormOp
                'this will be unused most the time
                Debug.Write(data)

        End Select
    End Sub


    'Serial message handlers...

    Public Sub pinStateRecv(ByVal msg As String)
        If msg.StartsWith("<ds>") Then
            digitalPins = msg
            GPSByteCount += msg.Length
        ElseIf msg.StartsWith("<an>") Then
            analogPins = msg
            GPSByteCount += msg.Length
            Dim GPSRespTime As Double = Now.Subtract(timeSent).TotalSeconds
            'MsgBox(GPSRespTime)
            'MsgBox(GPSByteCount)
            HardwareAverageSpeed = (HardwareAverageSpeed + GPSByteCount / GPSRespTime) / 2
            GPSByteCount = 0
            If Now.Subtract(UpdateUIState).TotalMilliseconds > 200 Then
                UpdateUIState = Now
                mainForm.ChangeText(Int(HardwareAverageSpeed) & " bytes/s", mainForm.lblHardwareSpeed)
            End If


            GPSResetEvent.Set()

            End If

    End Sub

    Public Sub Destroy()
        GPSThreadRunning = False
        hardwareChannel.stopCommunication()
    End Sub
    Private Sub addLog(ByVal data As String)
        mainForm.addLog(data, Color.Green)
    End Sub

End Class
