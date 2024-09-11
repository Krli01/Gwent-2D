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
                Summon(selectedCard);
                TurnSystem.ActionTaken = true;
                //Debug.Log($"Card placed in slot: {selectedCard.name}");
            }
        }
        
    }

    void Summon(GameCard card)
    {
        card.transform.SetParent(transform, false);
        card.transform.localPosition = Vector3.zero;
        card.DeSelect(card);
        if (this.transform.childCount > 0) TurnSystem.Active.battlefield.DisableAllZones();
        TurnSystem.Active.battlefield.UpdatePoints();
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
