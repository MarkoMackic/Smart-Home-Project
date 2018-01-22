Imports System.Threading.Tasks
Imports SmartHomeClient.Globals
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports System.Threading


Public Class DeviceManager
    Private devices As List(Of Device)
    Private UI As DeviceManagerUI
    Private pinsHashTable As New Hashtable()
    Private isInitialized = False

    Private logColor As Color = Color.Gold

    Private sendDevicesThrRunning As Boolean = True
    Private sendDevicesThr As Thread
    Public sendDevicesThrSignal As Boolean = True
    Private sendDevicesThrTimeout As Integer = 300




    Public Sub New()
        ''initalize resources
        devices = New List(Of Device)
        logInstantiation(Me)
        If Not cliManager Is Nothing Then
            sendDevicesThr = New Thread(AddressOf SendDevicesToServer)
            sendDevicesThr.Start()
        End If

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
            Log(String.Format("Device {0} can't be instantiated because there is pin conflicts with one of the previously instantiated devices", devName), logColor)
            Return False
        End If


        Try
            Dim dev As Device = New Device(devName, devPins, devType, devId, devMaster, devAddress)
            devices.Add(dev)
            Return True
        Catch ex As DriverNotFoundException
            Log("No driver found for : " + devName, logColor)
            Return False
        Catch ex As Exception
            Log(ex.Message, logColor)
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

    Private Sub SendDevicesToServer()
        While sendDevicesThrRunning
            If cliManager.LoggedIn And sendDevicesThrSignal Then
                Dim temp As New List(Of Object)

                For Each dev As Device In devices.ToArray()
                    temp.Add(dev.SerializableObject())
                Next

                sendDevicesThrSignal = False
                cliManager.SendDeviceData(JsonConvert.SerializeObject(temp))
            End If

            Thread.Sleep(sendDevicesThrTimeout)

            'Using jsonWriter As New System.IO.StreamWriter("devices.json")
            '    jsonWriter.WriteLine(JsonConvert.SerializeObject(temp))
            'End Using

        End While

    End Sub

    Public Sub Destroy()
        Try
            sendDevicesThrRunning = False
            If (sendDevicesThr.ThreadState = ThreadState.Running) Then
                sendDevicesThr.Abort()
            End If
        Catch ex As Exception
        End Try

    End Sub

    Public Function GetDevice(ByVal idx As Integer)
        If idx < devices.Count Then
            Return devices(idx)
        End If
        Return Nothing
    End Function

End Class
