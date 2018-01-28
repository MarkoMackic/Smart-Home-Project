Imports SmartHomeClient.Globals

Public Class OnOff
    Implements IStateChanger

    Public State As Integer
    Private ctlDev As Device

    Private Sub ClickHandler(ByVal sender As Object, ByVal e As System.EventArgs)

        ctlDev.ChangeState(New Object() {State Xor 1})
    End Sub


    Private Sub OnOff_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        Me.State = Int(ctlDev.StateStr())
        StateUpdate(ctlDev.StateStr())
        AddHandler Me.Click, AddressOf ClickHandler
        For Each ctl As Control In Me.Controls
            AddHandler ctl.Click, AddressOf ClickHandler
        Next
    End Sub

    Public Sub New(ByVal dev As Device)
        InitializeComponent()
        ctlDev = dev

    End Sub


    Public Function StateUpdate(ByVal state As Object) Implements IStateChanger.StateUpdate

        If TypeOf state Is String And IsNumeric(state) Then
            Dim tmpState As Integer = Int(state)

            If tmpState = 0 Then
                Me.Label1.Text = "OFF"
                Me.State = False
                Me.BackColor = Color.Red
            ElseIf tmpState = 1 Then
                Me.Label1.Text = "ON"
                Me.State = True
                Me.BackColor = Color.Green
            Else
                Throw New Exception("Invalid state")
            End If
            Me.State = tmpState

            Return True

        End If
        Return False
    End Function

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub
End Class
