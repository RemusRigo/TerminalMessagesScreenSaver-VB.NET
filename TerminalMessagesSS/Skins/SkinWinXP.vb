'--------------------------------------------------------------------------------------------------
' TerminalMessagesSS: SkinBase.vb
'    © 2026 Remus Rigo
'       v1.0.20261017
'
' Windows XP cmd.exe (classic style): beige 3D frame, blue gradient title bar, 3D caption buttons
'--------------------------------------------------------------------------------------------------

Public Class SkinWinXP
   Inherits SkinWin3D

   Public Sub New(px As Integer)
      MyBase.New(px)
      titleBackColor = Color.FromArgb(&H0, &H54, &HE3)
      titleBackColor2 = Color.FromArgb(&H3D, &H95, &HFF)
      windowTitle = "C:\WINDOWS\system32\cmd.exe"
      uiFont = New Font("Tahoma", 10.0F * px, FontStyle.Bold)
      c3dFace = Color.FromArgb(&HEC, &HE9, &HD8)
      c3dLight = c3dFace
      c3dHighlight = Color.White
      c3dShadow = Color.FromArgb(&HAC, &HA8, &H99)
      c3dDark = Color.FromArgb(&H71, &H6F, &H64)
      c3dIconStrip = Color.FromArgb(&H3D, &H95, &HFF)
      titleHeight = 25
      buttonW = 19
      buttonH = 21
   End Sub

End Class
