Namespace Drivers
    Public Class Driver
        Implements IDriver
        'needed for loading correct driver
        Public Shared Function supportsType(ByVal type As Integer) As Boolean
            'never load generic class
            Return False
        End Function
 

        Public Overridable Sub SerialDataRecieved(ByVal data As String, ByVal cmd As String) Implements IDriver.SerialDataRecieved

        End Sub

        Public Overridable Sub ChangeStateCallback(ByVal data As String, ByVal cmd As String) Implements IDriver.ChangeStateCallback

        End Sub
        Public Overridable Function UpdateState(ByVal state As String) Implements IDriver.UpdateState
            Return 0
        End Function
        Public Overridable Function ChangeState(ByVal state As String) Implements IDriver.ChangeState
            Return 0
        End Function

    End Class

End Namespace
