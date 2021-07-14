using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace PBS.Databases
{
    public class Loader
    {
        //create an object of SingleObject
        private static Loader singleton = new Loader();

        //make the constructor private so that this class cannot be
        //instantiated
        private Loader() { }

        //Get the only object available
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

        // Pokemon
        private Dictionary<string, PBS.Data.Pokemon> pokemonDB = new Dictionary<string, PBS.Data.Pokemon>();
        public PBS.Data.Pokemon LoadPokemon(string ID)
        {
            // Load from files
            //PBS.Data.Pokemon pk = New

            return null;
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
