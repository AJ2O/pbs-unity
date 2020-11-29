using System.Collections;
using System.Collections.Generic;

namespace PBS.Player.Query
{
    /// <summary>
    /// The base class for queries.
    /// </summary>
    public class QueryBase { }
    /// <summary>
    /// The base class for query responses.
    /// </summary>
    public class QueryResponseBase { }


    /// <summary>
    /// A query for retrieving a <seealso cref="MoveTargetResponse"/> for a move and user.
    /// </summary>
    public class MoveTarget : QueryBase
    {
        /// <summary>
        /// The uniqueID of the pokemon using the move.
        /// </summary>
        public string pokemonUniqueID;
        /// <summary>
        /// The move being used.
        /// </summary>
        public string moveID;
    }
    /// <summary>
    /// A response to <seealso cref="MoveTarget"/> containing the possible move targets.
    /// </summary>
    public class MoveTargetResponse : QueryResponseBase
    {
        /// <summary>
        /// A list of groups for potential move targets.
        /// </summary>
        public List<List<BattlePosition>> possibleTargets;
    }

}
