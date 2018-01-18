Imports SmartHomeClient.Globals
Namespace ClientMiddleware
    Public Class ClientManager
        Public IsConnected As Boolean = False
        Public Sub New(ByVal host As String, ByVal port As Integer)
            tcpCli = New NetClients.TCPClient()
            Try
                Dim ipAddr As String = Net.Dns.GetHostEntry(host).AddressList(0).ToString()
                tcpCli.Connect(ipAddr, port)
                AddHandler tcpCli.lineRecieved, AddressOf lineRecieved
                AddHandler tcpCli.servConnected, AddressOf serverConnected
            Catch ex As Exception
                MsgBox(ex.InnerException)
            End Try


        End Sub

        Private Sub lineRecieved(ByVal data As String)
            MsgBox(data)
        End Sub

        Private Sub serverConnected()
            IsConnected = True
            Login("Marko", "Mackic")
        End Sub

        Private Sub Login(ByVal uname As String, ByVal passwd As String)
            tcpCli.SendLine("LOGIN:" & uname & ":" & passwd)
        End Sub
    End Class
End Namespace

