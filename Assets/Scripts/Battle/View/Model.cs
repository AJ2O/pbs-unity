using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View
{

    /// <summary>
    /// This is the limited view of the battle model seen by the player.
    /// </summary>
    public class Model
    {
        public BattleSettings settings;
        public List<PBS.Battle.View.Compact.Team> teams;

        public Model() { }
        public Model(PBS.Battle.Core.Model obj)
        {
            teams = new List<Compact.Team>();
            for (int i = 0; i < obj.teams.Count; i++)
            {
                teams.Add(new Compact.Team(obj.teams[i]));
            }
        }

        // Matching methods
        public PBS.Battle.View.Compact.Pokemon GetMatchingPokemon(PBS.Battle.View.Compact.Pokemon searchPokemon)
        {
            return GetMatchingPokemon(searchPokemon.uniqueID);
        }
        public PBS.Battle.View.Compact.Pokemon GetMatchingPokemon(string uniqueID)
        {
            List<PBS.Battle.View.Compact.Pokemon> allPokemon = GetPokemonFromAllTrainers();
            for (int i = 0; i < allPokemon.Count; i++)
            {
                if (allPokemon[i].uniqueID == uniqueID)
                {
                    return allPokemon[i];
                }
            }
            return null;
        }
        
        public PBS.Battle.View.Compact.Trainer GetMatchingTrainer(PBS.Battle.View.Compact.Trainer searchTrainer)
        {
            return GetMatchingTrainer(searchTrainer.playerID);
        }
        public PBS.Battle.View.Compact.Trainer GetMatchingTrainer(int playerID)
        {
            List<PBS.Battle.View.Compact.Trainer> allTrainers = GetTrainers();
            for (int i = 0; i < allTrainers.Count; i++)
            {
                if (allTrainers[i].playerID == playerID)
                {
                    return allTrainers[i];
                }
            }
            return null;
        }

        public PBS.Battle.View.Compact.Team GetMatchingTeam(PBS.Battle.View.Compact.Team searchTeam)
        {
            return GetMatchingTeam(searchTeam.teamPos);
        }
        public PBS.Battle.View.Compact.Team GetMatchingTeam(int teamID)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamPos == teamID)
                {
                    return teams[i];
                }
            }
            return null;
        }

        // Pokemon
        public List<PBS.Battle.View.Compact.Pokemon> GetPokemonFromTeam(PBS.Battle.View.Compact.Team team)
        {
            List<PBS.Battle.View.Compact.Pokemon> allPokemon = new List<PBS.Battle.View.Compact.Pokemon>();
            for (int i = 0; i < team.trainers.Count; i++)
            {
                allPokemon.AddRange(team.trainers[i].party);
            }
            return allPokemon;
        }
        public List<PBS.Battle.View.Compact.Pokemon> GetPokemonFromAllTrainers()
        {
            List<PBS.Battle.View.Compact.Pokemon> allPokemon = new List<PBS.Battle.View.Compact.Pokemon>();
            for (int i = 0; i < teams.Count; i++)
            {
                allPokemon.AddRange(GetPokemonFromTeam(teams[i]));
            }
            return allPokemon;
        }


        // Trainer
        public PBS.Battle.View.Compact.Trainer GetTrainer(PBS.Battle.View.Compact.Pokemon pokemon)
        {
            return GetTrainer(pokemon.uniqueID);
        } 
        public PBS.Battle.View.Compact.Trainer GetTrainer(string pokemonUniqueID)
        {
            List<PBS.Battle.View.Compact.Trainer> trainers = GetTrainers();
            for (int i = 0; i < trainers.Count; i++)
            {
                PBS.Battle.View.Compact.Trainer trainer = trainers[i];
                for (int j = 0; j < trainer.party.Count; j++)
                {
                    if (pokemonUniqueID == trainer.party[j].uniqueID)
                    {
                        return trainer;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Returns all the trainers in the battle.
        /// </summary>
        /// <returns></returns>
        public List<PBS.Battle.View.Compact.Trainer> GetTrainers()
        {
            List<PBS.Battle.View.Compact.Trainer> allTrainers = new List<PBS.Battle.View.Compact.Trainer>();
            for (int i = 0; i < teams.Count; i++)
            {
                allTrainers.AddRange(teams[i].trainers);
            }
            return allTrainers;
        }
        public List<PBS.Battle.View.Compact.Trainer> GetTrainerAllies(PBS.Battle.View.Compact.Trainer trainer)
        {
            List<PBS.Battle.View.Compact.Trainer> trainers = new List<PBS.Battle.View.Compact.Trainer>();

            PBS.Battle.View.Compact.Team team = GetTeamOfTrainer(trainer);
            if (team != null)
            {
                for (int i = 0; i < team.trainers.Count; i++)
                {
                    if (team.trainers[i].playerID != trainer.playerID)
                    {
                        trainers.Add(team.trainers[i]);
                    }
                }
            }

            return trainers;
        }
        public List<PBS.Battle.View.Compact.Trainer> GetTrainerEnemies(PBS.Battle.View.Compact.Team teamPerspective)
        {
            List<PBS.Battle.View.Compact.Trainer> trainers = new List<PBS.Battle.View.Compact.Trainer>();
            List<PBS.Battle.View.Compact.Team> enemyTeams = GetTeamEnemies(teamPerspective);

            for (int i = 0; i < enemyTeams.Count; i++)
            {
                trainers.AddRange(enemyTeams[i].trainers);
            }
            return trainers;
        }


        // Team
        public PBS.Battle.View.Compact.Team GetTeamOfTrainer(PBS.Battle.View.Compact.Trainer trainer)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamPos == trainer.teamPos)
                {
                    return teams[i];
                }
            }
            return null;
        }
        public List<PBS.Battle.View.Compact.Team> GetTeamEnemiesOfTrainer(PBS.Battle.View.Compact.Trainer trainer)
        {
            PBS.Battle.View.Compact.Team allyTeam = GetTeamOfTrainer(trainer);
            return GetTeamEnemies(allyTeam);
        }
        public List<PBS.Battle.View.Compact.Team> GetTeamEnemies(PBS.Battle.View.Compact.Team team)
        {
            List<PBS.Battle.View.Compact.Team> enemyTeams = new List<PBS.Battle.View.Compact.Team>();
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamPos != team.teamPos)
                {
                    enemyTeams.Add(teams[i]);
                }
            }
            return enemyTeams;
        }
    }
}
