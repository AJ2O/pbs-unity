# Pokemon Battle Simulator in Unity (pbs-unity)

## Overview
This was a short project I worked on and off for about 4 months, and it simulates the Pokemon battling system for the 8th generation. It works for Single and Double battles, and uses a human-controlled player vs. the CPU. The focus of the project was solely the battle system, so there is no overworld or exploration component. Furthermore, most of the present game data such as Pokemon, items, moves, abilities, etc. are placeholders used to test the wide range of mechanics. The underlying system is in place for users to add their own game data to the project however, but I haven't documented it fully as of yet.

## How To Play
Make sure you select the scene 'BWBattleScene' in Assets/Scenes, and click the Play button at the top of the Editor.

The scene can also be played by building the project.

### Controls
- **Arrow Keys**: Navigate battle menus
- **Spacebar**: Confirm selection
- **X**: Cancel selection
- **Z**: Trigger special command (Mega Evolution, Dynamax, or Z-Move)

## IMPORTANT: Game Data
Game data entails Pokemon, types, moves, abilities, items, statuses, environmental conditions, etc, and each of these has its own class and associated database where their properties are meant to be stored. **MOST GAME DATA ARE PLACEHOLDERS.** They are not fully implemented and not meant to be used in a release. The databases are in-memory dictionaries for quick testing, but for a release, an external method of storing data (SQL, XML, etc.) is highly recommended.

## Battle Flow (Simplified)
The Pokemon battle flow is implemented, and here's a simplified run-down of how it works.
1. Battle initiates between two players
2. Players can choose commands for each of their controlled Pokemon (Moves, Switching, Items, Run, etc.)
3. All Commands are executed in proper order
4. If any Pokemon faint at the end of the turn, its player is prompted to replace it
5. If a player runs out of Pokemon, they lose, and the battle is over. If not, return to step 2

For those more familiar with the system, examples of more complex events are also implemented in the battle flow:
- Mega Evolution / Dynamax triggers
- Quick Claw / Quick Draw triggers
- Beak Blast / Focus Punch / Shell Trap prompts
- Switching mid-turn via U-Turn, Emergency Exit, etc.
- Pursuit triggering before switch-out
- Instruct forcing moves
- Dancer triggering after eligible moves (correctly does not execute for moves already triggered by Dancer)
- Future Sight / Doom Desire execution at the end of allotted turns
- Wish / Lunar Dance activating at the end of the turn
- Zen Mode / Schooling / Power Construct changing forms at HP-thresholds
- ... many, many others

## Move Mechanics
Most moves have synonymous counterparts differing only in simple metrics such as type, power, or accuracy. So I focused on implementing moves having different effects, and the counterparts can easily be added afterwards with a simple clone and attribute value change. Nearly every move effect is implemented, and here are a few examples:

- Almost all damage modifiers
  - ... too many to list
- Damage Overrides
  - ex. Counter, Dragon Rage, Seismic Toss, etc.
- Multi-turn attacks and associated invulnerabilites if applicable 
  - ex. Dig, Fly, Phantom Force, Sky Drop (both parties)
- Attacks needing recharge turns
- Most healing effects
  - ex. Aromatherapy, Recover, Roost (and grounding associated)
- Most item-manipulation effects
  - ex. Covet, Knock Off, Recycle, etc.
- Stat manipulation effects
  - ex. Leer, Power Swap, Belly Drum, etc.
- Type manipulation
  - ex. Burn Up, Judgement, Soak, Weather Ball
- Environment manipulation
  - ex. Gravity, Sunny Day, Trick Room, Wonder Room, etc.
- Teleport
- Relic Song
- Flying Press
- False Swipe
- G-Max Moves
- Z-Moves
- ... many, many others

Many of these were implemented using a legacy move effect class (that still works), but I was working on a new class to enhance how effects are defined. Both classes work in the system, but the new effect class is much improved.

## Ability Mechanics
All ability mechanics have been implemented up until the Crown Tundra DLC (don't think I missed any). Some notables include:
- Form-Changing
  - ex. Forecast, Hunger Switch, Multitype, Zen Mode, etc.
- Environment-Changing
  - ex. Drought, Primordial Sea, Psychic Terrain
  - Note: Easy to set custom environments, such as an ability that initiates Gravity, or Magic Room
- Stat Manipulation
  - ex. Intimidate, Justified, Moody, Rattled
- Type Properties
  - ex. Corrosion, Pixilate, Scrappy, etc.
- Multihit Overrides
  - ex. Parental Bond, Skill Link, etc.
- ... many, many others

## Item Mechanics
Most item effects are unimplimented, but the basics are present, and the others can easily be implemented following the same route as moves and abilities. Some notables implemented include:
- Healing
  - ex. Potion, Antidote, Lum Berry (and activation trigger), etc.
- Form-Changing
  - ex. Arceus Plates, Griseous Orbs, Genesect Drives, Mega Stones, etc.
- Damage modifiers
  - ex. Charcoal, Mind Plate, etc.
- Type Berries
  - ex. Chilan Berry, Occa Berry, etc.
  
## Status Conditions, Team, & Environment
All non-volatile status conditions are implemented and most volatile status conditions. Some of the status effects are in the enhanced effect class, and some are in the legacy effect class. 
The same is true for team conditions (ex. Stealth Rock, G-Max Wildfire) and environmental conditions (weather, terrain, rooms, etc.).

## Customizability
I tried to make all my game data as customizable and plug-and-play as possible. It's easy to modify or create new moves, abilities, items, status conditions, weather, terrain, and effects from pre-existing ones, **without modifying the battle system at all**. It's up to the user to populate the databases with whatever content they need, and it should all fit into the battle system seamlessly. This is a really cool feature to play around with, I'm reachable if a user needs assistance knowing how it all works. For example,  without modifying the battle code at all, the following can be achieved or changed:
  - 'Heavy Rain' extending 'Rain' (simply adding new effects on top of Rain)
  - 'Toxic' extend from 'Poison' (1 additional damage stacking effect)
  - 'Blaze' and 'Overgrow' differ by one property value (type affected)
  - Forms added or removed from 'Forecast' and will trigger based on the weather (or terrain, rooms, if you so choose)
  - The HP thresholds on Power Construct-like abilities
  - Parental Bond hits and damage scaling
  - ... many, many more

# Final Notes
Most of the code isn't documented as well as I'd like, and it has some legacy code, but I figured it'd be better if it was put up publicly than deleted. It was a short, fun project that I worked on, but I'm probably not coming back to it. If anyone else wants to work on it, I hope my prototype can help you out, and good luck! If you need help with learning what the code does, I am reachable to help you out.
