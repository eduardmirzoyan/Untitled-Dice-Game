using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiceSlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
      [SerializeField] private DiceUI containedDiceUI;
      [SerializeField] private ActionUI actionUI;
      [SerializeField] private Image image;
      [SerializeField] private Color defaultColor;
      [SerializeField] private Color highlightColor;
      [SerializeField] private bool isActive;

      private void Awake() {
         image.color = defaultColor;
      }

      public void OnDrop(PointerEventData eventData)
      {
         // If the dropped data is a die
         // Make sure there isnt already a die in this slot
         if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DiceUI diceUI) && containedDiceUI == null) {
            
            // Check for any constraints
            var action = actionUI.GetAction();
            var dice = diceUI.GetDice();

            if (action.passesContraints(dice)) {
               // Sets a new parent for the dice to snap back to
               diceUI.SetParent(gameObject.transform);
               
               // Stores UI
               containedDiceUI = diceUI;
            
               // Update visuals
               image.color = defaultColor;

               // Select this
               CombatManager.instance.SelectAction(action, dice);
               
               // Update visuals
               actionUI.Update();
            }
            else {
               print("Dice does not pass contraints of selected action");
            }
            
         }
      }

      public void OnPointerEnter(PointerEventData eventData)
      {
         // If the dropped data is a die
         if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DiceUI diceUI)) {
            image.color = highlightColor;
         }
      }
   
      public void OnPointerExit(PointerEventData eventData)
      {
         // If the dropped data is a die
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DiceUI diceUI)) {
               image.color = defaultColor;
            }
      }

      public Dice GetContainedDice() {
         return containedDiceUI.GetDice();
      }

      public DiceUI GetDiceUI() {
         return containedDiceUI;
      }

      public bool ContainsDie() {
         return containedDiceUI != null;
      }

      public void RemoveInsertedDie() {
         if (!ContainsDie()) {
            throw new System.Exception("Tried to remove a die that wasn't there.");
         }

         // Return Die to origin
         containedDiceUI.ResetLocation();

         // Set contained Dice to null
         containedDiceUI = null;
      }
}
