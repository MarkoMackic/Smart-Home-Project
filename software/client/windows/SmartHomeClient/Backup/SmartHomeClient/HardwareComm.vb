Imports System.IO.Ports
Imports System.Reflection
Imports SmartHomeClient.Globals

Public Class HardwareComm
    Private comPort As SerialPort
    Private waiter As Object = Nothing
    Private msg As String

    Public Sub New(ByVal comName As String, ByVal baudRate As Integer)
        comPort = New SerialPort(comName, baudRate)
        msg = ""
        AddHandler comPort.DataReceived, AddressOf dataRecieved
    End Sub

    Public Function startCommunication()
        comPort.Open()
        If (Me.comPort.IsOpen()) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function sendData(ByVal cmd As String, Optional ByVal waitForData As Boolean = False, Optional ByVal caller As Object = Nothing)
        comPort.Write(cmd + vbNewLine)
        If (waitForData) Then
            If (caller IsNot Nothing) Then
                waiter = caller
            Else
                Throw New Exception("This is not allowed")
            End If
        End If

    End Function
    Private Function dataRecieved(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)


        If (waiter IsNot Nothing) Then
            Dim magicMethod As MethodInfo = waiter.GetType().GetMethod("serialDataRecieved")
            Dim magicValue As Object = magicMethod.Invoke(waiter, New Object() {comPort.ReadTo(vbNewLine)})
        Else
            msgHandler.handleDeviceMessage(comPort.ReadTo(vbNewLine))
        End If


    End Function

End Class
