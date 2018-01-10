Imports System.Threading.Tasks
Imports SmartHomeClient.Globals

Public Class DeviceManager
    Private devices As List(Of Device)
    Private UI As DeviceManagerUI

    Private isInitialized = False

    Public Sub New()
        ''initalize resources
        devices = New List(Of Device)

    End Sub


    Public Sub addDevice(ByVal devName As String, ByVal devPins() As Integer, ByVal devType As Integer)

    End Sub

    Public Sub attachUI(ByVal UI As DeviceManagerUI)
        Me.UI = UI
        AddHandler UI.FormClosed, AddressOf detachUI
    End Sub
    Private Sub detachUI()
        Me.UI = Nothing
    End Sub

    Public Sub broadcastPinStates()
        If Me.isInitialized Then

        End If
    End Sub
End Class
