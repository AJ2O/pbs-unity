using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking.CustomSerialization.Player
{
    public static class Main
    {
        public static void WritePlayerCommand(this NetworkWriter writer, PBS.Player.Command obj)
        {
            writer.WriteInt32((int)obj.commandType);
            writer.WriteString(obj.commandUser);
            writer.WriteInt32(obj.commandTrainer);
            writer.WriteBoolean(obj.inProgress);
            writer.WriteBoolean(obj.completed);
            writer.WriteInt32(obj.commandPriority);
            writer.WriteBoolean(obj.isExplicitlySelected);

            writer.WriteString(obj.moveID);
            writer.WriteBoolean(obj.consumePP);
            writer.WriteArray(obj.targetPositions);
            writer.WriteBoolean(obj.displayMove);
            writer.WriteBoolean(obj.forceOneHit);
            writer.WriteBoolean(obj.bypassRedirection);
            writer.WriteBoolean(obj.bypassStatusInterrupt);
            writer.WriteBoolean(obj.isDanceMove);
            writer.WriteBoolean(obj.isMoveCalled);
            writer.WriteBoolean(obj.isMoveReflected);
            writer.WriteBoolean(obj.isMoveHijacked);
            writer.WriteBoolean(obj.isFutureSightMove);
            writer.WriteBoolean(obj.isPursuitMove);
            writer.WriteBoolean(obj.isMoveSnatched);
            writer.WriteBoolean(obj.isMagicCoatMove);

            writer.WriteBoolean(obj.isMegaEvolving);
            writer.WriteBoolean(obj.isZMove);
            writer.WriteBoolean(obj.isDynamaxing);

            writer.WriteInt32(obj.switchPosition);
            writer.WriteInt32(obj.switchingTrainer);
            writer.WriteString(obj.switchInPokemon);

            writer.WriteString(obj.itemID);
            writer.WriteInt32(obj.itemTrainer);
        }
        public static PBS.Player.Command ReadPlayerCommand(this NetworkReader reader)
        {
            return new PBS.Player.Command
            {
                commandType = (BattleCommandType)reader.ReadInt32(),
                commandUser = reader.ReadString(),
                commandTrainer = reader.ReadInt32(),
                inProgress = reader.ReadBoolean(),
                completed = reader.ReadBoolean(),
                commandPriority = reader.ReadInt32(),
                isExplicitlySelected = reader.ReadBoolean(),

                moveID = reader.ReadString(),
                consumePP = reader.ReadBoolean(),
                targetPositions = reader.ReadArray<BattlePosition>(),
                displayMove = reader.ReadBoolean(),
                forceOneHit = reader.ReadBoolean(),
                bypassRedirection = reader.ReadBoolean(),
                bypassStatusInterrupt = reader.ReadBoolean(),
                isDanceMove = reader.ReadBoolean(),
                isMoveCalled = reader.ReadBoolean(),
                isMoveReflected = reader.ReadBoolean(),
                isMoveHijacked = reader.ReadBoolean(),
                isFutureSightMove = reader.ReadBoolean(),
                isPursuitMove = reader.ReadBoolean(),
                isMoveSnatched = reader.ReadBoolean(),
                isMagicCoatMove = reader.ReadBoolean(),

                isMegaEvolving = reader.ReadBoolean(),
                isZMove = reader.ReadBoolean(),
                isDynamaxing = reader.ReadBoolean(),

                switchPosition = reader.ReadInt32(),
                switchingTrainer = reader.ReadInt32(),
                switchInPokemon = reader.ReadString(),

                itemID = reader.ReadString(),
                itemTrainer = reader.ReadInt32()
            };
        }
    }
}
