using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBS.Databases
{
    public class Loader
    {
        // Only 1 instance of this class ever
        private static Loader singleton = new Loader();
        private Loader() { 
            pokemonDB = new Dictionary<string, Data.Pokemon>();
            typesDB = new Dictionary<string, Data.ElementalType>();
        }
        public static Loader instance
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

        // Elemental Types
        public Dictionary<string, PBS.Data.ElementalType> typesDB { get; private set; }
        public PBS.Data.ElementalType LoadElementalType(string ID)
        {
            // Load from files
            JObject json = JObject.Parse(File.ReadAllText(
                @"" + Application.dataPath + "\\Data\\ElementalTypes\\default.json"));
            JToken jToken = json[ID];
            PBS.Data.ElementalType data = jToken.ToObject<PBS.Data.ElementalType>();

            // add it to the local cache
            this.typesDB[ID] = data;
            
            return data;
        }
        public PBS.Data.ElementalType GetElementalTypeData(string ID)
        {
            if (typesDB.ContainsKey(ID))
            {
                return typesDB[ID];
            }
            Debug.LogWarning("Could not find type with ID: " + ID + " in DB cache.");
            return LoadElementalType(ID);
        }

        // Pokemon
        public Dictionary<string, PBS.Data.Pokemon> pokemonDB {get; private set; }
        public PBS.Data.Pokemon LoadPokemon(string ID)
        {
            // Load from files
            JObject json = JObject.Parse(File.ReadAllText(
                @"" + Application.dataPath + "\\Data\\Pokemon\\default.json"));
            JToken jToken = json[ID];
            PBS.Data.Pokemon data = jToken.ToObject<PBS.Data.Pokemon>();

            // add it to the local cache
            this.pokemonDB[ID] = data;
            
            return data;
        }
        public PBS.Data.Pokemon GetPokemonData(string ID)
        {
            if (pokemonDB.ContainsKey(ID))
            {
                return pokemonDB[ID];
            }
            Debug.LogWarning("Could not find Pokemon with ID: " + ID + " in DB cache.");
            return LoadPokemon(ID);
        }
    }
}
