Namespace Drivers
    Public Interface IDriver




        Function StateStr(Optional ByVal state As String = Nothing)

        Function AcceptStateType() As String

        Function ChangeState(ByVal state() As Object, Optional ByVal slave As Device = Nothing, Optional ByVal initalDevId As Integer = -1)
        Function UpdateState(ByVal state() As Object, Optional ByVal slave As Device = Nothing, Optional ByVal initalDevId As Integer = -1)



    End Interface

End Namespace