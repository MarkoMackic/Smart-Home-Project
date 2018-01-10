Imports System.Reflection

Public Class Device
    'Will hold device information
    'Instantiate a driver for device

    Private isInitalized As Boolean
    Private Name As String
    Private Pins As List(Of Integer)
    Private Type As Integer
    Private Driver As Object

    Private Function InstantiateDriver(ByVal type As Integer)
        Dim resultDriver As Object = Nothing
        Dim types = From t In Assembly.GetExecutingAssembly().GetTypes()
                     Where t.IsClass And t.Namespace = "SmartHomeClient.Drivers"
                     Select t


        For Each drv_type As Type In types
            If drv_type.GetMethod("supportsType").Invoke(Nothing, New Object() {type}) Then
                resultDriver = Activator.CreateInstance(drv_type)
                Exit For
            End If
        Next
        Return resultDriver
    End Function
    Private Function CallDriverFunction(ByVal fName As String)

    End Function
    Public Sub New(ByVal devName As String, ByVal devPins() As Integer, ByVal devType As Integer)

        Name = devName
        Pins = devPins.ToList()
        Type = devType
        Driver = InstantiateDriver(Type)

        MsgBox(Driver.GetType().ToString())
        If Driver Is Nothing Then
            Throw New Exception(DRV_NOT_FOUND)
        End If
    End Sub
End Class
