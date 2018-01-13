Public Class Timeout
    Private startTime As Date
    Private timeoutTime As Integer
    Public Sub New(ByVal milliseconds As Integer)
        startTime = Now
        timeoutTime = milliseconds
    End Sub
    Public Sub UpdateTime()
        startTime = Now
    End Sub
    Public Function IsElapsed() As Boolean
        If Now.Subtract(startTime).TotalMilliseconds > timeoutTime Then
            Return True
        End If
        Return False
    End Function
End Class
