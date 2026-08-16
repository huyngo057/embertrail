# Embertrail

Top-down **2D action-adventure** built with **Godot 4.7 (.NET / C#)**.

You play a trailblazer exploring a crumbling frontier of forgotten shrines and wild roads. Combat is real-time (dash, light/heavy attacks); adventure comes from short explorable regions, unlockable shortcuts, and **ember** checkpoints.

## Status

| Step | Status |
|------|--------|
| Git repo | Done |
| Godot .NET + C# scaffold | Done |
| Playable draft v0 (one room) | Done |
| Enhance systems | Next |

## Requirements

- [Godot 4.7+ **.NET / Mono** build](https://godotengine.org/download) (`brew install --cask godot-mono`)
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (`dotnet --list-sdks` should show `8.x`)
- [Cursor](https://cursor.com) for C# editing (with `anysphere.csharp` extension)

Do **not** use the standard (non-.NET) Godot build — C# will not run.

## Workflow

| Tool | Use for |
|------|---------|
| **Godot_mono** | Scenes, Input Map, run/debug (F5) |
| **Cursor** | C# scripts, OOP structure, git, agent |

In Godot: **Editor Settings → Dotnet → Editor → External Editor → Custom**, set exec path to `/opt/homebrew/bin/cursor` and args `{project} --goto {file}:{line}:{col}`.

## Controls (draft)

- **WASD / arrows** — move
- **Space** — dash

## Run

```bash
# from repo root
dotnet build
godot-mono --path . scenes/Main.tscn
# or open the folder in Godot_mono and press F5
```

## Project layout

```
entities/player/   Player scene + C#
entities/enemy/    Enemy placeholder + C#
scenes/Main.tscn   One bounded room draft
```

Local agent skill installs (`.agents/`, `skills-lock.json`) are gitignored and are not part of this game repo.
