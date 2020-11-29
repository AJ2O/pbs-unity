using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Player
{
    /// <summary>
    /// This class is a command from the player's perspective for a given pokemon. Once sent to battle,
    /// it is converted into a formal <seealso cref="BattleCommand"/> to work with
    /// <seealso cref="Battle.Model"/>.
    /// </summary>
    public class Command
    {
        #region General
        /// <summary>
        /// See <seealso cref="BattleCommand.commandType"/>.
        /// </summary>
        public BattleCommandType commandType = BattleCommandType.None;
        /// <summary>
        /// The uniqueID of the pokemon executing the command.
        /// </summary>
        public string commandUser = "";
        /// <summary>
        /// The playerID of the trainer executing the command.
        /// </summary>
        public int commandTrainer = 0;
        /// <summary>
        /// See <seealso cref="BattleCommand.inProgress"/>.
        /// </summary>
        public bool inProgress = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.completed"/>.
        /// </summary>
        public bool completed = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.commandPriority"/>.
        /// </summary>
        public int commandPriority = 0;
        /// <summary>
        /// See <seealso cref="BattleCommand.iteration"/>.
        /// </summary>
        public int iteration = 1;
        /// <summary>
        /// See <seealso cref="BattleCommand.isExplicitlySelected"/>.
        /// </summary>
        public bool isExplicitlySelected = false;
        #endregion

        #region Fight
        /// <summary>
        /// See <seealso cref="BattleCommand.moveID"/>.
        /// </summary>
        public string moveID;
        /// <summary>
        /// See <seealso cref="BattleCommand.consumePP"/>.
        /// </summary>
        public bool consumePP = true;
        /// <summary>
        /// See <seealso cref="BattleCommand.targetPositions"/>.
        /// </summary>
        public BattlePosition[] targetPositions = new BattlePosition[0];
        /// <summary>
        /// See <seealso cref="BattleCommand.displayMove"/>.
        /// </summary>
        public bool displayMove = true;
        /// <summary>
        /// See <seealso cref="BattleCommand.forceOneHit"/>.
        /// </summary>
        public bool forceOneHit = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.bypassRedirection"/>.
        /// </summary>
        public bool bypassRedirection = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.bypassStatusInterrupt"/>.
        /// </summary>
        public bool bypassStatusInterrupt = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isDanceMove"/>.
        /// </summary>
        public bool isDanceMove = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isMoveCalled"/>.
        /// </summary>
        public bool isMoveCalled = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isMoveReflected"/>.
        /// </summary>
        public bool isMoveReflected = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isMoveHijacked"/>.
        /// </summary>
        public bool isMoveHijacked = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isFutureSightMove"/>.
        /// </summary>
        public bool isFutureSightMove = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isPursuitMove"/>.
        /// </summary>
        public bool isPursuitMove = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isMoveSnatched"/>.
        /// </summary>
        public bool isMoveSnatched = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isMagicCoatMove"/>.
        /// </summary>
        public bool isMagicCoatMove = false;

        /// <summary>
        /// See <seealso cref="BattleCommand.isMegaEvolving"/>.
        /// </summary>
        public bool isMegaEvolving = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isZMove"/>.
        /// </summary>
        public bool isZMove = false;
        /// <summary>
        /// See <seealso cref="BattleCommand.isDynamaxing"/>.
        /// </summary>
        public bool isDynamaxing = false;
        #endregion

        #region Switch/Replace
        /// <summary>
        /// See <seealso cref="BattleCommand.switchPosition"/>.
        /// </summary>
        public int switchPosition = 0;
        /// <summary>
        /// The playerID of the trainer switching party members.
        /// </summary>
        public int switchingTrainer = 0;
        /// <summary>
        /// The uniqueID of the pokemon switching in.
        /// </summary>
        public string switchInPokemon = "";
        #endregion

        #region
        /// <summary>
        /// See <seealso cref="BattleCommand.itemID"/>.
        /// </summary>
        public string itemID = "";
        /// <summary>
        /// The playerID of the trainer using the item.
        /// </summary>
        public int itemTrainer = 0;
        #endregion

        #region Command Creation
        public Command(bool isExplicitlySelected = false)
        {
            commandType = BattleCommandType.None;
            this.isExplicitlySelected = isExplicitlySelected;
        }

        /// <summary>
        /// Returns a constructed command for executing a move.
        /// </summary>
        /// <param name="commandUser"></param>
        /// <param name="commandTrainer"></param>
        /// <param name="moveID"></param>
        /// <param name="targetPositions"></param>
        /// <param name="isExplicitlySelected"></param>
        /// <param name="isMegaEvolving"></param>
        /// <param name="isZMove"></param>
        /// <param name="isDynamaxing"></param>
        /// <returns></returns>
        public static Command CreateMoveCommand(
            string commandUser,
            int commandTrainer,
            string moveID,
            List<BattlePosition> targetPositions,
            bool isExplicitlySelected = false,
            bool isMegaEvolving = false,
            bool isZMove = false,
            bool isDynamaxing = false)
        {
            Command command = new Command(isExplicitlySelected);
            command.commandTrainer = commandTrainer;
            command.commandType = BattleCommandType.Fight;
            command.commandUser = commandUser;
            command.moveID = moveID;

            command.targetPositions = targetPositions.ToArray();
            command.isMegaEvolving = isMegaEvolving;
            command.isZMove = isZMove;
            command.isDynamaxing = isDynamaxing;

            return command;
        }

        /// <summary>
        /// Returns a constructed command for switching pokemon.
        /// </summary>
        /// <param name="commandUser"></param>
        /// <param name="switchPosition"></param>
        /// <param name="trainer"></param>
        /// <param name="switchInPokemon"></param>
        /// <param name="isExplicitlySelected"></param>
        /// <returns></returns>
        public static Command CreateSwitchCommand(
            string commandUser,
            int switchPosition,
            int trainer,
            string switchInPokemon,
            bool isExplicitlySelected = false)
        {
            Command command = new Command(isExplicitlySelected);
            command.commandTrainer = trainer;
            command.commandType = BattleCommandType.Party;
            command.switchPosition = switchPosition;
            command.switchingTrainer = trainer;
            command.commandUser = commandUser;
            command.switchInPokemon = switchInPokemon;
            return command;
        }

        /// <summary>
        /// Returns a constructed command for replacing on-field pokemon (ex. fainted pokemon).
        /// </summary>
        /// <param name="switchPosition"></param>
        /// <param name="trainer"></param>
        /// <param name="switchInPokemon"></param>
        /// <param name="isExplicitlySelected"></param>
        /// <returns></returns>
        public static Command CreateReplaceCommand(
            int switchPosition,
            int trainer,
            string switchInPokemon,
            bool isExplicitlySelected = false)
        {
            Command command = new Command(isExplicitlySelected);
            command.commandTrainer = trainer;
            command.commandType = BattleCommandType.PartyReplace;
            command.switchPosition = switchPosition;
            command.switchingTrainer = trainer;
            command.commandUser = switchInPokemon;
            command.switchInPokemon = switchInPokemon;
            return command;
        }

        /// <summary>
        /// Returns a constructed command for recharge turns.
        /// </summary>
        /// <param name="commandUser"></param>
        /// <returns></returns>
        public static Command CreateRechargeCommand(
            string commandUser
            )
        {
            Command command = new Command();
            command.commandType = BattleCommandType.Recharge;
            command.commandUser = commandUser;
            return command;
        }

        /// <summary>
        /// Creates a constructed command for using an item.
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemPokemon"></param>
        /// <param name="trainer"></param>
        /// <param name="isExplicitlySelected"></param>
        /// <returns></returns>
        public static Command CreateBagCommand(
            string itemID,
            string itemPokemon,
            int trainer,
            bool isExplicitlySelected = false)
        {
            Command command = new Command(isExplicitlySelected);
            command.commandTrainer = trainer;
            command.commandType = BattleCommandType.Bag;
            command.itemID = itemID;
            command.commandUser = itemPokemon;
            command.itemTrainer = trainer;
            return command;
        }

        /// <summary>
        /// Returns a constructed command for running from battle.
        /// </summary>
        /// <param name="commandUser"></param>
        /// <param name="commandTrainer"></param>
        /// <param name="isExplicitlySelected"></param>
        /// <returns></returns>
        public static Command CreateRunCommand(
            string commandUser,
            int commandTrainer,
            bool isExplicitlySelected = false
            )
        {
            Command command = new Command(isExplicitlySelected);
            command.commandTrainer = commandTrainer;
            command.commandType = BattleCommandType.Run;
            command.commandUser = commandUser;
            return command;
        }
        #endregion
    }
}
