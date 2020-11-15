using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Player
{
    public class Command
    {
        // General
        public BattleCommandType commandType;
        public string commandUser;
        public bool inProgress = false;
        public bool completed = false;
        public int commandPriority = 0;
        public int iteration = 1;
        public bool isExplicitlySelected = false;

        // Move
        public string moveID;
        public bool consumePP = true;
        public BattlePosition[] targetPositions;
        public bool displayMove = true;
        public bool forceOneHit = false;
        public bool bypassRedirection = false;
        public bool bypassStatusInterrupt = false;
        public bool isDanceMove = false;
        public bool isMoveCalled = false;
        public bool isMoveReflected = false;
        public bool isMoveHijacked = false;
        public bool isFutureSightMove = false;
        public bool isPursuitMove = false;
        public bool isMoveSnatched = false;
        public bool isMagicCoatMove = false;

        public bool isMegaEvolving = false;
        public bool isZMove = false;
        public bool isDynamaxing = false;

        // Switch
        public int switchPosition;
        public int switchingTrainer;
        public string switchInPokemon;

        // Item
        public string itemID;
        public int itemTrainer;

        public Command(bool isExplicitlySelected = false)
        {
            commandType = BattleCommandType.None;
            this.isExplicitlySelected = isExplicitlySelected;
        }

        public static Command CreateMoveCommand(
            string commandUser,
            string moveID,
            List<BattlePosition> targetPositions,
            bool isExplicitlySelected = false,
            bool isMegaEvolving = false,
            bool isZMove = false,
            bool isDynamaxing = false)
        {
            Command command = new Command(isExplicitlySelected);
            command.commandType = BattleCommandType.Fight;
            command.commandUser = commandUser;
            command.moveID = moveID;

            command.targetPositions = targetPositions.ToArray();
            command.isMegaEvolving = isMegaEvolving;
            command.isZMove = isZMove;
            command.isDynamaxing = isDynamaxing;

            return command;
        }

        public static Command CreateSwitchCommand(
            string commandUser,
            int switchPosition,
            int trainer,
            string switchInPokemon,
            bool isExplicitlySelected = false)
        {
            Command command = new Command(isExplicitlySelected);
            command.commandType = BattleCommandType.Party;
            command.switchPosition = switchPosition;
            command.switchingTrainer = trainer;
            command.commandUser = commandUser;
            command.switchInPokemon = switchInPokemon;
            return command;
        }

        public static Command CreateReplaceCommand(
            int switchPosition,
            int trainer,
            string switchInPokemon,
            bool isExplicitlySelected = false)
        {
            Command command = new Command(isExplicitlySelected);
            command.commandType = BattleCommandType.PartyReplace;
            command.switchPosition = switchPosition;
            command.switchingTrainer = trainer;
            command.commandUser = switchInPokemon;
            command.switchInPokemon = switchInPokemon;
            return command;
        }

        public static Command CreateRechargeCommand(
            string commandUser
            )
        {
            Command command = new Command();
            command.commandType = BattleCommandType.Recharge;
            command.commandUser = commandUser;
            return command;
        }

        public static Command CreateBagCommand(
            string itemID,
            string itemPokemon,
            int trainer,
            bool isExplicitlySelected = false)
        {
            Command command = new Command(isExplicitlySelected);
            command.commandType = BattleCommandType.Bag;
            command.itemID = itemID;
            command.commandUser = itemPokemon;
            command.itemTrainer = trainer;
            return command;
        }

        public static Command CreateRunCommand(
            string commandUser,
            bool isExplicitlySelected = false
            )
        {
            Command command = new Command(isExplicitlySelected);
            command.commandType = BattleCommandType.Run;
            command.commandUser = commandUser;
            return command;
        }
    }
}
