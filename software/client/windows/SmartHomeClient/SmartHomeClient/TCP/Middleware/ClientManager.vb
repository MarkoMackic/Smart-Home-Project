Imports SmartHomeClient.Globals
Namespace ClientMiddleware
    Public Class ClientManager
        Private uName As String, uPass As String

        Public Sub New(ByVal host As String, ByVal port As Integer, ByVal username As String, ByVal password As String)
            logInstantiation(Me)
            uName = username
            uPass = password
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
            Login(uName, uPass)
        End Sub

        Private Sub Login(ByVal uname As String, ByVal passwd As String)
            If tcpCli.isConnected() Then
                tcpCli.SendLine("LOGIN:" & uname & ":" & passwd)
            End If
        End Sub
    End Class
End Namespace

