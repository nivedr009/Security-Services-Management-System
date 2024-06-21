Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.SqlClient
Public Class Form7

    ' Database connection string
    Private connectionString As String = "Data Source=DESKTOP-O16HO1G\SQLEXPRESS;Initial Catalog=Security Service Management System;Integrated Security=True;"


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Close the current form (Form7) and open the first form (Form3) again
        Dim form3 As New Form3()
        form3.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ' Additional check: Ensure that salary has not already been generated for the same employee for the same month and year
        Dim empId As String = TextBox1.Text
        Dim month As String = ComboBox1.SelectedItem.ToString()
        Dim year As String = ComboBox2.SelectedItem.ToString()
        Dim designation As String = TextBox3.Text

        Dim regexEmpID As New Regex("^SG\d+$")

        ' Employee ID Presence Check
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("Employee ID cannot be blank", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
        ElseIf Not regexEmpID.IsMatch(TextBox1.Text) Then
            MessageBox.Show("Please enter a valid Employee ID", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
        End If

        ' Check if TextBox2 (total salary) is not empty
        If String.IsNullOrEmpty(TextBox2.Text) Then
            MessageBox.Show("Please calculate the total salary before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check if the employee ID exists in the Emp_table
        If Not EmployeeExists(TextBox1.Text) Then
            MessageBox.Show("Employee ID not Registered. Please register the Employee.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
        End If

        ' Check if the employee ID exists in the site_assignment_table
        If Not EmployeeAssigned(TextBox1.Text) Then
            MessageBox.Show("Employee ID is not assigned to any site.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
        End If

        If SalaryAlreadyGenerated(empId, month, year) Then
            MessageBox.Show("Salary has already been generated for the same employee for the same month and year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
        End If

        ' If all validations pass, proceed with saving
        SaveToPaymentTable()
        ' Clear textboxes and reset ComboBoxes
        If MessageBox.Show("Salary Generated and Saved Successfully") Then
            ClearInputs()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        ' Only proceed if the TextBox1 contains text and it is different from the default value "SG"
        If TextBox1.Text.Trim().Length > "SG".Length Then
            ' Validate the employee ID format
            Dim empId As String = TextBox1.Text.Trim()
            Dim regexEmpID As New Regex("^SG\d+$")

            If Not regexEmpID.IsMatch(empId) Then
                MessageBox.Show("Please enter a valid Employee ID", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox1.Focus()
                TextBox3.Text = "" ' Clear TextBox3 if the ID is invalid
            Else
                ' If the format is valid, proceed to fetch the designation
                Dim designation As String = FetchDesignation(empId)
                If Not String.IsNullOrEmpty(designation) Then
                    ' Set the designation in TextBox3
                    TextBox3.Text = designation
                Else
                    ' If designation is not found, clear TextBox3
                    TextBox3.Text = ""
                End If
            End If
        Else
            ' If TextBox1 is empty or contains the default value "SG", clear TextBox3
            TextBox3.Text = ""
        End If
    End Sub

    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set the form's AcceptButton property to Button1
        Me.AcceptButton = Button1

        ' Set TextBox2 as read-only
        TextBox2.ReadOnly = True

        ' Emp ID to start with "S"
        TextBox1.Text = "SG"

        ' Disable TextBox3
        TextBox3.ReadOnly = True


        ' Get the current month
        Dim currentMonth As Integer = DateTime.Now.Month

        ' Add the range of months to the ComboBox (current month - 1, current month, current month + 1)
        For month As Integer = currentMonth - 1 To currentMonth + 1
            ' Adjust the month to ensure it falls within the range of 1 to 12
            Dim adjustedMonth As Integer = If(month <= 0, month + 12, If(month > 12, month - 12, month))

            ' Add the month name to the ComboBox
            ComboBox1.Items.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(adjustedMonth))
        Next

        ' Set the default selected index to the current month (which is the second item in the list)
        ComboBox1.SelectedIndex = 1

        ' Select Current Year
        Dim currentYear As Integer = DateTime.Now.Year
        ' Add the range of years to the ComboBox (current year - 1, current year, current year + 1)
        For year As Integer = currentYear - 1 To currentYear + 1
            ComboBox2.Items.Add(year.ToString())
        Next
        ' Set the default selected index to the current year
        ComboBox2.SelectedIndex = 1 ' Index 1 corresponds to the current year


        ' Add numbers from 0 to 7 to the leaves combo box
        For i As Integer = 0 To 7
            ComboBox3.Items.Add(i.ToString())
        Next
        ' Set the default selected index as o
        ComboBox3.SelectedIndex = 0 ' Selecting 0 by default

        'Disable manual entry into comboBox
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        'Format for EMP ID
        Dim regexEmpID As New Regex("^SG\d+$")

        ' Define salaries for different positions
        Dim salaries As New Dictionary(Of String, Integer) From
        {
            {"Security Guard", 18000},
            {"Lady Guard", 17000},
            {"Field Officer", 22000},
            {"Supervisor", 30000}
        }

        ' Get selected position from TextBox3
        Dim selectedPosition As String = TextBox3.Text.Trim()

        ' Employee ID Presence Check
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("Employee ID cannot be blank", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            ' Check if the Employee ID matches the pattern "SG" followed by numbers
        ElseIf Not regexEmpID.IsMatch(TextBox1.Text) Then
            MessageBox.Show("Please enter a valid Employee ID", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()

            ' Check if designation is selected from ComboBox4
        ElseIf String.IsNullOrWhiteSpace(selectedPosition) Then
            MessageBox.Show("Employee is Not registered", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()

            ' Check if leaves are selected in ComboBox3
        ElseIf ComboBox3.SelectedIndex >= 0 Then
            ' Get the number of leaves from ComboBox3
            Dim leaves As Integer = ComboBox3.SelectedIndex

            ' Calculate deduction for leaves (assuming 500 Rs per leave)
            Dim leaveDeduction As Integer = leaves * 500

            ' Retrieve the corresponding salary based on the selected position
            Dim salary As Integer = salaries(selectedPosition)

            ' Subtract leave deduction from salary only if leaves are not zero
            If leaves > 0 Then
                salary -= leaveDeduction
            End If

            ' Display the final salary in TextBox2
            TextBox2.Text = "Rs " & salary.ToString()
            TextBox1.Enabled = False
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            ComboBox3.Enabled = False

        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ClearInputs()
    End Sub

    ' Function to clear input fields and reset ComboBoxes
    Private Sub ClearInputs()
        ' Set Employee ID as SG
        TextBox1.Text = "SG"

        ' Set the month ComboBox to the current month
        ComboBox1.SelectedIndex = 1

        ' Set the year ComboBox to the current year
        ComboBox2.SelectedItem = DateTime.Now.Year.ToString()

        TextBox3.Text = ""

        ' Set the number of leaves ComboBox to the first option
        ComboBox3.SelectedIndex = 0

        ' Clear the total salary TextBox
        TextBox2.Text = ""

        ' Enable textboxes and comboboxes
        TextBox1.Enabled = True
        ComboBox1.Enabled = True
        ComboBox2.Enabled = True
        ComboBox3.Enabled = True
    End Sub

    ' Function to check if an employee exists in the Emp_table
    Private Function EmployeeExists(empId As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM Emp_table WHERE empid = @empid"
        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                connection.Open()
                command.Parameters.AddWithValue("@empid", empId)
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    ' Function to check if an employee is assigned to any site in the site_assignment_table
    Private Function EmployeeAssigned(empId As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM site_assignment_table WHERE empid = @empid"
        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                connection.Open()
                command.Parameters.AddWithValue("@empid", empId)
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function


    ' Function to save data to the payment_table
    Private Sub SaveToPaymentTable()
        ' Extract data from controls
        Dim empid As String = TextBox1.Text
        Dim month As String = ComboBox1.SelectedItem.ToString()
        Dim year As Integer = Convert.ToInt32(ComboBox2.SelectedItem.ToString())
        Dim designation As String = TextBox3.Text
        Dim leaves As Integer = ComboBox3.SelectedIndex
        Dim totalSal As Decimal = Decimal.Parse(TextBox2.Text.Trim().Replace("Rs ", ""), CultureInfo.InvariantCulture)

        ' SQL query to insert data into payment_table
        Dim query As String = "INSERT INTO [dbo].[payment_table] ([empid], [month], [year], [designation], [leaves], [total_sal]) VALUES (@empid, @month, @year, @designation, @leaves, @total_sal)"

        ' Create connection and command objects
        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                ' Add parameters
                command.Parameters.AddWithValue("@empid", empid)
                command.Parameters.AddWithValue("@month", month)
                command.Parameters.AddWithValue("@year", year)
                command.Parameters.AddWithValue("@designation", designation)
                command.Parameters.AddWithValue("@leaves", leaves)
                command.Parameters.AddWithValue("@total_sal", totalSal)

                ' Open connection and execute query
                connection.Open()
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Function SalaryAlreadyGenerated(empId As String, month As String, year As String) As Boolean
        ' Query to check if salary has already been generated for the same employee for the same month and year
        Dim query As String = "SELECT COUNT(*) FROM payment_table WHERE empid = @empid AND [month] = @month AND [year] = @year"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                connection.Open()
                command.Parameters.AddWithValue("@empid", empId)
                command.Parameters.AddWithValue("@month", month)
                command.Parameters.AddWithValue("@year", year)

                ' Execute the query and get the count of records
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())

                ' If count is greater than 0, it means salary has already been generated
                Return count > 0
            End Using
        End Using
    End Function

    Private Function FetchDesignation(empId As String) As String
        Dim query As String = "SELECT designation FROM Emp_table WHERE empid = @empid"
        Dim designation As String = String.Empty

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@empid", empId)
                Try
                    connection.Open()
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing Then
                        designation = Convert.ToString(result)
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error while fetching designation: " & ex.Message)
                End Try
            End Using
        End Using

        Return designation
    End Function

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        ' Exit the application
        Application.Exit()
    End Sub
End Class