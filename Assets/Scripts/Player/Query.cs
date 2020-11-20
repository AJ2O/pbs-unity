using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Player.Query
{
    public class QueryBase { }
    public class QueryResponseBase { }



    public class MoveTarget : QueryBase
    {
        public string pokemonUniqueID;
        public string moveID;
    }
    public class MoveTargetResponse : QueryResponseBase
    {
        public List<List<BattlePosition>> possibleTargets;
    }

}
