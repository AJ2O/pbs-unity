using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class BaseButton : MonoBehaviour
    {
        public Image image;
        public Color colorSel, colorUnsel;
        public bool isSelected = false;
        public bool isBackButton = false;
        [HideInInspector] public RectTransform rectTransform;

        public void InitializeSelf()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        public void SelectSelf()
        {
            image.color = colorSel;
            isSelected = true;
        }
        public void UnselectSelf()
        {
            image.color = colorUnsel;
            isSelected = false;
        }
    }
}

