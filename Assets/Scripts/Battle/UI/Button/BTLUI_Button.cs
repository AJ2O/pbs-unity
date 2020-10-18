using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUI_Button : MonoBehaviour
{
    public Image image;
    public Color colorSel, colorUnsel;
    public bool isSelected = false;
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
