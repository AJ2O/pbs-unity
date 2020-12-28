using PBS.Data;
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
        private Dictionary<string, PokemonNature> database = new Dictionary<string, PokemonNature>
    {
        // neutral nature
        {"hardy",
            new PokemonNature(
                ID: "hardy",
                natureName: "Hardy")},

        {"docile",
            new PokemonNature(
                ID: "docile",
                natureName: "Docile")},

        {"serious",
            new PokemonNature(
                ID: "serious",
                natureName: "Serious")},

        {"bashful",
            new PokemonNature(
                ID: "bashful",
                natureName: "Bashful")},

        {"quirky",
            new PokemonNature(
                ID: "quirky",
                natureName: "Quirky")},

        // + ATK
        {"lonely",
            new PokemonNature(
                ID: "lonely",
                natureName: "Lonely",
                ATKMod: 1.1f, DEFMod: 0.9f)},

        {"brave",
            new PokemonNature(
                ID: "brave",
                natureName: "Brave",
                ATKMod: 1.1f, SPEMod: 0.9f)},

        {"adamant",
            new PokemonNature(
                ID: "adamant",
                natureName: "Adamant",
                ATKMod: 1.1f, SPEMod: 0.9f)},

        {"naughty",
            new PokemonNature(
                ID: "naughty",
                natureName: "Naughty",
                ATKMod: 1.1f, SPEMod: 0.9f)},

        // + DEF
        {"bold",
            new PokemonNature(
                ID: "bold",
                natureName: "Bold",
                DEFMod: 1.1f, ATKMod: 0.9f)},

        {"relaxed",
            new PokemonNature(
                ID: "relaxed",
                natureName: "Relaxed",
                DEFMod: 1.1f, SPEMod: 0.9f)},

        {"impish",
            new PokemonNature(
                ID: "impish",
                natureName: "Impish",
                DEFMod: 1.1f, SPEMod: 0.9f)},

        {"lax",
            new PokemonNature(
                ID: "lax",
                natureName: "Lax",
                DEFMod: 1.1f, SPDMod: 0.9f)},

        // + SPA
        {"modest",
            new PokemonNature(
                ID: "modest",
                natureName: "Modest",
                SPAMod: 1.1f, ATKMod: 0.9f)},

        {"mild",
            new PokemonNature(
                ID: "mild",
                natureName: "Mild",
                SPAMod: 1.1f, DEFMod: 0.9f)},

        {"quiet",
            new PokemonNature(
                ID: "quiet",
                natureName: "Quiet",
                SPAMod: 1.1f, SPEMod: 0.9f)},

        {"rash",
            new PokemonNature(
                ID: "rash",
                natureName: "Rash",
                SPAMod: 1.1f, SPDMod: 0.9f)},

        // + SPD
        {"calm",
            new PokemonNature(
                ID: "calm",
                natureName: "Calm",
                SPDMod: 1.1f, ATKMod: 0.9f)},

        {"gentle",
            new PokemonNature(
                ID: "gentle",
                natureName: "Gentle",
                SPDMod: 1.1f, DEFMod: 0.9f)},

        {"sassy",
            new PokemonNature(
                ID: "sassy",
                natureName: "Sassy",
                SPDMod: 1.1f, SPEMod: 0.9f)},

        {"careful",
            new PokemonNature(
                ID: "careful",
                natureName: "Careful",
                SPDMod: 1.1f, SPAMod: 0.9f)},

        // + SPE
        {"timid",
            new PokemonNature(
                ID: "timid",
                natureName: "Timid",
                SPEMod: 1.1f, ATKMod: 0.9f)},

        {"hasty",
            new PokemonNature(
                ID: "hasty",
                natureName: "Hasty",
                SPEMod: 1.1f, DEFMod: 0.9f)},

        {"jolly",
            new PokemonNature(
                ID: "jolly",
                natureName: "Jolly",
                SPEMod: 1.1f, SPAMod: 0.9f)},

        {"naive",
            new PokemonNature(
                ID: "naive",
                natureName: "Naive",
                SPEMod: 1.1f, SPDMod: 0.9f)},

    };

        public PokemonNature GetNatureData(string ID)
        {
            if (database.ContainsKey(ID))
            {
                return database[ID];
            }
            Debug.LogWarning("Could not find Pokemon with ID: " + ID);
            return database[""];
        }
        public PokemonNature GetRandomNature()
        {
            List<string> allNatures = new List<string>(database.Keys);
            return GetNatureData(allNatures[Random.Range(0, allNatures.Count)]);
        }

    }
}