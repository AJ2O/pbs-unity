using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    /// <summary>
    /// This component handles displaying a move on the <seealso cref="Fight"/> panel.
    /// </summary>
    public class FightButton : BaseButton
    {
        [Tooltip("Text displaying the name of the move.")]
        public Text moveTxt;
        [Tooltip("Text displaying the remaining and max PP of the move.")]
        public Text ppTxt;
        [Tooltip("Text displaying the type of the move.")]
        public Text typeTxt;
        /// <summary>
        /// The move ID associated with this button.
        /// </summary>
        [HideInInspector] public string moveID;
        /// <summary>
        /// The moveslot associated with this button.
        /// </summary>
        [HideInInspector] public Events.CommandAgent.Moveslot moveslot;
    }
}

