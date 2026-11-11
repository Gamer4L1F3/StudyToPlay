# Study to Play

A tModLoader 1.4.4 mod that turns studying into Terraria progression.

## Features in the current foundation

- **In-game Study Menu:** press **P** or type `/study`.
- **Global question bank:** one bank shared across all worlds on this Terraria installation.
- **In-game question manager:** browse questions, create new ones, edit existing ones, and delete unwanted ones.
- **Four-choice questions:** choose the correct answer while creating or editing.
- **Energy and streaks:** correct answers earn Energy; consecutive correct answers improve rewards.
- **Insight:** every five correct answers earns Insight.
- **Upgrades:** Double Energy (150 Energy) and Extra Insight (200 Energy).
- **Boss Permit:** purchase a permit for 100 Energy and consume it with `/studyboss`.

## Global question bank storage

The mod manages its own file at the tModLoader config location named `StudyToPlay_Questions.json`. You do not need to edit this file: all question creation and management is done in-game. Because the file is global client configuration, it is shared between worlds on the same installation.

## Install / build

1. Install tModLoader 1.4.4.
2. Put this folder in `Documents/My Games/Terraria/tModLoader/ModSources/StudyToPlay`.
3. Open tModLoader, choose Workshop → Develop Mods → Build + Reload.
4. Enter a world and press **P** or type `/study`.

## Known limitations

This is still an early development build and has not yet been compiled against a local tModLoader installation. Boss Permits are tracked as authorizations but do not yet intercept every vanilla or modded boss-summoning route. Multiplayer synchronization and server-authoritative question rewards are not implemented yet.

## Planned next

- True/false, typed-answer, multi-select, and timed question formats.
- Boss-specific costs and progression-aware summon integration.
- More upgrade branches, achievements, daily goals, and study modes.
- Multiplayer support, accessibility options, and UI polish.
