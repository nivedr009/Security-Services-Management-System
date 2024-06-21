Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.SqlClient

Public Class Form6

    ' Database connection string
    Private connectionString As String = "Data Source=DESKTOP-O16HO1G\SQLEXPRESS;Initial Catalog=Security Service Management System;Integrated Security=True;"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ' Format for EMP ID
        Dim regexEmpID As New Regex("^SG\d+$")

        ' Format for Site ID
        Dim regexSiteID As New Regex("^S\d+$")

        ' Initialising for Start date and End date
        Dim startDate As Date
        Dim endDate As Date

        ' Employee ID Presence Check
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("Employee ID cannot be blank", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
        ElseIf Not regexEmpID.IsMatch(TextBox1.Text) Then
            MessageBox.Show("Invalid EMPLOYEE ID", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
        End If

        ' Site ID Presence Check
        If String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MessageBox.Show("Site ID cannot be blank", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox2.Focus()
            Exit Sub
        ElseIf Not regexSITEID.IsMatch(TextBox2.Text) Then
            MessageBox.Show("Invalid SITE ID", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox2.Focus()
            Exit Sub
        End If

        ' Check if both start date and end date are selected
        If Not DateTimePicker1.Checked OrElse Not DateTimePicker2.Checked Then
            MessageBox.Show("Both Start date and End date are required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Extract start and end dates from DateTimePicker controls
        startDate = DateTimePicker1.Value.Date
        endDate = DateTimePicker2.Value.Date

        ' Check if Start Date is after End Date or same as End Date
        If startDate >= endDate Then
            MessageBox.Show("Start Date cannot be equal to or after End Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Define duration 
        Dim duration As TimeSpan = endDate - startDate

        ' Check if Start Date is in the past
        If startDate < Date.Today Then
            MessageBox.Show("Start Date should not be in the past.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check If the duration Is at least 1 month
        If duration.TotalDays < 30 Then
            MessageBox.Show("The duration between Start Date and End Date should be at least 1 month.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check if End Date is more than 1 year from the Start Date
        If endDate > startDate.AddYears(1) Then
            MessageBox.Show("End Date should not be more than 1 year from the Start Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check if Employee ID exists
        If Not EmployeeExists(TextBox1.Text) Then
            MessageBox.Show("Employee ID does not exist.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
        End If

        ' Check if Site ID exists
        If Not SiteExists(TextBox2.Text) Then
            MessageBox.Show("Site ID does not exist.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox2.Focus()
            Exit Sub
        End If

        ' Check for overlapping assignments
        If AssignmentOverlap(TextBox1.Text, startDate, endDate) Then
            Exit Sub
        End If

        ' If all validations pass, insert data into the database
        If InsertAssignment(TextBox1.Text, TextBox2.Text, startDate, endDate) Then
            MessageBox.Show("Saved Successfully")
            ' Clear all textboxes and reset DateTimePicker controls
            ClearInputs()
        Else
            MessageBox.Show("Error occurred while saving data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ' Function to check if an employee exists
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

    ' Function to check if a site exists
    Private Function SiteExists(siteId As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM Site_table WHERE siteid = @siteid"
        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                connection.Open()
                command.Parameters.AddWithValue("@siteid", siteId)
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    ' Function to check for overlapping assignments
    Private Function AssignmentOverlap(empId As String, startDate As Date, endDate As Date) As Boolean
        Dim query As String = "SELECT COUNT(*), MAX(end_date) FROM site_assignment_table WHERE empid = @empid AND ((strt_date <= @endDate AND end_date >= @startDate) OR (strt_date >= @startDate AND end_date <= @endDate))"
        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                connection.Open()
                command.Parameters.AddWithValue("@empid", empId)
                command.Parameters.AddWithValue("@startDate", startDate)
                command.Parameters.AddWithValue("@endDate", endDate)
                Dim reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    Dim count As Integer = Convert.ToInt32(reader(0))
                    If count > 0 Then
                        Dim overlappingEndDate As Date = Convert.ToDateTime(reader(1))
                        MessageBox.Show("There is already an assignment for this employee within the specified date range. Overlapping assignment ends on: " & overlappingEndDate.ToShortDateString(), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return True
                    End If
                End If
            End Using
        End Using
        Return False
    End Function

    ' Function to insert assignment into the database
    Private Function InsertAssignment(empId As String, siteId As String, startDate As Date, endDate As Date) As Boolean
        Dim query As String = "INSERT INTO site_assignment_table (empid, siteid, strt_date, end_date) VALUES (@empid, @siteid, @startDate, @endDate)"
        Try
            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(query, connection)
                    connection.Open()
                    ' Add parameters with Trim() function applied
                    command.Parameters.AddWithValue("@empid", empId.Trim())
                    command.Parameters.AddWithValue("@siteid", siteId.Trim())
                    command.Parameters.AddWithValue("@startDate", startDate)
                    command.Parameters.AddWithValue("@endDate", endDate)
                    Return command.ExecuteNonQuery() > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error inserting assignment: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function


    ' Function to clear all input fields
    Private Sub ClearInputs()
        TextBox1.Text = "SG"
        TextBox2.Text = "S"
        DateTimePicker1.Value = Date.Today
        DateTimePicker2.Value = Date.Today
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Close the current form (Form6) and open the first form (Form3) again
        Dim form3 As New Form3()
        form3.Show()
        Me.Close()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged

    End Sub

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set the form's AcceptButton property to Button1
        Me.AcceptButton = Button1

        ' Emp ID to start with "SG"
        TextBox1.Text = "SG"

        ' Site ID to start with "S"
        TextBox2.Text = "S"

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Reset Employee ID to start with "SG"
        TextBox1.Text = "SG"

        ' Reset Site ID to start with "S"
        TextBox2.Text = "S"

        ' Reset Start and End Date to current date
        DateTimePicker1.Value = Date.Today
        DateTimePicker2.Value = Date.Today
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        ' Exit the application
        Application.Exit()
    End Sub
End Class