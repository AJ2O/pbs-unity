using PBS.Data;
using PBS.Databases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Main.Pokemon
{
    public class Moveslot
    {
        public string moveID;
        public int PP;
        public int PPUps;
        private int p_maxPP;
        public int maxPP
        {
            get
            {
                if (p_maxPP >= 0)
                {
                    return p_maxPP;
                }

                int basePP = Moves.instance.GetMoveData(moveID).PP;
                int bonusPP = basePP / 5 * PPUps;
                return basePP + bonusPP;
            }
        }
        public MoveData data
        {
            get
            {
                return Moves.instance.GetMoveData(moveID);
            }
        }

        // Constructor
        public Moveslot(string moveID, int PPUps = 0, int PP = -1, int p_maxPP = -1)
        {
            this.p_maxPP = p_maxPP;

            this.moveID = moveID;
            this.PPUps = PPUps;

            if (PP < 0)
            {
                this.PP = maxPP;
            }
            else
            {
                this.PP = PP;
            }
        }

        // Clone
        public static Moveslot Clone(Moveslot original)
        {
            Moveslot cloneSlot = new Moveslot(
                moveID: original.moveID,
                PPUps: original.PPUps,
                PP: original.PP,
                p_maxPP: original.p_maxPP
                );
            return cloneSlot;
        }
    }
}