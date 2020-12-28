# Pokemon Battle Simulator in Unity (pbs-unity)

## Testing - 2020.12.28
- This branch is reserved for AWS integration testing

## Update - 2020.11.20
Came back to this project for some small updates and to more formally open-source it.
- Integrated the [Mirror Package](https://github.com/vis2k/Mirror) to enable PvP functionality.
- Created a ZenHub Board for this project to better organize issues and epics. To see it, just install the [ZenHub browser extension](https://www.zenhub.com/extension), and you should see the "ZenHub" tab listed beside "Pull Requests".
- Code cleanup and asset folder reorganization. This will be an ongoing project, with issues going under [this epic](https://github.com/AJ2O/pbs-unity/issues/11)

## Overview
This was a short project I worked on and off for about 4 months, and it simulates the Pokemon battling system for the 8th generation. The focus of the project was solely the battle system, so there is no overworld or exploration component. Furthermore, most of the present game data such as Pokemon, items, moves, abilities, etc. are placeholders used to test the wide range of mechanics. The underlying system is in place for users to add their own game data to the project.

## Screenshots
<table>
  <tr>
    <td><img src="Screenshots/Doubles Battle.png" alt="BattleScreen" width="480"/></td>
    <td><img src="Screenshots/Z-Move Select.png" alt="BattleScreen" width="480"/></td>
  </tr>
  <tr>
    <td><img src="Screenshots/Party Select.png" alt="BattleScreen" width="480"/></td>
    <td><img src="Screenshots/Turn Event 1.png" alt="BattleScreen" width="480"/></td>
  </tr>
</table>

### Wifi Battle Demo
[![Youtube Link](https://img.youtube.com/vi/AqeltOmgfMQ/0.jpg)](https://www.youtube.com/watch?v=AqeltOmgfMQ)

## Notable Features
- Single & Double Battles
- P v. P, P v. AI
- Mega Evolution, Z-Moves, Dynamax & Gigantamax
- Most Moves & Abilities
- Status (non-volatile & volatile), weather, terrain conditions
- Entry Hazards
- Some items
- More... 

## Notable Missing Features
- Audio, will be captured by [this epic](https://github.com/AJ2O/pbs-unity/issues/20)
- Most graphics (Trainers, Moves, etc.), captured by [this epic](https://github.com/AJ2O/pbs-unity/issues/21)
- Animations

## How To Play
Make sure you select the scene 'BWBattleScene' in Assets/Scenes, and click the Play button at the top of the Editor.

The scene can also be played by building the project.

### Controls
- **Arrow Keys**: Navigate battle menus
- **Spacebar**: Confirm selection
- **X**: Cancel selection
- **Z**: Trigger special command (Mega Evolution, Dynamax, or Z-Move)

### To change battle settings:
- On the Scene Hierarchy, go to **PBS -> Network Manager**
- There should be an expandable **Battle Settings** attribute
  - **Required Players** are the amount of human players needed to start a battle
  - **Total Players** are the amount of players in the battle. The amount of AI Players = Total - Required
  - Currently only tested with P v. P, and P v. AI

## IMPORTANT: Game Data
Game data entails Pokemon, types, moves, abilities, items, statuses, environmental conditions (weather, terrain), and each of these has its own class and associated database where their properties are meant to be stored. **MOST GAME DATA ARE PLACEHOLDERS.** They are not fully implemented and not meant to be used in a release. The databases are in-memory dictionaries for quick testing, but in the future these should be formal databases (SQL most likely).

## Customizability
All the game data classes are designed to be customizable and allow for easy plug-and-play of new features (moves, abilities, items, status conditions, weather, etc.). It is even possible to build new features using old ones as a base. For example, I was able to create the following features without much battle code editing:
  - 'Heavy Rain' extends from 'Rain' (simply adding new effects on top of Rain)
  - 'Toxic' extends from 'Poison' (1 additional damage stacking effect)
  - 'Blaze' and 'Overgrow' differ by one property value (the type affected)
  - 'Parental Bond' can have more hits, and scale damage differently
  - ... many, many more

# Looking for Contributors
[Discord Link](https://discord.com/invite/nNApAGQ)

### Anyone can contribute to this huge project, and contributors will be needed!
You can contribute by:
- Opening issues for missing features
- Working on open issues
- Reporting bugs
- Suggestions for improvement and optimization
- Any other way you can think of

Any small contribution helps! Here are some [smaller issues](https://github.com/AJ2O/pbs-unity/issues?q=is%3Aopen+is%3Aissue+label%3A%22good+first+issue%22) I've noted that anyone can help out with.

# Final Notes
- Most of the code isn't documented as well as I'd like, and that work will be captured by [this epic](https://github.com/AJ2O/pbs-unity/issues/9) 
- There is some legacy code, and that will generally be phased out, captured by [this epic](https://github.com/AJ2O/pbs-unity/issues/11)
- If you need help with learning what certain parts of code does, just hit up the [Discord](https://discord.com/invite/nNApAGQ)
