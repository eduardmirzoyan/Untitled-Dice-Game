using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueSlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image shading;
    [SerializeField] private Image iconImage;

    public void Initialize(GameObject unitMask, Color color) {
        //iconImage.sprite = icon;
        Instantiate(unitMask, transform);
        shading.color = color;
    }
}
