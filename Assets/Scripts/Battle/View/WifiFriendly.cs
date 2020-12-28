using PBS.Enums.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.WifiFriendly
{
    public class Pokemon
    {
        public string uniqueID;
        public string pokemonID;
        public string nickname;

        public int teamPos;
        public int battlePos;

        public int currentHP;
        public int maxHP;
        public bool isFainted;

        public int level;
        public PokemonGender gender;
        public string nonVolatileStatus;
        public Main.Pokemon.Pokemon.DynamaxState dynamaxState;

        public Pokemon() { }
        public Pokemon(Main.Pokemon.Pokemon obj)
        {
            uniqueID = obj.uniqueID;
            pokemonID = obj.pokemonID;
            nickname = obj.nickname;
            teamPos = obj.teamPos;
            battlePos = obj.battlePos;
            currentHP = obj.currentHP;
            maxHP = obj.maxHP;
            isFainted = !obj.IsAbleToBattle();
            level = obj.level;
            gender = obj.gender;
            nonVolatileStatus = (obj.nonVolatileStatus == null)? "" : obj.nonVolatileStatus.statusID;
            dynamaxState = obj.dynamaxState;
        }

        public void Update(Pokemon obj)
        {
            uniqueID = obj.uniqueID;
            pokemonID = obj.pokemonID;
            nickname = obj.nickname;
            teamPos = obj.teamPos;
            battlePos = obj.battlePos;
            currentHP = obj.currentHP;
            maxHP = obj.maxHP;
            isFainted = obj.isFainted;
            level = obj.level;
            gender = obj.gender;
            nonVolatileStatus = obj.nonVolatileStatus;
            dynamaxState = obj.dynamaxState;
        }
    }

    public class Trainer
    {
        public string name;
        public int playerID;
        public int teamPos;
        public List<Pokemon> party;
        public List<string> partyIDs = new List<string>();
        public List<string> items;
        public List<int> controlPos;

        public Trainer() { }
        public Trainer(Main.Trainer.Trainer obj)
        {
            name = obj.name;
            playerID = obj.playerID;
            teamPos = obj.teamID;

            party = new List<Pokemon>();
            for (int i = 0; i < obj.party.Count; i++)
            {
                party.Add(new Pokemon(obj.party[i]));
            }

            items = new List<string>();
            for (int i = 0; i < obj.items.Count; i++)
            {
                items.Add(obj.items[i].itemID);
            }

            controlPos = new List<int>(obj.controlPos);
        }

        public Pokemon GetPokemon(string uniqueID)
        {
            for (int i = 0; i < party.Count; i++)
            {
                if (party[i].uniqueID == uniqueID)
                {
                    return party[i];
                }
            }
            return null;
        }
    }

    public class Team
    {
        public int teamID;
        public TeamMode teamMode;
        public List<WifiFriendly.Trainer> trainers;
        public List<int> playerIDs = new List<int>();

        public Team() { }
        public Team(global::BattleTeam obj)
        {
            teamID = obj.teamID;
            teamMode = (obj.teamMode == BattleTeam.TeamMode.Single)? TeamMode.Single
                : (obj.teamMode == BattleTeam.TeamMode.Double)? TeamMode.Double
                : TeamMode.Triple;
            trainers = new List<Trainer>();
            for (int i = 0; i < obj.trainers.Count; i++)
            {
                trainers.Add(new Trainer(obj.trainers[i]));
            }
        }

        public WifiFriendly.Trainer GetTrainer(int playerID)
        {
            for (int i = 0; i < trainers.Count; i++)
            {
                if (trainers[i].playerID == playerID)
                {
                    return trainers[i];
                }
            }
            return null;
        }
    }
}
