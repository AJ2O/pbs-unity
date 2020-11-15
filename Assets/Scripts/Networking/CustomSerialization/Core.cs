using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking.CustomSerialization
{
    public static class Core
    {
        // Pokemon
        public static void WriteBattleViewCompactPokemon(this NetworkWriter writer, PBS.Battle.View.Compact.Pokemon obj)
        {
            writer.WriteString(obj.uniqueID);
            writer.WriteString(obj.pokemonID);
            writer.WriteString(obj.nickname);
            writer.WriteInt32(obj.teamPos);
            writer.WriteInt32(obj.battlePos);
        }
        public static PBS.Battle.View.Compact.Pokemon ReadBattleViewCompactPokemon(this NetworkReader reader)
        {
            return new PBS.Battle.View.Compact.Pokemon
            {
                uniqueID = reader.ReadString(),
                pokemonID = reader.ReadString(),
                nickname = reader.ReadString(),
                teamPos = reader.ReadInt32(),
                battlePos = reader.ReadInt32()
            };
        }


        // Trainer
        public static void WriteBattleViewCompactTrainer(this NetworkWriter writer, PBS.Battle.View.Compact.Trainer obj)
        {
            writer.WriteString(obj.name);
            writer.WriteInt32(obj.playerID);
            writer.WriteInt32(obj.teamPos);
            writer.WriteList(obj.party);
            writer.WriteList(obj.items);
        }
        public static PBS.Battle.View.Compact.Trainer ReadBattleViewCompactTrainer(this NetworkReader reader)
        {
            return new PBS.Battle.View.Compact.Trainer
            {
                name = reader.ReadString(),
                playerID = reader.ReadInt32(),
                teamPos = reader.ReadInt32(),
                party = reader.ReadList<PBS.Battle.View.Compact.Pokemon>(),
                items = reader.ReadList<string>()
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
        public static void WriteBattleViewCompactTeam(this NetworkWriter writer, PBS.Battle.View.Compact.Team obj)
        {
            writer.WriteInt32(obj.teamPos);
            writer.WriteList(obj.trainers);
        }
        public static PBS.Battle.View.Compact.Team ReadBattleViewCompactTeam(this NetworkReader reader)
        {
            return new PBS.Battle.View.Compact.Team
            {
                teamPos = reader.ReadInt32(),
                trainers = reader.ReadList<PBS.Battle.View.Compact.Trainer>()
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

