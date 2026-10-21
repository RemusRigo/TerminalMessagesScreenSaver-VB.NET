Imports System.Drawing.Drawing2D

' Windows 7 Aero Command Prompt: slate glass frame with rounded corners, glass caption buttons, navy console
Public Class SkinWin7
   Inherits SkinBase

   Private Const FRAME As Integer = 9           ' left, right and bottom border incl. the inner lines
   Private Const TITLE_HEIGHT As Integer = 30   ' top border incl. the title
   Private Const PADDING As Integer = 2
   Private Const CORNER_RADIUS As Integer = 7

   Public Sub New(px As Integer)
      MyBase.New(px)
      TermBackColor = Color.FromArgb(&H7, &H17, &H30)        ' dark navy, like the screenshot
      TermForeColor = Color.Silver
      titleForeColor = Color.Black
      windowTitle = "Command Prompt"
      uiFont = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point)
   End Sub

   Public Overrides Sub Layout(area As Rectangle)
      WindowRect = area
      titleBarRect = New Rectangle(area.X, area.Y, area.Width, TITLE_HEIGHT)
      BodyRect = New Rectangle(area.X + FRAME, area.Y + TITLE_HEIGHT, area.Width - FRAME * 2, area.Height - TITLE_HEIGHT - FRAME)
      Pad = PADDING
   End Sub

   Public Overrides Sub PaintWindow(g As Graphics)
      g.SmoothingMode = SmoothingMode.AntiAlias

      ' Glass frame: slate gradient, lighter towards the top
      Using winPath As GraphicsPath = RoundedRect(WindowRect, CORNER_RADIUS, 3)
         Using br As New LinearGradientBrush(WindowRect, Color.Black, Color.Black, LinearGradientMode.Horizontal)
            Dim blend As New ColorBlend()
            blend.Colors = {Color.FromArgb(&H4A, &H54, &H70), Color.FromArgb(&H4E, &H56, &H6E), Color.FromArgb(&H73, &H7A, &H8D), Color.FromArgb(&H65, &H6C, &H7F)}
            blend.Positions = {0.0F, 0.5F, 0.82F, 1.0F}
            br.InterpolationColors = blend
            g.FillPath(br, winPath)
         End Using
         Using br As New LinearGradientBrush(titleBarRect, Color.FromArgb(60, Color.White), Color.FromArgb(0, Color.White), LinearGradientMode.Vertical)
            g.SetClip(winPath)
            g.FillRectangle(br, titleBarRect)
            g.ResetClip()
         End Using

         ' Light inner line, dark outer line
         Using innerPath As GraphicsPath = RoundedRect(Rectangle.Inflate(WindowRect, -1, -1), CORNER_RADIUS - 1, 2)
            Using pen As New Pen(Color.FromArgb(&HDC, &HE1, &HF4), 1)
               g.DrawPath(pen, innerPath)
            End Using
         End Using
         Using pen As New Pen(Color.FromArgb(&H1A, &H21, &H2B), 1)
            g.DrawPath(pen, winPath)
         End Using
      End Using

      ' Icon + title text
      DrawCmdIcon(g, WindowRect.X + 10, WindowRect.Y + 8, Color.FromArgb(&HA0, &HA8, &HB8))
      TextRenderer.DrawText(g, windowTitle, uiFont,
                            New Rectangle(WindowRect.X + 34, WindowRect.Y, 400, TITLE_HEIGHT - 2),
                            titleForeColor,
                            TextFormatFlags.VerticalCenter Or TextFormatFlags.Left)

      DrawCaptionButtons(g)

      ' Body with a light and a dark line around it
      g.SmoothingMode = SmoothingMode.None
      Using br As New SolidBrush(Color.FromArgb(&HB2, &HB8, &HCE))
         g.FillRectangle(br, Rectangle.Inflate(BodyRect, 2, 2))
      End Using
      Using br As New SolidBrush(Color.FromArgb(&H21, &H2E, &H41))
         g.FillRectangle(br, Rectangle.Inflate(BodyRect, 1, 1))
      End Using
      Using br As New SolidBrush(TermBackColor)
         g.FillRectangle(br, BodyRect)
      End Using
   End Sub

   ' Minimise, maximise and close hanging from the top edge, as one glass group with rounded bottom corners
   Private Sub DrawCaptionButtons(g As Graphics)
      Const BTN_H As Integer = 20
      Dim closeR As New Rectangle(WindowRect.Right - 7 - 47, WindowRect.Y, 47, BTN_H)
      Dim maxR As New Rectangle(closeR.X - 26, WindowRect.Y, 26, BTN_H)
      Dim minR As New Rectangle(maxR.X - 26, WindowRect.Y, 26, BTN_H)
      Dim group As New Rectangle(minR.X, WindowRect.Y, closeR.Right - minR.X, BTN_H)
      Dim dark As Color = Color.FromArgb(&H34, &H38, &H43)

      Using groupPath As GraphicsPath = RoundedRect(group, 0, 4)
         ' Glass: light top half, darker bottom half
         Using br As New LinearGradientBrush(group, Color.Black, Color.Black, LinearGradientMode.Vertical)
            Dim blend As New ColorBlend()
            blend.Colors = {Color.FromArgb(&HC8, &HCC, &HD6), Color.FromArgb(&HA0, &HA5, &HB2), Color.FromArgb(&H86, &H8B, &H98), Color.FromArgb(&H98, &H9E, &HAE)}
            blend.Positions = {0.0F, 0.48F, 0.52F, 1.0F}
            br.InterpolationColors = blend
            g.FillPath(br, groupPath)
         End Using
         Using innerPath As GraphicsPath = RoundedRect(Rectangle.Inflate(group, -1, -1), 0, 3)
            Using pen As New Pen(Color.FromArgb(110, Color.White), 1)
               g.DrawPath(pen, innerPath)
            End Using
         End Using
         Using pen As New Pen(dark, 1)
            g.DrawPath(pen, groupPath)
            g.DrawLine(pen, minR.Right, group.Y, minR.Right, group.Bottom)
            g.DrawLine(pen, maxR.Right, group.Y, maxR.Right, group.Bottom)
         End Using
      End Using

      ' White glyphs with a dark outline
      Dim cy As Integer = group.Y + BTN_H \ 2
      Dim minCx As Integer = minR.X + minR.Width \ 2
      Dim maxCx As Integer = maxR.X + maxR.Width \ 2
      Dim closeCx As Integer = closeR.X + closeR.Width \ 2
      g.SmoothingMode = SmoothingMode.None
      Using darkBr As New SolidBrush(dark)
         g.FillRectangle(darkBr, minCx - 5, cy + 1, 11, 5)
         g.FillRectangle(Brushes.White, minCx - 4, cy + 2, 9, 3)
         g.FillRectangle(darkBr, maxCx - 5, cy - 5, 11, 10)
         g.FillRectangle(Brushes.White, maxCx - 4, cy - 4, 9, 8)
         g.FillRectangle(darkBr, maxCx - 2, cy - 1, 5, 3)
      End Using
      g.SmoothingMode = SmoothingMode.AntiAlias
      Using pen As New Pen(dark, 4.5F)
         DrawCross(g, pen, closeCx, cy, 4)
      End Using
      Using pen As New Pen(Color.White, 2.5F)
         DrawCross(g, pen, closeCx, cy, 4)
      End Using
   End Sub

End Class
