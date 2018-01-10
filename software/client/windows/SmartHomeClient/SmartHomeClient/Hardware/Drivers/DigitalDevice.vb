Imports SmartHomeClient.Globals

Namespace Drivers


    Public Class DigitalDevice
        Inherits Drivers.Driver

        Private parentDevice As Device

        Public Sub New(ByVal parentDev As Device)
            parentDevice = parentDev

        End Sub


        Public Shared Shadows Function supportsType(ByVal type As Integer) As Boolean
            Return True
        End Function

     
    End Class
End Namespace
