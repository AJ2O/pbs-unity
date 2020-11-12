using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking
{
    public static class CustomSerialization
    {
        // Pokemon


        // Trainer
        public static void WriteTrainer(this NetworkWriter writer, Trainer trainer)
        {
            writer.WriteString(trainer.name);
            writer.WriteInt32(trainer.playerID);
            writer.WriteInt32(trainer.teamPos);
            writer.WriteBoolean(trainer.isWildPokemon);

            writer.WriteItem(trainer.megaRing);
            writer.WriteItem(trainer.ZRing);
            writer.WriteItem(trainer.dynamaxBand);

            Debug.Log("Before writing pokemon");
            writer.WriteList<Pokemon>(trainer.party);
            writer.WriteList<Item>(trainer.items);

            writer.WriteArray<int>(trainer.controlPos);
            writer.Write<TrainerBattleProperties>(trainer.bProps);
        }
        public static Trainer ReadTrainer(this NetworkReader reader)
        {
            Trainer cloneTrainer = new Trainer(
                name: reader.ReadString(),
                playerID: reader.ReadInt32(),
                teamPos: reader.ReadInt32(),
                isWildPokemon: reader.ReadBoolean(),

                megaRing: reader.ReadItem(),
                ZRing: reader.ReadItem(),
                dynamaxBand: reader.ReadItem(),

                party: reader.ReadList<Pokemon>(),
                items: reader.ReadList<Item>()
            );
            cloneTrainer.controlPos = reader.ReadArray<int>();
            cloneTrainer.bProps = reader.Read<TrainerBattleProperties>();
            return cloneTrainer;
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



        // View

    }
}

