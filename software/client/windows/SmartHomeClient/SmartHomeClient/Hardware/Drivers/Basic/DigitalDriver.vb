﻿Imports SmartHomeClient.Globals

Namespace Drivers


    Public Class DigitalDriver
        Inherits Drivers.Driver


        Dim protocol_operations = New Dictionary(Of Integer, String) From {
            {1, "pm:{0}:{1}"},
            {2, "dw:{0}:{1}"}
        }

        Enum CMD
            SETMODE = 1
            SETSTATE = 2
        End Enum

        Private pin As Integer
        Private device As Device

        Public Sub New(ByVal dev As Device)
            Dim pins As List(Of Integer) = dev.Pins
            If pins.Count <> 1 Then
                Throw New DriverCreationException("There should be only one pin")
            End If
            pin = pins(0)
            device = dev
            masterCont.SendData(String.Format(protocol_operations(CMD.SETMODE), pin, 1), True, Me, "InitStateReport")
        End Sub


        Public Shared Shadows Function supportsType(ByVal type As Integer) As Boolean
            If type = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Shadows Function supportsChild(ByVal type As Integer) As Boolean
            Return False
        End Function

        Public Sub InitStateReport(ByVal data As String, ByVal cmd As String)
            If data = "OK" Then
                mainForm.addLog(String.Format("Driver for device ( {0} ) is normally initialized", device.Name))
            Else
                mainForm.addLog(String.Format("Driver for device ( {0} ) is initialized, but device is already setup on hardware", device.Name))
            End If
            IsInitalized = True
        End Sub
        Public Overrides Sub SerialDataRecieved(ByVal data As String, ByVal cmd As String)
            mainForm.addLog(device.Name + "_driver : " + data)

        End Sub

        Public Overrides Sub ChangeStateCallback(ByVal data As String, ByVal cmd As String)
            If data <> "OK" Then
                mainForm.addLog("Didn't change device state")
            Else

            End If
        End Sub
        Public Overrides Function ChangeState(ByVal state() As Object, Optional ByVal slave As Device = Nothing)
            Try
                Dim isHandled As Boolean

                isHandled = ValueHandler(state)
                If Not isHandled Then
                    'daisy chain this block for additional handlers
                End If

                Return isHandled
            Catch ex As Exception
                Return False
            End Try
        End Function

        'State change handlers
        Private Function ValueHandler(ByVal state() As Object)
            If state.Length = 1 Then
                Dim s As Integer = CType(state(0), Integer)
                If s = 1 Or s = 0 Then
                    If device.Master Is Nothing Then

                        masterCont.SendData(String.Format(protocol_operations(CMD.SETSTATE), pin, s), True, Me, "ChangeStateCallback")
                        Return True
                    Else
                        Return device.Master.ChangeState(New Object() {pin, state}, device)
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function
    End Class




End Namespace