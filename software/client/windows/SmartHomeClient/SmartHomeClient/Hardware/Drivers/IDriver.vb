Namespace Drivers
    Public Interface IDriver


        Sub SerialDataRecieved(ByVal data As String, ByVal cmd As String)
        Function ChangeState(ByVal state As String)
        Function UpdateState(ByVal state As String)

        'Serial callbacks
        Sub ChangeStateCallback(ByVal data As String, ByVal cmd As String)

    End Interface

End Namespace