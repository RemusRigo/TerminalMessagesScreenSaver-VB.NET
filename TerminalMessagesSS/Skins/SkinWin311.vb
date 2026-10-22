'--------------------------------------------------------------------------------------------------
' TerminalMessagesSS: SkinBase.vb
'    © 2026 Remus Rigo
'       v1.0.20261017
'
' Windows 3.11 MS-DOS Prompt: grey frame, navy title bar, caption buttons; drawn pixel by pixel, scaled by px
'--------------------------------------------------------------------------------------------------

Public Class SkinWin311
   Inherits SkinBase

   Private Const FRAME As Integer = 4           ' black, grey, grey, black
   Private Const TITLE_HEIGHT As Integer = 19   ' title bar + black line under it
   Private Const BUTTON As Integer = 18         ' caption buttons are 18x18
   Private Const PADDING As Integer = 2         ' inner padding of the text area (real pixels)

   Public Sub New(px As Integer)
      MyBase.New(px)
      TermBackColor = Color.Black
      TermForeColor = Color.Silver
      titleBackColor = Color.Navy
      titleForeColor = Color.White
      windowTitle = "MS-DOS Prompt"
      uiFont = New Font("Microsoft Sans Serif", 8.25F * px, FontStyle.Bold)
   End Sub

   Public Overrides Sub Layout(area As Rectangle)
      SnapToUnits(area)
      Dim w As Integer = WindowRect.Width \ px
      Dim h As Integer = WindowRect.Height \ px

      ' Title bar between the system menu box (left) and the two caption buttons (right), each followed by a black line
      Dim titleX As Integer = FRAME + BUTTON + 1
      titleBarRect = UnitsToRect(titleX, FRAME, w - FRAME - (BUTTON + 1) * 2 - titleX, TITLE_HEIGHT - 1)
      BodyRect = UnitsToRect(FRAME, FRAME + TITLE_HEIGHT, w - FRAME * 2, h - FRAME * 2 - TITLE_HEIGHT)
      Pad = PADDING
   End Sub

   Public Overrides Sub PaintWindow(g As Graphics)
      Dim w As Integer = WindowRect.Width \ px
      Dim h As Integer = WindowRect.Height \ px

      ' Frame: black outline, 2 grey, black inner line (the inner area is black too)
      FillUnits(g, Brushes.Black, 0, 0, w, h)
      FillUnits(g, Brushes.Silver, 1, 1, w - 2, h - 2)
      FillUnits(g, Brushes.Black, 3, 3, w - 6, h - 6)

      ' Resize marks on the frame, 22 units from each corner
      For Each x In {22, w - 23}
         FillUnits(g, Brushes.Black, x, 1, 1, 2)
         FillUnits(g, Brushes.Black, x, h - 3, 1, 2)
      Next
      For Each y In {22, h - 23}
         FillUnits(g, Brushes.Black, 1, y, 2, 1)
         FillUnits(g, Brushes.Black, w - 3, y, 2, 1)
      Next

      ' System menu box: grey, with a white bar and its shadow
      FillUnits(g, Brushes.Silver, 4, 4, 18, 18)
      FillUnits(g, Brushes.Black, 6, 11, 13, 3)
      FillUnits(g, Brushes.White, 7, 12, 11, 1)
      FillUnits(g, Brushes.Gray, 19, 12, 1, 2)
      FillUnits(g, Brushes.Gray, 7, 14, 13, 1)

      ' Title bar
      Using br As New SolidBrush(titleBackColor)
         g.FillRectangle(br, titleBarRect)
      End Using
      TextRenderer.DrawText(g, windowTitle, uiFont, titleBarRect, titleForeColor,
                            TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine)

      ' Minimise (down triangle) and maximise (up triangle) buttons
      Dim minX As Integer = w - 41
      Dim maxX As Integer = w - 22
      DrawButton(g, minX, 4, BUTTON)
      DrawButton(g, maxX, 4, BUTTON)
      For i As Integer = 0 To 3
         FillUnits(g, Brushes.Black, minX + 5 + i, 11 + i, 7 - i * 2, 1)
         FillUnits(g, Brushes.Black, maxX + 8 - i, 10 + i, 1 + i * 2, 1)
      Next

      ' Screen area
      Using br As New SolidBrush(TermBackColor)
         g.FillRectangle(br, BodyRect)
      End Using
   End Sub

   ' Raised Windows 3.11 button: white top-left edge, 2 units of dark grey shadow at the bottom-right
   Private Sub DrawButton(g As Graphics, x As Integer, y As Integer, size As Integer)
      FillUnits(g, Brushes.Silver, x, y, size, size)
      FillUnits(g, Brushes.White, x, y, size - 1, 1)
      FillUnits(g, Brushes.White, x, y, 1, size - 1)
      FillUnits(g, Brushes.Gray, x + size - 1, y, 1, size)
      FillUnits(g, Brushes.Gray, x + size - 2, y + 1, 1, size - 1)
      FillUnits(g, Brushes.Gray, x, y + size - 1, size, 1)
      FillUnits(g, Brushes.Gray, x + 1, y + size - 2, size - 1, 1)
   End Sub

End Class
