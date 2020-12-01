using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Scene.Entities
{
    /// <summary>
    /// An object on <seealso cref="Canvas"/> representing a Pokemon.
    /// </summary>
    public class Pokemon : BaseEntity
    {
        /// <summary>
        /// The uniqueID belonging to this pokemon. Used to reference this specific pokemon.
        /// </summary>
        public string pokemonUniqueID;
        /// <summary>
        /// The ID of this pokemon species in the database. Used to draw this pokemon on the canvas.
        /// </summary>
        public string pokemonID;
    }
}
