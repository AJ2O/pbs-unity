using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Main.Pokemon
{
    public class TransformProperties
    {
        public string pokemonID;
        public Moveslot[] moveslots;

        public int ATK;
        public int DEF;
        public int SPA;
        public int SPD;
        public int SPE;

        // Constructor
        private TransformProperties()
        {
            moveslots = new Moveslot[4];
        }
        public TransformProperties(Pokemon pokemon)
        {
            pokemonID = pokemon.pokemonID;

            moveslots = new Moveslot[4];
            for (int i = 0; i < pokemon.moveslots.Length; i++)
            {
                Moveslot slot = pokemon.moveslots[i];
                if (slot != null)
                {
                    moveslots[i] = new Moveslot(moveID: slot.moveID, p_maxPP: Mathf.Min(slot.maxPP, 5));
                }
            }

            ATK = pokemon.ATK;
            DEF = pokemon.DEF;
            SPA = pokemon.SPA;
            SPD = pokemon.SPD;
            SPE = pokemon.SPE;
        }

        // Clone
        public static TransformProperties Clone(TransformProperties original)
        {
            TransformProperties clone = new TransformProperties();

            clone.moveslots = new Moveslot[4];
            for (int i = 0; i < original.moveslots.Length; i++)
            {
                Moveslot slot = original.moveslots[i];
                if (slot != null)
                {
                    clone.moveslots[i] = Moveslot.Clone(original.moveslots[i]);
                }
            }

            clone.ATK = original.ATK;
            clone.DEF = original.DEF;
            clone.SPA = original.SPA;
            clone.SPD = original.SPD;
            clone.SPE = original.SPE;

            return clone;
        }
    }
}
