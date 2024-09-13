using System.Collections;
using System.Collections.Generic;
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
            if (/*transform.childCount == 0 &&*/ Game.Selected.Count > 0)
            {
                GameCard selectedCard = Game.Selected[0];
                Summon(selectedCard);
                //Debug.Log($"Card placed in slot: {selectedCard.name}");
            }
        }
        
    }

    void Summon(GameCard card)
    {
        card.transform.SetParent(transform, false);
        card.transform.localPosition = Vector3.zero;
        card.DeSelect();
        TurnSystem.Instance.Active.battlefield.DisableAllZones();
        TurnSystem.Instance.Active.battlefield.UpdatePoints();
        TurnSystem.Instance.TakeAction();
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
