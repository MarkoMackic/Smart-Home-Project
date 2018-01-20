Imports System.Threading.Tasks
Imports SmartHomeClient.Globals
Imports System.Text.RegularExpressions
Public Class DeviceManager
    Private devices As List(Of Device)
    Private UI As DeviceManagerUI
    Private pinsHashTable As New Hashtable()
    Public isInitialized = False

    Public Sub New()
        ''initalize resources
        devices = New List(Of Device)
        logInstantiation(Me)
    End Sub

    Private Function pinConflicts(ByVal devMaster As Integer, ByVal devPins() As Integer)
       
            If pinsHashTable.ContainsKey(devMaster) Then
                If CType(pinsHashTable(devMaster), List(Of Integer)).Any(Function(pin) devPins.Contains(pin)) Then
                    Return True 'conflict exists
                Else
                pinsHashTable(devMaster) = CType(pinsHashTable(devMaster), List(Of Integer)).Concat(devPins)
                    Return False
                End If
            Else
                pinsHashTable(devMaster) = devPins.ToList()
                Return False
            End If

    End Function

    Public Function addDevice(ByVal devName As String,
                         ByVal devPins() As Integer,
                         ByVal devType As Integer,
                         ByVal devId As Integer,
                         Optional ByVal devMasterId As Integer = -1,
                         Optional ByVal devAddress As String = Nothing)


        Dim devMaster As Device = Nothing
        If devMasterId <> -1 Then

            For Each dev As Device In Me.devices

                If dev.ID = devMasterId Then
                    devMaster = dev
                    Exit For
                End If
            Next
        End If

        If pinConflicts(devMasterId, devPins) Then
            mainForm.addLog(String.Format("Device {0} can't be instantiated because there is pin conflicts with one of the previously instantiated devices", devName))
            Return False
        End If


        Try
            Dim dev As Device = New Device(devName, devPins, devType, devId, devMaster, devAddress)
            devices.Add(dev)
            Return True
        Catch ex As DriverNotFoundException
            mainForm.addLog("No driver found for : " + devName)
            Return False
        Catch ex As Exception
            mainForm.addLog(ex.Message)
            Return False
        End Try

    End Function

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

End Class
