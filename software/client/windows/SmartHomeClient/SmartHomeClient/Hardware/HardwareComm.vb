Imports System.IO.Ports
Imports System.Reflection
Imports SmartHomeClient.Globals
Imports SmartHomeClient.CommonFunctions
Imports System.Threading
Imports System.Threading.Tasks

Public Class HardwareComm
    Private comPort As SerialPort
    Private msg As String
    Public waiters As LinkedList(Of Object)
    Dim currentWaiter As Object = Nothing

    Private recievingThr As Thread
    Private recievingThrRunning As Boolean = False

    Public lockObject As New Object

    Public Sub New(ByVal comName As String, ByVal baudRate As Integer)
        comPort = New SerialPort(comName, baudRate)
        comPort.DtrEnable = True
        comPort.Parity = Parity.None
        comPort.StopBits = 1

        waiters = New LinkedList(Of Object)
        msg = ""
        AddHandler comPort.DataReceived, AddressOf dataRecieved
        logInstantiation(Me)
    End Sub

    Public Function startCommunication()
        If comPort.IsOpen = False Then
            Try
                comPort.Open()
                If Me.comPort.IsOpen Then
                    recievingThrRunning = True
                    recievingThr = New Thread(AddressOf processRecievedData)
                    recievingThr.IsBackground = True
                    recievingThr.Start()
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try

        Else
            Return False
        End If

    End Function
    Public Function stopCommunication()
        comPort.Close()
        recievingThrRunning = False
        If (recievingThr.ThreadState = ThreadState.Running) Then
            recievingThr.Abort()
        End If


        If Me.comPort.IsOpen Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function sendData(ByVal cmd As String,
                             Optional ByVal wantedState As String = "SOMETHING_WENT_WRONG",
                             Optional ByVal waitForData As Boolean = False,
                             Optional ByVal caller As Object = Nothing,
                             Optional ByVal callback As String = "SerialDataRecieved",
                             Optional ByVal initialDevId As Integer = -1) As Boolean

        If comPort.IsOpen() Then
            waiters.AddLast(New Object() {cmd, wantedState, waitForData, caller, callback, Now, initialDevId})
            Return True
        Else
            Return False
        End If
    End Function


    Private Sub processRecievedData()
  
        While recievingThrRunning

            If waiters.Count > 0 And currentWaiter Is Nothing Then
                Try
                    currentWaiter = waiters.First.Value
                    waiters.RemoveFirst()
                    '{cmd, waitForData, caller, callback, Now}
                    Dim cmd As String = currentWaiter(0).ToString()

                    comPort.Write(cmd + Chr(10))
                Catch ex As NullReferenceException
                End Try


            End If

            If Not currentWaiter Is Nothing Then

                Dim cmd As String = CType(currentWaiter(0), String)
                Dim wantedState As String = currentWaiter(1)
                Dim waitForData As Boolean = CType(currentWaiter(2), Boolean)
                Dim waiter As Object = currentWaiter(3)
                Dim callback As String = CType(currentWaiter(4), String)
                Dim sent As DateTime = currentWaiter(5)
                Dim intialDevId As Integer = Int(currentWaiter(6))
                If waitForData = True And Not waiter Is Nothing Then
                    If msg.Contains(Chr(10)) Then

                        Dim temp() As String = msg.Split(New Char() {Chr(10)})

                        If (Not String.IsNullOrEmpty(temp(0).Trim())) Then

                            msg = msg.Substring(msg.IndexOf(Chr(10)) + 1)



                            Dim callback_msg As String = temp(0).Trim()

                            Dim magicMethod As MethodInfo = waiter.GetType().GetMethod(callback)

                            If Not magicMethod Is Nothing Then
                                Dim paramsCount As Integer = magicMethod.GetParameters().Count
                                Select Case paramsCount
                                    Case 1
                                        magicMethod.Invoke(waiter, New Object() {callback_msg})
                                    Case 2
                                        magicMethod.Invoke(waiter, New Object() {callback_msg, cmd})
                                    Case 4
                                        magicMethod.Invoke(waiter, New Object() {callback_msg, cmd, intialDevId, wantedState})

                                    Case Else
                                        Throw New NotImplementedException("Not implemented")
                                End Select


                            End If

                            currentWaiter = Nothing

                        End If
                    End If

                Else
                    msgHandler.SerialDataRecieved(cmd)
                    currentWaiter = Nothing
                End If





            End If
            Thread.Sleep(2)
        End While

    End Sub
    Private Sub dataRecieved(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)

        Dim tempData As String = comPort.ReadExisting()
        msg &= trimSpaces(tempData)


    End Sub



End Class
