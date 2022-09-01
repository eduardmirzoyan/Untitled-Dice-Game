using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceBoardUI : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Canvas playerScreen;
    [SerializeField] private HorizontalLayoutGroup allyDiceGroup;
    [SerializeField] private HorizontalLayoutGroup enemyDiceGroup;
    [SerializeField] private GameObject diceOutlinePrefab;
    [SerializeField] private GameObject dicePrefab;

    public void CreateAllyDie() {
        // TODO?
    }

    public void CreateEnemyDie() {
        // TODO?
    }
}
