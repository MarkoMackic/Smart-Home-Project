Namespace Drivers
    Public Interface IDriver


        Sub SerialDataRecieved(ByVal data As String, ByVal cmd As String)
        Function UpdateState(ByVal state As String)
        Sub UpdateStateCallback(ByVal data As String, ByVal cmd As String)

    End Interface

End Namespace