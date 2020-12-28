using PBS.Battle;
using PBS.Databases;
using PBS.Main.Pokemon;
using PBS.Main.Trainer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.AI
{
    public class Battle
    {
        Model model;
        List<BattleCommand> committedCommands;
        int switchPosition;

        public void UpdateModel(Model battleModel)
        {
            this.model = battleModel;
        }
        public void ResetSelf()
        {
            model = null;
            committedCommands = new List<BattleCommand>();
        }

        // Trainer Command Prompts
        public List<BattleCommand> GetCommandsByPrompt(
            Trainer trainer,
            List<PBS.Main.Pokemon.Pokemon> pokemonToControl)
        {
            committedCommands = new List<BattleCommand>();
            for (int i = 0; i < pokemonToControl.Count; i++)
            {
                PBS.Main.Pokemon.Pokemon pokemon = pokemonToControl[i];
                switchPosition = pokemonToControl[i].battlePos;
                List<PBS.Main.Pokemon.Pokemon> otherCommandablePokemon = 
                    pokemonToControl.GetRange(i + 1, pokemonToControl.Count - i - 1);

                BattleCommand selectedCommand = SelectCommand(
                    pokemon,
                    trainer,
                    committedCommands,
                    otherCommandablePokemon
                    );
                committedCommands.Add(selectedCommand);
            }
            return committedCommands;
        }

        public List<BattleCommand> GetReplacementsByPrompt(
            Trainer trainer,
            List<int> fillPositions)
        {
            committedCommands = new List<BattleCommand>();
            for (int i = 0; i < fillPositions.Count; i++)
            {
                switchPosition = fillPositions[i];
                BattleCommand selectedReplacement = SelectReplacement(
                    trainer,
                    committedCommands
                    );
                committedCommands.Add(selectedReplacement);
            }
            return committedCommands;
        }

        // Specific Pokemon Prompts
        private BattleCommand SelectCommand(
            PBS.Main.Pokemon.Pokemon pokemon,
            Trainer trainer,
            List<BattleCommand> setCommands,
            List<PBS.Main.Pokemon.Pokemon> otherCommandablePokemon
            )
        {
            // keep a list of potential commands, then choose the best one at the end
            List<BattleCommand> potentialCommands = new List<BattleCommand>();

            // TODO: sophisticated AI
            // just selects a random move for now
            potentialCommands.AddRange(GetMoveCommands(
                pokemon,
                trainer,
                setCommands,
                otherCommandablePokemon
                ));

            potentialCommands = model.GetAbleCommands(potentialCommands, setCommands);
            return GetBestCommand(potentialCommands);
        }

        private BattleCommand SelectReplacement(
            Trainer trainer,
            List<BattleCommand> setCommands
            )
        {
            // keep a list of potential commands, then choose the best one at the end
            List<BattleCommand> potentialCommands = new List<BattleCommand>();

            potentialCommands.AddRange(GetPartyCommands(
                null,
                trainer,
                committedCommands,
                new List<PBS.Main.Pokemon.Pokemon> { },
                true
                ));

            potentialCommands = model.GetAbleCommands(potentialCommands, setCommands);
            return GetBestCommand(potentialCommands);
        }

        // Move Commands
        private List<BattleCommand> GetMoveCommands(
            PBS.Main.Pokemon.Pokemon pokemon,
            Trainer trainer,
            List<BattleCommand> setCommands,
            List<PBS.Main.Pokemon.Pokemon> otherCommandablePokemon)
        {
            List<BattleCommand> commands = new List<BattleCommand>();
            List<Moveslot> useableMoves = model.GetPokemonUseableMoves(pokemon);
            if (useableMoves.Count == 0)
            {
                // Struggle
                commands.Add(model.GetStruggleCommand(pokemon));
            }
            else
            {
                for (int i = 0; i < useableMoves.Count; i++)
                {
                    // TODO: proper targeting system
                    BattleCommand moveCommand = BattleCommand.CreateMoveCommand(
                        commandUser: pokemon,
                        moveID: useableMoves[i].moveID,
                        targetPositions: 
                            model.GetMoveAutoTargets(
                                pokemon, 
                                Moves.instance.GetMoveData(useableMoves[i].moveID)),
                        isExplicitlySelected: true);
                    commands.Add(moveCommand);
                }
            }
            return commands;
        }
    
        // Party Commands
        private List<BattleCommand> GetPartyCommands(
            PBS.Main.Pokemon.Pokemon pokemon,
            Trainer trainer,
            List<BattleCommand> setCommands,
            List<PBS.Main.Pokemon.Pokemon> otherCommandablePokemon,
            bool forceReplace = false)
        {
            List<BattleCommand> commands = new List<BattleCommand>();
            List<PBS.Main.Pokemon.Pokemon> availablePokemon = model.GetTrainerFirstXAvailablePokemon(trainer, trainer.party.Count);

            for (int i = 0; i < availablePokemon.Count; i++)
            {
                BattleCommand partyCommand = (forceReplace) ?
                    BattleCommand.CreateReplaceCommand(
                        switchPosition,
                        trainer,
                        availablePokemon[i],
                        true)
                    :
                    BattleCommand.CreateSwitchCommand(
                        pokemon,
                        switchPosition,
                        trainer,
                        availablePokemon[i],
                        true);
                commands.Add(partyCommand);
            }
            return commands;
        }

    
        // Command Judgement
        private BattleCommand GetBestCommand(
            List<BattleCommand> choices
            )
        {
            return choices[Random.Range(0, choices.Count)];
        }
    }
}
