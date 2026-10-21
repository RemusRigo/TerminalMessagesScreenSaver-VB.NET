'--------------------------------------------------------------------------------------------------
' TerminalMessagesSS: frmCfg.vb: Settings form
'    © 2026 Remus Rigo
'       v1.0.20261017
'--------------------------------------------------------------------------------------------------

Imports Microsoft.Win32
Imports System.IO
Imports TerminalMessagesSS.API

Public Class frmCfg

   Private Const SYSMENU_ABOUT_ID As UInteger = 1000

   Private scannedPath As String = ""   ' folder the file list was filled from

   Protected Overrides Sub OnHandleCreated(e As EventArgs)
      MyBase.OnHandleCreated(e)
      Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
      ' Add a separator and then your custom item
      AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
      AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "About...")
   End Sub

   Protected Overrides Sub WndProc(ByRef m As Message)
      MyBase.WndProc(m)
      If m.Msg = WM_SYSCOMMAND Then
         If CUInt(m.WParam) = SYSMENU_ABOUT_ID Then
            frmAbout.ShowDialog()
         End If
      End If
   End Sub


   Public Sub LoadSettings()
      ' Defaults, used when the key or a value is missing (first run)
      Dim randomMessages As Boolean = True
      Dim randomSkin As Boolean = True
      Dim loadSkin As Integer = 0
      Dim msgPath As String = ""
      Dim msgFile As String = ""

      Try
         Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(regAppPath)
            If key IsNot Nothing Then
               randomMessages = CBool(key.GetValue("RandomMessages", True))
               msgPath = CStr(key.GetValue("msgPath", ""))
               msgFile = CStr(key.GetValue("msgFile", ""))
               randomSkin = CBool(key.GetValue("RandomSkin", True))
               loadSkin = CInt(key.GetValue("LoadSkin", 0))
            End If
         End Using
      Catch
         ' unreadable value: keep the defaults
      End Try

      If String.IsNullOrWhiteSpace(msgPath) Then msgPath = Path.Combine(Application.StartupPath, "Messages")
      If loadSkin < 0 OrElse loadSkin >= skinList.Count Then loadSkin = 0

      chkBoxRandomMessages.Checked = randomMessages
      chkBoxRandomSkin.Checked = randomSkin
      cmbBoxSkins.SelectedIndex = loadSkin
      txtBoxMessagePath.Text = msgPath

      ' The list must be filled before the saved file can be selected
      ScanMessages(msgPath, showError:=False)


      ' check if file from registry exists, if not fall back to random messages
      Dim index As Integer = If(msgFile = "", ListBox.NoMatches, lstBoxMessages.FindStringExact(msgFile))
      If index <> ListBox.NoMatches Then
         lstBoxMessages.SelectedIndex = index
      Else
         chkBoxRandomMessages.Checked = True
      End If

   End Sub

   Public Sub SaveSettings()
      ' Path typed but not scanned yet: the list (and the selected file) must belong to the saved path
      If txtBoxMessagePath.Text <> scannedPath Then ScanMessages(txtBoxMessagePath.Text)

      Try
         Dim msgFile As String = If(lstBoxMessages.SelectedItem?.ToString(), "")
         Using key As RegistryKey = Registry.CurrentUser.CreateSubKey(regAppPath)
            key.SetValue("RandomMessages", chkBoxRandomMessages.Checked OrElse msgFile = "", RegistryValueKind.DWord)   ' no file selected: random
            key.SetValue("msgPath", txtBoxMessagePath.Text, RegistryValueKind.String)
            key.SetValue("msgFile", msgFile, RegistryValueKind.String)
            key.SetValue("RandomSkin", chkBoxRandomSkin.Checked, RegistryValueKind.DWord)
            key.SetValue("LoadSkin", cmbBoxSkins.SelectedIndex, RegistryValueKind.DWord)
         End Using
         Me.DialogResult = DialogResult.OK
         Me.Close()
      Catch ex As Exception
         MessageBox.Show($"Error occurred while saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
         Me.DialogResult = DialogResult.None
      End Try
   End Sub



   Private Sub ScanMessages(msgPath As String, Optional showError As Boolean = True)
      Dim rootPath As String = msgPath

      ' Clear first, so the list never shows the files of a previous folder
      lstBoxMessages.Items.Clear()
      scannedPath = msgPath

      ' Check if the specified folder exists
      If Not System.IO.Directory.Exists(rootPath) Then
         If showError Then MessageBox.Show($"The specified folder does not exist: {rootPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
         Return
      End If

      ' Ensure the root path ends with a separator for clean path trimming
      If Not rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()) Then
         rootPath &= Path.DirectorySeparatorChar
      End If

      ' Get all JSON files recursively from the selected folder and subfolders
      Dim jsonFiles As String()
      Try
         jsonFiles = Directory.GetFiles(rootPath, "*.json", SearchOption.AllDirectories)
      Catch ex As Exception
         If showError Then MessageBox.Show($"Cannot read the folder {rootPath}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
         Return
      End Try

      For Each filePath As String In jsonFiles
         ' Strip the root path to get the relative path structure
         Dim relativePath As String = filePath.Substring(rootPath.Length)
         lstBoxMessages.Items.Add(relativePath)
      Next
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmCfg: OnLoad
   Private Sub frmCfg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      cmbBoxSkins.DataSource = skinList
      LoadSettings()
   End Sub

   Private Sub chkBoxRandomMessages_CheckedChanged(sender As Object, e As EventArgs) Handles chkBoxRandomMessages.CheckedChanged
      lstBoxMessages.Enabled = Not chkBoxRandomMessages.Checked
   End Sub

   Private Sub txtBoxMessagePath_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBoxMessagePath.KeyDown
      If e.KeyCode = Keys.Enter Then ScanMessages(txtBoxMessagePath.Text)
   End Sub

   Private Sub txtBoxMessagePath_Leave(sender As Object, e As EventArgs) Handles txtBoxMessagePath.Leave
      If txtBoxMessagePath.Text <> scannedPath Then ScanMessages(txtBoxMessagePath.Text)
   End Sub


   Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
      Dim dlgBrowse As New FolderBrowserDialog()
      dlgBrowse.Description = "Select the folder containing the messages"
      dlgBrowse.InitialDirectory = Application.StartupPath
      dlgBrowse.ShowNewFolderButton = True
      If dlgBrowse.ShowDialog() = DialogResult.OK Then
         txtBoxMessagePath.Text = dlgBrowse.SelectedPath
         ScanMessages(dlgBrowse.SelectedPath)
      End If
   End Sub

   Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOk.Click
      SaveSettings()
   End Sub

End Class