Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions

' Text side of the terminal: screen buffer (colored lines, wrapping, scrolling),
' messages read from JSON files and the typing effect. It does no drawing:
' the form paints Lines and the cursor whenever Changed is raised.
Public Class TerminalScreen

   ' ---- Typing settings ----
   Public Prompt As String = "C:\>"
   Public TypeDelayMin As Integer = 40        ' ms between two typed characters (random in this range)
   Public TypeDelayMax As Integer = 140
   Public EnterDelay As Integer = 500         ' ms after the prompt / after the title, before the description shows
   Public MessageDelay As Integer = 2000      ' ms the description stays before the next prompt

   ' ---- Colors ----
   Public ForeColor As Color                  ' prompt, description and other text
   Public TitleColor As Color = Color.Lime    ' the "typed command" (message title)

   ' ---- Grid ----
   Public ReadOnly Property Cols As Integer
   Public ReadOnly Property Rows As Integer

   ' Raised after the screen content changed, so the form can repaint
   Public Event Changed As EventHandler

   ' Screen buffer: each line is a list of colored pieces; the last line is the one being written
   Public Class Segment
      Public Text As String
      Public Color As Color
   End Class
   Private ReadOnly screenLines As New List(Of List(Of Segment))

   Public ReadOnly Property Lines As IReadOnlyList(Of List(Of Segment))
      Get
         Return screenLines
      End Get
   End Property

   ' Messages
   Private Class MessageItem
      Public Title As String
      Public Description As String
   End Class
   Private ReadOnly messages As New List(Of MessageItem)
   Private messageIndex As Integer

   ' What the typing timer does on its next tick
   Private Enum TypeState
      Prompt          ' write the prompt on the current line
      Typing          ' type the next character of the title
      Description     ' "Enter": show the description and an empty separator line
   End Enum
   Private state As TypeState = TypeState.Prompt
   Private typePos As Integer
   Private WithEvents typeTimer As New Timer()

   Private ReadOnly rnd As New Random()

   Public Sub New(cols As Integer, rows As Integer, foreColor As Color)
      Me.Cols = Math.Max(cols, 1)
      Me.Rows = Math.Max(rows, 1)
      Me.ForeColor = foreColor
      NewLine()
   End Sub

   ' ------------------------------------------------------------------
   ' Typing
   ' ------------------------------------------------------------------
   Public Sub Start()
      If messages.Count = 0 Then AddMessage("No messages found", "Call LoadMessages first")
      typeTimer.Interval = EnterDelay
      typeTimer.Start()
   End Sub

   Public Sub [Stop]()
      typeTimer.Stop()
   End Sub

   ' Each message is shown as:   C:\>title        (title typed char by char, in TitleColor)
   '                             description
   '                             (empty line)
   Private Sub typeTimer_Tick(sender As Object, e As EventArgs) Handles typeTimer.Tick
      Dim msg As MessageItem = messages(messageIndex)

      Select Case state
         Case TypeState.Prompt
            Write(Prompt, ForeColor)
            typePos = 0
            state = TypeState.Typing
            typeTimer.Interval = EnterDelay

         Case TypeState.Typing
            If typePos < msg.Title.Length Then
               Write(msg.Title(typePos), TitleColor)
               typePos += 1
               typeTimer.Interval = rnd.Next(TypeDelayMin, TypeDelayMax)
            Else
               state = TypeState.Description
               typeTimer.Interval = EnterDelay
            End If

         Case TypeState.Description
            NewLine()
            If msg.Description <> "" Then
               Write(msg.Description, ForeColor)
               NewLine()
            End If
            NewLine()
            ' Next message; when all were shown, start again with the rest in a new random order (the title only once)
            messageIndex += 1
            If messageIndex >= messages.Count Then
               messageIndex = If(messages.Count > 1, 1, 0)
               ShuffleMessages()
            End If
            state = TypeState.Prompt
            typeTimer.Interval = MessageDelay
      End Select

      RaiseEvent Changed(Me, EventArgs.Empty)
   End Sub

   ' ------------------------------------------------------------------
   ' Screen buffer (works like a console: write text, wrap at Cols, scroll when full)
   ' ------------------------------------------------------------------
   Private ReadOnly Property CurrentLine As List(Of Segment)
      Get
         Return screenLines(screenLines.Count - 1)
      End Get
   End Property

   ' Cursor position (0-based), right after the last character
   Public ReadOnly Property CursorCol As Integer
      Get
         Return Math.Min(LineLength(CurrentLine), Cols - 1)
      End Get
   End Property

   Public ReadOnly Property CursorRow As Integer
      Get
         Return screenLines.Count - 1
      End Get
   End Property

   Private Shared Function LineLength(line As List(Of Segment)) As Integer
      Return line.Sum(Function(s) s.Text.Length)
   End Function

   ' Starts a new line; when the screen is full, all lines move up and the new line is the last row
   Public Sub NewLine()
      screenLines.Add(New List(Of Segment))
      Do While screenLines.Count > Rows
         screenLines.RemoveAt(0)
      Loop
   End Sub

   ' Writes text at the cursor, wrapping at Cols characters (line breaks in the text start a new line)
   Public Sub Write(text As String, color As Color)
      text = text.Replace(vbTab, "    ").Replace(vbCrLf, vbLf).Replace(vbCr, vbLf)
      For Each ch As Char In text
         If ch = vbLf Then
            NewLine() ' \n
            Continue For
         End If

         If LineLength(CurrentLine) >= Cols Then NewLine()

         ' Same color as the last piece: just extend it
         Dim line As List(Of Segment) = CurrentLine
         If line.Count > 0 AndAlso line(line.Count - 1).Color = color Then
            line(line.Count - 1).Text &= ch
         Else
            line.Add(New Segment With {.Text = ch, .Color = color})
         End If
      Next
   End Sub

   Public Sub WriteLine(text As String)
      Write(text, ForeColor)
      NewLine()
   End Sub

   ' ------------------------------------------------------------------
   ' Messages
   ' ------------------------------------------------------------------
   ' Loads fileName (relative to msgPath) or, when it is "" or missing, a random *.json file from msgPath (and subfolders).
   ' A message file is a JSON object of  "title": "description"  pairs.
   ' The first pair is the title of the file: it is shown first, the other messages follow in random order.
   ' If it is not strict JSON (e.g. "Press "Enter"": "Run"), it is read line by line (lenient mode).
   Public Sub LoadMessages(msgPath As String, Optional fileName As String = "")
      messages.Clear()
      messageIndex = 0

      Dim msgFile As String = ""
      Try
         If fileName <> "" Then msgFile = Path.Combine(msgPath, fileName)
         If Not File.Exists(msgFile) Then msgFile = RandomMessageFile(msgPath)
         If msgFile <> "" Then
            Dim text As String = File.ReadAllText(msgFile)
            Try
               ' Read the pairs in file order (a Dictionary does not guarantee the order, and drops duplicate titles)
               Using doc As JsonDocument = JsonDocument.Parse(text)
                  For Each prop In doc.RootElement.EnumerateObject()
                     AddMessage(prop.Name, prop.Value.GetString())   ' a value that is not a string throws: lenient mode
                  Next
               End Using
            Catch ex As Exception
               messages.Clear()
               LoadMessagesLenient(text)   ' re-read the whole data in lenient mode
            End Try
         End If
      Catch ex As Exception
         ' locked file, no access, drive offline...: show the reason instead of crashing
         messages.Clear()
         AddMessage("Cannot read messages", If(msgFile = "", msgPath, msgFile) & vbLf & ex.Message)
         Return
      End Try
      If messages.Count = 0 Then
         AddMessage("No messages found", If(msgFile = "", "No *.json files in " & msgPath, "Check " & msgFile))
      End If
      ShuffleMessages()
   End Sub

   ' Shuffles the messages after the first one: the first record is the title of the file and always comes first
   Private Sub ShuffleMessages()
      For i As Integer = messages.Count - 1 To 2 Step -1
         Dim j As Integer = rnd.Next(1, i + 1)
         Dim tmp As MessageItem = messages(i)
         messages(i) = messages(j)
         messages(j) = tmp
      Next
   End Sub

   ' Returns a random *.json file from msgPath (and subfolders), or "" if there is none
   Private Function RandomMessageFile(msgPath As String) As String
      If Not Directory.Exists(msgPath) Then Return ""
      Dim jsonFiles As String() = Directory.GetFiles(msgPath, "*.json", SearchOption.AllDirectories)
      If jsonFiles.Length = 0 Then Return ""
      Return jsonFiles(rnd.Next(0, jsonFiles.Length))
   End Function

   ' Key/value separator in lenient mode:  ":  with optional spaces before the colon
   Private Shared ReadOnly KeySeparator As New Regex("""\s*:")

   ' Reads lines of the form  "key": "value",  where key and value may contain unescaped quotes.
   ' The key ends at the first  ": , everything between the outer quotes is taken literally
   ' (JSON escapes like \" \\ \n \t \uXXXX are still unescaped, so properly escaped lines work too).
   Private Sub LoadMessagesLenient(text As String)
      For Each rawLine In text.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.None)
         Dim line As String = rawLine.Trim().TrimEnd(","c).Trim()
         If line.Length < 2 OrElse Not line.StartsWith("""") OrElse Not line.EndsWith("""") Then Continue For

         Dim sep As Match = KeySeparator.Match(line, 1)
         If Not sep.Success Then Continue For

         Dim key As String = line.Substring(1, sep.Index - 1)
         Dim value As String = line.Substring(sep.Index + sep.Length).Trim()
         If value.Length >= 2 AndAlso value.StartsWith("""") Then value = value.Substring(1, value.Length - 2)

         AddMessage(Unescape(key), Unescape(value))
      Next
   End Sub

   ' Unescapes a string that was read from a JSON file, the same way strict JSON does:
   ' \" \\ \/ \n \r \t and \uXXXX; an unknown escape is kept as it is
   Private Shared Function Unescape(s As String) As String
      If Not s.Contains("\"c) Then Return s
      Dim sb As New StringBuilder(s.Length)
      Dim i As Integer = 0
      Do While i < s.Length
         If s(i) <> "\"c OrElse i = s.Length - 1 Then
            sb.Append(s(i))
            i += 1
            Continue Do
         End If

         Dim code As Integer
         Select Case s(i + 1)
            Case """"c, "\"c, "/"c : sb.Append(s(i + 1))
            Case "n"c : sb.Append(vbLf)
            Case "r"c : sb.Append(vbCr)
            Case "t"c : sb.Append(vbTab)
            Case "u"c
               If i + 5 < s.Length AndAlso Integer.TryParse(s.Substring(i + 2, 4), NumberStyles.HexNumber, Nothing, code) Then
                  sb.Append(ChrW(code))
                  i += 6
                  Continue Do
               End If
               sb.Append(s, i, 2)
            Case Else : sb.Append(s, i, 2)
         End Select
         i += 2
      Loop
      Return sb.ToString()
   End Function

   ' Adds a message to the list, if the title is not empty.
   ' Line breaks become a single vbLf here: the title is typed one character at a time,
   ' so Write would turn the two characters of vbCrLf into two new lines.
   Private Sub AddMessage(title As String, description As String)
      If String.IsNullOrWhiteSpace(title) Then Return
      messages.Add(New MessageItem With {.Title = NormalizeLineBreaks(title), .Description = NormalizeLineBreaks(If(description, ""))})
   End Sub

   Private Shared Function NormalizeLineBreaks(s As String) As String
      Return s.Replace(vbCrLf, vbLf).Replace(vbCr, vbLf)
   End Function

End Class
