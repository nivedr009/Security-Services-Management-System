Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form2


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

        If TextBox1.Text = "" AndAlso TextBox2.Text = "" Then
            ' Display a message indicating both fields are empty
            MessageBox.Show("Please enter username and password")
        ElseIf TextBox1.Text = "" Then
            ' Display a message indicating username is empty
            MessageBox.Show("Please enter username")
        ElseIf TextBox2.Text = "" Then
            ' Display a message indicating password is empty
            MessageBox.Show("Please enter password")
        ElseIf TextBox1.Text = "abc" AndAlso TextBox2.Text = "123" Then
            ' Simulate a login process (replace this with your actual login logic)
            Dim username As String = TextBox1.Text
            Dim password As String = TextBox2.Text

            ' Show the progress bar
            ProgressBar1.Visible = True

            ' Simulate login progress
            For i As Integer = 1 To 100
                ' Update the progress bar value
                ProgressBar1.Value = i

                ' Simulate processing time
                Threading.Thread.Sleep(10)
            Next

            ' Display a message indicating successful login
            MessageBox.Show("Login Successful")

            ' Close the login form and open the next form
            Dim form3 As New Form3
            form3.Show()
            Close()
        Else
            ' Display a message indicating unsuccessful login
            MessageBox.Show("Invalid username or password")
        End If

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        ' Close the current form (Form2) and open the first form (Form1) again
        Dim form1 As New Form1
        form1.Show()
        Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Exit the application
        Application.Exit()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set the form's AcceptButton property to Button1
        Me.AcceptButton = Button1

        ' Set the progress bar style to Continuous
        ProgressBar1.Style = ProgressBarStyle.Continuous

        'Msk the password textbox
        TextBox2.UseSystemPasswordChar = True

        ' Hide the progress bar (outside of the successful login condition)
        ProgressBar1.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Clear the username and password fields
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        ' Exit the application
        Application.Exit()
    End Sub

End Class