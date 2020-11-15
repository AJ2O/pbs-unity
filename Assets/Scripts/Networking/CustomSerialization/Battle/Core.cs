using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking.CustomSerialization.Battle
{
    public static class Core
    {
        public static void WriteBattleViewModel(this NetworkWriter writer, PBS.Battle.View.Model obj)
        {
            writer.WriteBattleSettings(obj.settings);
            writer.WriteList(obj.teams);
        }
        public static PBS.Battle.View.Model ReadBattleViewModel(this NetworkReader reader)
        {
           PBS.Battle.View.Model obj = new PBS.Battle.View.Model
           {
               settings = reader.ReadBattleSettings(),
               teams = reader.ReadList<PBS.Battle.View.Compact.Team>()
           };
           return obj;
        }
    }
}
