using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PBS.Enums.Battle;
using PBS.Main.Pokemon;

namespace PBS.Networking.CustomSerialization
{
    public static class Main
    {
        // Pokemon
        public static void WriteBattleViewCompactPokemon(this NetworkWriter writer, PBS.Battle.View.WifiFriendly.Pokemon obj)
        {
            writer.WriteString(obj.uniqueID);
            writer.WriteString(obj.pokemonID);
            writer.WriteString(obj.nickname);
            writer.WriteInt32(obj.teamPos);
            writer.WriteInt32(obj.battlePos);
            writer.WriteInt32(obj.currentHP);
            writer.WriteInt32(obj.maxHP);
            writer.WriteBoolean(obj.isFainted);
            writer.WriteInt32(obj.level);
            writer.WriteInt32((int)obj.gender);
            writer.WriteString(obj.nonVolatileStatus);
            writer.WriteInt32((int)obj.dynamaxState);
        }
        public static PBS.Battle.View.WifiFriendly.Pokemon ReadBattleViewCompactPokemon(this NetworkReader reader)
        {
            return new PBS.Battle.View.WifiFriendly.Pokemon
            {
                uniqueID = reader.ReadString(),
                pokemonID = reader.ReadString(),
                nickname = reader.ReadString(),
                teamPos = reader.ReadInt32(),
                battlePos = reader.ReadInt32(),
                currentHP = reader.ReadInt32(),
                maxHP = reader.ReadInt32(),
                isFainted = reader.ReadBoolean(),
                level = reader.ReadInt32(),
                gender = (PokemonGender)reader.ReadInt32(),
                nonVolatileStatus = reader.ReadString(),
                dynamaxState = (Pokemon.DynamaxState)reader.ReadInt32()
            };
        }


        // Trainer
        public static void WriteBattleViewCompactTrainer(this NetworkWriter writer, PBS.Battle.View.WifiFriendly.Trainer obj)
        {
            writer.WriteString(obj.name);
            writer.WriteInt32(obj.playerID);
            writer.WriteInt32(obj.teamPos);
            writer.WriteList(obj.party);
            writer.WriteList(obj.items);
            writer.WriteList(obj.controlPos);
        }
        public static PBS.Battle.View.WifiFriendly.Trainer ReadBattleViewCompactTrainer(this NetworkReader reader)
        {
            return new PBS.Battle.View.WifiFriendly.Trainer
            {
                name = reader.ReadString(),
                playerID = reader.ReadInt32(),
                teamPos = reader.ReadInt32(),
                party = reader.ReadList<PBS.Battle.View.WifiFriendly.Pokemon>(),
                items = reader.ReadList<string>(),
                controlPos = reader.ReadList<int>()
            };
        }


        // Item
        public static void WriteItem(this NetworkWriter writer, Item item)
        {
            writer.WriteString(item.itemID);
            writer.WriteBoolean(item.useable);
        }
        public static Item ReadItem(this NetworkReader reader)
        {
            return new Item(
                reader.ReadString(),
                reader.ReadBoolean());
        }


        // Battle Team
        public static void WriteBattleViewCompactTeam(this NetworkWriter writer, PBS.Battle.View.WifiFriendly.Team obj)
        {
            writer.WriteInt32(obj.teamID);
            writer.WriteInt32((int)obj.teamMode);
            writer.WriteList(obj.trainers);
        }
        public static PBS.Battle.View.WifiFriendly.Team ReadBattleViewCompactTeam(this NetworkReader reader)
        {
            return new PBS.Battle.View.WifiFriendly.Team
            {
                teamID = reader.ReadInt32(),
                teamMode = (TeamMode)reader.ReadInt32(),
                trainers = reader.ReadList<PBS.Battle.View.WifiFriendly.Trainer>()
            };
        }


        // Battle
        public static void WriteBattleSettings(this NetworkWriter writer, BattleSettings obj)
        {
            writer.WriteInt32((int)obj.battleType);
            writer.WriteBoolean(obj.isWildBattle);
            writer.WriteBoolean(obj.isInverse);
            writer.WriteBoolean(obj.canMegaEvolve);
            writer.WriteBoolean(obj.canZMove);
            writer.WriteBoolean(obj.canDynamax);
        }
        public static BattleSettings ReadBattleSettings(this NetworkReader reader)
        {
            return new BattleSettings(
                battleType: (BattleType)reader.ReadInt32(),
                isWildBattle: reader.ReadBoolean(),
                isInverse: reader.ReadBoolean(),
                canMegaEvolve: reader.ReadBoolean(),
                canZMove: reader.ReadBoolean(),
                canDynamax: reader.ReadBoolean()
                );
        }
    }
}

