Imports System.Reflection

Public Class Device
    'Will hold device information
    'Instantiate a driver for device

    Private isInitalized As Boolean = False
    Private Name As String
    Private Pins As List(Of Integer)
    Private Type As Integer
    Private Driver As Drivers.IDriver

    Private Function InstantiateDriver(ByVal type As Integer)
        Dim resultDriver As Object = Nothing
        Dim types = From t In Assembly.GetExecutingAssembly().GetTypes()
                     Where t.IsClass And t.Namespace = "SmartHomeClient.Drivers"
                     Select t


        For Each drv_type As Type In types
            If drv_type.GetMethod("supportsType").Invoke(Nothing, New Object() {type}) Then
                resultDriver = Activator.CreateInstance(drv_type, New Object() {Pins})

                Exit For
            End If
        Next
        Return resultDriver
    End Function

    Public Sub New(ByVal devName As String, ByVal devPins() As Integer, ByVal devType As Integer)

        Name = devName
        Pins = devPins.ToList()
        Type = devType
        Driver = InstantiateDriver(Type)


        If Driver Is Nothing Then
            Throw New DriverNotFoundException(DRV_NOT_FOUND)
        End If

        isInitalized = True
    End Sub

    Public Function UpdateState(ByVal state As String) As Boolean
        If isInitalized Then
            Return Driver.UpdateState(state)
        Else
            Return False
        End If
    End Function
End Class
