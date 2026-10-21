'--------------------------------------------------------------------------------------------------
' TerminalMessagesSS: AppData.vb
'    © 2026 Remus Rigo
'       v1.0.20261017
'--------------------------------------------------------------------------------------------------

Public Module AppData

   Public Const appName As String = "Terminal Messages SS"
   Public Const appVersion As String = "v1.1"
   Public Const appBuild As String = "202601017"
   Public Const appAuthor As String = "Remus Rigo"
   Public Const appLink As String = "© 2026 " & appAuthor
   Public Const appTitle As String = appName & " " & appVersion & "." & appBuild

   Public regAppPath As String = "Software\Remus RIGO\TerminalMessagesSS.VB-NET"

   Public skinList As New List(Of String) From {
      "Windows 3.11",
      "Windows 95",
      "Windows XP",
      "Windows 7",
      "Windows 11"
   }

   Public Enum TerminalSkin
      Windows311   ' Windows 3.11 "MS-DOS Prompt": grey frame, navy title bar, caption buttons
      Windows95    ' Windows 95: raised 3D frame ("Desert" colors), teal title bar, 3D caption buttons
      WindowsXP    ' Windows XP (classic style): beige 3D frame, blue gradient title bar, 3D caption buttons
      Windows7     ' Windows 7 Aero: slate glass frame, rounded corners, glass caption buttons, navy console
      Windows11    ' Windows 11: light title bar with caption buttons, rounded corners
   End Enum

End Module
