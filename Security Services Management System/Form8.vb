Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form8

    ' Database connection string
    Private connectionString As String = "Data Source=DESKTOP-O16HO1G\SQLEXPRESS;Initial Catalog=Security Service Management System;Integrated Security=True;"

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        ' Clear TextBox2, TextBox5, TextBox6, and TextBox7
        TextBox2.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox1.Text = "SG"
        TextBox2.Text = Nothing
        TextBox5.Text = Nothing
        TextBox6.Text = Nothing
        TextBox7.Text = Nothing

        ' Set ComboBox1 to the current month
        ComboBox1.SelectedItem = DateTime.Now.ToString("MMMM")

        ' Set ComboBox2 to the current year
        ComboBox2.SelectedItem = DateTime.Now.Year.ToString()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Close the current form (Form8) and open the first form (Form3) again
        Dim form3 As New Form3()
        form3.Show()
        Me.Close()
    End Sub

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the form's AcceptButton property to Button2
        Me.AcceptButton = Button2

        ' Set TextBox2 as read-only
        TextBox2.ReadOnly = True
        ' Set TextBox5 as read-only
        TextBox5.ReadOnly = True
        ' Set TextBox6 as read-only
        TextBox6.ReadOnly = True
        ' Set TextBox7 as read-only
        TextBox7.ReadOnly = True

        ' Emp ID to start with "S"
        TextBox1.Text = "SG"

        ' Change the dropdown style of ComboBox1 and ComboBox2 to DropDownList
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList

        ' Populate ComboBox1 with all the months
        Dim months As String() = System.Globalization.DateTimeFormatInfo.InvariantInfo.MonthNames
        For Each month As String In months
            If Not String.IsNullOrEmpty(month) Then
                ComboBox1.Items.Add(month)
            End If
        Next

        ' Set ComboBox1 to display the current month
        ComboBox1.SelectedItem = DateTime.Now.ToString("MMMM")

        ' Populate ComboBox2 with the current year and the previous five years
        Dim currentYear As Integer = DateTime.Now.Year
        For i As Integer = 0 To 5
            ComboBox2.Items.Add((currentYear - i).ToString())
        Next

        ' Set ComboBox2 to display the current year
        ComboBox2.SelectedItem = currentYear.ToString()

    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Format for EMP ID
        Dim regexEmpID As New Regex("^SG\d+$")

        ' Emp ID Presence check
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("Enter Employee ID to Search", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
            ' Check if the Employee ID matches the pattern "SG" followed by numbers
        ElseIf Not regexEmpID.IsMatch(TextBox1.Text) Then
            MessageBox.Show("Invalid EMPLOYEE ID. Enter a valid Employee ID", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Exit Sub
        End If

        ' Check if the employee exists in Emp_table
        If Not EmployeeExists(TextBox1.Text) Then
            MessageBox.Show("Employee ID does not exist.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Get the selected month and year from comboboxes
        Dim selectedMonth As String = ComboBox1.SelectedItem.ToString()
        Dim selectedYear As String = ComboBox2.SelectedItem.ToString()

        ' Get the payment details for the selected month and year
        Dim paymentDetails As Tuple(Of String, String, String, String, String, String) = GetPaymentDetails(TextBox1.Text, selectedMonth, selectedYear)
        If paymentDetails Is Nothing Then
            MessageBox.Show($"Payment details not found for employee {TextBox1.Text} for {selectedMonth} {selectedYear}. Payment not generated for this EmpId.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        ' Display payment details
        TextBox2.Text = paymentDetails.Item1 ' Emp ID
        TextBox7.Text = paymentDetails.Item4 ' Designation
        TextBox6.Text = paymentDetails.Item5 ' Leaves taken
        TextBox5.Text = "Rs " & paymentDetails.Item6 ' Salary
    End Sub

    Private Function EmployeeExists(empId As String) As Boolean
        ' Query to check if the employee exists in Emp_table
        Dim query As String = "SELECT COUNT(*) FROM Emp_table WHERE empid = @empid"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                connection.Open()
                command.Parameters.AddWithValue("@empid", empId)
                ' Execute the query and get the count of records
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                ' If count is greater than 0, it means employee exists
                Return count > 0
            End Using
        End Using
    End Function

    Private Function GetPaymentDetails(empId As String, month As String, year As String) As Tuple(Of String, String, String, String, String, String)
        ' Query to fetch payment details for the employee for the selected month and year
        Dim query As String = "SELECT empid, month, year, designation, leaves, total_sal FROM payment_table WHERE empid = @empid AND month = @month AND year = @year"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                connection.Open()
                command.Parameters.AddWithValue("@empid", empId)
                command.Parameters.AddWithValue("@month", month)
                command.Parameters.AddWithValue("@year", year)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        ' If payment details found, return a tuple containing the details
                        Dim empIdResult As String = reader.GetString(0)
                        Dim monthResult As String = reader.GetString(1)
                        Dim yearResult As Integer = reader.GetInt32(2) ' Change here to GetInt32
                        Dim designation As String = reader.GetString(3)
                        Dim leaves As Byte = reader.GetByte(4)
                        Dim totalSal As Decimal = reader.GetDecimal(5)
                        Return Tuple.Create(empIdResult, monthResult, yearResult.ToString(), designation, leaves.ToString(), totalSal.ToString())
                    End If
                End Using
            End Using
        End Using
        ' If no payment details found, return Nothing
        Return Nothing
    End Function


    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Check if TextBox5 is empty
        If String.IsNullOrEmpty(TextBox5.Text) Then
            MessageBox.Show("Please enter a valid employee ID and search to retrieve payment details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox1.Focus()
            Return
        End If

        ' Generate PDF
        GeneratePDF()
    End Sub

    Private Sub GeneratePDF()
        ' Get the selected month and year from the ComboBoxes
        Dim selectedMonth As String = ComboBox1.SelectedItem.ToString()
        Dim selectedYear As String = ComboBox2.SelectedItem.ToString()

        ' Check if both month and year are selected
        If String.IsNullOrEmpty(selectedMonth) OrElse String.IsNullOrEmpty(selectedYear) Then
            MessageBox.Show("Please select both month and year.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Get payment details for the selected month and year
        Dim paymentDetails As Tuple(Of String, String, String, String, String, String) = GetPaymentDetails(TextBox1.Text, selectedMonth, selectedYear)

        If paymentDetails IsNot Nothing Then
            ' Create a new PDF document
            Dim doc As New Document()
            Dim directoryPath As String = "C:\PDF Generated"
            Dim fileName As String = $"PaymentDetails_{TextBox1.Text}.pdf"
            Dim filePath As String = Path.Combine(directoryPath, fileName)

            Try
                Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
                doc.Open()

                ' Add content to the PDF document
                Dim para As New Paragraph()
                para.Add("Employee ID: " & paymentDetails.Item1 & vbCrLf)
                para.Add("Month: " & paymentDetails.Item2 & vbCrLf)
                para.Add("Year: " & paymentDetails.Item3 & vbCrLf)
                para.Add("Designation: " & paymentDetails.Item4 & vbCrLf)
                para.Add("Leaves Taken: " & paymentDetails.Item5 & vbCrLf)
                para.Add("Salary: Rs " & paymentDetails.Item6 & vbCrLf)

                doc.Add(para)
            Catch ex As Exception
                MessageBox.Show("Error creating PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                doc.Close()
            End Try

            MessageBox.Show("PDF generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show($"No payment details found for the selected month ({selectedMonth}) and year ({selectedYear}).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If
    End Sub


    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
        ' Exit the application
        Application.Exit()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ' Exit the application
        Application.Exit()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ' Clear TextBox2, TextBox5, TextBox6, and TextBox7
        TextBox2.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        ' Clear TextBox2, TextBox5, TextBox6, and TextBox7
        TextBox2.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
    End Sub
End Class