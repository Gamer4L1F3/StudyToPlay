# Study to Play

A tModLoader 1.4.4 mod that turns studying into Terraria progression.

## Current foundation (0.1.0)

- **Study menu:** press **P** or type `/study` to open the in-game menu.
- **Question bank:** answer multiple-choice questions; correct answers grant Energy.
- **Streaks:** consecutive correct answers increase rewards.
- **In-game question editor:** add custom questions with four answers and choose the correct answer. Questions are saved with the world.
- **Progression:** persistent Energy, Insight, streaks, and answer counts.
- **Upgrades:** buy Double Energy (150 Energy) and Extra Insight (200 Energy).
- **Boss Permit:** buy a permit for 100 Energy, then use `/studyboss` to consume it and authorize a boss attempt. This first version tracks authorization; it does not yet intercept every vanilla boss-summoning path.

## Install / build

1. Install tModLoader 1.4.4.
2. Put this folder in `Documents/My Games/Terraria/tModLoader/ModSources/StudyToPlay`.
3. Open tModLoader, choose Workshop → Develop Mods → Build + Reload.
4. In a world, press **P** or type `/study`.

## Roadmap

- More question formats (true/false, typed answer, multi-select, timed rounds).
- Better editor tools: edit/delete/search questions, categories, import/export JSON.
- Boss-specific costs and safe integration with vanilla and modded boss summons.
- More upgrades, achievements, daily goals, study modes, configurable rewards, and multiplayer synchronization.
- Automated build/test workflow and compatibility testing.

## Notes

This is an initial playable foundation, not a fully tested release. Please report compiler errors with the full tModLoader version and log. Balance values and features will be expanded in follow-up commits.
