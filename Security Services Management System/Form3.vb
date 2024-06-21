
Imports System.Text
Imports System.Data
Imports System.Data.SqlClient


Public Class Form3

    ' SQL connection string
    Dim connectionString As String = "Data Source=DESKTOP-O16HO1G\SQLEXPRESS;Initial Catalog=Security Service Management System;Integrated Security=True;"



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Close and open employee registration form
        Dim form4 As New Form4()
        form4.Show()
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Close and open site registration form
        Dim form5 As New Form5()
        form5.Show()
        Me.Close()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ' Close the current form (Form2) and open the first form (Form1) again
        Dim form2 As New Form2()
        form2.Show()
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Close and open site assignment form
        Dim form6 As New Form6()
        form6.Show()
        Me.Close()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ' Close and open payment details form form
        Dim form8 As New Form8()
        form8.Show()
        Me.Close()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Close and open payment generation form form
        Dim form7 As New Form7()
        form7.Show()
        Me.Close()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ' Create an instance of Form2
        Dim form1 As New Form1()

        ' Show Form2
        form1.Show()

        ' Hide the current form
        Me.Hide()
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        ' Exit the application
        Application.Exit()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        ' Generate and display the report
        Dim reportContent As String = GenerateReport()
        MessageBox.Show(reportContent, "Report", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Function GenerateReport() As String
        Dim reportContent As New StringBuilder()

        Try
            ' Connect to the database
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Retrieve data from Emp_table
                Dim empQuery As String = "SELECT * FROM Emp_table"
                Dim empAdapter As New SqlDataAdapter(empQuery, connection)
                Dim empTable As New DataTable()
                empAdapter.Fill(empTable)

                ' Retrieve data from Site_table
                Dim siteQuery As String = "SELECT * FROM Site_table"
                Dim siteAdapter As New SqlDataAdapter(siteQuery, connection)
                Dim siteTable As New DataTable()
                siteAdapter.Fill(siteTable)

                ' Retrieve data from site_assignment_table
                Dim siteAssignmentQuery As String = "SELECT * FROM site_assignment_table"
                Dim siteAssignmentAdapter As New SqlDataAdapter(siteAssignmentQuery, connection)
                Dim siteAssignmentTable As New DataTable()
                siteAssignmentAdapter.Fill(siteAssignmentTable)

                ' Retrieve data from payment_table
                Dim paymentQuery As String = "SELECT * FROM payment_table"
                Dim paymentAdapter As New SqlDataAdapter(paymentQuery, connection)
                Dim paymentTable As New DataTable()
                paymentAdapter.Fill(paymentTable)

                ' Format the report
                reportContent.AppendLine("Employee Table:")
                For Each row As DataRow In empTable.Rows
                    reportContent.AppendLine($"Emp ID: {row("empid")}, Name: {row("efname")} {row("elname")}, Designation: {row("designation")}")
                Next

                ' Add an extra line gap
                reportContent.AppendLine()

                ' Add Site Table data to the report
                reportContent.AppendLine("Site Table:")
                For Each row As DataRow In siteTable.Rows
                    reportContent.AppendLine($"Site ID: {row("siteid")}, Name: {row("site_name")}, City: {row("site_city")}, Client: {row("cf_name")} {row("cl_name")}")
                Next

                ' Add an extra line gap
                reportContent.AppendLine()

                ' Add Site Assignment Table data to the report
                reportContent.AppendLine("Site Assignment Table:")
                For Each row As DataRow In siteAssignmentTable.Rows
                    Dim startDate As DateTime = Convert.ToDateTime(row("strt_date"))
                    Dim endDate As DateTime = Convert.ToDateTime(row("end_date"))

                    ' Format the dates without time
                    Dim formattedStartDate As String = startDate.ToString("yyyy-MM-dd")
                    Dim formattedEndDate As String = endDate.ToString("yyyy-MM-dd")

                    reportContent.AppendLine($"Emp ID: {row("empid")}, Site ID: {row("siteid")}, Start Date: {formattedStartDate}, End Date: {formattedEndDate}")
                Next

                ' Add an extra line gap
                reportContent.AppendLine()

                ' Add Payment Table data to the report
                reportContent.AppendLine("Payment Table:")
                For Each row As DataRow In paymentTable.Rows
                    reportContent.AppendLine($"Emp ID: {row("empid")}, Month: {row("month")}, Year: {row("year")}, Designation: {row("designation")}, Leaves: {row("leaves")}, Total Salary: {row("total_sal")}")
                Next
            End Using

        Catch ex As Exception
            MessageBox.Show("Error generating report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return reportContent.ToString()
    End Function



End Class