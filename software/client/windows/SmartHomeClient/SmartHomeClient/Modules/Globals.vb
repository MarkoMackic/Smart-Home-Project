Public Class Globals
    'Resources
    Public Shared hardwareChannel As HardwareComm = Nothing
    Public Shared msgHandler As MessageHandler = Nothing
    Public Shared dbAdapter As DBDriver = Nothing
    Public Shared faceRecognizer As FaceRecognition = Nothing
    Public Shared masterCont As MasterController = Nothing
    Public Shared devManager As DeviceManager = Nothing
    Public Shared crProvider As CryptoProvider.AESCrypto = Nothing
    Public Shared tcpCli As NetClients.TCPClient = Nothing
    Public Shared cliManager As ClientMiddleware.ClientManager = Nothing

    'it is to hold the instance of MainUI
    Public Shared mainForm As MainUI

    Public Shared authmessage = "authmessage"

    Public Shared resLoaded As Boolean = False



    'Client configuration 
    Public Shared pingInterval As Integer = 7000


End Class
