Imports SmartHomeClient.Globals

Namespace Drivers


    Public Class TLC5940
        Inherits Drivers.Driver


        Shared supportedChildren As New List(Of Integer) From {1, 2}

        Private _acceptStateType = "PIN_VALUE"

        Public Shadows Event StateChanged(ByVal state As String)

        Private StateString As String = "Abstract"
        Dim protocol_operations = New Dictionary(Of Integer, String) From {
            {1, "tl59i:{0}"},
            {2, "tl59s:{0}:{1}"},
            {3, "tl59c"},
            {4, "tl59u"}
        }

        Enum CMD
            INIT = 1
            SETSTATE = 2
            CLEAR = 3
            UPDATE = 4
        End Enum

        Private device As Device
        Private devicePins As List(Of Integer)
        'devicePins(0) -> OC2B (Mega pin 9) -> GSCLK (TLC pin 18) 
        'devicePins(1) -> OC1A (Mega pin 11) -> XLAT (TLC pin 24)
        'devicePins(2) -> OC1B (Mega pin 12) -> BLANK (TLC pin 23)
        'devicePins(3) -> MOSI (Mega pin 51) -> SIN (TLC pin 26) 
        'devicePins(4) -> SCK (Mega pin 52) -> SCLK (TLC pin 25)

        'Static functions
        Public Shared Shadows Function supportsType(ByVal type As Integer) As Boolean
            If type = 5 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Shadows Function supportsChild(ByVal type As Integer) As Boolean
            Return supportedChildren.Contains(type)
        End Function


        Public Overrides Function AcceptStateType() As String
            Return Me._acceptStateType
        End Function


        Public Sub New(ByVal dev As Device)
            Dim pins As List(Of Integer) = dev.Pins

            If pins.Count <> 5 Then
                Throw New DriverCreationException("There should be 5 pins")
            End If
            checkPins(pins)
            devicePins = pins
            device = dev
            masterCont.SendData(String.Format(protocol_operations(CMD.INIT), 1), , True, Me, "InitReport")
        End Sub

        Private Function checkPins(ByVal pins As List(Of Integer))
            'depends on the board.. MEGA,UNO,Due etc, Have to implement static board info..
            'Throws execption that gets retrown to device that tried instantiation
            Return True
        End Function

        Public Overrides Function StateStr(Optional ByVal state As String = Nothing)
            Return Me.StateString
        End Function

        Public Overrides Function ChangeState(ByVal state() As Object, Optional ByVal slave As Device = Nothing, Optional ByVal initialDevId As Integer = -1)

            Dim isHandled As Boolean = False

            isHandled = PinValueHandler(state, slave, initialDevId)

            'this will be call to second one,
            'but this is just an example, daisy chain handlers here like this

            If Not isHandled Then
                'isHandled = SomeHandlerFunction(state,slave)
            End If



            Return isHandled
        End Function

        'Serial data handlers 

        Public Sub SerialDataRecieved(ByVal data As String, ByVal cmd As String)
            Driver.driverLog(String.Format("{0}_driver : {1}", device.Name, data))
        End Sub


        Public Sub InitReport(ByVal data As String, ByVal cmd As String)
            If data = "OK" Then
                Driver.driverLog(String.Format("Driver for device ( {0} ) is normally initialized", device.Name))
            Else
                Driver.driverLog(String.Format("Driver for device ( {0} ) is initialized, but device is already setup on hardware", device.Name))
            End If
            IsInitalized = True
        End Sub

        Public Sub ChangeStateCallback(ByVal data As String, ByVal cmd As String, ByVal initialSlaveId As Integer, ByVal wantedState As String)
  
            If initialSlaveId <> -1 Then
                Dim dev As Device = devManager.GetDeviceById(initialSlaveId)
                If Not dev Is Nothing Then
                    If data.Trim() <> "OK" Then
                        Driver.driverLog(device.Name & " : Didn't change device state")
                    Else
                        dev.StateChangeCallback(wantedState)
                    End If
                End If

            End If
        End Sub


        'State change handlers
        ' pin, state
        Function PinValueHandler(ByVal state() As Object, ByVal slave As Device, ByVal initialDevId As Integer)
            If state.Length = 2 And (state.All(Function(x) TypeOf x Is Integer Or TypeOf x Is String)) Then
                Dim pin, value, real_value As Integer
                If Integer.TryParse(state(0), pin) And Integer.TryParse(state(1), value) Then

                    If Not slave Is Nothing Then
                        Select Case slave.Type
                            Case 1
                                real_value = value * 4095
                            Case 2
                                real_value = Int(Map(value, 0, 255, 0, 4095))

                        End Select
                    Else
                        real_value = value
                    End If

                    If real_value > -1 And real_value < 4096 And pin > -1 And pin < 16 Then
                        If device.Master Is Nothing Then
                            If slave Is Nothing Then
                                masterCont.SendData(String.Format(protocol_operations(CMD.SETSTATE), pin, real_value), Me.StateStr(), True, Me, "ChangeStateCallback")
                            Else
                                masterCont.SendData(String.Format(protocol_operations(CMD.SETSTATE), pin, real_value), value.ToString(), True, Me, "ChangeStateCallback", initialDevId)
                            End If

                            masterCont.SendData(protocol_operations(CMD.UPDATE), , True, Me)
                            Return True
                        Else
                            Return False
                        End If

                    End If
                Else
                    Return False
                End If
            End If
            Return False
        End Function
        'dictonary , pinNumber -> value
    End Class
End Namespace