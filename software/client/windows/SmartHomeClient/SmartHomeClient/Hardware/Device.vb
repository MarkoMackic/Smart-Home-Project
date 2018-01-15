Imports System.Reflection
Imports SmartHomeClient.Globals

Public Class Device
    'Will hold device information
    'Instantiate a driver for device

    Private isInitalized As Boolean = False
    Public Name As String 'Unique
    Public ID As Integer 'Unique
    Public Type As Integer
    Public Pins As List(Of Integer)
    Private Driver As Drivers.IDriver
    Public Master As Device = Nothing
    Public IsSlave As Boolean = False
    Public Adress As String = Nothing
    Private Function InstantiateDriver(ByVal type As Integer)
        Dim resultDriver As Object = Nothing
        Dim types = From t In Assembly.GetExecutingAssembly().GetTypes()
                     Where t.IsClass And t.Namespace = "SmartHomeClient.Drivers"
                     Select t


        For Each drv_type As Type In types
            If drv_type.GetMethod("supportsType").Invoke(Nothing, New Object() {type}) Then
                resultDriver = Activator.CreateInstance(drv_type, New Object() {Me})
                Exit For
            End If
        Next
        Return resultDriver
    End Function

    Public Sub New(ByVal devName As String, ByVal devPins() As Integer, ByVal devType As Integer, ByVal devID As Integer, Optional ByVal devMaster As Device = Nothing, Optional ByVal devAddress As String = Nothing)

        Name = devName
        Pins = devPins.ToList()
        Type = devType
        ID = devID
        Driver = InstantiateDriver(Type)

        If Driver Is Nothing Then
            Throw New DriverNotFoundException(DRV_NOT_FOUND)
        End If

        If Not devMaster Is Nothing Then
            If devMaster.supportsChild(devType) Then
                Master = devMaster
            Else
                Throw New Exception("blabla")
            End If
        End If



        If Not Master Is Nothing Then
            IsSlave = True
        End If
        mainForm.addLog(devName + IsSlave.ToString())
        isInitalized = True
        mainForm.addLog(String.Format("Device ({0}) initialized.", Name))
    End Sub
    Public Function supportsChild(ByVal devType As Integer)
        Return Driver.GetType().GetMethod("supportsChild").Invoke(Driver, New Object() {devType})
    End Function
    'Every device should have this and driver should handle
    Public Function UpdateState(ByVal state() As Object) As Boolean
        If isInitalized Then
            Return Driver.UpdateState(state)
        Else
            Return False
        End If
    End Function
    Public Function ChangeState(ByVal state() As Object, Optional ByVal slave As Device = Nothing) As Boolean
        If isInitalized Then
            Return Driver.ChangeState(state, slave)
        Else
            Return False
        End If
    End Function
    'Since there is no way to support all functions driver might have, we must have some generic interface
    Public Function GeneralFunction(ByVal functionName As String, ByVal params() As Object)
        If isInitalized Then
            Try
                Return Driver.GetType().GetMethod(functionName).Invoke(Driver, params)
            Catch ex As Exception 'we don't care about exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function
End Class
