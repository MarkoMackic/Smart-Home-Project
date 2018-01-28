Imports System.Reflection
Imports SmartHomeClient.Globals
Imports System.Dynamic

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
    Public Adress As String = Nothing

    Public Event StateChanged(ByVal state As String)

    Private Function InstantiateDriver(ByVal type As Integer)
        Dim resultDriver As Object = Nothing
        Dim types = From t In Assembly.GetExecutingAssembly().GetTypes()
                     Where t.IsClass And t.Namespace = "SmartHomeClient.Drivers" And t.BaseType.Name = "Driver"
                     Select t


        For Each drv_type As Type In types

            If drv_type.GetMethod("supportsType").Invoke(Nothing, New Object() {type}) Then
                resultDriver = Activator.CreateInstance(drv_type, New Object() {Me})
                Exit For
            End If
        Next
        Return resultDriver
    End Function
    Private Sub addLog(ByVal data As String)
        mainForm.addLog(data, Color.SteelBlue)
    End Sub
    Public Sub New(ByVal devName As String, ByVal devPins() As Integer, ByVal devType As Integer, ByVal devID As Integer, Optional ByVal devMaster As Device = Nothing, Optional ByVal devAddress As String = Nothing)
     

        Name = devName
        Pins = devPins.ToList()
        Type = devType
        ID = devID


    

        If Not devMaster Is Nothing Then
            If devMaster.supportsChild(devType) Then
                Master = devMaster
            Else
                Throw New Exception("blabla")
            End If
        Else
            Master = Nothing
        End If

        Driver = InstantiateDriver(Type)

        If Driver Is Nothing Then
            Throw New DriverNotFoundException(DRV_NOT_FOUND)
        End If

        isInitalized = True
        addLog(String.Format("Device ({0}) initialized.", Name))
    End Sub

    Public Sub StateChangeCallback(ByVal state As String)
        addLog(String.Format("Device {0} changed state to : {1}", Name, state))
        Driver.StateStr(state)
        RaiseEvent StateChanged(state)
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
    Public Function ChangeState(ByVal state() As Object, Optional ByVal slave As Device = Nothing, Optional ByVal initialDevId As Integer = -1) As Boolean
        If isInitalized Then
            If initialDevId = -1 Then
                Return Driver.ChangeState(state, slave, Me.ID)
            Else
                Return Driver.ChangeState(state, slave, initialDevId)
            End If

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

    Public Function SerializableObject()
        Dim tempObj As Object
        tempObj = New ExpandoObject()

        tempObj.Name = Me.Name
        tempObj.Type = Me.Type
        tempObj.ID = Me.ID
        If Me.Master Is Nothing Then
            tempObj.MasterName = "NO_MASTER"
        Else
            tempObj.MasterName = Me.Master.Name
        End If

        tempObj.State = Driver.StateStr()
        tempObj.Driver = Driver.GetType.Name.ToString()

        Return tempObj


    End Function
End Class
