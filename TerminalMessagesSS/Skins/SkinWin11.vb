'--------------------------------------------------------------------------------------------------
' TerminalMessagesSS: SkinBase.vb
'    © 2026 Remus Rigo
'       v1.0.20261017
'
' Windows 11 cmd.exe window: light title bar with caption buttons, rounded top corners
'--------------------------------------------------------------------------------------------------

Imports System.Drawing.Drawing2D

Public Class SkinWin11
   Inherits SkinBase

   Private Const TITLE_HEIGHT As Integer = 34
   Private Const PADDING As Integer = 4         ' inner padding of the text area
   Private Const CORNER_RADIUS As Integer = 8

   Public Sub New(px As Integer)
      MyBase.New(px)
      TermBackColor = Color.FromArgb(12, 12, 12)            ' Windows console black
      TermForeColor = Color.FromArgb(204, 204, 204)         ' Windows console grey
      titleBackColor = Color.FromArgb(243, 243, 243)
      titleForeColor = Color.Black
      windowTitle = "Command Prompt"
      uiFont = New Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point)
   End Sub

   Public Overrides Sub Layout(area As Rectangle)
      WindowRect = area
      titleBarRect = New Rectangle(area.X, area.Y, area.Width, TITLE_HEIGHT)
      BodyRect = New Rectangle(area.X, area.Y + TITLE_HEIGHT, area.Width, area.Height - TITLE_HEIGHT)
      Pad = PADDING
   End Sub

   ' Like cmd.exe: bottom quarter of the cell
   Public Overrides Sub PaintCursor(g As Graphics, cell As Rectangle)
      Dim curH As Integer = cell.Height \ 4
      Using br As New SolidBrush(TermForeColor)
         g.FillRectangle(br, cell.X, cell.Bottom - curH, cell.Width, curH)
      End Using
   End Sub

   Public Overrides Sub PaintWindow(g As Graphics)
      g.SmoothingMode = SmoothingMode.AntiAlias

      ' Window shape (rounded top corners); everything inside is clipped to it
      Using winPath As GraphicsPath = RoundedRect(WindowRect, CORNER_RADIUS, 0)
         g.SetClip(winPath)

         ' Title bar
         Using br As New SolidBrush(titleBackColor)
            g.FillRectangle(br, titleBarRect)
         End Using

         ' Terminal body
         Using br As New SolidBrush(TermBackColor)
            g.FillRectangle(br, BodyRect)
         End Using

         g.ResetClip()

         ' Thin window border
         Using pen As New Pen(Color.FromArgb(90, 90, 90), 1)
            g.DrawPath(pen, winPath)
         End Using
      End Using

      ' cmd icon + title text
      Dim cy As Integer = titleBarRect.Y + TITLE_HEIGHT \ 2
      DrawCmdIcon(g, titleBarRect.X + 12, cy - 8, Color.FromArgb(160, 160, 160))
      TextRenderer.DrawText(g, windowTitle, uiFont,
                            New Rectangle(titleBarRect.X + 40, titleBarRect.Y, 400, TITLE_HEIGHT),
                            titleForeColor,
                            TextFormatFlags.VerticalCenter Or TextFormatFlags.Left)

      ' Caption buttons on the right: minimise, maximise, close
      Dim btnW As Integer = 46
      Dim closeX As Integer = titleBarRect.Right - btnW \ 2
      Dim maxX As Integer = closeX - btnW
      Dim minX As Integer = maxX - btnW
      Using pen As New Pen(titleForeColor, 1)
         g.DrawLine(pen, minX - 5, cy, minX + 5, cy)
         g.DrawRectangle(pen, maxX - 5, cy - 5, 10, 10)
         DrawCross(g, pen, closeX, cy, 5)
      End Using
   End Sub

End Class
