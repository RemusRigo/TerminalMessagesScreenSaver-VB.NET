'--------------------------------------------------------------------------------------------------
' TerminalMessagesSS: SkinBase.vb
'    © 2026 Remus Rigo
'       v1.0.20261017
'
' Windows 95 MS-DOS Prompt: raised 3D frame ("Desert" colors), teal title bar, 3D caption buttons
'--------------------------------------------------------------------------------------------------

Public Class SkinWin95
   Inherits SkinWin3D

   Public Sub New(px As Integer)
      MyBase.New(px)
      titleBackColor = Color.FromArgb(0, 128, 128)
      titleBackColor2 = titleBackColor
      windowTitle = "MS-DOS Prompt"
      uiFont = New Font("Microsoft Sans Serif", 8.25F * px, FontStyle.Bold)
      c3dFace = Color.FromArgb(&HD5, &HCC, &HBB)
      c3dLight = c3dFace
      c3dHighlight = Color.FromArgb(&HEA, &HE6, &HDD)
      c3dShadow = Color.FromArgb(&HA2, &H8D, &H68)
      c3dDark = Color.FromArgb(&H40, &H40, &H40)
      c3dIconStrip = Color.FromArgb(&HD3, &HD9, &HE0)
      titleHeight = 18
      buttonW = 16
      buttonH = 14
   End Sub

End Class
