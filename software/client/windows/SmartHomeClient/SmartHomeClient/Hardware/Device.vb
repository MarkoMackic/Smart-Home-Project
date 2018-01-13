Imports System.Reflection
Imports SmartHomeClient.Globals

Public Class Device
    'Will hold device information
    'Instantiate a driver for device

    Private isInitalized As Boolean = False
    Public Name As String 'Unique
    Public ID As Integer 'Unique
    Private Type As Integer
    Private Pins As List(Of Integer)
    Private Driver As Drivers.IDriver
    Private Master As Device = Nothing
    Public IsSlave As Boolean = False
    Private Adress As String = Nothing
    Private Function InstantiateDriver(ByVal type As Integer)
        Dim resultDriver As Object = Nothing
        Dim types = From t In Assembly.GetExecutingAssembly().GetTypes()
                     Where t.IsClass And t.Namespace = "SmartHomeClient.Drivers"
                     Select t


        For Each drv_type As Type In types
            If drv_type.GetMethod("supportsType").Invoke(Nothing, New Object() {type}) Then
                resultDriver = Activator.CreateInstance(drv_type, New Object() {Pins, Name})

                Exit For
            End If
        Next
        Return resultDriver
    End Function

    Public Sub New(ByVal devName As String, ByVal devPins() As Integer, ByVal devType As Integer, Optional ByVal devMaster As Device = Nothing, Optional ByVal devAddress As String = Nothing)

        Name = devName
        Pins = devPins.ToList()
        Type = devType
        Driver = InstantiateDriver(Type)

        If Driver Is Nothing Then
            Throw New DriverNotFoundException(DRV_NOT_FOUND)
        End If

        Master = devMaster

        If Not Master Is Nothing Then
            IsSlave = True
        End If

        isInitalized = True
        mainForm.addLog(String.Format("Device ({0}) initialized.", Name))
    End Sub

    Public Function UpdateState(ByVal state As String) As Boolean
        If isInitalized Then
            Return Driver.UpdateState(state)
        Else
            Return False
        End If
    End Function
    Public Function ChangeState(ByVal state As String) As Boolean
        If isInitalized Then
            Return Driver.ChangeState(state)
        Else
            Return False
        End If
    End Function
End Class
