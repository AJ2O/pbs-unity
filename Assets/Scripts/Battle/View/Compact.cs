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

        public Pokemon() { }
        public Pokemon(global:: Pokemon obj)
        {
            uniqueID = obj.uniqueID;
            pokemonID = obj.pokemonID;
            nickname = obj.nickname;
            teamPos = obj.teamPos;
            battlePos = obj.battlePos;
        }
    }

    public class Trainer
    {
        public string name;
        public int playerID;
        public int teamPos;
        public List<Compact.Pokemon> party;
        public List<string> items;

        public Trainer() { }
        public Trainer(global:: Trainer obj)
        {
            name = obj.name;
            playerID = obj.playerID;
            teamPos = obj.teamPos;

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
        }
    }

    public class Team
    {
        public int teamPos;
        public List<Compact.Trainer> trainers;

        public Team() { }
        public Team(global::BattleTeam obj)
        {
            teamPos = obj.teamPos;

            trainers = new List<Trainer>();
            for (int i = 0; i < obj.trainers.Count; i++)
            {
                trainers.Add(new Trainer(obj.trainers[i]));
            }
        }
    }
}
