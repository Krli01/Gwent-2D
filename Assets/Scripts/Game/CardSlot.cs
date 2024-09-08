using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool Available;
    public bool isBooster;
    
    void Awake()
    {
        Available = false;
        isBooster = transform.name == "Booster" ? true : false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Available)
        {
            if (transform.childCount == 0 && Game.Selected.Count > 0)
            {
                GameCard selectedCard = Game.Selected[0];
                selectedCard.transform.SetParent(transform, false);
                selectedCard.isSelected = false;
                selectedCard.transform.localPosition = Vector3.zero;
                Game.Selected.RemoveAt(0);
                if (this.transform.childCount > 0) Game.DisableAllZones(TurnSystem.Active);
                TurnSystem.ActionTaken = true;
                //Debug.Log($"Card placed in slot: {selectedCard.name}");
            }
            //Debug.Log("clicked");
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //iluminar marco
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //restablecer
    }
}
