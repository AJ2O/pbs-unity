using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Data
{
    public class PokemonNature
    {
        // General
        public string ID { get; private set; }
        public string natureName { get; private set; }

        public float HPMod { get; set; }
        public float ATKMod { get; set; }
        public float DEFMod { get; set; }
        public float SPAMod { get; set; }
        public float SPDMod { get; set; }
        public float SPEMod { get; set; }

        public PokemonNature(
            string ID,
            string natureName = "",
            float HPMod = 1f,
            float ATKMod = 1f, float DEFMod = 1f, float SPAMod = 1f, float SPDMod = 1f, float SPEMod = 1f)
        {
            this.ID = ID;
            this.natureName = natureName;

            this.HPMod = HPMod;
            this.ATKMod = ATKMod;
            this.DEFMod = DEFMod;
            this.SPAMod = SPAMod;
            this.SPDMod = SPDMod;
            this.SPEMod = SPEMod;
        }

    }
}