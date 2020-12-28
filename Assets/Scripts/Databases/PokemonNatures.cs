using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases
{
    public class PokemonNatures
    {
        //create an object of SingleObject
        private static PokemonNatures singleton = new PokemonNatures();

        //make the constructor private so that this class cannot be
        //instantiated
        private PokemonNatures() { }

        //Get the only object available
        public static PokemonNatures instance
        {
            get
            {
                return singleton;
            }
            private set
            {
                singleton = value;
            }
        }

        // Database
        private Dictionary<string, NatureData> database = new Dictionary<string, NatureData>
    {
        // neutral nature
        {"hardy",
            new NatureData(
                ID: "hardy",
                natureName: "Hardy")},

        {"docile",
            new NatureData(
                ID: "docile",
                natureName: "Docile")},

        {"serious",
            new NatureData(
                ID: "serious",
                natureName: "Serious")},

        {"bashful",
            new NatureData(
                ID: "bashful",
                natureName: "Bashful")},

        {"quirky",
            new NatureData(
                ID: "quirky",
                natureName: "Quirky")},

        // + ATK
        {"lonely",
            new NatureData(
                ID: "lonely",
                natureName: "Lonely",
                ATKMod: 1.1f, DEFMod: 0.9f)},

        {"brave",
            new NatureData(
                ID: "brave",
                natureName: "Brave",
                ATKMod: 1.1f, SPEMod: 0.9f)},

        {"adamant",
            new NatureData(
                ID: "adamant",
                natureName: "Adamant",
                ATKMod: 1.1f, SPEMod: 0.9f)},

        {"naughty",
            new NatureData(
                ID: "naughty",
                natureName: "Naughty",
                ATKMod: 1.1f, SPEMod: 0.9f)},

        // + DEF
        {"bold",
            new NatureData(
                ID: "bold",
                natureName: "Bold",
                DEFMod: 1.1f, ATKMod: 0.9f)},

        {"relaxed",
            new NatureData(
                ID: "relaxed",
                natureName: "Relaxed",
                DEFMod: 1.1f, SPEMod: 0.9f)},

        {"impish",
            new NatureData(
                ID: "impish",
                natureName: "Impish",
                DEFMod: 1.1f, SPEMod: 0.9f)},

        {"lax",
            new NatureData(
                ID: "lax",
                natureName: "Lax",
                DEFMod: 1.1f, SPDMod: 0.9f)},

        // + SPA
        {"modest",
            new NatureData(
                ID: "modest",
                natureName: "Modest",
                SPAMod: 1.1f, ATKMod: 0.9f)},

        {"mild",
            new NatureData(
                ID: "mild",
                natureName: "Mild",
                SPAMod: 1.1f, DEFMod: 0.9f)},

        {"quiet",
            new NatureData(
                ID: "quiet",
                natureName: "Quiet",
                SPAMod: 1.1f, SPEMod: 0.9f)},

        {"rash",
            new NatureData(
                ID: "rash",
                natureName: "Rash",
                SPAMod: 1.1f, SPDMod: 0.9f)},

        // + SPD
        {"calm",
            new NatureData(
                ID: "calm",
                natureName: "Calm",
                SPDMod: 1.1f, ATKMod: 0.9f)},

        {"gentle",
            new NatureData(
                ID: "gentle",
                natureName: "Gentle",
                SPDMod: 1.1f, DEFMod: 0.9f)},

        {"sassy",
            new NatureData(
                ID: "sassy",
                natureName: "Sassy",
                SPDMod: 1.1f, SPEMod: 0.9f)},

        {"careful",
            new NatureData(
                ID: "careful",
                natureName: "Careful",
                SPDMod: 1.1f, SPAMod: 0.9f)},

        // + SPE
        {"timid",
            new NatureData(
                ID: "timid",
                natureName: "Timid",
                SPEMod: 1.1f, ATKMod: 0.9f)},

        {"hasty",
            new NatureData(
                ID: "hasty",
                natureName: "Hasty",
                SPEMod: 1.1f, DEFMod: 0.9f)},

        {"jolly",
            new NatureData(
                ID: "jolly",
                natureName: "Jolly",
                SPEMod: 1.1f, SPAMod: 0.9f)},

        {"naive",
            new NatureData(
                ID: "naive",
                natureName: "Naive",
                SPEMod: 1.1f, SPDMod: 0.9f)},

    };

        public NatureData GetNatureData(string ID)
        {
            if (database.ContainsKey(ID))
            {
                return database[ID];
            }
            Debug.LogWarning("Could not find Pokemon with ID: " + ID);
            return database[""];
        }
        public NatureData GetRandomNature()
        {
            List<string> allNatures = new List<string>(database.Keys);
            return GetNatureData(allNatures[Random.Range(0, allNatures.Count)]);
        }

    }
}