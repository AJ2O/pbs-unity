using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Compact
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
        public global::Pokemon.DynamaxState dynamaxState;

        public Pokemon() { }
        public Pokemon(global:: Pokemon obj)
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
    }

    public class Trainer
    {
        public string name;
        public int playerID;
        public int teamPos;
        public List<Compact.Pokemon> party;
        public List<string> items;
        public List<int> controlPos;

        public Trainer() { }
        public Trainer(global:: Trainer obj)
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
    }

    public class Team
    {
        public int teamID;
        public Battle.Enums.TeamMode teamMode;
        public List<Compact.Trainer> trainers;

        public Team() { }
        public Team(global::BattleTeam obj)
        {
            teamID = obj.teamID;
            teamMode = (obj.teamMode == BattleTeam.TeamMode.Single)? Battle.Enums.TeamMode.Single
                : (obj.teamMode == BattleTeam.TeamMode.Double)? Battle.Enums.TeamMode.Double
                : Battle.Enums.TeamMode.Triple;
            trainers = new List<Trainer>();
            for (int i = 0; i < obj.trainers.Count; i++)
            {
                trainers.Add(new Trainer(obj.trainers[i]));
            }
        }
    }
}
