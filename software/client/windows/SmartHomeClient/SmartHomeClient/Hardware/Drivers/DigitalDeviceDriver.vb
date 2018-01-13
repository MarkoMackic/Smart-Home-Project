Imports SmartHomeClient.Globals
Imports SmartHomeClient.ProtocolSpecification

Namespace Drivers


    Public Class DigitalDeviceDriver
        Inherits Drivers.Driver

        Private pin As Integer
        Private deviceName As String

        Public Sub New(ByVal pins As List(Of Integer), ByVal devName As String)
            If pins.Count <> 1 Then
                Throw New DriverCreationException("There should be only one pin")
            End If
            pin = pins(0)
            deviceName = devName
            masterCont.SendData(String.Format(setPinMode, pin, 1), True, Me)
        End Sub


        Public Shared Shadows Function supportsType(ByVal type As Integer) As Boolean
            If type = 1 Then
                Return True
            End If
            Return False
        End Function

        Public Overrides Sub SerialDataRecieved(ByVal data As String, ByVal cmd As String)
            mainForm.addLog(deviceName + "_driver : " + data)

        End Sub

        Public Overrides Sub ChangeStateCallback(ByVal data As String, ByVal cmd As String)
            If data <> "OK" Then
                mainForm.addLog("Didn't change device state")
            Else

            End If
        End Sub
        Public Overrides Function ChangeState(ByVal state As String)
            If state = "1" Or state = "0" Then
                masterCont.SendData(String.Format(writeDigitalState, pin, state), True, Me, "ChangeStateCallback")
                Return True
            Else
                Return False
            End If
        End Function

    End Class
End Namespace
