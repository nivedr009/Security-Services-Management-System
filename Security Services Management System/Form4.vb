
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Data
Imports System.Data.SqlClient

Public Class Form4

    Dim connectionString As String = "Data Source=DESKTOP-O16HO1G\SQLEXPRESS;Initial Catalog=Security Service Management System;Integrated Security=True;"

    'Function to generate new Empid
    Private Function GenerateNextEmpIDFromDatabase() As String
        Dim nextEmpID As String = "SG01" ' Default value

        Try
            ' SQL query to get the last employee ID from the database
            Dim queryLastEmpID As String = "SELECT TOP 1 empid FROM Emp_table ORDER BY CAST(SUBSTRING(empid, 3, LEN(empid)) AS INT) DESC"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(queryLastEmpID, connection)
                    connection.Open()
                    Dim lastEmpID As String = Convert.ToString(command.ExecuteScalar())

                    ' If there are records in the database, generate the next employee ID
                    If Not String.IsNullOrEmpty(lastEmpID) Then
                        ' Extract the numeric part of the last employee ID
                        Dim numericPart As String = lastEmpID.Substring(2)
                        ' Convert the numeric part to an integer and increment by 1
                        Dim nextNumericPart As Integer = Convert.ToInt32(numericPart) + 1
                        ' Generate the next employee ID by combining the prefix ("SG") and the incremented numeric part
                        nextEmpID = "SG" & nextNumericPart.ToString("D2")
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error generating next employee ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return nextEmpID
    End Function



    Private Function IsValidEmail(email As String) As Boolean
        ' Regular expression pattern for email validation
        Dim pattern As String = "^[a-z0-9](?:[a-z0-9](?!\.)|\.(?![^.])){3,18}[a-z0-9]@(gmail\.com|gmail\.net|gmail\.org|gmail\.in|hotmail\.com|hotmail\.co\.uk|yahoo\.com|yahoo\.co\.uk|outlook\.com|live\.com)$"
        ' Check if the email matches the pattern
        Return Regex.IsMatch(email, pattern)
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


    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set the form's AcceptButton property to Button1
        Me.AcceptButton = Button1

        ' Emp ID to start with "SG"
        TextBox1.Text = "SG"

        ' Set initial text of phone number textbox to "+91"
        TextBox5.Text = "+91"

        ' Set default date for dob
        DateTimePicker1.Value = DateTime.Today

        ' Add photo ID types to the ComboBox
        ComboBox1.Items.Add("Aadhar Card")
        ComboBox1.Items.Add("Driver's License")
        ComboBox1.Items.Add("Passport")
        ComboBox1.Items.Add("Voter ID")

        ' Set a default selection if needed
        ComboBox1.SelectedIndex = 0

        ' Add states to the ComboBox
        ComboBox2.Items.Add("Andhra Pradesh")
        ComboBox2.Items.Add("Arunachal Pradesh")
        ComboBox2.Items.Add("Assam")
        ComboBox2.Items.Add("Bihar")
        ComboBox2.Items.Add("Chhattisgarh")
        ComboBox2.Items.Add("Goa")
        ComboBox2.Items.Add("Gujarat")
        ComboBox2.Items.Add("Haryana")
        ComboBox2.Items.Add("Himachal Pradesh")
        ComboBox2.Items.Add("Jharkhand")
        ComboBox2.Items.Add("Karnataka")
        ComboBox2.Items.Add("Kerala")
        ComboBox2.Items.Add("Madhya Pradesh")
        ComboBox2.Items.Add("Maharashtra")
        ComboBox2.Items.Add("Manipur")
        ComboBox2.Items.Add("Meghalaya")
        ComboBox2.Items.Add("Mizoram")
        ComboBox2.Items.Add("Nagaland")
        ComboBox2.Items.Add("Odisha")
        ComboBox2.Items.Add("Punjab")
        ComboBox2.Items.Add("Rajasthan")
        ComboBox2.Items.Add("Sikkim")
        ComboBox2.Items.Add("Tamil Nadu")
        ComboBox2.Items.Add("Telangana")
        ComboBox2.Items.Add("Tripura")
        ComboBox2.Items.Add("Uttar Pradesh")
        ComboBox2.Items.Add("Uttarakhand")
        ComboBox2.Items.Add("West Bengal")
        ComboBox2.Items.Add("Andaman and Nicobar Islands")
        ComboBox2.Items.Add("Chandigarh")
        ComboBox2.Items.Add("Dadra and Nagar Haveli")
        ComboBox2.Items.Add("Daman and Diu")
        ComboBox2.Items.Add("Lakshadweep")
        ComboBox2.Items.Add("Delhi")
        ComboBox2.Items.Add("Puducherry")

        ' Add different ranks to Designation
        ComboBox3.Items.Add("Security Guard")
        ComboBox3.Items.Add("Lady Guard")
        ComboBox3.Items.Add("Field Officer")
        ComboBox3.Items.Add("Supervisor")

        ' Reset designation ComboBox to its first default option
        ComboBox3.SelectedIndex = 0

        'Disable manual entry into comboBox
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList


    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub TextBox11_TextChanged(sender As Object, e As EventArgs) Handles TextBox11.TextChanged
        If TextBox11.Text.Length > 0 Then
            ' Get the current selection start position
            Dim selectionStart As Integer = TextBox11.SelectionStart
            ' Capitalize the first letter
            TextBox11.Text = TextBox11.Text.Substring(0, 1).ToUpper() + TextBox11.Text.Substring(1)
            ' Restore the selection start position
            TextBox11.SelectionStart = selectionStart
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Close the current form (Form4) and open the first form (Form3) again
        Dim form3 As New Form3()
        form3.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        ' Perform frontend validations
        If Not ValidateInputs() Then
            Return
        End If

        ' Check if empid already exists
        Dim empIdExists As Boolean = False
        Dim queryEmpIdExists As String = "SELECT COUNT(*) FROM Emp_table WHERE empid = @empid"

        Try
            Using connection As New SqlConnection(connectionString)
                Using commandCheckEmpId As New SqlCommand(queryEmpIdExists, connection)
                    connection.Open()
                    commandCheckEmpId.Parameters.AddWithValue("@empid", TextBox1.Text)
                    Dim count As Integer = Convert.ToInt32(commandCheckEmpId.ExecuteScalar())
                    empIdExists = (count > 0)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking employee ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        If empIdExists Then
            MessageBox.Show("Employee ID is already taken.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If


        ' Insert data into the database
        Dim query As String = "INSERT INTO [dbo].[Emp_table] ([empid], [efname], [elname], [dob], [designation], [phn_no], [alt_phnno], [email], [photo_id], [house_noname], [street_name], [city_name], [district_name], [state], [pincode]) " &
                               "VALUES (@empid, @efname, @elname, @dob, @designation, @phn_no, @alt_phnno, @email, @photo_id, @house_noname, @street_name, @city_name, @district_name, @state, @pincode)"

        Try
            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(query, connection)
                    ' Add parameters with Trim() function applied
                    command.Parameters.AddWithValue("@empid", TextBox1.Text.Trim())
                    command.Parameters.AddWithValue("@efname", TextBox2.Text.Trim())
                    command.Parameters.AddWithValue("@elname", TextBox3.Text.Trim())
                    command.Parameters.AddWithValue("@dob", DateTimePicker1.Value)
                    command.Parameters.AddWithValue("@designation", ComboBox3.SelectedItem.ToString().Trim())
                    command.Parameters.AddWithValue("@phn_no", TextBox5.Text.Trim())
                    command.Parameters.AddWithValue("@alt_phnno", If(String.IsNullOrEmpty(TextBox6.Text), DBNull.Value, TextBox6.Text.Trim()))
                    command.Parameters.AddWithValue("@email", If(String.IsNullOrEmpty(TextBox7.Text), DBNull.Value, TextBox7.Text.Trim()))
                    command.Parameters.AddWithValue("@photo_id", ComboBox1.SelectedItem.ToString().Trim())
                    command.Parameters.AddWithValue("@house_noname", TextBox9.Text.Trim())
                    command.Parameters.AddWithValue("@street_name", TextBox10.Text.Trim())
                    command.Parameters.AddWithValue("@city_name", TextBox11.Text.Trim())
                    command.Parameters.AddWithValue("@district_name", TextBox12.Text.Trim())
                    command.Parameters.AddWithValue("@state", ComboBox2.SelectedItem.ToString().Trim())
                    command.Parameters.AddWithValue("@pincode", TextBox14.Text.Trim())

                    connection.Open()
                    command.ExecuteNonQuery()

                    MessageBox.Show("Employee Registered Successfully", "Confirmation", MessageBoxButtons.OK)
                    ClearInputs()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error inserting data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Function ValidateInputs() As Boolean ' Frontend validations

        Dim regexEmpID As New Regex("^SG\d+$")


        ' Employee ID Presence Check
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("Employee ID cannot be blank", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Return False
        ElseIf Not regexEmpID.IsMatch(TextBox1.Text) Then
            MessageBox.Show("Invalid EMPLOYEE ID", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Return False
        End If

        'First and Last Name Presence check
        If String.IsNullOrWhiteSpace(TextBox2.Text) OrElse String.IsNullOrWhiteSpace(TextBox3.Text) Then
            MessageBox.Show("Both first name and last name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        ElseIf Not TextBox2.Text.All(Function(c) Char.IsLetter(c) Or c = " ") Then
            MessageBox.Show("Invalid first name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox2.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox2.Text) Then
            MessageBox.Show("First name should not contain repeated characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox2.Focus()
            Return False
        End If

        'Last Name Presence check
        If Not TextBox3.Text.All(Function(c) Char.IsLetter(c) Or c = " ") Then
            MessageBox.Show("Invalid last name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox3.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox3.Text) Then
            MessageBox.Show("Last name should not contain repeated characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox3.Focus()
            Return False
        End If

        ' Date Presence check
        If DateTimePicker1.Value = DateTime.Today Then
            MessageBox.Show("Please enter a valid DOB", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DateTimePicker1.Focus()
            Return False
        ElseIf (DateTimePicker1.Value > DateTime.Today) Then
            MessageBox.Show("DOB cannot be in future", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DateTimePicker1.Focus()
            Return False
        ElseIf (DateTime.Today - DateTimePicker1.Value).TotalDays < (18 * 365.25) Then
            MessageBox.Show("Employee must be atleast 18 years old to register", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DateTimePicker1.Focus()
            Return False
        ElseIf (DateTime.Today - DateTimePicker1.Value).TotalDays > (60 * 365.25) Then
            MessageBox.Show("Employee cannot be above 60 years old to register", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DateTimePicker1.Focus()
            Return False
        End If

        ' Phone Number Presence check
        If String.IsNullOrWhiteSpace(TextBox5.Text) Then
            MessageBox.Show("Phone number cannot be blank", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox5.Focus()
            Return False
        ElseIf TextBox5.Text = "+91" Then
            MessageBox.Show("Please enter phone number ", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox5.Focus()
            Return False
        ElseIf Not TextBox5.Text.StartsWith("+91") OrElse TextBox5.Text.Length <> 13 OrElse Not TextBox5.Text.Substring(3).All(Function(c) Char.IsDigit(c)) OrElse TextBox5.Text.Substring(3, 1) Like "[0-5]" Then
            MessageBox.Show("Invalid Phone number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox5.Focus()
            Return False
        ElseIf ContainsRepeatingSequence(TextBox3.Text) Then
            MessageBox.Show("Phone number contains a repeating sequence of more than 6 digits", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox3.Focus()
            Return False
        End If

        ' Alternate Phone Number Presence and Format check
        If Not String.IsNullOrEmpty(TextBox6.Text) Then
            If TextBox6.Text = "+91" Then
                MessageBox.Show("Please enter alternate number ", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox6.Focus()
                Return False
            ElseIf Not TextBox6.Text.StartsWith("+91") OrElse TextBox6.Text.Length <> 13 OrElse Not TextBox6.Text.Substring(3).All(Function(c) Char.IsDigit(c)) OrElse TextBox6.Text.Substring(3, 1) Like "[0-5]" Then
                MessageBox.Show("Invalid alternate phone number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox6.Focus()
                Return False
            ElseIf TextBox5.Text = TextBox6.Text Then
                MessageBox.Show("Phone number and alternate phone number cannot be the same", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox5.Focus()
                Return False
            ElseIf ContainsRepeatingSequence(TextBox6.Text) Then
                MessageBox.Show("Invalid Alternate Phone Number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox6.Focus()
                Return False
            End If
        End If

        ' Email format check
        If Not String.IsNullOrWhiteSpace(TextBox7.Text) AndAlso Not IsValidEmail(TextBox7.Text) Then
            MessageBox.Show("Invalid email ID. Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox7.Focus()
            Return False
        End If

        ' House no/name Presence and Format check
        If String.IsNullOrWhiteSpace(TextBox9.Text) Then
            MessageBox.Show("Please enter house/building name or number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox9.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox9.Text) Then
            MessageBox.Show("House/building name or number should not contain repeated characters, numbers, or special characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox9.Focus()
            Return False
        End If

        ' Street Presence and Format check
        If String.IsNullOrWhiteSpace(TextBox10.Text) Then
            MessageBox.Show("Please enter street", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox10.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox10.Text) Then
            MessageBox.Show("Street should not contain repeated characters, numbers, or special characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox10.Focus()
            Return False
        End If

        ' City Presence and Format check
        If String.IsNullOrWhiteSpace(TextBox11.Text) Then
            MessageBox.Show("Please enter the City", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox11.Focus()
            Return False
        ElseIf Not TextBox11.Text.All(Function(c) Char.IsLetter(c) Or c = " ") Then
            MessageBox.Show("Invalid City name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox11.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox11.Text) Then
            MessageBox.Show("City should not contain repeated characters, numbers, or special characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox11.Focus()
            Return False
        End If

        ' District Presence and Format check
        If String.IsNullOrWhiteSpace(TextBox12.Text) Then
            MessageBox.Show("Please enter the District", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox12.Focus()
            Return False
        ElseIf Not TextBox12.Text.All(Function(c) Char.IsLetter(c) Or c = " ") Then
            MessageBox.Show("Invalid District name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox12.Focus()
            Return False
        ElseIf ContainsRepeatedCharacters(TextBox12.Text) Then
            MessageBox.Show("District should not contain repeated characters, numbers, or special characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox12.Focus()
            Return False
        End If

        ' Check if State is selected from ComboBox2
        If ComboBox2.SelectedIndex = -1 Then
            MessageBox.Show("Please select the State", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ComboBox2.Focus()
            Return False
        End If

        ' Pincode Presence and Format check
        If String.IsNullOrWhiteSpace(TextBox14.Text) Then
            MessageBox.Show("Please enter the Pincode", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox14.Focus()
            Return False
        ElseIf TextBox14.Text.Length <> 6 OrElse Not TextBox14.Text.All(Function(c) Char.IsDigit(c)) Then
            MessageBox.Show("Invalid Pincode", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox14.Focus()
            Return False
        ElseIf TextBox14.Text.StartsWith("0") Then
            MessageBox.Show("Pincode should not start with '0'", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox14.Focus()
            Return False
        ElseIf TextBox14.Text.GroupBy(Function(c) c).Any(Function(g) g.Count() > 3) Then
            MessageBox.Show("Pincode should not contain a digit repeated more than 3 times", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox14.Focus()
            Return False
        End If


        Return True
    End Function


    Private Sub ClearInputs()
        ' Clear all textboxes
        TextBox1.Text = "SG"
        TextBox2.Text = ""
        TextBox3.Text = ""
        DateTimePicker1.Value = DateTime.Today ' Clear DateTimePicker1
        TextBox5.Text = "+91"
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""
        TextBox12.Text = ""
        TextBox14.Text = ""

        ' Reset to default ComboBox1 (PhotoID)
        ComboBox1.SelectedIndex = 0
        ' Clear ComboBox2 (State)
        ComboBox2.SelectedIndex = -1
        'Clear Designation ComboBox
        ComboBox3.SelectedIndex = 0
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        ' Generate the next employee ID and display it in TextBox1
        TextBox1.Text = GenerateNextEmpIDFromDatabase()

        ' Disable TextBox1 to prevent editing
        TextBox1.Enabled = False
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
        If TextBox2.Text.Length > 0 AndAlso TextBox3.Text.Length > 0 Then
            ' Get the current selection start position
            Dim selectionStart As Integer = TextBox3.SelectionStart
            ' Capitalize the first letter
            TextBox3.Text = TextBox3.Text.Substring(0, 1).ToUpper() + TextBox3.Text.Substring(1)
            ' Restore the selection start position
            TextBox3.SelectionStart = selectionStart
        End If
    End Sub




    Private Sub TextBox10_TextChanged(sender As Object, e As EventArgs) Handles TextBox10.TextChanged
        If TextBox10.Text.Length > 0 Then
            ' Get the current selection start position
            Dim selectionStart As Integer = TextBox10.SelectionStart
            ' Capitalize the first letter
            TextBox10.Text = TextBox10.Text.Substring(0, 1).ToUpper() + TextBox10.Text.Substring(1)
            ' Restore the selection start position
            TextBox10.SelectionStart = selectionStart
        End If
    End Sub

    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
        If TextBox9.Text.Length > 0 Then
            ' Get the current selection start position
            Dim selectionStart As Integer = TextBox9.SelectionStart
            ' Capitalize the first letter
            TextBox9.Text = TextBox9.Text.Substring(0, 1).ToUpper() + TextBox9.Text.Substring(1)
            ' Restore the selection start position
            TextBox9.SelectionStart = selectionStart
        End If
    End Sub

    Private Sub TextBox12_TextChanged(sender As Object, e As EventArgs) Handles TextBox12.TextChanged
        If TextBox12.Text.Length > 0 Then
            ' Get the current selection start position
            Dim selectionStart As Integer = TextBox12.SelectionStart
            ' Capitalize the first letter
            TextBox12.Text = TextBox12.Text.Substring(0, 1).ToUpper() + TextBox12.Text.Substring(1)
            ' Restore the selection start position
            TextBox12.SelectionStart = selectionStart
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Clear all textboxes and reset controls
        TextBox1.Text = "SG"  ' Employee ID set to SG
        TextBox2.Text = ""  ' First Name
        TextBox3.Text = ""  ' Last Name
        DateTimePicker1.Value = DateTime.Today  ' DOB to current date
        TextBox5.Text = "+91" ' Phone number back to +91
        TextBox6.Text = ""  ' Alternate phone number
        TextBox7.Text = ""  ' Email ID
        TextBox9.Text = ""  ' House no/name
        TextBox10.Text = "" ' Street
        TextBox11.Text = "" ' City
        TextBox12.Text = "" ' District
        TextBox14.Text = "" ' Pincode

        ComboBox1.SelectedIndex = 0  ' Reset ComboBox1 (PhotoID) to default
        ComboBox2.SelectedIndex = -1 ' Clear ComboBox2 (State)
        'Clear Designation ComboBox
        ComboBox3.SelectedIndex = 0
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged

    End Sub

    Private Sub Label17_Click(sender As Object, e As EventArgs) Handles Label17.Click
        ' Exit the application
        Application.Exit()
    End Sub
End Class