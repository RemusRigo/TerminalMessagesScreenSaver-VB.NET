# Terminal Messages ScreenSaver: User's Guide



## Downloads & Runtimes

Download the latest release from the [Releases page](https://github.com/RemusRigo/TerminalScreenSaver-VB.NET/releases).

Each release contains one folder per framework and architecture:

|Folder|Target Framework|Architecture|Required Runtime|
|-|-|-|-|
|`net8.0 x64`|.NET 8.0|64-bit|[.NET 8.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net8.0 x86`|.NET 8.0|32-bit|[.NET 8.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net10.0 x64`|.NET 10.0|64-bit|[.NET 10.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/10.0)|
|`net10.0 x86`|.NET 10.0|32-bit|[.NET 10.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/10.0)|

> **Which one do I need?** On 64-bit Windows 10/11, use **x64**. Pick the .NET version whose
> **Desktop Runtime** you already have installed, or install one from the links above.
> The plain ".NET Runtime" is not enough; the screensaver needs the **Desktop** Runtime.

To check which runtimes are installed, open a command prompt and run:

```
dotnet --list-runtimes
```

Look for a `Microsoft.WindowsDesktop.App 8.x` or `10.x` line.



## Install

1. Download and extract the folder that matches your system (see the table above), e.g. `net8.0 x64`.
2. Copy the **whole folder** to a permanent location, for example:
   `D:\My Apps\Terminal Messages ScreenSaver\`
   The folder must contain these files; the screensaver won't start if any of them is missing:
   * `TerminalMessagesSS.scr`
   * `TerminalMessagesSS.dll`
   * `TerminalMessagesSS.deps.json`
   * `TerminalMessagesSS.runtimeconfig.json`
   * the `Messages` folder, with at least one `*.json` file
3. Right-click `TerminalMessagesSS.scr` and choose **Install**.
   The Windows **Screen Saver Settings** dialog opens with *TerminalMessagesSS* selected.
4. Set the **Wait** time and click **OK**.

> If Windows blocks the file ("Windows protected your PC"), right-click `TerminalMessagesSS.scr` →
> **Properties** → check **Unblock** → **OK**, then try again.



## Test it

* In **Screen Saver Settings**, click **Preview**, or
* double-click `TerminalMessagesSS.scr`.

A terminal window fills every monitor, taskbar included, and starts typing messages.
Press any key, click, or move the mouse on any monitor to exit.

> The small preview in the Screen Saver Settings dialog stays blank for now.



## Settings

In **Screen Saver Settings**, click **Settings...** to open the settings dialog
(or right-click `TerminalMessagesSS.scr` → **Configure**).

|Setting|What it does|
|-|-|
|**Random Skin**|Checked (default): each monitor gets a random skin, chosen again at every start|
|Skin list|Used when **Random Skin** is unchecked: the same skin on every monitor|
|**Random Messages**|Checked (default): a random message file is used|
|Message file list|Used when **Random Messages** is unchecked; lists every `*.json` in the message folder|
|Message folder|The folder with the message files; type a path and press **Enter**, click **Browse**, or just leave the box. Default: the `Messages` folder next to `TerminalMessagesSS.scr`|

Click **Ok** to save. With no file selected, **Random Messages** is saved.
If the chosen message file no longer exists, the screensaver (and the dialog) falls back to **Random Messages**.

The settings are stored in the registry, under `HKEY_CURRENT_USER\Software\Remus RIGO\TerminalMessagesSS.VB-NET`.



## Messages

Each time it starts, the screensaver uses the message file chosen in **Settings**, or **one random**
`*.json` file from the message folder (subfolders included).

A message file is a JSON object of `"title": "description"` pairs.
The **first pair is the title of the file** and is always shown first. The other messages follow in
**random order**; when all were shown, they are shuffled again (the title is not repeated).

```json
{
  "System check": "© 2026 Remus Rigo",
  "Initializing system diagnostics": "please wait...",
  "Memory banks": "OK",
  "Mainframe": "CONNECTED to \"MAIN-01\"",
  "All systems nominal": ""
}
```

Each message is shown as:

```
C:\>System check
© 2026 Remus Rigo

C:\>Memory banks
OK

```

* The **title** is typed character by character, in green.
* The **description** appears on the next line; leave it empty (`""`) to show only the title.
* `\n` in a title or description starts a new line, `\t` is shown as 4 spaces.
  To show `\n` as text, write `\\n`.
* Other JSON escapes work too: `\"`, `\\`, `\/`, `\uXXXX` (e.g. `\u00A9` for ©).

### Quotes inside messages

In strict JSON, quotes inside a text must be escaped as `\"`.
If a file is not valid JSON, it is read line by line instead, so this works too:

```
  "Type "help"": "for a list of "commands"",
```

In this mode, each message must be on its own line, as `"title": "description"`.
Everything between the first `"` and the `":` is the title (spaces before the `:` are allowed);
everything between the next pair of outer quotes is the description.
The JSON escapes above (`\n`, `\"`...) work in this mode too.

> If the message folder is missing or empty, the screensaver types *"No messages found"*.
> If a file cannot be read (locked, no access, drive offline), it types *"Cannot read messages"* and the reason.

### Add your own

1. Create a new `.json` file (UTF-8) in the message folder, or in a subfolder.
2. Start with the title pair, e.g. `"My commands": "© 2026 Your Name"`, then add your `"title": "description"` pairs.
3. Select the file in **Settings**, or start the screensaver a few times: each run picks a random file.



## Uninstall

1. In **Screen Saver Settings**, choose another screensaver (or *None*) and click **OK**.
2. Delete the folder you copied in step 2 of *Install*.
3. Optional: delete the settings key `HKEY_CURRENT_USER\Software\Remus RIGO\TerminalMessagesSS.VB-NET` with **regedit**.
