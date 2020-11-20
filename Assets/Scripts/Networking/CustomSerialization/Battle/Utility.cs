using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking.CustomSerialization.Battle
{
    public static class Utility
    {

        public static void WriteBattleUtilityPosition(this NetworkWriter writer, BattlePosition obj)
        {
            writer.WriteInt32(obj.battlePos);
            writer.WriteInt32(obj.teamPos);
        }
        public static BattlePosition ReadBattleUtilityPosition(this NetworkReader reader)
        {
            return new BattlePosition
            (
                battlePos: reader.ReadInt32(),
                teamPos: reader.ReadInt32()
            );
        }
    
        public static void WriteBattleUtilityPositionList(this NetworkWriter writer, List<BattlePosition> obj)
        {
            writer.WriteList(obj);
        }
        public static List<BattlePosition> ReadBattleUtilityPositionList(this NetworkReader reader)
        {
            return reader.ReadList<BattlePosition>();
        }
    }
}