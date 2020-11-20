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
        public BattleSettings settings = new BattleSettings();
        public List<PBS.Battle.View.WifiFriendly.Team> teams = new List<WifiFriendly.Team>();

        public Model() { }
        public Model(Battle.Model obj)
        {
            settings = obj.battleSettings;
            teams = new List<WifiFriendly.Team>();
            for (int i = 0; i < obj.teams.Count; i++)
            {
                teams.Add(new WifiFriendly.Team(obj.teams[i]));
            }
        }

        // Matching methods
        public PBS.Battle.View.WifiFriendly.Pokemon GetMatchingPokemon(PBS.Battle.View.Events.CommandAgent agent)
        {
            if (agent == null)
            {
                return null;
            }
            return GetMatchingPokemon(agent.pokemonUniqueID);
        }
        public PBS.Battle.View.WifiFriendly.Pokemon GetMatchingPokemon(PBS.Battle.View.WifiFriendly.Pokemon searchPokemon)
        {
            if (searchPokemon == null)
            {
                return null;
            }
            return GetMatchingPokemon(searchPokemon.uniqueID);
        }
        public PBS.Battle.View.WifiFriendly.Pokemon GetMatchingPokemon(string uniqueID)
        {
            List<PBS.Battle.View.WifiFriendly.Pokemon> allPokemon = GetPokemonFromAllTrainers();
            for (int i = 0; i < allPokemon.Count; i++)
            {
                if (allPokemon[i].uniqueID == uniqueID)
                {
                    return allPokemon[i];
                }
            }
            Debug.LogWarning("Couldn't find pokemon with ID " + uniqueID);
            return null;
        }
        
        public PBS.Battle.View.WifiFriendly.Trainer GetMatchingTrainer(PBS.Battle.View.WifiFriendly.Trainer searchTrainer)
        {
            return GetMatchingTrainer(searchTrainer.playerID);
        }
        public PBS.Battle.View.WifiFriendly.Trainer GetMatchingTrainer(int playerID)
        {
            List<PBS.Battle.View.WifiFriendly.Trainer> allTrainers = GetTrainers();
            for (int i = 0; i < allTrainers.Count; i++)
            {
                if (allTrainers[i].playerID == playerID)
                {
                    return allTrainers[i];
                }
            }
            Debug.LogWarning("Couldn't find player with ID " + playerID);
            return null;
        }

        public PBS.Battle.View.WifiFriendly.Team GetMatchingTeam(PBS.Battle.View.WifiFriendly.Team searchTeam)
        {
            return GetMatchingTeam(searchTeam.teamID);
        }
        public PBS.Battle.View.WifiFriendly.Team GetMatchingTeam(int teamID)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamID == teamID)
                {
                    return teams[i];
                }
            }
            Debug.LogWarning("Couldn't find team with ID " + teamID);
            return null;
        }


        // Pokemon
        public List<PBS.Battle.View.WifiFriendly.Pokemon> GetPokemonFromTeam(PBS.Battle.View.WifiFriendly.Team team)
        {
            List<PBS.Battle.View.WifiFriendly.Pokemon> allPokemon = new List<PBS.Battle.View.WifiFriendly.Pokemon>();
            for (int i = 0; i < team.trainers.Count; i++)
            {
                allPokemon.AddRange(team.trainers[i].party);
            }
            return allPokemon;
        }
        public List<PBS.Battle.View.WifiFriendly.Pokemon> GetPokemonFromAllTrainers()
        {
            List<PBS.Battle.View.WifiFriendly.Pokemon> allPokemon = new List<PBS.Battle.View.WifiFriendly.Pokemon>();
            for (int i = 0; i < teams.Count; i++)
            {
                allPokemon.AddRange(GetPokemonFromTeam(teams[i]));
            }
            return allPokemon;
        }
        public bool IsPokemonOnField(PBS.Battle.View.WifiFriendly.Pokemon pokemon)
        {
            return pokemon.battlePos >= 0;
        }
        public BattlePosition GetPokemonPosition(PBS.Battle.View.WifiFriendly.Pokemon pokemon)
        {
            return new BattlePosition(teamPos: pokemon.teamPos, battlePos: pokemon.battlePos);
        }
        public BattlePosition GetPokemonPosition(PBS.Battle.View.Events.CommandAgent pokemon)
        {
            return GetPokemonPosition(GetMatchingPokemon(pokemon.pokemonUniqueID));
        }
        public List<BattlePosition> GetAllBattlePositions() 
        {
            List<BattlePosition> battlePositions = new List<BattlePosition>();
            for (int i = 0; i < teams.Count; i++)
            {
                int teamPos = teams[i].teamID;
                for (int j = 0; j < teams[i].trainers.Count; j++)
                {
                    for (int k = 0; k < teams[i].trainers[j].controlPos.Count; k++)
                    {
                        int battlePos = teams[i].trainers[j].controlPos[k];
                        battlePositions.Add(new BattlePosition(teamPos, battlePos));
                    }
                }
            }
            return battlePositions;
        }
        public PBS.Battle.View.WifiFriendly.Pokemon GetPokemonAtPosition(BattlePosition position)
        {
            List<PBS.Battle.View.WifiFriendly.Pokemon> allPokemon = GetPokemonFromAllTrainers();
            for (int i = 0; i < allPokemon.Count; i++)
            {
                if (allPokemon[i].teamPos == position.teamPos
                    && allPokemon[i].battlePos == position.battlePos)
                {
                    return allPokemon[i];
                }
            }
            return null;
        }


        // Trainer
        public PBS.Battle.View.WifiFriendly.Trainer GetTrainer(PBS.Battle.View.WifiFriendly.Pokemon pokemon)
        {
            return GetTrainer(pokemon.uniqueID);
        } 
        public PBS.Battle.View.WifiFriendly.Trainer GetTrainer(string pokemonUniqueID)
        {
            List<PBS.Battle.View.WifiFriendly.Trainer> trainers = GetTrainers();
            for (int i = 0; i < trainers.Count; i++)
            {
                PBS.Battle.View.WifiFriendly.Trainer trainer = trainers[i];
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
        public List<PBS.Battle.View.WifiFriendly.Trainer> GetTrainers()
        {
            List<PBS.Battle.View.WifiFriendly.Trainer> allTrainers = new List<PBS.Battle.View.WifiFriendly.Trainer>();
            for (int i = 0; i < teams.Count; i++)
            {
                allTrainers.AddRange(teams[i].trainers);
            }
            return allTrainers;
        }
        public List<PBS.Battle.View.WifiFriendly.Trainer> GetTrainerAllies(PBS.Battle.View.WifiFriendly.Trainer trainer)
        {
            List<PBS.Battle.View.WifiFriendly.Trainer> trainers = new List<PBS.Battle.View.WifiFriendly.Trainer>();

            PBS.Battle.View.WifiFriendly.Team team = GetTeamOfTrainer(trainer);
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
        public List<PBS.Battle.View.WifiFriendly.Trainer> GetTrainerEnemies(PBS.Battle.View.WifiFriendly.Team teamPerspective)
        {
            List<PBS.Battle.View.WifiFriendly.Trainer> trainers = new List<PBS.Battle.View.WifiFriendly.Trainer>();
            List<PBS.Battle.View.WifiFriendly.Team> enemyTeams = GetTeamEnemies(teamPerspective);

            for (int i = 0; i < enemyTeams.Count; i++)
            {
                trainers.AddRange(enemyTeams[i].trainers);
            }
            return trainers;
        }


        // Team
        public PBS.Battle.View.WifiFriendly.Team GetTeamOfTrainer(PBS.Battle.View.WifiFriendly.Trainer trainer)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamID == trainer.teamPos)
                {
                    return teams[i];
                }
            }
            return null;
        }
        public List<PBS.Battle.View.WifiFriendly.Team> GetTeamEnemiesOfTrainer(PBS.Battle.View.WifiFriendly.Trainer trainer)
        {
            PBS.Battle.View.WifiFriendly.Team allyTeam = GetTeamOfTrainer(trainer);
            return GetTeamEnemies(allyTeam);
        }
        public List<PBS.Battle.View.WifiFriendly.Team> GetTeamEnemies(PBS.Battle.View.WifiFriendly.Team team)
        {
            List<PBS.Battle.View.WifiFriendly.Team> enemyTeams = new List<PBS.Battle.View.WifiFriendly.Team>();
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamID != team.teamID)
                {
                    enemyTeams.Add(teams[i]);
                }
            }
            return enemyTeams;
        }
    }
}
