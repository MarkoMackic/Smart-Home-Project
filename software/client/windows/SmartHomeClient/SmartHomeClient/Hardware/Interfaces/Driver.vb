Imports SmartHomeClient.Globals
Namespace Drivers
    Public Class Driver
        Implements IDriver

        Public IsInitalized As Boolean
        Public Version As Integer = 0
        Private _acceptStateType As String

        'needed for loading correct driver
        Public Shared Function supportsType(ByVal type As Integer) As Boolean
            'never load generic class
            Return False
        End Function

        Public Shared Function supportsChild(ByVal type As Integer) As Boolean
            'never load generic class
            Return False
        End Function

        Public Shared Sub driverLog(ByVal msg As String)
            mainForm.addLog(msg, Color.Yellow)
        End Sub

        Public Overridable Function AcceptStateType() As String Implements IDriver.AcceptStateType
            Return Me._acceptStateType
        End Function

        Public Overridable Function StateStr(Optional ByVal state As String = Nothing) Implements IDriver.StateStr
            Return ""
        End Function

     
        Public Overridable Function GetState(ByVal state() As Object, Optional ByVal slave As Device = Nothing, Optional ByVal initalDevId As Integer = -1) Implements IDriver.UpdateState
            Return 0
        End Function

        Public Overridable Function ChangeState(ByVal state() As Object, Optional ByVal slave As Device = Nothing, Optional ByVal initalDevId As Integer = -1) Implements IDriver.ChangeState
            Return 0
        End Function

    End Class

End Namespace
