using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    /// <summary>
    /// The base component for displaying buttons on the UI.
    /// </summary>
    public class BaseButton : MonoBehaviour
    {
        #region Attributes
        public Image image;
        public Color colorSel, colorUnsel;
        public bool isSelected = false;
        public bool isBackButton = false;
        [HideInInspector] public RectTransform rectTransform;
        #endregion

        #region Functions
        /// <summary>
        /// Initializes this component.
        /// </summary>
        public void InitializeSelf()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        /// <summary>
        /// Highlights this button.
        /// </summary>
        public void SelectSelf()
        {
            image.color = colorSel;
            isSelected = true;
        }
        /// <summary>
        /// Unhighlights this button.
        /// </summary>
        public void UnselectSelf()
        {
            image.color = colorUnsel;
            isSelected = false;
        }
        #endregion
    }
}

