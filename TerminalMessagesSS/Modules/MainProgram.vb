Imports Microsoft.Win32

Module MainProgram

   <STAThread()>
   Sub Main(args As String())
#If Not NETFRAMEWORK Then
      Application.SetHighDpiMode(HighDpiMode.PerMonitorV2)
#End If
      Application.EnableVisualStyles()
      Application.SetCompatibleTextRenderingDefault(False)

      ' Windows may send "/c:12345", so only look at the first 2 characters
      ' No arguments (e.g. right-click > Configure on the .scr) means settings, like other screensavers
      Dim mode As String = "/c"
      If args.Length > 0 AndAlso args(0).Length >= 2 Then
         mode = args(0).Substring(0, 2).ToLower()
      End If

      Select Case mode
         Case "/s"   ' run the screensaver
            RunScreensaver()
         Case "/c"   ' configuration
            ShowConfig()
         Case "/p"   ' preview in the Settings dialog - ignored for now
            ' do nothing
      End Select
   End Sub

   Private exiting As Boolean = False

   Private Sub RunScreensaver()
      Dim skin As Integer = -1 ' random skin
      Dim rnd As New Random()

      ' Use the saved skin only when "Random Skin" is unchecked
      Try
         Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(regAppPath)
            If key IsNot Nothing AndAlso Not CBool(key.GetValue("RandomSkin", True)) Then
               skin = CInt(key.GetValue("LoadSkin", 0))
            End If
         End Using
      Catch
         skin = -1   ' unreadable value: random skin
      End Try

      ' One full screen window per monitor
      For Each monitor As Screen In Screen.AllScreens
         ' No settings yet, random requested, or a bad value: each monitor gets its own random skin
         Dim monitorSkin As Integer = skin
         If monitorSkin < 0 OrElse monitorSkin >= skinList.Count Then
            monitorSkin = rnd.Next(skinList.Count)
         End If

         ' LoadSkin is the combo box index (0-4), which matches the TerminalSkin enum order
         Dim frm As New frmSS(CType(monitorSkin, TerminalSkin), monitor)
         AddHandler frm.FormClosed, AddressOf ScreensaverClosed
         frm.Show()
      Next

      Application.Run()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' ScreensaverClosed: input on any monitor closes the windows on all monitors
   Private Sub ScreensaverClosed(sender As Object, e As FormClosedEventArgs)
      If exiting Then Return
      exiting = True
      Application.Exit()
   End Sub

   Private Sub ShowConfig()
      Application.Run(New frmCfg)
   End Sub

End Module
