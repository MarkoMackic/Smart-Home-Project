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
    Private waitersTimeout As Integer = 100 ' milliseconds
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

    Public Function sendData(ByVal cmd As String, Optional ByVal waitForData As Boolean = False, Optional ByVal caller As Object = Nothing, Optional ByVal callback As String = "SerialDataRecieved") As Boolean

        If comPort.IsOpen() Then

            If waitForData = True Then

                If (caller IsNot Nothing) Then
                    SyncLock lockObject
                        waiters.AddLast(New Object() {caller, callback, cmd, Now})
                    End SyncLock

                Else
                    Throw New Exception("This is not allowed")
                End If

            End If
            comPort.Write(cmd + Chr(10))
            Return True

        Else
            Return False
        End If
    End Function
    Public Sub CleanUpTimedOutWaiters()

        Dim Count As Integer = 0
        SyncLock lockObject
            For i As Integer = waiters.Count - 1 To 0 Step -1

                Dim d As DateTime = waiters.ElementAt(i)(3)
                If Now.Subtract(d).TotalMilliseconds > waitersTimeout Then
                    waiters.Remove(waiters.ElementAt(i))
                End If

            Next
        End SyncLock


    End Sub

    Private Sub processRecievedData()
        'If Not String.IsNullOrEmpty(trimSpaces(msg)) Then
        '    MsgBox(msg)
        '    MsgBox(msg.Contains(Chr(10)))
        'End If
        While recievingThrRunning
         

            If msg.Contains(Chr(10)) Then
                Dim temp() As String = msg.Split(New Char() {Chr(10)})
                Dim idx As Integer = 0
                For Each i As String In temp

                    If (Not String.IsNullOrEmpty(i.Trim())) Then
                        ' MsgBox(i) 'debug the message

                        msg = msg.Substring(msg.IndexOf(Chr(10)) + 1)

                        If waiters.Count > 0 Then


                            Dim skipString As Boolean = False
                            Dim tempDeq() As Object = waiters.First().Value
                            waiters.RemoveFirst()
                            Dim waiter As Object = tempDeq(0)
                            Dim callback As String = CType(tempDeq(1), String)
                            Dim cmd As String = CType(tempDeq(2), String)
                            Dim sent As DateTime = tempDeq(3)
                            If Now.Subtract(sent).TotalMilliseconds > 100 Then
                                skipString = True
                            End If
                            If (Not skipString) Then


                                'MsgBox(tempDeq(1))
                                Dim callback_msg As String = i.Trim()
                                Dim task As New Task(Sub()

                                                         Dim magicMethod As MethodInfo = waiter.GetType().GetMethod(callback)

                                                         If Not magicMethod Is Nothing Then
                                                             If (magicMethod.GetParameters().Count > 1) Then
                                                                 magicMethod.Invoke(waiter, New Object() {callback_msg, cmd})
                                                             Else
                                                                 magicMethod.Invoke(waiter, New Object() {callback_msg})
                                                             End If

                                                         End If

                                                     End Sub)

                                task.Start()


                            End If

                        Else
                            msgHandler.SerialDataRecieved(i.Trim())
                        End If

                        Exit For
                    End If
                    idx += 1
                Next


            End If
            Thread.Sleep(5)
        End While

    End Sub
    Private Sub dataRecieved(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)
        Dim tempData As String = comPort.ReadExisting()

        msg += trimSpaces(tempData)

    End Sub



End Class
