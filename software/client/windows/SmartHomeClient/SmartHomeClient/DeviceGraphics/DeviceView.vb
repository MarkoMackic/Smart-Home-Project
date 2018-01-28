Imports System.Dynamic

Public Class DeviceView
    Public Sub New(ByVal dev As Device)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim devProps As Object = dev.SerializableObject()

        'tempObj.Name = Me.Name
        'tempObj.Type = Me.Type
        'tempObj.ID = Me.ID
        'If Not Me.Master Is Nothing Then
        '    tempObj.MasterName = Me.Master.Name
        'End If
        'tempObj.State = Driver.StateStr()
        'tempObj.Driver = Driver.GetType.ToString()

        'Return tempObj

        lblID.Text = devProps.ID
        lblName.Text = devProps.Name
        lblDriver.Text = devProps.Driver

        lblParent.Text = devProps.MasterName





    End Sub
    Private Sub DeviceView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
