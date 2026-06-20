# MetSozluk — Turkish Dictionary with Trie Data Structure

**A C# WinForms Turkish dictionary application that uses a trie (prefix tree) for fast word lookup and auto-suggestion.**

![Language](https://img.shields.io/badge/language-C%23-blue)
![Platform](https://img.shields.io/badge/platform-.NET%20WinForms-green)
![IDE](https://img.shields.io/badge/IDE-Visual%20Studio%202008-9cf)

---

## Overview

MetSozluk is a university project from **April 2008** (Sakarya University, Computer Science) that implements a **Turkish dictionary** using a custom trie data structure. As the user types into a text box, the application searches the trie in real-time and displays the word's definition — a classic auto-complete / prefix search system.

The dictionary data is loaded from a plain-text file (`sozluk.txt`) at startup. Each entry is parsed, inserted into the trie, and then available for O(k) lookup where k is the length of the query word.

---

## Features

- **🔤 Real-time word search** — type a word and see its definition instantly
- **🌳 Trie-based data structure** — efficient prefix-tree storage and retrieval
- **📂 Flat-file dictionary** — words and definitions stored in `sozluk.txt`
- **🔄 Auto-suggestion** — trie enables prefix-based look-ahead
- **🎯 Turkish language support** — works with Turkish characters and dictionary format

---

## How It Works

### Trie Structure (`harf` class)

Each node in the trie is a `harf` object with:

| Field     | Type      | Purpose                                    |
|-----------|-----------|--------------------------------------------|
| `h`       | `char`    | The letter value at this node              |
| `yan`     | `harf`    | Horizontal pointer (next sibling at same depth) |
| `alt`     | `harf`    | Vertical pointer (next letter in the word) |
| `goster`  | `int`     | Display flag; `-1` = not a word end, `> -1` = line index in dictionary file |

### Building the Trie

At startup, `dosyayiYukle()`:

1. Opens `sozluk.txt` from the application directory
2. For each line, extracts the word (the part between `$` and `:`)
3. Calls `ekle(word, lineNumber)` to insert it into the trie

The insertion algorithm (`ekle`):
- Walks through each character of the word
- At each depth level, searches horizontally (`yan`) for the matching letter
- If not found, creates a new node
- Moves vertically (`alt`) to the next character position
- Marks the last node's `goster` with the dictionary line number

### Searching

The `ara(word)` method:
- Traverses the trie character by character
- At each step, finds the matching letter horizontally (`yan`) then descends vertically (`alt`)
- Returns the `goster` value (line number) if found, or `-1` if not found

### Auto-Complete Flow

```
User types → textBox1_TextChanged → ara() → trie lookup → display definition
```

---

## File Format

The dictionary file (`sozluk.txt`) uses the following line format:

```
$word:definition
```

For example:
```
$merhaba:hello, hi
$kitap:book
$bilgisayar:computer
```

The word is extracted between `$` and `:`, and the definition follows the `:`.

---

## Project Structure

```
MetSozlukl/
├── Form1.cs          # Main form with trie logic (206 lines)
├── Form1.Designer.cs # WinForms designer code
├── Form1.resx        # Form resources
├── Program.cs        # Application entry point
├── Properties/       # Assembly info and resources
└── sozluk.txt        # Dictionary data file (must be in the same folder)
MetSozlukl.sln        # Visual Studio solution file
```

---

## Building & Running

1. **Prerequisites**: .NET Framework 2.0+ (or compatible), Visual Studio 2008+
2. Open `MetSozlukl.sln` in Visual Studio
3. Ensure `sozluk.txt` is placed in the output directory (or same folder as the `.exe`)
4. Build and run (F5)

> **Note**: You may need to retarget the .NET Framework version in project properties if using a newer Visual Studio.

---

## Concepts Demonstrated

| Concept | Implementation |
|---------|---------------|
| **Trie (Prefix Tree)** | Custom linked-list-based trie with horizontal/vertical pointers |
| **Dictionary Data Structure** | Key-value storage using trie nodes + line indices |
| **Prefix Search** | Real-time lookup as user types each character |
| **Auto-Complete** | Trie enables O(k) prefix-based word completion |
| **File I/O** | `StreamReader` to parse structured text files |
| **Linked Lists** | `yan` (horizontal) and `alt` (vertical) form a custom linked node structure |

---

## License

Academic project — provided as-is for educational purposes.
