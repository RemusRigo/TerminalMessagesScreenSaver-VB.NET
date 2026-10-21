Imports System.IO
Imports Microsoft.Win32

Public Class frmSS

   Private ReadOnly skinKind As TerminalSkin
   Private skin As SkinBase                     ' draws the window around the text (see the Skins folder)

   ' ---- Geometry ----
   Private Const EDGE_GAP As Integer = 10       ' gap between the screen edge and the window

   ' ---- Messages ----
   Private Const MESSAGES_FOLDER As String = "Messages"   ' next to the exe, searched for *.json (with subfolders)

   Private termFont As Font
   Private cellW As Integer
   Private cellH As Integer

   Private screenRect As Rectangle              ' the text grid (cols x rows cells)

   Private WithEvents terminal As TerminalScreen

   Private mouseStart As Point = Point.Empty

   '-----------------------------------------------------------------------------------------------
   ' Constructor: covers the whole monitor (taskbar included)
   Public Sub New(skin As TerminalSkin, monitor As Screen)
      InitializeComponent()
      Me.skinKind = skin
      Me.FormBorderStyle = FormBorderStyle.None
      Me.StartPosition = FormStartPosition.Manual
      Me.Bounds = monitor.Bounds
   End Sub

   ' ----------------------------------------------------------------------------------------------
   ' frmSS: OnLoad
   Private Sub frmSS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      Me.BackColor = Color.Black
      Me.ShowInTaskbar = False
      Me.DoubleBuffered = True
      Me.KeyPreview = True
#If DEBUG Then
      Me.TopMost = False
#Else
      Me.TopMost = True
      Cursor.Hide()
#End If

      skin = SkinBase.Create(skinKind, Me.ClientSize.Height)

      ' Terminal font and the size of one character cell
      termFont = CreateTerminalFont(14)
      Dim sz As Size = TextRenderer.MeasureText("W", termFont, New Size(Integer.MaxValue, Integer.MaxValue), TextFormatFlags.NoPadding)
      cellW = sz.Width
      cellH = sz.Height

      Dim cols, rows As Integer
      LayoutTerminal(cols, rows)

      ' Terminal text + typing
      terminal = New TerminalScreen(cols, rows, skin.TermForeColor)
      terminal.WriteLine("Terminal ScreenSaver v1.0")
      terminal.WriteLine("   © 2026 Remus Rigo")
      terminal.NewLine()

      ' Messages: registry settings if present, else a random file from <exe folder>\Messages
      Dim msgPath As String = Path.Combine(AppContext.BaseDirectory, MESSAGES_FOLDER)
      Dim msgFile As String = ""   ' "" = random file
      Try
         Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(regAppPath)
            If key IsNot Nothing Then
               Dim regPath As String = CStr(key.GetValue("msgPath", ""))
               If Not String.IsNullOrWhiteSpace(regPath) Then msgPath = regPath
               If Not CBool(key.GetValue("RandomMessages", True)) Then msgFile = CStr(key.GetValue("msgFile", ""))
            End If
         End Using
      Catch
         ' unreadable value: keep the defaults
      End Try
      terminal.LoadMessages(msgPath, msgFile)
      terminal.Start()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' CreateTerminalFont
   Private Function CreateTerminalFont(size As Single) As Font
      For Each wanted In {"Consolas", "Cascadia Mono", "Courier New"}
         For Each ff In FontFamily.Families
            If ff.Name.Equals(wanted, StringComparison.OrdinalIgnoreCase) Then
               Return New Font(ff, size, FontStyle.Regular, GraphicsUnit.Point)
            End If
         Next
      Next
      Return New Font(FontFamily.GenericMonospace, size)
   End Function

   Private Sub terminal_Changed(sender As Object, e As EventArgs) Handles terminal.Changed
      Invalidate(screenRect)
   End Sub

   ' ------------------------------------------------------------------
   ' LayoutTerminal: lets the skin place the window and returns the size of the text grid
   Private Sub LayoutTerminal(ByRef cols As Integer, ByRef rows As Integer)
      ' Window fills the screen, leaving EDGE_GAP pixels on every side
      skin.Layout(New Rectangle(EDGE_GAP, EDGE_GAP, Me.ClientSize.Width - EDGE_GAP * 2, Me.ClientSize.Height - EDGE_GAP * 2))

      ' How many characters fit in the body
      Dim body As Rectangle = skin.BodyRect
      cols = (body.Width - skin.Pad * 2) \ cellW
      rows = (body.Height - skin.Pad * 2) \ cellH
      screenRect = New Rectangle(body.X + skin.Pad, body.Y + skin.Pad, cols * cellW, rows * cellH)
   End Sub

   ' Top-left pixel of a character cell (0-based column and row)
   Private Function CellToPoint(col As Integer, row As Integer) As Point
      Return New Point(screenRect.X + col * cellW, screenRect.Y + row * cellH)
   End Function

   ' ------------------------------------------------------------------
   ' frmSS: OnPaint
   Private Sub frmSS_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
      If terminal Is Nothing Then Return
      Dim g As Graphics = e.Graphics

      skin.PaintWindow(g)

      ' Text lines, piece by piece in their own color
      For row As Integer = 0 To terminal.Lines.Count - 1
         Dim col As Integer = 0
         For Each seg In terminal.Lines(row)
            TextRenderer.DrawText(g, seg.Text, termFont, CellToPoint(col, row), seg.Color, TextFormatFlags.NoPadding)
            col += seg.Text.Length
         Next
      Next

      ' Cursor after the last character
      skin.PaintCursor(g, New Rectangle(CellToPoint(terminal.CursorCol, terminal.CursorRow), New Size(cellW, cellH)))
   End Sub

   ' ------------------------------------------------------------------
   ' Exit the screensaver on any input
   ' ------------------------------------------------------------------
   Private Sub frmSS_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
      Me.Close()
   End Sub

   Private Sub frmSS_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
      Me.Close()
   End Sub

   Private Sub frmSS_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
      If mouseStart.IsEmpty Then
         mouseStart = e.Location          ' first event just records the position
      ElseIf Math.Abs(e.X - mouseStart.X) > 5 OrElse Math.Abs(e.Y - mouseStart.Y) > 5 Then
         Me.Close()
      End If
   End Sub

   Private Sub frmSS_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
      If terminal IsNot Nothing Then terminal.Stop()
      Cursor.Show()
      If termFont IsNot Nothing Then termFont.Dispose()
      If skin IsNot Nothing Then skin.Dispose()
   End Sub

End Class
