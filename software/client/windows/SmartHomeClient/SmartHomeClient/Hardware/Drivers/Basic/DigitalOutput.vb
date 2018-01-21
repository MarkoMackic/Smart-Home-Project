﻿Imports SmartHomeClient.Globals

Namespace Drivers


    Public Class DigitalOutput
        Inherits Drivers.Driver

        Public Shadows Event StateChanged(ByVal state As String)
        Public StateString As String = "0"

        Dim protocol_operations = New Dictionary(Of Integer, String) From {
            {1, "pm:{0}:{1}"},
            {2, "dw:{0}:{1}"},
            {3, "gds:{0}"}
        }

        Enum CMD
            SETMODE = 1
            SETSTATE = 2
            GETVALUE = 3
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



            masterCont.SendData(String.Format(protocol_operations(CMD.SETMODE), pin, 1), True, Me, "PinModeCallback")
            If dev.Master Is Nothing Then
                masterCont.SendData(String.Format(protocol_operations(CMD.GETVALUE), pin), True, Me, "InitalStateCallback")
            End If


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

        Public Overrides Function StateStr()
            Return Me.StateString
        End Function


        Public Overrides Function ChangeState(ByVal state() As Object, Optional ByVal slave As Device = Nothing)

            Dim isHandled As Boolean

            isHandled = ValueHandler(state)
            If Not isHandled Then
                'daisy chain this block for additional handlers
            End If

            Return isHandled

        End Function

        'State change handlers
        Private Function ValueHandler(ByVal state() As Object, Optional ByVal validate As Boolean = False)
            If state.Length = 1 Then
                Dim s As Integer = CType(state(0), Integer)
                If s = 1 Or s = 0 Then
                    If Me.StateString <> s.ToString() Then
                        Me.StateString = s.ToString()
                        RaiseEvent StateChanged(Me.StateString)
                        Driver.driverLog(String.Format("Driver ({0}) state change : {1} ", device.Name, StateString))
                    End If
                    If validate Then
                        Return True
                    End If
                    If device.Master Is Nothing Then 'means device executes on chip
                        masterCont.SendData(String.Format(protocol_operations(CMD.SETSTATE), pin, s), True, Me, "ChangeStateCallback")
                        Return True
                    Else
                        Return device.Master.ChangeState(New Object() {pin, s}, device)
                    End If


                Else
                    Return False
                End If
            Else
                Return False
            End If

            Return False

        End Function


        'Serial callbacks
        Public Sub PinModeCallback(ByVal data As String, ByVal cmd As String)
            If data = "OK" Then
                Driver.driverLog(String.Format("Driver for device ( {0} ) is normally initialized", device.Name))
            Else
                Driver.driverLog(String.Format("Driver for device ( {0} ) is initialized, but device is already setup on hardware", device.Name))
            End If
            IsInitalized = True
        End Sub

        Public Sub InitalStateCallback(ByVal data As String, ByVal cmd As String)
            Dim isHandled As Boolean

            isHandled = ValueHandler(New Object() {data}, True)
            'you can daisychain here too
            If Not isHandled Then
                Driver.driverLog(String.Format("Driver ( {0} ) failed inital state set", device.Name))
            End If


        End Sub

        Public Overrides Sub SerialDataRecieved(ByVal data As String, ByVal cmd As String)
            Driver.driverLog(String.Format("{0}_driver : {1}", device.Name, data))
        End Sub

        Public Overrides Sub ChangeStateCallback(ByVal data As String, ByVal cmd As String)
            If data <> "OK" Then
                Driver.driverLog(String.Format("{0}_driver : Didn't change device state", device.Name))
            Else

            End If
        End Sub
    End Class




End Namespace