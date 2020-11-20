using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking.CustomSerialization.Player
{
    public static class Query
    {
        const int BASE = 0;

        const int MOVETARGET = 10;

        // Queries
        public static void WritePlayerQuery(this NetworkWriter writer, PBS.Player.Query.QueryBase obj)
        {
            if (obj is PBS.Player.Query.MoveTarget moveTarget)
            {
                writer.WriteInt32(MOVETARGET);
                writer.WriteString(moveTarget.pokemonUniqueID);
                writer.WriteString(moveTarget.moveID);
            }
            else
            {
                writer.WriteInt32(BASE);
            }
        }
        public static PBS.Player.Query.QueryBase ReadBattleViewEvent(this NetworkReader reader)
        {
            int type = reader.ReadInt32();
            switch (type)
            {
                case MOVETARGET:
                    return new PBS.Player.Query.MoveTarget
                    {
                        pokemonUniqueID = reader.ReadString(),
                        moveID = reader.ReadString()
                    };

                default:
                    throw new System.Exception($"Invalid query type {type}");
            }
        }

        // Responses
        public static void WritePlayerQueryResponse(this NetworkWriter writer, PBS.Player.Query.QueryResponseBase obj)
        {
            if (obj is PBS.Player.Query.MoveTargetResponse moveTarget)
            {
                writer.WriteInt32(MOVETARGET);
                writer.WriteList(moveTarget.possibleTargets);
            }
            else
            {
                writer.WriteInt32(BASE);
            }
        }
        public static PBS.Player.Query.QueryResponseBase ReadPlayerQueryResponse(this NetworkReader reader)
        {
            int type = reader.ReadInt32();
            switch (type)
            {
                case MOVETARGET:
                    return new PBS.Player.Query.MoveTargetResponse
                    {
                        possibleTargets = reader.ReadList<List<BattlePosition>>()
                    };

                default:
                    throw new System.Exception($"Invalid query type {type}");
            }
        }


    }
}

