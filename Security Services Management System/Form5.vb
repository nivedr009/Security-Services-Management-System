Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Data
Imports System.Data.SqlClient

Public Class Form5

    ' SQL connection string
    Dim connectionString As String = "Data Source=DESKTOP-O16HO1G\SQLEXPRESS;Initial Catalog=Security Service Management System;Integrated Security=True;"

    'Function to generate new siteid
    Private Function GenerateNextSiteId(connectionString As String) As String
        Dim nextSiteID As String = "S01" ' Default value

        Try
            ' SQL query to get the last site ID from the database
            Dim queryLastSiteID As String = "SELECT TOP 1 siteid FROM Site_table ORDER BY CAST(SUBSTRING(siteid, 2, LEN(siteid)) AS INT) DESC"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(queryLastSiteID, connection)
                    connection.Open()
                    Dim lastSiteID As String = Convert.ToString(command.ExecuteScalar())

                    ' If there are records in the database, generate the next site ID
                    If Not String.IsNullOrEmpty(lastSiteID) Then
                        ' Extract the numeric part of the last site ID
                        Dim numericPart As String = lastSiteID.Substring(1)
                        ' Convert the numeric part to integer and increment by 1
                        Dim nextNumericPart As Integer = Convert.ToInt32(numericPart) + 1
                        ' Generate the next site ID by combining the prefix ("S") and the incremented numeric part
                        nextSiteID = "S" & nextNumericPart.ToString("D2")
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error generating next site ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return nextSiteID
    End Function

    'function to validate email
    Private Function IsValidEmail(email As String) As Boolean
        ' Regular expression pattern for email validation
        Dim pattern As String = "^[a-z0-9](?:[a-z0-9](?!\.)|\.(?![^.])){3,18}[a-z0-9]@(gmail\.com|gmail\.net|gmail\.org|gmail\.in|hotmail\.com|hotmail\.co\.uk|yahoo\.com|yahoo\.co\.uk|outlook\.com|live\.com)$"
        ' Check if the email matches the pattern
        Return Regex.IsMatch(email, pattern)
    End Function



    ' Check if any character, number, or special character is repeated more than once
    Private Function ContainsRepeatedCharacters(input As String) As Boolean
        Dim count As Integer = 1
        For i As Integer = 1 To input.Length - 1
            If input(i) = input(i - 1) Then
                count += 1
                If count > 2 Then
                    Return True
                End If
            Else
                count = 1
            End If
        Next
        Return False
    End Function


    'Function to validate whether the phone number is a repeating number upto 6 digits
    Private Function ContainsRepeatingSequence(phoneNumber As String) As Boolean
        ' Remove the country code
        Dim numberWithoutCode As String = phoneNumber.Substring(3)

        ' Check if the remaining digits contain repeating sequences
        For i As Integer = 0 To numberWithoutCode.Length - 1
            Dim currentDigit As Char = numberWithoutCode(i)
            Dim sequenceLength As Integer = 1

            ' Check subsequent digits for repetition
            For j As Integer = i + 1 To numberWithoutCode.Length - 1
                If numberWithoutCode(j) = currentDigit Then
                    sequenceLength += 1
                    If sequenceLength = 6 Then ' A sequence of 6 repeating digits found
                        Return True
                    End If
                Else
                    Exit For
                End If
            Next j
        Next i

        ' No repeating sequences found
        Return False
    End Function
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Perform frontend validations
        If Not ValidateInputs() Then
            Return
        End If

        ' Check if siteid already exists
        Dim siteIdExists As Boolean = False
        Dim querySiteIdExists As String = "SELECT COUNT(*) FROM Site_table WHERE siteid = @siteid"

        Try
            Using connection As New SqlConnection(connectionString)
                Using commandCheckSiteId As New SqlCommand(querySiteIdExists, connection)
                    connection.Open()
                    commandCheckSiteId.Parameters.AddWithValue("@siteid", TextBox1.Text)
                    Dim count As Integer = Convert.ToInt32(commandCheckSiteId.ExecuteScalar())
                    siteIdExists = (count > 0)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking site ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        If siteIdExists Then
            MessageBox.Show("Site ID is already taken.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Insert data into the database
        Dim query As String = "INSERT INTO [dbo].[Site_table] ([siteid], [site_name], [site_city], [cf_name], [cl_name], [client_phno], [client_email]) " &
                              "VALUES (@siteid, @site_name, @site_city, @cf_name, @cl_name, @client_phno, @client_email)"

        Try
            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(query, connection)
                    ' Add parameters with Trim() function applied
                    command.Parameters.AddWithValue("@siteid", TextBox1.Text.Trim())
                    command.Parameters.AddWithValue("@site_name", TextBox2.Text.Trim())
                    command.Parameters.AddWithValue("@site_city", TextBox3.Text.Trim())
                    command.Parameters.AddWithValue("@cf_name", TextBox4.Text.Trim())
                    command.Parameters.AddWithValue("@cl_name", TextBox5.Text.Trim())
                    command.Parameters.AddWithValue("@client_phno", TextBox6.Text.Trim())
                    command.Parameters.AddWithValue("@client_email", TextBox7.Text.Trim())

                    connection.Open()
                    command.ExecuteNonQuery()

                    MessageBox.Show("Site Registered Successfully", "Confirmation", MessageBoxButtons.OK)
                    ClearInputs()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error inserting data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub



    Private Function ValidateInputs() As Boolean

        ' Site Name Presence check
        If String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MessageBox.Show("Site Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox2.Focus()
            Return False
        ElseIf Not TextBox2.Text.All(Function(c) Char.IsLetter(c) Or c = " ") Then
            MessageBox.Show("Invalid Site Name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox2.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox2.Text) Then
            MessageBox.Show("Site Name should not contain repeated characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox2.Focus()
            Return False
        End If

        ' City Presence check
        If String.IsNullOrWhiteSpace(TextBox3.Text) Then
            MessageBox.Show("City is Required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox3.Focus()
            Return False
        ElseIf Not TextBox3.Text.All(Function(c) Char.IsLetter(c) Or c = " ") Then
            MessageBox.Show("Invalid City", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox3.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox3.Text) Then
            MessageBox.Show("City should not contain repeated characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox3.Focus()
            Return False
        End If

        ' First and Last Name Presence check
        If String.IsNullOrWhiteSpace(TextBox4.Text) OrElse String.IsNullOrWhiteSpace(TextBox5.Text) Then
            MessageBox.Show("Both First name and Last name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        ' First Name Character Type Check
        If Not TextBox4.Text.All(Function(c) Char.IsLetter(c) Or c = " ") Then
            MessageBox.Show("Invalid First Name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox4.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox4.Text) Then
            MessageBox.Show("First Name should not contain repeated characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox4.Focus()
            Return False
        End If

        ' Last Name Character Type Check
        If Not TextBox5.Text.All(Function(c) Char.IsLetter(c) Or c = " ") Then
            MessageBox.Show("Invalid Last Name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox5.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox5.Text) Then
            MessageBox.Show("Last Name should not contain repeated characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox5.Focus()
            Return False
        End If

        ' Phone Number Presence check
        If String.IsNullOrWhiteSpace(TextBox6.Text) Then
            MessageBox.Show("Phone number cannot be blank", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox6.Focus()
            Return False
        ElseIf TextBox6.Text = "+91" Then
            MessageBox.Show("Please enter phone number ", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox6.Focus()
            Return False
        ElseIf Not TextBox6.Text.StartsWith("+91") OrElse TextBox6.Text.Length <> 13 OrElse Not TextBox6.Text.Substring(3).All(Function(c) Char.IsDigit(c)) OrElse TextBox6.Text.Substring(3, 1) Like "[0-5]" Then
            MessageBox.Show("Invalid Phone number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox6.Focus()
            Return False
        ElseIf ContainsRepeatingSequence(TextBox6.Text) Then
            MessageBox.Show("Invalid Phone number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox6.Focus()
            Return False
        End If

        ' Email ID Presence check
        If String.IsNullOrWhiteSpace(TextBox7.Text) Then
            MessageBox.Show("Email ID id required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox7.Focus()
            Return False
        ElseIf Not IsValidEmail(TextBox7.Text) Then
            MessageBox.Show("Invalid Email ID. Please enter a valid Email Address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox7.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearInputs()
        ' Clear all textboxes
        TextBox1.Text = "S"
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = "+91"
        TextBox7.Text = ""
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Close the current form (Form4) and open the first form (Form3) again
        Dim form3 As New Form3()
        form3.Show()
        Me.Close()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text.Length > 0 Then
            ' Get the current selection start position
            Dim selectionStart As Integer = TextBox2.SelectionStart
            ' Capitalize the first letter
            TextBox2.Text = TextBox2.Text.Substring(0, 1).ToUpper() + TextBox2.Text.Substring(1)
            ' Restore the selection start position
            TextBox2.SelectionStart = selectionStart
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        If TextBox3.Text.Length > 0 Then
            ' Get the current selection start position
            Dim selectionStart As Integer = TextBox3.SelectionStart
            ' Capitalize the first letter
            TextBox3.Text = TextBox3.Text.Substring(0, 1).ToUpper() + TextBox3.Text.Substring(1)
            ' Restore the selection start position
            TextBox3.SelectionStart = selectionStart
        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        If TextBox4.Text.Length > 0 Then
            ' Get the current selection start position
            Dim selectionStart As Integer = TextBox4.SelectionStart
            ' Capitalize the first letter
            TextBox4.Text = TextBox4.Text.Substring(0, 1).ToUpper() + TextBox4.Text.Substring(1)
            ' Restore the selection start position
            TextBox4.SelectionStart = selectionStart
        End If
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        If TextBox5.Text.Length > 0 Then
            ' Get the current selection start position
            Dim selectionStart As Integer = TextBox5.SelectionStart
            ' Capitalize the first letter
            TextBox5.Text = TextBox5.Text.Substring(0, 1).ToUpper() + TextBox5.Text.Substring(1)
            ' Restore the selection start position
            TextBox5.SelectionStart = selectionStart
        End If
    End Sub

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set the form's AcceptButton property to Button1
        Me.AcceptButton = Button1

        ' Site ID to start with "S"
        TextBox1.Text = "S"
        ' Set initial text of phone number textbox to "+91"
        TextBox6.Text = "+91"
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Clear Site ID, Site Name, City, First Name, Last Name, Email
        TextBox1.Text = "S" ' Reset Site ID to start with "S"
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = "+91" ' Set phone number to +91""
        TextBox7.Text = ""
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        ' Generate the next siteid and display it in TextBox1
        TextBox1.Text = GenerateNextSiteId(connectionString)

        ' Disable TextBox1 to prevent editing
        TextBox1.Enabled = False
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
        ' Exit the application
        Application.Exit()
    End Sub
End Class