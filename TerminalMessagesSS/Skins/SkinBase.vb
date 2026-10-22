'--------------------------------------------------------------------------------------------------
' TerminalMessagesSS: SkinBase.vb
'    © 2026 Remus Rigo
'       v1.0.20261017
'
' Look of the terminal window
' Base of all skins: colors, title, layout and drawing of the window around the text
'--------------------------------------------------------------------------------------------------

Imports System.Drawing.Drawing2D

Public MustInherit Class SkinBase
   Implements IDisposable

   ' ---- Colors and title (set by each skin) ----
   Public Property TermBackColor As Color
   Public Property TermForeColor As Color
   Protected titleBackColor As Color
   Protected titleForeColor As Color
   Protected windowTitle As String
   Protected uiFont As Font

   ' Windows 3.11, 95 and XP sizes are in "units" (screen pixels of the original), multiplied by px
   Protected ReadOnly px As Integer

   ' ---- Layout (set by Layout) ----
   Public Property WindowRect As Rectangle      ' whole "window" incl. title bar
   Public Property BodyRect As Rectangle        ' area behind the text (incl. padding)
   Public Property Pad As Integer               ' inner padding of the text area
   Protected titleBarRect As Rectangle

   Protected Sub New(px As Integer)
      Me.px = px
   End Sub

   Public Shared Function Create(kind As TerminalSkin, screenHeight As Integer) As SkinBase
      Dim px As Integer = Math.Max(1, screenHeight \ 720)   ' 1 up to 1439 px screen height, 2 up to 2159, ...
      Select Case kind
         Case TerminalSkin.Windows311 : Return New SkinWin311(px)
         Case TerminalSkin.Windows95 : Return New SkinWin95(px)
         Case TerminalSkin.WindowsXP : Return New SkinWinXP(px)
         Case TerminalSkin.Windows7 : Return New SkinWin7(px)
         Case Else : Return New SkinWin11(px)
      End Select
   End Function

   ' Sets WindowRect, titleBarRect, BodyRect and Pad for a window filling the area
   Public MustOverride Sub Layout(area As Rectangle)

   ' Draws the window: frame, title bar, caption buttons and the empty body
   Public MustOverride Sub PaintWindow(g As Graphics)

   ' Cursor in a character cell: an underline
   Public Overridable Sub PaintCursor(g As Graphics, cell As Rectangle)
      Using br As New SolidBrush(TermForeColor)
         g.FillRectangle(br, cell.X, cell.Bottom - 4, cell.Width, 3)
      End Using
   End Sub

   ' ------------------------------------------------------------------
   ' Drawing helpers
   ' ------------------------------------------------------------------
   ' Window size rounded down to whole units, so the right and bottom parts line up
   Protected Sub SnapToUnits(area As Rectangle)
      WindowRect = New Rectangle(area.X, area.Y, area.Width \ px * px, area.Height \ px * px)
   End Sub

   ' Rectangle in units, relative to the top-left corner of the window
   Protected Function UnitsToRect(x As Integer, y As Integer, w As Integer, h As Integer) As Rectangle
      Return New Rectangle(WindowRect.X + x * px, WindowRect.Y + y * px, w * px, h * px)
   End Function

   Protected Sub FillUnits(g As Graphics, br As Brush, x As Integer, y As Integer, w As Integer, h As Integer)
      g.FillRectangle(br, UnitsToRect(x, y, w, h))
   End Sub

   Protected Sub FillUnits(g As Graphics, c As Color, x As Integer, y As Integer, w As Integer, h As Integer)
      Using br As New SolidBrush(c)
         g.FillRectangle(br, UnitsToRect(x, y, w, h))
      End Using
   End Sub

   ' Rectangle with rounded top and bottom corners (a radius of 0 gives square corners)
   Protected Shared Function RoundedRect(r As Rectangle, topRadius As Integer, bottomRadius As Integer) As GraphicsPath
      Dim p As New GraphicsPath()
      If topRadius > 0 Then
         Dim d As Integer = topRadius * 2
         p.AddArc(r.X, r.Y, d, d, 180, 90)
         p.AddArc(r.Right - d, r.Y, d, d, 270, 90)
      Else
         p.AddLine(r.X, r.Y, r.Right, r.Y)
      End If
      If bottomRadius > 0 Then
         Dim d As Integer = bottomRadius * 2
         p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90)
         p.AddArc(r.X, r.Bottom - d, d, d, 90, 90)
      Else
         p.AddLine(r.Right, r.Bottom, r.X, r.Bottom)
      End If
      p.CloseFigure()
      Return p
   End Function

   Protected Shared Sub DrawCross(g As Graphics, pen As Pen, cx As Integer, cy As Integer, half As Integer)
      g.DrawLine(pen, cx - half, cy - half, cx + half, cy + half)
      g.DrawLine(pen, cx - half, cy + half, cx + half, cy - half)
   End Sub

   ' Tiny cmd icon: black box with a colored title strip, ">" and "_"
   Protected Shared Sub DrawCmdIcon(g As Graphics, x As Integer, y As Integer, stripColor As Color)
      g.FillRectangle(Brushes.Black, x, y, 18, 16)
      Using br As New SolidBrush(stripColor)
         g.FillRectangle(br, x, y, 18, 3)
      End Using
      Using pen As New Pen(Color.White, 1)
         g.DrawLines(pen, {New Point(x + 3, y + 6), New Point(x + 6, y + 9), New Point(x + 3, y + 12)})
         g.DrawLine(pen, x + 8, y + 12, x + 13, y + 12)
      End Using
   End Sub

   Public Sub Dispose() Implements IDisposable.Dispose
      If uiFont IsNot Nothing Then uiFont.Dispose()
   End Sub

End Class
