using PBS.Databases;
using System.Collections.Generic;

namespace PBS.Main.Team
{
    public class BattleProperties
    {
        public class EntryHazard
        {
            public string hazardID;
            public int turnsLeft;
            public int layers;

            public EntryHazard(string moveID, int turnsLeft, int layers)
            {
                hazardID = moveID;
                this.turnsLeft = turnsLeft;
                this.layers = layers;
            }
            public static EntryHazard Clone(EntryHazard other)
            {
                return new EntryHazard(
                    moveID: other.hazardID,
                    turnsLeft: other.turnsLeft,
                    layers: other.layers
                    );
            }
        }
        public List<EntryHazard> entryHazards;

        public class GMaxWildfire
        {
            public string statusID;
            public int turnsLeft;
            public GMaxWildfire(
                string statusID,
                int turnsLeft
                )
            {
                this.statusID = statusID;
                this.turnsLeft = turnsLeft;
            }
            public GMaxWildfire Clone()
            {
                return new GMaxWildfire(statusID, turnsLeft);
            }
        }
        public GMaxWildfire GMaxWildfireStatus;

        public List<TeamCondition> lightScreens;

        public List<Effects.General.Protect> matBlocks;

        public class ReflectScreen
        {
            public string moveID;
            public int turnsLeft;

            public ReflectScreen(string moveID, int turnsLeft)
            {
                this.moveID = moveID;
                this.turnsLeft = turnsLeft;
            }
            public static ReflectScreen Clone(ReflectScreen other)
            {
                return new ReflectScreen(
                    moveID: other.moveID,
                    turnsLeft: other.turnsLeft
                    );
            }
        }
        public List<ReflectScreen> reflectScreens;

        public class Safeguard
        {
            public string moveID;
            public int turnsLeft;

            public Safeguard(string moveID, int turnsLeft)
            {
                this.moveID = moveID;
                this.turnsLeft = turnsLeft;
            }
            public static Safeguard Clone(Safeguard other)
            {
                return new Safeguard(
                    moveID: other.moveID,
                    turnsLeft: other.turnsLeft
                    );
            }
        }
        public List<Safeguard> safeguards;

        public List<string> protectMovesActive;
        public List<TeamCondition> conditions;

        // Constructor
        public BattleProperties()
        {
            Reset();
        }

        // Clone
        public static BattleProperties Clone(BattleProperties original)
        {
            BattleProperties clone = new BattleProperties();

            for (int i = 0; i < original.entryHazards.Count; i++)
            {
                clone.entryHazards.Add(EntryHazard.Clone(original.entryHazards[i]));
            }

            clone.GMaxWildfireStatus = original.GMaxWildfireStatus == null ? null : original.GMaxWildfireStatus.Clone();

            for (int i = 0; i < original.lightScreens.Count; i++)
            {
                clone.lightScreens.Add(TeamCondition.Clone(original.lightScreens[i]));
            }
            for (int i = 0; i < original.matBlocks.Count; i++)
            {
                clone.matBlocks.Add(original.matBlocks[i].Clone());
            }

            for (int i = 0; i < original.reflectScreens.Count; i++)
            {
                clone.reflectScreens.Add(ReflectScreen.Clone(original.reflectScreens[i]));
            }

            for (int i = 0; i < original.safeguards.Count; i++)
            {
                clone.safeguards.Add(Safeguard.Clone(original.safeguards[i]));
            }

            clone.protectMovesActive = new List<string>(original.protectMovesActive);

            for (int i = 0; i < original.conditions.Count; i++)
            {
                clone.conditions.Add(TeamCondition.Clone(original.conditions[i]));
            }

            return clone;
        }

        public void Reset()
        {
            entryHazards = new List<EntryHazard>();
            GMaxWildfireStatus = null;
            lightScreens = new List<TeamCondition>();
            matBlocks = new List<Effects.General.Protect>();
            reflectScreens = new List<ReflectScreen>();
            safeguards = new List<Safeguard>();

            protectMovesActive = new List<string>();

            conditions = new List<TeamCondition>();
        }
    }
}