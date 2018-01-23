Imports SmartHomeClient.Globals
Imports System.Runtime.InteropServices

Namespace ClientMiddleware
    Public Class ClientManager
        Private uName As String, uPass As String
        Private Usable As Boolean = True ' If we get error from server
        Public LoggedIn As Boolean = False

        Public Event Connected()
        Public Event ReceivedData(ByVal data As String)

        Private timeSent As DateTime = Nothing
        Private updateUI As DateTime
        Private AVGNetSpeed As Integer = 0
        Private LastPayloadSize As Double

        Private logColor As Color = Color.Maroon


        Public Sub New(ByVal host As String, ByVal port As Integer, ByVal username As String, ByVal password As String)
            logInstantiation(Me)
            updateUI = Now

            uName = username
            uPass = password
            tcpCli = New NetClients.TCPClient()

            Try
                Dim ipAddr As String = Net.Dns.GetHostEntry(host).AddressList(0).ToString()
                tcpCli.Connect(ipAddr, port)
                AddHandler tcpCli.lineRecieved, AddressOf lineRecieved
                AddHandler tcpCli.servConnected, AddressOf serverConnected
            Catch ex As Exception
                Log(ex.Message, logColor)
            End Try


        End Sub

        Private Sub lineRecieved(ByVal data As String)
            data = Trim(data) 'anyway trim it

            Dim isHandled As Boolean = False

            isHandled = ProtocolConfirmation(data)
            'daisy chain handlers
            If Not isHandled Then
                isHandled = LoginResponse(data)
            End If
            If Not isHandled Then
                isHandled = SetDevStatesResponse(data)
            End If


            If Not isHandled Then
                Log(String.Format("Server response '{0}' isn't handled", data), logColor)
            End If

        End Sub

        Private Sub serverConnected()
            Log("Connected to server", logColor)
        End Sub

        Private Sub Login(ByVal uname As String, ByVal passwd As String)
            tcpCli.SendLine("LOGIN:" & uname & ":" & passwd)
        End Sub

        Public Sub SendDeviceData(ByVal jsonData As String)
            timeSent = Now
            LastPayloadSize = jsonData.Length
            tcpCli.SendLine("SDS:" & jsonData)
        End Sub


        Public Sub Destroy()
            Try
                tcpCli.Destroy(0)
            Catch ex As Exception
            End Try
        End Sub
        'Message handlers 

        Private Function ProtocolConfirmation(ByVal data As String) As Boolean
            If data = "V2_SMART_HOME_AUTOMATION" Then
                Login(uName, uPass)
                Return True
            End If
            Return False
        End Function
        Private Function LoginResponse(ByVal data As String) As Boolean
            Dim parts() As String = data.Split(":")
            parts(0) = parts(0).ToUpper()
            If parts(0) = "LOGIN" Then
                Select Case parts(1)
                    Case "0"
                        Log(parts(2), logColor)
                        LoggedIn = True
                End Select
                Return True
            End If
            Return False
        End Function


        Private Function SetDevStatesResponse(ByVal data As String) As Boolean ' the most used network function, so we'll calculate speed on it

            Dim parts() As String = data.Split(":")
            parts(0) = parts(0).ToUpper()
            If parts(0) = "SDS" Then
                'calculate speed

                If Not timeSent = Nothing Then
                    Dim msDiff As Double
                    Try
                        msDiff = Now.Subtract(timeSent).TotalMilliseconds
                        If msDiff <> 0 Then
                            AVGNetSpeed = (AVGNetSpeed + LastPayloadSize / msDiff) / 2
                        End If

                    Catch ex As OverflowException
                        MsgBox(ex.Message)
                        MsgBox(LastPayloadSize)
                        MsgBox(msDiff)

                    End Try




                End If


                If Now.Subtract(updateUI).TotalMilliseconds > 200 Then
                    updateUI = Now
                    mainForm.ChangeText(AVGNetSpeed & " kb/s", mainForm.lblNetSpeed)
                End If

                'do other stuff
                Select Case parts(1)
                    Case "0"
                        devManager.sendDevicesThrSignal = True
                End Select
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace

