Namespace Drivers
    Public Class Driver
    
        'needed for loading correct driver
        Public Shared Function supportsType(ByVal type As Integer) As Boolean
            'never load generic class
            Return False
        End Function




    End Class

End Namespace
