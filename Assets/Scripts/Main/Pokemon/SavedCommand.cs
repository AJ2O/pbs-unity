using System.Collections.Generic;

namespace PBS.Main.Pokemon
{
    public class PokemonSavedCommand
    {
        public string moveID;
        public List<BattlePosition> targetPositions;
        public bool isPlayerCommand;
        public int iteration;
        public bool consumePP;
        public bool displayMove;

        // Constructor
        public PokemonSavedCommand(
            string moveID,
            List<BattlePosition> targetPositions,
            bool isPlayerCommand = false,
            int iteration = 1,
            bool consumePP = true,
            bool displayMove = true
            )
        {
            this.moveID = moveID;
            this.targetPositions = new List<BattlePosition>();
            for (int i = 0; i < targetPositions.Count; i++)
            {
                this.targetPositions.Add(BattlePosition.Clone(targetPositions[i]));
            }
            this.isPlayerCommand = isPlayerCommand;
            this.iteration = iteration;
            this.consumePP = consumePP;
            this.displayMove = displayMove;
        }
        public static PokemonSavedCommand CreateSavedCommand(BattleCommand command)
        {
            List<BattlePosition> targetPositions = new List<BattlePosition>(command.targetPositions);
            PokemonSavedCommand savedCommand = new PokemonSavedCommand(
                moveID: command.moveID,
                targetPositions: targetPositions,
                isPlayerCommand: command.isExplicitlySelected,
                iteration: command.iteration,
                consumePP: command.consumePP,
                displayMove: command.displayMove
                );
            return savedCommand;
        }

        // Clone
        public static PokemonSavedCommand Clone(PokemonSavedCommand original)
        {
            PokemonSavedCommand cloneCommand = new PokemonSavedCommand(
                moveID: original.moveID,
                targetPositions: original.targetPositions,
                isPlayerCommand: original.isPlayerCommand,
                iteration: original.iteration,
                consumePP: original.consumePP
                );
            return cloneCommand;
        }
    }
}