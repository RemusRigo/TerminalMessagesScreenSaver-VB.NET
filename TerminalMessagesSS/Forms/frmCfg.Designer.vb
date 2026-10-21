<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCfg
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCfg))
      btnOk = New Button()
      chkBoxRandomMessages = New CheckBox()
      txtBoxMessagePath = New TextBox()
      btnBrowse = New Button()
      lstBoxMessages = New ListBox()
      grpBoxSkin = New GroupBox()
      chkBoxRandomSkin = New CheckBox()
      cmbBoxSkins = New ComboBox()
      grpBoxSkin.SuspendLayout()
      SuspendLayout()
      ' 
      ' btnOk
      ' 
      btnOk.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
      btnOk.Location = New Point(847, 425)
      btnOk.Name = "btnOk"
      btnOk.Size = New Size(35, 23)
      btnOk.TabIndex = 0
      btnOk.Text = "Ok"
      btnOk.UseVisualStyleBackColor = True
      ' 
      ' chkBoxRandomMessages
      ' 
      chkBoxRandomMessages.AutoSize = True
      chkBoxRandomMessages.Location = New Point(12, 3)
      chkBoxRandomMessages.Name = "chkBoxRandomMessages"
      chkBoxRandomMessages.Size = New Size(125, 19)
      chkBoxRandomMessages.TabIndex = 2
      chkBoxRandomMessages.Text = "Random Messages"
      chkBoxRandomMessages.UseVisualStyleBackColor = True
      ' 
      ' txtBoxMessagePath
      ' 
      txtBoxMessagePath.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
      txtBoxMessagePath.Location = New Point(12, 426)
      txtBoxMessagePath.Name = "txtBoxMessagePath"
      txtBoxMessagePath.Size = New Size(649, 23)
      txtBoxMessagePath.TabIndex = 3
      ' 
      ' btnBrowse
      ' 
      btnBrowse.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
      btnBrowse.Location = New Point(667, 425)
      btnBrowse.Name = "btnBrowse"
      btnBrowse.Size = New Size(60, 23)
      btnBrowse.TabIndex = 4
      btnBrowse.Text = "Browse"
      btnBrowse.UseVisualStyleBackColor = True
      ' 
      ' lstBoxMessages
      ' 
      lstBoxMessages.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
      lstBoxMessages.FormattingEnabled = True
      lstBoxMessages.Location = New Point(12, 28)
      lstBoxMessages.Name = "lstBoxMessages"
      lstBoxMessages.Size = New Size(649, 394)
      lstBoxMessages.TabIndex = 5
      ' 
      ' grpBoxSkin
      ' 
      grpBoxSkin.Controls.Add(cmbBoxSkins)
      grpBoxSkin.Controls.Add(chkBoxRandomSkin)
      grpBoxSkin.Location = New Point(667, 28)
      grpBoxSkin.Name = "grpBoxSkin"
      grpBoxSkin.Size = New Size(215, 82)
      grpBoxSkin.TabIndex = 7
      grpBoxSkin.TabStop = False
      grpBoxSkin.Text = "Skins"
      ' 
      ' chkBoxRandomSkin
      ' 
      chkBoxRandomSkin.AutoSize = True
      chkBoxRandomSkin.Location = New Point(6, 22)
      chkBoxRandomSkin.Name = "chkBoxRandomSkin"
      chkBoxRandomSkin.Size = New Size(96, 19)
      chkBoxRandomSkin.TabIndex = 7
      chkBoxRandomSkin.Text = "Random Skin"
      chkBoxRandomSkin.UseVisualStyleBackColor = True
      ' 
      ' cmbBoxSkins
      ' 
      cmbBoxSkins.FormattingEnabled = True
      cmbBoxSkins.Location = New Point(6, 47)
      cmbBoxSkins.Name = "cmbBoxSkins"
      cmbBoxSkins.Size = New Size(203, 23)
      cmbBoxSkins.TabIndex = 8
      ' 
      ' frmCfg
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(884, 451)
      Controls.Add(grpBoxSkin)
      Controls.Add(lstBoxMessages)
      Controls.Add(btnBrowse)
      Controls.Add(txtBoxMessagePath)
      Controls.Add(chkBoxRandomMessages)
      Controls.Add(btnOk)
      Icon = CType(resources.GetObject("$this.Icon"), Icon)
      Name = "frmCfg"
      StartPosition = FormStartPosition.CenterScreen
      Text = "Settings"
      grpBoxSkin.ResumeLayout(False)
      grpBoxSkin.PerformLayout()
      ResumeLayout(False)
      PerformLayout()
   End Sub

   Friend WithEvents btnOk As Button
   Friend WithEvents chkBoxRandomMessages As CheckBox
   Friend WithEvents txtBoxMessagePath As TextBox
   Friend WithEvents btnBrowse As Button
   Friend WithEvents lstBoxMessages As ListBox
   Friend WithEvents grpBoxSkin As GroupBox
   Friend WithEvents chkBoxRandomSkin As CheckBox
   Friend WithEvents cmbBoxSkins As ComboBox
End Class
