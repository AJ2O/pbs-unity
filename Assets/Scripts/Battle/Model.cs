using PBS.Data;
using PBS.Databases;
using PBS.Main.Pokemon;
using PBS.Main.Team;
using PBS.Main.Trainer;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle
{
    public class Model
    {
        // General
        public int turns;
        public BattleSettings battleSettings;
        public BattleEnvironment environment;

        // Teams
        public List<Main.Pokemon.Pokemon> pokemonOnField;
        public List<Team> teams;

        // Battling Conditions
        public string defaultWeather;
        public string defaultTerrain;
        public string defaultGravity;
        public string defaultMagicRoom;
        public string defaultTrickRoom;
        public string defaultWonderRoom;

        public BattleCondition weather;
        public BattleCondition terrain;
        public BattleCondition gravity;
        public BattleCondition magicRoom;
        public BattleCondition trickRoom;
        public BattleCondition wonderRoom;
        public List<BattleCondition> statusConditions;

        // Other Variables
        public Main.Pokemon.Pokemon runningPokemon;
        public string lastUsedMove;
        public List<BattleFutureSightCommand> futureSightCommands;
        public List<BattleWishCommand> wishCommands;

        // Constructor
        public Model(
            BattleSettings battleSettings,
            BattleEnvironment environment = null,
            int turns = 0,
            List<Team> teams = null,
            bool setPos = true,
            List<BattleCondition> statusConditions = null,

            string defaultWeather = null,
            string defaultTerrain = null,
            string defaultGravity = null,
            string defaultMagicRoom = null,
            string defaultTrickRoom = null,
            string defaultWonderRoom = null
            )
        {
            this.turns = turns;
            this.battleSettings = battleSettings;
            this.environment = environment == null ? new BattleEnvironment()
                : BattleEnvironment.Clone(environment);

            this.teams = new List<Team>();
            if (teams != null)
            {
                for (int i = 0; i < teams.Count; i++)
                {
                    this.teams.Add(teams[i]);
                    if (setPos)
                    {
                        this.teams[i].SetTeamPos(teams[i].teamID);
                    }
                }
            }
            pokemonOnField = new List<Main.Pokemon.Pokemon>();

            weather = !string.IsNullOrEmpty(defaultWeather) ? new BattleCondition(defaultWeather)
                : new BattleCondition(BattleStatuses.instance.GetDefaultWeather().ID);
            terrain = !string.IsNullOrEmpty(defaultTerrain) ? new BattleCondition(defaultTerrain)
                : new BattleCondition(BattleStatuses.instance.GetDefaultTerrain().ID);
            gravity = !string.IsNullOrEmpty(defaultGravity) ? new BattleCondition(defaultGravity)
                : new BattleCondition(BattleStatuses.instance.GetDefaultGravity().ID);
            magicRoom = !string.IsNullOrEmpty(defaultMagicRoom) ? new BattleCondition(defaultMagicRoom)
                : new BattleCondition(BattleStatuses.instance.GetDefaultMagicRoom().ID);
            trickRoom = !string.IsNullOrEmpty(defaultTrickRoom) ? new BattleCondition(defaultTrickRoom)
                : new BattleCondition(BattleStatuses.instance.GetDefaultTrickRoom().ID);
            wonderRoom = !string.IsNullOrEmpty(defaultWonderRoom) ? new BattleCondition(defaultWonderRoom)
                : new BattleCondition(BattleStatuses.instance.GetDefaultWonderRoom().ID);

            this.defaultWeather = weather.statusID;
            this.defaultTerrain = terrain.statusID;
            this.defaultGravity = gravity.statusID;
            this.defaultMagicRoom = magicRoom.statusID;
            this.defaultTrickRoom = trickRoom.statusID;
            this.defaultWonderRoom = wonderRoom.statusID;

            this.statusConditions = new List<BattleCondition>();
            if (statusConditions != null)
            {
                for (int i = 0; i < statusConditions.Count; i++)
                {
                    this.statusConditions.Add(BattleCondition.Clone(statusConditions[i]));
                }
            }

            runningPokemon = null;
            lastUsedMove = null;
            futureSightCommands = new List<BattleFutureSightCommand>();
            wishCommands = new List<BattleWishCommand>();
        }

        // Clone
        public static Model CloneModel(Model original)
        {
            // teams
            List<Team> teams = new List<Team>();
            for (int i = 0; i < original.teams.Count; i++)
            {
                teams.Add(Team.Clone(original.teams[i]));
            }

            Model cloneModel = new Model(
                battleSettings: original.battleSettings,
                environment: original.environment,

                turns: original.turns,
                teams: teams,
                setPos: false,

                statusConditions: original.statusConditions,

                defaultWeather: original.defaultWeather,
                defaultTerrain: original.defaultTerrain,
                defaultGravity: original.defaultGravity,
                defaultMagicRoom: original.defaultMagicRoom,
                defaultTrickRoom: original.defaultTrickRoom,
                defaultWonderRoom: original.defaultWonderRoom
                );

            // pokemon on field
            List<Main.Pokemon.Pokemon> pokemonOnField = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < original.pokemonOnField.Count; i++)
            {
                pokemonOnField.Add(cloneModel.GetBattleInstanceOfPokemon(original.pokemonOnField[i]));
            }
            cloneModel.pokemonOnField = pokemonOnField;
            cloneModel.lastUsedMove = original.lastUsedMove;

            cloneModel.weather = BattleCondition.Clone(original.weather);
            cloneModel.terrain = BattleCondition.Clone(original.terrain);
            cloneModel.gravity = BattleCondition.Clone(original.gravity);
            cloneModel.magicRoom = BattleCondition.Clone(original.magicRoom);
            cloneModel.trickRoom = BattleCondition.Clone(original.trickRoom);
            cloneModel.wonderRoom = BattleCondition.Clone(original.wonderRoom);

            cloneModel.runningPokemon = original.runningPokemon == null ? null
                : cloneModel.GetBattleInstanceOfPokemon(original.runningPokemon);
            for (int i = 0; i < original.futureSightCommands.Count; i++)
            {
                cloneModel.futureSightCommands.Add(BattleFutureSightCommand.Clone(original.futureSightCommands[i]));
            }
            for (int i = 0; i < original.wishCommands.Count; i++)
            {
                cloneModel.wishCommands.Add(BattleWishCommand.Clone(original.wishCommands[i]));
            }

            return cloneModel;
        }

        // To String
        public override string ToString()
        {
            string str = "Battle.Core.Model - Turn " + turns + "\n";

            str += "Pokemon On Field:\n";
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                Main.Pokemon.Pokemon pokemon = pokemonOnField[i];
                str += pokemon.nickname + " (" + pokemon.uniqueIDTrunc + ") " + " / ";
            }
            str += "\n";

            for (int i = 0; i < teams.Count; i++)
            {
                Team team = teams[i];
                str += "Team " + team.teamID + ": ";
                for (int j = 0; j < team.trainers.Count; j++)
                {
                    Trainer trainer = team.trainers[j];
                    str += trainer.name + " / ";
                }
                str += "\n";
            }

            return str;
        }

        // General Methods
        public void AdvanceTurn()
        {
            turns++;
            AdvanceBattleStatusTurns();

            for (int i = 0; i < futureSightCommands.Count; i++)
            {
                if (futureSightCommands[i].turnsLeft > 0)
                {
                    futureSightCommands[i].turnsLeft--;
                }
            }
            for (int i = 0; i < wishCommands.Count; i++)
            {
                if (wishCommands[i].turnsLeft > 0)
                {
                    wishCommands[i].turnsLeft--;
                }
            }

        }
        public void AdvanceTurnPokemon(Main.Pokemon.Pokemon pokemon)
        {
            pokemon.bProps.turnsActive++;

            // Abilities
            List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(pokemon);
            for (int i = 0; i < pbAbilities.Count; i++)
            {
                pbAbilities[i].turnsActive++;
            }

            // Dynamax / Gigantamax
            if (pokemon.dynamaxProps.turnsLeft > 0
                && pokemon.dynamaxState != Main.Pokemon.Pokemon.DynamaxState.None)
            {
                pokemon.dynamaxProps.turnsLeft--;
            }

            // Electrify
            pokemon.bProps.electrify = null;
            // Embargo
            if (pokemon.bProps.embargo != null)
            {
                pokemon.bProps.embargo.justInitialized = false;
                if (pokemon.bProps.embargo.turnsLeft > 0)
                {
                    pokemon.bProps.embargo.turnsLeft--;
                }
            }
            // Endure
            pokemon.bProps.endure = null;
            // Flinch
            pokemon.bProps.flinch = null;
            // Forest's Curse
            for (int i = 0; i < pokemon.bProps.forestsCurses.Count; i++)
            {
                if (pokemon.bProps.forestsCurses[i].turnsLeft > 0)
                {
                    pokemon.bProps.forestsCurses[i].turnsLeft--;
                }
            }
            // Lock On
            for (int i = 0; i < pokemon.bProps.lockOnTargets.Count; i++)
            {
                if (pokemon.bProps.lockOnTargets[i].turnsLeft > 0)
                {
                    pokemon.bProps.lockOnTargets[i].turnsLeft--;
                }
            }
            // Move-Limiting (Disable, Encore, Taunt, etc.)
            for (int i = 0; i < pokemon.bProps.moveLimiters.Count; i++)
            {
                pokemon.bProps.moveLimiters[i].justInitialized = false;
                if (pokemon.bProps.moveLimiters[i].turnsLeft > 0)
                {
                    pokemon.bProps.moveLimiters[i].turnsLeft--;
                }
            }
            // Perish Song
            if (pokemon.bProps.perishSong != null)
            {
                if (pokemon.bProps.perishSong.turnsLeft > 0)
                {
                    pokemon.bProps.perishSong.turnsLeft--;
                }
            }
            // Protect
            pokemon.bProps.protect = null;
            // Truant
            AbilityEffectPair truantEffectPair = PBPGetAbilityEffectPair(pokemon, AbilityEffectType.Truant);
            if (truantEffectPair == null)
            {
                pokemon.bProps.truantTurns = 0;
            }

            // Yawn
            if (pokemon.bProps.yawn != null)
            {
                if (pokemon.bProps.yawn.turnsLeft > 0)
                {
                    pokemon.bProps.yawn.turnsLeft--;
                }
            }

            pokemon.bProps.ATKRaised = false;
            pokemon.bProps.ATKLowered = false;
            pokemon.bProps.DEFRaised = false;
            pokemon.bProps.DEFLowered = false;
            pokemon.bProps.SPARaised = false;
            pokemon.bProps.SPALowered = false;
            pokemon.bProps.SPDRaised = false;
            pokemon.bProps.SPDLowered = false;
            pokemon.bProps.SPERaised = false;
            pokemon.bProps.SPELowered = false;
            pokemon.bProps.ACCRaised = false;
            pokemon.bProps.ACCLowered = false;
            pokemon.bProps.EVARaised = false;
            pokemon.bProps.EVALowered = false;

            pokemon.bProps.actedThisTurn = false;
            pokemon.bProps.switchedIn = false;
            pokemon.bProps.isSwitchingOut = false;
            pokemon.bProps.isFlinching = false;
            pokemon.bProps.isMagicCoatActive = false;
            pokemon.bProps.usingMeFirst = false;
            pokemon.bProps.isCenterOfAttention = false;

            pokemon.bProps.endureMove = null;
            pokemon.bProps.protectMove = null;

            pokemon.bProps.magicCoatMove = null;
            pokemon.bProps.powderMove = null;
            pokemon.bProps.roostMove = null;

            pokemon.bProps.beakBlastMove = null;
            pokemon.bProps.focusPunchMove = null;
            pokemon.bProps.shellTrapMove = null;

            pokemon.bProps.wasStruckForDamage = false;
            pokemon.bProps.wasHitByOpponent = false;
            pokemon.bProps.wasHitByAlly = false;

            pokemon.bProps.preHPPercent = GetPokemonHPAsPercentage(pokemon);
            pokemon.bProps.turnPhysicalDamageTaken = 0;
            pokemon.bProps.turnSpecialDamageTaken = 0;

            if (pokemon.bProps.bideTurnsLeft > 0)
            {
                pokemon.bProps.bideTurnsLeft--;
            }
            if (pokemon.bProps.bindTurns > 0)
            {
                pokemon.bProps.bindTurns--;
            }
            if (pokemon.bProps.confusionTurns > 0)
            {
                pokemon.bProps.confusionTurns--;
            }
            if (pokemon.bProps.lastMoveTargetedTurns > 0)
            {
                pokemon.bProps.lastMoveTargetedTurns--;
                if (pokemon.bProps.lastMoveTargetedTurns == 0)
                {
                    pokemon.bProps.lastMoveTargetedBy = null;
                    pokemon.bProps.lastTargeterPokemon = null;
                }
            }

            pokemon.bProps.helpingHandMoves.Clear();

            // Advance Status
            List<StatusCondition> statusConditions = GetPokemonStatusConditions(pokemon);
            for (int i = 0; i < statusConditions.Count; i++)
            {
                StatusCondition condition = statusConditions[i];
                condition.turnsActive++;
                if (condition.data.HasTag(PokemonSTag.TurnsDecreaseOnEnd))
                {
                    if (condition.turnsLeft > 0)
                    {
                        condition.turnsLeft--;
                    }
                }
            }

        }
        public void AdvanceTurnTeam(Team team)
        {
            // entry hazards
            for (int i = 0; i < team.bProps.entryHazards.Count; i++)
            {
                if (team.bProps.entryHazards[i].turnsLeft > 0)
                {
                    team.bProps.entryHazards[i].turnsLeft--;
                }
            }
            if (team.bProps.GMaxWildfireStatus != null)
            {
                if (team.bProps.GMaxWildfireStatus.turnsLeft > 0)
                {
                    team.bProps.GMaxWildfireStatus.turnsLeft--;
                }
            }
            // Mat Block
            team.bProps.matBlocks.Clear();
            // screens
            for (int i = 0; i < team.bProps.reflectScreens.Count; i++)
            {
                if (team.bProps.reflectScreens[i].turnsLeft > 0)
                {
                    team.bProps.reflectScreens[i].turnsLeft--;
                }
            }
            // safeguards
            for (int i = 0; i < team.bProps.safeguards.Count; i++)
            {
                if (team.bProps.safeguards[i].turnsLeft > 0)
                {
                    team.bProps.safeguards[i].turnsLeft--;
                }
            }

            team.bProps.protectMovesActive.Clear();

            // status
            AdvanceTeamStatusTurns(team);
        }

        public bool IsFinished()
        {
            // Check running pokemon
            if (runningPokemon != null)
            {
                return true;
            }

            List<Team> teamsLeft = new List<Team>(teams);
            for (int i = 0; i < teams.Count; i++)
            {
                Team team = teams[i];
                if (!IsTeamAbleToBattle(team))
                {
                    teamsLeft.Remove(team);
                }
            }

            // 1 Winner = Decisive Winner
            // 0 Winners = Tie
            return teamsLeft.Count <= 1;
        }
        public Team GetWinningTeam()
        {
            List<Team> teamsLeft = new List<Team>(teams);
            for (int i = 0; i < teams.Count; i++)
            {
                Team team = teams[i];
                if (!IsTeamAbleToBattle(team))
                {
                    teamsLeft.Remove(team);
                }
            }
            return teamsLeft.Count == 0 ? null : teamsLeft[0];
        }
        public List<Team> GetLosingTeams()
        {
            List<Team> losingTeams = new List<Team>();
            for (int i = 0; i < teams.Count; i++)
            {
                Team team = teams[i];
                if (!IsTeamAbleToBattle(team))
                {
                    losingTeams.Add(team);
                }
            }
            return losingTeams;
        }

        // Battle.Core.Model Modes
        public bool IsSinglesBattle()
        {
            if (teams.Count > 2)
            {
                return false;
            }
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamMode != Team.TeamMode.Single)
                {
                    return false;
                }
            }
            return true;
        }

        // Command methods
        public List<BattleCommandType> GetPokemonPossibleCommandTypes(Main.Pokemon.Pokemon pokemon)
        {
            return new List<BattleCommandType>
            {
                BattleCommandType.Fight,
                BattleCommandType.Party,
                BattleCommandType.Bag,
                BattleCommandType.Run
            };
        }
        public List<ItemBattlePocket> GetTrainerItemPockets(Trainer trainer)
        {
            return new List<ItemBattlePocket>
            {
                ItemBattlePocket.HPRestore,
                ItemBattlePocket.Pokeballs,
                ItemBattlePocket.StatusRestore,
                ItemBattlePocket.BattleItems
            };
        }
        public void ConvertToInstanceBattleCommand(BattleCommand command)
        {
            ConvertToInstanceBattleCommands(new List<BattleCommand> { command });
        }
        public void ConvertToInstanceBattleCommands(List<BattleCommand> commands)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                BattleCommand curCommand = commands[i];
                if (curCommand.commandUser != null)
                {
                    curCommand.commandUser = GetBattleInstanceOfPokemon(curCommand.commandUser);
                }
                if (curCommand.switchInPokemon != null)
                {
                    curCommand.switchInPokemon = GetBattleInstanceOfPokemon(curCommand.switchInPokemon);
                }
                if (curCommand.switchingTrainer != null)
                {
                    curCommand.switchingTrainer = GetBattleInstanceOfTrainer(curCommand.switchingTrainer);
                }
                if (curCommand.itemTrainer != null)
                {
                    curCommand.itemTrainer = GetBattleInstanceOfTrainer(curCommand.itemTrainer);
                }
            }
        }
        public BattleCommand SanitizeCommand(Player.Command command)
        {
            return SanitizeCommands(new List<Player.Command> { command })[0];
        }
        public List<BattleCommand> SanitizeCommands(List<Player.Command> commands)
        {
            List<BattleCommand> sanitizedCommands = new List<BattleCommand>();
            for (int i = 0; i < commands.Count; i++)
            {
                Player.Command curCommand = commands[i];
                BattleCommand sanitizedCommand = BattleCommand.CreateFromPlayerCommand(curCommand);

                if (!string.IsNullOrEmpty(curCommand.commandUser))
                {
                    sanitizedCommand.commandUser = GetBattleInstanceOfPokemon(curCommand.commandUser);
                }
                if (!string.IsNullOrEmpty(curCommand.switchInPokemon))
                {
                    sanitizedCommand.switchInPokemon = GetBattleInstanceOfPokemon(curCommand.switchInPokemon);
                }
                if (curCommand.commandTrainer != 0)
                {
                    sanitizedCommand.commandTrainer = GetBattleInstanceOfTrainer(curCommand.commandTrainer);
                }
                if (curCommand.switchingTrainer != 0)
                {
                    sanitizedCommand.switchingTrainer = GetBattleInstanceOfTrainer(curCommand.switchingTrainer);
                }
                if (curCommand.itemTrainer != 0)
                {
                    sanitizedCommand.itemTrainer = GetBattleInstanceOfTrainer(curCommand.itemTrainer);
                }

                sanitizedCommands.Add(sanitizedCommand);
            }
            return sanitizedCommands;
        }
        public void RecalibrateCommand(BattleCommand command)
        {
            if (command.commandType == BattleCommandType.Fight)
            {
                // priority
                Move moveData = GetPokemonMoveData(
                    userPokemon: command.commandUser,
                    moveID: command.moveID,
                    command: command);
                command.commandPriority = moveData.priority;
            }
        }

        /// <summary>
        /// Recalculates the priority of the given commands.
        /// </summary>
        /// <param name="commands"></param>
        public void RecalibrateCommands(List<BattleCommand> commands)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                RecalibrateCommand(commands[i]);
            }
        }
        public static bool IsFightCommand(BattleCommandType commandType)
        {
            return commandType == BattleCommandType.Fight
                || commandType == BattleCommandType.Recharge;
        }
        public BattleOrder GetCommandOrder(BattleCommand command1, BattleCommand command2)
        {
            // set base order to be between the speeds
            BattleOrder battleOrder = GetPokemonOrder(command1.commandUser, command2.commandUser);

            // Special Cases
            // If wild battle, make wild pokemon fleeing go last
            Trainer trainer = GetPokemonOwner(command1.commandUser);
            if (trainer.isWildPokemon && command1.commandType == BattleCommandType.Run)
            {
                return BattleOrder.Last;
            }

            // different command types
            if (command1.commandType != command2.commandType)
            {
                // check command type: move always goes last
                if (IsFightCommand(command2.commandType) && !IsFightCommand(command1.commandType))
                {
                    return BattleOrder.First;
                }
                else if (IsFightCommand(command1.commandType) && !IsFightCommand(command2.commandType))
                {
                    return BattleOrder.Last;
                }

                // Order is: 1. Switch, 2. Bag, 3. Run, 4. Fight
                if (command1.commandType == BattleCommandType.Party && command2.commandType != BattleCommandType.Party)
                {
                    return BattleOrder.First;
                }
                if (command1.commandType != BattleCommandType.Party && command2.commandType == BattleCommandType.Party)
                {
                    return BattleOrder.Last;
                }

                if (command1.commandType == BattleCommandType.Bag && command2.commandType != BattleCommandType.Bag)
                {
                    return BattleOrder.First;
                }
                if (command1.commandType != BattleCommandType.Bag && command2.commandType == BattleCommandType.Bag)
                {
                    return BattleOrder.Last;
                }

                if (command1.commandType == BattleCommandType.Run && command2.commandType != BattleCommandType.Run)
                {
                    return BattleOrder.First;
                }
                if (command1.commandType != BattleCommandType.Run && command2.commandType == BattleCommandType.Run)
                {
                    return BattleOrder.Last;
                }

                // check priority
                if (command1.commandPriority > command2.commandPriority)
                {
                    return BattleOrder.First;
                }
                else if (command2.commandPriority > command1.commandPriority)
                {
                    return BattleOrder.Last;
                }
            }
            // same command type
            else
            {
                // check priority
                if (command1.commandPriority > command2.commandPriority)
                {
                    return BattleOrder.First;
                }
                else if (command2.commandPriority > command1.commandPriority)
                {
                    return BattleOrder.Last;
                }
            }
            return battleOrder;
        }
        public List<BattleCommand> GetFilteredCommands(BattleCommandType commandType, List<BattleCommand> originalCommands)
        {
            List<BattleCommand> filteredCommands = new List<BattleCommand>();
            for (int i = 0; i < originalCommands.Count; i++)
            {
                if (originalCommands[i].commandType == commandType)
                {
                    filteredCommands.Add(originalCommands[i]);
                }
            }
            return filteredCommands;
        }
        /// <summary>
        /// Reorders a given list of commands based on the current battle state.
        /// </summary>
        /// <param name="originalCommands"></param>
        /// <returns></returns>
        public List<BattleCommand> ReorderCommands(List<BattleCommand> originalCommands, bool recalibrateFirst = true)
        {
            if (recalibrateFirst)
            {
                RecalibrateCommands(originalCommands);
            }

            List<BattleCommand> orderedCommands = new List<BattleCommand>();

            for (int i = 0; i < originalCommands.Count; i++)
            {
                BattleCommand unsortedCommand = originalCommands[i];
                // if no sorted commands, make it the first sorted element
                if (orderedCommands.Count == 0)
                {
                    orderedCommands.Add(unsortedCommand);
                }
                // compare command to the already sorted commands
                else
                {
                    bool isCommandInserted = false;
                    for (int j = 0; j < orderedCommands.Count && !isCommandInserted; j++)
                    {
                        BattleCommand sortedCommand = orderedCommands[j];

                        // Get the order of the commands, of the unsorted in relation to the sorted
                        BattleOrder commandOrder = GetCommandOrder(unsortedCommand, sortedCommand);

                        // insert before if it goes first
                        if (commandOrder == BattleOrder.First)
                        {
                            orderedCommands.Insert(j, unsortedCommand);
                            isCommandInserted = true;
                            break;
                        }
                        // if it goes after, we continue
                        else if (commandOrder == BattleOrder.Last)
                        {
                            // continue
                        }
                        // if it's a speed tie, flip a coin to see who goes first
                        else if (commandOrder == BattleOrder.SpeedTie)
                        {
                            if (Random.value < 0.5f)
                            {
                                orderedCommands.Insert(j, unsortedCommand);
                                isCommandInserted = true;
                                break;
                            }
                        }
                    }
                    // if we didn't get to insert the command, it was lower priority than all the sorted commands
                    // we add it to the end of the list
                    if (!isCommandInserted)
                    {
                        orderedCommands.Add(unsortedCommand);
                    }
                }
            }

            return orderedCommands;
        }
        public List<BattleCommand> GetPresetCommands()
        {
            List<BattleCommand> commands = new List<BattleCommand>();
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                bool setMove = false;
                // Recharge Turn
                if (!setMove && pokemonOnField[i].bProps.rechargeTurns > 0)
                {
                    setMove = true;
                    BattleCommand command = BattleCommand.CreateRechargeCommand(pokemonOnField[i]);
                    commands.Add(command);
                }

                // Fixed Command
                if (!setMove)
                {
                    BattleCommand nextCommand = pokemonOnField[i].ConvertNextCommand();
                    if (nextCommand != null)
                    {
                        setMove = true;
                        commands.Add(nextCommand);
                        pokemonOnField[i].UnsetNextCommand();
                    }
                }
            }
            return commands;
        }
        public List<BattleCommand> GetAbleCommands(
            List<BattleCommand> choices,
            IEnumerable<BattleCommand> committedCommmands)
        {
            List<BattleCommand> commands = new List<BattleCommand>();
            for (int i = 0; i < choices.Count; i++)
            {
                if (CanChooseCommand(choices[i], committedCommmands).isQuerySuccessful)
                {
                    commands.Add(choices[i]);
                }
            }
            return commands;
        }
        public View.Events.MessageParameterized CanChooseCommand(
            BattleCommand attemptedCommand,
            IEnumerable<BattleCommand> committedCommands)
        {
            View.Events.MessageParameterized response = new View.Events.MessageParameterized
            {
                isQueryResponse = true
            };
            bool bypassChecks = false;
            bool commandSuccess = true;
            List<BattleCommand> setCommands = new List<BattleCommand>(committedCommands);
            Main.Pokemon.Pokemon userPokemon = attemptedCommand.commandUser;

            // move commands
            if (attemptedCommand.commandType == BattleCommandType.Fight)
            {
                // auto-success for struggle
                Move moveData = Moves.instance.GetMoveData(attemptedCommand.moveID);
                if (moveData.GetEffect(MoveEffectType.Struggle) != null)
                {
                    bypassChecks = true;
                }

                if (!bypassChecks)
                {
                    // Can't Mega-Evolve / Z-Move / Dynamax twice
                    if (commandSuccess)
                    {
                        for (int i = 0; i < setCommands.Count; i++)
                        {
                            if (setCommands[i] != null)
                            {
                                if (setCommands[i].isMegaEvolving && attemptedCommand.isMegaEvolving)
                                {
                                    commandSuccess = false;
                                    response.messageCode = "bpc-mega-already";
                                    break;
                                }
                                else if (setCommands[i].isZMove && attemptedCommand.isZMove)
                                {
                                    commandSuccess = false;
                                    response.messageCode = "bpc-zmove-already";
                                    break;
                                }
                                else if (setCommands[i].isDynamaxing && attemptedCommand.isDynamaxing)
                                {
                                    commandSuccess = false;
                                    response.messageCode = "bpc-dynamax-already";
                                    break;
                                }
                            }
                        }
                    }

                    // Imprison
                    if (commandSuccess)
                    {
                        Main.Pokemon.Pokemon imprisonPokemon = PBPGetImprison(userPokemon, moveData);
                        if (imprisonPokemon != null)
                        {
                            commandSuccess = false;
                            response.messageCode = imprisonPokemon.bProps.imprison.chooseText;
                            response.pokemonUserID = userPokemon.uniqueID;
                        }
                    }

                    // Choiced Moves
                    if (commandSuccess)
                    {
                        if (!string.IsNullOrEmpty(userPokemon.bProps.choiceMove)
                            && IsPokemonChoiced(userPokemon))
                        {
                            bool bypassChoiceLock = moveData.HasTag(MoveTag.IgnoreChoiceLock);
                            Move choiceMoveData = Moves.instance.GetMoveData(userPokemon.bProps.choiceMove);
                            if (!bypassChoiceLock
                                && choiceMoveData.ID != moveData.ID)
                            {
                                commandSuccess = false;
                                response.messageCode = "pokemon-choiced";
                                response.moveID = choiceMoveData.ID;
                                response.pokemonUserID = userPokemon.uniqueID;
                            }
                        }
                    }

                    // Limited Moves
                    if (commandSuccess)
                    {
                        for (int i = 0; i < userPokemon.bProps.moveLimiters.Count && commandSuccess; i++)
                        {
                            Main.Pokemon.BattleProperties.MoveLimiter moveLimiter =
                                userPokemon.bProps.moveLimiters[i];
                            PBS.Databases.Effects.PokemonStatuses.MoveLimiting effect_ = moveLimiter.effect;

                            string moveLimitID = moveData.ID;

                            // Disable
                            if (effect_ is PBS.Databases.Effects.PokemonStatuses.Disable)
                            {
                                if (moveLimiter.affectedMoves.Contains(moveData.ID))
                                {
                                    commandSuccess = false;
                                }
                            }
                            // Encore
                            else if (effect_ is PBS.Databases.Effects.PokemonStatuses.Encore)
                            {
                                if (!moveLimiter.affectedMoves.Contains(moveData.ID))
                                {
                                    commandSuccess = false;
                                    moveLimitID = moveLimiter.GetMove();
                                }
                            }
                            // Heal Block
                            else if (effect_ is PBS.Databases.Effects.PokemonStatuses.HealBlock)
                            {
                                if (IsHealingMove(moveData))
                                {
                                    commandSuccess = false;
                                }
                            }
                            // Taunt
                            else if (effect_ is PBS.Databases.Effects.PokemonStatuses.Taunt)
                            {
                                PBS.Databases.Effects.PokemonStatuses.Taunt taunt = effect_ as PBS.Databases.Effects.PokemonStatuses.Taunt;
                                if (taunt.category == moveData.category)
                                {
                                    commandSuccess = false;
                                }
                            }
                            // Torment
                            else if (effect_ is PBS.Databases.Effects.PokemonStatuses.Torment)
                            {
                                if (moveLimiter.affectedMoves.Contains(moveData.ID))
                                {
                                    commandSuccess = false;
                                }
                            }

                            if (!commandSuccess)
                            {
                                response.messageCode = effect_.attemptText;
                                response.moveID = moveLimitID;
                                response.pokemonUserID = userPokemon.uniqueID;
                            }
                        }
                    }
                }
            }
            // party commands
            else if (attemptedCommand.commandType == BattleCommandType.Party
                || attemptedCommand.commandType == BattleCommandType.PartyReplace)
            {
                // check if switching is possible

                // party-specfic restrictions
                if (commandSuccess
                    && attemptedCommand.commandType == BattleCommandType.Party)
                {
                    // Trapped
                    if (commandSuccess)
                    {
                        // Block
                        if (commandSuccess && !string.IsNullOrEmpty(userPokemon.bProps.blockMove))
                        {
                            commandSuccess = false;
                            Main.Pokemon.Pokemon blockUser = GetFieldPokemonByID(userPokemon.bProps.blockPokemon);
                            Move moveData = Moves.instance.GetMoveData(userPokemon.bProps.blockMove);
                            MoveEffect effect = moveData.GetEffect(MoveEffectType.Block);

                            string textCodeID = effect.GetString(2);
                            textCodeID = textCodeID == "DEFAULT" ? "move-block-trap-default"
                                : textCodeID;
                            response.messageCode = textCodeID;
                            response.pokemonUserID = blockUser.uniqueID;
                            response.pokemonTargetID = userPokemon.uniqueID;
                            response.moveID = userPokemon.bProps.blockMove;
                        }

                        // Bind
                        if (commandSuccess && !string.IsNullOrEmpty(userPokemon.bProps.bindMove))
                        {
                            commandSuccess = false;
                            Main.Pokemon.Pokemon bindUser = GetFieldPokemonByID(userPokemon.bProps.bindPokemon);
                            Move moveData = Moves.instance.GetMoveData(userPokemon.bProps.bindMove);
                            MoveEffect effect = moveData.GetEffect(MoveEffectType.Bind);

                            string textCodeID = effect.GetString(3);
                            textCodeID = textCodeID == "DEFAULT" ? "move-bind-trap-default"
                                : textCodeID;
                            response.messageCode = textCodeID;
                            response.pokemonUserID = bindUser.uniqueID;
                            response.pokemonTargetID = userPokemon.uniqueID;
                            response.moveID = userPokemon.bProps.bindMove;
                        }

                        // Sky Drop
                        if (commandSuccess && !string.IsNullOrEmpty(userPokemon.bProps.skyDropMove))
                        {
                            commandSuccess = false;
                            Main.Pokemon.Pokemon trapUser = GetFieldPokemonByID(userPokemon.bProps.skyDropUser);
                            Move moveData = Moves.instance.GetMoveData(userPokemon.bProps.skyDropMove);
                            MoveEffect effect = moveData.GetEffect(MoveEffectType.SkyDrop);

                            string textCodeID = effect.GetString(2);
                            textCodeID = textCodeID == "DEFAULT" ? "move-skydrop-trap-default"
                                : textCodeID;
                            response.messageCode = textCodeID;
                            response.pokemonUserID = trapUser.uniqueID;
                            response.pokemonTargetID = userPokemon.uniqueID;
                            response.moveID = userPokemon.bProps.skyDropMove;
                        }

                        // Ingrain
                        if (commandSuccess)
                        {
                            for (int i = 0; i < userPokemon.bProps.ingrainMoves.Count; i++)
                            {
                                Move ingrainData = Moves.instance.GetMoveData(
                                    userPokemon.bProps.ingrainMoves[i]
                                    );
                                MoveEffect ingrainEffect = ingrainData.GetEffect(MoveEffectType.Ingrain);
                                if (ingrainEffect.GetBool(0))
                                {
                                    commandSuccess = false;
                                    response.messageCode = "pokemon-switch-ingrain";
                                    response.pokemonUserID = userPokemon.uniqueID;
                                    response.moveID = ingrainData.ID;
                                    break;
                                }
                            }
                        }
                    }

                    // TODO: Trapping Abilities (Arena Trap, Shadow Tag, etc.)
                    if (commandSuccess)
                    {
                        for (int i = 0; i < pokemonOnField.Count && commandSuccess; i++)
                        {
                            Main.Pokemon.Pokemon trapPokemon = pokemonOnField[i];
                            Data.Ability abilityData =
                                PBPGetAbilityDataWithEffect(trapPokemon, AbilityEffectType.ShadowTag);
                            if (!trapPokemon.IsTheSameAs(userPokemon) && abilityData != null)
                            {
                                PBS.Databases.Effects.Abilities.AbilityEffect shadowTag_ =
                                    abilityData.GetEffectNew(AbilityEffectType.ShadowTag);
                                PBS.Databases.Effects.Abilities.ShadowTag shadowTag =
                                    shadowTag_ as PBS.Databases.Effects.Abilities.ShadowTag;

                                if (DoEffectFiltersPass(
                                    filters: shadowTag.filters,
                                    userPokemon: trapPokemon,
                                    targetPokemon: userPokemon,
                                    abilityData: abilityData
                                    ))
                                {
                                    bool trapped = true;

                                    // Shed Shell
                                    if (trapped)
                                    {
                                        PBS.Databases.Effects.Items.ItemEffect shedShell_ =
                                            PBPGetItemEffect(userPokemon, ItemEffectType.ShedShell);
                                        if (shedShell_ != null)
                                        {
                                            PBS.Databases.Effects.Items.ShedShell shedShell =
                                                shedShell_ as PBS.Databases.Effects.Items.ShedShell;
                                            trapped = false;
                                        }
                                    }

                                    // Immune to own ability
                                    if (trapped
                                        && shadowTag.immuneToSelf)
                                    {
                                        Data.Ability abilityData2 = PBPGetAbilityDataWithEffect(
                                            userPokemon,
                                            AbilityEffectType.ShadowTag);
                                        if (abilityData2 != null)
                                        {
                                            if (abilityData.ID == abilityData2.ID)
                                            {
                                                trapped = false;
                                            }
                                        }
                                    }

                                    // Arena Trap
                                    if (trapped
                                        && shadowTag.arenaTrap
                                        && !PBPIsPokemonGrounded(userPokemon))
                                    {
                                        trapped = false;
                                    }

                                    // Must be adjacent
                                    if (trapped
                                        && shadowTag.mustBeAdjacent
                                        && !ArePokemonAdjacent(trapPokemon, userPokemon))
                                    {
                                        trapped = false;
                                    }

                                    if (trapped)
                                    {
                                        commandSuccess = false;
                                        response.messageCode = shadowTag.displayText;
                                        response.pokemonUserID = trapPokemon.uniqueID;
                                        response.pokemonTargetID = userPokemon.uniqueID;
                                        response.abilityID = abilityData.ID;
                                    }
                                }
                            }
                        }
                    }
                }

                // can't switch in fainted pokemon
                if (commandSuccess
                    && IsPokemonFainted(attemptedCommand.switchInPokemon))
                {
                    commandSuccess = false;
                    response.messageCode = "bpc-switch-unable";
                    response.pokemonID = attemptedCommand.switchInPokemon.uniqueID;
                }

                // can't switch in on-field pokemon
                if (commandSuccess
                    && IsPokemonOnField(attemptedCommand.switchInPokemon))
                {
                    commandSuccess = false;
                    response.messageCode = "bpc-switch-already";
                    response.pokemonID = attemptedCommand.switchInPokemon.uniqueID;
                }

                // can't switch in a pokemon already committed to switch in
                if (commandSuccess)
                {
                    // can't switch in a pokemon already committed to switch in
                    for (int i = 0; i < setCommands.Count; i++)
                    {
                        if (setCommands[i] != null)
                        {
                            Main.Pokemon.Pokemon otherPokemon = setCommands[i].switchInPokemon;
                            if (otherPokemon != null)
                            {
                                if (otherPokemon.IsTheSameAs(attemptedCommand.switchInPokemon))
                                {
                                    commandSuccess = false;
                                    response.messageCode = "bpc-switch-already-switch";
                                    response.pokemonID = attemptedCommand.switchInPokemon.uniqueID;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            // bag commands
            else if (attemptedCommand.commandType == BattleCommandType.Bag)
            {
                Main.Pokemon.Pokemon itemPokemon = attemptedCommand.commandUser;
                Trainer itemTrainer = attemptedCommand.itemTrainer;
                PBS.Data.Item itemData = Items.instance.GetItemData(attemptedCommand.itemID);

                response.messageCode = "item-use-fail";
                // Make sure the trainer has enough of the items in their inventory
                if (commandSuccess)
                {
                    int itemCount = itemTrainer.GetItemCount(itemData.ID);
                    for (int i = 0; i < setCommands.Count; i++)
                    {
                        if (setCommands[i] != null)
                        {
                            BattleCommand itemCommand = setCommands[i];
                            if (itemCommand.itemID == itemData.ID)
                            {
                                itemCount--;
                            }
                        }
                    }

                    if (itemCount <= 0)
                    {
                        commandSuccess = false;
                        response.messageCode = "item-use-fail-notenough";
                        response.pokemonUserID = itemPokemon.uniqueID;
                        response.itemID = itemData.ID;
                    }
                }

                // Fainted?
                if (commandSuccess)
                {
                    if (IsPokemonFainted(itemPokemon))
                    {
                        // check items that can be used on fainted pokemon
                        if (itemData.GetEffect(ItemEffectType.Revive) == null)
                        {
                            commandSuccess = false;
                            response.pokemonUserID = itemPokemon.uniqueID;
                            response.itemID = itemData.ID;
                        }
                    }
                    else
                    {
                        // check items that can only be used on fainted pokemon
                        if (itemData.GetEffect(ItemEffectType.Revive) != null)
                        {
                            commandSuccess = false;
                            response.pokemonUserID = itemPokemon.uniqueID;
                            response.itemID = itemData.ID;
                        }
                    }
                }

                // Embargo
                if (commandSuccess)
                {
                    if (itemPokemon.bProps.embargo != null && !itemData.HasTag(ItemTag.BypassEmbargo))
                    {
                        commandSuccess = false;
                        response.messageCode = itemPokemon.bProps.embargo.effect.attemptText;
                        response.pokemonUserID = itemPokemon.uniqueID;
                        response.itemID = itemData.ID;
                    }
                }

                // Only useable in battle
                if (commandSuccess)
                {
                    if (itemData.HasTag(ItemTag.OnlyUseableInBattle)
                        && !IsPokemonOnField(itemPokemon))
                    {
                        commandSuccess = false;
                        response.messageCode = "item-use-fail-battle";
                        response.pokemonUserID = itemPokemon.uniqueID;
                        response.itemID = itemData.ID;
                    }
                }
            }
            // run commands
            else if (attemptedCommand.commandType == BattleCommandType.Run)
            {
                response.messageCode = "pokemon-run-trap";
                response.pokemonUserID = userPokemon.uniqueID;

                // Trapped
                if (commandSuccess)
                {
                    PBS.Databases.Effects.Abilities.AbilityEffect runAway_ =
                        PBPGetAbilityEffect(userPokemon, AbilityEffectType.RunAway);
                    PBS.Databases.Effects.Abilities.RunAway runAway = runAway_ == null ? null :
                        runAway_ as PBS.Databases.Effects.Abilities.RunAway;

                    // trainer battle
                    if (!battleSettings.isWildBattle)
                    {
                        commandSuccess = false;
                        response.messageCode = "bpc-run-trainer";
                    }

                    // Block
                    if (commandSuccess
                        && !string.IsNullOrEmpty(userPokemon.bProps.blockMove)
                        && runAway == null)
                    {
                        commandSuccess = false;
                    }

                    // Bind
                    if (commandSuccess
                        && !string.IsNullOrEmpty(userPokemon.bProps.bindMove)
                        && runAway == null)
                    {
                        commandSuccess = false;
                    }

                    // Sky Drop
                    if (commandSuccess && !string.IsNullOrEmpty(userPokemon.bProps.skyDropMove))
                    {
                        commandSuccess = false;
                    }

                    // Trapped
                    StatusCondition trapStatus = GetTrapStatusCondition(attemptedCommand.commandUser);
                    if (commandSuccess
                        && trapStatus != null
                        && runAway == null)
                    {
                        commandSuccess = false;
                    }

                    // Ingrain
                    if (commandSuccess && runAway == null)
                    {
                        for (int i = 0; i < userPokemon.bProps.ingrainMoves.Count; i++)
                        {
                            Move ingrainData = Moves.instance.GetMoveData(
                                userPokemon.bProps.ingrainMoves[i]);
                            MoveEffect ingrainEffect = ingrainData.GetEffect(MoveEffectType.Ingrain);
                            if (ingrainEffect.GetBool(0))
                            {
                                commandSuccess = false;
                                response.messageCode = "pokemon-run-ingrain";
                                response.pokemonUserID = userPokemon.uniqueID;
                                response.moveID = ingrainData.ID;
                                break;
                            }
                        }
                    }
                }

                // TODO: Trapping Abilities (Arena Trap, Shadow Tag, etc.)
            }

            response.isQuerySuccessful = commandSuccess;
            return response;
        }

        // Future Sight Commands
        public void AddFutureSightCommand(BattleFutureSightCommand command)
        {
            futureSightCommands.Add(command);
        }
        public bool CanFutureSightTargets(BattlePosition[] battlePositions)
        {
            for (int i = 0; i < futureSightCommands.Count; i++)
            {
                BattleFutureSightCommand command = futureSightCommands[i];
                for (int k = 0; k < battlePositions.Length; k++)
                {
                    if (command.IsTargetingPosition(battlePositions[k]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public BattleCommand CreateFutureSightMoveCommand(BattleFutureSightCommand command)
        {
            BattleCommand moveCommand = BattleCommand.CreateMoveCommand(
                commandUser: GetPokemonByID(command.pokemonID),
                moveID: command.moveID,
                targetPositions: command.targetPositions
                );
            moveCommand.bypassStatusInterrupt = true;
            moveCommand.consumePP = false;
            moveCommand.isFutureSightMove = true;

            Move moveData = Moves.instance.GetMoveData(command.moveID);
            MoveEffect effect = moveData.GetEffect(MoveEffectType.FutureSight);
            if (effect != null)
            {
                moveCommand.bypassRedirection = effect.GetBool(0);
            }
            return moveCommand;
        }

        // Wish Command
        public void AddWishCommand(BattleWishCommand command)
        {
            wishCommands.Add(command);
        }

        // Matching methods
        public Trainer GetBattleInstanceOfTrainer(Trainer searchTrainer)
        {
            List<Trainer> allTrainers = GetTrainers();
            for (int i = 0; i < allTrainers.Count; i++)
            {
                if (allTrainers[i].IsTheSameAs(searchTrainer))
                {
                    return allTrainers[i];
                }
            }
            return null;
        }
        public Trainer GetBattleInstanceOfTrainer(int playerID)
        {
            List<Trainer> allTrainers = GetTrainers();
            for (int i = 0; i < allTrainers.Count; i++)
            {
                if (allTrainers[i].IsTheSameAs(playerID))
                {
                    return allTrainers[i];
                }
            }
            return null;
        }
        public Main.Pokemon.Pokemon GetBattleInstanceOfPokemon(Main.Pokemon.Pokemon searchPokemon)
        {
            List<Main.Pokemon.Pokemon> allPokemon = GetPokemonFromAllTrainers();
            for (int i = 0; i < allPokemon.Count; i++)
            {
                if (allPokemon[i].IsTheSameAs(searchPokemon))
                {
                    return allPokemon[i];
                }
            }
            return null;
        }
        public Main.Pokemon.Pokemon GetBattleInstanceOfPokemon(string pokemonID)
        {
            List<Main.Pokemon.Pokemon> allPokemon = GetPokemonFromAllTrainers();
            for (int i = 0; i < allPokemon.Count; i++)
            {
                if (allPokemon[i].IsTheSameAs(pokemonID))
                {
                    return allPokemon[i];
                }
            }
            return null;
        }
        public Team GetBattleInstanceOfTeam(Team searchTeam)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamID == searchTeam.teamID)
                {
                    return teams[i];
                }
            }
            return null;
        }
        public Team GetBattleInstanceOfTeam(int teamID)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamID == teamID)
                {
                    return teams[i];
                }
            }
            return null;
        }

        // Team Methods
        public Team GetAllyTeam(int teamPos)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamID == teamPos)
                {
                    return teams[i];
                }
            }
            return null;
        }
        public List<Team> GetEnemyTeams(int teamPos)
        {
            List<Team> enemyTeams = new List<Team>();
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamID != teamPos)
                {
                    enemyTeams.Add(teams[i]);
                }
            }
            return enemyTeams;
        }
        public Team GetTeam(Main.Pokemon.Pokemon pokemon)
        {
            if (pokemon.teamPos != -1)
            {
                return GetTeamFromPosition(pokemon.teamPos);
            }
            Trainer trainer = GetPokemonOwner(pokemon);
            return GetTeamFromPosition(trainer.teamID);
        }
        public Team GetTeamFromPosition(int teamPos)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamID == teamPos)
                {
                    return teams[i];
                }
            }
            return null;
        }
        public Team GetTeamFromBattlePosition(BattlePosition position)
        {
            return GetTeamFromPosition(position.teamPos);
        }
        /// <summary>
        /// Returns true if this team can still battle.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public bool IsTeamAbleToBattle(Team team)
        {
            for (int j = 0; j < team.trainers.Count; j++)
            {
                Trainer trainer = team.trainers[j];
                if (IsTrainerAbleToBattle(trainer))
                {
                    return true;
                }
            }
            return false;
        }
        public Dictionary<Trainer, List<Main.Pokemon.Pokemon>> ForceSendAllPokemon()
        {
            Dictionary<Trainer, List<Main.Pokemon.Pokemon>> sentOutMap = new Dictionary<Trainer, List<Main.Pokemon.Pokemon>>();

            // Force out on all sides
            for (int i = 0; i < teams.Count; i++)
            {
                for (int j = 0; j < teams[i].trainers.Count; j++)
                {
                    Trainer trainer = teams[i].trainers[j];
                    List<Main.Pokemon.Pokemon> sentPokemon = ForceSendTrainerPokemon(trainer);
                    if (sentPokemon.Count > 0)
                    {
                        sentOutMap.Add(trainer, sentPokemon);
                    }
                }
            }

            return sentOutMap;
        }
        public List<Main.Pokemon.Pokemon> GetTeamPokemonOnField(Team team)
        {
            List<Main.Pokemon.Pokemon> pokemonOnField = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < team.trainers.Count; i++)
            {
                pokemonOnField.AddRange(GetTrainerOnFieldPokemon(team.trainers[i]));
            }
            return pokemonOnField;
        }

        // Trainer Methods
        /// <summary>
        /// Returns true if the given trainer is still able to battle.
        /// </summary>
        /// <param name="trainer"></param>
        /// <returns></returns>
        public bool IsTrainerAbleToBattle(Trainer trainer)
        {
            for (int i = 0; i < trainer.party.Count; i++)
            {
                if (trainer.party[i].IsAbleToBattle())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Returns the first pokemon found in the trainer's party that is able to battle that's not already battling.
        /// </summary>
        /// <param name="trainer">The trainer whose party is being searched.</param>
        /// <returns></returns>
        public Main.Pokemon.Pokemon GetTrainerFirstAvailablePokemon(Trainer trainer)
        {
            List<Main.Pokemon.Pokemon> pokemon = GetTrainerFirstXAvailablePokemon(trainer, 1);
            if (pokemon.Count > 0)
            {
                return pokemon[0];
            }
            return null;
        }
        /// <summary>
        /// Returns up to X pokemon in the trainer's party that are able to battle that are not already in battle.
        /// </summary>
        /// <param name="x">The maximum number of Pokemon to search for.</param>
        /// <param name="trainer">The trainer whose party is being searched.</param>
        /// <returns></returns>
        public List<Main.Pokemon.Pokemon> GetTrainerFirstXAvailablePokemon(Trainer trainer, int x)
        {
            List<Main.Pokemon.Pokemon> pokemon = new List<Main.Pokemon.Pokemon>();
            int count = 0;

            for (int i = 0; i < trainer.party.Count; i++)
            {
                if (count == x)
                {
                    break;
                }
                if (trainer.party[i].IsAbleToBattle() && !IsPokemonOnField(trainer.party[i]))
                {
                    pokemon.Add(trainer.party[i]);
                    count++;
                }
            }
            return pokemon;
        }
        public List<Main.Pokemon.Pokemon> GetTrainerAllAvailablePokemon(Trainer trainer)
        {
            List<Main.Pokemon.Pokemon> pokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < trainer.party.Count; i++)
            {
                if (trainer.party[i].IsAbleToBattle())
                {
                    pokemon.Add(trainer.party[i]);
                }
            }
            return pokemon;
        }
        /// <summary>
        /// Returns all of the pokemon on the battlefield for the given trainer.
        /// </summary>
        /// <param name="trainer"></param>
        /// <returns></returns>
        public List<Main.Pokemon.Pokemon> GetTrainerOnFieldPokemon(Trainer trainer)
        {
            List<Main.Pokemon.Pokemon> pokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (trainer.HasPokemon(pokemonOnField[i]))
                {
                    pokemon.Add(pokemonOnField[i]);
                }
            }
            return pokemon;
        }
        /// <summary>
        /// Returns all of the Pokemon on the battlefield that the given trainer has control to give commands for.
        /// </summary>
        /// <param name="trainer"></param>
        /// <returns></returns>
        public List<Main.Pokemon.Pokemon> GetTrainerCommandablePokemon(Trainer trainer)
        {
            List<Main.Pokemon.Pokemon> trainerPokemon = GetTrainerOnFieldPokemon(trainer);
            List<Main.Pokemon.Pokemon> commandablePokemon = new List<Main.Pokemon.Pokemon>();

            for (int i = 0; i < trainerPokemon.Count; i++)
            {
                // TODO: Run some checks
                bool commandable = true;

                // Two-Turn Attacks, Recharge Turns, etc.
                if (trainerPokemon[i].bProps.nextCommand != null)
                {
                    commandable = false;
                }
                if (trainerPokemon[i].bProps.rechargeTurns > 0)
                {
                    commandable = false;
                }

                if (commandable)
                {
                    bool addedPokemon = false;

                    for (int k = 0; k < commandablePokemon.Count; k++)
                    {
                        if (trainerPokemon[i].battlePos < commandablePokemon[k].battlePos)
                        {
                            commandablePokemon.Insert(k, trainerPokemon[i]);
                            addedPokemon = true;
                            break;
                        }
                    }
                    if (!addedPokemon)
                    {
                        commandablePokemon.Add(trainerPokemon[i]);
                    }
                }
            }

            return commandablePokemon;
        }
        /// <summary>
        /// Returns all of the Pokemon on the battlefield that the given trainer has control to give commands for.
        /// </summary>
        /// <param name="trainer"></param>
        /// <returns></returns>
        public List<View.Events.CommandAgent> GetTrainerCommandableAgents(Trainer trainer)
        {
            List<Main.Pokemon.Pokemon> trainerPokemon = GetTrainerOnFieldPokemon(trainer);
            List<View.Events.CommandAgent> agents = new List<View.Events.CommandAgent>();

            for (int i = 0; i < trainerPokemon.Count; i++)
            {
                Main.Pokemon.Pokemon pokemon = trainerPokemon[i];

                // TODO: Run some checks
                bool commandable = true;

                // Two-Turn Attacks, Recharge Turns, etc.
                if (pokemon.bProps.nextCommand != null)
                {
                    commandable = false;
                }
                if (pokemon.bProps.rechargeTurns > 0)
                {
                    commandable = false;
                }

                if (commandable)
                {
                    // Moves
                    List<View.Events.CommandAgent.Moveslot> agentMoves
                        = new List<View.Events.CommandAgent.Moveslot>();
                    List<View.Events.CommandAgent.Moveslot> agentZMoves
                        = new List<View.Events.CommandAgent.Moveslot>();
                    List<View.Events.CommandAgent.Moveslot> agentMaxMoves
                        = new List<View.Events.CommandAgent.Moveslot>();
                    List<Moveslot> moveslots = GetPokemonBattleMoveslots(pokemon);

                    // Z-Moves
                    bool canZMove = false;
                    for (int k = 0; k < moveslots.Count; k++)
                    {
                        Move moveData = Moves.instance.GetMoveData(moveslots[k].moveID);
                        Move ZMoveData = GetPokemonZMoveData(pokemon, moveslots[k].moveID);
                        Move maxMoveData = GetPokemonMaxMoveData(pokemon, moveData);

                        if (ZMoveData != null)
                        {
                            canZMove = true;
                            agentZMoves.Add(new View.Events.CommandAgent.Moveslot
                            {
                                moveID = ZMoveData.ID,
                                PP = moveslots[k].PP,
                                maxPP = moveslots[k].maxPP,

                                basePower = ZMoveData.basePower,
                                accuracy = ZMoveData.accuracy,

                                useable = CanPokemonUseMove(pokemon, ZMoveData.ID)
                            });
                        }
                        else
                        {
                            agentZMoves.Add(new View.Events.CommandAgent.Moveslot
                            {
                                moveID = moveData.ID,
                                PP = moveslots[k].PP,
                                maxPP = moveslots[k].maxPP,

                                basePower = moveData.basePower,
                                accuracy = moveData.accuracy,

                                hide = true,
                                useable = CanPokemonUseMove(pokemon, moveData.ID)
                            });
                        }
                        agentMoves.Add(new View.Events.CommandAgent.Moveslot
                        {
                            moveID = moveData.ID,
                            PP = moveslots[k].PP,
                            maxPP = moveslots[k].maxPP,

                            basePower = moveData.basePower,
                            accuracy = moveData.accuracy,

                            useable = CanPokemonUseMove(pokemon, moveData.ID)
                        });
                        agentMaxMoves.Add(new View.Events.CommandAgent.Moveslot
                        {
                            moveID = maxMoveData.ID,
                            PP = moveslots[k].PP,
                            maxPP = moveslots[k].maxPP,

                            basePower = maxMoveData.basePower,
                            accuracy = maxMoveData.accuracy,

                            useable = CanPokemonUseMove(pokemon, maxMoveData.ID)
                        });
                    }

                    // Mega Evolution
                    bool canMegaEvolve = false;
                    Item item = pokemon.item;
                    if (item != null)
                    {
                        PBS.Databases.Effects.Items.ItemEffect formChangeItemEffect =
                            PBPGetItemFormChangeEffect(pokemon, item);
                        if (formChangeItemEffect != null)
                        {
                            if (formChangeItemEffect is PBS.Databases.Effects.Items.MegaStone)
                            {
                                canMegaEvolve = true;
                            }
                            // Ultra Burst / Other form change items
                            else
                            {

                            }
                        }
                    }

                    // Dynamax
                    bool canDynamax = true;

                    // Set all parameters
                    View.Events.CommandAgent agent = new View.Events.CommandAgent
                    {
                        pokemonUniqueID = pokemon.uniqueID,
                        canMegaEvolve = canMegaEvolve,
                        canZMove = canZMove,
                        canDynamax = canDynamax,
                        isDynamaxed = pokemon.dynamaxState != Main.Pokemon.Pokemon.DynamaxState.None,
                        commandTypes = GetPokemonPossibleCommandTypes(pokemon),
                        moveslots = agentMoves,
                        zMoveSlots = agentZMoves,
                        dynamaxMoveSlots = agentMaxMoves
                    };

                    bool addedPokemon = false;

                    for (int k = 0; k < agents.Count; k++)
                    {
                        if (trainerPokemon[i].battlePos < pokemon.battlePos)
                        {
                            agents.Insert(k, agent);
                            addedPokemon = true;
                            break;
                        }
                    }
                    if (!addedPokemon)
                    {
                        agents.Add(agent);
                    }
                }
            }

            return agents;
        }
        public List<Main.Pokemon.Pokemon> ForceSendTrainerPokemon(Trainer trainer)
        {
            List<Main.Pokemon.Pokemon> sentPokemon = new List<Main.Pokemon.Pokemon>();

            List<int> emptyPositions = GetEmptyTrainerControlPositions(trainer);
            List<Main.Pokemon.Pokemon> availablePokemon = GetTrainerFirstXAvailablePokemon(trainer, emptyPositions.Count);
            for (int i = 0; i < availablePokemon.Count; i++)
            {
                sentPokemon.Add(availablePokemon[i]);
                sentPokemon[i].teamPos = trainer.teamID;
                sentPokemon[i].battlePos = emptyPositions[i];
                pokemonOnField.Add(sentPokemon[i]);
            }

            return sentPokemon;
        }
        public List<int> GetEmptyTrainerControlPositions(Trainer trainer)
        {
            // start a list with all potential positions
            List<int> positions = new List<int>();
            for (int i = 0; i < trainer.controlPos.Length; i++)
            {
                positions.Add(trainer.controlPos[i]);
            }

            // remove all filled positions if they occur
            for (int j = 0; j < pokemonOnField.Count; j++)
            {
                Main.Pokemon.Pokemon pokemon = pokemonOnField[j];
                if (pokemon.teamPos == trainer.teamID
                    && positions.Contains(pokemon.battlePos))
                {
                    positions.Remove(pokemon.battlePos);
                }
            }
            return positions;
        }
        public List<int> GetFillableTrainerPositions(Trainer trainer)
        {
            List<int> emptyPositions = GetEmptyTrainerControlPositions(trainer);
            List<Main.Pokemon.Pokemon> availablePokemon = GetTrainerFirstXAvailablePokemon(trainer, emptyPositions.Count);
            for (int i = 0; i < trainer.party.Count; i++)
            {

            }
            return emptyPositions;
        }
        public void TrainerWithdrawPokemon(Trainer trainer, Main.Pokemon.Pokemon pokemon)
        {
            if (pokemon.dynamaxState != Main.Pokemon.Pokemon.DynamaxState.None)
            {
                if (pokemon.dynamaxState == Main.Pokemon.Pokemon.DynamaxState.Gigantamax)
                {
                    PBPRevertForm(pokemon);
                }
                pokemon.dynamaxState = Main.Pokemon.Pokemon.DynamaxState.None;
            }

            // Natural Cure / Regenerator
            if (!IsPokemonFainted(pokemon))
            {
                List<PBS.Databases.Effects.Abilities.AbilityEffect> naturalCure_ =
                    PBPGetAbilityEffects(pokemon, AbilityEffectType.NaturalCure);
                for (int i = 0; i < naturalCure_.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.NaturalCure naturalCure =
                        naturalCure_[i] as PBS.Databases.Effects.Abilities.NaturalCure;
                    if (DoEffectFiltersPass(
                        filters: naturalCure.filters,
                        targetPokemon: pokemon
                        ))
                    {
                        // Natural Cure
                        if (naturalCure.conditions.Count > 0)
                        {
                            for (int k = 0; k < naturalCure.conditions.Count; k++)
                            {
                                // heal statuses
                                List<StatusCondition> healableSCs = PBPGetSCs(pokemon, naturalCure.conditions[k]);
                                for (int j = 0; j < healableSCs.Count; j++)
                                {
                                    HealStatusCondition(pokemon, healableSCs[j]);
                                }
                            }
                        }

                        // Regenerator
                        if (naturalCure.regenerator != null)
                        {
                            int HPToHeal = GetHeal(heal: naturalCure.regenerator, targetPokemon: pokemon);
                            AddPokemonHP(pokemon, HPToHeal);
                        }
                    }
                }
            }
            RemovePokemonOnField(pokemon);
        }
        public void TrainerSendPokemon(Trainer trainer, Main.Pokemon.Pokemon pokemon, int emptyPos)
        {
            pokemon.teamPos = trainer.teamID;
            pokemon.battlePos = emptyPos;
            pokemon.bProps.switchedIn = true;
            AddPokemonToField(pokemon);
        }
        public void TrainerSwapPokemon(Trainer trainer, Main.Pokemon.Pokemon pokemon1, Main.Pokemon.Pokemon pokemon2)
        {
            trainer.SwapPartyPokemon(pokemon1, pokemon2);
        }

        /// <summary>
        /// Returns all the trainers in the battle.
        /// </summary>
        /// <returns></returns>
        public List<Trainer> GetTrainers()
        {
            List<Trainer> allTrainers = new List<Trainer>();
            for (int i = 0; i < teams.Count; i++)
            {
                allTrainers.AddRange(teams[i].trainers);
            }
            return allTrainers;
        }
        /// <summary>
        /// Gets the trainer whose player ID matches the given one.
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public Trainer GetTrainerWithID(int playerID)
        {
            foreach (Team team in teams)
            {
                foreach (Trainer trainer in team.trainers)
                {
                    if (trainer.playerID == playerID)
                    {
                        return trainer;
                    }
                }
            }
            return null;
        }
        public Main.Pokemon.Pokemon GetTrainerPokemonAtFaintPos(Trainer trainer, int faintPos)
        {
            for (int i = 0; i < trainer.party.Count; i++)
            {
                if (trainer.party[i].faintPos == faintPos)
                {
                    return trainer.party[i];
                }
            }
            return null;
        }

        // Pokemon: Trainer Methods
        /// <summary>
        /// Returns the trainer that owns the given pokemon. Returns null if there is no such trainer.
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public Trainer GetPokemonOwner(Main.Pokemon.Pokemon pokemon)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                for (int j = 0; j < teams[i].trainers.Count; j++)
                {
                    Trainer trainer = teams[i].trainers[j];
                    if (trainer.HasPokemon(pokemon))
                    {
                        return trainer;
                    }
                }
            }
            return null;
        }
        public bool ArePokemonAndTrainerSameTeam(Main.Pokemon.Pokemon pokemon, Trainer trainer)
        {
            return pokemon.teamPos == trainer.teamID;
        }

        // Pokemon: Field / Positioning Methods
        public Main.Pokemon.Pokemon GetPokemonByID(string uniqueID)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                for (int k = 0; k < teams[i].trainers.Count; k++)
                {
                    Trainer trainer = teams[i].trainers[k];
                    for (int j = 0; j < trainer.party.Count; j++)
                    {
                        Main.Pokemon.Pokemon pokemon = trainer.party[j];
                        if (pokemon.uniqueID == uniqueID)
                        {
                            return pokemon;
                        }
                    }
                }
            }
            return null;
        }
        public Main.Pokemon.Pokemon GetFieldPokemonByID(string uniqueID)
        {
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (pokemonOnField[i].uniqueID == uniqueID)
                {
                    return pokemonOnField[i];
                }
            }
            return null;
        }
        public bool AddPokemonToField(Main.Pokemon.Pokemon pokemon)
        {
            pokemonOnField.Add(pokemon);
            return true;
        }
        public bool RemovePokemonOnField(Main.Pokemon.Pokemon pokemon)
        {
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (pokemonOnField[i].IsTheSameAs(pokemon))
                {
                    pokemonOnField[i].teamPos = -1;
                    pokemonOnField[i].battlePos = -1;

                    // unset dynamax
                    if (pokemonOnField[i].dynamaxState != Main.Pokemon.Pokemon.DynamaxState.None)
                    {
                        if (pokemonOnField[i].dynamaxState == Main.Pokemon.Pokemon.DynamaxState.Gigantamax)
                        {
                            PBPRevertForm(pokemonOnField[i]);
                        }
                        PBPUndynamax(pokemonOnField[i]);
                    }

                    ResetBattleOnlyProperties(pokemonOnField[i]);
                    pokemonOnField.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public void RemoveAllOnFieldPokemon()
        {
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                pokemonOnField[i].teamPos = -1;
                pokemonOnField[i].battlePos = -1;
            }
            pokemonOnField.Clear();
        }
        public void SetOnFieldPokemon()
        {
            RemoveAllOnFieldPokemon();
            for (int i = 0; i < teams.Count; i++)
            {
                for (int j = 0; j < teams[i].trainers.Count; j++)
                {
                    for (int k = 0; k < teams[i].trainers[j].party.Count; k++)
                    {
                        Main.Pokemon.Pokemon pokemon = teams[i].trainers[j].party[k];
                        if (pokemon.teamPos != -1 && pokemon.battlePos != -1)
                        {
                            pokemonOnField.Add(pokemon);
                        }
                    }
                }
            }
        }
        public bool IsPokemonOnField(Main.Pokemon.Pokemon pokemon)
        {
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (pokemon.IsTheSameAs(pokemonOnField[i]))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsPokemonOnFieldAndAble(Main.Pokemon.Pokemon pokemon)
        {
            if (pokemon == null) return false;
            return !IsPokemonFainted(pokemon) && IsPokemonOnField(pokemon);
        }
        public List<BattlePosition> GetAllBattlePositions()
        {
            List<BattlePosition> battlePositions = new List<BattlePosition>();
            for (int i = 0; i < teams.Count; i++)
            {
                int teamPos = teams[i].teamID;
                for (int j = 0; j < teams[i].trainers.Count; j++)
                {
                    for (int k = 0; k < teams[i].trainers[j].controlPos.Length; k++)
                    {
                        int battlePos = teams[i].trainers[j].controlPos[k];
                        battlePositions.Add(new BattlePosition(teamPos, battlePos));
                    }
                }
            }
            return battlePositions;
        }
        public Main.Pokemon.Pokemon GetPokemonAtPosition(BattlePosition position)
        {
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (pokemonOnField[i].teamPos == position.teamPos
                    && pokemonOnField[i].battlePos == position.battlePos)
                {
                    return pokemonOnField[i];
                }
            }
            return null;
        }
        public void SetPokemonPosition(Main.Pokemon.Pokemon pokemon, BattlePosition position)
        {
            pokemon.battlePos = position.battlePos;
            pokemon.teamPos = position.teamPos;
        }
        public BattlePosition GetPokemonPosition(Main.Pokemon.Pokemon pokemon)
        {
            return new BattlePosition(pokemon);
        }
        public Team GetPokemonOpposingTeam(Main.Pokemon.Pokemon pokemon)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamID != pokemon.teamPos)
                {
                    return teams[i];
                }
            }
            return null;
        }
        public bool ArePokemonAllies(Main.Pokemon.Pokemon pokemon1, Main.Pokemon.Pokemon pokemon2)
        {
            return pokemon1.teamPos == pokemon2.teamPos;
        }
        public bool ArePokemonEnemies(Main.Pokemon.Pokemon pokemon1, Main.Pokemon.Pokemon pokemon2)
        {
            return pokemon1.teamPos != pokemon2.teamPos;
        }
        public List<Main.Pokemon.Pokemon> GetAllyPokemon(Main.Pokemon.Pokemon pokemon)
        {
            List<Main.Pokemon.Pokemon> allyPokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (ArePokemonAllies(pokemon, pokemonOnField[i])
                    && !pokemon.IsTheSameAs(pokemonOnField[i]))
                {
                    allyPokemon.Add(pokemonOnField[i]);
                }
            }
            return allyPokemon;
        }
        public List<Main.Pokemon.Pokemon> GetOpposingPokemon(Main.Pokemon.Pokemon pokemon)
        {
            List<Main.Pokemon.Pokemon> opposingPokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (ArePokemonEnemies(pokemon, pokemonOnField[i])
                    && !pokemon.IsTheSameAs(pokemonOnField[i]))
                {
                    opposingPokemon.Add(pokemonOnField[i]);
                }
            }
            return opposingPokemon;
        }
        public Main.Pokemon.Pokemon GetRandomOpponent(Main.Pokemon.Pokemon pokemon)
        {
            List<Main.Pokemon.Pokemon> opposingPokemon = GetOpposingPokemon(pokemon);
            if (opposingPokemon.Count > 0)
            {
                return opposingPokemon[Random.Range(0, opposingPokemon.Count)];
            }
            return null;
        }
        public List<Main.Pokemon.Pokemon> GetPokemonFromTeam(Team team)
        {
            List<Main.Pokemon.Pokemon> allPokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < team.trainers.Count; i++)
            {
                allPokemon.AddRange(team.trainers[i].party);
            }
            return allPokemon;
        }
        public List<Main.Pokemon.Pokemon> GetPokemonFromAllTrainers()
        {
            List<Main.Pokemon.Pokemon> allPokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < teams.Count; i++)
            {
                allPokemon.AddRange(GetPokemonFromTeam(teams[i]));
            }
            return allPokemon;
        }
        public List<Main.Pokemon.Pokemon> GetPokemonFainted()
        {
            return GetPokemonFaintedFrom(pokemonOnField);
        }
        public List<Main.Pokemon.Pokemon> GetPokemonFaintedFrom(List<Main.Pokemon.Pokemon> pokemon)
        {
            List<Main.Pokemon.Pokemon> faintedPokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < pokemon.Count; i++)
            {
                if (IsPokemonFainted(pokemon[i]))
                {
                    faintedPokemon.Add(pokemon[i]);
                }
            }
            return faintedPokemon;
        }
        public List<Main.Pokemon.Pokemon> GetPokemonUnfainted()
        {
            return GetPokemonUnfaintedFrom(pokemonOnField);
        }
        public List<Main.Pokemon.Pokemon> GetPokemonUnfaintedFrom(List<Main.Pokemon.Pokemon> pokemon)
        {
            List<Main.Pokemon.Pokemon> alivePokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < pokemon.Count; i++)
            {
                if (!IsPokemonFainted(pokemon[i]))
                {
                    alivePokemon.Add(pokemon[i]);
                }
            }
            return alivePokemon;
        }
        public List<Main.Pokemon.Pokemon> GetPokemonOnFieldFrom(List<Main.Pokemon.Pokemon> pokemon)
        {
            List<Main.Pokemon.Pokemon> fieldPokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < pokemon.Count; i++)
            {
                if (IsPokemonOnField(pokemon[i]))
                {
                    fieldPokemon.Add(pokemon[i]);
                }
            }
            return fieldPokemon;
        }

        // Pokemon: Health / Stats Methods
        /// <summary>
        /// Returns the given pokemon's HP as a percentage of its maximum HP.
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public float GetPokemonHPAsPercentage(Main.Pokemon.Pokemon pokemon)
        {
            return pokemon.HPPercent;
        }
        public float GetPokemonHPPercentGivenHP(Main.Pokemon.Pokemon pokemon, int HPValue)
        {
            return (float)HPValue / pokemon.maxHP;
        }
        public void SetPokemonHP(Main.Pokemon.Pokemon pokemon, float hpPercent)
        {

        }
        public void FaintPokemon(Main.Pokemon.Pokemon pokemon)
        {
            pokemon.faintPos = pokemon.battlePos;
            if (pokemon.megaState != Main.Pokemon.Pokemon.MegaState.None)
            {
                pokemon.megaState = Main.Pokemon.Pokemon.MegaState.None;
                PBPRevertForm(pokemon);
            }
            if (pokemon.dynamaxState != Main.Pokemon.Pokemon.DynamaxState.None)
            {
                if (pokemon.dynamaxState == Main.Pokemon.Pokemon.DynamaxState.Gigantamax)
                {
                    PBPRevertForm(pokemon);
                }
                pokemon.dynamaxState = Main.Pokemon.Pokemon.DynamaxState.None;
            }
            RemovePokemonOnField(pokemon);
        }
        public bool IsPokemonFainted(Main.Pokemon.Pokemon pokemon)
        {
            return !pokemon.IsAbleToBattle();
        }
        public List<Main.Pokemon.Pokemon> GetFaintedPokemon()
        {
            List<Main.Pokemon.Pokemon> pokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (IsPokemonFainted(pokemonOnField[i]))
                {
                    pokemon.Add(pokemonOnField[i]);
                }
            }
            return pokemon;
        }
        public float GetPokemonHeight(Main.Pokemon.Pokemon pokemon)
        {
            float height = pokemon.data.height;
            return height;
        }
        public float GetPokemonWeight(Main.Pokemon.Pokemon pokemon)
        {
            // base weight
            float weight = pokemon.data.weight;

            // Light Metal / Heavy Metal
            List<PBS.Databases.Effects.Abilities.AbilityEffect> heavyMetal_ =
                PBPGetAbilityEffects(pokemon, AbilityEffectType.HeavyMetal);
            for (int i = 0; i < heavyMetal_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.HeavyMetal heavyMetal =
                    heavyMetal_[i] as PBS.Databases.Effects.Abilities.HeavyMetal;
                weight *= heavyMetal.weightMultiplier;
            }
            return weight;
        }
        public int GetPokemonATK(Main.Pokemon.Pokemon pokemon, bool applyStatStage = true, bool applyModifiers = true)
        {
            PokemonStats statType = pokemon.bProps.ATKMappedStat;
            float stat = GetPokemonStat(pokemon, statType, applyStatStage, applyModifiers);

            return Mathf.FloorToInt(stat);
        }
        public int GetPokemonDEF(Main.Pokemon.Pokemon pokemon, bool applyStatStage = true, bool applyModifiers = true)
        {
            PokemonStats statType = pokemon.bProps.DEFMappedStat;
            float stat = GetPokemonStat(pokemon, statType, applyStatStage, applyModifiers);

            return Mathf.FloorToInt(stat);
        }
        public int GetPokemonSPA(Main.Pokemon.Pokemon pokemon, bool applyStatStage = true, bool applyModifiers = true)
        {
            PokemonStats statType = pokemon.bProps.SPAMappedStat;
            float stat = GetPokemonStat(pokemon, statType, applyStatStage, applyModifiers);

            return Mathf.FloorToInt(stat);
        }
        public int GetPokemonSPD(Main.Pokemon.Pokemon pokemon, bool applyStatStage = true, bool applyModifiers = true)
        {
            PokemonStats statType = pokemon.bProps.SPDMappedStat;
            float stat = GetPokemonStat(pokemon, statType, applyStatStage, applyModifiers);

            return Mathf.FloorToInt(stat);
        }
        public int GetPokemonSPE(Main.Pokemon.Pokemon pokemon, bool applyStatStage = true, bool applyModifiers = true)
        {
            PokemonStats statType = pokemon.bProps.SPEMappedStat;
            float stat = GetPokemonStat(pokemon, statType, applyStatStage, applyModifiers);

            return Mathf.FloorToInt(stat);
        }
        public PokemonStats GetPokemonHighestStat(Main.Pokemon.Pokemon pokemon, bool applyStatStage = true, bool applyModifiers = false)
        {
            PokemonStats highestStat = PokemonStats.None;
            int highestValue = 0;

            int ATK = GetPokemonATK(pokemon: pokemon, applyStatStage: applyStatStage, applyModifiers: applyModifiers);
            int DEF = GetPokemonDEF(pokemon: pokemon, applyStatStage: applyStatStage, applyModifiers: applyModifiers);
            int SPA = GetPokemonSPA(pokemon: pokemon, applyStatStage: applyStatStage, applyModifiers: applyModifiers);
            int SPD = GetPokemonSPD(pokemon: pokemon, applyStatStage: applyStatStage, applyModifiers: applyModifiers);
            int SPE = GetPokemonSPE(pokemon: pokemon, applyStatStage: applyStatStage, applyModifiers: applyModifiers);

            if (ATK > highestValue)
            {
                highestStat = PokemonStats.Attack;
                highestValue = ATK;
            }
            if (DEF > highestValue)
            {
                highestStat = PokemonStats.Defense;
                highestValue = DEF;
            }
            if (SPA > highestValue)
            {
                highestStat = PokemonStats.SpecialAttack;
                highestValue = SPA;
            }
            if (SPD > highestValue)
            {
                highestStat = PokemonStats.SpecialDefense;
                highestValue = SPD;
            }
            if (SPE > highestValue)
            {
                highestStat = PokemonStats.Speed;
                highestValue = SPE;
            }

            return highestStat;
        }
        public float GetPokemonStat(
            Main.Pokemon.Pokemon pokemon,
            PokemonStats statType,
            bool applyStatStage = true,
            bool applyModifiers = true)
        {
            applyStatStage = applyModifiers ? applyStatStage : false;

            // Wonder Room
            if (BBPIsPokemonAffectedByBSC(pokemon, wonderRoom))
            {
                PBS.Databases.Effects.BattleStatuses.BattleSE wonderRoom_ = wonderRoom.data.GetEffectNew(BattleSEType.WonderRoom);
                if (wonderRoom_ != null)
                {
                    PBS.Databases.Effects.BattleStatuses.WonderRoom wonderRoomEffect = wonderRoom_ as PBS.Databases.Effects.BattleStatuses.WonderRoom;
                    statType = wonderRoomEffect.GetMappedStat(statType);
                }
            }

            float stat = statType == PokemonStats.Attack ? pokemon.ATK
                : statType == PokemonStats.Defense ? pokemon.DEF
                : statType == PokemonStats.SpecialAttack ? pokemon.SPA
                : statType == PokemonStats.SpecialDefense ? pokemon.SPD
                : statType == PokemonStats.Speed ? pokemon.SPE
                : 1f; // Accuracy and Evasion should be 1

            // stat stage
            if (applyStatStage)
            {
                stat *= GetPokemonStatModifier(pokemon, statType);
            }

            // modifiers
            if (applyModifiers)
            {
                List<StatusCondition> conditions = GetAllPokemonFilteredStatus(pokemon, PokemonSEType.StatScale);
                List<BattleCondition> bConditions = BBPGetSCs();
                List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(pokemon);

                // Abilities

                // Slow Start
                for (int i = 0; i < pbAbilities.Count; i++)
                {
                    Main.Pokemon.Ability ability = pbAbilities[i];
                    PBS.Databases.Effects.Abilities.AbilityEffect slowStart_ =
                        ability.data.GetEffectNew(AbilityEffectType.SlowStart);
                    if (slowStart_ != null)
                    {
                        PBS.Databases.Effects.Abilities.SlowStart slowStart =
                            slowStart_ as PBS.Databases.Effects.Abilities.SlowStart;
                        if (DoEffectFiltersPass(
                            filters: slowStart.filters,
                            targetPokemon: pokemon
                            ))
                        {
                            if (ability.turnsActive > slowStart.turnsActive)
                            {
                                stat *= slowStart.statScale.GetPokemonStatScale(statType);
                            }
                        }
                    }
                }

                // Compound Eyes
                List<PBS.Databases.Effects.Abilities.AbilityEffect> compoundEyes_ =
                    PBPGetAbilityEffects(pokemon, AbilityEffectType.CompoundEyes);
                for (int k = 0; k < compoundEyes_.Count; k++)
                {
                    PBS.Databases.Effects.Abilities.CompoundEyes compoundEyes =
                        compoundEyes_[k] as PBS.Databases.Effects.Abilities.CompoundEyes;

                    bool applyEffect = DoEffectFiltersPass(
                        filters: compoundEyes.filters,
                        targetPokemon: pokemon
                        );

                    // Defeatist
                    if (applyEffect && compoundEyes.defeatistThreshold > 0)
                    {
                        if (GetPokemonHPAsPercentage(pokemon) > compoundEyes.defeatistThreshold)
                        {
                            applyEffect = false;
                        }
                    }

                    // Apply stat scaling
                    if (applyEffect)
                    {
                        stat *= compoundEyes.statScale.GetPokemonStatScale(statType);
                    }

                }

                // Unburden
                if (pokemon.bProps.unburdenItem != null)
                {
                    List<PBS.Databases.Effects.Abilities.AbilityEffect> unburden_ =
                        PBPGetAbilityEffects(pokemon, AbilityEffectType.Unburden);
                    for (int i = 0; i < unburden_.Count; i++)
                    {
                        PBS.Databases.Effects.Abilities.Unburden unburden =
                            unburden_[i] as PBS.Databases.Effects.Abilities.Unburden;
                        if (DoEffectFiltersPass(
                            filters: unburden.filters,
                            targetPokemon: pokemon
                            ))
                        {
                            stat *= unburden.statScale.GetPokemonStatScale(statType);
                        }
                    }
                }

                // Victory Star
                List<Main.Pokemon.Pokemon> allyPokemon = GetAllyPokemon(pokemon);
                for (int i = 0; i < allyPokemon.Count; i++)
                {
                    List<PBS.Databases.Effects.Abilities.AbilityEffect> victoryStar_ =
                        PBPGetAbilityEffects(allyPokemon[i], AbilityEffectType.CompoundEyes);
                    for (int k = 0; k < victoryStar_.Count; k++)
                    {
                        PBS.Databases.Effects.Abilities.CompoundEyes victoryStar =
                            victoryStar_[k] as PBS.Databases.Effects.Abilities.CompoundEyes;
                        if (victoryStar.victoryStar)
                        {
                            bool applyEffect = DoEffectFiltersPass(
                                filters: victoryStar.filters,
                                userPokemon: pokemon,
                                targetPokemon: allyPokemon[i]
                                );

                            // Defeatist
                            if (applyEffect && victoryStar.defeatistThreshold > 0)
                            {
                                if (GetPokemonHPAsPercentage(allyPokemon[i]) > victoryStar.defeatistThreshold)
                                {
                                    applyEffect = false;
                                }
                            }

                            // Apply stat scaling
                            if (applyEffect)
                            {
                                stat *= victoryStar.statScale.GetPokemonStatScale(statType);
                            }
                        }
                    }
                }

                // Plus / Minus
                List<PBS.Databases.Effects.Abilities.AbilityEffect> minus_ =
                    PBPGetAbilityEffects(pokemon, AbilityEffectType.Minus);
                for (int i = 0; i < allyPokemon.Count; i++)
                {
                    for (int k = 0; k < minus_.Count; k++)
                    {
                        PBS.Databases.Effects.Abilities.Minus minus = minus_[k] as PBS.Databases.Effects.Abilities.Minus;
                        if (DoEffectFiltersPass(
                            filters: minus.filters,
                            userPokemon: pokemon
                            ))
                        {
                            bool applyMinus = true;

                            // ally ability list
                            if (minus.allyAbilities.Count > 0)
                            {
                                applyMinus = false;
                                for (int j = 0; j < minus.allyAbilities.Count && !applyMinus; j++)
                                {
                                    if (PBPHasAbility(minus.allyAbilities[j], pokemon))
                                    {
                                        applyMinus = true;
                                    }
                                }
                            }

                            if (applyMinus)
                            {
                                stat *= minus.statScale.GetPokemonStatScale(statType);
                            }
                        }
                    }
                }

                // Guts
                bool gutsOverwrite = false;
                List<PBS.Databases.Effects.Abilities.AbilityEffect> guts_ =
                    PBPGetAbilityEffects(pokemon, AbilityEffectType.Guts);
                for (int i = 0; i < guts_.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.Guts guts = guts_[i] as PBS.Databases.Effects.Abilities.Guts;
                    if (DoEffectFiltersPass(
                        filters: guts.filters,
                        targetPokemon: pokemon
                        ))
                    {
                        if (DoesEffectFilterPass(
                            effect_: guts.conditionCheck,
                            targetPokemon: pokemon
                            ))
                        {
                            gutsOverwrite = true;
                            stat *= guts.statScale.GetPokemonStatScale(statType);
                        }
                    }
                }

                // Swift Swim / Chlorophyll / Surge Surfer
                List<PBS.Databases.Effects.Abilities.AbilityEffect> swiftSwim_ =
                    PBPGetAbilityEffects(pokemon: pokemon, effectType: AbilityEffectType.SwiftSwim);
                for (int i = 0; i < swiftSwim_.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.SwiftSwim swiftSwim = swiftSwim_[i] as PBS.Databases.Effects.Abilities.SwiftSwim;
                    for (int k = 0; k < swiftSwim.conditions.Count; k++)
                    {
                        PBS.Databases.Effects.Abilities.SwiftSwim.SwiftSwimCondition ssCond = swiftSwim.conditions[k];
                        // Check existing battle conditions
                        bool isFound = false;
                        for (int j = 0; j < ssCond.conditions.Count && !isFound; j++)
                        {
                            BattleCondition bCond = BBPGetSC(ssCond.conditions[i], descendant: true);
                            if (bCond != null)
                            {
                                if (BBPIsPokemonAffectedByBS(pokemon, bCond.data))
                                {
                                    isFound = true;
                                }
                            }
                        }
                        if (isFound)
                        {
                            stat *= ssCond.statScale.GetPokemonStatScale(statType);
                        }
                    }
                }

                // Flower Gift
                for (int i = 0; i < allyPokemon.Count; i++)
                {
                    List<PBS.Databases.Effects.Abilities.AbilityEffect> flowerGift_ =
                        PBPGetAbilityEffects(pokemon: allyPokemon[i], effectType: AbilityEffectType.SwiftSwim);
                    for (int k = 0; k < flowerGift_.Count; k++)
                    {
                        PBS.Databases.Effects.Abilities.SwiftSwim flowerGift = flowerGift_[k] as PBS.Databases.Effects.Abilities.SwiftSwim;
                        for (int j = 0; j < flowerGift.conditions.Count; j++)
                        {
                            PBS.Databases.Effects.Abilities.SwiftSwim.SwiftSwimCondition ssCond = flowerGift.conditions[j];
                            // Check existing battle conditions
                            bool isFound = false;
                            for (int l = 0; l < ssCond.conditions.Count && !isFound; l++)
                            {
                                BattleCondition bCond = BBPGetSC(ssCond.conditions[l], descendant: true);
                                if (bCond != null)
                                {
                                    if (BBPIsPokemonAffectedByBS(allyPokemon[i], bCond.data))
                                    {
                                        isFound = true;
                                    }
                                }
                            }
                            if (isFound)
                            {
                                stat *= ssCond.statScale.GetPokemonStatScale(statType);
                            }
                        }
                    }
                }

                // pokemon status
                float pokemonModifiers = 1f;
                for (int i = 0; i < conditions.Count && !gutsOverwrite; i++)
                {
                    List<PokemonCEff> effects = conditions[i].data.GetEffects(PokemonSEType.StatScale);
                    for (int k = 0; k < effects.Count; k++)
                    {
                        List<PokemonStats> scaledStats = PBS.Databases.GameText.GetStatsFromList(effects[k].stringParams);
                        if (scaledStats.Contains(statType))
                        {
                            pokemonModifiers *= effects[k].GetFloat(0);
                        }
                    }
                }
                stat *= pokemonModifiers;

                // battle status
                float battleModifiers = 1f;

                for (int i = 0; i < bConditions.Count; i++)
                {
                    if (BBPIsPokemonAffectedByBSC(pokemon, bConditions[i]))
                    {
                        List<PBS.Databases.Effects.BattleStatuses.BattleSE> statScales_ =
                            bConditions[i].data.GetEffectsNew(BattleSEType.StatScale);
                        for (int k = 0; k < statScales_.Count; k++)
                        {
                            PBS.Databases.Effects.BattleStatuses.StatScale statScale =
                                statScales_[k] as PBS.Databases.Effects.BattleStatuses.StatScale;
                            if (DoesBattleEFiltersPass(
                                effect: statScale,
                                targetPokemon: pokemon
                                ))
                            {
                                battleModifiers *= statScale.GetStatMod(statType);
                            }
                        }
                    }
                }
                stat *= battleModifiers;

                // Items
                float itemModifiers = 1f;
                List<PBS.Databases.Effects.Items.ItemEffect> choiceBandStats_ =
                    PBPGetItemEffects(pokemon: pokemon, effectType: ItemEffectType.ChoiceBandStats);
                for (int k = 0; k < choiceBandStats_.Count; k++)
                {
                    PBS.Databases.Effects.Items.ChoiceBandStats choiceBandStats =
                        choiceBandStats_[k] as PBS.Databases.Effects.Items.ChoiceBandStats;

                    bool applyEffect = DoEffectFiltersPass(
                        filters: choiceBandStats.filters,
                        targetPokemon: pokemon
                        );

                    // Defeatist
                    if (applyEffect && choiceBandStats.defeatistThreshold > 0)
                    {
                        if (GetPokemonHPAsPercentage(pokemon) > choiceBandStats.defeatistThreshold)
                        {
                            applyEffect = false;
                        }
                    }

                    // Apply stat scaling
                    if (applyEffect)
                    {
                        stat *= choiceBandStats.statScale.GetPokemonStatScale(statType);
                    }

                }

                stat *= itemModifiers;
            }

            return stat;
        }
        public int GetPokemonStatStage(Main.Pokemon.Pokemon pokemon, PokemonStats statType)
        {
            return statType == PokemonStats.Attack ? pokemon.bProps.ATKStage
                : statType == PokemonStats.Defense ? pokemon.bProps.DEFStage
                : statType == PokemonStats.SpecialAttack ? pokemon.bProps.SPAStage
                : statType == PokemonStats.SpecialDefense ? pokemon.bProps.SPDStage
                : statType == PokemonStats.Speed ? pokemon.bProps.SPEStage
                : statType == PokemonStats.Accuracy ? pokemon.bProps.ACCStage
                : statType == PokemonStats.Evasion ? pokemon.bProps.EVAStage
                : 0;
        }
        public void SetPokemonStatStage(Main.Pokemon.Pokemon pokemon, PokemonStats statType, int stage)
        {
            if (statType == PokemonStats.Attack) pokemon.bProps.ATKStage = stage;
            if (statType == PokemonStats.Defense) pokemon.bProps.DEFStage = stage;
            if (statType == PokemonStats.SpecialAttack) pokemon.bProps.SPAStage = stage;
            if (statType == PokemonStats.SpecialDefense) pokemon.bProps.SPDStage = stage;
            if (statType == PokemonStats.Speed) pokemon.bProps.SPEStage = stage;
            if (statType == PokemonStats.Accuracy) pokemon.bProps.ACCStage = stage;
            if (statType == PokemonStats.Evasion) pokemon.bProps.EVAStage = stage;
        }
        public float GetPokemonStatModifier(Main.Pokemon.Pokemon pokemon, PokemonStats statType)
        {
            int statStage = GetPokemonStatStage(pokemon, statType);
            int absStatStage = Mathf.Abs(statStage);

            float numerator = 1f;
            float denominator = 1f;
            if (statType == PokemonStats.Attack
                || statType == PokemonStats.Defense
                || statType == PokemonStats.SpecialAttack
                || statType == PokemonStats.SpecialDefense
                || statType == PokemonStats.Speed)
            {
                numerator = 2f + (statStage > 0 ? absStatStage : 0);
                denominator = 2f + (statStage < 0 ? absStatStage : 0);
            }
            else if (statType == PokemonStats.Accuracy
                || statType == PokemonStats.Evasion)
            {
                numerator = 3f + (statStage > 0 ? absStatStage : 0);
                denominator = 3f + (statStage < 0 ? absStatStage : 0);
            }

            float modifier = numerator / denominator;
            return modifier;
        }
        public int AddPokemonHP(Main.Pokemon.Pokemon pokemon, int HPGain)
        {
            int realHPGain;
            if (pokemon.currentHP + HPGain >= pokemon.maxHP)
            {
                realHPGain = pokemon.maxHP - pokemon.currentHP;
                pokemon.currentHP = pokemon.maxHP;
            }
            else
            {
                realHPGain = HPGain;
                pokemon.currentHP += HPGain;
            }
            return realHPGain;
        }
        public int SubtractPokemonHP(Main.Pokemon.Pokemon pokemon, int HPLost)
        {
            int realHPLost;
            if (pokemon.currentHP <= HPLost)
            {
                realHPLost = pokemon.currentHP;
                pokemon.currentHP = 0;
            }
            else
            {
                realHPLost = HPLost;
                pokemon.currentHP -= HPLost;
            }
            return realHPLost;
        }
        public void ReplenishPokemonPP(Main.Pokemon.Pokemon pokemon)
        {
            List<Moveslot> moveslots = GetPokemonBattleMoveslots(pokemon);
            for (int i = 0; i < moveslots.Count; i++)
            {
                moveslots[i].PP = moveslots[i].maxPP;
            }
        }
        public int SubtractSubstituteHP(Main.Pokemon.Pokemon pokemon, int HPLost)
        {
            int realHPLost;
            if (pokemon.bProps.substituteHP <= HPLost)
            {
                realHPLost = pokemon.bProps.substituteHP;
                pokemon.bProps.substituteHP = 0;
            }
            else
            {
                realHPLost = HPLost;
                pokemon.bProps.substituteHP -= HPLost;
            }
            return realHPLost;
        }
        public int GetPokemonHPByPercent(Main.Pokemon.Pokemon pokemon, float HPPercent, bool roundDown = true, bool roundUp = false)
        {
            // restrict to positive values
            HPPercent = Mathf.Max(0, HPPercent);
            return roundDown ? Mathf.FloorToInt(pokemon.maxHP * HPPercent)
                : roundUp ? Mathf.CeilToInt(pokemon.maxHP * HPPercent)
                : Mathf.RoundToInt(pokemon.maxHP * HPPercent);
        }

        public BattleOrder GetPokemonOrder(Main.Pokemon.Pokemon pokemon1, Main.Pokemon.Pokemon pokemon2)
        {
            int speed1 = GetPokemonSPE(pokemon1);
            int speed2 = GetPokemonSPE(pokemon2);
            BattleOrder battleOrder = BattleOrder.SpeedTie;

            bool isStall1 = false;
            bool isStall2 = false;
            List<PBS.Databases.Effects.Abilities.AbilityEffect> stall1_ = PBPGetAbilityEffects(pokemon1, AbilityEffectType.Stall);
            List<PBS.Databases.Effects.Abilities.AbilityEffect> stall2_ = PBPGetAbilityEffects(pokemon2, AbilityEffectType.Stall);
            for (int i = 0; i < stall1_.Count && !isStall1; i++)
            {
                PBS.Databases.Effects.Abilities.Stall stall = stall1_[i] as PBS.Databases.Effects.Abilities.Stall;
                if (DoEffectFiltersPass(
                    filters: stall.filters,
                    userPokemon: pokemon1,
                    targetPokemon: pokemon1
                    ))
                {
                    isStall1 = true;
                }
            }
            for (int i = 0; i < stall2_.Count && !isStall2; i++)
            {
                PBS.Databases.Effects.Abilities.Stall stall = stall2_[i] as PBS.Databases.Effects.Abilities.Stall;
                if (DoEffectFiltersPass(
                    filters: stall.filters,
                    userPokemon: pokemon2,
                    targetPokemon: pokemon2
                    ))
                {
                    isStall2 = true;
                }
            }

            bool bothSameBracket = isStall1 == isStall2;

            if (bothSameBracket)
            {
                if (trickRoom.data.GetEffectNew(BattleSEType.TrickRoom) != null)
                {
                    PBS.Databases.Effects.BattleStatuses.BattleSE trickRoom_ = trickRoom.data.GetEffectNew(BattleSEType.TrickRoom);
                    PBS.Databases.Effects.BattleStatuses.TrickRoom trickRoomEffect = trickRoom_ as PBS.Databases.Effects.BattleStatuses.TrickRoom;
                    PokemonStats statToUse = trickRoomEffect.speedStat;

                    bool pokemon1Affected = false;
                    bool pokemon2Affected = false;
                    if (BBPIsPokemonAffectedByBSC(pokemon1, trickRoom))
                    {
                        pokemon1Affected = true;
                        speed1 = statToUse == PokemonStats.Attack ? GetPokemonATK(pokemon1)
                            : statToUse == PokemonStats.Defense ? GetPokemonDEF(pokemon1)
                            : statToUse == PokemonStats.SpecialAttack ? GetPokemonSPA(pokemon1)
                            : statToUse == PokemonStats.SpecialDefense ? GetPokemonSPD(pokemon1)
                            : statToUse == PokemonStats.Speed ? GetPokemonSPE(pokemon1)
                            : GetPokemonSPE(pokemon1);
                    }
                    if (BBPIsPokemonAffectedByBSC(pokemon2, trickRoom))
                    {
                        pokemon2Affected = true;
                        speed2 = statToUse == PokemonStats.Attack ? GetPokemonATK(pokemon2)
                            : statToUse == PokemonStats.Defense ? GetPokemonDEF(pokemon2)
                            : statToUse == PokemonStats.SpecialAttack ? GetPokemonSPA(pokemon2)
                            : statToUse == PokemonStats.SpecialDefense ? GetPokemonSPD(pokemon2)
                            : statToUse == PokemonStats.Speed ? GetPokemonSPE(pokemon2)
                            : GetPokemonSPE(pokemon2);
                    }

                    battleOrder = speed1 > speed2 ? BattleOrder.First
                        : speed2 > speed1 ? BattleOrder.Last
                        : BattleOrder.SpeedTie;

                    if (trickRoomEffect.reverse && pokemon1Affected && pokemon2Affected)
                    {
                        battleOrder = battleOrder == BattleOrder.First ? BattleOrder.Last
                            : battleOrder == BattleOrder.Last ? BattleOrder.First
                            : battleOrder;
                    }
                }
                else
                {
                    battleOrder = speed1 > speed2 ? BattleOrder.First
                        : speed2 > speed1 ? BattleOrder.Last
                        : BattleOrder.SpeedTie;
                }
            }
            else
            {
                if (isStall1)
                {
                    return BattleOrder.Last;
                }
                else if (isStall2)
                {
                    return BattleOrder.First;
                }
            }

            return battleOrder;
        }
        public List<Main.Pokemon.Pokemon> GetPokemonBySpeed(List<Main.Pokemon.Pokemon> originalPokemon)
        {
            List<Main.Pokemon.Pokemon> orderedPokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < originalPokemon.Count; i++)
            {
                Main.Pokemon.Pokemon unsortedPokemon = originalPokemon[i];
                // if no sorted commands, make it the first sorted element
                if (orderedPokemon.Count == 0)
                {
                    orderedPokemon.Add(unsortedPokemon);
                }
                // compare command to the already sorted commands
                else
                {
                    bool isCommandInserted = false;
                    for (int j = 0; j < orderedPokemon.Count && !isCommandInserted; j++)
                    {
                        Main.Pokemon.Pokemon sortedPokemon = orderedPokemon[j];

                        // Get the order of the commands, of the unsorted in relation to the sorted
                        BattleOrder commandOrder = GetPokemonOrder(unsortedPokemon, sortedPokemon);

                        // insert before if it goes first
                        if (commandOrder == BattleOrder.First)
                        {
                            orderedPokemon.Insert(j, unsortedPokemon);
                            isCommandInserted = true;
                            break;
                        }
                        // if it goes after, we continue
                        else if (commandOrder == BattleOrder.Last)
                        {
                            // continue
                        }
                        // if it's a speed tie, flip a coin to see who goes first
                        else if (commandOrder == BattleOrder.SpeedTie)
                        {
                            if (Random.value < 0.5f)
                            {
                                orderedPokemon.Insert(j, unsortedPokemon);
                                isCommandInserted = true;
                                break;
                            }
                        }
                    }
                    // if we didn't get to insert the command, it was lower priority than all the sorted commands
                    // we add it to the end of the list
                    if (!isCommandInserted)
                    {
                        orderedPokemon.Add(unsortedPokemon);
                    }
                }
            }
            return orderedPokemon;
        }
        public void ReorderPokemonOnField()
        {
            pokemonOnField = GetPokemonBySpeed(pokemonOnField);
        }

        // Pokemon: Stat Stage Methods
        public void SetStatStage(Main.Pokemon.Pokemon pokemon, PokemonStats stat, int stage)
        {
            switch (stat)
            {
                case PokemonStats.Attack:
                    if (pokemon.bProps.ATKStage < stage) pokemon.bProps.ATKRaised = true;
                    if (pokemon.bProps.ATKStage > stage) pokemon.bProps.ATKLowered = true;

                    pokemon.bProps.ATKStage = stage;
                    break;

                case PokemonStats.Defense:
                    if (pokemon.bProps.DEFStage < stage) pokemon.bProps.DEFRaised = true;
                    if (pokemon.bProps.DEFStage > stage) pokemon.bProps.DEFLowered = true;

                    pokemon.bProps.DEFStage = stage;
                    break;

                case PokemonStats.SpecialAttack:
                    if (pokemon.bProps.SPAStage < stage) pokemon.bProps.SPARaised = true;
                    if (pokemon.bProps.SPAStage > stage) pokemon.bProps.SPALowered = true;

                    pokemon.bProps.SPAStage = stage;
                    break;

                case PokemonStats.SpecialDefense:
                    if (pokemon.bProps.SPDStage < stage) pokemon.bProps.SPDRaised = true;
                    if (pokemon.bProps.SPDStage > stage) pokemon.bProps.SPDLowered = true;

                    pokemon.bProps.SPDStage = stage;
                    break;

                case PokemonStats.Speed:
                    if (pokemon.bProps.SPEStage < stage) pokemon.bProps.SPERaised = true;
                    if (pokemon.bProps.SPEStage > stage) pokemon.bProps.SPELowered = true;

                    pokemon.bProps.SPEStage = stage;
                    break;

                case PokemonStats.Accuracy:
                    if (pokemon.bProps.ACCStage < stage) pokemon.bProps.ACCRaised = true;
                    if (pokemon.bProps.ACCStage > stage) pokemon.bProps.ACCLowered = true;

                    pokemon.bProps.ACCStage = stage;
                    break;

                case PokemonStats.Evasion:
                    if (pokemon.bProps.EVAStage < stage) pokemon.bProps.EVARaised = true;
                    if (pokemon.bProps.EVAStage > stage) pokemon.bProps.EVALowered = true;

                    pokemon.bProps.EVAStage = stage;
                    break;
            }
        }
        public int GetTrueStatMod(
            List<PokemonStats> statsToModify,
            int modValue,
            Main.Pokemon.Pokemon targetPokemon,
            Main.Pokemon.Pokemon userPokemon = null,
            bool maximize = false,
            bool minimize = false)
        {
            Data.Ability targetAbilityData = PBPGetAbilityData(targetPokemon);

            // Contrary
            AbilityEffect contraryEffect = targetAbilityData.GetEffect(AbilityEffectType.Contrary);
            if (contraryEffect != null)
            {
                modValue = -modValue;
                if (maximize)
                {
                    maximize = false;
                    minimize = true;
                }
                else if (minimize)
                {
                    maximize = true;
                    minimize = false;
                }
            }

            modValue = maximize ? GameSettings.GetMaxStatBoost() : modValue;
            modValue = minimize ? GameSettings.GetMinStatBoost() : modValue;

            // Simple
            AbilityEffect simpleEffect = targetAbilityData.GetEffect(AbilityEffectType.Simple);
            if (simpleEffect != null
                && !maximize
                && !minimize)
            {
                int simpleFactor = Mathf.FloorToInt(simpleEffect.GetFloat(0));
                modValue *= simpleFactor;
            }
            return modValue;
        }
        public bool CanPokemonStatModsExecute(
            List<PokemonStats> statsToModify,
            int modValue,
            Main.Pokemon.Pokemon targetPokemon,
            Main.Pokemon.Pokemon userPokemon = null,
            bool maximize = false,
            bool minimize = false
            )
        {
            modValue = GetTrueStatMod(
                statsToModify,
                modValue,
                targetPokemon: targetPokemon,
                userPokemon: userPokemon,
                maximize: maximize,
                minimize: minimize
                );

            // check if each stat can be modified
            for (int i = 0; i < statsToModify.Count; i++)
            {
                PokemonStats currentStat = statsToModify[i];
                int currentMod = GetPokemonStatStage(targetPokemon, currentStat);

                int afterMod = currentMod + modValue;
                afterMod = afterMod >= GameSettings.btlStatStageMax ? GameSettings.btlStatStageMax
                    : afterMod <= GameSettings.btlStatStageMin ? GameSettings.btlStatStageMin
                    : afterMod;
                int realMod = afterMod - currentMod;

                // if one stat is modifiable, we can modify stats
                if (realMod != 0)
                {
                    return true;
                }
            }

            // no stat was found to be modifiable
            return false;
        }
        public bool IsStatModsTooHigh(
            List<PokemonStats> statsToModify,
            int modValue,
            Main.Pokemon.Pokemon targetPokemon,
            Main.Pokemon.Pokemon userPokemon = null,
            bool maximize = false,
            bool minimize = false)
        {
            modValue = GetTrueStatMod(
                statsToModify,
                modValue,
                targetPokemon: targetPokemon,
                userPokemon: userPokemon,
                maximize: maximize,
                minimize: minimize
                );
            return modValue > 0;
        }

        // Pokemon: Command Methods
        public List<Moveslot> GetPokemonBattleMoveslots(Main.Pokemon.Pokemon pokemon)
        {
            List<Moveslot> moveslots = new List<Moveslot>();

            // Transform moves
            Moveslot[] moveslotSet;
            if (pokemon.bProps.tProps != null)
            {
                moveslotSet = pokemon.bProps.tProps.moveslots;
            }
            // Regular moves
            else
            {
                moveslotSet = pokemon.moveslots;
            }

            for (int i = 0; i < moveslotSet.Length; i++)
            {
                Moveslot curSlot = moveslotSet[i];
                if (curSlot != null)
                {
                    bool moveAdded = false;

                    // Mimic
                    if (!moveAdded &&
                        pokemon.bProps.mimicBaseMove == curSlot.moveID)
                    {
                        moveAdded = true;
                        moveslots.Add(pokemon.bProps.mimicMoveslot);
                    }

                    // Sketch
                    if (!moveAdded &&
                        pokemon.bProps.sketchBaseMove == curSlot.moveID)
                    {
                        moveAdded = true;
                        moveslots.Add(pokemon.bProps.sketchMoveslot);
                    }

                    // Base move
                    if (!moveAdded)
                    {
                        moveslots.Add(curSlot);
                    }
                }
            }

            // TODO: Modified moveslots
            // Ex. Copycat

            return moveslots;
        }
        public bool DoesPokemonHaveMove(Main.Pokemon.Pokemon pokemon, string moveID)
        {
            if (pokemon.HasMove(moveID))
            {
                return true;
            }
            List<Moveslot> moveslots = GetPokemonBattleMoveslots(pokemon);
            for (int i = 0; i < moveslots.Count; i++)
            {
                if (moveslots[i].moveID == moveID)
                {
                    return true;
                }
            }
            return false;
        }
        public int GetPokemonMovePP(Main.Pokemon.Pokemon pokemon, string moveID)
        {
            List<Moveslot> moveslots = GetPokemonBattleMoveslots(pokemon);
            for (int i = 0; i < moveslots.Count; i++)
            {
                if (moveslots[i].moveID == moveID)
                {
                    return moveslots[i].PP;
                }
            }
            return -1;
        }
        public bool CanPokemonStillExecuteCommand(Main.Pokemon.Pokemon pokemon, BattleCommand command)
        {

            // Fainted
            if (IsPokemonFainted(pokemon) && !command.isFutureSightMove)
            {
                if (command.commandType == BattleCommandType.Fight
                    || command.commandType == BattleCommandType.Party
                    || command.commandType == BattleCommandType.PartyReplace
                    || command.commandType == BattleCommandType.Run)
                {
                    return false;
                }
            }

            // Not In Battle.Core.Model
            if (!IsPokemonOnField(pokemon) && !command.isFutureSightMove)
            {
                if (command.commandType == BattleCommandType.Fight
                    || command.commandType == BattleCommandType.Party
                    || command.commandType == BattleCommandType.PartyReplace
                    || command.commandType == BattleCommandType.Run)
                {
                    return false;
                }
            }

            // Sky Drop
            if (!string.IsNullOrEmpty(pokemon.bProps.skyDropMove) && !command.isFutureSightMove)
            {
                if (command.commandType == BattleCommandType.Fight
                    || command.commandType == BattleCommandType.Party
                    || command.commandType == BattleCommandType.PartyReplace
                    || command.commandType == BattleCommandType.Run)
                {
                    return false;
                }
            }

            return true;
        }
        public bool DoesMoveslotHaveEnoughPP(Main.Pokemon.Pokemon pokemon, Moveslot moveslot)
        {
            // TODO: real checks
            return moveslot.PP > 0;
        }
        public bool CanPokemonUseMove(Main.Pokemon.Pokemon pokemon, string moveID)
        {
            Move moveData = Moves.instance.GetMoveData(moveID);

            // TODO: real checks

            // imprison
            Main.Pokemon.Pokemon imprison = PBPGetImprison(pokemon, moveData);
            if (imprison != null)
            {
                return false;
            }

            if (IsPokemonMoveLimited(pokemon, moveData))
            {
                return false;
            }

            return true;
        }
        public bool IsPokemonMoveLimited(
            Main.Pokemon.Pokemon pokemon,
            Move moveData
            )
        {
            for (int i = 0; i < pokemon.bProps.moveLimiters.Count; i++)
            {
                if (IsPokemonMoveLimited(
                    pokemon,
                    moveData,
                    pokemon.bProps.moveLimiters[i]
                    ))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsPokemonMoveLimited(
            Main.Pokemon.Pokemon pokemon,
            Move moveData,
            Main.Pokemon.BattleProperties.MoveLimiter moveLimiter)
        {
            // Disable
            if (moveLimiter.effect is PBS.Databases.Effects.PokemonStatuses.Disable)
            {
                if (moveLimiter.affectedMoves.Contains(moveData.ID))
                {
                    return true;
                }
            }
            // Encore
            else if (moveLimiter.effect is PBS.Databases.Effects.PokemonStatuses.Encore)
            {
                if (!moveLimiter.affectedMoves.Contains(moveData.ID))
                {
                    return true;
                }
            }
            // Heal Block
            else if (moveLimiter.effect is PBS.Databases.Effects.PokemonStatuses.HealBlock)
            {
                if (IsHealingMove(moveData))
                {
                    return true;
                }
            }
            // Taunt
            else if (moveLimiter.effect is PBS.Databases.Effects.PokemonStatuses.Taunt)
            {
                PBS.Databases.Effects.PokemonStatuses.Taunt taunt = moveLimiter.effect as PBS.Databases.Effects.PokemonStatuses.Taunt;
                if (taunt.category == moveData.category)
                {
                    return true;
                }
            }
            // Torment
            else if (moveLimiter.effect is PBS.Databases.Effects.PokemonStatuses.Torment)
            {
                if (moveLimiter.affectedMoves.Contains(moveData.ID))
                {
                    return true;
                }
            }

            return false;
        }
        public List<Moveslot> GetPokemonUseableMoves(Main.Pokemon.Pokemon pokemon)
        {
            List<Moveslot> moveslots = new List<Moveslot>();
            List<Moveslot> moveset = GetPokemonBattleMoveslots(pokemon);
            for (int i = 0; i < moveset.Count; i++)
            {
                if (DoesMoveslotHaveEnoughPP(pokemon, pokemon.moveslots[i])
                    && CanPokemonUseMove(pokemon, pokemon.moveslots[i].moveID))
                {
                    moveslots.Add(pokemon.moveslots[i]);
                }
            }
            return moveslots;
        }
        public BattleCommand GetStruggleCommand(Main.Pokemon.Pokemon pokemon, bool isPlayer = false)
        {
            Moveslot struggleMove = new Moveslot("struggle");
            BattleCommand struggleCommand = BattleCommand.CreateMoveCommand(
                pokemon,
                struggleMove.moveID,
                GetMoveAutoTargets(pokemon, Moves.instance.GetMoveData(struggleMove.moveID)));
            return struggleCommand;
        }
        public bool IsPokemonPreventedFromHealing(Main.Pokemon.Pokemon pokemon)
        {
            // Heal Block
            for (int i = 0; i < pokemon.bProps.moveLimiters.Count; i++)
            {
                if (pokemon.bProps.moveLimiters[i].effect is PBS.Databases.Effects.PokemonStatuses.HealBlock)
                {
                    return true;
                }
            }

            return false;
        }

        // Pokemon: Misc Methods
        public void ResetBattleOnlyProperties(Main.Pokemon.Pokemon pokemon)
        {
            // stat stages
            pokemon.isInBattleMode = false;
            ResetStatChanges(pokemon);
            ResetCommands(pokemon);

            // reset status counter
            if (pokemon.nonVolatileStatus.turnsActive > 0)
            {
                pokemon.nonVolatileStatus.turnsActive = 0;
            }
            pokemon.bProps.Reset(pokemon);
        }
        public void ResetStatChanges(Main.Pokemon.Pokemon pokemon)
        {
            pokemon.bProps.ATKStage = 0;
            pokemon.bProps.DEFStage = 0;
            pokemon.bProps.SPAStage = 0;
            pokemon.bProps.SPDStage = 0;
            pokemon.bProps.SPEStage = 0;
            pokemon.bProps.ACCStage = 0;
            pokemon.bProps.EVAStage = 0;
        }
        public void ResetCommands(Main.Pokemon.Pokemon pokemon)
        {
            pokemon.UnsetNextCommand();
        }
        public void DisruptPokemon(Main.Pokemon.Pokemon pokemon, Move moveData)
        {
            // Bide
            if (moveData.GetEffect(MoveEffectType.Bide) != null)
            {
                pokemon.DisruptBide();
            }
        }

        // ---POKEMON BATTLE PROPERTIES---

        // Form Changes
        public void PBPDynamax(Main.Pokemon.Pokemon pokemon)
        {
            float preHPPercent = GetPokemonHPAsPercentage(pokemon);
            int preMaxHP = pokemon.maxHP;

            if (!string.IsNullOrEmpty(pokemon.dynamaxProps.GMaxForm))
            {
                pokemon.dynamaxState = Main.Pokemon.Pokemon.DynamaxState.Gigantamax;
            }
            else
            {
                pokemon.dynamaxState = Main.Pokemon.Pokemon.DynamaxState.Dynamax;
            }
            pokemon.dynamaxProps.turnsLeft = GameSettings.pkmnDynamaxTurns;

            int postMaxHP = pokemon.maxHP;
            if (postMaxHP != preMaxHP)
            {
                if (pokemon.currentHP > 0)
                {
                    int newCurHP = GetPokemonHPByPercent(pokemon: pokemon, HPPercent: preHPPercent);
                    pokemon.currentHP = Mathf.Max(1, newCurHP);
                }
            }
        }
        public void PBPUndynamax(Main.Pokemon.Pokemon pokemon)
        {
            float preHPPercent = GetPokemonHPAsPercentage(pokemon);
            int preMaxHP = pokemon.maxHP;

            if (pokemon.dynamaxState == Main.Pokemon.Pokemon.DynamaxState.Gigantamax)
            {
                PBPRevertForm(pokemon);
            }
            pokemon.dynamaxState = Main.Pokemon.Pokemon.DynamaxState.None;

            int postMaxHP = pokemon.maxHP;
            if (postMaxHP != preMaxHP)
            {
                if (pokemon.currentHP > 0)
                {
                    int newCurHP = GetPokemonHPByPercent(pokemon: pokemon, HPPercent: preHPPercent);
                    pokemon.currentHP = Mathf.Max(1, newCurHP);
                }
            }
        }
        public void PBPChangeForm(Main.Pokemon.Pokemon pokemon, string newPokemonID)
        {
            float preHPPercent = GetPokemonHPAsPercentage(pokemon);
            int preMaxHP = pokemon.maxHP;

            pokemon.pokemonID = newPokemonID;
            pokemon.bProps.ResetOverrides(pokemon);

            int postMaxHP = pokemon.maxHP;
            if (postMaxHP != preMaxHP)
            {
                if (pokemon.currentHP > 0)
                {
                    int newCurHP = GetPokemonHPByPercent(pokemon: pokemon, HPPercent: preHPPercent);
                    pokemon.currentHP = Mathf.Max(1, newCurHP);
                }
            }
        }
        public void PBPRevertForm(Main.Pokemon.Pokemon pokemon)
        {
            float preHPPercent = GetPokemonHPAsPercentage(pokemon);
            int preMaxHP = pokemon.maxHP;

            pokemon.pokemonID = pokemon.basePokemonID;
            pokemon.bProps.ResetOverrides(pokemon);

            int postMaxHP = pokemon.maxHP;
            if (postMaxHP != preMaxHP)
            {
                if (pokemon.currentHP > 0)
                {
                    int newCurHP = GetPokemonHPByPercent(pokemon: pokemon, HPPercent: preHPPercent);
                    pokemon.currentHP = Mathf.Max(1, newCurHP);
                }
            }
        }
        public void PBPTransformIntoPokemon(Main.Pokemon.Pokemon transformPokemon, Main.Pokemon.Pokemon targetPokemon)
        {
            // Reset overridden values
            transformPokemon.bProps.ResetOverrides(transformPokemon);

            transformPokemon.bProps.tProps = new TransformProperties(targetPokemon);
            transformPokemon.bProps.ATKStage = targetPokemon.bProps.ATKStage;
            transformPokemon.bProps.DEFStage = targetPokemon.bProps.DEFStage;
            transformPokemon.bProps.SPAStage = targetPokemon.bProps.SPAStage;
            transformPokemon.bProps.SPDStage = targetPokemon.bProps.SPDStage;
            transformPokemon.bProps.SPEStage = targetPokemon.bProps.SPEStage;

            List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(targetPokemon, ignoreSuppression: true);
            transformPokemon.bProps.abilities = new List<Main.Pokemon.Ability>();
            for (int i = 0; i < pbAbilities.Count; i++)
            {
                transformPokemon.bProps.abilities.Add(pbAbilities[i].TransformClone());
            }

            transformPokemon.bProps.types = new List<string>(targetPokemon.bProps.types);
        }
        public bool PBPIsPokemonOfForm(
            Main.Pokemon.Pokemon pokemon,
            string formPokemon,
            bool allowDerivatives = false,
            bool allowAncestors = false,
            bool allowTransform = true)
        {
            string userID = pokemon.pokemonID;
            PBS.Data.Pokemon pokemonData = Databases.Pokemon.instance.GetPokemonData(formPokemon);

            // form inequality
            if (userID != pokemonData.ID)
            {
                // If the user is a derivative of the target form
                if (pokemon.data.IsABaseID(pokemonData.ID))
                {
                    if (allowDerivatives)
                    {
                        return true;
                    }
                }
                // The user is an ancestor of the target form
                else if (pokemonData.IsABaseID(userID))
                {
                    if (allowAncestors)
                    {
                        return true;
                    }
                }
                else if (allowTransform && pokemon.bProps.tProps != null)
                {
                    PBS.Data.Pokemon trnsData = Databases.Pokemon.instance.GetPokemonData(pokemon.bProps.tProps.pokemonID);
                    if (trnsData.ID != pokemonData.ID)
                    {
                        // If the user is a derivative of the target form
                        if (trnsData.IsABaseID(pokemonData.ID))
                        {
                            if (allowDerivatives)
                            {
                                return true;
                            }
                        }
                        // The user is an ancestor of the target form
                        else if (pokemonData.IsABaseID(trnsData.ID))
                        {
                            if (allowAncestors)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            // the forms are the same
            else
            {
                return true;
            }
            return false;
        }

        // Abilities
        public List<Main.Pokemon.Ability> PBPGetAbilities(
            Main.Pokemon.Pokemon pokemon,
            bool bypassAbility = false,
            bool ignoreSuppression = false,
            bool skipNeutralizingGasCheck = false)
        {
            if (pokemon == null)
            {
                Debug.LogWarning("Checking for a null Pokemon");
                return new List<Main.Pokemon.Ability>();
            }

            List<Main.Pokemon.Ability> abilities = new List<Main.Pokemon.Ability>();
            for (int i = 0; i < pokemon.bProps.abilities.Count; i++)
            {
                Main.Pokemon.Ability curAbility = pokemon.bProps.abilities[i];
                // suppressed (ex. Gastro Acid, Core Enforcer)
                if (!curAbility.isSuppressed || ignoreSuppression)
                {
                    // Mold Breaker
                    if (!bypassAbility || curAbility.data.HasTag(AbilityTag.BypassMoldBreaker))
                    {
                        // Neutralizing Gas check
                        if (skipNeutralizingGasCheck)
                        {
                            abilities.Add(curAbility);
                        }
                        else
                        {
                            // can be neutralized
                            bool isNeutralized =
                                PBPIsNeutralizingGasInEffect() && !curAbility.data.HasTag(AbilityTag.CannotNeutralize);
                            if (!isNeutralized)
                            {
                                abilities.Add(curAbility);
                            }
                        }
                    }
                }
            }
            return abilities;
        }
        public Main.Pokemon.Ability PBPGetAbility(
            Main.Pokemon.Pokemon pokemon,
            bool bypassAbility = false,
            bool ignoreSuppression = false)
        {
            List<Main.Pokemon.Ability> abilities = PBPGetAbilities(
                pokemon: pokemon,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression
                );
            if (abilities.Count > 0)
            {
                return abilities[0];
            }
            return null;
        }

        public List<Main.Pokemon.Ability> PBPGetAbilitiesGainable(
            Main.Pokemon.Pokemon pokemon
            )
        {
            List<Main.Pokemon.Ability> abilities = new List<Main.Pokemon.Ability>();
            List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(
                pokemon: pokemon,
                ignoreSuppression: true
                );
            for (int i = 0; i < pbAbilities.Count; i++)
            {
                Main.Pokemon.Ability ability = pbAbilities[i];
                bool addAbility = true;

                if (ability.data.HasTag(AbilityTag.CannotRolePlay))
                {
                    addAbility = false;
                }

                if (addAbility)
                {
                    abilities.Add(ability);
                }
            }

            return abilities;
        }

        public Main.Pokemon.Ability PBPGetAbilityWithEffect(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false,
            bool ignoreSuppression = false
            )
        {
            List<Main.Pokemon.Ability> abilities = PBPGetAbilitiesWithEffect(
                pokemon: pokemon,
                effectType: effectType,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression
                );
            if (abilities.Count > 0)
            {
                return abilities[0];
            }
            return null;
        }
        public List<Main.Pokemon.Ability> PBPGetAbilitiesWithEffect(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false,
            bool ignoreSuppression = false
            )
        {
            List<Main.Pokemon.Ability> abilities = new List<Main.Pokemon.Ability>();
            List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(
                pokemon: pokemon,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression
                );
            for (int i = 0; i < pbAbilities.Count; i++)
            {
                Main.Pokemon.Ability ability = pbAbilities[i];
                if (ability.data.GetEffectNew(effectType) != null)
                {
                    abilities.Add(ability);
                }
            }
            return abilities;
        }

        public bool PBPHasAbility(
            string abilityID,
            Main.Pokemon.Pokemon pokemon,
            bool bypassAbility = false,
            bool ignoreSuppression = true
            )
        {
            List<Main.Pokemon.Ability> abilities = PBPGetAbilities(
                pokemon: pokemon,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression);
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].data.ID == abilityID)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Data.Ability> PBPGetAbilityDatas(
            Main.Pokemon.Pokemon pokemon,
            bool bypassAbility = false,
            bool ignoreSuppression = false)
        {
            List<Data.Ability> abilities = new List<Data.Ability>();
            List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(
                pokemon: pokemon,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression);

            for (int i = 0; i < pbAbilities.Count; i++)
            {
                abilities.Add(pbAbilities[i].data);
            }
            return abilities;
        }
        public Data.Ability PBPGetAbilityData(
            Main.Pokemon.Pokemon pokemon,
            bool bypassAbility = false,
            bool ignoreSuppression = false)
        {
            List<Data.Ability> abilities = PBPGetAbilityDatas(
                pokemon: pokemon,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression);
            if (abilities.Count > 0)
            {
                return abilities[0];
            }
            return null;
        }



        public List<Data.Ability> PBPGetAbilityDatasWithEffect(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false,
            bool ignoreSuppression = false
            )
        {
            List<Data.Ability> abilities = new List<Data.Ability>();
            List<Data.Ability> abilityDatas = PBPGetAbilityDatas(
                pokemon: pokemon,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression
                );
            for (int i = 0; i < abilityDatas.Count; i++)
            {
                if (abilityDatas[i].GetEffectNew(effectType) != null)
                {
                    abilities.Add(abilityDatas[i]);
                }
            }
            return abilities;
        }
        public Data.Ability PBPGetAbilityDataWithEffect(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false,
            bool ignoreSuppression = false)
        {
            List<Data.Ability> abilityDatas = PBPGetAbilityDatasWithEffect(
                pokemon: pokemon,
                effectType: effectType,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression
                );
            if (abilityDatas.Count > 0)
            {
                return abilityDatas[0];
            }
            return null;
        }

        public List<PBS.Databases.Effects.Abilities.AbilityEffect> PBPGetAbilityEffects(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false,
            bool ignoreSuppression = false
            )
        {
            List<Data.Ability> pbAbilityDatas = PBPGetAbilityDatasWithEffect(
                pokemon: pokemon,
                effectType: effectType,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression
                );
            List<PBS.Databases.Effects.Abilities.AbilityEffect> abilityEffects =
                new List<PBS.Databases.Effects.Abilities.AbilityEffect>();
            for (int i = 0; i < pbAbilityDatas.Count; i++)
            {
                abilityEffects.AddRange(pbAbilityDatas[i].GetEffectsNew(effectType));
            }
            return abilityEffects;
        }
        public PBS.Databases.Effects.Abilities.AbilityEffect PBPGetAbilityEffect(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false,
            bool ignoreSuppression = false
            )
        {
            List<PBS.Databases.Effects.Abilities.AbilityEffect> abilityEffects =
                PBPGetAbilityEffects(
                    pokemon: pokemon,
                    effectType: effectType,
                    bypassAbility: bypassAbility,
                    ignoreSuppression: ignoreSuppression);
            if (abilityEffects.Count > 0)
            {
                return abilityEffects[0];
            }
            return null;
        }

        public List<AbilityEffectPair> PBPGetAbilityEffectPairs(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false,
            bool ignoreSuppression = false
            )
        {
            List<AbilityEffectPair> effectPairs = new List<AbilityEffectPair>();
            List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(
                pokemon: pokemon,
                bypassAbility: bypassAbility,
                ignoreSuppression: ignoreSuppression);

            for (int i = 0; i < pbAbilities.Count; i++)
            {
                Main.Pokemon.Ability ability = pbAbilities[i];
                List<PBS.Databases.Effects.Abilities.AbilityEffect> effectsNew = ability.data.GetEffectsNew(effectType);
                for (int k = 0; k < effectsNew.Count; k++)
                {
                    PBS.Databases.Effects.Abilities.AbilityEffect effect = effectsNew[k];
                    effectPairs.Add(new AbilityEffectPair(ability, effect));
                }
            }
            return effectPairs;
        }
        public AbilityEffectPair PBPGetAbilityEffectPair(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false,
            bool ignoreSuppression = false
            )
        {
            List<AbilityEffectPair> effectPairs =
                PBPGetAbilityEffectPairs(
                    pokemon: pokemon,
                    effectType: effectType,
                    bypassAbility: bypassAbility,
                    ignoreSuppression: ignoreSuppression);
            if (effectPairs.Count > 0)
            {
                return effectPairs[0];
            }
            return null;
        }

        public Main.Pokemon.Pokemon PBPGetPokemonWithAbilityEffect(
            AbilityEffectType effectType,
            bool bypassAbility = false,
            bool ignoreSuppression = false)
        {
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (!IsPokemonFainted(pokemonOnField[i]))
                {
                    if (PBPGetAbilityEffect(
                        pokemon: pokemonOnField[i],
                        effectType: effectType,
                        bypassAbility: bypassAbility,
                        ignoreSuppression: ignoreSuppression) != null)
                    {
                        return pokemonOnField[i];
                    }
                }
            }
            return null;
        }

        public List<Main.Pokemon.Ability> PBPGetAbilitiesReplaceable(
            Main.Pokemon.Pokemon pokemon,
            List<Main.Pokemon.Ability> worrySeedAbilities)
        {
            List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(pokemon);
            List<Main.Pokemon.Ability> replaceableAbilities = new List<Main.Pokemon.Ability>();
            for (int i = 0; i < pbAbilities.Count; i++)
            {
                if (!pbAbilities[i].data.HasTag(AbilityTag.CannotWorrySeed))
                {
                    bool hasAbility = false;
                    for (int k = 0; k < worrySeedAbilities.Count; k++)
                    {
                        if (worrySeedAbilities[k].abilityID == pbAbilities[i].abilityID)
                        {
                            hasAbility = true;
                        }
                    }
                    if (!hasAbility)
                    {
                        replaceableAbilities.Add(pbAbilities[i]);
                    }
                }
            }
            return replaceableAbilities;
        }
        public List<Main.Pokemon.Ability> PBPSetAbilitiesReplaceable(
            Main.Pokemon.Pokemon pokemon,
            List<Main.Pokemon.Ability> worrySeedAbilities
            )
        {
            List<Main.Pokemon.Ability> setAbilities = new List<Main.Pokemon.Ability>();
            List<Main.Pokemon.Ability> replaceableAbilities = PBPGetAbilitiesReplaceable(
                pokemon: pokemon,
                worrySeedAbilities: worrySeedAbilities
                );
            for (int i = 0; i < replaceableAbilities.Count; i++)
            {
                pokemon.bProps.abilities.Remove(replaceableAbilities[i]);
            }

            List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(
                pokemon: pokemon,
                ignoreSuppression: true
                );
            for (int i = 0; i < worrySeedAbilities.Count; i++)
            {
                bool addAbility = true;
                for (int k = 0; k < pbAbilities.Count && addAbility; k++)
                {
                    if (pbAbilities[k].abilityID == worrySeedAbilities[i].abilityID)
                    {
                        addAbility = false;
                    }
                }
                if (addAbility)
                {
                    setAbilities.Add(worrySeedAbilities[i]);
                }
            }
            return setAbilities;
        }

        public bool PBPSuppressAbility(Main.Pokemon.Pokemon pokemon, Main.Pokemon.Ability ability)
        {
            return PBPSuppressAbilities(pokemon, new List<Main.Pokemon.Ability> { ability });
        }
        public bool PBPSuppressAbilities(Main.Pokemon.Pokemon pokemon, List<Main.Pokemon.Ability> abilities)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (!abilities[i].data.HasTag(AbilityTag.CannotSuppress))
                {
                    abilities[i].isSuppressed = true;
                }
            }
            return false;
        }
        public bool PBPIsNeutralizingGasInEffect(bool skipFaint = true)
        {
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                Main.Pokemon.Pokemon pokemon = pokemonOnField[i];
                if (!skipFaint || !IsPokemonFainted(pokemon))
                {
                    List<Main.Pokemon.Ability> abilities = PBPGetAbilities(
                        pokemon: pokemon,
                        skipNeutralizingGasCheck: true);
                    for (int k = 0; k < abilities.Count; k++)
                    {
                        Main.Pokemon.Ability ability = abilities[k];
                        if (ability.data.GetEffectNew(AbilityEffectType.NeutralizingGas) != null)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public List<Main.Pokemon.Ability> PBPGetNeutralizedAbilities(Main.Pokemon.Pokemon pokemon)
        {
            List<Main.Pokemon.Ability> neutralizedAbilities = new List<Main.Pokemon.Ability>();
            List<Main.Pokemon.Ability> pbAbilities = PBPGetAbilities(pokemon);
            for (int i = 0; i < pbAbilities.Count; i++)
            {
                Main.Pokemon.Ability ability = pbAbilities[i];
                if (!ability.data.HasTag(AbilityTag.CannotNeutralize))
                {
                    neutralizedAbilities.Add(ability);
                }
            }
            return neutralizedAbilities;
        }

        public Data.Ability PBPLegacyGetAbilityDataWithEffect(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false)
        {
            Data.Ability abilityData = Abilities.instance.GetAbilityData(pokemon.GetAbility());
            if (abilityData.GetEffect(effectType) != null)
            {
                if (!pokemon.bProps.isAbilitySuppressed || abilityData.HasTag(AbilityTag.CannotSuppress))
                {
                    if (!bypassAbility || abilityData.HasTag(AbilityTag.BypassMoldBreaker))
                    {
                        return abilityData;
                    }
                }
            }
            return null;
        }
        public AbilityEffect PBPLegacyGetAbilityEffect(
            Main.Pokemon.Pokemon pokemon,
            AbilityEffectType effectType,
            bool bypassAbility = false)
        {
            Data.Ability abilityData = PBPLegacyGetAbilityDataWithEffect(pokemon, effectType, bypassAbility);
            if (abilityData != null)
            {
                return abilityData.GetEffect(effectType);
            }
            return null;
        }

        // Status Conditions
        public StatusCondition PBPGetSC(Main.Pokemon.Pokemon pokemon, string statusID, bool descendant = true)
        {
            List<StatusCondition> conditions = PBPGetSCs(pokemon);
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i].statusID == statusID)
                {
                    return conditions[i];
                }
                if (descendant
                    && conditions[i].data.IsABaseID(statusID))
                {
                    return conditions[i];
                }
            }
            return null;
        }
        public List<StatusCondition> PBPGetSCs(Main.Pokemon.Pokemon pokemon)
        {
            List<StatusCondition> conditions = new List<StatusCondition>();
            if (!pokemon.nonVolatileStatus.data.HasTag(PokemonSTag.IsDefault))
            {
                conditions.Add(pokemon.nonVolatileStatus);
            }
            conditions.AddRange(pokemon.bProps.statusConditions);
            return conditions;
        }
        public List<StatusCondition> PBPGetSCs(Main.Pokemon.Pokemon pokemon, PBS.Databases.Effects.Filter.Harvest filter)
        {
            List<StatusCondition> scs = new List<StatusCondition>();
            if (filter.conditionType == PBS.Databases.Effects.Filter.Harvest.ConditionType.Pokemon)
            {
                if (filter.conditions.Count > 0)
                {
                    for (int i = 0; i < filter.conditions.Count; i++)
                    {
                        StatusCondition statusCond = PBPGetSC(pokemon, filter.conditions[i]);
                        if (statusCond != null && !scs.Contains(statusCond))
                        {
                            scs.Add(statusCond);
                        }
                    }
                }
            }
            return scs;
        }
        public Main.Pokemon.Ability PBPGetComatoseSCAbility(Main.Pokemon.Pokemon pokemon, string statusID)
        {
            List<Main.Pokemon.Ability> abilities = PBPGetAbilities(pokemon);

            for (int i = 0; i < abilities.Count; i++)
            {
                Main.Pokemon.Ability ability = abilities[i];
                List<PBS.Databases.Effects.Abilities.AbilityEffect> comatose_ =
                    ability.data.GetEffectsNew(AbilityEffectType.Comatose);
                for (int k = 0; k < comatose_.Count; k++)
                {
                    PBS.Databases.Effects.Abilities.Comatose comatose =
                        comatose_[k] as PBS.Databases.Effects.Abilities.Comatose;
                    PokemonStatus statusData = PokemonStatuses.instance.GetStatusData(comatose.statusID);
                    if (statusData.IsABaseID(statusID)
                        || statusData.ID == statusID)
                    {
                        return ability;
                    }
                }
            }
            return null;
        }

        // Type-Related
        public List<string> PBPGetTypes(Main.Pokemon.Pokemon pokemon)
        {
            List<string> pokemonTypes = new List<string>(pokemon.bProps.types);

            // Forest's Curse / Trick-or-Treat
            for (int i = 0; i < pokemon.bProps.forestsCurses.Count; i++)
            {
                pokemonTypes.Add(pokemon.bProps.forestsCurses[i].typeID);
            }

            // Roost
            if (!string.IsNullOrEmpty(pokemon.bProps.roostMove))
            {
                Move moveData = Moves.instance.GetMoveData(pokemon.bProps.roostMove);
                MoveEffect effect = moveData.GetEffect(MoveEffectType.RoostTypeLoss);
                if (effect != null)
                {
                    string[] typesLost = effect.stringParams;
                    for (int i = 0; i < typesLost.Length; i++)
                    {
                        pokemonTypes.Remove(typesLost[i]);
                    }
                }
            }

            // Smack Down
            if (pokemon.bProps.isSmackedDown)
            {
                for (int i = 0; i < pokemonTypes.Count; i++)
                {
                    Data.ElementalType typeData = Databases.ElementalTypes.instance.GetTypeData(pokemonTypes[i]);
                    if (typeData.HasTag(TypeTag.Airborne))
                    {
                        pokemonTypes.Remove(typeData.ID);
                        i--;
                    }
                }

            }

            return pokemonTypes;
        }
        public List<string> PBPGetTypeResistances(Main.Pokemon.Pokemon pokemon, bool allowInverse = true)
        {
            return GetTypeResistances(
                typeIDs: PBPGetTypes(pokemon),
                pokemon: pokemon,
                allowInverse: allowInverse
                );
        }
        public List<string> PBPGetTypeWeaknesses(Main.Pokemon.Pokemon pokemon, bool allowInverse = true)
        {
            return GetTypeWeaknesses(
                typeIDs: PBPGetTypes(pokemon),
                pokemon: pokemon,
                allowInverse: allowInverse
                );
        }
        public List<string> PBPGetTypeImmunities(Main.Pokemon.Pokemon pokemon, bool allowInverse = true)
        {
            return GetTypeImmunities(
                typeIDs: PBPGetTypes(pokemon),
                pokemon: pokemon,
                allowInverse: allowInverse
                );
        }
        public bool PBPIsPokemonGrounded(Main.Pokemon.Pokemon pokemon)
        {
            // Gravity forces grounded pokemon
            if (BBPIsPokemonAffectedByBSC(pokemon, gravity))
            {
                PBS.Databases.Effects.BattleStatuses.BattleSE gravity_ = gravity.data.GetEffectNew(BattleSEType.Gravity);
                if (gravity_ != null)
                {
                    PBS.Databases.Effects.BattleStatuses.Gravity gravityEffect = gravity_ as PBS.Databases.Effects.BattleStatuses.Gravity;
                    if (gravityEffect.intensifyGravity)
                    {
                        return true;
                    }
                }
            }

            // Ingrain causes grounded
            for (int i = 0; i < pokemon.bProps.ingrainMoves.Count; i++)
            {
                Move moveData = Moves.instance.GetMoveData(pokemon.bProps.ingrainMoves[i]);
                MoveEffect effect = moveData.GetEffect(MoveEffectType.Ingrain);
                if (effect.GetBool(0))
                {
                    return true;
                }
            }

            List<string> types = PBPGetTypes(pokemon);
            List<Main.Pokemon.Ability> abilities = PBPGetAbilities(pokemon);

            if (!pokemon.bProps.isSmackedDown)
            {
                // airborne types (ex. Flying)
                for (int i = 0; i < types.Count; i++)
                {
                    Data.ElementalType typeData = Databases.ElementalTypes.instance.GetTypeData(types[i]);
                    if (typeData.HasTag(TypeTag.Airborne))
                    {
                        return false;
                    }
                }

                // Levitate Check
                for (int i = 0; i < abilities.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.AbilityEffect levitate_ =
                        abilities[i].data.GetEffectNew(AbilityEffectType.Levitate);
                    if (levitate_ != null)
                    {
                        PBS.Databases.Effects.Abilities.Levitate levitate = levitate_ as PBS.Databases.Effects.Abilities.Levitate;
                        if (DoEffectFiltersPass(
                            filters: levitate.filters,
                            targetPokemon: pokemon
                            ))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public bool PBPIsPokemonTypeInversed(Main.Pokemon.Pokemon pokemon, string specificType)
        {
            return PBPIsPokemonTypeInversed(pokemon, new List<string> { specificType });
        }
        public bool PBPIsPokemonTypeInversed(Main.Pokemon.Pokemon pokemon, List<string> specificTypes = null)
        {
            specificTypes = specificTypes == null ? new List<string>() : specificTypes;

            bool inverse = false;
            if (BBPIsInversed())
            {
                inverse = !inverse;
            }

            return inverse;
        }

        // Misc. Properties
        public Main.Pokemon.Pokemon PBPGetImprison(Main.Pokemon.Pokemon pokemon, Move moveData)
        {
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (!IsPokemonFainted(pokemonOnField[i])
                    && !pokemonOnField[i].IsTheSameAs(pokemon))
                {
                    if (pokemonOnField[i].bProps.imprison != null)
                    {
                        bool imprisonCheck = false;

                        if (DoesPokemonHaveMove(pokemonOnField[i], moveData.ID))
                        {
                            imprisonCheck = true;
                        }

                        // Opponents only
                        if (imprisonCheck && ArePokemonAllies(pokemon, pokemonOnField[i]))
                        {
                            imprisonCheck = false;
                        }

                        if (imprisonCheck)
                        {
                            return pokemonOnField[i];
                        }
                    }
                }
            }
            return null;
        }
        public bool IsPokemonLockedOn(Main.Pokemon.Pokemon userPokemon, Main.Pokemon.Pokemon targetPokemon)
        {
            for (int i = 0; i < userPokemon.bProps.lockOnTargets.Count; i++)
            {
                if (userPokemon.bProps.lockOnTargets[i].pokemonUniqueID == targetPokemon.uniqueID)
                {
                    return true;
                }
            }
            return false;
        }

        // ---TEAM BATTLE PROPERTIES---

        // Status Conditions
        public TeamCondition TBPGetSC(Team team, string statusID, bool descendant = true)
        {
            TeamStatus statusData = TeamStatuses.instance.GetStatusData(statusID);
            List<TeamCondition> conditions = TBPGetSCs(team);
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i].statusID == statusID)
                {
                    return conditions[i];
                }
                if (descendant
                    && conditions[i].data.IsABaseID(statusID))
                {
                    return conditions[i];
                }
            }
            return null;
        }
        public List<TeamCondition> TBPGetSCs(Team team)
        {
            List<TeamCondition> conditions = new List<TeamCondition>();
            conditions.AddRange(team.bProps.conditions);
            conditions.AddRange(team.bProps.lightScreens);
            return conditions;
        }
        public List<TeamCondition> TBPGetSCs(Team team, PBS.Databases.Effects.Filter.Harvest filter)
        {
            List<TeamCondition> scs = new List<TeamCondition>();
            if (filter.conditionType == PBS.Databases.Effects.Filter.Harvest.ConditionType.Team)
            {
                if (filter.conditions.Count > 0)
                {
                    for (int i = 0; i < filter.conditions.Count; i++)
                    {
                        TeamCondition statusCond = TBPGetSC(team, filter.conditions[i]);
                        if (statusCond != null && !scs.Contains(statusCond))
                        {
                            scs.Add(statusCond);
                        }
                    }
                }
            }
            return scs;
        }

        public void AddTeamEntryHazard(Team team, Main.Team.BattleProperties.EntryHazard entryHazard)
        {
            Move moveData = Moves.instance.GetMoveData(entryHazard.hazardID);

            // Below is the order that entry hazards are applied
            MoveEffect SREffect = moveData.GetEffect(MoveEffectType.EntryHazardStealthRock);
            MoveEffect dmgEffect = moveData.GetEffect(MoveEffectType.EntryHazardDamage);
            MoveEffect statsEffect = moveData.GetEffect(MoveEffectType.EntryHazardStickyWeb);
            MoveEffect statusEffect = moveData.GetEffect(MoveEffectType.EntryHazardToxicSpikes);

            bool wasAdded = false;
            for (int i = 0; i < team.bProps.entryHazards.Count; i++)
            {
                Main.Team.BattleProperties.EntryHazard curHazard = team.bProps.entryHazards[i];
                Move orderedMoveData = Moves.instance.GetMoveData(curHazard.hazardID);

                bool addToList = false;
                if (!addToList
                    && SREffect != null
                    && orderedMoveData.GetEffect(MoveEffectType.EntryHazardStealthRock) == null)
                {
                    addToList = true;
                }

                if (!addToList
                    && dmgEffect != null
                    && orderedMoveData.GetEffect(MoveEffectType.EntryHazardDamage) == null)
                {
                    addToList = true;
                }

                if (!addToList
                    && statsEffect != null
                    && orderedMoveData.GetEffect(MoveEffectType.EntryHazardStickyWeb) == null)
                {
                    addToList = true;
                }

                if (!addToList
                    && statusEffect != null
                    && orderedMoveData.GetEffect(MoveEffectType.EntryHazardToxicSpikes) == null)
                {
                    addToList = true;
                }

                if (addToList)
                {
                    team.bProps.entryHazards.Insert(i, entryHazard);
                    wasAdded = true;
                    break;
                }
            }

            if (!wasAdded)
            {
                team.bProps.entryHazards.Add(entryHazard);
            }
        }
        public void AddTeamReflectScreen(Team team, Main.Team.BattleProperties.ReflectScreen reflectScreen)
        {
            team.bProps.reflectScreens.Add(reflectScreen);
        }
        public void AddTeamSafeguard(Team team, Main.Team.BattleProperties.Safeguard safeguard)
        {
            team.bProps.safeguards.Add(safeguard);
        }
        public Main.Team.BattleProperties.EntryHazard GetTeamEntryHazard(Team team, string moveID)
        {
            for (int i = 0; i < team.bProps.entryHazards.Count; i++)
            {
                if (team.bProps.entryHazards[i].hazardID == moveID)
                {
                    return team.bProps.entryHazards[i];
                }
            }
            return null;
        }
        public Main.Team.BattleProperties.ReflectScreen GetTeamReflectScreen(Team team, string moveID)
        {
            for (int i = 0; i < team.bProps.reflectScreens.Count; i++)
            {
                if (team.bProps.reflectScreens[i].moveID == moveID)
                {
                    return team.bProps.reflectScreens[i];
                }
            }
            return null;
        }
        public Main.Team.BattleProperties.Safeguard GetTeamSafeguard(Team team, string moveID)
        {
            for (int i = 0; i < team.bProps.safeguards.Count; i++)
            {
                if (team.bProps.safeguards[i].moveID == moveID)
                {
                    return team.bProps.safeguards[i];
                }
            }
            return null;
        }

        public bool TBPIsPokemonAffectedByTS(Main.Pokemon.Pokemon pokemon, TeamStatus statusData)
        {
            /*// Aerial only?
            if (statusData.HasTag(BattleSTag.IsAerial))
            {
                if (PBPIsPokemonGrounded(pokemon)
                    || pokemon.bProps.inDigState
                    || pokemon.bProps.inDiveState
                    || pokemon.bProps.inShadowForceState)
                {
                    return false;
                }
            }

            // Grounded only?
            if (statusData.HasTag(BattleSTag.IsGrounded))
            {
                if (!PBPIsPokemonGrounded(pokemon)
                    || pokemon.bProps.inDigState
                    || pokemon.bProps.inDiveState
                    || pokemon.bProps.inFlyState
                    || pokemon.bProps.inShadowForceState)
                {
                    return false;
                }
            }*/

            return true;
        }

        // ---BATTLE BATTLE PROPERTIES---

        // Status Conditions
        public BattleCondition BBPGetSC(string statusID, bool descendant = true)
        {
            BattleStatus statusData = BattleStatuses.instance.GetStatusData(statusID);
            List<BattleCondition> conditions = BBPGetSCs();
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i].statusID == statusID)
                {
                    return conditions[i];
                }
                if (descendant
                    && conditions[i].data.IsABaseID(statusID))
                {
                    return conditions[i];
                }
            }
            return null;
        }
        public List<BattleCondition> BBPGetSCs()
        {
            List<BattleCondition> conditions = new List<BattleCondition> { weather, terrain, gravity, magicRoom, trickRoom, wonderRoom };
            conditions.AddRange(statusConditions);
            return conditions;
        }
        public List<BattleCondition> BBPGetSCs(PBS.Databases.Effects.Filter.Harvest filter)
        {
            List<BattleCondition> scs = new List<BattleCondition>();
            if (filter.conditionType == PBS.Databases.Effects.Filter.Harvest.ConditionType.Battle)
            {
                if (filter.conditions.Count > 0)
                {
                    for (int i = 0; i < filter.conditions.Count; i++)
                    {
                        BattleCondition statusCond = BBPGetSC(filter.conditions[i]);
                        if (statusCond != null && !scs.Contains(statusCond))
                        {
                            scs.Add(statusCond);
                        }
                    }
                }
            }
            return scs;
        }
        public bool BBPIsActive(string statusID)
        {
            return BBPGetSC(statusID, true) != null;
        }
        public bool BBPIsActiveFromList(List<string> statusIDs, bool exact = false)
        {
            for (int i = 0; i < statusIDs.Count; i++)
            {
                if (BBPIsActive(statusIDs[i]))
                {
                    if (!exact)
                    {
                        return true;
                    }
                }
            }
            return exact ? true : false;
        }

        // Strong Winds
        public List<PBS.Databases.Effects.BattleStatuses.StrongWinds> BBPGetStrongWindsEffects(BattleHitTarget hitTarget)
        {
            return BBPGetStrongWindsEffects(new List<BattleHitTarget>() { hitTarget });
        }
        public List<PBS.Databases.Effects.BattleStatuses.StrongWinds> BBPGetStrongWindsEffects(List<BattleHitTarget> hitTargets)
        {
            List<PBS.Databases.Effects.BattleStatuses.StrongWinds> strongWinds = new List<PBS.Databases.Effects.BattleStatuses.StrongWinds>();
            List<BattleCondition> conditions = BBPGetSCs();
            for (int i = 0; i < conditions.Count; i++)
            {
                List<PBS.Databases.Effects.BattleStatuses.BattleSE> effects =
                    conditions[i].data.GetEffectsNew(BattleSEType.StrongWinds);
                for (int k = 0; k < effects.Count; k++)
                {
                    PBS.Databases.Effects.BattleStatuses.StrongWinds curEff = effects[k] as PBS.Databases.Effects.BattleStatuses.StrongWinds;
                    bool addEffect = false;

                    for (int j = 0; j < hitTargets.Count; j++)
                    {
                        BattleHitTarget curTarget = hitTargets[j];
                        if (BBPIsPokemonAffectedByBSC(curTarget.pokemon, conditions[i]))
                        {
                            if (BBPGetTargetStrongWindTypes(curTarget, curEff).Count > 0)
                            {
                                addEffect = true;
                                break;
                            }
                        }
                    }
                    if (addEffect)
                    {
                        strongWinds.Add(curEff);
                    }
                }
            }
            return strongWinds;
        }
        public List<string> BBPGetTargetStrongWindTypes(BattleHitTarget target, PBS.Databases.Effects.BattleStatuses.StrongWinds effect)
        {
            List<string> affectedTypes = new List<string>();

            for (int i = 0; i < effect.types.Count; i++)
            {
                if (
                    // Neutral
                    effect.effectivenessFilter.Contains(TypeEffectiveness.Neutral)
                        && target.effectiveness.IsTypeNeutral(effect.types[i])
                    ||
                    // Super-Effective
                    effect.effectivenessFilter.Contains(TypeEffectiveness.SuperEffective)
                        && target.effectiveness.IsTypeSuperEffective(effect.types[i])
                    ||
                    // Not Very Effective
                    effect.effectivenessFilter.Contains(TypeEffectiveness.NotVeryEffective)
                        && target.effectiveness.IsTypeNotVeryEffective(effect.types[i])
                    ||
                    // Immune
                    effect.effectivenessFilter.Contains(TypeEffectiveness.Immune)
                        && target.effectiveness.IsTypeNoEffect(effect.types[i])
                        )
                {
                    affectedTypes.Add(effect.types[i]);
                }
            }
            return affectedTypes;
        }


        // Inverse
        public bool BBPIsInversed()
        {
            if (battleSettings.isInverse)
            {
                return true;
            }
            return false;
        }
        public bool BBPIsPokemonAffectedByBSC(Main.Pokemon.Pokemon pokemon, BattleCondition condition)
        {
            return BBPIsPokemonAffectedByBS(pokemon: pokemon, statusData: condition.data);
        }
        public bool BBPIsPokemonAffectedByBS(Main.Pokemon.Pokemon pokemon, BattleStatus statusData)
        {
            // Weather
            if (statusData.GetEffectNew(BattleSEType.Weather) != null)
            {
                // Air Lock negates weather effects
                if (PBPGetPokemonWithAbilityEffect(AbilityEffectType.AirLock) != null)
                {
                    return false;
                }
            }

            // Aerial only?
            if (statusData.HasTag(BattleSTag.IsAerial))
            {
                if (PBPIsPokemonGrounded(pokemon)
                    || pokemon.bProps.inDigState
                    || pokemon.bProps.inDiveState
                    || pokemon.bProps.inShadowForceState)
                {
                    return false;
                }
            }

            // Grounded only?
            if (statusData.HasTag(BattleSTag.IsGrounded))
            {
                if (!PBPIsPokemonGrounded(pokemon)
                    || pokemon.bProps.inDigState
                    || pokemon.bProps.inDiveState
                    || pokemon.bProps.inFlyState
                    || pokemon.bProps.inShadowForceState)
                {
                    return false;
                }
            }

            return true;
        }


        // Types
        public bool IsTypeContained(string containerType, string checkType)
        {
            return AreTypesContained(containerTypes: new List<string> { containerType }, checkType: checkType);
        }
        public bool AreTypesContained(List<string> containerTypes, string checkType)
        {
            return AreTypesContained(containerTypes, new List<string> { checkType });
        }
        public bool AreTypesContained(string containerType, List<string> checkTypes)
        {
            return AreTypesContained(
                containerTypes: new List<string> { containerType },
                checkTypes: checkTypes
                );
        }
        public bool AreTypesContained(List<string> containerTypes, List<string> checkTypes)
        {
            for (int i = 0; i < checkTypes.Count; i++)
            {
                string curTypeCheck = checkTypes[i];
                Data.ElementalType typeData = Databases.ElementalTypes.instance.GetTypeData(curTypeCheck);

                bool typeContained = false;
                for (int k = 0; k < containerTypes.Count; k++)
                {
                    string curTypeCont = containerTypes[k];
                    if (curTypeCheck == curTypeCont || typeData.IsABaseID(curTypeCont))
                    {
                        typeContained = true;
                    }
                }
                if (!typeContained)
                {
                    return false;
                }
            }
            return true;
        }
        public bool DoesPokemonHaveType(Main.Pokemon.Pokemon pokemon, string typeID)
        {
            return DoesPokemonHaveATypeInList(pokemon, new string[] { typeID });
        }
        public bool DoesPokemonHaveATypeInList(Main.Pokemon.Pokemon pokemon, string[] types)
        {
            List<string> pokemonTypes = PBPGetTypes(pokemon);
            for (int i = 0; i < types.Length; i++)
            {
                if (pokemonTypes.Contains(types[i]))
                {
                    return true;
                }
            }
            return false;
        }
        public void SetPokemonType(Main.Pokemon.Pokemon pokemon, string type)
        {
            SetPokemonTypes(pokemon, new List<string> { type });
        }
        public void SetPokemonTypes(Main.Pokemon.Pokemon pokemon, List<string> types)
        {
            pokemon.bProps.types = new List<string>(types);
        }

        public string GetMoveType(string moveID)
        {
            return Moves.instance.GetMoveData(moveID).moveType;
        }
        public BattleTypeEffectiveness GetTypeEffectiveness(
            string offensiveType,
            Main.Pokemon.Pokemon targetPokemon,
            bool allowInverse = true
            )
        {
            return GetTypeEffectiveness(
                offensiveTypes: new List<string> { offensiveType },
                targetPokemon: targetPokemon,
                allowInverse: allowInverse);
        }
        public BattleTypeEffectiveness GetTypeEffectiveness(
            List<string> offensiveTypes,
            Main.Pokemon.Pokemon targetPokemon,
            HashSet<string> overwrittenTypes = null,
            HashSet<string> bypassResistances = null,
            HashSet<string> bypassWeaknesses = null,
            HashSet<string> bypassImmunities = null,
            bool allowInverse = true)
        {
            BattleTypeEffectiveness effectiveness = new BattleTypeEffectiveness();
            overwrittenTypes = overwrittenTypes == null ? new HashSet<string>() : overwrittenTypes;
            bypassResistances = bypassResistances == null ? new HashSet<string>() : bypassResistances;
            bypassWeaknesses = bypassWeaknesses == null ? new HashSet<string>() : bypassWeaknesses;
            bypassImmunities = bypassImmunities == null ? new HashSet<string>() : bypassImmunities;

            // Get Target type chart
            List<string> targetTypes = PBPGetTypes(targetPokemon);
            for (int i = 0; i < targetTypes.Count; i++)
            {
                float curEffectiveness = 1f;
                Data.ElementalType typeData = Databases.ElementalTypes.instance.GetTypeData(targetTypes[i]);
                // skip overwritten types
                if (overwrittenTypes.Contains(typeData.ID))
                {
                    continue;
                }

                List<string> resistances = GetTypeResistances(typeData.ID, targetPokemon, allowInverse);
                List<string> weaknesses = GetTypeWeaknesses(typeData.ID, targetPokemon, allowInverse);
                List<string> immunities = GetTypeImmunities(typeData.ID, targetPokemon, allowInverse);

                // Loop through offensive types
                for (int k = 0; k < offensiveTypes.Count; k++)
                {
                    Data.ElementalType offensiveTypeData = Databases.ElementalTypes.instance.GetTypeData(offensiveTypes[k]);
                    for (int j = 0; j < resistances.Count; j++)
                    {
                        string curType = resistances[j];
                        if ((offensiveTypeData.ID == curType || offensiveTypeData.IsABaseID(curType))
                            && !bypassResistances.Contains(curType))
                        {
                            curEffectiveness *= GameSettings.btlNotVeryEffectivenessMultiplier;
                        }
                    }
                    for (int j = 0; j < weaknesses.Count; j++)
                    {
                        string curType = weaknesses[j];
                        if ((offensiveTypeData.ID == curType || offensiveTypeData.IsABaseID(curType))
                            && !bypassWeaknesses.Contains(curType))
                        {
                            curEffectiveness *= GameSettings.btlSuperEffectivenessMultiplier;
                        }
                    }
                    for (int j = 0; j < immunities.Count; j++)
                    {
                        string curType = immunities[j];
                        if ((offensiveTypeData.ID == curType || offensiveTypeData.IsABaseID(curType))
                            && !bypassImmunities.Contains(curType))
                        {
                            curEffectiveness *= GameSettings.btlImmuneEffectivenessMultiplier;
                        }
                    }
                }
                effectiveness.FactorType(typeData.ID, curEffectiveness);
            }

            /*List<string> resistances = PBPGetTypeResistances(targetPokemon, allowInverse);
            List<string> weaknesses = PBPGetTypeWeaknesses(targetPokemon, allowInverse);
            List<string> immunities = PBPGetTypeImmunities(targetPokemon, allowInverse);

            // Loop through chart
            for (int i = 0; i < resistances.Count; i++)
            {
                float curEffectiveness = 1f;

                // skip overwritten types
                if (overwrittenTypes.Contains(resistances[i]))
                {
                    continue;
                }
                TypeData typeData = TypeDatabase.instance.GetTypeData(resistances[i]);

                // Loop through offensive types
                for (int k = 0; k < offensiveTypes.Count; k++)
                {
                    TypeData offensiveTypeData = TypeDatabase.instance.GetTypeData(offensiveTypes[k]);
                    if ((offensiveTypeData.ID == typeData.ID || offensiveTypeData.IsABaseID(typeData.ID))
                        && !bypassResistances.Contains(typeData.ID))
                    {
                        curEffectiveness *= GameSettings.btlNotVeryEffectivenessMultiplier;
                    }
                }
                effectiveness.FactorType(typeData.ID, curEffectiveness);
            }
            for (int i = 0; i < weaknesses.Count; i++)
            {
                float curEffectiveness = 1f;

                // skip overwritten types
                if (overwrittenTypes.Contains(weaknesses[i]))
                {
                    continue;
                }
                TypeData typeData = TypeDatabase.instance.GetTypeData(weaknesses[i]);

                // Loop through offensive types
                for (int k = 0; k < offensiveTypes.Count; k++)
                {
                    TypeData offensiveTypeData = TypeDatabase.instance.GetTypeData(offensiveTypes[k]);
                    if ((offensiveTypeData.ID == typeData.ID || offensiveTypeData.IsABaseID(typeData.ID))
                        && !bypassWeaknesses.Contains(typeData.ID))
                    {
                        curEffectiveness *= GameSettings.btlSuperEffectivenessMultiplier;
                    }
                }
                effectiveness.FactorType(typeData.ID, curEffectiveness);
            }
            for (int i = 0; i < immunities.Count; i++)
            {
                float curEffectiveness = 1f;

                // skip overwritten types
                if (overwrittenTypes.Contains(immunities[i]))
                {
                    continue;
                }
                TypeData typeData = TypeDatabase.instance.GetTypeData(immunities[i]);

                // Loop through offensive types
                for (int k = 0; k < offensiveTypes.Count; k++)
                {
                    TypeData offensiveTypeData = TypeDatabase.instance.GetTypeData(offensiveTypes[k]);
                    if ((offensiveTypeData.ID == typeData.ID || offensiveTypeData.IsABaseID(typeData.ID))
                        && !bypassImmunities.Contains(typeData.ID))
                    {
                        curEffectiveness *= GameSettings.btlImmuneEffectivenessMultiplier;
                    }
                }
                effectiveness.FactorType(typeData.ID, curEffectiveness);
            }*/

            return effectiveness;
        }
        public BattleTypeEffectiveness GetMoveEffectiveness(Main.Pokemon.Pokemon userPokemon, Move moveData, Main.Pokemon.Pokemon targetPokemon)
        {
            BattleTypeEffectiveness effectiveness = new BattleTypeEffectiveness();

            string moveType = moveData.moveType;
            List<string> targetTypes = PBPGetTypes(targetPokemon);

            // If the move deals typeless damage, no extra calculation needed
            if (moveData.GetEffect(MoveEffectType.TypelessDamage) != null)
            {
                return effectiveness;
            }

            // Thousand Arrows effect
            List<MoveEffect> thousandArrowList = moveData.GetEffects(MoveEffectType.ThousandArrows);
            if (thousandArrowList.Count > 0)
            {
                for (int i = 0; i < thousandArrowList.Count; i++)
                {
                    MoveEffect thousandArrows = thousandArrowList[i];
                    List<string> overrideTypes = new List<string>(thousandArrows.stringParams);

                    for (int k = 0; k < targetTypes.Count; k++)
                    {
                        string curType = targetTypes[k];
                        if (overrideTypes.Contains(curType))
                        {
                            float forceEffectiveness = 1f;
                            int iterations = Mathf.FloorToInt(thousandArrows.GetFloat(0));
                            iterations = Mathf.Max(1, iterations);
                            for (int j = 0; j < iterations; j++)
                            {
                                if (thousandArrows.GetBool(0))
                                {
                                    forceEffectiveness *= 1f;
                                }
                                else if (thousandArrows.GetBool(1))
                                {
                                    forceEffectiveness *= GameSettings.btlSuperEffectivenessMultiplier;
                                }
                                else if (thousandArrows.GetBool(2))
                                {
                                    forceEffectiveness *= GameSettings.btlNotVeryEffectivenessMultiplier;
                                }
                                else if (thousandArrows.GetBool(3))
                                {
                                    forceEffectiveness *= GameSettings.btlImmuneEffectivenessMultiplier;
                                }
                            }
                            effectiveness.FactorType(curType, forceEffectiveness);
                            return effectiveness;
                        }
                    }
                }
            }

            // Type calculations and overrides
            List<string> offensiveTypes = new List<string> { moveType };

            // Flying Press
            HashSet<string> bypassImmunities = new HashSet<string>();
            HashSet<string> bypassResistances = new HashSet<string>();
            HashSet<string> bypassWeaknesses = new HashSet<string>();
            List<MoveEffect> flyingPressEffects = moveData.GetEffects(MoveEffectType.FlyingPress);
            for (int i = 0; i < flyingPressEffects.Count; i++)
            {
                List<string> effectTypes = new List<string>(flyingPressEffects[i].stringParams);
                if (effectTypes.Contains("ALL"))
                {
                    effectTypes = Databases.ElementalTypes.instance.GetAllTypes();
                }

                // Bypass Resistance
                if (flyingPressEffects[i].GetBool(0))
                {
                    bypassResistances.UnionWith(effectTypes);
                }
                // Bypass Weakness
                if (flyingPressEffects[i].GetBool(1))
                {
                    bypassWeaknesses.UnionWith(effectTypes);
                }
                // Bypass Immunity
                if (flyingPressEffects[i].GetBool(2))
                {
                    bypassImmunities.UnionWith(effectTypes);
                }

                offensiveTypes.AddRange(effectTypes);
            }

            // Scrappy
            List<PBS.Databases.Effects.Abilities.AbilityEffect> scrappy_
                = PBPGetAbilityEffects(userPokemon, AbilityEffectType.Scrappy);
            for (int i = 0; i < scrappy_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.Scrappy scrappy = scrappy_[i] as PBS.Databases.Effects.Abilities.Scrappy;
                bypassImmunities.UnionWith(scrappy.bypassImmunities);
            }

            // Freeze-Dry
            HashSet<string> resistedTypes = new HashSet<string>();
            HashSet<string> weakTypes = new HashSet<string>();
            HashSet<string> immuneTypes = new HashSet<string>();
            List<MoveEffect> freezeDryEffects = moveData.GetEffects(MoveEffectType.FreezeDry);
            for (int i = 0; i < freezeDryEffects.Count; i++)
            {
                List<string> effectTypes = new List<string>(freezeDryEffects[i].stringParams);
                if (effectTypes.Contains("ALL"))
                {
                    effectTypes = Databases.ElementalTypes.instance.GetAllTypes();
                }

                // Types that resist this move
                if (freezeDryEffects[i].GetBool(0))
                {
                    resistedTypes.UnionWith(effectTypes);
                }
                // Types that are weak to this move
                if (freezeDryEffects[i].GetBool(1))
                {
                    weakTypes.UnionWith(effectTypes);
                }
                // Types that are immune to this move
                if (freezeDryEffects[i].GetBool(2))
                {
                    immuneTypes.UnionWith(effectTypes);
                }
            }

            // Record types that will be overwritten, we will not check these types in traditional calculations
            HashSet<string> overwrittenTypes = new HashSet<string>();
            for (int i = 0; i < targetTypes.Count; i++)
            {
                float curEffectiveness = 1f;
                Data.ElementalType typeData = Databases.ElementalTypes.instance.GetTypeData(targetTypes[i]);

                bool skipTypeCheck = false;

                // Skip type check of airborne types on a grounded pokemon
                if (typeData.HasTag(TypeTag.Airborne) && PBPIsPokemonGrounded(targetPokemon))
                {
                    skipTypeCheck = true;
                }

                // check type overwrites
                if (resistedTypes.Contains(typeData.ID))
                {
                    curEffectiveness *= GameSettings.btlNotVeryEffectivenessMultiplier;
                    skipTypeCheck = true;
                }
                if (weakTypes.Contains(typeData.ID))
                {
                    curEffectiveness *= GameSettings.btlSuperEffectivenessMultiplier;
                    skipTypeCheck = true;
                }
                if (immuneTypes.Contains(typeData.ID))
                {
                    curEffectiveness *= GameSettings.btlNotVeryEffectivenessMultiplier;
                    skipTypeCheck = true;
                }

                if (skipTypeCheck)
                {
                    overwrittenTypes.Add(typeData.ID);
                }

                effectiveness.FactorType(typeData.ID, curEffectiveness);
            }

            effectiveness.FactorOther(GetTypeEffectiveness(
                offensiveTypes: offensiveTypes,
                targetPokemon: targetPokemon,
                overwrittenTypes: overwrittenTypes,
                bypassResistances: bypassResistances,
                bypassWeaknesses: bypassWeaknesses,
                bypassImmunities: bypassImmunities,
                allowInverse: true
                ));

            return effectiveness;
        }
        public List<string> GetMoveOffensiveTypes(Main.Pokemon.Pokemon pokemon, Move moveData)
        {
            List<string> offensiveTypes = new List<string> { moveData.moveType };

            // Flying Press
            List<MoveEffect> flyingPressEffects = moveData.GetEffects(MoveEffectType.FlyingPress);
            for (int i = 0; i < flyingPressEffects.Count; i++)
            {
                List<string> effectTypes = new List<string>(flyingPressEffects[i].stringParams);
                if (effectTypes.Contains("ALL"))
                {
                    effectTypes = Databases.ElementalTypes.instance.GetAllTypes();
                }

                offensiveTypes.AddRange(effectTypes);
            }
            return offensiveTypes;
        }

        public List<string> GetTypeResistances(string typeID, Main.Pokemon.Pokemon pokemon = null, bool allowInverse = true)
        {
            return GetTypeResistances(
                typeIDs: new List<string> { typeID },
                pokemon: pokemon,
                allowInverse: allowInverse
                );
        }
        public List<string> GetTypeResistances(List<string> typeIDs, Main.Pokemon.Pokemon pokemon = null, bool allowInverse = true)
        {
            bool inverse = pokemon == null ? BBPIsInversed() : PBPIsPokemonTypeInversed(pokemon);

            List<string> types = new List<string>();

            // Tar Shot
            if (pokemon != null)
            {
                for (int i = 0; i < pokemon.bProps.tarShots.Count; i++)
                {
                    types.AddRange(pokemon.bProps.tarShots[i].resistances);
                }
            }

            for (int i = 0; i < typeIDs.Count; i++)
            {
                Data.ElementalType typeData = Databases.ElementalTypes.instance.GetTypeData(typeIDs[i]);
                if (inverse && allowInverse)
                {
                    types.AddRange(typeData.weaknesses);
                }
                else
                {
                    types.AddRange(typeData.resistances);
                }
            }
            return types;
        }
        public List<string> GetTypeWeaknesses(string typeID, Main.Pokemon.Pokemon pokemon = null, bool allowInverse = true)
        {
            return GetTypeWeaknesses(
                typeIDs: new List<string> { typeID },
                pokemon: pokemon,
                allowInverse: allowInverse
                );
        }
        public List<string> GetTypeWeaknesses(List<string> typeIDs, Main.Pokemon.Pokemon pokemon = null, bool allowInverse = true)
        {
            bool inverse = pokemon == null ? BBPIsInversed() : PBPIsPokemonTypeInversed(pokemon);

            List<string> types = new List<string>();

            // Tar Shot
            if (pokemon != null)
            {
                for (int i = 0; i < pokemon.bProps.tarShots.Count; i++)
                {
                    types.AddRange(pokemon.bProps.tarShots[i].weaknesses);
                }
            }

            for (int i = 0; i < typeIDs.Count; i++)
            {
                Data.ElementalType typeData = Databases.ElementalTypes.instance.GetTypeData(typeIDs[i]);
                if (inverse && allowInverse)
                {
                    types.AddRange(typeData.resistances);
                    types.AddRange(typeData.immunities);
                }
                else
                {
                    types.AddRange(typeData.weaknesses);
                }
            }
            return types;
        }
        public List<string> GetTypeImmunities(string typeID, Main.Pokemon.Pokemon pokemon = null, bool allowInverse = true)
        {
            return GetTypeImmunities(
                typeIDs: new List<string> { typeID },
                pokemon: pokemon,
                allowInverse: allowInverse
                );
        }
        public List<string> GetTypeImmunities(List<string> typeIDs, Main.Pokemon.Pokemon pokemon = null, bool allowInverse = true)
        {
            bool inverse = pokemon == null ? BBPIsInversed() : PBPIsPokemonTypeInversed(pokemon);

            // Foresight / Odor Sleuth / Miracle Eye (Filter out immunities)
            List<PBS.Databases.Effects.PokemonStatuses.Identification> identifieds = new List<PBS.Databases.Effects.PokemonStatuses.Identification>();
            if (pokemon != null)
            {
                identifieds = new List<PBS.Databases.Effects.PokemonStatuses.Identification>(pokemon.bProps.identifieds);
            }

            List<string> types = new List<string>();

            // Tar Shot
            if (pokemon != null)
            {
                for (int i = 0; i < pokemon.bProps.tarShots.Count; i++)
                {
                    types.AddRange(pokemon.bProps.tarShots[i].immunities);
                }
            }

            for (int i = 0; i < typeIDs.Count; i++)
            {
                Data.ElementalType typeData = Databases.ElementalTypes.instance.GetTypeData(typeIDs[i]);
                bool addImmunities = true;

                // Foresight / Odor Sleuth / Miracle Eye
                for (int k = 0; k < identifieds.Count; k++)
                {
                    PBS.Databases.Effects.PokemonStatuses.Identification curID = identifieds[k];
                    if (AreTypesContained(curID.types, typeData.ID))
                    {
                        addImmunities = false;
                        break;
                    }
                }

                if (addImmunities)
                {
                    if (inverse && allowInverse)
                    {

                    }
                    else
                    {
                        types.AddRange(typeData.immunities);
                    }
                }
            }

            return types;
        }

        // Moves: General
        public Move GetPokemonZMoveData(
            Main.Pokemon.Pokemon userPokemon,
            string moveID
            )
        {
            Move baseMoveData = Moves.instance.GetMoveData(moveID);
            if (baseMoveData.category == MoveCategory.Physical || baseMoveData.category == MoveCategory.Special)
            {
                PBS.Databases.Effects.Items.ItemEffect zCrystal_ = PBPGetItemEffect(userPokemon, ItemEffectType.ZCrystal);
                List<PBS.Databases.Effects.Items.ItemEffect> zCrystalSignatures_ =
                    PBPGetItemEffects(userPokemon, ItemEffectType.ZCrystalSignature);

                // Signature Z-Crystal Check
                for (int i = 0; i < zCrystalSignatures_.Count; i++)
                {
                    PBS.Databases.Effects.Items.ZCrystalSignature zCrystalSignature =
                        zCrystalSignatures_[i] as PBS.Databases.Effects.Items.ZCrystalSignature;

                    bool eligibleUser = false;
                    bool eligibleMove = false;
                    List<string> pokemonIDs = new List<string>(zCrystalSignature.pokemonIDs);
                    List<string> moveIDs = new List<string>(zCrystalSignature.eligibleMoves);

                    for (int k = 0; k < pokemonIDs.Count && !eligibleUser; k++)
                    {
                        if (PBPIsPokemonOfForm(
                            pokemon: userPokemon,
                            formPokemon: pokemonIDs[k],
                            allowDerivatives: true
                            ))
                        {
                            eligibleUser = true;
                        }
                    }
                    for (int k = 0; k < moveIDs.Count && !eligibleMove; k++)
                    {
                        if (baseMoveData.ID == moveIDs[k]
                            || baseMoveData.IsABaseID(moveIDs[k]))
                        {
                            eligibleMove = true;
                        }
                    }

                    if (eligibleMove)
                    {
                        return Moves.instance.GetMoveData(zCrystalSignature.ZMove);
                    }
                }
                // General Z-Crystal Check
                if (zCrystal_ != null)
                {
                    PBS.Databases.Effects.Items.ZCrystal zCrystal = zCrystal_ as PBS.Databases.Effects.Items.ZCrystal;
                    if (baseMoveData.moveType == zCrystal.moveType)
                    {
                        Move zMoveData = Moves.instance.GetMoveData(zCrystal.ZMove);

                        // Get Z-Power
                        int ZBasePower = baseMoveData.ZBasePower;
                        if (baseMoveData.ZBasePower > 0)
                        {
                            ZBasePower = baseMoveData.ZBasePower;
                        }
                        else
                        {
                            ZBasePower = baseMoveData.basePower <= 55 ? 100
                                : baseMoveData.basePower <= 65 ? 120
                                : baseMoveData.basePower <= 75 ? 140
                                : baseMoveData.basePower <= 85 ? 160
                                : baseMoveData.basePower <= 95 ? 175
                                : baseMoveData.basePower <= 100 ? 180
                                : baseMoveData.basePower <= 110 ? 185
                                : baseMoveData.basePower <= 125 ? 190
                                : baseMoveData.basePower <= 130 ? 195
                                : 200;
                        }
                        zMoveData.basePower = ZBasePower;
                        zMoveData.category = baseMoveData.category;
                        return zMoveData;
                    }
                }
            }


            return null;
        }
        public Move GetPokemonMaxMoveData(
            Main.Pokemon.Pokemon userPokemon,
            Move moveData
            )
        {
            // Max Guard
            if (moveData.category == MoveCategory.Status)
            {
                return GetPokemonMaxGuard(userPokemon, moveData);
            }
            // Other attacks
            else
            {
                Move maxMoveData;

                if (!string.IsNullOrEmpty(userPokemon.dynamaxProps.GMaxForm)
                    && userPokemon.dynamaxProps.moveType == moveData.moveType)
                {
                    maxMoveData = Moves.instance.GetMoveData(userPokemon.dynamaxProps.GMaxMove);
                }
                else
                {
                    Data.ElementalType typeData = Databases.ElementalTypes.instance.GetTypeData(moveData.moveType);
                    maxMoveData = Moves.instance.GetMoveData(typeData.maxMove);
                }
                if (maxMoveData.basePower <= 0)
                {
                    maxMoveData.basePower = moveData.MaxPower > 0 ? moveData.MaxPower : moveData.basePower;
                }
                maxMoveData.category = moveData.category;

                return maxMoveData;
            }
        }
        public Move GetPokemonMaxGuard(Main.Pokemon.Pokemon userPokemon, Move moveData)
        {
            return Moves.instance.GetMoveData("maxguard");
        }
        public Move GetPokemonMoveData(
            Main.Pokemon.Pokemon userPokemon,
            string moveID,
            Main.Pokemon.Pokemon targetPokemon,
            BattleCommand command = null,
            bool overrideZMove = false, bool overrideMaxMove = false,
            bool forceZMove = false, bool forceMaxMove = false,
            int hit = 1,
            PBS.Databases.Effects.Moves.Magnitude.MagnitudeLevel magnitudeLevel = null,
            PBS.Databases.Effects.Abilities.ParentalBond.BondedHit parentalBondHit = null
            )
        {
            return GetPokemonMoveData(
                userPokemon: userPokemon, targetPokemon: new List<Main.Pokemon.Pokemon> { targetPokemon },
                moveID: moveID, hit: hit,
                magnitudeLevel: magnitudeLevel, parentalBondHit: parentalBondHit,
                command: command,
                overrideZMove: overrideZMove, overrideMaxMove: overrideMaxMove,
                forceZMove: forceZMove, forceMaxMove: forceMaxMove
                );
        }
        public Move GetPokemonMoveData(
            Main.Pokemon.Pokemon userPokemon,
            string moveID,
            List<Main.Pokemon.Pokemon> targetPokemon = null,
            BattleCommand command = null,
            bool overrideZMove = false, bool overrideMaxMove = false,
            bool forceZMove = false, bool forceMaxMove = false,
            int hit = 1,
            PBS.Databases.Effects.Moves.Magnitude.MagnitudeLevel magnitudeLevel = null,
            PBS.Databases.Effects.Abilities.ParentalBond.BondedHit parentalBondHit = null
            )
        {
            Move baseMoveData = Moves.instance.GetMoveData(moveID);

            // Z-Move Overwrite
            bool convertedToZMove = false;
            if (!overrideZMove)
            {
                bool willConvertToZMove = forceZMove;
                if (command != null && !willConvertToZMove)
                {
                    willConvertToZMove = command.isZMove;
                }

                if (willConvertToZMove)
                {
                    Move zMoveData = GetPokemonZMoveData(userPokemon, moveID);
                    if (zMoveData != null)
                    {
                        convertedToZMove = true;
                        baseMoveData = zMoveData;
                    }
                }
            }
            

            MoveCategory category = baseMoveData.category;
            string moveType = baseMoveData.moveType;
            float basePower = baseMoveData.basePower;
            float accuracy = baseMoveData.accuracy;
            int priority = baseMoveData.priority;

            Move pokemonMoveData = baseMoveData.Clone();
            targetPokemon = targetPokemon == null ? new List<Main.Pokemon.Pokemon>() : targetPokemon;
            List<Main.Pokemon.Pokemon> allyPokemon = GetAllyPokemon(userPokemon);
            List<StatusCondition> userConditions = PBPGetSCs(userPokemon);
            List<BattleCondition> bConditions = BBPGetSCs();

            // ---MOVES---


            // ---Overwriting Move Base Power---

            // Beat Up
            PBS.Databases.Effects.Moves.MoveEffect beatUp_ = pokemonMoveData.GetEffectNew(MoveEffectType.BeatUp);
            if (beatUp_ != null)
            {
                PBS.Databases.Effects.Moves.BeatUp beatUp = beatUp_ as PBS.Databases.Effects.Moves.BeatUp;
                Trainer trainer = GetPokemonOwner(userPokemon);

                Main.Pokemon.Pokemon beatUpUser = null;
                List<Main.Pokemon.Pokemon> beatUpPokemon = GetTrainerAllAvailablePokemon(trainer);
                if (beatUpPokemon.Count >= hit - 1)
                {
                    beatUpUser = beatUpPokemon[hit - 1];
                }
                else
                {
                    beatUpUser = userPokemon;
                    Debug.LogWarning("Couldn't find a beat up user for hit " + hit);
                }
                basePower = beatUpUser.data.baseATK / 10f + 5;
            }

            // Fling
            MoveEffect fling = pokemonMoveData.GetEffect(MoveEffectType.Fling);
            if (fling != null)
            {
                Item item = GetPokemonItemFiltered(userPokemon, ItemEffectType.Fling);
                if (item != null)
                {
                    ItemEffect effect = item.data.GetEffect(ItemEffectType.Fling);
                    basePower = Mathf.FloorToInt(effect.GetFloat(0));
                }
            }

            // Hidden Power
            PBS.Databases.Effects.Moves.MoveEffect hiddenPower_ = pokemonMoveData.GetEffectNew(MoveEffectType.HiddenPower);
            if (hiddenPower_ != null)
            {
                PBS.Databases.Effects.Moves.HiddenPower hiddenPower = hiddenPower_ as PBS.Databases.Effects.Moves.HiddenPower;
                if (hiddenPower.calculateType && hiddenPower.types.Count > 0)
                {
                    List<string> possibleTypes = new List<string>(hiddenPower.types);
                    if (possibleTypes.Contains("ALL"))
                    {
                        possibleTypes = Databases.ElementalTypes.instance.GetAllTypes();
                    }
                    int hpBit = userPokemon.ivHP % 2;
                    int atkBit = userPokemon.ivATK % 2;
                    int defBit = userPokemon.ivDEF % 2;
                    int spaBit = userPokemon.ivSPA % 2;
                    int spdBit = userPokemon.ivSPD % 2;
                    int speBit = userPokemon.ivSPE % 2;

                    float factor1 = hpBit + 2 * atkBit + 4 * defBit + 8 * speBit + 16 * spaBit + 32 * spdBit;
                    float factor2 = ((float)possibleTypes.Count - 1) / 63;
                    int typeValue = Mathf.FloorToInt(factor1 * factor2);
                    string newMoveType = possibleTypes[typeValue];

                    moveType = newMoveType;
                    Debug.Log((object)("DEBUG - Hidden Power Type: " + Databases.ElementalTypes.instance.GetTypeData(newMoveType).typeName));
                }
                if (hiddenPower.calculateDamage)
                {
                    int hpBit = userPokemon.ivHP % 4 == 2 ? 1
                        : userPokemon.ivHP % 4 == 3 ? 1
                        : 0;
                    int atkBit = userPokemon.ivATK % 4 == 2 ? 1
                        : userPokemon.ivATK % 4 == 3 ? 1
                        : 0;
                    int defBit = userPokemon.ivDEF % 4 == 2 ? 1
                        : userPokemon.ivDEF % 4 == 3 ? 1
                        : 0;
                    int spaBit = userPokemon.ivSPA % 4 == 2 ? 1
                        : userPokemon.ivSPA % 4 == 3 ? 1
                        : 0;
                    int spdBit = userPokemon.ivSPD % 4 == 2 ? 1
                        : userPokemon.ivSPD % 4 == 3 ? 1
                        : 0;
                    int speBit = userPokemon.ivSPE % 4 == 2 ? 1
                        : userPokemon.ivSPE % 4 == 3 ? 1
                        : 0;

                    float factor1 = hpBit + 2 * atkBit + 4 * defBit + 8 * speBit + 16 * spaBit + 32 * spdBit;
                    float factor2 = hiddenPower.highestBasePower / 63;
                    int newBasePower = Mathf.FloorToInt(factor1 * factor2 + hiddenPower.lowestBasePower);

                    basePower = newBasePower;
                    Debug.Log("DEBUG - Hidden Power Damage: " + basePower);
                }
            }

            // Heavy Slam
            PBS.Databases.Effects.Moves.MoveEffect heavySlam_ = pokemonMoveData.GetEffectNew(MoveEffectType.HeavySlam);
            if (heavySlam_ != null && targetPokemon.Count > 0)
            {
                PBS.Databases.Effects.Moves.HeavySlam heavySlam = heavySlam_ as PBS.Databases.Effects.Moves.HeavySlam;
                float userWeight = GetPokemonWeight(userPokemon);
                float totalTargetWeight = 0f;
                for (int i = 0; i < targetPokemon.Count; i++)
                {
                    totalTargetWeight += GetPokemonWeight(targetPokemon[i]);
                }
                float targetWeight = totalTargetWeight / targetPokemon.Count;

                basePower = heavySlam.lowestPower;
                if (userWeight > 0)
                {
                    for (int i = 0; i < heavySlam.relativeWeightThresholds.Count; i++)
                    {
                        float ratio = targetWeight / userWeight;
                        float curRatio = heavySlam.relativeWeightThresholds[i];
                        if (ratio < curRatio)
                        {
                            if (i == heavySlam.relativeWeightThresholds.Count - 1)
                            {
                                basePower = heavySlam.highestPower;
                            }
                            else
                            {
                                basePower = heavySlam.powerRange[i];
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                basePower = heavySlam.lowestPower;
                            }
                            else
                            {
                                basePower = heavySlam.powerRange[i - 1];
                            }
                        }
                    }
                }
            }

            // Low Kick
            PBS.Databases.Effects.Moves.MoveEffect lowKick_ = pokemonMoveData.GetEffectNew(MoveEffectType.LowKick);
            if (lowKick_ != null && targetPokemon.Count > 0)
            {
                PBS.Databases.Effects.Moves.LowKick lowKick = lowKick_ as PBS.Databases.Effects.Moves.LowKick;
                float userWeight = GetPokemonWeight(userPokemon);
                float totalTargetWeight = 0f;
                for (int i = 0; i < targetPokemon.Count; i++)
                {
                    totalTargetWeight += GetPokemonWeight(targetPokemon[i]);
                }
                float targetWeight = totalTargetWeight / targetPokemon.Count;

                basePower = lowKick.lowestPower;
                if (targetWeight > 0)
                {
                    for (int i = 0; i < lowKick.weightThresholds.Count; i++)
                    {
                        float curWeight = lowKick.weightThresholds[i];
                        if (targetWeight > curWeight)
                        {
                            if (i == lowKick.weightThresholds.Count - 1)
                            {
                                basePower = lowKick.highestPower;
                            }
                            else
                            {
                                basePower = lowKick.powerRange[i];
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                basePower = lowKick.lowestPower;
                            }
                            else
                            {
                                basePower = lowKick.powerRange[i - 1];
                            }
                        }
                    }
                }
            }

            // Magnitude
            if (magnitudeLevel != null)
            {
                basePower = magnitudeLevel.basePower;
            }

            // Natural Gift
            PBS.Databases.Effects.Moves.MoveEffect naturalGift = pokemonMoveData.GetEffectNew(MoveEffectType.NaturalGift);
            if (naturalGift != null)
            {
                Item naturalGiftItem = PBPGetItemWithEffect(userPokemon, ItemEffectType.NaturalGift);
                if (naturalGiftItem != null)
                {
                    PBS.Databases.Effects.Items.NaturalGift naturalGiftEffect =
                        naturalGiftItem.data.GetEffectNew(ItemEffectType.NaturalGift)
                        as PBS.Databases.Effects.Items.NaturalGift;
                    if (!string.IsNullOrEmpty(naturalGiftEffect.moveType))
                    {
                        moveType = naturalGiftEffect.moveType;
                    }
                    if (naturalGiftEffect.basePower > 0)
                    {
                        basePower = naturalGiftEffect.basePower;
                    }
                }
            }

            // Punishment
            PBS.Databases.Effects.Moves.MoveEffect punishment_ = pokemonMoveData.GetEffectNew(MoveEffectType.Punishment);
            if (punishment_ != null && targetPokemon.Count > 0)
            {
                PBS.Databases.Effects.Moves.Punishment punishment = punishment_ as PBS.Databases.Effects.Moves.Punishment;
                basePower = punishment.GetBasePower(targetPokemon[0]);
                Debug.Log("DEBUG - Punishment - " + basePower);
            }

            // Reversal
            PBS.Databases.Effects.Moves.MoveEffect reversal_ = pokemonMoveData.GetEffectNew(MoveEffectType.Reversal);
            if (reversal_ != null)
            {
                PBS.Databases.Effects.Moves.Reversal reversal = reversal_ as PBS.Databases.Effects.Moves.Reversal;
                basePower = reversal.lowestBasePower;

                float HPPercent = GetPokemonHPAsPercentage(userPokemon);
                for (int i = 0; i < reversal.reversalPowers.Count; i++)
                {
                    if (HPPercent < reversal.reversalPowers[i].HPThreshold)
                    {
                        basePower = reversal.reversalPowers[i].basePower;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            // ---Adding Move Base Power---

            // Stored Power
            PBS.Databases.Effects.Moves.MoveEffect storedPower_ = pokemonMoveData.GetEffectNew(MoveEffectType.StoredPower);
            if (storedPower_ != null)
            {
                PBS.Databases.Effects.Moves.StoredPower storedPower = storedPower_ as PBS.Databases.Effects.Moves.StoredPower;
                basePower += storedPower.GetPowerBoost(userPokemon);
                Debug.Log("DEBUG - Stored Power - " + basePower);
            }


            // ---Changing Move Category---

            // Photon Geyser
            PBS.Databases.Effects.Moves.MoveEffect photonGeyser_ = pokemonMoveData.GetEffectNew(MoveEffectType.PhotonGeyser);
            if (photonGeyser_ != null)
            {
                PBS.Databases.Effects.Moves.PhotonGeyser photonGeyser = photonGeyser_ as PBS.Databases.Effects.Moves.PhotonGeyser;
                int ATK = GetPokemonATK(
                    pokemon: userPokemon,
                    applyStatStage: true,
                    applyModifiers: false
                    );
                int SPA = GetPokemonSPA(
                    pokemon: userPokemon,
                    applyStatStage: true,
                    applyModifiers: false
                    );

                category = ATK > SPA ? MoveCategory.Physical
                    : SPA > ATK ? MoveCategory.Special
                    : category;
            }

            // Shell Side Arm
            PBS.Databases.Effects.Moves.MoveEffect shellSideArm_ = pokemonMoveData.GetEffectNew(MoveEffectType.ShellSideArm);
            if (shellSideArm_ != null
                && targetPokemon.Count > 0)
            {
                PBS.Databases.Effects.Moves.ShellSideArm shellSideArm = shellSideArm_ as PBS.Databases.Effects.Moves.ShellSideArm;
                int totalDEF = 0;
                int totalSPD = 0;
                for (int i = 0; i < targetPokemon.Count; i++)
                {
                    totalDEF +=
                        GetPokemonDEF(
                            pokemon: targetPokemon[i],
                            applyStatStage: true,
                            applyModifiers: false
                            );
                    totalSPD +=
                        GetPokemonSPD(
                            pokemon: targetPokemon[i],
                            applyStatStage: true,
                            applyModifiers: false
                            );
                }

                category = totalDEF < totalSPD ? MoveCategory.Physical
                    : totalSPD < totalDEF ? MoveCategory.Special
                    : category;
            }


            // ---Changing Move Typing---

            // Aura Wheel
            List<PBS.Databases.Effects.Moves.MoveEffect> auraWheel_ = pokemonMoveData.GetEffectsNew(MoveEffectType.AuraWheel);
            if (auraWheel_.Count > 0)
            {
                bool foundPokemon = false;
                for (int i = 0; i < auraWheel_.Count; i++)
                {
                    PBS.Databases.Effects.Moves.AuraWheel auraWheel = auraWheel_[i] as PBS.Databases.Effects.Moves.AuraWheel;
                    for (int k = 0; k < auraWheel.pokemonIDs.Count; k++)
                    {
                        if (userPokemon.pokemonID == auraWheel.pokemonIDs[k])
                        {
                            foundPokemon = true;
                            break;
                        }
                        // Transform
                        if (!foundPokemon && userPokemon.bProps.tProps != null)
                        {
                            if (userPokemon.bProps.tProps.pokemonID == auraWheel.pokemonIDs[k])
                            {
                                foundPokemon = true;
                                break;
                            }
                        }
                    }

                    if (foundPokemon)
                    {
                        moveType = auraWheel.type;
                        break;
                    }
                }
            }

            // Judgement
            PBS.Databases.Effects.Moves.MoveEffect judgment_ = pokemonMoveData.GetEffectNew(MoveEffectType.Judgment);
            if (judgment_ != null)
            {
                PBS.Databases.Effects.Moves.Judgment judgment = judgment_ as PBS.Databases.Effects.Moves.Judgment;
                PBS.Databases.Effects.Items.ItemEffect judgmentItem_ = PBPGetItemEffect(userPokemon, ItemEffectType.Judgment);
                if (judgmentItem_ != null)
                {
                    PBS.Databases.Effects.Items.Judgment judgmentItem = judgmentItem_ as PBS.Databases.Effects.Items.Judgment;
                    moveType = judgmentItem.moveType;
                }
            }

            // Weather Ball
            PBS.Databases.Effects.Moves.MoveEffect weatherBall_ = pokemonMoveData.GetEffectNew(MoveEffectType.WeatherBall);
            if (weatherBall_ != null)
            {
                PBS.Databases.Effects.Moves.WeatherBall weatherBall = weatherBall_ as PBS.Databases.Effects.Moves.WeatherBall;
                PBS.Databases.Effects.BattleStatuses.BattleSE weather_ = weather.data.GetEffectNew(BattleSEType.Weather);
                if (weather_ != null)
                {
                    PBS.Databases.Effects.BattleStatuses.Weather weatherEffect = weather_ as PBS.Databases.Effects.BattleStatuses.Weather;
                    if (!string.IsNullOrEmpty(weatherEffect.weatherBallType))
                    {
                        moveType = weatherEffect.weatherBallType;
                    }
                }
            }

            // Terrain Pulse
            PBS.Databases.Effects.Moves.MoveEffect terrainPulse_ = pokemonMoveData.GetEffectNew(MoveEffectType.TerrainPulse);
            if (terrainPulse_ != null)
            {
                PBS.Databases.Effects.Moves.TerrainPulse terrainPulse = terrainPulse_ as PBS.Databases.Effects.Moves.TerrainPulse;
                PBS.Databases.Effects.BattleStatuses.BattleSE terrain_ = terrain.data.GetEffectNew(BattleSEType.Terrain);
                if (terrain_ != null)
                {
                    PBS.Databases.Effects.BattleStatuses.Terrain terrainEffect = terrain_ as PBS.Databases.Effects.BattleStatuses.Terrain;
                    if (!string.IsNullOrEmpty(terrainEffect.terrainPulseType))
                    {
                        moveType = terrainEffect.terrainPulseType;
                    }
                }
            }

            // Parental Bond
            if (parentalBondHit != null)
            {
                basePower *= parentalBondHit.damageModifier;
            }

            // Aerilate
            if (!convertedToZMove)
            {
                PBS.Databases.Effects.Abilities.AbilityEffect aerilate_ = PBPGetAbilityEffect(userPokemon, AbilityEffectType.Aerilate);
                if (aerilate_ != null)
                {
                    Move aerilateMoveData = pokemonMoveData.PartialClone(
                        category: category,
                        moveType: moveType,
                        basePower: basePower, accuracy: accuracy, priority: priority
                        );

                    PBS.Databases.Effects.Abilities.Aerilate aerilate = aerilate_ as PBS.Databases.Effects.Abilities.Aerilate;
                    if (DoEffectFiltersPass(
                        filters: aerilate.filters,
                        userPokemon: userPokemon,
                        moveData: aerilateMoveData
                        ))
                    {
                        moveType = aerilate.toMoveType;
                        basePower *= aerilate.powerMultiplier;
                    }
                }
            }

            // Ion Deluge
            for (int i = 0; i < bConditions.Count; i++)
            {
                BattleCondition bCond = bConditions[i];
                Data.ElementalType moveTypeData = Databases.ElementalTypes.instance.GetTypeData(moveType);

                PBS.Databases.Effects.BattleStatuses.BattleSE ionDeluge_ = bCond.data.GetEffectNew(BattleSEType.IonDeluge);
                if (ionDeluge_ != null)
                {
                    PBS.Databases.Effects.BattleStatuses.IonDeluge ionDeluge = ionDeluge_ as PBS.Databases.Effects.BattleStatuses.IonDeluge;
                    bool foundCond = false;
                    for (int k = 0; k < ionDeluge.fromTypes.Count; k++)
                    {
                        if (moveTypeData.ID == ionDeluge.fromTypes[k]
                            || moveTypeData.IsABaseID(ionDeluge.fromTypes[k]))
                        {
                            foundCond = true;
                            break;
                        }
                    }
                    if (foundCond)
                    {
                        moveType = ionDeluge.toType;
                        break;
                    }
                }
            }


            // Max Move Overwrite
            bool convertedToMaxMove = false;
            if (!overrideMaxMove)
            {
                bool willConvertToMaxMove = forceMaxMove || userPokemon.dynamaxState != Main.Pokemon.Pokemon.DynamaxState.None;
                if (command != null && !willConvertToMaxMove)
                {
                    willConvertToMaxMove = command.isDynamaxing;
                }
                if (willConvertToMaxMove)
                {
                    convertedToMaxMove = true;

                    Move maxMovePriorData = pokemonMoveData.Clone();
                    maxMovePriorData.category = category;
                    maxMovePriorData.moveType = moveType;
                    maxMovePriorData.basePower = Mathf.FloorToInt(basePower);
                    maxMovePriorData.accuracy = accuracy;
                    maxMovePriorData.priority = priority;

                    pokemonMoveData = GetPokemonMaxMoveData(userPokemon, maxMovePriorData).Clone();

                    category = baseMoveData.category;
                    moveType = baseMoveData.moveType;
                    basePower = baseMoveData.basePower;
                    accuracy = baseMoveData.accuracy;
                    priority = baseMoveData.priority;
                }
            }
            


            // Electrify
            if (userPokemon.bProps.electrify != null)
            {
                moveType = userPokemon.bProps.electrify.moveType;
            }


            // ---Changing Move Priority---

            // Gale Wings / Prankster
            List<PBS.Databases.Effects.Abilities.AbilityEffect> galeWings_ = PBPGetAbilityEffects(userPokemon, AbilityEffectType.GaleWings);
            for (int i = 0; i < galeWings_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.GaleWings galeWings = galeWings_[i] as PBS.Databases.Effects.Abilities.GaleWings;
                Move tempMoveData = pokemonMoveData.PartialClone(
                    category: category,
                    moveType: moveType,
                    basePower: basePower, accuracy: accuracy, priority: priority
                    );

                if (DoEffectFiltersPass(
                    filters: galeWings.filters,
                    userPokemon: userPokemon,
                    targetPokemon: userPokemon,
                    moveData: tempMoveData
                    ))
                {
                    if (galeWings.mode == PBS.Databases.Effects.Abilities.GaleWings.PriorityMode.Add)
                    {
                        priority += galeWings.priority;
                    }
                    else if (galeWings.mode == PBS.Databases.Effects.Abilities.GaleWings.PriorityMode.Set)
                    {
                        priority = galeWings.priority;
                        break;
                    }
                }
            }

            // Grassy Glide
            PBS.Databases.Effects.Moves.MoveEffect grassyGlide_ = pokemonMoveData.GetEffectNew(MoveEffectType.GrassyGlide);
            if (grassyGlide_ != null)
            {
                PBS.Databases.Effects.Moves.GrassyGlide grassyGlide = grassyGlide_ as PBS.Databases.Effects.Moves.GrassyGlide;
                List<BattleCondition> conditions = BBPGetSCs();

                for (int i = 0; i < conditions.Count; i++)
                {
                    bool foundType = false;
                    for (int k = 0; k < grassyGlide.conditions.Count; k++)
                    {
                        if (BBPIsPokemonAffectedByBS(userPokemon, conditions[k].data))
                        {
                            BattleStatus statusData =
                                BattleStatuses.instance.GetStatusData(grassyGlide.conditions[k]);
                            if (conditions[i].statusID == statusData.ID
                                || conditions[i].data.IsABaseID(statusData.ID))
                            {
                                foundType = true;
                                break;
                            }
                        }
                    }
                    if (foundType)
                    {
                        priority =
                             grassyGlide.mode == PBS.Databases.Effects.Moves.GrassyGlide.PriorityMode.Add
                             ? priority + grassyGlide.priority : grassyGlide.priority;
                        break;
                    }
                }
            }

            // ---Scaling Move Base Power---

            // Eruption
            PBS.Databases.Effects.Moves.MoveEffect eruption_ = pokemonMoveData.GetEffectNew(MoveEffectType.Eruption);
            if (eruption_ != null)
            {
                float HPPercent = GetPokemonHPAsPercentage(userPokemon);
                basePower *= HPPercent;
                if (basePower < 1)
                {
                    basePower = 1;
                }
            }

            // Technician
            PBS.Databases.Effects.Abilities.AbilityEffect technician_ = PBPGetAbilityEffect(userPokemon, AbilityEffectType.Technician);
            if (technician_ != null)
            {
                PBS.Databases.Effects.Abilities.Technician technician = technician_ as PBS.Databases.Effects.Abilities.Technician;
                if (basePower <= technician.threshold)
                {
                    basePower *= technician.powerMultiplier;
                }
            }

            // Sheer Force
            List<PBS.Databases.Effects.Abilities.AbilityEffect> sheerForce_ = PBPGetAbilityEffects(userPokemon, AbilityEffectType.SheerForce);
            if (sheerForce_.Count > 0)
            {
                Move tempData = pokemonMoveData.PartialClone(
                    category: category,
                    moveType: moveType,
                    basePower: basePower, accuracy: accuracy, priority: priority
                    );
                if (DoesMoveTriggerSheerForce(tempData))
                {
                    for (int i = 0; i < sheerForce_.Count; i++)
                    {
                        PBS.Databases.Effects.Abilities.SheerForce sheerForce =
                            sheerForce_[i] as PBS.Databases.Effects.Abilities.SheerForce;
                        basePower *= sheerForce.powerMultiplier;
                    }
                }
            }

            // Rivalry
            List<PBS.Databases.Effects.Abilities.AbilityEffect> rivalry_ = PBPGetAbilityEffects(userPokemon, AbilityEffectType.Rivalry);
            if (rivalry_.Count > 0 && targetPokemon.Count == 1)
            {
                Main.Pokemon.Pokemon singleTarget = targetPokemon[0];
                PokemonGender gender1 = userPokemon.gender;
                PokemonGender gender2 = singleTarget.gender;

                for (int i = 0; i < rivalry_.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.Rivalry rivalry = rivalry_[i] as PBS.Databases.Effects.Abilities.Rivalry;
                    if (gender1 == gender2)
                    {
                        basePower *= rivalry.sameMultiplier;
                    }
                    else if (Main.Pokemon.Pokemon.IsGenderOpposite(gender1, gender2))
                    {
                        basePower *= rivalry.oppositeMultiplier;
                    }
                }
            }

            // Battery
            // (Self)
            List<PBS.Databases.Effects.Abilities.AbilityEffect> battery_ = PBPGetAbilityEffects(userPokemon, AbilityEffectType.Battery);
            if (battery_.Count > 0)
            {
                for (int i = 0; i < battery_.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.Battery battery = battery_[i] as PBS.Databases.Effects.Abilities.Battery;

                    if (battery.affectsSelf)
                    {
                        Move tempMoveData = pokemonMoveData.PartialClone(
                            category: category,
                            moveType: moveType,
                            basePower: basePower, accuracy: accuracy, priority: priority
                            );
                        if (DoEffectFiltersPass(
                            filters: battery.filters,
                            userPokemon: userPokemon,
                            targetPokemon: userPokemon,
                            moveData: tempMoveData
                            ))
                        {
                            basePower *= battery.powerMultiplier;
                        }
                    }
                }
            }
            // (Ally)
            for (int i = 0; i < allyPokemon.Count; i++)
            {
                Main.Pokemon.Pokemon ally = allyPokemon[i];
                if (!IsPokemonFainted(ally))
                {
                    List<PBS.Databases.Effects.Abilities.AbilityEffect> batteryAlly_ = PBPGetAbilityEffects(ally, AbilityEffectType.Battery);
                    if (batteryAlly_.Count > 0)
                    {
                        for (int k = 0; k < batteryAlly_.Count; k++)
                        {
                            PBS.Databases.Effects.Abilities.Battery battery = batteryAlly_[k] as PBS.Databases.Effects.Abilities.Battery;
                            Move tempMoveData = pokemonMoveData.PartialClone(
                                category: category,
                                moveType: moveType,
                                basePower: basePower, accuracy: accuracy, priority: priority
                                );
                            if (DoEffectFiltersPass(
                                filters: battery.filters,
                                userPokemon: ally,
                                targetPokemon: userPokemon,
                                moveData: tempMoveData
                                ))
                            {
                                basePower *= battery.powerMultiplier;
                            }
                        }
                    }
                }
            }

            // Analytic
            PBS.Databases.Effects.Abilities.AbilityEffect analytic_ = PBPGetAbilityEffect(userPokemon, AbilityEffectType.Analytic);
            if (analytic_ != null && targetPokemon.Count > 0)
            {
                PBS.Databases.Effects.Abilities.Analytic analytic = analytic_ as PBS.Databases.Effects.Abilities.Analytic;
                for (int i = 0; i < targetPokemon.Count; i++)
                {
                    if (targetPokemon[i].bProps.actedThisTurn)
                    {
                        basePower *= analytic.powerMultiplier;
                        break;
                    }
                }
            }

            // Flare Boost / Toxic Boost
            List<PBS.Databases.Effects.Abilities.AbilityEffect> flareBoost_ =
                    PBPGetAbilityEffects(userPokemon, AbilityEffectType.FlareBoost);
            for (int i = 0; i < flareBoost_.Count; i++)
            {
                Move tempMoveData = pokemonMoveData.PartialClone(
                    category: category,
                    moveType: moveType,
                    basePower: basePower, accuracy: accuracy, priority: priority
                    );
                PBS.Databases.Effects.Abilities.FlareBoost flareBoost = flareBoost_[i] as PBS.Databases.Effects.Abilities.FlareBoost;
                if (DoEffectFiltersPass(
                    filters: flareBoost.filters,
                    userPokemon: userPokemon,
                    moveData: tempMoveData
                    ))
                {
                    basePower *= flareBoost.powerMultiplier;
                }
            }

            // Iron Fist
            List<PBS.Databases.Effects.Abilities.AbilityEffect> ironFist_ = PBPGetAbilityEffects(userPokemon, AbilityEffectType.IronFist);
            if (ironFist_.Count > 0)
            {
                Move ironFistData = pokemonMoveData.Clone();
                ironFistData.category = category;
                ironFistData.moveType = moveType;
                ironFistData.basePower = Mathf.FloorToInt(basePower);
                ironFistData.accuracy = accuracy;
                ironFistData.priority = priority;

                for (int i = 0; i < ironFist_.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.IronFist ironFist = ironFist_[i] as PBS.Databases.Effects.Abilities.IronFist;
                    if (DoEffectFiltersPass(
                        filters: ironFist.filters,
                        userPokemon: userPokemon,
                        moveData: ironFistData
                        ))
                    {
                        bool isBoosted = true;

                        // Blaze / Overgrow / Swarm / Torrent
                        if (isBoosted && ironFist.blazeThreshold > 0)
                        {
                            if (GetPokemonHPAsPercentage(userPokemon) > ironFist.blazeThreshold)
                            {
                                isBoosted = false;
                            }
                        }

                        if (isBoosted)
                        {
                            basePower *= ironFist.powerMultiplier;
                        }
                    }

                }
            }
            // Steely Spirit (Ally)
            for (int i = 0; i < allyPokemon.Count; i++)
            {
                Main.Pokemon.Pokemon ally = allyPokemon[i];
                if (!IsPokemonFainted(ally))
                {
                    List<PBS.Databases.Effects.Abilities.AbilityEffect> steelySpirit_ =
                        PBPGetAbilityEffects(ally, AbilityEffectType.IronFist);
                    if (steelySpirit_.Count > 0)
                    {
                        for (int k = 0; k < steelySpirit_.Count; k++)
                        {
                            PBS.Databases.Effects.Abilities.IronFist steelySpirit = steelySpirit_[k] as PBS.Databases.Effects.Abilities.IronFist;
                            Move tempMoveData = pokemonMoveData.PartialClone(
                                category: category,
                                moveType: moveType,
                                basePower: basePower, accuracy: accuracy, priority: priority
                                );
                            if (DoEffectFiltersPass(
                                filters: steelySpirit.filters,
                                userPokemon: ally,
                                targetPokemon: userPokemon,
                                moveData: tempMoveData
                                ) && steelySpirit.steelySpirit)
                            {
                                basePower *= steelySpirit.powerMultiplier;
                            }
                        }
                    }
                }
            }

            // Stakeout
            PBS.Databases.Effects.Abilities.AbilityEffect stakeout_ = PBPGetAbilityEffect(userPokemon, AbilityEffectType.Stakeout);
            if (stakeout_ != null && targetPokemon.Count > 0)
            {
                PBS.Databases.Effects.Abilities.Stakeout stakeout = stakeout_ as PBS.Databases.Effects.Abilities.Stakeout;
                for (int i = 0; i < targetPokemon.Count; i++)
                {
                    if (targetPokemon[i].bProps.switchedIn)
                    {
                        basePower *= stakeout.powerMultiplier;
                        break;
                    }
                }
            }

            // Expanding Force
            PBS.Databases.Effects.Moves.MoveEffect expandingForce_ = pokemonMoveData.GetEffectNew(MoveEffectType.ExpandingForcePower);
            if (expandingForce_ != null)
            {
                PBS.Databases.Effects.Moves.ExpandingForcePower expandingForce = expandingForce_ as PBS.Databases.Effects.Moves.ExpandingForcePower;
                List<BattleCondition> conditions = BBPGetSCs();
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (BBPIsPokemonAffectedByBS(userPokemon, conditions[i].data))
                    {
                        bool foundCondition = false;
                        for (int k = 0; k < expandingForce.conditions.Count; k++)
                        {
                            if (conditions[i].statusID == expandingForce.conditions[k]
                                || conditions[i].data.IsABaseID(expandingForce.conditions[k]))
                            {
                                foundCondition = true;
                                break;
                            }
                        }
                        if (foundCondition)
                        {
                            Debug.Log("DEBUG - Expanding Force - " + expandingForce.damageScale);
                            basePower *= expandingForce.damageScale;
                            break;
                        }
                    }
                }
            }

            // Pursuit
            PBS.Databases.Effects.Moves.MoveEffect pursuit_ = pokemonMoveData.GetEffectNew(MoveEffectType.Pursuit);
            if (pursuit_ != null)
            {
                PBS.Databases.Effects.Moves.Pursuit pursuit = pursuit_ as PBS.Databases.Effects.Moves.Pursuit;
                if (targetPokemon.Count > 0)
                {
                    for (int i = 0; i < targetPokemon.Count; i++)
                    {
                        if (targetPokemon[i].bProps.isSwitchingOut)
                        {
                            basePower *= pursuit.damageScale;
                            break;
                        }
                    }
                }

            }

            // Rising Voltage
            PBS.Databases.Effects.Moves.MoveEffect risingVoltage_ = pokemonMoveData.GetEffectNew(MoveEffectType.RisingVoltage);
            if (risingVoltage_ != null && targetPokemon.Count > 0)
            {
                PBS.Databases.Effects.Moves.RisingVoltage risingVoltage = risingVoltage_ as PBS.Databases.Effects.Moves.RisingVoltage;
                List<BattleCondition> conditions = BBPGetSCs();
                for (int i = 0; i < conditions.Count; i++)
                {
                    bool foundTarget = false;
                    for (int k = 0; k < targetPokemon.Count; k++)
                    {
                        if (BBPIsPokemonAffectedByBS(targetPokemon[k], conditions[i].data))
                        {
                            bool foundCondition = false;
                            for (int j = 0; j < risingVoltage.conditions.Count; j++)
                            {
                                if (conditions[i].statusID == risingVoltage.conditions[j]
                                    || conditions[i].data.IsABaseID(risingVoltage.conditions[j]))
                                {
                                    foundCondition = true;
                                    break;
                                }
                            }
                            if (foundCondition)
                            {
                                foundTarget = true;
                                break;
                            }
                        }
                    }
                    if (foundTarget)
                    {
                        Debug.Log("DEBUG - Rising Voltage " + risingVoltage.damageScale);
                        basePower *= risingVoltage.damageScale;
                        break;
                    }
                }
            }

            // Rollout
            PBS.Databases.Effects.Moves.MoveEffect rollout_ = pokemonMoveData.GetEffectNew(MoveEffectType.Rollout);
            if (rollout_ != null)
            {
                PBS.Databases.Effects.Moves.Rollout rollout = rollout_ as PBS.Databases.Effects.Moves.Rollout;
                if (command != null)
                {
                    float scaleAmount = Mathf.Pow(rollout.damageScale, command.iteration - 1);
                    pokemonMoveData.basePower =
                        Mathf.FloorToInt(scaleAmount * pokemonMoveData.basePower);
                    Debug.Log("DEBUG - Rollout BP - " + pokemonMoveData.basePower);
                }
            }
            if (pokemonMoveData.HasTag(MoveTag.RollingMove))
            {
                if (userPokemon.bProps.defenseCurl != null)
                {
                    basePower *= userPokemon.bProps.defenseCurl.damageScale;
                    Debug.Log("DEBUG - Defense Curl BP - " + pokemonMoveData.basePower);
                }
            }

            // Triple Kick
            PBS.Databases.Effects.Moves.MoveEffect tripleKick_ = pokemonMoveData.GetEffectNew(MoveEffectType.TripleKick);
            if (tripleKick_ != null)
            {
                PBS.Databases.Effects.Moves.TripleKick tripleKick = tripleKick_ as PBS.Databases.Effects.Moves.TripleKick;
                basePower *= hit;
            }

            // Weather Ball
            if (weatherBall_ != null)
            {
                PBS.Databases.Effects.Moves.WeatherBall weatherBall = weatherBall_ as PBS.Databases.Effects.Moves.WeatherBall;
                PBS.Databases.Effects.BattleStatuses.BattleSE weather_ = weather.data.GetEffectNew(BattleSEType.Weather);
                if (weather_ != null)
                {
                    PBS.Databases.Effects.BattleStatuses.Weather weatherEffect = weather_ as PBS.Databases.Effects.BattleStatuses.Weather;
                    if (weatherEffect.weatherBallBoost)
                    {
                        basePower *= weatherBall.damageScale;
                    }
                }
            }

            // Terrain Pulse
            if (terrainPulse_ != null)
            {
                PBS.Databases.Effects.Moves.TerrainPulse terrainPulse = terrainPulse_ as PBS.Databases.Effects.Moves.TerrainPulse;
                PBS.Databases.Effects.BattleStatuses.BattleSE terrain_ = terrain.data.GetEffectNew(BattleSEType.Terrain);
                if (terrain_ != null)
                {
                    PBS.Databases.Effects.BattleStatuses.Terrain terrainEffect = terrain_ as PBS.Databases.Effects.BattleStatuses.Terrain;
                    basePower *= terrainPulse.damageScale;
                }
            }

            // Flash Fire
            for (int i = 0; i < userPokemon.bProps.flashFireBoosts.Count; i++)
            {
                if (userPokemon.bProps.flashFireBoosts[i].moveType == moveType)
                {
                    basePower *= userPokemon.bProps.flashFireBoosts[i].boost;
                }
            }

            // Charcoal
            List<PBS.Databases.Effects.Items.ItemEffect> charcoal_ = PBPGetItemEffects(userPokemon, ItemEffectType.Charcoal);
            for (int i = 0; i < charcoal_.Count; i++)
            {
                Move charcoalData = pokemonMoveData.PartialClone(
                    category: category,
                    moveType: moveType,
                    basePower: basePower, accuracy: accuracy, priority: priority
                    );

                PBS.Databases.Effects.Items.Charcoal charcoal = charcoal_[i] as PBS.Databases.Effects.Items.Charcoal;
                if (DoEffectFiltersPass(
                    filters: charcoal.filters,
                    targetPokemon: userPokemon,
                    moveData: charcoalData
                    ))
                {
                    bool applyCharcoal = true;

                    if (applyCharcoal)
                    {
                        basePower *= charcoal.powerModifier;
                    }
                }
            }

            // --- Removing Move Characteristics ---

            // Long Reach
            List<PBS.Databases.Effects.Abilities.AbilityEffect> longReach_ = PBPGetAbilityEffects(userPokemon, AbilityEffectType.LongReach);
            for (int i = 0; i < longReach_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.LongReach longReach = longReach_[i] as PBS.Databases.Effects.Abilities.LongReach;
                Move tempMoveData = pokemonMoveData.PartialClone(
                    category: category,
                    moveType: moveType,
                    basePower: basePower, accuracy: accuracy, priority: priority
                    );

                if (DoEffectFiltersPass(
                    filters: longReach.filters,
                    userPokemon: userPokemon,
                    moveData: tempMoveData
                    ))
                {
                    List<MoveTag> moveTags = new List<MoveTag>(longReach.moveTags);
                    for (int k = 0; k < moveTags.Count; k++)
                    {
                        pokemonMoveData.moveTags.Remove(moveTags[k]);
                    }
                }
            }





            pokemonMoveData.category = category;
            pokemonMoveData.moveType = moveType;
            pokemonMoveData.basePower = Mathf.FloorToInt(basePower);
            pokemonMoveData.accuracy = accuracy;
            pokemonMoveData.priority = priority;

            return pokemonMoveData;
        }
        public List<PBS.Databases.Effects.Moves.MoveEffect> GetPokemonSecretPowerEffects(
            Main.Pokemon.Pokemon userPokemon,
            Move moveData,
            PBS.Databases.Effects.Moves.SecretPower secretPowerMain)
        {
            List<PBS.Databases.Effects.Moves.MoveEffect> secretPowerEffects = new List<PBS.Databases.Effects.Moves.MoveEffect>();
            bool checkEnvironment = true;

            if (checkEnvironment)
            {
                // Building = paralysis
                if (environment.environmentType == BattleEnvironmentType.Building)
                {
                    PBS.Databases.Effects.Moves.InflictStatus effect = new PBS.Databases.Effects.Moves.InflictStatus(
                        new PBS.Databases.Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "paralysis"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: secretPowerMain.secondaryEffectChance
                        );
                    secretPowerEffects.Add(effect);
                }
                // Cave = flinching
                else if (environment.environmentType == BattleEnvironmentType.Cave)
                {
                    PBS.Databases.Effects.Moves.InflictStatus effect = new PBS.Databases.Effects.Moves.InflictStatus(
                        new PBS.Databases.Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: secretPowerMain.secondaryEffectChance
                        );
                    secretPowerEffects.Add(effect);
                }
                // Snow = freezing
                else if (environment.environmentType == BattleEnvironmentType.Snow)
                {
                    PBS.Databases.Effects.Moves.InflictStatus effect = new PBS.Databases.Effects.Moves.InflictStatus(
                        new PBS.Databases.Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "freeze"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: secretPowerMain.secondaryEffectChance
                        );
                    secretPowerEffects.Add(effect);
                }
                // Grass = sleep
                else if (environment.environmentType == BattleEnvironmentType.Field)
                {
                    PBS.Databases.Effects.Moves.InflictStatus effect = new PBS.Databases.Effects.Moves.InflictStatus(
                        new PBS.Databases.Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "sleep"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: secretPowerMain.secondaryEffectChance
                        );
                    secretPowerEffects.Add(effect);
                }
                // Volcano = burn
                else if (environment.environmentType == BattleEnvironmentType.Volcano)
                {
                    PBS.Databases.Effects.Moves.InflictStatus effect = new PBS.Databases.Effects.Moves.InflictStatus(
                        new PBS.Databases.Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "burn"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: secretPowerMain.secondaryEffectChance
                        );
                    secretPowerEffects.Add(effect);
                }
            }

            return secretPowerEffects;
        }

        public bool IsMoveDamaging(Move moveData)
        {
            return moveData.category == MoveCategory.Physical
                || moveData.category == MoveCategory.Special;
        }
        public bool IsMoveTagSatisfied(Main.Pokemon.Pokemon userPokemon, Move moveData, MoveTag tag, Main.Pokemon.Pokemon targetPokemon = null)
        {
            if (tag == MoveTag.MakesContact)
            {
                return DoesMoveMakeContact(
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    );
            }
            return moveData.HasTag(tag);
        }
        public bool DoesMoveMakeContact(Main.Pokemon.Pokemon userPokemon, Move moveData, Main.Pokemon.Pokemon targetPokemon = null)
        {
            bool contact = moveData.HasTag(MoveTag.MakesContact);

            // Long Reach

            // Protective Pads

            return contact;
        }
        public bool DoesMoveTriggerSheerForce(Move moveData)
        {
            for (int i = 0; i < moveData.effectsNew.Count; i++)
            {
                PBS.Databases.Effects.Moves.MoveEffect effect = moveData.effectsNew[i];
                if (IsMoveEffectAdditional(moveData, effect))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsMoveEffectAdditional(Move moveData, PBS.Databases.Effects.Moves.MoveEffect effect_)
        {
            // inflict status
            if (effect_ is PBS.Databases.Effects.Moves.InflictStatus)
            {
                PBS.Databases.Effects.Moves.InflictStatus effect = effect_ as PBS.Databases.Effects.Moves.InflictStatus;
                if (effect.targetType == MoveEffectTargetType.Target
                    || effect.targetType == MoveEffectTargetType.Team
                    || effect.targetType == MoveEffectTargetType.Battlefield)
                {
                    return true;
                }
            }
            // stat stage mods
            else if (effect_ is PBS.Databases.Effects.Moves.StatStageMod)
            {
                PBS.Databases.Effects.Moves.StatStageMod effect = effect_ as PBS.Databases.Effects.Moves.StatStageMod;
                if (effect.targetType == MoveEffectTargetType.Self
                    || effect.targetType == MoveEffectTargetType.SelfTeam)
                {
                    if (effect.statStageMod.RaisesAStat())
                    {
                        return true;
                    }
                }
                else if (effect.targetType == MoveEffectTargetType.Target
                    || effect.targetType == MoveEffectTargetType.Team
                    || effect.targetType == MoveEffectTargetType.Battlefield)
                {
                    if (effect.statStageMod.LowersAStat())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Moves: Targeting Methods
        public bool ArePokemonAdjacent(Main.Pokemon.Pokemon pokemon1, Main.Pokemon.Pokemon pokemon2)
        {
            return ArePositionsAdjacent(
                position1: GetPokemonPosition(pokemon1),
                position2: GetPokemonPosition(pokemon2));
        }
        public bool ArePositionsAdjacent(BattlePosition position1, BattlePosition position2)
        {
            Team team1 = GetTeamFromPosition(position1.teamPos);
            Team team2 = GetTeamFromPosition(position2.teamPos);

            // same team
            if (team1 == team2)
            {
                return Mathf.Abs(position1.battlePos - position2.battlePos) == 1;
            }
            // different teams
            else
            {
                // everyone is adjacent in a non-even battle
                if (team1.teamMode != team2.teamMode)
                {
                    return true;
                }
                else
                {
                    return Mathf.Abs(position1.battlePos - position2.battlePos) <= 1;
                }
            }
        }
        public bool ArePositionsAllies(BattlePosition position1, BattlePosition position2)
        {
            return position1.teamPos == position2.teamPos;
        }
        public bool ArePositionsEnemies(BattlePosition position1, BattlePosition position2)
        {
            return position1.teamPos != position2.teamPos;
        }
        public bool IsMoveAdjacentTargeting(Main.Pokemon.Pokemon pokemon, Move moveData, MoveTargetType overwriteTarget = MoveTargetType.None)
        {
            MoveTargetType targetType = moveData.targetType;
            targetType = overwriteTarget == MoveTargetType.None ? targetType : overwriteTarget;

            return targetType == MoveTargetType.Adjacent
                || targetType == MoveTargetType.AdjacentAlly
                || targetType == MoveTargetType.AdjacentOpponent
                || targetType == MoveTargetType.AllAdjacent
                || targetType == MoveTargetType.AllAdjacentOpponents
                || targetType == MoveTargetType.AllAllies
                || targetType == MoveTargetType.AllAlliesButUser
                || targetType == MoveTargetType.AllButUser
                || targetType == MoveTargetType.AllPokemon
                || targetType == MoveTargetType.Any;
        }
        public bool IsMoveSelfTargeting(Main.Pokemon.Pokemon pokemon, Move moveData, MoveTargetType overwriteTarget = MoveTargetType.None)
        {
            MoveTargetType targetType = moveData.targetType;
            targetType = overwriteTarget == MoveTargetType.None ? targetType : overwriteTarget;

            return targetType == MoveTargetType.Self
                || targetType == MoveTargetType.SelfOrAdjacentAlly
                || targetType == MoveTargetType.AllPokemon
                || targetType == MoveTargetType.AllAllies
                || targetType == MoveTargetType.TeamAlly;
        }
        public bool IsMoveAllyTargeting(Main.Pokemon.Pokemon pokemon, Move moveData, MoveTargetType overwriteTarget = MoveTargetType.None)
        {
            MoveTargetType targetType = moveData.targetType;
            targetType = overwriteTarget == MoveTargetType.None ? targetType : overwriteTarget;

            return targetType == MoveTargetType.AdjacentAlly
                || targetType == MoveTargetType.SelfOrAdjacentAlly
                || targetType == MoveTargetType.AllAdjacent
                || targetType == MoveTargetType.AllAllies
                || targetType == MoveTargetType.AllAlliesButUser
                || targetType == MoveTargetType.AllButUser
                || targetType == MoveTargetType.AllPokemon
                || targetType == MoveTargetType.TeamAlly
                || targetType == MoveTargetType.Adjacent
                || targetType == MoveTargetType.Any;
        }
        public bool IsMoveOpponentTargeting(Main.Pokemon.Pokemon pokemon, Move moveData, MoveTargetType overwriteTarget = MoveTargetType.None)
        {
            MoveTargetType targetType = moveData.targetType;
            targetType = overwriteTarget == MoveTargetType.None ? targetType : overwriteTarget;

            return targetType == MoveTargetType.AdjacentOpponent
                || targetType == MoveTargetType.AllAdjacentOpponents
                || targetType == MoveTargetType.AllAdjacent
                || targetType == MoveTargetType.AllOpponents
                || targetType == MoveTargetType.AllPokemon
                || targetType == MoveTargetType.TeamOpponent
                || targetType == MoveTargetType.Adjacent
                || targetType == MoveTargetType.Any;
        }
        public bool IsMoveMultiTargeting(Main.Pokemon.Pokemon pokemon, Move moveData, MoveTargetType overwriteTarget = MoveTargetType.None)
        {
            MoveTargetType targetType = moveData.targetType;
            targetType = overwriteTarget == MoveTargetType.None ? targetType : overwriteTarget;

            return targetType == MoveTargetType.AllAdjacent
                || targetType == MoveTargetType.AllAdjacentOpponents
                || targetType == MoveTargetType.AllAlliesButUser
                || targetType == MoveTargetType.AllAllies
                || targetType == MoveTargetType.AllOpponents
                || targetType == MoveTargetType.AllButUser
                || targetType == MoveTargetType.AllPokemon
                || targetType == MoveTargetType.TeamAlly
                || targetType == MoveTargetType.TeamOpponent
                || targetType == MoveTargetType.Battlefield;
        }
        public bool IsMoveFieldTargeting(Main.Pokemon.Pokemon pokemon, Move moveData, MoveTargetType overwriteTarget = MoveTargetType.None)
        {
            MoveTargetType targetType = moveData.targetType;
            targetType = overwriteTarget == MoveTargetType.None ? targetType : overwriteTarget;

            return targetType == MoveTargetType.TeamAlly
                || targetType == MoveTargetType.TeamOpponent
                || targetType == MoveTargetType.Battlefield;
        }
        public bool IsMoveSideTargeting(Main.Pokemon.Pokemon pokemon, Move moveData, MoveTargetType overwriteTarget = MoveTargetType.None)
        {
            MoveTargetType targetType = moveData.targetType;
            targetType = overwriteTarget == MoveTargetType.None ? targetType : overwriteTarget;

            return targetType == MoveTargetType.TeamAlly
                || targetType == MoveTargetType.TeamOpponent;
        }
        public List<List<BattlePosition>> GetMovePossibleTargets(
            Main.Pokemon.Pokemon pokemon,
            Move moveData,
            MoveTargetType overwriteTarget = MoveTargetType.None)
        {
            List<List<BattlePosition>> targetingPositions = new List<List<BattlePosition>>();
            List<BattlePosition> allPositions = GetAllBattlePositions();

            MoveTargetType targetType = moveData.targetType;
            targetType = overwriteTarget == MoveTargetType.None ? targetType : overwriteTarget;

            bool mustBeAdjacent = IsMoveAdjacentTargeting(pokemon, moveData, overwriteTarget);
            bool canTargetSelf = IsMoveSelfTargeting(pokemon, moveData, overwriteTarget);
            bool canTargetAlly = IsMoveAllyTargeting(pokemon, moveData, overwriteTarget);
            bool canTargetOpponent = IsMoveOpponentTargeting(pokemon, moveData, overwriteTarget);
            bool isMultiTargeting = IsMoveMultiTargeting(pokemon, moveData, overwriteTarget);
            List<BattlePosition> multiTargetPositions = new List<BattlePosition>();

            BattlePosition pokemonPosition = new BattlePosition(pokemon);
            for (int i = 0; i < allPositions.Count; i++)
            {
                BattlePosition position = allPositions[i];

                // battlefield is always everyone
                if (targetType == MoveTargetType.Battlefield
                    || targetType == MoveTargetType.AllPokemon)
                {
                    multiTargetPositions.Add(position);
                }
                // self-targeting
                else if (canTargetSelf
                    && pokemon.teamPos == position.teamPos
                    && pokemon.battlePos == position.battlePos)
                {
                    if (isMultiTargeting) { multiTargetPositions.Add(position); }
                    else { targetingPositions.Add(new List<BattlePosition> { position }); }

                }
                // ally targeting
                else if (canTargetAlly
                    && pokemon.teamPos == position.teamPos
                    && pokemon.battlePos != position.battlePos)
                {
                    if (!mustBeAdjacent
                        // adjacency means at most 1 step away
                        || ArePositionsAdjacent(position, pokemonPosition))
                    {
                        if (isMultiTargeting) { multiTargetPositions.Add(position); }
                        else { targetingPositions.Add(new List<BattlePosition> { position }); }
                    }
                }
                // opponent targeting
                else if (canTargetOpponent
                    && pokemon.teamPos != position.teamPos)
                {
                    if (!mustBeAdjacent
                        || ArePositionsAdjacent(position, pokemonPosition))
                    {
                        if (isMultiTargeting) { multiTargetPositions.Add(position); }
                        else { targetingPositions.Add(new List<BattlePosition> { position }); }
                    }
                }
            }

            if (isMultiTargeting)
            {
                targetingPositions.Add(multiTargetPositions);
            }

            return targetingPositions;
        }
        public bool DoesBattleAutoTarget()
        {
            return IsSinglesBattle();
        }
        public List<BattlePosition> GetMoveAutoTargets(
            Main.Pokemon.Pokemon pokemon,
            Move moveData,
            BattlePosition biasPosition = null,
            MoveTargetType overwriteTarget = MoveTargetType.None,
            bool filterAlive = true,
            bool useFallBack = false)
        {
            List<List<BattlePosition>> possibleTargets = GetMovePossibleTargets(pokemon, moveData, overwriteTarget);
            List<List<BattlePosition>> filteredTargets;
            BattlePosition rootPosition = new BattlePosition(pokemon);

            // TODO: make special cases for certain moves
            // ex. Heal Pulse / Pollen Puff should prioritize allies

            // seek enemy targets
            if (IsMoveOpponentTargeting(pokemon, moveData, overwriteTarget))
            {
                filteredTargets = FilterPossibleTargets(
                    rootPosition,
                    possibleTargets,
                    filterEnemy: true,
                    biasPosition: biasPosition,
                    filterAlive: filterAlive
                    );
            }
            // seek ally targets
            else if (IsMoveAllyTargeting(pokemon, moveData, overwriteTarget))
            {
                filteredTargets = FilterPossibleTargets(
                    rootPosition,
                    possibleTargets,
                    filterAlly: true,
                    biasPosition: biasPosition,
                    filterAlive: filterAlive
                    );
            }
            // seek self target by default
            else
            {
                filteredTargets = FilterPossibleTargets(
                    rootPosition,
                    possibleTargets,
                    filterSelf: true,
                    biasPosition: biasPosition,
                    filterAlive: filterAlive
                    );
            }

            // if we filter out everything and want to use the fallback, 
            // set the filtered targets to the original possibilities
            if (filteredTargets.Count == 0)
            {
                if (useFallBack)
                {
                    filteredTargets = possibleTargets;
                }
                else
                {
                    return new List<BattlePosition>();
                }
            }

            // return a random filtered target
            return filteredTargets[Random.Range(0, filteredTargets.Count)];
        }
        public List<List<BattlePosition>> FilterPossibleTargets(
            BattlePosition rootPosition,
            List<List<BattlePosition>> possibleTargets,
            bool filterEnemy = false,
            bool filterAlly = false,
            bool filterSelf = false,
            bool filterAlive = false,
            BattlePosition biasPosition = null
            )
        {
            List<List<BattlePosition>> targets = new List<List<BattlePosition>>();
            for (int i = 0; i < possibleTargets.Count; i++)
            {
                bool hasAlly = false;
                bool hasEnemy = false;
                bool hasSelf = false;
                bool hasAlive = false;
                List<BattlePosition> currentTargets = possibleTargets[i];

                for (int k = 0; k < currentTargets.Count; k++)
                {
                    if (ArePositionsAllies(rootPosition, currentTargets[k]))
                    {
                        hasAlly = true;
                        if (rootPosition.battlePos == currentTargets[k].battlePos)
                        {
                            hasSelf = true;
                        }
                    }
                    if (ArePositionsEnemies(rootPosition, currentTargets[k]))
                    {
                        hasEnemy = true;
                    }

                    Main.Pokemon.Pokemon pokemon = GetPokemonAtPosition(currentTargets[k]);
                    if (pokemon != null)
                    {
                        if (!IsPokemonFainted(pokemon))
                        {
                            hasAlive = true;
                        }
                    }
                }
                if (filterEnemy && hasEnemy
                    || filterAlly && hasAlly
                    || filterSelf && hasSelf)
                {
                    if (!filterAlive
                        || filterAlive && hasAlive)
                    {
                        targets.Add(currentTargets);
                    }
                }
            }

            // Bias Pokemon
            if (biasPosition != null)
            {
                List<List<BattlePosition>> biasTargets = new List<List<BattlePosition>>();
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].Contains(biasPosition))
                    {
                        biasTargets.Add(targets[i]);
                    }
                }
                return biasTargets;
            }
            return targets;
        }
        public List<Main.Pokemon.Pokemon> GetMoveTargetsLive(BattleCommand command)
        {
            List<Main.Pokemon.Pokemon> targetPokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < command.targetPositions.Length; i++)
            {
                Main.Pokemon.Pokemon pokemon = GetPokemonAtPosition(command.targetPositions[i]);
                if (pokemon != null)
                {
                    targetPokemon.Add(pokemon);
                }
            }
            return targetPokemon;
        }
        public List<Main.Pokemon.Pokemon> GetTargetsLive(BattlePosition[] battlePositions)
        {
            List<Main.Pokemon.Pokemon> targetPokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < battlePositions.Length; i++)
            {
                Main.Pokemon.Pokemon pokemon = GetPokemonAtPosition(battlePositions[i]);
                if (pokemon != null)
                {
                    targetPokemon.Add(pokemon);
                }
            }
            return targetPokemon;
        }

        // Moves: Damaging Methods
        public int GetPokemonOffensiveStat(
            Main.Pokemon.Pokemon userPokemon,
            Move moveData,
            Main.Pokemon.Pokemon targetPokemon = null,
            MoveCategory overwriteCategory = MoveCategory.None,
            bool criticalHit = false,
            bool bypassAbility = false)
        {
            MoveCategory category = overwriteCategory == MoveCategory.None ? moveData.category : overwriteCategory;

            PokemonStats statToUse = category == MoveCategory.Physical ? PokemonStats.Attack
                : category == MoveCategory.Special ? PokemonStats.SpecialAttack
                    : PokemonStats.Attack; // attack by default

            // Psyshock Offense
            MoveEffect psyshock = moveData.GetEffect(MoveEffectType.PsyshockOffense);
            if (psyshock != null)
            {
                statToUse = PBS.Databases.GameText.GetStatFromString(psyshock.GetString(0));
            }

            // Ignore Stat Changes
            bool applyStatStage = true;
            if (targetPokemon != null)
            {
                // Target has Unaware
                PBS.Databases.Effects.Abilities.AbilityEffect unaware_ =
                    PBPGetAbilityEffect(targetPokemon, AbilityEffectType.Unaware, bypassAbility);
                if (unaware_ != null)
                {
                    PBS.Databases.Effects.Abilities.Unaware unaware = unaware_ as PBS.Databases.Effects.Abilities.Unaware;
                    if (unaware.attackerStatsIgnored.Contains(statToUse))
                    {
                        applyStatStage = false;
                    }
                }
            }

            // Critical Hits negate negative user stat stages
            if (criticalHit && GetPokemonStatStage(userPokemon, statToUse) < 0)
            {
                applyStatStage = false;
            }

            return statToUse == PokemonStats.Attack ? GetPokemonATK(userPokemon, applyStatStage: applyStatStage)
                : statToUse == PokemonStats.Defense ? GetPokemonDEF(userPokemon, applyStatStage: applyStatStage)
                : statToUse == PokemonStats.SpecialAttack ? GetPokemonSPA(userPokemon, applyStatStage: applyStatStage)
                : statToUse == PokemonStats.SpecialDefense ? GetPokemonSPD(userPokemon, applyStatStage: applyStatStage)
                : statToUse == PokemonStats.Speed ? GetPokemonSPE(userPokemon, applyStatStage: applyStatStage)
                : 1;
        }
        public int GetPokemonDefensiveStat(
            Main.Pokemon.Pokemon targetPokemon,
            Move moveData,
            Main.Pokemon.Pokemon userPokemon = null,
            MoveCategory overwriteCategory = MoveCategory.None,
            bool criticalHit = false,
            bool bypassAbility = false)
        {
            MoveCategory category = overwriteCategory == MoveCategory.None ? moveData.category : overwriteCategory;
            PokemonStats statToUse = category == MoveCategory.Physical ? PokemonStats.Defense
                : category == MoveCategory.Special ? PokemonStats.SpecialDefense
                    : PokemonStats.Defense; // attack by default

            // Psyshock?
            MoveEffect psyshock = moveData.GetEffect(MoveEffectType.Psyshock);
            if (psyshock != null)
            {
                statToUse = PBS.Databases.GameText.GetStatFromString(psyshock.GetString(0));
            }

            // Ignore Stat Changes
            bool applyStatStage = true;
            if (userPokemon != null)
            {
                // Attacker has Unaware
                AbilityEffect unaware
                    = PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.UnawareOffense);
                if (unaware != null)
                {
                    List<PokemonStats> ignoredStats = PBS.Databases.GameText.GetStatsFromList(unaware.stringParams);
                    if (ignoredStats.Contains(statToUse))
                    {
                        applyStatStage = false;
                    }
                }
            }
            // Chip Away / Sacred Sword
            MoveEffect chipAway = moveData.GetEffect(MoveEffectType.ChipAway);
            if (chipAway != null)
            {
                List<PokemonStats> ignoredStats = PBS.Databases.GameText.GetStatsFromList(chipAway.stringParams);
                if (ignoredStats.Contains(statToUse))
                {
                    applyStatStage = false;
                }
            }

            // Critical Hits negate postive target stat stages
            if (criticalHit && GetPokemonStatStage(targetPokemon, statToUse) > 0)
            {
                applyStatStage = false;
            }

            return statToUse == PokemonStats.Attack ? GetPokemonATK(targetPokemon, applyStatStage: applyStatStage)
                : statToUse == PokemonStats.Defense ? GetPokemonDEF(targetPokemon, applyStatStage: applyStatStage)
                : statToUse == PokemonStats.SpecialAttack ? GetPokemonSPA(targetPokemon, applyStatStage: applyStatStage)
                : statToUse == PokemonStats.SpecialDefense ? GetPokemonSPD(targetPokemon, applyStatStage: applyStatStage)
                : statToUse == PokemonStats.Speed ? GetPokemonSPE(targetPokemon, applyStatStage: applyStatStage)
                : 1;
        }
        public bool CalculateCriticalHit(Main.Pokemon.Pokemon userPokemon, Main.Pokemon.Pokemon targetPokemon, Move moveData)
        {
            int critStage = 0;
            bool applyCritical = false;

            bool bypassAbility = false;
            // Mold Breaker bypasses ability immunities
            if (!bypassAbility)
            {
                PBS.Databases.Effects.Abilities.AbilityEffect moldBreakerEffect =
                    PBPGetAbilityEffect(userPokemon, AbilityEffectType.MoldBreaker);
                if (moldBreakerEffect != null)
                {
                    bypassAbility = true;
                }

                // Sunsteel Strike bypasses ability immunities
                PBS.Databases.Effects.Moves.MoveEffect effect = moveData.GetEffectNew(MoveEffectType.SunteelStrike);
                if (effect != null)
                {
                    bypassAbility = true;
                }
            }

            // Luck Chant negates critical hits
            Team targetTeam = GetTeam(targetPokemon);
            AbilityEffect infiltratorEffect = PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.Infiltrator);
            for (int i = 0; i < targetTeam.bProps.safeguards.Count; i++)
            {
                Main.Team.BattleProperties.Safeguard safeguard = targetTeam.bProps.safeguards[i];
                Move reflectData = Moves.instance.GetMoveData(safeguard.moveID);
                MoveEffect effect = reflectData.GetEffect(MoveEffectType.Safeguard);
                MoveEffect luckyChantEffect = reflectData.GetEffect(MoveEffectType.SafeguardLuckyChant);

                if (luckyChantEffect != null)
                {
                    if (!effect.GetBool(0) || infiltratorEffect == null)
                    {
                        return false;
                    }
                }
            }

            // Battle.Core.Model Armor negates critical hits
            PBS.Databases.Effects.Abilities.AbilityEffect battleArmor_
                = PBPGetAbilityEffect(targetPokemon, AbilityEffectType.BattleArmor, bypassAbility);
            if (battleArmor_ != null)
            {
                return false;
            }

            // Critical Moves (Stone Edge, Freeze Dry, etc.)
            PBS.Databases.Effects.Moves.MoveEffect karateChop_ = moveData.GetEffectNew(MoveEffectType.KarateChop);
            if (karateChop_ != null)
            {
                PBS.Databases.Effects.Moves.KarateChop karateChop = karateChop_ as PBS.Databases.Effects.Moves.KarateChop;
                critStage += karateChop.criticalBoost;
                if (karateChop.alwaysCritical)
                {
                    applyCritical = true;
                }
            }

            // Super Luck
            PBS.Databases.Effects.Abilities.AbilityEffect superLuck_ = PBPGetAbilityEffect(userPokemon, AbilityEffectType.SuperLuck);
            if (superLuck_ != null)
            {
                PBS.Databases.Effects.Abilities.SuperLuck superLuck = superLuck_ as PBS.Databases.Effects.Abilities.SuperLuck;
                if (DoEffectFiltersPass(
                    filters: superLuck.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    critStage += superLuck.criticalBoost;
                    if (superLuck.alwaysCritical)
                    {
                        applyCritical = true;
                    }
                }
            }

            if (moveData.category != MoveCategory.Status)
            {
                if (critStage == 0)
                {
                    if (Random.value <= 1f / 24)
                    {
                        applyCritical = true;
                    }
                }
                else if (critStage == 1)
                {
                    if (Random.value <= 1f / 8)
                    {
                        applyCritical = true;
                    }
                }
                else if (critStage == 2)
                {
                    if (Random.value <= 1f / 2)
                    {
                        applyCritical = true;
                    }
                }
                // guaranteed critical hit
                else if (critStage > 2)
                {
                    applyCritical = true;
                }
            }

            return applyCritical;
        }

        public int GetDamage(
            PBS.Databases.Effects.General.Damage damage,
            Main.Pokemon.Pokemon targetPokemon,
            Main.Pokemon.Pokemon attackerPokemon = null
            )
        {
            int damageDealt = 0;
            if (attackerPokemon != null)
            {
                attackerPokemon = targetPokemon;
            }

            // Calculated
            if (damage.mode == PBS.Databases.Effects.General.Damage.DamageMode.Calculated)
            {
                int calculatedDamage = Mathf.FloorToInt(GetMoveBaseDamage(
                    userPokemon: attackerPokemon,
                    targetPokemon: targetPokemon,
                    overwriteBasePower: Mathf.FloorToInt(damage.value),

                    overwriteCategory: damage.category
                ));

                return calculatedDamage;
            }
            // Direct HP
            else if (damage.mode == PBS.Databases.Effects.General.Damage.DamageMode.DirectHP)
            {
                return Mathf.FloorToInt(damage.value);
            }
            // Max HP %
            else if (damage.mode == PBS.Databases.Effects.General.Damage.DamageMode.MaxHPPercent)
            {
                return Mathf.FloorToInt(GetPokemonHPByPercent(targetPokemon, damage.value));
            }
            // Innards Out (Remaining HP %)
            else if (damage.mode == PBS.Databases.Effects.General.Damage.DamageMode.InnardsOut)
            {
                return Mathf.FloorToInt(targetPokemon.currentHP * damage.value);
            }
            return damageDealt;
        }
        public int GetHeal(
            PBS.Databases.Effects.General.HealHP heal,
            Main.Pokemon.Pokemon targetPokemon,
            Main.Pokemon.Pokemon healerPokemon = null
            )
        {
            int healedHP = 0;
            // Hit Points
            if (heal.healMode == PBS.Databases.Effects.General.HealHP.HealMode.HitPoints)
            {
                return Mathf.FloorToInt(heal.healValue);
            }
            // Max HP %
            else if (heal.healMode == PBS.Databases.Effects.General.HealHP.HealMode.MaxHPPercent)
            {
                return GetPokemonHPByPercent(targetPokemon, heal.healValue);
            }

            return healedHP;
        }

        public float GetMoveBaseDamage(
            Main.Pokemon.Pokemon userPokemon,
            Main.Pokemon.Pokemon targetPokemon,
            Move moveData = null,
            MoveCategory overwriteCategory = MoveCategory.None,
            int overwriteBasePower = 0,
            bool criticalHit = false,
            bool bypassAbility = false
            )
        {
            int offensiveStat;
            int defensiveStat;

            offensiveStat = GetPokemonOffensiveStat(
                userPokemon: userPokemon,
                moveData: moveData,
                targetPokemon: targetPokemon,
                overwriteCategory: overwriteCategory,
                criticalHit: criticalHit,
                bypassAbility: bypassAbility);

            defensiveStat = GetPokemonDefensiveStat(
                targetPokemon: targetPokemon,
                moveData: moveData,
                userPokemon: userPokemon,
                overwriteCategory: overwriteCategory,
                criticalHit: criticalHit,
                bypassAbility: bypassAbility);

            int basePower = overwriteBasePower != 0 ? overwriteBasePower
                : moveData != null ? moveData.basePower
                    : 1;

            float userWeight = userPokemon.data.weight;
            float targetWeight = targetPokemon.data.weight;

            // Heavy Slam
            MoveEffect heavySlam = moveData.GetEffect(MoveEffectType.HeavySlam);
            if (heavySlam != null)
            {
                if (targetWeight == 0)
                {
                    basePower = 120;
                }
                else if (userWeight == 0)
                {
                    basePower = 40;
                }
                else
                {
                    float relativeWeight = targetWeight / userWeight;
                    basePower = relativeWeight > 0.5f ? 40
                        : relativeWeight > 1f / 3 ? 60
                        : relativeWeight > 1f / 4 ? 80
                        : relativeWeight > 1f / 5 ? 100
                        : 120;
                }
            }

            // Low Kick
            MoveEffect lowKick = moveData.GetEffect(MoveEffectType.LowKick);
            if (lowKick != null)
            {
                basePower = targetWeight < 10 ? 20
                    : targetWeight < 25 ? 40
                    : targetWeight < 50 ? 60
                    : targetWeight < 100 ? 80
                    : targetWeight < 200 ? 100
                    : 120;
            }

            float baseDamage = (2 * userPokemon.level / (float)5 + 2) * basePower * (
                offensiveStat / (float)defensiveStat) / 50 + 2;

            return baseDamage;
        }
        public int GetMoveDamage(
            Main.Pokemon.Pokemon userPokemon,
            Main.Pokemon.Pokemon targetPokemon,
            Move moveData,
            bool criticalHit = false,
            bool bypassAbility = false,
            BattleTypeEffectiveness typeEffectiveness = null,
            float presetMultipliers = 1f)
        {
            Team userTeam = GetTeam(userPokemon);
            Team targetTeam = GetTeam(targetPokemon);
            List<Main.Pokemon.Pokemon> onFieldEnemyPokemon = GetAllyPokemon(targetPokemon);
            Data.ElementalType moveTypeData = Databases.ElementalTypes.instance.GetTypeData(moveData.moveType);
            string moveType = moveData.moveType;

            // get base move damage
            float baseDamage = GetMoveBaseDamage(
                userPokemon: userPokemon,
                targetPokemon: targetPokemon,
                moveData: moveData,
                criticalHit: criticalHit,
                bypassAbility: bypassAbility
                );

            // type effectivenesss
            typeEffectiveness = typeEffectiveness == null ? GetMoveEffectiveness(userPokemon, moveData, targetPokemon)
                : typeEffectiveness;
            float effectivenessFactor = typeEffectiveness.GetTotalEffectiveness();

            // critical hit
            float criticalFactor = criticalHit ? GameSettings.btlCriticalHitMultiplier : 1f;
            if (criticalHit)
            {
                // Sniper
                List<PBS.Databases.Effects.Abilities.AbilityEffect> sniper_ =
                    PBPGetAbilityEffects(userPokemon, AbilityEffectType.Sniper);
                for (int i = 0; i < sniper_.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.Sniper sniper = sniper_[i] as PBS.Databases.Effects.Abilities.Sniper;
                    if (DoEffectFiltersPass(
                        filters: sniper.filters,
                        userPokemon: userPokemon,
                        targetPokemon: targetPokemon,
                        moveData: moveData
                        ))
                    {
                        criticalFactor *= sniper.criticalBoost;
                    }
                }
            }

            // damage multiplier base
            float dmgMultiplier = 1f;

            // ---TYPE MODIFIERS (USER)---

            // STAB
            List<string> userTypes = PBPGetTypes(userPokemon);
            PBS.Databases.Effects.Abilities.AbilityEffect adaptability_
                = PBPGetAbilityEffect(userPokemon, AbilityEffectType.Adaptability);
            for (int i = 0; i < userTypes.Count; i++)
            {
                if (moveData.moveType == userTypes[i])
                {
                    // Adaptability
                    if (adaptability_ != null)
                    {
                        PBS.Databases.Effects.Abilities.Adaptability adaptability =
                            adaptability_ as PBS.Databases.Effects.Abilities.Adaptability;
                        dmgMultiplier *= adaptability.STABMultiplier;
                    }
                    else
                    {
                        dmgMultiplier *= GameSettings.btlSTABMultiplier;
                    }
                }
            }

            // ---MOVE MODIFIERS (USER)---

            // Damage Multipliers (General)
            List<PBS.Databases.Effects.Moves.MoveEffect> dmgMultipliers_ = moveData.GetEffectsNew(MoveEffectType.DamageMultiplier);
            if (dmgMultipliers_.Count > 0)
            {
                for (int i = 0; i < dmgMultipliers_.Count; i++)
                {
                    PBS.Databases.Effects.Moves.DamageMultiplier dmgMultiplierEff =
                        dmgMultipliers_[i] as PBS.Databases.Effects.Moves.DamageMultiplier;
                    if (DoesMoveEffectFiltersPass(
                            effect: dmgMultiplierEff,
                            moveData: moveData,
                            userPokemon: userPokemon,
                            targetPokemon: targetPokemon
                            ))
                    {
                        dmgMultiplier *= dmgMultiplierEff.damageScale;
                    }
                }
            }

            // Helping Hand
            if (!userPokemon.bProps.actedThisTurn)
            {
                for (int i = 0; i < userPokemon.bProps.helpingHandMoves.Count; i++)
                {
                    Move helpData = Moves.instance.GetMoveData(userPokemon.bProps.helpingHandMoves[i]);
                    MoveEffect effect = helpData.GetEffect(MoveEffectType.HelpingHand);
                    dmgMultiplier *= effect.GetFloat(0);
                }
            }

            // Me First
            if (userPokemon.bProps.usingMeFirst)
            {
                dmgMultiplier *= 1.5f;
            }

            // Minimize
            if (targetPokemon.bProps.isMinimizeActive)
            {
                MoveEffect minimizeEffect = moveData.GetEffect(MoveEffectType.Minimize);
                if (minimizeEffect != null)
                {
                    dmgMultiplier *= minimizeEffect.GetFloat(0);
                }
            }

            // ---ABILITY MODIFIERS (USER)---
            float abilityMultiplier = 1f;

            // Dark Aura / Aura Break
            float darkAuraMultiplier = 1f;
            HashSet<string> darkAuraAbilities = new HashSet<string>();
            Main.Pokemon.Pokemon auraBreakUser = PBPGetPokemonWithAbilityEffect(AbilityEffectType.AuraBreak);
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                Data.Ability abilityData = PBPGetAbilityDataWithEffect(pokemonOnField[i], AbilityEffectType.DarkAura);
                if (abilityData != null)
                {
                    if (!darkAuraAbilities.Contains(abilityData.ID))
                    {
                        darkAuraAbilities.Add(abilityData.ID);
                        List<PBS.Databases.Effects.Abilities.AbilityEffect> darkAuras_ =
                            abilityData.GetEffectsNew(AbilityEffectType.DarkAura);
                        for (int k = 0; k < darkAuras_.Count; k++)
                        {
                            PBS.Databases.Effects.Abilities.DarkAura darkAura =
                                darkAuras_[k] as PBS.Databases.Effects.Abilities.DarkAura;
                            if (DoEffectFiltersPass(
                                filters: darkAura.filters,
                                userPokemon: userPokemon,
                                targetPokemon: targetPokemon,
                                targetTeam: targetTeam,
                                moveData: moveData,
                                abilityData: abilityData
                                ))
                            {
                                if (auraBreakUser == null)
                                {
                                    darkAuraMultiplier *= darkAura.damageMultiplier;
                                }
                                else
                                {
                                    darkAuraMultiplier /= darkAura.damageMultiplier;
                                }
                            }
                        }
                    }
                }
            }
            abilityMultiplier *= darkAuraMultiplier;

            // Neuroforce / Tinted Lens
            List<PBS.Databases.Effects.Abilities.AbilityEffect> tintedLens_ = PBPGetAbilityEffects(
                pokemon: userPokemon,
                effectType: AbilityEffectType.TintedLens
                );
            for (int i = 0; i < tintedLens_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.TintedLens tintedLens = tintedLens_[i] as PBS.Databases.Effects.Abilities.TintedLens;
                if (DoEffectFiltersPass(
                    filters: tintedLens.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    if (effectivenessFactor > 1f)
                    {
                        abilityMultiplier *= tintedLens.neuroforceModifier;
                    }
                    else if (effectivenessFactor < 1f)
                    {
                        abilityMultiplier *= tintedLens.notVeryEffectiveModifier;
                    }
                }
            }

            // Reckless
            AbilityEffect recklessEffect = PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.Reckless);
            if (recklessEffect != null
                && moveData.GetEffect(MoveEffectType.Struggle) == null)
            {
                if (moveData.GetEffect(MoveEffectType.Recoil) != null
                    || moveData.GetEffect(MoveEffectType.JumpKick) != null)
                {
                    dmgMultiplier *= recklessEffect.GetFloat(0);
                }
            }

            // Strong Jaw
            AbilityEffect strongJawEffect = PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.StrongJaw);
            if (strongJawEffect != null)
            {
                if (moveData.HasTag(MoveTag.BiteMove))
                {
                    dmgMultiplier *= strongJawEffect.GetFloat(0);
                }
            }

            dmgMultiplier *= abilityMultiplier;
            // ---ITEM MODIFIERS (USER)---
            float userItemMultipliers = 1f;

            // Move-Type modifying items (ex. Charcoal, Magnet, Plates)
            List<ItemEffect> charcoalEffects = GetPokemonItemEffects(userPokemon, ItemEffectType.Charcoal);
            for (int i = 0; i < charcoalEffects.Count; i++)
            {
                List<string> affectedTypes = new List<string>(charcoalEffects[i].stringParams);
                for (int k = 0; k < affectedTypes.Count; k++)
                {
                    if (moveTypeData.ID == affectedTypes[k]
                        || moveTypeData.IsABaseID(affectedTypes[k]))
                    {
                        Debug.Log("DEBUG - Charcoal Mod: " + charcoalEffects[i].GetFloat(0));
                        userItemMultipliers *= charcoalEffects[i].GetFloat(0);
                        break;
                    }
                }
            }

            dmgMultiplier *= userItemMultipliers;

            // ---MOVE MODIFIERS (TARGET)---

            // semi-invulnerable states
            float semiInvulnerableMultiplier = 1f;
            if (targetPokemon.bProps.inDigState)
            {
                // Miss semi-invulnerable state
                MoveEffect hitEffect = moveData.GetEffect(MoveEffectType.DmgDigState);
                if (hitEffect != null)
                {
                    semiInvulnerableMultiplier = hitEffect.GetFloat(0);
                }
            }
            if (targetPokemon.bProps.inDiveState)
            {
                // Miss semi-invulnerable state
                MoveEffect hitEffect = moveData.GetEffect(MoveEffectType.DmgDiveState);
                if (hitEffect == null)
                {
                    semiInvulnerableMultiplier = hitEffect.GetFloat(0);
                }
            }
            if (targetPokemon.bProps.inFlyState)
            {
                // Miss semi-invulnerable state
                MoveEffect hitEffect = moveData.GetEffect(MoveEffectType.DmgFlyState);
                if (hitEffect == null)
                {
                    semiInvulnerableMultiplier = hitEffect.GetFloat(0);
                }
            }
            if (targetPokemon.bProps.inShadowForceState)
            {
                // Miss semi-invulnerable state
                MoveEffect hitEffect = moveData.GetEffect(MoveEffectType.DmgShadowForceState);
                if (hitEffect == null)
                {
                    semiInvulnerableMultiplier = hitEffect.GetFloat(0);
                }
            }

            dmgMultiplier *= semiInvulnerableMultiplier;

            // ---ABILITY MODIFIERS (TARGET)---
            float targetAbilityMultiplier = 1f;

            // Friend Guard
            for (int i = 0; i < onFieldEnemyPokemon.Count; i++)
            {
                List<PBS.Databases.Effects.Abilities.AbilityEffect> friendGuards_ = PBPGetAbilityEffects(
                    pokemon: onFieldEnemyPokemon[i],
                    effectType: AbilityEffectType.FriendGuard,
                    bypassAbility: bypassAbility
                    );
                for (int k = 0; k < friendGuards_.Count; k++)
                {
                    PBS.Databases.Effects.Abilities.FriendGuard friendGuard =
                        friendGuards_[i] as PBS.Databases.Effects.Abilities.FriendGuard;
                    if (DoEffectFiltersPass(
                        filters: friendGuard.filters,
                        userPokemon: onFieldEnemyPokemon[i],
                        targetPokemon: targetPokemon,
                        moveData: moveData
                        ))
                    {
                        targetAbilityMultiplier *= friendGuard.damageModifier;
                    }
                }
            }

            // Ice Scales
            List<PBS.Databases.Effects.Abilities.AbilityEffect> iceScales_ =
                PBPGetAbilityEffects(
                    pokemon: targetPokemon,
                    effectType: AbilityEffectType.IceScales,
                    bypassAbility: bypassAbility);
            for (int i = 0; i < iceScales_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.IceScales iceScales = iceScales_[i] as PBS.Databases.Effects.Abilities.IceScales;
                if (DoEffectFiltersPass(
                    filters: iceScales.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    bool applyEffect = true;

                    // Category
                    if (applyEffect
                        && iceScales.useCategory
                        && iceScales.category != moveData.category)
                    {
                        applyEffect = false;
                    }

                    // Tag
                    if (applyEffect
                        && iceScales.tags.Count > 0)
                    {
                        applyEffect = false;
                        List<MoveTag> moveTags = new List<MoveTag>(moveData.moveTags);
                        for (int k = 0; k < moveTags.Count && !applyEffect; k++)
                        {
                            if (iceScales.tags.Contains(moveTags[k])
                                    && IsMoveTagSatisfied(userPokemon, moveData, moveTags[k]))
                            {
                                applyEffect = true;
                            }
                        }
                    }

                    if (applyEffect)
                    {
                        targetAbilityMultiplier *= iceScales.damageModifier;
                    }
                }
            }

            // Multiscale
            List<PBS.Databases.Effects.Abilities.AbilityEffect> multiscale_ =
                PBPGetAbilityEffects(pokemon: targetPokemon,
                effectType: AbilityEffectType.Multiscale,
                bypassAbility: bypassAbility);
            for (int i = 0; i < multiscale_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.Multiscale multiscale =
                    multiscale_[i] as PBS.Databases.Effects.Abilities.Multiscale;
                if (DoEffectFiltersPass(
                    filters: multiscale.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    if (GetPokemonHPAsPercentage(targetPokemon) >= multiscale.hpThreshold)
                    {
                        targetAbilityMultiplier *= multiscale.damageModifier;
                    }
                }
            }

            // Solid Rock / Filter / Prism Armor
            List<PBS.Databases.Effects.Abilities.AbilityEffect> solidRocks_ =
                PBPGetAbilityEffects(
                    pokemon: targetPokemon,
                    effectType: AbilityEffectType.SolidRock,
                    bypassAbility: bypassAbility);
            for (int i = 0; i < solidRocks_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.SolidRock solidRock = solidRocks_[i] as PBS.Databases.Effects.Abilities.SolidRock;
                if (DoEffectFiltersPass(
                    filters: solidRock.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    if (effectivenessFactor > 1f)
                    {
                        targetAbilityMultiplier *= solidRock.superEffectiveModifier;
                    }
                    else if (effectivenessFactor < 1f)
                    {
                        targetAbilityMultiplier *= solidRock.notVeryEffectiveModifier;
                    }
                }
            }

            dmgMultiplier *= targetAbilityMultiplier;

            // ---TEAM MODIFIERS (TARGET)---

            // Different team check
            if (userTeam != targetTeam)
            {
                bool bypassScreens = false;
                List<PBS.Databases.Effects.Abilities.AbilityEffect> infiltrator_ =
                    PBPGetAbilityEffects(userPokemon, AbilityEffectType.Infiltrator);
                for (int i = 0; i < infiltrator_.Count && !bypassScreens; i++)
                {
                    PBS.Databases.Effects.Abilities.Infiltrator infiltrator =
                        infiltrator_[i] as PBS.Databases.Effects.Abilities.Infiltrator;
                    if (infiltrator.bypassScreens)
                    {
                        bypassScreens = true;
                    }
                }

                // Reflect / Light Screen / Aurora Veil
                AbilityEffect infiltratorEffect = PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.Infiltrator);
                float reflectMultiplier = 1f;
                for (int i = 0; i < targetTeam.bProps.lightScreens.Count; i++)
                {
                    TeamCondition lightScreenCondition = targetTeam.bProps.lightScreens[i];
                    if (TBPIsPokemonAffectedByTS(targetPokemon, lightScreenCondition.data))
                    {
                        List<PBS.Databases.Effects.TeamStatuses.TeamSE> lightScreen_ =
                            lightScreenCondition.data.GetEffectsNew(TeamSEType.LightScreen);
                        for (int k = 0; k < lightScreen_.Count; k++)
                        {
                            PBS.Databases.Effects.TeamStatuses.LightScreen lightScreen =
                                lightScreen_[k] as PBS.Databases.Effects.TeamStatuses.LightScreen;
                            if (DoEffectFiltersPass(
                                filters: lightScreen.filters,
                                userPokemon: userPokemon,
                                targetPokemon: targetPokemon,
                                targetTeam: targetTeam,
                                moveData: moveData
                                )
                                && (!lightScreen.canBeInfiltrated || !bypassScreens))
                            {
                                float multiplier = onFieldEnemyPokemon.Count > 0 ? lightScreen.damageMultiMultiplier
                                    : lightScreen.damageMultiplier;
                                reflectMultiplier *= multiplier;
                            }
                        }
                    }
                }
                dmgMultiplier *= reflectMultiplier;
            }

            // ---BATTLE MODIFIERS---
            List<BattleCondition> battleConditions = BBPGetSCs();

            // Terrain / Weather Type Multiplier
            float battleModifiers = 1f;
            for (int i = 0; i < battleConditions.Count; i++)
            {
                // Type Damage Modifiers
                List<PBS.Databases.Effects.BattleStatuses.BattleSE> typeDamageModifers_ =
                        battleConditions[i].data.GetEffectsNew(BattleSEType.TypeDamageModifier);
                for (int k = 0; k < typeDamageModifers_.Count; k++)
                {
                    PBS.Databases.Effects.BattleStatuses.TypeDamageModifier typeDamageModifer =
                        typeDamageModifers_[k] as PBS.Databases.Effects.BattleStatuses.TypeDamageModifier;
                    bool proceed = true;
                    if (proceed && typeDamageModifer.offensiveCheck)
                    {
                        if (!BBPIsPokemonAffectedByBS(userPokemon, battleConditions[i].data))
                        {
                            proceed = false;
                        }
                    }
                    if (proceed && typeDamageModifer.defensiveCheck)
                    {
                        if (!BBPIsPokemonAffectedByBS(targetPokemon, battleConditions[i].data))
                        {
                            proceed = false;
                        }
                    }
                    if (proceed)
                    {
                        if (DoesBattleEFiltersPass(
                            effect: typeDamageModifer,
                            userPokemon: userPokemon,
                            targetPokemon: targetPokemon
                            ))
                        {
                            bool foundType = false;
                            for (int j = 0; j < typeDamageModifer.types.Count; j++)
                            {
                                if (moveTypeData.ID == typeDamageModifer.types[j]
                                    || moveTypeData.IsABaseID(typeDamageModifer.types[j]))
                                {
                                    foundType = true;
                                    break;
                                }
                            }

                            if (typeDamageModifer.invert)
                            {
                                foundType = !foundType;
                            }
                            if (foundType)
                            {
                                battleModifiers *= typeDamageModifer.damageScale;
                            }
                        }
                    }
                }

                // Move Damage Modifiers
                List<PBS.Databases.Effects.BattleStatuses.BattleSE> moveDamageModifers_ =
                    battleConditions[i].data.GetEffectsNew(BattleSEType.MoveDamageModifier);
                for (int k = 0; k < moveDamageModifers_.Count; k++)
                {
                    PBS.Databases.Effects.BattleStatuses.MoveDamageModifier moveDamageModifer =
                        moveDamageModifers_[k] as PBS.Databases.Effects.BattleStatuses.MoveDamageModifier;
                    bool proceed = true;
                    if (proceed && moveDamageModifer.offensiveCheck)
                    {
                        if (!BBPIsPokemonAffectedByBS(userPokemon, battleConditions[i].data))
                        {
                            proceed = false;
                        }
                    }
                    if (proceed && moveDamageModifer.defensiveCheck)
                    {
                        if (!BBPIsPokemonAffectedByBS(targetPokemon, battleConditions[i].data))
                        {
                            proceed = false;
                        }
                    }
                    if (proceed)
                    {
                        if (DoesBattleEFiltersPass(
                            effect: moveDamageModifer,
                            userPokemon: userPokemon,
                            targetPokemon: targetPokemon
                            ))
                        {
                            bool foundType = false;
                            for (int j = 0; j < moveDamageModifer.moves.Count; j++)
                            {
                                if (moveData.ID == moveDamageModifer.moves[j]
                                    || moveData.IsABaseID(moveDamageModifer.moves[j]))
                                {
                                    foundType = true;
                                    break;
                                }
                            }

                            if (foundType)
                            {
                                battleModifiers *= moveDamageModifer.damageScale;
                            }
                        }
                    }
                }
            }
            dmgMultiplier *= battleModifiers;

            // random
            float randomFactor = Random.Range(85, 101) / (float)100;

            float totalDamage = baseDamage
                * effectivenessFactor
                * criticalFactor
                * dmgMultiplier
                * randomFactor
                * presetMultipliers;
            return Mathf.Max(1, Mathf.FloorToInt(totalDamage));
        }
        public bool DoesMoveOverrideDamage(Main.Pokemon.Pokemon userPokemon, Move moveData)
        {
            // Bide
            MoveEffect bideEffect = moveData.GetEffect(MoveEffectType.Bide);
            if (bideEffect != null)
            {
                return true;
            }

            // Counter / Mirror Coat
            MoveEffect counterEffect = moveData.GetEffect(MoveEffectType.Counter);
            if (counterEffect != null)
            {
                return true;
            }

            // Endeavor
            MoveEffect endeavorEffect = moveData.GetEffect(MoveEffectType.Endeavor);
            if (endeavorEffect != null)
            {
                return true;
            }

            // One-Hit KO: Guillotine, Sheer Cold, etc.
            PBS.Databases.Effects.Moves.MoveEffect guillotine_ = moveData.GetEffectNew(MoveEffectType.Guillotine);
            if (guillotine_ != null)
            {
                return true;
            }

            // Level: Night Shade, Seismic Toss
            PBS.Databases.Effects.Moves.MoveEffect seismicToss_ = moveData.GetEffectNew(MoveEffectType.SeismicToss);
            if (seismicToss_ != null)
            {
                return true;
            }

            // Set-Damage: Dragon Rage, Sonic Boom
            PBS.Databases.Effects.Moves.MoveEffect dragonRage_ = moveData.GetEffectNew(MoveEffectType.DragonRage);
            if (dragonRage_ != null)
            {
                return true;
            }

            // Percentage Damage: Super Fang, Guardian of Alola
            PBS.Databases.Effects.Moves.MoveEffect superFang_ = moveData.GetEffectNew(MoveEffectType.SuperFang);
            if (superFang_ != null)
            {
                return true;
            }

            // Psywave
            PBS.Databases.Effects.Moves.MoveEffect psywave_ = moveData.GetEffectNew(MoveEffectType.Psywave);
            if (psywave_ != null)
            {
                return true;
            }

            return false;
        }
        public int GetMoveOverrideDamage(Main.Pokemon.Pokemon userPokemon, Main.Pokemon.Pokemon targetPokemon, Move moveData)
        {
            // Bide
            MoveEffect bideEffect = moveData.GetEffect(MoveEffectType.Bide);
            if (bideEffect != null)
            {
                return Mathf.FloorToInt(userPokemon.bProps.bideDamageTaken * bideEffect.GetFloat(0));
            }

            // Counter / Mirror Coat
            MoveEffect counterEffect = moveData.GetEffect(MoveEffectType.Counter);
            if (counterEffect != null)
            {
                return Mathf.FloorToInt(counterEffect.GetFloat(0)
                    * (counterEffect.GetBool(0) ? userPokemon.bProps.turnPhysicalDamageTaken
                        : userPokemon.bProps.turnSpecialDamageTaken));
            }

            // Endeavor
            MoveEffect endeavorEffect = moveData.GetEffect(MoveEffectType.Endeavor);
            if (endeavorEffect != null)
            {
                int damage = targetPokemon.currentHP - userPokemon.currentHP;
                return Mathf.Max(0, damage);
            }

            // One-Hit KO: Guillotine, Sheer Cold, etc.
            PBS.Databases.Effects.Moves.MoveEffect guillotine_ = moveData.GetEffectNew(MoveEffectType.Guillotine);
            if (guillotine_ != null)
            {
                return targetPokemon.maxHP;
            }

            // Level: Night Shade, Seismic Toss
            PBS.Databases.Effects.Moves.MoveEffect seismicToss_ = moveData.GetEffectNew(MoveEffectType.SeismicToss);
            if (seismicToss_ != null)
            {
                PBS.Databases.Effects.Moves.SeismicToss seismicToss = seismicToss_ as PBS.Databases.Effects.Moves.SeismicToss;
                return userPokemon.level;
            }

            // Set-Damage: Dragon Rage, Sonic Boom
            PBS.Databases.Effects.Moves.MoveEffect dragonRage_ = moveData.GetEffectNew(MoveEffectType.DragonRage);
            if (dragonRage_ != null)
            {
                PBS.Databases.Effects.Moves.DragonRage dragonRage = dragonRage_ as PBS.Databases.Effects.Moves.DragonRage;
                return dragonRage.damage;
            }

            // Percentage Damage: Super Fang, Guardian of Alola
            PBS.Databases.Effects.Moves.MoveEffect superFang_ = moveData.GetEffectNew(MoveEffectType.SuperFang);
            if (superFang_ != null)
            {
                PBS.Databases.Effects.Moves.SuperFang superFang = superFang_ as PBS.Databases.Effects.Moves.SuperFang;
                return Mathf.FloorToInt(targetPokemon.currentHP * superFang.damagePercent);
            }

            // Psywave
            PBS.Databases.Effects.Moves.MoveEffect psywave_ = moveData.GetEffectNew(MoveEffectType.Psywave);
            if (psywave_ != null)
            {
                PBS.Databases.Effects.Moves.Psywave psywave = psywave_ as PBS.Databases.Effects.Moves.Psywave;
                float numerator = Random.Range(0, GameSettings.pkmnMaxLevel + 1) + psywave.lowestScaleValue;
                float denominator = GameSettings.pkmnMaxLevel;
                float damage = userPokemon.level * (numerator / denominator);
                return Mathf.Max(1, Mathf.FloorToInt(damage));
            }

            return 0;
        }

        // Moves: Accuracy Methods
        public float GetMoveAccuracy(
            Main.Pokemon.Pokemon userPokemon,
            Main.Pokemon.Pokemon targetPokemon,
            Move moveData,
            bool bypassTraditionalCheck = false)
        {
            float accuracy = bypassTraditionalCheck ? -1 : moveData.accuracy;

            // Guarantee move hit for field-targeting moves
            if (IsMoveFieldTargeting(userPokemon, moveData))
            {
                return -1;
            }

            bool bypassAbility = false;
            // Mold Breaker bypasses ability immunities
            if (!bypassAbility)
            {
                PBS.Databases.Effects.Abilities.AbilityEffect moldBreakerEffect =
                    PBPGetAbilityEffect(userPokemon, AbilityEffectType.MoldBreaker);
                if (moldBreakerEffect != null)
                {
                    bypassAbility = true;
                }

                // Sunsteel Strike bypasses ability immunities
                PBS.Databases.Effects.Moves.MoveEffect effect = moveData.GetEffectNew(MoveEffectType.SunteelStrike);
                if (effect != null)
                {
                    bypassAbility = true;
                }
            }

            // Lock-On bypasses checks
            if (IsPokemonLockedOn(userPokemon, targetPokemon))
            {
                return -1;
            }

            // No Guard bypasses checks

            // attacker
            bool noGuardActive = false;
            List<PBS.Databases.Effects.Abilities.AbilityEffect> attackerNoGuard_ =
                PBPGetAbilityEffects(userPokemon, AbilityEffectType.NoGuard);
            for (int i = 0; i < attackerNoGuard_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.NoGuard noGuard = attackerNoGuard_[i] as PBS.Databases.Effects.Abilities.NoGuard;
                if (DoEffectFiltersPass(
                    filters: noGuard.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    if (noGuard.bypassWhenAttacking)
                    {
                        noGuardActive = true;
                    }
                }
            }

            // attacked
            List<PBS.Databases.Effects.Abilities.AbilityEffect> attackedNoGuard_ =
                PBPGetAbilityEffects(targetPokemon, AbilityEffectType.NoGuard, bypassAbility: bypassAbility);
            for (int i = 0; i < attackedNoGuard_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.NoGuard noGuard = attackedNoGuard_[i] as PBS.Databases.Effects.Abilities.NoGuard;
                if (DoEffectFiltersPass(
                    filters: noGuard.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    if (noGuard.bypassWhenAttacked)
                    {
                        noGuardActive = true;
                    }
                }
            }

            if (noGuardActive)
            {
                return -1;
            }

            if (PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.NoGuard) != null
                || PBPLegacyGetAbilityEffect(targetPokemon, AbilityEffectType.NoGuard, bypassAbility) != null)
            {
                return -1;
            }

            // Weather accuracy (flat)
            if (!bypassTraditionalCheck)
            {
                List<MoveEffect> weatherEffects = moveData.GetEffects(MoveEffectType.AccuracyInWeather);
                for (int i = 0; i < weatherEffects.Count; i++)
                {
                    for (int k = 0; k < weatherEffects[i].stringParams.Length; k++)
                    {
                        string curWeather = weatherEffects[i].stringParams[k];
                        if (weather.data.ID == curWeather || weather.data.IsABaseID(curWeather))
                        {
                            accuracy = weatherEffects[i].GetFloat(k);
                            break;
                        }
                    }
                }
            }

            // Wonder Skin (flat)
            if (!bypassTraditionalCheck && userPokemon != targetPokemon)
            {
                List<PBS.Databases.Effects.Abilities.AbilityEffect> wonderSkin_ =
                    PBPGetAbilityEffects(
                        pokemon: targetPokemon,
                        effectType: AbilityEffectType.WonderSkin,
                        bypassAbility: bypassAbility);
                for (int i = 0; i < wonderSkin_.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.WonderSkin wonderSkin =
                        wonderSkin_[i] as PBS.Databases.Effects.Abilities.WonderSkin;
                    if (wonderSkin.mode == PBS.Databases.Effects.Abilities.WonderSkin.AccuracyMode.Set)
                    {
                        if (DoEffectFiltersPass(
                            filters: wonderSkin.filters,
                            userPokemon: userPokemon,
                            targetPokemon: targetPokemon,
                            moveData: moveData
                        ))
                        {
                            accuracy = wonderSkin.accuracyValue;
                            break;
                        }
                    }
                }
            }

            // Minimize
            if (targetPokemon.bProps.isMinimizeActive)
            {
                MoveEffect minimizeEffect = moveData.GetEffect(MoveEffectType.Minimize);
                if (minimizeEffect != null)
                {
                    accuracy = -1;
                }
            }

            // Semi-invulnerable states
            if (targetPokemon.bProps.inDigState)
            {
                // Miss semi-invulnerable state
                MoveEffect hitEffect = moveData.GetEffect(MoveEffectType.DmgDigState);
                if (hitEffect == null)
                {
                    accuracy = 0;
                }
                else
                {
                    accuracy = -1;
                }
            }
            if (targetPokemon.bProps.inDiveState)
            {
                // Miss semi-invulnerable state
                MoveEffect hitEffect = moveData.GetEffect(MoveEffectType.DmgDiveState);
                if (hitEffect == null)
                {
                    accuracy = 0;
                }
                else
                {
                    accuracy = -1;
                }
            }
            if (targetPokemon.bProps.inFlyState)
            {
                // Miss semi-invulnerable state
                MoveEffect hitFlyEffect = moveData.GetEffect(MoveEffectType.DmgFlyState);
                if (hitFlyEffect == null)
                {
                    accuracy = 0;
                }
                else
                {
                    accuracy = -1;
                }
            }
            if (targetPokemon.bProps.inShadowForceState)
            {
                // Miss semi-invulnerable state
                MoveEffect hitEffect = moveData.GetEffect(MoveEffectType.DmgShadowForceState);
                if (hitEffect == null)
                {
                    accuracy = 0;
                }
                else
                {
                    accuracy = -1;
                }
            }

            // bypass accuracy checks if we're guaranteed to miss or guaranteed to hit
            if (accuracy > 0)
            {
                bool applyAccuracyStage = true;
                bool applyEvasionStage = true;

                PBS.Databases.Effects.Abilities.AbilityEffect userUnaware_ =
                    PBPGetAbilityEffect(userPokemon, AbilityEffectType.Unaware);
                if (userUnaware_ != null)
                {
                    PBS.Databases.Effects.Abilities.Unaware unaware = userUnaware_ as PBS.Databases.Effects.Abilities.Unaware;
                    if (unaware.targetStatsIgnored.Contains(PokemonStats.Evasion))
                    {
                        applyEvasionStage = false;
                    }
                }

                PBS.Databases.Effects.Abilities.AbilityEffect targetUnaware_ =
                    PBPGetAbilityEffect(targetPokemon, AbilityEffectType.Unaware, bypassAbility);
                if (targetUnaware_ != null)
                {
                    PBS.Databases.Effects.Abilities.Unaware unaware = targetUnaware_ as PBS.Databases.Effects.Abilities.Unaware;
                    if (unaware.targetStatsIgnored.Contains(PokemonStats.Accuracy))
                    {
                        applyAccuracyStage = false;
                    }
                }

                // stat stage changes
                float accuracyMod = GetPokemonStat(
                    pokemon: userPokemon,
                    statType: PokemonStats.Accuracy,
                    applyStatStage: applyAccuracyStage);

                float evasionMod = GetPokemonStat(
                    pokemon: targetPokemon,
                    statType: PokemonStats.Evasion,
                    applyStatStage: applyEvasionStage);

                // Hustle
                List<PBS.Databases.Effects.Abilities.AbilityEffect> hustle_ = PBPGetAbilityEffects(userPokemon, AbilityEffectType.Hustle);
                for (int i = 0; i < hustle_.Count; i++)
                {
                    PBS.Databases.Effects.Abilities.Hustle hustle =
                        hustle_[i] as PBS.Databases.Effects.Abilities.Hustle;
                    if (DoEffectFiltersPass(
                        filters: hustle.filters,
                        userPokemon: userPokemon,
                        targetPokemon: targetPokemon,
                        moveData: moveData
                        ))
                    {
                        bool applyHustle = true;

                        // category
                        if (!applyHustle && hustle.moveCategories.Count > 0)
                        {
                            applyHustle = false;
                            if (hustle.moveCategories.Contains(moveData.category))
                            {
                                applyHustle = true;
                            }
                        }

                        if (applyHustle)
                        {
                            accuracyMod *= hustle.statScale.GetPokemonStatScale(PokemonStats.Accuracy);
                            evasionMod *= hustle.statScale.GetPokemonStatScale(PokemonStats.Evasion);
                        }
                    }


                }

                // Wonder Skin
                if (userPokemon != targetPokemon)
                {
                    List<PBS.Databases.Effects.Abilities.AbilityEffect> wonderSkin_ =
                    PBPGetAbilityEffects(
                        pokemon: targetPokemon,
                        effectType: AbilityEffectType.WonderSkin,
                        bypassAbility: bypassAbility);
                    for (int i = 0; i < wonderSkin_.Count; i++)
                    {
                        PBS.Databases.Effects.Abilities.WonderSkin wonderSkin =
                            wonderSkin_[i] as PBS.Databases.Effects.Abilities.WonderSkin;
                        if (wonderSkin.mode == PBS.Databases.Effects.Abilities.WonderSkin.AccuracyMode.Multiplier)
                        {
                            if (DoEffectFiltersPass(
                                filters: wonderSkin.filters,
                                userPokemon: userPokemon,
                                targetPokemon: targetPokemon,
                                moveData: moveData
                            ))
                            {
                                accuracyMod *= wonderSkin.accuracyValue;
                            }
                        }
                    }
                }

                // Identified
                if (targetPokemon.bProps.identifieds.Count > 0 && evasionMod > 1)
                {
                    evasionMod = 1;
                }

                accuracy *= accuracyMod / evasionMod;
            }

            // One-Hit KO accuracy bypasses traditional checks
            if (accuracy != 0)
            {
                PBS.Databases.Effects.Moves.MoveEffect guillotine_ = moveData.GetEffectNew(MoveEffectType.GuillotineAccuracy);
                if (guillotine_ != null)
                {
                    accuracy = (Mathf.Max(0, userPokemon.level - targetPokemon.level) + accuracy * 100) / 100f;
                }
            }

            return accuracy;
        }

        // Moves: Misc Methods
        public bool IsMultiHitMove(Main.Pokemon.Pokemon userPokemon, Move moveData)
        {
            MoveEffect skyDropEffect = moveData.GetEffect(MoveEffectType.SkyDrop);
            if (skyDropEffect != null)
            {
                // On the charging turn of sky drop, restrict to 1 hit
                if (userPokemon.bProps.attemptingToSkyDrop)
                {
                    return false;
                }
            }

            // Beat Up
            PBS.Databases.Effects.Moves.MoveEffect beatUp_ = moveData.GetEffectNew(MoveEffectType.BeatUp);
            if (beatUp_ != null)
            {
                return true;
            }

            // Double Slap, Icicle Spear, Rock Blast, etc.
            PBS.Databases.Effects.Moves.MoveEffect furyAttack_ = moveData.GetEffectNew(MoveEffectType.FuryAttack);
            if (furyAttack_ != null)
            {
                return true;
            }

            // Double Kick, Dragon Darts
            PBS.Databases.Effects.Moves.MoveEffect doubleKick_ = moveData.GetEffectNew(MoveEffectType.DoubleKick);
            if (doubleKick_ != null)
            {
                return true;
            }

            // Triple Kick
            PBS.Databases.Effects.Moves.MoveEffect tripleKick_ = moveData.GetEffectNew(MoveEffectType.TripleKick);
            if (tripleKick_ != null)
            {
                return true;
            }

            return false;
        }
        public bool IsHealingMove(Move moveData)
        {
            return moveData.GetEffect(MoveEffectType.Recover) != null
                || moveData.GetEffect(MoveEffectType.Rest) != null;
        }
        public int GetMoveHits(Main.Pokemon.Pokemon userPokemon, Move moveData)
        {
            // default
            int hits = 1;

            MoveEffect skyDropEffect = moveData.GetEffect(MoveEffectType.SkyDrop);
            if (skyDropEffect != null)
            {
                // On the charging turn of sky drop, restrict to 1 hit
                if (userPokemon.bProps.attemptingToSkyDrop)
                {
                    return 1;
                }
            }

            // Beat Up
            PBS.Databases.Effects.Moves.MoveEffect beatUp_ = moveData.GetEffectNew(MoveEffectType.BeatUp);
            if (beatUp_ != null)
            {
                PBS.Databases.Effects.Moves.BeatUp beatUp = beatUp_ as PBS.Databases.Effects.Moves.BeatUp;
                Trainer trainer = GetPokemonOwner(userPokemon);
                return GetTrainerAllAvailablePokemon(trainer).Count;
            }

            // Double Slap, Icicle Spear, Rock Blast, etc.
            PBS.Databases.Effects.Moves.MoveEffect furyAttack_ = moveData.GetEffectNew(MoveEffectType.FuryAttack);
            if (furyAttack_ != null)
            {
                PBS.Databases.Effects.Moves.FuryAttack furyAttack = furyAttack_ as PBS.Databases.Effects.Moves.FuryAttack;
                PBS.Databases.Effects.Abilities.AbilityEffect skillLink =
                    PBPGetAbilityEffect(userPokemon, AbilityEffectType.SkillLink);
                if (skillLink != null)
                {
                    int maxHits = furyAttack.lowestHits + furyAttack.hitChances.Count;
                    hits *= maxHits;
                }
                else
                {
                    // Accumulate total hit chances
                    List<float> hitChances = new List<float>();
                    float totalChance = 0;
                    for (int i = 0; i < furyAttack.hitChances.Count; i++)
                    {
                        totalChance += furyAttack.hitChances[i];
                        hitChances.Add(totalChance);
                    }

                    // Normalize hit chances
                    if (totalChance > 0)
                    {
                        for (int i = 0; i < hitChances.Count; i++)
                        {
                            hitChances[i] /= totalChance;
                        }

                        // Calculate hit by chance
                        float randValue = Random.value;
                        for (int i = 0; i < hitChances.Count; i++)
                        {
                            if (randValue <= hitChances[i])
                            {
                                hits *= i + furyAttack.lowestHits;
                                break;
                            }
                        }
                    }
                    else
                    {
                        hits *= furyAttack.lowestHits;
                    }
                }
            }

            // Double Kick, Dragon Darts
            PBS.Databases.Effects.Moves.MoveEffect doubleKick_ = moveData.GetEffectNew(MoveEffectType.DoubleKick);
            if (doubleKick_ != null)
            {
                PBS.Databases.Effects.Moves.DoubleKick doubleKick = doubleKick_ as PBS.Databases.Effects.Moves.DoubleKick;
                hits *= doubleKick.hits;
            }

            // Triple Kick
            PBS.Databases.Effects.Moves.MoveEffect tripleKick_ = moveData.GetEffectNew(MoveEffectType.TripleKick);
            if (tripleKick_ != null)
            {
                PBS.Databases.Effects.Moves.TripleKick tripleKick = tripleKick_ as PBS.Databases.Effects.Moves.TripleKick;
                hits *= tripleKick.hits;
            }

            // filter out negative numbers
            return Mathf.Max(1, hits);
        }
        public int GetPPConsumed(
            Main.Pokemon.Pokemon userPokemon,
            List<Main.Pokemon.Pokemon> targetPokemon,
            Move moveData)
        {
            float PPLost = 1f;

            float PPLostFlat = 0f;
            // Pressure
            for (int i = 0; i < targetPokemon.Count; i++)
            {
                Main.Pokemon.Pokemon pokemon = targetPokemon[i];

                // doesn't affect allies
                if (!ArePokemonAllies(userPokemon, pokemon))
                {
                    List<PBS.Databases.Effects.Abilities.AbilityEffect> pressure_ =
                        PBPGetAbilityEffects(pokemon, AbilityEffectType.Pressure);
                    for (int k = 0; k < pressure_.Count; k++)
                    {
                        PBS.Databases.Effects.Abilities.Pressure pressure =
                            pressure_[k] as PBS.Databases.Effects.Abilities.Pressure;
                        if (DoEffectFiltersPass(
                            filters: pressure.filters,
                            userPokemon: pokemon,
                            targetPokemon: userPokemon,
                            moveData: moveData
                            ))
                        {
                            if (pressure.mode == PBS.Databases.Effects.Abilities.Pressure.DeductionMode.Flat)
                            {
                                PPLostFlat += pressure.ppLoss;
                            }
                            else if (pressure.mode == PBS.Databases.Effects.Abilities.Pressure.DeductionMode.Scale)
                            {
                                PPLost *= pressure.ppLoss;
                            }
                        }
                    }
                }
            }
            PPLost += PPLostFlat;

            return userPokemon.ConsumePP(moveData.ID, Mathf.CeilToInt(PPLost));
        }
        public bool IsMagicCoated(Main.Pokemon.Pokemon userPokemon, List<Main.Pokemon.Pokemon> targetPokemon, Move moveData)
        {
            for (int i = 0; i < targetPokemon.Count; i++)
            {
                bool isCoated = IsMagicCoated(userPokemon, targetPokemon[i], moveData);
                if (isCoated)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsMagicCoated(Main.Pokemon.Pokemon userPokemon, Main.Pokemon.Pokemon targetPokemon, Move moveData)
        {
            // Magic Coatable
            if (moveData.moveTags.Contains(MoveTag.MagicCoatSusceptible))
            {
                // Magic Coat
                // TODO: Mold Breaker
                if (targetPokemon.bProps.isMagicCoatActive)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsMoveReflected(Main.Pokemon.Pokemon userPokemon, List<Main.Pokemon.Pokemon> targetPokemon, Move moveData)
        {
            for (int i = 0; i < targetPokemon.Count; i++)
            {
                bool isReflected = IsMoveReflected(userPokemon, targetPokemon[i], moveData);
                if (isReflected)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsMoveReflected(Main.Pokemon.Pokemon userPokemon, Main.Pokemon.Pokemon targetPokemon, Move moveData)
        {
            // Magic Coat
            if (IsMagicCoated(userPokemon, targetPokemon, moveData))
            {
                return true;
            }

            return false;
        }
        public void SetPokemonLastMove(Main.Pokemon.Pokemon pokemon, string moveID)
        {
            pokemon.bProps.lastMove = moveID;
            // Torment
            for (int i = 0; i < pokemon.bProps.moveLimiters.Count; i++)
            {
                Main.Pokemon.BattleProperties.MoveLimiter limiter
                    = pokemon.bProps.moveLimiters[i];
                if (limiter.effect is PBS.Databases.Effects.PokemonStatuses.Torment)
                {
                    limiter.SetMove(moveID);
                }
            }
        }
        public void SetPokemonLastTargeted(Main.Pokemon.Pokemon pokemon, Main.Pokemon.Pokemon targeterPokemon, string moveID)
        {
            pokemon.bProps.lastMoveTargetedTurns = 2;
            pokemon.bProps.lastMoveTargetedBy = moveID;
            pokemon.bProps.lastTargeterPokemon = targeterPokemon.uniqueID;
        }
        public void UnsetPokemonLastTargeted(Main.Pokemon.Pokemon pokemon)
        {
            pokemon.bProps.lastMoveTargetedBy = null;
            pokemon.bProps.lastTargeterPokemon = null;
            pokemon.bProps.lastMoveTargetedTurns = 0;
        }
        public void ResetPokemonProtect(Main.Pokemon.Pokemon pokemon)
        {
            pokemon.bProps.protectCounter = 0;
        }

        public void SetPokemonThrashMove(Main.Pokemon.Pokemon pokemon, BattleCommand command, int thrashTurns)
        {
            pokemon.bProps.thrashMove = command.moveID;
            pokemon.bProps.thrashTurns = thrashTurns;
            command.consumePP = false;
            pokemon.SetNextCommand(command);
        }
        public void SetPokemonUproarMove(Main.Pokemon.Pokemon pokemon, BattleCommand command, int uproarTurns)
        {
            pokemon.bProps.uproarMove = command.moveID;
            pokemon.bProps.uproarTurns = uproarTurns;
            command.consumePP = false;
            pokemon.SetNextCommand(command);
        }
        public void UnsetPokemonThrashMove(Main.Pokemon.Pokemon pokemon)
        {
            if (pokemon.bProps.thrashMove != null)
            {
                pokemon.bProps.thrashMove = null;
                pokemon.bProps.thrashTurns = -1;
                pokemon.UnsetNextCommand();
            }
        }
        public void UnsetPokemonUproarMove(Main.Pokemon.Pokemon pokemon)
        {
            if (pokemon.bProps.uproarMove != null)
            {
                pokemon.bProps.uproarMove = null;
                pokemon.bProps.uproarTurns = -1;
                pokemon.UnsetNextCommand();
            }
        }
        public void UnsetPokemonBlockMove(Main.Pokemon.Pokemon pokemon)
        {
            pokemon.bProps.blockMove = null;
            pokemon.bProps.blockPokemon = null;
        }
        public void UnsetPokemonBindMove(Main.Pokemon.Pokemon pokemon)
        {
            pokemon.bProps.bindMove = null;
            pokemon.bProps.bindPokemon = null;
            pokemon.bProps.bindTurns = 0;
        }
        public void UnsetPokemonRageMove(Main.Pokemon.Pokemon pokemon)
        {
            pokemon.bProps.rageMove = null;
            pokemon.bProps.rageCounter = 0;
        }

        public bool IsPokemonChoiced(Main.Pokemon.Pokemon pokemon)
        {
            // Gorilla Tactics
            PBS.Databases.Effects.Abilities.AbilityEffect gorillaTactics_ =
                PBPGetAbilityEffect(pokemon, AbilityEffectType.GorillaTactics);
            if (gorillaTactics_ != null)
            {
                return true;
            }

            // Choice Item
            PBS.Databases.Effects.Items.ItemEffect choiceBand_ = PBPGetItemEffect(pokemon, ItemEffectType.ChoiceBand);
            if (choiceBand_ != null)
            {
                return true;
            }

            return false;
        }
        public bool CanMoveBeChoiceLocked(Main.Pokemon.Pokemon pokemon, Move moveData)
        {
            // Z-Moves, Max Moves, etc. cannot be locked
            if (moveData.HasTag(MoveTag.ZMove)
                || moveData.HasTag(MoveTag.DynamaxMove))
            {
                return false;
            }

            // Cannot lock moves that are not already in the moveset
            if (!DoesPokemonHaveMove(pokemon, moveData.ID))
            {
                return false;
            }

            return true;
        }

        public bool CanPokemonMoveBeDisabled(Main.Pokemon.Pokemon pokemon, string moveID)
        {
            if (string.IsNullOrEmpty(moveID))
            {
                return false;
            }
            if (!DoesPokemonHaveMove(pokemon, moveID))
            {
                return false;
            }
            Move moveData = Moves.instance.GetMoveData(moveID);
            return !moveData.HasTag(MoveTag.CannotDisable);
        }

        public List<Main.Pokemon.Pokemon> GetPokemonUsingSnatch(Main.Pokemon.Pokemon ignorePokemon)
        {
            List<Main.Pokemon.Pokemon> pokemon = new List<Main.Pokemon.Pokemon>();
            for (int i = 0; i < pokemonOnField.Count; i++)
            {
                if (!string.IsNullOrEmpty(pokemonOnField[i].bProps.snatchMove))
                {
                    if (pokemonOnField[i] != ignorePokemon)
                    {
                        pokemon.Add(pokemonOnField[i]);
                    }
                }
            }
            return pokemon;
        }

        // Moves: Call other moves
        public string GetNaturePowerMove(Main.Pokemon.Pokemon pokemon)
        {
            // TODO: Nature Power
            string naturePowerMove = null;

            // Move based on terrain
            PBS.Databases.Effects.BattleStatuses.BattleSE terrain_ = terrain.data.GetEffectNew(BattleSEType.Terrain);
            if (terrain_ != null)
            {
                PBS.Databases.Effects.BattleStatuses.Terrain terrainEffect = terrain_ as PBS.Databases.Effects.BattleStatuses.Terrain;
                if (!string.IsNullOrEmpty(terrainEffect.naturePowerMove))
                {
                    return terrainEffect.naturePowerMove;
                }
            }

            // Move based on environment

            naturePowerMove = environment.environmentType == BattleEnvironmentType.Building ? "tackle"
                : environment.environmentType == BattleEnvironmentType.Cave ? "rollingkick"
                : "scratch";
            return naturePowerMove;
        }


        public List<string> GetPokemonAssistMoves(Main.Pokemon.Pokemon pokemon)
        {
            List<string> moves = new List<string>();
            Team team = GetTeam(pokemon);
            if (team != null)
            {
                for (int i = 0; i < team.trainers.Count; i++)
                {
                    for (int k = 0; k < team.trainers[i].party.Count; k++)
                    {
                        List<Moveslot> moveslots = team.trainers[i].party[k].GetMoveset();
                        for (int j = 0; j < moveslots.Count; j++)
                        {
                            if (moveslots[j] != null)
                            {
                                // Check Assist Validity
                                Move moveData = Moves.instance.GetMoveData(moveslots[j].moveID);
                                if (!moves.Contains(moveData.ID)
                                    && !moveData.HasTag(MoveTag.UncallableByAssist)
                                    && !moveData.HasTag(MoveTag.ZMove))
                                {
                                    moves.Add(moveData.ID);
                                }
                            }
                        }
                    }
                }
            }
            return moves;
        }
        public List<string> GetMetronomeMoves(Main.Pokemon.Pokemon pokemon)
        {
            List<string> moves = Moves.instance.GetMetronomeMoves();
            return moves;
        }
        public List<string> GetPokemonSleepTalkMoves(Main.Pokemon.Pokemon pokemon)
        {
            List<string> moves = new List<string>();
            List<Moveslot> moveslots = GetPokemonBattleMoveslots(pokemon);
            for (int i = 0; i < moveslots.Count; i++)
            {
                Move moveData = Moves.instance.GetMoveData(moveslots[i].moveID);
                if (!moveData.HasTag(MoveTag.UncallableCommon)
                    && !moveData.HasTag(MoveTag.UncallableBySleepTalk)
                    && !moveData.HasTag(MoveTag.ZMove))
                {
                    moves.Add(moveData.ID);
                }
            }
            return moves;
        }
        public List<BattlePosition> GetRedirectionTargets(
            Main.Pokemon.Pokemon userPokemon,
            Move moveData,
            MoveTargetType overwriteTarget = MoveTargetType.None,
            bool stopAtFirst = true)
        {
            List<BattlePosition> positions = new List<BattlePosition>();
            List<Main.Pokemon.Pokemon> fastestPokemon = GetPokemonBySpeed(pokemonOnField);

            // Set ability bypass if possible
            bool bypassAbility = false;
            // Mold Breaker bypasses ability immunities
            if (!bypassAbility)
            {
                PBS.Databases.Effects.Abilities.AbilityEffect moldBreakerEffect =
                    PBPGetAbilityEffect(userPokemon, AbilityEffectType.MoldBreaker);
                if (moldBreakerEffect != null)
                {
                    bypassAbility = true;
                }

                // Sunsteel Strike bypasses ability immunities
                PBS.Databases.Effects.Moves.MoveEffect effect = moveData.GetEffectNew(MoveEffectType.SunteelStrike);
                if (effect != null)
                {
                    bypassAbility = true;
                }
            }

            for (int i = 0; i < fastestPokemon.Count; i++)
            {
                Main.Pokemon.Pokemon candidate = fastestPokemon[i];
                if (userPokemon != candidate)
                {
                    bool redirected = false;

                    // Lightning Rod
                    if (!redirected)
                    {
                        AbilityEffect effect = PBPLegacyGetAbilityEffect(
                            candidate,
                            AbilityEffectType.LightningRod,
                            bypassAbility);
                        if (effect != null)
                        {
                            List<string> drawTypes = new List<string>(effect.stringParams);
                            if (drawTypes.Contains(moveData.moveType))
                            {
                                redirected = true;
                            }
                        }
                    }

                    if (redirected)
                    {
                        positions.Add(GetPokemonPosition(candidate));
                        if (stopAtFirst)
                        {
                            break;
                        }
                    }
                }
            }

            return positions;
        }
        public List<BattlePosition> GetFollowMeTargets(
            Main.Pokemon.Pokemon userPokemon,
            Move moveData,
            MoveTargetType overwriteTarget = MoveTargetType.None,
            bool stopAtFirst = true)
        {
            List<BattlePosition> positions = new List<BattlePosition>();

            List<Main.Pokemon.Pokemon> fastestPokemon = GetPokemonBySpeed(pokemonOnField);
            for (int i = 0; i < fastestPokemon.Count; i++)
            {
                Main.Pokemon.Pokemon candidate = fastestPokemon[i];
                if (ArePokemonEnemies(userPokemon, candidate)
                    && !IsPokemonFainted(candidate)
                    && candidate.bProps.isCenterOfAttention)
                {
                    if (IsMoveAdjacentTargeting(userPokemon, moveData, overwriteTarget)
                        && !IsMoveMultiTargeting(userPokemon, moveData, overwriteTarget))
                    {
                        positions.Add(GetPokemonPosition(candidate));
                        if (stopAtFirst)
                        {
                            break;
                        }
                    }
                }
            }

            return positions;
        }


        // Status Conditions
        public List<StatusCondition> GetPokemonStatusConditions(Main.Pokemon.Pokemon pokemon)
        {
            List<StatusCondition> conditions = pokemon.GetAllStatusConditions();
            return conditions;
        }
        public List<StatusCondition> GetAllPokemonFilteredStatus(Main.Pokemon.Pokemon pokemon, PokemonSEType effect)
        {
            List<StatusCondition> conditions = new List<StatusCondition>();
            List<StatusCondition> allConditions = GetPokemonStatusConditions(pokemon);
            for (int i = 0; i < allConditions.Count; i++)
            {
                if (allConditions[i].data.GetEffect(effect) != null)
                {
                    conditions.Add(allConditions[i]);
                }
            }
            return conditions;
        }
        public StatusCondition GetPokemonFilteredStatus(Main.Pokemon.Pokemon pokemon, PokemonSEType effect)
        {
            List<StatusCondition> conditions = GetAllPokemonFilteredStatus(pokemon, effect);
            if (conditions.Count > 0)
            {
                return conditions[0];
            }
            return null;
        }
        public void AdvanceStatusTurns(Main.Pokemon.Pokemon pokemon, PokemonSTag filterTag)
        {
            List<StatusCondition> statusConditions = GetPokemonStatusConditions(pokemon);
            for (int i = 0; i < statusConditions.Count; i++)
            {
                StatusCondition condition = statusConditions[i];
                if (condition.data.HasTag(filterTag))
                {
                    AdvanceStatusTurns(pokemon, condition);
                }
            }
        }
        public void AdvanceStatusTurns(Main.Pokemon.Pokemon pokemon, StatusCondition condition)
        {
            if (condition.turnsLeft > 0)
            {
                condition.turnsLeft--;
            }
        }
        public void HealStatusCondition(Main.Pokemon.Pokemon pokemon, StatusCondition condition)
        {
            if (condition.data.HasTag(PokemonSTag.NonVolatile)
                && pokemon.nonVolatileStatus.statusID == condition.statusID)
            {
                pokemon.UnsetNonVolatileStatus();
            }
            pokemon.RemoveStatusCondition(condition.statusID);
        }

        public StatusCondition GetConfusionStatusCondition(Main.Pokemon.Pokemon pokemon)
        {
            List<StatusCondition> conditions = GetPokemonStatusConditions(pokemon);
            for (int i = 0; i < conditions.Count; i++)
            {
                PokemonCEff trapEffect = conditions[i].data.GetEffect(PokemonSEType.Confusion);
                if (trapEffect != null)
                {
                    return conditions[i];
                }
            }
            return null;
        }
        public StatusCondition GetDisableStatusCondition(Main.Pokemon.Pokemon pokemon)
        {
            List<StatusCondition> conditions = GetPokemonStatusConditions(pokemon);
            for (int i = 0; i < conditions.Count; i++)
            {
                PokemonCEff effect = conditions[i].data.GetEffect(PokemonSEType.Disable);
                if (effect != null)
                {
                    return conditions[i];
                }
            }
            return null;
        }
        public StatusCondition GetTrapStatusCondition(Main.Pokemon.Pokemon pokemon)
        {
            List<StatusCondition> conditions = GetPokemonStatusConditions(pokemon);
            for (int i = 0; i < conditions.Count; i++)
            {
                PokemonCEff trapEffect = conditions[i].data.GetEffect(PokemonSEType.Trap);
                if (trapEffect != null)
                {
                    return conditions[i];
                }
            }
            return null;
        }

        public StatusCondition ApplyStatusCondition(Main.Pokemon.Pokemon pokemon, string statusID, int turnsLeft = -1)
        {
            PokemonStatus statusData = PokemonStatuses.instance.GetStatusData(statusID);
            StatusCondition condition = new StatusCondition(
                statusID: statusID,
                turnsLeft: turnsLeft
                );
            pokemon.ApplyStatusCondition(condition);
            return condition;
        }

        // Team Conditions
        public List<TeamCondition> GetTeamSCs(Team team)
        {
            List<TeamCondition> conditions = team.GetAllStatusConditions();
            return conditions;
        }
        public TeamCondition GetTeamFilteredSC(Team team, TeamSEType effect)
        {
            List<TeamCondition> conditions = GetTeamAllFilteredSC(team, effect);
            if (conditions.Count > 0)
            {
                return conditions[0];
            }
            return null;
        }
        public List<TeamCondition> GetTeamAllFilteredSC(Team team, TeamSEType effect)
        {
            List<TeamCondition> conditions = new List<TeamCondition>();
            List<TeamCondition> allConditions = GetTeamSCs(team);
            for (int i = 0; i < allConditions.Count; i++)
            {
                if (allConditions[i].data.GetEffect(effect) != null)
                {
                    conditions.Add(allConditions[i]);
                }
            }
            return conditions;
        }
        public void HealTeamSC(Team team, TeamCondition condition)
        {
            team.RemoveStatusCondition(condition.statusID);
            team.bProps.conditions.Remove(condition);
            team.bProps.lightScreens.Remove(condition);
        }
        public void AdvanceTeamStatusTurns(Team team, TeamSTag filterTag = TeamSTag.TurnsDecreaseOnEnd)
        {
            List<TeamCondition> conditions = GetTeamSCs(team);
            for (int i = 0; i < conditions.Count; i++)
            {
                TeamCondition condition = conditions[i];
                if (condition.data.HasTag(filterTag))
                {
                    AdvanceTeamStatusTurns(team, condition);
                }
            }
        }
        public void AdvanceTeamStatusTurns(Team team, TeamCondition condition)
        {
            if (condition.turnsLeft > 0)
            {
                condition.turnsActive++;
                condition.turnsLeft--;
            }
        }

        public bool ApplyTeamSC(Team team, TeamCondition condition)
        {
            bool isAdded = false;
            // Screens
            if (condition.data.GetEffectNew(TeamSEType.LightScreen) != null)
            {
                isAdded = true;
                team.bProps.lightScreens.Add(condition);
            }
            // others
            else
            {
                isAdded = true;
                team.bProps.conditions.Add(condition);
            }
            return isAdded;
        }
        public TeamCondition ApplyTeamSC(Team team, string statusID, int turnsLeft = -1)
        {
            TeamStatus statusData = TeamStatuses.instance.GetStatusData(statusID);
            TeamCondition condition = new TeamCondition(
                statusID: statusID,
                turnsLeft: turnsLeft
                );
            team.AddStatusCondition(condition);
            return condition;
        }

        // Battle.Core.Model Conditions
        public BattleCondition GetBattleFilteredSC(BattleSEType effect)
        {
            List<BattleCondition> conditions = GetAllBattleFilteredSC(effect);
            if (conditions.Count > 0)
            {
                return conditions[0];
            }
            return null;
        }
        public List<BattleCondition> GetAllBattleFilteredSC(BattleSEType effect)
        {
            List<BattleCondition> conditions = new List<BattleCondition>();
            List<BattleCondition> allConditions = BBPGetSCs();
            for (int i = 0; i < allConditions.Count; i++)
            {
                if (allConditions[i].data.GetEffect(effect) != null)
                {
                    conditions.Add(allConditions[i]);
                }
            }
            return conditions;
        }
        public bool HealBattleSC(BattleCondition condition)
        {
            bool isRemoved = false;
            if (condition.data.GetEffectNew(BattleSEType.Weather) != null)
            {
                weather = new BattleCondition(defaultWeather);
                isRemoved = true;
            }
            if (condition.data.GetEffectNew(BattleSEType.Terrain) != null)
            {
                terrain = new BattleCondition(defaultTerrain);
                isRemoved = true;
            }
            if (condition.data.GetEffectNew(BattleSEType.Gravity) != null)
            {
                gravity = new BattleCondition(defaultGravity);
                isRemoved = true;
            }
            if (condition.data.GetEffectNew(BattleSEType.MagicRoom) != null)
            {
                magicRoom = new BattleCondition(defaultMagicRoom);
                isRemoved = true;
            }
            if (condition.data.GetEffectNew(BattleSEType.TrickRoom) != null)
            {
                trickRoom = new BattleCondition(defaultTrickRoom);
                isRemoved = true;
            }
            if (condition.data.GetEffectNew(BattleSEType.WonderRoom) != null)
            {
                wonderRoom = new BattleCondition(defaultWonderRoom);
                isRemoved = true;
            }
            if (!isRemoved)
            {
                List<BattleCondition> conditions = new List<BattleCondition>(statusConditions);
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (conditions[i].statusID == condition.statusID)
                    {
                        statusConditions.Remove(conditions[i]);
                        isRemoved = true;
                    }
                }
            }
            return isRemoved;
        }
        public void AdvanceBattleStatusTurns(BattleSTag filterTag = BattleSTag.TurnsDecreaseOnEnd)
        {
            List<BattleCondition> conditions = BBPGetSCs();
            for (int i = 0; i < conditions.Count; i++)
            {
                BattleCondition condition = conditions[i];
                if (condition.data.HasTag(filterTag))
                {
                    AdvanceBattleStatusTurns(condition);
                }
            }
        }
        public void AdvanceBattleStatusTurns(BattleCondition condition)
        {
            if (condition.turnsLeft > 0)
            {
                condition.turnsActive++;
                condition.turnsLeft--;
            }
        }
        public bool HasStatusCondition(string statusID)
        {
            List<BattleCondition> conditions = BBPGetSCs();
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i].statusID == statusID)
                {
                    return true;
                }
            }
            return false;
        }

        public BattleCondition InflictBattleSC(string statusID, int turnsLeft = -1)
        {
            BattleStatus statusData = BattleStatuses.instance.GetStatusData(statusID);
            BattleCondition condition = new BattleCondition(
                statusID: statusID,
                turnsLeft: turnsLeft
                );

            bool isAdded = false;
            if (statusData.GetEffectNew(BattleSEType.Weather) != null)
            {
                weather = condition;
                isAdded = true;
            }
            if (statusData.GetEffectNew(BattleSEType.Terrain) != null)
            {
                terrain = condition;
                isAdded = true;
            }
            if (statusData.GetEffectNew(BattleSEType.Gravity) != null)
            {
                gravity = condition;
                isAdded = true;
            }
            if (statusData.GetEffectNew(BattleSEType.MagicRoom) != null)
            {
                magicRoom = condition;
                isAdded = true;
            }
            if (statusData.GetEffectNew(BattleSEType.TrickRoom) != null)
            {
                trickRoom = condition;
                isAdded = true;
            }
            if (statusData.GetEffectNew(BattleSEType.WonderRoom) != null)
            {
                wonderRoom = condition;
                isAdded = true;
            }

            if (!isAdded)
            {
                statusConditions.Add(condition);
            }
            return condition;
        }

        // Effects
        public bool DoesEffectFilterPass(
            PBS.Databases.Effects.Filter.FilterEffect effect_,
            Main.Pokemon.Pokemon userPokemon = null,
            Main.Pokemon.Pokemon targetPokemon = null,
            Team targetTeam = null,
            Move moveData = null,
            Data.Ability abilityData = null,
            Item item = null
            )
        {
            bool success = true;
            if (targetTeam == null)
            {
                if (targetPokemon != null)
                {
                    targetTeam = GetTeam(targetPokemon);
                }
            }

            // Burning Jealousy
            if (effect_.effectType == FilterEffectType.BurningJealousy)
            {
                success = false;
                PBS.Databases.Effects.Filter.BurningJealousy burningJealousy = effect_ as PBS.Databases.Effects.Filter.BurningJealousy;
                if (burningJealousy.targetType == PBS.Databases.Effects.Filter.BurningJealousy.TargetType.Self
                    && userPokemon != null)
                {
                    success = burningJealousy.DoesPokemonPassStatCheck(userPokemon);
                }
                else if (burningJealousy.targetType == PBS.Databases.Effects.Filter.BurningJealousy.TargetType.Target
                    && targetPokemon != null)
                {
                    success = burningJealousy.DoesPokemonPassStatCheck(targetPokemon);
                }
                else if (burningJealousy.targetType == PBS.Databases.Effects.Filter.BurningJealousy.TargetType.AllyTeam
                    && userPokemon != null)
                {
                    List<Main.Pokemon.Pokemon> teamPokemon = GetAllyPokemon(userPokemon);
                    teamPokemon.Add(userPokemon);
                    for (int i = 0; i < teamPokemon.Count; i++)
                    {
                        if (burningJealousy.DoesPokemonPassStatCheck(teamPokemon[i]))
                        {
                            success = true;
                            break;
                        }
                    }
                }
                else if (burningJealousy.targetType == PBS.Databases.Effects.Filter.BurningJealousy.TargetType.TargetTeam
                    && targetTeam != null)
                {
                    List<Main.Pokemon.Pokemon> teamPokemon = GetTeamPokemonOnField(targetTeam);
                    for (int i = 0; i < teamPokemon.Count; i++)
                    {
                        if (burningJealousy.DoesPokemonPassStatCheck(teamPokemon[i]))
                        {
                            success = true;
                            break;
                        }
                    }
                }
            }
            // Harvest
            else if (effect_.effectType == FilterEffectType.Harvest)
            {
                success = false;
                PBS.Databases.Effects.Filter.Harvest harvest = effect_ as PBS.Databases.Effects.Filter.Harvest;
                if (harvest.conditionType == PBS.Databases.Effects.Filter.Harvest.ConditionType.Battle)
                {
                    if (BBPGetSCs(harvest).Count > 0)
                    {
                        success = true;
                    }
                }
                else if (harvest.conditionType == PBS.Databases.Effects.Filter.Harvest.ConditionType.Team
                    && targetTeam != null)

                {
                    if (TBPGetSCs(targetTeam, harvest).Count > 0)
                    {
                        success = true;
                    }
                }
                else if (harvest.conditionType == PBS.Databases.Effects.Filter.Harvest.ConditionType.Pokemon
                    && targetPokemon != null)
                {
                    if (PBPGetSCs(targetPokemon, harvest).Count > 0)
                    {
                        success = true;
                    }
                }
            }
            // Item Check
            else if (effect_.effectType == FilterEffectType.ItemCheck && item != null)
            {
                PBS.Databases.Effects.Filter.ItemCheck itemCheck = effect_ as PBS.Databases.Effects.Filter.ItemCheck;
                success = itemCheck.DoesItemPassFilter(item);
            }
            // Move Check
            else if (effect_.effectType == FilterEffectType.MoveCheck && moveData != null)
            {
                success = true;
                PBS.Databases.Effects.Filter.MoveCheck moveCheck = effect_ as PBS.Databases.Effects.Filter.MoveCheck;

                // Specific Moves
                if (success && moveCheck.specificMoveIDs.Count > 0)
                {
                    success = false;
                    for (int i = 0; i < moveCheck.specificMoveIDs.Count && !success; i++)
                    {
                        if (moveData.ID == moveCheck.specificMoveIDs[i]
                            || moveData.IsABaseID(moveCheck.specificMoveIDs[i]))
                        {
                            success = true;
                        }
                    }
                }

                // Category
                if (success && moveCheck.moveCategories.Count > 0)
                {
                    success = false;
                    if (moveCheck.moveCategories.Contains(moveData.category))
                    {
                        success = true;
                    }
                }

                // Effects
                if (success && moveCheck.moveEffects.Count > 0)
                {
                    success = false;
                    List<MoveEffectType> moveEffects = new List<MoveEffectType>(moveCheck.moveEffects);
                    for (int i = 0; i < moveEffects.Count && !success; i++)
                    {
                        if (moveData.GetEffectNew(moveEffects[i]) != null)
                        {
                            success = true;
                        }
                    }
                }

                // Tags
                if (success && moveCheck.moveTags.Count > 0)
                {
                    success = false;
                    List<MoveTag> moveTags = new List<MoveTag>(moveCheck.moveTags);
                    for (int i = 0; i < moveTags.Count && !success; i++)
                    {
                        if (IsMoveTagSatisfied(userPokemon, moveData, moveTags[i], targetPokemon))
                        {
                            success = true;
                        }
                    }
                }

                // Damaging Only
                if (success && moveCheck.damagingOnly && !IsMoveDamaging(moveData))
                {
                    success = false;
                }

                // Healing Only
                if (success && moveCheck.healingOnly)
                {
                    success = IsHealingMove(moveData);
                }

                // Sheer-Force Trigger
                if (success && moveCheck.sheerForceOnly)
                {
                    if (!DoesMoveTriggerSheerForce(moveData))
                    {
                        success = false;
                    }
                }
            }
            // Pollen Puff
            else if (effect_.effectType == FilterEffectType.PollenPuff && userPokemon != null && targetPokemon != null)
            {
                success = false;
                PBS.Databases.Effects.Filter.PollenPuff pollenPuff = effect_ as PBS.Databases.Effects.Filter.PollenPuff;
                if (pollenPuff.targetTypes.Contains(PBS.Databases.Effects.Filter.PollenPuff.TargetType.Self)
                    && userPokemon.IsTheSameAs(targetPokemon))
                {
                    success = true;
                }
                else if (pollenPuff.targetTypes.Contains(PBS.Databases.Effects.Filter.PollenPuff.TargetType.Ally)
                    && ArePokemonAllies(userPokemon, targetPokemon))
                {
                    success = true;
                }
                else if (pollenPuff.targetTypes.Contains(PBS.Databases.Effects.Filter.PollenPuff.TargetType.Enemy)
                    && ArePokemonEnemies(userPokemon, targetPokemon))
                {
                    success = true;
                }
            }
            // Type List
            else if (effect_.effectType == FilterEffectType.TypeList)
            {
                success = false;
                PBS.Databases.Effects.Filter.TypeList typeList = effect_ as PBS.Databases.Effects.Filter.TypeList;
                // Pokemon
                if (typeList.targetType == PBS.Databases.Effects.Filter.TypeList.TargetType.Pokemon && targetPokemon != null)
                {
                    List<string> pokemonTypes = PBPGetTypes(targetPokemon);
                    for (int i = 0; i < pokemonTypes.Count; i++)
                    {
                        if (AreTypesContained(containerTypes: typeList.types, checkType: pokemonTypes[i]))
                        {
                            success = true;
                        }
                        else if (typeList.exact)
                        {
                            success = false;
                            break;
                        }
                    }
                }
                // Move
                else if (typeList.targetType == PBS.Databases.Effects.Filter.TypeList.TargetType.Move && moveData != null)
                {
                    if (AreTypesContained(containerTypes: typeList.types, checkType: moveData.moveType))
                    {
                        success = true;
                    }
                }
            }
            if (effect_.invert)
            {
                success = !success;
            }
            return success;
        }
        public bool DoEffectFiltersPass(
            List<PBS.Databases.Effects.Filter.FilterEffect> filters,
            Main.Pokemon.Pokemon userPokemon = null,
            Main.Pokemon.Pokemon targetPokemon = null,
            Team targetTeam = null,
            Move moveData = null,
            Data.Ability abilityData = null,
            Item item = null
            )
        {
            for (int i = 0; i < filters.Count; i++)
            {
                if (!DoesEffectFilterPass(
                    effect_: filters[i],
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    targetTeam: targetTeam,
                    moveData: moveData,
                    item: item
                    ))
                {
                    return false;
                }
            }
            return true;
        }

        public bool DoesMoveEffectFiltersPass(
            PBS.Databases.Effects.Moves.MoveEffect effect,
            Move moveData,
            Main.Pokemon.Pokemon userPokemon = null,
            Main.Pokemon.Pokemon targetPokemon = null,
            Team targetTeam = null
            )
        {
            return DoEffectFiltersPass(
                filters: effect.filters,
                userPokemon: userPokemon,
                targetPokemon: targetPokemon,
                targetTeam: targetTeam,
                moveData: moveData
                );
        }
        public bool DoesPokemonSEFiltersPass(
            PBS.Databases.Effects.PokemonStatuses.PokemonSE effect,
            Main.Pokemon.Pokemon userPokemon = null,
            Main.Pokemon.Pokemon targetPokemon = null,
            Team targetTeam = null
            )
        {
            for (int i = 0; i < effect.filters.Count; i++)
            {
                if (!DoesEffectFilterPass(
                    effect_: effect.filters[i],
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    targetTeam: targetTeam
                    ))
                {
                    return false;
                }
            }
            return true;
        }
        public bool DoesTeamSEFiltersPass(
            PBS.Databases.Effects.TeamStatuses.TeamSE effect,
            Main.Pokemon.Pokemon userPokemon = null,
            Main.Pokemon.Pokemon targetPokemon = null,
            Team targetTeam = null
            )
        {
            for (int i = 0; i < effect.filters.Count; i++)
            {
                if (!DoesEffectFilterPass(
                    effect_: effect.filters[i],
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    targetTeam: targetTeam
                    ))
                {
                    return false;
                }
            }
            return true;
        }
        public bool DoesBattleEFiltersPass(
            PBS.Databases.Effects.BattleStatuses.BattleSE effect,
            Main.Pokemon.Pokemon userPokemon = null,
            Main.Pokemon.Pokemon targetPokemon = null,
            Team targetTeam = null
            )
        {
            for (int i = 0; i < effect.filters.Count; i++)
            {
                if (!DoesEffectFilterPass(
                    effect_: effect.filters[i],
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    targetTeam: targetTeam
                    ))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanApplyMoveEffect(Main.Pokemon.Pokemon userPokemon, Main.Pokemon.Pokemon targetPokemon, Move moveData, MoveEffect effect)
        {
            if (effect.HasFilter(MoveEffectFilter.AlliesOnly) && !ArePokemonAllies(userPokemon, targetPokemon))
            {
                return false;
            }
            if (effect.HasFilter(MoveEffectFilter.EnemiesOnly) && !ArePokemonEnemies(userPokemon, targetPokemon))
            {
                return false;
            }

            return true;
        }
        public bool DoesMoveEffectPassChance(
            MoveEffect effect,
            Move moveData,
            Main.Pokemon.Pokemon userPokemon = null,
            Main.Pokemon.Pokemon targetPokemon = null,
            Team targetTeam = null,
            float startChance = 0)
        {
            float chance;
            // set base chance
            if (startChance == 0)
            {
                chance = effect.effectChance;
            }
            else
            {
                chance = startChance;
            }

            // if negative, bypass check
            if (chance < 0)
            {
                return true;
            }

            // Serene Grace
            AbilityEffect sereneGrace = PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.SereneGrace);
            if (sereneGrace != null)
            {
                chance *= sereneGrace.GetFloat(0);
            }

            return Random.value <= chance;
        }

        public bool DoesMoveEffectPassChecks(
            PBS.Databases.Effects.Moves.MoveEffect effect,
            Move moveData,
            Main.Pokemon.Pokemon userPokemon = null,
            Main.Pokemon.Pokemon targetPokemon = null,
            Team targetTeam = null,
            float startChance = 0)
        {
            // Check Filters here
            if (!DoesMoveEffectFiltersPass(
                effect: effect,
                moveData: moveData,
                userPokemon: userPokemon,
                targetPokemon: targetPokemon,
                targetTeam: targetTeam
                ))
            {
                return false;
            }
            // Sheer Force Check
            PBS.Databases.Effects.Abilities.AbilityEffect sheerForce_ =
                PBPGetAbilityEffect(userPokemon, AbilityEffectType.SheerForce);
            if (sheerForce_ != null)
            {
                if (IsMoveEffectAdditional(moveData, effect))
                {
                    return false;
                }
            }

            if (targetPokemon == null)
            {
                return true;
            }

            // Shield Dust
            List<PBS.Databases.Effects.Abilities.AbilityEffect> shieldDust_ =
                PBPGetAbilityEffects(targetPokemon, AbilityEffectType.ShieldDust);
            for (int i = 0; i < shieldDust_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.ShieldDust shieldDust =
                    shieldDust_[i] as PBS.Databases.Effects.Abilities.ShieldDust;
                if (DoEffectFiltersPass(
                    filters: shieldDust.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    if (IsMoveEffectAdditional(moveData, effect))
                    {
                        return false;
                    }
                }
            }


            // Check Chance here
            float chance;
            // set base chance
            if (startChance == 0)
            {
                chance = effect.chance;
            }
            else
            {
                chance = startChance;
            }
            // if negative, bypass check
            if (chance < 0)
            {
                return true;
            }

            // Serene Grace
            List<PBS.Databases.Effects.Abilities.AbilityEffect> sereneGrace_ =
                PBPGetAbilityEffects(userPokemon, AbilityEffectType.SereneGrace);
            for (int i = 0; i < sereneGrace_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.SereneGrace sereneGrace =
                    sereneGrace_[i] as PBS.Databases.Effects.Abilities.SereneGrace;
                if (DoEffectFiltersPass(
                    filters: sereneGrace.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    chance *= sereneGrace.chanceMultiplier;
                }
            }

            return Random.value <= chance;
        }

        public bool DoesStatusEffectPassChance(
            PokemonCEff effect,
            StatusCondition condition,
            Main.Pokemon.Pokemon userPokemon = null,
            Main.Pokemon.Pokemon targetPokemon = null,
            float startChance = 0)
        {
            float chance = 1f;

            // set base chance
            if (startChance == 0)
            {
                if (effect.effectType == PokemonSEType.Confusion
                    || effect.effectType == PokemonSEType.Paralysis
                    || effect.effectType == PokemonSEType.Freeze)
                {
                    chance = effect.GetFloat(0);
                }
            }
            else
            {
                chance = startChance;
            }

            // if negative, bypass check
            if (chance < 0)
            {
                return true;
            }

            return Random.value <= chance;
        }

        // Items
        public Item GetPokemonItem(Main.Pokemon.Pokemon pokemon, string itemID)
        {
            Item item = PBPGetHeldItem(pokemon);
            if (item != null)
            {
                if (item.itemID == itemID)
                {
                    return item;
                }
            }
            return null;
        }
        public Item PBPGetHeldItem(Main.Pokemon.Pokemon pokemon)
        {
            List<Item> items = PBPGetHeldItems(pokemon);
            if (items.Count > 0)
            {
                return items[0];
            }
            return null;
        }
        public List<Item> PBPGetHeldItems(Main.Pokemon.Pokemon pokemon)
        {
            List<Item> items = new List<Item>();
            if (pokemon.item != null)
            {
                items.Add(pokemon.item);
            }
            return items;
        }
        public void PBPConsumeItem(Main.Pokemon.Pokemon pokemon, Item item)
        {
            pokemon.UnsetItem(item);
        }
        public Item GetPokemonItemFiltered(Main.Pokemon.Pokemon pokemon, ItemEffectType effect)
        {
            Item item = PBPGetHeldItem(pokemon);
            if (item != null)
            {
                if (item.data.GetEffect(effect) != null)
                {
                    if (CanPokemonUseItem(pokemon, item))
                    {
                        return item;
                    }
                }
            }
            return null;
        }
        public PBS.Databases.Effects.Items.ItemEffect PBPGetItemEffect(Main.Pokemon.Pokemon pokemon, ItemEffectType effectType)
        {
            List<PBS.Databases.Effects.Items.ItemEffect> effects = PBPGetItemEffects(pokemon, effectType);
            if (effects.Count > 0)
            {
                return effects[0];
            }
            return null;
        }
        public List<PBS.Databases.Effects.Items.ItemEffect> PBPGetItemEffects(Main.Pokemon.Pokemon pokemon, ItemEffectType effectType)
        {
            List<PBS.Databases.Effects.Items.ItemEffect> effects = new List<PBS.Databases.Effects.Items.ItemEffect>();
            Item item = PBPGetHeldItem(pokemon);
            if (item != null)
            {
                if (CanPokemonUseItem(pokemon, item))
                {
                    effects.AddRange(item.data.GetEffectsNew(effectType));
                }
            }
            return effects;
        }
        public Item PBPGetItemWithEffect(Main.Pokemon.Pokemon pokemon, ItemEffectType effectType)
        {
            List<Item> items = PBPGetItemsWithEffect(pokemon, effectType);
            if (items.Count > 0)
            {
                return items[0];
            }
            return null;
        }
        public List<Item> PBPGetItemsWithEffect(Main.Pokemon.Pokemon pokemon, ItemEffectType effectType)
        {
            List<Item> items = new List<Item>();
            List<Item> heldItems = PBPGetHeldItems(pokemon);
            for (int i = 0; i < heldItems.Count; i++)
            {
                if (heldItems[i].data.GetEffectNew(effectType) != null)
                {
                    if (CanPokemonUseItem(pokemon, heldItems[i]))
                    {
                        items.Add(heldItems[i]);
                    }
                }
            }
            return items;
        }
        public ItemEffect GetPokemonItemEffect(Main.Pokemon.Pokemon pokemon, ItemEffectType effectType)
        {
            List<ItemEffect> itemEffects = GetPokemonItemEffects(pokemon, effectType);
            if (itemEffects.Count > 0)
            {
                return itemEffects[0];
            }
            return null;
        }
        public List<ItemEffect> GetPokemonItemEffects(Main.Pokemon.Pokemon pokemon, ItemEffectType effectType)
        {
            List<ItemEffect> itemEffects = new List<ItemEffect>();

            Item item = PBPGetHeldItem(pokemon);
            if (item != null)
            {
                if (CanPokemonUseItem(pokemon, item))
                {
                    itemEffects.AddRange(item.data.GetEffects(effectType));
                }
            }
            return itemEffects;
        }
        public bool CanPokemonUseItem(Main.Pokemon.Pokemon pokemon, Item item)
        {
            bool canUse = true;
            if (canUse && !item.useable)
            {
                canUse = false;
            }

            // Klutz
            if (canUse && !item.data.HasTag(ItemTag.BypassKlutz))
            {
                List<PBS.Databases.Effects.Abilities.AbilityEffect> klutz_ = PBPGetAbilityEffects(pokemon, AbilityEffectType.Klutz);
                for (int i = 0; i < klutz_.Count && canUse; i++)
                {
                    PBS.Databases.Effects.Abilities.Klutz klutz = klutz_[i] as PBS.Databases.Effects.Abilities.Klutz;
                    if (DoEffectFiltersPass(
                        filters: klutz.filters,
                        userPokemon: pokemon,
                        targetPokemon: pokemon,
                        item: item
                        ))
                    {
                        canUse = false;
                    }
                }
            }

            // Magic Room
            if (canUse)
            {
                if (BBPIsPokemonAffectedByBSC(pokemon, magicRoom))
                {
                    PBS.Databases.Effects.BattleStatuses.BattleSE magicRoom_ = magicRoom.data.GetEffectNew(BattleSEType.MagicRoom);
                    if (magicRoom_ != null)
                    {
                        PBS.Databases.Effects.BattleStatuses.MagicRoom magicRoomEffect = magicRoom_ as PBS.Databases.Effects.BattleStatuses.MagicRoom;
                        if (magicRoomEffect.suppressItems)
                        {
                            canUse = false;
                        }
                    }
                }
            }

            // Embargo
            if (canUse)
            {
                if (pokemon.bProps.embargo != null && !item.data.HasTag(ItemTag.BypassEmbargo))
                {
                    canUse = false;
                }
            }

            return canUse;
        }
        public bool CanPokemonItemBeLost(Main.Pokemon.Pokemon pokemon)
        {
            Item item = PBPGetHeldItem(pokemon);
            if (item != null)
            {
                return CanPokemonItemBeLost(pokemon, item);
            }
            return false;
        }
        public bool CanPokemonItemBeLost(Main.Pokemon.Pokemon pokemon, Item item)
        {
            // Arceus / Genesect / Giratina / Silvally / etc.
            if (item.data.GetEffectNew(ItemEffectType.GriseousOrb) != null)
            {
                if (CanPokemonUseFormChangeItem(pokemon, item))
                {
                    return false;
                }
            }

            // Z-Crystals
            PBS.Databases.Effects.Items.ItemEffect zCrystal_ = item.data.GetEffectNew(ItemEffectType.ZCrystal);
            PBS.Databases.Effects.Items.ItemEffect zCrystalSignature_ = item.data.GetEffectNew(ItemEffectType.ZCrystalSignature);
            if (zCrystal_ != null || zCrystalSignature_ != null)
            {
                return false;
            }

            // Sticky Hold
            List<PBS.Databases.Effects.Abilities.AbilityEffect> stickyHold_ = PBPGetAbilityEffects(pokemon, AbilityEffectType.StickyHold);
            for (int i = 0; i < stickyHold_.Count; i++)
            {
                PBS.Databases.Effects.Abilities.StickyHold stickyHold = stickyHold_[i] as PBS.Databases.Effects.Abilities.StickyHold;
                if (DoEffectFiltersPass(
                    filters: stickyHold.filters,
                    targetPokemon: pokemon,
                    item: item
                    ))
                {
                    return false;
                }
            }

            return true;
        }
        public bool CanPokemonItemBeGained(Main.Pokemon.Pokemon pokemon, Item item)
        {
            // Check if already held item is unusable
            Item heldItem = PBPGetHeldItem(pokemon);
            if (heldItem != null)
            {
                if (!heldItem.useable)
                {
                    return false;
                }
            }

            // Arceus / Genesect / Giratina / etc.
            if (item.data.GetEffect(ItemEffectType.FormChange) != null)
            {
                if (CanPokemonUseFormChangeItem(pokemon, item))
                {
                    return false;
                }
            }

            // TODO: Mega Stones / Z-Crystals

            return true;
        }
        public bool CanPokemonSwapItems(Main.Pokemon.Pokemon pokemon1, Item item1, Main.Pokemon.Pokemon pokemon2, Item item2)
        {
            if (item1 == null && item2 == null)
            {
                return false;
            }

            // Check item1 restrictions
            if (item1 != null)
            {
                if (!CanPokemonItemBeLost(pokemon1, item1) || !CanPokemonItemBeGained(pokemon2, item1))
                {
                    return false;
                }
            }

            // Check item2 restrictions
            if (item2 != null)
            {
                if (!CanPokemonItemBeLost(pokemon2, item2) || !CanPokemonItemBeGained(pokemon1, item2))
                {
                    return false;
                }
            }
            return true;
        }
        public bool CanPokemonConsumeBerries(Main.Pokemon.Pokemon pokemon)
        {
            bool canConsume = true;

            // Unnerve
            if (canConsume)
            {
                for (int i = 0; i < pokemonOnField.Count; i++)
                {
                    if (ArePokemonEnemies(pokemon, pokemonOnField[i]))
                    {
                        if (PBPLegacyGetAbilityEffect(pokemonOnField[i], AbilityEffectType.Unnerve) != null)
                        {
                            canConsume = false;
                            break;
                        }
                    }
                }
            }


            return canConsume;
        }
        public bool CanPokemonUseFormChangeItem(Main.Pokemon.Pokemon pokemon, Item item)
        {
            PBS.Databases.Effects.Items.ItemEffect itemEffect =
                PBPGetItemFormChangeEffect(pokemon, item);
            if (itemEffect != null)
            {
                return true;
            }
            return false;
        }
        public PBS.Databases.Effects.Items.ItemEffect PBPGetItemFormChangeEffect(Main.Pokemon.Pokemon pokemon, Item item)
        {
            List<PBS.Databases.Effects.Items.ItemEffect> griseousOrbs_ = item.data.GetEffectsNew(ItemEffectType.GriseousOrb);
            for (int i = 0; i < griseousOrbs_.Count; i++)
            {
                PBS.Databases.Effects.Items.GriseousOrb griseousOrb = griseousOrbs_[i] as PBS.Databases.Effects.Items.GriseousOrb;
                PBS.Data.Pokemon basePokemonData = Databases.Pokemon.instance.GetPokemonData(griseousOrb.baseFormID);
                PBS.Data.Pokemon toPokemonData = Databases.Pokemon.instance.GetPokemonData(griseousOrb.formID);

                // Validate if the pokemon is contained
                if (pokemon.pokemonID == basePokemonData.ID
                    || pokemon.pokemonID == toPokemonData.ID
                    || pokemon.data.IsABaseID(basePokemonData.ID))
                {
                    bool canUse = true;
                    // Arceus Plate = Multitype Check
                    if (griseousOrb is PBS.Databases.Effects.Items.ArceusPlate)
                    {
                        if (PBPGetAbilityEffect(pokemon, AbilityEffectType.Multitype) != null)
                        {
                            canUse = false;
                        }
                    }

                    // RKS Memory = RKS System Check
                    if (griseousOrb is PBS.Databases.Effects.Items.RKSMemory)
                    {
                        if (PBPGetAbilityEffect(pokemon, AbilityEffectType.RKSSystem) != null)
                        {
                            canUse = false;
                        }
                    }

                    if (canUse)
                    {
                        return griseousOrb;
                    }
                }
            }
            return null;
        }

    }
}

