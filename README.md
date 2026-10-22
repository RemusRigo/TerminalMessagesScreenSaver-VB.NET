# Terminal Messages ScreenSaver
Terminal Messages ScreenSaver: a terminal window that types messages on your screen



## 👤 Author

* [@remusrigo](https://github.com/RemusRigo)
* [@remusrigo](https://sourceforge.net/u/remusrigo/profile)



## ✨ Features

* A full-screen terminal window that types commands character by character, like someone is typing them
* Multi-monitor: one terminal window on every monitor
* Messages are read from a `*.json` file in the messages folder (subfolders included)
  * The folder set in Settings, or the `Messages` folder next to the `.scr` when nothing is saved
  * A random file, or a fixed file chosen in Settings (random again if that file is gone)
  * Strict JSON, or a lenient mode for lines with unescaped quotes
  * Both modes understand the JSON escapes `\"` `\\` `\/` `\n` `\r` `\t` `\uXXXX` (`\n` starts a new line, write `\\n` to show it as text)
  * The first record of the file is its title (it is shown first, the other messages follow in random order)
  * When all messages were shown, they are shuffled again and shown in a new order (the title is not repeated)
  * A file that cannot be read shows the reason in the terminal instead of an error dialog
* The text wraps at the window width and scrolls up when the screen is full, like a real console
* Skins:
  * **Windows311**: Windows 3.11 MS-DOS Prompt, navy title bar, caption buttons, tips box, `C>` prompt and underline cursor
  * **Windows95**: Windows 95 MS-DOS Prompt, raised 3D frame ("Desert" colors), teal title bar, 3D caption buttons
  * **WindowsXP**: Windows XP cmd.exe (classic style), beige 3D frame, blue gradient title bar, 3D caption buttons
  * **Windows7**: Windows 7 Command Prompt, slate glass frame, rounded corners, glass caption buttons, navy console
  * **Windows11**: Windows 11 Command Prompt, light title bar with caption buttons, rounded corners
* Settings are saved in the registry: `HKCU\Software\Remus RIGO\TerminalMessagesSS.VB-NET`



## 📥 User's Guide

See the [User's Guide](UsersGuide.md) for downloads, runtimes, installation and message files.



## 🛠️ Statistics

![VB.NET](https://img.shields.io/badge/Language-VB.NET-blue)
![API](https://img.shields.io/badge/API-System.Text.Json-lightblue)
![.NET8.0](https://img.shields.io/badge/Runtime-.NET%208.0-lightblue)
![.NET10.0](https://img.shields.io/badge/Runtime-.NET%2010.0-lightblue)
![VS](https://img.shields.io/badge/IDE-Visual%20Studio-purple)
<br>
![status](https://img.shields.io/badge/status-in%20progress-yellow)
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/RemusRigo/TerminalScreenSaver-VB.NET/total)



## 🗺️ Roadmap

* ToDo: Settings: colors, typing speed
* ToDo: Preview in the Screen Saver Settings dialog
* 2026-10-21: Upd: The first record of a message file is its title, shown first; the other messages are shown in random order
* 2026-10-21: Fix: Messages are read in file order, duplicate titles are kept
* 2026-10-21: Fix: Settings: "msgFile" was saved as "String" when no file was selected
* 2026-10-21: Fix: Settings: the file list could show the files of the previous folder
* 2026-10-21: Fix: Unreadable registry values or message files no longer crash the screensaver
* 2026-10-21: Fix: A line break (`\r\n`) in a title made two new lines
* 2026-10-21: Fix: Lenient mode: JSON escapes (`\n`, `\t`, `\uXXXX`...) and spaces before the colon
* 2026-10-21: Add: Implement settings in ScreenSaver form
* 2026-10-19: Add: [Terminal icon](https://www.flaticon.com/free-icon/terminal_7560719) by [Royyan Wijaya](https://www.flaticon.com/authors/royyan-wijaya)
* 2026-10-17: Add: Multi-Monitor support (one window per monitor, taskbar covered)
* 2026-10-17: Add: Settings dialog: random or fixed skin, message folder and file
* 2026-10-17: Upd: Build output is TerminalSS.scr (no manual rename)
* 2026-10-17: Upd: Each skin in its own class (Skins folder)
* 2026-10-17: Add: Skins: Windows95, WindowsXP, Windows7
* 2026-10-17: Del: Scroll bar of the Windows311 skin
* 2026-10-17: Del: Skin: Classic
* 2026-10-17: Upd: Skin: Modern renamed to Windows11
* 2026-10-17: Add: Skin: Windows311 (Windows 3.11 MS-DOS Prompt)
* 2026-10-16: Add: Lenient JSON mode for message files with unescaped quotes
* 2026-10-16: Add: Messages loaded from a random *.json file in the Messages folder (with subfolders)
* 2026-10-16: Add: Screen buffer with colored text, word wrapping and scrolling
* 2026-10-16: Add: Typing effect with random delay between characters
* 2026-10-16: Add: Skins: Classic (MS-DOS) and Modern (Windows 11 Command Prompt)
* 2026-10-15: Project started
