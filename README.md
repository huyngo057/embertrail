# Embertrail

Top-down **2D action-adventure**. You play a trailblazer exploring a crumbling frontier of forgotten shrines and wild roads. Combat is real-time (dash, light/heavy attacks); adventure comes from short explorable regions, unlockable shortcuts, and **ember** checkpoints that mark how far you’ve pushed into the unknown.

## Status

**Repo bootstrap only.** No Godot project, scenes, or gameplay code yet.

| Step | Status |
|------|--------|
| 1. Git repo | Done |
| 2. Install Godot agent skills | Next |
| 3. Godot 4 + C# scaffold | Later |
| 4. Playable draft → enhance | Later |

## Next: agent skills

Install into this project (project-local Cursor skills). Start with **`godot-master` only** — never `--all` (that floods context).

```bash
npx skills add thedivergentai/gd-agentic-skills/skills/godot-master -a cursor -y
```

Verify:

```bash
npx skills list -a cursor
```

Then restart the agent so it picks up the skill. Add domain skills later as needed (e.g. character body 2D, combat, input). Optional local reference clone: `~/Projects/GD-Agentic-Skills` — do not vendor the whole library into this repo.

Project skills land under Cursor’s project skill path (e.g. `.agents/skills/` or `.cursor/skills/` depending on the installer).

## Stack (planned)

- Godot 4 (.NET / C#)
- OOP-first entities and systems; draft a small playable loop first, then enhance
