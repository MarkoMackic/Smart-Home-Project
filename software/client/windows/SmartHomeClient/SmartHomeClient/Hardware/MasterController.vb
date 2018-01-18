Imports SmartHomeClient.Globals
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

    Private GPSThread As Boolean = False
    Private GPSNextRun As Boolean = False
    Private GPSCount As Integer = 0
    Private GPSTimeout As Timeout
    Public GPSTime As Integer
    Public digitalPins, pwmPins, analogPins As String

    Private timeSent As DateTime


    Public Event LoggedIn()


    Sub New(Optional ByVal st As Integer = 1)
        logInstantiation(Me)
        If hardwareChannel.startCommunication() Then
            IsConnected = True
            mainForm.addLog("Connected to device .")
        End If
        hardwareChannel.sendData("lo")
        Me.state = st
        Select Case state
            Case States.Login
                DeviceLogin(authmessage)
        End Select
        GPSTimeout = New Timeout(400) 'give it 300 milliseconds to get to itself

    End Sub

    Public Sub DeviceLogin(ByVal authmessage As String)
        Me.state = States.Login
        hardwareChannel.sendData(authmessage, True, Me)
    End Sub

    Public Sub SendData(ByVal data As String, Optional ByVal waitForData As Boolean = False, Optional ByVal caller As Object = Nothing, Optional ByVal callback As String = "SerialDataRecieved")
        'Interface to hardwareChannel 

        If IsConnected And IsLoggedIn Then
            mainForm.addLog("Sending data to controller : " + data)
            hardwareChannel.sendData(data, waitForData, caller, callback)
        End If
    End Sub

    Public Sub GetPinStates()
        While GPSThread
  
            If GPSNextRun Then
                GPSTimeout.UpdateTime()
                timeSent = Now
                hardwareChannel.sendData("pds", True, Me, "pinStateRecv")
                hardwareChannel.sendData("pps", True, Me, "pinStateRecv")
                hardwareChannel.sendData("pas", True, Me, "pinStateRecv")
                GPSNextRun = False
            Else
                If GPSTimeout.IsElapsed() Then
                    hardwareChannel.CleanUpTimedOutWaiters()
                    GPSCount = 0
                    GPSNextRun = True
                    mainForm.addLog("Error in communication")

                End If
            End If

        End While
    End Sub

    Private Sub LoginDataArrived(ByRef data As String)
        Select Case data
            Case "OK"
                state = States.NormOp
                IsLoggedIn = True
                mainForm.addLog("Logged in to device")
                'Raise event, so procedures can go on
                RaiseEvent LoggedIn()
                'Start listening for pin states
                Dim thr As New Threading.Thread(AddressOf GetPinStates)
                GPSThread = True
                GPSNextRun = True
                thr.IsBackground = True
                thr.Start()


            Case "ERROR"
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
      
        'We wait for 3 messages. And then again.
        GPSCount += 1

        If msg.StartsWith("<pwm>") Then
            pwmPins = msg
        ElseIf msg.StartsWith("<ds>") Then
            digitalPins = msg
        ElseIf msg.StartsWith("<an>") Then
            analogPins = msg
            GPSTime = Now.Subtract(timeSent).TotalMilliseconds
            GPSNextRun = True
            GPSCount = 0
        End If
   
        'Broadcast among Devices
        devManager.broadcastPinStates()

    End Sub
End Class
