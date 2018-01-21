Imports System.Net, System.Net.Sockets, System.Text
Imports System.Timers
Imports SmartHomeClient.Globals

Namespace NetClients
    Public Class TCPClient
        Private currentmessage As String = ""

        Private tcpIP As String
        Private tcpPort As Integer
        Private clientSocket As Socket
        Private byteData(1023) As Byte
        Private ping As Timer

        Private logColor As Color = Color.Beige
     
        Public Event lineRecieved(ByVal data As String)
        Public Event servConnected()



        Public Sub New()
            logInstantiation(Me)
        End Sub

        Public Function Connect(Optional ByVal address As String = Nothing, Optional ByVal port As Integer = Nothing)
            If address Is Nothing Or port = Nothing Then
                Return False
            End If
            tcpIP = address
            tcpPort = port
            clientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Dim ipAddress As IPAddress = ipAddress.Parse(address)
            Dim ipEndPoint As IPEndPoint = New IPEndPoint(ipAddress, port)
            clientSocket.BeginConnect(ipEndPoint, New AsyncCallback(AddressOf OnConnect), Nothing)
            Return True
        End Function
        Private Sub OnConnect(ByVal ar As IAsyncResult)
            Try

                clientSocket.EndConnect(ar)
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, _
                                          New AsyncCallback(AddressOf OnRecieve), clientSocket)
                ping = New Timer
                ping.Interval = pingInterval
                ping.AutoReset = True
                AddHandler ping.Elapsed, AddressOf pingServer
                ping.Start() 'start timer

                RaiseEvent servConnected()

            Catch e As Exception
                clientSocket.Dispose()
                If MessageBox.Show("Error on connection, try again?", "Error", MessageBoxButtons.OKCancel) = DialogResult.OK Then
                    Connect(tcpIP, tcpPort) ' connect again
                End If
            End Try

        End Sub

        Private Sub OnRecieve(ByVal ar As IAsyncResult)
            Try
                If Not clientSocket Is Nothing Then
                    clientSocket.EndReceive(ar)
                End If

                'swap bytes into a new array
                Dim bytesRec As Byte() = byteData.Clone()
                'get the string representation
                Dim message As String = System.Text.ASCIIEncoding.ASCII.GetString(bytesRec)
                'send message to read
                Read(message)
                'check for disconnection
                If message.Length = 1 Then
                    If Asc(message) = 0 Then 'if we get message that contains ASCII 0 we can suppose we're disconnected
                        If Not clientSocket Is Nothing Then
                            clientSocket.Disconnect(False)
                        End If
                        Throw New Exception("Disconnected from server!")
                    End If
                End If
                ReDim byteData(0) ' get bytedata to 0 again
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, New AsyncCallback(AddressOf OnRecieve), clientSocket) 'recieve again
            Catch e As Exception
                If Not clientSocket Is Nothing Then
                    clientSocket.Disconnect(False)
                End If
                clientSocket.Dispose()
                Log("Error : " & e.Message, Me.logColor) 'log error
            End Try

        End Sub

        Private Sub Read(ByVal msg As String)
            msg = msg.Replace(Chr(0), "")
            currentmessage &= msg 'we append msg to current message 
            If currentmessage.Contains(Chr(10)) Then
                Dim temp() As String = currentmessage.Split(New Char() {Chr(10)}, 2)
                currentmessage = temp(1)
                RaiseEvent lineRecieved(temp(0).Trim())
            End If
        End Sub

        Public Sub SendLine(ByVal message As String)
            Try
                If Not clientSocket Is Nothing And clientSocket.Connected Then
                    clientSocket.Send(Encoding.ASCII.GetBytes(message & vbNewLine)) 'send it and new line
                End If
            Catch e As Exception
                Log("Error : " & e.Message, Me.logColor) 'log error
                If Not clientSocket Is Nothing Then
                    clientSocket.Disconnect(False)
                    clientSocket.Dispose()
                    clientSocket = Nothing
                End If
                clientSocket.Dispose()
            End Try


        End Sub

        Public Sub Destroy(ByVal reuse As Boolean) 'destroy the client socket
            If Not clientSocket Is Nothing Then
                clientSocket.Disconnect(reuse)
                If Not reuse Then
                    clientSocket.Dispose()
                End If
                ping.Stop()
            End If
        End Sub

        Private Sub pingServer(ByVal sender As Object, ByVal e As EventArgs)
            If Not clientSocket Is Nothing And clientSocket.Connected Then
                Me.SendLine("PING")
            End If


        End Sub

        Public Function isConnected()
            If clientSocket.Connected And Not clientSocket Is Nothing Then
                Return True
            End If
            Return False
        End Function
    End Class

End Namespace

