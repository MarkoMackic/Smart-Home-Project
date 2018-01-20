Namespace Drivers
    Public Interface IDriver



        Event StateChanged(ByVal state As String)

        Function StateStr()


        Function ChangeState(ByVal state() As Object, Optional ByVal slave As Device = Nothing)
        Function UpdateState(ByVal state() As Object, Optional ByVal slave As Device = Nothing)

        'Serial callbacks
        Sub ChangeStateCallback(ByVal data As String, ByVal cmd As String)
        Sub SerialDataRecieved(ByVal data As String, ByVal cmd As String)

    End Interface

End Namespace