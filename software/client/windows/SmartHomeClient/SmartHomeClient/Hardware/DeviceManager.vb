Imports System.Threading.Tasks
Imports SmartHomeClient.Globals

Public Class DeviceManager
    Private devices As List(Of Device)
    Private UI As DeviceManagerUI

    Private isInitialized = False

    Public Sub New()
        ''initalize resources
        devices = New List(Of Device)
        logInstantiation(Me)
    End Sub


    Public Sub addDevice(ByVal devName As String, ByVal devPins() As Integer, ByVal devType As Integer)
        Try
            Dim dev As Device = New Device(devName, devPins, devType)
            devices.Add(dev)
        Catch ex As DriverNotFoundException
            mainForm.addLog("No driver found for : " + devName)
        End Try


    End Sub

    Public Sub attachUI(ByVal UI As DeviceManagerUI)
        Me.UI = UI
        AddHandler UI.FormClosed, AddressOf detachUI
    End Sub
    Private Sub detachUI()
        Me.UI = Nothing
    End Sub

    Public Function GetDevice(ByVal idx As Integer)
        If idx < devices.Count Then
            Return devices(idx)
        End If
        Return Nothing
    End Function
    Public Sub broadcastPinStates()
        If Me.isInitialized Then

        End If
    End Sub
End Class
