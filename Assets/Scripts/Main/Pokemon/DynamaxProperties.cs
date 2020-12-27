using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Main.Pokemon
{
    public class DynamaxProperties
    {
        public int dynamaxLevel;
        public string GMaxForm;
        public string GMaxMove;
        public string moveType;
        public int turnsLeft;

        public DynamaxProperties(
            int dynamaxLevel = 0,
            string GMaxForm = null,
            string GMaxMove = null,
            string moveType = null,
            int turnsLeft = 0
            )
        {
            this.dynamaxLevel = dynamaxLevel;
            this.GMaxForm = GMaxForm;
            this.moveType = moveType;
            this.GMaxMove = GMaxMove;
            this.turnsLeft = turnsLeft;
        }
        public DynamaxProperties Clone()
        {
            return new DynamaxProperties(
                dynamaxLevel: dynamaxLevel,
                GMaxForm: GMaxForm,
                moveType: moveType,
                GMaxMove: GMaxMove,
                turnsLeft: turnsLeft
                );
        }
    }
}
