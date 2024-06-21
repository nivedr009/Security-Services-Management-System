<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form3))
        Button1 = New Button()
        Label1 = New Label()
        Button2 = New Button()
        Button3 = New Button()
        Button4 = New Button()
        Button5 = New Button()
        Button6 = New Button()
        GroupBox1 = New GroupBox()
        Button7 = New Button()
        Label4 = New Label()
        Button8 = New Button()
        GroupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.BackColor = SystemColors.HotTrack
        Button1.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button1.ForeColor = SystemColors.ControlLightLight
        Button1.Location = New Point(19, 26)
        Button1.Name = "Button1"
        Button1.Size = New Size(214, 39)
        Button1.TabIndex = 0
        Button1.Text = "Employee Registration"
        Button1.UseVisualStyleBackColor = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI Black", 13.8F, FontStyle.Bold Or FontStyle.Italic, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(324, 37)
        Label1.Name = "Label1"
        Label1.Size = New Size(165, 31)
        Label1.TabIndex = 2
        Label1.Text = "DASHBOARD"
        ' 
        ' Button2
        ' 
        Button2.BackColor = SystemColors.MenuHighlight
        Button2.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button2.ForeColor = SystemColors.ControlLightLight
        Button2.Location = New Point(19, 88)
        Button2.Name = "Button2"
        Button2.Size = New Size(214, 37)
        Button2.TabIndex = 3
        Button2.Text = "Site Registration"
        Button2.UseVisualStyleBackColor = False
        ' 
        ' Button3
        ' 
        Button3.BackColor = SystemColors.HotTrack
        Button3.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button3.ForeColor = SystemColors.ControlLightLight
        Button3.Location = New Point(19, 149)
        Button3.Name = "Button3"
        Button3.Size = New Size(214, 41)
        Button3.TabIndex = 4
        Button3.Text = "Site Assignment"
        Button3.UseVisualStyleBackColor = False
        ' 
        ' Button4
        ' 
        Button4.BackColor = SystemColors.MenuHighlight
        Button4.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button4.ForeColor = SystemColors.ControlLightLight
        Button4.Location = New Point(19, 211)
        Button4.Name = "Button4"
        Button4.Size = New Size(214, 40)
        Button4.TabIndex = 5
        Button4.Text = "Payroll Generation"
        Button4.UseVisualStyleBackColor = False
        ' 
        ' Button5
        ' 
        Button5.BackColor = SystemColors.HotTrack
        Button5.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button5.ForeColor = SystemColors.ControlLightLight
        Button5.Location = New Point(19, 272)
        Button5.Name = "Button5"
        Button5.Size = New Size(214, 40)
        Button5.TabIndex = 6
        Button5.Text = "Payment Details"
        Button5.UseVisualStyleBackColor = False
        ' 
        ' Button6
        ' 
        Button6.BackColor = SystemColors.Highlight
        Button6.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button6.ForeColor = SystemColors.ControlLightLight
        Button6.Location = New Point(681, 350)
        Button6.Name = "Button6"
        Button6.Size = New Size(107, 33)
        Button6.TabIndex = 7
        Button6.Text = "Back"
        Button6.UseVisualStyleBackColor = False
        ' 
        ' GroupBox1
        ' 
        GroupBox1.BackColor = Color.Transparent
        GroupBox1.Controls.Add(Button1)
        GroupBox1.Controls.Add(Button2)
        GroupBox1.Controls.Add(Button5)
        GroupBox1.Controls.Add(Button3)
        GroupBox1.Controls.Add(Button4)
        GroupBox1.Location = New Point(276, 71)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(250, 326)
        GroupBox1.TabIndex = 8
        GroupBox1.TabStop = False
        GroupBox1.Text = " "
        ' 
        ' Button7
        ' 
        Button7.BackColor = SystemColors.HotTrack
        Button7.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button7.ForeColor = SystemColors.ControlLightLight
        Button7.Location = New Point(681, 389)
        Button7.Name = "Button7"
        Button7.Size = New Size(107, 49)
        Button7.TabIndex = 9
        Button7.Text = "Logout"
        Button7.UseVisualStyleBackColor = False
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label4.ForeColor = Color.Red
        Label4.Location = New Point(771, 4)
        Label4.Name = "Label4"
        Label4.Size = New Size(25, 28)
        Label4.TabIndex = 19
        Label4.Text = "X"
        ' 
        ' Button8
        ' 
        Button8.BackColor = SystemColors.HotTrack
        Button8.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button8.ForeColor = SystemColors.ControlLightLight
        Button8.Location = New Point(12, 11)
        Button8.Name = "Button8"
        Button8.Size = New Size(105, 50)
        Button8.TabIndex = 20
        Button8.Text = "Report"
        Button8.UseVisualStyleBackColor = False
        ' 
        ' Form3
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), Image)
        BackgroundImageLayout = ImageLayout.Stretch
        ClientSize = New Size(800, 450)
        Controls.Add(Button8)
        Controls.Add(Label4)
        Controls.Add(Button7)
        Controls.Add(GroupBox1)
        Controls.Add(Button6)
        Controls.Add(Label1)
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.None
        Name = "Form3"
        Text = "Form3"
        GroupBox1.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button7 As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Button8 As Button
End Class
