Public Class Form1


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        ' Creating an instance of the next form
        Dim form2 As New Form2

        ' Hiding the current form
        Hide()

        ' Show the next form
        form2.Show()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        ' Exit the application
        Application.Exit()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the form's AcceptButton property to Button1
        Me.AcceptButton = Button1
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        ' Exit the application
        Application.Exit()
    End Sub
End Class
