Public Class Globals

    Public Shared hardwareChannel As HardwareComm = Nothing
    Public Shared msgHandler As MessageHandler = Nothing
    Public Shared dbAdapter As DBDriver = Nothing
    Public Shared faceRecognizer As FaceRecognition = Nothing
    Public Shared masterCont As MasterController = Nothing
    Public Shared devManager As DeviceManager = Nothing


    'it is to hold the instance of MainUI
    Public Shared mainForm As MainUI

    Public Shared authmessage = "authmessage"

    Public Shared resLoaded As Boolean = False


End Class
