Imports SmartHomeClient.Globals
Imports SmartHomeClient.ProtocolSpecification

Namespace Drivers


    Public Class DigitalDeviceDriver
        Inherits Drivers.Driver
        Implements Drivers.IDriver
        Private pin As Integer


        Public Sub New(ByVal pins As List(Of Integer))
            If pins.Count <> 1 Then
                Throw New DriverCreationException("There should be only one pin")
            End If
            pin = pins(0)

            masterCont.SendData(String.Format(setPinMode, pin, 1), True, Me)
        End Sub


        Public Shared Shadows Function supportsType(ByVal type As Integer) As Boolean
            If type = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Sub SerialDataRecieved(ByVal data As String, ByVal cmd As String) Implements IDriver.SerialDataRecieved
            MsgBox("Driver1 : " + data + ":" + cmd)
        End Sub

        Public Sub UpdateStateCallback(ByVal data As String, ByVal cmd As String) Implements IDriver.UpdateStateCallback
            If data <> "OK" Then
                mainForm.addLog("Didn't change device state")
            End If
        End Sub
        Public Function UpdateState(ByVal state As String) Implements IDriver.UpdateState
            If state = "1" Or state = "0" Then
                masterCont.SendData(String.Format(writeDigitalState, pin, state), True, Me, "UpdateStateCallback")
                Return True
            Else
                Return False
            End If
        End Function

    End Class
End Namespace
