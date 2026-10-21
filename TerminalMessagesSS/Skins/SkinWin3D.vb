Imports System.Drawing.Drawing2D

' Windows 95 / XP classic window: raised 3D frame, (gradient) title bar, 3D caption buttons, sunken body.
' SkinWin95 and SkinWinXP set the colors and sizes.
Public MustInherit Class SkinWin3D
   Inherits SkinBase

   ' 3D frame (4), title bar, 1 unit gap, sunken client edge (2)
   Private Const FRAME As Integer = 4
   Private Const EDGE As Integer = 2
   Private Const PADDING As Integer = 2         ' inner padding of the text area (real pixels)

   Protected titleHeight As Integer
   Protected buttonW As Integer
   Protected buttonH As Integer

   Protected titleBackColor2 As Color           ' right end of the title bar gradient
   Protected c3dFace As Color
   Protected c3dLight As Color                  ' outer top-left line of the frame
   Protected c3dHighlight As Color
   Protected c3dShadow As Color
   Protected c3dDark As Color
   Protected c3dIconStrip As Color              ' title strip of the console icon

   Protected Sub New(px As Integer)
      MyBase.New(px)
      TermBackColor = Color.Black
      TermForeColor = Color.Silver
      titleForeColor = Color.White
   End Sub

   Public Overrides Sub Layout(area As Rectangle)
      SnapToUnits(area)
      Dim w As Integer = WindowRect.Width \ px
      Dim h As Integer = WindowRect.Height \ px

      ' Frame, title bar, 1 unit gap, sunken edge around the body
      Dim top As Integer = FRAME + titleHeight + 1 + EDGE
      Dim side As Integer = FRAME + EDGE
      titleBarRect = UnitsToRect(FRAME, FRAME, w - FRAME * 2, titleHeight)
      BodyRect = UnitsToRect(side, top, w - side * 2, h - top - side)
      Pad = PADDING
   End Sub

   Public Overrides Sub PaintWindow(g As Graphics)
      Dim w As Integer = WindowRect.Width \ px
      Dim h As Integer = WindowRect.Height \ px

      ' Raised frame: light + highlight on the top-left, shadow + dark on the bottom-right
      FillUnits(g, c3dDark, 0, 0, w, h)
      FillUnits(g, c3dLight, 0, 0, w - 1, h - 1)
      FillUnits(g, c3dShadow, 1, 1, w - 2, h - 2)
      FillUnits(g, c3dHighlight, 1, 1, w - 3, h - 3)
      FillUnits(g, c3dFace, 2, 2, w - 4, h - 4)

      ' Title bar
      Using br As New LinearGradientBrush(titleBarRect, titleBackColor, titleBackColor2, LinearGradientMode.Horizontal)
         g.FillRectangle(br, titleBarRect)
      End Using
      Dim iconY As Integer = FRAME + (titleHeight - 12) \ 2
      DrawConsoleIcon(g, FRAME + 2, iconY)

      ' Caption buttons: minimise and maximise side by side, close 2 units further right
      Dim btnY As Integer = FRAME + (titleHeight - buttonH) \ 2
      Dim closeX As Integer = w - FRAME - 2 - buttonW
      Dim maxX As Integer = closeX - 2 - buttonW
      Dim minX As Integer = maxX - buttonW
      For Each x In {minX, maxX, closeX}
         DrawButton(g, x, btnY)
      Next

      ' Glyphs, designed for a 16x14 button and centered in bigger ones
      Dim ox As Integer = (buttonW - 16) \ 2
      Dim oy As Integer = btnY + (buttonH - 14) \ 2
      FillUnits(g, Color.Black, minX + ox + 4, oy + 9, 6, 2)
      FillUnits(g, Color.Black, maxX + ox + 3, oy + 2, 9, 2)
      FillUnits(g, Color.Black, maxX + ox + 3, oy + 2, 1, 9)
      FillUnits(g, Color.Black, maxX + ox + 11, oy + 2, 1, 9)
      FillUnits(g, Color.Black, maxX + ox + 3, oy + 10, 9, 1)
      Dim closeRows As Integer(,) = {{4, 10}, {5, 9}, {6, 8}, {7, 7}, {6, 8}, {5, 9}, {4, 10}}   ' x of the left and right 2-unit pieces
      For i As Integer = 0 To 6
         FillUnits(g, Color.Black, closeX + ox + closeRows(i, 0), oy + 3 + i, 2, 1)
         FillUnits(g, Color.Black, closeX + ox + closeRows(i, 1), oy + 3 + i, 2, 1)
      Next

      ' Title text after the icon
      Dim textX As Integer = WindowRect.X + (FRAME + 2 + 16 + 3) * px
      TextRenderer.DrawText(g, windowTitle, uiFont,
                            New Rectangle(textX, titleBarRect.Y, WindowRect.X + minX * px - textX, titleBarRect.Height),
                            titleForeColor,
                            TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine Or TextFormatFlags.EndEllipsis)

      ' Sunken edge: shadow + dark on the top-left, highlight + face on the bottom-right
      Dim bx As Integer = FRAME + EDGE
      Dim by As Integer = FRAME + titleHeight + 1 + EDGE
      Dim bw As Integer = BodyRect.Width \ px
      Dim bh As Integer = BodyRect.Height \ px
      FillUnits(g, c3dHighlight, bx - 2, by - 2, bw + 4, bh + 4)
      FillUnits(g, c3dShadow, bx - 2, by - 2, bw + 3, bh + 3)
      FillUnits(g, c3dFace, bx - 1, by - 1, bw + 2, bh + 2)
      FillUnits(g, c3dDark, bx - 1, by - 1, bw + 1, bh + 1)

      Using br As New SolidBrush(TermBackColor)
         g.FillRectangle(br, BodyRect)
      End Using
   End Sub

   ' Raised 3D button: highlight top-left, shadow + dark bottom-right
   Private Sub DrawButton(g As Graphics, x As Integer, y As Integer)
      FillUnits(g, c3dDark, x, y, buttonW, buttonH)
      FillUnits(g, c3dHighlight, x, y, buttonW - 1, buttonH - 1)
      FillUnits(g, c3dShadow, x + 1, y + 1, buttonW - 2, buttonH - 2)
      FillUnits(g, c3dFace, x + 1, y + 1, buttonW - 3, buttonH - 3)
   End Sub

   ' 16x13 console icon of Windows 95 / XP: border, title strip, black box with "C:\"
   Private Sub DrawConsoleIcon(g As Graphics, x As Integer, y As Integer)
      FillUnits(g, Color.FromArgb(&H8F, &H92, &H97), x, y, 16, 13)
      FillUnits(g, c3dIconStrip, x + 1, y + 1, 14, 2)
      FillUnits(g, Color.Black, x + 1, y + 3, 14, 9)
      Dim glyph As String() = {
         "  ##    #     ",
         " #  # # #     ",
         " #      ##    ",
         " #  # #  #    ",
         "  ##     # ## "}
      For row As Integer = 0 To glyph.Length - 1
         For col As Integer = 0 To glyph(row).Length - 1
            If glyph(row)(col) = "#"c Then FillUnits(g, Color.White, x + 1 + col, y + 5 + row, 1, 1)
         Next
      Next
   End Sub

End Class
