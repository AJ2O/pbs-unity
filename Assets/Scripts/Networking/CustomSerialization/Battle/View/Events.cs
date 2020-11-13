using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking.CustomSerialization.Battle.View
{
    public static class Events
    {
        const int BASE = 0;

        // Battle Phases (1 - 99)
        const int STARTBATTLE = 1;
        const int ENDBATTLE = 2;

        // Messages (101 - 199)
        const int MESSAGE = 101;

        // Backend (201 - 299)
        const int MODELUPDATE = 201;

        // Trainer Interactions (1001 - 1099)

        const int TRAINERSENDOUT = 1001;
        const int TRAINERMULTISENDOUT = 1002;

        public static void WriteBattleViewEvent(this NetworkWriter writer, PBS.Battle.View.Events.Base obj)
        {
            if (obj is PBS.Battle.View.Events.Message message)
            {
                writer.WriteInt32(MESSAGE);
                writer.WriteString(message.message);
            }
            else if (obj is PBS.Battle.View.Events.ModelUpdate modelUpdate)
            {
                writer.WriteInt32(MODELUPDATE);
                writer.WriteInt32((int)modelUpdate.updateType);
                writer.WriteBoolean(modelUpdate.synchronize);
                writer.Write(modelUpdate.model);
            }
            else if (obj is PBS.Battle.View.Events.StartBattle startBattle)
            {
                writer.WriteInt32(STARTBATTLE);
            }
            else if (obj is PBS.Battle.View.Events.EndBattle endBattle)
            {
                writer.WriteInt32(ENDBATTLE);
            }
            else if (obj is PBS.Battle.View.Events.TrainerSendOut trainerSendOut)
            {
                writer.WriteInt32(TRAINERSENDOUT);
                writer.WriteInt32(trainerSendOut.playerID);
                writer.WriteList(trainerSendOut.pokemonUniqueIDs);
                Debug.Log("Could write base send out");
            }
            else if (obj is PBS.Battle.View.Events.TrainerMultiSendOut trainerMultiSendOut)
            {
                writer.WriteInt32(TRAINERMULTISENDOUT);
                writer.WriteList(trainerMultiSendOut.sendEvents);
            }
        }

        public static PBS.Battle.View.Events.Base ReadBattleViewEvent(this NetworkReader reader)
        {
            int type = reader.ReadInt32();
            switch(type)
            {
                case STARTBATTLE:
                    return new PBS.Battle.View.Events.StartBattle
                    {

                    };
                case ENDBATTLE:
                    return new PBS.Battle.View.Events.EndBattle
                    {

                    };

                case MESSAGE:
                    return new PBS.Battle.View.Events.Message
                    {
                        message = reader.ReadString()
                    };

                case MODELUPDATE:
                    return new PBS.Battle.View.Events.ModelUpdate
                    {
                        updateType = (PBS.Battle.View.Events.ModelUpdate.UpdateType)reader.ReadInt32(),
                        synchronize = reader.ReadBoolean(),
                        model = reader.Read<PBS.Battle.View.Model>()
                    };

                case TRAINERSENDOUT:
                    return new PBS.Battle.View.Events.TrainerSendOut
                    {
                        playerID = reader.ReadInt32(),
                        pokemonUniqueIDs = reader.ReadList<string>()
                    };
                case TRAINERMULTISENDOUT:
                    return new PBS.Battle.View.Events.TrainerMultiSendOut
                    {
                        sendEvents = reader.ReadList<PBS.Battle.View.Events.TrainerSendOut>()
                    };
                
                default:
                    throw new System.Exception($"Invalid event type {type}");
            }
        }
    }
}
